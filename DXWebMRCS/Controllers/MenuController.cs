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
    public class MenuController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        public ActionResult TreeListPartial()
        {
            return PartialView(NorthwindDataProvider.GetMenus());
        }

        public ActionResult TreeListAddNewPartial(Menu Menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.InsertMenu(Menu);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }

        public ActionResult TreeListUpdatePartial(Menu Menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NorthwindDataProvider.UpdateMenu(Menu);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }

        public ActionResult TreeListMovePartial(Menu Menu)
        {
            NorthwindDataProvider.MoveMenu(Menu.MenuID, Menu.ParentId);
            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }

        public ActionResult TreeListDeletePartial(Menu Menu)
        {
            try
            {
                NorthwindDataProvider.DeleteMenu(Menu.MenuID);
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }
    }
}

