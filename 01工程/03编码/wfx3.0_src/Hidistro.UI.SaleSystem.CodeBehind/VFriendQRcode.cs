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
    public class VFriendQRcode : VWeiXinOAuthTemplatedWebControl
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
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //string qrCodeBackImgUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";

            //朋友推广码: 
            string savepath = HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("fromMember_{0}", currentMember.UserId) + ".jpg";
            if (!File.Exists(savepath))
            {
                TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("fromMember_{0}", currentMember.UserId), false);
            }
            string qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("fromMember_{0}", currentMember.UserId) + ".jpg";
            litimgorcode.ImageUrl = qrCodeBackImgUrl;


            this.litstorename.Text = "我的专属二维码，成功邀请好友关注后，可获得一张优惠券！" ;
            PageTitle.AddSiteNameTitle(this.litstorename.Text + "我的二维码");


        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VFriendQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

