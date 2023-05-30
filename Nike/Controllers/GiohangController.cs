using Nike.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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

        public ActionResult Capnhatgiohang(int IdProduct, FormCollection f)
        {
            // Lấy giỏ hàng từ session
            List<Giohang> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm có trong giỏ hàng hay không
            Giohang product = listGiohang.SingleOrDefault(n => n.IdProduct == IdProduct);
            if (product != null)
            {
                product.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            List<Giohang> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        // Thêm sản phẩm vào giỏ hàng - Thắng
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
        // Đặt hàng - Thịnh
        [HttpGet]
        public ActionResult DatHang()
        {
            // Kiểm tra đăng nhập
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["Giohang"] == null)
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
            KhachHang khsession = (KhachHang)Session["Taikhoan"];
            KhachHang kh = _db.KhachHangs.Find(khsession.idUser);
            List<Giohang> gh = Laygiohang();
            order.KhachHangID = kh.idUser;
            order.NgayDat = DateTime.Now;
            order.NgayGiao = order.NgayDat.Value.AddDays(3);
            order.Status = "Chưa giao hàng";
            order.Payment = false;
            string DiaChi = collection["DiaChi"];
            string Tinh = collection["Tinh"];
            string Quan = collection["Quan"];
            string Phuong = collection["Phuong"];
            order.Address = String.Concat(DiaChi, " ", Tinh, " ", Quan, " ", Phuong);
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
                _db.Order_Detail.Add(ctdh);
            }
            _db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        [HttpGet]
        public ActionResult DatHangkoTK()
        {
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy giỏ hàng từ session 
            List<Giohang> listGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }
        public ActionResult DatHangkoTK(FormCollection collection)
        {
            var dsProduct = (from s in _db.Products select s).ToList();
            //Thêm đơn hàng
            var HoTen = collection["HoTen"];
            var Sdt = collection["Sdt"];
            var Email = collection["Email"];

            KhachHang kh = new KhachHang();
            kh.LastName = HoTen.ToString();
            kh.Email = Email.ToString();
            String anh = "user.jpg";
            kh.Picture = anh;
            if (Sdt.ToString().Length == 10)
            {
                kh.Sdt = Sdt.ToString();
            }
            else
            {
                kh.Sdt = null;
            }
            _db.KhachHangs.Add(kh);
            _db.SaveChanges();
            Order order = new Order();
            List<Giohang> gh = Laygiohang();
            order.KhachHangID = kh.idUser;
            order.NgayDat = DateTime.Now;
            order.NgayGiao = order.NgayDat.Value.AddDays(3);
            order.Status = "Chưa giao hàng";
            order.Payment = false;
            var DiaChi = collection["DiaChi"];
            order.Address = DiaChi.ToString();
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
                _db.Order_Detail.Add(ctdh);
            }
            _db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
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
        public ActionResult Xacnhandonhang()
        {
            return View();
        }

        public ActionResult Payment() // tạo yêu cầu thanh toán và chuyển hướng người dùng đến trang thanh toán
        {
           
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (TongTien()*100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", _db.Orders.ToList().LastOrDefault().ID.ToString()+1); //mã hóa đơn
            //pay.AddRequestData("vnp_payment", order.Payment.ToString()); //mã hóa đơn


            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        Order order = new Order();
                        KhachHang khsession = (KhachHang)Session["Taikhoan"];
                        KhachHang kh = _db.KhachHangs.Find(khsession.idUser);
                        List<Giohang> gh = Laygiohang();
                        if (kh != null)
                        {
                            order.KhachHangID = kh.idUser;
                            order.NgayDat = DateTime.Now;
                            order.NgayGiao = order.NgayDat.Value.AddDays(3);
                            order.Status = "Chưa giao hàng";
                            order.Payment = true;
                            order.Address = kh.Address;
                            order.ThanhTien = TongTien();
                            order.TongSoLuong = TongSoLuong();
                        }
                        _db.Orders.Add(order);
                        foreach (var item in gh)
                        {
                            Order_Detail ctdh = new Order_Detail();
                            ctdh.ID_Order = order.ID;
                            ctdh.ID_Product = item.IdProduct;
                            ctdh.SoLuong = item.SoLuong;
                            ctdh.Price = item.Price;
                            Product product = _db.Products.ToList().Find(n => n.Id == item.IdProduct);
                            product.SoLuong -= 1;
                            product.ProductSold += 1;
                            _db.Entry(product).State = EntityState.Modified;
                            _db.Order_Detail.Add(ctdh);
                        }
                        _db.SaveChanges();
                        Session["Giohang"] = null;
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + order.ID.ToString() + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

    }
}