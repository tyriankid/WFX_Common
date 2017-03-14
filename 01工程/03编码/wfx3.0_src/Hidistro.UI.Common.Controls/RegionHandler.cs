namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Linq;
    using System.Data;

    public class RegionHandler : IHttpHandler
    {
        private void GetRegionInfo(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int result = 0;
            int.TryParse(context.Request["regionId"], out result);
            if (result <= 0)
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                XmlNode region = RegionHelper.GetRegion(result);
                if (region == null)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    int num2 = 1;
                    if (region.Name.Equals("city"))
                    {
                        num2 = 2;
                    }
                    else if (region.Name.Equals("county"))
                    {
                        num2 = 3;
                    }
                    string str = (num2 > 1) ? RegionHelper.GetFullPath(result) : "";
                    string str2 = "";
                    if (!region.Name.Equals("province"))
                    {
                        str2 = region.ParentNode.Attributes["id"].Value;
                    }
                    string s = "{";
                    s = (((((s + "\"Status\":\"OK\",") + "\"RegionId\":\"" + result.ToString(CultureInfo.InvariantCulture) + "\",") + "\"Depth\":\"" + num2.ToString(CultureInfo.InvariantCulture) + "\",") + "\"Path\":\"" + str + "\",") + "\"ParentId\":\"" + str2 + "\"") + "}";
                    context.Response.Write(s);
                }
            }
        }

        private static void GetRegions(HttpContext context)
        {
            Dictionary<int, string> citys;
            string fieldCode = "ProvinceId";
            context.Response.ContentType = "application/json";
            int result = 0;
            int.TryParse(context.Request["parentId"], out result);
            if (result > 0)//如果传回来的省份有值,则根据省份筛选
            {
                XmlNode region = RegionHelper.GetRegion(result);
                if (region == null)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                    return;
                }
                if (region.Name.Equals("province"))
                {
                    citys = RegionHelper.GetCitys(result);
                    fieldCode = "CityId";
                }
                else
                {
                    citys = RegionHelper.GetCountys(result);
                    fieldCode = "CountyId";
                }
            }
            else//如果没有传递值来,是初始化载入所有的省
            {
                citys = RegionHelper.GetAllProvinces();

                //如果开启了门店配送功能,则是根据支持的街道信息来过滤绑定省份信息
                
                
            }

            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping && System.Web.HttpContext.Current.Request.Path.IndexOf("/" + "admin" + "/") < 0)
            {
                //如果是后台,则不进行过滤,前端过滤.

                DataTable dtAll = Hidistro.ControlPanel.Sales.SalesHelper.GetAllStreetRegionId();
                var vcitys = citys.Where(x => dtAll.AsEnumerable().Select(a => a.Field<int>(fieldCode)).Any(a => x.Key.Equals(a)));

                if (vcitys.Count() == 0)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.Append("\"Regions\":[");
                    foreach (var q in vcitys)
                    {
                        builder.Append("{");
                        builder.AppendFormat("\"RegionId\":\"{0}\",", q.Key);
                        builder.AppendFormat("\"RegionName\":\"{0}\"", q.Value);
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("]}");
                    citys.Clear();
                    context.Response.Write(builder.ToString());
                }
            }
            else
            {
                #region
                if (citys.Count == 0)
                {
                    context.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.Append("\"Regions\":[");
                    foreach (int num2 in citys.Keys)
                    {
                        builder.Append("{");
                        builder.AppendFormat("\"RegionId\":\"{0}\",", num2.ToString(CultureInfo.InvariantCulture));
                        builder.AppendFormat("\"RegionName\":\"{0}\"", citys[num2]);
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("]}");
                    citys.Clear();
                    context.Response.Write(builder.ToString());
                }
                #endregion
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str2 = context.Request["action"];
                if (str2 != null)
                {
                    if (!(str2 == "getregions"))
                    {
                        if (str2 == "getregioninfo")
                        {
                            goto Label_003A;
                        }
                    }
                    else
                    {
                        GetRegions(context);
                    }
                }
                return;
            Label_003A:
                this.GetRegionInfo(context);
            }
            catch
            {
            }
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

