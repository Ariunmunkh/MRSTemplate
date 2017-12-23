﻿using DevExpress.Web;
using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    public class BranchController : Controller
    {
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
	}
}