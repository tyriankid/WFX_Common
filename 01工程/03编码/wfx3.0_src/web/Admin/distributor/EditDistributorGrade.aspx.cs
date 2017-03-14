
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
		//[PrivilegeCheck(Entities.Store.Privilege.edit)]    // The parameters of the attribute are missing, because its assembly was not found
	public partial class EditDistributorGrade : AdminPage
	{
		protected string ReUrl = "distributorgradelist.aspx";

		private int m_GradeId;

		protected string htmlOperatorName = "编辑";

		private void AAbiuZJB()
		{
			if (this.m_GradeId > 0)
			{
				DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(this.m_GradeId);
				if (distributorGradeInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.txtName.Text = distributorGradeInfo.Name;
				TextBox str = this.txtCommissionsLimit;
				decimal commissionsLimit = distributorGradeInfo.CommissionsLimit;
				str.Text = commissionsLimit.ToString("F2");
				this.txtFirstCommissionRise.Text = distributorGradeInfo.FirstCommissionRise.ToString();
				this.txtSecondCommissionRise.Text = distributorGradeInfo.SecondCommissionRise.ToString();
				this.txtThirdCommissionRise.Text = distributorGradeInfo.ThirdCommissionRise.ToString();
				this.rbtnlIsDefault.SelectedIndex = (distributorGradeInfo.IsDefault ? 0 : 1);
				if (distributorGradeInfo.IsDefault)
				{
					this.rbtnlIsDefault.Enabled = false;
				}
				this.txtDescription.Text = distributorGradeInfo.Description;
				string ico = distributorGradeInfo.Ico;
				if (ico != "/utility/pics/grade.png")
				{
					this.uploader1.UploadedImageUrl = ico;
				}
			}
		}

        //private bool ABdcZUiWNfsGNbV(oArVIo9LHFJa(MemberInfo memberInfo)
        //{
        //    string[] strArrays = new string[] { "ValMember" };
        //    ValidationResults validationResults = Validation.Validate<MemberInfo>(memberInfo, strArrays);
        //    string empty = string.Empty;
        //    if (!validationResults.IsValid)
        //    {
        //        foreach (ValidationResult validationResult in (IEnumerable<ValidationResult>)validationResults)
        //        {
        //            empty = string.Concat(empty, Formatter.FormatErrorMessage(validationResult.Message));
        //        }
        //        this.ShowMsg(empty, false);
        //    }
        //    return validationResults.IsValid;
        //}

		protected void btnEditUser_Click(object sender, EventArgs e)
		{
			decimal num = new decimal(0, 0, 0, false, 1);
			decimal num1 = new decimal(0, 0, 0, false, 1);
			decimal num2 = new decimal(0, 0, 0, false, 1);
			decimal num3 = new decimal(0, 0, 0, false, 1);
			DistributorGradeInfo distributorGradeInfo = new DistributorGradeInfo();
			if (this.m_GradeId > 0)
			{
				distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(this.m_GradeId);
			}
			distributorGradeInfo.Name = this.txtName.Text.Trim();
			decimal.TryParse(this.txtCommissionsLimit.Text.Trim(), out num);
			decimal.TryParse(this.txtFirstCommissionRise.Text.Trim(), out num1);
			decimal.TryParse(this.txtSecondCommissionRise.Text.Trim(), out num2);
			decimal.TryParse(this.txtThirdCommissionRise.Text.Trim(), out num3);
			distributorGradeInfo.CommissionsLimit = num;
			distributorGradeInfo.FirstCommissionRise = num1;
			distributorGradeInfo.SecondCommissionRise = num2;
			distributorGradeInfo.ThirdCommissionRise = num3;
			distributorGradeInfo.IsDefault = (this.rbtnlIsDefault.SelectedIndex == 0 ? true : false);
			distributorGradeInfo.Description = this.txtDescription.Text.Trim();
			distributorGradeInfo.Ico = this.uploader1.UploadedImageUrl;
			if (DistributorGradeBrower.IsExistsMinAmount(this.m_GradeId, num))
			{
				this.ShowMsg("已存在相同佣金的分销商等级", false);
				return;
			}
			if (this.m_GradeId <= 0)
			{
				if (!DistributorGradeBrower.CreateDistributorGrade(distributorGradeInfo))
				{
					this.ShowMsg("分销商等级新增失败", false);
					return;
				}
				this.ShowMsgAndReUrl("成功新增了分销商等级", true, this.ReUrl);
				return;
			}
			if (!DistributorGradeBrower.UpdateDistributor(distributorGradeInfo))
			{
				this.ShowMsg("分销商等级修改失败", false);
				return;
			}
            if (base.Request.QueryString["reurl"] != null)
			{
                this.ReUrl = base.Request.QueryString["reurl"].ToString();
			}
            this.ShowMsgAndReUrl("成功修改了分销商等级", true, this.ReUrl);
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