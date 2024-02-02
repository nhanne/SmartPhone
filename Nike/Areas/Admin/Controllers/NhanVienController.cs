using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            // Tìm kiếm nhân viên trong quản lí nhân viên - Thịnh 
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ViewBag.NhanVien = dsNhanVien.Where(s => s.FullName.ToLower().Contains(searchString)).ToList();
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

        // Sửa thông tin nhân viên - Nhân
        public ActionResult Edit(int id)
        {
            if (id.ToString() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanvien = _db.NhanViens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaChucVu = new SelectList(_db.ChucVus, "MaChucVu", "ChucVu1", nhanvien.MaChucVu);
            return View(nhanvien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Email,Address,NgaySinh,Password,MaChucVu,Picture,Sex,Sdt")] NhanVien nv, HttpPostedFileBase file)
        {

            NhanVien nhanvien = _db.NhanViens.Find(nv.Id);
            ViewBag.MaChucVu = new SelectList(_db.ChucVus, "MaChucVu", "ChucVu1", nhanvien.MaChucVu);
            if (ModelState.IsValid)
            {
                String anh = nhanvien.Picture;
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Hinh/NhanVien"), pic);
                    file.SaveAs(path);
                    anh = pic;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                nhanvien.Picture = anh;
                nhanvien.FullName = nv.FullName;
                nhanvien.Email = nv.Email;
                nhanvien.Address = nv.Address;
                if (nv.NgaySinh != null)
                {
                    nhanvien.NgaySinh = nv.NgaySinh;
                }
                else
                {
                    nhanvien.NgaySinh = nhanvien.NgaySinh;
                }

                nhanvien.Password = nv.Password;
                nhanvien.MaChucVu = nv.MaChucVu;
                nhanvien.Sex = nv.Sex;
                nhanvien.Sdt = nv.Sdt;
                _db.Entry(nhanvien).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nv);

        }
        //Xóa nhân viên - Duy
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanvien = _db.NhanViens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(nhanvien);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
                NhanVien nhanvien = _db.NhanViens.Find(Id);
                _db.NhanViens.Remove(nhanvien);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }


        // Xem chi tiết Nhân viên- Nhân
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanvien = _db.NhanViens.Find(id);
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            return View(nhanvien);
        }
    }
}