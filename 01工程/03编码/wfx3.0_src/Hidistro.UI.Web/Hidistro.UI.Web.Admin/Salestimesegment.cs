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
    public class Salestimesegment :AdminPage
	{

        protected HtmlInputHidden hidTablesY;
        protected HtmlInputHidden hidTablesX;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected WebCalendar calendarStart;
        protected DropDownList DDLservice;
        protected DropDownList DDLcategories;
        protected DropDownList DDLproduct;
        protected System.Web.UI.HtmlControls.HtmlImage imgChartOfSevenDay;


        private System.DateTime? startTime;
        private int storeid;
        private int categoryid;
        private int productid;


        private void BindList()
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
            {
                DateTime dtTarget = DateTime.Now;

                this.calendarStart.SelectedDate = dtTarget;
                this.startTime = this.calendarStart.SelectedDate;
            }
            if (startTime == null )
            {
                ShowMsg("请填写开始日期和结束日期!", false); return;
            }

            DataTable dtNums = SalesHelper.GetSaleTimesegment(startTime, storeid, categoryid, productid);
            string numsY = "";
            string numsX = "";
            string token = "t_";
            for (int i = 0; i < 24; i++)
            {
                numsX += i + ",";
                numsY += decimal.Parse(dtNums.Rows[0][token + i].ToString()).ToString("F0") + ",";
            }
            hidTablesX.Value =  numsX.TrimEnd(',');
            hidTablesY.Value =  numsY.TrimEnd(',') ;

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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeid"]))
                {
                    this.storeid = int.Parse(this.Page.Request.QueryString["storeid"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryid"]))
                {
                    this.categoryid = int.Parse(this.Page.Request.QueryString["categoryid"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productid"]))
                {
                    this.productid = int.Parse(this.Page.Request.QueryString["productid"]);
                }
                this.calendarStart.SelectedDate = this.startTime;

                #region 门店下拉框
                DataTable dtsender = ManagerHelper.GetAllManagers();
                ListItem itemFirst = new ListItem()
                {
                    Text ="全部",
                    Value = "0"
                };
                DDLservice.Items.Add(itemFirst);
                foreach (DataRow da in dtsender.Rows)
                {
                    ListItem item = new ListItem()
                    {
                        Text = da["UserName"].ToString() + "：" + da["agentName"].ToString(),
                        Value = da["userid"].ToString(),
                    };
                    if (!string.IsNullOrEmpty(da["clientUserId"].ToString()) && da["clientUserId"].ToString() != "0")
                        DDLservice.Items.Add(item);
                }
                DDLservice.SelectedIndex = 0;
                DDLservice.SelectedValue = storeid.ToString();
                #endregion

                #region 分类下拉框
                ListItem itemFirst1 = new ListItem()
                {
                    Text = "全部",
                    Value = "0"
                };
                DDLcategories.Items.Add(itemFirst1);
                IList<CategoryInfo> list = new List<CategoryInfo>();
                list = CatalogHelper.GetMainCategories();
                foreach (CategoryInfo info in list)
                {
                    ListItem item = new ListItem();
                    item.Text = info.Name;
                    item.Value = info.CategoryId.ToString();
                    DDLcategories.Items.Add(item);
                }
                DDLcategories.SelectedIndex = 0;
                DDLcategories.SelectedValue = categoryid.ToString();
                #endregion

                #region 商品下拉框
                ListItem itemFirst2 = new ListItem()
                {
                    Text = "全部",
                    Value = "0"
                };
                DDLproduct.Items.Add(itemFirst2);
                DataTable product = ManagerHelper.GetHishop_Product();
                foreach (DataRow row in product.Rows)
                {
                    ListItem item = new ListItem()
                    {
                        Text = row["ProductName"].ToString(),
                        Value = row["ProductId"].ToString(),
                    };
                    DDLproduct.Items.Add(item);
                }
                DDLproduct.SelectedIndex = 0;
                DDLproduct.SelectedValue = productid.ToString();
                #endregion
            }
            else
            {
                this.startTime = this.calendarStart.SelectedDate;
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
            queryStrings.Add("startTime", this.calendarStart.SelectedDate.ToString());//日期
            queryStrings.Add("storeid", this.DDLservice.SelectedValue.ToString());//门店id
            queryStrings.Add("categoryid", this.DDLcategories.SelectedValue.ToString());//分类id
            queryStrings.Add("productid", this.DDLproduct.SelectedValue.ToString());//商品id
            base.ReloadPage(queryStrings);
        }

	}
}
