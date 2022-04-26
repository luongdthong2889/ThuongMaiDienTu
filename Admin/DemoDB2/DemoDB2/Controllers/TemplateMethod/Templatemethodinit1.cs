using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoDB2.Controllers.TemplateMethod
{
    public class TempletemethodInit1 : TemplateMethod
    {
        public TempletemethodInit1() { }
        public override string CheckTemp()
        {
            return "Sai thông tin";
        }
    }
}