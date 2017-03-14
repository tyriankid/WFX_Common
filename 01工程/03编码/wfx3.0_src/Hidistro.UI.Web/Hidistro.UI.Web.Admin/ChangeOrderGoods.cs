using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
    public class ChangeOrderGoods : AdminPage
    {
        protected Repeater rptOrderGoods;
        protected Button btnSubmit;
        protected Literal litOrderId;
        protected Literal litOrderTime;
        protected TextBox txtRemark;
        private string orderId;

        protected static string orderID;

        protected DataTable dtSku;

        private void OrderGoodsBind()
        {
            OrderInfo currentOrder = OrderHelper.GetOrderInfo(orderId);
            litOrderId.Text = orderId;
            litOrderTime.Text = currentOrder.OrderDate.ToString();
            rptOrderGoods.DataSource = currentOrder.LineItems.Values;
            rptOrderGoods.DataBind();
        }
     
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            OrderInfo currentOrder = OrderHelper.GetOrderInfo(orderId);
            //获取退款之前的商品skuid和数量,用于计算库存的减少增加
            dtSku = new DataTable();
            dtSku.Columns.Add("skuid");
            dtSku.Columns.Add("quantity");
            foreach (LineItemInfo linfo in currentOrder.LineItems.Values)
            {
                dtSku.Rows.Add(linfo.SkuId, linfo.Quantity);
            }
            if (!this.Page.IsPostBack)
            {
                this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
                this.OrderGoodsBind();
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
                OrderInfo info = OrderHelper.GetOrderInfo(orderId);
                //使用批量提交修改OrderItems的信息
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("orderId");
                dt1.Columns.Add("skuId");
                dt1.Columns.Add("ItemDescription");
                dt1.Columns.Add("ItemAdjustedPrice");
                dt1.Columns.Add("ThumbnailsUrl");
                dt1.Columns.Add("Quantity");
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["orderId"] };
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["skuId"] };
                DataTable dt2 = dt1.Clone();   
                    foreach (LineItemInfo lInfo in info.LineItems.Values)
                    {
                        dt1.Rows.Add(info.OrderId, lInfo.SkuId, lInfo.ItemDescription, lInfo.ItemAdjustedPrice, lInfo.ThumbnailsUrl, lInfo.Quantity);
                    }
                dt1.AcceptChanges();
                foreach (RepeaterItem rInfo in rptOrderGoods.Items)
                {
                        dt2.Rows.Add(info.OrderId,
                            ((Literal)rInfo.FindControl("litSkuId")).Text,
                            ((Literal)rInfo.FindControl("litProductName")).Text,
                            ((Literal)rInfo.FindControl("litProductAmount")).Text,
                            ((Image)rInfo.FindControl("imgUrl")).ImageUrl,
                            ((TextBox)rInfo.FindControl("quantity")).Text
                            );
                }

                //修改Order主表的OrderTotal,OrderPoint,OrderCostPrice(成本价),ORderProfit(利润),amount(商品总额)
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                decimal OrderTotal = 0M;
                int OrderPoint = 0;
                decimal OrderCostPrice = 0M;
                decimal OrderProfit = 0M;
                decimal Amount = 0M;
                foreach (DataRow row in dt2.Rows)
                {
                    decimal rowPrice = Convert.ToDecimal(row["ItemAdjustedPrice"]);
                    int rowQuantity = Convert.ToInt16(row["Quantity"]);
                    Amount += rowPrice * rowQuantity;
                    OrderTotal += rowPrice * rowQuantity;
                    OrderPoint += Convert.ToInt32(rowPrice * rowQuantity * masterSettings.PointsRate);
                    OrderCostPrice += ProductHelper.GetSkuCostPrice(row["skuId"].ToString()) * rowQuantity;
                }
                OrderProfit = Amount - OrderCostPrice;

                //增加订单备注
                bool flag = false;
                if (this.txtRemark.Text.Length > 300)
                {
                    this.ShowMsg("备忘录长度限制在300个字符以内", false);
                }
                else
                {
                    Regex regex = new Regex("^[^*?|<>']+$");
                    if (!regex.IsMatch(this.txtRemark.Text))
                    {
                        this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                        flag = false;
                    }
                    else
                    {
                            info.ManagerMark = OrderMark.ExclamationMark;//this.orderRemarkImageForRemark.SelectedValue;
                            info.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                            if (OrderHelper.SaveRemark(info))
                            {
                                //this.BindOrders();
                                // this.ShowMsg("保存备忘录成功", true);
                                flag = true;
                            }
                            else
                            {
                                this.ShowMsg("保存失败", false);
                                flag = false;
                            }
                    }
                }
                if (!flag) { return; }
                dt1.PrimaryKey = new DataColumn[] { dt1.Columns["skuId"] };
                dt2.PrimaryKey = new DataColumn[] { dt2.Columns["skuId"] };

                DataBaseHelper.GetDtDifferent(dt1, dt2);
            if (OrderHelper.UpdateOrderAmountInfo(orderId, OrderTotal, OrderPoint, OrderCostPrice, OrderProfit, Amount) && DataBaseHelper.CommitDataTable(dt1, "Select orderId,skuId,ItemDescription,ItemAdjustedPrice,ThumbnailsUrl,Quantity From Hishop_OrderItems") != -1)
                {
                    DataTable dtSku2 = OrderHelper.GetOrderItemSkuInfo(orderId);
                    if (OrderHelper.UpdateOrderQuantity(dtSku, dtSku2))
                    {
                        this.ShowMsg("退单成功", true);
                    }
                    else
                    {
                        this.ShowMsg("退单失败", false);
                    }                                 
                }                      
            }
        }
   }