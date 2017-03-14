namespace Hidistro.ControlPanel.AdminMenu
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.AdminMenus;
    using Hidistro.SqlDal.Members;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public static class AdminMenuHelper
    {

        public static DataTable GetAllAdminMenus()
        {
            return new AdminMenuDao().GetAllAdminMenus();
        }

        public static DataTable GetRoleMenusId(int roleId)
        {
            return new AdminMenuDao().GetRoleMenusId(roleId);
        }

        public static DataTable GetRoleMenuInfos()
        {
            return new AdminMenuDao().GetRoleMenuInfos();
        }

        public static string GetRoleSelectIds(int roleId)
        {
            return new AdminMenuDao().GetRoleSelectIds(roleId);
        }

        public static string GetRoleSelectLayoutIds(int roleId)
        {
            return new AdminMenuDao().GetRoleSelectLayoutIds(roleId);
        }

        public static DataTable GetSelectIds()
        {
            return new AdminMenuDao().GetSelectIds();
        }

        public static bool UpdateSelectMenus(string ids)
        {
            return new AdminMenuDao().UpdateSelectMenus(ids);
        }

        public static DataTable GetCurrentRoleMenuInfo(int roleId)
        {
            return new AdminMenuDao().GetCurrentRoleMenuInfo(roleId);
        }
    }
}

