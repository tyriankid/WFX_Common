using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    public class CouponsImg : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/text";
            string imgstr = context.Request.Form["image"].Split(',')[1];
            int ID =Convert.ToInt32(context.Request.Form["ID"]);
            string hz = context.Request.Form["hz"];
            if (imgstr != "")
            {
                try
                {
                    byte[] bytes = System.Convert.FromBase64String(imgstr);
                    string imgPath = "/Storage/master/topic/" + Guid.NewGuid()+"." + hz;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                        img.Save(context.Server.MapPath(imgPath));
                    }
                    MemberInfo member = MemberProcessor.GetCurrentMember();
                    CouponsAct ca = CouponHelper.GetCouponsAct(ID);
                    CouponsActShare cas = new CouponsActShare();
                    cas.CouponsID = ca.CouponsID;
                    cas.CouponsActID = ca.ID;
                    cas.ShareTime = DateTime.Now;
                    cas.UserID = member.UserId;
                    cas.UserName = member.UserName;
                    cas.UserImg = imgPath;
                    cas.UseCount = 0;
                    int NewID= CouponHelper.addCouponsActShare(cas);
                    context.Response.Write(NewID);
                }
                catch (Exception ex)
                {
                    context.Response.Write("false");
                }
            }
            else
            {
                context.Response.Write("false");
            }
        }  
    }
}
