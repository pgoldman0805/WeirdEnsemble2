﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Threading.Tasks;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class AccountController : Controller
    {

        Entities db = new Entities();
        private UserManager<IdentityUser> _userManager;
        private IAuthenticationManager _authenticationManager;
        protected UserManager<IdentityUser> UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
                }
                return _userManager;
            }
        }
        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authenticationManager == null)
                {
                    _authenticationManager = HttpContext.GetOwinContext().Authentication;
                }
                return _authenticationManager;
            }
        }
        //Type override and hit tab
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                if (UserManager != null)
                {
                    UserManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        // GET: Account
        [Authorize]
        public ActionResult Index()
        {
            string merchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
            string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
            string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
            string environment = ConfigurationManager.AppSettings["Braintree.Environment"];
            Braintree.BraintreeGateway braintreeGateway = new Braintree.BraintreeGateway
                (environment, merchantId, publicKey, privateKey);
            Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
            search.Email.Equals(User.Identity.Name);
            var customers = braintreeGateway.Customer.Search(search);
            if (customers.Ids != null && customers.Ids.Count() > 0)
            {
                var customer = customers.FirstItem;
                ViewBag.Addresses = customer.Addresses;
                ViewBag.CreditCards = customer.CreditCards;
            }
            return View(db.Customers.FirstOrDefault(x => x.AspNetUser.UserName == User.Identity.Name));
        }

        [Authorize]
        public ActionResult DeleteAddress(string id)
        {
            string merchantId = System.Configuration.ConfigurationManager.AppSettings["Braintree.MerchantID"];
            string environment = ConfigurationManager.AppSettings["Braintree.Environment"];
            string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
            string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
            Braintree.BraintreeGateway braintreeGateway = new Braintree.BraintreeGateway(environment, merchantId, publicKey, privateKey);
            Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
            search.Email.Equals(User.Identity.Name);
            var customers = braintreeGateway.Customer.Search(search);
            if (customers.Ids != null)
            {
                var customer = customers.FirstItem;
                braintreeGateway.Address.Delete(customer.Id, id);
                TempData["Message"] = "Address Deleted";
            }
            return RedirectToAction("Index");
        }


        public ActionResult CreateAddress()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateAddress(string fname, string lname, string region, string locality, string postalCode, string streetAddress)
        {
            
            if (ModelState.IsValid)
            {
                string merchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
                string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
                string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
                string environment = ConfigurationManager.AppSettings["Braintree.Environment"];
                Braintree.BraintreeGateway braintreeGateway = new Braintree.BraintreeGateway
                    (environment, merchantId, publicKey, privateKey);
                Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
                search.Email.Equals(User.Identity.Name);
                var customers = await braintreeGateway.Customer.SearchAsync(search);
                if (customers != null)
                {
                    var customer = customers.FirstItem;
                    await braintreeGateway.Address.CreateAsync(customer.Id, new Braintree.AddressRequest {
                        FirstName = fname,
                        LastName = lname,
                        StreetAddress = streetAddress,
                        Locality = locality,
                        Region = region,
                        
                    });
                }
                TempData["Message"] = "Address created";
                return RedirectToAction("Index");
            }
            return View();
        }
        // GET: Account/SignIn
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        // POST: Account/SignIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(string username, string password, bool? rememberMe, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByEmailAsync(username);
                if (user != null)
                {
                    if (await UserManager.CheckPasswordAsync(user,password))
                    {
                        var userIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties()
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string username, string title, string fname, string mname, string lname, DateTime dateOfBirth, string phone, string password)
        {
            IdentityUser newUser = new IdentityUser(username)
            {
                // set the Email to be the username
                Email = username
            };

            IdentityResult result = await UserManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                ViewBag.Errors = result.Errors;
                return View();
            }
            var newCustomer = new Customer
            {
                AspNetUserID = newUser.Id,
                Title = title,
                FirstName = fname,
                MiddleName = mname,
                LastName = lname,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phone
            };
            db.Customers.Add(newCustomer);
            await db.SaveChangesAsync();

            string merchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
            string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
            string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
            string environment = ConfigurationManager.AppSettings["Braintree.Environment"];
            Braintree.BraintreeGateway braintreeGateway = new Braintree.BraintreeGateway
                (environment, merchantId, publicKey, privateKey);

            Braintree.CustomerSearchRequest search = new Braintree.CustomerSearchRequest();
            search.Email.Equals(username);

            var existingCustomers = await braintreeGateway.Customer.SearchAsync(search);
            if (existingCustomers != null || !existingCustomers.Ids.Any())
            {
                Braintree.CustomerRequest c = new Braintree.CustomerRequest
                {
                    FirstName = fname,
                    LastName = lname,
                    CustomerId = newCustomer.Id.ToString(),
                    Email = username,
                    Phone = phone
                };
                var creationResult = await braintreeGateway.Customer.CreateAsync(c);
            }
            
            string token = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
            string body = string.Format(
                "<a href=\"{0}/account/confirmaccount?email={1}&token={2}\">Confirm Your Account</a>",
                Request.Url.GetLeftPart(UriPartial.Authority),
                username,
                token);

            await UserManager.SendEmailAsync(newUser.Id, "Confirm Your WeirdEnsemble Account", body);
            TempData["ConfirmEmail"] = "Account created. Please check your email inbox to confirm your account!";
            return RedirectToAction("SignIn");

        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = UserManager.FindByEmail(email);

                // : if unrecognized email is entered, display error message and return the same view
                if (user == null)
                {
                    ViewBag.Message = email + " isn't a registered emailed address.";
                    return View();
                }
                string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                
                string body = string.Format(
                    "<a href=\"{0}/account/resetpassword?email={1}&token={2}\">Reset your password</a>",
                    Request.Url.GetLeftPart(UriPartial.Authority),
                    email,
                    resetToken);
                await UserManager.SendEmailAsync(user.Id, "Reset Your WeirdEnsemble.com Password", body);

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
        public async Task<ActionResult> ResetPassword(string email, string token, string newPassword)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByEmailAsync(email);
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, token, newPassword);
                if (result.Succeeded)
                {
                    TempData["PasswordReset"] = "Your password has been reset successfully";
                    return RedirectToAction("SignIn");
                }
                ViewBag.Errors = result.Errors;
            }
            return View();
        }

        public async Task<ActionResult> ConfirmAccount(string email, string token)
        {
            IdentityUser user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                IdentityResult confirmResult = await UserManager.ConfirmEmailAsync(user.Id, token);
                if (confirmResult.Succeeded)
                {
                    TempData["AccountMessage"] = "Your email address has been confirmed";
                }
            }
            return RedirectToAction("SignIn");
        }
    }
}