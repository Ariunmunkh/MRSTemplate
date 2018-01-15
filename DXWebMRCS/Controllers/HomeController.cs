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
using System.Collections;

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
            menuList = db.Database.SqlQuery<Menu>("SELECT * FROM Menu WHERE BranchID IS NULL").ToList();
            return PartialView("_HeaderPartial", menuList);
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

            return View("Index");
        }

        #region Home Partial Page
        [HttpGet]
        public ActionResult SliderViewPartial()
        {
            var list = db.SliderPhotos.ToList();
            return PartialView("_SliderViewPartial", list);
        }

        [HttpGet]
        public ActionResult RecentNewsPartial()
        {
            var newslist = db.Database.SqlQuery<News>("SELECT TOP 3 * FROM News ORDER BY Date DESC").ToList();
            return PartialView("_RecentNewsPartial", newslist);
        }

        [HttpGet]
        public ActionResult TrainingEventPartial()
        {
            var user = HttpContext.Session["UserProfile"] as UserProfile;
            var user2 = HttpContext.Application["UserProfile"] as UserProfile;
            IEnumerable<TrainingModel> traininglist1;
            IEnumerable<TrainingModel> traininglist2;
            if (WebSecurity.CurrentUserId > 0)
            {
                traininglist1 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(3) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId + " WHERE Type = 2").ToList();

                traininglist2 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(7) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId + " WHERE Type = 1").ToList();     
            }
            else
            {
                traininglist1= db.Database.SqlQuery<TrainingModel>("SELECT TOP(3) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
                                                                    "FROM Trainings t WHERE Type = 2").ToList();

                traininglist2 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(7) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
                                                                    "FROM Trainings t WHERE Type = 1").ToList();
            }
            var model = new Tuple<IEnumerable<TrainingModel>, IEnumerable<TrainingModel>>(traininglist1, traininglist2);
            return PartialView("_TrainingEventPartial", model);
        }


        [HttpGet]
        public ActionResult TrainingDetailUserListPartial(int trainingId)
        {
            var userlist = db.Database.SqlQuery<UserProfile>("SELECT * FROM UserProfile u INNER JOIN TrainingRequests r ON u.UserId = r.UserID WHERE r.TrainingID = " + trainingId).ToList();
            return PartialView("_TrainingDetailUserListPartial", userlist);
        }
        #endregion

        #region Training

        public ActionResult TrainingEventDetail(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingModel training;
            if (WebSecurity.CurrentUserId > 0)
            {
                training = db.Database.SqlQuery<TrainingModel>("SELECT TOP(1) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId).FirstOrDefault();
            }
            else
            {
                training = db.Database.SqlQuery<TrainingModel>("SELECT TOP(1) t.TrainingID, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
                                                                    "FROM Trainings t ").FirstOrDefault();
            }
            if (training == null)
            {
                return HttpNotFound();
            }
            return View(training);
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

        [Authorize]
        public ActionResult TrainingRegister(int id)
        {
            var user = HttpContext.Session["UserProfile"] as UserProfile;
            var user2 = HttpContext.Application["UserProfile"] as UserProfile;
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
                return Json(new { success = true, userId = request.UserID }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult TrainingCancel(int userid, int trainingid)
        {
            db.Database.ExecuteSqlCommand("UPDATE TrainingRequests SET [Status] = 9 WHERE UserID = " + userid + " AND TrainingID = " + trainingid);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        
    }
}