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
    [ParseChildren(true)]
    public class VProductSearchBuy : VWeiXinOAuthTemplatedWebControl
    {
        private HiImage imglogo;//店铺logo
        private Literal litTitle;//店铺名
        private Literal litDes;//店铺描述
        private Literal litMemberGradeInfo;//会员等级优惠信息
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private System.Web.UI.HtmlControls.HtmlInputHidden litCategoryId;
        private Literal litStoreList;//门店选择列表
        private int storeId = 0;

        public int rangeId = 1;//范围id 1:pc端,0:微信端,


        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imglogo = (HiImage)this.FindControl("imglogo");
            this.litTitle = (Literal)this.FindControl("litTitle");
            this.litDes = (Literal)this.FindControl("litDes");
            this.litMemberGradeInfo = (Literal)this.FindControl("litMemberGradeInfo");
            this.imgUrl = (HiImage)this.FindControl("imgUrl");
            this.litContent = (Literal)this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.litCategoryId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("litCategoryId");
            this.litCategoryId.SetWhenIsNotNull(this.categoryId.ToString());
            this.Page.Session["stylestatus"] = "4";

            this.storeId = !string.IsNullOrEmpty(this.Page.Request.QueryString["storeid"]) ? int.Parse(this.Page.Request.QueryString["storeid"]) : 0;

            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
            {
                this.Page.Response.Redirect("UserLogin.aspx");
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            int ReferralUserId=0;
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                ReferralUserId=Convert.ToInt32(cookie.Value);
            }
            
            Hidistro.Entities.Members.DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(ReferralUserId);
            Hidistro.Core.Entities.SiteSettings masterSettings = Hidistro.Core.SettingsManager.GetMasterSettings(false);
            //店铺logo
            if (userIdDistributors != null && !string.IsNullOrEmpty(userIdDistributors.Logo))
                this.imglogo.ImageUrl = userIdDistributors.Logo;
            else
            {
                if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
                {
                    this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
                }
            }
            //店铺名和店铺描述
            if (ReferralUserId == 0)//如果没有上级店铺
            {
                this.litTitle.Text = masterSettings.SiteName;
                this.litDes.Text = masterSettings.ShopIntroduction;
            }
            else
            {
                this.litTitle.Text = userIdDistributors.StoreName;
                this.litDes.Text = userIdDistributors.StoreDescription;
            }

            //会员等级优惠信息
            if (this.litMemberGradeInfo != null)
            {
                MemberGradeInfo gradeInfo = MemberHelper.GetMemberGrade(currentMember.GradeId);//会员等级信息
                string gradeName = gradeInfo.Name;
                string currentMemberGradeName = (currentMember == null) ? "" :
                string.Format("<span style='font-size:12px; background:#F90; color:#FFF; border-radius:3px; padding:3px 5px; margin-right:5px;'>{0}</span>"
                , gradeName);
                if (gradeInfo.Discount < 100)
                {
                    litMemberGradeInfo.Text = string.Format("{0}以下商品已获得{1}%折扣！", currentMemberGradeName, 100 - gradeInfo.Discount);
                }
                else
                {
                    litMemberGradeInfo.Text = string.Format("{0}以下商品均无打折", currentMemberGradeName, 100 - gradeInfo.Discount); ;
                }
            }
            switch (Hidistro.Core.SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                case "common":
                case "hotel":
                    Hidistro.Core.HiCache.Remove("DataCache-CategoriesRange");//清除分类缓存
                    //获取手机端所有商品的分类
                    DataTable dt = CategoryBrowser.GetCategoriesByRange(rangeId);//CategoryBrowser.GetCategoriesRange(ProductInfo.ProductRanage.NormalSelect);

                    int total = 0;
                    DataTable dt2 = new DataTable();
                    if (categoryId == 0)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            //获取手机端商品的第一个分类id
                            int FirstCategoryID = Convert.ToInt32(dt.Rows[0]["categoryId"]);
                            this.litCategoryId.SetWhenIsNotNull(FirstCategoryID.ToString());

                            dt2 = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0, FirstCategoryID, this.keyWord, 1, 20, out total, "ShowSaleCounts", "desc", "", rangeId, storeId);
                        }
                    }
                    else
                    {
                        dt2 = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, 0, categoryId, this.keyWord, 1, 20, out total, "ShowSaleCounts", "desc", "", rangeId, storeId);
                    }
                    //根据商品id判断是否包含其余规格,如果有,则新增一个字段存入规格号
                    dt2.Columns.Add("skuCounts");
                    dt2.Columns.Add("Quantity");
                    List<Hidistro.Entities.Sales.ShoppingCartInfo> cart = ShoppingCartProcessor.GetShoppingCartList();//获取购物车信息
                    foreach (DataRow row in dt2.Rows)
                    {
                        DataTable skus = ProductBrowser.GetSkus(Convert.ToInt32(row["ProductId"]));
                        row["skuCounts"] = skus.Rows.Count;
                        row["Quantity"] = 0;
                        //根据商品id获取购物车中已存在的数量,防止页面刷新后选中的数量遗失
                        foreach (Hidistro.Entities.Sales.ShoppingCartInfo info in cart)
                        {
                            foreach (Hidistro.Entities.Sales.ShoppingCartItemInfo itemInfo in info.LineItems)
                            {
                                if (Convert.ToInt32(row["ProductId"]) == itemInfo.ProductId)
                                {
                                    row["Quantity"] = itemInfo.Quantity;
                                }
                            }
                        }
                    }
                    this.rptCategories.DataSource = dt;
                    this.rptCategories.DataBind();

                    this.rptProducts.DataSource = dt2;
                    this.rptProducts.DataBind();
                    break;
                default:
                    this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
                    if (this.Page.Request.QueryString["TypeId"] != null)
                    {
                        this.rptCategories.DataSource = CategoryBrowser.GetCategoriesByPruductType(100, Convert.ToInt32(this.Page.Request.QueryString["TypeId"]));
                        this.rptCategories.DataBind();
                    }
                    else
                    {
                        IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategoriesRange(this.categoryId, 0x3e8, DistributorsBrower.GetCurrStoreProductRange());
                        this.rptCategories.DataSource = maxSubCategories;
                        this.rptCategories.DataBind();
                    }
                    PageTitle.AddSiteNameTitle("移动点餐");
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vProductSearchBuy.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal.Text))
                {
                    literal.Text = "<img src='" + literal.Text + "'></img>";
                }
                else
                {
                    literal.Text = "<img src='/Storage/master/default.png'></img>";
                }
            }
        }
        //private void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "fl")
        //    {
        //        int CategoryId = Convert.ToInt32(e.CommandArgument);
        //        this.Page.Response.Redirect("/Vshop/ProductSearch.aspx?categoryId=" + CategoryId);
        //    }
        //}



    }
}

