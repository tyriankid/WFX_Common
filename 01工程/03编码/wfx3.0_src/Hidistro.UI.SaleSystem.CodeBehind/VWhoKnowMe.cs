namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using System.Data;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    using System.Web;
    using Entities.VShop;

    public class VWhoKnowMe : VWeiXinOAuthTemplatedWebControl
    {
        private Image image;//用户头像
        private Literal litUserName;//用户名
        private Literal litContent;//活动描述
        private Literal litCopyRight;//版权信息
        private Literal litLogo;//版权信息
        private HtmlInputHidden hidWKMId;//隐藏域,活动id
        private HtmlInputHidden hidMemberId;//隐藏域,用户
        private HtmlInputHidden hidPageType;//隐藏域,页面类型: 1:未设置题目,2:已设置好题目查看匹配人列表,3:已回答完题目查看自己的匹配度 4:guest开始答题
        private HtmlInputHidden hidIsHosterExist;
        private HtmlInputHidden hidIsGuestExist;
        private HtmlInputHidden hidBackImgUrl;
        private HtmlInputHidden hidAnswer;
        private HtmlInputHidden hidGuidePageUrl;

        private Literal ad1;
        private Literal ad2;

        private Literal litSbjListHtml;//题目列表html
        private Literal litSbjBarHtml;//题目进度条html
        private Literal litItemParams;

        private int hosterId = -1;
        private int guestId;



        protected override void AttachChildControls()
        {
            BindControls();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();//当前用户
            WKMInfo wkmInfo = new WKMInfo();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (currentMember == null)
            {
                this.Page.Response.Redirect("UserLogin.aspx");
            }
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["wkmId"]))
            {
                GotoResourceNotFound("当前活动不存在!");
            }
            else
            {
                wkmInfo = PromoteHelper.GetWKMInfo(new Guid(this.Page.Request.QueryString["wkmId"].ToString()));//当前活动
            }
            //根据url参数判断该页面类型
            string pageType = this.Page.Request.QueryString["type"];
            if (!string.IsNullOrEmpty(pageType))
            {
                hidPageType.Value = pageType;
                hosterId = Convert.ToInt32(this.Page.Request.QueryString["hosterId"]);
                guestId = Convert.ToInt32(this.Page.Request.QueryString["guestId"]);
                //类型和参数的非法判断
                if (pageType == "4" && hosterId == 0)//如果第四种类型下取不到hosterId,跳转到非法页面
                {
                    GotoResourceNotFound("缺少参数!");
                }
            }
            else
            {
                hidPageType.Value = "1";//如果是初始化的活动,则type为1:hoster开始设置题目
            }
            //判断该用户是否已经出完题目
            hidIsHosterExist.Value = PromoteHelper.isHosterExist(currentMember.UserId, wkmInfo.WKMId) ? "1" : "0";
            //判断该用户是否已经答过题目
            hidIsGuestExist.Value = PromoteHelper.isGuestExist(hosterId, currentMember.UserId, wkmInfo.WKMId) ? "1" : "0";
            //活动背景图和logo
            DataTable dtLogoImgAndBackImg = PromoteHelper.getBackImgUrl(wkmInfo.WKMId);
            litLogo.Text = string.Format("<img src='{0}' />", dtLogoImgAndBackImg.Rows[0]["logoUrl"].ToString()); 
            hidBackImgUrl.Value = dtLogoImgAndBackImg.Rows[0]["backImgUrl"].ToString();
            //版权信息
            litCopyRight.Text = PromoteHelper.getWKMCopyRight(wkmInfo.WKMId).Replace("\r\n","</br>");
            //关注引导页
            hidGuidePageUrl.Value = PromoteHelper.getGuidePageUrl(wkmInfo.WKMId);//masterSettings.GuidePageSet;
            //活动广告图和链接
            foreach (DataRow row in PromoteHelper.getAdImgAndUrls(wkmInfo.WKMId).Rows)
            {
                if (!string.IsNullOrEmpty(row["adlink1"].ToString()) && !string.IsNullOrEmpty(row["adimgurl1"].ToString()))
                    ad1.Text = string.Format("<a href='{0}'><img src='{1}'/></a>", row["adlink1"].ToString(), row["adimgurl1"].ToString());
                if (!string.IsNullOrEmpty(row["adlink2"].ToString()) && !string.IsNullOrEmpty(row["adimgurl2"].ToString()))
                    ad2.Text = string.Format("<a href='{0}'><img src='{1}'/></a>", row["adlink2"].ToString(), row["adimgurl2"].ToString());
            }

            //活动id
            if (this.hidWKMId != null)
                this.hidWKMId.Value = wkmInfo.WKMId.ToString();
            //用户id
            if (this.hidMemberId != null)
                this.hidMemberId.Value = currentMember.UserId.ToString();
            if (hosterId > 0 && pageType == "4")
            {
                MemberInfo hosterInfo = MemberProcessor.GetMember(hosterId);
                //hoster头像
                if (!string.IsNullOrEmpty(hosterInfo.UserHead))
                    this.image.ImageUrl = hosterInfo.UserHead;
                //hoster用户名
                if (this.litUserName != null)
                    this.litUserName.Text = hosterInfo.UserName;
                //获取答案,填写至隐藏域,用于判断guest是否选对
                string sbjOptId = string.Empty;
                foreach(DataRow row in PromoteHelper.GetHosterDetail(hosterId, wkmInfo.WKMId).Rows)
                {
                    sbjOptId += row["TitleId"] + "/" + row["OptionId"] + ";";
                }
                sbjOptId.TrimEnd(';');
                this.hidAnswer.Value = sbjOptId;
            }
            if ((hosterId < 0 && (pageType == "1" || string.IsNullOrEmpty(pageType))) || (pageType == "2"))
            {
                //自己头像
                if (!string.IsNullOrEmpty(currentMember.UserHead))
                    this.image.ImageUrl = currentMember.UserHead;
                //自己用户名
                if (this.litUserName != null)
                    this.litUserName.Text = currentMember.UserName;
            }
            

            //活动详情
            if (this.litContent != null)
                this.litContent.Text = HttpUtility.HtmlDecode(wkmInfo.TitleDescription);

            //题目进度bar生成
            string sbjBarHtml = string.Empty;
            for(int j=0;j<wkmInfo.SubjectInfo.WKMSubjectId.Count;j++)
            {
                int widRate = Convert.ToInt32((10.00 / wkmInfo.SubjectInfo.WKMSubjectId.Count) * 10);
                sbjBarHtml += "<li style='width:"+widRate+"%'><span class='num'>"+(j+1)+"</span></li>";
            }
            this.litSbjBarHtml.Text = sbjBarHtml;

            //题目列表html生成
            string sbjHtml = string.Empty;
            for (int i = 0; i < wkmInfo.SubjectInfo.WKMSubjectId.Count; i++)
            {
                sbjHtml += "<div name='sbjDiv' order='" + i + "'>";
                sbjHtml += string.Format("<div class='ttbox brad5' id='{0}' name='sbj'>{2} <h1>{1}</h1></div>", wkmInfo.SubjectInfo.WKMSubjectId[i], wkmInfo.SubjectInfo.SubjectContent[i], "<img src='" + wkmInfo.SubjectInfo.ImgUrl[i] + "' />");
                WKMOptionInfo optInfo = PromoteHelper.GetWKMOptInfoBySubjectId(wkmInfo.SubjectInfo.WKMSubjectId[i]);
                for (int j = 0; j < optInfo.WKMOptionId.Count; j++)
                {
                    sbjHtml += string.Format("<div class='nrbox brad5'><h1><input type='radio' name='opt' value='{0}' />{1}</h1></div>", optInfo.WKMOptionId[j], optInfo.OptionContent[j]);
                }
                sbjHtml += "</div>";
            }
            this.litSbjListHtml.Text = sbjHtml;


            
            string str3 = "";
            if (!string.IsNullOrEmpty(dtLogoImgAndBackImg.Rows[0]["ShareImgUrl"].ToString()))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + dtLogoImgAndBackImg.Rows[0]["ShareImgUrl"].ToString(); //Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            //设置分享参数
            if (wkmInfo != null)
            {
                string currentUrl = HttpContext.Current.Request.Url.ToString().Substring(0, HttpContext.Current.Request.Url.ToString().IndexOf('?'));
                switch (this.Page.Request.QueryString["type"])
                {
                    //还没有回答,单纯的邀请别人设置问题
                    case "1":
                    case "3":
                    case "4":
                    case null:
                        currentUrl = currentUrl + "?wkmId=" + wkmInfo.WKMId;
                        break;
                    //已经回答,邀请别人回答自己设置好的问题
                    case "2":
                        currentUrl = currentUrl + "?wkmId=" + wkmInfo.WKMId + "&type=4&hosterId=" + currentMember.UserId;
                        break;
                }

                this.litItemParams.Text = string.Concat(new object[] { 
                    str3, "|", 
                    wkmInfo.ShareTitle, "|", 
                    wkmInfo.ShareDescription, "$", 
                    Globals.HostPath(HttpContext.Current.Request.Url), currentMember.UserHead, "|", 
                    wkmInfo.TitleDescription, "|", 
                    wkmInfo.ShareDescription, "|", 
                    currentUrl 
                });
            }
            PageTitle.AddSiteNameTitle(wkmInfo.ShareTitle);
        }

        private void BindControls()
        {
            this.image = (Image)this.FindControl("image");
            this.litUserName = (Literal)this.FindControl("litUserName");
            this.hidWKMId = (HtmlInputHidden)this.FindControl("hidWKMId");
            this.hidMemberId = (HtmlInputHidden)this.FindControl("hidMemberId");
            this.hidPageType = (HtmlInputHidden)this.FindControl("hidPageType");
            this.hidIsHosterExist = (HtmlInputHidden)this.FindControl("hidIsHosterExist");
            this.hidIsGuestExist = (HtmlInputHidden)this.FindControl("hidIsGuestExist");
            this.hidBackImgUrl = (HtmlInputHidden)this.FindControl("hidBackImgUrl");
            this.hidAnswer = (HtmlInputHidden)this.FindControl("hidAnswer");
            this.hidGuidePageUrl = (HtmlInputHidden)this.FindControl("hidGuidePageUrl");
            this.litCopyRight = (Literal)this.FindControl("litCopyRight");
            this.litLogo = (Literal)this.FindControl("litLogo");
            this.ad1 = (Literal)this.FindControl("ad1");
            this.ad2 = (Literal)this.FindControl("ad2");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.litContent = (Literal)this.FindControl("litContent");
            this.litSbjListHtml = (Literal)this.FindControl("litSbjListHtml");
            this.litSbjBarHtml = (Literal)this.FindControl("litSbjBarHtml");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VWhoKnowMe.html";
            }
            base.OnInit(e);
        }
    }
}

