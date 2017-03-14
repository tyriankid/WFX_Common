using Hidistro.Core;
using Hishop.Weixin.MP.Util;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
namespace Hidistro.UI.Web.API
{
	public class wx : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="log"></param>
        void WxLogger(string log)
        {
            string logFile = @"D:\Project\WebSite\WFX_Yihui\wx_.txt";
            File.AppendAllText(logFile, string.Format("{0}:{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), log));
        }

		public void ProcessRequest(System.Web.HttpContext context)
		{
			System.Web.HttpRequest request = context.Request;
			string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
			string signature = request["signature"];
			string nonce = request["nonce"];
			string timestamp = request["timestamp"];
			string s = request["echostr"];
			if (request.HttpMethod == "GET")
			{
				if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
				{
					context.Response.Write(s);
				}
				else
				{
					context.Response.Write("");
				}
				context.Response.End();
			}
			else
			{
				try
				{
					CustomMsgHandler handler = new CustomMsgHandler(request.InputStream);
					handler.Execute();
					context.Response.Write(handler.ResponseDocument);
				}
				catch (System.Exception exception)
				{
					System.IO.StreamWriter writer = System.IO.File.AppendText(context.Server.MapPath("error.txt"));
					writer.WriteLine(exception.Message);
					writer.WriteLine(exception.StackTrace);
					writer.WriteLine(System.DateTime.Now);
					writer.Flush();
					writer.Close();
				}
			}
		}

        #region POST提交参数
        /// <summary>
        /// POST提交参数
        /// </summary>
        /// <param name="PostUrl">POST的地址，需要传送的地址</param>
        /// <param name="Parameters">POST提交参数，例如“client_id=2866517568&client_secret=9c”和get的链接类似</param>
        /// <returns></returns>
        public static string Post(string PostUrl, string Parameters)
        {
            string content = string.Empty;
            try
            {
                //转换为字节数组
                byte[] bytesRequestData = Encoding.UTF8.GetBytes(Parameters);
                //path不是登录界面，是登录界面向服务器提交数据的界面
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(PostUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                //获取服务器返回信息
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader reader = new StreamReader(myStream, Encoding.UTF8);
                content = reader.ReadToEnd();
                reader.Close();
                HttpWResp.Close();
            }
            catch (Exception ex)
            {
                content = ex.ToString();
            }
            return content;
        }
        #endregion



	}
}
