﻿using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(NhanVien objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.NhanViens.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();               
                var data = _db.NhanViens.Where(s => s.Email.Equals(objUser.Email) && s.Password.Equals(objUser.Password)).ToList();
                if (obj != null && obj.MaChucVu == 2)
                {
                    Session["NV"] = obj;
                    return RedirectToAction("Index", "Home");
                }
                else if (obj != null && obj.MaChucVu == 1)
                {
                    Session["NV"] = obj;
                    return RedirectToAction("Index", "POS");
                }
                else
                {
                    ModelState.AddModelError("", "Không phải là tài khoản quản trị viên");
                }
            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["NV"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public ActionResult Logout()
        {
            Session["NV"] = null;
            return RedirectToAction("Login", "Home");
        }


    }
}