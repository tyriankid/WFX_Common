using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin.sales
{
	public class Print : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected string height = "";
		protected string mailNo = "";
		protected string orderIds = "";
		protected string templateName = "";
		protected string UpdateOrderIds = string.Empty;
		protected string width = "";
		private decimal CalculateOrderTotal(System.Data.DataRow order, System.Data.DataSet ds)
		{
			decimal result = 0m;
			decimal num2 = 0m;
			decimal num3 = 0m;
			decimal num4 = 0m;
			bool flag = false;
			decimal.TryParse(order["AdjustedFreight"].ToString(), out result);
			decimal.TryParse(order["PayCharge"].ToString(), out num2);
			string str = order["CouponCode"].ToString();
			decimal.TryParse(order["CouponValue"].ToString(), out num3);
			decimal.TryParse(order["AdjustedDiscount"].ToString(), out num4);
			bool.TryParse(order["OptionPrice"].ToString(), out flag);
			System.Data.DataRow[] orderGift = null;
			System.Data.DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
			decimal num5 = this.GetAmount(orderGift, orderLine, order) + result;
			num5 += num2;
			if (!string.IsNullOrEmpty(str))
			{
				num5 -= num3;
			}
			return num5 + num4;
		}
		public decimal GetAmount(System.Data.DataRow[] orderGift, System.Data.DataRow[] orderLine, System.Data.DataRow order)
		{
			return this.GetGoodDiscountAmount(order, orderLine);
		}
		public decimal GetGiftAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow row = rows[i];
				num += decimal.Parse(row["CostPrice"].ToString());
			}
			return num;
		}
		public decimal GetGoodDiscountAmount(System.Data.DataRow order, System.Data.DataRow[] orderLine)
		{
			decimal result = 0m;
			decimal.TryParse(order["DiscountAmount"].ToString(), out result);
			decimal goodsAmount = this.GetGoodsAmount(orderLine);
			order["ReducedPromotionName"].ToString();
			if (order["ReducedPromotionAmount"] != System.DBNull.Value)
			{
				goodsAmount = System.Convert.ToDecimal(order["ReducedPromotionAmount"]);
			}
			return goodsAmount;
		}
		public decimal GetGoodsAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow row = rows[i];
				num += decimal.Parse(row["ItemAdjustedPrice"].ToString()) * int.Parse(row["Quantity"].ToString());
			}
			return num;
		}
		private System.Data.DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return OrderHelper.GetOrdersAndLines(orderIds);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.mailNo = base.Request["mailNo"];
				int shipperId = int.Parse(base.Request["shipperId"]);
				this.orderIds = base.Request["orderIds"].Trim(new char[]
				{
					','
				});
				string path = System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
				if (System.IO.File.Exists(path))
				{
					XmlDocument document = new XmlDocument();
					document.Load(path);
					XmlNode node = document.DocumentElement.SelectSingleNode("//printer");
					this.templateName = node.SelectSingleNode("kind").InnerText;
					string innerText = node.SelectSingleNode("pic").InnerText;
					string str3 = node.SelectSingleNode("size").InnerText;
					this.width = str3.Split(new char[]
					{
						':'
					})[0];
					this.height = str3.Split(new char[]
					{
						':'
					})[1];
					System.Data.DataSet printData = this.GetPrintData(this.orderIds);
					int num2 = 0;
					foreach (System.Data.DataRow row in printData.Tables[0].Rows)
					{
						this.UpdateOrderIds = this.UpdateOrderIds + row["orderid"] + ",";
						System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
						if (!string.IsNullOrEmpty(innerText) && innerText != "noimage")
						{
							using (Image image = Image.FromFile(System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", innerText))))
							{
								child.Attributes["style"] = string.Format("background-image: url(../../Storage/master/flex/{0}); width: {1}px; height: {2}px;text-align: center; position: relative;", innerText, image.Width, image.Height);
							}
						}
						System.Data.DataTable table = printData.Tables[1];
						ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
						string str4 = (row["shippingRegion"] != null) ? row["shippingRegion"].ToString().Replace("，", " ").Replace(",", " ") : "";
						char ch = ' ';
						string[] strArray = str4.Split(new char[]
						{
							ch
						});
						foreach (XmlNode node2 in node.SelectNodes("item"))
						{
							System.Text.StringBuilder builder = new System.Text.StringBuilder(node2.SelectSingleNode("name").InnerText);
							builder.Replace("收货人-姓名", row["ShipTo"].ToString());
							builder.Replace("收货人-电话", row["TelPhone"].ToString());
							builder.Replace("收货人-手机", row["CellPhone"].ToString());
							builder.Replace("收货人-邮编", row["ZipCode"].ToString());
							builder.Replace("收货人-地址", row["Address"].ToString());
							string newValue = string.Empty;
							if (strArray.Length > 0)
							{
								newValue = strArray[0];
							}
							builder.Replace("收货人-地区1级", newValue);
							newValue = string.Empty;
							if (strArray.Length > 1)
							{
								newValue = strArray[1];
							}
							builder.Replace("收货人-地区2级", newValue);
							newValue = string.Empty;
							if (strArray.Length > 2)
							{
								newValue = strArray[2];
							}
							builder.Replace("收货人-地区3级", newValue);
							string[] strArray2 = new string[]
							{
								"",
								"",
								""
							};
							if (shipper != null)
							{
								strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[]
								{
									'-'
								});
							}
							builder.Replace("发货人-姓名", (shipper != null) ? shipper.ShipperName : "");
							builder.Replace("发货人-手机", (shipper != null) ? shipper.CellPhone : "");
							builder.Replace("发货人-电话", (shipper != null) ? shipper.TelPhone : "");
							builder.Replace("发货人-地址", (shipper != null) ? shipper.Address : "");
							builder.Replace("发货人-邮编", (shipper != null) ? shipper.Zipcode : "");
							string str5 = string.Empty;
							if (strArray2.Length > 0)
							{
								str5 = strArray2[0];
							}
							builder.Replace("发货人-地区1级", str5);
							str5 = string.Empty;
							if (strArray2.Length > 1)
							{
								str5 = strArray2[1];
							}
							builder.Replace("发货人-地区2级", str5);
							str5 = string.Empty;
							if (strArray2.Length > 2)
							{
								str5 = strArray2[2];
							}
							builder.Replace("发货人-地区3级", str5);
							builder.Replace("订单-订单号", "订单号：" + row["OrderId"].ToString());
							builder.Replace("订单-总金额", decimal.Parse(row["OrderTotal"].ToString()).ToString("F2"));
							builder.Replace("订单-物品总重量", row["Weight"].ToString());
							builder.Replace("订单-备注", row["Remark"].ToString());
							System.Data.DataRow[] rowArray = table.Select(" OrderId='" + row["OrderId"] + "'");
							string str6 = string.Empty;
							if (rowArray.Length > 0)
							{
								System.Data.DataRow[] array = rowArray;
								for (int i = 0; i < array.Length; i++)
								{
									System.Data.DataRow row2 = array[i];
									str6 = string.Concat(new object[]
									{
										str6,
										"规格 ",
										row2["SKUContent"],
										" ×",
										row2["ShipmentQuantity"],
										"\n货号 :",
										row2["SKU"]
									});
								}
								str6 = str6.Replace("；", "");
							}
							builder.Replace("订单-详情", str6);
							builder.Replace("订单-送货时间", "");
							builder.Replace("网店名称", SettingsManager.GetMasterSettings(true).SiteName);
							builder.Replace("自定义内容", "");
							string str7 = builder.ToString();
							string str8 = node2.SelectSingleNode("font").InnerText;
							string text = node2.SelectSingleNode("fontsize").InnerText;
							string str9 = node2.SelectSingleNode("position").InnerText;
							string str10 = node2.SelectSingleNode("align").InnerText;
							string str11 = str9.Split(new char[]
							{
								':'
							})[0];
							string str12 = str9.Split(new char[]
							{
								':'
							})[1];
							string str13 = str9.Split(new char[]
							{
								':'
							})[2];
							string str14 = str9.Split(new char[]
							{
								':'
							})[3];
							System.Web.UI.HtmlControls.HtmlGenericControl control2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div")
							{
								Visible = true,
								InnerText = str7.Split(new char[]
								{
									'_'
								})[0]
							};
							control2.Style["font-family"] = str8;
							control2.Style["font-size"] = "16px";
							control2.Style["width"] = str11 + "px";
							control2.Style["height"] = str12 + "px";
							control2.Style["text-align"] = str10;
							control2.Style["position"] = "absolute";
							control2.Style["left"] = str13 + "px";
							control2.Style["top"] = str14 + "px";
							control2.Style["padding"] = "0";
							control2.Style["margin-left"] = "0px";
							control2.Style["margin-top"] = "0px";
							child.Controls.Add(control2);
						}
						this.divContent.Controls.Add(child);
						num2++;
						if (num2 < printData.Tables[0].Rows.Count)
						{
							System.Web.UI.HtmlControls.HtmlGenericControl control3 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
							control3.Attributes["class"] = "PageNext";
							this.divContent.Controls.Add(control3);
						}
					}
					this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(new char[]
					{
						','
					});
				}
			}
		}
	}
}
