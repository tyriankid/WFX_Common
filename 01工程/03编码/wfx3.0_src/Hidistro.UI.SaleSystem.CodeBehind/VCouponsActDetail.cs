namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VCouponsActDetail : VWeiXinOAuthTemplatedWebControl
    {
        protected HtmlInputHidden txtID;
        protected HtmlInputHidden txtBgImg;

        protected override void AttachChildControls()
        {
            this.txtID = (HtmlInputHidden)this.FindControl("txtID");
            this.txtBgImg = (HtmlInputHidden)this.FindControl("txtBgImg");
            if (this.Page.Request.QueryString["ID"]!=null)
            {
                int ID = Convert.ToInt32(this.Page.Request.QueryString["ID"]);
                CouponsAct ca = CouponHelper.GetCouponsAct(ID);
                this.txtID.Value = ca.ID+"";
                this.txtBgImg.Value = ca.BgImg;
                PageTitle.AddSiteNameTitle("优惠卷活动");
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VCouponsActDetail.html";
            }
            base.OnInit(e);
        }
    }
}

