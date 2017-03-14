namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;

    public class shippingStreets : AdminPage
    {
        protected RegionSelector dropRegion;
        protected Pager pager;
        protected Pager pager1;
        protected Grid grdStreetsInfo;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.TextBox txtStreetName;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidRegionCode;

        private void BindData()
        {
            StreetInfoQuery query = this.GetStreetQuery();
            DbQueryResult streetInfos = SalesHelper.GetStreetInfo(query);
            //根据取出来的regionId获取完整的省市区名
            DataTable dtStreet = (DataTable)streetInfos.Data;
            dtStreet.Columns.Add("regionName");
            foreach (DataRow row in dtStreet.Rows)
            {
                row["regionName"] = Hidistro.Entities.RegionHelper.GetFullRegion(Convert.ToInt32(row["regionCode"])," ");
            }
            grdStreetsInfo.DataSource = streetInfos.Data;
            grdStreetsInfo.DataBind();
            this.pager.TotalRecords = streetInfos.TotalRecords;
            this.pager1.TotalRecords = streetInfos.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReloadStreetInfos();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int num = base.Request.QueryString["regionId"].ToInt();
            if (num > 0)
            {
                this.dropRegion.SetSelectedRegionId(new int?(num));
            }
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            this.grdStreetsInfo.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdStreetsInfo_RowDeleting);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
        }

        private void grdStreetsInfo_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string streetId = this.grdStreetsInfo.DataKeys[e.RowIndex].Value.ToString();
            if (!SalesHelper.DeleteStreetInfo(streetId.ToString()))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了一个街道信息", true);
            }
        }

        private void ReloadStreetInfos()
        {
            System.Collections.Specialized.NameValueCollection queryStrings = new System.Collections.Specialized.NameValueCollection();

            queryStrings.Add("StreetName", this.txtStreetName.Text);
            queryStrings.Add("RegionCode", this.hidRegionCode.Value);
            base.ReloadPage(queryStrings);
        }

        [System.Web.Services.WebMethod]
        public static string AddStreetInfo(string regionCode,string streetName)
        {
            if (SalesHelper.AddStreetInfo(regionCode,streetName))//更新成功
            {
                return string.Format("success");
            }
            else
            {
                return string.Format("fail");
            }
        }


        private StreetInfoQuery GetStreetQuery()
        {
            StreetInfoQuery query = new StreetInfoQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StreetName"]))
            {
                query.streetName = base.Server.UrlDecode(this.Page.Request.QueryString["StreetName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RegionCode"]))
            {
                query.regionCode = base.Server.UrlDecode(this.Page.Request.QueryString["RegionCode"]);
            }
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            return query;
        }


    }
}

