namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VChirldrenDistributors : VWeiXinOAuthTemplatedWebControl
    {
        private Panel onedistributor;
        private VshopTemplatedRepeater rpdistributor;
        private Panel twodistributor;
        private Literal leftTitle;//左侧动态标题
        private Literal rightTitle;//右侧动态标题

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("下级分销商");
            this.onedistributor = (Panel)this.FindControl("onedistributor");
            this.twodistributor = (Panel)this.FindControl("twodistributor");
            this.rpdistributor = (VshopTemplatedRepeater)this.FindControl("rpdistributor");
            this.leftTitle = (Literal)this.FindControl("leftTitle");
            this.rightTitle = (Literal)this.FindControl("rightTitle");

            DistributorsQuery query = new DistributorsQuery
            {
                PageIndex = 1,
                PageSize = 0x2710
            };
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId());
            query.GradeId = 2;
            this.twodistributor.Visible = false;
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["gradeId"], out result))
            {
                query.GradeId = result;
            }
            query.UserId = currentDistributors.UserId;
            //新增判断,如果是代理商,[我的下属]页面就会变成下级代理+下级分销的模式, 如果是分销商,则按默认的来
            switch (currentDistributors.IsAgent)
            {
                case 1://代理商
                    query.AgentPath = currentDistributors.UserId.ToString();
                    this.rpdistributor.DataSource = DistributorsBrower.GetDownDistributorsAndAgents(query);
                    this.leftTitle.Text = "下级代理商";
                    this.rightTitle.Text = "下级分销商";
                    break;
                case 0://分销商
                    query.ReferralPath = currentDistributors.UserId.ToString();
                    this.rpdistributor.DataSource = DistributorsBrower.GetDownDistributors(query);
                    this.leftTitle.Text = "二级分销商";
                    this.rightTitle.Text = "三级分销商";
                    break;
            }

            this.rpdistributor.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VChirldrenDistributors.html";
            }
            base.OnInit(e);
        }
    }
}
