﻿using DevExpress.Web;
using DXWebMRCS.Models;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin,BranchUser")]
    public class BranchController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult BranchView(int branchId, int menuID)
        {
            var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
            branch.menuID = menuID;
            return View(branch);
        }

        [AllowAnonymous]
        public ActionResult BranchHeaderPartial(int branchId)
        {
            var menuList = db.Database.SqlQuery<Menu>("SELECT * FROM Menu WHERE BranchID = " + branchId).ToList();
            return PartialView("_BranchHeaderPartial", menuList);
        }

        [AllowAnonymous]
        public ActionResult BranchNewsListPartial(int branchId, int menuID)
        {
            if (menuID > 0)
            {
                var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " AND MenuID =" + menuID).ToList();
                if (newsList.Count == 1)
                {
                    return PartialView("BranchViewDetail", newsList.FirstOrDefault());
                }
                return PartialView("BranchNewsListPartial", newsList);
            }
            else
            {
                var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId).ToList();
                if (newsList.Count == 1)
                {
                    return PartialView("BranchViewDetail", newsList.FirstOrDefault());
                }
                return PartialView("BranchNewsListPartial", newsList);
            }            
        }

        [AllowAnonymous]
        public ActionResult BranchViewDetail(int newsId)
        {
            var news = db.Database.SqlQuery<News>("SELECT TOP 1 * FROM News WHERE CID = " + newsId).FirstOrDefault();
            return PartialView("BranchViewDetail", news);
        }

        [AllowAnonymous]
        public ActionResult BranchListDetail(int newsId, int branchId)
        {
            var news = db.Database.SqlQuery<News>("SELECT TOP 1 * FROM News WHERE CID = " + newsId).FirstOrDefault();
            var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
            branch.news = news;
            return View("BranchListDetail", branch);
        }

        #region Admin
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


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchID, NameMon, NameEng, Logo, Image, email, phone, address")] Branch Branch, HttpPostedFileBase ImageFile, HttpPostedFileBase LogoFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Branch.Image = "/Content/Images/NewsImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    ImageFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }

                if (LogoFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(LogoFile.FileName);
                    string extension = Path.GetExtension(LogoFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Branch.Logo = "/Content/Images/NewsImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    LogoFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }

                db.Branchs.Add(Branch);
                db.SaveChanges();
                NorthwindDataProvider.InsertBranchMenu(Branch);
                return RedirectToAction("Index");
            }

            return View(Branch);
        }

        // GET: /Branch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch Branch = db.Branchs.Find(id);
            if (Branch == null)
            {
                return HttpNotFound();
            }
            return View(Branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID, NameMon, NameEng, Logo, Image, email, phone, address")] Branch Branch, HttpPostedFileBase ImageFile, HttpPostedFileBase LogoFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Branch.Image = "/Content/Images/NewsImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    ImageFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }

                if (LogoFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(LogoFile.FileName);
                    string extension = Path.GetExtension(LogoFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    Branch.Logo = "/Content/Images/NewsImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    LogoFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }

                db.Entry(Branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Branch);
        }

        // GET: /Branch/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch Branch = db.Branchs.Find(id);
            if (Branch == null)
            {
                return HttpNotFound();
            }
            return View(Branch);
        }

        // POST: /Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch Branch = db.Branchs.Find(id);
            db.Branchs.Remove(Branch);
            db.SaveChanges();
            return RedirectToAction("Index");
        } 
        #endregion

	}
}