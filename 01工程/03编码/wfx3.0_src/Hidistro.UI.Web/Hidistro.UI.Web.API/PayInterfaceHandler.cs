using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.AliPay.QuickPay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    public class PayInterfaceHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private IDictionary<string, string> jsondict = new Dictionary<string, string>();

        
        public void ProcessRequest(System.Web.HttpContext context)
        {
            string text = context.Request["action"];
            if (string.IsNullOrEmpty(text)) return;
            switch (text.ToLower())
            {
                case "wxpay":
                    get_WX_QRCode(context);
                    break;
                case "alipay":
                    get_Alipay(context);
                    break;
            }
        }

        /// <summary>
        /// ��ȡ΢��֧����ά��
        /// </summary>
        private void get_WX_QRCode(System.Web.HttpContext context)
        {
            string result = "{\"success\":false,\"msg\":\"��ȡ΢��֧����ά�룬���Ժ����ԣ�\"}";
            context.Response.ContentType = "application/json";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //���д���͸����������ֻ������www.XXX.comԴ�µĽű��ſ��Խ��з��ʡ�
            string str_productId = context.Request["productId"];
            string str_productDescribe = context.Request["productDescribe"];
            string str_total_fee = context.Request["total_fee"];
            string str_orderid = context.Request["orderid"];
            Uri prevUri = HttpContext.Current.Request.UrlReferrer;
            string strDomainName = (prevUri != null) ? prevUri.Host : Globals.DomainName;
            if (!string.IsNullOrEmpty(str_productId) && !string.IsNullOrEmpty(str_productDescribe)
                && !string.IsNullOrEmpty(str_total_fee))
            {
                NativePay nativePay = new NativePay();
                //֧��url,��Ч��Ϊ10Сʱ
                string strQRCodeUrl = nativePay.GetPayUrl(str_productId, str_productDescribe, int.Parse(str_total_fee)
                    , string.Format(WxPayConfig.NOTIFY_URL_Custom, strDomainName), str_orderid);
                if (!string.IsNullOrEmpty(strQRCodeUrl))
                {
                    string t = "֧���ɹ��ص���ַ��" + string.Format(WxPayConfig.NOTIFY_URL_Custom, strDomainName);
                    strQRCodeUrl = "http://" + Globals.DomainName + "/API/MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(strQRCodeUrl);
                    result = "{\"success\":true,\"msg\":\"" + strQRCodeUrl + "\",\"remark\":\"" + t + "\"}";
                }
            }
            context.Response.Write(result);
            context.Response.End();
        }


        private void get_Alipay(System.Web.HttpContext context)
        {
            //�̻������ţ��̻���վ����ϵͳ��Ψһ�����ţ�����
            string out_trade_no = context.Request["orderid"];

            //�������ƣ�����
            string subject = context.Request["productDescribe"];

            //���������
            string total_fee = context.Request["total_fee"];

            //��Ʒ�������ɿ�
            string body = context.Request["productDescribe"];

            Uri prevUri = HttpContext.Current.Request.UrlReferrer;
            string strDomainName = (prevUri != null) ? prevUri.Host : Globals.DomainName;

            ////////////////////////////////////////////////////////////////////////////////////////////////

            //������������������
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            AlipayConfig ac = new AlipayConfig();
            sParaTemp.Add("service", AlipayConfig.service);
            sParaTemp.Add("partner", AlipayConfig.partner);
            sParaTemp.Add("seller_id", AlipayConfig.seller_id);
            sParaTemp.Add("_input_charset", AlipayConfig.input_charset.ToLower());
            sParaTemp.Add("payment_type", AlipayConfig.payment_type);
            sParaTemp.Add("notify_url", AlipayConfig.notify_url);
            sParaTemp.Add("return_url", string.Format(AlipayConfig.return_url_Custom, strDomainName));//����
            sParaTemp.Add("anti_phishing_key", AlipayConfig.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", AlipayConfig.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            //����ҵ������������߿����ĵ�����Ӳ���.�ĵ���ַ:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
            //��sParaTemp.Add("������","����ֵ");

            //��������
            string sHtmlText = AlipaySubmit.BuildRequest(sParaTemp, "get", "ȷ��");
            context.Response.Write(sHtmlText);
            context.Response.End();
        }

    }
}
