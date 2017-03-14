namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyLogistics : VWeiXinOAuthTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string str = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(str))
            {
                base.GotoResourceNotFound("");
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str);
            Literal control = this.FindControl("litCompanyName") as Literal;
            Literal literal2 = this.FindControl("litLogisticsNumber") as Literal;
            control.SetWhenIsNotNull(orderInfo.ExpressCompanyName);
            literal2.SetWhenIsNotNull(orderInfo.ShipOrderNumber);
            PageTitle.AddSiteNameTitle("我的物流");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyLogistics.html";
            }
            base.OnInit(e);
        }
    }
}

