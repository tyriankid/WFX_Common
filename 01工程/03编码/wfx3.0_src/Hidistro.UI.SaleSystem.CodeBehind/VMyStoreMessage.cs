namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyStoreMessage : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptStoreMessage;
        protected TextBox txtMsgCon;
        protected Button btnSave;
        private HtmlInputHidden txtTotalPages;
        private HtmlInputHidden txtt;
        private int t = 0;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.rptStoreMessage = (VshopTemplatedRepeater)this.FindControl("rptStoreMessage");
            this.rptStoreMessage.ItemDataBound += new RepeaterItemEventHandler(this.rptStoreMessage_ItemDataBound);
            this.rptStoreMessage.ItemCommand += new RepeaterCommandEventHandler(this.rptStoreMessage_ItemCommond);
            this.txtMsgCon = (TextBox)this.FindControl("txtMsgCon");
            this.btnSave = (Button)this.FindControl("btnSave");
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
            this.txtt = (HtmlInputHidden)this.FindControl("txtt");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
           
            StoreMessageQuery query = new StoreMessageQuery();
            if (this.Page.Request.QueryString["t"] != null)
            {
                query.MsgUserID = currentMember.UserId;
                t = Convert.ToInt32(this.Page.Request.QueryString["t"]);
                txtt.Value = t + ""; ;
            }
            query.PageSize = num2;
            query.PageIndex = num;
            query.SortBy = "MsgTime";
            query.SortOrder = SortAction.Desc;
            DbQueryResult res = StoreMessageBrowser.GetMsgList(query);
            this.rptStoreMessage.DataSource = res.Data;
            this.rptStoreMessage.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(res.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("我的留言");
        }

        protected void rptStoreMessage_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btndel = (LinkButton)e.Item.Controls[0].FindControl("btndel");
                if (t == 0)
                {
                    btndel.Visible = false;
                }
            }
        }

        protected void rptStoreMessage_ItemCommond(object sender,RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                StoreMessageBrowser.DeleteStoreMessage(ID);
                this.Page.Response.Write("<script>alert('删除成功！');window.location='MyStoreMessage.aspx?t=1';</script>");
            }
        }

        protected void btnSave_Click(object sender,EventArgs e)
        {
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            //int DisUserID = Globals.GetCurrentDistributorId();
            string MsgCon = this.txtMsgCon.Text.Trim();
            StoreMessage sm = new StoreMessage();
            sm.DisUserID = 0;
            sm.MsgUserID = currentMember.UserId;
            sm.MessaegeCon = MsgCon;
            sm.MsgTime = DateTime.Now;
            StoreMessageBrowser.AddStoreMessage(sm);
            this.txtMsgCon.Text = "";
            if (t == 1)
            {
                this.Page.Response.Write("<script>alert('发布成功！');window.location='MyStoreMessage.aspx?t=1';</script>");
            }
            else
            {
                this.Page.Response.Write("<script>alert('发布成功！');window.location='MyStoreMessage.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyStoreMessage.html";
            }
            base.OnInit(e);
        }
    }
}

