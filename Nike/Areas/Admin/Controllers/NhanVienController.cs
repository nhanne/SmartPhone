using Nike.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        // Hiển thị giao diện Admin - Thắng
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
        // Thêm nhân viên mới - Nhân
        public ActionResult Create()
        {
            ViewBag.MaChucVu = new SelectList(_db.ChucVus, "MaChucVu", "ChucVu1");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "FullName,Email,Address,NgaySinh,Password,MaChucVu,Sex,Sdt")] NhanVien nhanvien)
        {

            if (ModelState.IsValid)
            {
                String anh = "user.jpg";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(Server.MapPath("~/Hinh/NhanVien"), pic);
                    file.SaveAs(path);
                    anh = pic;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                nhanvien.Picture = anh;
                _db.NhanViens.Add(nhanvien); // thêm đối tượng Link
                _db.SaveChanges(); // lưu lại
                return RedirectToAction("Index"); // quay trở về trang Index để xem kết quả thay đổi
            }
            ViewBag.MaChucVu = new SelectList(_db.ChucVus, "MaChucVu", "ChucVu1", nhanvien.MaChucVu);
            return View(nhanvien); // nếu không thể tạo mới thì trả về View như cũ
        }




    }
}