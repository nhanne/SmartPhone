using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Controllers
{
    public class AccountController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                var check = _db.KhachHangs.FirstOrDefault(s => s.Email == khachhang.Email);
                if (check == null)
                {
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.KhachHangs.Add(khachhang);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        // Đăng nhập và đăng xuất của Phat
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var data = _db.KhachHangs.Where(s => s.Email.Equals(email) && s.Password.Equals(password)).ToList();
                KhachHang kh = _db.KhachHangs.SingleOrDefault(s => s.Email.Equals(email) && s.Password.Equals(password));
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                }
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullNamekh"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Emailkh"] = data.FirstOrDefault().Email;
                    Session["idUserkh"] = data.FirstOrDefault().idUser;
                    Session["Avatarkh"] = data.FirstOrDefault().Picture;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sai email hoặc mật khẩu");
                }
            }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //hoàng - code
        public ActionResult OrderList(string sr)
        {

            var orderList = (from s in _db.Orders select s).ToList();
            var orderDetail = (from s in _db.Order_Detail select s).ToList();
            ViewBag.orderDetail = orderDetail;
            //ViewBag.orderList = orderList;
            if (String.IsNullOrEmpty(sr))
            {
                ViewBag.orderList = orderList;
            }
            else
            {
                switch (sr)
                {
                    case "Wait":
                        ViewBag.orderList = orderList.Where(s => s.Status == "Chưa giao hàng");
                        break;
                    case "Deli":
                        ViewBag.orderList = orderList.Where(s => s.Status == "Đang giao hàng");
                        break;
                    case "Done":
                        ViewBag.orderList = orderList.Where(s => s.Status == "Hoàn thành");
                        break;
                    case "Cancel":
                        ViewBag.orderList = orderList.Where(s => s.Status == "Đã hủy");
                        break;
                    default:
                        ViewBag.orderList = orderList.Where(s => s.ID.ToString().Contains(sr));
                        break;
                }
            }

            //ViewBag.TongSoLuong = TongSoLuong();
            KhachHang kh = new KhachHang();
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                kh = (KhachHang)Session["Taikhoan"];
            }
            return View(kh);
        }
        //Hủy đơn hàng của Thắng
        public ActionResult CancelOrder(int ID)
        {
            var orderList = (from s in _db.Orders select s).ToList();
            Order order = _db.Orders.Find(ID);
            if (order.Status == "Chưa giao hàng")
            {
                order.Status = "Đã hủy";
            }
            else
            {
                order.Status = order.Status;
            }
            return View(order);

        }
        [HttpPost, ActionName("CancelOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelOrderConfirmed(int ID)
        {
            try
            {
                Order order = _db.Orders.Find(ID);
                order.Status = "Đã hủy";
                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("OrderList");
        }
    }
}