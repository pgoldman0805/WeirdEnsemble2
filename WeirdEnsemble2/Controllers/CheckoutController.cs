using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class CheckoutController : Controller
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

        // GET: Checkout
        public ActionResult Index()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                model.CurrentCart = db.Carts.Single(x => x.Name == cartName);

            }

            if (User.Identity.IsAuthenticated)
            {
                Customer currentCustomer = db.Customers.FirstOrDefault(x => x.AspNetUser.UserName == User.Identity.Name);
                if (currentCustomer != null)
                {
                    model.EmailAddress = currentCustomer.AspNetUser.Email;
                    model.PhoneNumber = currentCustomer.PhoneNumber;
                    model.ShippingRecipient = currentCustomer.FirstName + " " + currentCustomer.LastName;
                    
                }
            }
            return View(model);
        }

        // POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel model)
        {
            Customer currentCustomer = db.Customers.FirstOrDefault(x => x.AspNetUser.UserName == User.Identity.Name);

            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                model.CurrentCart = db.Carts.Single(x => x.Name == cartName);

            }
            if (ModelState.IsValid)
            {
                
                var order = new Order
                {
                    DatePlaced = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    CustomerID = currentCustomer.Id,
                    OrderItems = model.CurrentCart.CartItems.Select(x => new OrderItem
                    {
                        DateLastModified = DateTime.UtcNow,
                        Quantity = x.Quantity,
                        ProductId = x.ProductId,
                        PurchasePrice = (decimal)x.Product.ListPrice
                    }).ToArray(),
                    ShippingAddressLine1 = model.ShippingAddressLine1
                    
                };

                db.Orders.Add(order);

                // Remove the cart form the database and convert it to an order
                db.CartItems.RemoveRange(model.CurrentCart.CartItems);
                db.Carts.Remove(model.CurrentCart);

                db.SaveChanges();

                //REmove the basket cookie!
                Response.SetCookie(new HttpCookie("CartName")
                {
                    Expires = DateTime.UtcNow
                });
                
                // TODO: Try to check out!
                return RedirectToAction("Index", "Receipt", new { id = order.Id });
                
            }
            
            return View(model);
        }
    }
}