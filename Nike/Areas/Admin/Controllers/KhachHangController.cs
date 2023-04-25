﻿using Nike.Models;
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
        public ActionResult Index(string searchStr)
        {
            NhanVien nv = (NhanVien)Session["NV"];
            if (nv.MaChucVu == 1)
            {
                return RedirectToAction("Index", "Order");
            }
            var dsKhachHang = _db.KhachHangs.ToList();
            // Tìm kiếm khách hàng trong quản lí khách hàng bằng email - Duy 
            if (!String.IsNullOrEmpty(searchStr))
            {
                searchStr = searchStr.ToLower();
                ViewBag.NhanVien = dsKhachHang.Where(s => s.Email.ToLower().Contains(searchStr));
            }
            else
            {
                ViewBag.NhanVien = dsKhachHang;
            }

            return View(ViewBag.NhanVien);
        }
    }
}