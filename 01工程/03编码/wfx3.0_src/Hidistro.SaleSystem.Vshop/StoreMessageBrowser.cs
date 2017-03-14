using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SqlDal.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hidistro.SaleSystem.Vshop
{
    public static class StoreMessageBrowser
    {
        /// <summary>
        /// 新增留言
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool AddStoreMessage(StoreMessage msg)
        {
            return new StoreMessageDao().AddStoreMessage(msg);
        }
        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool DeleteStoreMessage(int ID) 
        {
            return new StoreMessageDao().DeleteStoreMessage(ID);
        }

        /// <summary>
        /// 得到实体类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static StoreMessage GetModel(int ID)
        {
            return new StoreMessageDao().GetModel(ID);
        }

        /// <summary>
        /// 根据条件查询留言数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DbQueryResult GetMsgList(StoreMessageQuery query)
        {
            return new StoreMessageDao().GetMsgList(query);
        }

        public static int DeleteStoreMessages(IList<int> MsgIDs)
        {
            if ((MsgIDs == null) || (MsgIDs.Count == 0))
            {
                return 0;
            }
            int num = 0;
            foreach (int num2 in MsgIDs)
            {
                new StoreMessageDao().DeleteStoreMessage(num2);
                num++;
            }
            return num;
        }

        public static DataTable GetMyMsg(int UserID)
        {
            return new StoreMessageDao().GetMyMsg(UserID);
        }
    }
}
