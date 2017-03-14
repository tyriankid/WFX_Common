namespace Hidistro.SqlDal.AdminMenus
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class AdminMenuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        /// <summary>
        /// 获取所有的menu
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAdminMenus()
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From YiHui_MenuInfo order by layout");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取当前角色权限内的菜单信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetRoleMenusId(int roleId)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select MR.*,Layout from Yihui_Menurelation MR inner join YiHui_MenuInfo MI on mr.MRMenuId=mi.MIID where MRRoleId = " + roleId +" order by layout");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取(已经被管理员设置了显示的菜单)用户的menu
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetRoleMenuInfos()
        {
            //DataTable roleMenu = GetRoleMenus(roleId);
            //string menuIds = string.Empty;
            //if (roleMenu.Rows.Count > 0)//如果当前角色下面有菜单
            //{
            //    for (int i = 0; i < roleMenu.Rows.Count; i++)
            //    {
            //        menuIds += roleMenu.Rows[i]["MIID"] + ",";
            //    }
            //    menuIds.TrimEnd(',');
            //}
            //else
            //{
            //    return null;
            //}
            
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from YiHui_MenuInfo where visible=1 order by layout");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取当前角色的menuids(一般用于树形控件绑定选中项)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleSelectIds(int roleId)
        {
            DataTable roleMenu = GetRoleMenusId(roleId);
            string menuIds = string.Empty;
            if (roleMenu.Rows.Count > 0)//如果当前角色下面有菜单
            {
                for (int i = 0; i < roleMenu.Rows.Count; i++)
                {
                    menuIds += roleMenu.Rows[i]["MIID"] + ",";
                }
                menuIds.TrimEnd(',');
                return menuIds;
            }
            else
            {
                return null;
            }
        }

        public string GetRoleSelectLayoutIds(int roleId)
        {
            DataTable roleMenu = GetRoleMenusId(roleId);
            string menuLayoutIds = string.Empty;
            if (roleMenu.Rows.Count > 0)//如果当前角色下面有菜单
            {
                for (int i = 0; i < roleMenu.Rows.Count; i++)
                {
                    menuLayoutIds += roleMenu.Rows[i]["Layout"] + ",";
                }
                menuLayoutIds.TrimEnd(',');
                return menuLayoutIds;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取选中的菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelectIds()
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT layout From YiHui_MenuInfo where visible = 1 order by layout");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 更新选中的menu
        /// </summary>
        /// <param name="ids">选中的id</param>
        /// <returns></returns>
        public bool UpdateSelectMenus(string ids)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update YiHui_MenuInfo set Visible = 0 where Layout not in (" + ids + ");update YiHui_MenuInfo set Visible = 1 where Layout  in (" + ids + ")");
            
            return this.database.ExecuteNonQuery(sqlStringCommand)>0;
        }

        /// <summary>
        /// 获取当前觉色拥有的菜单信息(一般用户首页导航栏的展示)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetCurrentRoleMenuInfo(int roleId)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select mi.* from YiHui_MenuRelation MR inner join YiHui_MenuInfo MI on mr.MRMenuId=mi.MIID where MRRoleId = " + roleId + " order by layout");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
    }
}

