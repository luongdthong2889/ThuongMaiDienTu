using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class ProductController : Controller
    {
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
                PRODUCT sp = new PRODUCT();
                return View(sp);
            }
        }
        [HttpPost]
        public ActionResult Create(PRODUCT sp)
        {
            {
                try
                {
                    if (sp.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                        string extent = Path.GetExtension(sp.UploadImage.FileName);
                        filename = filename + extent;
                        sp.image_link = "~/Content/images/" + filename;
                        sp.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                    }
                    db.PRODUCTs.Add(sp);
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
            return View(db.PRODUCTs.Where(s => s.product_id == id).FirstOrDefault());
        }

        public ActionResult Edit(int id)
        {
            return View(db.PRODUCTs.Where(s => s.product_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, PRODUCT sp)
        {
            if(sp.UploadImage != null)
            {
                string filename = Path.GetFileNameWithoutExtension(sp.UploadImage.FileName);
                string extent = Path.GetExtension(sp.UploadImage.FileName);
                filename = filename + extent;
                sp.image_link = "~/Content/images/" + filename;
                sp.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var t = db.PRODUCTs.AsNoTracking().Where(x => x.product_id == id).FirstOrDefault();
                sp.image_link = t.image_link;
                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.PRODUCTs.Where(s => s.product_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, PRODUCT sp)
        {
            try
            {
                sp = db.PRODUCTs.Where(s => s.product_id == id).FirstOrDefault();
                db.PRODUCTs.Remove(sp);
                db.SaveChanges();
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
                if (_name == null)
                    return View(db.PRODUCTs.ToList());
                else
                    return View(db.PRODUCTs.Where(s => s.product_name.Contains(_name)).ToList());
            }
        }
        public ActionResult SelectCate()
        {
            CATALOG se_cate = new CATALOG();
            se_cate.ListCate = db.CATALOGs.ToList<CATALOG>();
            return PartialView(se_cate);
        }
    }
}