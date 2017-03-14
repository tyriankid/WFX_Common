using System;
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
	public partial class AddActivities : AdminPage
	{

		private void AAbiuZJB(object obj, EventArgs eventArg)
		{
			int num = 0;
			if (!this.txtStartDate.SelectedDate.HasValue)
			{
                this.ShowMsg("��ѡ��ʼ���ڣ�", false);
				return;
			}
			if (!this.txtEndDate.SelectedDate.HasValue)
			{
                this.ShowMsg("��ѡ��������ڣ�", false);
				return;
			}
			if (this.txtStartDate.SelectedDate.Value.CompareTo(this.txtEndDate.SelectedDate.Value) > 0)
			{
                this.ShowMsg("��ʼ���ڲ������ڽ������ڣ�", false);
				return;
			}
			if (this.txtReductionMoney.Text.Trim() == "" || !int.TryParse(this.txtReductionMoney.Text.Trim(), out num))
			{
                this.ShowMsg("������������������", false);
				return;
			}
			if (this.txtMeetMoney.Text.Trim() == "" || !int.TryParse(this.txtMeetMoney.Text.Trim(), out num))
			{
                this.ShowMsg("������������������", false);
				return;
			}
			if (int.Parse(this.txtReductionMoney.Text.Trim()) >= int.Parse(this.txtMeetMoney.Text.Trim()))
			{
                this.ShowMsg("������ܴ��ڵ��������", false);
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
				activitiesInfo.ActivitiesType = 0;
				activitiesInfo.Type = 1;
			}
			else
			{
				int? selectedValue = this.dropCategories.SelectedValue;
				activitiesInfo.ActivitiesType = int.Parse(selectedValue.ToString());
				activitiesInfo.Type = 0;
			}
			DataTable dataTable = new DataTable();
			dataTable = (activitiesInfo.Type != 1 ? VShopHelper.GetType(1) : VShopHelper.GetType(0));
			if (dataTable.Rows.Count > 0)
			{
                this.ShowMsg("��Ŀ��ȫ������ͬʱ�μӻ��", false);
				return;
			}
			if (VShopHelper.AddActivities(activitiesInfo) > 0)
			{
                base.Response.Redirect("ActivitiesList.aspx");
				return;
			}
            this.ShowMsg("���ʧ��", false);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.IsTopCategory = true;
				this.dropCategories.IsUnclassified = false;
				this.dropCategories.DataBind();
			}
			this.btnAddActivity.Click += new EventHandler(this.AAbiuZJB);
		}
	}
}