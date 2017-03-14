namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ShoppingCartDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// add by hj 20150918 增加礼品
        /// </summary>
        /// <param name="giftId"></param>
        /// <param name="quantity"></param>
        /// <param name="promotype"></param>
        /// <returns></returns>
        public bool AddGiftItem(MemberInfo member, int giftId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId ) UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId ; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType) VALUES (@UserId, @GiftId, @Quantity, @AddTime,0)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void AddLineItem(MemberInfo member, string skuId, int quantity, int categoryid)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryid);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public void AddLineItemPC(int pcUserid, string skuId, int quantity, int categoryid)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, pcUserid);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryid);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 清除礼物购物车
        /// </summary>
        /// <param name="userId"></param>
        public void ClearGiftShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetAllFull(int ActivitiesType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 获取购物车明细项信息
        /// </summary>
        public ShoppingCartItemInfo GetCartItemInfo(MemberInfo member, string skuId, int quantity,int pcUserid=0)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select sc.*,hs.CostPrice,hs.SalePrice from Hishop_ShoppingCarts as sc left join dbo.Hishop_SKUs as hs on sc.SkuId = hs.SkuId  where sc.UserId = {0} and sc.SkuId = '{1}' order by hs.SalePrice desc"
                , (member != null) ? member.UserId : pcUserid, skuId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            DataTable dtCart = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];

            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }

                info = new ShoppingCartItemInfo();
                info.SkuId = skuId;
                info.Quantity = info.ShippQuantity = quantity;
                //买一送一修改
                if (dtCart.Rows.Count > 0 && !string.IsNullOrEmpty(dtCart.Rows[0]["GiveQuantity"].ToString()))
                    info.GiveQuantity = int.Parse(dtCart.Rows[0]["GiveQuantity"].ToString());
                else
                    info.GiveQuantity = 0;
                //第二杯半价
                if (dtCart.Rows.Count > 0 && !string.IsNullOrEmpty(dtCart.Rows[0]["HalfQuantity"].ToString()))
                    info.HalfPriceQuantity = int.Parse(dtCart.Rows[0]["HalfQuantity"].ToString());
                else
                    info.HalfPriceQuantity = 0;

                info.MainCategoryPath = reader["MainCategoryPath"].ToString();
                info.ProductId = (int)reader["ProductId"];

                if (reader["SKU"] != DBNull.Value)
                {
                    info.SKU = (string)reader["SKU"];
                }
                info.Name = (string)reader["ProductName"];
                if (DBNull.Value != reader["Weight"])
                {
                    info.Weight = (int)reader["Weight"];
                }
                info.MemberPrice = info.AdjustedPrice = (decimal)reader["SalePrice"];
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    info.ThumbnailUrl40 = reader["ThumbnailUrl40"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    info.ThumbnailUrl60 = reader["ThumbnailUrl60"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    info.ThumbnailUrl100 = reader["ThumbnailUrl100"].ToString();
                }
                if (DBNull.Value != reader["IsfreeShipping"])
                {
                    info.IsfreeShipping = Convert.ToBoolean(reader["IsfreeShipping"]);
                }
                if (DBNull.Value != reader["CategoryId"])
                {
                    info.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                }
                string str = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string)reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string)reader["ValueStr"])))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                info.SkuContent = str;
            }
            return info;
        }

        /// <summary>
        /// 后台代理商采购时获取购物车明细项信息,支持对仓库中的商品的采购功能
        /// </summary>
        public ShoppingCartItemInfo GetCartItemInfoAll(MemberInfo member, string skuId, int quantity, int pcUserid = 0)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select sc.*,hs.CostPrice,hs.SalePrice from Hishop_ShoppingCarts as sc left join dbo.Hishop_SKUs as hs on sc.SkuId = hs.SkuId  where sc.UserId = {0} and sc.SkuId = '{1}' order by hs.SalePrice desc"
                , (member != null) ? member.UserId : pcUserid, skuId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            DataTable dtCart = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];

            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfoAll");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }

                info = new ShoppingCartItemInfo();
                info.SkuId = skuId;
                info.Quantity = info.ShippQuantity = quantity;
                //买一送一修改
                if (dtCart.Rows.Count > 0 && !string.IsNullOrEmpty(dtCart.Rows[0]["GiveQuantity"].ToString()))
                    info.GiveQuantity = int.Parse(dtCart.Rows[0]["GiveQuantity"].ToString());
                else
                    info.GiveQuantity = 0;
                //第二杯半价
                if (dtCart.Rows.Count > 0 && !string.IsNullOrEmpty(dtCart.Rows[0]["HalfQuantity"].ToString()))
                    info.HalfPriceQuantity = int.Parse(dtCart.Rows[0]["HalfQuantity"].ToString());
                else
                    info.HalfPriceQuantity = 0;

                info.MainCategoryPath = reader["MainCategoryPath"].ToString();
                info.ProductId = (int)reader["ProductId"];

                if (reader["SKU"] != DBNull.Value)
                {
                    info.SKU = (string)reader["SKU"];
                }
                info.Name = (string)reader["ProductName"];
                if (DBNull.Value != reader["Weight"])
                {
                    info.Weight = (int)reader["Weight"];
                }
                info.MemberPrice = info.AdjustedPrice = (decimal)reader["SalePrice"];
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    info.ThumbnailUrl40 = reader["ThumbnailUrl40"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    info.ThumbnailUrl60 = reader["ThumbnailUrl60"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    info.ThumbnailUrl100 = reader["ThumbnailUrl100"].ToString();
                }
                if (DBNull.Value != reader["IsfreeShipping"])
                {
                    info.IsfreeShipping = Convert.ToBoolean(reader["IsfreeShipping"]);
                }
                if (DBNull.Value != reader["CategoryId"])
                {
                    info.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                }
                string str = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string)reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string)reader["ValueStr"])))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                info.SkuContent = str;
            }
            return info;
        }

        public DataTable GetShopping(string CategoryId, MemberInfo member, int pcUserid=0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select * from Hishop_ShoppingCarts where CategoryId=", CategoryId, " and UserId = ", (member!=null)?member.UserId:pcUserid }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }


        /// <summary>
        /// 获取购物车列表 新增了对giftList的支持
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public ShoppingCartInfo GetShoppingCart(MemberInfo member,int pcUserid=0)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : pcUserid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartItemInfo item;
                    if (pcUserid > 0)
                    {
                        item = this.GetCartItemInfoAll(member, (string)reader["SkuId"], (int)reader["Quantity"], pcUserid);
                    }
                    else
                    {
                        item = this.GetCartItemInfo(member, (string)reader["SkuId"], (int)reader["Quantity"], pcUserid);
                    }
                    
                    if (item != null)
                    {
                        info.LineItems.Add(item);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShoppingCartGiftInfo info3 = DataMapper.PopulateGiftCartItem(reader);
                    info3.Quantity = (int)reader["Quantity"];
                    info.LineGifts.Add(info3);
                }
            }
            return info;
        }

        /// <summary>
        /// 获取购物车列表 新增了对giftList的支持
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public List<ShoppingCartInfo> GetShoppingCartList(MemberInfo member)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartItemInfo item = this.GetCartItemInfo(member, (string)reader["SkuId"], (int)reader["Quantity"]);
                    if (item != null)
                    {
                        info.LineItems.Add(item);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShoppingCartGiftInfo info3 = DataMapper.PopulateGiftCartItem(reader);
                    info3.Quantity = (int)reader["Quantity"];
                    info.LineGifts.Add(info3);
                }
                list.Add(info);
            }
            return list;
        }

        public List<ShoppingCartInfo> GetShoppingCartAviti(MemberInfo member,int pcUserid=0)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            DataTable shoppingCategoryId = this.GetShoppingCategoryId();
            DataTable shopping = new DataTable();
            for (int i = 0; i < shoppingCategoryId.Rows.Count; i++)
            {
                ShoppingCartInfo item = new ShoppingCartInfo
                {
                    CategoryId = int.Parse(shoppingCategoryId.Rows[i]["CategoryId"].ToString())
                };
                shopping = this.GetShopping(item.CategoryId.ToString(), member, pcUserid);
                for (int j = 0; j < shopping.Rows.Count; j++)
                {
                    ShoppingCartItemInfo info2 = this.GetCartItemInfo(member, shopping.Rows[j]["SkuId"].ToString(), int.Parse(shopping.Rows[j]["Quantity"].ToString()), pcUserid);
                    if (info2 != null)
                    {
                        item.LineItems.Add(info2);
                    }
                }
                list.Add(item);
            }
            return list;
        }

        public DataTable GetShoppingCategoryId()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select distinct CategoryId from Hishop_ShoppingCarts ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void UpdateLineItemQuantity(MemberInfo member, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
        public void UpdateLineItemQuantityPC(int pcUserid, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, pcUserid);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        //20150921新增,针对giftitem在购物车中的减删除
        public void RemoveGiftItem(int userId, int giftId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.String, giftId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void UpdateGiftItemQuantity(MemberInfo member, int giftId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.String, giftId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 2016-01-22，根据用户ID得到购物车表信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>购物车表数据集</returns>
        public DataTable GetCartShopping(int userId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select sc.*,hs.CostPrice,hs.SalePrice from Hishop_ShoppingCarts as sc left join dbo.Hishop_SKUs as hs on sc.SkuId = hs.SkuId  where UserId = {0} order by hs.SalePrice desc", userId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        /// <summary>
        /// 2016-01-22，修改购物车商品信息，买一送一活动使用
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void UpdateLineItemBuyToGive(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET GiveQuantity = isnull(GiveQuantity,0) + 1 WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        /// <summary>
        /// 2016-10-27，修改购物车商品信息，第二杯半价活动使用
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void UpdateLineItemBuyHalfGive(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET HalfQuantity = isnull(HalfQuantity,0) + 1 WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int GetShoppingCartNum(int memberId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ISNULL(sum(Quantity),0)c from Hishop_ShoppingCarts where UserId = " + memberId);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public decimal GetShoppingCartTotal(int memberId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ISNULL(SUM(HS.SalePrice * Quantity),0)as totalprice from Hishop_ShoppingCarts HC left join Hishop_SKUs HS on hc.SkuId = hs.SkuId where UserId = " + memberId);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }
    }
}

