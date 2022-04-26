using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Decorater
{
    public class Decoooo : Decooo
    {
        public Decoooo(IDeco custom) : base(custom){}
        public virtual string getDec()
        {
            return base.getDec() + " successfully created!";
        }
    }
}