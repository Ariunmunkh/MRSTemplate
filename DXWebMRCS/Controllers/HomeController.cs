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
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView
            var menuList = db.Menus.ToList();
            return View(menuList);    
        }
        
        public ActionResult GridViewPartialView() 
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            return PartialView();
        }

        [HttpGet]
        public ActionResult _HeaderPartial()
        {
            // DXCOMMENT: Pass a data model for GridView in the PartialView method's second parameter
            var menuList = db.Menus.ToList();
            return View(menuList);
        }

        #region Menu code
        public ActionResult TreeListPartial()
        {
            return PartialView(db.Menus.ToList());
        }

        public ActionResult TreeListAddNewPartial(Menu Menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Menus.Add(Menu);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", db.Menus.ToList());
        }

        public ActionResult TreeListUpdatePartial(Menu Menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //NorthwindDataProvider.UpdateMenu(Menu);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", db.Menus.ToList());
        }

        public ActionResult TreeListMovePartial(int ID, int? PARENTID)
        {
            //NorthwindDataProvider.MoveMenu(ID, Convert.ToInt32(PARENTID));
            return PartialView("TreeListPartial", db.Menus.ToList());
        }

        public ActionResult TreeListDeletePartial(int ID)
        {
            try
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Menus WHERE MenuID = " + ID);
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("TreeListPartial", db.Menus.ToList());
        }
        #endregion
    }
}