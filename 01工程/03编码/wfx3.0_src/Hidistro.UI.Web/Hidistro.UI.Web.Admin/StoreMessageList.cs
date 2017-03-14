namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class StoreMessageList : AdminPage
    {
        protected Button btnSearch;
        protected Grid grdStoreMessage;
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtUserName;
        protected TextBox txtStoreName;

        private void BindSearch()
        {
            StoreMessageQuery query = new StoreMessageQuery
            {
                UserName = this.txtUserName.Text.Trim(),
                StoreName = this.txtStoreName.Text,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "MsgTime",
                SortOrder = SortAction.Desc
            };
            DbQueryResult res = StoreMessageBrowser.GetMsgList(query);
            this.grdStoreMessage.DataSource = res.Data;
            this.grdStoreMessage.DataBind();
            this.pager.TotalRecords = res.TotalRecords;
            this.pager1.TotalRecords = res.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindSearch();
        }

        //private void grdArticleList_ReBindData(object sender)
        //{
        //    this.BindSearch();
        //}

       

        private void grdArticleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = (int)this.grdStoreMessage.DataKeys[e.RowIndex].Value;
            if (StoreMessageBrowser.DeleteStoreMessage(ID))
            {
                this.BindSearch();
                this.ShowMsg("成功删除了一条留言", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            IList<int> msgIDs = new List<int>();
            int num = 0;
            foreach (GridViewRow row in this.grdStoreMessage.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    int item = Convert.ToInt32(this.grdStoreMessage.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    msgIDs.Add(item);
                }
            }
            if (num != 0)
            {
                int num3 = StoreMessageBrowser.DeleteStoreMessages(msgIDs);
                this.BindSearch();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除{0}条留言", new object[] { num3 }), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的留言", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
               
            }
            else
            {
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdStoreMessage.RowDeleting += new GridViewDeleteEventHandler(this.grdArticleList_RowDeleting);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            //this.grdStoreMessage.ReBindData += new Grid.ReBindDataEventHandler(this.grdArticleList_ReBindData);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
               
                this.BindSearch();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        
    }
}

