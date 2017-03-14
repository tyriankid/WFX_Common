namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Config;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VSubmmitOrderPro : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor aLinkToShipping;
        private int buyAmount;
        private Common_CouponSelect dropCoupon;
        private Common_UserRedPagerSelect dropRedPager;
        private Common_ShippingTypeSelect dropShippingType;
        private Common_StreetSelect dropStreets;//街道选择
        private Common_AgentSelect dropAgents;//天使服务门店选择
        private HtmlInputControl groupbuyHiddenBox;
        private int groupBuyId;
        private HtmlInputControl countdownHiddenBox;
        private int countDownId;
        private HtmlInputControl cutdownHiddenBox;
        private int cutDownId;

        private HtmlInputHidden hiddenCartTotal;
        private Literal litAddAddress;
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litExemption;
        private Literal litOrderTotal;
        private Literal litTotalPoint;//所需积分
        private Literal litProductTotalPrice;
        private Literal litShipTo;
        private string productSku;
        private HtmlInputHidden regionId;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptCartProducts;//商品
        private VshopTemplatedRepeater rptCartGifts;//礼品
        private HtmlInputHidden selectShipTo;
        private HtmlInputHidden isStreetEnable;//是否打开街道选择功能
        private HtmlInputHidden isStreetMatch;//消费者的所属区域是否包含配送范围内的街道
        private HtmlInputHidden couponRecharge;//是否开启充值送优惠券功能
        private HtmlInputHidden isSelectAgentEnable;//是否打开选择天使门店功能
        private HtmlInputHidden claimcode;
        private HtmlInputHidden usercount;

        protected override void AttachChildControls()
        {
            this.litShipTo = (Literal)this.FindControl("litShipTo");
            this.litCellPhone = (Literal)this.FindControl("litCellPhone");
            this.litAddress = (Literal)this.FindControl("litAddress");
            this.rptCartProducts = (VshopTemplatedRepeater)this.FindControl("rptCartProducts");//商品
            this.rptCartGifts = (VshopTemplatedRepeater)this.FindControl("rptCartGifts");//礼品
            this.dropShippingType = (Common_ShippingTypeSelect)this.FindControl("dropShippingType");
            this.dropCoupon = (Common_CouponSelect)this.FindControl("dropCoupon");
            this.dropRedPager = (Common_UserRedPagerSelect)this.FindControl("dropRedPager");
            this.dropStreets = (Common_StreetSelect)this.FindControl("dropStreets");//街道下拉框
            this.dropAgents = (Common_AgentSelect)this.FindControl("dropAgents");//天使下拉框
            this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
            this.litTotalPoint = (Literal)this.FindControl("litTotalPoint");//所需积分
            this.isSelectAgentEnable = (HtmlInputHidden)this.FindControl("isSelectAgentEnable");
            this.hiddenCartTotal = (HtmlInputHidden)this.FindControl("hiddenCartTotal");
            this.aLinkToShipping = (HtmlAnchor)this.FindControl("aLinkToShipping");
            this.groupbuyHiddenBox = (HtmlInputControl)this.FindControl("groupbuyHiddenBox");
            this.countdownHiddenBox = (HtmlInputControl)this.FindControl("countdownHiddenBox");
            this.cutdownHiddenBox = (HtmlInputControl)this.FindControl("cutdownHiddenBox");
            this.couponRecharge = (HtmlInputHidden)this.FindControl("couponRecharge");
            this.usercount = (HtmlInputHidden)this.FindControl("usercount");
            this.rptAddress = (VshopTemplatedRepeater)this.FindControl("rptAddress");
            this.selectShipTo = (HtmlInputHidden)this.FindControl("selectShipTo");
            this.regionId = (HtmlInputHidden)this.FindControl("regionId");
            Literal literal = (Literal)this.FindControl("litProductTotalPrice");
            this.litExemption = (Literal)this.FindControl("litExemption");
            this.litAddAddress = (Literal)this.FindControl("litAddAddress");
            this.isStreetEnable = (HtmlInputHidden)this.FindControl("isStreetEnable");
            this.isStreetMatch = (HtmlInputHidden)this.FindControl("isStreetMatch");
            this.claimcode = (HtmlInputHidden)this.FindControl("claimcode");
            this.litExemption.Text = "0.00";
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                                         orderby item.IsDefault
                                         select item;
            this.rptAddress.DataBind();
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }

            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
                this.regionId.SetWhenIsNotNull(info.RegionId.ToString());
            }
            this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + HttpContext.Current.Request.Url.ToString() + "'";
            if (CustomConfigHelper.Instance.IsProLa) this.litAddAddress.Text = " href='/Vshop/AddShippingAddressPro.aspx?returnUrl=" + HttpContext.Current.Request.Url.ToString() + "'";
            if ((shippingAddresses == null) || (shippingAddresses.Count == 0))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/AddShippingAddress.aspx?returnUrl=" + HttpContext.Current.Request.Url.ToString());
            }
            else
            {
                this.aLinkToShipping.HRef = Globals.ApplicationPath + "/Vshop/ShippingAddresses.aspx?returnUrl=" + HttpContext.Current.Request.Url.ToString();
                ShoppingCartInfo shoppingCart = null;
                string msg = "";
                string str = this.Page.Request.QueryString["from"];
                if (((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"])) && ((this.Page.Request.QueryString["from"] == "signBuy") || (this.Page.Request.QueryString["from"] == "groupBuy") || (this.Page.Request.QueryString["from"] == "countDown") || (this.Page.Request.QueryString["from"] == "cutDown")))
                {
                    this.productSku = this.Page.Request.QueryString["productSku"];

                    //团购//待完善
                    if ((str == "groupBuy") && int.TryParse(this.Page.Request.QueryString["groupbuyId"], out this.groupBuyId))
                    {
                        this.groupbuyHiddenBox.SetWhenIsNotNull(this.groupBuyId.ToString());
                        shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.groupBuyId, this.productSku, this.buyAmount);
                    }
                    //限时抢购
                    else if ((str == "countDown") && int.TryParse(this.Page.Request.QueryString["countdownId"], out this.countDownId))
                    {
                        this.countdownHiddenBox.SetWhenIsNotNull(this.countDownId.ToString());
                        CountDownInfo info4 = ProductBrowser.GetCountDownInfo(this.countDownId, this.buyAmount, out msg);
                        if (info4 == null)
                        {
                            base.GotoResourceNotFound(msg);
                            return;
                        }
                        if (string.IsNullOrEmpty(this.productSku) || (this.productSku.Split(new char[] { '_' })[0] != info4.ProductId.ToString()))
                        {
                            base.GotoResourceNotFound("错误的商品信息");
                            return;
                        }

                        shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(this.productSku, this.buyAmount);
                    }
                    //砍价
                    else if ((str == "cutDown") && int.TryParse(this.Page.Request.QueryString["cutDownId"], out this.cutDownId))
                    {
                        this.cutdownHiddenBox.SetWhenIsNotNull(this.cutDownId.ToString());
                        CutDownInfo info5 = PromoteHelper.GetCutDown(this.cutDownId);
                        if (info5 == null)
                        {
                            base.GotoResourceNotFound();
                            return;
                        }
                        if (string.IsNullOrEmpty(this.productSku) || (this.productSku.Split(new char[] { '_' })[0] != info5.ProductId.ToString()))
                        {
                            base.GotoResourceNotFound("错误的商品信息");
                            return;
                        }

                        shoppingCart = ShoppingCartProcessor.GetCutDownShoppingCart(this.productSku, this.buyAmount, info5.CutDownId);
                    }
                    else
                    {
                        shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
                    }
                }
                else
                {
                    shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                }



                //获取/新建了购物车对象后,进行商品价格根据分销商特殊优惠设置进行减价处理.
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                int distributorid = Globals.GetCurrentDistributorId();
                //根据分销商的特殊优惠设置进行计算返佣
                switch (SettingsManager.GetMasterSettings(false).DistributorCutOff)
                {
                    case "default"://直接退出,没有任何操作一切照常
                        break;
                    case "bycostprice"://根据进货价进行返佣
                        if (currentMember.UserId != distributorid)//如果不是分销商购买自己的产品,则退出特殊处理
                            break;
                        for (int i = 0; i < shoppingCart.LineItems.Count; i++)
                        {
                            shoppingCart.LineItems[i].AdjustedPrice = ProductHelper.GetSkuCostPrice(shoppingCart.LineItems[i].SkuId);
                        }
                        break;

                    default://根据指定折扣进行返佣(此种情况下,不存在直接分佣)
                        if (currentMember.UserId != distributorid)//如果不是分销商购买自己的产品,则退出特殊处理
                            break;
                        decimal rate = Convert.ToDecimal(SettingsManager.GetMasterSettings(false).DistributorCutOff) / 100;//获取折扣比例
                        for (int i = 0; i < shoppingCart.LineItems.Count; i++)
                        {
                            shoppingCart.LineItems[i].AdjustedPrice = shoppingCart.LineItems[i].AdjustedPrice * rate;
                        }
                        break;
                }

                if (shoppingCart != null)
                {
                    //根据购物车内商品名是否为[会员充值]和是否开启了会员充值送优惠券的功能来给隐藏域赋值,前端js再进行相应的隐藏显示,默认选中等处理
                    if (shoppingCart.LineItems.Where(n => n.Name == "会员充值") != null && shoppingCart.LineItems.Where(n => n.Name == "会员充值").Count() == 1 && CustomConfigHelper.Instance.CouponRecharge)
                        this.couponRecharge.Value = "1";
                    else
                        this.couponRecharge.Value = "0";
                    //提交订单前先检查是否达到规则要求上限
                    if (!TradeHelper.CheckShoppingStock(shoppingCart, out msg))
                    {
                        this.ShowMessage("订单中有商品(" + msg + ")库存不足", false);
                        if (!this.Page.ClientScript.IsClientScriptBlockRegistered("AlertStockScript"))
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine("var stockError = true;");
                            builder.AppendLine(string.Format("var stockErrorInfo=\"订单中有商品({0})库存不足,请返回购物车修改库存.\";", msg));
                            this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "AlertStockScript", "var stockError=true;function AlertStock(){alert('订单中有商品(" + msg + ")库存不足,请返回购物车修改库存.');}", true);
                        }
                    }

                        //当前用户已使用的优惠券信息         
                        DataTable userCoupon = CouponHelper.GetUserCoupons(currentMember.UserId, 2);//代表已使用的
                        if (userCoupon.Rows.Count > 0)
                        {
                            this.usercount.Value = "1";                            
                        }
                        else
                        {
                            ////把当前用户所有优惠券信息取出来
                            DataTable userCouponList = CouponHelper.GetUserCoupons(currentMember.UserId, 1);//1代表未使用的,有效期内的
                            ////对当前购物车shoppingCart.LineItems进行循环查找,如果优惠券的categoryId与商品的品类不匹配,把优惠券的claimcode取出来放到隐藏域内,传递到前台,前台再进行相关的隐藏操作
                            foreach (ShoppingCartItemInfo shopp in shoppingCart.LineItems)
                            {
                                foreach (DataRow coupon in userCouponList.Rows)
                                {
                                    if (!Convert.IsDBNull(coupon["CategoryId"]))
                                    {
                                        int cp = Convert.ToInt32(coupon["CategoryId"]);
                                        if (shopp.CategoryId != cp && cp != 0)
                                        {
                                            claimcode.Value += coupon["ClaimCode"].ToString() + ";";
                                        }
                                    }
                                }
                            }                          
                        }
                        claimcode.Value = claimcode.Value.TrimEnd(';');
                          
                     
                    this.rptCartProducts.DataSource = shoppingCart.LineItems;
                    this.rptCartProducts.DataBind();
                    //绑定礼品
                    this.rptCartGifts.DataSource = shoppingCart.LineGifts;
                    this.rptCartGifts.DataBind();
                    //绑定配送方式
                    this.dropShippingType.ShoppingCartItemInfo = shoppingCart.LineItems;
                    this.dropShippingType.RegionId = 0;
                    this.dropShippingType.Weight = shoppingCart.Weight;
                    this.dropCoupon.CartTotal = shoppingCart.GetTotal();
                    /*
                    //addby hj 2015-04-23(迪蔓)
                    this.isSelectAgentEnable.Value = CustomConfigHelper.Instance.SelectServerAgent.ToString();
                    //addby hj 2015-12-25(爽爽挝咖)
                    //给出是否开启街道选择和门店配送功能的标识
                    this.isStreetEnable.Value = Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping.ToString();
                    //绑定街道下拉框
                    if (this.isStreetEnable.Value == "True")
                    {
                        DataTable dtStreetsInfo = Hidistro.ControlPanel.Sales.SalesHelper.GetStreetInfo(regionId.Value.ToString());
                        if (dtStreetsInfo.Rows.Count > 0)
                        {
                            this.dropStreets.StreetsInfo = dtStreetsInfo;
                            this.isStreetMatch.Value = "True";
                        }
                        else//此种情况下消费者的所在区域内并没有相应的配送店铺,给出提示,页面无法提交
                        {
                            this.isStreetMatch.Value = "False";
                        }
                    }
                     */ 
                    string noCouponIds = string.Empty;
                    string tempCategoryId = string.Empty;
                    DataTable dtDB = CouponHelper.GetCouponAllCate(); //new DataTable();//设置的使用权限的有效期内的优惠卷 两表关联信息
                    foreach (DataRow dr in dtDB.Rows)
                    {
                        tempCategoryId = "," + dr["CategoryIds"].ToString() + ",";
                        decimal dAmount = (decimal)dr["Amount"];

                        //计算排除掉不能参与的分类金额
                        //shoppingCart.LineItems 
                        decimal dAmount2 = dropCoupon.CartTotal;
                        foreach (ShoppingCartItemInfo scii in shoppingCart.LineItems)
                        {
                            if (tempCategoryId.IndexOf("," + scii.CategoryId + ",") > -1)
                                dAmount2 -= scii.SubTotal;
                        }

                        //待排除的
                        if (dAmount > dAmount2)
                        {
                            noCouponIds += dr["CouponId"] + ",";
                        }

                    }
                    if (noCouponIds != "")
                    {
                        noCouponIds = noCouponIds.TrimEnd(',');
                    }
                    this.dropCoupon.CouponIds = noCouponIds;
                    this.dropRedPager.CartTotal = shoppingCart.GetTotal();
                    this.hiddenCartTotal.Value = literal.Text = shoppingCart.GetTotal().ToString("F2");
                    decimal num = this.DiscountMoney(shoppingCart.LineItems);
                    this.litOrderTotal.Text = (shoppingCart.GetTotal() - num).ToString("F2");
                    this.litTotalPoint.Text = getTotalPoints(shoppingCart).ToString();
                    this.litExemption.Text = num.ToString("0.00");
                }
                else
                {
                    this.Page.Response.Redirect("/Vshop/ShoppingCart.aspx");
                }
                PageTitle.AddSiteNameTitle("订单确认");
            }
        }

        /// <summary>
        /// 计算所需积分
        /// </summary>
        /// <returns></returns>
        private int getTotalPoints(ShoppingCartInfo shoppingCart)
        {
            int result = 0;
            foreach (ShoppingCartGiftInfo gift in shoppingCart.LineGifts)
            {
                result += gift.NeedPoint * gift.Quantity;
            }

            return result;
        }


        public decimal DiscountMoney(IList<ShoppingCartItemInfo> infoList)
        {
            decimal num = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;
            DataTable type = ProductBrowser.GetType();
            for (int i = 0; i < type.Rows.Count; i++)
            {
                decimal num5 = 0M;
                foreach (ShoppingCartItemInfo info in infoList)
                {
                    if (!string.IsNullOrEmpty(info.MainCategoryPath) && ((int.Parse(type.Rows[i]["ActivitiesType"].ToString()) == int.Parse(info.MainCategoryPath.Split(new char[] { '|' })[0].ToString())) || (int.Parse(type.Rows[i]["ActivitiesType"].ToString()) == 0)))
                    {
                        num5 += info.SubTotal;
                    }
                }
                if (num5 != 0M)
                {
                    DataTable allFull = ProductBrowser.GetAllFull(int.Parse(type.Rows[i]["ActivitiesType"].ToString()));
                    if (allFull.Rows.Count > 0)
                    {
                        for (int j = 0; j < allFull.Rows.Count; j++)
                        {
                            if (num5 >= decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString());
                                num = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["ReductionMoney"].ToString());
                                break;
                            }
                            if (num5 <= decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString());
                                num += decimal.Parse(allFull.Rows[0]["ReductionMoney"].ToString());
                                break;
                            }
                            if (num5 >= decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString()))
                            {
                                num2 = decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString());
                                num = decimal.Parse(allFull.Rows[j]["ReductionMoney"].ToString());
                            }
                        }
                        if (num5 >= num2)
                        {
                            num3 += num;
                        }
                    }
                }
            }
            return num3;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VSubmmitOrderPro.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}

