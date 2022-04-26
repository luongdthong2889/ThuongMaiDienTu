using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.TemplateMethod
{
    public class TempletemethodInit2 : Templatemethod
    {
        public TempletemethodInit2() { }
        public override string CheckTemp()
        {
            return "Email đã tồn tại";
        }
    }
}