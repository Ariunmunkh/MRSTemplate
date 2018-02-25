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
using System.IO;
using ImageResizer;
using PagedList;
using System.Text;
using DevExpress.Web;


namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class ProjectController : Controller
    {
        private UsersContext db = new UsersContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProjectViewPartial()
        {
            var model = db.Projects;
            return PartialView("_ProjectViewPartial", db.Projects.ToList());
        }

        #region Project Create, Edit, Delete
        // GET: /Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ImageMedium,BeginDate,EndDate,Type")] Project Project, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {

                    string subPath = "/Content/Images/ProjectImage"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    Project.Image = "/Content/Images/ProjectImage/" + fileName;
                    Project.ImageMedium = "/Content/Images/ProjectImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/ProjectImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/ProjectImage/"), fileNameMedium);
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

                db.Projects.Add(Project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Project);
        }

        // GET: /Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = db.Projects.Find(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: /Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ImageMedium,BeginDate,EndDate,Type")] Project Project, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {

                    string subPath = "/Content/Images/ProjectImage"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    Project.Image = "/Content/Images/ProjectImage/" + fileName;
                    Project.ImageMedium = "/Content/Images/ProjectImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/ProjectImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/ProjectImage/"), fileNameMedium);
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

                db.Entry(Project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        // GET: /Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = db.Projects.Find(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project Project = db.Projects.Find(id);
            db.Projects.Remove(Project);
            db.SaveChanges();
            NorthwindDataProvider.DeleteTagDetail(id, "Project");
            return RedirectToAction("Index");
        }
        #endregion

        #region bodyMon
        public ActionResult EditBodyMonPartial()
        {
            return PartialView("EditBodyMonPartial");
        }
        public ActionResult BodyMonPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyMon", ProjectControllerBodyMonSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyMonPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyMon", ProjectControllerBodyMonSettings.ImageUploadValidationSettings, ProjectControllerBodyMonSettings.ImageUploadDirectory);
            return null;
        }

        #endregion

        #region bodyEng

        public ActionResult EditBodyEngPartial()
        {
            return PartialView("EditBodyEngPartial");
        }
        public ActionResult BodyEngPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyEng", ProjectControllerBodyEngSettings1.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyEngPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyEng", ProjectControllerBodyEngSettings1.ImageUploadValidationSettings, ProjectControllerBodyEngSettings1.ImageUploadDirectory);
            return null;
        }

        #endregion
    }

    public class ProjectControllerBodyEngSettings1
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Project", Action = "BodyEngPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }


    }

    public class ProjectControllerImageSettings
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

    public class ProjectControllerBodyMonSettings
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Project", Action = "BodyMonPartialImageSelectorUpload" };
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