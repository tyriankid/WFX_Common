namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

    public static class CouponHelper
    {
        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().CreateCoupon(coupon, count, out lotNumber);
        }

        public static int CreateCoupon(CouponInfo coupon, int count)
        {
            return new CouponDao().CreateCoupon(coupon, count);
        }

        public static bool DeleteCoupon(int couponId)
        {
            return new CouponDao().DeleteCoupon(couponId);
        }
        /// <summary>
        /// 更新优惠券是否首页赠送的字段
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public static bool UpdateIsSendAtHomepage(int couponId,string type)
        {
            if (type == "on")
                return new CouponDao().UpdateIsSendAtHomepage(couponId);
            else if (type == "off")
                return new CouponDao().UpdateNoSendAtHomepage(couponId);
            else
                return false;
        }

        /// <summary>
        /// 更新优惠券是否在成为分销商时赠送的字段
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool UpdateIsSendAtDistributor(int couponId, string type)
        {
            if (type == "on")
                return new CouponDao().UpdateIsSendAtDistributor(couponId);
            else if (type == "off")
                return new CouponDao().UpdateNoSendAtDistributor();
            else
                return false;
        }

        public static CouponInfo GetCoupon(int couponId)
        {
            return new CouponDao().GetCouponDetails(couponId);
        }

        public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            return new CouponDao().GetCouponItemInfos(lotNumber);
        }
        public static List<CategoryQuery> GetHishop_Categories()
        {
            return new CouponDao().GetHishop_Categories();
        }
     
        public static List<CouponInfo> GetHishop_Coupons()
        {
            return new CouponDao().GetHishop_Coupons();
        }
        public static List<CategoryQuery> Getaspnet_ManagersClientUserId()
        {
            return new CouponDao().Getaspnet_ManagersClientUserId();
        } 

        public static DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            return new CouponDao().GetCouponsList(query);
        }

        public static DataTable GetUserCoupons(int userId, int useType = 0)
        {
            return new CouponDao().GetUserCoupons(userId,useType);
        }

        public static DataTable GetAllCouponsID()
        {
            return new CouponDao().GetAllCouponsID();
        }

        public static DataTable GetAllCoupons()
        {
            return new CouponDao().GetAllCoupons();
        }

        public static DataTable GetAllCouponItemsClaimCode()
        {
            return new CouponDao().GetAllCouponItemsClaimCode();
        }

        public static DbQueryResult GetNewCoupons(Pagination page)
        {
            return new CouponDao().GetNewCoupons(page);
        }

        public static void SendClaimCodes(int couponId, IList<CouponItemInfo> listCouponItem)
        {
            foreach (CouponItemInfo info in listCouponItem)
            {
                new CouponDao().SendClaimCodes(couponId, info);
            }
        }
        /// <summary>
        /// 新增会员领取优惠卷信息
        /// </summary>
        /// <param name="CouponID"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool SendClaimCodes(int CouponID,CouponItemInfo info)
        {
            return new CouponDao().SendClaimCodes(CouponID, info);
        }

        public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return new CouponDao().UpdateCoupon(coupon);
        }
        /// <summary>
        /// 查询优惠卷带不能使用商品分类ID
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCouponAllCate()
        {
            return new CouponDao().GetCouponAllCate();
        }

        /// <summary>
        /// 获取该用户可被领取的优惠券的id
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUseableCoupons(int userId)
        {
            return new CouponDao().GetUseableCoupons(userId);//后台有效的优惠券
        }


        /// <summary>
        /// 生成唯一加密优惠券code
        /// </summary>
        public static string GetShowCode(DateTime time, string rid)
        {
            string timeStamp = ConvertDateTimeToInt(time).ToString();//获取时间戳
            string leftStr = timeStamp.Substring(timeStamp.Length - 5);
            string rightStr = rid.ToString().PadLeft(5,'0');
            return leftStr+rightStr;
        }

        /// <summary>
        /// 反生成唯一加密优惠券code,返回该优惠券的claimCode
        /// </summary>
        public static DataTable DeCodeShowCode(string showCode)
        {
            return new CouponDao().DeCodeShowCode(showCode);
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
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        private static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 添加优惠卷活动
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public static bool AddCouponsAct(CouponsAct ac)
        {
            return new CouponDao().AddCouponsAct(ac);
        }
        /// <summary>
        /// 修改优惠卷活动
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public static bool UpdateConponsAct(CouponsAct ac)
        {
            return new CouponDao().UpdateConponsAct(ac);
        }

        /// <summary>
        /// 删除优惠卷活动
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool deleteCouponsAct(int ID)
        {
            return new CouponDao().deleteCouponsAct(ID);
        }
        /// <summary>
        /// 得到优惠卷活动实体类
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static CouponsAct GetCouponsAct(int ID)
        {
            return new CouponDao().GetCouponsAct(ID);
        }
        /// <summary>
        /// 得到正在进行的活动
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCouponsActNow()
        {
            return new CouponDao().GetCouponsActNow();
        }

        /// <summary>
        /// 得到优惠卷活动
        /// </summary>
        /// <param name="CouponsName"></param>
        /// <returns></returns>
        public static DataTable GetCouponsAct(string CouponsName)
        {
            return new CouponDao().GetCouponsAct(CouponsName);
        }
        /// <summary>
        /// 添加用户分享优惠卷信息
        /// </summary>
        /// <param name="cas"></param>
        /// <returns></returns>
        public static int addCouponsActShare(CouponsActShare cas)
        {
            return new CouponDao().addCouponsActShare(cas);
        }

        public static CouponsActShare GetCouponsActShare(int ID)
        {
            return new CouponDao().GetCouponsActShare(ID);
        }
        /// <summary>
        /// 判断用户是否已领取优惠卷
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CouponID"></param>
        /// <returns></returns>
        public static bool CheckUserIsCoupon(int UserID,int CouponID)
        {
            return new CouponDao().CheckUserIsCoupon(UserID,CouponID);
        }

        /// <summary>
        /// 查看用户有没分享优惠卷 有则返回ID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CouponActID"></param>
        /// <returns></returns>
        public static int GetShareID(int UserID, int CouponActID)
        {
            return new CouponDao().GetShareID(UserID,CouponActID);
        }
        /// <summary>
        /// 根据来源得到当天领取优惠卷数量
        /// </summary>
        /// <param name="ShareID"></param>
        /// <returns></returns>
        public static int GetNowDayCount(int ShareID)
        {
            return new CouponDao().GetNowDayCount(ShareID);
        }
    }
}

