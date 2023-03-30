using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nike.Models;


namespace Nike.Controllers
{
    public class HomeController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        public ActionResult Index()
        {
            return View();     
        }

        public ActionResult Product(int? id)
        {
            Product product = _db.Products.Find(id);
            return View(product);
        }

    }
}