using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nike.Models;
using System.Security.Cryptography;
using System.Text;
using PagedList;
using PagedList.Mvc;

namespace Nike.Controllers
{
    public class HomeController : Controller
    {
        private QuanLySanPhamEntities _db = new QuanLySanPhamEntities();
        public ActionResult Index(string sort, int pageIndex = 1)
        {
            Sort(sort, pageIndex);
            return View();         
        }
        public void Sort(string sort, int pageIndex)
        {
            //Catalog
            ViewBag.catalogs = (from s in _db.Catalogs select s).ToList();           
            //Product
            var dsProduct = (from s in _db.Products select s).ToList();
            var totalPage = (double)dsProduct.Count() / (double)8;
            ViewBag.totalPage = Math.Ceiling(totalPage);
            ViewBag.currentPage = pageIndex;
            // sắp xếp theo filter
            if (String.IsNullOrEmpty(sort))
            {               
                ViewBag.products = dsProduct.ToPagedList(pageIndex, 8);
            }
            else
            {
                sort = sort.ToLower();
                switch (sort)
                {
                    case "asc":
                        ViewBag.products = dsProduct.OrderBy(c => c.UnitPrice).ToPagedList(pageIndex, 8);
                        ViewBag.currentSort = "asc";
                        break;
                    case "desc":
                        ViewBag.products = dsProduct.OrderByDescending(c => c.UnitPrice).ToPagedList(pageIndex, 8);
                        ViewBag.currentSort = "desc";
                        break;
                    case "new":
                        ViewBag.products = dsProduct.OrderByDescending(c => c.Id).ToPagedList(pageIndex, 8);
                        ViewBag.currentSort = "new";
                        break;
                    case "hot":
                        ViewBag.products = dsProduct.OrderByDescending(c => c.ProductSold).ToPagedList(pageIndex, 8);
                        ViewBag.currentSort = "hot";
                        break;
                    default:
                        ViewBag.products = dsProduct.Where(s => s.ProductName.ToLower().Contains(sort) || s.Catalog.CatalogName.ToLower().Contains(sort));
                        ViewBag.currentSort = "";
                        break;
                }
            }
           
        }
        public ActionResult Product(int? id)
        {
            Product product = _db.Products.Find(id);
            return View(product);
        }


    }
}