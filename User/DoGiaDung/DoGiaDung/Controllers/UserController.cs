using DoGiaDung.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;

namespace DoGiaDung.Controllers
{
    public class UserController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        Facade.FacdeController facde = new Facade.FacdeController();
        TemplateMethod.TempletemethodInit2 temp2 = new TemplateMethod.TempletemethodInit2();
        TemplateMethod.TempletemethodInit1 temp1 = new TemplateMethod.TempletemethodInit1();
        Strategy.StrategyY strategy = new Strategy.StrategyY();
        SingleTon.Init Singleton = SingleTon.Init.Instance;
        Decorater.Decoooo decoooo = new Decorater.Decoooo(new Decorater.Decoo());
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThongTin(string id)
        {
            if (Session["user_email"] != null)
            {
                var a = Session["user_email"];
                return View(db.USERs.Where(s => s.user_email == a.ToString()).FirstOrDefault());
            }
            return View(db.USERs.Where(x => x.user_email == id).FirstOrDefault());
        }
        public ActionResult EditUser(int id)
        {
            if (Session["user_email"] != null)
            {
                var a = Session["user_email"];
                return View(db.USERs.Where(s => s.user_email == a.ToString()).FirstOrDefault());
            }
            return View(db.USERs.Where(s => s.user_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditUser(int id, USER user)
        {
            db.USERs.Attach(user);
            user.ErrorLogin = "NULL";
            if (user.user_address == null)
            {
                user.user_address = "NULL";
            }
            //if (user.ResetPasswordCode == null)
            //{
            //    user.ResetPasswordCode = Guid.NewGuid().ToString();
            //}
            if (user.password == null)
            {
                user.password = "";
                return View();
            }
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ThongTin");
        }
        [HttpPost]
        public ActionResult LoginAcc(FormCollection frc)
        {
            var _user_email = frc["email"];
            var _pass = frc["pass"];
            var check = db.USERs.Where(s => s.user_email == _user_email && s.password == _pass).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai thông tin";
                return View("Index");
            }
            else
            {
               
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["user_email"] = _user_email;
                Session["password"] = _pass;
                db.SaveChanges();
                ViewBag.Message = "Đăng nhập thành công";
                return RedirectToAction("Index", "Home");

            }
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(FormCollection frc)
        {
            var _name = frc["name"];
            var _phone = frc["phone"];
            var _email = frc["email"];
            var _pass = frc["pass"];
            var _repass = frc["repeat-pass"];
            if (ModelState.IsValid)
            {
                var check_id = db.USERs.Where(s =>s.user_email == _email).FirstOrDefault();

                if (_name == "" || _phone == "" || _email == "" || _pass == "")
                {
                    ViewBag.ErrorRegister = temp1.Trave();
                    return View();
                }
                if (_pass.Length < 8)
                {
                    ViewBag.ErrorRegister = facde.SaiMatKhau();
                    return View();
                }
                if (check_id == null)//chưa có id
                {
                    USER _user = new USER();
                    _user.user_name = _name;
                    _user.user_phone = _phone;
                    _user.user_email = _email;
                    _user.password = _pass;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.USERs.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = temp2.Trave();
                    return View();
                }
            }
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("raovat.huflit@gmail.com", "Đồ  Gia Dụng Reset Password");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "raovat2889"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = decoooo.getDec();
                body = "<br/><br/>We are excited to tell you that your Rao Vat account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = strategy.QuenPass();
                body = "Chào bạn: " + emailID + "," +
                    "<br><br />Bạn vừa yêu cầu lấy lại mật khẩu tài khoản trên Website Dogiadung.vn" +
                    "<br><br /><b>Chú ý:</b> Bạn có thể bỏ qua email này nếu <b>người yêu cầu lấy lại mật khẩu không phải là bạn</b>" +
                    "<br><br />Hãy click vào dòng dưới đây để lấy lại mật khẩu của mình" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>" +
                    "<br><br />Nếu bạn muốn liên hệ với chúng tôi thì bạn đừng Reply lại theo email này!" +
                    "<br><br />Để biết thêm thông tin, xin hãy liên hệ với chúng tôi theo thông tin dưới đây:" +
                    "<br><br /><center><b>CÔNG TY TNHH Đồ Gia Dụng</b>" +
                    "<br><br />Địa chỉ: 363 Bình Trị Đông, Phường Bình Trị Đông A, Quận Bình Tân, TP Hồ Chí Minh" +
                    "<br><br />Website: https://dogiadung.vn </center>";

            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpPost]
        public ActionResult ForgotPassWord(string EmailID)
        {
            string message = "";
            bool status = false;
            using (DBGiaDungEntities db = new DBGiaDungEntities())
            {
                var account = db.USERs.Where(x => x.user_email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Gửi mail
                    
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.user_email, resetCode, "ResetPassword");
                    account.resetPasswordCode = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Đã gửi thư đến email " + EmailID + " vui lòng kiểm tra";
                }
                else
                {
                    message = Singleton.NotFoundAccount();
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            var user = db.USERs.Where(a => a.resetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (DBGiaDungEntities db = new DBGiaDungEntities())
                {
                    var user = db.USERs.Where(a => a.resetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = /*Crypto.Hash(model.NewPassword);*/ model.NewPassword;
                        user.resetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Mật khẩu tạo mới thành công";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult History(int? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var checemail = Session["user_email"];
            var toHistory = db.TRANSACTIONs.Where(s => s.user_email == checemail.ToString()).ToList();
            return View(toHistory.ToPagedList(pageNum, pageSize));
        }
        public ActionResult DetailHis(int id)
        {  
            return View(db.TRANSACTIONs.Where(x => x.transaction_id == id).FirstOrDefault());
        }
        public ActionResult YeuCauHuyDon(int id)
        {
            return View(db.TRANSACTIONs.Where(s => s.transaction_id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult YeuCauHuyDon(int id, TRANSACTION tRANSACTION)
        {
            try
            {
                tRANSACTION = db.TRANSACTIONs.Where(s => s.transaction_id == id).FirstOrDefault();
                tRANSACTION.transaction_status = -1;
                db.Entry(tRANSACTION).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("History");
            }
            catch
            {
                return Content("This data is using in other table, ERROR DELETE");
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
