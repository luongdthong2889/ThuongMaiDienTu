using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoDB2.Controllers.TemplateMethod
{
    public abstract class TemplateMethod
    {
        public abstract string CheckTemp();
        public string Trave()
        {
            return this.CheckTemp();
        }
    }
}