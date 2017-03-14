namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using System.Data;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    using System.Web;

    public class VCutDownDetail : VWeiXinOAuthTemplatedWebControl
    {
        private int cutDownId = -1;
        private Image image;//用户头像
        private Literal litUserName;//用户名
        private Literal litMinPrice;//最底价
        private Literal litCurrentPrice;//当前价
        private Literal litCutDownPriceTotal;//被砍价总数
        private Literal litContent;//活动描述
        private HtmlInputHidden hidCutDownId;//隐藏域,商品id
        private HtmlInputHidden hidIsAlreadyCut;//自己是不是已经砍过价
        private HtmlInputHidden hidMemberId;//当前用户id
        private HtmlInputHidden hidCutDownDetailHtml;//页面载入时的
        private Literal litItemParams;



        protected override void AttachChildControls()
        {
            BindControls();
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();//当前用户
            CutDownInfo cutDown = new CutDownInfo();
            ProductInfo currentProduct = ProductBrowser.GetProduct(currentMember, Convert.ToInt32(this.Page.Request.QueryString["productId"]));//当前商品
            if (currentMember == null)
            {
                this.Page.Response.Redirect("UserLogin.aspx");
            }
            if (int.TryParse(this.Page.Request.QueryString["CutDownId"], out cutDownId))
            {
                cutDown = PromoteHelper.GetCutDown(cutDownId);
                if(cutDown==null)
                    GotoResourceNotFound("当前活动已结束!");
            }
            else
            {
                GotoResourceNotFound();
            }
            //当前用户id
            if (this.hidMemberId != null)
                this.hidMemberId.Value = currentMember.UserId.ToString();
            //砍价id(隐藏域)
            if (this.hidCutDownId != null)
                this.hidCutDownId.Value = this.cutDownId.ToString();
            //头像
            if (!string.IsNullOrEmpty(currentMember.UserHead))
                this.image.ImageUrl = currentMember.UserHead;
            //用户名
            if (this.litUserName != null)
                this.litUserName.Text = currentMember.UserName;
            //最底价
            if (this.litMinPrice != null)
                this.litMinPrice.Text = cutDown.MinPrice.ToString("F2");
            //当前价
            if (this.litCurrentPrice != null)
                this.litCurrentPrice.Text = cutDown.CurrentPrice.ToString("F2");
            //被砍总价
            if (this.litCutDownPriceTotal != null)
                this.litCutDownPriceTotal.Text = PromoteHelper.GetCutDownTotalPrice(cutDown.CutDownId).ToString("F2");
            //当前用户是否已经参加砍价
            if (this.hidIsAlreadyCut != null)
                this.hidIsAlreadyCut.Value = PromoteHelper.IsAlreadyCut(this.cutDownId, currentMember.UserId).ToString();
            //活动详情
            if (this.litContent != null)
                this.litContent.Text = HttpUtility.HtmlDecode(cutDown.Content);

            //获取最新列表
            CutDownDetailsQuery dQuery = new CutDownDetailsQuery()
            {
                PageSize = 30,
                PageIndex = 1,
                CutDownId = cutDown.CutDownId,
                SortBy = "cutTime",
                SortOrder = SortAction.Desc,
            };
            DataTable dtCutDownDetailList = (DataTable)PromoteHelper.GetCutDownDetailList(dQuery).Data;//获取砍价详情
            string detailList = "";
            MemberInfo rowMember = new MemberInfo();
            foreach (DataRow row in dtCutDownDetailList.Rows)
            {
                rowMember = MemberProcessor.GetMember( (Convert.ToInt32(row["memberId"])));
                if (rowMember == null)
                    continue;
                string MemberName = rowMember.UserName;
                string cutTime = DateTime.Parse(row["cutTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string imgUserHead = "<img src='" + rowMember.UserHead + "'/>";
                string cutPrice = ((decimal)row["cutDownPrice"]).ToString("F2");
                detailList += string.Format("<li><b>{0}</b><span>{1} 帮你砍了<i>{2}元</i></span></li>", imgUserHead, MemberName, cutPrice);
            }
            hidCutDownDetailHtml.Value = detailList;

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.GoodsPic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.GoodsPic;
            }
            if (currentProduct != null)
            {
                string strDes="快来砍价!";//"正品低价砍回家,澳洲尖货就在考拉萌购!;
                if(Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.BrandShow) strDes = "正品低价砍回家,澳洲尖货就在考拉萌购!";
                this.litItemParams.Text = string.Concat(new object[] { str3, "|", masterSettings.GoodsName, "|", masterSettings.GoodsDescription, "$", Globals.HostPath(HttpContext.Current.Request.Url), currentMember.UserHead, "|", strDes, "|", currentProduct.ProductName + "最低可以砍到" + cutDown.MinPrice.ToString("F0") + "元!还不快行动!", "|", HttpContext.Current.Request.Url });
            }
            PageTitle.AddSiteNameTitle("砍价详情页");
        }

        private void BindControls()
        {
            this.image = (Image)this.FindControl("image");
            this.litUserName = (Literal)this.FindControl("litUserName");
            this.litMinPrice = (Literal)this.FindControl("litMinPrice");
            this.litCurrentPrice = (Literal)this.FindControl("litCurrentPrice");
            this.litCutDownPriceTotal = (Literal)this.FindControl("litCutDownPriceTotal");
            this.hidCutDownId = (HtmlInputHidden)this.FindControl("hidCutDownId");
            this.hidIsAlreadyCut = (HtmlInputHidden)this.FindControl("hidIsAlreadyCut");
            this.hidMemberId = (HtmlInputHidden)this.FindControl("hidMemberId");
            this.hidCutDownDetailHtml = (HtmlInputHidden)this.FindControl("hidCutDownDetailHtml");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litContent = (Literal)this.FindControl("litContent");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCutDownDetail.html";
            }
            base.OnInit(e);
        }
    }
}

