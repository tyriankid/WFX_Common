namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class SkuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public DataTable GetExpandAttributes(int productId)
        {
            DataTable table;
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
            builder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            DataTable table2 = new DataTable();
            if ((table != null) && (table.Rows.Count > 0))
            {
                table2 = table.Clone();
                foreach (DataRow row in table.Rows)
                {
                    bool flag = false;
                    if (table2.Rows.Count > 0)
                    {
                        foreach (DataRow row2 in table2.Rows)
                        {
                            if (((int) row2["AttributeId"]) == ((int) row["AttributeId"]))
                            {
                                DataRow row4;
                                flag = true;
                                (row4 = row2)["ValueStr"] = row4["ValueStr"] + ", " + row["ValueStr"];
                            }
                        }
                    }
                    if (!flag)
                    {
                        DataRow row3 = table2.NewRow();
                        row3["AttributeId"] = row["AttributeId"];
                        row3["AttributeName"] = row["AttributeName"];
                        row3["ValueStr"] = row["ValueStr"];
                        table2.Rows.Add(row3);
                    }
                }
            }
            return table2;
        }

        public SKUItem GetProductAndSku(MemberInfo currentMember, int productId, string options,bool isMemberPrice = true)//是否需要计算会员价
        {
            if (string.IsNullOrEmpty(options))
            {
                return null;
            }
            string[] strArray = options.Split(new char[] { ',' });
            if ((strArray == null) || (strArray.Length <= 0))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            if (currentMember != null)
            {
                int discount = new MemberGradeDao().GetMemberGrade(currentMember.GradeId).Discount;
                if (!isMemberPrice) { discount = 100; }//如果传递的参数为不需要计算会员价,折扣值为100,不打折.
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice,");
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", currentMember.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", currentMember.GradeId, discount);
                builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new char[] { ':' });
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", strArray2[0], strArray2[1]);
            }
            SKUItem item = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    item = DataMapper.PopulateSKU(reader);
                }
            }
            return item;
        }

        public SKUItem GetSkuItem(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            SKUItem item = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    item = DataMapper.PopulateSKU(reader);
                }
            }
            return item;
        }

        public DataTable GetSkus(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 得到所有商品规格信息，得到所有商品规格信息  -使用在代理商采购
        /// </summary>
        /// <returns></returns>
        public DataTable GetSkuItems()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(@"SELECT sku.SkuId,sku.ProductId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s 
                inner join Hishop_Attributes a on s.AttributeId = a.AttributeId 
                inner join Hishop_AttributeValues av on s.ValueId = av.ValueId 
                inner join Hishop_SKUs as sku on s.SkuId = sku.SkuId 
                ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据Skuid及用户Id（代理商）删除代理商订购列表
        /// </summary>
        /// <param name="skuids">skuId集合以'',''分割</param>
        /// <param name="userId">用户Id;后台用户Id</param>
        /// <returns></returns>
        public int DeleteAgentProduct(string skuids, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Erp_AgentProduct WHERE SkuId IN ({0}) and UserId = {1}", skuids, userId));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

    }
}

