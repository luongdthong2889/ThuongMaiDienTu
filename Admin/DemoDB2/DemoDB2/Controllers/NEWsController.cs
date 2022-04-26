using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class NEWsController : Controller
    {
        private DBGiaDungEntities db = new DBGiaDungEntities();

        // GET: NEWs
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var nEWs = db.NEWs.Include(n => n.ADMIN).Include(n => n.CATALOG).Include(n => n.TAG);
                return View(nEWs.ToList());
            }
        }

        // GET: NEWs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEW nEW = db.NEWs.Find(id);
            if (nEW == null)
            {
                return HttpNotFound();
            }
            return View(nEW);
        }

        // GET: NEWs/Create
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.new_created_by = new SelectList(db.ADMINs, "admin_id", "username");
                ViewBag.new_catalog_id = new SelectList(db.CATALOGs, "catalog_id", "catalog_name");
                ViewBag.tag_id = new SelectList(db.TAGs, "tag_id", "tag_name");
                NEW nEW = new NEW();
                return View(nEW);
            }
        }

        // POST: NEWs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "new_id,tag_id,new_catalog_id,new_created_by,tittle,description,new_image,detail,new_created")] NEW nEW, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                var useradmin = Session["username"];
                var userCurrent = db.ADMINs.Where(x => x.username == useradmin.ToString()).FirstOrDefault();
                if (UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(UploadImage.FileName);
                    string extent = Path.GetExtension(UploadImage.FileName);
                    filename = filename + extent;
                    nEW.new_image = "~/Content/img/" + filename;
                    UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                nEW.new_created_by = Convert.ToInt32(userCurrent.admin_id);
                nEW.new_created = DateTime.Now;
                db.NEWs.Add(nEW);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            ViewBag.new_created_by = new SelectList(db.ADMINs, "admin_id", "username", nEW.new_created_by);
            ViewBag.new_catalog_id = new SelectList(db.CATALOGs, "catalog_id", "catalog_name", nEW.new_catalog_id);
            ViewBag.tag_id = new SelectList(db.TAGs, "tag_id", "tag_name", nEW.tag_id);
            return View(nEW);
        }

        // GET: NEWs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEW nEW = db.NEWs.Find(id);
            if (nEW == null)
            {
                return HttpNotFound();
            }
            ViewBag.new_created_by = new SelectList(db.ADMINs, "admin_id", "username", nEW.new_created_by);
            ViewBag.new_catalog_id = new SelectList(db.CATALOGs, "catalog_id", "catalog_name", nEW.new_catalog_id);
            ViewBag.tag_id = new SelectList(db.TAGs, "tag_id", "tag_name", nEW.tag_id);
            return View(nEW);
        }

        // POST: NEWs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "new_id,tag_id,new_catalog_id,new_created_by,tittle,description,new_image,detail,new_created")] NEW nEW,int id, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                if (UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(UploadImage.FileName);
                    string extent = Path.GetExtension(UploadImage.FileName);
                    filename = filename + extent;
                    nEW.new_image = "~/Content/img/" + filename;
                    UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                    nEW.new_created = DateTime.Now;
                    db.Entry(nEW).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var t = db.NEWs.AsNoTracking().Where(x => x.new_id == id).FirstOrDefault();
                    nEW.new_image = t.new_image;
                    nEW.new_created = DateTime.Now;
                    db.Entry(nEW).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.new_created_by = new SelectList(db.ADMINs, "admin_id", "username", nEW.new_created_by);
            ViewBag.new_catalog_id = new SelectList(db.CATALOGs, "catalog_id", "catalog_name", nEW.new_catalog_id);
            ViewBag.tag_id = new SelectList(db.TAGs, "tag_id", "tag_name", nEW.tag_id);
            return View(nEW);
        }

        // GET: NEWs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEW nEW = db.NEWs.Find(id);
            if (nEW == null)
            {
                return HttpNotFound();
            }
            return View(nEW);
        }

        // POST: NEWs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NEW nEW = db.NEWs.Find(id);
            db.NEWs.Remove(nEW);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
