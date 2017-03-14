using Hidistro.Core;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace Hidistro.SqlDal.Commodities
{

    public class WxPoiDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 同步微信门店信息
        /// </summary>
        public bool SyncPoiListInfo(List<PoiInfoList> poiList)
        {
            int count=0;
            bool isAdd = false;
            /*
            DbCommand isIxist = this.database.GetSqlStringCommand("select count(*)c from wx_poiInfo");
            isAdd = Convert.ToInt32(this.database.ExecuteScalar(isIxist)) == 0;

            if (!isAdd)//如果存在先删除
            {
                DbCommand deletSql = this.database.GetSqlStringCommand("delete from wx_poiInfo");
                this.database.ExecuteNonQuery(deletSql);
            }
            else
            {
             */
            //先删除
            DbCommand deletSql = this.database.GetSqlStringCommand("delete from wx_poiInfo");
            this.database.ExecuteNonQuery(deletSql);
            //再同步增加
            foreach (PoiInfoList poiInfo in poiList)
            {
                DbCommand execSql = this.database.GetSqlStringCommand(@"insert into wx_poiInfo (poi_id,[sid],update_status,available_state,business_name,branch_name,province,city,district,[address],telephone,categories,offset_type,longitude,latitude,photo_list,avg_price,introduction,recommend) values  (@poi_id,@sid,@update_status,@available_state,@business_name,@branch_name,@province,@city,@district,@address,@telephone,@categories,@offset_type,@longitude,@latitude,@photo_list,@avg_price,@introduction,@recommend)");
                this.database.AddInParameter(execSql, "@poi_id", DbType.String, poiInfo.base_info.poi_id);
                this.database.AddInParameter(execSql, "@sid", DbType.String, poiInfo.base_info.sid);
                this.database.AddInParameter(execSql, "@update_status", DbType.Int32, poiInfo.base_info.update_status);
                this.database.AddInParameter(execSql, "@available_state", DbType.Int32, poiInfo.base_info.available_state);
                this.database.AddInParameter(execSql, "@business_name", DbType.String, poiInfo.base_info.business_name);
                this.database.AddInParameter(execSql, "@branch_name", DbType.String, poiInfo.base_info.branch_name);
                this.database.AddInParameter(execSql, "@province", DbType.String, poiInfo.base_info.province);
                this.database.AddInParameter(execSql, "@city", DbType.String, poiInfo.base_info.city);
                this.database.AddInParameter(execSql, "@district", DbType.String, poiInfo.base_info.district);
                this.database.AddInParameter(execSql, "@address", DbType.String, poiInfo.base_info.address);
                this.database.AddInParameter(execSql, "@telephone", DbType.String, poiInfo.base_info.telephone);
                this.database.AddInParameter(execSql, "@categories", DbType.String, "");
                this.database.AddInParameter(execSql, "@offset_type", DbType.String, poiInfo.base_info.offset_type);
                this.database.AddInParameter(execSql, "@longitude", DbType.String, poiInfo.base_info.longitude);
                this.database.AddInParameter(execSql, "@latitude", DbType.String, poiInfo.base_info.latitude);
                this.database.AddInParameter(execSql, "@photo_list", DbType.String, "");
                //this.database.AddInParameter(execSql, "@special_open_time", DbType.String, "");
                this.database.AddInParameter(execSql, "@avg_price", DbType.Int32, poiInfo.base_info.avg_price);
                this.database.AddInParameter(execSql, "@introduction", DbType.String, poiInfo.base_info.introduction);
                this.database.AddInParameter(execSql, "@recommend", DbType.String, poiInfo.base_info.recommend);

                count += this.database.ExecuteNonQuery(execSql);
            }
            
            
            
            return count > 0;
        }

        /// <summary>
        /// 获取门店信息列表
        /// </summary>
        public DataTable GetPoiListInfo()
        {
            DataTable dt = new DataTable();
            DbCommand selectCommand = this.database.GetSqlStringCommand("select AD.storename,AM.userid as sender,AM.clientuserid as storeid,AM.UserName,WP.* from wx_poiInfo WP left join aspnet_Managers AM on wp.poi_id = AM.poi_id left join aspnet_distributors AD on AM.clientuserid = AD.userid");
            using (IDataReader reader = this.database.ExecuteReader(selectCommand))
            {
                dt = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return dt;
        }



        /// <summary>
        /// 对后台账号绑定微信门店id
        /// </summary>
        public bool BindSender(string poi_id, string sender)
        {
            DbCommand clearCommand = this.database.GetSqlStringCommand(string.Format("update aspnet_Managers set poi_id = '' where poi_id = {0}", poi_id));
            this.database.ExecuteNonQuery(clearCommand);
            DbCommand updateCommand = this.database.GetSqlStringCommand(string.Format("update aspnet_Managers set poi_id = '{0}' where UserId = {1}",poi_id,sender));
            return this.database.ExecuteNonQuery(updateCommand) > 0;
        }

        /// <summary>
        /// 根据门店id匹配到对应的子账号id:sender,相对应的前端id : distributrId
        /// </summary>
        public DataTable GetSenderByPoiId(string poi_id)
        {
            DbCommand selectCommand = this.database.GetSqlStringCommand(string.Format("select UserId,clientUserId from aspnet_Managers where poi_id = '{0}'",poi_id));
            using (IDataReader reader = this.database.ExecuteReader(selectCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }


        /***************要求开店start******************/

        /// <summary>
        /// 新增要求开店信息
        /// </summary>
        public bool AddRequireStoreInfo(int userId,string locationText)
        {
            DbCommand insertCommand = this.database.GetSqlStringCommand("insert into YiHui_RequireStore (RSId,UserId,Location,AddTime) values (NEWID(),@UserId,@Location,getdate())");
            this.database.AddInParameter(insertCommand, "@UserId", DbType.Int32, userId);
            this.database.AddInParameter(insertCommand, "@Location", DbType.String, locationText);
            return this.database.ExecuteNonQuery(insertCommand) == 1;
        }

        /// <summary>
        /// 获取要求开店的留言信息列表
        /// </summary>
        public DataTable GetRequireStoreInfo()
        {
            DbCommand selectCommand = this.database.GetSqlStringCommand("select top 30 YR.*,AM.UserHead,AM.UserName from YiHui_RequireStore YR left join aspnet_Members AM on YR.userid=AM.UserId order by YR.AddTime desc");
            using (IDataReader reader = this.database.ExecuteReader(selectCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /***************要求开店end******************/


        public DataTable GetStoreName(string storeIds)
        {
            DbCommand selectCommand = this.database.GetSqlStringCommand(string.Format("select userid,storename from aspnet_Distributors where userid in ({0})", storeIds));
            using (IDataReader reader = this.database.ExecuteReader(selectCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetStoreStreets(string storeIds)
        {
            DbCommand selectCommand = this.database.GetSqlStringCommand(string.Format("wp.latitude,wp.longitude,distributorId,RegionName from YiHui_DistributorRegion YD left join aspnet_Managers AM on yd.distributorId = am.ClientUserId left join wx_poiInfo wp on am.poi_id = wp.poi_id where YD.distributorId in ({0})", storeIds));
            using (IDataReader reader = this.database.ExecuteReader(selectCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
    }
}

