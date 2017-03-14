namespace Hishop.Plugins
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Xml;

    public abstract class RefundRequest : ConfigablePlugin, IPlugin
    {
        private const string FormFormat = "<form id=\"refundform\" name=\"refundform\" action=\"{0}\" method=\"POST\">{1}</form>";
        private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

        protected RefundRequest()
        {
        }

        protected virtual string CreateField(string name, string strValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[] { name, strValue });
        }

        protected virtual string CreateForm(string content, string action)
        {
            content = content + "<input type=\"submit\" value=\"退款请求\" style=\"display:none;\">";
            return string.Format(CultureInfo.InvariantCulture, "<form id=\"refundform\" name=\"refundform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[] { action, content });
        }

        public static RefundRequest CreateInstance(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Type plugin = RefundPlugins.Instance().GetPlugin("RefundRequest", name);
            if (plugin == null)
            {
                return null;
            }
            return (Activator.CreateInstance(plugin) as RefundRequest);
        }

        public static RefundRequest CreateInstance(string name, string configXml, string[] orderId, string refundOrderId, decimal[] amount, decimal[] refundaAmount, string[] body, string buyerEmail, DateTime date, string returnUrl, string notifyUrl, string attach)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            object[] args = new object[] { orderId, refundOrderId, amount, refundaAmount, body, buyerEmail, date, returnUrl, notifyUrl, attach };
            Type plugin = RefundPlugins.Instance().GetPlugin("RefundRequest", name);
            if (plugin == null)
            {
                return null;
            }
            RefundRequest request = Activator.CreateInstance(plugin, args) as RefundRequest;
            if (!((request == null) || string.IsNullOrEmpty(configXml)))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                request.InitConfig(document.FirstChild);
            }
            return request;
        }

        protected virtual void RedirectToGateway(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }

        public abstract void SendRequest();
        protected virtual void SubmitRefundForm(string formContent)
        {
            string s = formContent + "<script>document.forms['refundform'].submit();</script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }

        public abstract bool IsMedTrade { get; }
    }
}

