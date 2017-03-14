namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.Config;

    [ParseChildren(true)]
    public class VRemoveBox : VWeiXinOAuthTemplatedWebControl
    {
        private int activityid;
        //private HtmlImage bgimg;
        private Literal litActivityDesc;
        //private Literal litEndDate;
        private Common_PrizeNames litPrizeNames;
        private Common_PrizeUsers litPrizeUsers;
        //private Literal litStartDate;
        private Literal litSurplusNum;//剩余次数提示
        private Literal litNum;//已参与人数
        private Literal litimage;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["activityid"], out this.activityid))
            {
                base.GotoResourceNotFound("");
            }
            //this.bgimg = (HtmlImage) this.FindControl("bgimg");
            this.litActivityDesc = (Literal) this.FindControl("litActivityDesc");
            //this.litStartDate = (Literal) this.FindControl("litStartDate");
            //this.litEndDate = (Literal) this.FindControl("litEndDate");
            this.litPrizeNames = (Common_PrizeNames) this.FindControl("litPrizeNames");
            this.litPrizeUsers = (Common_PrizeUsers) this.FindControl("litPrizeUsers");
            this.litSurplusNum = (Literal)this.FindControl("litSurplusNum");
            this.litNum = (Literal)this.FindControl("litNum");
            this.litimage = (Literal)this.FindControl("litimage");
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(this.activityid);
            if (lotteryActivity == null)
            {
                base.GotoResourceNotFound("");
            }
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member") {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddYears(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            PageTitle.AddSiteNameTitle(lotteryActivity.ActivityName);
            litimage.Text = Globals.HostPath(HttpContext.Current.Request.Url) + lotteryActivity.ActivityPic;
            //this.litStartDate.Text = lotteryActivity.StartTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            //this.litEndDate.Text = lotteryActivity.EndTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            //if (lotteryActivity.PrizeSettingList.Count > 3)
            //{   
            //    this.bgimg.Src = Globals.GetVshopSkinPath(null) + "/images/process/panpic2.png";
            //}
            if ((lotteryActivity.StartTime < DateTime.Now) && (DateTime.Now < lotteryActivity.EndTime))
            {
                this.litActivityDesc.Text = lotteryActivity.ActivityDesc;
                this.litPrizeNames.Activity = lotteryActivity;
                this.litPrizeUsers.Activity = lotteryActivity;
                int userPrizeCount = VshopBrowser.GetUserPrizeCount(this.activityid);
                this.litActivityDesc.Text = this.litActivityDesc.Text;
                //剩余次数提示
                this.litSurplusNum.Text = string.Format("您一共有{0}次参与机会，目前还剩<b class=\"sign\">{1}</b>次。", lotteryActivity.MaxNum, lotteryActivity.MaxNum - userPrizeCount);

                PrizeQuery page = new PrizeQuery
			    {
                    ActivityId = this.activityid
			    };
                System.Collections.Generic.List<PrizeRecordInfo> prizeList = VShopHelper.GetPrizeListEx(page);
                if (prizeList != null && prizeList.Count > 0)
                    this.litNum.Text = prizeList.Count.ToString();
                else
                    this.litNum.Text = "0";

                //前台JS刷的营销数据
                if (!string.IsNullOrEmpty(lotteryActivity.GradeIds))
                    this.litNum.Text = lotteryActivity.GradeIds;
                
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\");});</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vremovebox.html";
            }
            base.OnInit(e);
        }

        [System.Web.Services.WebMethod]
        public static string AjaxGetNum() {
            return "111";
        }

    }
}

