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
        public ActionResult Index(string sort)
        {
            Sort(sort);
            return View();
        }


        public void Sort(string sort)
        {
            var orderList = (from s in _db.Orders select s).ToList();
            foreach (var order in orderList)
            {
                if (order.NgayGiao < DateTime.Now && order.Status == "Đang giao hàng")
                {
                    Order editOrder = _db.Orders.Find(order.ID);
                    editOrder.Status = "Hoàn thành";
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
        public ActionResult Edit(int Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _db.Orders.Find(Id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(_db.Order_Status, "Status", "Status", order.Status);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID")] Order model)
        {
            Order order = _db.Orders.Find(model.ID);
            order.Status = "Đang giao hàng";
            order.NgayGiao = DateTime.Now.AddDays(3);
            _db.Entry(order).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}