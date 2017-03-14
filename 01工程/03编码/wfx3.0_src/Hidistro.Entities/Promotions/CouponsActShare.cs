using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Promotions
{
    public class CouponsActShare
    {
        public int ID { get; set; }
        public int CouponsActID { get; set; }
        public int CouponsID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public DateTime ShareTime { get; set; }
        public int UseCount { get; set; }
    }
}
