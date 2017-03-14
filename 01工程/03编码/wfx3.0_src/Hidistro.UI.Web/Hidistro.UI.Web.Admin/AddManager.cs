using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public class AddManager : AdminPage
    {
        private int clientUserId;
        protected System.Web.UI.WebControls.Button btnCreate;
        protected RoleDropDownList dropRole;
        protected System.Web.UI.WebControls.TextBox txtEmail;
        protected System.Web.UI.WebControls.TextBox txtPassword;
        protected System.Web.UI.WebControls.TextBox txtPasswordagain;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtAgentName;

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
                    AgentName = this.txtAgentName.Text.Trim()
                };
                if (ManagerHelper.Create(manager))
                {
                    this.txtEmail.Text = string.Empty;
                    this.txtUserName.Text = string.Empty;
                    this.ShowMsg("成功添加了一个管理员", true);

                    //跳转地址
                    if (clientUserId > 0)
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/distributor/DistributorList.aspx"), true);
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

            //存储Url参数值 clientUserId
            if (int.TryParse(this.Page.Request.QueryString["userId"], out this.clientUserId))
            {
                //判断是否升级过
                if (ManagerHelper.ExistClientUserId(clientUserId))
                {
                    //已经升级过
                    this.PanelID.Visible = false;//禁用保存按钮
                    this.litTitle.Text = "该用户已经设置为代理商。";//设置提示
                    this.litTitle.Visible = true;//显示提示
                }
                else
                {
                    //未升级过，默认选择代理商角色，不能修改
                    IList<RoleInfo> roles = ManagerHelper.GetRoles();//设置部门为代理商
                    for (int i = 0; i < roles.Count; i++)
                    {
                        if (roles[i].RoleName.Contains("代理商"))
                        {
                            //this.dropRole.SelectedValue = roles[i].RoleId;
                            //this.dropRole.Enabled = false;
                            break;
                        }
                    }

                }

            }
        }


    }
}
