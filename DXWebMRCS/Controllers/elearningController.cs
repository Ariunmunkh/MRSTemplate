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
using WebMatrix.WebData;
using DXWebMRCS.Filters;
using System.Web.UI;

namespace DXWebMRCS.Controllers
{
    [Authorize]
    [RequireHttps]
    public class elearningController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /elearning/
        [RequireHttps]
        [InitializeSimpleMembership]
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var eservice = db.Database.SqlQuery<eServiceModel>(@"select  t.UserId, 
                                       sc.LessonName, 
	                                   sc.LessonBody, 
                                       sc.Description,
	                                   sc.Image, 
	                                   t.Score as totalscore, 
	                                   t.DateTime as nowdate,
                                       sum(sc.Time)Time
                                  from Elearns as sc 
                                  left join eServices as t 
                                    On t.LessonId = sc.ELID
                                 left join UserProfile as st 
                                    on t.UserId = st.UserId
                                 where t.UserId = " + WebSecurity.CurrentUserId + @"		and exists (select null from 
                                 (select userid,lessonid,max(score)score from eServices group by userid, lessonid) tbl where tbl.userid = t.userid and tbl.lessonid = t.lessonid and tbl.score = t.score)
		
			                                 group by t.UserId, 
                                       sc.LessonName, 
	                                   sc.LessonBody,
                                       sc.Description, 
	                                   sc.Image, 
	                                   t.Score, 
	                                   t.DateTime");
            var list = eservice.ToList();

            return View(list);
        }

        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult List()
        {
            return View(db.Elearn.ToList());
        }
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult Search(string lessonName)
        {
            var str = "Хайлт олдсонгүй";
            if (lessonName == null)
            {
                return View(str.ToString());
            }
            else
            {
                var employees = from emps in db.Elearn
                                where emps.LessonName.Contains(lessonName)
                                select emps;
                return View(employees.ToList());
            }
        }
        public static int[] TotalScore(int id)
        {
            if (id == -1)
            {
                return new int[] { 0, 0 };
            }
            UsersContext db1 = new UsersContext();
            var totalscore = db1.Database.SqlQuery<CountViewModel>(@"select UserProfile.userid,
                                                                    sum(isnull(Elearns.time,0)) timeSum, 
                                                                    COUNT (DISTINCT Elearns.elid) lessonCount 
                                                                    from UserProfile 
                                                                    left join eServices 
                                                                    on eServices.userid = " + id + @"
                                                                    left join Elearns on Elearns.elid =  eServices.lessonid
                                                                    group by UserProfile.userid ").FirstOrDefault();
            return new int[] { totalscore.lessonCount, totalscore.timeSum };
        }
        public static IEnumerable<eServiceModel> CertCount()
        {
            UsersContext db2 = new UsersContext();
            var certcount = db2.Database.SqlQuery<eServiceModel>(@"select es.eServiceId, el.LessonName from eservices 
                                                                    as es left join Elearns as el 
                                                                    on es.LessonId = el.Elid left join UserProfile as us 
                                                                    on es.UserId = us.Userid 
                                                                    where es.UserId =" + WebSecurity.CurrentUserId).ToList();
            return certcount.ToList();

        }
        [InitializeSimpleMembership]
        // GET: /elearning/Details/5
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "ID", Location = OutputCacheLocation.Client)]
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
        // GET: /elearning/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /elearning/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ELID,LessonName,Description,Image,LessonBody,Time")] Elearn elearn)
        {
            if (ModelState.IsValid)
            {
                elearn.Date = DateTime.Now;
                db.Elearn.Add(elearn);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(elearn);
        }

        // GET: /elearning/Edit/5
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

        // POST: /elearning/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ELID,LessonName,Description,Image,LessonBody,Time")] Elearn elearn)
        {
            if (ModelState.IsValid)
            {
                elearn.Date = DateTime.Now;
                db.Entry(elearn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(elearn);
        }

        // GET: /elearning/Delete/5
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

        // POST: /elearning/Delete/5
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

        public ActionResult ImageHtmlEditPartial()
        {
            return PartialView("ImageHtmlEditPartial");
        }
        public ActionResult ImageHtmlEditPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("Image", elearningControllerImageSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult ImageHtmlEditPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("Image", elearningControllerImageSettings.ImageUploadValidationSettings, elearningControllerImageSettings.ImageUploadDirectory);
            return null;
        }
    }
    public class elearningControllerImageSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb";

        public static string GetHtmlContentByFileName(string fileName)
        {
            return System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(string.Format("{0}{1}", ImageUploadDirectory, fileName)));
        }

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 8000000
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
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "elearning", Action = "ImageHtmlEditPartialImageSelectorUpload" };
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
