using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    /*
     活动相关的无刷新操作
     */
    public class ActivityHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            switch (text)
            {
                case "goSign":
                    this.goSign(context);
                    break;
            }
        }

        /// <summary>
        /// 进行签到操作
        /// </summary>
        /// <param name="context"></param>
        private void goSign(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string getPoints = PromoteHelper.GoSign(currentMember).ToString();


            context.Response.Write("{\"success\":true,\"points\":\"" + getPoints + "\"}");

        }
    }
}
