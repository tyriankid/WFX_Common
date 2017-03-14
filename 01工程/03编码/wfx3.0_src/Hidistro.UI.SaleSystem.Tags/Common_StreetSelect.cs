namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_StreetSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if ((StreetsInfo != null) && (StreetsInfo.Rows.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择街道<span class=\"caret\"></span></button>");
                builder.AppendLine("<ul id=\"streetsUl\" class=\"dropdown-menu\" role=\"menu\">");
                foreach (DataRow row in StreetsInfo.Rows)
                {
                    string streetId = row["streetId"].ToString();
                    string streetName = row["streetName"].ToString();
                    builder.AppendFormat("<li><a href=\"#\" name=\"{1}\" value=\"{1}\">{0}</a></li>", streetName, streetId).AppendLine();
                }
                builder.AppendLine("</ul>");
                writer.Write(builder.ToString());
            }
        }

        public string RegionCode { get; set; }

        public DataTable StreetsInfo { get; set; }
    }
}

