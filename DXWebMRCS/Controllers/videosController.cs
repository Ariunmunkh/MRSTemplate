using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using System.Web.UI;

namespace DXWebMRCS.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class videosController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: videos
        [AllowAnonymous]
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Any)]
        public ActionResult Index()
        {
            return View(db.videos.ToList());
        }
        public ActionResult List()
        {
            return View(db.videos.ToList());
        }
        // GET: videos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VID,title,Link")] videos videos)
        {
            if (ModelState.IsValid)
            {
                db.videos.Add(videos);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(videos);
        }

        // GET: videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            videos videos = db.videos.Find(id);
            if (videos == null)
            {
                return HttpNotFound();
            }
            return View(videos);
        }

        // POST: videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VID,title,Link")] videos videos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(videos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(videos);
        }

        // GET: videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            videos videos = db.videos.Find(id);
            if (videos == null)
            {
                return HttpNotFound();
            }
            return View(videos);
        }

        // POST: videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            videos videos = db.videos.Find(id);
            db.videos.Remove(videos);
            db.SaveChanges();
            return RedirectToAction("List");
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
