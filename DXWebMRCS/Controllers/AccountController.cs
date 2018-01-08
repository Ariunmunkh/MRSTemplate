using System;
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

namespace DXWebMRCS.Controllers {
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller {

        private UsersContext db = new UsersContext();

        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl) {
            if(ModelState.IsValid) {
                if(WebSecurity.Login(model.Email, model.Password, persistCookie: model.RememberMe)) {
                    //var role1 = Roles.GetRolesForUser();
                    //var role2 = Roles.IsUserInRole(model.Email, "BranchUser");
                    //var role3 = Roles.GetRolesForUser(model.Email);
                    if (Roles.IsUserInRole(model.Email, "BranchUser"))
                    {
                         return Redirect(returnUrl ?? "/SysAdmin");
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

        public ActionResult LogOff() {
            WebSecurity.Logout();
            return Redirect("/");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register() {
            
            return View();   
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid) {
                // Attempt to register the user
                try {
                    WebSecurity.CreateUserAndAccount(model.Email, model.Password, propertyValues: new
                    {
                        Name = model.UserName,
                        LastName = model.LastName,
                        BirthOfDay = model.BirthOfDay,
                        Gender = Convert.ToInt32(model.Gender),
                        PhoneNumber = model.PhoneNumber,
                        Type = Convert.ToInt32(model.Type)
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
                catch(MembershipCreateUserException e) {
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
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.Timeout = 100000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("gmunkhuu1124@gmail.com", "Uuriin1234");

                    var callbackUrl = "http://localhost:4659/Account/ChangePassword?email=" + mail.Email;

                    var body = string.Format("Dear {0} <BR/>" +
                                "Thank you for registering our site!!! Please click link below in resert your password."
                                + "Please reset your password by clicking <a href=\"{1}\" >{1} here. </a>" +
                                " If you get any difficulties please contact our support. <br/><br/>From Red cross.<br/>Mongolia 2017</p>",
                                _user.UserName, callbackUrl);

                    MailMessage mailMessage = new MailMessage("gmunkhuu1124@gmail.com", mail.Email, "Reset Password Red Cross", body);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                    client.Send(mailMessage);
                    return RedirectToAction("ChangePasswordSuccess");  
                }
                else
                {
                    ViewBag.ErrorMessage = "Not Send";
                    return View();  
                }
            }

            return View();
        }
        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model) {
            if(ModelState.IsValid) {
                bool changePasswordSucceeded;
                try {
                    //changePasswordSucceeded = WebSecurity.ChangePassword(model.email, model.OldPassword, model.NewPassword);
                    var token = WebSecurity.GeneratePasswordResetToken(model.email);
                    changePasswordSucceeded = WebSecurity.ResetPassword(token, model.NewPassword);
                }
                catch(Exception) {
                    changePasswordSucceeded = false;
                }
                if(changePasswordSucceeded) {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else {
                    ViewBag.ErrorMessage = "The current password is incorrect or the new password is invalid.";
                }
                
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
        public ActionResult ChangePasswordSuccess() {
            return View();
        }

 
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch(createStatus) {
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