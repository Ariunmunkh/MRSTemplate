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
using System.IO;
using ImageResizer;
using PagedList;

namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin,BranchUser")]
    public class TrainingController : Controller
    {

        private UsersContext db = new UsersContext();

        // GET: /Training/
        public ActionResult Index()
        {
            return View(db.Trainings.ToList());
        }

        public ActionResult MenuClick(int id)
        {
            var pageNumber = 1;
            var pageSize = 4;
            var Training = db.Database.SqlQuery<Training>("SELECT * FROM Training ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (Training == null)
            {
                return HttpNotFound();
            }

            if (Training.Count == 1)
            {
                return View("TrainingDetail", Training.First());
            }
            return View("TrainingList", Training);
        }

        public ActionResult PageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var Training = db.Database.SqlQuery<Training>("SELECT * FROM Training").ToPagedList(pageNumber, pageSize);
            if (Training == null)
            {
                return HttpNotFound();
            }

            if (Training.Count() == 1)
            {
                return View("TrainingDetail", Training.First());
            }
            return View("TrainingList", Training);
        }

        // GET: /Training/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Training Training = db.Trainings.Find(id);
            if (Training == null)
            {
                return HttpNotFound();
            }
            return View(Training);
        }

        //GET: /Training/TrainingDetail/5
        public ActionResult TrainingDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Training Training = db.Database.SqlQuery<Training>("SELECT TOP 1 * FROM Training WHERE TrainingID = " + id).FirstOrDefault();
            if (Training == null)
            {
                return HttpNotFound();
            }
            return View(Training);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainingID,NameMon,NameEng,ContentMon,ContentEng,Where,When,Duration,Status")] Training Training)
        {
            if (ModelState.IsValid)
            {
                Training.Status = 0;
                db.Trainings.Add(Training);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Training);
        }

        // GET: /Training/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Training Training = db.Trainings.Find(id);
            if (Training == null)
            {
                return HttpNotFound();
            }
            return View(Training);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainingID,NameMon,NameEng,ContentMon,ContentEng,Where,When,Duration,Status")] Training Training)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Training).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Training);
        }

        // GET: /Training/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Training Training = db.Trainings.Find(id);
            if (Training == null)
            {
                return HttpNotFound();
            }
            return View(Training);
        }

        // POST: /Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Training Training = db.Trainings.Find(id);
            db.Trainings.Remove(Training);
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


        [ValidateInput(false)]
        public ActionResult GridViewMasterPartial()
        {
            var model = db.Trainings;
            return PartialView("GridViewMasterPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TrainingViewPartialAddNew(Training item)
        {
            var model = db.Trainings;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("GridViewMasterPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TrainingViewPartialUpdate(Training item)
        {
            var model = db.Trainings;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.TrainingID == item.TrainingID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("GridViewMasterPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TrainingViewPartialDelete(Int32 TrainingID)
        {
            var model = db.Trainings;
            if (TrainingID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.TrainingID == TrainingID);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridViewMasterPartial", model.ToList());
        }

        public ActionResult GridViewDetailPartial(string TrainingID)
        {
            ViewData["TrainingID"] = TrainingID;
            return PartialView("GridViewDetailPartial", NorthwindDataProvider.GetTrainingRequests(Convert.ToInt32( TrainingID)));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailsAddNewPartial(TrainingRequest TrainingRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.InsertTrainingRequest(TrainingRequest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("GridViewDetailPartial", NorthwindDataProvider.GetTrainingRequests(TrainingRequest.TrainingID));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailsUpdatePartial(TrainingRequest TrainingRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.UpdateTrainingRequest(TrainingRequest);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("GridViewDetailPartial", NorthwindDataProvider.GetTrainingRequests(TrainingRequest.TrainingID));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailsDeletePartial(int TrainingRequestID = -1)
        {
            if (TrainingRequestID >= 0)
            {
                try
                {
                    NorthwindDataProvider.DeleteTrainingRequest(TrainingRequestID);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridViewDetailPartial", NorthwindDataProvider.GetTrainingRequests(TrainingRequestID));
        }
    }
}