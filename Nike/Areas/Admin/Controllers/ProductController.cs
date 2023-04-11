using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Nike.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Product
        public ActionResult Index(int? page, string searchString)
        {
            int pageNum = (page ?? 1);
            int pageSize = 10;

            NhanVien nv = (NhanVien)Session["NV"];
            if (nv.MaChucVu == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = (from s in _db.Products select s).ToList().ToPagedList(pageNum, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ViewBag.products = _db.Products.Where(s => s.ProductName.ToLower().Contains(searchString) || s.Catalog.CatalogName.ToLower().Contains(searchString)).ToPagedList(pageNum, pageSize);
            }
            else
            {
                ViewBag.products = products;
            }
            return View(products);
        }
    }
}