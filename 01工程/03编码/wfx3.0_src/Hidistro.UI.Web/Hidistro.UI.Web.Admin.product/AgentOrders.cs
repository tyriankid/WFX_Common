using System;
using Hishop.Alipay.OpenHome;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.ControlPanel.Promotions;
using Hishop.AliPay.QuickPay;


namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public class AgentOrders : AdminPage
    {
        protected string Reurl = string.Empty;

        protected HyperLink hlinkAllOrder;

        protected HyperLink hlinkNotPay;

        protected HyperLink hlinkYetPay;

        protected HyperLink hlinkSendGoods;

        protected HyperLink hlinkDelete;

        protected HyperLink hlinkTradeFinished;

        protected HyperLink hlinkClose;

        protected HyperLink hlinkHistory;

        protected WebCalendar calendarStartDate;

        protected WebCalendar calendarEndDate;

        //start代理商处理相关添加内容
        protected DropDownList ddlOrderAgent;

        protected TextBox txtRealName;

        protected TextBox txtUserName;

        protected TextBox txtOrderId;

        protected Label lblStatus;

        protected TextBox txtProductName;

        protected TextBox txtShopTo;

        protected DropDownList ddlIsPrinted;

        protected ShippingModeDropDownList shippingModeDropDownList;

        protected RegionSelector dropRegion;

        protected DropDownList OrderFromList;

        protected Button btnSearchButton;

        protected PageSize hrefPageSize;

        protected Pager pager1;

        protected ImageLinkButton lkbtnDeleteCheck;

        protected HtmlInputHidden hidOrderId;

        protected DataList dlstOrders;

        protected Pager pager;

        protected CloseTranReasonDropDownList ddlCloseReason;

        protected FormatedMoneyLabel lblOrderTotalForRemark;

        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

        protected TextBox txtRemark;

        protected Label lblOrderId;

        protected Label lblOrderTotal;

        protected Label lblRefundType;

        protected Label lblRefundRemark;

        protected Label lblContacts;

        protected Label lblEmail;

        protected Label lblTelephone;

        protected Label lblAddress;

        protected TextBox txtAdminRemark;

        protected Label return_lblOrderId;

        protected Label return_lblOrderTotal;

        protected Label return_lblRefundType;

        protected Label return_lblReturnRemark;

        protected Label return_lblContacts;

        protected Label return_lblEmail;

        protected Label return_lblTelephone;

        protected Label return_lblAddress;

        protected TextBox return_txtRefundMoney;

        protected TextBox return_txtAdminRemark;

        protected Label replace_lblOrderId;

        protected Label replace_lblOrderTotal;

        protected Label replace_lblComments;

        protected Label replace_lblContacts;

        protected Label replace_lblEmail;

        protected Label replace_lblTelephone;

        protected Label replace_lblAddress;

        protected Label replace_lblPostCode;

        protected TextBox replace_txtAdminRemark;

        protected HtmlInputHidden hidOrderTotal;

        protected HtmlInputHidden hidRefundType;

        protected HtmlInputHidden hidRefundMoney;

        protected HtmlInputHidden hidAdminRemark;

        protected Button btnCloseOrder;

        protected Button btnAcceptRefund;

        protected Button btnRefuseRefund;

        protected Button btnAcceptReturn;

        protected Button btnRefuseReturn;

        protected Button btnAcceptReplace;

        protected Button btnRefuseReplace;

        protected Button btnRemark;

        protected Button btnOrderGoods;

        protected Button btnProductGoods;

        protected Button btnExport;

        protected HtmlControl agentPpurchase;//代理商采购相关功能区域

        protected System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;//判断是不是爽爽挝啡的隐藏域

        protected DropDownList ddlStoreType;

        private void bindOrderType()
        {
            int result = 0;
            int.TryParse(base.Request.QueryString["orderType"], out result);
            this.OrderFromList.SelectedIndex = result;
        }

        private void btnRefuseRefund_Click(object obj, EventArgs eventArg)
        {
            string userName = ManagerHelper.GetCurrentManager().UserName;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(hidOrderId.Value);
            RefundHelper.EnsureRefund(orderInfo.OrderId, userName, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退款", true);
        }



        private void dlstOrders_ItemDataBound(object obj, DataListItemEventArgs dataListItemEventArg)
        {
            if (dataListItemEventArg.Item.ItemType == ListItemType.Item || dataListItemEventArg.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderStatus orderStatu = (OrderStatus)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "OrderStatus");
                Button button8 = (Button)dataListItemEventArg.Item.FindControl("Button8");
                button8.Visible = orderStatu == OrderStatus.WaitBuyerPay ? true : false;
            }
            
        }

        private void btnSendGoods_Click(object sender, System.EventArgs e)
        {

        }



        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            string roleType = ManagerHelper.GetRole(ManagerHelper.GetCurrentManager().RoleId).RoleName;
            if (roleType == "门店发货" || roleType == "活动账号")
            {
                orderQuery.Sender = ManagerHelper.GetCurrentManager().UserId.ToString();
                orderQuery.ClientUserId = ManagerHelper.GetCurrentManager().ClientUserId;
            }
            
            DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            
            foreach (DataRow row in ((DataTable)orders.Data).Rows)
            {
                if (row["UserName"].ToString() == "[堂食用户]" || row["UserName"].ToString() == "[匿名用户]")
                {
                    row["ModeName"] = "店内";
                    row["Address"] = "店内";
                }
            }
            this.dlstOrders.DataSource = orders.Data;
            this.dlstOrders.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;
            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.lblStatus.Text = orderQuery.Status.ToString();
            this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
            if (orderQuery.IsPrinted.HasValue)
            {
                this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
            }
            if (orderQuery.RegionId.HasValue)
            {
                this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
            }
            //代理商采购添加
            if (orderQuery.OrderAgent > 0)
                this.ddlOrderAgent.SelectedValue = orderQuery.OrderAgent.ToString();
            //this.txtRealName.Text = orderQuery.RealName;
            this.ddlStoreType.SelectedValue = orderQuery.StoreType;


        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out num))
                {
                    query.Status = (OrderStatus)num;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int num3 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            int num4;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out num4))
            {
                query.RegionId = new int?(num4);
            }
            int num5;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && int.TryParse(this.Page.Request.QueryString["UserId"], out num5))
            {
                query.UserId = new int?(num5);
            }
            //下属信息浏览添加
            int num7;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralUserId"]) && int.TryParse(this.Page.Request.QueryString["ReferralUserId"], out num7))
            {
                query.referralUserId = new int?(num7);
            }
            int result = 0;
            if (int.TryParse(base.Request.QueryString["orderType"], out result) && result > 0)
            {
                query.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)result);
            }
            //代理商采购添加
            int num6;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderAgent"]) && int.TryParse(this.Page.Request.QueryString["OrderAgent"], out num6))
            {
                query.OrderAgent = new int?(num6);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
            {
                query.RealName = Globals.UrlDecode(this.Page.Request.QueryString["RealName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreType"]))
            {
                query.StoreType = Globals.UrlDecode(this.Page.Request.QueryString["StoreType"]);
            }
            //爽爽挝啡的特殊功能过滤
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)//如果门店配送功能开启,载入页面时根据门店的子账号进行订单过滤
            {
                //门店配送添加子账号的过滤
                DistributorsInfo shipDistributor = DistributorsBrower.GetDistributorInfo(currentManager.ClientUserId);
                if (shipDistributor != null)
                {
                    query.modeName = shipDistributor.StoreName;
                }
                this.specialHideShow.Value = "sswk";//爽爽挝啡
                //活动账号的过滤
                if (ManagerHelper.GetRole(currentManager.RoleId).RoleName == "活动账号")
                {
                    query.UserName = "[活动用户]";
                }

            }
            query.UserId = Convert.ToInt32("99999"+currentManager.UserId.ToString());
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        private void ReloadOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("UserName", this.txtUserName.Text);
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("ProductName", this.txtProductName.Text);
            queryStrings.Add("ShipTo", this.txtShopTo.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("OrderType", this.OrderFromList.SelectedValue);
            queryStrings.Add("OrderStatus", this.lblStatus.Text);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                queryStrings.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                queryStrings.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
            }
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                queryStrings.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlOrderAgent.SelectedValue))
            {
                queryStrings.Add("OrderAgent", this.ddlOrderAgent.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.ddlStoreType.SelectedValue))
            {
                queryStrings.Add("StoreType", this.ddlStoreType.SelectedValue);
            }
            queryStrings.Add("RealName", this.txtRealName.Text);
            if (!string.IsNullOrEmpty(this.ddlCloseReason.SelectedValue))
            {
                queryStrings.Add("OrderAgent", this.ddlOrderAgent.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }
        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            this.hlinkDelete.NavigateUrl = string.Format(format,88);
            this.hlinkHistory.NavigateUrl = string.Format(format, 99);
        }

        private void btnRemark_Click(object sender, System.EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtRemark.Text))
                {
                    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                    orderInfo.OrderId = this.hidOrderId.Value;
                    if (this.orderRemarkImageForRemark.SelectedItem != null)
                    {
                        orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                    }
                    orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                    if (OrderHelper.SaveRemark(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMsg("保存备忘录成功", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败", false);
                    }
                }
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            string userName = ManagerHelper.GetCurrentManager().UserName;
            if (RefundHelper.EnsureRefund(OrderHelper.GetOrderInfo(this.hidOrderId.Value).OrderId, userName, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindOrders();
                this.ShowMsg("成功的确认了订单退款", true);
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadOrders(true);
        }

        protected void dlstOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            bool flag = false;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if (e.CommandName == "PAY" && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)//开始后台支付流程
                {
                    if (orderInfo.Gateway != "hishop.plugins.payment.advancerequest")
                    {
                        //HttpContext.Current.Response.Redirect("AliPay/default.aspx?orderId=" + orderInfo.OrderId);//(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[] { orderInfo.OrderId }));
                        ////////////////////////////////////////////请求参数////////////////////////////////////////////

                        //商户订单号，商户网站订单系统中唯一订单号，必填
                        string out_trade_no = orderInfo.OrderId;

                        //订单名称，必填
                        string subject = "代理商"+orderInfo.RealName+"的采购订单";

                        //付款金额，必填
                        string total_fee = orderInfo.GetCostPrice().ToString("F2"); //去成本价

                        //商品描述，可空
                        string body = orderInfo.ProductDescription;

                        ////////////////////////////////////////////////////////////////////////////////////////////////

                        //把请求参数打包成数组
                        SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                        AlipayConfig ac = new AlipayConfig();
                        sParaTemp.Add("service", AlipayConfig.service);
                        sParaTemp.Add("partner", AlipayConfig.partner);
                        sParaTemp.Add("seller_id", AlipayConfig.seller_id);
                        sParaTemp.Add("_input_charset", AlipayConfig.input_charset.ToLower());
                        sParaTemp.Add("payment_type", AlipayConfig.payment_type);
                        sParaTemp.Add("notify_url", AlipayConfig.notify_url);
                        sParaTemp.Add("return_url", AlipayConfig.return_url);
                        sParaTemp.Add("anti_phishing_key", AlipayConfig.anti_phishing_key);
                        sParaTemp.Add("exter_invoke_ip", AlipayConfig.exter_invoke_ip);
                        sParaTemp.Add("out_trade_no", out_trade_no);
                        sParaTemp.Add("subject", subject);
                        sParaTemp.Add("total_fee", total_fee);
                        sParaTemp.Add("body", body);
                        //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
                        //如sParaTemp.Add("参数名","参数值");

                        //建立请求
                        string sHtmlText = AlipaySubmit.BuildRequest(sParaTemp, "get", "确认");
                        Response.Write(sHtmlText);
                    
                    
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(string.Format("/user/pay.aspx?OrderId={0}", orderInfo.OrderId));
                    }
                }
                
            }
        }

        protected void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的订单", false);
            }
            else
            {
                int num = OrderHelper.DeleteOrders("'" + str.Replace(",", "','") + "'");
                this.BindOrders();
                this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
            }
        }
        /// <summary>
        /// 整理购物车中商品的规格内容,保持与前端一致
        /// </summary>
        /// <param name="skuContent">商品的原规格字符串</param>
        /// <returns>整理后的字符串</returns>ss
        private static string skuContentFormat(string skuContent)
        {
            skuContent = skuContent.Trim().TrimEnd(';');
            IList<string> skus = skuContent.Split(';');
            if (skus.Count <= 0 || skus[0] == "") return "默认";
            string result = string.Empty;
            for (int i = skus.Count - 1; i >= 0; i--)
            {
                skus[i] = skus[i].Substring(skus[i].IndexOf("：") + 1);
                result += skus[i] + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }
       

        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            string str;
            string str1;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction == false && agentPpurchase!=null)//如果区域功能没有打开,则隐藏相关功能
                agentPpurchase.Visible = false;
            this.Reurl = base.Request.Url.ToString();

            if (!this.Reurl.Contains("?"))
            {
                AgentOrders manageOrder = this;
                manageOrder.Reurl = string.Concat(manageOrder.Reurl, "?pageindex=1");
            }
            this.Reurl = Regex.Replace(this.Reurl, @"&t=(\d+)", "");
            this.Reurl = Regex.Replace(this.Reurl, @"(\?)t=(\d+)", "?");
            if ((string.IsNullOrEmpty(base.Request["isCallback"]) ? false : base.Request["isCallback"] == "true"))
            {
                if (string.IsNullOrEmpty(base.Request["ReturnsId"]))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
                StringBuilder stringBuilder = new StringBuilder();
                if (base.Request["type"] != "refund")
                {
                    num = 0;
                    str = "";
                }
                else
                {
                    RefundHelper.GetRefundType(base.Request["orderId"], out num, out str);
                }
                str1 = (num != 1 ? "银行转帐" : "退到预存款");
                stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
                if (base.Request["type"] != "replace")
                {
                    stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                    stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", str1);
                }
                stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.ShipTo);
                stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
                stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", string.Concat(orderInfo.TelPhone, " "), orderInfo.CellPhone.Trim());
                stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
                stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", str.Replace(",", ""));
                stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
                stringBuilder.AppendFormat(",\"GroupBuyId\":\"{0}\"", (orderInfo.GroupBuyId > 0 ? orderInfo.GroupBuyId : 0));
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write(string.Concat("{\"Status\":\"1\"", stringBuilder.ToString(), "}"));
                base.Response.End();

                
            }

            this.dlstOrders.ItemDataBound += new DataListItemEventHandler(this.dlstOrders_ItemDataBound);
            if (!this.Page.IsPostBack)
            {
                this.shippingModeDropDownList.DataBind();
                this.ddlIsPrinted.Items.Clear();
                this.ddlIsPrinted.Items.Add(new ListItem("全部", string.Empty));
                this.ddlIsPrinted.Items.Add(new ListItem("已打印", "1"));
                this.ddlIsPrinted.Items.Add(new ListItem("未打印", "0"));
                this.SetOrderStatusLink();
                this.bindOrderType();
                this.BindOrders();

                ViewState["StoreFiter"] = (this.specialHideShow.Value == "sswk") ? "1" : "0";
            }
            this.dlstOrders.ItemCommand += new DataListCommandEventHandler(this.dlstOrders_ItemCommand);
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        protected void btnExPortButton_Click(object sender, EventArgs e)
        {
            OrderQuery query = new OrderQuery();
            query.UserName = this.txtUserName.Text;
            query.OrderId = this.txtOrderId.Text;
            query.ProductName = this.txtProductName.Text;
            query.ShipTo = txtShopTo.Text;
            int num = 0;
            if (int.TryParse(this.lblStatus.Text, out num))
            {
                query.Status = (OrderStatus)num;
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                query.StartDate = new System.DateTime?(System.DateTime.Parse(this.calendarStartDate.SelectedDate.Value.ToString()));
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                query.EndDate = new System.DateTime?(System.DateTime.Parse(this.calendarEndDate.SelectedDate.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                int num3 = 0;
                if (int.TryParse(this.shippingModeDropDownList.SelectedValue.Value.ToString(), out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                int num2 = 0;
                if (int.TryParse(this.ddlIsPrinted.SelectedValue, out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            int num4;
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                if (int.TryParse(this.dropRegion.GetSelectedRegionId().Value.ToString(), out num4))
                {
                    query.RegionId = new int?(num4);
                }
            }
            int result = 0;
            if (int.TryParse(this.OrderFromList.SelectedValue, out result) && result > 0)
            {
                query.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)result);
            }
            DataTable dt = OrderHelper.GetOrder(query);
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            fields.Add("OrderId");
            list2.Add("订单编号");
            fields.Add("Username");
            list2.Add("昵称");
            fields.Add("ShipTo");
            list2.Add("收货人");
            fields.Add("OrderDate");
            list2.Add("提交时间");
            fields.Add("PaymentType");
            list2.Add("支付方式");
            fields.Add("IsPrinted");
            list2.Add("是否打印");
            fields.Add("OrderTotal");
            list2.Add("订单实收款");
            fields.Add("ReferralUserId");
            list2.Add("订单来源");
            fields.Add("OrderStatus");
            list2.Add("订单状态");
            fields.Add("ShipOrderNumber");
            list2.Add("物流单号");

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (System.Data.DataRow row in dt.Rows)
            {
                foreach (string str2 in fields)
                {
                    if (str2 == "IsPrinted")
                    {
                        builder.Append(row[str2] != DBNull.Value && (bool)row[str2] ? "已打印" : "未打印").Append(",");
                    }
                    else if (str2 == "ReferralUserId")
                    {
                        builder.Append(row[str2].ToString() == "" ? "订单来源：主站" : row[str2].ToString() == "0" ? "订单来源：主站" : "订单来源：" + row["StoreName"]).Append(",");
                    }
                    else if (str2 == "OrderStatus")
                    {
                        builder.Append((OrderInfo.GetOrderStatusName((OrderStatus)row["OrderStatus"])).Replace(",", "，")).Append(",");
                    }
                    else
                    {
                        builder.Append(row[str2]).Append(",");
                    }
                    if (str2 == fields[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }
    }
}