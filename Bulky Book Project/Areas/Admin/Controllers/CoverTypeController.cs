using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataAccess.IServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utilities;

namespace Bulky_Book_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.AdminRole)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CoverTypeController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }


        #region ACTIONS

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            var parameters = new DynamicParameters();
            if (id == null)
            {
                return View(coverType);
            }
            parameters.Add("@id", coverType.CoverTypeId);
            coverType = unitOfWork.storedProcedureCall.singleRecord<CoverType>(StaticDetails.SP_DELETECOVERTYPE, parameters);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }


        #endregion

        #region API Calls


        [HttpGet]
        public IActionResult GetAllCoverTypes()
        {
            var coverType = unitOfWork.storedProcedureCall.RetrievTable<CoverType>(StaticDetails.SP_GETALLCOVERTYPES, null);
            return Json(new { data = coverType });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var parameters = new DynamicParameters();
                if (coverType.CoverTypeId != 0)
                {
                    unitOfWork.storedProcedureCall.Execute(StaticDetails.SP_UPDATECOVERTYPE, parameters);
                }
                else
                {
                    parameters.Add("@name", coverType.CoverTypeName);
                    unitOfWork.storedProcedureCall.Execute(StaticDetails.SP_INSERTCOVERTYPE, parameters);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }




        [HttpDelete]

        public ActionResult DeleteCoverType(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            var dataFromDb = unitOfWork.storedProcedureCall.singleRecord<CoverType>(StaticDetails.SP_GETCOVERTYPE, parameters);
            if (dataFromDb == null)
            {
                return Json(new { success = false, message = "Unable to Delete Cover Type" });
            }
            unitOfWork.storedProcedureCall.Execute(StaticDetails.SP_DELETECOVERTYPE, parameters);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion
    }
}
