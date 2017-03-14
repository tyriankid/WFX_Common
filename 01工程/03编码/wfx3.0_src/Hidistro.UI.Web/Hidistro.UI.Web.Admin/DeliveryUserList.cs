using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class DeliveryUserList : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnOK;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected WebCalendar calendarEndDate;
		protected WebCalendar calendarStartDate;

		private System.DateTime? endDate;
        protected Grid grdDeliveryMembers;

        private string StoreId;

		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Pager pager1;
		private System.DateTime? startDate;
        protected System.Web.UI.WebControls.TextBox txtUserName;

		private void BindProducts()
		{
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            
			this.LoadParameters();
            DeliveryMemberQuery entity = new DeliveryMemberQuery
			{
                UserName = this.txtUserName.Text,
				StartDate = this.startDate,
				EndDate = this.endDate,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "AddTime",
                StoreId = currentManager.ClientUserId
			};

			Globals.EntityCoding(entity, true);

            DbQueryResult memberList = StoreHelper.GetDeliveryMemberList(entity);

            this.grdDeliveryMembers.DataSource = memberList.Data;
            this.grdDeliveryMembers.DataBind();
            this.txtUserName.Text = entity.UserName;
            this.pager1.TotalRecords = (this.pager.TotalRecords = memberList.TotalRecords);
		}


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

		private void LoadParameters()
		{

			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
               this.txtUserName.Text = (this.Page.Request.QueryString["UserName"]);
            }
			
			this.calendarStartDate.SelectedDate = this.startDate;
			this.calendarEndDate.SelectedDate = this.endDate;
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("userName", Globals.UrlEncode(this.txtUserName.Text.Trim()));

			queryStrings.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(queryStrings);
		}
	}
}
