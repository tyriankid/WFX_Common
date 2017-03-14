namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class MessageDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool Create(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate, ClientUserId, AgentName) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate, @ClientUserId, @AgentName)");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, manager.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, manager.ClientUserId);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateAgent(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate, ClientUserId, AgentName, [State]) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate, @ClientUserId, @AgentName, @State)");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, manager.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.Int32, manager.ClientUserId);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, manager.State);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public ManagerInfo GetManager(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        public ManagerInfo GetManager(string userName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserName = @UserName");
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        public DbQueryResult GetManagers(ManagerQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            if (query.RoleId != 0)
            {
                builder.AppendFormat(" AND RoleId = {0}", query.RoleId);
            }
            if (GetManager(Globals.GetCurrentManagerUserId()).UserName.ToLower() != "yihui") {
                builder.AppendFormat(" AND UserName != 'yihui'");
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Managers", "UserId", builder.ToString(), "*");
        }

        /// <summary>
        /// 获取所有后台manager
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllManagers()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select UserId,UserName,RoleId,AgentName,ClientUserId from aspnet_Managers");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        public DataTable GetHishop_Product()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Products");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool Update(ManagerInfo manager)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Managers SET RoleId = @RoleId, UserName = @UserName, Password = @Password, Email = @Email, AgentName = @AgentName, [State] = @State WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, manager.UserId);
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, manager.RoleId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, manager.UserName);
            this.database.AddInParameter(sqlStringCommand, "Password", DbType.String, manager.Password);
            this.database.AddInParameter(sqlStringCommand, "Email", DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "AgentName", DbType.String, manager.AgentName);
            this.database.AddInParameter(sqlStringCommand, "State", DbType.Int32, manager.State);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 获取当前管理员的所属区域
        /// </summary>
        /// <param name="userId">管理员id</param>
        /// <returns></returns>
        public DataTable GetManagerArea(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from erp_managersregion where userid = "+userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
            //StringBuilder builder = new StringBuilder();
            //builder.Append("select * from erp_managersregion where userid = " + userId);
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            //return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 判断前台微信用户是否为代理商
        /// </summary>
        /// <param name="userId">前端用户ID</param>
        /// <returns></returns>
        public bool ExistClientUserId(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Managers WHERE ClientUserId = @ClientUserId");
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.String, userId);
            return (((int)this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        /// <summary>
        /// 根据前端用户Id得到管理端用户对象（是否是代理商）
        /// </summary>
        /// <param name="clientuserId">前端用户Id</param>
        /// <returns>管理端用户对象</returns>
        public ManagerInfo GetManagerByClientUserId(int clientuserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE ClientUserId = @ClientUserId");
            this.database.AddInParameter(sqlStringCommand, "ClientUserId", DbType.String, clientuserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ManagerInfo>(reader);
            }
        }

        /// <summary>
        /// 后台账号打卡
        /// </summary>
        public bool DutyOn(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into YiHui_DutyShift (ID,ManagerId,DutyDate,LoginTime,DutyState) values (@ID,@ManagerId,@DutyDate,@LoginTime,1)");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Guid, Guid.NewGuid());
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            this.database.AddInParameter(sqlStringCommand, "DutyDate", DbType.DateTime, DateTime.Today);
            this.database.AddInParameter(sqlStringCommand, "LoginTime", DbType.DateTime, DateTime.Now);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 后台账号关班
        /// </summary>
        public bool DutyOff(int managerId)
        {
            string dutyOffSql = @"declare @LoginTime datetime
                        set @LoginTime  = (select LoginTime from Yihui_DutyShift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23))
                        update YiHui_DutyShift set LoginOutTime=getdate(),OrdersCount=(select COUNT(*) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus=5),
                        OrdersTotal = (select ISNULL(sum(OrderTotal),0) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus=5),DutyState=2
                        where managerId = @ManagerId and DutyDate=Convert(varchar,getdate(),23)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(dutyOffSql);
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        /// <summary>
        /// 关班重复判断
        /// </summary>
        public bool isDutyOffExist(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*)c from yihui_dutyshift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23) and DutyState=2");
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt16(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) == 1;
            }
        }

        /// <summary>
        /// 打卡重复判断
        /// </summary>
        public bool isDutyExist(int managerId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select COUNT(*)c from yihui_dutyshift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23)");
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt16(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) == 1;
            }
        }
        /// <summary>
        /// 获取当天打卡信息
        /// </summary>
        public DataTable GetDutyInfo(int managerId)
        {
            string sql = @"declare @LoginTime datetime
                        set @LoginTime  = (select LoginTime from Yihui_DutyShift where managerId=@ManagerId and DutyDate=Convert(varchar,getdate(),23))
                        select id,ManagerId,DutyDate,LoginTime,
                        (select COUNT(*) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus = 5) as orderCount,
                        (select sum(OrderTotal) from Hishop_Orders where Sender = @ManagerId and OrderDate>@LoginTime and OrderStatus = 5) as orderTotal
                        from yihui_DutyShift where managerId = @ManagerId and DutyDate=Convert(varchar,getdate(),23) ";
            DataTable dtDutyFull = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            this.database.AddInParameter(sqlStringCommand, "ManagerId", DbType.Int16, managerId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取后台账号的交接班列表
        /// </summary>
        public DataTable GetDutyInfoList(DateTime? dateStart, DateTime? dateEnd,int managerId=0)
        {
            string sql = "Select ID,am.UserName,DutyDate,LoginTime,LoginOutTime,OrdersCount,OrdersTotal From YiHui_DutyShift YD left join aspnet_Managers AM on YD.managerId = am.UserId Where YD.DutyState = 2 ";
            string sqlWhere = " and DutyDate = Convert(varchar,getdate(),23) ";
            if (dateStart != null)
            {
                sqlWhere = string.Format(" and DutyDate >= '{0}' ", dateStart);
            }
            if (dateEnd != null)
            {
                sqlWhere = string.Format(" and DutyDate <= '{0}' ", dateEnd);
            }
            if (dateStart != null && dateEnd != null)
            {
                sqlWhere = string.Format(" and DutyDate between '{0}' and '{1}' ", dateStart, dateEnd);
            }
            if (managerId != 0)
            {
                sqlWhere += string.Format(" and managerId = {0}",managerId);
            }
            sql += sqlWhere;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }

        }

        /// <summary>
        /// 爽爽挝啡pc点餐系统生成的订单根据sender获取后台点餐账号对应的店铺名
        /// </summary>
        public string getPcOrderStorenameBySender(int sender)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select storename from aspnet_Distributors where UserId = (select ClientUserId from aspnet_Managers where UserId="+sender+")");
            object returnStr = this.database.ExecuteScalar(sqlStringCommand);
            return returnStr == null ? "店内" : returnStr.ToString();
        }

        public string getPcOrderStorenameByClientuserid(int clientuserid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select storename from aspnet_Distributors where UserId = "+clientuserid);
            object returnStr = this.database.ExecuteScalar(sqlStringCommand);
            return returnStr == null ? "店内" : returnStr.ToString();
        }


        

        public int getSenderIdByClientUserId(int clientuserid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select userid from aspnet_Managers where ClientUserId = "+clientuserid);
            object returnStr = this.database.ExecuteScalar(sqlStringCommand);
            return returnStr.ToInt();
        }

        public int getClientUserIdBySenderId(int senderid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ClientUserId from aspnet_Managers where UserId = "+senderid);
            object returnStr = this.database.ExecuteScalar(sqlStringCommand);
            return returnStr.ToInt();
        }

        /// <summary>
        /// 根据门店clientuserid增加1的粉丝数
        /// </summary>
        /// <param name="clientuserid"></param>
        /// <returns></returns>
        public bool addStoreFansCount(int clientuserid,int userid,int subscribetype)
        {
            int canAdd = this.canAddFansCount(userid);
            if (subscribetype==0 || (canAdd==2 &&subscribetype==1))//无论用户有没有记录,只要是关注,都允许插入
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into aspnet_storefans (subscribetime,storeid,subscribetype,userid ) values (convert(varchar(19),getdate(),120),@storeid,@subscribetype,@userid)");
                this.database.AddInParameter(sqlStringCommand, "storeid", DbType.Int16, subscribetype == 1 ? getFansStoreId(userid) : clientuserid);
                this.database.AddInParameter(sqlStringCommand, "subscribetype", DbType.Int16, subscribetype);
                this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int16, userid);
                return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            else //其余情况不允许插入,: 当用户取消关注时,有数据,但最新数据是取消关注时,或,没数据时
            {
                return false;
            }

        }

        /// <summary>
        /// 判断用户是否处于取消关注状态
        /// </summary>
        public int canAddFansCount(int userid)
        {
            //首先查该用户是否已经是某个店的粉丝,(按取消关注时间排倒叙,取第一个值,如果是取消关注类型,下面代码继续执行,否则不允许重复插入
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 storeid,subscribetype from aspnet_storefans where userid = @userid order by subscribetime desc");
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int16, userid);
            DataTable rstTable = new DataTable();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                rstTable = (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
            if (rstTable.Rows.Count == 0)//没有记录 0
            {
                return 0;
            }
            else if (rstTable.Rows[0]["subscribetype"].ToString() == "1") //有记录,最新状态式取消关注 1
            {
                return 1;
            }
            else //有记录,最新状态是关注 2
            {
                return 2;
            }
        }

        private int getFansStoreId(int userid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 storeid from aspnet_storefans where subscribetype=0 and userid = @userid order by subscribetime desc");
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int16, userid);
            DataTable rstTable = new DataTable();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return  DataHelper.ConverDataReaderToDataTable(reader).Rows[0]["storeid"].ToInt();
            }
        }

        /// <summary>
        /// 通过门店id获取门店名
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public string GetStoreName(int storeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select storename from aspnet_distributors where userid = "+storeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                
                DataTable dtResult = DataHelper.ConverDataReaderToDataTable(reader);
                if (dtResult.Rows.Count > 0)
                {
                    return dtResult.Rows[0]["storename"].ToString();
                }
                else
                {
                    return "总店";
                }
                
            }
        }

        public DataSet getStoreFans(DateTime? startTime, DateTime? endTime, int storeId)
        {
            /*
             --新关注
                select COUNT(*) from aspnet_storeFans where SubscribeType = 0
                and SubscribeTime >= starttime and SubscribeTime <= endtime and storeid = 1

                --取消关注
                select COUNT(*) from aspnet_storeFans where SubscribeType = 1
                and SubscribeTime >= starttime and SubscribeTime <= endtime and storeid = 1
                --净增关注人数
                select (select COUNT(*) from aspnet_storeFans where SubscribeType = 0 and SubscribeTime >= starttime and SubscribeTime <= endtime and storeid = 1) 
                - (select COUNT(*) from aspnet_storeFans where SubscribeType = 1 and SubscribeTime >= starttime and SubscribeTime <= endtime and storeid = 1)

                --累计关注人数(截止于endtime,没有starttime的条件)
                select (select COUNT(*) from aspnet_storeFans where SubscribeType = 0 and SubscribeTime <= endtime and storeid = 1) - 
                (select COUNT(*) from aspnet_storeFans where SubscribeType = 1 and SubscribeTime <= endtime and storeid = 1)
 
             */
            IList<DateTime?> dateList = new List<DateTime?>();
            DateTime sTime = DateTime.Parse( startTime.ToString());
            DateTime eTime = DateTime.Parse( endTime.ToString());


            DataSet ds = new DataSet();

            for (int i = 0; i <= eTime.Subtract(sTime).Days; i++)
            {
                DateTime timeSearch = sTime.AddDays(i);

                string startTimeWhere = string.Format(" and datediff(dd,'{0}',SubscribeTime)>=0 ", timeSearch);
                string endTimeWhere = string.Format(" and datediff(dd,'{0}',SubscribeTime)<=0 ", timeSearch);

                string date = string.Format(" ( select '{0}' ) as dateNow ",timeSearch.ToString( "yyyy-MM-dd "));
                string newSubSql = " ( select COUNT(*) from aspnet_storeFans where storeid = {0}  and SubscribeType = 0  {1} ) as newSubNum ";
                string unSubSql = " ( select COUNT(*) from aspnet_storeFans where storeid = {0}  and SubscribeType = 1 {1}  ) as unSubNum";
                string realSubSql = "( select (select COUNT(*) from aspnet_storeFans where SubscribeType = 0 and storeid = {0} {1}) ";
                realSubSql += " - (select COUNT(*) from aspnet_storeFans where SubscribeType = 1 and storeid = {0} {1} ) ) as realSubSql";
                string totalSubSql = " ( select (select COUNT(*) from aspnet_storeFans where SubscribeType = 0 and storeid = {0} {1} ) - ";
                totalSubSql += " (select COUNT(*) from aspnet_storeFans where SubscribeType = 1  and storeid = {0} {1} ) ) as totalSubSql";


                //string sql = "select * from aspnet_storeFans where  storeid = " + storeId;
                if (startTime != null && endTime != null)
                {
                    newSubSql = string.Format(newSubSql, storeId, startTimeWhere + endTimeWhere);
                    unSubSql = string.Format(unSubSql, storeId, startTimeWhere + endTimeWhere);
                    realSubSql = string.Format(realSubSql, storeId, startTimeWhere + endTimeWhere);
                    totalSubSql = string.Format(totalSubSql, storeId, endTimeWhere);
                }

                string sqlAllTables = "select " + date  + "," + newSubSql + "," + unSubSql + "," + realSubSql + "," + totalSubSql;
                DbCommand command = this.database.GetSqlStringCommand(sqlAllTables);

                using (IDataReader reader = this.database.ExecuteReader(command))
                {
                    ds.Tables.Add(DataHelper.ConverDataReaderToDataTable(reader));
                }
            }

            return ds;
        }

    }
}

