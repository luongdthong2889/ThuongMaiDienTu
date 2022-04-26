using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Controllers.Strategy
{
    public class StrategyY
    {
        Strategy strategy;
        public StrategyY()
        {
            this.strategy = new StrategyInit();
        }
        public string QuenPass()
        {
            return strategy.QuenMatKhau();
        }
    }
}