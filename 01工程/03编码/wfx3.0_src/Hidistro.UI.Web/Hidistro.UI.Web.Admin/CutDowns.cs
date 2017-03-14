namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public class CutDowns : AdminPage
    {
        protected LinkButton btnOrder;//保存排序
        protected Button btnSearch;//搜索
        protected Grid grdCutDownList;//砍价活动列表
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbtnDeleteCheck;//删除按钮
        protected Pager pager;
        protected Pager pager1;
        private string productName = string.Empty;
        protected TextBox txtProductName;

        private void BindCutDown()
        {
            CutDownQuery query = new CutDownQuery
            {
                ProductName = this.productName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "DisplaySequence",
                SortOrder = SortAction.Desc
            };
            DbQueryResult cutDownList = PromoteHelper.GetCutDownList(query);
            this.grdCutDownList.DataSource = cutDownList.Data;
            this.grdCutDownList.DataBind();
            this.pager.TotalRecords = cutDownList.TotalRecords;
            this.pager1.TotalRecords = cutDownList.TotalRecords;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdCutDownList.Rows)
            {
                int result = 0;
                TextBox box = (TextBox)row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int productId = (int)this.grdCutDownList.DataKeys[row.RowIndex].Value;
                    PromoteHelper.SwapCutDownSequence(productId, result);
                }
            }
            this.BindCutDown();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadHelpList(true);
        }

        private void grdCutDownList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CutDownInfo cutDown = PromoteHelper.GetCutDown((int)this.grdCutDownList.DataKeys[e.Row.RowIndex].Value);
                FormatedMoneyLabel label = (FormatedMoneyLabel)e.Row.FindControl("lblCurrentPrice");
                label.Money = cutDown.CurrentPrice;
            }
        }

        private void grdCutDownList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            CutDownInfo cutDown = PromoteHelper.GetCutDown((int)this.grdCutDownList.DataKeys[e.RowIndex].Value);
            /*
            if ((cutDown.Status == CutDownStatus.) || (groupBuy.Status == GroupBuyStatus.EndUntreated))
            {
                this.ShowMsg("团购活动正在进行中或结束未处理，不允许删除", false);
            }
            */
            if (PromoteHelper.DeleteCutDown((int)this.grdCutDownList.DataKeys[e.RowIndex].Value))
            {
                this.BindCutDown();
                this.ShowMsg("成功删除了选择的砍价活动", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int? nullable = null;
            foreach (GridViewRow row in this.grdCutDownList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    nullable = new int?(nullable.GetValueOrDefault());
                    int cutDownId = Convert.ToInt32(this.grdCutDownList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    CutDownInfo cutDown = PromoteHelper.GetCutDown(cutDownId);
                    nullable = new int?(nullable.GetValueOrDefault() + 1);
                    PromoteHelper.DeleteCutDown(cutDownId);
                }
            }
            if (nullable.HasValue)
            {
                this.BindCutDown();
                this.ShowMsg(string.Format("成功删除{0}条砍价活动", nullable), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的砍价活动", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                this.txtProductName.Text = this.productName;
            }
            else
            {
                this.productName = this.txtProductName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            this.grdCutDownList.RowDeleting += new GridViewDeleteEventHandler(this.grdCutDownList_RowDeleting);
            this.grdCutDownList.RowDataBound += new GridViewRowEventHandler(this.grdCutDownList_RowDataBound);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindCutDown();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", this.grdCutDownList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

