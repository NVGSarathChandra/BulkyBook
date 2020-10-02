using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book_Project.Dataaccess.data;
using Dapper;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Bulky_Book_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = unitOfWork.category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                }),
                CoverTypeList = unitOfWork.coverType.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.CoverTypeName,
                    Value = i.CoverTypeId.ToString()
                })
            };
            if (id == null)
            {
                return View(productVM);
            }
            productVM.Product = unitOfWork.product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var result = unitOfWork.product.GetAll(/*includeProperties: "Category,CoverType"*/);
            List<Product> product = unitOfWork.product.GetAll().ToList();
            List<Category> category = unitOfWork.category.GetAll().ToList();
            List<CoverType> coverType = unitOfWork.coverType.GetAll().ToList();

            var productData = from p in product
                              join c in category on p.CategoryId equals c.CategoryId into table1
                              from c in table1.ToList()
                              join ct in coverType on p.CoverTypeId equals ct.CoverTypeId into table2
                              from ct in table2.ToList()
                              select new ProductVM()
                              {
                                  Product = p
                                  //CategoryList = table1.Select(i => new SelectListItem()
                                  //{
                                  //    Text = i.CategoryName,
                                  //    Value = i.CategoryId.ToString()
                                  //}),

                                  //CoverTypeList = table2.Select(i => new SelectListItem()
                                  //{
                                  //    Text = i.CoverTypeName,
                                  //    Value = i.CoverTypeId.ToString()
                                  //})
                              };
            return Json(new { data = productData });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            var param = new DynamicParameters();

            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images/Product");
                    var extension = Path.GetExtension(files[0].FileName);   
                    if (productVM.Product.ImageURL != null)
                    {
                        //This is an edit functionality since there exists a file URL
                        var imagepath = Path.Combine(webRootPath, productVM.Product.ImageURL.TrimStart('/'));
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                    FileStream fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    productVM.Product.ImageURL = @"/Images/Product/" + fileName + extension;
                }
                else
                {
                    if (productVM.Product.ProductID != 0)
                    {
                        Product productFromDb = unitOfWork.product.Get(productVM.Product.ProductID);
                        productVM.Product.ImageURL = productFromDb.ImageURL;
                    }
                }
                if (productVM.Product.ProductID != 0)
                {
                    param.Add("@id", productVM.Product.ProductID);
                    unitOfWork.product.Update(productVM.Product);
                }
                else
                {
                    unitOfWork.product.Add(productVM.Product);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //Validates server side when model state is invalid
                productVM.CategoryList = unitOfWork.category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.CategoryName,
                    Value = i.CategoryId.ToString()
                });
                productVM.CoverTypeList = unitOfWork.coverType.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.CoverTypeName,
                    Value = i.CoverTypeId.ToString()
                });
            }

            return View(productVM);
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
          
            Product resultFromDb = unitOfWork.product.Get(id);
            if (resultFromDb == null)
            {
                return Json(new { success = false, message = "Unable to Delete Product" });
            }
            if (resultFromDb.ImageURL != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webRootPath,resultFromDb.ImageURL.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            unitOfWork.product.Remove(resultFromDb.ProductID);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
