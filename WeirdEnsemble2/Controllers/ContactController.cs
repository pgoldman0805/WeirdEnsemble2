
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
    public class ContactController : Controller
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
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        //POST: Contact
        [HttpPost]
        public ActionResult Index(string fname, string lname, string email, string comment)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact()
                {
                    FirstName = fname,
                    LastName = lname,
                    Email = email,
                    Comment = comment,
                    DateCreated = DateTime.UtcNow
                };
                db.Contacts.Add(contact);
                db.SaveChanges();
                TempData["ContactConfirmation"] = "Thanks for your comments! We will reply soon.";
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}