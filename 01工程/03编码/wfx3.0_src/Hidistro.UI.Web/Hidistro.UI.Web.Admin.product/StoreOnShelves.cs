using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public class StoreOnShelves : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnSaveInfo;
        protected Repeater SelectedProducts;
        protected int storeId =0;

        private void BindProduct()
        {
            this.SelectedProducts.DataSource = ProductHelper.GetTopStoreProductBaseInfo();
            this.SelectedProducts.DataBind();
        }

        private void btnSaveInfo_Click(object sender, System.EventArgs e)
        {
            //获取当前子账号门店
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            storeId = ManagerHelper.getClientUserIdBySenderId(currentManager.UserId);
            if (storeId <= 0)
            {
                this.CloseWindow();
            }
            bool flag = false;

            foreach (RepeaterItem rs in SelectedProducts.Items)
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox chb = ((System.Web.UI.HtmlControls.HtmlInputCheckBox)rs.FindControl("chkboxProductid"));
                if (chb.Checked)
                {
                    string productid = chb.Value;//获取选中项的productid
                    System.Collections.Generic.IList<int> tagsId = null;
                    System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary;
                    ProductInfo product = ProductHelper.GetProductDetails(Convert.ToInt32(productid), out dictionary, out tagsId);

                    product.StoreId = storeId;//当前商品为当前门店所有
                    //爽爽挝啡:新上架的门店商品默认为待审核状态
                    if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AnonymousOrder)
                    {
                        product.ReviewState = 1;//待审核
                    }
                    product.AddedDate = DateTime.Now;

                    switch (ProductHelper.AddProduct(product, product.Skus, dictionary, tagsId))
                    {
                        case ProductActionStatus.Success:
                            flag = true;
                            
                            break;
                        case ProductActionStatus.DuplicateName:
                            this.ShowMsg("添加商品失败，商品名称不能重复", false);
                            return;
                        case ProductActionStatus.DuplicateSKU:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;
                        case ProductActionStatus.SKUError:
                            this.ShowMsg("添加商品失败，商家编码不能重复", false);
                            return;
                        case ProductActionStatus.AttributeError:
                            this.ShowMsg("添加商品失败，保存商品属性时出错", false);
                            return;
                        case ProductActionStatus.ProductTagEroor:
                            this.ShowMsg("添加商品失败，保存商品标签时出错", false);
                            return;
                    }
                }

            }

            if (flag)
            {
                this.ShowMsg("添加商品成功", true);
                this.CloseWindow();
            }
            /*
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("MarketPrice");
            if (this.grdSelectedProducts.Rows.Count > 0)
            {
                decimal result = 0m;
                foreach (System.Web.UI.WebControls.GridViewRow row in this.grdSelectedProducts.Rows)
                {
                    int num = (int)this.grdSelectedProducts.DataKeys[row.RowIndex].Value;
                    System.Web.UI.WebControls.TextBox box = row.FindControl("txtProductName") as System.Web.UI.WebControls.TextBox;
                    System.Web.UI.WebControls.TextBox box2 = row.FindControl("txtProductCode") as System.Web.UI.WebControls.TextBox;
                    System.Web.UI.WebControls.TextBox box3 = row.FindControl("txtMarketPrice") as System.Web.UI.WebControls.TextBox;
                    if (!string.IsNullOrEmpty(box3.Text.Trim()) && !decimal.TryParse(box3.Text.Trim(), out result))
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(box3.Text.Trim()))
                    {
                        result = 0m;
                    }
                    System.Data.DataRow row2 = dt.NewRow();
                    row2["ProductId"] = num;
                    row2["ProductName"] = Globals.HtmlEncode(box.Text.Trim());
                    row2["ProductCode"] = Globals.HtmlEncode(box2.Text.Trim());
                    if (result >= 0m)
                    {
                        row2["MarketPrice"] = result;
                    }
                    dt.Rows.Add(row2);
                }
                if (ProductHelper.UpdateProductBaseInfo(dt))
                {
                    this.CloseWindow();
                }
                else
                {
                    this.ShowMsg("批量修改商品信息失败", false);
                }
                this.BindProduct();
            }
             */ 
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
    }
}
