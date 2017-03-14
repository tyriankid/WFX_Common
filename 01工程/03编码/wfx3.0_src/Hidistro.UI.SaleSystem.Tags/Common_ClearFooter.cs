using Hidistro.UI.Common.Controls;
using System;
namespace Hidistro.UI.SaleSystem.Tags
{
    public class Common_ClearFooter : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            
        }
        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_ClearFooter.html";
            }
            base.OnInit(e);
        }
    }
}

