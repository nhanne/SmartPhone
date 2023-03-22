﻿using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Nike.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        public ActionResult Index()
        {
            return RedirectToAction("ProFile");
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
                    khachhang.Password = GetMD5(khachhang.Password);
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
                var f_password = GetMD5(password);
                var data = _db.KhachHangs.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                KhachHang kh = _db.KhachHangs.SingleOrDefault(s => s.Email.Equals(email) && s.Password.Equals(f_password));
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
                    return RedirectToAction("Index","Home");
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
            return RedirectToAction("Index","Home");
        }

        //create a string MD5,Dùng chuyển đổi một chuỗi về dạng mã hóa MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }


        public ActionResult ProFile()
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if(kh!= null)
            {
                return View(kh);
            }    
            return HttpNotFound();

        }
        public ActionResult EditProFile(int idUser)
        {
            KhachHang khsession = (KhachHang)Session["Taikhoan"];
            KhachHang kh = _db.KhachHangs.Find(idUser);
            if (khsession.idUser != kh.idUser)
            {
                return HttpNotFound();
               
            }
            return View(kh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProFile([Bind(Include = "idUser,FirstName,LastName,Email,Password,Picture,Address,NgaySinh,CMT,Sdt")] KhachHang kh, HttpPostedFileBase file)
        {
            KhachHang khachhang = _db.KhachHangs.Find(kh.idUser);
            if (ModelState.IsValid)
            {
                String anh = khachhang.Picture;
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Hinh/KhachHang"), pic);
                    file.SaveAs(path);
                    anh = pic;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                khachhang.Picture = anh;
                khachhang.FirstName = kh.FirstName;
                khachhang.LastName = kh.LastName;
                khachhang.Email = kh.Email;
                //khachhang.Password = GetMD5(kh.Password);
                khachhang.Address = kh.Address;
                if(kh.NgaySinh != null)
                {
                    khachhang.NgaySinh = kh.NgaySinh;
                }
                else
                {
                    khachhang.NgaySinh = khachhang.NgaySinh;
                }
                khachhang.CMT = kh.CMT;
                khachhang.Sdt = kh.Sdt;
                _db.Entry(khachhang).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ProFile");
            }
            return View(kh);
        }
           
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
                        ViewBag.orderList = orderList.Where(s=>s.Status == "Hoàn thành");
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
            if(Session["Taikhoan"] == null)
            {
                return RedirectToAction("Login","Account");
            }
            else
            {
                kh = (KhachHang)Session["Taikhoan"];
            }
            return View(kh);
        }

        public ActionResult CancelOrder(int ID)
        {
            var orderList = (from s in _db.Orders select s).ToList();
            Order order = _db.Orders.Find(ID);
            if(order.Status == "Chưa giao hàng")
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