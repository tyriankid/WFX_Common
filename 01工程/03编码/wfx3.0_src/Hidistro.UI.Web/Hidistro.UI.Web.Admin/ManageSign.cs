using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class ManageSign : AdminPage
    {
        protected HtmlInputHidden hidRoleInfo;
        protected HtmlInputHidden hidState;
        protected Button btnSave;

        protected void BindMaterial()
        {
            DataTable ruleDT = PromoteHelper.GetSignRule();
            if (ruleDT.Rows.Count > 0)
            {
                hidRoleInfo.Value = ruleDT.Rows[0]["NeedDays"].ToString() + ";" + ruleDT.Rows[0]["SendPoints"].ToString();
                hidState.Value = "1";
            }
            else
            {
                hidState.Value = "0";
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindMaterial();
            }
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string[] row = hidRoleInfo.Value.Split(';');
            if (PromoteHelper.SetSignRoles(row[0], row[1],Convert.ToInt32(hidState.Value)))
            {
                this.ShowMsg("设置成功", true);
            }
            else
            {
                this.ShowMsg("设置失败", false);
            }
            
        }
    }
}
