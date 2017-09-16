using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeirdEnsemble2.Models;
using System.Threading.Tasks;

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

        // GET: Product/Detail/{id}
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
            //ProductImage image = db.Products.Find(id).ProductImages.FirstOrDefault(m => m.ProductID == id);

            return View("Detail", db.Products.Find(id));
        }

        //POST: Product/Details/{id}
        // When someone adds an item to their cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(int id,int quantity)
        {
            if (ModelState.IsValid)
            {
                string cartName = "";
                Cart cart = null;
                

                // CHECK TO SEE IF USER IS LOGGED IN
                // IF THEY ARE, CHECK IF THEY ALREADY HAVE A CART
                // OTHERWISE, CREATE AND ASSIGN A CART TO THAT USER

                if (User.Identity.IsAuthenticated)
                {
                    var customerId = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name).Customers.First().Id;
                    cart = db.Carts.FirstOrDefault(x => x.CustomerId == customerId);
                    if (cart == null)
                    {
                        // If a user doesn't have a cart yet, lets generate a GUID 
                        // to identify the cart
                        cartName = Guid.NewGuid().ToString();
                        // create a new cart item and assign it's name
                        cart = new Cart()
                        {
                            Name = cartName,
                            DateCreated = DateTime.UtcNow,
                            CustomerId = customerId,
                        };
                        db.Carts.Add(cart);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    // Check to see if there is an existing cookie for the cart
                    // The cookie name is arbitrary!
                    if (Request.Cookies.AllKeys.Contains("CartName"))
                    {
                        //Cart name is going to be a GUID
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
                            DateCreated = DateTime.UtcNow,
                            DateLastModified = DateTime.UtcNow
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
                }

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

                // Finally, save changes to the DB 
                await db.SaveChangesAsync();
                TempData["Message"] = string.Format("{0} item{1} added to your cart!", quantity, (quantity > 1 ? "s" : ""));
                
                return RedirectToAction("Index", "Product");

            }
            return View(db.Products.Find(id));
        }

        //GET: Product/Review/{id}
        public ActionResult Review(int? id)
        {
            
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (!db.Products.Any(m => m.Id == id))
            {
                return HttpNotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                TempData["UnauthorizedReview"] = "You must be signed in to leave a review.";
                return RedirectToAction("Detail", db.Products.Find(id));
            }
            return View(db.Products.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Review(int prodId, int rating, string comment)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var customerID = db.AspNetUsers.Single(x => x.UserName == User.Identity.Name).Customers.First().Id;
                    ProductReview newReview = new ProductReview()
                    {
                        ProductID = prodId,
                        CustomerID = customerID,
                        Rating = rating,
                        Comments = comment,
                        DateCreated = DateTime.UtcNow
                    };

                    db.ProductReviews.Add(newReview);
                    await db.SaveChangesAsync();
                    string productName = db.Products.FirstOrDefault(x => x.Id == prodId).Name;
                    TempData["AddedReview"] = "Thanks for reviewing " + productName + "!";
                    return RedirectToAction("Detail", db.Products.Find(prodId));
                }
                
            }
            return View(db.Products.Find(prodId));
            
        }
    }
}