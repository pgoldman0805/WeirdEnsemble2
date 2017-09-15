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
                    model.CurrentCart = currentCustomer.Carts.First();
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
            // if there are errors on the form, refresh the page with the previous model
            // along with errors
            if (ModelState.IsValid)
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

                    if (Request.Cookies.AllKeys.Contains("CartName"))
                    {
                        string cartName = Request.Cookies["CartName"].Value;
                        model.CurrentCart = db.Carts.Single(x => x.Name == cartName);

                    }
                }
                else
                {
                    model.CurrentCart = currentCustomer.Carts.First();
                }

                
            
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
                    string body = "<h2>Receipt For WeirdEnsemble.com Order #" + order.TransactionID + "</h2><br/><br/>";
                    body += "<table><thead><tr><th>Item</th><th>List Price</th><th>Quantity</th><th>Total</th></tr></thead>";
                    body += "<tbody>";
                    foreach (var item in order.OrderItems)
                    {
                        body += "<tr>";
                        body += "<td>" + item.Product.Name + "</td>";
                        body += "<td>" + (item.Product.ListPrice ?? 0).ToString("C") + "</td>";
                        body += "<td>" + item.Quantity + "</td>";
                        body += "<td>" + (item.Quantity * (item.Product.ListPrice ?? 0)).ToString("C") + "</td>";
                        body += "</tr>";
                    }
                    body += "</tbody><tfoot><tr><td colspan=\"2\">";
                    body += "<td><strong>Total:</strong></td>";
                    body += "<td><strong>" + (order.OrderItems.Sum(x => x.Quantity * x.Product.ListPrice) ?? 0).ToString("C") + "</strong></td>";
                    body += "</tr></tfoot></table>";

                    SendGridEmailService mail = new SendGridEmailService();
                    await mail.SendAsync(new Microsoft.AspNet.Identity.IdentityMessage
                    {

                        Destination = order.Customer.EmailAddress,
                        Subject = "Your WeirdEnsemble Order #" + order.TransactionID + " Receipt",
                        Body = body
                    });


                    return RedirectToAction("Index", "Receipt", new { id = order.TransactionID });
                }
                else
                {
                    ModelState.AddModelError("CreditCardNumber", "Unable to authorize this card number");
                }
            }
            if (Request.Cookies.AllKeys.Contains("CartName"))
            {
                string cartName = Request.Cookies["CartName"].Value;
                model.CurrentCart = db.Carts.Single(x => x.Name == cartName);

            }

            return View(model);
        }
    }
}