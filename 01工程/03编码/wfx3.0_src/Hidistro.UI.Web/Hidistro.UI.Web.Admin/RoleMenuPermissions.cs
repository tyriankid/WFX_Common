using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class RoleMenuPermissions : AdminPage
	{
        static string roleId = string.Empty;//当前角色id
        HiddenField hiSelectIDS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                roleId = Request.QueryString["roleId"];
                DataTable currentMenuIds = AdminMenuHelper.GetRoleMenusId(Convert.ToInt32(roleId));
                currentMenuIds.PrimaryKey = new DataColumn[] { currentMenuIds.Columns["MRId"] };
                ViewState["currentMenuIds"] = currentMenuIds;
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();              
            }
        }

        /// <summary>
        /// 获取当前角色的所有菜单
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetZnodes()
        {
            //DataTable allMenus = AdminMenuHelper.GetAllAdminMenus();//获取所有菜单信息
            DataTable currentMenus = AdminMenuHelper.GetRoleMenuInfos();
            string znodes = string.Empty;
            if (currentMenus != null)//如果
            {
                for (int i = 0; i < currentMenus.Rows.Count; i++)
                {
                    string dataid=currentMenus.Rows[i]["MIID"].ToString();
                    string id = currentMenus.Rows[i]["Layout"].ToString();
                    string pid = id.Length <= 2 ? "00" : id.Substring(0, id.Length - 2);//上级id,如果当前layout是一级菜单,则为00,否则就是去掉后两位.(向前一级)
                    string name = currentMenus.Rows[i]["MIName"].ToString();
                    string link = currentMenus.Rows[i]["MiUrl"].ToString();
                    znodes += "{\"id\":\"" + id + "\",\"pId\":\"" + pid + "\",\"name\":\"" + name + "\",\"open\":\"true\",\"Link\":\"" + link + "\",\"DataId\":\"" + dataid + "\" },";
                }
                znodes = znodes.TrimEnd(',');               
            }
            return znodes;
        }
        /// <summary>
        /// 获取仅供显示的菜单
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetSelect()
        {
            string selectLayoutIds = AdminMenuHelper.GetRoleSelectLayoutIds(Convert.ToInt32(roleId));
            if (selectLayoutIds != null)
            {
                return selectLayoutIds;
            }
            else
            {
                return "false";
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            //待处理的数据集
            DataTable dtData = (DataTable)ViewState["currentMenuIds"];
            this.hiSelectIDS = (HiddenField)this.FindControl("hiSelectIDS");
            string[] arrayDataID = hiSelectIDS.Value.ToString().Split(',');
            

            //新增处理
            foreach (string dataID in arrayDataID)
            {
                if (dtData.Select("MRMenuid = '" + dataID + "'").Length==0)
                {
                    DataRow drNew = dtData.NewRow();
                    drNew["MRID"] = Guid.NewGuid();
                    drNew["MRRoleID"] = Request.QueryString["roleId"];
                    drNew["MRMenuid"] = dataID;
                    dtData.Rows.Add(drNew);
                }
            }
            int rowsCount = dtData.Rows.Count;
            //减少处理
            for (int i = 0; i < rowsCount; i++)
            {
                if (this.hiSelectIDS.Value.ToLower().IndexOf(dtData.Rows[i]["MRMenuid"].ToString().ToLower()) == -1)
                    dtData.Rows[i].Delete();
            }

            //开始对数据库进行更新操作
            string str = "";
            if (DataBaseHelper.CommitDataTable(dtData, "SELECT * from YiHui_MenuRelation") != -1)
            {
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存功能配置菜单生效。", "true");
            }
            else
            {
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存功能配置菜单失败！", "false");
            }
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }

        }


        

	}
}
