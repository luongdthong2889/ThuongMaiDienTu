using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Facade
{
    public class FacdeController
    {
        FacadeSubController facadesub;
        public FacdeController()
        {
            this.facadesub = new FacadeSubController();
        }
        public string SaiMatKhau()
        {
            return this.facadesub.getSubController();
        }
    }
}