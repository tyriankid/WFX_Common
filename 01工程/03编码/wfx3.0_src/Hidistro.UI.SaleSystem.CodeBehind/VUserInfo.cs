namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VUserInfo : VshopTemplatedWebControl
    {
        private Image image;
        private Literal litUserType;//用户类别
        private Literal litUserName;//昵称
        protected override void AttachChildControls()
        {
            this.image = (Image)this.FindControl("image");
            this.litUserType = (Literal)this.FindControl("litUserType");
            this.litUserName = (Literal)this.FindControl("litUserName");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (currentMember != null)
            {
                HtmlInputText control = (HtmlInputText)this.FindControl("txtUserName");
                HtmlInputText text2 = (HtmlInputText) this.FindControl("txtRealName");
                HtmlInputText text3 = (HtmlInputText) this.FindControl("txtPhone");
                HtmlInputText text4 = (HtmlInputText) this.FindControl("txtEmail");
                litUserName.SetWhenIsNotNull(currentMember.UserName);
                control.SetWhenIsNotNull(currentMember.UserName);
                text2.SetWhenIsNotNull(currentMember.RealName);
                text3.SetWhenIsNotNull(currentMember.CellPhone);
                text4.SetWhenIsNotNull(currentMember.QQ);
            }
            if (!string.IsNullOrEmpty(currentMember.UserHead) && image!=null)
            {
                this.image.ImageUrl = currentMember.UserHead;
            }
            //通过当前用户id查找分销商信息,从而来判断用户类型
            DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(currentMember.UserId);
            if (this.litUserType != null)
            {
                /*
                if (currentDistributor == null)
                    this.litUserType.Text = "普通用户";
                else if (!string.IsNullOrEmpty(currentDistributor.ReferralPath))
                    this.litUserType.Text = "分仓";
                else if (currentDistributor.IsAgent == 0)
                    this.litUserType.Text = "分销商";
                */
                if (currentDistributor == null)
                    this.litUserType.Text = Hidistro.ControlPanel.Members.MemberHelper.GetMemberGrade(currentMember.GradeId).Name;
                else
                    this.litUserType.Text = DistributorGradeBrower.GetDistributorGradeInfo(currentDistributor.DistriGradeId).Name;

            }
            PageTitle.AddSiteNameTitle("修改用户信息");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VUserInfo.html";
            }
            base.OnInit(e);
        }
    }
}

