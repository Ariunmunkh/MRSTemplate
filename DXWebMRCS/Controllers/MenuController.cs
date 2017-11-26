using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using DevExpress.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    public class MenuController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /Menu/
        public ActionResult Index()
        {
            var menus = db.Menus;// .Include(m => m.Menu1);
            return View(menus.ToList());
        }

        // GET: /Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: /Menu/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Menus, "MenuID", "NameMon");
            return View();
        }

        // POST: /Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MenuID,NameMon,NameEng,NavigateUrl,BodyMon,BodyEng,Image,ParentId")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.Date = DateTime.Now;
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Menus, "MenuID", "NameMon", menu.ParentId);
            return View(menu);
        }

        // GET: /Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Menus, "MenuID", "NameMon", menu.ParentId);
            return View(menu);
        }

        // POST: /Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MenuID,NameMon,NameEng,NavigateUrl,BodyMon,BodyEng,Image,Date,ParentId")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Menus, "MenuID", "NameMon", menu.ParentId);
            return View(menu);
        }

        // GET: /Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: /Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult MenuBodyMonPartial()
        {
            return PartialView("_MenuBodyMonPartial");
        }
        public ActionResult MenuBodyMonPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("MenuBodyMon", MenuControllerMenuBodyMonSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult MenuBodyMonPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("MenuBodyMon", MenuControllerMenuBodyMonSettings.ImageUploadValidationSettings, MenuControllerMenuBodyMonSettings.ImageUploadDirectory);
            return null;
        }

        public ActionResult MenuBodyEngPartial()
        {
            return PartialView("_MenuBodyEngPartial");
        }
        public ActionResult MenuBodyEngPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("MenuBodyEng", MenuControllerMenuBodyEngSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult MenuBodyEngPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("MenuBodyEng", MenuControllerMenuBodyEngSettings.ImageUploadValidationSettings, MenuControllerMenuBodyEngSettings.ImageUploadDirectory);
            return null;
        }
    }
    public class MenuControllerMenuBodyEngSettings
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Menu", Action = "MenuBodyEngPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

    public class MenuControllerMenuBodyMonSettings
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Menu", Action = "MenuBodyMonPartialImageSelectorUpload" };
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
