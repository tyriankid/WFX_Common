namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class WgwWapArticleDetail : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor aFront;
        private HtmlAnchor aNext;
        private Common_ArticleRelative ariticlative;
        private int articleId;
        private Label lblFront;
        private Label lblFrontTitle;
        private Label lblNext;
        private Label lblNextTitle;
        private FormatedTimeLabel litArticleAddedDate;
        private Literal litArticleContent;
        private Literal litArticleDescription;
        private Literal litArticleTitle;
        private HtmlInputHidden txtCatgoryId;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
            {
                base.GotoResourceNotFound("");
            }
            this.txtCatgoryId = (HtmlInputHidden) this.FindControl("txtCatgoryId");
            this.litArticleAddedDate = (FormatedTimeLabel) this.FindControl("litArticleAddedDate");
            this.litArticleContent = (Literal) this.FindControl("litArticleContent");
            this.litArticleDescription = (Literal) this.FindControl("litArticleDescription");
            this.litArticleTitle = (Literal) this.FindControl("litArticleTitle");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            this.ariticlative = (Common_ArticleRelative) this.FindControl("list_Common_ArticleRelative");
            if (!this.Page.IsPostBack)
            {
                ArticleInfo article = CommentBrowser.GetArticle(this.articleId);
                if ((article != null) && article.IsRelease)
                {
                    if (this.txtCatgoryId != null)
                    {
                        this.txtCatgoryId.Value = article.CategoryId.ToString();
                    }
                    PageTitle.AddSiteNameTitle(article.Title);
                    if (!string.IsNullOrEmpty(article.MetaKeywords))
                    {
                        MetaTags.AddMetaKeywords(article.MetaKeywords, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(article.MetaDescription))
                    {
                        MetaTags.AddMetaDescription(article.MetaDescription, HiContext.Current.Context);
                    }
                    this.litArticleTitle.Text = article.Title;
                    this.litArticleDescription.Text = article.Description;
                    string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { this.articleId });
                    this.litArticleContent.Text = article.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
                    this.litArticleAddedDate.Time = article.AddedDate;
                    ArticleInfo info2 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Front", article.CategoryId);
                    if ((info2 != null) && (info2.ArticleId > 0))
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = "ArticleDetails.aspx?ArticleId=" + info2.ArticleId;
                            this.lblFrontTitle.Text = info2.Title;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    ArticleInfo info3 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Next", article.CategoryId);
                    if ((info3 != null) && (info3.ArticleId > 0))
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = "ArticleDetails.aspx?ArticleId=" + info3.ArticleId;
                            this.lblNextTitle.Text = info3.Title;
                        }
                    }
                    else if (this.lblNext != null)
                    {
                        this.lblNext.Visible = false;
                    }


                    //如果该文章绑定了优惠券,则自动领取优惠券
                    if (article.CouponId > 0)
                    {
                       
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        if (!CouponHelper.CheckUserIsCoupon(currentMember.UserId, article.CouponId))
                        {
                            //如果还没有领取过,开始领取优惠券
                            int number;
                            string claimCode = string.Empty;
                            claimCode += article.CouponId + "|" + currentMember.UserId;
                            claimCode = claimCode.PadLeft(15, 'w');//w代表文章列表获取的
                            CouponItemInfo item = new CouponItemInfo();
                            System.Collections.Generic.IList<CouponItemInfo> listCouponItem = new System.Collections.Generic.List<CouponItemInfo>();
                            item = new CouponItemInfo(article.CouponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
                            listCouponItem.Add(item);
                            CouponHelper.SendClaimCodes(article.CouponId, listCouponItem);
                            this.Page.Response.Write("<script>alert('恭喜您成功领取了一张优惠券!')</script>");
                        }
                        else
                        {
                            //this.Page.Response.Write("<script>alert('请勿重复领取!')</script>");
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-WgwArticleDetail.html";
            }
            base.OnInit(e);
        }
    }
}

