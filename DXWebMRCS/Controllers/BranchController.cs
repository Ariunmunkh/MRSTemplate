using DevExpress.Web;
using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DXWebMRCS.Controllers
{
    public class BranchController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartialView()
        {
            return PartialView("GridViewPartialView", NorthwindDataProvider.GetBranchs());
        }
        public ActionResult ChangeEditModePartial(GridViewEditingMode editMode)
        {
            //GridViewEditingHelper.EditMode = editMode;
            return PartialView("GridViewPartialView", NorthwindDataProvider.GetBranchs());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditModesAddNewPartial(Branch Branch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.InsertBranch(Branch);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("GridViewPartialView", NorthwindDataProvider.GetBranchs());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditModesUpdatePartial(Branch Branch)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.UpdateBranch(Branch);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("GridViewPartialView", NorthwindDataProvider.GetBranchs());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditModesDeletePartial(int BranchID = -1)
        {
            if (BranchID >= 0)
            {
                try
                {
                    NorthwindDataProvider.DeleteBranch(BranchID);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridViewPartialView", NorthwindDataProvider.GetBranchs());
        }

        public ActionResult BranchUserCreate()
        {
            ViewBag.BranchList = db.Branchs.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchUserCreate([Bind(Include = "UserName,Email,Password,ConfirmPassword,BranchId")] BranchRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.Email, model.Password, propertyValues: new
                {
                    Name = model.UserName,
                    BranchId = model.BranchId
                });
                var isRole = Roles.RoleExists("BranchUser");
                if (!isRole)
                {
                    Roles.CreateRole("BranchUser");
                }
                Roles.AddUserToRole(model.Email, "BranchUser");
                return RedirectToAction("Index");
            }

            return View(model);
        }
	}
}