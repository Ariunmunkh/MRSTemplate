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
using System.Text;
using DevExpress.Web;

namespace DXWebMRCS.Controllers
{
    [Authorize(Roles = "Admin,BranchUser")]
    [RequireHttps]
    public class NewsController : Controller
    {
        private UsersContext db = new UsersContext();

        // GET: /News/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.News.ToList().OrderByDescending(x => x.Date));
        }

        #region News Role user
        [AllowAnonymous]
        public ActionResult MenuClick(int id)
        {
            var pageNumber = 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News WHERE MenuID = " + id + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }

            ViewBag.menuId = id;

            if (news.Count == 1)
            {
                IEnumerable<News> newslist = db.Database.SqlQuery<News>("SELECT TOP 3 * FROM News WHERE CID <> " + news.First().CID + " ORDER BY Date ASC").ToList();
                return View("NewsDetail", new Tuple<News, IEnumerable<News>>(news.First(), newslist));
                //return View("NewsDetail", news.First());
            }
            return View("MenuNewsList", news);
        }

        [AllowAnonymous]
        public ActionResult MenuPageClick(int? page, int menuId)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News WHERE MenuID = " + menuId + " ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.menuId = menuId;
            return View("MenuNewsList", news);
        }

        [AllowAnonymous]
        public ActionResult PageClick(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View("NewsList", news);
        }

        [AllowAnonymous]
        // GET: /News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [AllowAnonymous]
        //GET: /News/NewsDetail/5
        public ActionResult NewsDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Database.SqlQuery<News>("SELECT TOP 1 * FROM News WHERE CID = " + id).FirstOrDefault();
            if (news == null)
            {
                return HttpNotFound();
            }

            IEnumerable<News> newslist = db.Database.SqlQuery<News>("SELECT TOP 3 * FROM News WHERE CID <> " + news.CID + " ORDER BY Date ASC").ToList();
            var detail = new Tuple<News, IEnumerable<News>>(news, newslist);
            return View(detail);
        }

        [AllowAnonymous]
        public ActionResult NewsList()
        {
            var pageNumber = 1;
            var pageSize = 6;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News ORDER BY Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        [AllowAnonymous]
        public ActionResult TagNewsList(int tagID)
        {
            var pageNumber = 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News n INNER JOIN TagDetails t ON n.CID = t.SourceID WHERE t.Source = 'News' AND t.TagID = " + tagID + " ORDER BY n.Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            var model = new Tuple<IPagedList<News>, int>(news, tagID);
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult TagNewsPageClick(int? page, int tagID)
        {
            var pageNumber = page ?? 1;
            var pageSize = 4;
            var news = db.Database.SqlQuery<News>("SELECT * FROM News n INNER JOIN TagDetails t ON n.CID = t.SourceID WHERE t.Source = 'News' AND t.TagID = " + tagID + " ORDER BY n.Date DESC").ToPagedList(pageNumber, pageSize);
            if (news == null)
            {
                return HttpNotFound();
            }
            var model = new Tuple<IPagedList<News>, int>(news, tagID);
            return View("TagNewsList", model);
        }
        #endregion

        #region News Create, Edit, Delete
        // GET: /News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ContentType,MenuID,Tags")] News news, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {

                    string subPath = "/Content/Images/NewsImage"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    news.Image = "/Content/Images/NewsImage/" + fileName;
                    news.ImageMedium = "/Content/Images/NewsImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileNameMedium);
                    ImageFile.SaveAs(fileName);
                    ImageFile.SaveAs(fileNameMedium);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                    ResizeSettings resizeSetting2 = new ResizeSettings
                    {
                        Width = 360,
                        Height = 240,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileNameMedium, fileNameMedium, resizeSetting2);
                }
                if (news.MenuID.HasValue)
                {
                    news.BranchID = NorthwindDataProvider.GetBranchID((int)news.MenuID);
                    if (news.BranchID == -1)
                        news.BranchID = null;
                }
                news.Date = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                SendNotificationMessage(news.TitleMon);
                NorthwindDataProvider.InsertTagDetail(news.CID, news.Tags, "News");
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: /News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: /News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,TitleMon,TitleEng,BodyMon,BodyEng,Image,ContentType,MenuID,Tags")] News news, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {

                    string subPath = "/Content/Images/NewsImage"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string fileNameMedium = fileName + "360x240" + DateTime.Now.ToString("yymmssfff") + extension;
                    news.Image = "/Content/Images/NewsImage/" + fileName;
                    news.ImageMedium = "/Content/Images/NewsImage/" + fileNameMedium;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileName);
                    fileNameMedium = Path.Combine(Server.MapPath("~/Content/Images/NewsImage/"), fileNameMedium);
                    ImageFile.SaveAs(fileName);
                    ImageFile.SaveAs(fileNameMedium);

                    ResizeSettings resizeSetting = new ResizeSettings
                    {
                        Width = 1920,
                        Height = 1280,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileName, fileName, resizeSetting);
                    ResizeSettings resizeSetting2 = new ResizeSettings
                    {
                        Width = 360,
                        Height = 240,
                        Format = "png"
                    };
                    ImageBuilder.Current.Build(fileNameMedium, fileNameMedium, resizeSetting2);
                }

                news.Date = DateTime.Now;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                NorthwindDataProvider.InsertTagDetail(news.CID, news.Tags, "News");
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: /News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: /News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            NorthwindDataProvider.DeleteTagDetail(id, "News");
            return RedirectToAction("Index");
        }
        #endregion

        #region tag
        [AllowAnonymous]
        public ActionResult TagPartial()
        {
            var taglist = db.Database.SqlQuery<Tag>("SELECT * FROM Tags").ToList();
            return PartialView("_TagPartial", taglist);
        }
        public ActionResult Tag()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult TagPartialView()
        {
            return PartialView("TagPartialView", NorthwindDataProvider.GetTag());
        }

        public ActionResult DropDownEdit()
        {
            return View("DropDownEdit");
        }

        #region Tag Create, Edit, Delete
        // GET: /Tag/Create
        public ActionResult TagCreate()
        {
            return View();
        }

        // POST: /Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TagCreate([Bind(Include = "TagID,NameMon,NameEng")] Tag Tag)
        {
            if (ModelState.IsValid)
            {
              
                db.Tag.Add(Tag);
                db.SaveChanges();
                return RedirectToAction("Tag");
            }

            return View(Tag);
        }

        // GET: /Tag/Edit/5
        public ActionResult TagEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag Tag = db.Tag.Find(id);
            if (Tag == null)
            {
                return HttpNotFound();
            }
            return View(Tag);
        }

        // POST: /Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TagEdit([Bind(Include = "TagID,NameMon,NameEng")] Tag Tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tag");
            }
            return View(Tag);
        }

        // GET: /Tag/Delete/5
        public ActionResult TagDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag Tag = db.Tag.Find(id);
            if (Tag == null)
            {
                return HttpNotFound();
            }
            return View(Tag);
        }

        // POST: /Tag/Delete/5
        [HttpPost, ActionName("TagDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult TagDeleteConfirmed(int id)
        {
            Tag Tag = db.Tag.Find(id);
            db.Tag.Remove(Tag);
            db.SaveChanges();
            return RedirectToAction("Tag");
        }
        #endregion
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult EditBodyMonPartial()
        {
            return PartialView("EditBodyMonPartial");
        }
        public ActionResult BodyMonPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyMon", NewsControllerBodyMonSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyMonPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyMon", NewsControllerBodyMonSettings.ImageUploadValidationSettings, NewsControllerBodyMonSettings.ImageUploadDirectory);
            return null;
        }

        public ActionResult ImageUpload()
        {
            UploadControlExtension.GetUploadedFiles("Image", NewsControllerImageSettings.UploadValidationSettings, NewsControllerImageSettings.FileUploadComplete);
            return null;
        }

        public ActionResult EditBodyEngPartial()
        {
            return PartialView("EditBodyEngPartial");
        }
        public ActionResult BodyEngPartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("BodyEng", NewsControllerBodyEngSettings1.ImageSelectorSettings);
            return null;
        }
        public ActionResult BodyEngPartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("BodyEng", NewsControllerBodyEngSettings1.ImageUploadValidationSettings, NewsControllerBodyEngSettings1.ImageUploadDirectory);
            return null;
        }



        DXWebMRCS.Models.UsersContext db1 = new DXWebMRCS.Models.UsersContext();

        [ValidateInput(false)]
        public ActionResult NewsViewPartial()
        {
            var model = db1.News.OrderByDescending(x=>x.Date);
            return PartialView("_NewsViewPartial", NorthwindDataProvider.GetNews(WebMatrix.WebData.WebSecurity.CurrentUserId));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialAddNew(DXWebMRCS.Models.News item)
        {
            var model = db1.News;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_NewsViewPartial", NorthwindDataProvider.GetNews(WebMatrix.WebData.WebSecurity.CurrentUserId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialUpdate(DXWebMRCS.Models.News item)
        {
            var model = db1.News;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.CID == item.CID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_NewsViewPartial", NorthwindDataProvider.GetNews(WebMatrix.WebData.WebSecurity.CurrentUserId));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NewsViewPartialDelete(System.Int32 CID)
        {
            var model = db1.News;
            if (CID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.CID == CID);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_NewsViewPartial", NorthwindDataProvider.GetNews(WebMatrix.WebData.WebSecurity.CurrentUserId));
        }


        static void SendNotificationMessage(string title)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic MTM0MDA4MTYtOGQyOS00NzU1LTliNjktNzQ1YWY3ZDQzNThj");

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"571b8baa-797b-41e3-ac8d-f4c1bb196d29\","
                                                    + "\"headings\": {\"en\": \"" + "Red Cross" + "\"},"
                                                    + "\"contents\": {\"en\": \"'" + title + "'\"},"
                                                    + "\"included_segments\": [\"All\"]}");

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
            //return string.Empty;
        }
    }
    public class NewsControllerBodyEngSettings1
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "News", Action = "BodyEngPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }


    }

    public class NewsControllerImageSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
            MaxFileSize = 4000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                // Save uploaded file to some location
            }
        }
    }

    public class NewsControllerBodyMonSettings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "News", Action = "BodyMonPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}
