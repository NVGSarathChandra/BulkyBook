using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using Models.ViewModels;
using Utilities;

namespace Bulky_Book_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        private readonly UserManager<IdentityUser> userManager;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork _unitOfWork, IEmailSender _emailSender, UserManager<IdentityUser> _userManager)
        {
            unitOfWork = _unitOfWork;
            emailSender = _emailSender;
            userManager = _userManager;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                CartList = unitOfWork.ShoppingCart.GetAll(u => u.ApplicatinUserId == claims.Value, includeProperties: "Product")
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claims.Value, includeProperties: "Organization");

            foreach (var item in ShoppingCartVM.CartList)
            {
                item.Price = StaticDetails.CalculateBooksPrice(item.Count, item.Product.Price, item.Product.Price50, item.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
                item.Product.ProductDescription = StaticDetails.ConvertToRawHtml(item.Product.ProductDescription);
                if (item.Product.ProductDescription.Length > 100)
                {
                    item.Product.ProductDescription = item.Product.ProductDescription.Substring(0, 99) + "...";
                }
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var applicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claims.Value);
            if (applicationUser == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email is empty");
            }
            var code = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = applicationUser.Id, code = code },
                protocol: Request.Scheme);

            await emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email is sent. Verify your email");
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Plus(int cartId)
        {
            var cart = unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ShoppingCartId == cartId, includeProperties: "Product");
            cart.Count += 1;
            cart.Price = StaticDetails.CalculateBooksPrice(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {


            var cart = unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ShoppingCartId == cartId, includeProperties: "Product");
            var count = unitOfWork.ShoppingCart.GetAll(u => u.ApplicatinUserId == cart.ApplicatinUserId).ToList().Count();

            if (cart.Count == 1)
            {
                unitOfWork.ShoppingCart.RemoveEntity(cart);
                HttpContext.Session.SetObject(StaticDetails.SessionShoppingCart, count - 1);
            }
            else
            {
                cart.Count -= 1;
                cart.Price = StaticDetails.CalculateBooksPrice(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);

            }
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ShoppingCartId == cartId, includeProperties: "Product");
            var count = unitOfWork.ShoppingCart.GetAll(u => u.ApplicatinUserId == cart.ApplicatinUserId).ToList().Count();
            unitOfWork.ShoppingCart.RemoveEntity(cart);
            HttpContext.Session.SetObject(StaticDetails.SessionShoppingCart, count - 1);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
