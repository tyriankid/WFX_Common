using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
	public class EditDistributor : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.DropDownList DrStatus;
        protected System.Web.UI.WebControls.DropDownList DrAgentGrade;
		protected System.Web.UI.WebControls.TextBox txtStoreDescription;
		protected System.Web.UI.WebControls.TextBox txtStoreName;
		private int userid;
		private void Bind()
		{
			DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(this.Page.Request.QueryString["UserId"]));
			if (userIdDistributors != null)
			{
				this.txtStoreName.Text = userIdDistributors.StoreName;
				this.txtStoreDescription.Text = userIdDistributors.StoreDescription;
				this.DrStatus.SelectedValue = userIdDistributors.ReferralStatus.ToString();
                this.DrAgentGrade.DataBind();
                int grade = DistributorsBrower.GetDistributorInfo(int.Parse(this.Page.Request.QueryString["UserId"])).AgentGradeId;
                DrAgentGrade.SelectedIndex = grade-1;
			}
			else
			{
                this.ShowMsg("店铺不存在！", false);
			}
		}
		private void btn_Submint(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtStoreName.Text.Trim()))
			{
				this.ShowMsg("店铺名不能为空", false);
			}
            
            else
            {
                DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(this.Page.Request.QueryString["UserId"]));
                if (userIdDistributors != null)
                {
                    userIdDistributors.StoreName = this.txtStoreName.Text.Trim();
                    userIdDistributors.StoreDescription = this.txtStoreDescription.Text.Trim();
                    //userIdDistributors.ReferralStatus = int.Parse(this.DrStatus.SelectedValue);

                    userIdDistributors.AgentGradeId = int.Parse( this.DrAgentGrade.SelectedValue);
                    if (DistributorsBrower.UpdateDistributor(userIdDistributors))
                    {
                        DistributorsBrower.RemoveDistributorCache(userIdDistributors.UserId);
                        this.ShowMsg("店铺信息修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("店铺信息店修改失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("店铺不存在！", false);
                }
            }
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmit.Click += new System.EventHandler(this.btn_Submint);
			if (!base.IsPostBack)
			{
				if (int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
				{
					this.Bind();
				}
				else
				{
					this.Page.Response.Redirect("DistributorList.aspx");
				}
			}
		}
	}
}
