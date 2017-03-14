namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ShippingTypeSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            Hidistro.Core.Entities.SiteSettings masterSettings = Hidistro.Core.SettingsManager.GetMasterSettings(false);
            IList<ShippingModeInfo> shippingModes = ShoppingProcessor.GetShippingModes();
            if ((shippingModes != null) && (shippingModes.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择配送啊啊方式<span class=\"caret\"></span></button>");
                builder.AppendLine("<ul id=\"shippingTypeUl\" class=\"dropdown-menu\" role=\"menu\">");
                decimal total = 0m;//商品总额
                for (int i = 0; i < ShoppingCartItemInfo.Count; i++)
                {
                    total += ShoppingCartItemInfo[i].SubTotal;
                }
                bool isAllFreeShipping=false;
                if(masterSettings.SpecialOrderAmountType == "freeShipping")
                    isAllFreeShipping = total >= masterSettings.SpecialValue1 && masterSettings.SpecialValue1 > 0M;
                foreach (ShippingModeInfo info in shippingModes)
                {
                    decimal num = 0M;
                    if (this.ShoppingCartItemInfo.Count != this.ShoppingCartItemInfo.Count<Hidistro.Entities.Sales.ShoppingCartItemInfo>(a => a.IsfreeShipping))
                    {
                        num = ShoppingProcessor.CalcFreight(this.RegionId, this.Weight, info);
                        if (isAllFreeShipping)//如果达到了设置好的包邮金额,那么邮费全免
                        {
                            num = 0M;
                        }
                    }
                    string introduced5 = info.Name + "： ￥" + num.ToString("F2") + "(包邮)";
                    builder.AppendFormat("<li><a href=\"#\" name=\"{0}\" value=\"{2}\">{1}</a></li>", info.ModeId, introduced5, num.ToString("F2")).AppendLine();
                }
                builder.AppendLine("</ul>");
                writer.Write(builder.ToString());
            }
        }

        public int RegionId { get; set; }

        public IList<Hidistro.Entities.Sales.ShoppingCartItemInfo> ShoppingCartItemInfo { get; set; }

        public decimal Weight { get; set; }
    }
}

