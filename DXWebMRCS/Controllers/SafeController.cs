using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    public class SafeController : Controller
    {
        private UsersContext db = new UsersContext();
        // GET: /Safe/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Safe/
        [HttpGet]
        public ActionResult AddSafeUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSafeUser(SafeUser safeUser)
        {
            if (ModelState.IsValid)
            {
                db.SafeUsers.Add(safeUser);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult SafeUserList()
        {
            var list = db.SafeUsers.ToList();
            return View(list);
        }
	}
}