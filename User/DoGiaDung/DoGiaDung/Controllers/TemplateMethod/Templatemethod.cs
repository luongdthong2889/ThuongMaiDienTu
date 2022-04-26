using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.TemplateMethod
{
    public abstract class Templatemethod
    {
        public abstract string CheckTemp();
        public string Trave()
        {
            return this.CheckTemp();
        }
    }
}