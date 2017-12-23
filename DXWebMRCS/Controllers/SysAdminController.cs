using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DXWebMRCS.Models;
using System.IO;
using ImageResizer;
using System.Data.Entity;

namespace DXWebMRCS.Controllers
{
    public class SysAdminController : Controller
    {
        private UsersContext db = new UsersContext();
        //
        // GET: /SysAdmin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HtmlEditorPartial()
        {
            return PartialView("_HtmlEditorPartial");
        }
        public ActionResult HtmlEditorPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", SysAdminControllerHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditorPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor", SysAdminControllerHtmlEditorSettings.ImageUploadValidationSettings, SysAdminControllerHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }

        [HttpPost]
        public ActionResult SliderAdd(SliderPhoto slider, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string fileNameMedium = fileName + "1920x1280" + DateTime.Now.ToString("yymmssfff") + extension;
                slider.ImagePath = "/Content/Images/SliderImage/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/Images/SliderImage/"), fileName);
                ImageFile.SaveAs(fileName);
                ResizeSettings resizeSetting = new ResizeSettings
                {
                    Width = 1920,
                    Height = 1280,
                    Format = "jpg"
                };

                ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                slider.CreatedDate = DateTime.Now;
                if (slider != null && slider.Id > 0)
                {
                    db.Entry(slider).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("SliderList", db.SliderPhotos.ToList());
                }
                else
                {
                    db.SliderPhotos.Add(slider);
                    db.SaveChanges();
                    return View("SliderList", db.SliderPhotos.ToList());
                }
            }

            return View(slider);
        }

        [HttpGet]
        public ActionResult SliderAdd(int id)
        {
            if (id > 0)
            {
                SliderPhoto slider = new SliderPhoto();
                slider = db.SliderPhotos.FirstOrDefault(x => x.Id == id);
                return View(slider);
            }
            else
            {
                SliderPhoto slider = new SliderPhoto();
                return View(slider);
            }
        }

        [HttpPost]
        public ActionResult SliderDelete(int id)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM SliderPhotoes WHERE id = " + id);
            return Json("Success");
        }

        [HttpGet]
        public ActionResult SliderList()
        {
            // DXCOMMENT: Pass a data model for GridView      
            var list = db.SliderPhotos.OrderByDescending(x => x.CreatedDate).ToList();
            return View(list);
        }
	}

    public class SysAdminControllerHtmlEditorSettings
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "SysAdmin", Action = "HtmlEditorPartialImageSelectorUpload" };
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