namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VCouponsActShare : VWeiXinOAuthTemplatedWebControl
    {
        protected Image uimg;
        protected Button btnlq;
        protected HtmlInputHidden txtID;
        protected HtmlInputHidden txtBgImg;
        protected Literal litMoney;

        protected override void AttachChildControls()
        {
            this.uimg = (Image)this.FindControl("uimg");
            this.btnlq = (Button)this.FindControl("btnlq");
            this.txtID = (HtmlInputHidden)this.FindControl("txtID");
            this.txtBgImg = (HtmlInputHidden)this.FindControl("txtBgImg");
            this.litMoney = (Literal)this.FindControl("litMoney");
            this.btnlq.Click += new EventHandler(this.btnlq_Click);
                if (this.Page.Request.QueryString["ID"] != null)
                {
                    int ID = Convert.ToInt32(this.Page.Request.QueryString["ID"]);
                    if (ID != 0)
                    {
                        CouponsActShare cas=CouponHelper.GetCouponsActShare(ID);
                        CouponsAct ca=CouponHelper.GetCouponsAct(cas.CouponsActID);
                        this.txtBgImg.Value = ca.BgImg;
                        this.txtID.Value = cas.ID + "" ;
                        this.uimg.ImageUrl = cas.UserImg;
                        CouponInfo c=CouponHelper.GetCoupon(ca.CouponsID);
                        this.litMoney.Text = c.DiscountValue.ToString("0.00");
                    }
                }
                else
                {

                }
            
        }

        private void btnlq_Click(object sender,EventArgs e)
        {
            int ID = Convert.ToInt32(this.txtID.Value);
            CouponsActShare cas = CouponHelper.GetCouponsActShare(ID);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (!CouponHelper.CheckUserIsCoupon(currentMember.UserId, cas.CouponsID))
            {
                CouponsAct ca = CouponHelper.GetCouponsAct(cas.CouponsActID);
                int NowDayCount = CouponHelper.GetNowDayCount(cas.ID);
                if (ca.ColValue1 == 0 || ca.ColValue1 > NowDayCount)
                {
                    string claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    CouponItemInfo item = new CouponItemInfo();
                    item.CouponId = cas.CouponsID;
                    item.ClaimCode = claimCode;
                    item.UserId = currentMember.UserId;
                    item.UserName = currentMember.UserName;
                    item.EmailAddress = currentMember.Email;
                    item.GenerateTime = DateTime.Now;
                    item.FromInfo = cas.ID;
                        //new CouponItemInfo(cas.CouponsID, claimCode, new int?(currentMember.UserId), currentMember.UserName, currentMember.Email, System.DateTime.Now,cas.ID);
                    if (CouponHelper.SendClaimCodes(cas.CouponsID, item))
                    {
                        this.Page.Response.Write("<script>alert('领取优惠卷成功！');</script>");
                    }
                    else
                    {
                        this.Page.Response.Write("<script>alert('服务器繁忙，请重新尝试！');</script>");
                    }
                }
                else
                {
                    this.Page.Response.Write("<script>alert('今天可被领取的优惠卷被领完啦，您可以转到用户中心里微信活动栏目里面分享并领取优惠卷');</script>");
                }

            }
            else
            {
                this.Page.Response.Write("<script>alert('您领取过该优惠卷啦~');</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VCouponsActShare.html";
            }
            base.OnInit(e);
        }
    }
}

