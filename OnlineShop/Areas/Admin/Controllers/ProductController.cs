using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _we;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment we)
        {
            _db = db;
            _we = we;
        }

        public IActionResult Index()
        {
            var data = _db.Products.Include(x => x.ProductTypes).ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Index(int? lowAmount,int? largeAmount)
        {
            var data = _db.Products.Include(x => x.ProductTypes)
                .Where(x=>x.Price>=lowAmount && x.Price<=largeAmount).ToList();

            if (lowAmount==null|| largeAmount==null)
            {
                data = _db.Products.Include(x => x.ProductTypes).ToList();
            }
            return View(data);
        }



        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product p, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(x => x.Name == p.Name);
                if (searchProduct!=null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    return View(p);
                }


                if (image != null)
                {
                    var name = Path.Combine(_we.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    p.Image = "Images/" + image.FileName;
                }
                _db.Products.Add(p);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            if (id == null)
                return NotFound();

            var product = _db.Products.Include(x => x.ProductTypes).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product p, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_we.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    p.Image = "Images/" + image.FileName;
                }


                _db.Products.Update(p);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _db.Products.Include(x => x.ProductTypes).FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();


            return View(product);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _db.Products.Include(x => x.ProductTypes).Where(x => x.Id == id).FirstOrDefault();

            if (product == null)
                return NotFound();


            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}