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
        //Xóa sản phẩm khỏi giỏ hàng
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
        public ActionResult Capnhatgiohang(int IdProduct,FormCollection f)
        {
            // Lấy giỏ hàng từ session
            List<Giohang> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm có trong giỏ hàng hay không
            Giohang product = listGiohang.SingleOrDefault(n => n.IdProduct == IdProduct);
            if(product!= null)
            {
                product.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            // Kiểm tra đăng nhập
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if(Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy giỏ hàng từ session 
            List<Giohang> listGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            var dsProduct = (from s in _db.Products select s).ToList();
            //Thêm đơn hàng
            Order order = new Order();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            order.KhachHangID = kh.idUser;
            order.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            order.NgayGiao = DateTime.Parse(ngaygiao).AddHours(12);
            order.Status = "Chưa giao hàng";
            order.Payment = false;
            order.Address = kh.Address;
            order.ThanhTien = TongTien();
            order.TongSoLuong = TongSoLuong();
            _db.Orders.Add(order);
            _db.SaveChanges();
            foreach (var item in gh)
            {
                Order_Detail ctdh = new Order_Detail();
                ctdh.ID_Order = order.ID;
                ctdh.ID_Product = item.IdProduct;
                ctdh.SoLuong = item.SoLuong;
                ctdh.Price = item.Price;
                Product product = dsProduct.Find(n => n.Id == item.IdProduct);
                product.SoLuong -= ctdh.SoLuong;
                product.ProductSold += ctdh.SoLuong;
                _db.Order_Detail.Add(ctdh);
            }
            _db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang","Giohang");
        }
        public ActionResult Xacnhandonhang()
        {

            return View();
        }
    }
}