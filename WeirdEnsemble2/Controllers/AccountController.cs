
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;

using Microsoft.AspNet.Identity.Owin;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class AccountController : Controller
    {

        Entities db = new Entities();

        //Type override and hit tab
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/SignIn
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        // POST: Account/SignIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string username, string password, bool? rememberMe, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
                IdentityUser user = userManager.FindByName(username);
                if (user != null)
                {
                    if (userManager.CheckPassword(user,password))
                    {
                        var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        var authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties()
                        {
                            IsPersistent = rememberMe ?? false,
                            ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddDays(7))
                        },
                        userIdentity);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Errors = new string[] { "Could not sign in with this username / password" };
                    }

                }
            }
            return View();
        }

        public ActionResult SignOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string title, string fname, string mname, string lname, DateTime dateOfBirth, string phone, string password)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
            IdentityUser newUser = new IdentityUser(username)
            {
                // set the Email to be the username
                Email = username
            };

            IdentityResult result = manager.Create(newUser, password);

            if (!result.Succeeded)
            {
                ViewBag.Errors = result.Errors;
                return View();
            }

            db.Customers.Add(new Customer
            {
                AspNetUserID = newUser.Id,
                Title = title,
                FirstName = fname,
                MiddleName = mname,
                LastName = lname,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phone
            });

            db.SaveChanges();
            //If the user creation succeeded, use the code below to set the sign-in cookie:
            //TODO: Most sites require you to confirm email before letting you sign-in.

            string token = manager.GenerateEmailConfirmationToken(newUser.Id);
            string sendGridApiKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];
            // instantiate SendGrid client using the API Key
            SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridApiKey);

            // create a message to be sent
            SendGrid.Helpers.Mail.SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();

            // set message fields
            message.AddTo(username);
            message.Subject = "Confirm your WeirdEnsemble.com Account";
            message.SetFrom("no-reply@codingtemple.com");
            string body = string.Format(
                "<a href=\"{0}/account/confirmaccount?email={1}&token={2}\">Confirm your account</a>",
                Request.Url.GetLeftPart(UriPartial.Authority),
                username,
                token);
            message.AddContent("text/html", body);
            message.SetTemplateId("9f449d8f-c608-4336-9cbf-8a73ca391f3e");

            var response = client.SendEmailAsync(message).Result;

            TempData["ConfirmEmail"] = "Account created. Please check your email inbox to confirm your account!";

            return RedirectToAction("SignIn");

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
                IdentityUser user = userManager.FindByEmail(email);

                string resetToken = userManager.GeneratePasswordResetToken(user.Id);
                string sendGridApiKey = System.Configuration.ConfigurationManager.AppSettings["SendGrid.ApiKey"];

                // instantiate SendGrid client using the API Key
                SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridApiKey);

                // create a message to be sent
                SendGrid.Helpers.Mail.SendGridMessage message = new SendGrid.Helpers.Mail.SendGridMessage();

                // set message fields
                message.AddTo(email);
                message.Subject = "Reset your WeirdEnsemble.com Password";
                message.SetFrom("no-reply@codingtemple.com");
                string body = string.Format(
                    "<a href=\"{0}/account/resetpassword?email={1}&token={2}\">Reset your password</a>",
                    Request.Url.GetLeftPart(UriPartial.Authority),
                    email,
                    resetToken);
                message.AddContent("text/html", body);
                message.SetTemplateId("9f449d8f-c608-4336-9cbf-8a73ca391f3e");

                var response = client.SendEmailAsync(message).Result;

                return RedirectToAction("ForgotPasswordSent");
            }
            return View();
        }

        public ActionResult ForgotPasswordSent()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email, string token, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var manager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
                IdentityUser user = manager.FindByEmail(email);
                IdentityResult result = manager.ResetPassword(user.Id, token, newPassword);
                if (result.Succeeded)
                {
                    TempData["PasswordReset"] = "Your password has been reset successfully";
                    return RedirectToAction("SignIn");
                }
                ViewBag.Errors = result.Errors;
            }
            return View();
        }

        public ActionResult ConfirmAccount()
        {
            return View();
        }
    }
}