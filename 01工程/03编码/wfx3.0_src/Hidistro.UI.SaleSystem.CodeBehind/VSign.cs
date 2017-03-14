using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{

    public class VSign : VWeiXinOAuthTemplatedWebControl
    {
        protected Common_SignController mainSignController;
        protected HtmlInputHidden hidUserId;
        protected HtmlInputHidden hidIsTodaySigned;

        protected override void AttachChildControls()
        {
            this.mainSignController = (Common_SignController)this.FindControl("mainSignController");
            this.hidUserId = (HtmlInputHidden)this.FindControl("hidUserId");
            this.hidIsTodaySigned = (HtmlInputHidden)this.FindControl("hidIsTodaySigned");

            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember == null)
                GotoResourceNotFound("请登录系统!");
            System.Data.DataTable dtInfo = PromoteHelper.GetSignRule();
            if(dtInfo.Rows.Count <=0 || dtInfo.Rows[0]["State"].ToString() == "0")
                GotoResourceNotFound("活动还未开启!");
            this.hidUserId.Value = currentMember.UserId.ToString();
            this.hidIsTodaySigned.Value = PromoteHelper.isTodaySigned(currentMember.UserId)?"1":"0";
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vSign.html";
            }
            base.OnInit(e);
        }


    }
}

