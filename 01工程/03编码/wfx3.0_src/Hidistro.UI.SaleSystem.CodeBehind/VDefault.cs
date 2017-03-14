using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.Entities.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Config;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true)]
    public class VDefault : VWeiXinOAuthTemplatedWebControl
    {
        private DataTable dtpromotion;
        private HtmlImage img;
        private HiImage imglogo;
        protected int itemcount;
        private Literal litattention;
        private Literal litdescription;
        private Literal litImgae;
        private Literal litActivity;
        private Literal litItemParams;
        private Literal litstorename;
        private Pager pager;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;
        private HtmlControl div_activity;//大图展示窗口

        private VshopTemplatedRepeater rptCategories1;
        private VshopTemplatedRepeater rptCategories2;
        private VshopTemplatedRepeater rptCategories3;
        private VshopTemplatedRepeater rptCategories4;
        private Literal ShopName;
        private Literal Gonggao;
        private Literal ProductCount;
        private VshopTemplatedRepeater rptProductqqg;
        private Literal Erweima;

        protected override void AttachChildControls()
        {
            if (SettingsManager.GetMasterSettings(true).VTheme.ToLower() == "common")
            {
                this.Page.Response.Redirect("index.aspx");
                return;
            }
            this.rptCategories = (VshopTemplatedRepeater)this.FindControl("rptCategories");
            this.rptProducts = (VshopTemplatedRepeater)this.FindControl("rptProducts");
            this.rptProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptProducts_ItemDataBound);
            this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
            this.img = (HtmlImage)this.FindControl("imgDefaultBg");
            this.pager = (Pager)this.FindControl("pager");
            this.litstorename = (Literal)this.FindControl("litstorename");
            this.litdescription = (Literal)this.FindControl("litdescription");
            this.litattention = (Literal)this.FindControl("litattention");
            this.imglogo = (HiImage)this.FindControl("imglogo");
            if (CustomConfigHelper.Instance.IsLogoOn == "false")
                imglogo.Visible = false;
            this.litImgae = (Literal)this.FindControl("litImgae");
            this.litActivity = (Literal)this.FindControl("litActivity");//大图展示页面
            this.litItemParams = (Literal)this.FindControl("litItemParams");

            //店铺推广码送过来的地址(参数：ReferralId)
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                //无传参，取COOKIE
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + cookie.Value);
                }
            }
            if (this.rptCategories.Visible)
            {
                DataTable brandCategories = CategoryBrowser.GetBrandCategories();
                this.itemcount = brandCategories.Rows.Count;
                if (brandCategories.Rows.Count > 0)
                {
                    this.rptCategories.DataSource = brandCategories;
                    this.rptCategories.DataBind();
                }
            }
            this.Page.Session["stylestatus"] = "3";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            PageTitle.AddSiteNameTitle(masterSettings.SiteName);
            this.litstorename.Text = masterSettings.SiteName;
            this.litdescription.Text = masterSettings.ShopIntroduction;
            if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
            {
                    this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
            }
            if (base.referralId <= 0)
            {
                HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie2 != null) && !string.IsNullOrEmpty(cookie2.Value))
                {
                    base.referralId = int.Parse(cookie2.Value);
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);
                }
            }
            else
            {
                HttpCookie cookie3 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if (((cookie3 != null) && !string.IsNullOrEmpty(cookie3.Value)) && (this.referralId.ToString() != cookie3.Value))
                {
                    this.Page.Response.Redirect("Default.aspx?ReferralId=" + this.referralId.ToString(), true);//跳转到自己的店铺
                }
            }
            IList<BannerInfo> allBanners = new List<BannerInfo>();
            allBanners = VshopBrowser.GetAllBanners();
            foreach (BannerInfo info in allBanners)
            {
                TplCfgInfo info2 = new NavigateInfo
                {
                    LocationType = info.LocationType,
                    Url = info.Url
                };
                string loctionUrl = "javascript:";
                if (!string.IsNullOrEmpty(info.Url))
                {
                    loctionUrl = info2.LoctionUrl;
                }
                string text = this.litImgae.Text;
                this.litImgae.Text = text + "<li><a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + info.ImageUrl + "\" title=\"" + info.ShortDesc + "\" alt=\"" + info.ShortDesc + "\" /></a></li>";
                //this.litActivity.Text = text + "<a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + info.ImageUrl + "\" title=\"" + info.ShortDesc + "\" alt=\"" + info.ShortDesc + "\" /></a>";
            }
            if (allBanners.Count == 0)
            {
                this.litImgae.Text = "<li><a id=\"ahref\"  href='javascript:'><img src=\"/Utility/pics/default.jpg\" title=\"\"  /></a></li>";
            }

            
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
                this.litdescription.Text = userIdDistributors.StoreDescription;
                this.litstorename.Text = userIdDistributors.StoreName;
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                {
                    this.imglogo.ImageUrl = userIdDistributors.Logo;
                }
                else if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
                {
                    this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
                }
                if (!string.IsNullOrEmpty(userIdDistributors.BackImage))
                {
                    this.litImgae.Text = "";
                    foreach (string str2 in userIdDistributors.BackImage.Split(new char[] { '|' }))
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            this.litImgae.Text = this.litImgae.Text + "<a ><img src=\"" + str2 + "\" title=\"\"  /></a>";
                        }
                    }
                }
            }
            this.dtpromotion = ProductBrowser.GetAllFull();
            if (this.rptProducts != null)
            {
                ProductQuery query = new ProductQuery
                {
                    PageSize = this.pager.PageSize,
                    PageIndex = this.pager.PageIndex,
                    SortBy = "DisplaySequence",
                    SortOrder = SortAction.Desc
                };
                 //查询条件处理
                switch (SettingsManager.GetMasterSettings(true).VTheme.ToLower())
                {
                    case "underwear":
                        this.pager.Visible = false;
                        query.PageSize = 1000;
                        query.PageIndex = 1;
                        query.SortBy = "ShowSaleCounts";
                        query.SortOrder = SortAction.Desc;
                        query.TypeId=1;
                        break;
                }
                DbQueryResult homeProduct = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), query);
                this.rptProducts.DataSource = homeProduct.Data;
                this.rptProducts.DataBind();
                this.pager.TotalRecords = homeProduct.TotalRecords;
                if (this.pager.TotalRecords <= this.pager.PageSize)
                {
                    this.pager.Visible = false;
                }
            }
            
            if (this.img != null)
            {
                this.img.Src = new VTemplateHelper().GetDefaultBg();
            }
            if (!string.IsNullOrEmpty(masterSettings.GuidePageSet))
            {
                this.litattention.Text = masterSettings.GuidePageSet;
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
            this.litItemParams.Text = str3 + "|" + masterSettings.ShopHomeName + "|" + masterSettings.ShopHomeDescription + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str4, "|好店推荐之", str5, "商城|一个购物赚钱的好去处|", HttpContext.Current.Request.Url });

            int userid = 18;
            string userFomartCode = userid.ToString("000000000000000");

            LoadProductTop();
            getImgBanner();//载入大图
            getMyCoupon();//获取首页赠送优惠券
            InitDataByTheme();
            distributorVisitCont();//更新店铺访问信息
        }

#region 主页获赠优惠券功能
        /// <summary>
        /// 进入主页获赠优惠券
        /// </summary>
        private void getMyCoupon()
        {
            DataTable allCoupons = CouponHelper.GetAllCoupons();//优惠券列表
            DataTable allCouponItemsClaimCode = CouponHelper.GetAllCouponItemsClaimCode();//所有已发送优惠券的claimcode
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                return;//如果当前没有登录则不执行该方法
            for (int i = 0; i < allCoupons.Rows.Count; i++)
            {
                if (allCoupons.Rows[i]["sendAtHomepage"].ToString() == "1")//如果需要在主页赠送,就开始判断该用户是否已获取过
                {
                    for(int o=0;o<allCouponItemsClaimCode.Rows.Count;o++)
                    {
                        string currentClaimCode = allCouponItemsClaimCode.Rows[o]["ClaimCode"].ToString();
                        bool isSend = currentMember.UserId.ToString() == currentClaimCode.TrimStart('0');
                        if (isSend)
                            return;//如果发送过,返回
                        else
                            continue;//如果没有发送,继续循环查找
                    }
                    //发送优惠券
                    int couponId=Convert.ToInt32(allCoupons.Rows[i]["CouponId"]);
                    string claimCode = currentMember.UserId.ToString("000000000000000");
                    CouponItemInfo item = new CouponItemInfo();
                    System.Collections.Generic.IList<CouponItemInfo> listCouponItem = new System.Collections.Generic.List<CouponItemInfo>();
                    item = new CouponItemInfo(couponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
                    listCouponItem.Add(item);
                    CouponHelper.SendClaimCodes(couponId, listCouponItem);
                    break;
                }
            }
        }
#endregion 
        /// <summary>
        /// 加载TOP类控件数据
        /// </summary>
        private void LoadProductTop()
        {
            foreach (Control c in base.Controls)
            {
                if (c is VshopTemplatedRepeater)
                {
                    if (c.ID.IndexOf("rptProductTop_") > -1)
                    {
                        BindProductTop(c);
                    }
                }
                foreach (Control c2 in c.Controls)
                {
                    if (c2 is VshopTemplatedRepeater)
                    {
                        if (c2.ID.IndexOf("rptProductTop_") > -1)
                        {
                            BindProductTop(c2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绑定TOP类控件
        /// </summary>
        private void BindProductTop(Control c)
        {
            //声明变量
            DataTable dtProductTop = null;
            
            //找到控件后,获取对应的数据集
            string topName = c.ID.Substring(c.ID.IndexOf("_")+1);
            ProductInfo.ProductTop pt = (ProductInfo.ProductTop)Enum.Parse(typeof(ProductInfo.ProductTop), topName);
            switch (pt)
            {
                case ProductInfo.ProductTop.New:
                case ProductInfo.ProductTop.Hot:
                case ProductInfo.ProductTop.Discount:
                    dtProductTop = ProductBrowser.GetHomeProductTop(3, pt);
                    break;
                case ProductInfo.ProductTop.MostLike:
                    dtProductTop = ProductBrowser.GetHomeProductTop(5, pt);
                    break;
                case ProductInfo.ProductTop.Activity:
                    dtProductTop = ProductBrowser.GetHomeProductTop(2, pt);
                    break;
                default:
                    //dtProductTop = ProductBrowser.GetHomeProductTop(3, pt);
                    break;
            }

            //数据集处理
            switch (SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                case "green":
                    dtProductTop.Columns.Add("styleName", typeof(string));
                    for (int i = 0; i < dtProductTop.Rows.Count; i++)
                    {
                        dtProductTop.Rows[i]["styleName"] = (i % 2 == 0) ? "imgstye_l" : "imgstye_r";
                    }
                    break;
                case "e0404":
                    for (int i = 1; i <= dtProductTop.Rows.Count; i++)
                    {
                        dtProductTop.Rows[i-1]["ProductName"] = ((i+4) % 5 == 0) ? dtProductTop.Rows[i-1]["ProductName"] : "";
                    }
                    break;
                case "style01":
                    switch (pt)
                    {
                        case ProductInfo.ProductTop.New:
                            dtProductTop = ProductBrowser.GetHomeProductTop(4, pt);
                            break;
                        case ProductInfo.ProductTop.Discount:
                            dtProductTop = ProductBrowser.GetHomeProductTop(3, pt);
                            break;
                        case ProductInfo.ProductTop.MostLike:
                            dtProductTop = ProductBrowser.GetHomeProductTop(6, pt);
                            break;
                    }
                    break;
                case "style02":
                case "style03":
                    switch (pt)
                    {
                        case ProductInfo.ProductTop.Category:
                            DataTable categories = CategoryBrowser.GetAllCategoriesRange(DistributorsBrower.GetCurrStoreProductRange());
                            if (categories.Rows.Count > 0)
                            {
                                int cateCount = categories.Rows.Count;//把总行数用变量储存起来,以免循环删除列后,rows总数会随时变动.
                                //只保留dt的前三行
                                for (int i = 0; i < cateCount; i++)
                                {
                                    if (categories.Rows.Count > 3)
                                    {
                                        categories.Rows.RemoveAt(3);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                //绑定
                                dtProductTop = categories;//ProductBrowser.GetHomeProductTop("3", pt,Convert.ToInt32( categories.Rows[0]["categoryid"]));
                                
                            }
                            break;
                        case ProductInfo.ProductTop.New:
                            dtProductTop = ProductBrowser.GetHomeProductTop(4, pt);
                            break;
                        case ProductInfo.ProductTop.MostLike:
                            dtProductTop = ProductBrowser.GetHomeProductTop(5, pt);
                            break;
                        case ProductInfo.ProductTop.Discount:
                            dtProductTop = ProductBrowser.GetHomeProductTop(3, ProductInfo.ProductTop.Hot);
                            break;
                    }
                    break;
                case "bytype":
                    break;
                
            }
            (c as VshopTemplatedRepeater).ItemDataBound += new RepeaterItemEventHandler(this.rptProductTop_ItemDataBound);
            //绑定
            (c as VshopTemplatedRepeater).DataSource = dtProductTop;
            (c as VshopTemplatedRepeater).DataBind();
        }


        /// <summary>
        /// 获取所有大图
        /// </summary>
        private void getImgBanner()
        {
            IList<BannerInfo> allImgBanners = new List<BannerInfo>();
            allImgBanners = VshopBrowser.GetAllImgBanners();
            int activityCount = allImgBanners.Count;//获取所有的活动数量

            for (int i = 0; i < allImgBanners.Count;i++ )
            {
                TplCfgInfo info2 = new NavigateInfo
                {
                    LocationType = allImgBanners[i].LocationType,
                    Url = allImgBanners[i].Url
                };
                string loctionUrl = "javascript:";
                if (!string.IsNullOrEmpty(allImgBanners[i].Url))
                {
                    loctionUrl = info2.LoctionUrl;
                }
                if (litActivity != null)
                {
                    //排版处理
                    string text = this.litActivity.Text;
                    int cellCounts = 0;//排版的数量基数
                    switch (SettingsManager.GetMasterSettings(true).VTheme.ToLower())
                    {
                        case "e0402":
                        case "e0403":
                            cellCounts = 2;
                            
                            if (activityCount % cellCounts != 0 && i == activityCount - 1)//如果是单数,并且到了循环的最后一条图片时,
                            {
                                this.litActivity.Text = text + "<li style='list-style:none;width:100%'><a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + allImgBanners[i].ImageUrl + "\" title=\"" + allImgBanners[i].ShortDesc + "\" alt=\"" + allImgBanners[i].ShortDesc + "\" /></a></li>";
                            }
                            else
                            {
                                this.litActivity.Text = text + "<li style='list-style:none'><a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + allImgBanners[i].ImageUrl + "\" title=\"" + allImgBanners[i].ShortDesc + "\" alt=\"" + allImgBanners[i].ShortDesc + "\" /></a></li>";
                            }
                            break;

                        default://默认样式:直接堆叠.
                            this.litActivity.Text = text + "<li style='list-style:none'><a  id=\"ahref\" href='" + loctionUrl + "'><img src=\"" + allImgBanners[i].ImageUrl + "\" title=\"" + allImgBanners[i].ShortDesc + "\" alt=\"" + allImgBanners[i].ShortDesc + "\" /></a></li>";
                            break;
                    }


                }

            }


            if (allImgBanners.Count == 0)
            {
                this.div_activity = (HtmlControl)this.FindControl("div_activity");
                if (div_activity!= null)
                {
                    div_activity.Visible = false;
                }
                
                //this.litActivity.Text = "<a id=\"ahref\"  href='javascript:'><img src=\"/Utility/pics/default.jpg\" title=\"\"  /></a>";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDefault.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (((e.Item.ItemIndex + 1) % 2) == 1)
                {
                    Literal literal = (Literal)e.Item.Controls[0].FindControl("litStart");
                    literal.Visible = true;
                }
                else if ((((e.Item.ItemIndex + 1) % 2) == 0) || ((e.Item.ItemIndex + 1) == this.itemcount))
                {
                    Literal literal2 = (Literal)e.Item.Controls[0].FindControl("litEnd");
                    literal2.Visible = true;
                }
                Literal literal3 = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal3.Text))
                {
                    literal3.Text = "<img class='sp-image' src='" + literal3.Text + "'/>";
                }
                else
                {
                    literal3.Text = "<img class='sp-image' src='/Storage/master/default.png'/>";
                }
            }
        }

        private void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
                string str = "";
                if (DataBinder.Eval(e.Item.DataItem, "MainCategoryPath") != null)
                {
                    str = DataBinder.Eval(e.Item.DataItem, "MainCategoryPath").ToString();
                }
                DataView defaultView = this.dtpromotion.DefaultView;
                if (!string.IsNullOrEmpty(str))
                {
                    defaultView.RowFilter = " ActivitiesType=0 ";
                    if (defaultView.Count > 0)
                    {
                        literal.Text = "<span class=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                    }
                    else
                    {
                        defaultView.RowFilter = " ActivitiesType= " + str.Split(new char[] { '|' })[0].ToString();
                        if (defaultView.Count > 0)
                        {
                            literal.Text = "<span class=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                        }
                    }
                }
            }
        }

        private void rptProductTop_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                liPromotiontBind(sender, e);

                VshopTemplatedRepeater vtr = (VshopTemplatedRepeater)e.Item.Controls[0].FindControl("inner");
                if (vtr != null)
                {
                    vtr.ItemDataBound += new RepeaterItemEventHandler(this.liPromotiontBind);
                    vtr.DataSource = ProductBrowser.GetHomeProductTop("3", ProductInfo.ProductTop.Category, Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "CategoryId")));
                    vtr.DataBind();
                    
                }
            }
        }

        private void liPromotiontBind(object sender, RepeaterItemEventArgs e)
        {
            Literal literal = (Literal)e.Item.Controls[0].FindControl("litpromotion");
            if (literal == null) return;
            string str = "";
            if (DataBinder.Eval(e.Item.DataItem, "MainCategoryPath") != null)
            {
                str = DataBinder.Eval(e.Item.DataItem, "MainCategoryPath").ToString();
            }
            DataView defaultView = this.dtpromotion.DefaultView;
            if (!string.IsNullOrEmpty(str))
            {
                defaultView.RowFilter = " ActivitiesType=0 ";
                if (defaultView.Count > 0)
                {
                    literal.Text = "<span id=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                }
                else
                {
                    defaultView.RowFilter = " ActivitiesType= " + str.Split(new char[] { '|' })[0].ToString();
                    if (defaultView.Count > 0)
                    {
                        literal.Text = "<span id=\"sale-favourable\"><i>满" + decimal.Parse(defaultView[0]["MeetMoney"].ToString()).ToString("0") + "</i><i>减" + decimal.Parse(defaultView[0]["ReductionMoney"].ToString()).ToString("0") + "</i></span>";
                    }
                }
            }
        }

        private void InitDataByTheme()
        {
            switch (SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                case "underwear"://内衣
                    this.ShopName = (Literal)this.FindControl("ShopName");
                    if (Globals.GetCurrentDistributorId()!=0)
                    {
                        this.ShopName.Text = DistributorsBrower.GetDistributorInfo(Globals.GetCurrentDistributorId()).StoreName;
                    }
                    if (this.imglogo.ImageUrl.Equals("/Utility/pics/headLogo.jpg"))
                    {
                        this.imglogo.ImageUrl = "/Templates/vshop/underwear/images/LOGO.jpg";
                    }
                    this.rptCategories1 = (VshopTemplatedRepeater)this.FindControl("rptCategories1");
                    this.rptCategories1.DataSource = CategoryBrowser.GetCategoriesByPruductType(4,2);//根据ProductType查询分类
                    this.rptCategories1.DataBind();
                    this.rptCategories2 = (VshopTemplatedRepeater)this.FindControl("rptCategories2");
                    this.rptCategories3 = (VshopTemplatedRepeater)this.FindControl("rptCategories3");
                    this.rptCategories3.DataSource = CategoryBrowser.GetCategoriesByPruductType(12,1);//根据ProductType查询分类
                    this.rptCategories3.DataBind();
                    this.rptCategories4 = (VshopTemplatedRepeater)this.FindControl("rptCategories4");
                    this.rptCategories4.DataSource = CategoryBrowser.GetCategoriesByPruductType(11,4);//根据ProductType查询分类
                    this.rptCategories4.DataBind();
                    this.rptProductqqg = (VshopTemplatedRepeater)this.FindControl("rptProductqqg");
                    ProductQuery query = new ProductQuery
                    {
                        PageSize = 6,
                        PageIndex = 1,
                        SortBy = "ShowSaleCounts",
                        SortOrder = SortAction.Desc,
                        TypeId = 2
                    };
                    this.rptProductqqg.DataSource = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), query).Data;
                    this.rptProductqqg.DataBind();
                    DataTable dt = ProductBrowser.GetHomeProductTop(0, ProductInfo.ProductTop.New);
                    this.ProductCount = (Literal)this.FindControl("ProductCount");
                    ProductCount.Text = dt.Rows[0][0]+"";
                    this.Gonggao = (Literal)this.FindControl("Gonggao");
                    IList<ArticleInfo> artlist = CommentBrowser.GetArticleList(4,1);
                    if (artlist.Count > 0)
                    {
                        this.Gonggao.Text = artlist[0].Content;
                    }
                    this.Erweima = (Literal)this.FindControl("Erweima");
                    if (Globals.GetCurrentDistributorId() == 0)
                    {
                        this.Erweima.Text = "<li><a href='QRcode.aspx?ReferralId='><i><img src='/Templates/vshop/underwear/images/iconfont-erweima.png' /></i><p>二维码</p></a></li>";
                    }
                    else
                    {
                        this.Erweima.Text = "<li><a href='QRcode.aspx?ReferralId=" + Globals.GetCurrentDistributorId() + "'><i><img src='/Templates/vshop/underwear/images/iconfont-erweima.png' /></i><p>二维码</p></a></li>";
                    }
                break;
            };
        }

        /// <summary>
        /// add:2015-11-27 增加分销商的店铺访的问量,载入一次主页+1
        /// </summary>
        private void distributorVisitCont()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null && currentMember.OpenId!=null)
            {
                //如果当前用户是微信用户,则增加一次访问量,(包括自己的点击)
                DistributorsBrower.UpdateDistributorVisitCount(currentMember.UserId, this.referralId);
            }
        }


    }
}

