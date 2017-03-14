namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VGetCouponsEx : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptUseableCouponList;
        private VshopTemplatedRepeater rptCouponsAct;

        protected override void AttachChildControls()
        {
            
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }
            string str2 = this.Page.Request.QueryString["status"];
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "1";
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            int num = 0;
            int.TryParse(str2, out num);
            this.rptUseableCouponList = (VshopTemplatedRepeater)this.FindControl("rptUseableCouponList");
           
            this.rptCouponsAct = (VshopTemplatedRepeater)this.FindControl("rptCouponsAct");
            this.rptCouponsAct.ItemCommand += new RepeaterCommandEventHandler(this.rptCouponsAct_ItemCommand);
            this.rptCouponsAct.DataSource = CouponHelper.GetCouponsActNow();
            this.rptCouponsAct.DataBind();

            DataTable dtData = VshopBrowser.GetLotteryActivity_Valid(0, currentMember.UserId);
            this.rptUseableCouponList.DataSource = dtData;
            this.rptUseableCouponList.DataBind();
            
            PageTitle.AddSiteNameTitle("微信红包活动");
        }

        private void rptCouponsAct_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("tz"))
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                MemberInfo member = MemberProcessor.GetCurrentMember();
                int ShareID = CouponHelper.GetShareID(member.UserId,ID);
                if (ShareID > 0)
                {
                    this.Page.Response.Redirect("CouponsActShare.aspx?ID="+ShareID);
                }
                else
                {
                    this.Page.Response.Redirect("CouponsActDetail.aspx?ID="+ID);
                }
            }
        }

        public static string GetUrl(string id) {
            string strUrl = string.Empty;
            int ID = Convert.ToInt32(id);
            MemberInfo member = MemberProcessor.GetCurrentMember();
            int ShareID = CouponHelper.GetShareID(member.UserId, ID);
            if (ShareID > 0)
            {
                strUrl = "CouponsActShare.aspx?ID=" + ShareID;
            }
            else
            {
                strUrl = "CouponsActDetail.aspx?ID=" + ID;
            }
            return strUrl;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vGetCouponsEx.html";
            }
            base.OnInit(e);
        }
    }
}

