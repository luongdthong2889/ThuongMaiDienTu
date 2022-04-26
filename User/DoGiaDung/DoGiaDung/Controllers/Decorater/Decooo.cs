using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Decorater
{
    public abstract class Decooo : IDeco
    {
        IDeco decDeco;
        public Decooo(IDeco custom)
        {
            this.decDeco = custom;
        }
        public virtual string getDec()
        {
            return this.decDeco.getDec();
        }
    }
}