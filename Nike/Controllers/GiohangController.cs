using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Controllers
{
    public class GiohangController : Controller
    {
        QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            List<Giohang> listGioHang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }
        // Hàm mặc định để lấy giỏ hàng
        public List<Giohang> Laygiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;

            if (listGiohang == null)
            {
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;

            }
            return listGiohang;
        }
        // Tính số lượng sản phẩm
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng tiền sản phẩm
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongTien = listGiohang.Sum(n => n.Price);
            }
            return iTongTien;
        }

        //Xóa sản phẩm khỏi giỏ hàng Phát
        public ActionResult Xoagiohang(int IdProduct)
        {
            // Lấy giỏ hàng từ session
            List<Giohang> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm có trong giỏ hàng hay không
            Giohang product = listGiohang.SingleOrDefault(n => n.IdProduct == IdProduct);
            //nếu tồn tại thì sửa số lượng
            if (product != null)
            {
                listGiohang.RemoveAll(n => n.IdProduct == IdProduct);
                return RedirectToAction("Index");
            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        // Thêm sản phẩm vào giỏ hàng
        public ActionResult Themgiohang(int IdProduct, string strURL)
        {
            // Lấy ra session giỏ hàng
            List<Giohang> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm này có trong giỏ hàng chưa
            Giohang product = listGiohang.Find(n => n.IdProduct == IdProduct);
            if (product == null)
            {
                product = new Giohang(IdProduct);
                listGiohang.Add(product);
                return Redirect(strURL);
            }
            else
            {
                product.SoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult BuyNow(int IdProduct)
        {
            List<Giohang> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm này có trong giỏ hàng chưa
            Giohang product = listGiohang.Find(n => n.IdProduct == IdProduct);
            if (product == null)
            {
                product = new Giohang(IdProduct);
                listGiohang.Add(product);
            }
            else
            {
                product.SoLuong++;
            }
            return RedirectToAction("Index");
        }
    }
}