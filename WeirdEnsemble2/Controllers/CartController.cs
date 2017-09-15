using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using WeirdEnsemble2.Models;
using System.Web;
using System.Threading.Tasks;
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

            // if user is logged in, try to retrieve their existing cart
            // if they don't have a cart, create a new one and assign that cart to the user's id
            if (User.Identity.IsAuthenticated)
            {
                var customerId = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name).Customers.First().Id;
                cart = db.Carts.FirstOrDefault(x => x.CustomerId == customerId);
                if (cart != null)
                {
                    return View(cart);
                }
                else
                {
                    // create an empty cart
                    cart = new Cart()
                    {
                        CartItems = new CartItem[0],
                        CustomerId = customerId,
                        
                    };

                }
            }

            // If user is NOT logged in
            else
            {
                if (Request.Cookies.AllKeys.Contains("CartName"))
                {
                    string cartName = Request.Cookies["CartName"].Value;
                    cart = db.Carts.Single(x => x.Name == cartName);

                    //if (User.Identity.IsAuthenticated)
                    //{
                    //    cart.CustomerId = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name)
                    //        .Customers.First().Id;
                    //    db.SaveChanges();
                    //}
                }
                else
                {
                    // create an empty cart
                    cart = new Cart()
                    {
                        CartItems = new CartItem[0]
                    };
                }
            }
            
            return View(cart);
        }

        // POST: Index --> When item gets removed from cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(int prodId)
        {
            // access the current user's cart

            Cart cart = null;
            if (User.Identity.IsAuthenticated)
            {
                var customerId = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name).Customers.First().Id;
                cart = db.Carts.FirstOrDefault(x => x.CustomerId == customerId);
            }
            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                cart = db.Carts.Single(x => x.Name == cartName);
            }
            if (User.Identity.IsAuthenticated)
            {
                var customerId = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name).Customers.First().Id;
                cart = db.Carts.Single(x => x.CustomerId == customerId);
            }

            //target the item to deleted
            CartItem itemToDelete = cart.CartItems.Single(x => x.ProductId == prodId);
            string itemName = itemToDelete.Product.Name;
            // delete and save
            cart.CartItems.Remove(itemToDelete);
            await db.SaveChangesAsync();
            TempData["Message"] = string.Format("{0} successfully removed", itemName);


            return RedirectToAction("Index");
        }
    }
}