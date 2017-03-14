using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Promotions
{
    public class CouponsAct
    {
        public int ID { get; set; }
        public int CouponsID { get; set; }
        public string BgImg { get; set; }
        public DateTime CreateTime { get; set; }
        public int ColValue1 { get; set; }
        public string ColValue2 { get; set; }
    }
}
