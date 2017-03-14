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

    [ParseChildren(true)]
    public class VBigWheelEx : VWeiXinOAuthTemplatedWebControl
    {
        private int activityid;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["activityid"], out this.activityid))
            {
                base.GotoResourceNotFound("");
            }
           
            PageTitle.AddSiteNameTitle("微信红包活动");
            LotteryActivityInfo lotteryActivity = VshopBrowser.GetLotteryActivity(this.activityid);
            if (lotteryActivity == null)
            {
                base.GotoResourceNotFound("");
            }
           
            if ((lotteryActivity.StartTime < DateTime.Now) && (DateTime.Now < lotteryActivity.EndTime))
            {
               
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vbigwheelex.html";
            }
            base.OnInit(e);
        }
    }
}

