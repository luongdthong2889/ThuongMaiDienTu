using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Decorater
{
    public class Decoo : IDeco
    {
        public virtual string getDec()
        {
            return "Your account is";
        }
    }
}