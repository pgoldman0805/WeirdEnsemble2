using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using WeirdEnsemble2.Models;
using System.Web;
namespace WeirdEnsemble2.Controllers
{
    public class CartController : Controller
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
        // GET: Cart
        public ActionResult Index()
        {
            Cart cart = null;
            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                cart = db.Carts.Single(x => x.Name == cartName);
            }
            else
            {
                // create an empty cart
                cart = new Cart()
                {
                    CartItems = new CartItem[0]
                };
            }
            return View(cart);
        }
    }
}