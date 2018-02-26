using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using DXWebMRCS.Filters;
using System.Web.UI;
using WebMatrix.WebData;
using DevExpress.XtraReports.UI;
using DevExpress.Web.Mvc;


namespace DXWebMRCS.Controllers
{
    [Authorize]
    public class eLearntController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /eLearnt/
        [InitializeSimpleMembership]
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {

            var eservice = db.Database.SqlQuery<eServiceModel>(@"select  t.UserId, 
                                max(t.eServiceid)eServiceid,
                                       sc.LessonName, 
	                                   sc.LessonBody, 
	                                   sc.Image, 
	                                   t.Score as totalscore, 
	                                   t.DateTime as nowdate 
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
	                                   sc.Image, 
	                                   t.Score, 
	                                   t.DateTime");
            var list = eservice.ToList();

            return View(list);
        }

        //[InitializeSimpleMembership]
        // GET: /eLearnt/Details/5

        public ActionResult PDF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var eservice = db.eService.Include(x => x.Elearn).FirstOrDefault(x => x.eServiceID == id);
            if (eservice == null)
            {
                return HttpNotFound();
            }
            //Certificate cert = new Certificate();
            HttpContext.Session.Add("eservice", eservice);
            return View(eservice);
            //return new ActionAsPdf("CreatePdf",eservice, new { id = id }) { FileName = "Certificate.pdf" };
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        getCertificate report = new getCertificate();

        public ActionResult getCertificatePartial()
        {
            return PartialView("_getCertificatePartial", report);
        }

        public ActionResult getCertificatePartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
    }
}
