using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{

    [ParseChildren(true)]
    public class VIndex : VWeiXinOAuthTemplatedWebControl
    {
        public Panel panelHomePage;
        private Literal litItemParams;
        private System.Web.UI.HtmlControls.HtmlInputHidden hidFlowWindow;
        protected override void AttachChildControls()
        {
            this.panelHomePage = (Panel)this.FindControl("panelHomePage");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            this.hidFlowWindow = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidFlowWindow");

            //店铺推广码送过来的地址(参数：ReferralId)
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                //无传参，取COOKIE
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    this.Page.Response.Redirect("Index.aspx?ReferralId=" + cookie.Value);
                }
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            this.hidFlowWindow.Value = masterSettings.isFlowWindowsOn ? "1" : "0";
            PageTitle.AddSiteNameTitle(masterSettings.SiteName);
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
            }
            if (base.referralId <= 0)
            {
                HttpCookie cookie2 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if ((cookie2 != null) && !string.IsNullOrEmpty(cookie2.Value))
                {
                    base.referralId = int.Parse(cookie2.Value);
                    this.Page.Response.Redirect("Index.aspx?ReferralId=" + this.referralId.ToString(), true);
                }
            }
            else
            {
                HttpCookie cookie3 = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                if (((cookie3 != null) && !string.IsNullOrEmpty(cookie3.Value)) && (this.referralId.ToString() != cookie3.Value))
                {
                    this.Page.Response.Redirect("Index.aspx?ReferralId=" + this.referralId.ToString(), true);//跳转到自己的店铺
                }
            }


            panelHomePage.Controls.Clear();
            string selectSql = string.Format("Select * From YiHui_HomePage Where PageType={0} order by PageSN", 11);
            DataSet ds = DataBaseHelper.GetDataSet(selectSql);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                baseModel.PKID = new Guid(dr["PageID"].ToString());//模块的内容ID
                baseModel.PageSN = dr["PageSN"] + "";
                panelHomePage.Controls.Add(baseModel);
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
                strDes = "我很喜欢这家平台的东西,澳洲奶粉,保健品,化妆品价格也挺便宜,你也来看看把!";
            }
            this.litItemParams.Text = str3 + "|" + masterSettings.ShopHomeName + "|" + strDes + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str4, "|好店推荐之", str5, "商城|一个购物赚钱的好去处|", HttpContext.Current.Request.Url });
            getMyCoupon();//获取首页赠送优惠券
            distributorVisitCont();//更新店铺访问信息
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VIndex.html";
            }
            base.OnInit(e);
        }

        /// <summary>
        /// add:2015-11-27 增加分销商的店铺访的问量,载入一次主页+1
        /// </summary>
        private void distributorVisitCont()
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null && currentMember.OpenId != null)
            {
                //如果当前用户是微信用户,则增加一次访问量,(包括自己的点击)
                DistributorsBrower.UpdateDistributorVisitCount(currentMember.UserId, this.referralId);
            }
        }

        /// <summary>
        /// 进入主页获赠优惠券
        /// </summary>
        private void getMyCoupon()
        {          
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                return;//如果当前没有登录则不执行该方法
            DataTable allCoupons = CouponHelper.GetAllCoupons();//优惠券列表          
            DataTable allCouponItemsClaimCode = CouponHelper.GetAllCouponItemsClaimCode();//所有已发送优惠券的claimcode 
                for (int i = 0; i < allCoupons.Rows.Count; i++)
                {
                    if (allCoupons.Rows[i]["sendAtHomepage"].ToString() == "1")//如果需要在主页赠送,就开始判断该用户是否已获取过
                    {
                        bool isSend = true;
                        //发送优惠券
                        int couponId = Convert.ToInt32(allCoupons.Rows[i]["CouponId"]);
                        for (int o = 0; o < allCouponItemsClaimCode.Rows.Count; o++)
                        {
                            string currentClaimCode = allCouponItemsClaimCode.Rows[o]["ClaimCode"].ToString();
                            string[] claimCodeUnhandle = currentClaimCode.TrimStart('b').Split('|');
                            if (claimCodeUnhandle.Length == 2)
                            {
                                isSend = (currentMember.UserId.ToString() == claimCodeUnhandle[1] && couponId.ToString() == claimCodeUnhandle[0]);
                                if (isSend)
                                    break;//如果发送过,跳出循环
                                else
                                    continue;//如果没有发送,继续循环查找
                            }
                            else
                            {
                                isSend = currentMember.UserId.ToString() == currentClaimCode.TrimStart('0');
                                if (isSend)
                                    break;//如果发送过,跳出循环
                                else
                                    continue;//如果没有发送,继续循环查找
                            }
                        }
                        if (!isSend)
                        {
                            int number;
                            string claimCode = string.Empty;
                            claimCode += couponId + "|" + currentMember.UserId;
                            claimCode = claimCode.PadLeft(15, 'b');
                            CouponItemInfo item = new CouponItemInfo();
                            System.Collections.Generic.IList<CouponItemInfo> listCouponItem = new System.Collections.Generic.List<CouponItemInfo>();
                            item = new CouponItemInfo(couponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
                            listCouponItem.Add(item);
                            CouponHelper.SendClaimCodes(couponId, listCouponItem);
                        }
                    }
                }
            }
        }
    }

