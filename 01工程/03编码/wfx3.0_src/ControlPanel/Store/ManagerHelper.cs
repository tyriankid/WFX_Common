namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.Caching;
    public static class ManagerHelper
    {
        public static void AddPrivilegeInRoles(int roleId, string strPermissions)
        {
            new RoleDao().AddPrivilegeInRoles(roleId, strPermissions);
        }

        public static bool AddRole(RoleInfo role)
        {
            return new RoleDao().AddRole(role);
        }

        public static void CheckPrivilege(Privilege privilege)
        {
            if (GetCurrentManager() == null)
            {
                HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
            }
        }

        public static void ClearRolePrivilege(int roleId)
        {
            new RoleDao().ClearRolePrivilege(roleId);
        }

        public static bool Create(ManagerInfo manager)
        {
            //新增对重复用户名的判断
            if (GetManager(manager.UserName) != null)
            {
                return false;
            }
            return new MessageDao().Create(manager);
        }

        public static bool CreateAgent(ManagerInfo manager)
        {
            //新增对重复用户名的判断
            if (GetManager(manager.UserName) != null)
            {
                return false;
            }
            return new MessageDao().CreateAgent(manager);
        }



        public static bool Delete(int userId)
        {
            if (GetManager(userId).UserId == Globals.GetCurrentManagerUserId())
            {
                return false;
            }
            HiCache.Remove(string.Format("DataCache-Manager-{0}", userId));
            return new MessageDao().DeleteManager(userId);
        }

        public static bool DeleteRole(int roleId)
        {
            return new RoleDao().DeleteRole(roleId);
        }

        public static ManagerInfo GetCurrentManager()
        {
            return GetManager(Globals.GetCurrentManagerUserId());
        }

        public static ManagerInfo GetManager(int userId)
        {
            ManagerInfo manager = HiCache.Get(string.Format("DataCache-Manager-{0}", userId)) as ManagerInfo;
            if (manager == null)
            {
                manager = new MessageDao().GetManager(userId);
                HiCache.Insert(string.Format("DataCache-Manager-{0}", userId), manager, 360, CacheItemPriority.Normal);
            }
            return manager;
        }
       
        public static ManagerInfo GetManager(string userName)
        {
            return new MessageDao().GetManager(userName);
        }

        public static DbQueryResult GetManagers(ManagerQuery query)
        {
            return new MessageDao().GetManagers(query);
        }

        public static DataTable GetAllManagers()
        {
            return new MessageDao().GetAllManagers();
        }
        public static DataTable GetHishop_Product()
        {
            return new MessageDao().GetHishop_Product();
        }

        public static IList<int> GetPrivilegeByRoles(int roleId)
        {
            return new RoleDao().GetPrivilegeByRoles(roleId);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public static RoleInfo GetRole(int roleId)
        {
            return new RoleDao().GetRole(roleId);
        }

        /// <summary>
        /// 获取某角色下的第一个权限的页面菜单地址
        /// </summary>
        public static string GetFirstRoleUrl(int roleId,string layoutStart="")
        {
            return new RoleDao().GetFirstRoleUrl(roleId,layoutStart);
        }

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        public static IList<RoleInfo> GetRoles()
        {
            return new RoleDao().GetRoles();
        }

        public static bool RoleExists(string roleName)
        {
            return new RoleDao().RoleExists(roleName);
        }

        public static bool Update(ManagerInfo manager)
        {
            HiCache.Remove(string.Format("DataCache-Manager-{0}", manager.UserId));
            return new MessageDao().Update(manager);
        }

        public static bool UpdateRole(RoleInfo role)
        {
            return new RoleDao().UpdateRole(role);
        }

        /// <summary>
        /// 获取当前管理员的所属区域
        /// </summary>
        /// <param name="userId">管理员id</param>
        /// <returns></returns>
        public static DataTable GetManagerArea(int userId)
        {
            return new MessageDao().GetManagerArea(userId);
        }

        /// <summary>
        /// 根据条件得到当前区域表信息
        /// </summary>
        /// <param name="where">条件，为空则查询所有信息</param>
        /// <returns></returns>
        public static DataTable GetRegion(string where)
        {
            return new RegionDao().GetRegion(where);
        }

        /// <summary>
        /// 根据条件得到当前商品区域关系信息
        /// </summary>
        /// <param name="where">条件，为空则查询所有信息</param>
        /// <returns></returns>
        public static DataTable GetProductRegion(string where)
        {
            return new RegionDao().GetProductRegion(where);
        }
             
        /// <summary>
        /// 根据条件得到下单列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static DataTable GetAgentProduct(string where)
        {
            return new RegionDao().GetAgentProduct(where);
        }

        /// <summary>
        /// 根据SkuId集合删除订货列表值
        /// </summary>
        /// <param name="skuids">SkuId值集合</param>
        /// <param name="userid">当前登录用户ID</param>
        /// <returns>true成功；false失败</returns>
        public static bool DeleteAgentProduct(string skuids, int userid)
        {
            return new RegionDao().DeleteAgentProduct(skuids, userid);
        }

        /// <summary>
        /// 判断前台微信用户是否为代理商
        /// </summary>
        /// <param name="userId">前端用户ID</param>
        /// <returns></returns>
        public static bool ExistClientUserId(int userId)
        {
            return new MessageDao().ExistClientUserId(userId);
        }

        /// <summary>
        /// 根据前端用户Id得到管理端用户对象（是否是代理商）
        /// </summary>
        /// <param name="clientuserId">前端用户Id</param>
        /// <returns>管理端用户对象</returns>
        public static ManagerInfo GetManagerByClientUserId(int clientuserId)
        {
            return new MessageDao().GetManagerByClientUserId(clientuserId);  
        }

        /// <summary>
        /// 后台账号打卡
        /// </summary>
        public static string DutyOn(int managerId)
        {
            string msg=string.Empty;
            //如果当天的打卡信息不存在,则打卡
            if (!new MessageDao().isDutyExist(managerId))
            {
                new MessageDao().DutyOn(managerId);
                msg="打卡成功";
            }
            else
            {
                msg="不允许重复打卡";
            }
            return msg;
        }

        /// <summary>
        /// 打卡重复判断
        /// </summary>
        public static bool isDutyExist(int managerId)
        {
            return new MessageDao().isDutyExist(managerId);
        }

        /// <summary>
        /// 后台账号关班
        /// </summary>
        public static string DutyOff(int managerId)
        {
            string msg = string.Empty;
            //如果当天的关班信息不存在,则关班
            if (!new MessageDao().isDutyOffExist(managerId))
            {
                new MessageDao().DutyOff(managerId);
                msg = "关班成功";
            }
            else
            {
                msg = "不允许重复关班";
            }
            return msg;
        }

        /// <summary>
        /// 获取当天打卡信息
        /// </summary>
        public static DataTable GetDutyInfo(int managerId)
        {
            return new MessageDao().GetDutyInfo(managerId);
        }

        /// <summary>
        /// 获取后台账号的交接班列表
        /// </summary>
        public static DataTable GetDutyInfoList(DateTime? dateStart, DateTime? dateEnd, int managerId = 0)
        {
            DataTable dt = new MessageDao().GetDutyInfoList(dateStart, dateEnd, managerId);
            dt.Columns.Add("DutyHours");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i]["LoginOutTime"].ToString()))//如果登出时间为空,表示还未关班
                {
                    //dt.Rows[i]["LoginOutTime"] = "未交班";
                    //dt.Rows[i]["OrdersCount"] = "未交班";
                    //dt.Rows[i]["OrdersTotal"] = "未交班";
                }
                else
                {
                    TimeSpan ts = ((DateTime)dt.Rows[i]["LoginOutTime"]).Subtract((DateTime)dt.Rows[i]["LoginTime"]);
                    string timespan = (ts.Days.ToString() == "0" ? "" : ts.Days.ToString() + "天")
                    + (ts.Hours.ToString() == "0" ? "" : ts.Hours.ToString() + "小时")
                    + (ts.Minutes.ToString() == "0" ? "" : ts.Minutes.ToString() + "分钟");
                    dt.Rows[i]["DutyHours"] = timespan;
                }
            }

            return dt;
        }
        /// <summary>
        /// 爽爽挝啡pc点餐系统生成的订单根据sender获取后台点餐账号对应的店铺名
        /// </summary>
        public static string getPcOrderStorenameBySender(int sender)
        {
            return new MessageDao().getPcOrderStorenameBySender(sender);
        }

        public static string getPcOrderStorenameByClientuserid(int clientuserid)
        {
            return new MessageDao().getPcOrderStorenameByClientuserid(clientuserid);
        }

        public static int getSenderIdByClientUserId(int clientuserid)
        {
            return new MessageDao().getSenderIdByClientUserId(clientuserid);
        }

        public static int getClientUserIdBySenderId(int senderid)
        {
            return new MessageDao().getClientUserIdBySenderId(senderid);
        }

        public static bool addStoreFansCount(int clientuserid, int userid, int subscribetype)
        {
            return new MessageDao().addStoreFansCount(clientuserid,userid,subscribetype);
        }

        public static DataSet getStoreFans(DateTime? startTime, DateTime? endTime, int storeId)
        {
            return new MessageDao().getStoreFans(startTime, endTime, storeId);
        }

        public static string GetStoreName(int storeid)
        {
            return new MessageDao().GetStoreName(storeid);
        }

    }
}

