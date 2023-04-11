using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: Admin/NhanVien
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Admin/NhanVien
        public ActionResult Index(string searchString)
        {
            NhanVien nv = (NhanVien)Session["NV"];
            if (nv.MaChucVu == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            var dsNhanVien = (from s in _db.NhanViens select s).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ViewBag.NhanVien = dsNhanVien.Where(s => s.FullName.ToLower().Contains(searchString));
            }
            else
            {
                ViewBag.NhanVien = dsNhanVien;
            }
            return View();
        }
    }
}