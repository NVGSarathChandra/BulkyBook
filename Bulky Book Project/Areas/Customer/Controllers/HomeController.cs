using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bulky_Book_Project.Models.ViewModels;
using DataAccess.IServiceContracts;
using Models;
using Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Utilities;
using Microsoft.AspNetCore.Http;

namespace Bulky_Book_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork _unitOfWork)
        {
            _logger = logger;
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> product = unitOfWork.Product.GetAll(includeProperties: "CoverType,Category").ToList();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = unitOfWork.ShoppingCart.GetAll(u => u.ApplicatinUserId == claim.Value).ToList().Count();

                HttpContext.Session.SetObject(StaticDetails.SessionShoppingCart, count);
            }
            //List<Category> category = unitOfWork.Category.GetAll().ToList();
            //List<CoverType> coverType = unitOfWork.CoverType.GetAll().ToList();

            //IEnumerable<Product> productList = from p in product
            //                                   join c in category on p.CategoryId equals c.CategoryId into table1
            //                                   from c in table1.ToList()
            //                                   join ct in coverType on p.CoverTypeId equals ct.CoverTypeId into table2
            //                                   from ct in table2.ToList()
            //                                   select p;

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Details(int productId)
        {
            var product = unitOfWork.Product.GetFirstOrDefault(u => u.ProductID == productId, includeProperties: "Category,CoverType");
            var shoppingCart = new ShoppingCart()
            {
                ProductId = product.ProductID,
                Product = product,
            };
            return View(shoppingCart);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] MVC's anti-forgery support writes a unique value to an HTTP-only cookie and then the same value is written to the form. When the page is submitted, an error is raised if the cookie value doesn't match the form value.
        //[Authorize] should be authorized to change content
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.ShoppingCartId = 0;
            if (ModelState.IsValid)
            {
                // Add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicatinUserId = claim.Value;

                ShoppingCart shoppingCartfromDB = unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicatinUserId == shoppingCart.ApplicatinUserId && u.ProductId == shoppingCart.ProductId,
                    includeProperties: "Product"
                    );
                if (shoppingCartfromDB == null)
                {
                    unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    shoppingCartfromDB.Count +=shoppingCart.Count;
                    unitOfWork.ShoppingCart.Update(shoppingCartfromDB);
                }
                unitOfWork.Save();
                var count = unitOfWork.ShoppingCart.GetAll(u => u.ApplicatinUserId == shoppingCart.ApplicatinUserId).ToList().Count();

                //HttpContext.Session.SetObject(StaticDetails.SessionShoppingCart, shoppingCartfromDB);
                HttpContext.Session.SetInt32(StaticDetails.SessionShoppingCart, count);
               // HttpContext.Session.GetObject<ShoppingCart>(StaticDetails.SessionShoppingCart);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = unitOfWork.Product.GetFirstOrDefault(u => u.ProductID == 12, includeProperties: "Category,CoverType");
                 shoppingCart = new ShoppingCart()
                {
                    ProductId = product.ProductID,
                    Product = product,
                };
                return View(shoppingCart);
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
