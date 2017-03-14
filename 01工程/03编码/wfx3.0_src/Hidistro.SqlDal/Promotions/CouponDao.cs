namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class CouponDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddClaimCodeToUser(string claimCode, int userId,string UserName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CouponItems SET UserId = @UserId,UserName=@UserName  WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, UserName);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode and CouponStatus!=1");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            CouponActionStatus unknowError = CouponActionStatus.UnknowError;
            lotNumber = string.Empty;
            if (count <= 0)
            {
                lotNumber = string.Empty;
                if (null == coupon)
                {
                    return CouponActionStatus.UnknowError;
                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint,CategoryId,SenderId) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint,@CategoryId,@SenderId); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, coupon.CategoryId);
                this.database.AddInParameter(sqlStringCommand, "SenderId", DbType.Int32, coupon.SenderId);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    unknowError = CouponActionStatus.CreateClaimCodeSuccess;
                }
                return unknowError;
            }
            unknowError = CouponActionStatus.CreateClaimCodeSuccess;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
            this.database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, coupon.CouponId);
            this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
            this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                lotNumber = (string) this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
            }
            catch
            {
                unknowError = CouponActionStatus.CreateClaimCodeError;
            }
            return unknowError;
        }

        public int CreateCoupon(CouponInfo coupon, int count)
        {
            int result=-1;
            if (count <= 0)
            {
                if (null == coupon)
                {
                    return -1;
                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                result = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
                if(result > 0) return result;//如果重复,直接取couponId

                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    return Convert.ToInt32(obj2);
                }
                return -1;
            }
            result = 0;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
            this.database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, coupon.CouponId);
            this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
            this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        public bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public DataTable GetCoupon(decimal orderAmount,string CouponIds)
        {
            DataTable table = new DataTable();
            string str = "";
            if (!string.IsNullOrEmpty(CouponIds) && CouponIds != "")
            {
                str += " and c.CouponId not in ("+CouponIds+")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name, ClaimCode,Amount,SenderId,DiscountValue FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId" + str);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);//DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, Globals.GetCurrentMemberUserId());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 查询所有的优惠券信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCoupons()
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Hishop_Coupons");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
        /// <summary>
        /// 查询所有的已送出的优惠券ClaimCode
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCouponItemsClaimCode()
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId,ClaimCode,UserId from Hishop_CouponItems");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public CouponInfo GetCouponDetails(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponInfo>(reader);
            }
        }



        public CouponInfo GetCouponDetails(string couponCode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE @DateTime>StartTime AND  @DateTime <ClosingTime AND CouponId = (SELECT CouponId FROM Hishop_CouponItems WHERE ClaimCode =@ClaimCode AND CouponStatus =0)");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponInfo>(reader);
            }
        }

        /// <summary>
        /// 更新是否在首页赠送
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public bool UpdateIsSendAtHomepage(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons set sendAtHomepage=1 where CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, couponId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
   
        }

        /// <summary>
        /// 更新是否在成为分销商时赠送
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public bool UpdateIsSendAtDistributor(int couponId)
        {
            if (UpdateNoSendAtDistributor())//首先让所有的优惠券都不在成为分销商时赠送,然后再把当前优惠券设为成为分销商时赠送
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons set sendAtDistributor=1 where CouponId = @CouponId");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, couponId);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新所有的优惠券为非在首页赠送
        /// </summary>
        /// <returns></returns>
        public bool UpdateNoSendAtHomepage(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons set sendAtHomepage=0 Where CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, couponId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        /// <summary>
        /// 更新所有的优惠券为非在成为分销商时赠送
        /// </summary>
        /// <returns></returns>
        public bool UpdateNoSendAtDistributor()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons set sendAtDistributor=0");
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<CouponItemInfo>(reader);
            }
        }

        public List<CategoryQuery> GetHishop_Categories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Categories");
            List<CategoryQuery> list = new List<CategoryQuery>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = (List<CategoryQuery>)ReaderConvert.ReaderToList<CategoryQuery>(reader);
            }
            return list;
        }

        public List<CouponInfo> GetHishop_Coupons()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Coupons");
            List<CouponInfo> list = new List<CouponInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = (List<CouponInfo>)ReaderConvert.ReaderToList<CouponInfo>(reader);
            }
            return list;
        }


        public List<CategoryQuery> Getaspnet_ManagersClientUserId()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from aspnet_Managers where ClientUserId != 0");
            List<CategoryQuery> list = new List<CategoryQuery>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                list = (List<CategoryQuery>)ReaderConvert.ReaderToList<CategoryQuery>(reader);
            }
            return list;
        }
        //获取所有生效中的优惠券id,一般用于新会员的自动发放
        public DataTable GetAllCouponsID()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select CouponId from Hishop_Coupons where GETDATE() between StartTime and ClosingTime order by StartTime desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.CouponId.HasValue)
            {
                builder.AppendFormat("CouponId = {0}", query.CouponId.Value);
            }
            if (!string.IsNullOrEmpty(query.CounponName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Name = '{0}'", query.CounponName);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.CouponStatus.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CouponInfo", "ClaimCode", builder.ToString(), "*");
        }

        public DbQueryResult GetNewCoupons(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", string.Empty, "*");
        }

        public DataTable GetUserCoupons(int userId, int useType = 0)
        {
            string str = "";
            if (useType == 1)
            {
                str = " AND ci.CouponStatus = 0 AND ci.UsedTime is NULL and c.ClosingTime > @ClosingTime";
            }
            else if (useType == 2) //已使用的
            {
                str = " AND ci.UsedTime is not NULL and c.ClosingTime > @ClosingTime";
            }
            else if (useType == 3) //过期
            {
                str = " AND c.ClosingTime<getdate()";
            }
            else if (useType == 4) //历史的
            {
                str = " AND ci.CouponStatus = 1";
            }
            //DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus  FROM Hishop_CouponItems ci INNER JOIN Hishop_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId " + str);

            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select c.*, ci.ClaimCode,ci.CouponStatus,ci.GenerateTime,ci.RID from (SELECT RID=ROW_NUMBER() over(order by LotNumber),*  FROM Hishop_CouponItems ) ci INNER JOIN Hishop_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId " + str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus,FromInfo) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus,@FromInfo)");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            this.database.AddInParameter(sqlStringCommand, "FromInfo", DbType.Int32, couponItem.FromInfo);
            if (couponItem.UserId.HasValue)
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            if (!string.IsNullOrEmpty(couponItem.UserName))
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.String, 0);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            if (null != coupon)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint ,CategoryId=@CategoryId ,SenderId=@SenderId WHERE CouponId=@CouponId");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, coupon.CouponId);
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, coupon.CategoryId);
                this.database.AddInParameter(sqlStringCommand, "SenderId", DbType.Int32, coupon.SenderId);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                {
                    return CouponActionStatus.Success;
                }
            }
            return CouponActionStatus.UnknowError;
        }

        /// <summary>
        /// 查询优惠卷带不能使用商品分类ID
        /// </summary>
        /// <returns></returns>
        public DataTable GetCouponAllCate()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select CategoryIds=dbo.Fn_jhb_CatagoryIdByRule(CouponId),* From Hishop_Coupons ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取可被领取的优惠券的id
        /// </summary>
        /// <returns></returns>
        public DataTable GetUseableCoupons(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select * from Hishop_Coupons where couponid not in (select distinct (CouponId) from Hishop_CouponItems where UserId = {0}) and GETDATE() between StartTime and ClosingTime",userId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }   
        /// <summary>
        /// 根据showCode获取优惠券table
        /// </summary>
        /// <param name="showCode"></param>
        /// <returns></returns>
        public DataTable DeCodeShowCode(string showCode)
        {
            DataTable tbCoupon = new DataTable();
            //获取rid
            int rid = showCode.Substring(showCode.Length - 5).ToInt();
            //获取左侧时间戳的后5位
            string leftTimeStamp = showCode.Substring(0,showCode.Length-5);

            //根据rid获取信息
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Select c.*, ci.ClaimCode,ci.CouponStatus,ci.GenerateTime,ci.RID from (SELECT RID=ROW_NUMBER() over(order by LotNumber),*  FROM Hishop_CouponItems ) ci INNER JOIN Hishop_Coupons c ON c.CouponId = ci.CouponId WHERE RID = @RID ");
            this.database.AddInParameter(sqlStringCommand, "RID", DbType.Int32, rid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                tbCoupon = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if(tbCoupon.Rows.Count > 0)
            {
                string timeStamp=ConvertDateTimeToInt(Convert.ToDateTime(tbCoupon.Rows[0]["GenerateTime"])).ToString();
                string leftStr = timeStamp.Substring(timeStamp.Length - 5);
                if (tbCoupon.Rows.Count == 1 || leftStr == leftTimeStamp)
                {
                    return tbCoupon;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        private static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        public bool AddCouponsAct(CouponsAct ac)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into Hishop_CouponsAct values(@CouponsID,@BgImg,@CreateTime,@ColValue1,@ColValue2)");
            this.database.AddInParameter(sqlStringCommand, "CouponsID", DbType.Int32, ac.CouponsID);
            this.database.AddInParameter(sqlStringCommand, "BgImg", DbType.String, ac.BgImg);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, ac.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "ColValue1", DbType.Int32, ac.ColValue1);
            this.database.AddInParameter(sqlStringCommand, "ColValue2", DbType.String, ac.ColValue2);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool UpdateConponsAct(CouponsAct ac)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_CouponsAct set CouponsID=@CouponsID,BgImg=@BgImg,CreateTime=@CreateTime,ColValue1=@ColValue1,ColValue2=@ColValue2");
            this.database.AddInParameter(sqlStringCommand, "CouponsID", DbType.Int32, ac.CouponsID);
            this.database.AddInParameter(sqlStringCommand, "BgImg", DbType.String, ac.BgImg);
            this.database.AddInParameter(sqlStringCommand, "CreateTime", DbType.DateTime, ac.CreateTime);
            this.database.AddInParameter(sqlStringCommand, "ColValue1", DbType.String, ac.ColValue1);
            this.database.AddInParameter(sqlStringCommand, "ColValue2", DbType.String, ac.ColValue2);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public bool deleteCouponsAct(int ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete Hishop_CouponsAct where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, ID);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public CouponsAct GetCouponsAct(int ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_CouponsAct where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, ID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponsAct>(reader);
            }
        }

        public DataTable GetCouponsAct(string CouponsName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select c.*,ca.ID,ca.BgImg,ca.CreateTime,ca.ColValue1,ca.ColValue2 from Hishop_CouponsAct as ca left join Hishop_Coupons as c on c.CouponId=ca.CouponsId where c.Name like '%" + DataHelper.CleanSearchString(CouponsName) + "%'");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetCouponsActNow()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select c.*,ca.ID,ca.BgImg,ca.CreateTime,ca.ColValue1,ca.ColValue2 from Hishop_CouponsAct as ca left join Hishop_Coupons as c on c.CouponId=ca.CouponsId where c.StartTime<getdate() and c.ClosingTime>getDate()");
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public int addCouponsActShare(CouponsActShare cas)
        {
            int num;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into Hishop_CouponsActShare values(@CouponsActID,@CouponsID,@UserID,@UserName,@UserImg,@ShareTime,@UseCount);select @@IDENTITY ");
            this.database.AddInParameter(sqlStringCommand, "CouponsActID", DbType.Int32, cas.CouponsActID);
            this.database.AddInParameter(sqlStringCommand, "CouponsID", DbType.Int32, cas.CouponsID);
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, cas.UserID);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, cas.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserImg", DbType.String, cas.UserImg);
            this.database.AddInParameter(sqlStringCommand, "ShareTime", DbType.DateTime, cas.ShareTime);
            this.database.AddInParameter(sqlStringCommand, "UseCount", DbType.Int32, cas.UseCount);
            if (!int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out num))
            {
                return 0;
            }
            return num;
        }

        public CouponsActShare GetCouponsActShare(int ID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_CouponsActShare where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, ID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CouponsActShare>(reader);
            }
        }

        public bool CheckUserIsCoupon(int UserID,int CouponsID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select Count(CouponId) from Hishop_CouponItems where UserId=@UserId and CouponId=@CouponId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserID);
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, CouponsID);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand).ToString())>0;
        }

        public bool UpdateShareUseCount(CouponsActShare cas)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_CouponsActShare set UseCount=@UseCount where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, cas.ID);
            this.database.AddInParameter(sqlStringCommand, "UseCount", DbType.Int32, cas.UseCount);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
        }

        public int GetShareID(int UserID, int CouponActID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ID from Hishop_CouponsActShare where UserID=@UserID and CouponsActID=@CouponsActID");
            this.database.AddInParameter(sqlStringCommand, "UserID", DbType.Int32, UserID);
            this.database.AddInParameter(sqlStringCommand, "CouponsActID", DbType.Int32, CouponActID);
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return (int)dt.Rows[0][0];
            }
            return 0;
        }

        public int GetNowDayCount(int ShareID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select CouponId from Hishop_CouponItems where FromInfo=@FromInfo and DateDiff(dd,GenerateTime,getdate())=0");
            this.database.AddInParameter(sqlStringCommand, "FromInfo", DbType.Int32, ShareID);
            DataTable dt = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            return dt.Rows.Count;
        }
    }
}

