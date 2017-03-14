using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web
{
	//[PrivilegeCheck(Privilege.Summary)]
	public class Index:Page
	{
        Literal litMenuBanner;//导航栏
        Literal litManagerName;//当前用户名
        //static int roleId;//当前角色id
        int userId;//当前管理员id
        RoleInfo currentRole;//角色信息
        ManagerInfo currentManager;//管理员基本信息

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (ManagerHelper.GetCurrentManager() == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!base.IsPostBack)
            {
                this.litManagerName = (Literal)this.FindControl("litManagerName");//当前用户名
                this.litMenuBanner = (Literal)this.FindControl("litMenuBanner");//当前角色权限下的菜单导航

                //获取当前管理员角色信息
                currentRole = ManagerHelper.GetRole(ManagerHelper.GetCurrentManager().RoleId);
                //roleId = currentRole.RoleId;
                //ViewState["roleId"] = currentRole.RoleId;
                //获取当前管理员基本信息
                userId = ManagerHelper.GetCurrentManager().UserId;
                currentManager = ManagerHelper.GetManager(userId);

                BindData();
            }
        }

        private void BindData()
        {
            //绑定当前用户名
            litManagerName.Text = currentManager.UserName;
            DataTable MenuInfo = new DataTable();
            
            if (litManagerName.Text != "yihui")
            {
                MenuInfo = AdminMenuHelper.GetCurrentRoleMenuInfo(ManagerHelper.GetCurrentManager().RoleId);
            }
            else
            {
                MenuInfo = AdminMenuHelper.GetAllAdminMenus();
            }
            //动态绑定当前角色的导航菜单
            for (int i = 0; i < MenuInfo.Rows.Count; i++)
            {
                if (MenuInfo.Rows[i]["Layout"].ToString().Length == 2)//只有是一级菜单才能添加
                {
                    this.litMenuBanner.Text += string.Format("<a onclick=\"ShowMenuLeft('{0}','{1}',null)\">{2}</a>", MenuInfo.Rows[i]["Layout"], MenuInfo.Rows[i]["MIurl"], MenuInfo.Rows[i]["MIName"]);
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetLeftMenuInfo(string firstNode)
        {
            DataTable MenuInfo = new DataTable();
            int roldId = ManagerHelper.GetCurrentManager().RoleId;
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            if (currentManager.UserName.ToString() != "yihui")
            {
                MenuInfo = AdminMenuHelper.GetCurrentRoleMenuInfo(roldId);
            }
            else
            {
                MenuInfo = AdminMenuHelper.GetAllAdminMenus();
            }
            string json = "";

            DataRow[] rows = MenuInfo.Select(string.Format("Layout like '{0}%'", firstNode));
            foreach (DataRow row in rows)
            {
                var url = string.Empty;
                var layout=row["layout"].ToString();
                if (layout.Length <= 4 && currentManager.UserName!="yihui")//如果当前是一二级菜单,则获取权限内的第一个三级菜单地址
                {
                    url = ManagerHelper.GetFirstRoleUrl(roldId, layout);
                }
                else
                {
                    url = row["MIUrl"].ToString();
                }
                json += string.Format("DataID=\"{0}\",Title=\"{1}\",Link=\"{2}\",Layout=\"{3}\",IconLink=\"{4}\";", row["MIID"], row["MIName"],/*row["MIUrl"]*/url, row["Layout"], row["IconLink"]);
            }
            return json;
        }

	}
}
