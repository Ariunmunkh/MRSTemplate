using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using DevExpress.Web.Mvc;
using System.Globalization;
using DXWebMRCS.Models;
using WebMatrix.WebData;

namespace DXWebMRCS.Constants
{
    public class Config
    {
        public static string Lang = "en";

        public static string GetUserName(string email)
        {
            UsersContext db = new UsersContext();
            
            var name = db.Database.SqlQuery<string>("SELECT TOP 1 Name FROM UserProfile WHERE UserName = '" + email + "'").FirstOrDefault();
            return name;
        }
    }

}