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
        Literal litMenuBanner;//������
        Literal litManagerName;//��ǰ�û���
        //static int roleId;//��ǰ��ɫid
        int userId;//��ǰ����Աid
        RoleInfo currentRole;//��ɫ��Ϣ
        ManagerInfo currentManager;//����Ա������Ϣ

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (ManagerHelper.GetCurrentManager() == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!base.IsPostBack)
            {
                this.litManagerName = (Literal)this.FindControl("litManagerName");//��ǰ�û���
                this.litMenuBanner = (Literal)this.FindControl("litMenuBanner");//��ǰ��ɫȨ���µĲ˵�����

                //��ȡ��ǰ����Ա��ɫ��Ϣ
                currentRole = ManagerHelper.GetRole(ManagerHelper.GetCurrentManager().RoleId);
                //roleId = currentRole.RoleId;
                //ViewState["roleId"] = currentRole.RoleId;
                //��ȡ��ǰ����Ա������Ϣ
                userId = ManagerHelper.GetCurrentManager().UserId;
                currentManager = ManagerHelper.GetManager(userId);

                BindData();
            }
        }

        private void BindData()
        {
            //�󶨵�ǰ�û���
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
            //��̬�󶨵�ǰ��ɫ�ĵ����˵�
            for (int i = 0; i < MenuInfo.Rows.Count; i++)
            {
                if (MenuInfo.Rows[i]["Layout"].ToString().Length == 2)//ֻ����һ���˵��������
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
                if (layout.Length <= 4 && currentManager.UserName!="yihui")//�����ǰ��һ�����˵�,���ȡȨ���ڵĵ�һ�������˵���ַ
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
