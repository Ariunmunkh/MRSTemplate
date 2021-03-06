﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using DXWebMRCS.Filters;
using DXWebMRCS.Models;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Owin.Security;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI;

namespace DXWebMRCS.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    //[RequireHttps]
    public class AccountController : Controller
    {

        private UsersContext db = new UsersContext();

        //public AccountController()
        //    : this(new UserManager<UserProfile>(new UserStore<UserProfile>(new UsersContext())))
        //{
        //}

        //public AccountController(UserManager<UserProfile> userManager)
        //{
        //    UserManager = userManager;
        //}

        //public UserManager<UserProfile> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Client)]
        public ActionResult Login(string returnUrl)
        {
            if (WebSecurity.CurrentUserId > 0)
            {
                return Redirect("/Home");
            }
            if (!WebSecurity.UserExists("admin@redcross.mn"))
                AddUserAndRoles();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private void AddUserAndRoles()
        {
            if (!Roles.RoleExists("Admin"))
                Roles.CreateRole("Admin");
            WebSecurity.CreateUserAndAccount("admin@redcross.mn", "Pa$$word123", propertyValues: new
            {
                Name = "Admin",
                LastName = "Admin",
                BirthOfDay = DateTime.Now,
                Gender = 1,
                PhoneNumber = 0,
                orderField41 = false,
                orderField42 = false,
                orderField43 = false,
                orderField44 = false,
                orderField45 = false
            });
            Roles.AddUserToRole("admin@redcross.mn", "Admin");
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                //var user = await UserManager.FindAsync(model.Email, model.Password);

                if (WebSecurity.Login(model.Email, model.Password, persistCookie: model.RememberMe))
                {
                    //var role1 = Roles.GetRolesForUser();
                    //var role2 = Roles.IsUserInRole(model.Email, "BranchUser");
                    //var role3 = Roles.GetRolesForUser(model.Email);
                    //var user = db.Database.SqlQuery<UserProfile>("SELECT TOP 1 * FROM UserProfile WHERE UserName = '" + model.Email + "'").FirstOrDefault();
                    //HttpContext.Session["UserProfile"] = user;
                    //HttpContext.Application["UserProfile"] = user;
                    if (Roles.IsUserInRole(model.Email, "BranchUser"))
                    {
                        return Redirect(returnUrl ?? "/News");
                    }
                    if (Roles.IsUserInRole(model.Email, "Admin"))
                    {
                        return Redirect(returnUrl ?? "/SysAdmin");
                    }
                    return Redirect(returnUrl ?? "/");
                }
                ViewBag.ErrorMessage = "The user name or password provided is incorrect";
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return Redirect("/");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(CacheProfile = "CacheMax", VaryByParam = "none", NoStore = true, Location = OutputCacheLocation.Client)]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    if (!model.orderField45)
                    {
                        model.orderField451 = string.Empty;
                    }
                    WebSecurity.CreateUserAndAccount(model.Email, model.Password, propertyValues: new
                    {
                        Name = model.FirstName,
                        LastName = model.LastName,
                        BirthOfDay = model.BirthOfDay,
                        Gender = Convert.ToInt32(model.Gender),
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        CityTown = model.CityTown,
                        StateProvince = model.StateProvince,
                        orderField1 = model.orderField1,
                        orderField2 = model.orderField2,
                        orderField3 = model.orderField3,
                        orderField41 = model.orderField41,
                        orderField42 = model.orderField42,
                        orderField43 = model.orderField43,
                        orderField44 = model.orderField44,
                        orderField45 = model.orderField45,
                        orderField451 = model.orderField451,
                        orderField5 = model.orderField5
                    });
                    var isRole = Roles.RoleExists("User");
                    if (!isRole)
                    {
                        Roles.CreateRole("User");
                    }
                    Roles.AddUserToRole(model.Email, "User");
                    WebSecurity.Login(model.Email, model.Password);
                    return Redirect("/");
                }
                catch (MembershipCreateUserException e)
                {
                    ViewBag.ErrorMessage = ErrorCodeToString(e.StatusCode);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        [AllowAnonymous]
        public ActionResult ChangePassword(string email)
        {
            ChangePasswordModel changePass = new ChangePasswordModel();
            changePass.email = email;
            return View(changePass);
        }

        [AllowAnonymous]
        public ActionResult ChangePasswordSendEmail()
        {
            return View();
        }

        [HttpPost]
        // Post: /Account/ChangePasswordSendEmail
        [AllowAnonymous]
        public ActionResult ChangePasswordSendEmail(ChangePasswordSendEmailModel mail)
        {
            if (ModelState.IsValid)
            {
                var _user = db.Database.SqlQuery<UserProfile>("SELECT TOP 1 * FROM UserProfile WHERE UserName = '" + mail.Email + "'").FirstOrDefault();

                if (_user != null)
                {
                    try
                    {

                        string systemEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SystemEmailAddress"];
                        string systemEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SystemEmailPassword"];
                        string systemEmailHost = System.Configuration.ConfigurationManager.AppSettings["SystemEmailHost"];
                        string systemEmailPort = System.Configuration.ConfigurationManager.AppSettings["SystemEmailPort"];
                        string password = Membership.GeneratePassword(8, 1);


                        SmtpClient client = new SmtpClient(systemEmailHost, Convert.ToInt32(systemEmailPort));
                        client.EnableSsl = true;
                        client.Timeout = 100000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(systemEmailAddress, systemEmailPassword);

                        var body = string.Format("Dear {0} <BR/>" +
                                    "Thank you for registering our site!!!  <br/>"
                                    + "Your password is:{1} <br/>" +
                                    " <br/>From Red cross.<br/>Mongolia {2}</p>",
                                    _user.Name, password, DateTime.Now.Year);

                        MailMessage mailMessage = new MailMessage(systemEmailAddress, mail.Email, "Reset Password Red Cross", body);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                        client.Send(mailMessage);

                        string token = WebSecurity.GeneratePasswordResetToken(mail.Email);
                        WebSecurity.ResetPassword(token, password);

                        return RedirectToAction("ChangePasswordSuccess", new { result = true });
                    }
                    catch (Exception)
                    {
                        ViewBag.ErrorMessage = "Not Send";
                        return View();
                    }
                }
                else
                {

                    return RedirectToAction("ChangePasswordSuccess", new { result = false });
                }
            }

            return View();
        }
        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);//ашиглахгүй болсон
                //bool changePasswordSucceeded;
                //try
                //{
                //    //changePasswordSucceeded = WebSecurity.ChangePassword(model.email, model.OldPassword, model.NewPassword);
                //    var token = WebSecurity.GeneratePasswordResetToken(model.email);
                //    changePasswordSucceeded = WebSecurity.ResetPassword(token, model.NewPassword);
                //}
                //catch (Exception)
                //{
                //    changePasswordSucceeded = false;
                //}
                //if (changePasswordSucceeded)
                //{
                //    return RedirectToAction("ChangePasswordSuccess", new { result = true });
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "The current password is incorrect or the new password is invalid.";
                //}

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            //var user = await UserManager.FindAsync(loginInfo.Login);
            if (loginInfo != null)
            {
                //await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //
        // GET: /Account/ChangePasswordSuccess
        [AllowAnonymous]
        public ActionResult ChangePasswordSuccess(bool result)
        {
            if (result)
            {
                ViewBag.Title = "Change Password";
                ViewBag.Message = "Your password has been changed successfully.";
            }
            else
            {
                ViewBag.Title = "This email does not exist";
                ViewBag.Message = "Not Send";
            }
            return View();
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
        private const string XsrfKey = "XsrfId";
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}