namespace Hishop.Weixin.Pay
{
    using Hishop.Weixin.Pay.Domain;
    //using Hishop.Weixin.Pay.Lib;
    using System;
    using System.Collections.Generic;

    public class Refund
    {
        public static string SendRequest(RefundInfo info, PayConfig config)
        {
        //    WxPayData inputObj = new WxPayData();
        //    if (!string.IsNullOrEmpty(info.transaction_id))
        //    {
        //        inputObj.SetValue("transaction_id", info.transaction_id);
        //    }
        //    else
        //    {
        //        inputObj.SetValue("out_trade_no", info.out_trade_no);
        //    }
        //    inputObj.SetValue("total_fee", (int) info.TotalFee.Value);
        //    inputObj.SetValue("refund_fee", (int) info.RefundFee.Value);
        //    inputObj.SetValue("out_refund_no", info.out_refund_no);
        //    inputObj.SetValue("op_user_id", config.MchID);
        //    SortedDictionary<string, object> values = WxPayApi.Refund(inputObj, config, 6).GetValues();
        //    if (values["return_code"].ToString() == "SUCCESS")
        //    {
        //        return "SUCCESS";
        //    }
            return "";// values["return_msg"].ToString();
        }
    }
}

