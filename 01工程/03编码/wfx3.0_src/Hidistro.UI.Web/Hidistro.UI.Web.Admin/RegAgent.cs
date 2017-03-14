using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class RegAgent : System.Web.UI.Page
    {
        protected RegionSelector dropRegion;
        private int clientUserId;
        protected System.Web.UI.WebControls.Button btnCreate;
        protected RoleDropDownList dropRole;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtPassword;
        protected System.Web.UI.WebControls.TextBox txtPasswordagain;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAgentName;
        protected System.Web.UI.WebControls.TextBox txtCellPhone;
        protected System.Web.UI.WebControls.TextBox txtShipName;
        protected System.Web.UI.WebControls.TextBox txtAddress;

        //2015-11-17日修改
        protected System.Web.UI.WebControls.Literal litTitle;
        protected System.Web.UI.WebControls.Panel PanelID;

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else
            {
                ManagerInfo manager = new ManagerInfo
                {
                    RoleId = this.dropRole.SelectedValue,
                    UserName = this.txtUserName.Text.Trim(),
                    Email = this.txtEmail.Text.Trim(),
                    Password = HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()),
                    ClientUserId = clientUserId > 0 ? clientUserId : 0,
                    AgentName = this.txtAgentName.Text.Trim(),
                    State = 1 //通过此页面注册的后台代理商账号默认值都为1:未激活
                };

                if (ManagerHelper.CreateAgent(manager) )
                {
                    //如果创建成功了账号,同时还要保存地址信息,
                    ShippingAddressInfo shippingAddress = new ShippingAddressInfo
                    {
                        Address = this.txtAddress.Text,
                        CellPhone = this.txtCellPhone.Text,
                        ShipTo = this.txtShipName.Text,
                        Zipcode = "",
                        IsDefault = true,
                        UserId = Convert.ToInt32("99999" + ManagerHelper.GetManager(this.txtUserName.Text.Trim()).UserId.ToString()),
                        RegionId = Convert.ToInt32(this.dropRegion.GetSelectedRegionId())
                    };
                    Hidistro.SaleSystem.Vshop.MemberProcessor.AddAgentShippingAddress(shippingAddress);

                    this.txtEmail.Text = string.Empty;
                    this.txtUserName.Text = string.Empty;
                    this.ShowMsgAndReUrl("成功添加了一个代理商，请联系超级管理员激活该账号", true, "login.aspx");
                }
                else
                {
                    this.ShowMsg("添加失败!", false);
                }
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRole.DataBind();
            }


            IList<RoleInfo> roles = ManagerHelper.GetRoles();//设置部门为代理商
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].RoleName.Contains("代理商"))
                {
                    this.dropRole.SelectedValue = roles[i].RoleId;
                    this.dropRole.Enabled = false;
                    break;
                }
            }

            int num = base.Request.QueryString["regionId"].ToInt();
            if (num > 0)
            {
                this.dropRegion.SetSelectedRegionId(new int?(num));
            }

        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgAndReUrl(string msg, bool success, string url)
        {
            string str = string.Format("ShowMsgAndReUrl(\"{0}\", {1}, \"{2}\")", msg, success ? "true" : "false", url);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }


    }
}
