using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoGiaDung.Models;
using PagedList;
using PagedList.Mvc;

namespace DoGiaDung.Controllers
{
    public class ProductController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: Product
        public ActionResult Index(string catalog, int? page, string searchstring, string sortBy)
        {

            int pageSize = 9;
            int pageNum = (page ?? 1);

            ViewBag.PriceHighLow = "Price_desc";
            ViewBag.PriceLowHigh = "Price_asc";
            if (sortBy != null)
            {
                switch (sortBy)
                {
                    case "Price_desc":
                        var listsort = db.PRODUCTs.OrderByDescending(x => x.price).ToList();
                        return View(listsort.ToPagedList(pageNum, pageSize));
                    case "Price_asc":
                        var listsort1 = db.PRODUCTs.OrderBy(x => x.price).ToList();
                        return View(listsort1.ToPagedList(pageNum, pageSize));
                    default:
                        break;
                }
            }

            if (searchstring != null)
            {
                var z = db.PRODUCTs.Where(s => s.product_name.Contains(searchstring)).ToList();
                return View(z.ToPagedList(pageNum, pageSize));
            }
            if (catalog == null)
            {
                var list = db.PRODUCTs.Include("CATALOG").Where(s => s.quantity > 0).ToList();
                return View(list.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var l = db.PRODUCTs.Include("CATALOG").Where(x => x.CATALOG.catalog_name == catalog).ToList();
                return View(l.ToPagedList(pageNum, pageSize));
            }

        }
        public ActionResult SearchGia(int? page, double min = double.MinValue, double max = double.MaxValue)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var s = db.PRODUCTs.Where(p => (double)p.price >= min && (double)p.price <= max).ToList();
            return View(s.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Product_Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            return View(db.PRODUCTs.Include("CATALOG").Where(x => x.product_id == id).FirstOrDefault());
        }
        public PartialViewResult CatalogPartial()
        {
            var catalist = db.CATALOGs.ToList();
            return PartialView(catalist);
        }
        public PartialViewResult TagPartial()
        {
            var taglist = db.TAGs.ToList();
            return PartialView(taglist);
        }
    }
}