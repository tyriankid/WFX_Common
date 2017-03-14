namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
    using Hidistro.Entities.Members;
    using Hidistro.ControlPanel.Members;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Linq;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    public class VSelectStore : VWeiXinOAuthTemplatedWebControl
    {
        HtmlInputHidden hidAccess_token;
        HtmlInputHidden isSanZuo;
        HtmlInputHidden isClose;
        Literal litStoreName;
        Literal litBuildings;

        protected override void AttachChildControls()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //if (MemberProcessor.GetCurrentMember().UserId != 215)
            //    GotoResourceNotFound("前方高能预警,非测试人员请回避!");
            hidAccess_token = (HtmlInputHidden)this.FindControl("hidAccess_token");
            isSanZuo = (HtmlInputHidden)this.FindControl("isSanZuo");
            isClose = (HtmlInputHidden)this.FindControl("isClose");
            litStoreName = (Literal)this.FindControl("litStoreName");
            litBuildings = (Literal)this.FindControl("litBuildings");
            hidAccess_token.Value = Access_token.GetAccess_token();
            isClose.Value = masterSettings.isCloseStore.ToString();
            litStoreName.Text = CustomConfigHelper.Instance.BusinessName;
            isSanZuo.Value = CustomConfigHelper.Instance.IsSanzuo ? "1" : "0";
            isSanZuo.Value = CustomConfigHelper.Instance.IsProLa ? "2" : "0";
            /*
               <li>
                    <a>取水楼</a>
                    <span>环亚大厦</span>
                    <span>民生银行大厦</span>
                    <span>浦发银行大厦</span>
                    <span>伟业大厦</span>
                    <span>庭瑞大厦</span>
                    <span>IFC国际金融中心</span>
                    <span>登月大厦</span>
                    <span>良友大厦</span>
                    <span>福星国际商会大厦</span>
                    <span>银湖大厦</span>
                </li>
             */
           
            string buildingHtml = string.Empty;
            DataTable dtAllStreets = SalesHelper.GetStreetsInfo();
            if (CustomConfigHelper.Instance.IsSanzuo)
            {
                buildingHtml += "<li><a>武汉市 武昌区 水岸国际F6漫时区商圈</a>";
            }
            else
            {
                buildingHtml += "<li><a>深圳宝安区</a>";
            }
            
            foreach (DataRow row in dtAllStreets.Rows)
            {
                buildingHtml += "<span role='btnStreet' distributorId='" + row["clientUserId"].ToString() + "'>" + row["StreetName"].ToString() + "</span>";
            }
            buildingHtml += "</li>";
            litBuildings.Text = buildingHtml;
          
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vSelectStore.html";
            }
            base.OnInit(e);
        }


       
    }
}

