using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoDB2.Controllers.TemplateMethod
{
    public class templatemethodinit2:TemplateMethod
    {
        public templatemethodinit2() { }
        public override string CheckTemp()
        {
            return "parent không được < 0";
        }
    }
}