namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VGetCoupons : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptUseableCouponList;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num2;
            int num3;
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }
            string str2 = this.Page.Request.QueryString["status"];
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "1";
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int num = 0;
            int.TryParse(str2, out num);
            this.rptUseableCouponList = (VshopTemplatedRepeater)this.FindControl("rptUseableCouponList");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 20;
            }

            this.rptUseableCouponList.DataSource = CouponHelper.GetUseableCoupons(currentMember.UserId);
            //转换成DataTable类型,加一列适用范围说明.
            ((DataTable)this.rptUseableCouponList.DataSource).Columns.Add("txtUseRange");
            //对优惠券列表数据源遍历,判断categoryid和senderid是否有限制
            foreach (DataRow row in ((DataTable)this.rptUseableCouponList.DataSource).Rows)
            {
                string categoryName = "";
                string senderName = "";
                //判断categoryid
                if (row["categoryid"] != DBNull.Value && row["categoryid"].ToString() != "0")
                {
                    categoryName = CategoryBrowser.GetCategory(Convert.ToInt32(row["categoryid"])).Name;
                }
                //判断senderid
                if (row["senderid"] != DBNull.Value && row["senderid"].ToString() != "0" )
                {
                    senderName = DistributorsBrower.GetDistributorInfo(Convert.ToInt32(row["senderid"])).StoreName;
                }
                //拼接到适用范围字段内
                if (categoryName == "" && senderName == "")
                {
                    row["txtUseRange"] = "在任何商品和门店下使用";
                }
                else if (categoryName == "" && senderName != "")
                {
                    row["txtUseRange"] = string.Format("在{0}门店下使用", senderName);
                }
                else if (categoryName != "" && senderName == "")
                {
                    row["txtUseRange"] = string.Format("在{0}类商品下使用", categoryName);
                }
                else if (categoryName != "" && senderName != "")
                {
                    row["txtUseRange"] = string.Format("在{0}门店,{1}类商品下使用", senderName, categoryName);
                }
            }
            
            if (currentMember.UserName != "[堂食用户]")
            {
                this.rptUseableCouponList.DataBind();
            }
            this.txtTotal.SetWhenIsNotNull(CouponHelper.GetUseableCoupons(currentMember.UserId).Rows.Count.ToString());
            PageTitle.AddSiteNameTitle("领取优惠券");           
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vGetCoupons.html";
            }
            base.OnInit(e);
        }
    }
}

