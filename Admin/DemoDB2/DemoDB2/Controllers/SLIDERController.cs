using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class SLIDERController : Controller
    {
        // GET: SLIDER
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: DanhSachPRODUCT
        //public ActionResult Index()
        //{
        //    return View(db.PRODUCTs.ToList());
        //}
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                SLIDE sp = new SLIDE();
                return View(sp);
            }
        }
        [HttpPost]
        public ActionResult Create(SLIDE sp)
        {
            {
                try
                {
                    if (sp.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                        string extent = Path.GetExtension(sp.UploadImage.FileName);
                        filename = filename + extent;
                        sp.image = "~/Content/images/" + filename;
                        sp.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                    }
                    db.SLIDEs.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                catch
                {
                    return View();
                }
            }
        }
        public ActionResult Details(int id)
        {
            return View(db.SLIDEs.Where(s => s.slide_id == id).FirstOrDefault());
        }

        public ActionResult Edit(int id)
        {
            return View(db.SLIDEs.Where(s => s.slide_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, SLIDE sp)
        {
            if (sp.UploadImage != null)
            {
                string filename = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                string extent = Path.GetExtension(sp.UploadImage.FileName);
                filename = filename + extent;
                sp.image = "~/Content/images/" + filename;
                sp.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var t = db.SLIDEs.AsNoTracking().Where(x => x.slide_id == id).FirstOrDefault();
                sp.image = t.image;
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.SLIDEs.Where(s => s.slide_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, SLIDE sp)
        {
            try
            {
                sp = db.SLIDEs.Where(s => s.slide_id == id).FirstOrDefault();
                db.SLIDEs.Remove(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Delete!");
            }
        }
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View(db.SLIDEs.ToList());
            }
        }
    }
}