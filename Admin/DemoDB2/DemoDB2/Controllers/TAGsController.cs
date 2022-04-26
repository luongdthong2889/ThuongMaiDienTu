using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoDB2.Models;

namespace DemoDB2.Controllers
{
    public class TAGsController : Controller
    {
        private DBGiaDungEntities db = new DBGiaDungEntities();

        // GET: TAGs
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                return View(db.TAGs.ToList());
        }

        // GET: TAGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAG tAG = db.TAGs.Find(id);
            if (tAG == null)
            {
                return HttpNotFound();
            }
            return View(tAG);
        }

        // GET: TAGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TAGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tag_id,tag_name")] TAG tAG)
        {
            if (ModelState.IsValid)
            {
                db.TAGs.Add(tAG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tAG);
        }

        // GET: TAGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAG tAG = db.TAGs.Find(id);
            if (tAG == null)
            {
                return HttpNotFound();
            }
            return View(tAG);
        }

        // POST: TAGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tag_id,tag_name")] TAG tAG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tAG);
        }

        // GET: TAGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAG tAG = db.TAGs.Find(id);
            if (tAG == null)
            {
                return HttpNotFound();
            }
            return View(tAG);
        }

        // POST: TAGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TAG tAG = db.TAGs.Find(id);
            db.TAGs.Remove(tAG);
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
