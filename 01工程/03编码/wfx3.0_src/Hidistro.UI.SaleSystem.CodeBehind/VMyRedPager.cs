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
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMyRedPager : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptRedPagerList;
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
            this.rptRedPagerList = (VshopTemplatedRepeater)this.FindControl("rptRedPagerList");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 20;
            }

            DataTable userCouponList = CouponHelper.GetUserCoupons(currentMember.UserId, Convert.ToInt32(str2));
            //根据排序id和生成时间生成一个新的唯一字段,用于展示与核销优惠券
            userCouponList.Columns.Add("ShowCode");
            foreach (DataRow row in userCouponList.Rows)
            {
                string showCode = CouponHelper.GetShowCode(Convert.ToDateTime(row["GenerateTime"]), row["RID"].ToString());
                row["ShowCode"] = showCode;
            }

            this.rptRedPagerList.DataSource = userCouponList;
            this.rptRedPagerList.DataBind();
            this.txtTotal.SetWhenIsNotNull(userCouponList.Rows.Count.ToString());
            PageTitle.AddSiteNameTitle("我的优惠券");
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyRedPager.html";
            }
            base.OnInit(e);
        }
    }
}

