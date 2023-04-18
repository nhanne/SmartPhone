using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Net;
using System.Data.Entity;

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
        // Chức năng sửa thông tin sản phẩm - Nhân
        public ActionResult Edit(int Id)
        {


            if (Id.ToString() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName", product.CatalogId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CatalogId,Picture,ProductName,ProductCode,PriceOld,UnitPrice,ProductSold,ProductSale,SoLuong")] Product product, HttpPostedFileBase file)
        {
            Product pr = _db.Products.Find(product.Id);
            if (ModelState.IsValid)
            {
                String anh = pr.Picture;
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
                pr.Picture = anh;
                pr.CatalogId = product.CatalogId;
                pr.ProductName = product.ProductName;
                pr.PriceOld = product.PriceOld;
                pr.UnitPrice = (pr.ProductSale != null) ? (pr.UnitPrice = (pr.PriceOld - (pr.PriceOld * int.Parse(pr.ProductSale)) / 100)) : (pr.UnitPrice = pr.PriceOld);
                pr.ProductCode = product.ProductCode;
                pr.ProductSold = product.ProductSold;
                pr.ProductSale = product.ProductSale;
                pr.SoLuong = 500 - pr.ProductSold;
                _db.Entry(pr).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName", pr.CatalogId);
            return View(product);
        }
    }
}