namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using System.Web;

    public class VCutDownList : VshopTemplatedWebControl
    {
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;
        private Literal litItemParams;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int num3;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptCutDownProducts");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            if (this.rptCategories != null)
            {
                IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 0x3e8);
                this.rptCategories.DataSource = maxSubCategories;
                this.rptCategories.DataBind();
            }
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            this.rptProducts.DataSource = ProductBrowser.GetCutDownProducts(new int?(this.categoryId), this.keyWord, num, num2, out num3, true);
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(num3.ToString());
            PageTitle.AddSiteNameTitle("砍价列表页");

            #region 微信分享内容配置
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
            }
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
            }
            string str4 = "";
            string str5 = (userIdDistributors == null) ? masterSettings.SiteName : userIdDistributors.StoreName;
            if (!string.IsNullOrEmpty(masterSettings.DistributorBackgroundPic))
            {
                str4 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
            }
            string strDes = masterSettings.ShopHomeDescription;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.BrandShow)
            {
                strDes = "正品低价砍回家，澳洲尖货就在考拉萌购！";
            }

            this.litItemParams.Text = str3 + "|" + masterSettings.ShopHomeName + "|" + strDes + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str4, "|好店推荐之", str5, "商城|" + strDes + "|", HttpContext.Current.Request.Url });
            #endregion
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCutDownList.html";
            }
            base.OnInit(e);
        }
    }
}

