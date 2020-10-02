using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utilities;

namespace Bulky_Book_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.AdminRole+","+StaticDetails.EmployeeRole)]
    public class OrganizationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrganizationController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Organization organization = new Organization();
            if (id == null)
            {
                return View(organization);
            }
            organization = unitOfWork.organization.GetFirstOrDefault(s => s.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }
        #region API Calls
        [HttpGet]
        public IActionResult GetOrganizations()
        {
            var result = unitOfWork.organization.GetAll();
            return Json(new { data = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Organization organization)
        {
            if (ModelState.IsValid)
            {
                if (organization.OrganizationId != 0)
                {
                    unitOfWork.organization.Update(organization);
                }
                else
                {
                    unitOfWork.organization.Add(organization);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);

        }

        [HttpDelete]
        public IActionResult DeleteOrganization(int id)
        {
            var dataFromDb = unitOfWork.organization.Get(id);
            if (dataFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.organization.RemoveEntity(dataFromDb);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
