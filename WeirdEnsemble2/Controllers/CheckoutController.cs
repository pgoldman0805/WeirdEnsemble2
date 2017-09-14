using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using WeirdEnsemble2.Models;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Index(CheckoutViewModel model)
        {
            // Try to find an existing customer
            Customer currentCustomer = db.Customers.FirstOrDefault(x => x.AspNetUser.UserName == User.Identity.Name);

            // if this is an anonymous customer, create a new Customer record for them
            if (currentCustomer == null)
            {
                currentCustomer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    DateCreated = DateTime.UtcNow
                };
                db.Customers.Add(currentCustomer);
                await db.SaveChangesAsync();
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

                Braintree.Result<Braintree.Transaction> result = await braintreeGateway.Transaction.SaleAsync(request);


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
                            PurchasePrice = x.Product.ListPrice ?? 0
                        }).ToArray(),
                        ShippingAddressLine1 = model.ShippingAddressLine1,
                        TransactionID = transactionId

                    };

                    // Remove the cart form the database and convert it to an order
                    db.CartItems.RemoveRange(model.CurrentCart.CartItems);
                    db.Carts.Remove(model.CurrentCart);
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();

                    //Remove the basket cookie!
                    Response.SetCookie(new HttpCookie("CartName")
                    {
                        Expires = DateTime.UtcNow
                    });

                    // Send the user an e-mail with their order receipt
                    
                    SendGridEmailService mail = new SendGridEmailService();
                    await mail.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage
                    {
                        Destination = order.Customer.EmailAddress,
                        Subject = "Your WeirdEnsemble Order #" + order.TransactionID + " Receipt",
                        Body = "Your total price for order #" + order.TransactionID + " is:\n"
                                                                  + request.Amount + "!"
                    });


                    return RedirectToAction("Index", "Receipt", new { id = order.TransactionID });
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