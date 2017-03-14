namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;


    public class WxPoiHelper
    {
        /// <summary>
        /// 插入wx门店列表信息
        /// </summary>
        public static bool SyncPoiListInfo(List<PoiInfoList> poiList)
        {
            return new WxPoiDao().SyncPoiListInfo(poiList);
        }

        public static DataTable GetPoiListInfo()
        {
            return new WxPoiDao().GetPoiListInfo();
        }

        /// <summary>
        /// 对后台账号绑定微信门店id
        /// </summary>
        public static bool BindSender(string poi_id, string sender)
        {
            return new WxPoiDao().BindSender(poi_id, sender);
        }

        /// <summary>
        /// 根据门店id匹配到对应的子账号id:sender
        /// </summary>
        public static DataTable GetSenderByPoiId(string poi_id)
        {
            return new WxPoiDao().GetSenderByPoiId(poi_id);
        }

        /// <summary>
        /// 新增要求开店信息
        /// </summary>
        public static bool AddRequireStoreInfo(int userId, string locationText)
        {
            return new WxPoiDao().AddRequireStoreInfo(userId, locationText);
        }

        /// <summary>
        ///  获取要求开店列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRequireStoreInfo()
        {
            return new WxPoiDao().GetRequireStoreInfo();
        }

        public static DataTable GetStoreName(string storeIds)
        {
            return new WxPoiDao().GetStoreName(storeIds);
        }

        public static DataTable GetStoreStreets(string storeIds)
        {
            return new WxPoiDao().GetStoreStreets(storeIds);
        }
    }
}

