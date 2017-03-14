using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Comments
{
    public class StoreMessage
    {
        public int ID { get; set; }
        public int DisUserID { get; set; }
        public int MsgUserID { get; set; }
        public string MessaegeCon { get; set; }
        public DateTime MsgTime { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string StoreName { get; set; }
        public string UserName { get; set; }
    }
}
