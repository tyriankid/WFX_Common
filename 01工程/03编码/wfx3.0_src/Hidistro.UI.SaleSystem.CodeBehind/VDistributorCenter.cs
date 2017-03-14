namespace Hidistro.UI.SaleSystem.CodeBehind
{
using Hidistro.ControlPanel.Config;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hishop.Weixin.MP.Api;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorCenter : VWeiXinOAuthTemplatedWebControl
    {
        private Image imgGrade;
        private HiImage imglogo;
        private Literal litStroeName;
        private Literal litTodayOrdersNum;
        private FormatedMoneyLabel refrraltotal;
        private FormatedMoneyLabel saletotal;
        private FormatedMoneyLabel lblsurpluscommission;
        //------新模板-------
        private HtmlInputHidden val_MyCommission;//我的佣金金额
        private HtmlInputHidden val_MyChirldrenDistributors;//我的下属数量
        private HtmlInputHidden val_MyDistributorOrders;//店铺订单数量
        private HtmlInputHidden val_MyProducts;//店铺商品
        private HtmlInputHidden val_MyVisitCount;//粉丝数量
        private HtmlInputHidden val_MyUnderOrder;//下属店铺订单数量
        private HtmlInputHidden val_isAgent;//当前用户是不是代理商
        private HtmlInputHidden val_joinTime;//加入时间
        private HtmlInputHidden name_MyCommission;//我的佣金字眼
        private HtmlInputHidden name_MyChirldrenDistributors;//我的下属字眼
        private HtmlInputHidden specialHideShow;//特殊客户屏蔽展示功能

        protected override void AttachChildControls()
        {
            findControls();
            bindValues();
        }

        private void findControls()
        {
            this.litTodayOrdersNum = (Literal)this.FindControl("litTodayOrdersNum");
            this.imglogo = (HiImage)this.FindControl("imglogo");
            this.imgGrade = (Image)this.FindControl("imgGrade");
            this.litStroeName = (Literal)this.FindControl("litStroeName");
            this.saletotal = (FormatedMoneyLabel)this.FindControl("saletotal");
            this.refrraltotal = (FormatedMoneyLabel)this.FindControl("refrraltotal");
            this.lblsurpluscommission = (FormatedMoneyLabel)this.FindControl("lblsurpluscommission");
            //-------新模板-----
            this.val_isAgent = (HtmlInputHidden)this.FindControl("val_isAgent");
            this.val_joinTime = (HtmlInputHidden)this.FindControl("val_joinTime");
            this.val_MyCommission = (HtmlInputHidden)this.FindControl("val_MyCommission");
            this.val_MyChirldrenDistributors = (HtmlInputHidden)this.FindControl("val_MyChirldrenDistributors");
            this.val_MyDistributorOrders = (HtmlInputHidden)this.FindControl("val_MyDistributorOrders");
            this.val_MyProducts = (HtmlInputHidden)this.FindControl("val_MyProducts");
            this.val_MyVisitCount = (HtmlInputHidden)this.FindControl("val_MyVisitCount");
            this.val_MyUnderOrder = (HtmlInputHidden)this.FindControl("val_MyUnderOrder");
            this.name_MyCommission = (HtmlInputHidden)this.FindControl("name_MyCommission");
            this.name_MyChirldrenDistributors = (HtmlInputHidden)this.FindControl("name_MyChirldrenDistributors");
            this.specialHideShow = (HtmlInputHidden)this.FindControl("specialHideShow");
        }

        private void bindValues()
        {
            PageTitle.AddSiteNameTitle("店铺中心");
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
            if (userIdDistributors != null)
            {
                OrderQuery query = new OrderQuery
                {
                    UserId = new int?(currentMemberUserId),
                    Status = OrderStatus.Today
                };
                this.litTodayOrdersNum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                //新模板--------------
                this.litStroeName.Text = userIdDistributors.StoreName;//店名
                this.saletotal.Money = userIdDistributors.OrdersTotal;//订单总额
                this.refrraltotal.Money = DistributorsBrower.GetUserCommissions(userIdDistributors.UserId, DateTime.Now);//今日佣金
                //头像
                if ((distributorGradeInfo != null) && (distributorGradeInfo.Ico.Length > 10))
                    this.imgGrade.ImageUrl = distributorGradeInfo.Ico;
                else
                    this.imgGrade.Visible = false;
                //店铺logo
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                    this.imglogo.ImageUrl = userIdDistributors.Logo;
                else
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    if (!string.IsNullOrEmpty(masterSettings.DistributorLogoPic))
                    {
                        this.imglogo.ImageUrl = masterSettings.DistributorLogoPic.Split(new char[] { '|' })[0];
                    }
                }
                //当前用户是不是分销商
                val_isAgent.Value = userIdDistributors.IsAgent.ToString();
                //加入时间
                val_joinTime.Value = userIdDistributors.CreateTime.ToLongDateString();
                //可提现佣金
                lblsurpluscommission.Money = userIdDistributors.ReferralBlance;
                //我的佣金=剩余可体现佣金+提现中的佣金
                val_MyCommission.Value = (userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance).ToString("F2");
                //我的下属分销商数量
                val_MyChirldrenDistributors.Value = DistributorsBrower.GetDownDistributorNum(userIdDistributors.UserId.ToString()).ToString();
                //店铺订单数量
                OrderQuery distributorOrderQuery = new OrderQuery { UserId = userIdDistributors.UserId, };
                val_MyDistributorOrders.Value = DistributorsBrower.GetDistributorOrderCount(distributorOrderQuery).ToString();
                //我的下属店铺订单数量
                OrderQuery underOrderQuery = new OrderQuery { UserId = userIdDistributors.UserId, };
                val_MyUnderOrder.Value = DistributorsBrower.GetUnderOrders(underOrderQuery).TotalRecords.ToString();
                //粉丝数量(对代理商,对分销商)
                val_MyVisitCount.Value = DistributorsBrower.GetDistributorVisitCount(userIdDistributors.UserId).ToString();
                //特殊字眼处理
                name_MyCommission.Value = CustomConfigHelper.Instance.MyCashText;//我的佣金
                name_MyChirldrenDistributors.Value = CustomConfigHelper.Instance.MyDistributorText;//我的下属
                //传递爽爽挝啡的特殊名到前端,前端用jquery进行相应的功能隐藏
                if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
                {
                    specialHideShow.Value = "sswk";//爽爽挝啡
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorCenter.html";
            }
            base.OnInit(e);
        }
    }
}

