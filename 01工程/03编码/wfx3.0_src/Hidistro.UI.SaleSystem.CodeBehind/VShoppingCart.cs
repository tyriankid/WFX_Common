namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [ParseChildren(true)]
    public class VShoppingCart : VWeiXinOAuthTemplatedWebControl
    {
        private HtmlAnchor aLink;
        private List<ShoppingCartInfo> cart;
        private HtmlGenericControl divShowTotal;
        private Literal litcount;
        private Literal litExemption;
        private Literal litStoreMoney;
        private Literal littext;
        private Literal litTotal;//总价
        private Literal litTotalPoint;//总积分
        private Literal litPointLeft;//剩余积分
        private decimal ReductionMoneyALL;
        private VshopTemplatedRepeater rptCartProducts;
        private VshopTemplatedRepeater rptCartGifts;//礼品列表
        private HtmlControl gifts;//礼物板块
        private HtmlControl products;//商品板块
        private HtmlInputHidden specialHideShow;//特殊客户屏蔽展示功能

        protected override void AttachChildControls()
        {
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.rptCartGifts = (VshopTemplatedRepeater)this.FindControl("rptCartGifts");//礼品列表
            this.gifts = (HtmlControl)this.FindControl("gifts");//礼物板块
            this.products = (HtmlControl)this.FindControl("products");//商品板块
            this.litTotalPoint = (Literal)this.FindControl("litTotalPoint");//总积分
            this.litPointLeft = (Literal)this.FindControl("litPointLeft");//剩余积分
            this.litTotalPoint.Text = "0";
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            this.rptCartGifts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartGifts_ItemDataBound);
            this.litTotal = (Literal) this.FindControl("litTotal");
            this.littext = (Literal) this.FindControl("littext");
            this.litStoreMoney = (Literal) this.FindControl("litStoreMoney");
            this.litExemption = (Literal) this.FindControl("litExemption");
            this.litcount = (Literal) this.FindControl("litcount");
            this.divShowTotal = (HtmlGenericControl) this.FindControl("divShowTotal");
            this.aLink = (HtmlAnchor) this.FindControl("aLink");
            this.specialHideShow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("specialHideShow");
            this.Page.Session["stylestatus"] = "0";
            this.litExemption.Text = "0.00";
            //this.cart = ShoppingCartProcessor.GetShoppingCartAviti();
            this.cart = ShoppingCartProcessor.GetShoppingCartList();//获取购物车信息
            int num = 0;
            //购物车数据源
            if (this.cart != null)
            {
                this.rptCartProducts.DataSource = this.cart;
                this.rptCartProducts.DataBind();
                for (int i = 0; i < this.cart.Count; i++)
                {
                    num += this.cart[i].GetQuantity();
                }
            }
            else
            {
                if (products != null)
                products.Visible = false;
            }
            //绑定礼品数据
            if (cart != null && cart.Count > 0)
            {
                this.rptCartGifts.DataSource = cart;
                this.rptCartGifts.DataBind();
                for (int i = 0; i < this.cart.Count; i++)
                {
                    num += this.cart[i].GetGiftQuantity();
                }
            }
            else
            {
                if (gifts != null)
                    gifts.Visible = false;
            }
            this.litcount.Text = num.ToString();
            //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
            {
                specialHideShow.Value = "sswk";//爽爽挝啡
            }

            this.litTotal.Text = "￥" + "0.00";
            if (MemberProcessor.GetCurrentMember() != null)
            {
                //显示剩余积分
                this.litPointLeft.Text = MemberProcessor.GetCurrentMember().Points.ToString();
                decimal num3 = 0M;
                if (this.cart != null)
                {
                    foreach (ShoppingCartInfo info in this.cart)
                    {
                        num3 += info.GetAmount();
                    }
                }
                PageTitle.AddSiteNameTitle("购物车");
                DataTable allFull = ProductBrowser.GetAllFull(0);
                if (allFull.Rows.Count > 0)
                {
                    #region 获取相关活动
                    decimal num4 = 0M;
                    decimal num5 = 0M;
                    string str = "";
                    string str2 = "";
                    string str3 = "";
                    for (int j = 0; j < allFull.Rows.Count; j++)
                    {
                        if (num3 >= decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesName"].ToString();
                            num5 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString());
                            num4 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesId"].ToString();
                            break;
                        }
                        if (num3 <= decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[0]["ActivitiesName"].ToString();
                            num5 = decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString());
                            num4 = decimal.Parse(allFull.Rows[0]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[0]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[0]["ActivitiesId"].ToString();
                            break;
                        }
                        if (num3 >= decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[j]["ActivitiesName"].ToString();
                            num5 = decimal.Parse(allFull.Rows[j]["MeetMoney"].ToString());
                            num4 = decimal.Parse(allFull.Rows[j]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[j]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[j]["ActivitiesId"].ToString();
                        }
                    }
                    this.littext.Text = "";
                    if (num3 >= num5)
                    {
                        string text = this.littext.Text;
                        this.littext.Text = text + "<div id=\"cartProducts\" class=\"well shopcart\"><div class=\"price-sale\"><span class=\"title\">促销活动：</span><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=" + str2 + "&CategoryId=" + str3 + "\">" + str + "</a></span><span>已满" + num5.ToString("0") + "已减" + num4.ToString("0") + "</span></div></div>";
                        this.ReductionMoneyALL += num4;
                    }
                    else
                    {
                        string str5 = this.littext.Text;
                        this.littext.Text = str5 + "<div id=\"cartProducts\" class=\"well shopcart\"><div class=\"price-sale\"><span class=\"title\">促销活动：</span><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=" + str2 + "&CategoryId=" + str3 + "\">" + str + "</a></span><span>满" + num5.ToString("0") + "减" + num4.ToString("0") + "</span></div></div>";
                    }
                    this.litExemption.Text = this.ReductionMoneyALL.ToString("0.00");
                    if (num3 == 0M)
                    {
                        this.littext.Text = "";
                    }
                    #endregion
                }
                this.litStoreMoney.Text = "￥" + num3.ToString("0.00");
                this.litTotal.Text = "￥" + ((num3 - this.ReductionMoneyALL)).ToString("0.00");
                //如果有gift,则需要提示需要支付的积分总额
                if (cart.Count > 0)
                {
                    this.litTotalPoint.Text = getTotalPoints().ToString() == "0" ? "0" : getTotalPoints().ToString();
                }

                //隐藏
                if (getTotalPoints() == 0)
                {
                    gifts.Visible = false;
                }
            }
            
            
        }

        /// <summary>
        /// 计算所需积分
        /// </summary>
        /// <returns></returns>
        private int getTotalPoints()
        {
            int result=0;
            foreach (ShoppingCartInfo giftList in cart)
            {
                foreach(ShoppingCartGiftInfo gift in giftList.LineGifts)
                {
                    result += gift.NeedPoint*gift.Quantity;
                }
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCart.html";
            }
            base.OnInit(e);
        }

        /// <summary>
        /// 礼品绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rptCartGifts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                
            }
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.Controls[0].FindControl("litpromotion");
                Literal literal1 = (Literal) e.Item.Controls[0].FindControl("litline");
                decimal num = 0M;
                foreach (ShoppingCartInfo info in this.cart)
                {
                    if (info.CategoryId == ((int) DataBinder.Eval(e.Item.DataItem, "CategoryId")))
                    {
                        num += info.GetAmount();
                    }
                }
                DataTable allFull = ProductBrowser.GetAllFull((int) DataBinder.Eval(e.Item.DataItem, "CategoryId"));
                string str = "";
                decimal num2 = 0M;
                decimal num3 = 0M;
                string str2 = "";
                string str3 = "";
                if (allFull.Rows.Count > 0)
                {
                    for (int i = 0; i < allFull.Rows.Count; i++)
                    {
                        if (num >= decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesName"].ToString();
                            num2 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["MeetMoney"].ToString());
                            num3 = decimal.Parse(allFull.Rows[allFull.Rows.Count - 1]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[allFull.Rows.Count - 1]["ActivitiesId"].ToString();
                            break;
                        }
                        if (num <= decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[0]["ActivitiesName"].ToString();
                            num2 = decimal.Parse(allFull.Rows[0]["MeetMoney"].ToString());
                            num3 = decimal.Parse(allFull.Rows[0]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[0]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[0]["ActivitiesId"].ToString();
                            break;
                        }
                        if (num >= decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString()))
                        {
                            str = allFull.Rows[i]["ActivitiesName"].ToString();
                            num2 = decimal.Parse(allFull.Rows[i]["MeetMoney"].ToString());
                            num3 = decimal.Parse(allFull.Rows[i]["ReductionMoney"].ToString());
                            str3 = allFull.Rows[i]["ActivitiesType"].ToString();
                            str2 = allFull.Rows[i]["ActivitiesId"].ToString();
                        }
                    }
                    if (allFull.Rows[0]["ActivitiesType"].ToString() != "0")
                    {
                        literal.Text = "";
                        if (num >= num2)
                        {
                            string text = literal.Text;
                            literal.Text = text + "<div class=\"price-sale\"><span class=\"title\">促销活动：</span><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=" + str2 + "&CategoryId=" + str3 + "\">" + str + "</a></span><span>已满" + num2.ToString("0") + "已减" + num3.ToString("0") + "</span></div>";
                            this.ReductionMoneyALL += num3;
                        }
                        else
                        {
                            string str5 = literal.Text;
                            literal.Text = str5 + "<div class=\"price-sale\"><span class=\"title\">促销活动：</span><span class=\"purchase\"><a href=\"/Vshop/ActivityDetail.aspx?ActivitiesId=" + str2 + "&CategoryId=" + str3 + "\">" + str + "</a></span><span>满" + num2.ToString("0") + "减" + num3.ToString("0") + "</span></div>";
                        }
                        this.litExemption.Text = this.ReductionMoneyALL.ToString("0.00");
                    }
                }
                if (num == 0M)
                {
                    literal.Text = "";
                }
            }
        }
    }
}

