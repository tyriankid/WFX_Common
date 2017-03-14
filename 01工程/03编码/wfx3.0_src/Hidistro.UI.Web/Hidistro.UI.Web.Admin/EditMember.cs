using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMember : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnEditUser;
		private int currentUserId;
		protected MemberGradeDropDownList drpMemberRankList;
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected FormatedTimeLabel lblRegsTimeValue;
		protected System.Web.UI.WebControls.Literal lblTotalAmountValue;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtprivateEmail;
		protected System.Web.UI.WebControls.TextBox txtQQ;
		protected System.Web.UI.WebControls.TextBox txtRealName;
        protected DropDownList DDLMemberActive;

		protected void btnEditUser_Click(object sender, System.EventArgs e)
		{
			MemberInfo member = MemberHelper.GetMember(this.currentUserId);
			member.GradeId = this.drpMemberRankList.SelectedValue.Value;
			member.RealName = this.txtRealName.Text.Trim();

			if (this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                //爽爽挝啡采购系统修改:member的TopRegionId改成是否被激活 0 或 1
                if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsBuyerNeedsToBeActive)
                {
                    member.TopRegionId = Convert.ToInt32(DDLMemberActive.SelectedValue);
                }
                else
                {
                    member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
                }
				
			}
			member.Address = Globals.HtmlEncode(this.txtAddress.Text);
			member.QQ = this.txtQQ.Text;
			member.Email = member.QQ + "@qq.com";
			member.CellPhone = this.txtCellPhone.Text;
			if (this.ValidationMember(member))
			{
				if (MemberHelper.Update(member))
				{
					this.ShowMsg("成功修改了当前会员的个人资料", true);
				}
				else
				{
					this.ShowMsg("当前会员的个人信息修改失败", false);
				}
			}
		}
		private void LoadMemberInfo()
		{
			MemberInfo member = MemberHelper.GetMember(this.currentUserId);
			if (member == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
                this.DDLMemberActive.SelectedIndex = member.TopRegionId == 1 ? 0 : 1;
				this.drpMemberRankList.SelectedValue = new int?(member.GradeId);
				this.lblLoginNameValue.Text = member.UserName;
				this.lblRegsTimeValue.Time = member.CreateDate;
				this.lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
				this.txtRealName.Text = member.RealName;
				this.txtAddress.Text = Globals.HtmlDecode(member.Address);
				this.rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
				this.txtQQ.Text = member.QQ;
				this.txtCellPhone.Text = member.CellPhone;
				this.txtprivateEmail.Text = member.Email;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
				if (!this.Page.IsPostBack)
				{
					this.drpMemberRankList.AllowNull = false;
					this.drpMemberRankList.DataBind();
					this.LoadMemberInfo();
				}
			}
		}
		private bool ValidationMember(MemberInfo member)
		{
			ValidationResults results = Validation.Validate<MemberInfo>(member, new string[]
			{
				"ValMember"
			});
			string msg = string.Empty;
			if (!results.IsValid)
			{
				foreach (ValidationResult result in (System.Collections.Generic.IEnumerable<ValidationResult>)results)
				{
					msg += Formatter.FormatErrorMessage(result.Message);
				}
				this.ShowMsg(msg, false);
			}
			return results.IsValid;
		}
	}
}
