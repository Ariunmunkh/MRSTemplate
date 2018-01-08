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
using WebMatrix.WebData;
using System.Web.Security;
using DXWebMRCS.Filters;
using System.Net;

namespace DXWebMRCS.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
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

        public ActionResult UserProfile()
        {
            var model = db.Database.SqlQuery<EditRegisterModel>("SELECT TOP 1 UserId as Id, LastName, Name AS UserName, BirthOfDay, PhoneNumber, UserName AS Email, CONVERT(varchar(5), Type) AS TYPE FROM UserProfile WHERE UserId = " + WebSecurity.CurrentUserId).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult UserProfile(EditRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var _user = db.UserProfiles.Find(model.Id);
                _user.LastName = model.LastName;
                _user.Name = model.UserName;
                _user.UserName = model.Email;
                _user.PhoneNumber = model.PhoneNumber;
                _user.Type = Convert.ToInt32(model.Type);

                db.Entry(_user).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(model);
        }

        #region Slider
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
        #endregion

        #region Branch
        [AllowAnonymous]
        public ActionResult BranchUserCreate()
        {
            ViewBag.BranchList = db.Branchs.ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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


        [ValidateInput(false)]
        public ActionResult UserManagerViewPartial()
        {
            var model = db.UserProfiles;
            return PartialView("_UserManagerViewPartial", model.ToList());
        }

        public ActionResult BranchUserEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile user = db.UserProfiles.Find(id);
            ViewBag.BranchList = db.Branchs.ToList();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchUserEdit([Bind(Include = "UserId,UserName,Name,BranchId")] UserProfile User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(User);
        }


        public ActionResult BranchUserDelete(int id)
        {
            if (id >= 0)
            {
                try
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM webpages_UsersInRoles WHERE UserId = " + id + " " +
                                                  "DELETE FROM UserProfile WHERE UserId = " + id + " " +
                                                  "DELETE FROM webpages_Membership WHERE UserId = " + id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return RedirectToAction("index");
        } 
        #endregion
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