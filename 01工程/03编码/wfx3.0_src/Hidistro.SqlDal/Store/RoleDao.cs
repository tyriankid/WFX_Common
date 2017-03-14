namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class RoleDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddPrivilegeInRoles(int roleId, string strPermissions)
        {
            string[] strArray = strPermissions.Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder(" ");
            if ((strArray != null) && (strArray.Length > 0))
            {
                foreach (string str in strArray)
                {
                    builder.AppendFormat("INSERT INTO Hishop_PrivilegeInRoles (RoleId, Privilege) VALUES (@RoleId, {0}); ", str);
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.String, roleId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool AddRole(RoleInfo role)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Roles (RoleName, Description) VALUES (@RoleName, @Description)");
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, role.RoleName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, role.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void ClearRolePrivilege(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PrivilegeInRoles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteRole(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("if( select count(*) from aspnet_Managers where RoleId = @RoleId ) = 0 DELETE FROM aspnet_Roles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<int> GetPrivilegeByRoles(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PrivilegeInRoles  WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            IList<int> list = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["Privilege"]);
                }
            }
            return list;
        }

        public RoleInfo GetRole(int roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, roleId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<RoleInfo>(reader);
            }
        }

        public IList<RoleInfo> GetRoles()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<RoleInfo>(reader);
            }
        }

        public bool RoleExists(string roleName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Roles WHERE RoleName = @RoleName");
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, roleName);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool UpdateRole(RoleInfo role)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Roles SET RoleName = @RoleName, Description = @Description WHERE RoleId = @RoleId");
            this.database.AddInParameter(sqlStringCommand, "RoleId", DbType.Int32, role.RoleId);
            this.database.AddInParameter(sqlStringCommand, "RoleName", DbType.String, role.RoleName);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, role.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
        /// <summary>
        /// 获取某角色下的第一个权限的页面菜单地址,layoutStart参数用于过滤出一级菜单或者二级菜单下的第一个权限内的菜单地址.
        /// </summary>
        public string GetFirstRoleUrl(int roleId,string layoutStart="")
        {

            string sqlStringCommand = string.Empty;
            sqlStringCommand = string.Format("select top 1 MIUrl from YiHui_MenuInfo where MIID in (select mrmenuid from YiHui_MenuRelation where MRRoleId={0}) and LEN(Layout)>= 6 ", roleId);
            if (layoutStart != "")
            {
                sqlStringCommand += " and layout like '" + layoutStart + "%'";
            }
            sqlStringCommand += " order by Layout";

            using (IDataReader reader = this.database.ExecuteReader(CommandType.Text, sqlStringCommand))
            {
                return ((DataTable)DataHelper.ConverDataReaderToDataTable(reader)).Rows[0]["MIUrl"].ToString();
            }
        }



    }
}

