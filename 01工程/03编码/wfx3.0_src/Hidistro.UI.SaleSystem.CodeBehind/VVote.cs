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
    public class VVote : VWeiXinOAuthTemplatedWebControl
    {
        //private HtmlGenericControl divVoteOk;
        //private HtmlInputHidden hidCheckNum;
        //private Literal litVoteName;
        //private Literal litVoteNum;
        //private VshopTemplatedRepeater rptVoteItems;
        private long voteId;
        //private int voteNum;

        public Panel panelHomePage;

        protected override void AttachChildControls()
        {
            if (!long.TryParse(this.Page.Request.QueryString["voteId"], out this.voteId))
            {
                base.GotoResourceNotFound("");
            }
            VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
            if (voteById != null && voteById.VoteId > 0)
                PageTitle.AddSiteNameTitle(voteById.VoteName);

            this.panelHomePage = (Panel)this.FindControl("panelHomePage");
            if (MemberProcessor.GetCurrentMember() == null)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                member.UserName = "";
                member.OpenId = "";
                member.CreateDate = DateTime.Now;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now;
                MemberProcessor.CreateMember(member);
                member = MemberProcessor.GetMember(generateId);
                HttpCookie cookie = new HttpCookie("Vshop-Member") {
                    Value = member.UserId.ToString(),
                    Expires = DateTime.Now.AddYears(10)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            //加载配置模块
            panelHomePage.Controls.Clear();
            string selectSql = string.Format("Select * From Yihui_Votes_Model Where VoteId={0} order by ModelSN", voteId);
            DataSet ds = DataBaseHelper.GetDataSet(selectSql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                baseModel.PKID = new Guid(dr["VMID"].ToString());//模块的内容ID
                baseModel.PageSN = dr["ModelSN"] + "";
                panelHomePage.Controls.Add(baseModel);
            }
        }

        //private string GetVoteItemCountString(int num)
        //{
        //    string str = string.Empty;
        //    if (this.voteNum != 0)
        //    {
        //        int num2 = (num * 30) / this.voteNum;
        //        for (int i = 0; i < num2; i++)
        //        {
        //            str = str + "&nbsp;";
        //        }
        //    }
        //    return str;
        //}

        //private void LoadVoteItemTable(DataTable table)
        //{
        //    table.Columns.Add("Lenth");
        //    table.Columns.Add("Percentage");
        //    foreach (DataRow row in table.Rows)
        //    {
        //        row["Lenth"] = this.GetVoteItemCountString((int) row["ItemCount"]);
        //        if (this.voteNum != 0)
        //        {
        //            row["Percentage"] = ((decimal.Parse(row["ItemCount"].ToString()) * 100M) / decimal.Parse(this.voteNum.ToString())).ToString("F2");
        //        }
        //        else
        //        {
        //            row["Percentage"] = 0.0;
        //        }
        //    }
        //}

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                //this.SkinName = "Skin-VVote.html";
                this.SkinName = "skin-vvoteindex.html";
            }
            base.OnInit(e);
        }
    }
}

