using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
    public class AddProducRange : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnAddBanner;
        protected System.Web.UI.WebControls.CheckBoxList cboxCategory;
        protected System.Web.UI.WebControls.CheckBoxList cboxBrand;
        protected System.Web.UI.WebControls.CheckBoxList cboxType;
        protected void btnAddBanner_Click(object sender, System.EventArgs e)
        {
            DataTable dtData = (DataTable)ViewState["dtData"];
            string strProductRange1 = string.Empty;
            string strProductRange2 = string.Empty;
            string strProductRange3 = string.Empty;
            foreach (ListItem li in this.cboxCategory.Items)
            {
                if (li.Selected)
                    strProductRange1 += string.Format("{0},", li.Value);
            }
            foreach (ListItem li in this.cboxBrand.Items)
            {
                if (li.Selected)
                    strProductRange2 += string.Format("{0},", li.Value);
            }
            foreach (ListItem li in this.cboxType.Items)
            {
                if (li.Selected)
                    strProductRange3 += string.Format("{0},", li.Value);
            }
            strProductRange1 = (string.IsNullOrEmpty(strProductRange1)) ? null : strProductRange1.TrimEnd(',');
            strProductRange2 = (string.IsNullOrEmpty(strProductRange2)) ? null : strProductRange2.TrimEnd(',');
            strProductRange3 = (string.IsNullOrEmpty(strProductRange3)) ? null : strProductRange3.TrimEnd(',');

            DataRow drData = null;
            if (dtData.Rows.Count == 0)
            {
                drData = dtData.NewRow();
                dtData.Rows.Add(drData);
            }
            else
                drData = dtData.Rows[0];
            drData["UserId"] = Request.QueryString["UserID"];
            drData["ProductRange1"] = strProductRange1;
            drData["ProductRange2"] = strProductRange2;
            drData["ProductRange3"] = strProductRange3;
            DistributorsBrower.CommitDistributorProductRange(dtData);
            this.CloseWindow();
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {

                this.cboxCategory.DataSource = CategoryBrowser.GetAllCategories();
                this.cboxCategory.DataTextField = "Name";
                this.cboxCategory.DataValueField = "CategoryId";
                this.cboxCategory.DataBind();

                this.cboxBrand.DataSource = CategoryBrowser.GetBrandCategories();
                this.cboxBrand.DataTextField = "BrandName";
                this.cboxBrand.DataValueField = "BrandId";
                this.cboxBrand.DataBind();

                this.cboxType.DataSource = ProductTypeHelper.GetProductTypes();
                this.cboxType.DataTextField = "TypeName";
                this.cboxType.DataValueField = "TypeId";
                this.cboxType.DataBind();

                DataTable dtData = DistributorsBrower.GetDistributorProductRangeByUserid(int.Parse(Request.QueryString["UserID"]));
                if (dtData.Rows.Count > 0)
                {
                    string strs=string.Empty;
                    if (dtData.Rows[0]["ProductRange1"] != DBNull.Value)
                    { 
                        strs=","+dtData.Rows[0]["ProductRange1"].ToString().Trim()+",";
                        foreach (ListItem li in this.cboxCategory.Items)
                        {
                            if (strs.IndexOf(li.Value) > -1)
                                li.Selected = true;
                        }
                    }
                    if (dtData.Rows[0]["ProductRange2"] != DBNull.Value)
                    {
                        strs = "," + dtData.Rows[0]["ProductRange2"].ToString().Trim() + ",";
                        foreach (ListItem li in this.cboxBrand.Items)
                        {
                            if (strs.IndexOf(li.Value) > -1)
                                li.Selected = true;
                        }
                    }
                    if (dtData.Rows[0]["ProductRange3"] != DBNull.Value)
                    {
                        strs = "," + dtData.Rows[0]["ProductRange3"].ToString().Trim() + ",";
                        foreach (ListItem li in this.cboxType.Items)
                        {
                            if (strs.IndexOf(li.Value) > -1)
                                li.Selected = true;
                        }
                    }
                }
                ViewState["dtData"] = dtData;
            }
        }
        private void Reset()
        {
        }
    }
}
