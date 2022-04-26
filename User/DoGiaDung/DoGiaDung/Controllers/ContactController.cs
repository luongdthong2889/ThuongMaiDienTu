using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoGiaDung.Models;

namespace DoGiaDung.Controllers
{
    public class ContactController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Fail()
        {
            return View();
        }
        public ActionResult CreateFeedback(FormCollection frc)
        {
            var feedname = frc["Feedname"];
            var feedmail = frc["Feedmail"];
            var feedphone = frc["Feedphone"];
            var feedaddress = frc["Feedaddress"];
            var feedmess = frc["Feedmess"];
            FEEDBACK fb = new FEEDBACK()
            {
                user_id = 1,
                user_name = feedname,
                user_email = feedmail,
                user_phone = feedphone,
                user_address = feedaddress,
                feedback_conten = feedmess,
                feedback_created = DateTime.Now
            };
            db.FEEDBACKs.Add(fb);
            db.SaveChanges();
            return RedirectToAction("Success", "Contact");
        }
    }
}