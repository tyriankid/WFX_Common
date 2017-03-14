using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Hidistro.Core;
using Hidistro.Core.Entities;

namespace WxPayAPI
{
    public class WxPayApi
    {
        private static SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
        /**
        * �ύ��ɨ֧��API
        * ����Աʹ��ɨ���豸��ȡ΢���û�ˢ����Ȩ���Ժ󣬶�ά���������Ϣ�������̻�����̨��
        * ���̻�����̨�����̻���̨���øýӿڷ���֧����
        * @param WxPayData inputObj �ύ����ɨ֧��API�Ĳ���
        * @param int timeOut ��ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���ص��ý�����������쳣
        */
        public static WxPayData Micropay(WxPayData inputObj, int timeOut = 1)
        {
            
            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            //���������
            if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("�ύ��ɨ֧��API�ӿ��У�ȱ�ٱ������body��");
            }
            else if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("�ύ��ɨ֧��API�ӿ��У�ȱ�ٱ������out_trade_no��");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("�ύ��ɨ֧��API�ӿ��У�ȱ�ٱ������total_fee��");
            }
            else if (!inputObj.IsSet("auth_code"))
            {
                throw new WxPayException("�ύ��ɨ֧��API�ӿ��У�ȱ�ٱ������auth_code��");
            }

            inputObj.SetValue("spbill_create_ip", WxPayConfig.IP);//�ն�ip
            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//����ʼʱ��

            Log.Debug("WxPayApi", "MicroPay request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//����HTTPͨ�Žӿ����ύ���ݵ�API
            Log.Debug("WxPayApi", "MicroPay response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//��ýӿں�ʱ

            //��xml��ʽ�Ľ��ת��Ϊ�����Է���
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        *    
        * ��ѯ����
        * @param WxPayData inputObj �ύ����ѯ����API�Ĳ���
        * @param int timeOut ��ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���ض�����ѯ������������쳣
        */
        public static WxPayData OrderQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //���������
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("������ѯ�ӿ��У�out_trade_no��transaction_id������һ����");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            Log.Debug("WxPayApi", "OrderQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//����HTTPͨ�Žӿ��ύ����
            Log.Debug("WxPayApi", "OrderQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//��ýӿں�ʱ

            //��xml��ʽ������ת��Ϊ�����Է���
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        * 
        * ��������API�ӿ�
        * @param WxPayData inputObj �ύ����������API�ӿڵĲ�����out_trade_no��transaction_id����һ��
        * @param int timeOut �ӿڳ�ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ����API���ý�����������쳣
        */
        public static WxPayData Reverse(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            //���������
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("��������API�ӿ��У�����out_trade_no��transaction_id������дһ����");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//����ʼʱ��

            Log.Debug("WxPayApi", "Reverse request : " + xml);

            string response = HttpService.Post(xml, url, true, timeOut);

            Log.Debug("WxPayApi", "Reverse response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        * 
        * �����˿�
        * @param WxPayData inputObj �ύ�������˿�API�Ĳ���
        * @param int timeOut ��ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���ؽӿڵ��ý�����������쳣
        */
        public static WxPayData Refund(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //���������
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("�˿�����ӿ��У�out_trade_no��transaction_id������һ����");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WxPayException("�˿�����ӿ��У�ȱ�ٱ������out_refund_no��");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("�˿�����ӿ��У�ȱ�ٱ������total_fee��");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WxPayException("�˿�����ӿ��У�ȱ�ٱ������refund_fee��");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new WxPayException("�˿�����ӿ��У�ȱ�ٱ������op_user_id��");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            Log.Debug("WxPayApi", "Refund request : " + xml);
            string response = HttpService.Post(xml, url, true, timeOut);//����HTTPͨ�Žӿ��ύ���ݵ�API
            Log.Debug("WxPayApi", "Refund response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//��ýӿں�ʱ

            //��xml��ʽ�Ľ��ת��Ϊ�����Է���
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
	    * 
	    * ��ѯ�˿�
	    * �ύ�˿������ͨ���ýӿڲ�ѯ�˿�״̬���˿���һ����ʱ��
	    * ����Ǯ֧�����˿�20�����ڵ��ˣ����п�֧�����˿�3�������պ����²�ѯ�˿�״̬��
	    * out_refund_no��out_trade_no��transaction_id��refund_id�ĸ���������һ��
	    * @param WxPayData inputObj �ύ����ѯ�˿�API�Ĳ���
	    * @param int timeOut �ӿڳ�ʱʱ��
	    * @throws WxPayException
	    * @return �ɹ�ʱ���أ��������쳣
	    */
        public static WxPayData RefundQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            //���������
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new WxPayException("�˿��ѯ�ӿ��У�out_refund_no��out_trade_no��transaction_id��refund_id�ĸ���������һ����");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��

            string xml = inputObj.ToXml();

            var start = DateTime.Now;//����ʼʱ��

            Log.Debug("WxPayApi", "RefundQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//����HTTPͨ�Žӿ����ύ���ݵ�API
            Log.Debug("WxPayApi", "RefundQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//��ýӿں�ʱ

            //��xml��ʽ�Ľ��ת��Ϊ�����Է���
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        * ���ض��˵�
        * @param WxPayData inputObj �ύ�����ض��˵�API�Ĳ���
        * @param int timeOut �ӿڳ�ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���أ��������쳣
        */
        public static WxPayData DownloadBill(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            //���������
            if (!inputObj.IsSet("bill_date"))
            {
                throw new WxPayException("���˵��ӿ��У�ȱ�ٱ������bill_date��");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��

            string xml = inputObj.ToXml();

            Log.Debug("WxPayApi", "DownloadBill request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//����HTTPͨ�Žӿ����ύ���ݵ�API
            Log.Debug("WxPayApi", "DownloadBill result : " + response);

            WxPayData result = new WxPayData();
            //���ӿڵ���ʧ�ܻ᷵��xml��ʽ�Ľ��
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //�ӿڵ��óɹ��򷵻ط�xml��ʽ������
            else
                result.SetValue("result", response);

            return result;
        }


        /**
	    * 
	    * ת��������
	    * �ýӿ���Ҫ����ɨ��ԭ��֧��ģʽһ�еĶ�ά������ת�ɶ�����(weixin://wxpay/s/XXXXXX)��
	    * ��С��ά��������������ɨ���ٶȺ;�ȷ�ȡ�
	    * @param WxPayData inputObj �ύ��ת��������API�Ĳ���
	    * @param int timeOut �ӿڳ�ʱʱ��
	    * @throws WxPayException
	    * @return �ɹ�ʱ���أ��������쳣
	    */
        public static WxPayData ShortUrl(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            //���������
            if (!inputObj.IsSet("long_url"))
            {
                throw new WxPayException("��Ҫת����URL��ǩ����ԭ����������URL encode��");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���	
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//����ʼʱ��

            Log.Debug("WxPayApi", "ShortUrl request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            Log.Debug("WxPayApi", "ShortUrl response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        * 
        * ͳһ�µ�
        * @param WxPaydata inputObj �ύ��ͳһ�µ�API�Ĳ���
        * @param int timeOut ��ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���أ��������쳣
        */
        public static WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //���������
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("ȱ��ͳһ֧���ӿڱ������out_trade_no��");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("ȱ��ͳһ֧���ӿڱ������body��");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("ȱ��ͳһ֧���ӿڱ������total_fee��");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new WxPayException("ȱ��ͳһ֧���ӿڱ������trade_type��");
            }

            //��������
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new WxPayException("ͳһ֧���ӿ��У�ȱ�ٱ������openid��trade_typeΪJSAPIʱ��openidΪ���������");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new WxPayException("ͳһ֧���ӿ��У�ȱ�ٱ������product_id��trade_typeΪJSAPIʱ��product_idΪ���������");
            }

            //�첽֪ͨurlδ���ã���ʹ�������ļ��е�url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", WxPayConfig.NOTIFY_URL);//�첽֪ͨurl
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("spbill_create_ip", WxPayConfig.IP);//�ն�ip	
            if (!inputObj.IsSet("nonce_str"))
                inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���

            //ǩ��
            inputObj.SetValue("sign", inputObj.MakeSign());
            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            Log.Info("WxPayApi", "UnfiedOrder request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            Log.Info("WxPayApi", "UnfiedOrder response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
        * 
        * �رն���
        * @param WxPayData inputObj �ύ���رն���API�Ĳ���
        * @param int timeOut �ӿڳ�ʱʱ��
        * @throws WxPayException
        * @return �ɹ�ʱ���أ��������쳣
        */
        public static WxPayData CloseOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //���������
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("�رն����ӿ��У�out_trade_no���");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���		
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//����ʼʱ��

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//�����ϱ�

            return result;
        }


        /**
	    * 
	    * �����ϱ�
	    * @param string interface_url �ӿ�URL
	    * @param int timeCost �ӿں�ʱ
	    * @param WxPayData inputObj��������
	    */
        private static void ReportCostTime(string interface_url, int timeCost, WxPayData inputObj)
        {
            //�������Ҫ�����ϱ�
            if (WxPayConfig.REPORT_LEVENL == 0)
            {
                return;
            }

            //�����ʧ���ϱ�
            if (WxPayConfig.REPORT_LEVENL == 1 && inputObj.IsSet("return_code") && inputObj.GetValue("return_code").ToString() == "SUCCESS" &&
             inputObj.IsSet("result_code") && inputObj.GetValue("result_code").ToString() == "SUCCESS")
            {
                return;
            }

            //�ϱ��߼�
            WxPayData data = new WxPayData();
            data.SetValue("interface_url", interface_url);
            data.SetValue("execute_time_", timeCost);
            //����״̬��
            if (inputObj.IsSet("return_code"))
            {
                data.SetValue("return_code", inputObj.GetValue("return_code"));
            }
            //������Ϣ
            if (inputObj.IsSet("return_msg"))
            {
                data.SetValue("return_msg", inputObj.GetValue("return_msg"));
            }
            //ҵ����
            if (inputObj.IsSet("result_code"))
            {
                data.SetValue("result_code", inputObj.GetValue("result_code"));
            }
            //�������
            if (inputObj.IsSet("err_code"))
            {
                data.SetValue("err_code", inputObj.GetValue("err_code"));
            }
            //�����������
            if (inputObj.IsSet("err_code_des"))
            {
                data.SetValue("err_code_des", inputObj.GetValue("err_code_des"));
            }
            //�̻�������
            if (inputObj.IsSet("out_trade_no"))
            {
                data.SetValue("out_trade_no", inputObj.GetValue("out_trade_no"));
            }
            //�豸��
            if (inputObj.IsSet("device_info"))
            {
                data.SetValue("device_info", inputObj.GetValue("device_info"));
            }

            try
            {
                Report(data);
            }
            catch (WxPayException ex)
            {
                //�����κδ���
            }
        }


        /**
	    * 
	    * �����ϱ��ӿ�ʵ��
	    * @param WxPayData inputObj �ύ�������ϱ��ӿڵĲ���
	    * @param int timeOut �����ϱ��ӿڳ�ʱʱ��
	    * @throws WxPayException
	    * @return �ɹ�ʱ���ز����ϱ��ӿڷ��صĽ�����������쳣
	    */
        public static WxPayData Report(WxPayData inputObj, int timeOut = 1)
        {
            string url = "https://api.mch.weixin.qq.com/payitil/report";
            //���������
            if (!inputObj.IsSet("interface_url"))
            {
                throw new WxPayException("�ӿ�URL��ȱ�ٱ������interface_url��");
            }
            if (!inputObj.IsSet("return_code"))
            {
                throw new WxPayException("����״̬�룬ȱ�ٱ������return_code��");
            }
            if (!inputObj.IsSet("result_code"))
            {
                throw new WxPayException("ҵ������ȱ�ٱ������result_code��");
            }
            if (!inputObj.IsSet("user_ip"))
            {
                throw new WxPayException("���ʽӿ�IP��ȱ�ٱ������user_ip��");
            }
            if (!inputObj.IsSet("execute_time_"))
            {
                throw new WxPayException("�ӿں�ʱ��ȱ�ٱ������execute_time_��");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//�����˺�ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//�̻���
            inputObj.SetValue("user_ip", WxPayConfig.IP);//�ն�ip
            inputObj.SetValue("time", DateTime.Now.ToString("yyyyMMddHHmmss"));//�̻��ϱ�ʱ��	 
            inputObj.SetValue("nonce_str", GenerateNonceStr());//����ַ���
            inputObj.SetValue("sign", inputObj.MakeSign());//ǩ��
            string xml = inputObj.ToXml();

            Log.Info("WxPayApi", "Report request : " + xml);

            string response = HttpService.Post(xml, url, false, timeOut);

            Log.Info("WxPayApi", "Report response : " + response);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /**
        * ���ݵ�ǰϵͳʱ���������������ɶ�����
         * @return ������
        */
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }

        /**
        * ����ʱ�������׼����ʱ�䣬ʱ��Ϊ����������1970��1��1�� 0��0��0������������
         * @return ʱ���
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * ����������������������ĸ������
        * @return �����
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}