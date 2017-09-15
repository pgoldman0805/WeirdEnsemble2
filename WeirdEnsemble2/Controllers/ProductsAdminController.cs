using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WeirdEnsemble2.Models;


namespace WeirdEnsemble2.Controllers
{
    // DECLARATIVE CHECKING
    [Authorize(Roles = "Admin")]
    public class ProductsAdminController : Controller
    {
        private Entities db = new Entities();

        // GET: ProductsAdmin
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var claimsIdentity = (System.Security.Claims.ClaimsPrincipal)User.Identity;
            //    var roleClaims = claimsIdentity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role);
            //}

            // IMPERATIVE CHECKING
            //if (User.Identity.IsAuthenticated)
            //{
                //var products = db.Products.Include(p => p.ProductInventory);
                return View(db.Products.ToList());
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            
            
        }

        // GET: ProductsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.ProductInventories, "ProductId", "ProductId");
            return View();
        }

        // POST: ProductsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product, string path, string altText)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    ProductImage newImage = new ProductImage()
                    {
                        ProductID = product.Id,
                        ImagePath = path,
                        AlternateText = altText ?? "",
                        DateCreated = DateTime.UtcNow
                    };

                    db.ProductImages.Add(newImage);
                }
                product.DateCreated = DateTime.UtcNow;
                product.DateLastModified = DateTime.UtcNow;
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.ProductInventories, "ProductId", "ProductId", product.Id);
            return View(product);
        }

        // GET: ProductsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.ProductInventories, "ProductId", "ProductId", product.Id);
            return View(product);
        }

        // POST: ProductsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, string path, string altText)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    ProductImage img = db.ProductImages.Single(i => i.ProductID == product.Id);
                    img.ImagePath = path;
                    img.AlternateText = altText ?? "";
                    img.DateLastModified = DateTime.UtcNow;
                }
                
                db.Entry(product).State = EntityState.Modified;

                product.DateLastModified = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.ProductInventories, "ProductId", "ProductId", product.Id);
            return View(product);
        }

        // GET: ProductsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
