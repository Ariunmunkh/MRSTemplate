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

namespace DXWebMRCS.Controllers
{
    //[RequireHttps]
    [Authorize(Roles = "Admin")]
    public class jobdetailsController : Controller
    {
        private UsersContext db = new UsersContext();
        // GET: jobdetails
        public ActionResult Index()
        {
            return View(db.jobdetails.ToList());
        }
        public FileResult DownLoad(int id)
        {
            var file = db.Database.SqlQuery<jobdetail>("SELECT TOP 1 * FROM jobdetails WHERE JDID = " + id).FirstOrDefault();

            string path = AppDomain.CurrentDomain.BaseDirectory + "Content\\CV\\";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + file.FileName + file.FileExtension);
            string fileName = file.FileName + file.FileExtension;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: jobdetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jobdetail jobdetail = db.jobdetails.Find(id);
            if (jobdetail == null)
            {
                return HttpNotFound();
            }
            return View(jobdetail);
        }
        public IEnumerable<jobdetail> GetJobName()
        {
            var result = db.Database.SqlQuery<jobdetail>("SELECT JID, JobName from jobdetails");
            return result;
        }  
        // GET: jobdetails/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.JID = new SelectList(db.jobs, "JobName", "JobName");
            return View();
        }

        // POST: jobdetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(jobdetail model, HttpPostedFileBase ImageFile)
        {
            
            if (ModelState.IsValid)
            {
                string imageName = string.Empty;
                string fileName = string.Empty;
                String FileExt = Path.GetExtension(model.File.FileName).ToUpper();

                if (FileExt == ".PDF" || FileExt == ".DOCX")
                {

                    string subPath = "/Content/CV"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    if (model.File != null)
                    {
                        fileName = Path.GetFileNameWithoutExtension(model.File.FileName);
                        string extension2 = Path.GetExtension(model.File.FileName);

                        model.FileName = fileName + DateTime.Now.ToString("yymmssfff");
                        model.FileExtension = extension2;
                        fileName = model.FileName + extension2;

                        model.FilePath = "/Content/CV/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/CV/"), fileName);
                        model.File.SaveAs(fileName);
                    }
                    if (model != null)
                    {
                        db.Entry(model).State = EntityState.Modified;
                        model.DateEnd = System.DateTime.Now;
                        db.jobdetails.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("index", "jobs");
                    }
                    
                }
            }

            return PartialView(model);
        }

        public ActionResult application()
        {
            ViewBag.JID = new SelectList(db.jobs, "JobName", "JobName");            
            return View();
        }

        // POST: jobdetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult application(jobdetail model, HttpPostedFileBase ImageFile)
        {

            if (ModelState.IsValid)
            {
                string imageName = string.Empty;
                string fileName = string.Empty;
                String FileExt = Path.GetExtension(model.File.FileName).ToUpper();

                if (FileExt == ".PDF" || FileExt == ".DOCX")
                {

                    string subPath = "/Content/CV"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    if (model.File != null)
                    {
                        fileName = Path.GetFileNameWithoutExtension(model.File.FileName);
                        string extension2 = Path.GetExtension(model.File.FileName);

                        model.FileName = fileName + DateTime.Now.ToString("yymmssfff");
                        model.FileExtension = extension2;
                        fileName = model.FileName + extension2;

                        model.FilePath = "/Content/CV/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/CV/"), fileName);
                        model.File.SaveAs(fileName);
                    }
                    if (model != null)
                    {
                        db.Entry(model).State = EntityState.Modified;
                        model.DateEnd = System.DateTime.Now;
                        db.jobdetails.Add(model);
                        db.SaveChanges();
                        return RedirectToAction("index", "jobs");
                    }

                }
            }

            return PartialView(model);
        }

        // GET: jobdetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jobdetail jobdetail = db.jobdetails.Find(id);
            if (jobdetail == null)
            {
                return HttpNotFound();
            }
            return View(jobdetail);
        }

        // POST: jobdetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JDID,LastName,FirstName,Email,phone,gender,address,description,cv,DateEnd")] jobdetail jobdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobdetail);
        }

        // GET: jobdetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jobdetail jobdetail = db.jobdetails.Find(id);
            if (jobdetail == null)
            {
                return HttpNotFound();
            }
            return View(jobdetail);
        }

        // POST: jobdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jobdetail jobdetail = db.jobdetails.Find(id);
            db.jobdetails.Remove(jobdetail);
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
