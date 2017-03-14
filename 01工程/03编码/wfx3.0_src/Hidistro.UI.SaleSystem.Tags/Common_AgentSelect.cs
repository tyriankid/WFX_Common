namespace Hidistro.UI.SaleSystem.Tags
{
    using Entities.Members;
    using Core.Entities;
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

    public class Common_AgentSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (!Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.SelectServerAgent)
                return;
            DistributorsQuery agentQuery = new DistributorsQuery
            {
                PageIndex = 1,
                PageSize = 1000,
                IsAgent=-1,//包括天使和非天使
                IsServiceStore=1,
            };
            DbQueryResult agentsInfo = DistributorsBrower.GetDistributors(agentQuery);
            if (agentsInfo.TotalRecords > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(" <button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择服务门店<span class=\"caret\"></span></button>");
                builder.AppendLine("<ul id=\"agentsUl\" class=\"dropdown-menu\" role=\"menu\">");
                builder.AppendFormat("<li><a href=\"#\" name=\"{1}\" value=\"{1}\">{0}</a></li>", "无", 0).AppendLine();
                foreach (DataRow row in ((System.Data.DataTable)agentsInfo.Data).Rows)
                {
                    string userId = row["UserId"].ToString();
                    string StoreName = row["StoreName"].ToString();
                    builder.AppendFormat("<li><a href=\"#\" name=\"{1}\" value=\"{1}\">{0}</a></li>", StoreName, userId).AppendLine();
                }
                builder.AppendLine("</ul>");
                writer.Write(builder.ToString());
            }
        }
         

    }
}

