namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_PaymentTypeSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
            StringBuilder builder = new StringBuilder();
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsProLa)
            {
                builder.Append("<button  class=\"choose\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
            }
            else
            {
                builder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
            }
            
            builder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
            if (MemberProcessor.GetCurrentMember().RegSource == 0)
            {
                if (SettingsManager.GetMasterSettings(false).EnableWeiXinRequest)
                {
                    builder.AppendLine("<li><a href=\"#\" name=\"88\">微信支付</a></li>");
                }

                if (SettingsManager.GetMasterSettings(false).EnableOffLineRequest)
                {
                    builder.AppendLine("<li><a href=\"#\" name=\"99\">线下付款</a></li>");
                }
                if (SettingsManager.GetMasterSettings(false).EnablePodRequest)
                {
                    builder.AppendLine("<li><a href=\"#\" name=\"0\">货到付款</a></li>");
                }
                if ((paymentModes != null) && (paymentModes.Count > 0))
                {
                    foreach (PaymentModeInfo info in paymentModes)
                    {
                        string xml = HiCryptographer.Decrypt(info.Settings);
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(xml);
                        if (document.GetElementsByTagName("Partner").Count != 0)
                        {
                            if ((!string.IsNullOrEmpty(document.GetElementsByTagName("Partner")[0].InnerText) && !string.IsNullOrEmpty(document.GetElementsByTagName("Key")[0].InnerText)))// && !string.IsNullOrEmpty(document.GetElementsByTagName("Seller_account_name")[0].InnerText)
                            {
                                builder.AppendFormat("<li><a href=\"#\" name=\"{0}\">{1}</a></li>", info.ModeId, info.Name).AppendLine();
                            }
                        }
                        else if ((SettingsManager.GetMasterSettings(false).EnableWapShengPay && !string.IsNullOrEmpty(document.GetElementsByTagName("SenderId")[0].InnerText)) && !string.IsNullOrEmpty(document.GetElementsByTagName("SellerKey")[0].InnerText))
                        {
                            builder.AppendFormat("<li><a href=\"#\" name=\"{0}\">{1}</a></li>", info.ModeId, info.Name).AppendLine();
                        }
                    }
                }
                builder.AppendLine("</ul>");
                writer.Write(builder.ToString());
            }
            else
            {
                

                if (SettingsManager.GetMasterSettings(false).EnableAliOHOffLinePay)
                {
                    builder.AppendLine("<li><a href=\"#\" name=\"99\">线下付款</a></li>");
                }
                if (SettingsManager.GetMasterSettings(false).EnableAliOHPodPay)
                {
                    builder.AppendLine("<li><a href=\"#\" name=\"0\">货到付款</a></li>");
                }
                if (SettingsManager.GetMasterSettings(false).EnableAliOHAliPay)
                {
                    PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
                    string xml = HiCryptographer.Decrypt(paymentMode.Settings);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    if (document.GetElementsByTagName("Partner").Count != 0)
                    {
                        if ((!string.IsNullOrEmpty(document.GetElementsByTagName("Partner")[0].InnerText) && !string.IsNullOrEmpty(document.GetElementsByTagName("Key")[0].InnerText)))// && !string.IsNullOrEmpty(document.GetElementsByTagName("Seller_account_name")[0].InnerText)
                        {
                            builder.AppendFormat("<li><a href=\"#\" name=\"{0}\">{1}</a></li>", paymentMode.ModeId, paymentMode.Name).AppendLine();
                        }
                    }
                }
                
                builder.AppendLine("</ul>");
                writer.Write(builder.ToString());
            }
        }
    }
}

