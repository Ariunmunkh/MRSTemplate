using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DXWebMRCS.Controllers
{
    //[RequireHttps]
    public class BranchViewController : Controller
    {
        private UsersContext db = new UsersContext();
        
        // GET: /BranchView/
        public ActionResult Index()
        {
            return View();
        }
	}
}