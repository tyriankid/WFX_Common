
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public partial class EditAgentGrade : AdminPage
	{
		protected string ReUrl = "agentgradelist.aspx";

		private int m_GradeId;

		protected string htmlOperatorName = "编辑";

		private void AAbiuZJB()
		{
			if (this.m_GradeId > 0)
			{
				AgentGradeInfo agentGradeInfo = DistributorGradeBrower.GetAgentGradeInfo(this.m_GradeId);
                if (agentGradeInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
                this.txtName.Text = agentGradeInfo.AgentGradeName;
                this.txtFirstCommissionRise.Text = agentGradeInfo.FirstCommissionRise.ToString();
                this.txtDescription.Text = agentGradeInfo.Description;
                string ico = agentGradeInfo.Ico;
				if (ico != "/utility/pics/grade.png")
				{
					this.uploader1.UploadedImageUrl = ico;
				}
			}
		}

		protected void btnEditUser_Click(object sender, EventArgs e)
		{
			decimal num1 = new decimal(0, 0, 0, false, 1);
            AgentGradeInfo agentGradeInfo = new AgentGradeInfo();
			if (this.m_GradeId > 0)
			{
                agentGradeInfo = DistributorGradeBrower.GetAgentGradeInfo(this.m_GradeId);
			}
            agentGradeInfo.AgentGradeName = this.txtName.Text.Trim();
			decimal.TryParse(this.txtFirstCommissionRise.Text.Trim(), out num1);
            agentGradeInfo.FirstCommissionRise = num1;
            agentGradeInfo.Description = this.txtDescription.Text.Trim();
            agentGradeInfo.Ico = this.uploader1.UploadedImageUrl;
			if (this.m_GradeId <= 0)
			{
                if (!DistributorGradeBrower.CreateAgentGrade(agentGradeInfo))
				{
					this.ShowMsg("代理商等级新增失败", false);
					return;
				}
				this.ShowMsgAndReUrl("成功新增了代理商等级", true, this.ReUrl);
				return;
			}
            if (!DistributorGradeBrower.UpdateAgent(agentGradeInfo))
			{
				this.ShowMsg("代理商等级修改失败", false);
				return;
			}
            if (base.Request.QueryString["reurl"] != null)
			{
                this.ReUrl = base.Request.QueryString["reurl"].ToString();
			}
            this.ShowMsgAndReUrl("成功修改了代理商等级", true, this.ReUrl);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Page.Request.QueryString["ID"] == null)
			{
				this.htmlOperatorName = "新增";
			}
            else if (!int.TryParse(this.Page.Request.QueryString["ID"], out this.m_GradeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
			if (!this.Page.IsPostBack)
			{
				this.AAbiuZJB();
			}
		}
	}
}