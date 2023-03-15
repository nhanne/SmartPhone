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
        private QuanLySanPhamEntities1 _db = new QuanLySanPhamEntities1();
        public ActionResult Index()
        {
            var dsKhachHang = (from s in _db.KhachHangs select s).ToList();
            return View(dsKhachHang);
        }
    }
}