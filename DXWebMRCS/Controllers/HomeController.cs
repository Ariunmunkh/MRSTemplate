using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;

namespace DXWebMRCS.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();
        public IEnumerable<Menu> menuList;
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView
            
            var menuList = db.Menus.ToList();
            return View();    
        }
        
        public ActionResult GridViewPartialView() 
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView();
        }

        
        public ActionResult _HeaderPartial()
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            if (menuList != null && menuList.Count() > 0)
            {
                 return PartialView("_HeaderPartial", menuList);
            }
            menuList = db.Menus.ToList();
            return PartialView("_HeaderPartial", menuList);
        }
        
    }
}