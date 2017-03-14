namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
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
    public class VStoreQRcode : VWeiXinOAuthTemplatedWebControl
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
            string savepath = HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("StoreSenderId_{0}", currentMember.UserId) + ".jpg";
            if (!File.Exists(savepath))
            {
                TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("StoreSenderId_{0}", currentMember.UserId), false);
            }
            string qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("StoreSenderId_{0}", currentMember.UserId) + ".jpg";
            litimgorcode.ImageUrl = qrCodeBackImgUrl;

            string storename = ManagerHelper.getPcOrderStorenameByClientuserid(currentMember.UserId);
            this.litstorename.Text = "「" + storename + "」专属二维码，推广扫码关注后，增加粉丝数！";
            PageTitle.AddSiteNameTitle(this.litstorename.Text + "我的二维码");


        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VStoreQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

