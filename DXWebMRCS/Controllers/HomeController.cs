using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using System.Threading;
using System.Globalization;
using ImageResizer;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;
using WebMatrix.WebData;
using PagedList;
using DXWebMRCS.Filters;

namespace DXWebMRCS.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();
        public IEnumerable<Menu> menuList;
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView            
            return View();    
        }
        
        public ActionResult GridViewPartialView() 
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView();
        }
                
        public ActionResult _HeaderPartial()
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            if (menuList != null && menuList.Count() > 0)
            {
                 return PartialView("_HeaderPartial", menuList);
            }
            menuList = db.Menus.ToList();
            return PartialView("_HeaderPartial", menuList);
        }

        public ActionResult Change(String languageAbbrevation)
        {
            if (languageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbrevation);
            }
            
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = languageAbbrevation;
            Response.Cookies.Add(cookie);

            return View("Index");
        }

        [HttpGet]
        public ActionResult SliderViewPartial()
        {
            var list = db.SliderPhotos.ToList();
            return PartialView("_SliderViewPartial", list);
        }

        public ActionResult TrainingList()
        {
            var pageNumber = 1;
            var pageSize = 8;
            var list = db.Database.SqlQuery<TrainingModel>("SELECT t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                           "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId).ToPagedList(pageNumber, pageSize);
            return View(list);
        }

        public ActionResult TrainingPageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var list = db.Database.SqlQuery<TrainingModel>("SELECT t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                          "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId).ToPagedList(pageNumber, pageSize);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View("TrainingList", list);
        }

        public ActionResult TrainingRegister(int id)
        {
            var count = db.Database.SqlQuery<TrainingRequest>("SELECT TOP 1 * FROM TrainingRequests WHERE TrainingID = " + id + " AND UserID = " + WebSecurity.CurrentUserId).FirstOrDefault();
            if (count != null)
            {
                db.Database.ExecuteSqlCommand("UPDATE TrainingRequests SET [Status] = 0 WHERE UserID = " + WebSecurity.CurrentUserId + " AND TrainingID = " + id);
                return Json(new { success = true, userId = count.UserID }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TrainingRequest request = new TrainingRequest();
                request.TrainingID = id;
                request.UserID = WebSecurity.CurrentUserId;
                request.Status = 0;
                db.TrainingRequests.Add(request);
                db.SaveChanges();
                return Json(new {success = true, userId = request.UserID }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TrainingCancel(int userid, int trainingid)
        {
            db.Database.ExecuteSqlCommand("UPDATE TrainingRequests SET [Status] = 9 WHERE UserID = " + userid + " AND TrainingID = " + trainingid);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        
    }
}