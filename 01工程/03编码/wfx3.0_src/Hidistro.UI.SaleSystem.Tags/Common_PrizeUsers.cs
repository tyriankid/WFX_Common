namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_PrizeUsers : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Activity != null)
            {
                PrizeQuery page = new PrizeQuery {
                    ActivityId = this.Activity.ActivityId,
                    SortOrder = SortAction.Desc,
                    SortBy = "PrizeTime"
                };
                IOrderedEnumerable<PrizeRecordInfo> source = from a in VshopBrowser.GetPrizeList(page)
                    orderby a.PrizeTime descending
                    select a;
                StringBuilder builder = new StringBuilder();
                if ((source != null) && (source.Count<PrizeRecordInfo>() > 0))
                {
                    builder.Append("<table class=\"tabstyle\" width=\"100%\" border=\"0\" cellspacing=\"\" cellpadding=\"\">");
                    //<td style=\"text-align:center; width:30%\">获取人</td>
                    builder.Append("<tbody><tr><th>电话</th><th>奖项</th></tr>");
                    int i = 1;
                    foreach (PrizeRecordInfo info in source)
                    {
                        if (i <= 5)
                        {
                            i++;
                            if (!string.IsNullOrEmpty(info.CellPhone) && !string.IsNullOrEmpty(info.RealName))
                            {
                                builder.Append("<tr>");
                                //builder.AppendFormat("<td style=\"text-align:center; width:30%\">{0}<td>", info.RealName);
                                builder.AppendFormat("<td style=\"text-align:center; width:50%\">{0}</td>", this.ShowCellPhone(info.CellPhone));
                                builder.AppendFormat("<td style=\"text-align:center; width:50%\">{0}</td>", info.Prizelevel);
                                builder.Append("</tr>");
                            }
                        }
                    }
                    builder.Append("</tbody></table>");
                    writer.Write(builder.ToString());
                }
                else
                {
                    builder.AppendFormat("<p>暂无获奖名单！</p>", new object[0]);
                }
            }
        }

        private string ShowCellPhone(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                return Regex.Replace(phone, @"(?im)(\d{3})(\d{4})(\d{4})", "$1****$3");
            }
            return "";
        }

        public LotteryActivityInfo Activity { get; set; }
    }
}

