namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class PromotionDao
    {
        private Database database = DatabaseFactory.CreateDatabase();
        /*
        public int AddPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Promotions(Name, PromoteType, Condition, DiscountValue, StartDate, EndDate, Description) VALUES(@Name, @PromoteType, @Condition, @DiscountValue, @StartDate, @EndDate, @Description); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int) promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }
        */
        public bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM Hishop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
            foreach (int num in memberGrades)
            {
                builder.AppendFormat(" INSERT INTO Hishop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, num);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool AddPromotionProducts(int activityId, string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Hishop_PromotionProducts SELECT @ActivityId, ProductId FROM Hishop_Products WHERE ProductId IN ({0})", productIds) + " AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts)");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePromotion(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Promotions WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeletePromotionProducts(int activityId, int? productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId");
            if (productId.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND ProductId = {0}", productId.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool EditPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Promotions SET Name = @Name, PromoteType = @PromoteType, Condition = @Condition, DiscountValue = @DiscountValue, StartDate = @StartDate, EndDate = @EndDate, Description = @Description WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int)promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, promotion.ActivityId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int? GetActiveIdByProduct(int productId)
        {
            int? nullable = null;
            string commandText = string.Format("SELECT ActivityId FROM Hishop_PromotionProducts WHERE ProductId = {0}", productId);
            object obj2 = this.database.ExecuteScalar(CommandType.Text, commandText);
            if (obj2 != null)
            {
                nullable = new int?((int)obj2);
            }
            return nullable;
        }

        public PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
        {
            string query = string.Empty;
            if (type == "Next")
            {
                query = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId<@activityId AND PromoteType=@PromoteType  ORDER BY activityId DESC";
            }
            else
            {
                query = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId>@activityId AND PromoteType=@PromoteType ORDER BY activityId ASC";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "activityId", DbType.Int32, promote.ActivityId);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, Convert.ToInt32(promote.PromoteType));
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
                reader.Close();
            }
            return info;
        }

        public DataTable GetHasPromotionsPromotionTypes()
        {
            string query = "SELECT PromoteType FROM Hishop_Promotions GROUP BY PromoteType";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public DataTable GetPromotes(Pagination pagination, int promotionType, out int totalPromotes)
        {
            string query = string.Format("SELECT COUNT(*) FROM Hishop_Promotions WHERE 1=1 ", new object[0]);
            if (promotionType != 0)
            {
                query = query + string.Format(" AND PromoteType={0} ", promotionType);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            totalPromotes = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str2 = string.Empty;
            StringBuilder builder = new StringBuilder("case Hishop_Promotions.PromoteType");
            builder.AppendFormat(" when 1 then '商品直接打折'", new object[0]);
            builder.AppendFormat(" when 2 then '商品固定金额出售'", new object[0]);
            builder.AppendFormat(" when 3 then '商品减价优惠'", new object[0]);
            builder.AppendFormat(" when 4 then '批发打折'", new object[0]);
            builder.AppendFormat(" when 5 then '买商品赠送礼品'", new object[0]);
            builder.AppendFormat(" when 6 then '商品有买有送'", new object[0]);
            builder.AppendFormat(" when 11 then '订单满额打折'", new object[0]);
            builder.AppendFormat(" when 12 then '订单满额优惠金额'", new object[0]);
            builder.AppendFormat(" when 13 then '混合批发打折'", new object[0]);
            builder.AppendFormat(" when 14 then '混合批发优惠金额'", new object[0]);
            builder.AppendFormat(" when 15 then '订单满额送礼品'", new object[0]);
            builder.AppendFormat(" when 16 then '订单满额送倍数积分'", new object[0]);
            builder.AppendFormat(" when 17 then '订单满额免运费'", new object[0]);
            builder.Append(" end as PromoteTypeName");
            if (pagination.PageIndex == 1)
            {
                str2 = "SELECT TOP 10 *," + builder + " FROM Hishop_Promotions WHERE 1=1 ";
            }
            else
            {
                str2 = string.Format("SELECT TOP {0} *," + builder + " FROM Hishop_Promotions WHERE ActivityId NOT IN (SELECT TOP {1} ActivityId FROM Hishop_Promotions) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            if (promotionType != 0)
            {
                str2 = str2 + string.Format(" AND PromoteType={0} ", promotionType);
            }
            str2 = str2 + " ORDER BY ActivityId DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str2);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public PromotionInfo GetPromotion(int activityId)
        {
            PromotionInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId = @ActivityId; SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.MemberGradeIds.Add((int)reader["GradeId"]);
                }
            }
            return info;
        }

        public DataTable GetPromotionProducts(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_BrowseProductList WHERE ProductId IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId) ORDER BY DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetPromotions(bool isProductPromote, bool isWholesale)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions");
            if (isProductPromote)
            {
                if (isWholesale)
                {
                    sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType = {0}", 4);
                }
                else
                {
                    sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType <> {0} AND PromoteType < 10", 4);
                }
            }
            else if (isWholesale)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType = {0} OR PromoteType = {1}", 13, 14);
            }
            else
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10", 13, 14);
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + " ORDER BY ActivityId DESC";
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /*
        public PromotionInfo GetReducedPromotion(Member member, decimal amount, int quantity, out decimal reducedAmount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType BETWEEN 11 AND 14 AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            IList<PromotionInfo> list = new List<PromotionInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePromote(reader));
                }
            }
            PromotionInfo info = null;
            reducedAmount = 0M;
            foreach (PromotionInfo info2 in list)
            {
                switch (info2.PromoteType)
                {
                    case PromoteType.FullAmountDiscount:
                        if ((amount >= info2.Condition) && ((amount - (amount * info2.DiscountValue)) > reducedAmount))
                        {
                            reducedAmount = amount - (amount * info2.DiscountValue);
                            info = info2;
                        }
                        break;

                    case PromoteType.FullAmountReduced:
                        if ((amount >= info2.Condition) && (info2.DiscountValue > reducedAmount))
                        {
                            reducedAmount = info2.DiscountValue;
                            info = info2;
                        }
                        break;

                    case PromoteType.FullQuantityDiscount:
                        if ((quantity >= ((int) info2.Condition)) && ((amount - (amount * info2.DiscountValue)) > reducedAmount))
                        {
                            reducedAmount = amount - (amount * info2.DiscountValue);
                            info = info2;
                        }
                        break;

                    case PromoteType.FullQuantityReduced:
                        if ((quantity >= ((int) info2.Condition)) && (info2.DiscountValue > reducedAmount))
                        {
                            reducedAmount = info2.DiscountValue;
                            info = info2;
                        }
                        break;
                }
            }
            return info;
        }

        public PromotionInfo GetSendPromotion(Member member, decimal amount, PromoteType promoteType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND Condition <= @Condition AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId) ORDER BY DiscountValue DESC");
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int) promoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, amount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
            }
            return info;
        }
         */

        public bool AddWKM(WKMInfo info)
        {
            string WKMCommand = "insert into WKM_Main (id,TitleDescription,ShareTitle,ShareDescription,StartDate,EndDate)values(@id,@TitleDescription,@ShareTitle,@ShareDescription,@StartDate,@EndDate);";
            string subjectCommand = string.Empty;
            string optionCommand = string.Empty;
            string command = string.Empty;
            for (int i = 0; i < info.SubjectInfo.WKMSubjectId.Count; i++)
            {
                //插入题目内容
                subjectCommand += string.Format("insert into WKM_Subject (id,ActivityId,SubjectContent,imgUrl) values('{0}','{1}','{2}','{3}');", info.SubjectInfo.WKMSubjectId[i], info.SubjectInfo.ActivityId[i], info.SubjectInfo.SubjectContent[i], info.SubjectInfo.ImgUrl[i]);
                //插入该题目的答案选项内容
                for (int j = 0; j < info.OptionsInfo[i].WKMOptionId.Count; j++)
                {
                    optionCommand += string.Format("insert into WKM_Options (id,TitleId,OptionContent) values('{0}','{1}','{2}');", info.OptionsInfo[i].WKMOptionId[j], info.OptionsInfo[i].TitleId[j], info.OptionsInfo[i].OptionContent[j]);
                }
            }
            command = (WKMCommand + subjectCommand + optionCommand).TrimEnd(';');//去掉末尾;

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(command);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Guid, info.WKMId);
            this.database.AddInParameter(sqlStringCommand, "TitleDescription", DbType.String, info.TitleDescription);
            this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, info.ShareTitle);
            this.database.AddInParameter(sqlStringCommand, "ShareDescription", DbType.String, info.ShareDescription);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, info.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, info.EndDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateWKM(WKMInfo info)
        {
            string WKMCommand = "update WKM_Main set TitleDescription=@TitleDescription,ShareTitle=@ShareTitle,ShareDescription=@ShareDescription,StartDate=@StartDate,EndDate=@EndDate where id=@id;";
            string subjectCommand = string.Empty;
            string optionCommand = string.Empty;
            string command = string.Empty;
            for (int i = 0; i < info.SubjectInfo.WKMSubjectId.Count; i++)
            {
                //插入题目内容
                if (isSbjExist(info.SubjectInfo.WKMSubjectId[i]))
                    subjectCommand += string.Format("update WKM_Subject set SubjectContent='{0}',imgUrl='{1}' where Id='{2}';", info.SubjectInfo.SubjectContent[i], info.SubjectInfo.ImgUrl[i], info.SubjectInfo.WKMSubjectId[i]);
                else
                    subjectCommand += string.Format("insert into WKM_Subject (id,ActivityId,SubjectContent,imgUrl) values('{0}','{1}','{2}','{3}');", info.SubjectInfo.WKMSubjectId[i], info.SubjectInfo.ActivityId[i], info.SubjectInfo.SubjectContent[i], info.SubjectInfo.ImgUrl[i]);
                //插入该题目的答案选项内容
                for (int j = 0; j < info.OptionsInfo[i].WKMOptionId.Count; j++)
                {
                    if (isOptExist(info.OptionsInfo[i].WKMOptionId[j]))
                        optionCommand += string.Format("update WKM_Options set OptionContent='{0}' where id='{1}';", info.OptionsInfo[i].OptionContent[j], info.OptionsInfo[i].WKMOptionId[j]);
                    else
                        optionCommand += string.Format("insert into WKM_Options (id,TitleId,OptionContent) values('{0}','{1}','{2}');", info.OptionsInfo[i].WKMOptionId[j], info.OptionsInfo[i].TitleId[j], info.OptionsInfo[i].OptionContent[j]);
                }
            }
            command = (WKMCommand + subjectCommand + optionCommand).TrimEnd(';');//去掉末尾;

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(command);
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Guid, info.WKMId);
            this.database.AddInParameter(sqlStringCommand, "TitleDescription", DbType.String, info.TitleDescription);
            this.database.AddInParameter(sqlStringCommand, "ShareTitle", DbType.String, info.ShareTitle);
            this.database.AddInParameter(sqlStringCommand, "ShareDescription", DbType.String, info.ShareDescription);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, info.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, info.EndDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        public bool UpdateHishop_products(ModifyGoodsQuery query)
        {
            string updateSql = string.Format("Update Hishop_productsList set CommodityCode='{0}',CommoditySource='{1}' where CommodityID='{2}'",query.CommodityCode,query.CommoditySource,query.CommodityID);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(updateSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        } 
        private bool isSbjExist(Guid sbjId)
        {
            string command = "select COUNT(*)c from WKM_Subject where id='" + sbjId + "'";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) > 0;
            }
        }
        private bool isOptExist(Guid optId)
        {
            string command = "select COUNT(*)c from WKM_Options where id='" + optId + "'";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) > 0;
            }
        }
        public ModifyGoodsQuery GetHishop_productsListID(int CommodityID)
        {
            string query = string.Format("select * from Hishop_productsList where CommodityID='{0}'", CommodityID);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ModifyGoodsQuery>(reader);
            }
          
        }   
        public WKMInfo GetWKMInfo(Guid wkmId)
        {
            WKMInfo info = new WKMInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from WKM_Main where id=@id;select * from WKM_Subject where ActivityId=@id");
            this.database.AddInParameter(sqlStringCommand, "id", DbType.Guid, wkmId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                //活动主要信息
                if (reader.Read())
                {
                    info.WKMId = (Guid)reader["Id"];
                    info.TitleDescription = (string)reader["TitleDescription"];
                    info.ShareTitle = (string)reader["ShareTitle"];
                    info.ShareDescription = (string)reader["ShareDescription"];
                    info.StartDate = (reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : ((DateTime)reader["StartDate"]);
                    info.EndDate = (reader["EndDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["EndDate"]);

                }
                //活动题目内容
                reader.NextResult();
                int i = 0;
                while (reader.Read())
                {
                    info.SubjectInfo.WKMSubjectId.Add((Guid)reader["Id"]);
                    info.SubjectInfo.SubjectContent.Add((string)reader["SubjectContent"]);
                    info.SubjectInfo.ActivityId.Add((Guid)reader["ActivityId"]);
                    info.SubjectInfo.ImgUrl.Add(reader["imgUrl"] == DBNull.Value ? "" : (string)reader["imgUrl"]);
                    //活动题目答案内容
                    info.OptionsInfo.Add(GetWKMOptInfoBySubjectId((Guid)reader["Id"]));
                    i++;
                }
            }
            return info;
        }

        /// <summary>
        /// 根据活动id(wkmId)获取整个活动的所有问题的答案对象
        /// </summary>
        /// <returns></returns>
        public WKMOptionInfo GetWKMOptInfoByActivityId(Guid wkmId)
        {
            WKMOptionInfo info = new WKMOptionInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select WO.* from WKM_Options WO left join WKM_Subject WS on wo.TitleId=ws.id where ws.ActivityId = @activityId");
            this.database.AddInParameter(sqlStringCommand, "activityId", DbType.Guid, wkmId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info.WKMOptionId.Add((Guid)reader["Id"]);
                    info.OptionContent.Add((string)reader["OptionContent"]);
                    info.TitleId.Add((Guid)reader["TitleId"]);
                }
            }
            return info;
        }
        /// <summary>
        /// 根据问题id(sbjId)获取该问题的答案对象
        /// </summary>
        public WKMOptionInfo GetWKMOptInfoBySubjectId(Guid sbjId)
        {
            WKMOptionInfo info = new WKMOptionInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from WKM_Options where TitleId = @titleId");
            this.database.AddInParameter(sqlStringCommand, "titleId", DbType.Guid, sbjId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info.WKMOptionId.Add((Guid)reader["Id"]);
                    info.OptionContent.Add((string)reader["OptionContent"]);
                    info.TitleId.Add((Guid)reader["TitleId"]);
                }
            }
            return info;
        }
        /// <summary>
        /// 设置出题者详情
        /// </summary>
        /// <param name="hosterId">出题者id</param>
        /// <param name="sbjIds">题目id列表</param>
        /// <param name="optIds">答案id列表</param>
        /// <returns></returns>
        public bool SetHosterDetail(int hosterId, IList<string> sbjIds, IList<string> optIds, Guid activityId)
        {
            string command = string.Empty;
            for (int i = 0; i < sbjIds.Count; i++)
            {
                command += string.Format("insert into WKM_HostDetail (id,HosterId,TitleId,OptionId,ActivityId) values ('{0}',{1},'{2}','{3}','{4}');", Guid.NewGuid(), hosterId, sbjIds[i], optIds[i], activityId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(command);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0); ;
        }

        /// <summary>
        /// 答题者详情
        /// </summary>
        /// <param name="guestId">答题者id</param>
        /// <param name="sbjIds">题目id列表</param>
        /// <param name="optIds">答案id列表</param>
        /// <returns></returns>
        public bool SetGuestDetail(int guestId, int hosterId, IList<string> optIds, Guid activityId)
        {
            string command = string.Empty;
            for (int i = 0; i < optIds.Count; i++)
            {
                command += string.Format("insert into WKM_GuestDetail (id,GuestId,HosterId,OptionId,ActivityId) values ('{0}',{1},{2},'{3}','{4}');", Guid.NewGuid(), guestId, hosterId, optIds[i], activityId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(command);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0); ;
        }

        /// <summary>
        /// 获取hoster的guest匹配信息
        /// </summary>
        public DataTable GetGuestsDetail(int hosterId, int guestId, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from WKM_GuestDetail where HosterId={0} and GuestId={1} and ActivityId='{2}' order by id desc", hosterId, guestId, activityId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 获取出题者详情
        /// </summary>
        /// <param name="hosterId"></param>
        /// <returns></returns>
        public DataTable GetHosterDetail(int hosterId, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from WKM_HostDetail where HosterId={0} and ActivityId='{1}' ", hosterId, activityId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int getMatchRate(Guid activityId, int hosterId, int guestId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select matchrate from wkm_matchlist where activityid='{0}' and hosterid ={1}  and guestid ={2} ",  activityId,hosterId,guestId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["matchrate"]);
            }
            
        }

        /// <summary>
        /// 获取匹配度
        /// </summary>
        public int getMatchInfo(Guid activityId, int hosterId, int guestId)
        {
            int matchRate = -1;//匹配度
            //首先获取hoster的题目列表和答案列表信息
            DataTable dtHosterDetail = GetHosterDetail(hosterId, activityId);
            //再获取guest的题目列表和答案列表信息
            DataTable dtGuestDetail = GetGuestsDetail(hosterId, guestId, activityId);
            //计算匹配度
            dtHosterDetail.PrimaryKey = new DataColumn[] { dtHosterDetail.Columns["ID"] };
            int findCount = 0;//匹配的数量
            foreach (DataRow rowG in dtGuestDetail.Rows)
            {
                foreach (DataRow rowH in dtHosterDetail.Rows)
                {
                    if (rowG["OptionId"].ToString() == rowH["OptionId"].ToString())//如果找到了匹配的答案,
                    {
                        findCount++;
                    }
                }

            }

            matchRate = Convert.ToInt32((Convert.ToDouble(findCount) / dtGuestDetail.Rows.Count) * 100);// (答对数/题目总数)*100
            //首先判断是否已存在
            if (AddMatchInfo(hosterId, guestId, matchRate, activityId))
            {
                return matchRate;
            }
            else
            {
                return -1;
            }

        }
        /// <summary>
        /// 插入匹配度至结果表
        /// </summary>
        public bool AddMatchInfo(int hosterId, int guestId, int matchRate, Guid activityId)
        {
            string command = string.Empty;
            command = string.Format("insert into WKM_MatchList (id,HosterId,GuestId,MatchRate,ActivityId) values ('{0}',{1},{2},{3},'{4}')", Guid.NewGuid(), hosterId, guestId, matchRate, activityId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(command);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0); ;
        }

        /// <summary>
        /// 获取匹配度列表
        /// </summary>
        public DataTable GetMatchInfoList(int hosterId, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select WML.*,AM.UserName,AM.UserHead from WKM_MatchList WML left join aspnet_Members AM on wml.GuestId=AM.UserId where HosterId = {0} and ActivityId = '{1}'", hosterId, activityId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 根据匹配度获取匹配度描述
        /// </summary>
        public string GetMatchDescription(int matchRate, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select [description] from WKM_MatchInfo where activityId='{0}' and {1} between matchrateStart and MatchRateend", activityId, matchRate));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["description"].ToString();
            }
        }

        /// <summary>
        /// 判断出题者是否已经出题
        /// </summary>
        public bool isHosterExist(int hosterId, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select COUNT(*)as c from WKM_HostDetail where HosterId ={0} and ActivityId='{1}'", hosterId, activityId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) > 0;
            }
        }
        /// <summary>
        /// 判断答题者是否已经对hoster答过了题目
        /// </summary>
        public bool isGuestExist(int hosterId, int guestId, Guid activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select COUNT(*)c from WKM_GuestDetail where GuestId={0} and HosterId={1} and ActivityId='{2}'", guestId, hosterId, activityId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) > 0;
            }
        }

        /// <summary>
        /// 删除活动(联动删除)
        /// </summary>
        public bool DeleteWKMInfo(Guid activityId)
        {
            string sqlStringCommand = "delete from WKM_Options where TitleId in (select id from WKM_Subject where ActivityId='" + activityId + "');";
            sqlStringCommand += "delete from WKM_Subject where ActivityId='" + activityId + "';";
            sqlStringCommand += "delete from WKM_Main where ActivityId='" + activityId + "';";
            sqlStringCommand += "delete from WKM_GuestDetail where ActivityId='" + activityId + "';";
            sqlStringCommand += "delete from WKM_HostDetail where ActivityId='" + activityId + "';";
            sqlStringCommand += "delete from WKM_matchlist where ActivityId='" + activityId + "'";
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0); ;
        }

        /// <summary>
        /// 添加活动背景图片
        /// </summary>
        public bool addBackImgUrl(Guid activityId,string imgUrl)
        {
            string sqlStringCommand = string.Format("update WKM_Main set backImgUrl='{0}' where id='{1}'", imgUrl, activityId);
            return (this.database.ExecuteNonQuery(CommandType.Text, sqlStringCommand) > 0);
        }

        /// <summary>
        /// 添加活动logo图片
        /// </summary>
        public bool addLogoImgUrl(Guid activityId, string imgUrl)
        {
            string sqlStringCommand = string.Format("update WKM_Main set logoUrl='{0}' where id='{1}'", imgUrl, activityId);
            return (this.database.ExecuteNonQuery(CommandType.Text, sqlStringCommand) > 0);
        }

        /// <summary>
        /// 添加微信分享图标
        /// </summary>
        public bool addWxImgUrl(Guid activityId, string imgUrl)
        {
            string sqlStringCommand = string.Format("update WKM_Main set ShareImgUrl='{0}' where id='{1}'", imgUrl, activityId);
            return (this.database.ExecuteNonQuery(CommandType.Text, sqlStringCommand) > 0);
        }

        /// <summary>
        /// 获取背景图和logo图还有微信分享图标地址
        /// </summary>
        public DataTable getBackImgUrl(Guid activityId)
        {
            string sqlStringCommand = string.Format("select backImgUrl,logoUrl,ShareImgUrl from WKM_Main where id='{0}'", activityId);
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 添加广告图片
        /// </summary>
        public bool addAdImgUrl(Guid activityid, IList<string> imgUrlList,IList<string> adLinkList)
        {
            string command = string.Empty;
            for (int i = 0; i < imgUrlList.Count; i++)
            {
                command += string.Format("update WKM_Main set adImgUrl{0}='{1}',adLink{0}='{3}' where id='{2}' ;",i+1,imgUrlList[i],activityid,adLinkList[i]);
            }
            if (imgUrlList.Count > 0)
            {
                return (this.database.ExecuteNonQuery(CommandType.Text, command) > 0);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 获取广告图片和链接
        /// </summary>
        public DataTable getAdImgAndUrls(Guid activityId)
        {
            string command = string.Format("select adimgurl1,adlink1,adimgurl2,adlink2 from WKM_Main where id='{0}'",activityId);
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return ((DataTable)DataHelper.ConverDataReaderToDataTable(reader));
            }
        }
        ///// <summary>
        ///// 新增匹配度描述
        ///// </summary>
        //public bool addMatchInfoList(Guid activityId, IList<string> mStartList, IList<string> mEndList, IList<string> mDesList)
        //{
        //    string command = string.Empty;
        //    for (int i = 0; i < mStartList.Count; i++)
        //    {
        //        command += string.Format("insert into WKM_MatchInfo(id,MatchRateStart,MatchRateEnd,[Description],ActivityId) values('{0}',{1},{2},'{3}','{4}') ;", Guid.NewGuid(), mStartList[i],mEndList[i],mDesList[i],activityId);
        //    }
        //    return (this.database.ExecuteNonQuery(CommandType.Text, command) > 0);
        //}
        ///// <summary>
        ///// 修改匹配度描述
        ///// </summary>
        //public bool updateMatchInfoList(Guid activityId, IList<string> mStartList, IList<string> mEndList, IList<string> mDesList)
        //{
        //    string command = string.Empty;
        //    for (int i = 0; i < mStartList.Count; i++)
        //    {
        //        command += string.Format("update  WKM_MatchInfo set MatchRateStart={0},MatchRateEnd={1},[Description]='{2}' where ActivityId='{3}' ;",mStartList[i], mEndList[i], mDesList[i], activityId);
        //    }
        //    return (this.database.ExecuteNonQuery(CommandType.Text, command) > 0);
        //}

        ///// <summary>
        ///// 判断匹配度描述是否已经添加过
        ///// </summary>
        //public bool isMatchInfoListExist(Guid activityId)
        //{
        //    string command = ("select COUNT(*)c from WKM_MatchInfo where activityId='" + activityId + "'");
        //    using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
        //    {
        //        return Convert.ToInt32(((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["c"]) > 0;
        //    }
        //}
        /// <summary>
        /// 查询匹配度描述列表
        /// </summary>
        public DataTable getWKMMatchInfoList(Guid activityId)
        {
            string command = "select * from WKM_MatchInfo where activityId='" + activityId + "' order by MatchRateStart";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 设置copyright信息
        /// </summary>
        public bool setWKMCopyRight(string copyRight,Guid activityId)
        {
            string command = string.Format("update WKM_Main set CopyRight ='{0}' where id='{1}'",copyRight,activityId);
            return (this.database.ExecuteNonQuery(CommandType.Text, command) > 0);
        }

        /// <summary>
        /// 设置引导页url
        /// </summary>
        public bool setGuidePageUrl(string guidePageUrl, Guid activityId)
        {
            string command = string.Format("update WKM_Main set GuidePageUrl ='{0}' where id='{1}'", guidePageUrl, activityId);
            return (this.database.ExecuteNonQuery(CommandType.Text, command) > 0);
        }

        ///获取copyright信息
        public string getWKMCopyRight(Guid activityId)
        {
            string command = "select copyright from wkm_main where Id='" + activityId + "'";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return ((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["copyRight"].ToString();
            }
        }

        ///获取引导页url
        public string getGuidePageUrl(Guid activityId)
        {
            string command = "select GuidePageUrl from wkm_main where Id='" + activityId + "'";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return ((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["GuidePageUrl"].ToString();
            }
        }

        /*********微信问答活动end*********/

        /******** 签到送积分活动  *******/

        /// <summary>
        /// 设置签到活动规则
        /// </summary>
        public bool SetSignRule(string days, string points, int state)
        {
            string selectCommand = string.Format("select COUNT(*)c from yihui_signrule");
            string command = string.Empty;
            if (Convert.ToInt32(this.database.ExecuteScalar(CommandType.Text,selectCommand)) > 0)
            {
                command = string.Format("update YiHui_SignRule set needdays = '{0}',sendpoints = '{1}', State = {2} ", days, points,state);
            }
            else
            {
                command = string.Format("Insert Into YiHui_SignRule (SRID,NeedDays,SendPoints,State) values(NEWID(),'{0}','{1}',{2})", days, points,state);
            }
            return this.database.ExecuteNonQuery(CommandType.Text,command) > 0;
        }
        /// <summary>
        /// 获取签到奖励积分规则
        /// </summary>
        public DataTable GetSignRule()
        {
            string command = "select * from yihui_signrule";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 判断用户当天是否已经签到
        /// </summary>
        public bool IsTodaySigned(int userId)
        {
            string command = string.Format("Select COUNT(*)c from yihui_signInfo where userid = {0} and convert(varchar(10),SignTime,120) = convert(varchar(10),getdate(),120)",userId);
            return Convert.ToInt32(this.database.ExecuteScalar(CommandType.Text, command)) > 0;
        }

        /// <summary>
        /// 签到操作
        /// </summary>
        public bool GoSignToday(int userId)
        {
            string command = string.Format("Insert Into yihui_signInfo (Id,UserId,SignTime) values (NEWID(),{0},getdate())",userId);
            return this.database.ExecuteNonQuery(CommandType.Text,command) > 0;
        }

        /// <summary>
        /// 获取用户当前月份的签到信息
        /// </summary>
        public DataTable GetUserSignInfo(int userId)
        {
            string command = "select * from yihui_signInfo where userid=" + userId + " and month(SignTime) = month(getdate())";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetAllSignTime(int userId)
        {
            string command = "select SignTime from yihui_signInfo where userid = " + userId + " order by SignTime desc";
            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, command))
            {
                return (DataTable)DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /********签到送积分活动end*******/
    }
}

