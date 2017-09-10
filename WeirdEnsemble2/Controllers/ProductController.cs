using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class ProductController : Controller
    {
        private Entities db = new Entities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products);
        }

        // GET: Product Detail on specified ID
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (!db.Products.Any(m => m.Id == id))
            {
                return HttpNotFound();
            }

            return View("Detail", db.Products.Find(id));
        }

        //POST: Details 
        // When someone adds an item to their cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(int id,int quantity)
        {
            if (ModelState.IsValid)
            {
                string cartName = "";
                Cart cart = null;
                
                // Check to see if there is an existing cookie for the cart
                // The cookie name is arbitrary!
                if (Request.Cookies.AllKeys.Contains("CartName"))
                {
                    //Basket name is going to be a GUID
                    //e.g. FAD1F8B0-F3F1-40B1-B552-109AD15F1649
                    cartName = Request.Cookies["CartName"].Value;
                    cart = db.Carts.Single(x => x.Name == cartName);
                }
                else
                {
                    // If a user doesn't have a cart yet, lets generate a GUID 
                    // to identify the cart
                    cartName = Guid.NewGuid().ToString();

                    // create a new cart item and assign it's name
                    cart = new Cart()
                    {
                        Name = cartName,
                        DateCreated = DateTime.UtcNow
                    };

                    // This will queue up the cart for insertion into the database
                    // It won't actually get added until I "save" the DB
                    db.Carts.Add(cart);

                    //Finally, I add a cookie to the "response" object
                    //Every subsequent request to this site should include this cookie
                    // until it expires (in one month). This cookie is saved on the end user's computer
                    Response.SetCookie(new System.Web.HttpCookie("CartName", cartName)
                    {
                        Expires = DateTime.UtcNow.AddMonths(1)
                    });           
                }

                //Time-stamp this cart
                cart.DateLastModified = DateTime.UtcNow;

                // Check if I've already added this type of item to my cart
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == id);

                // If I haven't added this item yet, create the new item and add to cart
                if (cartItem == null)
                {
                    cartItem = new CartItem()
                    {
                        ProductId = id,
                        DateCreated = DateTime.UtcNow
                    };
                    // Add item to cart
                    cart.CartItems.Add(cartItem);
                }

                // Add the quantity selected
                cartItem.Quantity += quantity;

                //Updated the timestamp
                cartItem.DateLastModified = DateTime.UtcNow;

                // Finally, save changes to the DB 
                db.SaveChanges();

                TempData["Message"] = string.Format("{0} item{1} added to your cart!", quantity, (quantity > 1 ? "s" : ""));
                
                return RedirectToAction("Index", "Product");

            }
            return View(db.Products.Find(id));
        }
    }
}