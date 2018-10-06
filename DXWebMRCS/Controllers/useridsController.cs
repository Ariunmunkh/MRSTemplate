using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;

namespace DXWebMRCS.Controllers
{
    //[RequireHttps]
    [Authorize(Roles = "Admin")]
    public class useridsController : Controller
    {
        
        private UsersContext db = new UsersContext();

        // GET: userids
        public ActionResult Index()
        {
            return View(db.userid.ToList());
        }

        // GET: userids/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: userids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,uname")] userid userid)
        {
            if (ModelState.IsValid)
            {
                db.userid.Add(userid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userid);
        }

        // GET: userids/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userid userid = db.userid.Find(id);
            if (userid == null)
            {
                return HttpNotFound();
            }
            return View(userid);
        }

        // POST: userids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,uname")] userid userid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userid);
        }

        // GET: userids/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userid userid = db.userid.Find(id);
            if (userid == null)
            {
                return HttpNotFound();
            }
            return View(userid);
        }

        // POST: userids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userid userid = db.userid.Find(id);
            db.userid.Remove(userid);
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
    }
}
