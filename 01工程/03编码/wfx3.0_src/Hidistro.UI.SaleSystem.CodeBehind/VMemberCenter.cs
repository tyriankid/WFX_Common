using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System.Web.UI.HtmlControls;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Promotions;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true), WeiXinOAuth(Common.Controls.WeiXinOAuthPage.VMemberCenter)]
    public class VMemberCenter : VWeiXinOAuthTemplatedWebControl
    {
        private Image image;
        private Literal litExpenditure;
        private Literal litMemberGrade;
        private Literal litUserName;
        private Literal litWaitForPayCount;
        private Literal litWaitForRecieveCount;
        private Literal litWaitForReplace;
        private Literal litVantages;//我的积分
        private Literal litGetCoupon;//领取优惠券
        private Literal litMyCouponCount;//我的优惠券数量
        private Literal litUserType;//用户类别
        private HtmlControl ReturnChangeGoodsArea;//退换货区域
        private HtmlInputHidden specialHideShow;//特殊用户名,用于隐藏显示自定义功能块
        private HtmlInputHidden isSignOn;//签到活动是否开启隐藏域
        private Literal litRecommend;
        private Literal litHeadline;
        protected override void AttachChildControls()
        {
            this.ReturnChangeGoodsArea = (HtmlControl)this.FindControl("ReturnChangeGoodsArea");
            //退换货根据用户的特殊需求来配置
            if (this.ReturnChangeGoodsArea != null)
            {
                ReturnChangeGoodsArea.Visible = CustomConfigHelper.Instance.IsReturnChangeGoodsOn == "false"?false:true;
            }
            
            PageTitle.AddSiteNameTitle("会员中心");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                this.litUserType = (Literal)this.FindControl("litUserType");
                this.litUserName = (Literal)this.FindControl("litUserName");
                this.image = (Image)this.FindControl("image");
                this.litExpenditure = (Literal)this.FindControl("litExpenditure");
                this.litExpenditure.SetWhenIsNotNull(currentMember.Expenditure.ToString("F2"));
                this.litMemberGrade = (Literal)this.FindControl("litMemberGrade");
                this.specialHideShow = (HtmlInputHidden)this.FindControl("specialHideShow");
                this.isSignOn = (HtmlInputHidden)this.FindControl("isSignOn");
                this.litRecommend = (Literal)this.FindControl("litRecommend");
                this.litHeadline = (Literal)this.FindControl("litHeadline");
                //通过当前用户id查找分销商信息,从而来判断用户类型
                DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
                
                if (this.litUserType != null)
                {
                    if (currentDistributor == null)
                        this.litUserType.Text = "普通用户";
                    else if (currentDistributor.IsAgent == 1)
                        this.litUserType.Text = "代理商";
                    else if (currentDistributor.IsAgent == 0)
                        this.litUserType.Text = "分销商";
                }
                //迪曼展示推荐人需求（所属上一级分销商ID(推荐分销商ID)）
                if (currentDistributor != null)
                {
                    if (currentDistributor.ReferralPath!= null &&currentDistributor.ReferralPath!="")
                    {
                        DistributorsInfo currentInfo = DistributorsBrower.GetDistributorInfo(currentDistributor.ReferralUserId);
                        this.litRecommend.Text = currentInfo.UserName;
                    }
                    else
                    {
                        this.litHeadline.Visible = false;
                    }
                }
                else 
                {

                    this.litHeadline.Visible = false;
                }
                //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
                if (CustomConfigHelper.Instance.AutoShipping)
                {
                    specialHideShow.Value = "sswk";//爽爽挝啡
                }
                //传递pro辣的特殊名到前端,前端进行相应的隐藏调整
                if (CustomConfigHelper.Instance.IsProLa)
                {
                    specialHideShow.Value = "proLa";
                }
                //根据签到活动的状态,进行相应的功能隐藏显示
                System.Data.DataTable ruleDT = PromoteHelper.GetSignRule();
                if (ruleDT.Rows.Count > 0)
                {
                    isSignOn.Value = ruleDT.Rows[0]["State"].ToString();
                }
                else
                {
                    isSignOn.Value = "0";
                }

                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
                if (memberGrade != null)
                {
                    this.litMemberGrade.SetWhenIsNotNull(memberGrade.Name);
                }
                this.litUserName.Text = string.IsNullOrEmpty(currentMember.RealName) ? currentMember.UserName : currentMember.RealName;
                if (!string.IsNullOrEmpty(currentMember.UserHead))
                {
                    this.image.ImageUrl = currentMember.UserHead;
                }
                this.Page.Session["stylestatus"] = "1";
                this.litWaitForRecieveCount = (Literal)this.FindControl("litWaitForRecieveCount");
                this.litWaitForPayCount = (Literal)this.FindControl("litWaitForPayCount");
                this.litWaitForReplace = (Literal)this.FindControl("litWaitForReplace");
                OrderQuery query = new OrderQuery
                {
                    Status = OrderStatus.WaitBuyerPay
                };
                int userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.litWaitForPayCount.SetWhenIsNotNull((userOrderCount > 0) ? ("<i class=\"border-circle\">" + userOrderCount.ToString() + "<i>") : "");
                query.Status = OrderStatus.SellerAlreadySent;
                userOrderCount = MemberProcessor.GetUserOrderCount(Globals.GetCurrentMemberUserId(), query);
                this.litWaitForRecieveCount.SetWhenIsNotNull((userOrderCount > 0) ? ("<i class=\"border-circle\">" + userOrderCount.ToString() + "<i>") : "");
                int userOrderReturnCount = MemberProcessor.GetUserOrderReturnCount(Globals.GetCurrentMemberUserId());
                this.litWaitForReplace.SetWhenIsNotNull((userOrderReturnCount > 0) ? ("<i class=\"border-circle\">" + userOrderReturnCount.ToString() + "<i>") : "");

                this.litVantages = (Literal)this.FindControl("litVantages");
                if (litVantages != null)
                {
                    string str = litVantages.Text;
                    litVantages.Text = str + "我的积分:" + currentMember.Points.ToString();
                }
                //领取优惠券
                this.litGetCoupon = (Literal)this.FindControl("litGetCoupon");
                if (this.litGetCoupon != null)
                {
                    //可领取优惠券张数
                    int gCouponsCount=CouponHelper.GetUseableCoupons(currentMember.UserId).Rows.Count;
                    string gCouponsStr=string.Format(gCouponsCount>0?"<i style=\"float: right;margin-right: 20px;background: #F87575;color: #fff;width: 25px;height: 25px;line-height: 25px;text-align: center;text-indent: 0;border-radius: 50%;font-size: 15px;position: relative;top: 50%;transform: translateY(-50%);-webkit-transform: translateY(-50%);-moz-transform: translateY(-50%); font-style:normal;\">" + gCouponsCount + "</i>":"");
                    string str = litGetCoupon.Text;
                    litGetCoupon.Text = string.Format("<div class=\"bottom-content\"><a class=\"red\" style=\"background:#fff url('" + Globals.GetVshopSkinPath() + "/images/iconfont-lingqu.png') 10px center no-repeat;\" href=\"/Vshop/getcoupons.aspx\">领取优惠券{0}</a></div>", gCouponsStr);
                }

                //我的优惠券数量
                this.litMyCouponCount = (Literal)this.FindControl("litMyCouponCount");
                if (this.litMyCouponCount != null)
                {
                    string str = litMyCouponCount.Text;
                    int number = CouponHelper.GetUserCoupons(currentMember.UserId,1).Rows.Count;

                    if(number>0)
                        litMyCouponCount.Text = "<i>" + str + number + "</i>";
                }


            }
        }



        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
    }
}

