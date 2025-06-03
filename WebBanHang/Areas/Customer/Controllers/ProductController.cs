using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hosting;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }
        public IActionResult Index(int? categoryId = 1)
        {
            List<Product> products;
            if (categoryId.HasValue)
            {
                products = _db.Products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }
            else
            {
                products = _db.Products.ToList();
            }
            var categories = _db.Categories.ToList();
            var countProduct = _db.Products
                                   .GroupBy(p => p.CategoryId)
                                   .Select(g => new
                                   {
                                       CategoryId = g.Key,
                                       Count = g.Count()
                                   })
                                   .ToDictionary(x => x.CategoryId, x => x.Count);


            ViewBag.Products = products;
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.CountProduct = countProduct;

            return View();
        }

        // Ajax: Lấy sản phẩm theo category
        [HttpGet]
        public IActionResult Ajax(int? categoryId = 1)
        {
            List<Product> products;
            if (categoryId.HasValue)
            {
                products = _db.Products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }
            else
            {
                products = _db.Products.ToList();
            }
            var categories = _db.Categories.ToList();
            var countProduct = _db.Products
                                   .GroupBy(p => p.CategoryId)
                                   .Select(g => new
                                   {
                                       CategoryId = g.Key,
                                       Count = g.Count()
                                   })
                                   .ToDictionary(x => x.CategoryId, x => x.Count);


            ViewBag.Products = products;
            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.CountProduct = countProduct;

            return PartialView("_ProductListPartial", products);
        }
    }
}
