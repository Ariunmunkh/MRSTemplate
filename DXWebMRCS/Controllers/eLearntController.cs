using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using WebMatrix.WebData;
using DXWebMRCS.Filters;

namespace DXWebMRCS.Controllers
{
    
    public class eLearntController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /eLearnt/
        [InitializeSimpleMembership]
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
                                 where t.UserId = "+WebSecurity.CurrentUserId+@"		and exists (select null from 
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

        // GET: /eLearnt/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eService eservice = await db.eService.FindAsync(id);
            if (eservice == null)
            {
                return HttpNotFound();
            }
            return View(eservice);
        }

        // GET: /eLearnt/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "LastName");
            return View();
        }

        // POST: /eLearnt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="eServiceID,UserId,LessonId,Score,DateTime")] eService eservice)
        {
            if (ModelState.IsValid)
            {
                db.eService.Add(eservice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "LastName", eservice.UserId);
            return View(eservice);
        }

        // GET: /eLearnt/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eService eservice = await db.eService.FindAsync(id);
            if (eservice == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "LastName", eservice.UserId);
            return View(eservice);
        }

        // POST: /eLearnt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="eServiceID,UserId,LessonId,Score,DateTime")] eService eservice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eservice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "LastName", eservice.UserId);
            return View(eservice);
        }

        // GET: /eLearnt/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eService eservice = await db.eService.FindAsync(id);
            if (eservice == null)
            {
                return HttpNotFound();
            }
            return View(eservice);
        }

        // POST: /eLearnt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            eService eservice = await db.eService.FindAsync(id);
            db.eService.Remove(eservice);
            await db.SaveChangesAsync();
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
    }
}
