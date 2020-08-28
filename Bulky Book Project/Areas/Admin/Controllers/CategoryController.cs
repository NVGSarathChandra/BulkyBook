using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Bulky_Book_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        // ApplicationDbContext applicationDb;
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
            category = unitOfWork.category.GetFirstOrDefault(s => s.CategoryId == id);
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
            var result = unitOfWork.category.GetAll();
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
                    unitOfWork.category.Update(category);
                }
                else
                {
                    unitOfWork.category.Add(category);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var dataFromDb = unitOfWork.category.Get(id);
            if (dataFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.category.RemoveEntity(dataFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
