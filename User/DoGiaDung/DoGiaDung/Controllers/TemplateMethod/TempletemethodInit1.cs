using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.TemplateMethod
{
    public class TempletemethodInit1 : Templatemethod
    {
        public TempletemethodInit1() { }
        public override string CheckTemp()
        {
            return "Sai thông tin";
        }
    }
}