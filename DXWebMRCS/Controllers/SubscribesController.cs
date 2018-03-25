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
    public class SubscribesController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: Subscribes
        public ActionResult Index()
        {
            return View(db.Subscribes.ToList());
        }

        // GET: Subscribes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscribes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubID,email")] Subscribes subscribes)
        {
            if (ModelState.IsValid)
            {
                db.Subscribes.Add(subscribes);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return null;
        }

        // GET: Subscribes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribes subscribes = db.Subscribes.Find(id);
            if (subscribes == null)
            {
                return HttpNotFound();
            }
            return View(subscribes);
        }

        // POST: Subscribes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscribes subscribes = db.Subscribes.Find(id);
            db.Subscribes.Remove(subscribes);
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
