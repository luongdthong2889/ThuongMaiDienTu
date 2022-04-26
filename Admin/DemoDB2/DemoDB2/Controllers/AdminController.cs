using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class AdminController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        TemplateMethod.TempletemethodInit1 temp1 = new TemplateMethod.TempletemethodInit1();
        // GET: Admin
        public ActionResult Index()
        {
            CategorySingleton.Instance.Init(db);
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcc(FormCollection frc)
        {
            var _adname = frc["admin_name"];
            var _pass = frc["admin_pass"];
            var check = db.ADMINs.Where(s => s.username == _adname && s.paswword == _pass).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = temp1.Trave();
                return View("Index");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["username"] = _adname;
                Session["password"] = _pass;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}