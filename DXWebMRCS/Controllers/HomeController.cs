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
using System.Threading.Tasks;
using System.Text;
using System.Web.UI;

namespace DXWebMRCS.Controllers
{
    [InitializeSimpleMembership]
    //[RequireHttps]
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();
        public IEnumerable<Menu> menuList;
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView     
            return View();    
        }
        protected void sendEmail()
        {
            var elist = db.Database.SqlQuery<Subscribes>(@"select * from subscribes").ToList();
            string systemEmailAddress = "info@redcross.mn";
            string systemEmailPassword = "Mrcs323334";
            string systemEmailHost = "smtp.office365.com";
            string systemEmailPort = "587";
            foreach (var item in elist)
            {
                SmtpClient client = new SmtpClient(systemEmailHost, Convert.ToInt32(systemEmailPort));
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(systemEmailAddress, systemEmailPassword);

                var body = string.Format("Сайн байна уу,<BR/>" +
                                         "Нийгүүлсэл сэтгүүлийн шинэ дугаар гарлаа. Та доорх холбоосоор уншиж танилцаарай.<BR/>" +
                                         "<a href='www.redcross.mn/Home/Magazine'>ХОЛБООС</a><BR/>" +
                                         "<BR/>Хүндэтгэсэн,<BR/>Монголын улаан загалмай нийгэмлэг", DateTime.Now.Year);

                MailMessage mailMessage = new MailMessage(systemEmailAddress, item.email, "Нийгүүлсэл сэтгүүл шинэ дугаар", body);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
            }
        }
        //Branch uudiin list user haragdah
        public ActionResult BranchList()
        {
            var branchlist = db.Database.SqlQuery<Branch>("SELECT * FROM Branches").ToList();
            return View(branchlist);
        }

       

        #region File content Hadgalsan Pdf bolon Docx haruulah heseg
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Client)]
        public ActionResult FileContentView()
        {
            var pageNumber = 1;
            var pageSize = 8;
            var files = db.Database.SqlQuery<FileContent>("SELECT * FROM FileContents ORDER BY Id DESC").ToPagedList(pageNumber, pageSize);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        [AllowAnonymous]
        public ActionResult FilePageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var filelist = db.Database.SqlQuery<FileContent>("SELECT * FROM FileContents ORDER BY Id DESC").ToPagedList(pageNumber, pageSize);
            if (filelist == null)
            {
                return HttpNotFound();
            }
            return View("FileContentView", filelist);
        }

        public FileResult DownLoad(int id)
        {
            var file = db.Database.SqlQuery<FileContent>("SELECT TOP 1 * FROM FileContents WHERE id = " + id).FirstOrDefault();

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content\\FileContents\\";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + file.FileName + file.FileExtension);
            string fileName = file.FileName + file.FileExtension;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        } 
        #endregion

        #region Magazines
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Client)]
        public ActionResult Magazine()
        {
            var pageNumber = 1;
            var pageSize = 8;
            var files = db.Database.SqlQuery<Magazine>("SELECT * FROM Magazines ORDER BY Id DESC").ToPagedList(pageNumber, pageSize);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        [AllowAnonymous]
        public ActionResult MagazinePageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var filelist = db.Database.SqlQuery<Magazine>("SELECT * FROM Magazines ORDER BY Id DESC").ToPagedList(pageNumber, pageSize);
            if (filelist == null)
            {
                return HttpNotFound();
            }
            return View("Magazine", filelist);
        }

        public FileResult DownLoadMagazine(int id)
        {
            var file = db.Database.SqlQuery<Magazine>("SELECT TOP 1 * FROM Magazines WHERE id = " + id).FirstOrDefault();

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content\\Magazine\\";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + file.FileName + file.FileExtension);
            string fileName = file.FileName + file.FileExtension;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        #region Partial Page
        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult GalleryViewPartial()
        {
            var taglist = db.Database.SqlQuery<Tag>("SELECT * FROM Tags").ToList();
            var galleryList = db.Database.SqlQuery<GalleryModel>("SELECT g.GalleryID, g.TitleMon, g.TitleEng, g.Image, d.TagID FROM Galleries g INNER JOIN TagDetails d ON g.GalleryID = d.SourceID WHERE d.Source = 'Galleries'").ToList();

            var model = new Tuple<IEnumerable<Tag>, IEnumerable<GalleryModel>>(taglist, galleryList);

            return PartialView("_GalleryViewPartial", model);
        }
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult DonorSayViewPartial()
        {
            var model = db.Donor.ToList();
            return PartialView("_DonorSayViewPartial", model);
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
        #endregion

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

            return Redirect(Request.UrlReferrer.ToString());
        }
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult ProjectListPartial()
        {
            var model = db.Projects.ToList();
            return PartialView("_ProjectListPartial", model);
        }

        //public  async Task<ActionResult> Contact(ContactViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
        //        var message = new MailMessage();
        //        message.To.Add(new MailAddress("nyamsambuu.d@redcross.mn"));  // replace with valid value 
        //        message.Subject = "Таны санал хүсэлтийг хүлээн авлаа";
        //        message.Body = string.Format(body, model.Name, model.Email, model.Message);
        //        message.IsBodyHtml = true;

        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "nyamsambuu.d@redcross.mn",  // replace with valid value
        //                Password = "Pa$$word123"  // replace with valid value
        //            };
        //            smtp.Credentials = credential;
        //            smtp.Host = "smtp.office365.com";
        //            smtp.Port = 587;
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("Sent");
        //        }
        //    }
        //    return View(model);
        //}
        public ActionResult Contact()
        {
            return View();
        }
        #region Home Partial Page
        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult SliderViewPartial()
        {
            var list = db.SliderPhotos.ToList();
            return PartialView("_SliderViewPartial", list);
        }

        [HttpGet]
        public ActionResult RecentNewsPartial()
        {
            var newslist = db.Database.SqlQuery<News>("SELECT TOP 3 * FROM News WHERE BranchId IS NULL ORDER BY Date DESC").ToList();
            return PartialView("_RecentNewsPartial", newslist);
        }

        //Surgalt bolon Uil ajillagaa
        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult TrainingEventPartial()
        {
            var user = HttpContext.Session["UserProfile"] as UserProfile;
            var user2 = HttpContext.Application["UserProfile"] as UserProfile;
            IEnumerable<TrainingModel> traininglist1;
            IEnumerable<TrainingModel> traininglist2;
            if (WebSecurity.CurrentUserId > 0)
            {
                traininglist1 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(3) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId + " WHERE Type = 2").ToList();

                traininglist2 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(7) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId + " WHERE Type = 1").ToList();     
            }
            else
            {
                traininglist1 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(3) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
                                                                    "FROM Trainings t WHERE Type = 2").ToList();

                traininglist2 = db.Database.SqlQuery<TrainingModel>("SELECT TOP(7) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
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
            if (id <=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingModel training;
            if (WebSecurity.CurrentUserId > 0)
            {
                training = db.Database.SqlQuery<TrainingModel>("SELECT TOP(1) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                                    "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId).FirstOrDefault();
            }
            else
            {
                training = db.Database.SqlQuery<TrainingModel>("SELECT TOP(1) t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status] " +
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
            var list = db.Database.SqlQuery<TrainingModel>("SELECT t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
                                                           "FROM Trainings t LEFT JOIN TrainingRequests r ON t.TrainingID = r.TrainingID AND r.UserID = " + WebSecurity.CurrentUserId).ToPagedList(pageNumber, pageSize);
            return View(list);
        }

        public ActionResult TrainingPageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var list = db.Database.SqlQuery<TrainingModel>("SELECT t.TrainingID, t.Image, t.NameMon, t.NameEng, t.ContentMon, t.ContentEng, t.[Where], t.[When], t.Duration, t.[Status], r.ID AS RequestID, r.UserID, r.[Status] AS RequestStatus " +
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
            //var user = HttpContext.Session["UserProfile"] as UserProfile;
            //var user2 = HttpContext.Application["UserProfile"] as UserProfile;
            var count = db.Database.SqlQuery<TrainingRequest>("SELECT TOP 1 * FROM TrainingRequests WHERE TrainingID = " + id + " AND UserID = " + WebSecurity.CurrentUserId).FirstOrDefault();
            if (count != null)
            {
                if (count.Status == 1)
                {
                    db.Database.ExecuteSqlCommand("UPDATE TrainingRequests SET [Status] = 0 WHERE UserID = " + WebSecurity.CurrentUserId + " AND TrainingID = " + id);
                    return Json(new { success = true, userId = count.UserID, status = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE TrainingRequests SET [Status] = 1 WHERE UserID = " + WebSecurity.CurrentUserId + " AND TrainingID = " + id);
                    return Json(new { success = true, userId = count.UserID, status = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TrainingRequest request = new TrainingRequest();
                request.TrainingID = id;
                request.UserID = WebSecurity.CurrentUserId;
                request.Status = 1;
                db.TrainingRequests.Add(request);
                db.SaveChanges();
                return Json(new { success = true, userId = request.UserID, status = 1 }, JsonRequestBehavior.AllowGet);
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