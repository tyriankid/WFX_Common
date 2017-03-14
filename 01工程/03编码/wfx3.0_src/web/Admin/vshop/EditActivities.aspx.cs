using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;

namespace Hidistro.UI.Web.Admin.vshop
{
	public partial class EditActivities : AdminPage
	{
		 int m_ActivitiesId;





		private void AAbiuZJB(int num)
		{
			IList<ActivitiesInfo> activitiesInfo = VShopHelper.GetActivitiesInfo(num.ToString());
			if (activitiesInfo.Count > 0)
			{
				this.txtName.Text = activitiesInfo[0].ActivitiesName;
				this.txtDescription.Text = activitiesInfo[0].ActivitiesDescription;
				WebCalendar str = this.txtEndDate;
				DateTime endTIme = activitiesInfo[0].EndTIme;
                str.Text = endTIme.ToString("yyyy-MM-dd");
				WebCalendar webCalendar = this.txtStartDate;
				DateTime startTime = activitiesInfo[0].StartTime;
                webCalendar.Text = startTime.ToString("yyyy-MM-dd");
				TextBox textBox = this.txtMeetMoney;
				decimal meetMoney = activitiesInfo[0].MeetMoney;
                textBox.Text = meetMoney.ToString("0.00");
				TextBox str1 = this.txtReductionMoney;
				decimal reductionMoney = activitiesInfo[0].ReductionMoney;
                str1.Text = reductionMoney.ToString("0.00");
				this.dropCategories.SelectedValue = new int?(activitiesInfo[0].ActivitiesType);
			}
		}

		private void btnEditActivity_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			decimal num1 = new decimal(0);
			if (!this.txtStartDate.SelectedDate.HasValue)
			{
                this.ShowMsg("请选择开始日期！", false);
				return;
			}
			if (!this.txtEndDate.SelectedDate.HasValue)
			{
                this.ShowMsg("请选择结束日期！", false);
				return;
			}
			if (this.txtStartDate.SelectedDate.Value.CompareTo(this.txtEndDate.SelectedDate.Value) > 0)
			{
                this.ShowMsg("开始日期不能晚于结束日期！", false);
				return;
			}
			if (this.txtReductionMoney.Text.Trim() == "")
			{
                this.ShowMsg("减免金额请输入整数！", false);
				return;
			}
			if (!decimal.TryParse(this.txtReductionMoney.Text.Trim(), out num))
			{
                this.ShowMsg("减免金额请输入整数", false);
				return;
			}
			if (this.txtMeetMoney.Text.Trim() == "")
			{
                this.ShowMsg("满足金额请输入整数！", false);
				return;
			}
			if (!decimal.TryParse(this.txtMeetMoney.Text.Trim(), out num1))
			{
                this.ShowMsg("满足金额请输入整数", false);
				return;
			}
			if (decimal.Parse(this.txtReductionMoney.Text.Trim()) >= decimal.Parse(this.txtMeetMoney.Text.Trim()))
			{
                this.ShowMsg("减免金额不能大于等于满足金额！", false);
				return;
			}
			ActivitiesInfo activitiesInfo = new ActivitiesInfo()
			{
				ActivitiesName = this.txtName.Text.Trim(),
				ActivitiesDescription = this.txtDescription.Text.Trim(),
				StartTime = this.txtStartDate.SelectedDate.Value,
				EndTIme = this.txtEndDate.SelectedDate.Value,
				MeetMoney = decimal.Parse(this.txtMeetMoney.Text.Trim()),
				ReductionMoney = decimal.Parse(this.txtReductionMoney.Text.Trim())
			};
            if (this.dropCategories.SelectedValue.ToString() == "0" || this.dropCategories.SelectedValue.ToString() == "")
			{
				activitiesInfo.Type = 1;
				activitiesInfo.ActivitiesType = 0;
			}
			else
			{
				activitiesInfo.Type = 0;
				int? selectedValue = this.dropCategories.SelectedValue;
				activitiesInfo.ActivitiesType = int.Parse(selectedValue.ToString());
			}
			activitiesInfo.ActivitiesId = this.m_ActivitiesId;
			DataTable dataTable = new DataTable();
			dataTable = (activitiesInfo.Type != 1 ? VShopHelper.GetType(1) : VShopHelper.GetType(0));
			if (dataTable.Rows.Count > 0)
			{
                this.ShowMsg("类目和全部不能同时参加活动！", false);
				return;
			}
			if (VShopHelper.UpdateActivities(activitiesInfo))
			{
                this.ShowMsg("修改成功", true);
				return;
			}
            this.ShowMsg("修改失败", false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            this.m_ActivitiesId = base.GetUrlIntParam("activitiesid");
			this.btnEditActivity.Click += new EventHandler(this.btnEditActivity_Click);
			if (!this.Page.IsPostBack)
			{
				if (this.m_ActivitiesId == 0)
				{
                    this.Page.Response.Redirect("ActivitiesList.aspx");
					return;
				}
				this.dropCategories.IsTopCategory = true;
				this.dropCategories.IsUnclassified = false;
				this.dropCategories.DataBind();
				this.AAbiuZJB(this.m_ActivitiesId);
			}
		}
	}
}