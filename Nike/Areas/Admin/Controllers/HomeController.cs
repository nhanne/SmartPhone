using Nike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nike.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {

            return View();
        }
    }
}