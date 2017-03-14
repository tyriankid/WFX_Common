namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_ClearHeader : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_ClearHeader.html";
            }
            base.OnInit(e);
        }
    }
}

