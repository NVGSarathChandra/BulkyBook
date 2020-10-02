using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book_Project.Dataaccess.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var userList = dbContext.ApplicationUsers.Include(u => u.Organization).ToList();
            var userRole = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Organization == null)
                {
                    user.Organization = new Organization()
                    {
                        OrganizationName = string.Empty
                    };
                }
            }

            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockOrUnlockUser([FromBody] string id)
        {
            var userDetails = dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (userDetails == null)
            {
                return Json(new { success = false, message = "Unable to find user to lock or unlock" });
            }
            if (userDetails.LockoutEnd > DateTime.Now)
            {
                //Unlock user
                userDetails.LockoutEnd = DateTime.Now;
            }
            else
            {
                userDetails.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Lock/Unlock Successful" });

        }


        #endregion
    }
}