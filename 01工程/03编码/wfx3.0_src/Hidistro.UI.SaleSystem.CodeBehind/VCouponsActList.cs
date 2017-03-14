namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VCouponsActList : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptCouponsAct;

        protected override void AttachChildControls()
        {
            this.rptCouponsAct = (VshopTemplatedRepeater)this.FindControl("rptCouponsAct");
            this.rptCouponsAct.DataSource = CouponHelper.GetCouponsActNow() ;
            this.rptCouponsAct.DataBind();
            PageTitle.AddSiteNameTitle("优惠卷活动");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VCouponsActList.html";
            }
            base.OnInit(e);
        }
    }
}

