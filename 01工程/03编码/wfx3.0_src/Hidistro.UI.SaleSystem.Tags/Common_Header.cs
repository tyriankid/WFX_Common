namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Config;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_Header : VshopTemplatedWebControl
    {
        private System.Web.UI.HtmlControls.HtmlInputHidden specialShow;
        protected override void AttachChildControls()
        {
            this.specialShow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("specialShow");

            if (CustomConfigHelper.Instance.IsProLa) this.specialShow.Value = "proLa";
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Header.html";
            }
            base.OnInit(e);
        }
    }
}

