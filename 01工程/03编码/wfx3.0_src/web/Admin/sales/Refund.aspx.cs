using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public partial class Refund : AdminPage
	{

		private void btnAuditAcceptRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.txtAdminRemark.Text.Trim(),
				HandleTime = DateTime.Now,
				AuditTime = DateTime.Now.ToString(),
				HandleStatus = RefundInfo.Handlestatus.HasTheAudit
			};
			if (!decimal.TryParse(this.hidAuditM.Value, out num))
			{
                this.ShowMsg("输入的金额格式不正确", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("不能为负数?！", false);
				return;
			}
			if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
			{
                this.ShowMsg("审核失败，请重试。", false);
				return;
			}
            this.ShowMsg("审核成功", true);
			this.LoadReturnApplyData();
		}

		private void btnAuditRefuseRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.txtAdminRemark.Text.Trim(),
				HandleTime = DateTime.Now,
				HandleStatus = RefundInfo.Handlestatus.AuditNotThrough,
				Operator = Globals.GetCurrentManagerUserId().ToString()
			};
			if (!decimal.TryParse(this.hidAuditM.Value, out num))
			{
                this.ShowMsg("输入的金额格式不正确", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("不能为负数2！", false);
				return;
			}
			if (!RefundHelper.UpdateByAuditReturnsId(refundInfo))
			{
                this.ShowMsg("操作失败，请重试。", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				string skuId = null;
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.ProductId != int.Parse(this.hidProductId.Value))
					{
						continue;
					}
					skuId = value.SkuId;
				}
				if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, 3))
				{
                    this.ShowMsg("操作成功", true);
					this.LoadReturnApplyData();
					return;
				}
			}
		}

		private void lkbtnDeleteCheck_Click(object obj, EventArgs eventArg)
		{
			int num;
			string item = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
                item = base.Request["CheckBoxGroup"];
			}
			if (item.Length <= 0)
			{
                this.ShowMsg("请选要删除的退款申请单", false);
				return;
			}
            string str = "成功删除了{0}个退款申请单";
			char[] chrArray = new char[] { ',' };
            str = (!RefundHelper.DelRefundApply(item.Split(chrArray), out num) ? string.Concat(string.Format(str, num), ",待处理的申请不能删除") : string.Format(str, num));
			this.LoadReturnApplyData();
			this.ShowMsg(str, true);
		}

		private void btnSearchButton_Click(object obj, EventArgs eventArg)
		{
			this.AH8CVHyiSPoKgHss5CP(true);
		}

		private void dlstRefund_ItemDataBound(object obj, DataListItemEventArgs dataListItemEventArg)
		{
			if (dataListItemEventArg.Item.ItemType == ListItemType.Item || dataListItemEventArg.Item.ItemType == ListItemType.AlternatingItem)
			{
                HtmlAnchor htmlAnchor = (HtmlAnchor)dataListItemEventArg.Item.FindControl("lkbtnCheckRefund");
                Label label = (Label)dataListItemEventArg.Item.FindControl("lblHandleStatus");
                if (label.Text == "4")
				{
                    label.Text = "未审核";
					return;
				}
                if (label.Text == "5")
				{
                    label.Text = "已审核";
					return;
				}
                if (label.Text == "6")
				{
                    label.Text = "未退款";
					return;
				}
                if (label.Text == "2")
				{
                    label.Text = "已退款";
					return;
				}
                if (label.Text == "8")
				{
                    label.Text = "拒绝退款";
					return;
				}
                label.Text = "审核不通过";
			}
		}

		private void LoadReturnApplyData()
		{
			ReturnsApplyQuery returnsApplyQuery = this.AGPpN9tMpJSLGMt1AaU0jgdA();
			DbQueryResult returnOrderAll = RefundHelper.GetReturnOrderAll(returnsApplyQuery);
			this.dlstRefund.DataSource = returnOrderAll.Data;
			this.dlstRefund.DataBind();
			this.pager.TotalRecords = returnOrderAll.TotalRecords;
			this.pager1.TotalRecords = returnOrderAll.TotalRecords;
			this.txtOrderId.Text = returnsApplyQuery.OrderId;
			this.ddlHandleStatus.SelectedIndex = 0;
			this.ddlHandleStatus.SelectedValue = returnsApplyQuery.HandleStatus.Value.ToString();
		}

		private ReturnsApplyQuery AGPpN9tMpJSLGMt1AaU0jgdA()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
                returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				returnsApplyQuery.HandleStatus = new int?(-1);
			}
			else
			{
				int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num))
				{
					returnsApplyQuery.HandleStatus = new int?(num);
				}
			}
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
            returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}

		private void AH8CVHyiSPoKgHss5CP(bool flag)
		{
			NameValueCollection nameValueCollection = new NameValueCollection()
			{
				{ "OrderId", this.txtOrderId.Text },
				{ "PageSize", this.pager.PageSize.ToString() }
			};
			if (!flag)
			{
                nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
			{
                nameValueCollection.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
			}
			base.ReloadPage(nameValueCollection);
		}

		private void AINLPE2AqL(string str)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
            orderInfo.CloseReason = "客户要求退货(款)！";
			if (RefundHelper.CloseTransaction(orderInfo))
			{
				orderInfo.OnClosed();
				MemberInfo member = MemberHelper.GetMember(orderInfo.UserId);
				Messenger.OrderClosed(member, orderInfo, orderInfo.CloseReason);
			}
		}

		private void btnRefuseRefund_Click(object obj, EventArgs eventArg)
		{
			decimal num = new decimal(0);
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.hidAdminRemark.Value.Trim(),
				HandleTime = DateTime.Now,
				HandleStatus = RefundInfo.Handlestatus.RefuseRefunded,
				Operator = Globals.GetCurrentManagerUserId().ToString()
			};
			if (!decimal.TryParse(this.hidRefundM.Value, out num))
			{
                this.ShowMsg("输入的金额格式不正确", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("不能为负数3！", false);
				return;
			}
			if (!RefundHelper.UpdateByReturnsId(refundInfo))
			{
                this.ShowMsg("操作失败，请重试。", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				string skuId = null;
				int orderItemsStatus = 0;
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.ProductId != int.Parse(this.hidProductId.Value))
					{
						continue;
					}
					skuId = value.SkuId;
					orderItemsStatus = (int)value.OrderItemsStatus;
				}
				if (orderItemsStatus == 7)
				{
					this.hidStatus.Value = 3.ToString();
				}
				if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, (int.Parse(this.hidStatus.Value) == 6 ? 2 : 3)))
				{
                    this.ShowMsg("操作成功", true);
					this.LoadReturnApplyData();
					return;
				}
			}
		}

		protected void btnAcceptRefund_Click(object sender, EventArgs e)
		{
			decimal num = new decimal(0);
			int num1 = 0;
			RefundInfo refundInfo = new RefundInfo()
			{
				RefundId = int.Parse(this.hidReturnsId.Value),
				AdminRemark = this.hidAdminRemark.Value.Trim(),
				HandleTime = DateTime.Now,
				RefundTime = DateTime.Now.ToString(),
				HandleStatus = RefundInfo.Handlestatus.Refunded,
				Operator = Globals.GetCurrentManagerUserId().ToString()
			};
			if (!decimal.TryParse(this.hidRefundM.Value, out num))
			{
                this.ShowMsg("输入的金额格式不正确", false);
				return;
			}
			refundInfo.RefundMoney = num;
			if (num < new decimal(0))
			{
                this.ShowMsg("不能为负数4！", false);
				return;
			}
			if (!RefundHelper.UpdateByReturnsId(refundInfo))
			{
                this.ShowMsg("退款失败，请重试。", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				string skuId = null;
				string str = null;
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.ProductId != int.Parse(this.hidProductId.Value))
					{
						continue;
					}
					skuId = value.SkuId;
					str = value.Quantity.ToString();
				}
				if (RefundHelper.UpdateOrderGoodStatu(this.hidOrderId.Value, skuId, 9))
				{
					RefundHelper.UpdateRefundOrderStock(str, skuId);
					foreach (LineItemInfo lineItemInfo in OrderHelper.GetOrderInfo(this.hidOrderId.Value).LineItems.Values)
					{
						if (lineItemInfo.OrderItemsStatus.ToString() != OrderStatus.Refunded.ToString())
						{
							continue;
						}
						num1++;
					}
					if (orderInfo.LineItems.Values.Count == num1)
					{
						this.AINLPE2AqL(this.hidOrderId.Value);
					}
                    this.ShowMsg("成功退款", true);
					this.LoadReturnApplyData();
					return;
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dlstRefund.ItemDataBound += new DataListItemEventHandler(this.dlstRefund_ItemDataBound);
			this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
			this.btnAuditAcceptRefund.Click += new EventHandler(this.btnAuditAcceptRefund_Click);
			this.btnAuditRefuseRefund.Click += new EventHandler(this.btnAuditRefuseRefund_Click);
			if (!base.IsPostBack)
			{
				this.LoadReturnApplyData();
			}
		}
	}
}