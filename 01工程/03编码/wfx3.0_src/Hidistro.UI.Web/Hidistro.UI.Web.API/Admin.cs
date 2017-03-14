using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace Hidistro.UI.Web.API
{
    public class Admin : IHttpHandler
    {
        private IDictionary<string, string> jsondict = new Dictionary<string, string>();
        private string msg = "";

        public void delExtendCategory(HttpContext context)
        {
            this.jsondict.Clear();
            int result = 0;
            int.TryParse(context.Request["productId"], out result);
            int num2 = 0;
            int.TryParse(context.Request["extendIndex"], out num2);
            if ((result > 0) && (num2 > 0))
            {
                if (CatalogHelper.SetProductExtendCategory(result, null))
                {
                    this.jsondict.Add("msg", "更新成功");
                    this.WriteJson(context, 1);
                }
                else
                {
                    this.jsondict.Add("msg", "更新失败");
                    this.WriteJson(context, 0);
                }
            }
            else
            {
                this.jsondict.Add("msg", "参数错误");
                this.WriteJson(context, 0);
            }
        }

        public void ProcessNewOrders(HttpContext context)
        {
            string shipDistributorStoreName = "";
            int agentId = 0;
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            if (currentManager == null) return;
            RoleInfo roleinfo = ManagerHelper.GetRole(currentManager.RoleId);

            if (roleinfo.RoleName == "门店发货" && Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping)
            {
                shipDistributorStoreName = DistributorsBrower.GetDistributorInfo(currentManager.ClientUserId).StoreName;//如果是分店配送管理员进来了,那么将店铺名传参
            }
            if (roleinfo.RoleName == "代理商" && Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction)
            {
                agentId = currentManager.UserId;
            }

            DateTime now = DateTime.Now.AddDays(-1);
            string text1 = context.Request["lastTime"];
            //DateTime.TryParse(context.Request["lastTime"], out now);
            int ordersCount = 0;
            int payCount = 0;
            int refundOrderCount = 0;
            int replacementOrderCount = 0;
            int returnsOrderCount = 0;
            SalesHelper.GetNewlyOrdersCountAndPayCount(now, out ordersCount, out payCount, out refundOrderCount, out replacementOrderCount, out returnsOrderCount, shipDistributorStoreName,agentId);
            this.jsondict.Clear();
            this.jsondict.Add("OrdersCount", ordersCount.ToString());
            this.jsondict.Add("PayCount", payCount.ToString());
            this.jsondict.Add("RefundOrderCount", refundOrderCount.ToString());
            this.jsondict.Add("ReplacementOrderCount", replacementOrderCount.ToString());
            this.jsondict.Add("ReturnsOrderCount", returnsOrderCount.ToString());
            this.jsondict.Add("lastTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            if (((ordersCount > 0) || (payCount > 0)) || (((returnsOrderCount > 0) || (replacementOrderCount > 0)) || (returnsOrderCount > 0)))
            {
                this.WriteJson(context, 1);
            }
            else
            {
                this.WriteJson(context, 0);
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            /*
            if (HiContext.Current.User.UserRole != UserRole.SiteManager)
            {
                this.jsondict.Add("msg", "权限不足");
                this.WriteJson(context, 0);
            }
            */
            string str = context.Request["action"];
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
            }
            else
            {
                str = "";
            }
            string str2 = str;
            if (str2 != null)
            {
                if (!(str2 == "getneworders"))
                {
                    if (!(str2 == "delextendcategory"))
                    {
                        return;
                    }
                }
                else
                {
                    this.ProcessNewOrders(context);
                    return;
                }
                this.delExtendCategory(context);
            }
        }

        public void WriteJson(HttpContext context, int status)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            builder.Append("\"status\":\"" + status + "\"");
            if (this.jsondict.Count > 0)
            {
                foreach (string str in this.jsondict.Keys)
                {
                    builder.AppendFormat(",\"{0}\":\"{1}\"", str, this.jsondict[str]);
                }
            }
            builder.Append("}");
            context.Response.Write(builder.ToString());
            context.Response.End();
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