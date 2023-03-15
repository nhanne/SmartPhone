using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private QuanLySanPhamEntities1 _db = new QuanLySanPhamEntities1();
        // GET: Admin/Order
        public ActionResult Index(string searchString)
        {
            var orderDetail = (from s in _db.Orders select s).ToList();
            
            
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                //ViewBag.orders = orderDetail.Where(s => s.Order.KhachHang.FullName().ToLower().Contains(searchString) || s.Order.ID.ToString().Contains(searchString));
            }
            else
            {
                ViewBag.orders = orderDetail;
            }
            return View(orderDetail);
        }
    }
}