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
    public class ElearningController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /Elearning/
        public ActionResult Index()
        {
            return View(db.Elearn.ToList());
        }

        // GET: /Elearning/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elearn elearn = db.Elearn.Find(id);
            if (elearn == null)
            {
                return HttpNotFound();
            }
            return View(elearn);
        }

        // GET: /Elearning/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Elearning/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ELID,LessonName,Description,Image,LessonBody,Time")] Elearn elearn)
        {
            if (ModelState.IsValid)
            {
                elearn.Date = DateTime.Now;
                db.Elearn.Add(elearn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(elearn);
        }

        // GET: /Elearning/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elearn elearn = db.Elearn.Find(id);
            if (elearn == null)
            {
                return HttpNotFound();
            }
            return View(elearn);
        }

        // POST: /Elearning/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ELID,LessonName,Description,Image,LessonBody,Time,Date")] Elearn elearn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elearn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(elearn);
        }

        // GET: /Elearning/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elearn elearn = db.Elearn.Find(id);
            if (elearn == null)
            {
                return HttpNotFound();
            }
            return View(elearn);
        }

        // POST: /Elearning/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Elearn elearn = db.Elearn.Find(id);
            db.Elearn.Remove(elearn);
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

        [ValidateInput(false)]
        public ActionResult FileManagerPartial()
        {
            return PartialView("_FileManagerPartial", ElearningControllerFileManagerSettings.Model);
        }

        public FileStreamResult FileManagerPartialDownload()
        {
            return FileManagerExtension.DownloadFiles("FileManager", ElearningControllerFileManagerSettings.Model);
        }

        public ActionResult ImageUploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("ImageUploadControl", ElearningControllerImageUploadControlSettings.UploadValidationSettings, ElearningControllerImageUploadControlSettings.FileUploadComplete);
            return null;
        }

        public ActionResult ImageHTMLEditorPartial(Elearn objective)
        {
            return PartialView("_ImageHTMLEditorPartial", objective);
        }
        public ActionResult ImageHTMLEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("ImageHtmlEditor", ElearningControllerImageHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult ImageHTMLEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("ImageHtmlEditor", ElearningControllerImageHtmlEditorSettings.ImageUploadValidationSettings, ElearningControllerImageHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }
    }
    public class ElearningControllerImageHtmlEditorSettings
    {
        public const string ImageUploadDirectory = "~/Content/elearn/images";
        public const string ImageSelectorThumbnailDirectory = "~/Content/elearn/images";

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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Elearning", Action = "ImageHTMLEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

    public class ElearningControllerImageUploadControlSettings
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

    public class ElearningControllerFileManagerSettings
    {
        public const string RootFolder = @"~\Content\Uploadfile";

        public static string Model { get { return RootFolder; } }
    }

    public class UploadFileManagerSettings
    {
        public const string RootFolder = @"~\Content\Uploadfile";

        public static string Model { get { return RootFolder; } }
    }
    
}
