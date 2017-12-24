using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DXWebMRCS.Models;
using System.Threading;
using System.Globalization;
using ImageResizer;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;

namespace DXWebMRCS.Controllers
{
    public class HomeController : Controller
    {
        private UsersContext db = new UsersContext();
        public IEnumerable<Menu> menuList;
        public ActionResult Index()
        {
            // DXCOMMENT: Pass a data model for GridView            
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

        public ActionResult Change(String languageAbbrevation)
        {
            if (languageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbrevation);
            }
            
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = languageAbbrevation;
            Response.Cookies.Add(cookie);

            return View("Index");
        }

        [HttpGet]
        public ActionResult SliderViewPartial()
        {
            var list = db.SliderPhotos.ToList();
            return PartialView("_SliderViewPartial", list);
        }

        
    }
}