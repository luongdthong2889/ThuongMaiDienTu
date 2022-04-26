using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Strategy
{
    public class StrategyInit : Strategy
    {
        public override string QuenMatKhau()
        {
            return "Quên mật khẩu tài khoản";
        }
    }
}