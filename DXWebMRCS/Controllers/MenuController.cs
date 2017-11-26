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
        public ActionResult Index()
        {
            return View();
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

        public ActionResult TreeListMovePartial(int ID, int? PARENTID)
        {
            NorthwindDataProvider.MoveMenu(ID, Convert.ToInt32(PARENTID));
            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }

        public ActionResult TreeListDeletePartial(int ID)
        {
            try
            {
                NorthwindDataProvider.DeleteMenu(ID);
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("TreeListPartial", NorthwindDataProvider.GetMenus());
        }
    }
}

