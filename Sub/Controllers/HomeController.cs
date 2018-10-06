using Sub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Sub.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index(BranchPost post)
        {  
            try
            {
                var branchId = System.Configuration.ConfigurationManager.AppSettings["SubId"];

                var pageNumber = 1;
                var pageSize = 4;
                
                var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
                branch.menuID = post.menuID;

                if (post.menuID > 0)
                {
                    var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " AND MenuID =" + post.menuID + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                    branch.newsList = newsList;
                    if (newsList.Count == 1)
                    {
                        branch.news = newsList.First();
                        return View("BranchListDetail", branch);
                    }
                    return View(branch);
                }
                else
                {
                    var newsList = db.Database.SqlQuery<News>("SELECT * FROM News WHERE BranchID = " + branchId + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
                    branch.newsList = newsList;
                    if (newsList.Count == 1)
                    {
                        branch.news = newsList.First();
                        return View("Detail", branch);
                    }
                    return View(branch);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View(new BranchViewModel());
            }                   
        }

        public ActionResult Detail(int newsId, int branchId)
        {
            var news = db.Database.SqlQuery<News>("SELECT TOP 1 * FROM News WHERE CID = " + newsId).FirstOrDefault();
            var branch = db.Database.SqlQuery<BranchViewModel>("SELECT TOP(1) BranchID, NameMon, NameEng, Logo, Image, email, phone, address FROM Branches WHERE BranchID = " + branchId).FirstOrDefault();
            branch.news = news;
            return View("Detail", branch);
        }

        public ActionResult MenuHeaderPartial(int branchId)
        {
            var menuList = db.Database.SqlQuery<Menu>("SELECT * FROM Menu WHERE BranchID = " + branchId).ToList();
            return PartialView("_MenuHeaderPartial", menuList);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}