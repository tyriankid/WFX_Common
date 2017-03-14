namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VAddShippingAddressPro : VWeiXinOAuthTemplatedWebControl
    {
        private RegionSelector dropRegions;
        private System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;
        protected override void AttachChildControls()
        {
            //this.dropRegions = (RegionSelector) this.FindControl("dropRegions");
            this.specialHideShow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("specialHideShow");
            //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
            {
                specialHideShow.Value = "sswk";//爽爽挝啡
            }
            PageTitle.AddSiteNameTitle("添加收货地址");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Vaddshippingaddresspro.html";
            }
            base.OnInit(e);
        }
    }
}

