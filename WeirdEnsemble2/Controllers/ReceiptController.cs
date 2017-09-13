using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class ReceiptController : Controller
    {
        Entities db = new Entities();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Receipt
        public ActionResult Index(string id)
        {
            return View(db.Orders.Single(x => x.TransactionID == id));
        }
    }
}