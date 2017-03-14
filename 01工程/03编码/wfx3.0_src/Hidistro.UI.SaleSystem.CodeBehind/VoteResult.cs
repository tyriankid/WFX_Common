namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Function;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VoteResult : VWeiXinOAuthTemplatedWebControl
    {
        private int voteId;
        public Literal litVoteResult;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["voteId"], out this.voteId))
            {
                base.GotoResourceNotFound("");
            }
            //设置微信流量其中标头
            VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
            if (voteById != null && voteById.VoteId > 0)
                PageTitle.AddSiteNameTitle(voteById.VoteName + "结果");

            this.litVoteResult = (Literal)this.FindControl("litVoteResult");
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            //加载配置模块
            string selectSql = string.Format("Select * From Yihui_Votes_Model Where VoteId={0} order by ModelSN;", voteId);
            selectSql += string.Format("Select * From YiHui_HomePage_Model where PageID in (select VMID from Yihui_Votes_Model where VoteId = {0});",voteId);
            selectSql += string.Format("Select * From YIHui_Votes_Model_Detail where VMID in (select VMID from Yihui_Votes_Model where VoteId = {0}) order by VMID,Scode;",voteId);
            selectSql += string.Format("Select * From YiHui_Votes_Model_Result where VoteId = {0}",voteId);
            DataSet ds = DataBaseHelper.GetDataSet(selectSql);

            DataTable dtvm = ds.Tables[0];
            DataTable dthpm_Model = ds.Tables[1];
            DataTable dtvm_detail = ds.Tables[2];
            DataTable dtvm_Result = ds.Tables[3];

            int i=0;
            foreach (DataRow dr in dtvm.Rows)
            {
                if (dr["ModelCode"].ToString() == "XuanXiang")
                { 
                    i++;
                    DataRow[] drs = dthpm_Model.Select(string.Format("PageID = '{0}'",dr["VMID"].ToString()),"",DataViewRowState.CurrentRows);
                    if(drs.Length >0)
                    {
                        string[] strValues = drs[0]["PMContents"].ToString().Split('♦');
                        if (strValues.Length == 3)
                        {
                            builder.AppendFormat("<div id='vote-result{0}' i='{0}' class='vote-result'>", i);
                            builder.Append("<div class='vote-result-con'>");
                            builder.AppendFormat("<div class='vote-result-con-title'><h3>{0}</h3><h4>{1}</h4></div>", strValues[0], strValues[1]);
                            builder.Append("<ul>");
                            DataRow[] drsdetail = dtvm_detail.Select(string.Format("VMID = '{0}'", dr["VMID"].ToString()), "Scode", DataViewRowState.CurrentRows);
                            foreach (DataRow drxm in drsdetail)
                            {
                                DataRow[] drSumResult = dtvm_Result.Select(string.Format("PMID = '{0}'", drs[0]["PMID"].ToString()), "", DataViewRowState.CurrentRows);//当前投票项参与总人数
                                DataRow[] drResult = dtvm_Result.Select(string.Format("PMID = '{0}' and Result like '%{1},%'", drs[0]["PMID"].ToString(), drxm["Value"].ToString()), "", DataViewRowState.CurrentRows);
                                string strBl = "0%";
                                if (drSumResult.Length > 0)
                                {
                                    double dbBl = Convert.ToDouble(drResult.Length) / Convert.ToDouble(drSumResult.Length);
                                    strBl = string.Format("{0:0.00%}", dbBl);
                                }
                                builder.AppendFormat("<li n='{0}'>{1}<div class='progress'><p style='width:{4}'></p><span style='left:{4}'>{4}</span></div></li>", drxm["Scode"].ToString(), drxm["Name"].ToString(), drResult.Length, drSumResult.Length, strBl);
                            }
                            builder.Append("</ul>");
                            builder.Append("</div>");
                            builder.Append("</div>");    
                        }
                    }
                          
                }
            }
            this.litVoteResult.Text = builder.ToString();

        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-voteresult.html";
            }
            base.OnInit(e);
        }
    }
}

