using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Models;
using Utilities;

namespace Bulky_Book_Project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ILogger<RegisterModel> logger,
            IEmailSender emailSender, RoleManager<IdentityRole> _roleManager, IUnitOfWork _unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            roleManager = _roleManager;
            unitOfWork = _unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            public string Name { get; set; }
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }

            public int? OrganizatioinId { get; set; }

            public string Role { get; set; }

            public IEnumerable<SelectListItem> OrganizationList { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel()
            {
                OrganizationList = unitOfWork.organization.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.OrganizationName,
                    Value = i.OrganizationId.ToString()
                }),
                RoleList = roleManager.Roles.Where(u => u.Name != StaticDetails.IndividualCustomerRole).Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Name.ToString()
                })
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = Input.Name,
                    Email = Input.Email,
                    OrganizatioinId = Input.OrganizatioinId,
                    StreetAddress = Input.StreetAddress,
                    City = Input.City,
                    State = Input.State,
                    PostalCode = Input.PostalCode,
                    Name = Input.Name,
                    PhoneNumber = Input.PhoneNumber,
                    Role = Input.Role
                };
                var result = await _userManager.CreateAsync(applicationUser, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (!await roleManager.RoleExistsAsync(StaticDetails.AdminRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticDetails.AdminRole));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticDetails.EmployeeRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticDetails.EmployeeRole));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticDetails.OrganizationCustomerRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticDetails.OrganizationCustomerRole));
                    }
                    if (!await roleManager.RoleExistsAsync(StaticDetails.IndividualCustomerRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(StaticDetails.IndividualCustomerRole));
                    }
                    if (applicationUser.Role == null)
                    {
                        await _userManager.AddToRoleAsync(applicationUser, StaticDetails.IndividualCustomerRole);
                    }
                    else
                    {
                        if (applicationUser.OrganizatioinId > 0)
                        {
                            await _userManager.AddToRoleAsync(applicationUser, StaticDetails.OrganizationCustomerRole);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(applicationUser, applicationUser.Role);

                        }
                    }
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = applicationUser.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (applicationUser.Role == null)
                        {
                            await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            //Admin registering the user
                            return RedirectToAction("Index", "Users", new { Areas = "Admin" });
                        }

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
