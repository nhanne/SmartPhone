using Nike.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Admin/Order
        public ActionResult Index(string searchStr,string sort, int? page)
        {
            const int pageSize = 10;
            int pageNumber = page ?? 1;
            var orders = _db.Orders.ToList();
            // Tìm kiếm đơn hàng = Sđt - Sprin 6
            if (!String.IsNullOrEmpty(searchStr))
            {
                searchStr = searchStr.ToLower();
                ViewBag.searchStr = searchStr;
                orders = orders.Where(p => p.KhachHang.Sdt.ToString().ToLower().Contains(searchStr)).ToList();
                ViewBag.orderList = orders;
            }
            else
            {
                Sort(sort);
            }
            
            return View();

        }


        public void Sort(string sort)
        {
            ViewBag.Sort = sort;
            var orderList = (from s in _db.Orders select s).ToList();
            foreach (var order in orderList)
            {
                if (order.NgayGiao < DateTime.Now && order.Status == "Đang giao hàng")
                {
                    Order editOrder = _db.Orders.Find(order.ID);
                    editOrder.Status = "Hoàn thành";
                    editOrder.Payment = true;
                    _db.Entry(editOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            if (String.IsNullOrEmpty(sort))
            {
                ViewBag.orderList = orderList;
            }
            else
            {
                switch (sort)
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
                        ViewBag.orderList = orderList.Where(s => s.KhachHang.Sdt.ToString().Contains(sort));
                        break;
                }
            }
        }
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }


    }
}