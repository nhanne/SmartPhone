using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        public ActionResult Index()
        {
            NhanVien nv = (NhanVien)Session["NV"];
            if (nv.MaChucVu == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            var dsKhachHang = (from s in _db.KhachHangs select s).ToList();
            return View(dsKhachHang);
        }
    }
}