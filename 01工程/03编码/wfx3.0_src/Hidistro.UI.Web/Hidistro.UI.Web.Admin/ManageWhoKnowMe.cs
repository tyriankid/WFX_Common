using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class ManageWhoKnowMe : AdminPage
    {
        protected System.Web.UI.HtmlControls.HtmlAnchor addactivity;
        protected System.Web.UI.WebControls.Literal Litdesc;
        protected System.Web.UI.WebControls.Literal LitTitle;
        protected Pager pager;
        protected System.Web.UI.WebControls.Repeater rpWhoKnowMe;
        protected int type;
        protected void BindMaterial()
        {
            WhoKnowMeQuery page = new WhoKnowMeQuery
            {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "StartDate",
                SortOrder = SortAction.Desc
            };
            DbQueryResult WKMList = VShopHelper.GetWKMList(page);
            //如果当前人已经设置了答案,并且活动id重复,则不允许重复答题, 直接刷新页面跳到type2


            this.rpWhoKnowMe.DataSource = WKMList.Data;
            this.rpWhoKnowMe.DataBind();
            this.pager.TotalRecords = WKMList.TotalRecords;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindMaterial();
            }
        }
        protected void rpWhoKnowMe_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (PromoteHelper.DeleteWKMInfo(new Guid(e.CommandArgument.ToString())))
                {
                    this.ShowMsg("删除成功", true);
                    this.BindMaterial();
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
        }


        public string GetUrl(object activityId)
        {
            string str = string.Empty;
            string result;

            result = string.Concat(new object[]
				{
					"/Vshop/WhoKnowMe.aspx?wkmId=",
					activityId
				});
            return result;
        }
    }
}
