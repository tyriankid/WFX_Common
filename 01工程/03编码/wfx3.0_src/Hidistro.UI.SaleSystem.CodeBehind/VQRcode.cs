namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VQRcode : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litimage;
        private Literal litItemParams;
        private Literal litstorename;
        private Literal liturl;
        private Image litimgorcode;
        private Image image;


        protected override void AttachChildControls()
        {
            this.litimage = (Literal)this.FindControl("litimage");
            this.liturl = (Literal) this.FindControl("liturl");
            this.litstorename = (Literal) this.FindControl("litstorename");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            this.litimgorcode = (Image)this.FindControl("litimgorcode");
            this.image = (Image)this.FindControl("image");
            //头像
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (!string.IsNullOrEmpty(currentMember.UserHead) && this.image!=null)
            {
                this.image.ImageUrl = currentMember.UserHead;
            }
            //获取信息
            DistributorsInfo distributorsInfo = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
            string rid = "";//this.Page.Request.QueryString["ReferralId"];
            if (distributorsInfo != null)//如果当前是分销商或分销商以上的用户
            {
                rid = currentMember.UserId.ToString();
            }
            else
            {
                this.Page.Response.Redirect("MemberCenter.aspx");
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";
            
            if (!string.IsNullOrEmpty(rid))//分销商
            {               
                //店铺推广码: 设置带参数的固定二维码图片 (作为背景) 
                string savepath = HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("distributor_{0}", rid) + ".jpg";
                if (!File.Exists(savepath))
                {
                    TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("distributor_{0}", rid), false);
                }
                qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("distributor_{0}", rid) + ".jpg";
                litimgorcode.ImageUrl = qrCodeBackImgUrl;

                //快速开店码: 条码背景
                distributorsInfo = DistributorsBrower.GetCurrentDistributors(int.Parse(rid));
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sfsq"]) && this.Page.Request.QueryString["sfsq"] == "1")
                {
                    if (distributorsInfo != null && distributorsInfo.IsAgent == 1)//代理商
                    {
                        qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/ApplicationDescription.aspx?ReferralId=" + rid + "&sfsq=1";
                        litimgorcode.ImageUrl = "/API/GetQRCode.ashx?code=" + qrCodeBackImgUrl;  
                    }
                }
            }
            else//总店
            {
                qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Vshop/Default.aspx";
                litimgorcode.ImageUrl = "/API/GetQRCode.ashx?code=" + qrCodeBackImgUrl;  
            }

            this.litstorename.Text = (distributorsInfo == null) ? "总店" : distributorsInfo.StoreName;
            PageTitle.AddSiteNameTitle(this.litstorename.Text + "店铺二维码");


            //微信分享的宣传内容
            string str = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopSpreadingCodePic))
            {
                str = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopSpreadingCodePic;
            }
            this.litItemParams.Text = str + "|" + masterSettings.ShopSpreadingCodeName + "|" + masterSettings.ShopSpreadingCodeDescription;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

