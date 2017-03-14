namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Config;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

    public class Common_Footer : VshopTemplatedWebControl
    {
        private HyperLink hyperindex;
        private Literal lblStyle;
        private Literal litDistrbutorTitle;
        private Literal litDistrbutorUrl;
        private Panel paneldistributor;
        private Literal litAllProduct;
        private Literal litProductUrl;
        private HtmlInputHidden isTypeButtonHide;
        private HtmlInputHidden specialHideShow2;//特殊商户特殊隐藏处理

        protected override void AttachChildControls()
        {
            this.hyperindex = (HyperLink) this.FindControl("hyperindex");
            this.litDistrbutorTitle = (Literal) this.FindControl("litDistrbutorTitle");
            this.litDistrbutorUrl = (Literal) this.FindControl("litDistrbutorUrl");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.litDistrbutorUrl.Text = "ApplicationDescription.aspx";
            this.lblStyle = (Literal) this.FindControl("lblStyle");
            this.paneldistributor = (Panel) this.FindControl("paneldistributor");
            this.litAllProduct = (Literal)this.FindControl("litAllProduct");
            this.litProductUrl = (Literal)this.FindControl("litProductUrl");
            this.isTypeButtonHide = (HtmlInputHidden)this.FindControl("isTypeButtonHide");
            this.specialHideShow2 = (HtmlInputHidden)this.FindControl("specialHideShow2");

            if (this.Page.Session["stylestatus"] != null)
            {
                this.lblStyle.Text = this.Page.Session["stylestatus"].ToString();
            }
            this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_Showfootapply;
            if (currentMember != null)
            {
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    //this.litDistrbutorTitle.Text = "店铺管理";
                    this.litDistrbutorTitle.Text = CustomConfigHelper.Instance.DistributorType_Showfootmanage;
                    this.litDistrbutorUrl.Text = "DistributorValid.aspx";
                }
            }
            //迪曼（将分类按钮改为所有商品，并且改变跳转地址)
            if (CustomConfigHelper.Instance.ClassSkip)
            {
                this.litAllProduct.Text="分类";
                this.litProductUrl.Text="ProductSearch.aspx";
            }
            else
            {
                this.litAllProduct.Text = "所有商品";
                this.litProductUrl.Text = "ProductList.aspx";
            }
            //三座咖啡（隐藏掉分类按钮）
            this.isTypeButtonHide.Value = CustomConfigHelper.Instance.IsSanzuo ? "1" : "0";
            if (CustomConfigHelper.Instance.IsProLa)
            {
                this.specialHideShow2.Value = "proLa";
            }
            if (CustomConfigHelper.Instance.AutoShipping && CustomConfigHelper.Instance.AnonymousOrder)
            {
                this.specialHideShow2.Value = "sswf";
            }

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            MemberInfo info3 = MemberProcessor.GetCurrentMember();
            decimal expenditure = 0M;
            DistributorsInfo info4 = null;
            if ((info3 != null) && (info3.UserId > 0))
            {
                info4 = DistributorsBrower.GetUserIdDistributors(info3.UserId);
                expenditure = info3.Expenditure;
            }
            this.paneldistributor.Visible = (masterSettings.IsRequestDistributor && (expenditure >= masterSettings.FinishedOrderMoney)) || ((info4 != null) && (info4.UserId > 0));
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if ((this.hyperindex != null) && (currentDistributorId > 0))
            {
                this.hyperindex.NavigateUrl = "Default.aspx?ReferralId=" + currentDistributorId;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/skin-Common_Footer.html";
            }
            base.OnInit(e);
        }
    }
}

