using System.Linq;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Web.UI;
namespace Hidistro.UI.Web.Pay
{
	public class wx_Submit : System.Web.UI.Page
	{
		public string pay_json = "";
        public string orderId = "";
        public string uid = "";

		public string ConvertPayJson(PayRequestInfo req)
		{
			string packageValue = "{";
			packageValue = packageValue + " \"appId\": \"" + req.appId + "\", ";
			packageValue = packageValue + " \"timeStamp\": \"" + req.timeStamp + "\", ";
			packageValue = packageValue + " \"nonceStr\": \"" + req.nonceStr + "\", ";
			packageValue = packageValue + " \"package\": \"" + req.package + "\", ";
			packageValue += " \"signType\": \"MD5\", ";
			return packageValue + " \"paySign\": \"" + req.paySign + "\"}";
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string str = base.Request.QueryString.Get("orderId");
			if (!string.IsNullOrEmpty(str))
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
				if (orderInfo != null)
				{
                    string itemDescription = orderInfo.OrderId;
                    if (orderInfo.LineItems.Count() > 0)
                    {
                        itemDescription = ((LineItemInfo)orderInfo.LineItems.First().Value).ItemDescription;
                        if (itemDescription.Length > 30)
                            itemDescription = itemDescription.Substring(0, 30) + "...";
                        else if (orderInfo.LineItems.Count() > 1)
                            itemDescription = itemDescription + "...";
                    }
                    //itemDescription = (Globals.GetCurrentMemberUserId() == 134) ? "商品" : orderInfo.OrderId;
					PackageInfo package = new PackageInfo
					{
                        //Body = orderInfo.OrderId,//商品
                        Body = itemDescription,
						NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host),
                        OutTradeNo = orderInfo.OrderId,//商户单号
						TotalFee = (int)(orderInfo.GetTotal() * 100m)
					};
                    this.orderId = package.OutTradeNo;
					if (package.TotalFee < 1m)
					{
						package.TotalFee = 1m;
					}
					string openId = "";
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					if (currentMember != null)
					{
						openId = currentMember.OpenId;
					}
                    this.uid = currentMember.UserId.ToString();
					package.OpenId = openId;
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					PayRequestInfo req = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey).BuildPayRequest(package);
					this.pay_json = this.ConvertPayJson(req);
				}
			}
		}
	}
}
