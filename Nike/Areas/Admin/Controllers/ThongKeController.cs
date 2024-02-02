using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();

        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            return View(); ;
        }
        [HttpGet]
        public ActionResult GetStatistical(string FromDate, string toDate)
        {
            var query = from o in _db.Orders
                        join od in _db.Order_Detail
                        on o.ID equals od.ID_Order
                        join p in _db.Products
                        on od.ID_Product equals p.Id
                        select new
                        {
                            CreatedDate = o.NgayDat,
                            Quantity = od.SoLuong,
                            Price = od.Price,
                            OriginalPrice = p.UnitPrice
                        };
            if (!string.IsNullOrEmpty(FromDate))
            {
                DateTime startDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= endDate);
            }
            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.Price),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new{Data = result}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeDoanhThu()
        {
            List<Order> donHangs = _db.Orders.Where(d => d.Status == "Hoàn thành").ToList();
            //Thống kê doanh thu theo tháng 
            List<DoanhThuViewModel> ThongKeDoanhThu = new List<DoanhThuViewModel>();

            var doanhThuTheoThang = donHangs.GroupBy(d => new { d.NgayDat.Value.Year, d.NgayDat.Value.Month })
                                    .Select(g => new DoanhThuViewModel
                                    {
                                        Thang = $"{g.Key.Month}/{g.Key.Year}",
                                        TongDoanhThu = (double)g.Sum(d => d.ThanhTien)
                                    })
                                    .OrderBy(g => g.Thang);
            ThongKeDoanhThu.AddRange(doanhThuTheoThang);

            // Chuyển dữ liệu thống kê doanh thu qua View để hiển thị
            return View(ThongKeDoanhThu);
        }


    }
}