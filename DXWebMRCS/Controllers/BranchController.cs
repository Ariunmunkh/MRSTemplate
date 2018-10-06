using DevExpress.Web;
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
using PagedList;
using System.Threading;
using System.Globalization;

namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin,BranchUser")]
    //[RequireHttps]
    public class BranchController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(String lan)
        {
            if (lan != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lan);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lan);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = lan;
            Response.Cookies.Add(cookie);

            return RedirectToAction("BranchView", new { branchId = ViewBag.branchId });
        }

        [AllowAnonymous]
        public ActionResult PageClick(int? page, int branchId, int menuID)
        {
            var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
            var pageNumber = page ?? 1;
            var pageSize = 4;
            if (menuID > 0)
            {
                var newslist = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " AND MenuID =" + menuID + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                if (newslist == null)
                {
                    return HttpNotFound();
                }

                if (newslist.Count == 1)
                {
                    branch.news = newslist.First();
                    return View("BranchListDetail", branch);
                }

                branch.newsList = newslist;
                return View("BranchView", branch);
            }
            else
            {
                var newslist = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                if (newslist == null)
                {
                    return HttpNotFound();
                }

                if (newslist.Count == 1)
                {
                    branch.news = newslist.First();
                    return View("BranchListDetail", branch);
                }

                branch.newsList = newslist;
                return View("BranchView", branch);
            }
        }

        
        [AllowAnonymous]
        public ActionResult BranchView(int branchId, int menuID)
        {
            try
            {
                var pageNumber = 1;
                var pageSize = 4;

                ViewBag.menuId = menuID;
                ViewBag.branchId = branchId;

                var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
                branch.menuID = menuID;

                if (menuID > 0)
                {
                    var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " AND MenuID =" + menuID + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                    branch.newsList = newsList;
                    if (newsList.Count == 1)
                    {
                        branch.news = newsList.First();
                        return View("BranchListDetail", branch);
                    }
                    return View(branch);
                }
                else
                {
                    var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                    branch.newsList = newsList;
                    if (newsList.Count == 1)
                    {
                        branch.news = newsList.First();
                        return View("BranchListDetail", branch);
                    }
                    return View(branch);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View(new BranchViewModel());
            }        
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
                var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " AND MenuID =" + menuID + " ORDER BY Date DESC").ToList();
                if (newsList.Count == 1)
                {
                    return PartialView("BranchViewDetail", newsList.FirstOrDefault());
                }
                return PartialView("BranchNewsListPartial", newsList);
            }
            else
            {
                var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " ORDER BY Date DESC").ToList();
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


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "BranchID, NameMon, NameEng, Logo, Image, email, phone, address")] Branch Branch, HttpPostedFileBase ImageFile, HttpPostedFileBase LogoFile)
        {
            if (ModelState.IsValid)
            {

                string subPath = "/Content/Images/NewsImage"; // your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

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

                string subPath = "/Content/Images/NewsImage"; // your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

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
        [Authorize(Roles = "Admin")]
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