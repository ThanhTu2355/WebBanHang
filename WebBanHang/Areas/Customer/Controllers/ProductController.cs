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
        public IActionResult Index(int page = 1)
        {
            int pageSize = 7;
            int offset = 2;

            var totalItems = _db.Products.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = _db.Products
                              .Include(x => x.Category)
                              .OrderBy(p => p.CategoryId)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.from = Math.Max(1, page - offset);
            ViewBag.to = Math.Min(totalPages, page + offset);

            return View(products);
        }
    }
}
