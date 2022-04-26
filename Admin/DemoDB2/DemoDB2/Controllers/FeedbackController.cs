using DemoDB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoDB2.Controllers
{
    public class FeedbackController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        // GET: Feedback
        public ActionResult Index(string _name)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                if (_name == null)
                    return View(db.FEEDBACKs.ToList());
                else
                    return View(db.FEEDBACKs.Where(s => s.user_name.Contains(_name)).ToList());
            }
        }
    }
}