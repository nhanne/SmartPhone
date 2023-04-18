using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

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
            var dsProduct = (from s in _db.Products select s).ToList().ToPagedList(pageNum, pageSize);
            var totalPage = (double)dsProduct.Count();
            ViewBag.totalPage = Math.Ceiling(totalPage);
           
            // Tìm kiêm sản phẩm - Duy
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ViewBag.searchStr = searchString;

                var dsSearch = _db.Products.ToList().Where(s => s.ProductName.ToLower().Contains(searchString) || s.Catalog.CatalogName.ToLower().Contains(searchString));
                totalPage = (double)dsSearch.Count() / 10;
                ViewBag.totalPage = Math.Ceiling(totalPage);
                ViewBag.products = dsSearch.ToPagedList(pageNum, pageSize);
            }
            //
            else
            {
                ViewBag.products = dsProduct;
            }
            return View(dsProduct);
        }
        // Tạo sản phẩm mới - Nhân
        public ActionResult Create()
        {
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "CatalogId,ProductName,ProductCode,UnitPrice,SoLuong,ProductSold,ProductSale,PriceOld")] Product model)
        {

            if (ModelState.IsValid)
            {
                String anh = "/Hinh/Product/hinhphone.jpg";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Hinh"), pic);
                    file.SaveAs(path);
                    anh = pic;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                model.Picture = anh;
                Random prCode = new Random();
                model.ProductCode = String.Concat("PR", prCode.Next(5000, 7000).ToString());
                model.ProductSold = 0;
                model.UnitPrice = (model.ProductSale != null) ? (model.UnitPrice = (model.PriceOld - (model.PriceOld * int.Parse(model.ProductSale)) / 100)) : (model.UnitPrice = model.PriceOld);
                _db.Products.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName", model.CatalogId);
            return View(model);
        }

    }



}