using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
	public class CommissionsAllList : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnExportDetail;
		private string EndTime = "";
		private string OrderId = "";
		protected Pager pager;
		protected System.Web.UI.WebControls.Repeater reCommissions;
		private string StartTime = "";
		private string StoreName = "";
		protected WebCalendar txtEndTime;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected WebCalendar txtStartTime;
		protected System.Web.UI.WebControls.TextBox txtStoreName;
		private void BindData()
		{
			CommissionsQuery entity = new CommissionsQuery
			{
				EndTime = this.EndTime,
				StartTime = this.StartTime,
				StoreName = this.StoreName,
				OrderNum = this.OrderId,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
				SortBy = "CommId"
			};
			Globals.EntityCoding(entity, true);
			DbQueryResult commissions = VShopHelper.GetCommissions(entity);
			this.reCommissions.DataSource = commissions.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = commissions.TotalRecords;
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
				{
					this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
				{
					this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
				{
					this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
				}
				this.txtStoreName.Text = this.StoreName;
				this.txtOrderId.Text = this.OrderId;
				this.txtStartTime.Text = this.StartTime;
				this.txtEndTime.Text = this.EndTime;
			}
			else
			{
				this.OrderId = this.txtOrderId.Text;
				this.StoreName = this.txtStoreName.Text;
				this.StartTime = this.txtStartTime.Text;
				this.EndTime = this.txtEndTime.Text;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExportDetail.Click += new System.EventHandler(this.btnExportDetail_Click);
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void ReBind(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			queryStrings.Add("StoreName", this.txtStoreName.Text);
			queryStrings.Add("OrderId", this.txtOrderId.Text);
			queryStrings.Add("StartTime", this.txtStartTime.Text);
			queryStrings.Add("EndTime", this.txtEndTime.Text);
			queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}

        protected void btnExportDetail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
            {
                this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
            {
                this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
            {
                this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
            }
            DataTable dt = OrderHelper.GetCommissionDetails(this.OrderId,this.StoreName,this.StartTime,this.EndTime);
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            fields.Add("OrderId");
            list2.Add("订单编号");
            fields.Add("L1");
            list2.Add("一级佣金");
            fields.Add("L2");
            list2.Add("二级佣金");
            fields.Add("L3");
            list2.Add("三级佣金");
            fields.Add("productname");
            list2.Add("商品名");
            fields.Add("itemAdjustedPrice");
            list2.Add("商品单价");
            fields.Add("Quantity");
            list2.Add("商品数量");
            fields.Add("Time");
            list2.Add("时间");

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (System.Data.DataRow row in dt.Rows)
            {
                foreach (string str2 in fields)
                {
                    builder.Append(row[str2]).Append(",");
                    if (str2 == fields[list2.Count - 1])
                    {
                        
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=CommissionDetails.csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }
	}
}
