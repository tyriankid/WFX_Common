using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SaleDetails)]
    public class fansTable : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnQuery;
		protected WebCalendar calendarEnd;
		protected WebCalendar calendarStart;
        protected DropDownList DDLservice;
        protected HtmlInputHidden hidTables;

		private System.DateTime? endTime;

		protected Pager pager;
		private System.DateTime? startTime;
        private int storeid;

		private void BindList()
		{
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]) && string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
            {
                DateTime dtTarget = DateTime.Now;
                DateTime FirstDay = dtTarget.AddDays(-DateTime.Now.Day + 1);

                this.calendarStart.SelectedDate = FirstDay;
                this.calendarEnd.SelectedDate = dtTarget;
                this.startTime = this.calendarStart.SelectedDate;
                this.endTime = this.calendarEnd.SelectedDate;
            }
            if (startTime == null || endTime == null)
            {
                ShowMsg("请填写开始日期和结束日期!",false);return;
            }
            if (startTime > endTime)
            {
                ShowMsg("开始日期不得晚于结束日期!", false); return;
            }


            string nums = "";
            DataSet dsFans = ManagerHelper.getStoreFans(startTime, endTime, storeid);

            foreach (DataTable dt in dsFans.Tables)
            {
                nums += dt.Rows[0]["dateNow"] +"," + dt.Rows[0]["newSubNum"] + "," + dt.Rows[0]["unSubNum"] + "," + dt.Rows[0]["realSubSql"] + "," + dt.Rows[0]["totalSubSql"] +";";
            }
            nums = nums.TrimEnd(';');

            hidTables.Value = nums;
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
        private void LoadParameters()
        {
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
				{
					this.startTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startTime"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
				{
					this.endTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endTime"]));
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeid"]))
                {
                    this.storeid = int.Parse( this.Page.Request.QueryString["storeid"]);
                }
                this.calendarStart.SelectedDate = this.startTime;
				this.calendarEnd.SelectedDate = this.endTime;

                DataTable dtsender = ManagerHelper.GetAllManagers();
                foreach (DataRow da in dtsender.Rows)
                {
                    ListItem item = new ListItem()
                    {
                        Text = da["UserName"].ToString() + "：" + da["agentName"].ToString(),
                        Value = da["clientuserid"].ToString(),
                    };
                    if (!string.IsNullOrEmpty(da["clientUserId"].ToString()) && da["clientUserId"].ToString() != "0")
                        DDLservice.Items.Add(item);
                }
                DDLservice.SelectedValue = storeid.ToString();
               
			}
			else
			{
                
				this.startTime = this.calendarStart.SelectedDate;
				this.endTime = this.calendarEnd.SelectedDate;
			}          			
		}
        protected void Page_Load(object sender, System.EventArgs e)
        {

            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindList();
			}
        }

		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("startTime", this.calendarStart.SelectedDate.ToString());//开始日期
			queryStrings.Add("endTime", this.calendarEnd.SelectedDate.ToString());//结束日期
            queryStrings.Add("storeid", this.DDLservice.SelectedValue.ToString());//门店id

			base.ReloadPage(queryStrings);
		}
	}
}
