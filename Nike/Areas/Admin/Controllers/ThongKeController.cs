using Nike.Models;
using System;
using System.Collections.Generic;
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
     
      

    }
}