namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.GroupBuy)]
    public class AddCutDown : AdminPage
    {

        protected Button btnAddCutDown;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected ProductCategoriesDropDownList dropCategories;
        protected GroupBuyProductDropDownList dropCutDownProduct;
        protected HourDropDownList drophours;
        protected HourDropDownList HourDropDownList1;
        protected Label lblPrice;
        protected TextBox txtContent;
        protected TextBox TxtCount;//限购总数量
        protected TextBox TxtMaxCount;//最大被砍价数
        protected TextBox TxtPerCutPrice;//每次砍价金额
        protected TextBox TxtMinPrice;//最低金额
        protected TextBox TxtCurrentPrice;//起始价格
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void btnAddCutDown_Click(object sender, EventArgs e)
        {
            int num2;
            int num3;
            decimal num4;
            CutDownInfo cutDown = new CutDownInfo();
            string str = string.Empty;
            if (this.dropCutDownProduct.SelectedValue > 0)
            {
                if (PromoteHelper.ProductCutDownExist(this.dropCutDownProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
                    return;
                }
                cutDown.ProductId = this.dropCutDownProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择团购商品");
            }
            if (!this.calendarStartDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择开始日期");
            }
            if (!this.calendarEndDate.SelectedDate.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择结束日期");
            }
            else
            {
                cutDown.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
                if (DateTime.Compare(cutDown.EndDate, DateTime.Now) < 0)
                {
                    str = str + Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
                }
                else if (DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value), cutDown.EndDate) >= 0)
                {
                    str = str + Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
                }
                else
                {
                    cutDown.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
                }
            }
            //每次砍价金额
            if (!string.IsNullOrEmpty(this.TxtPerCutPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.TxtPerCutPrice.Text.Trim(), out num))
                {
                    cutDown.PerCutPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("每次砍价金额填写格式不正确");
                }
            }
            //最低金额
            if (!string.IsNullOrEmpty(this.TxtMinPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.TxtMinPrice.Text.Trim(), out num))
                {
                    cutDown.MinPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("最低金额填写格式不正确");
                }
            }
            //最大砍价次数
            if (int.TryParse(this.TxtMaxCount.Text.Trim(), out num2))
            {
                cutDown.MaxCount = num2;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("最大砍价次数数量不能为空，只能为整数");
            }
            //限购总数量
            if (int.TryParse(this.TxtCount.Text.Trim(), out num3))
            {
                cutDown.Count = num3;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("限购总数量不能为空，只能为整数");
            }
            //最大购买数量不得小于商品的库存

            //起始价格
            if (!string.IsNullOrEmpty(this.TxtCurrentPrice.Text))
            {
                decimal num;
                if (decimal.TryParse(this.TxtCurrentPrice.Text.Trim(), out num))
                {
                    cutDown.CurrentPrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("起始金额填写格式不正确");
                }
            }

            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
            }
            else
            {
                cutDown.Content = this.txtContent.Text;
                if (PromoteHelper.AddCutDown(cutDown))
                {
                    this.ShowMsg("添加砍价活动成功", true);
                }
                else
                {
                    this.ShowMsg("添加砍价活动失败", true);
                }
            }
        }

        private void DoCallback()
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            string str = base.Request.QueryString["action"];
            if (str.Equals("getGroupBuyProducts"))
            {
                ProductQuery query = new ProductQuery();
                if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                {
                    int num;
                    int.TryParse(base.Request.QueryString["categoryId"], out num);
                    if (num > 0)
                    {
                        query.CategoryId = new int?(num);
                        query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
                    }
                }
                string str2 = base.Request.QueryString["sku"];
                string str3 = base.Request.QueryString["productName"];
                query.Keywords = str3;
                query.ProductCode = str2;
                query.SaleStatus = ProductSaleStatus.OnSale;
                DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(query);
                if ((groupBuyProducts == null) || (groupBuyProducts.Rows.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{\"Status\":\"OK\",");
                    builder.AppendFormat("\"Product\":[{0}]", this.GenerateBrandString(groupBuyProducts));
                    builder.Append("}");
                    base.Response.Write(builder.ToString());
                }
            }
            base.Response.End();
        }

        private string GenerateBrandString(DataTable tb)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in tb.Rows)
            {
                builder.Append("{");
                builder.AppendFormat("\"ProductId\":\"{0}\",\"ProductName\":\"{1}\"", row["ProductId"], Uri.EscapeDataString(row["ProductName"].ToString()));
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
            {
                this.DoCallback();
            }
            this.btnAddCutDown.Click += new EventHandler(this.btnAddCutDown_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropCutDownProduct.DataBind();
                this.HourDropDownList1.DataBind();
                this.drophours.DataBind();
            }
        }
    }
         
}

