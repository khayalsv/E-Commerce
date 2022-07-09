using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var data = _db.ProductTypes.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes p)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Add(p);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
                return NotFound();


            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes p)
        {
            if (ModelState.IsValid)
            {
                _db.Update(p);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }


        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
                return NotFound();


            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes p)
        {
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
                return NotFound();


            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductTypes p)
        {
            if (id == null)
                return NotFound();

            if (id!=p.Id)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);

            if (productType == null)
                return NotFound();


            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }
    }
}
