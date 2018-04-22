using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using System.IO;
using ImageResizer;

namespace DXWebMRCS.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class DonorSayController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: DonorSay
        public ActionResult Index()
        {
            return View(db.Donor.ToList());
        }

        // GET: DonorSay/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donor.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // GET: DonorSay/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonorSay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DID,Pnamemon,Pnameeng,ParagraphMon,ParagraphEng,PositionMon,PositionEng,Image")] Donor donor, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string subPath = "/Content/Images/DonorSay"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    donor.Image = "/Content/Images/DonorSay/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/DonorSay/"), fileName);
                    ImageFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }
                db.Donor.Add(donor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donor);
        }

        // GET: DonorSay/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donor.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // POST: DonorSay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DID,Pnamemon,Pnameeng,ParagraphMon,ParagraphEng,PositionMon,PositionEng")] Donor donor, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {

                    string subPath = "/Content/Images/DonorSay"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    donor.Image = "/Content/Images/DonorSay/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/DonorSay/"), fileName);
                    ImageFile.SaveAs(fileName);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }
                db.Entry(donor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donor);
        }

        // GET: DonorSay/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donor donor = db.Donor.Find(id);
            if (donor == null)
            {
                return HttpNotFound();
            }
            return View(donor);
        }

        // POST: DonorSay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donor donor = db.Donor.Find(id);
            db.Donor.Remove(donor);
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
