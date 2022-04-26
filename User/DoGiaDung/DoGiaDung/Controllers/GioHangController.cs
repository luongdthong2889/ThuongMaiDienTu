using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DoGiaDung.Models;
using PayPal.Api;
using DemoVNPay.Others;


namespace DoGiaDung.Controllers
{
    public class GioHangController : Controller
    {
        DBGiaDungEntities db = new DBGiaDungEntities();
        private string strHang = "Cart";
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Payment()
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", "1000000"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        Session.Remove(strHang);
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }

        public ActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[strHang] == null)
            {
                List<Cart> lshang = new List<Cart>
                {
                    new Cart(db.PRODUCTs.Find(id),1)
                };
                Session[strHang] = lshang;
            }
            else
            {
                List<Cart> lshang = (List<Cart>)Session[strHang];
                int check = isexitstingCheck(id);
                if (check == -1)
                {
                    lshang.Add(new Cart(db.PRODUCTs.Find(id), 1));
                }
                else
                {
                    lshang[check].Quantity++;
                }
                Session[strHang] = lshang;
            }
            return View("Index");
        }
        private int isexitstingCheck(int? id)
        {
            List<Cart> lshang = (List<Cart>)Session[strHang];
            for (int i = 0; i < lshang.Count; i++)
            {
                if (lshang[i].Product.product_id == id) return i;
            }
            return -1;
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int check = isexitstingCheck(id);
            List<Cart> lshang = (List<Cart>)Session[strHang];
            lshang.RemoveAt(check);
            return View("Index");
        }
        public ActionResult Update_Cart(FormCollection frc)
        {
            string[] soLuongs = frc.GetValues("soluong");
            List<Cart> lshang = (List<Cart>)Session[strHang];
            for (int i = 0; i < lshang.Count; i++)
            {
                lshang[i].Quantity = Convert.ToInt32(soLuongs[i]);
            }
            Session[strHang] = lshang;
            return View("Index");
        }

        public ActionResult Checkout_view() 
        {
            return View();
        }
        public ActionResult Do_checkout(FormCollection frc) 
        {
            if (Session["user_email"] == null)
            {
                return RedirectToAction("Index","User");
            }
            else
            {
                var cusadd = frc["Cusaddress"];
                var cusmess = frc["Cusmess"];
                DateTime d = DateTime.Now;
                List<Cart> lshang = (List<Cart>)Session["Cart"];
                var userlogin = Session["user_email"];
                var usercurrent = db.USERs.Where(x => x.user_email == userlogin.ToString()).FirstOrDefault();
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                var total = lshang.Sum(x => x.Quantity * x.Product.price);
                TRANSACTION tRANSACTION = new TRANSACTION()
                {
                    user_id = usercurrent.user_id,
                    transaction_status = 1,
                    user_name = usercurrent.user_name,
                    user_email = usercurrent.user_email,
                    user_address = cusadd,
                    user_phone = usercurrent.user_phone,
                    message = cusmess,
                    transaction_created = d,
                    payment = 1,
                    payment_info = "COD",
                    security = finalString,
                    amount = total
                };
                db.TRANSACTIONs.Add(tRANSACTION);
                db.SaveChanges();
                foreach (Cart item in lshang)
                {
                    ORDER oRDER = new ORDER()
                    {
                        transaction_id = tRANSACTION.transaction_id,
                        product_id = item.Product.product_id,
                        quantity = item.Quantity,
                        amout = item.Quantity * item.Product.price,
                        order_status = 1
                    };
                    //var pro = db.PRODUCTs.Single(s => s.product_id == item.Product.product_id);
                    //if ((pro.quantity - item.Quantity) == 0)
                    //{
                    //    pro.quantity = 0;
                    //    pro.product_status = 2;
                    //}
                    //pro.quantity = pro.quantity - item.Quantity;
                    db.ORDERs.Add(oRDER);
                    db.SaveChanges();
                }
                
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/mail/neworder.html"));
                int c = Convert.ToInt32(total);

                content = content.Replace("{{CustomerName}}", usercurrent.user_name);
                content = content.Replace("{{Phone}}", usercurrent.user_phone);
                content = content.Replace("{{Email}}", usercurrent.user_email);
                content = content.Replace("{{DiaChi}}", usercurrent.user_address);
                content = content.Replace("{{Address}}", cusadd);
                content = content.Replace("{{Total}}", c.ToString("N0"));

                new MailHelper().SendMail(usercurrent.user_email, "Đơn hàng mới từ Dogiadung", content);

                lshang.Clear();
                return RedirectToAction("Success","GioHang");
            }
        }
        public ActionResult Success()
        {
            Session.Remove(strHang);
            return View();
        }
        public ActionResult Failure()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("Failure", "GioHang");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Failure", "GioHang");
            }
            //lưu vào db
            DateTime d = DateTime.Now;
            List<Cart> lshang = (List<Cart>)Session["Cart"];
            var userlogin = Session["user_email"];
            var usercurrent = db.USERs.Where(x => x.user_email == userlogin.ToString()).FirstOrDefault();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            var total = lshang.Sum(x => x.Quantity * x.Product.price);
            TRANSACTION tRANSACTION = new TRANSACTION()
            {
                user_id = usercurrent.user_id,
                transaction_status = 3,
                user_name = usercurrent.user_name,
                user_email = usercurrent.user_email,
                user_address = usercurrent.user_address,
                user_phone = usercurrent.user_phone,
                message = "Đã thanh toán PayPal",
                transaction_created = d,
                payment = 2,
                payment_info = "PayPal",
                security = finalString,
                amount = total
            };
            db.TRANSACTIONs.Add(tRANSACTION);
            db.SaveChanges();
            foreach (Cart item in lshang)
            {
                ORDER oRDER = new ORDER()
                {
                    transaction_id = tRANSACTION.transaction_id,
                    product_id = item.Product.product_id,
                    quantity = item.Quantity,
                    amout = item.Quantity * item.Product.price,
                    order_status = 1
                };
                //var pro = db.PRODUCTs.Single(s => s.product_id == item.Product.product_id);
                //if ((pro.quantity - item.Quantity) == 0)
                //{
                //    pro.quantity = 0;
                //    pro.product_status = 2;
                //}
                //pro.quantity = pro.quantity - item.Quantity;
                db.ORDERs.Add(oRDER);
                db.SaveChanges();
            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/mail/neworder.html"));
            int c = Convert.ToInt32(total);

            content = content.Replace("{{CustomerName}}", usercurrent.user_name);
            content = content.Replace("{{Phone}}", usercurrent.user_phone);
            content = content.Replace("{{Email}}", usercurrent.user_email);
            content = content.Replace("{{DiaChi}}", usercurrent.user_address);
            content = content.Replace("{{Address}}", usercurrent.user_address);
            content = content.Replace("{{Total}}", c.ToString("N0"));

            new MailHelper().SendMail(usercurrent.user_email, "Đơn hàng mới từ Dogiadung", content);
            //on successful payment, show success page to user.  
            return RedirectToAction("Success","GioHang");
            
            
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<Cart> lshang = (List<Cart>)Session["Cart"];
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            foreach (var x in lshang)
            {
                itemList.items.Add(new Item()
                {
                    name = x.Product.product_name,
                    currency = "USD",
                    price = x.Product.price.ToString(),
                    quantity = x.Quantity.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = lshang.Sum(x => x.Quantity * x.Product.price).ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}