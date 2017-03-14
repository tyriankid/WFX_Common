namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hishop.Weixin.MP.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VTodayOrder : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litstorename;
        protected override void AttachChildControls()
        {
            this.litstorename = (Literal)this.FindControl("litstorename");
            string startDate = DateTime.Now.ToString("yyyy-MM-dd")+" 00:00:00";
            string endDate = DateTime.Now.ToString("yyyy-MM-dd")+" 23:59:59";

           
            string modeName = "本店";
            int userid = 0;
            int couponCount = 0;
            decimal couponTotalPrice = 0m;//优惠券总价
            int giveCount = 0;
            int halfCount = 0;
            decimal halfPrice = 0m;//第二杯半价总价
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
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                //ManagerInfo currentManager = ManagerHelper.GetCurrentManager();

                int sendId = ManagerHelper.getSenderIdByClientUserId(currentMember.UserId);
                int senderId = 0;
                string storeName = "";
                if (sendId != 0)
                {
                    senderId = sendId;
                    storeName = ManagerHelper.getPcOrderStorenameBySender(sendId); //DistributorsBrower.GetUserIdDistributors(DistributorsBrower.GetSenderDistributorId(senderId.ToString())).StoreName;
                }
                else
                {
                    return;
                }
                userid = currentMember.UserId;
                IList<string> orderIds = OrderHelper.GetTodayOrders(startDate, endDate, senderId, storeName, userid, modeName); //将今天的orderid拆分成数组
                string rrr = "";
                foreach (string a in orderIds)
                {
                    rrr += a + ",";
                }
                DataTable dtProducts = OrderHelper.GetTodayProducts(startDate, endDate, senderId, storeName, userid, modeName);//将今天的所有卖出的商品存在DataTable里,下面循环会往里面填值
                dtProducts.PrimaryKey = new DataColumn[] { dtProducts.Columns["ProductId"] };

                System.Text.StringBuilder builder = new System.Text.StringBuilder("");
                //头部,开始时间,结束时间,制单时间
                builder.Append("<div style='width:270px;margin:0 auto;padding:10px;overflow:hidden' >");
                builder.AppendFormat("<div style='font-size:14px;width:100%;text-align:center'><h3>门店统计日结报表</h3><div style='text-align:left;padding-bottom:5px;'><span>开始时间： </span>{0}</div><div style='text-align:left;padding-bottom:5px;'><span>结束时间： </span>{1}</div><div style='text-align:left;padding-bottom:5px;'><span>制单时间： </span>{2}</div></div><div style='border-bottom:1px dashed #000; margin:10px 0'></div>", startDate.ToString(), endDate.ToString(), DateTime.Now.ToString());
                //列表table
                builder.Append("<table style='width:100%;font-size:14px'><thead><tr><th style='width:40%;border:none;text-align:left;'>项目</th><th style='border:none;width:30%;'>数量</th><th style='border:none;text-align:right;'>价格</th></tr></thead><tbody>");
                foreach (string orderId in orderIds)
                {
                    OrderQuery query = new OrderQuery
                    {
                        OrderId = orderId,
                    };
                    OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                    int SendOut = 0;
                    if (order.ManagerRemark != null)
                    {
                        SendOut = order.ManagerRemark.IndexOf("退单");
                    }
                    if (order.ManagerRemark == null || SendOut < 0)
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
                                halfCount += info2.HalfPriceQuantity;
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
                                builder.AppendFormat("<td>{0}</td>", giftInfo.GiftName + "(礼品)");
                                builder.AppendFormat("<td style='text-align:center;'>{0}</td>", giftInfo.Quantity);
                                builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", giftInfo.costPoint.ToString() + "积分");
                            }
                        }
                    }
                }
                foreach (DataRow row in dtProducts.Rows)
                {
                    builder.AppendFormat("<td>{0}</td>", row["ProductName"].ToString());
                    builder.AppendFormat("<td style='text-align:center;'>{0}</td>", row["quantity"].ToString());
                    builder.AppendFormat("<td style='text-align:right;'>{0}</td></tr>", Convert.ToDecimal(row["price"]).ToString("F2"));
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
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>半价数量：</span>{0}</div>", halfCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>半价减免：</span>{0}</div>", Convert.ToDouble(halfPrice));
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;padding-bottom:3px;'><span>优惠券数量：</span>{0}</div>", couponCount);
                builder.AppendFormat("<div style='width:50%;float:left;font-size:14px;text-align:right;padding-bottom:3px;'><span>优惠券减免：</span>{0}</div>", Convert.ToDouble(couponTotalPrice));
                builder.Append("</div>");
                litstorename.Text = builder.ToString();
            }
            catch (Exception ex)
            {
                string backjson = string.Format("\"success\":true,\"errmsg\":\"{0}\"", ex.Message);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vTodayOrder.html";
            }
            base.OnInit(e);
        }

    }
}

