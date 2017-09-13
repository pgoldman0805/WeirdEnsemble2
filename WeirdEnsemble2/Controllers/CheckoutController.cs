using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
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
            // check if current customer is logged in
            // if not, redirect them to 

            Customer currentCustomer = db.Customers.FirstOrDefault(x => x.AspNetUser.UserName == User.Identity.Name);
            if (currentCustomer == null)
            {
                currentCustomer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateCreated = DateTime.UtcNow
                };
                db.Customers.Add(currentCustomer);
                db.SaveChanges();
            }

            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                model.CurrentCart = db.Carts.Single(x => x.Name == cartName);

            }
            if (ModelState.IsValid)
            {
                string merchantId = ConfigurationManager.AppSettings["Braintree.MerchantID"];
                string publicKey = ConfigurationManager.AppSettings["Braintree.PublicKey"];
                string privateKey = ConfigurationManager.AppSettings["Braintree.PrivateKey"];
                string environment = ConfigurationManager.AppSettings["Braintree.Environment"];

                Braintree.BraintreeGateway braintreeGateway = new Braintree.BraintreeGateway
                    (environment, merchantId, publicKey, privateKey);

                Braintree.TransactionRequest request = new Braintree.TransactionRequest();
                request.Amount = model.CurrentCart.CartItems.Sum(x => x.Product.ListPrice * x.Quantity) ?? .01m;
                request.CreditCard = new Braintree.TransactionCreditCardRequest
                {
                    CardholderName = model.CreditCardHolder,
                    CVV = model.CreditCardVerificationValue,
                    Number = model.CreditCardNumber,
                    ExpirationMonth = model.CreditCardExpirationMonth.ToString().PadLeft(2,'0'),
                    ExpirationYear = model.CreditCardExpirationYear.ToString()
                };

                Braintree.Result<Braintree.Transaction> result = braintreeGateway.Transaction.Sale(request);


                if (result.Errors == null || result.Errors.Count == 0)
                {
                    string transactionId = result.Target.Id;

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
                        ShippingAddressLine1 = model.ShippingAddressLine1,
                        TransactionID = transactionId

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

                    return RedirectToAction("Index", "Receipt", new { id = order.Id });
                }
                else
                {
                    ModelState.AddModelError("CreditCardNumber", "Unable to authorize this card number");
                }
            }
            
            return View(model);
        }
    }
}