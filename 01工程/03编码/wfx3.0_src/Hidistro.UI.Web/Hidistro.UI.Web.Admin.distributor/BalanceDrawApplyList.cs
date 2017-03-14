using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
namespace Hidistro.UI.Web.Admin.distributor
{
    public class BalanceDrawApplyList : AdminPage
    {
        protected System.Web.UI.WebControls.Button btapply;
        protected System.Web.UI.WebControls.Button btnRefuse;//拒绝按钮
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdapplyid;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdreferralblance;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hduserid;
        protected Pager pager;
        protected System.Web.UI.WebControls.Repeater reBalanceDrawRequest;
        private string RequestEndTime = "";
        private string RequestStartTime = "";
        private string StoreName = "";
        protected System.Web.UI.HtmlControls.HtmlTextArea txtcontent;
        protected WebCalendar txtRequestEndTime;
        protected WebCalendar txtRequestStartTime;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery
            {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "CheckTime",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "0",
                UserId = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity);
            this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
            this.reBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }
        private void btnApply_Click(object sender, System.EventArgs e)
        {
            int id = int.Parse(this.hdapplyid.Value);
            string remark = this.txtcontent.Value;
            int userId = int.Parse(this.hduserid.Value);
            decimal referralRequestBalance = decimal.Parse(this.hdreferralblance.Value);
            if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
            {
                if (VShopHelper.UpdateBalanceDistributors(userId, referralRequestBalance))
                {
                    this.ShowMsg("结算成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("结算失败", false);
                }
            }
            else
            {
                this.ShowMsg("结算失败", false);
            }
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }

        /// <summary>
        /// 格式化支付方式,0:微信快捷支付,1:支付宝:2:银行
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public string formatPayType(int requestType)
        {
            string result = "";
            switch (requestType)
            {
                case 0:
                    result= "微信红包";
                    break;
                case 1:
                    result= "支付宝";
                    break;
                case 2:
                    result= "银行打款";
                    break;
            }
            return result;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                this.txtStoreName.Text = this.StoreName;
                this.txtRequestStartTime.Text = this.RequestStartTime;
                this.txtRequestEndTime.Text = this.RequestEndTime;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                this.RequestStartTime = this.txtRequestStartTime.Text;
                this.RequestEndTime = this.txtRequestEndTime.Text;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btapply.Click += new System.EventHandler(this.btnApply_Click);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("RequestStartTime", this.txtRequestStartTime.Text);
            queryStrings.Add("RequestEndTime", this.txtRequestEndTime.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }


        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int num = 0;
            int.TryParse(e.CommandArgument.ToString(), out num);


            if (num > 0)
            {
                string commandName = e.CommandName;
                string str = commandName;
                
                if (commandName != null)
                {
                    switch (str)
                    {
                        case "sendredpack":
                            if (str != "sendredpack")
                            {
                                return;
                            }
                            string balanceDrawRequest = DistributorsBrower.SendRedPackToBalanceDrawRequest(num);
                            string str1 = balanceDrawRequest;
                            string str2 = str1;
                            if (str1 != null)
                            {
                                if (str2 == "-1")
                                {
                                    base.Response.Redirect(string.Concat("SendRedpackRecord.aspx?serialid=", num));
                                    base.Response.End();
                                    return;
                                }
                                if (str2 == "1")
                                {
                                    base.Response.Redirect(string.Concat("SendRedpackRecord.aspx?serialid=", num));
                                    base.Response.End();
                                    return;
                                }
                            }
                            this.ShowMsg(string.Concat("生成红包失败，原因是：", balanceDrawRequest), false);
                            break;

                        case "refuse":
                                int id = Convert.ToInt32(e.CommandArgument);
                                string remark = "提现申请不通过";
                                if (VShopHelper.UpdateBalanceDrawRequest(id, remark))
                                {
                                    this.ShowMsg("操作成功,该提现申请已被拒绝", true);
                                    this.BindData();
                                }
                                else
                                {
                                    this.ShowMsg("操作失败", false);
                                }
                            break;
                    }
                }
            }


        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                LinkButton linkButton = (LinkButton)e.Item.FindControl("lkBtnSendRedPack");
                int num = int.Parse(((DataRowView)e.Item.DataItem).Row["serialid"].ToString());
                if (int.Parse(((DataRowView)e.Item.DataItem).Row["RedpackRecordNum"].ToString()) > 0)
                {
                    linkButton.PostBackUrl = string.Concat("SendRedpackRecord.aspx?serialid=", num);
                    linkButton.Text = "查看微信红包";
                    return;
                }
                int num1 = int.Parse(((DataRowView)e.Item.DataItem).Row["RequestType"].ToString());
                linkButton.OnClientClick = "return confirm('提现金额将会拆分为最大金额为200元的微信红包，等待发送！确定生成微信红包吗？')";
                if (num1 == 0)
                {
                    linkButton.Style.Add("color", "red");
                }

                LinkButton refuse = (LinkButton)e.Item.FindControl("btnRefuse");

                refuse.OnClientClick = "return confirm('确认要拒绝【" + ((System.Data.DataRowView)(e.Item.DataItem)).Row["storename"] + "】的提款申请吗?')";
                }
            }
        }
    }

