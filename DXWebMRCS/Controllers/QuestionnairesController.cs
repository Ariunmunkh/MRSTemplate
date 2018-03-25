using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using DevExpress.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class QuestionnairesController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: Questionnaires
        public ActionResult Index()
        {
            var questionnaire = db.Questionnaire.Include(q => q.lessonid).Include(q => q.userid);
            return View(questionnaire.ToList());
        }

        // GET: Questionnaires/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.LID = new SelectList(db.lessonid, "LID", "lname");
            ViewBag.UID = new SelectList(db.userid, "UID", "uname");
            return View();
        }

        // POST: Questionnaires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QID,UID,LID,c1a1,c1a2,c1a3,c1a4,c1a5,c1a6,c2a1,c2a2,c2a3,c2a4,c2a5,c3a1,c3a2,c3a3,c3a4,c3a5")] Questionnaire questionnaire)
        {
            if (ModelState.IsValid)
            {
                questionnaire.DateEnd = System.DateTime.Now;
                db.Questionnaire.Add(questionnaire);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LID = new SelectList(db.lessonid, "LID", "lname", questionnaire.LID);
            ViewBag.UID = new SelectList(db.userid, "UID", "uname", questionnaire.UID);
            return View(questionnaire);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        DXWebMRCS.Models.UsersContext db1 = new DXWebMRCS.Models.UsersContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {

            var model = db1.Questionnaire.Include(q => q.lessonid).Include(q => q.userid);
            ViewBag.LID = new SelectList(db.lessonid, "LID", "lname");
            ViewBag.UID = new SelectList(db.userid, "UID", "uname");
            return PartialView("_GridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(DXWebMRCS.Models.Questionnaire item)
        {
            var model = db1.Questionnaire;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(DXWebMRCS.Models.Questionnaire item)
        {
            var model = db1.Questionnaire;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.QID == item.QID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 QID)
        {
            var model = db1.Questionnaire;
            if (QID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.QID == QID);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model.ToList());
        }
    }
}
