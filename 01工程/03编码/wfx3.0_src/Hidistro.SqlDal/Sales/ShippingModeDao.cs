namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ShippingModeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Create");
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    this.database.ExecuteNonQuery(storedProcCommand, transaction);
                    flag = ((int)this.database.GetParameterValue(storedProcCommand, "Status")) == 0;
                    if (flag)
                    {
                        int parameterValue = (int)this.database.GetParameterValue(storedProcCommand, "ModeId");
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                        this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, parameterValue);
                        StringBuilder builder = new StringBuilder();
                        int num2 = 0;
                        builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                        foreach (string str in shippingMode.ExpressCompany)
                        {
                            builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num2, DbType.String, str);
                            num2++;
                        }
                        sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();
                        int num3 = (int)this.database.ExecuteScalar(sqlStringCommand, transaction);
                        if (num3 != 0)
                        {
                            throw new Exception("error");
                        }


                        transaction.Commit();

                        flag = true;
                    }
                    else
                    {
                        throw new Exception("error");
                    }

                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    //flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ShippingTemplates(TemplateName,Weight,AddWeight,Price,AddPrice) VALUES(@TemplateName,@Weight,@AddWeight,@Price,@AddPrice);SELECT @@Identity");
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, shippingMode.Weight);
            if (shippingMode.AddWeight.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, shippingMode.AddWeight);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, 0);
            }
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            if (shippingMode.AddPrice.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, 0);
            }
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    object obj2 = this.database.ExecuteScalar(sqlStringCommand, transaction);
                    int result = 0;
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        int.TryParse(obj2.ToString(), out result);
                        flag = result > 0;
                    }
                    if (flag)
                    {
                        DbCommand command = this.database.GetSqlStringCommand(" ");
                        this.database.AddInParameter(command, "TemplateId", DbType.Int32, result);
                        if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                        {
                            StringBuilder builder = new StringBuilder();
                            int num2 = 0;
                            int num3 = 0;
                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                            builder.Append(" DECLARE @GroupId Int;");
                            foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                            {
                                builder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num2).Append(",@AddPrice").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                this.database.AddInParameter(command, "Price" + num2, DbType.Currency, info.Price);
                                this.database.AddInParameter(command, "AddPrice" + num2, DbType.Currency, info.AddPrice);
                                builder.Append("Set @GroupId =@@identity;");
                                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                {
                                    builder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    this.database.AddInParameter(command, "RegionId" + num3, DbType.Int32, info2.RegionId);
                                    num3++;
                                }
                                num2++;
                            }
                            command.CommandText = builder.Append("SELECT @ERR;").ToString();
                            int num4 = (int)this.database.ExecuteScalar(command, transaction);
                            if (num4 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }

                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public bool DeleteShippingMode(int modeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_TemplateRelatedShipping Where ModeId=@ModeId;DELETE FROM Hishop_ShippingTypes Where ModeId=@ModeId;");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            this.database.AddOutParameter(sqlStringCommand, "Status", DbType.Int32, 4);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteShippingTemplate(int templateId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShippingTemplates Where TemplateId=@TemplateId");
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<string> GetExpressCompanysByMode(int modeId)
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        list.Add((string)reader["ExpressCompanyName"]);
                    }
                }
            }
            return list;
        }

        public DataTable GetShippingAllTemplates()
        {
            string query = "SELECT * FROM Hishop_ShippingTemplates ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId";
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_ShippingTypeGroups WHERE TemplateId = (SELECT TemplateId FROM Hishop_ShippingTypes WHERE ModeId =@ModeId )";
            }
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingMode(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        info.ExpressCompany.Add((string)reader["ExpressCompanyName"]);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShippingModeGroupInfo item = new ShippingModeGroupInfo
                    {
                        AddPrice = (decimal)reader["AddPrice"],
                        Price = (decimal)reader["Price"],
                        TemplateId = (int)reader["TemplateId"],
                        GroupId = (int)reader["GroupId"]
                    };
                    info.ModeGroup.Add(item);
                }
                string commandText = "";
                foreach (ShippingModeGroupInfo info3 in info.ModeGroup)
                {
                    commandText = "SELECT RegionId FROM Hishop_ShippingRegions WHERE GroupId = " + info3.GroupId;
                    using (IDataReader reader2 = this.database.ExecuteReader(CommandType.Text, commandText))
                    {
                        while (reader2.Read())
                        {
                            ShippingRegionInfo info4 = new ShippingRegionInfo
                            {
                                GroupId = info3.GroupId,
                                TemplateId = info3.TemplateId,
                                RegionId = (int)reader2["RegionId"]
                            };
                            info3.ModeRegions.Add(info4);
                        }
                    }
                }
            }
            return info;
        }

        public IList<ShippingModeInfo> GetShippingModes()
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTemplates Where TemplateId =@TemplateId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId =@TemplateId";
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
            }
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingTemplate(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    foreach (ShippingModeGroupInfo info2 in info.ModeGroup)
                    {
                        if (info2.GroupId == ((int)reader["GroupId"]))
                        {
                            info2.ModeRegions.Add(DataMapper.PopulateShippingRegion(reader));
                        }
                    }
                }
            }
            return info;
        }

        public DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Hishop_ShippingTemplates", "TemplateId", "", "*");
        }

        public void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ShippingTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public bool UpdateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Update");
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    this.database.ExecuteNonQuery(storedProcCommand, transaction);
                    flag = ((int)this.database.GetParameterValue(storedProcCommand, "Status")) == 0;
                    if (flag)
                    {
                        DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                        this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
                        StringBuilder builder = new StringBuilder();
                        int num = 0;
                        builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                        foreach (string str in shippingMode.ExpressCompany)
                        {
                            builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num, DbType.String, str);
                            num++;
                        }
                        sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();
                        int num2 = (int)this.database.ExecuteScalar(sqlStringCommand, transaction);
                        if (num2 != 0)
                        {
                            transaction.Rollback();
                            flag = false;
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("UPDATE Hishop_ShippingTemplates SET TemplateName=@TemplateName,Weight=@Weight,AddWeight=@AddWeight,Price=@Price,AddPrice=@AddPrice WHERE TemplateId=@TemplateId;").ToString());
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Currency, shippingMode.Weight);
            this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Currency, shippingMode.AddWeight);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    flag = this.database.ExecuteNonQuery(sqlStringCommand, transaction) > 0;
                    if (flag)
                    {
                        DbCommand command = this.database.GetSqlStringCommand(" ");
                        this.database.AddInParameter(command, "TemplateId", DbType.Int32, shippingMode.TemplateId);
                        StringBuilder builder2 = new StringBuilder();
                        int num = 0;
                        int num2 = 0;
                        builder2.Append("DELETE Hishop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
                        builder2.Append("DELETE Hishop_ShippingRegions WHERE TemplateId=@TemplateId;");
                        builder2.Append("DECLARE @ERR INT; Set @ERR =0;");
                        builder2.Append(" DECLARE @GroupId Int;");
                        if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                        {
                            foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                            {
                                builder2.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num).Append(",@AddPrice").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                this.database.AddInParameter(command, "Price" + num, DbType.Currency, info.Price);
                                this.database.AddInParameter(command, "AddPrice" + num, DbType.Currency, info.AddPrice);
                                builder2.Append("Set @GroupId =@@identity;");
                                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                {
                                    builder2.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    this.database.AddInParameter(command, "RegionId" + num2, DbType.Int32, info2.RegionId);
                                    num2++;
                                }
                                num++;
                            }
                        }
                        command.CommandText = builder2.Append("SELECT @ERR;").ToString();
                        int num3 = (int)this.database.ExecuteScalar(command, transaction);
                        if (num3 != 0)
                        {
                            transaction.Rollback();
                            flag = false;
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }
                    flag = false;
                }

                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        /// <summary>
        /// 获取所有街道信息
        /// </summary>
        public DbQueryResult GetStreetInfo(StreetInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" 1=1 ");
            //已经绑定了的街道不允许重复绑定

            if(query.isUserBind)
                builder.AppendFormat("and ys.StreetId not in (select StreetId from YiHui_DistributorRegion) ");
            if (query.streetName != null)
                builder.AppendFormat(" And ys.streetName LIKE '%{0}%'", DataHelper.CleanSearchString(query.streetName));
            if (query.regionCode != null)
                builder.AppendFormat(" AND regionCode = '{0}'", query.regionCode);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "YiHui_StreetInfo YS left join YiHui_DistributorRegion YDR on ys.StreetId = ydr.StreetId left join aspnet_Distributors AD on ydr.distributorId = ad.UserId left join aspnet_Members AM on ad.UserId=am.UserId", "ys.StreetId", builder.ToString(), "ys.*,ad.StoreName,am.UserName");
        }

        /// <summary>
        /// 获取所有的街道信息
        /// </summary>
        public DataTable GetStreetsInfo()
        {
            DbCommand command = this.database.GetSqlStringCommand("select *,isnull((select distributorId from YiHui_DistributorRegion where StreetId = YiHui_StreetInfo.StreetId),0)clientUserId from YiHui_StreetInfo");
            using (IDataReader reader = this.database.ExecuteReader(command))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据地区信息获取街道列表
        /// </summary>
        public DataTable GetStreetInfo(string regionCode)
        {
            string sql = string.Format("select * from yihui_streetinfo where RegionCode = '{0}'", regionCode);
            DbCommand sqlCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取所有街道所属的区域id
        /// </summary>
        public DataTable GetAllStreetRegionId()
        {
            string sql = string.Format("Select distinct er.ProvinceID,er.CityID,er.CountyID From YiHui_StreetInfo YS left join Erp_Regions ER on ys.RegionCode=er.CountyID");
            DbCommand sqlCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 添加街道信息
        /// </summary>
        public bool AddStreetInfo(string regionCode, string streetName)
        {
            DbCommand sqlCommand = this.database.GetSqlStringCommand(string.Format("insert into yihui_streetInfo (regioncode,streetname) values('{0}','{1}')", regionCode, streetName));
            return this.database.ExecuteNonQuery(sqlCommand) > 0;
        }
        /// <summary>
        /// 编辑街道信息
        /// </summary>
        public bool EditStreetInfo(string streetId, string streetName)
        {
            DbCommand sqlCommand = this.database.GetSqlStringCommand(string.Format("update yihui_streetInfo set streetname='{0}' where streetid='{1}'", streetName, streetId));
            return this.database.ExecuteNonQuery(sqlCommand) > 0;
        }
        /// <summary>
        /// 删除街道信息
        /// </summary>
        public bool DeleteStreetInfo(string streetId)
        {
            //联动删除
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Delete from yihui_streetInfo where streetid=@streetId;delete from YiHui_DistributorRegion where streetid=@streetId");
            this.database.AddInParameter(sqlStringCommand, "streetId", DbType.String, streetId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 增加分销商的配送范围
        /// </summary>
        public bool AddDistributorRegions(int distributorId, string streetIds)
        {
            string sql = string.Empty;
            IList<string> listStreetId = streetIds.Split(',');
            if (listStreetId.Count > 0)
            {
                for (int i = 0; i < listStreetId.Count; i++)
                {
                    sql += string.Format("insert into yihui_distributorregion (distributorid,regionid,streetid,regionname) values({0},(select regioncode from yihui_streetinfo where streetid='{1}'),'{1}',(select streetname from yihui_streetinfo where streetid='{1}'))", distributorId, listStreetId[i]);
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }
        /// <summary>
        /// 根据distributorid获取分销商的权限范围
        /// </summary>
        public DataTable GetDistributorRegions(int distributorId)
        {
            string sql = "select yd.*,ad.StoreName from yihui_distributorregion YD left join aspnet_Distributors AD on yd.distributorid=ad.UserId where distributorid=" + distributorId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据streetId获取配送范围包含此街道的分销商
        /// </summary>
        public DataTable GetDistributorRegions(string streetId)
        {
            string sql = "select yd.*,ad.StoreName from yihui_distributorregion YD left join aspnet_Distributors AD on yd.distributorid=ad.UserId where streetid='" + streetId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool DeleteDistributorRegions(int distributorId)
        {
            string sql = string.Format("delete from yihui_distributorregion where distributorId = {0}",distributorId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(sql);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

    }
}

