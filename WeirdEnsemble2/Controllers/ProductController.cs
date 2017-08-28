using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeirdEnsemble2.Models;

namespace WeirdEnsemble2.Controllers
{
    public class ProductController : Controller
    {
        protected static List<Product> products = new List<Product>();

        [NonAction]
        public static void InitializeProducts()
        {
            products.Add(new Product
            {
                ID = 1,
                Brand = "Roland",
                Category = "Accordion",
                Description = "In 2004, after several years of research, a dream of Roland founder Mr. Ikutaro Kakehashi came true: the V-Accordion was born. This instrument was the world’s first fully digital accordion, powered by the groundbreaking new Physical Behavior Modeling technology. Bringing together the playability of a fine acoustic accordion with all the conveniences of a modern digital instrument, the V-Accordion was immediately embraced by players around the world. Today, the V-Accordion lineup has grown to include a wide range of models, from student instruments to full-featured professional accordions.",
                Name = "FR18 VAccordion",
                Price = 149.99m,
                ImagePath = "/Content/images/products/accordion/Roland_FR18_VAccordion.jpg",
                Rating = 5,
                Website_Link = "http://www.rolandus.com/go/v-accordion/"
            });
            products.Add(new Product
            {
                ID = 2,
                Brand = "",
                Category = "Annoying",
                Description = "A Stadium Horn for Every Event! Pump up the volume at your event with this Air Horn. A great way to spice up any party. Measures 29½\" long x 4½\" wide and compacts down to 15¾\" long! Made of plastic .Obnoxious air horn sound! A noisemaker is an ideal party favor give-a-way for your next New Year's party or Mardi Gras event. Plus they also make a great prize for any carnival. Great for parties & sporting events... ",
                Name = "Vuvuzela",
                Price = 28.93m,
                ImagePath = "/Content/images/products/vuvuzela/vuvuzela_1.jpeg",
                Rating = 3
            });
        }

        



        // GET: Product
        public ActionResult Index()
        {
            return View(ProductController.products);
        }

        // GET: Product Detail on specified ID
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (!products.Any(m => m.ID == id))
            {
                return HttpNotFound();
            }

            Product p = products.Single(m => m.ID == id);
            return View("Detail", p);

        }
    }
}