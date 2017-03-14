namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Config;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VApplicationDescription : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litApplicationDescription;
        private Literal litButton;
        private Literal customInputs;//自定义控件

        protected override void AttachChildControls()
        {
            this.litApplicationDescription = (Literal)this.FindControl("litApplicationDescription");
            this.litButton = (Literal)this.FindControl("litButton");
            this.customInputs = (Literal)this.FindControl("customInputs");//自定义控件

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //根据自定义xml判断是否显示分销描述
            if (CustomConfigHelper.Instance.IsDistributorDescriptionOn == "true" && litApplicationDescription != null)
                this.litApplicationDescription.Text = masterSettings.ApplicationDescription;
            //自定义控件的插入
            if (CustomConfigHelper.Instance.CustomInputs != "" && this.customInputs != null)
                this.customInputs.Text = CustomConfigHelper.Instance.CustomInputs;

            if (litButton != null)
                litButton.Text = "<a type=\"button\" class=\"btn btn-danger btn-block btn-apply\" href=\"DistributorValid.aspx?action=" + CustomConfigHelper.Instance.DistributorType_Name + "\">" + CustomConfigHelper.Instance.DistributorType_Showbutton + "</a>";
            //自定义标题的赋值
            if (CustomConfigHelper.Instance.ApplicationDescriptionTitle != "分销商描述")
                PageTitle.AddSiteNameTitle(CustomConfigHelper.Instance.ApplicationDescriptionTitle);

            this.Page.Session["stylestatus"] = "2";

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sfsq"]) && this.Page.Request.QueryString["sfsq"] == "1")
            {
                this.Page.Response.Redirect("DistributorValid.aspx?sfsq=1&ReferralId=" + this.Page.Request.QueryString["ReferralId"]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (Hidistro.SaleSystem.Vshop.MemberProcessor.GetCurrentMember() == null)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/UserLogin.aspx");
                return;
            }
            //新增判断:如果当前会员已经是分销商,则直接跳转到店铺管理页面
            int currentMemberUserId = Globals.GetCurrentMemberUserId();
            if (Hidistro.SaleSystem.Vshop.DistributorsBrower.GetUserIdDistributors(currentMemberUserId)!=null)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/Vshop/DistributorCenter.aspx");
                return;
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VApplicationDescription.html";
            }
            base.OnInit(e);
        }
    }
}

