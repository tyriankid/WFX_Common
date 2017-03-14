namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VChooseProducts : VWeiXinOAuthTemplatedWebControl
    {
        private int categoryId;
        private string keyWord = string.Empty;
        private VshopTemplatedRepeater rpCategorys;
        private VshopTemplatedRepeater rpChooseProducts;
        private HtmlInputText txtkeywords;

        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.txtkeywords = (HtmlInputText) this.FindControl("keywords");
            this.rpChooseProducts = (VshopTemplatedRepeater) this.FindControl("rpChooseProducts");
            this.rpCategorys = (VshopTemplatedRepeater) this.FindControl("rpCategorys");
            this.DataBindSoruce();
        }

        private void DataBindSoruce()
        {
            //type:0正常显示店铺已上架的商品，1正常显示店铺未上架的商品，2显示所有出售状态的商品，3根据上架范围显示已上架的商品，4根据上架范围显示未上架的商品
            ProductInfo.ProductRanage productRanage = ProductInfo.ProductRanage.NormalUnSelect;
            bool bStoreProducAuto = Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto;
            bool bAgentProducRange = Hidistro.Core.SettingsManager.GetMasterSettings(true).EnableAgentProductRange;
            if (bStoreProducAuto)
            {
                productRanage = ProductInfo.ProductRanage.All;
            }
            else if (bAgentProducRange)
            {
                productRanage = ProductInfo.ProductRanage.RangeUnSelect;
            }

            int num;
            this.txtkeywords.Value = this.keyWord;
            this.rpCategorys.DataSource = CategoryBrowser.GetCategories();
            this.rpCategorys.DataBind();
            this.rpChooseProducts.DataSource = ProductBrowser.GetProductsEx(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, 1, 0x2710, out num, "DisplaySequence", "desc", productRanage);
            this.rpChooseProducts.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ChooseProducts.html";
            }
            base.OnInit(e);
        }
    }
}

