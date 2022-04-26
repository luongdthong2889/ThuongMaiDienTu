using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class UserController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: User
        //public ActionResult Index()
        //{
        //    return View(db.USERs.ToList());
        //}
        public ActionResult Edit(int id)
        {
            return View(db.USERs.Where(s => s.user_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, USER x)
        {
            db.USERs.Attach(x);
            x.ErrorLogin = "NULL";
            db.Entry(x).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
                    return View(db.USERs.ToList());
                else
                    return View(db.USERs.Where(s => s.user_name.Contains(_name)).ToList());
            }
        }
    }
}