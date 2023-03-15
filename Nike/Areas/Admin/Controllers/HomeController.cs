using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private QuanLySanPhamEntities1 _db = new QuanLySanPhamEntities1();
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
                if (obj != null)
                {
                    Session["NV"] = obj;
                    Session["FullName"] = data.FirstOrDefault().FullName;
                    Session["ChucVu"] = data.FirstOrDefault().ChucVu.ChucVu1;
                    Session["Avatar"] = data.FirstOrDefault().Picture;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Không phải là tài khoản admin");
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