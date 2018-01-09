﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using DevExpress.Web.Mvc;
using System.IO;
using ImageResizer;
using PagedList;

namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin,BranchUser")]
    public class NewsController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /News/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.News.ToList());
        }

        #region News Role user
        [AllowAnonymous]
        public ActionResult MenuClick(int id)
        {
            var pageNumber = 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News WHERE MenuID = " + id + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }

            ViewBag.menuId = id;

            if (news.Count == 1)
            {
                return View("NewsDetail", news.First());
            }
            return View("MenuNewsList", news);
        }

        [AllowAnonymous]
        public ActionResult MenuPageClick(int? page, int menuId)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News WHERE MenuID = " + menuId + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.menuId = menuId;
            return View("MenuNewsList", news);
        }

        [AllowAnonymous]
        public ActionResult PageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View("NewsList", news);
        }

        [AllowAnonymous]
        // GET: /News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [AllowAnonymous]
        //GET: /News/NewsDetail/5
        public ActionResult NewsDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Database.SqlQuery<News>("SELECT TOP 1 * FROM News WHERE CID = " + id).FirstOrDefault();
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [AllowAnonymous]
        public ActionResult NewsList()
        {
            var pageNumber = 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        } 
        #endregion

        // GET: /News/NewsDetail/5
        //public ActionResult NewsDetail(News news)
        //{            
        //    if (news == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(news);
        //}

        #region News Create, Edit, Delete
        // GET: /News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ContentType,MenuID")] News news, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    news.Image = "/Content/Images/NewsImage/" + fileName;
                    news.ImageMedium = "/Content/Images/NewsImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileNameMedium);
                    ImageFile.SaveAs(fileName);
                    ImageFile.SaveAs(fileNameMedium);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                    ResizeSettings resizeSetting2 = new ResizeSettings
                    {
                        Width = 360,
                        Height = 240,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileNameMedium, fileNameMedium, resizeSetting2);
                }

                news.Date = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: /News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: /News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ContentType,MenuID")] News news, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    news.Image = "/Content/Images/NewsImage/" + fileName;
                    news.ImageMedium = "/Content/Images/NewsImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileNameMedium);
                    ImageFile.SaveAs(fileName);
                    ImageFile.SaveAs(fileNameMedium);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                    ResizeSettings resizeSetting2 = new ResizeSettings
                    {
                        Width = 360,
                        Height = 240,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileNameMedium, fileNameMedium, resizeSetting2);
                }

                news.Date = DateTime.Now;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: /News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: /News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        } 
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult BodyMonPartial()
        //{
        //    return PartialView("_BodyMonPartial");
        //}
        public ActionResult EditBodyMonPartial(News objectValue)
        {
            return PartialView("_BodyMonPartial", objectValue);
        }
        public ActionResult BodyMonPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyMon", NewsControllerBodyMonSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyMonPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyMon", NewsControllerBodyMonSettings.ImageUploadValidationSettings, NewsControllerBodyMonSettings.ImageUploadDirectory);
            return null;
        }

        public ActionResult ImageUpload()
        {
            UploadControlExtension.GetUploadedFiles("Image", NewsControllerImageSettings.UploadValidationSettings, NewsControllerImageSettings.FileUploadComplete);
            return null;
        }

        //public ActionResult BodyEngPartial()
        //{
        //    return PartialView("_BodyEngPartial");
        //}
        public ActionResult EditBodyEngPartial(News objectValue)
        {
            return PartialView("_BodyEngPartial", objectValue);
        }
        public ActionResult BodyEngPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyEng", NewsControllerBodyEngSettings1.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyEngPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyEng", NewsControllerBodyEngSettings1.ImageUploadValidationSettings, NewsControllerBodyEngSettings1.ImageUploadDirectory);
            return null;
        }



        DXWebMRCS.Models.UsersContext db1 = new DXWebMRCS.Models.UsersContext();

        [ValidateInput(false)]
        public ActionResult NewsViewPartial()
        {
            var model = db1.News;
            return PartialView("_NewsViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialAddNew(DXWebMRCS.Models.News item)
        {
            var model = db1.News;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_NewsViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialUpdate(DXWebMRCS.Models.News item)
        {
            var model = db1.News;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.CID == item.CID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_NewsViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialDelete(System.Int32 CID)
        {
            var model = db1.News;
            if (CID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.CID == CID);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_NewsViewPartial", model.ToList());
        }
    }
    public class NewsControllerBodyEngSettings1
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "News", Action = "BodyEngPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

    public class NewsControllerImageSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
            MaxFileSize = 4000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                // Save uploaded file to some location
            }
        }
    }


    public class NewsControllerBodyMonSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "News", Action = "BodyMonPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}
