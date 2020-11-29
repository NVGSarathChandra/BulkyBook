using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book_Project.Dataaccess.data;
using Dapper;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utilities;

namespace Bulky_Book_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticDetails.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
       
        public CategoryController(IUnitOfWork _applicationDb)
        {
            unitOfWork = _applicationDb;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            category = unitOfWork.Category.GetFirstOrDefault(s => s.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var result = unitOfWork.Category.GetAll();
            return Json(new { data = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId != 0)
                {
                    unitOfWork.Category.Update(category);
                }
                else
                {
                    unitOfWork.Category.Add(category);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var dataFromDb = unitOfWork.Category.Get(id);
            if (dataFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.Category.RemoveEntity(dataFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

      
        #endregion
    }
}
