using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nike.Models;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
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
                ViewBag.products = _db.Products.Where(s => s.ProductName.ToLower().Contains(searchString) || s.Catalog.CatalogName.ToLower().Contains(searchString));
            }
            else
            {
                ViewBag.products = products;
            }
            return View(products);
        }
        public ActionResult Create()
        {
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName");
            return View() ;  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include= "CatalogId,ProductName,ProductCode,UnitPrice,SoLuong,ProductSold,ProductSale,PriceOld")] Product model)
        {

            if (ModelState.IsValid)
            {
                String anh = "/Hinh/default.png";
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
                model.ProductCode = prCode.Next(72000000, 79000000).ToString();
                model.UnitPrice = (model.ProductSale != null) ? (model.UnitPrice = (model.PriceOld - (model.PriceOld * int.Parse(model.ProductSale)) / 100)) : (model.UnitPrice = model.PriceOld);
                _db.Products.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName",model.CatalogId);
            return View(model);

    
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: LoaiPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            try
            {
                Product product = _db.Products.Find(Id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult Edit(int Id)
        {
           

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName",product.CatalogId);
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
                    anh =  pic;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                pr.Picture = anh;
                pr.CatalogId = product.CatalogId;
                pr.ProductName = product.ProductName;
                pr.UnitPrice = (pr.ProductSale != null) ? (pr.UnitPrice = (pr.PriceOld - (pr.PriceOld * int.Parse(pr.ProductSale)) / 100)) : (pr.UnitPrice = pr.PriceOld);
                pr.ProductCode = product.ProductCode;
                pr.ProductSold = product.ProductSold;
                pr.ProductSale = product.ProductSale;
                pr.SoLuong = product.SoLuong - pr.ProductSold;
                _db.Entry(pr).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatalogId = new SelectList(_db.Catalogs, "ID", "CatalogName", pr.CatalogId);
            return View(product);
        }
    }
}