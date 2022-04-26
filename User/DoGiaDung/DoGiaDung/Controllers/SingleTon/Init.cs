using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.SingleTon
{
    public sealed class Init
    {
        private Init() { }
        private static Init instance = null;
        public static Init Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Init();
                }
                return instance;
            }
        }
        public string NotFoundAccount()
        {
            return "Không tìm thấy tài khoản";
        }
    }
}