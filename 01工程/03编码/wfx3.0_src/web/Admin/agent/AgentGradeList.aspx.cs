using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;

namespace Hidistro.UI.Web.Admin.agent
{
    public partial class AgentGradeList : AdminPage
    {
        string m_txtName = "";
        protected string LocalUrl = string.Empty;


        private void LoadParams()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Name"]))
                {
                    this.m_txtName = base.Server.UrlDecode(this.Page.Request.QueryString["Name"]);
                }
                this.txtName.Text = this.m_txtName;
            }
            else
            {
                this.m_txtName = this.txtName.Text;
            }
        }

        private void ReLoad(bool flag)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Name", this.txtName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReLoad(true);
        }

        private void BindDistributorGrade()
        {
            AgentGradeQuery entity = new AgentGradeQuery
            {
                AgentGradeName = this.m_txtName,
                SortBy = "AgentGradeId",
                SortOrder = SortAction.Asc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            DbQueryResult agentGradeRequest = DistributorGradeBrower.GetAgentGradeRequest(entity);
            this.rptList.DataSource = agentGradeRequest.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = agentGradeRequest.TotalRecords;
        }

        protected string FormatCommissionRise(object commissionrise)
        {
            decimal result = 0.00M;
            decimal.TryParse(commissionrise.ToString(), out result);
            if (result == 0.00M)
            {
                return "同类目佣金";
            }
            return "+" + result + "%";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.LoadParams();
            if (!base.IsPostBack)
            {
                this.BindDistributorGrade();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str2;
            int result = 0;
            int.TryParse(e.CommandArgument.ToString(), out result);
            if ((result > 0) && ((str2 = e.CommandName) != null))
            {
                if (str2 == "setdefault")
                {
                    DistributorGradeBrower.SetAgentGradeDefault(result);
                    this.ReLoad(true);
                }
                else if (str2 == "del")
                {
                    string str3 = DistributorGradeBrower.DelOneAgentGrade(result);
                    if (str3 != null)
                    {
                        if (str3 == "-1")
                        {
                            this.ShowMsg("不能删除，因为该等级下面已经有代理商！", false);
                            return;
                        }
                        if (str3 == "1")
                        {
                            this.ReLoad(true);
                            return;
                        }
                    }
                    this.ShowMsg("删除失败", false);
                }
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //ImageButton button = (ImageButton)e.Item.FindControl("imgBtnSetDefault");
                //LinkButton button2 = (LinkButton)e.Item.FindControl("lbtnDel");
                //if (((DataRowView)e.Item.DataItem).Row["IsDefault"].ToString() == "False")
                //{
                //    button.ImageUrl = "../images/ta.gif";
                //    button.ToolTip = "点击设置为默认";
                //    button2.OnClientClick = "return confirm('确认要删除该分销商等级吗？')";
                //}
                //else
                //{
                //    button.ToolTip = "已经是默认的";
                //    button.Enabled = false;
                //    button2.Enabled = false;
                //    button2.Text = "默认等级不能删除";
                //}
            }
        }
    }
}
