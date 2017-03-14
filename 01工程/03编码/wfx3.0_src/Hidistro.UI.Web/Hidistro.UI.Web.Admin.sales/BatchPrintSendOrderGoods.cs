using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Orders;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class BatchPrintSendOrderGoods : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		private System.Collections.Generic.List<OrderInfo> GetPrintData(string orderIds)
		{
			System.Collections.Generic.List<OrderInfo> list = new System.Collections.Generic.List<OrderInfo>();
			string[] array = orderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
				list.Add(orderInfo);
			}
			return list;
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string orderIds = base.Request["orderIds"].Trim(new char[]
			{
				','
			});
			if (!string.IsNullOrEmpty(base.Request["orderIds"]))
			{
				foreach (OrderInfo info in this.GetPrintData(orderIds))
				{
                    System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    child.Attributes["class"] = "order print";
                    System.Text.StringBuilder builder = new System.Text.StringBuilder("");
					
                    switch (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
                    {
                        case true:/*
                            Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(info.ReferralUserId);
                            divContent.Attributes.Add("style", "width:300px;text-align: center;margin:0 auto;");
                            builder.AppendFormat("<div class=\"info\" style=\"font-size:14px\"><div class=\"prime-info\" style=\"margin-right: 10px;\"></div><ul class=\"sub-info\"><li style=\"text-align:center;width:100%\"><h3>{0}</h3></li><li><span>下单时间： </span>{1}</li><li><span>订单编号： </span>{2}</li><li><span>客户： </span>{3}<br/><span style=\"margin-left: 10px;\">电话： </span>{5}</li><li><span>地址： </span>{4}</li></ul><br class=\"clear\" /></div><div style=\"border-bottom:1px dashed #000; margin-bottom:10px\"></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", info.OrderDate.ToString("yyyy-MM-dd HH:mm"), info.OrderId, info.Username, info.Address, info.CellPhone);
                            builder.Append("<table style=\"width:100%;background:#fff;font-size:14px\"><thead><tr><th style=\"width:40%;background:#fff;border:none\">菜名</th><th style=\"background:#fff;border:none\">数量</th><th style=\"background:#fff;border:none\">价格</th></tr></thead><tbody>");
					        System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = info.LineItems;
					        if (lineItems1 != null)
					        {
						        foreach (string str2 in lineItems1.Keys)
						        {
							        LineItemInfo info2 = lineItems1[str2];
							        builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
							        builder.AppendFormat("<td>{0}</td>", info2.ShipmentQuantity);
							        builder.AppendFormat("<td>{0}</td></tr>", System.Math.Round(info2.GetSubTotal(), 2));
						        }
					        }

                            builder.AppendFormat("</tbody></table><div style=\"border-bottom:1px dashed #000; margin-top:10px\"></div><ul class=\"price\" style=\"margin-right:10px;\"><li style=\"text-align:right;width:100%;font-size:14px\">小计： ￥{0}</li>", System.Math.Round(info.GetAmount(), 2));
					        decimal reducedPromotionAmount1 = info.ReducedPromotionAmount;
					        if (reducedPromotionAmount1 > 0m)
					        {
						        builder.AppendFormat("<li><span>优惠金额：</span>{0}</li>", System.Math.Round(reducedPromotionAmount1, 2));
					        }
					        decimal payCharge1 = info.PayCharge;
					        if (payCharge1 > 0m)
					        {
						        builder.AppendFormat("<li><span>支付手续费：</span>{0}</li>", System.Math.Round(payCharge1, 2));
					        }
					        if (!string.IsNullOrEmpty(info.CouponCode))
					        {
						        decimal couponValue = info.CouponValue;
						        if (couponValue > 0m)
						        {
							        builder.AppendFormat("<li><span>优惠券：</span>{0}</li>", System.Math.Round(couponValue, 2));
						        }
					        }
                            builder.AppendFormat("<li style=\"text-align:center;width:100%;font-size:14px;\"><h4>谢谢惠顾！</h4></li><br>");
                        //decimal adjustedDiscount1 = info.AdjustedDiscount;
                            //if (adjustedDiscount1 > 0m)
                            //{
                            //    builder.AppendFormat("<li><span>管理员手工加价：</span>{0}</li>", System.Math.Round(adjustedDiscount1, 2));
                            //}
                            //else
                            //{
                            //    builder.AppendFormat("<li><span>管理员手工减价：</span>{0}</li>", System.Math.Round(-adjustedDiscount1, 2));
                            //}
                            //builder.AppendFormat("<li><span>实付金额：</span>{0}</li></ul><br class=\"clear\" /><br><br>", System.Math.Round(info.GetTotal(), 2));
                                   */

                            Hidistro.Entities.Members.DistributorsInfo currentDistributor = Hidistro.SaleSystem.Vshop.DistributorsBrower.GetCurrentDistributors(info.ReferralUserId);
                            builder.Append("<div style='width:270px;margin:0 auto;padding:10px;' >");
                            builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><img src='/Templates/vshop/common/images/login_logo2.png' /><h3>SALES MEMO</h3><div style='text-align:left;padding-bottom:5px;'><span>消费门店： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>下单时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>订单编号： </span>{2}</div><div style='text-align:left;padding-bottom:5px;'><span>消费客户： </span>{3}</div><div><span style=\"margin-left: 10px;\">电话： </span>{5}</div><div><span>地址： </span>{4}</div> </div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", currentDistributor != null ? currentDistributor.StoreName : "总店", info.OrderDate.ToString("yyyy-MM-dd HH:mm"), info.OrderId, info.Username, info.Address, info.CellPhone);
                            builder.Append("<table style='width:100%;background:#fff;font-size:14px'><thead><tr><th style='width:60%;background:#fff;border:none;text-align:left;'>菜名</th><th style='background:#fff;border:none'>数量</th><th style='background:#fff;border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                            System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems1 = info.LineItems;
                            if (lineItems1 != null)
                            {
                                foreach (string str2 in lineItems1.Keys)
                                {
                                    LineItemInfo info2 = lineItems1[str2];
                                    builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                                    builder.AppendFormat("<td style='text-align:center;'>{0}</td>", info2.ShipmentQuantity);
                                    builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", System.Math.Round(info2.GetSubShowTotal(), 2));
                                }
                            }
                            builder.AppendFormat("</tbody></table><div style='border-bottom:1px dashed #000; margin:10px 0;'></div>");

                            decimal reducedPromotionAmount1 = info.ReducedPromotionAmount;
                            if (reducedPromotionAmount1 > 0m)
                            {
                                builder.AppendFormat("<div><span>优惠金额：</span>{0}</div>", System.Math.Round(reducedPromotionAmount1, 2));
                            }
                            decimal payCharge1 = info.PayCharge;
                            if (payCharge1 > 0m)
                            {
                                builder.AppendFormat("<div><span>支付手续费：</span>{0}</div>", System.Math.Round(payCharge1, 2));
                            }
                            if (!string.IsNullOrEmpty(info.CouponCode))
                            {
                                decimal couponValue = info.CouponValue;
                                if (couponValue > 0m)
                                {
                                    builder.AppendFormat("<div><span>优惠抵扣： </span>￥{0}</div>", System.Math.Round(couponValue, 2));
                                }
                            }
                            //计算买一送一减免
                            decimal giveBuyPrice=0m;
                            foreach (LineItemInfo itemInfo in info.LineItems.Values)
                            {
                                if (itemInfo.GiveQuantity > 0)
                                {
                                    giveBuyPrice += itemInfo.GiveQuantity * itemInfo.ItemAdjustedPrice;
                                }
                            }
                            if (giveBuyPrice > 0m)
                            {
                                builder.AppendFormat("<div style='font-size:14px;margin:5px 0;'><span>买一赠一： </span>￥{0}</div>", System.Math.Round(giveBuyPrice, 2));
                            }
                            //应收
                            builder.AppendFormat("<div style='text-align:left;width:100%;font-size:26px'><span style='font-size:26px'>应收： </span>￥{0}</div>", System.Math.Round(info.GetAmount()-info.CouponValue, 2));
                            builder.AppendFormat("<div style='text-align:center;width:100%;font-size:14px;font-weight:bold;margin-top:30px;'>谢谢光临！Thank you for coming</div><div style='text-align:center;width:100%;font-size:12px;'>广东爽爽挝啡快饮有限公司</div>");
                            builder.Append("</div>");
                            break;
                        default:
                            divContent.Style.Value = "width:700px;text-align: center;margin:0 auto;";
                            builder.AppendFormat("<div class=\"info\"><div class=\"prime-info\" style=\"margin-right: 20px;\"><p><span><h3 style=\"font-weight: normal\">{0}</h3></span></p></div><ul class=\"sub-info\"><li><span>生成时间： </span>{1}</li><li><span>订单编号： </span>{2}</li></ul><br class=\"clear\" /></div>", info.ShipTo, info.OrderDate.ToString("yyyy-MM-dd HH:mm"), info.OrderId);
                            builder.Append("<table class=\"tb\"><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr ><th class=\"th1\">货号</th><th class=\"th1\">商品名称</th><th class=\"th1\">规格</th><th class=\"th1\">数量</th><th class=\"th1\">单价</th><th class=\"th1\">总价</th></tr></thead><tbody>");
					        System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = info.LineItems;
					        if (lineItems != null)
					        {
						        foreach (string str2 in lineItems.Keys)
						        {
							        LineItemInfo info2 = lineItems[str2];
                                    builder.AppendFormat("<tr ><td class=\"td1\">{0}</td>", info2.SKU);
                                    builder.AppendFormat("<td class=\"td1\">{0}</td>", info2.ItemDescription);
                                    builder.AppendFormat("<td class=\"td1\">{0}</td>", info2.SKUContent);
                                    builder.AppendFormat("<td class=\"td1\">{0}</td>", info2.ShipmentQuantity);
                                    builder.AppendFormat("<td class=\"td1\">{0}</td>", System.Math.Round(info2.ItemListPrice, 2));
                                    builder.AppendFormat("<td class=\"td1\">{0}</td></tr>", System.Math.Round(info2.GetSubTotal(), 2));
						        }
					        }
					        builder.AppendFormat("</tbody></table><ul class=\"price\"><li><span>商品总价： </span>{0}</li><li><span>运费： </span>{1}</li>", System.Math.Round(info.GetAmount(), 2), System.Math.Round(info.AdjustedFreight, 2));
					        decimal reducedPromotionAmount = info.ReducedPromotionAmount;
					        if (reducedPromotionAmount > 0m)
					        {
						        builder.AppendFormat("<li><span>优惠金额：</span>{0}</li>", System.Math.Round(reducedPromotionAmount, 2));
					        }
					        decimal payCharge = info.PayCharge;
					        if (payCharge > 0m)
					        {
						        builder.AppendFormat("<li><span>支付手续费：</span>{0}</li>", System.Math.Round(payCharge, 2));
					        }
					        if (!string.IsNullOrEmpty(info.CouponCode))
					        {
						        decimal couponValue = info.CouponValue;
						        if (couponValue > 0m)
						        {
							        builder.AppendFormat("<li><span>优惠券：</span>{0}</li>", System.Math.Round(couponValue, 2));
						        }
					        }
					        decimal adjustedDiscount = info.AdjustedDiscount;
					        if (adjustedDiscount > 0m)
					        {
						        builder.AppendFormat("<li><span>管理员手工加价：</span>{0}</li>", System.Math.Round(adjustedDiscount, 2));
					        }
					        else
					        {
						        builder.AppendFormat("<li><span>管理员手工减价：</span>{0}</li>", System.Math.Round(-adjustedDiscount, 2));
					        }
					        builder.AppendFormat("<li><span>实付金额：</span>{0}</li></ul><br class=\"clear\" /><br><br>", System.Math.Round(info.GetTotal(), 2));
                            break;
                    }
					
					child.InnerHtml = builder.ToString();
					this.divContent.Controls.AddAt(0, child);
				}
			}
		}
	}
}
