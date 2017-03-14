namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MenuDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool DeleteMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Menu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private int GetAllMenusCount()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from vshop_Menu");
            return (1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)));
        }

        public MenuInfo GetMenu(int menuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Menu WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
        {
            string selectSql = "SELECT * FROM vshop_Menu WHERE ParentMenuId = @ParentMenuId ";
            if (clientType == ClientType.VShop)
                selectSql += " AND (Client = @Client OR Client IS NULL)";
            else
                selectSql += " AND Client = @Client";
            selectSql += " ORDER BY DisplaySequence ASC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, parentId);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)clientType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public IList<MenuInfo> GetTopMenus(ClientType clientType)
        {
            string selectSql = "SELECT * FROM vshop_Menu WHERE ParentMenuId = 0 ";
            if (clientType == ClientType.VShop)
                selectSql += " AND (Client = " + (int)clientType + " OR Client IS NULL)";
            else
                selectSql += " AND Client = " + (int)clientType;
            selectSql += " ORDER BY DisplaySequence ASC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(selectSql);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<MenuInfo>(reader);
            }
        }

        public bool SaveMenu(MenuInfo menu)
        {
            int allMenusCount = this.GetAllMenusCount();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO vshop_Menu (ParentMenuId, Name, Type, ReplyId, DisplaySequence, Bind, [Content],Client) VALUES(@ParentMenuId, @Name, @Type, @ReplyId, @DisplaySequence, @Bind, @Content,@Client)");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, allMenusCount);
            this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int) menu.BindType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)menu.Client);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public void SwapMenuSequence(int menuId, bool isUp)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Menu_SwapDisplaySequence");
            this.database.AddInParameter(storedProcCommand, "MenuId", DbType.Int32, menuId);
            this.database.AddInParameter(storedProcCommand, "ZIndex", DbType.Int32, isUp ? 0 : 1);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public bool UpdateMenu(MenuInfo menu)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_Menu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type, ReplyId = @ReplyId, DisplaySequence = @DisplaySequence, Bind = @Bind, [Content] = @Content,Client=@Client WHERE MenuId = @MenuId");
            this.database.AddInParameter(sqlStringCommand, "ParentMenuId", DbType.Int32, menu.ParentMenuId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, menu.Name);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.String, menu.Type);
            this.database.AddInParameter(sqlStringCommand, "ReplyId", DbType.Int32, menu.ReplyId);
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, menu.DisplaySequence);
            this.database.AddInParameter(sqlStringCommand, "MenuId", DbType.Int32, menu.MenuId);
            this.database.AddInParameter(sqlStringCommand, "Bind", DbType.Int32, (int) menu.BindType);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, menu.Content);
            this.database.AddInParameter(sqlStringCommand, "Client", DbType.Int32, (int)menu.Client);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

