using System;
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
using System.Linq;
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
using Hidistro.Entities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Config;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Comments;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Api;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public class ManageOrder : AdminPage
    {
        protected string Reurl = string.Empty;

        protected HyperLink hlinkAllOrder;

        protected HyperLink hlinkNotPay;

        protected HyperLink hlinkYetPay;

        protected HyperLink hlinkSendGoods;

        protected HyperLink hlinkTradeFinished;

        protected HyperLink hlinkClose;

        protected HyperLink hlinkHistory;

        protected WebCalendar calendarStartDate;

        protected WebCalendar calendarEndDate;

        protected HyperLink hlinkDelete;

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

        protected ImageLinkButton lkbtnRemove;

        protected HtmlControl spDelete;

        protected HtmlControl spRemove;

        protected HtmlControl anchorsDelete;

        protected ImageLinkButton BackButton;

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

        protected TextBox txtChanneName;

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

        protected HtmlInputHidden BrandShow;

        protected HtmlInputHidden send;

        protected HtmlInputHidden modeName;

        protected HtmlInputHidden Back;

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

        protected System.Web.UI.WebControls.DropDownList DDLservice;

        protected System.Web.UI.WebControls.DropDownList DDLPayType;

        protected Button btnPack;
        protected Button btnUnPack;
   
        protected FileUpload fileOrderInfoPack;
        protected DropDownList dropExpress;
        protected Literal litContent;
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

        private void btnCloseOrder_Click(object sender, System.EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(orderInfo))
            {
                orderInfo.OnClosed();
                this.BindOrders();
                Messenger.OrderClosed(MemberHelper.GetMember(orderInfo.UserId), orderInfo, orderInfo.CloseReason);
                this.ShowMsg("关闭订单成功", true);
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        private void btnOrderGoods_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                string[] array = str.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Length; i++)
                {
                    string str2 = array[i];
                    list.Add("'" + str2 + "'");
                }
                System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>订单单号</td>");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("<td>备注</td>");
                builder.AppendLine("</tr>");
                foreach (System.Data.DataRow row in orderGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"] + "</td>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("<td>" + row["Remark"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }
        private void btnProductGoods_Click(object sender, System.EventArgs e)
        {
            
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                string[] array = str.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Length; i++)
                {
                    string str2 = array[i];
                    list.Add("'" + str2 + "'");
                }
                System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("</tr>");
                foreach (System.Data.DataRow row in productGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void dlstOrders_ItemDataBound(object obj, DataListItemEventArgs dataListItemEventArg)
        {
            bool flag;
            if (dataListItemEventArg.Item.ItemType == ListItemType.Item || dataListItemEventArg.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderStatus orderStatu = (OrderStatus)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "OrderStatus");
                string str = "";
                if (!(DataBinder.Eval(dataListItemEventArg.Item.DataItem, "Gateway") is DBNull))
                {
                    str = (string)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "Gateway");
                }
                int num = (DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyId")) != DBNull.Value ? (int)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyId") : 0;
                HyperLink hyperLink = (HyperLink)dataListItemEventArg.Item.FindControl("lkbtnEditPrice");
                Label label = (Label)dataListItemEventArg.Item.FindControl("lkbtnSendGoods");
                Label labelqw = (Label)dataListItemEventArg.Item.FindControl("lbJoinOrder");
                //如果是自提方式，则由门店发货
                DataRowView drv = (DataRowView)dataListItemEventArg.Item.DataItem;
                string strModeName = (drv["ModeName"] != DBNull.Value) ? drv["ModeName"].ToString() : "";
                string strRealModeName = (drv.Row.Table.Columns.Contains("RealModeName") && drv["RealModeName"] != DBNull.Value) ? drv["RealModeName"].ToString() : "";
                if (strRealModeName.IndexOf("自提") > -1 || (strRealModeName == "" && strModeName.IndexOf("自提") > -1))
                {
                    if (drv["ReferralUserId"].ToString() != "0")
                    {
                        label.Enabled = false;
                        label.ToolTip = "该订单为自提方式，由门店发货！";
                        label.Text = "<a style='color:Red;display:block;' href='javascript:ShowMsg(\"该订单为自提方式，由门店发货\",true)'>发货</a>";
                    
                    }
                }

                ImageLinkButton imageLinkButton = (ImageLinkButton)dataListItemEventArg.Item.FindControl("lkbtnPayOrder");
                ImageLinkButton imageLinkButton1 = (ImageLinkButton)dataListItemEventArg.Item.FindControl("lkbtnConfirmOrder");
                Literal literal = (Literal)dataListItemEventArg.Item.FindControl("litCloseOrder");
                HtmlAnchor htmlAnchor = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckRefund");
                HtmlAnchor htmlAnchor1 = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckReturn");
                HtmlAnchor htmlAnchor2 = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckReplace");
                HtmlControl anchorsDelete = (HtmlControl)dataListItemEventArg.Item.FindControl("anchorsDelete");

                if (orderStatu == OrderStatus.WaitBuyerPay)
                {
                    hyperLink.Visible = true;
                    literal.Visible = true;
                    if (str != "hishop.plugins.payment.podrequest")
                    {
                        imageLinkButton.Visible = true;
                    }
                }
                if (orderStatu == OrderStatus.ApplyForRefund)
                {
                    htmlAnchor.Visible = true;
                }
                if (orderStatu == OrderStatus.ApplyForReturns)
                {
                    htmlAnchor1.Visible = true;
                }
                if (orderStatu == OrderStatus.ApplyForReplacement)
                {
                    htmlAnchor2.Visible = true;
                }
                if (num <= 0)
                {
                    Label label1 = label;
                    Label label2 = labelqw;
                    if (orderStatu == OrderStatus.BuyerAlreadyPaid)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = (orderStatu != OrderStatus.WaitBuyerPay ? false : str == "hishop.plugins.payment.podrequest");
                    }
                    label1.Visible = flag;
                    if (CustomConfigHelper.Instance.IsProLa)
                    {
                        label2.Visible = flag;
                    }
                }
                else
                {
                    GroupBuyStatus groupBuyStatu = (GroupBuyStatus)DataBinder.Eval(dataListItemEventArg.Item.DataItem, "GroupBuyStatus");
                    label.Visible = (orderStatu != OrderStatus.BuyerAlreadyPaid ? false : groupBuyStatu == GroupBuyStatus.Success);
                }
                imageLinkButton1.Visible = orderStatu == OrderStatus.SellerAlreadySent;
            }
        }

        private void btnSendGoods_Click(object sender, System.EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + str));
            }
        }
        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            string roleType = ManagerHelper.GetRole(ManagerHelper.GetCurrentManager().RoleId).RoleName;
            if (roleType == "门店发货" || roleType == "活动账号")
            {
                orderQuery.Sender = ManagerHelper.GetCurrentManager().UserId.ToString();
                orderQuery.ClientUserId = ManagerHelper.GetCurrentManager().ClientUserId;
                anchorsDelete.Visible = false;
                
            }
            
            if(orderQuery.Status==OrderStatus.Delete)
            {
                spDelete.Style["display"] = "";
                spRemove.Visible = false;
            }
            else if (roleType == "超级管理员" && orderQuery.Sender.ToInt() > 0)//如果是超级管理员正在进行条件过滤查询,则可通过当前查询的senderid查找到clientuserid
            {
                orderQuery.modeName = ManagerHelper.getPcOrderStorenameBySender(orderQuery.Sender.ToInt());
            }
            DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            foreach (DataRow row in ((DataTable)orders.Data).Rows)
            {
                if (row["UserName"].ToString() == "[堂食用户]" || row["UserName"].ToString() == "[匿名用户]")
                {
                    //row["ModeName"] = "店内";
                    if (row["sender"] != DBNull.Value)
                    {
                        row["ModeName"] = ManagerHelper.getPcOrderStorenameBySender(Convert.ToInt32(row["sender"]));
                    }
                    
                    row["Address"] = "店内";
                }
            }
            this.dlstOrders.DataSource = orders.Data;
            this.dlstOrders.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;
            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;
            this.dropExpress.SelectedValue = orderQuery.ExpressCompanyName;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtChanneName.Text = orderQuery.ChannelName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.lblStatus.Text = orderQuery.Status.ToString();
            this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
            if (orderQuery.ClientUserId != 0)
            {
                this.DDLservice.SelectedValue = orderQuery.ClientUserId.ToString();
            }
            if (this.DDLservice.SelectedValue != "未选择")
            {              
                send.Value = orderQuery.ClientUserId.ToString();
                modeName.Value = ManagerHelper.getPcOrderStorenameByClientuserid(this.DDLservice.SelectedValue.ToInt());
            }
            else {
                send.Value ="1";
            }
            if (!string.IsNullOrEmpty(orderQuery.PayTypeName))
            {
                this.DDLPayType.SelectedValue = orderQuery.PayTypeName;
            }
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
            this.txtRealName.Text = orderQuery.RealName;
            this.ddlStoreType.SelectedValue = orderQuery.StoreType;
        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["payTypeName"]))
            {
                query.PayTypeName = Globals.UrlDecode(this.Page.Request.QueryString["payTypeName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ChannelName"]))
            {
                query.ChannelName = Globals.UrlDecode(this.Page.Request.QueryString["ChannelName"]);
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
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ExpressCompanyName"]))
            {
                query.ExpressCompanyName = Globals.UrlDecode(this.Page.Request.QueryString["ExpressCompanyName"]);
            }
            //爽爽挝啡的特殊功能过滤
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)//如果门店配送功能开启,载入页面时根据门店的子账号进行订单过滤
            {
                //门店配送添加子账号的过滤
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                DistributorsInfo shipDistributor = DistributorsBrower.GetDistributorInfo(currentManager.ClientUserId);
                RoleInfo roleinfo = ManagerHelper.GetRole(currentManager.RoleId);
                if (shipDistributor != null)
                {
                    query.modeName = shipDistributor.StoreName;
                }
                this.specialHideShow.Value = "sswk";//爽爽挝啡
                //活动账号的过滤
                if (roleinfo.RoleName == "活动账号")
                {
                    query.UserName = "[活动用户]";
                }

                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["senderid"]) && roleinfo.RoleName == "超级管理员")
                {
                    int clientuserid = Globals.UrlDecode(this.Page.Request.QueryString["senderid"]).ToInt();
                    query.ClientUserId = clientuserid;
                    query.Sender = ManagerHelper.getSenderIdByClientUserId(clientuserid).ToString();
                }
            }
            if (CustomConfigHelper.Instance.BrandShow)
            {
                this.specialHideShow.Value = "jzt";//九州通(考拉萌购)
            }
            //玖信健佳的代理商订单过滤
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction)
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                RoleInfo roleinfo = ManagerHelper.GetRole(currentManager.RoleId);
                if (roleinfo.RoleName == "代理商")
                {
                    query.AgentUserId = currentManager.UserId;
                }
            }
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
            queryStrings.Add("ChannelName", this.txtChanneName.Text);
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
            if (!string.IsNullOrEmpty(this.dropExpress.SelectedValue))
            {
                queryStrings.Add("ExpressCompanyName", this.dropExpress.SelectedValue);
            }
            queryStrings.Add("RealName", this.txtRealName.Text);       
            if (!string.IsNullOrEmpty(this.ddlCloseReason.SelectedValue))
            {
                queryStrings.Add("OrderAgent", this.ddlOrderAgent.SelectedValue);
            }
            if (this.DDLservice.SelectedValue != "未选择")//对门店的过滤
            {
                queryStrings.Add("senderId", this.DDLservice.SelectedValue);
            }
            if (this.DDLPayType.SelectedValue != "未选择")//对支付方式的过滤
            {
                queryStrings.Add("payTypeName", this.DDLPayType.SelectedValue);
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

        protected void BackButton_Click(object sender, System.EventArgs e)
        {
            string orderids = Back.Value;
            string orderidss = "'" + orderids.TrimEnd(',').Replace(",", "','") + "'";
            List<OrderInfo> curry = OrderHelper.GetOrderInfoList(orderids);
            DataTable dtsku = new DataTable();
            dtsku.Columns.Add("SkuId");
            dtsku.Columns.Add("Quantity");
            dtsku.Columns.Add("PaymentType");
            dtsku.Columns.Add("OrderStatus");
            foreach (OrderInfo info in curry)
            {
                foreach (LineItemInfo fo in info.LineItems.Values)
                {
                    dtsku.Rows.Add(fo.SkuId, fo.Quantity, info.PaymentType, info.OrderStatus);
                }
            }         
            int OrderTotal = 0;
            int OrderPoint = 0;
            int OrderCostPrice = 0;
            int Amount = 0;
            int OrderProfit = 0;
            if (OrderHelper.UpdateOrderInfoList(orderidss, OrderTotal, OrderPoint, OrderCostPrice, OrderProfit, Amount) && OrderHelper.UpdateOrderItemsQuantity(orderidss))
            {
                for (int i = 0; i < dtsku.Rows.Count; i++)
                {
                    int quantity = Convert.ToInt32(dtsku.Rows[i]["Quantity"]);
                    string skuid = dtsku.Rows[i]["SkuId"].ToString();
                    if (dtsku.Rows[i]["PaymentType"].ToString() == "微信支付" )
                    {                                        
                       if (dtsku.Rows[i]["OrderStatus"].ToString() == "BuyerAlreadyPaid" || dtsku.Rows[i]["OrderStatus"].ToString() == "SellerAlreadySent" || dtsku.Rows[i]["OrderStatus"].ToString() == "Finished")
                        {
                            if (OrderHelper.UpdateQuantity(quantity, skuid))
                            {
                                this.ShowMsg("退货成功", true);
                            }
                            else
                            {
                                this.ShowMsg("退货失败", false);
                            }
                        }
                        else {
                                this.ShowMsg("退货成功", true);
                        }
                    }
                    if (dtsku.Rows[i]["PaymentType"].ToString() == "货到付款")
                    {
                        if (dtsku.Rows[i]["OrderStatus"].ToString() == "SellerAlreadySent" || dtsku.Rows[i]["OrderStatus"].ToString() == "Finished")
                        {
                            if (OrderHelper.UpdateQuantity(quantity, skuid))
                            {
                                this.ShowMsg("退货成功", true);
                            }
                            else
                            {
                                this.ShowMsg("退货失败", false);
                            }
                        }
                        else
                        {
                            this.ShowMsg("退货成功", true);
                        }
                    }
                }
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
                if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    int groupBuyId = orderInfo.GroupBuyId;
                    if (!OrderHelper.ConfirmPay(orderInfo))
                    {
                        this.ShowMsg("确认订单收款失败", false);
                        return;
                    }
                    DebitNoteInfo debitNoteInfo = new DebitNoteInfo();

                    debitNoteInfo.NoteId = Globals.GetGenerateId();
                    debitNoteInfo.OrderId = e.CommandArgument.ToString();
                    debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
                    debitNoteInfo.Remark = string.Concat("后台", debitNoteInfo.Operator, "收款成功");

                    OrderHelper.SaveDebitNote(debitNoteInfo);
                    if (orderInfo.GroupBuyId > 0)
                    {
                        PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                    }
                    this.BindOrders();
                    orderInfo.OnPayment();
                    this.ShowMsg("成功的确认了订单收款", true);
                    return;
                }
                if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
                    LineItemInfo lineItemInfo = new LineItemInfo();
                    foreach (KeyValuePair<string, LineItemInfo> lineItem in lineItems)
                    {
                        lineItemInfo = lineItem.Value;
                        if (lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForRefund && lineItemInfo.OrderItemsStatus != OrderStatus.ApplyForReturns)
                        {
                            continue;
                        }
                        flag = true;
                    }
                    if (!flag)
                    {
                        if (!OrderHelper.ConfirmOrderFinish(orderInfo))
                        {
                            this.ShowMsg("完成订单失败", false);
                            return;
                        }
                        this.BindOrders();
                        DistributorsBrower.UpdateCalculationCommission(orderInfo);
                        foreach (LineItemInfo value in orderInfo.LineItems.Values)
                        {
                            if (value.OrderItemsStatus.ToString() != OrderStatus.SellerAlreadySent.ToString())
                            {
                                continue;
                            }
                            RefundHelper.UpdateOrderGoodStatu(orderInfo.OrderId, value.SkuId, 5);
                        }
                        //遍历订单车中包含[会员充值]的商品,生成等价的优惠券给用户
                        if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.CouponRecharge)
                        {
                            //找到所有会员充值的商品
                            List<LineItemInfo> couponGoodList = lineItems.Values.Where(n => Hidistro.ControlPanel.Commodities.ProductHelper.GetProductBaseInfo(n.ProductId).ProductName == "会员充值").ToList();
                            //创建后台couponInfo
                            foreach (LineItemInfo info in couponGoodList)
                            {
                                for (int i = 0; i < info.Quantity; i++)
                                {
                                    CouponInfo target = new CouponInfo
                                    {
                                        Name = "会员充值" + info.ItemAdjustedPrice.ToString("F2") + "元",
                                        ClosingTime = DateTime.MaxValue,
                                        StartTime = DateTime.Now,
                                        Amount = 0M,
                                        DiscountValue = info.ItemAdjustedPrice,
                                        NeedPoint = 0
                                    };
                                    string lotNumber = string.Empty;

                                    int resultCouponId = CouponHelper.CreateCoupon(target, 0);
                                    IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
                                    if (resultCouponId > 0) //如果后台新建成功或者已经存在,开始对用户进行发送
                                    {
                                        CouponItemInfo item = new CouponItemInfo();
                                        string claimCode = string.Empty;
                                        claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                                        item = new CouponItemInfo(resultCouponId, claimCode, new int?(orderInfo.UserId), orderInfo.Username, orderInfo.EmailAddress, System.DateTime.Now);
                                        listCouponItem.Add(item);
                                    }
                                    CouponHelper.SendClaimCodes(resultCouponId, listCouponItem);
                                }
                            }
                        }
                        this.ShowMsg("成功的完成了该订单", true);
                        return;
                    }
                    this.ShowMsg("订单中商品有退货(款)不允许完成!", false);

                }
            }
        }
        //订单删除
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
        //订单回收
        protected void lkbtnRemove_Click(object sender, System.EventArgs e)
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
                int num = OrderHelper.removeOrders("'" + str.Replace(",", "','") + "'");
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
        /// <summary>
        /// 打印订单信息(sswf)
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string PrintOrderInfo(string OrderIds)
        {
            string backJson = string.Empty;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                
                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };

                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    int distributorId = DistributorsBrower.GetSenderDistributorId(order.Sender);
                    DistributorsInfo currentSenderDistributor = DistributorsBrower.GetCurrentDistributors(distributorId);
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                    builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/login_logo2.png' /><h3>SALES MEMO</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>顾客名称： </span>{3}</div><div style='text-align:left;padding-bottom:5px;'><span >联系电话： </span>{5}</div><div style='text-align:left;padding-bottom:5px;'><span>联系地址： </span>{4}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId, order.Username, order.Address, order.CellPhone);
                    builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:50%;background:#fff;border:none;text-align:left;'>菜名</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", info2.GetSubShowTotal().ToString("F2"));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    if (reducedPromotionAmount1 > 0m)
                    {
                        builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2));
                    }
                    decimal payCharge1 = order.PayCharge;
                    if (payCharge1 > 0m)
                    {
                        builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                    }
                    if (!string.IsNullOrEmpty(order.CouponCode))
                    {
                        decimal couponValue = order.CouponValue;
                        if (couponValue > 0m)
                        {
                            builder.AppendFormat("<div><span>优惠抵扣： </span>-￥{0}</div>", System.Math.Round(couponValue, 2));
                        }
                    }
                    
                    decimal giveBuyPrice = 0m;
                    decimal halfBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {
                        //计算买一送一减免
                        if (giveBuyPrice > 0m)
                        {
                            builder.AppendFormat("<div style='font-size:12px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                        }
                        //第二杯半价
                        if (halfBuyPrice > 0m)
                        {
                            builder.AppendFormat("<div style='font-size:12px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    if (halfBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));
                    //插入二维码start-----------
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string savepath = System.Web.HttpContext.Current.Server.MapPath("~/Storage/TicketImage") + "\\" + string.Format("distributor_{0}", currentSenderDistributor) + ".jpg";
                    //if (!File.Exists(savepath))
                    //{
                    //    Hishop.Weixin.MP.Api.TicketAPI.GetTicketImage(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, string.Format("distributor_{0}", currentSenderDistributor), false);
                    //}
                    string qrCodeBackImgUrl = "/Storage/TicketImage/" + string.Format("distributor_{0}", currentSenderDistributor) + ".jpg";
                    builder.AppendFormat("<div style='text-align:center;width:100%;margin-top:10px;'><img src='{0}' style='width:30%' /></div>", qrCodeBackImgUrl);
                    //插入二维码end------------

                    //谢谢光临
                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:12px;'>全国加盟热线：400-043-0311</div>");

                    builder.Append("</div>");
                }
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                //return "success,str=123";
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
        }
        /// <summary>
        /// 三作咖啡发货打印订单
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string StampOrderInfo(string OrderIds)
        {
            string backJson = string.Empty;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                    builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/三作咖啡.png' /><h3>SanZuo Coffee</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>顾客名称： </span>{3}</div><div style='text-align:left;padding-bottom:5px;'><span >联系电话： </span>{5}</div><div style='text-align:left;padding-bottom:5px;'><span>联系地址： </span>{4}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId, order.Username, order.Address, order.CellPhone);
                    builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:50%;background:#fff;border:none;text-align:left;'>菜名</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", info2.GetSubShowTotal().ToString("F2"));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    
                    decimal giveBuyPrice = 0m;
                    decimal halfBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {   //计算买一送一减免
                        if (itemInfo.GiveQuantity > 0)
                        {
                            giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                        }
                        //第二杯半价
                        if (halfBuyPrice > 0m)
                        {
                            builder.AppendFormat("<div style='font-size:12px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    if (halfBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));

                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>三作咖啡外卖送货单</div>");
                    builder.Append("</div>");
                }
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
        }
        /// <summary>
        /// 爽爽挝啡发货单
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string JoinOrder(string OrderIds)
        {
            string backJson = string.Empty;
            try
            {
                IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组
                System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);

                    //首先推送消息,
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    TemplateMessage templateMessage = new TemplateMessage();
                    MemberInfo member = MemberHelper.GetMember(order.UserId);
                    templateMessage.TemplateId = "ABT7eR5E4Td4mqEJ9Xe_onkXSXJUEXomZPqRgdayJnc";//Globals.GetMasterSettings(true).WX_Template_01;// "b1_ARggaBzbc5owqmwrZ15QPj9Ksfs0p5i64C6MzXKw";//消息模板ID
                    templateMessage.Touser = member.OpenId;//用户OPENID
                    string productsDes = ""; int c = 0;
                    foreach (LineItemInfo info in order.LineItems.Values)
                    {
                        if (c > 3) break;
                        productsDes += info.ItemDescription + ",";
                        c++;
                    }
                    productsDes = productsDes.TrimEnd(',') + "等菜品";

                    TemplateMessage.MessagePart[] messateParts = new TemplateMessage.MessagePart[]{
                                                new TemplateMessage.MessagePart{Name = "first",Value = "亲，正在制作您的菜品！"},
                                                new TemplateMessage.MessagePart{Name = "keyword1",Value =productsDes},
                                                new TemplateMessage.MessagePart{Name = "keyword2",Value =DateTime.Now.ToShortTimeString()},
                                                new TemplateMessage.MessagePart{Name = "remark",Value = "我们已成功接单，大厨正在精心制作，请耐心等待并留意派送通知！"/*orderInfo.ShipToDate*/}};
                    templateMessage.Data = messateParts;
                    TemplateApi.SendMessage(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), templateMessage);


                    //打印小票
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                    builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><h3>Pro辣</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>顾客名称： </span>{3}</div><div style='text-align:left;padding-bottom:5px;'><span >联系电话： </span>{5}</div><div style='text-align:left;padding-bottom:5px;'><span>联系地址： </span>{4}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", order.OrderDate.ToString("yyyy-MM-dd HH:mm"), order.OrderId, order.Username, order.Address, order.CellPhone);
                    builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:40%;background:#fff;border:none;text-align:left;'>菜名</th><th style='width:20%;background:#fff;border:none'>规格</th><th style='width:20%;background:#fff;border:none'>数量</th><th style='width:20%;background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                    System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                    if (lineItems1 != null)
                    {
                        foreach (string str2 in lineItems1.Keys)
                        {
                            LineItemInfo info2 = lineItems1[str2];
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", skuContentFormat(info2.SKUContent));
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", info2.GetSubShowTotal().ToString("F2"));
                        }
                    }
                    builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    
                    decimal giveBuyPrice = 0m;
                    decimal halfBuyPrice = 0m;
                    foreach (LineItemInfo itemInfo in order.LineItems.Values)
                    {
                        //计算买一送一减免
                        if (itemInfo.GiveQuantity > 0)
                        {
                            giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                        }
                        //第二杯半价
                        if (halfBuyPrice > 0m)
                        {
                            builder.AppendFormat("<div style='font-size:12px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                        }
                    }
                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    if (halfBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>第二杯半价： </span>-￥{0}</div>", System.Math.Round(halfBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));

                    builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>Pro辣接货单</div>");
                    builder.Append("</div>");
                }
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
        }
        /// <summary>
        /// 导出订单
        /// </summary>
        public void packOrderInfos(object sender, EventArgs e)
        {
            string backJson = string.Empty;
            OrderQuery orderQuery = this.GetOrderQuery();
            string path = OrderHelper.PackOrderInfos(orderQuery.StartDate.ToString(), orderQuery.EndDate.ToString());
            Hidistro.Entities.FilePlus.DownFile(this.Page, path);
        }
        /// <summary>
        /// 导入订单
        /// </summary>
        public void UnPackOrderInfos(object sender, EventArgs e)
        {
            //反序列化订单文件并整表提交
            if (!string.IsNullOrEmpty(fileOrderInfoPack.FileName) && fileOrderInfoPack.FileContent.Length > 0)
            {
                string filePath = MapPath("/Storage/temp/") + "unpack_"+fileOrderInfoPack.FileName;
                fileOrderInfoPack.SaveAs(filePath);
                DataSet dsOrdersInfo = ObjectSerializerHelper.DataSetDeserialize(filePath);
                //拼接主键id,用于得出ds差别
                string orderIds = string.Empty;
                foreach (DataRow row in dsOrdersInfo.Tables[0].Rows)
                {
                    orderIds = orderIds + "'"+ row["OrderId"]+"'" + ",";
                }
                orderIds = orderIds.TrimEnd(',');
                string selectSql = "select * from Hishop_Orders where OrderId in (" + orderIds + ")" + ";" + "select * from Hishop_OrderItems where OrderId in (" + orderIds + ")";
                DataSet dsOrdersInfoCurrent = DataBaseHelper.GetDataSet(selectSql);
                dsOrdersInfo.Tables[0].PrimaryKey = new DataColumn[] { dsOrdersInfo.Tables[0].Columns["OrderId"] };
                dsOrdersInfo.Tables[1].PrimaryKey = new DataColumn[] { dsOrdersInfo.Tables[1].Columns["OrderId"] };
                dsOrdersInfoCurrent.Tables[0].PrimaryKey = new DataColumn[] { dsOrdersInfoCurrent.Tables[0].Columns["OrderId"] };
                dsOrdersInfoCurrent.Tables[1].PrimaryKey = new DataColumn[] { dsOrdersInfoCurrent.Tables[1].Columns["OrderId"] };


                dsOrdersInfoCurrent = DataBaseHelper.GetDsDifferent(dsOrdersInfoCurrent, dsOrdersInfo, false);

                string[] selectSqls = selectSql.Split(';');
                

                if (dsOrdersInfo.Tables.Count > 0)
                {
                    int count = DataBaseHelper.CommitDataSet(dsOrdersInfoCurrent, selectSqls);
                    if (count > 0)
                    {
                        this.ShowMsg("导入成功!", true);
                    }
                }
            }
            else
            {
                this.ShowMsg("请上传数据文件", false);
            }

        }
        /// <summary>
        /// 打印日结单(sswf)
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string todayOrderPrint(string startDate, string endDate, string send, string modeName)
        {
            int userid = 0;
            int couponCount = 0;

            decimal couponTotalPrice = 0m;//优惠券总价
            int giveCount = 0;
            int halfCount=0;
            decimal halfPrice =0m;//第二杯半价总价
            decimal givePrice = 0m;//买一送一总价
            int orderCount = 0;//订单总数
            int givequantity = 0;//商品总数
            decimal orderTotalPrice = 0m;//订单总价
            int pcOrderCount = 0;//店内订单总数
            decimal pcOrderTotalPrice = 0m;//店内订单总价
            int mobileOrderCount = 0;//移动端订单总数
            int microPayOrderCount = 0;//店内微信扫码支付总数
            decimal microPayOrderTotalPrice = 0m;//店内微信扫码支付总价
            decimal mobileOrderTotalPrice = 0m;//移动端订单总价
            decimal totalPriceGot = 0m;//实收总价

            string backJson = string.Empty;
            try
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                int senderId = 0;
                string storeName = "";
                if (ManagerHelper.GetRole(currentManager.RoleId).RoleName != "超级管理员") //如果当前后台账号不是超级管理员,则日结单根据当前账号id结合sender进行过滤
                {
                    senderId = currentManager.UserId;
                    storeName = DistributorsBrower.GetUserIdDistributors(DistributorsBrower.GetSenderDistributorId(senderId.ToString())).StoreName;
                }
                userid = ManagerHelper.getSenderIdByClientUserId(Convert.ToInt32(send));
                IList<string> orderIds = OrderHelper.GetTodayOrders(startDate, endDate, senderId, storeName, userid, modeName); //将今天的orderid拆分成数组
                string rrr = "";
                foreach (string a in orderIds)
                {
                    rrr += a + ",";
                }
                DataTable dtProducts = OrderHelper.GetTodayProducts(startDate, endDate, senderId, storeName, userid, modeName);//将今天的所有卖出的商品存在DataTable里,下面循环会往里面填值
                DataTable dtGifts = OrderHelper.GetTodayGifts(startDate, endDate, senderId, storeName, userid, modeName);//将今天的所有卖出的礼品存在DataTable里,下面循环会往里面填值
                dtGifts.PrimaryKey = new DataColumn[] { dtGifts.Columns["Giftid"] };
                dtProducts.PrimaryKey = new DataColumn[] { dtProducts.Columns["ProductId"] };


                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                //头部,开始时间,结束时间,制单时间
                builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><h3>门店统计日结报表</h3><div style='text-align:left;padding-bottom:5px;'><span>开始时间： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>结束时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>制单时间： </span>{2}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>",startDate.ToString(),endDate.ToString(),DateTime.Now.ToString());
                //列表table
                builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:65%;background:#fff;border:none;text-align:left;'>项目</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    int SendOut = 0;
                    if (order.ManagerRemark!= null)
                    {
                         SendOut = order.ManagerRemark.IndexOf("退单"); 
                    }
                    if (order.ManagerRemark == null ||SendOut<0)
                    {
                        System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                        IList<OrderGiftInfo> giftItems = order.Gifts;
                        //统计优惠券总价
                        if (!string.IsNullOrEmpty(order.CouponCode))
                        {
                            couponCount++;
                            couponTotalPrice += System.Math.Round(order.CouponValue);
                        }
                        //统计堂食总价和数量
                        if ((order.Username == "[堂食用户]" || order.Username == "[匿名用户]" || order.Username == "[活动用户]") && order.RealModeName != "微信扫码支付")
                        {
                            pcOrderCount++;
                            pcOrderTotalPrice += order.GetTotal();//实际消费(减去了买一送一和优惠券的金额)
                            totalPriceGot += order.GetTotal();
                        }
                        //统计移动端的总价和数量
                        if (order.Username != "[堂食用户]" && order.Username != "[匿名用户]" && order.Username != "[活动用户]")
                        {
                            mobileOrderCount++;
                            mobileOrderTotalPrice += order.GetTotal();
                        }
                        //统计店内微信扫码支付总价和数量
                        if ((order.Username == "[堂食用户]" || order.Username == "[匿名用户]" || order.Username == "[活动用户]") && order.RealModeName == "微信扫码支付")
                        {
                            microPayOrderCount++;
                            microPayOrderTotalPrice += order.GetTotal();
                        }
                        //订单数量和总价统计
                        orderCount++;
                        orderTotalPrice += order.GetShowAmount();

                        if (lineItems1 != null)
                        {
                            foreach (string str2 in lineItems1.Keys)
                            {
                                LineItemInfo info2 = lineItems1[str2];
                                //统计商品总数量
                                givequantity += info2.Quantity;
                                //统计买一送一赠送总价
                                giveCount += info2.GiveQuantity;
                                if (info2.GiveQuantity > 0)
                                {
                                    givePrice += (info2.GetSubShowTotal() - info2.GetSubTotal());//计算出买一送一的价格
                                }
                                //统计第二杯半价数量
                                halfCount +=info2 .HalfPriceQuantity;
                                if (info2.HalfPriceQuantity > 0)
                                {
                                    halfPrice += (info2.GetSubShowTotal() - info2.GetSubTotal());//计算出第二杯半价的价格
                                }
                               
                                //查找商品表,根据productid来刷新相应的商品的数量和金额
                                DataRow drProduct = dtProducts.Rows.Find(info2.ProductId);
                                if (drProduct != null)
                                {
                                    drProduct["quantity"] = Convert.ToInt32(drProduct["quantity"]) + info2.Quantity;
                                    drProduct["Price"] = Convert.ToDecimal(drProduct["Price"]) + info2.GetSubShowTotal();
                                }

                            }
                        }

                        if (giftItems.Count > 0)
                        {
                            foreach (OrderGiftInfo giftInfo in giftItems)
                            {
                                //查找商品表,根据productid来刷新相应的商品的数量和金额
                                DataRow drGifts = dtGifts.Rows.Find(giftInfo.GiftId);
                                if (drGifts != null)
                                {
                                    drGifts["quantity"] = Convert.ToInt32(drGifts["quantity"]) + giftInfo.Quantity;
                                    drGifts["costpoint"] = Convert.ToInt32(drGifts["costpoint"]) + giftInfo.costPoint;
                                }
                            }
                        }

                        /*
                        if (giftItems.Count > 0)
                        {
                            foreach (OrderGiftInfo giftInfo in giftItems)
                            {
                                builder.AppendFormat("<td>{0}</td>", giftInfo.GiftName + "(礼品)");
                                builder.AppendFormat("<td style='text-align:center;'>{0}</td>", giftInfo.Quantity);
                                builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", giftInfo.costPoint.ToString() + "积分");
                            }
                        }
                         */
                    }
                }

                foreach (DataRow row in dtGifts.Rows)
                {
                    builder.AppendFormat("<td>{0}</td>", row["giftname"].ToString() + "(礼品)");
                    builder.AppendFormat("<td style='text-align:center;'>{0}</td>", row["quantity"].ToString());
                    builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", Convert.ToInt32(row["costpoint"]).ToString() + "积分");
                }
                

                dtProducts.PrimaryKey = new DataColumn[] { dtProducts.Columns["ProductId"] };

                foreach (DataRow row in dtProducts.Rows)
                {
                    builder.AppendFormat("<td>{0}</td>", row["ProductName"].ToString());
                    builder.AppendFormat("<td style='text-align:center;'>{0}</td>", row["quantity"].ToString());
                    builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>",Convert.ToDecimal(row["price"]).ToString("F2"));
                }

                builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");
                //底部
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>订单总数：</span>{0}</div>", orderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>杯杯总计：</span>{0}</div>", givequantity);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>实收金额：</span>{0}</div>", Convert.ToDouble(totalPriceGot));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>消费金额：</span>{0}</div>", Convert.ToDouble(orderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>微信订单：</span>{0}</div>", mobileOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>微信消费：</span>{0}</div>", Convert.ToDouble(mobileOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>店内订单：</span>{0}</div>", pcOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>店内消费：</span>{0}</div>", Convert.ToDouble(pcOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>扫码支付订单：</span>{0}</div>", microPayOrderCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>扫码支付消费：</span>{0}</div>", Convert.ToDouble(microPayOrderTotalPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>买送数量：</span>{0}</div>", giveCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>买送减免：</span>{0}</div>", Convert.ToDouble(givePrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>半价数量：</span>{0}</div>",halfCount );
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>半价减免：</span>{0}</div>", Convert.ToDouble(halfPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>优惠券数量：</span>{0}</div>", couponCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>优惠券减免：</span>{0}</div>", Convert.ToDouble(couponTotalPrice));
                
                

                //builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                builder.Append("</div>");

                /*
                    
                    Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(order.ReferralUserId);
                    decimal reducedPromotionAmount1 = order.ReducedPromotionAmount;
                    if (reducedPromotionAmount1 > 0m)
                    {
                        builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2));
                    }
                    decimal payCharge1 = order.PayCharge;
                    if (payCharge1 > 0m)
                    {
                        builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                    }

                    if (giveBuyPrice > 0m)
                    {
                        builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>-￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                    }
                    //应收
                    builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(order.GetAmount() - order.CouponValue, 2));


                
                backJson = string.Format("\"success\":true,\"inHtml\":\"{0}\"", builder);
                //return "success,str=123";
                return builder.ToString();               */
                return builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
                return "{" + backJson + "}";
            }
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            string str;
            string str1;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction == false)//如果区域功能没有打开,则隐藏相关功能
                agentPpurchase.Visible = false;
            this.Reurl = base.Request.Url.ToString();
            
            //门店的下拉框
            List<CategoryQuery> clientuser = new List<CategoryQuery>();
            clientuser = CouponHelper.Getaspnet_ManagersClientUserId();
            DDLservice.Items.Add("未选择");
            foreach (CategoryQuery ct in clientuser)
            {
                ListItem item = new ListItem();
                item.Text = ct.UserName;
                item.Value = ct.ClientUserId.ToString();
                DDLservice.Items.Add(item);
            }

             
            if (!this.Reurl.Contains("?"))
            {
                ManageOrder manageOrder = this;
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
            this.btnUnPack.Click += new EventHandler(this.UnPackOrderInfos);
            this.btnPack.Click += new EventHandler(this.packOrderInfos);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            this.dlstOrders.ItemDataBound += new DataListItemEventHandler(this.dlstOrders_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.dlstOrders.ItemCommand += new DataListCommandEventHandler(this.dlstOrders_ItemCommand);
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.lkbtnRemove.Click += new EventHandler(this.lkbtnRemove_Click);
            
            this.BackButton.Click += new EventHandler(this.BackButton_Click);
            this.btnProductGoods.Click += new EventHandler(this.btnProductGoods_Click);
            this.btnOrderGoods.Click += new EventHandler(this.btnOrderGoods_Click);
            this.btnExport.Click += new EventHandler(this.btnExPortButton_Click);
            if (!this.Page.IsPostBack)
            {
                if (CustomConfigHelper.Instance.AutoShipping && CustomConfigHelper.Instance.AnonymousOrder)
                {
                    spDelete.Style["display"] = "none";
                    spRemove.Style["display"] = "";
                    anchorsDelete.Style["display"] = "";

                }
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
            
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
            if (CustomConfigHelper.Instance.BrandShow)
            {
                BrandShow.Value = "1";

            }
        }
        protected void btnExPortButton_Click(object sender, EventArgs e)
        {
            OrderQuery query = new OrderQuery();
            query.UserName = this.txtUserName.Text;
            query.ChannelName = this.txtChanneName.Text;
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
            if (this.DDLservice.SelectedValue != "未选择")
            {
                query.Sender = ManagerHelper.getSenderIdByClientUserId(this.DDLservice.SelectedValue.ToInt()).ToString();
                query.ClientUserId = this.DDLservice.SelectedValue.ToInt();
                query.modeName = ManagerHelper.getPcOrderStorenameByClientuserid(this.DDLservice.SelectedValue.ToInt());
            }
            if (this.DDLPayType.SelectedValue != "未选择")
            {
                query.PayTypeName = this.DDLPayType.SelectedValue;
            }


            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            DataTable dt=new DataTable ();
            if (CustomConfigHelper.Instance.IsSanzuo || CustomConfigHelper.Instance.BusinessName == "爽爽挝啡")
            {
                int quantity = 0;
                dt = OrderHelper.GetOrderSanZuo(query);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!Convert.IsDBNull(dt.Rows[i]["GiveQuantity"]))
                    {
                        quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]) - Convert.ToInt32(dt.Rows[i]["GiveQuantity"]);
                        dt.Rows[i]["Quantity"] = quantity;
                    }                
                }
            }
            else
            {
                dt = OrderHelper.GetOrder(query);
            }
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
            if (CustomConfigHelper.Instance.IsSanzuo || CustomConfigHelper.Instance.BusinessName == "爽爽挝啡")
            {
                fields.Add("ProductName");
                list2.Add("商品名称");
                fields.Add("Address");
                list2.Add("收货地址");
                fields.Add("ModeName");
                list2.Add("配送门店");
                fields.Add("Quantity");
                list2.Add("商品数量");
                fields.Add("HalfQuantity");
                list2.Add("半价数量");
                fields.Add("GiveQuantity");
                list2.Add("买送数量");
                fields.Add("ItemAdjustedPrice");
                list2.Add("商品金额");
                fields.Add("cellphone");
                list2.Add("手机号");
                fields.Add("remark");
                list2.Add("订单备注(街道信息)");
                fields.Add("managerremark");
                list2.Add("管理员备注");
            }
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
                    else if (str2 == "HalfQuantity")
                    {
                        builder.Append(row[str2] == DBNull.Value ? "0" : row["HalfQuantity"]).Append(",");
                    }
                    else if (str2 == "ReferralUserId")
                    {
                        builder.Append(row[str2].ToString() == "" ? "订单来源：主站" : row[str2].ToString() == "0" ? "订单来源：主站" : "订单来源：" + row["StoreName"]).Append(",");
                    }
                    else if (str2 == "OrderStatus")
                    {
                        builder.Append((OrderInfo.GetOrderStatusName((OrderStatus)row["OrderStatus"])).Replace(",", "，")).Append(",");
                    }
                    else if (str2 == "PaymentType")
                    {
                        builder.Append(row["RealModeName"].ToString() == "微信扫码支付" ? "微信扫码支付" : row["PaymentType"]).Append(",");
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
        //电商ID       
        private static string EBusinessID = "1259918";//"请到快递鸟官网申请http://www.kdniao.com/ServiceApply.aspx";
        //电商加密私钥，快递鸟提供，注意保管，不要泄漏
        private static string AppKey = "8822d37a-ea4a-4d83-bfe5-08c878ff53a5";//"请到快递鸟官网申请http://www.kdniao.com/ServiceApply.aspx";
        //请求url, 正式环境地址：http://api.kdniao.cc/api/Eorderservice
        //测试地址  http://testapi.kdniao.cc:8081/api/EOrderService  
        private static string ReqURL = "http://api.kdniao.cc/api/Eorderservice";
        /// <summary>
        /// Json方式  电子面单
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string orderTracesSubByJson(string OrderIds)
        {
                #region
                string printHtml = string.Empty;
                string productName = "";
                string requestData = "";
                JObject aaa =null;
                string province = null;
                string city = null;
                string area = null;
                int shipmentQuantity = 0;
                try
                {
                    IList<string> orderIds = OrderIds.Split(',');//将传来的orderid拆分成数组.
                    System.Text.StringBuilder builder = new System.Text.StringBuilder("");

                    foreach (string orderId in orderIds)
                    {
                        OrderQuery query = new OrderQuery
                        {
                            OrderId = orderId,
                        };
                        OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                        ShippersInfo shipper = SalesHelper.GetIsDefault(1);
                        if (shipper==null)
                        {
                            return("请添加发货人信息！");
                        }
                        string[] regionAddress = RegionHelper.GetFullRegion(shipper.RegionId, ",").Split(',');
                        if (order.RegionId !=0)
                        {   
                            string [] userRegionAddress=RegionHelper.GetFullRegion(order.RegionId, ",").Split(',');
                            province = userRegionAddress.GetValue(0).ToString();
                            city=userRegionAddress.GetValue(1).ToString();
                            if (userRegionAddress.Length>2)
                            area = userRegionAddress.GetValue(2).ToString();
                        }
                        else if (order.ShippingRegion != null)
                                {
                                    string region = order.ShippingRegion;
                                    int p= region.IndexOf("省") + 1;
                                    int c= region.IndexOf("市")+1;
                                    province = region.Substring(0,p);
                                    city = region.Substring(p,c- p);
                                    if (order.ShippingRegion.IndexOf("区") > 0)
                                    {
                                        int a = region.IndexOf("区") + 1;
                                        area = region.Substring(c, a - c);
                                    }
                                    else if (order.ShippingRegion.IndexOf("县") > 0)
                                    {
                                        int a = region.IndexOf("县") + 1;
                                        area = region.Substring(c, a - c);
                                    }
                                }
                        else
                        {
                            return ("订单编号为"+order.OrderId+"收货人地址信息错误！");
                        }
                        System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = order.LineItems;
                        if (lineItems1!= null)
                        {
                            foreach (string str in lineItems1.Keys)
                            {
                                LineItemInfo info = lineItems1[str];
                                productName += info.ItemDescription;
                                shipmentQuantity = info.ShipmentQuantity;
                            }
                        }
                        //if (productName.Length >60)
                        //{
                        //    productName = order.ManagerRemark;
                        //}
                        string phone = order.TelPhone;
                        string shipperPhone = shipper.TelPhone;
                        if (order.TelPhone == null || order.TelPhone=="")
                        {
                            phone = order.CellPhone;
                        }
                        if (shipper.TelPhone == null || shipper.TelPhone=="")
                        {
                            shipperPhone = shipper.CellPhone;
                        }
                        if (order.ExpressCompanyName == "韵达快运" || order.ExpressCompanyName == "圆通速递")
                        {
                            if (order.ExpressCompanyName == "韵达快运")
                            {
                                #region

                                requestData = "{'OrderCode': '" + order.OrderId + "'," +
                                        "'ShipperCode':'YD'," +
                                        "'PayType':1," +
                                        "'ExpType':1," +
                                        "'Cost':" + order.Freight + "," +
                                        "'OtherCost':1.0," +
                                        "'Sender':" +
                                        "{" +
                                        "'Company':' ','Name':'" + shipper.ShipperName + "','Mobile':'" + shipperPhone + "','ProvinceName':'" + regionAddress.GetValue(0) + "','CityName':'" + regionAddress.GetValue(1) + "','ExpAreaName':'" + regionAddress.GetValue(2) + "','Address':'" + shipper.Address + "'}," +
                                        "'Receiver':" +
                                        "{" +
                                        "'Company':' ','Name':'" + order.ShipTo + "','Mobile':'" + phone + "','ProvinceName':'" + province + "','CityName':'" + city + "','ExpAreaName':'" + area + "','Address':'" + order.Address + "'}," +
                                        "'Commodity':" +
                                        "[{" +
                                        "'GoodsName':'" + productName + "','Goodsquantity':" + shipmentQuantity + ",'GoodsWeight':" + order.Weight + "}]," +
                                    //"'AddService':" +
                                    //"[{" +
                                    //"'Name':'COD','Value':'1020'}]," +
                                       //"'CustomerPwd':'mhRcHwnBgiIYFWXfqZQ64NUDSCbG83'," +
                                       //"'CustomerName':'2660418786'," +
                                        "'CustomerPwd':'839MCiHY6ZXNfpB2W7qVuT5xdcAnmP'," +
                                       "'CustomerName':'43012088820'," +
                                        "'Weight':1.0," +
                                        "'Quantity':1," +
                                        "'Volume':0.0," +
                                        "'Remark':'小心轻放'," +
                                        "'IsReturnPrintTemplate':1}";
                                #endregion
                            }
                            if (order.ExpressCompanyName == "圆通速递")
                            {
                                #region
                                requestData = "{'OrderCode': '" + order.OrderId + "'," +
                                        "'ShipperCode':'YTO'," +
                                        "'PayType':1," +
                                        "'ExpType':1," +
                                        "'Cost':" + order.Freight + "," +
                                        "'OtherCost':1.0," +
                                        "'Sender':" +
                                        "{" +
                                        "'Company':' ','Name':'" + shipper.ShipperName + "','Mobile':'" + shipperPhone + "','ProvinceName':'" + regionAddress.GetValue(0) + "','CityName':'" + regionAddress.GetValue(1) + "','ExpAreaName':'" + regionAddress.GetValue(2) + "','Address':'" + shipper.Address + "'}," +
                                        "'Receiver':" +
                                        "{" +
                                        "'Company':' ','Name':'" + order.ShipTo + "','Mobile':'" + phone + "','ProvinceName':'" + province + "','CityName':'" + city + "','ExpAreaName':'" + area + "','Address':'" + order.Address + "'}," +
                                        "'Commodity':" +
                                        "[{" +
                                        "'GoodsName':'" + productName + "','Goodsquantity':" + shipmentQuantity + ",'GoodsWeight':" + order.Weight + "}]," +
                                    //"'AddService':" +
                                    //"[{" +
                                    //"'Name':'COD','Value':'1020'}]," +
                                       "'MonthCode':'QXACQh8Y'," +
                                       "'CustomerName':'k11108881'," +
                                        "'Weight':1.0," +
                                        "'Quantity':1," +
                                        "'Volume':0.0," +
                                        "'Remark':'小心轻放'," +
                                        "'IsReturnPrintTemplate':1}";
                                #endregion
                            }
                        }
                        else
                        {
                            return ("订单编号为"+order.OrderId+"快递公司命名不规范或为空！正确命名:'韵达快运'或'圆通速递'。");
                        }
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8));
                        param.Add("EBusinessID", EBusinessID);
                        param.Add("RequestType", "1007");
                        string dataSign = encrypt(requestData, AppKey, "UTF-8");
                        param.Add("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
                        param.Add("DataType", "2");
                        string result = sendPost(ReqURL, param);
                        aaa = JsonToObject<JObject>(result);
                        if (aaa["Reason"].ToString() != "该订单号已下单成功")
                        {
                            if (aaa["Success"].ToString() == "False")
                            {
                                return ("订单编号"+order.OrderId+""+aaa["Reason"].ToString());
                           
                            }
                        }
                        printHtml += aaa["PrintTemplate"].ToString();
                        OrderHelper.SetOrderShipNumber(order.OrderId, aaa["Order"]["LogisticCode"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    return "{" + ex.Message + "}";
                }
                //根据公司业务处理返回的信息......
                return printHtml;
                #endregion
        }
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>        
        private static string sendPost(string url, Dictionary<string, string> param)
                {
                    string result = "";
                    StringBuilder postData = new StringBuilder();
                    if (param != null && param.Count > 0)
                    {
                        foreach (var p in param)
                        {
                            if (postData.Length > 0)
                            {
                                postData.Append("&");
                            }
                            postData.Append(p.Key);
                            postData.Append("=");
                            postData.Append(p.Value);
                        }
                    }
                    byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Referer = url;
                        request.Accept = "*/*";
                        request.Timeout = 30 * 1000;
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                        request.Method = "POST";
                        request.ContentLength = byteData.Length;
                        Stream stream = request.GetRequestStream();
                        stream.Write(byteData, 0, byteData.Length);
                        stream.Flush();
                        stream.Close();
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream backStream = response.GetResponseStream();
                        StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                        result = sr.ReadToEnd();
                        sr.Close();
                        backStream.Close();
                        response.Close();
                        request.Abort();
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                    return result;
                }
        ///<summary>
        ///电商Sign签名
        ///</summary>
        ///<param name="content">内容</param>
        ///<param name="keyValue">Appkey</param>
        ///<param name="charset">URL编码 </param>
        ///<returns>DataSign签名</returns>        
        private static string encrypt(String content, String keyValue, String charset)
        {
            if (keyValue != null)
            {
                return base64(MD5(content + keyValue, charset), charset);
            }
            return base64(MD5(content, charset), charset);
        }
        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>      
        private static string MD5(string str, string charset)
                {
                    byte[] buffer = System.Text.Encoding.GetEncoding(charset).GetBytes(str);
                    try
                    {
                        System.Security.Cryptography.MD5CryptoServiceProvider check;
                        check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] somme = check.ComputeHash(buffer);
                        string ret = "";
                        foreach (byte a in somme)
                        {
                            if (a < 16)
                                ret += "0" + a.ToString("X");
                            else
                                ret += a.ToString("X");
                        }
                        return ret.ToLower();
                    }
                    catch
                    {
                        throw;
                    }
                }
        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>      
        private static string base64(String str, String charset)
                {
                    return Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(str));
                }
        public static T JsonToObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

   }

}