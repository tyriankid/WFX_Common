using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;

namespace WxPayAPI
{
    public class MicroPay
    {
        /**
        * ˢ��֧������ҵ�������߼�
        * @param body ��Ʒ����
        * @param total_fee �ܽ��
        * @param auth_code ֧����Ȩ��
        * @throws WxPayException
        * @return ˢ��֧�����
        */
        public static string Run(string orderid,string body, string total_fee, string auth_code)
        {
            Log.Info("MicroPay", "Micropay is processing...");

            WxPayData data = new WxPayData();
            data.SetValue("auth_code", auth_code);//��Ȩ��
            data.SetValue("body", body);//��Ʒ����
            data.SetValue("total_fee", int.Parse(total_fee));//�ܽ��
            data.SetValue("out_trade_no", orderid);//����������̻������� WxPayApi.GenerateOutTradeNo()

            WxPayData result = WxPayApi.Micropay(data, 10); //�ύ��ɨ֧�������շ��ؽ��

            //����ύ��ɨ֧���ӿڵ���ʧ�ܣ������쳣
            if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
            {
                string returnMsg = result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "";
                Log.Error("MicroPay", "Micropay API interface call failure, result : " + result.ToXml());
                throw new WxPayException("Micropay API interface call failure, return_msg : " + returnMsg);
            }

            //ǩ����֤
            result.CheckSign();
            Log.Debug("MicroPay", "Micropay response check sign success");

            //ˢ��֧��ֱ�ӳɹ�
            if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                result.GetValue("result_code").ToString() == "SUCCESS")
            {
                Log.Debug("MicroPay", "Micropay business success, result : " + result.ToXml());
                return result.ToPrintStr();
            }

            /******************************************************************
             * ʣ�µĶ��ǽӿڵ��óɹ���ҵ��ʧ�ܵ����
             * ****************************************************************/
            //1��ҵ������ȷʧ��
            if (result.GetValue("err_code").ToString() != "USERPAYING" &&
            result.GetValue("err_code").ToString() != "SYSTEMERROR")
            {
                Log.Error("MicroPay", "micropay API interface call success, business failure, result : " + result.ToXml());
                return result.ToPrintStr();
            }

            //2������ȷ���Ƿ�ʧ�ܣ���鵥
            //���̻�������ȥ�鵥
            string out_trade_no = data.GetValue("out_trade_no").ToString();

            //ȷ��֧���Ƿ�ɹ�,ÿ��һ��ʱ���ѯһ�ζ���������ѯ10��
            int queryTimes = 10;//��ѯ����������
            while (queryTimes-- > 0)
            {
                int succResult = 0;//��ѯ���
                WxPayData queryResult = Query(out_trade_no, out succResult);
                //�����Ҫ������ѯ����ȴ�2s�����
                if (succResult == 2)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                //��ѯ�ɹ�,���ض�����ѯ�ӿڷ��ص�����
                else if (succResult == 1)
                {
                    Log.Debug("MicroPay", "Mircopay success, return order query result : " + queryResult.ToXml());
                    return queryResult.ToPrintStr();
                }
                //��������ʧ�ܣ�ֱ�ӷ���ˢ��֧���ӿڷ��صĽ����ʧ��ԭ�����err_code������
                else
                {
                    Log.Error("MicroPay", "Micropay failure, return micropay result : " + result.ToXml());
                    return result.ToPrintStr();
                }
            }

            //ȷ��ʧ�ܣ���������
            Log.Error("MicroPay", "Micropay failure, Reverse order is processing...");
            if (!Cancel(out_trade_no))
            {
                Log.Error("MicroPay", "Reverse order failure");
                throw new WxPayException("Reverse order failure��");
            }

            return result.ToPrintStr();
        }


        /**
	    * 
	    * ��ѯ�������
	    * @param string out_trade_no  �̻�������
	    * @param int succCode         ��ѯ���������0��ʾ�������ɹ���1��ʾ�����ɹ���2��ʾ������ѯ
	    * @return ������ѯ�ӿڷ��ص����ݣ��μ�Э��ӿ�
	    */
        public static WxPayData Query(string out_trade_no, out int succCode)
        {
            WxPayData queryOrderInput = new WxPayData();
            queryOrderInput.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayApi.OrderQuery(queryOrderInput);

            if (result.GetValue("return_code").ToString() == "SUCCESS"
                && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //֧���ɹ�
                if (result.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    succCode = 1;
                    return result;
                }
                //�û�֧���У���Ҫ������ѯ
                else if (result.GetValue("trade_state").ToString() == "USERPAYING")
                {
                    succCode = 2;
                    return result;
                }
            }

            //������ش�����Ϊ���˽��׶����Ų����ڡ���ֱ���϶�ʧ��
            if (result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
            {
                succCode = 0;
            }
            else
            {
                //�����ϵͳ�������������
                succCode = 2;
            }
            return result;
        }


        /**
        * 
        * �������������ʧ�ܻ��ظ�����10��
        * @param string out_trade_no �̻�������
        * @param depth ���ô����������õݹ���ȱ�ʾ
        * @return false��ʾ����ʧ�ܣ�true��ʾ�����ɹ�
        */
        public static bool Cancel(string out_trade_no, int depth = 0)
        {
            if (depth > 10)
            {
                return false;
            }

            WxPayData reverseInput = new WxPayData();
            reverseInput.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayApi.Reverse(reverseInput);

            //�ӿڵ���ʧ��
            if (result.GetValue("return_code").ToString() != "SUCCESS")
            {
                return false;
            }

            //������Ϊsuccess�Ҳ���Ҫ���µ��ó��������ʾ�����ɹ�
            if (result.GetValue("result_code").ToString() != "SUCCESS" && result.GetValue("recall").ToString() == "N")
            {
                return true;
            }
            else if (result.GetValue("recall").ToString() == "Y")
            {
                return Cancel(out_trade_no, ++depth);
            }
            return false;
        }
    }
}