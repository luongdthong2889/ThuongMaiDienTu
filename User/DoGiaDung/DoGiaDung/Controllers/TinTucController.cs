using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoGiaDung.Models;

namespace DoGiaDung.Controllers
{
    public class TinTucController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: TinTuc
        public ActionResult Index()
        {
            return View(db.NEWs.Include("CATALOG").Include("TAG").ToList());
        }
        public ActionResult Details(int? id)
        {
            return View(db.NEWs.Include("CATALOG").Include("TAG").Where(s => s.new_id == id).FirstOrDefault());
        }
    }
}