namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class SiteSettings
    {
        public SiteSettings(string siteUrl)
        {
            this.SiteUrl = siteUrl;
            this.IsOpenSiteSale = true;
            this.Disabled = false;
            this.SiteDescription = "最安全，最专业的网上商店系统";
            this.Theme = "default";
            this.VTheme = "default";
            this.AliOHTheme = "default";
            this.WapTheme = "default";
            this.SiteName = "Hishop";
            this.LogoUrl = "/utility/pics/logo.jpg";
            this.DefaultProductImage = "/utility/pics/none.gif";
            this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
            this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
            this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
            this.DecimalLength = 2;
            this.PointsRate = 1M;
            this.OrderShowDays = 7;
            this.CloseOrderDays = 3;
            this.FinishOrderDays = 7;
            this.EndOrderDays = 7;
            this.IsOpenSiteSale = true;
            this.EnableMobileClient = false;
            this.EnablePodRequest = true;
            this.ServiceStatus = 1;
            this.OpenAliho = 0;
            this.OpenTaobao = 1;
            this.OpenMobbile = 0;
            this.OpenVstore = 0;
            this.OpenWap = 0;
            this.OpenReferral = 0;
            this.IsAuditReferral = true;
            this.OpenManyService = false;
            this.FlowReferralDeduct = 0M;
            this.RegReferralDeduct = 0M;
            this.AccountRate = 2M;
            this.FlowIpRate = 2M;
            this.RegIpRate = 2M;
            this.DeductMinDraw = 50M;
            this.ServiceCoordinate = "120";
            this.ServicePosition = 1;
            this.ServiceIsOpen = "1";
            this.ValueAddedServiceCoordinate = "90";
            this.ValueAddedServicePosition = 4;
            this.ValueAddedServiceSiteId = string.Empty;
            this.ValueAddedServiceSettingId = string.Empty;
            this.ValueAddedServiceIsOpen = "0";
            this.EnableVshopReferral = false;
            this.EnableBankUnionPay = false;
            this.EnableAliOHBankUnionPay = false;
            this.EnableWapBankUnionPay = false;
            this.EnableAPPBankUnionPay = false;
            this.WeixinCertPath = "";
            this.WeixinCertPassword = "";
            this.IOSStartImg = "";
            this.AndroidStartImg = "";
            this.OpenMultStore = false;
            this.StoreShipPostFee = 0M;
            this.StoreShipDesctiption = "";
            this.StoreStockValidateType = 1;
            this.AppSignInPoints = 0;
            this.AppDepletePoints = 0;
            this.AppFirstPrizeType = 1;
            this.AppFirstPrizeCouponId = 0;
            this.AppFirstPrizePoints = 0;
            this.AppFirstPrizePercent = 0;
            this.AppSecondPrizeType = 1;
            this.AppSecondPrizeCouponId = 0;
            this.AppSecondPrizePoints = 0;
            this.AppSecondPrizePercent = 0;
            this.AppThirdPrizeType = 1;
            this.AppThirdPrizeCouponId = 0;
            this.AppThirdPrizePoints = 0;
            this.AppThirdPrizePercent = 0;
            this.AppFourPrizeType = 1;
            this.AppFourPrizeCouponId = 0;
            this.AppFourPrizePoints = 0;
            this.AppFourPrizePercent = 0;
        }

        public static SiteSettings FromXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("Settings");
            return new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText) { 
                AssistantIv = node.SelectSingleNode("AssistantIv").InnerText, AssistantKey = node.SelectSingleNode("AssistantKey").InnerText, DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText), DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText, DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText, DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText, DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText, DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText, DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText, DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText, DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText, DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText, CheckCode = node.SelectSingleNode("CheckCode").InnerText, IsOpenSiteSale = bool.Parse(node.SelectSingleNode("IsOpenSiteSale").InnerText), Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText), ReferralDeduct = decimal.Parse(node.SelectSingleNode("ReferralDeduct").InnerText), 
                SubMemberDeduct = decimal.Parse(node.SelectSingleNode("SubMemberDeduct").InnerText), SubReferralDeduct = decimal.Parse(node.SelectSingleNode("SubReferralDeduct").InnerText), ReferralIntroduction = node.SelectSingleNode("ReferralIntroduction").InnerText, IsAuditReferral = bool.Parse(node.SelectSingleNode("IsAuditReferral").InnerText), EtaoID = node.SelectSingleNode("EtaoID").InnerText, IsCreateFeed = bool.Parse(node.SelectSingleNode("IsCreateFeed").InnerText), Footer = node.SelectSingleNode("Footer").InnerText, RegisterAgreement = node.SelectSingleNode("RegisterAgreement").InnerText, HtmlOnlineServiceCode = node.SelectSingleNode("HtmlOnlineServiceCode").InnerText, LogoUrl = node.SelectSingleNode("LogoUrl").InnerText, OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText), CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText), FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText), EndOrderDays = int.Parse(node.SelectSingleNode("EndOrderDays").InnerText), TaxRate = decimal.Parse(node.SelectSingleNode("TaxRate").InnerText), PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText), 
                SearchMetaDescription = node.SelectSingleNode("SearchMetaDescription").InnerText, SearchMetaKeywords = node.SelectSingleNode("SearchMetaKeywords").InnerText, SiteDescription = node.SelectSingleNode("SiteDescription").InnerText, SiteName = node.SelectSingleNode("SiteName").InnerText, SiteUrl = node.SelectSingleNode("SiteUrl").InnerText, UserId = null, Theme = node.SelectSingleNode("Theme").InnerText, YourPriceName = node.SelectSingleNode("YourPriceName").InnerText, EmailSender = node.SelectSingleNode("EmailSender").InnerText, EmailSettings = node.SelectSingleNode("EmailSettings").InnerText, SMSSender = node.SelectSingleNode("SMSSender").InnerText, SMSSettings = node.SelectSingleNode("SMSSettings").InnerText, SiteMapTime = node.SelectSingleNode("SiteMapTime").InnerText, SiteMapNum = node.SelectSingleNode(" SiteMapNum").InnerText, TaobaoShippingType = int.Parse(node.SelectSingleNode("TaobaoShippingType").InnerText), EnabledBFD = bool.Parse(node.SelectSingleNode("EnabledBFD").InnerText), 
                BFDUserName = node.SelectSingleNode("BFDUserName").InnerText, EnabledCnzz = bool.Parse(node.SelectSingleNode("EnabledCnzz").InnerText), CnzzUsername = node.SelectSingleNode("CnzzUsername").InnerText, CnzzPassword = node.SelectSingleNode("CnzzPassword").InnerText, EnableMobileClient = bool.Parse(node.SelectSingleNode("EnableMobileClient").InnerText), MobileClientSpread = node.SelectSingleNode("MobileClientSpread").InnerText, CellPhoneUserCode = node.SelectSingleNode("CellPhoneUserCode").InnerText, CellPhoneToken = node.SelectSingleNode("CellPhoneToken").InnerText, ApplicationMark = node.SelectSingleNode("ApplicationMark").InnerText, SiteToken = node.SelectSingleNode("SiteToken").InnerText, SiteTime = node.SelectSingleNode("SiteTime").InnerText, WeixinAppId = node.SelectSingleNode("WeixinAppId").InnerText, WeixinAppSecret = node.SelectSingleNode("WeixinAppSecret").InnerText, WeixinToken = node.SelectSingleNode("WeixinToken").InnerText, WeixinPaySignKey = node.SelectSingleNode("WeixinPaySignKey").InnerText, WeixinPartnerID = node.SelectSingleNode("WeixinPartnerID").InnerText, 
                WeixinPartnerKey = node.SelectSingleNode("WeixinPartnerKey").InnerText, VTheme = node.SelectSingleNode("VTheme").InnerText, VipCardLogo = node.SelectSingleNode("VipCardLogo").InnerText, VipCardBG = node.SelectSingleNode("VipCardBG").InnerText, VipCardQR = node.SelectSingleNode("VipCardQR").InnerText, VipCardName = node.SelectSingleNode("VipCardName").InnerText, VipCardPrefix = node.SelectSingleNode("VipCardPrefix").InnerText, VipRequireName = bool.Parse(node.SelectSingleNode("VipRequireName").InnerText), VipRequireMobile = bool.Parse(node.SelectSingleNode("VipRequireMobile").InnerText), VipRequireAdress = bool.Parse(node.SelectSingleNode("VipRequireAdress").InnerText), VipRequireQQ = bool.Parse(node.SelectSingleNode("VipRequireQQ").InnerText), VipEnableCoupon = bool.Parse(node.SelectSingleNode("VipEnableCoupon").InnerText), EnablePodRequest = bool.Parse(node.SelectSingleNode("EnablePodRequest").InnerText), EnableAppOffLinePay = bool.Parse(node.SelectSingleNode("EnableAppOffLinePay").InnerText), EnableAppPodPay = bool.Parse(node.SelectSingleNode("EnableAppPodPay").InnerText), EnableAppAliPay = bool.Parse(node.SelectSingleNode("EnableAppAliPay").InnerText), 
                EnableAppWapAliPay = bool.Parse(node.SelectSingleNode("EnableAppWapAliPay").InnerText), EnableWapAliPay = bool.Parse(node.SelectSingleNode("EnableWapAliPay").InnerText), EnableBankUnionPay = bool.Parse(node.SelectSingleNode("EnableBankUnionPay").InnerText), EnableWapBankUnionPay = bool.Parse(node.SelectSingleNode("EnableWapBankUnionPay").InnerText), EnableAliOHBankUnionPay = bool.Parse(node.SelectSingleNode("EnableAliOHBankUnionPay").InnerText), EnableAPPBankUnionPay = bool.Parse(node.SelectSingleNode("EnableAppBankUnionPay").InnerText), IsValidationService = bool.Parse(node.SelectSingleNode("IsValidationService").InnerText), EnableWeiXinRequest = bool.Parse(node.SelectSingleNode("EnableWeiXinRequest").InnerText), EnableOffLineRequest = bool.Parse(node.SelectSingleNode("EnableOffLineRequest").InnerText), EnableWeixinWapAliPay = bool.Parse(node.SelectSingleNode("EnableWeixinWapAliPay").InnerText), WeixinNumber = node.SelectSingleNode("WeixinNumber").InnerText, WeixinLoginUrl = node.SelectSingleNode("WeixinLoginUrl").InnerText, WeiXinCodeImageUrl = node.SelectSingleNode("WeiXinCodeImageUrl").InnerText, OffLinePayContent = node.SelectSingleNode("OffLinePayContent").InnerText, WapTheme = node.SelectSingleNode("WapTheme").InnerText, EnableWapOffLinePay = bool.Parse(node.SelectSingleNode("EnableWapOffLinePay").InnerText), 
                EnableWapPodPay = bool.Parse(node.SelectSingleNode("EnableWapPodPay").InnerText), AliOHAppId = node.SelectSingleNode("AliOHAppId").InnerText, EnableAliOHAliPay = bool.Parse(node.SelectSingleNode("EnableAliOHAliPay").InnerText), AliOHTheme = node.SelectSingleNode("AliOHTheme").InnerText, EnableAliOHOffLinePay = bool.Parse(node.SelectSingleNode("EnableAliOHOffLinePay").InnerText), EnableAliOHPodPay = bool.Parse(node.SelectSingleNode("EnableAliOHPodPay").InnerText), AliOHFollowRelay = node.SelectSingleNode("AliOHFollowRelay").InnerText, AliOHServerUrl = node.SelectSingleNode("AliOHServerUrl").InnerText, EnableVshopShengPay = bool.Parse(node.SelectSingleNode("EnableVshopShengPay").InnerText), EnableWapShengPay = bool.Parse(node.SelectSingleNode("EnableWapShengPay").InnerText), EnableAppShengPay = bool.Parse(node.SelectSingleNode("EnableAppShengPay").InnerText), EnableAliOHShengPay = bool.Parse(node.SelectSingleNode("EnableAliOHShengPay").InnerText), AliOHFollowRelayTitle = node.SelectSingleNode("AliOHFollowRelayTitle").InnerText, ServiceStatus = int.Parse(node.SelectSingleNode("ServiceStatus").InnerText), OpenVstore = int.Parse(node.SelectSingleNode("OpenVstore").InnerText), OpenAliho = int.Parse(node.SelectSingleNode("OpenAliho").InnerText), 
                OpenTaobao = int.Parse(node.SelectSingleNode("OpenTaobao").InnerText), OpenWap = int.Parse(node.SelectSingleNode("OpenWap").InnerText), OpenMobbile = int.Parse(node.SelectSingleNode("OpenMobbile").InnerText), OpenReferral = int.Parse(node.SelectSingleNode("OpenReferral").InnerText), OpenManyService = bool.Parse(node.SelectSingleNode("OpenManyService").InnerText), FlowReferralDeduct = decimal.Parse(node.SelectSingleNode("FlowReferralDeduct").InnerText), RegReferralDeduct = decimal.Parse(node.SelectSingleNode("RegReferralDeduct").InnerText), AccountRate = decimal.Parse(node.SelectSingleNode("AccountRate").InnerText), FlowIpRate = decimal.Parse(node.SelectSingleNode("FlowIpRate").InnerText), RegIpRate = decimal.Parse(node.SelectSingleNode("RegIpRate").InnerText), DeductMinDraw = decimal.Parse(node.SelectSingleNode("DeductMinDraw").InnerText), ServicePosition = int.Parse(node.SelectSingleNode("ServicePosition").InnerText), ServiceCoordinate = node.SelectSingleNode("ServiceCoordinate").InnerText, ServiceIsOpen = node.SelectSingleNode("ServiceIsOpen").InnerText, ValueAddedServicePosition = int.Parse(node.SelectSingleNode("ValueAddedServicePosition").InnerText), ValueAddedServiceCoordinate = node.SelectSingleNode("ValueAddedServiceCoordinate").InnerText, 
                ValueAddedServiceSiteId = node.SelectSingleNode("ValueAddedServiceSiteId").InnerText, ValueAddedServiceSettingId = node.SelectSingleNode("ValueAddedServiceSettingId").InnerText, ValueAddedServiceIsOpen = node.SelectSingleNode("ValueAddedServiceIsOpen").InnerText, EnableVshopReferral = bool.Parse(node.SelectSingleNode("EnableVshopReferral").InnerText), WeixinCertPath = node.SelectSingleNode("WeixinCertPath").InnerText, WeixinCertPassword = node.SelectSingleNode("WeixinCertPassword").InnerText, AndroidStartImg = node.SelectSingleNode("AndroidStartImg").InnerText.Trim(), IOSStartImg = node.SelectSingleNode("IOSStartImg").InnerText.Trim(), OpenMultStore = (node.SelectSingleNode("OpenMultStore").InnerText.Trim() == "1") ? true : false, StoreShipPostFee = node.SelectSingleNode("StoreShipPostFee").InnerText.ToDecimal(), StoreShipDesctiption = node.SelectSingleNode("StoreShipDesctiption").InnerText.Trim(), StoreStockValidateType = node.SelectSingleNode("StoreStockValidateType").InnerText.Trim().ToInt(), AppSignInPoints = node.SelectSingleNode("AppSignInPoints").InnerText.Trim().ToInt(), AppDepletePoints = node.SelectSingleNode("AppDepletePoints").InnerText.Trim().ToInt(), AppFirstPrizeType = node.SelectSingleNode("AppFirstPrizeType").InnerText.Trim().ToInt(), AppFirstPrizePoints = node.SelectSingleNode("AppFirstPrizePoints").InnerText.Trim().ToInt(), 
                AppFirstPrizeCouponId = node.SelectSingleNode("AppFirstPrizeCouponId").InnerText.Trim().ToInt(), AppSecondPrizeType = node.SelectSingleNode("AppSecondPrizeType").InnerText.Trim().ToInt(), AppSecondPrizePoints = node.SelectSingleNode("AppSecondPrizePoints").InnerText.Trim().ToInt(), AppSecondPrizeCouponId = node.SelectSingleNode("AppSecondPrizeCouponId").InnerText.Trim().ToInt(), AppThirdPrizeType = node.SelectSingleNode("AppThirdPrizeType").InnerText.Trim().ToInt(), AppThirdPrizePoints = node.SelectSingleNode("AppThirdPrizePoints").InnerText.Trim().ToInt(), AppThirdPrizeCouponId = node.SelectSingleNode("AppThirdPrizeCouponId").InnerText.Trim().ToInt(), AppFourPrizeType = node.SelectSingleNode("AppFourPrizeType").InnerText.Trim().ToInt(), AppFourPrizePoints = node.SelectSingleNode("AppFourPrizePoints").InnerText.Trim().ToInt(), AppFourPrizeCouponId = node.SelectSingleNode("AppFourPrizeCouponId").InnerText.Trim().ToInt(), AppFirstPrizePercent = node.SelectSingleNode("AppFirstPrizePercent").InnerText.Trim().ToInt(), AppSecondPrizePercent = node.SelectSingleNode("AppSecondPrizePercent").InnerText.Trim().ToInt(), AppThirdPrizePercent = node.SelectSingleNode("AppThirdPrizePercent").InnerText.Trim().ToInt(), AppFourPrizePercent = node.SelectSingleNode("AppFourPrizePercent").InnerText.Trim().ToInt()
             };
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
            SetNodeValue(doc, root, "AssistantIv", this.AssistantIv);
            SetNodeValue(doc, root, "AssistantKey", this.AssistantKey);
            SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
            SetNodeValue(doc, root, "CheckCode", this.CheckCode);
            SetNodeValue(doc, root, "IsOpenSiteSale", this.IsOpenSiteSale ? "true" : "false");
            SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
            SetNodeValue(doc, root, "ReferralDeduct", this.ReferralDeduct.ToString("F2"));
            SetNodeValue(doc, root, "SubMemberDeduct", this.SubMemberDeduct.ToString("F2"));
            SetNodeValue(doc, root, "SubReferralDeduct", this.SubReferralDeduct.ToString("F2"));
            SetNodeValue(doc, root, "ReferralIntroduction", this.ReferralIntroduction);
            SetNodeValue(doc, root, "IsAuditReferral", this.IsAuditReferral ? "true" : "false");
            SetNodeValue(doc, root, "EtaoID", this.EtaoID);
            SetNodeValue(doc, root, "IsCreateFeed", this.IsCreateFeed ? "true" : "false");
            SetNodeValue(doc, root, "Footer", this.Footer);
            SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
            SetNodeValue(doc, root, "HtmlOnlineServiceCode", this.HtmlOnlineServiceCode);
            SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "EndOrderDays", this.EndOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
            SetNodeValue(doc, root, "SearchMetaDescription", this.SearchMetaDescription);
            SetNodeValue(doc, root, "SearchMetaKeywords", this.SearchMetaKeywords);
            SetNodeValue(doc, root, "SiteDescription", this.SiteDescription);
            SetNodeValue(doc, root, "SiteName", this.SiteName);
            SetNodeValue(doc, root, "Theme", this.Theme);
            SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
            SetNodeValue(doc, root, "EmailSender", this.EmailSender);
            SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
            SetNodeValue(doc, root, "SMSSender", this.SMSSender);
            SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
            SetNodeValue(doc, root, "SiteMapNum", this.SiteMapNum);
            SetNodeValue(doc, root, "TaobaoShippingType", this.TaobaoShippingType.ToString());
            SetNodeValue(doc, root, "SiteMapTime", this.SiteMapTime);
            SetNodeValue(doc, root, "EnabledBFD", this.EnabledBFD ? "true" : "false");
            SetNodeValue(doc, root, "BFDUserName", this.BFDUserName);
            SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
            SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
            SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
            SetNodeValue(doc, root, "EnableMobileClient", this.EnableMobileClient ? "true" : "false");
            SetNodeValue(doc, root, "MobileClientSpread", this.MobileClientSpread);
            SetNodeValue(doc, root, "CellPhoneUserCode", this.CellPhoneUserCode);
            SetNodeValue(doc, root, "CellPhoneToken", this.CellPhoneToken);
            SetNodeValue(doc, root, "ApplicationMark", this.ApplicationMark);
            SetNodeValue(doc, root, "SiteToken", this.SiteToken);
            SetNodeValue(doc, root, "SiteTime", this.SiteTime);
            SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
            SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
            SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
            SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
            SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
            SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
            SetNodeValue(doc, root, "VTheme", this.VTheme);
            SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
            SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
            SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
            SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
            SetNodeValue(doc, root, "VipCardName", this.VipCardName);
            SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
            SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
            SetNodeValue(doc, root, "VipRemark", this.VipRemark);
            SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWeixinWapAliPay", this.EnableWeixinWapAliPay ? "true" : "false");
            SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
            SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
            SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
            SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
            SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
            SetNodeValue(doc, root, "EnableAppOffLinePay", this.EnableAppOffLinePay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAppPodPay", this.EnableAppPodPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAppAliPay", this.EnableAppAliPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAppWapAliPay", this.EnableAppWapAliPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapAliPay", this.EnableWapAliPay ? "true" : "false");
            SetNodeValue(doc, root, "WapTheme", this.WapTheme);
            SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapOffLinePay", this.EnableWapOffLinePay ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapPodPay", this.EnableWapPodPay ? "true" : "false");
            SetNodeValue(doc, root, "AliOHAppId", this.AliOHAppId);
            SetNodeValue(doc, root, "EnableAliOHOffLinePay", this.EnableAliOHOffLinePay ? "true" : "false");
            SetNodeValue(doc, root, "AliOHTheme", this.AliOHTheme);
            SetNodeValue(doc, root, "EnableAliOHAliPay", this.EnableAliOHAliPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAliOHPodPay", this.EnableAliOHPodPay ? "true" : "false");
            SetNodeValue(doc, root, "AliOHFollowRelay", this.AliOHFollowRelay);
            SetNodeValue(doc, root, "AliOHFollowRelayTitle", this.AliOHFollowRelayTitle);
            SetNodeValue(doc, root, "AliOHServerUrl", this.AliOHServerUrl);
            SetNodeValue(doc, root, "EnableVshopShengPay", this.EnableVshopShengPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAppShengPay", this.EnableAppShengPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAliOHShengPay", this.EnableAliOHShengPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableBankUnionPay", this.EnableBankUnionPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAliOHBankUnionPay", this.EnableAliOHBankUnionPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapBankUnionPay", this.EnableWapBankUnionPay ? "true" : "false");
            SetNodeValue(doc, root, "EnableAppBankUnionPay", this.EnableAPPBankUnionPay ? "true" : "false");
            SetNodeValue(doc, root, "ServiceStatus", this.ServiceStatus.ToString());
            SetNodeValue(doc, root, "OpenTaobao", this.OpenTaobao.ToString());
            SetNodeValue(doc, root, "OpenMobbile", this.OpenMobbile.ToString());
            SetNodeValue(doc, root, "OpenAliho", this.OpenAliho.ToString());
            SetNodeValue(doc, root, "OpenVstore", this.OpenVstore.ToString());
            SetNodeValue(doc, root, "OpenWap", this.OpenWap.ToString());
            SetNodeValue(doc, root, "OpenReferral", this.OpenReferral.ToString());
            SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
            SetNodeValue(doc, root, "FlowReferralDeduct", this.FlowReferralDeduct.ToString("f2"));
            SetNodeValue(doc, root, "RegReferralDeduct", this.RegReferralDeduct.ToString("f2"));
            SetNodeValue(doc, root, "AccountRate", this.AccountRate.ToString("f2"));
            SetNodeValue(doc, root, "FlowIpRate", this.FlowIpRate.ToString("f2"));
            SetNodeValue(doc, root, "RegIpRate", this.RegIpRate.ToString("f2"));
            SetNodeValue(doc, root, "DeductMinDraw", this.DeductMinDraw.ToString("f2"));
            SetNodeValue(doc, root, "ServicePosition", this.ServicePosition.ToString());
            SetNodeValue(doc, root, "ServiceCoordinate", this.ServiceCoordinate);
            SetNodeValue(doc, root, "ServiceIsOpen", this.ServiceIsOpen);
            SetNodeValue(doc, root, "ValueAddedServicePosition", this.ValueAddedServicePosition.ToString());
            SetNodeValue(doc, root, "ValueAddedServiceCoordinate", this.ValueAddedServiceCoordinate);
            SetNodeValue(doc, root, "ValueAddedServiceSiteId", this.ValueAddedServiceSiteId);
            SetNodeValue(doc, root, "ValueAddedServiceSettingId", this.ValueAddedServiceSettingId);
            SetNodeValue(doc, root, "ValueAddedServiceIsOpen", this.ValueAddedServiceIsOpen);
            SetNodeValue(doc, root, "EnableVshopReferral", this.EnableVshopReferral.ToString());
            SetNodeValue(doc, root, "EnableBankUnionPay", this.EnableBankUnionPay.ToString());
            SetNodeValue(doc, root, "WeixinCertPath", this.WeixinCertPath);
            SetNodeValue(doc, root, "WeixinCertPassword", this.WeixinCertPassword);
            SetNodeValue(doc, root, "AndroidStartImg", this.AndroidStartImg);
            SetNodeValue(doc, root, "IOSStartImg", this.IOSStartImg);
            SetNodeValue(doc, root, "OpenMultStore", this.OpenMultStore ? "1" : "0");
            SetNodeValue(doc, root, "StoreShipPostFee", this.StoreShipPostFee.ToString("F2"));
            SetNodeValue(doc, root, "StoreShipDesctiption", this.StoreShipDesctiption);
            SetNodeValue(doc, root, "StoreStockValidateType", this.StoreStockValidateType.ToString());
            SetNodeValue(doc, root, "AppSignInPoints", this.AppSignInPoints.ToString());
            SetNodeValue(doc, root, "AppDepletePoints", this.AppDepletePoints.ToString());
            SetNodeValue(doc, root, "AppFirstPrizeType", this.AppFirstPrizeType.ToString());
            SetNodeValue(doc, root, "AppFirstPrizePoints", this.AppFirstPrizePoints.ToString());
            SetNodeValue(doc, root, "AppFirstPrizeCouponId", this.AppFirstPrizeCouponId.ToString());
            SetNodeValue(doc, root, "AppFirstPrizePercent", this.AppFirstPrizePercent.ToString());
            SetNodeValue(doc, root, "AppSecondPrizeType", this.AppSecondPrizeType.ToString());
            SetNodeValue(doc, root, "AppSecondPrizePoints", this.AppSecondPrizePoints.ToString());
            SetNodeValue(doc, root, "AppSecondPrizeCouponId", this.AppSecondPrizeCouponId.ToString());
            SetNodeValue(doc, root, "AppSecondPrizePercent", this.AppSecondPrizePercent.ToString());
            SetNodeValue(doc, root, "AppThirdPrizeType", this.AppThirdPrizeType.ToString());
            SetNodeValue(doc, root, "AppThirdPrizePoints", this.AppThirdPrizePoints.ToString());
            SetNodeValue(doc, root, "AppThirdPrizeCouponId", this.AppThirdPrizeCouponId.ToString());
            SetNodeValue(doc, root, "AppThirdPrizePercent", this.AppThirdPrizePercent.ToString());
            SetNodeValue(doc, root, "AppFourPrizeType", this.AppFourPrizeType.ToString());
            SetNodeValue(doc, root, "AppFourPrizePoints", this.AppFourPrizePoints.ToString());
            SetNodeValue(doc, root, "AppFourPrizeCouponId", this.AppFourPrizeCouponId.ToString());
            SetNodeValue(doc, root, "AppFourPrizePercent", this.AppFourPrizePercent.ToString());
        }

        public decimal AccountRate { get; set; }

        public string AliOHAppId { get; set; }

        public string AliOHFollowRelay { get; set; }

        public string AliOHFollowRelayTitle { get; set; }

        public string AliOHServerUrl { get; set; }

        public string AliOHTheme { get; set; }

        public string AndroidStartImg { get; set; }

        public int AppDepletePoints { get; set; }

        public int AppFirstPrizeCouponId { get; set; }

        public int AppFirstPrizePercent { get; set; }

        public int AppFirstPrizePoints { get; set; }

        public int AppFirstPrizeType { get; set; }

        public int AppFourPrizeCouponId { get; set; }

        public int AppFourPrizePercent { get; set; }

        public int AppFourPrizePoints { get; set; }

        public int AppFourPrizeType { get; set; }

        public string ApplicationMark { get; set; }

        public int AppSecondPrizeCouponId { get; set; }

        public int AppSecondPrizePercent { get; set; }

        public int AppSecondPrizePoints { get; set; }

        public int AppSecondPrizeType { get; set; }

        public int AppSignInPoints { get; set; }

        public int AppThirdPrizeCouponId { get; set; }

        public int AppThirdPrizePercent { get; set; }

        public int AppThirdPrizePoints { get; set; }

        public int AppThirdPrizeType { get; set; }

        public string AssistantIv { get; set; }

        public string AssistantKey { get; set; }

        public string BFDUserName { get; set; }

        public string CellPhoneToken { get; set; }

        public string CellPhoneUserCode { get; set; }

        public string CheckCode { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="过期几天自动关闭订单的天数必须在1-90之间")]
        public int CloseOrderDays { get; set; }

        public string CnzzPassword { get; set; }

        public string CnzzUsername { get; set; }

        public DateTime? CreateDate { get; set; }

        public int DecimalLength { get; set; }

        public decimal DeductMinDraw { get; set; }

        public string DefaultProductImage { get; set; }

        public string DefaultProductThumbnail1 { get; set; }

        public string DefaultProductThumbnail2 { get; set; }

        public string DefaultProductThumbnail3 { get; set; }

        public string DefaultProductThumbnail4 { get; set; }

        public string DefaultProductThumbnail5 { get; set; }

        public string DefaultProductThumbnail6 { get; set; }

        public string DefaultProductThumbnail7 { get; set; }

        public string DefaultProductThumbnail8 { get; set; }

        public bool Disabled { get; set; }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings)) && (this.EmailSender.Trim().Length > 0)) && (this.EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender { get; set; }

        public string EmailSettings { get; set; }

        public bool EnableAliOHAliPay { get; set; }

        public bool EnableAliOHBankUnionPay { get; set; }

        public bool EnableAliOHOffLinePay { get; set; }

        public bool EnableAliOHPodPay { get; set; }

        public bool EnableAliOHShengPay { get; set; }

        public bool EnableAppAliPay { get; set; }

        public bool EnableAPPBankUnionPay { get; set; }

        public bool EnableAppOffLinePay { get; set; }

        public bool EnableAppPodPay { get; set; }

        public bool EnableAppShengPay { get; set; }

        public bool EnableAppWapAliPay { get; set; }

        public bool EnableBankUnionPay { get; set; }

        public bool EnabledBFD { get; set; }

        public bool EnabledCnzz { get; set; }

        public bool EnableMobileClient { get; set; }

        public bool EnableOffLineRequest { get; set; }

        public bool EnablePodRequest { get; set; }

        public bool EnableVshopReferral { get; set; }

        public bool EnableVshopShengPay { get; set; }

        public bool EnableWapAliPay { get; set; }

        public bool EnableWapBankUnionPay { get; set; }

        public bool EnableWapOffLinePay { get; set; }

        public bool EnableWapPodPay { get; set; }

        public bool EnableWapShengPay { get; set; }

        public bool EnableWeiXinRequest { get; set; }

        public bool EnableWeixinWapAliPay { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="订单完成几天自动结束交易的天数必须在1-90之间")]
        public int EndOrderDays { get; set; }

        public DateTime? EtaoApplyTime { get; set; }

        [StringLengthValidator(0, 60, Ruleset="ValMasterSettings", MessageTemplate="一淘账户名称为必填项，长度限制在60字符以内")]
        public string EtaoID { get; set; }

        public int EtaoStatus { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="发货几天自动完成订单的天数必须在1-90之间")]
        public int FinishOrderDays { get; set; }

        public decimal FlowIpRate { get; set; }

        public decimal FlowReferralDeduct { get; set; }

        public string Footer { get; set; }

        [StringLengthValidator(0, 0xfa0, Ruleset="ValMasterSettings", MessageTemplate="网页客服代码长度限制在4000个字符以内")]
        public string HtmlOnlineServiceCode { get; set; }

        public string IOSStartImg { get; set; }

        public bool IsAuditReferral { get; set; }

        public bool IsCreateFeed { get; set; }

        public bool IsOpenEtao { get; set; }

        public bool IsOpenSiteSale { get; set; }

        public bool IsValidationService { get; set; }

        public string LogoUrl { get; set; }

        public string MobileClientSpread { get; set; }

        public string OffLinePayContent { get; set; }

        public int OpenAliho { get; set; }

        public bool OpenManyService { get; set; }

        public int OpenMobbile { get; set; }

        public bool OpenMultStore { get; set; }

        public int OpenReferral { get; set; }

        public int OpenTaobao { get; set; }

        public int OpenVstore { get; set; }

        public int OpenWap { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="最近几天内订单的天数必须在1-90之间")]
        public int OrderShowDays { get; set; }

        [RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="几元一积分必须在0.1-10000000之间")]
        public decimal PointsRate { get; set; }

        public decimal ReferralDeduct { get; set; }

        public string ReferralIntroduction { get; set; }

        public decimal RegIpRate { get; set; }

        public string RegisterAgreement { get; set; }

        public decimal RegReferralDeduct { get; set; }

        public DateTime? RequestDate { get; set; }

        [HtmlCoding, StringLengthValidator(0, 260, Ruleset="ValMasterSettings", MessageTemplate="店铺描述META_DESCRIPTION的长度限制在260字符以内")]
        public string SearchMetaDescription { get; set; }

        [StringLengthValidator(0, 160, Ruleset="ValMasterSettings", MessageTemplate="搜索关键字META_KEYWORDS的长度限制在160字符以内")]
        public string SearchMetaKeywords { get; set; }

        public string ServiceCoordinate { get; set; }

        public string ServiceIsOpen { get; set; }

        public int ServicePosition { get; set; }

        public int ServiceStatus { get; set; }

        [StringLengthValidator(0, 100, Ruleset="ValMasterSettings", MessageTemplate="简单介绍TITLE的长度限制在100字符以内")]
        public string SiteDescription { get; set; }

        public string SiteMapNum { get; set; }

        public string SiteMapTime { get; set; }

        [StringLengthValidator(1, 60, Ruleset="ValMasterSettings", MessageTemplate="店铺名称为必填项，长度限制在60字符以内")]
        public string SiteName { get; set; }

        public string SiteTime { get; set; }

        public string SiteToken { get; set; }

        [StringLengthValidator(1, 0x80, Ruleset="ValMasterSettings", MessageTemplate="店铺域名必须控制在128个字符以内")]
        public string SiteUrl { get; set; }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings)) && (this.SMSSender.Trim().Length > 0)) && (this.SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender { get; set; }

        public string SMSSettings { get; set; }

        public string StoreShipDesctiption { get; set; }

        public decimal StoreShipPostFee { get; set; }

        public int StoreStockValidateType { get; set; }

        public decimal SubMemberDeduct { get; set; }

        public decimal SubReferralDeduct { get; set; }

        public int TaobaoShippingType { get; set; }

        [RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValMasterSettings", MessageTemplate="税率必须在0-100之间")]
        public decimal TaxRate { get; set; }

        public string Theme { get; set; }

        public int? UserId { get; private set; }

        public string ValueAddedServiceCoordinate { get; set; }

        public string ValueAddedServiceIsOpen { get; set; }

        public int ValueAddedServicePosition { get; set; }

        public string ValueAddedServiceSettingId { get; set; }

        public string ValueAddedServiceSiteId { get; set; }

        public string VipCardBG { get; set; }

        public string VipCardLogo { get; set; }

        public string VipCardName { get; set; }

        public string VipCardPrefix { get; set; }

        public string VipCardQR { get; set; }

        public bool VipEnableCoupon { get; set; }

        public string VipRemark { get; set; }

        public bool VipRequireAdress { get; set; }

        public bool VipRequireMobile { get; set; }

        public bool VipRequireName { get; set; }

        public bool VipRequireQQ { get; set; }

        public string VTheme { get; set; }

        public string WapTheme { get; set; }

        public string WeixinAppId { get; set; }

        public string WeixinAppSecret { get; set; }

        public string WeixinCertPassword { get; set; }

        public string WeixinCertPath { get; set; }

        public string WeiXinCodeImageUrl { get; set; }

        public string WeixinLoginUrl { get; set; }

        public string WeixinNumber { get; set; }

        public string WeixinPartnerID { get; set; }

        public string WeixinPartnerKey { get; set; }

        public string WeixinPaySignKey { get; set; }

        public string WeixinToken { get; set; }

        [StringLengthValidator(0, 10, Ruleset="ValMasterSettings", MessageTemplate="“您的价”重命名的长度限制在10字符以内")]
        public string YourPriceName { get; set; }
    }
}

