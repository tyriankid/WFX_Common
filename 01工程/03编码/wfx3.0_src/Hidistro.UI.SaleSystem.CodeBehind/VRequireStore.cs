namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Config;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    public class VRequireStore : VWeiXinOAuthTemplatedWebControl
    {
        protected HtmlInputText locationText;
        protected Literal litRequireStoreInfo;
        protected Button btnAddInfo;
        protected Literal litStoreName;

        protected override void AttachChildControls()
        {
            litRequireStoreInfo = (Literal)this.FindControl("litRequireStoreInfo");
            btnAddInfo = (Button)this.FindControl("btnAddInfo");
            locationText = (HtmlInputText)this.FindControl("locationText");
            litStoreName = (Literal)this.FindControl("litStoreName");

            string listHtml = string.Empty;
            DataTable dtInfo = WxPoiHelper.GetRequireStoreInfo();
            foreach (DataRow row in dtInfo.Rows)
            {
                listHtml += "<li>";
                listHtml += string.Format("<img src='{0}'>",row["UserHead"]);
                listHtml += string.Format("<div><p><span>{0}</span>", row["UserName"]);
                listHtml += string.Format("<span>{0}</span></p>", row["addTime"]);
                listHtml += string.Format("<p><span>{0}</span></p></div>", row["location"]);
                listHtml += "</li>";
            }
            litRequireStoreInfo.Text = listHtml;

            litStoreName.Text = CustomConfigHelper.Instance.BusinessName;

            btnAddInfo.Click += this.btnAddInfo_Click;
        }

        protected override void OnInit(EventArgs e)
        {
            
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vRequireStore.html";
            }
            base.OnInit(e);
        }

        protected void btnAddInfo_Click(object sender, EventArgs e)
        {
            string locationText = "希望在<span style='color:orange'>" + GetText(this.locationText.Value) + "</span>开设新门店";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if(currentMember ==null)
            {
                this.GotoResourceNotFound("请登录");
                return;
            }
            if (WxPoiHelper.AddRequireStoreInfo(currentMember.UserId, locationText))
            {
                this.Page.Response.Redirect(this.Page.Request.Url.ToString());
            }
        }
        /// <summary>
        /// 将标签转换成TEXT
        /// </summary>
        public static string GetText(string strHtml)
        {
            if (strHtml == "")
            {
                strHtml = "";
            }
            else
            {
                strHtml = Regex.Replace(strHtml, "<[^>]*>", "");
                //替换空格
                //strHtml = Regex.Replace(strHtml,"\\s+", " ");
                strHtml = Regex.Replace(strHtml, "&nbsp;+", "");
                strHtml = Regex.Replace(strHtml, "'+", "");
                strHtml = Regex.Replace(strHtml, "\r\n+", "");
                strHtml = Regex.Replace(strHtml, " ", "");
            }
            return strHtml;
        }
       
    }
}

