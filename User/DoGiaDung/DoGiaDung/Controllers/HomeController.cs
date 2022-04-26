using DoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoGiaDung.Controllers
{
    public class HomeController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: Product
        public PartialViewResult HomePartial()
        {
            var catalist = db.CATALOGs.Take(6).ToList();
            return PartialView(catalist);
        }
        public ActionResult Index(string catalog)
        {
            if (catalog != null)
            {
                return View(db.PRODUCTs.Include("CATALOG").Where(x => x.CATALOG.catalog_name == catalog).ToList());
            }
            return View(db.PRODUCTs.Take(12).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}