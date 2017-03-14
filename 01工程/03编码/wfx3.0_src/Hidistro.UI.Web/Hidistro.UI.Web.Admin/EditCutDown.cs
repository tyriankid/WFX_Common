namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.GroupBuy)]
    public class EditCutDown : AdminPage
    {
        protected Button btnUpdateCutDown;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected ProductCategoriesDropDownList dropCategories;
        protected GroupBuyProductDropDownList dropCutDownProduct;
        protected HourDropDownList drophours;
        private int cutDownId;
        protected HourDropDownList HourDropDownList1;
        protected Label lblPrice;
        protected kindeditor.Net.KindeditorControl txtContent;
        protected TextBox TxtCount;//限购总数量
        protected TextBox TxtMaxCount;//最大被砍价数
        protected TextBox TxtPerCutPrice;//每次砍价金额
        protected TextBox TxtMinPrice;//最低金额
        protected TextBox txtSearchText;
        protected TextBox txtSKU;


        private void btnUpdateCutDown_Click(object sender, EventArgs e)
        {
            int num2;
            int num3;
            decimal num4;
            CutDownInfo cutDown = new CutDownInfo
            {
                CutDownId = this.cutDownId
            };
            string str = string.Empty;
            if (this.dropCutDownProduct.SelectedValue > 0)
            {
                if ((PromoteHelper.GetCutDown(this.cutDownId).ProductId != this.dropCutDownProduct.SelectedValue.Value) && PromoteHelper.ProductCutDownExist(this.dropCutDownProduct.SelectedValue.Value))
                {
                    this.ShowMsg("已经存在此商品的砍价活动，并且活动正在进行中", false);
                    return;
                }
                cutDown.ProductId = this.dropCutDownProduct.SelectedValue.Value;
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请选择砍价商品");
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
                if ((DateTime.Compare(cutDown.EndDate, DateTime.Now) <= 0) && (cutDown.Status == CutDownStatus.Active))
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
            //砍价活动描述
            if (!string.IsNullOrEmpty(this.txtContent.Text))
            {
                cutDown.Content = txtContent.Text;
            }

            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
            }
            else if (PromoteHelper.UpdateCutDown(cutDown))
            {
                this.ShowMsg("编辑砍价活动成功", true);
            }
            else
            {
                this.ShowMsg("编辑团购活动失败", true);
            }
        }

        private void LoadCutDown(CutDownInfo cutDown)
        {
            this.TxtCount.Text = cutDown.Count.ToString();
            this.TxtMaxCount.Text = cutDown.MaxCount.ToString();
            this.TxtPerCutPrice.Text = cutDown.PerCutPrice.ToString("F2");
            this.TxtMinPrice.Text = cutDown.MinPrice.ToString("F2");
            this.txtContent.Text = Globals.HtmlDecode(cutDown.Content);

            this.calendarEndDate.SelectedDate = new DateTime?(cutDown.EndDate.Date);
            this.calendarStartDate.SelectedDate = new DateTime?(cutDown.StartDate.Date);
            this.drophours.SelectedValue = new int?(cutDown.StartDate.Hour);
            this.HourDropDownList1.SelectedValue = new int?(cutDown.EndDate.Hour);
            this.dropCutDownProduct.SelectedValue = new int?(cutDown.ProductId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int num;
                if (int.TryParse(base.Request["productId"], out num))
                {
                    string priceByProductId = PromoteHelper.GetPriceByProductId(num);
                    if (priceByProductId.Length > 0)
                    {
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{ ");
                        base.Response.Write("\"Status\":\"OK\",");
                        base.Response.Write(string.Format("\"Price\":\"{0}\"", decimal.Parse(priceByProductId).ToString("F2")));
                        base.Response.Write("}");
                        base.Response.End();
                    }
                }
            }
            else if (!int.TryParse(base.Request.QueryString["cutDownId"], out this.cutDownId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateCutDown.Click += new EventHandler(this.btnUpdateCutDown_Click);
                if (!base.IsPostBack)
                {
                    this.dropCutDownProduct.DataBind();
                    this.dropCategories.DataBind();
                    this.HourDropDownList1.DataBind();
                    this.drophours.DataBind();
                    CutDownInfo cutDown = PromoteHelper.GetCutDown(this.cutDownId);
                    if (PromoteHelper.GetCutDownOrderCount(this.cutDownId) > 0)
                    {
                        this.dropCutDownProduct.Enabled = false;
                    }
                    if (cutDown == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.LoadCutDown(cutDown);
                    }
                }
            }
        }
    }
}

