using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index(int? page)
        {
            var data = _db.Products.Include(x => x.ProductTypes).ToList().ToPagedList(pageNumber:page??1,pageSize:6);
            return View(data);
        }


        [HttpGet]
        public IActionResult Detail(int? id)
        {

            if (id == null)
                return NotFound();

            var product = _db.Products.Include(x => x.ProductTypes).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();


            return View(product);

        }

        [HttpPost]
        [ActionName("Detail")]
        public IActionResult ProductDetail(int? id)
        {
            List<Product> products = new List<Product>();

            if (id == null)
                return NotFound();

            var product = _db.Products.Include(x => x.ProductTypes).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();


            products = HttpContext.Session.Get<List<Product>>("products");
            if (products==null)
            {
                products = new List<Product>();
            }

            products.Add(product);
            HttpContext.Session.Set("products", products);

            return Redirect("/");

        }

        [HttpPost]
        public IActionResult Remove(int? id)
        {
            var products = HttpContext.Session.Get<List<Product>>("products");
            if (products!=null)
            {
                var product = products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return Redirect("/");
        }


        public IActionResult Cart()
        {
            var products = HttpContext.Session.Get<List<Product>>("products");
            if (products == null)
            {
                products = new List<Product>();
            }
            return View(products);
        }


        [HttpGet]
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            var products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return Redirect("/");
        }

    }
}
