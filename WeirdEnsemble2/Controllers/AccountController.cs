
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;

using Microsoft.AspNet.Identity.Owin;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

namespace WeirdEnsemble2.Controllers
{
    public class AccountController : Controller
    {
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
        public ActionResult Register(string username, string password)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
            IdentityUser newUser = new IdentityUser(username);
            //This can fail - password might not be complex enough, or username might already be in use

            IdentityResult result = manager.Create(newUser, password);

            if (!result.Succeeded)
            {
                ViewBag.Errors = result.Errors;
                return View();
            }
            //If the user creation succeeded, use the code below to set the sign-in cookie:
            //TODO: Most sites require you to confirm email before letting you sign-in.
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity = manager.CreateIdentity(newUser, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties(),userIdentity);

            return RedirectToAction("Index", "Home");
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

                SendGrid.SendGridClient client = new SendGrid.SendGridClient(sendGridApiKey);
                

                return RedirectToAction("ForgotPasswordSent");
            }
            return View();
        }

        public ActionResult ForgotPasswordSent()
        {
            return View();
        }
    }
}