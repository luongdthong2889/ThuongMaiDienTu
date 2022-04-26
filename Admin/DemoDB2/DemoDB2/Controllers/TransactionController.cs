using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class TransactionController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: Transaction
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View(db.TRANSACTIONs.ToList());
            }    
        }
        public ActionResult SelectTransactionStatus()
        {
            TRANSACTION se_tran_stt = new TRANSACTION();
            se_tran_stt.ListTransactionStatus = db.TRANSACTIONs.ToList();
            return PartialView(se_tran_stt);
        }
        public ActionResult Edit(int id)
        {
            return View(db.TRANSACTIONs.Where(s => s.transaction_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, TRANSACTION gd)
        {
            db.Entry(gd).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditDetail(int id)
        {
            return View(db.ORDERs.Where(x => x.order_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditDetail(ORDER x)
        {
            db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(db.TRANSACTIONs.Where(s => s.transaction_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, TRANSACTION gd, ORDER dd)
        {
            try
            {
                gd = db.TRANSACTIONs.Where(s => s.transaction_id == id).FirstOrDefault();
                dd = db.ORDERs.Where(s => s.transaction_id == gd.transaction_id).FirstOrDefault();
                db.ORDERs.Remove(dd);
                db.TRANSACTIONs.Remove(gd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, ERROR DELETE");
            }
        }
        public ActionResult Detail(int id)
        {
            return View(db.TRANSACTIONs.Where(x => x.transaction_id == id).FirstOrDefault());
        }
        public ActionResult Duyet(int id)
        {
            foreach (var p in db.TRANSACTIONs.Where(s => s.transaction_id == id))
            {
                if (p.transaction_status == 1)
                {
                    p.transaction_status = 2;
                }
                else if(p.transaction_status == 2) 
                {
                    p.transaction_status = 3;
                }
                else if (p.transaction_status == 3)
                {
                    p.transaction_status = 4;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Transaction");
        }
    }
}