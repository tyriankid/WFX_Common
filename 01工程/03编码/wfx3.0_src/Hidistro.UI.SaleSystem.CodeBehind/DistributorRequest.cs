namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using Hidistro.Entities.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using System.Data;

    [ParseChildren(true)]
    public class DistributorRequest : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litBackImg;
        private System.Web.UI.HtmlControls.HtmlInputHidden storeName;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请分销");
            this.Page.Session["stylestatus"] = "2";

            if (MemberProcessor.GetCurrentMember() != null)
            {
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(MemberProcessor.GetCurrentMember().UserId);
                if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
                {
                    this.Page.Response.Redirect("DistributorCenter.aspx", true);
                }
            }

            Literal litNext = (Literal)this.FindControl("litNext");
            //增加判断,如果选了快速开店,上架商品页面也必须要跳过.
            if(litNext!=null)
            {
                if ((Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreInfoSet))//如果开启了店铺配置(非快速开店)
                {
                    litNext.Text = (Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto) ? "" : "，上架商品";
                }
                else//如果没开店铺配置(快速开店)
                {
                    storeName = (HtmlInputHidden)this.FindControl("storeName");
                    if (storeName != null) //附一个默认值
                    {
                        int i = 1;
                        storeName.Value = MemberProcessor.GetCurrentMember().UserName.ToString() + "的小店";
                        while (DistributorsBrower.IsExiteDistributorsByStoreName(storeName.Value) > 0)
                        {
                            i++;
                            storeName.Value += i.ToString();
                        }
                    }
                    litNext.Text = "";
                }
            }
            
            //页面[马上成为分销商]按钮的传参判断
            if (Page.Request.QueryString["action"] != null)
            {
                switch (Page.Request.QueryString["action"])
                {
                    //针对阿黛尔艺丝,成为分销商(vip)后直接获取一个最新的优惠券
                    case "VIP":
                        int couponId;//优惠券id
                        string claimCode = string.Empty;
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        CouponItemInfo item = new CouponItemInfo();
			            System.Collections.Generic.IList<CouponItemInfo> listCouponItem = new System.Collections.Generic.List<CouponItemInfo>();
                        //DataTable coupons = CouponHelper.GetAllCouponsID();
                        DataTable allCoupons = CouponHelper.GetAllCoupons();//优惠券列表
                        for (int i = 0; i < allCoupons.Rows.Count; i++)
                        {
                            if (allCoupons.Rows[i]["sendAtDistributor"].ToString() == "1")//如果需要在成为分销商时赠送,就开始赠送
                            {
                                couponId = Convert.ToInt32(allCoupons.Rows[i]["CouponId"]);
                                claimCode = currentMember.UserId.ToString().PadLeft(15,'a');//左边填充
                                item = new CouponItemInfo(couponId, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now);
                                listCouponItem.Add(item);
                                CouponHelper.SendClaimCodes(couponId, listCouponItem);
                                break;
                            }
                        }
                        break;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRequest.html";
            }
            base.OnInit(e);
        }
    }
}

