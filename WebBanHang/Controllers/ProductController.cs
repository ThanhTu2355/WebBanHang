using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hosting;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hosting)
        {
            _db = db;
            _hosting = hosting;
        }

        //Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var productList = _db.Products.Include(x => x.Category).ToList();
            return View(productList);
        }

        //Hiển thị form thêm sản phẩm mới
        public IActionResult Add()
        {
            //truyền danh sách thể loại cho View để sinh ra điều khiển DropDownList
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }

        //Xử lý thêm sản phẩm
        [HttpPost]
        public IActionResult Add(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid) //kiem tra hop le
            {
                if (ImageUrl != null)
                {
                    //xu ly upload và lưu ảnh đại diện
                    product.ImageUrl = SaveImage(ImageUrl);
                }
                //thêm product vào table Product
                _db.Products.Add(product);
                _db.SaveChanges();
                TempData["success"] = "Product inserted success";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = _db.Categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
            return View();
        }

        private string SaveImage(IFormFile image)
        {
            //đặt lại tên file cần lưu
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            //lay duong dan luu tru wwwroot tren server
            var path = Path.Combine(_hosting.WebRootPath, @"images/products");
            var saveFile = Path.Combine(path, filename);
            using (var filestream = new FileStream(saveFile, FileMode.Create))
            {
                image.CopyTo(filestream);
            }
            return @"images/products/" + filename;
        } 
    }
}
