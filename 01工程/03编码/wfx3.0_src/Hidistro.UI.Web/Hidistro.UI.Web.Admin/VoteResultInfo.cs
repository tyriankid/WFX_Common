using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Votes)]
    public class VoteResultInfo : AdminPage
	{
        protected string LocalUrl = string.Empty;
        protected Literal litResultInfoTable;
        private long voteId;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
			{
				base.GotoResourceNotFound();
				return;
			}
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());

			if (!this.Page.IsPostBack)
			{
                //投票活动表信息
                VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);

                string selectSql = string.Format(@"select vm.*,hm.*,model.ModelName from Yihui_Votes_Model as vm 
                    left join YiHui_HomePage_Model as hm on vm.VMID = hm.PageID 
                    left join YiHui_Model as model on vm.ModelCode = model.ModelCode 
                    where vm.VoteId = {0} order by vm.ModelSN;", this.voteId);
                selectSql += string.Format(@"select mr.UserId,mb.UserName,mb.RealName from YiHui_Votes_Model_Result as mr 
                    left join dbo.aspnet_Members as mb on mr.UserId = mb.UserId where mr.VoteId = {0} 
                    group by mr.UserId,UserName,RealName;", this.voteId);
                selectSql += string.Format("select * from dbo.YiHui_Votes_Model_Result where VoteId = {0};", this.voteId);
                selectSql += string.Format("select * from dbo.YIHui_Votes_Model_Detail");
                DataSet dsData = DataBaseHelper.GetDataSet(selectSql);

                DataTable dtHomePageModel = dsData.Tables[0];
                DataTable dtVoteUser = dsData.Tables[1];
                DataTable dtResult = dsData.Tables[2];
                DataTable dtDetail = dsData.Tables[3];

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                //先构建结果表显示列
                if (dtHomePageModel.Rows.Count > 0)
                {
                    //设置标题行
                    builder.Append("<table><tr>");
                    builder.Append("<td>用户名/昵称</td>");
                    foreach (DataRow drTitle in dtHomePageModel.Rows)
                    {
                        //图片、文本除外
                        if(drTitle["ModelCode"].ToString() != "TuPian" && drTitle["ModelCode"].ToString() != "WenBen")
                        {
                            string[] strContens = drTitle["PMContents"].ToString().Split('♦');
                            if(strContens.Length >0)
                            {
                                builder.AppendFormat("<td>{0}</td>", strContens[0]);
                            }
                        }
                    }
                    builder.Append("</tr>");

                    if(dtVoteUser.Rows.Count > 0 && dtResult.Rows.Count >0)
                    {
                        foreach(DataRow drUser in dtVoteUser.Rows)
                        {
                            builder.Append("<tr>");
                            builder.AppendFormat("<td>{0}/{1}</td>",drUser["UserName"].ToString(),drUser["RealName"].ToString());
                            foreach (DataRow drTitle in dtHomePageModel.Rows)
                            { 
                                //图片、文本除外
                                if (drTitle["ModelCode"].ToString() != "TuPian" && drTitle["ModelCode"].ToString() != "WenBen")
                                {
                                    DataRow[] drsResult = dtResult.Select(string.Format("PMID = '{0}' and UserId = '{1}'", drTitle["PMID"].ToString(), drUser["UserId"].ToString()), "", DataViewRowState.CurrentRows);
                                    if (drsResult.Length > 0)
                                    {
                                        if (drTitle["ModelCode"].ToString() == "XuanXiang")
                                        {
                                            builder.AppendFormat("<td>{0}</td>", drsResult[0]["Result"].ToString());
                                            //DataRow[] drXuanXiang = dtDetail.Select(string.Format("VMID = '{0}' and Value = '{1}'", drTitle["PageID"].ToString(), drsResult[0]["Result"].ToString()), "", DataViewRowState.CurrentRows);
                                            //if (drXuanXiang.Length > 0)
                                            //    builder.AppendFormat("<td>{0}</td>", drXuanXiang[0]["Name"].ToString());
                                            //else
                                            //    builder.Append("<td></td>");
                                        }
                                        else
                                            builder.AppendFormat("<td>{0}</td>", drsResult[0]["Result"].ToString().Length > 20 ? drsResult[0]["Result"].ToString().Substring(0,20) : drsResult[0]["Result"].ToString());
                                    }
                                    else
                                        builder.Append("<td></td>");
                                }
                            }
                            builder.Append("</tr>");
                        }
                    }
                    builder.Append("</table>");
                }
                //设置前端显示
                litResultInfoTable.Text = builder.ToString();

			}
		}
	}
}
