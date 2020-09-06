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
            List<Product> product = unitOfWork.product.GetAll().ToList();
            List<Category> category = unitOfWork.category.GetAll().ToList();
            List<CoverType> coverType = unitOfWork.coverType.GetAll().ToList();

            IEnumerable<Product> productList = from p in product
                                               join c in category on p.CategoryId equals c.CategoryId into table1
                                               from c in table1.ToList()
                                               join ct in coverType on p.CoverTypeId equals ct.CoverTypeId into table2
                                               from ct in table2.ToList()
                                               select p;

            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
