<%@ WebHandler Language="C#" Class="Hidistro.UI.Web.API.wx" %>

using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web;
using Hidistro.Core;
using Hishop.Weixin.MP.Util;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Request.Event;

namespace Hidistro.UI.Web.API
{
    public class wx : IHttpHandler
    {
        
        public void ProcessRequest(System.Web.HttpContext context)
        {
            WriteLog("aaaa");
            System.Web.HttpRequest request = context.Request;
            string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            WriteLog("bbb");
            if (request.HttpMethod == "GET")
            {
                WriteLog("ccccc");
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
                    WriteLog("cddddd");
                    CustomMsgHandler handler = new CustomMsgHandler(request.InputStream);
                    handler.Execute();
                    context.Response.Write(handler.ResponseDocument);
                    
                }
                catch (System.Exception exception)
                {
                    WriteLog(exception.Message);
                    System.IO.StreamWriter writer = System.IO.File.AppendText(context.Server.MapPath("error.txt"));
                    writer.WriteLine(exception.Message);
                    writer.WriteLine(exception.StackTrace);
                    writer.WriteLine(System.DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
        
        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/error.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
