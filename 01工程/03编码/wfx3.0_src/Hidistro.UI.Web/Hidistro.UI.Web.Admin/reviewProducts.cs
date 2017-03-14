using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin.product
{
    public class reviewProducts : AdminPage
    {

        private string productIds = "";
        private string storeid = "";
        protected System.Web.UI.WebControls.TextBox txtOperationPrice;
        protected TrimTextBox txtPrices;
        protected System.Web.UI.WebControls.TextBox txtTargetPrice;
        protected DropDownList DDLStoreId;
        protected SelectGridSkuMemberPriceTable SelectGridSkuMemberPriceTable1;

        private System.Data.DataSet GetSkuPrices()
        {
            System.Data.DataSet set = null;
            XmlDocument document = new XmlDocument();
            System.Data.DataSet result;
            try
            {
                document.LoadXml(this.txtPrices.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if (list == null || list.Count == 0)
                {
                    result = null;
                    return result;
                }
                set = new System.Data.DataSet();
                System.Data.DataTable table = new System.Data.DataTable("skuPriceTable");
                table.Columns.Add("skuId");
                table.Columns.Add("costPrice");
                table.Columns.Add("salePrice");
                System.Data.DataTable table2 = new System.Data.DataTable("skuMemberPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("memberPrice");
                foreach (XmlNode node in list)
                {
                    System.Data.DataRow row = table.NewRow();
                    row["skuId"] = node.Attributes["skuId"].Value;
                    row["costPrice"] = (string.IsNullOrEmpty(node.Attributes["costPrice"].Value) ? 0m : decimal.Parse(node.Attributes["costPrice"].Value));
                    row["salePrice"] = decimal.Parse(node.Attributes["salePrice"].Value);
                    table.Rows.Add(row);
                    foreach (XmlNode node2 in node.SelectSingleNode("skuMemberPrices").ChildNodes)
                    {
                        System.Data.DataRow row2 = table2.NewRow();
                        row2["skuId"] = node.Attributes["skuId"].Value;
                        row2["gradeId"] = int.Parse(node2.Attributes["gradeId"].Value);
                        row2["memberPrice"] = decimal.Parse(node2.Attributes["memberPrice"].Value);
                        table2.Rows.Add(row2);
                    }
                }
                set.Tables.Add(table);
                set.Tables.Add(table2);
            }
            catch
            {
            }
            result = set;
            return result;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.storeid = this.Page.Request.QueryString["storeid"];

            productIds = ProductHelper.GetReviewProductIds(storeid);



            SelectGridSkuMemberPriceTable1.productIds = productIds;
            SelectGridSkuMemberPriceTable1.storeid = storeid;

            //门店的下拉框
            List<CategoryQuery> clientuser = new List<CategoryQuery>();
            clientuser = CouponHelper.Getaspnet_ManagersClientUserId();
            DDLStoreId.Items.Add("全部");
            foreach (CategoryQuery ct in clientuser)
            {
                ListItem item = new ListItem();
                item.Text = ct.UserName;
                item.Value = ct.ClientUserId.ToString();
                DDLStoreId.Items.Add(item);
            }

            this.DDLStoreId.SelectedIndexChanged += new System.EventHandler(this.DDLStoreId_SelectedIndexChanged);

            if (!this.Page.IsPostBack)
            {

                
                this.DDLStoreId.SelectedValue = storeid;
                


                //if (!string.IsNullOrEmpty(productIds))
                //{
                //    this.Page.Response.Redirect("reviewProducts.aspx?productids=" + productIds);
                //}
                

            }
        }


        private void DDLStoreId_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productids", this.productIds);
            queryStrings.Add("storeid", DDLStoreId.SelectedValue);
            base.ReloadPage(queryStrings);
        }

        [System.Web.Services.WebMethod]
        public static string passReview(string ProductIds,string RefuseReason,string Type)
        {
            string backJson = string.Empty;
            if (ProductHelper.GoReview(ProductIds, RefuseReason, Type))
            {
                backJson = ProductHelper.GetReviewProductIds().ToString();
            }
            else
            {
                backJson = "false";
            }

            return backJson;
        }

        [System.Web.Services.WebMethod]
        public static string refuseReview(string ProductIds, string refuseReason)
        {
            string backJson = string.Empty;
            if (ProductHelper.MutiProductSubmmitReview(ProductIds))
            {
                backJson = "true";
            }
            else
            {
                backJson = "false";
            }

            return backJson;
        }
    }
}
