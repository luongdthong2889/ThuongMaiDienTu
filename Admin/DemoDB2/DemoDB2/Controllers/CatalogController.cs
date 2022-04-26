using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class CatalogController : Controller
    {

        TemplateMethod.templatemethodinit2 templatemethodinit2 = new TemplateMethod.templatemethodinit2();
        // GET: Catalog
        DBGiaDungEntities db = new DBGiaDungEntities();
        //public ActionResult Index()
        //{
        //    return View(db.CATALOGs.ToList());
        //}
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Create(CATALOG x)
        {
            try
            {
                if(x.parent_id < 0) 
                {
                    ViewBag.ErrorInfo = templatemethodinit2.Trave();
                    return View("Create");
                }
                else
                {
                    db.CATALOGs.Add(x);
                    db.SaveChanges();
                    //CategorySingleton.Instance.listCatalog.Clear();
                    //CategorySingleton.Instance.Init(db);
                    CategorySingleton.Instance.Update(db);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return Content("Error Create New");
            }
        }
        public ActionResult Details(int id)
        {
            return View(db.CATALOGs.Where(s => s.catalog_id == id).FirstOrDefault());
        }
        public ActionResult Edit(int id)
        {
            return View(db.CATALOGs.Where(s => s.catalog_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, CATALOG x)
        {
            db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            CategorySingleton.Instance.Update(db);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.CATALOGs.Where(s => s.catalog_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, CATALOG x)
        {
            try
            {
                x = db.CATALOGs.Where(s => s.catalog_id == id).FirstOrDefault();
                db.CATALOGs.Remove(x);
                db.SaveChanges();
                CategorySingleton.Instance.Update(db);
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Delete!");
            }
        }
        public ActionResult Index(string _name)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                CategorySingleton.Instance.Init(db);
                if (_name == null)
                {
                    var items = CategorySingleton.Instance.listCatalog;
                    return View(items);
                    //return View(db.CATALOGs.ToList());
                }
                else
                    return View(db.CATALOGs.Where(s => s.catalog_name.Contains(_name)).ToList());
            }
        }
        public ActionResult Duplicate(int id)
        {
            return View(db.CATALOGs.Where(s=>s.catalog_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Duplicate(int id, CATALOG x)
        {
            try
            {
                x = db.CATALOGs.Where(s => s.catalog_id == id).FirstOrDefault();
                var d = x.clone();
                db.CATALOGs.Add((CATALOG)d);
                db.SaveChanges();
                CategorySingleton.Instance.Update(db);
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Delete!");
            }
        }
    }
}