namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    public class AddDeliveryScop : AdminPage
    {
        protected RegionSelector dropRegion;

        protected void Page_Load(object sender, EventArgs e)
        {
            int num = base.Request.QueryString["regionId"].ToInt();
            if (num > 0)
            {
                this.dropRegion.SetSelectedRegionId(new int?(num));
            }
        }
    }
}

