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
        public ActionResult SliderAdd()
        {
            // DXCOMMENT: Pass a data model for GridView            
            return View();
        }

        [HttpPost]
        public ActionResult SliderAdd(SliderPhoto slider, HttpPostedFileBase ImageFile)
        {
            //if (ModelState.IsValid)
            //{
                if (ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "1920x1280" + DateTime.Now.ToString("yymmssfff") + extension;
                    slider.ImagePath = "/Content/Images/SliderImage/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/SliderImage/"), fileName);
                    ImageFile.SaveAs(fileName);
                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "jpg"
                    };

                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                }
                slider.CreatedDate = DateTime.Now;
                db.SliderPhotos.Add(slider);
                db.SaveChanges();
            //}
            return View(slider);
        }

        [HttpGet]
        public ActionResult SliderList()
        {
            // DXCOMMENT: Pass a data model for GridView      
            var list = db.SliderPhotos.OrderByDescending(x => x.CreatedDate).ToList();
            return View(list);
        }
    }
}