using DXWebMRCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Security;
using DevExpress.Web.Mvc;
using System.Globalization;
using WebMatrix.WebData;

namespace DXWebMRCS.Constants
{
    public class Config
    {
        public static string Lang = "en";

        public static string GetUserName(string email)
        {
            UsersContext db = new UsersContext();
            
            var user = db.Database.SqlQuery<UserProfile>("SELECT TOP 1 * FROM UserProfile WHERE UserName = '" + email + "'").FirstOrDefault();
            
            return user.Name;
        }

        public static string GetUserAvatar(string email)
        {
            UsersContext db = new UsersContext();

            var user = db.Database.SqlQuery<UserProfile>("SELECT TOP 1 * FROM UserProfile WHERE UserName = '" + email + "'").FirstOrDefault();

            return user.AvatarPath;
        }

        public static UserProfile GetUser(string email)
        {
            UsersContext db = new UsersContext();

            var user = db.Database.SqlQuery<UserProfile>("SELECT TOP 1 * FROM UserProfile WHERE UserName = '" + email + "'").FirstOrDefault();

            return user;
        }
    }

}