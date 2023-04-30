using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class CatalogController : Controller
    { // GET: Catalog
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        public ActionResult Index()
        {
            NhanVien nv = (NhanVien)Session["NV"];
            if (nv.MaChucVu == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            var catalog = (from s 
                           in _db.Catalogs 
                           select s).ToList();
            ViewBag.catalogs = catalog;
            return View();

        }
        public ActionResult Create()
        {
            return View(new Catalog() { ID = 0, CatalogCode = "", CatalogName = "" });
        }
        [HttpPost]
        public ActionResult Create(Catalog model)
        {
            var catalogs = (from s in _db.Catalogs select s).ToList();
            model.ID = catalogs.Last().ID + 1;
            // lưu dữ liệu vào db
            if (string.IsNullOrEmpty(model.CatalogCode) && string.IsNullOrEmpty(model.CatalogName) == true)
            {
                ModelState.AddModelError("", "Mã danh mục không được để trống");
                return View(model);
            }
            _db.Catalogs.Add(model);
            _db.SaveChanges();

            if (model.ID > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Không lưu vào được database");
                return View(model);
            }
        }
        public ActionResult Edit(int ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalog catalog = _db.Catalogs.Find(ID);
            if (catalog == null)
            {
                return HttpNotFound();
            }
            return View(catalog);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "ID,CatalogCode,CatalogName")] Catalog catalogs)
        {
            Catalog catalog = _db.Catalogs.Find(catalogs.ID);
            if (ModelState.IsValid)
            {
                catalog.ID = catalogs.ID;
                catalog.CatalogCode = catalogs.CatalogCode;
                catalog.CatalogName = catalogs.CatalogName;
                _db.Entry(catalog).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalog);
        }
        public ActionResult Delete(int ID)
        {

            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catalog catalog = _db.Catalogs.Find(ID);
            if (catalog == null)
            {
                return HttpNotFound();
            }
            return View(catalog);
        }
        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            Catalog catalog = _db.Catalogs.Find(ID);
            _db.Catalogs.Remove(catalog);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
