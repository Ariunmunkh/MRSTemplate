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
    public class lessonidController : Controller
    {
        
        private UsersContext db = new UsersContext();

        // GET: lessonid
        public ActionResult Index()
        {
            return View(db.lessonid.ToList());
        }

        // GET: lessonid/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: lessonid/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LID,lname")] lessonid lessonid)
        {
            if (ModelState.IsValid)
            {
                db.lessonid.Add(lessonid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lessonid);
        }

        // GET: lessonid/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lessonid lessonid = db.lessonid.Find(id);
            if (lessonid == null)
            {
                return HttpNotFound();
            }
            return View(lessonid);
        }

        // POST: lessonid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LID,lname")] lessonid lessonid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessonid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lessonid);
        }

        // GET: lessonid/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lessonid lessonid = db.lessonid.Find(id);
            if (lessonid == null)
            {
                return HttpNotFound();
            }
            return View(lessonid);
        }

        // POST: lessonid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lessonid lessonid = db.lessonid.Find(id);
            db.lessonid.Remove(lessonid);
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
