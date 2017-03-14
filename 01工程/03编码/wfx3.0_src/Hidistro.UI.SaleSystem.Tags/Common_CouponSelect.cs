namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_CouponSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            DataTable coupon = ShoppingProcessor.GetCoupon(this.CartTotal,this.CouponIds);
            StringBuilder builder = new StringBuilder();
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsProLa)
            {
                builder.AppendLine("<button  class=\"choose\" data-toggle=\"dropdown\">请选择一张优惠券<span class=\"caret\"></span></button>");
            }
            else
            {
                builder.AppendLine("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一张优惠券<span class=\"caret\"></span></button>");
            }
            
            builder.AppendLine("<ul class=\"dropdown-menu\" role=\"menu\">");
            foreach (DataRow row in coupon.Rows)
            {
                object[] args = new object[] { row["ClaimCode"], row["Name"],((decimal)row["Amount"]).ToString("F2"), ((decimal)row["DiscountValue"]).ToString("F2"),row["SenderId"]};
                builder.AppendFormat("<li><a href=\"#\" name=\"{0}\" senderid =\"{4}\" value=\"{3}\">{1}(满{2}减{3})</a></li>", args).AppendLine();
            }
            builder.AppendLine("</ul>");
            writer.Write(builder.ToString());
        }

        public decimal CartTotal { get; set; }
        public string CouponIds { get; set; }
    }
}

