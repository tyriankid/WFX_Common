namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Hidistro.Entities.Commodities;
    using System.Data;
    using Hidistro.Entities.Members;
    using Hidistro.ControlPanel.Members;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    public class VPicOrder : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litOrderAlert;
        private Literal litDefaultAddress;
        private Literal litStoreList;//门店选择列表
        private HtmlInputHidden hidMinPrice;
        private HtmlInputHidden hidFromUserId;
        private HtmlInputHidden hidUserStoreId;//隐藏域,根据当前选择的配送地址存放最近门店的id(clientuserid)
        private HtmlInputHidden hidUserDistance;//隐藏域,根据当前选择的配送地址存放与最近门店的距离
        private HtmlInputHidden hidUserStoreName;//隐藏域,当前匹配的门店名


        protected override void AttachChildControls()
        {
            this.litOrderAlert = (Literal)this.FindControl("litOrderAlert");//公告栏
            this.litStoreList = (Literal)this.FindControl("litStoreList");
            this.litDefaultAddress = (Literal)this.FindControl("litDefaultAddress");
            this.hidMinPrice = (HtmlInputHidden)this.FindControl("hidMinPrice");
            this.hidFromUserId = (HtmlInputHidden)this.FindControl("hidFromUserId");
            this.hidUserDistance = (HtmlInputHidden)this.FindControl("hidUserDistance");
            this.hidUserStoreId = (HtmlInputHidden)this.FindControl("hidUserStoreId");
            this.hidUserStoreName = (HtmlInputHidden)this.FindControl("hidUserStoreName");

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.litOrderAlert.Text = masterSettings.orderAlert;
            
            this.hidMinPrice.Value = masterSettings.roadPriceInfo ;
            
            //获取我的推广人id
            try
            {
                string VisitFromMemberId = HiCache.Get(string.Format("DataCache-FromMemberId-{0}", currentMember.OpenId)) as string;
                if (!string.IsNullOrEmpty(VisitFromMemberId))
                {
                    this.hidFromUserId.Value = VisitFromMemberId;
                }
            }
            catch (Exception ex){
                //WriteLog(ex.Message);
            }

            //获取用户默认配送地址,如果没有填写,则跳转到新增地址页面
            ShippingAddressInfo shippingAddress = MemberProcessor.GetDefaultShippingAddress();
            if (shippingAddress == null)
            {
                this.Page.Response.Redirect("AddShippingAddressPro.aspx?returnUrl=picOrder.aspx"); return;
            }

            this.litDefaultAddress.Text = MemberProcessor.GetDefaultShippingAddress().Address;


            //如果填写了地址,获取所有门店坐标,并且匹配用户的默认配送地址,获取最近门店,并计算出相应的配送费和配送距离等
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


            //拼装门店列表
            string storelisthtml = "<div class='storeNow'><p><span class='switch' onclick='chooseStore()'>选择门店</span></p></div>";
            storelisthtml += "<ul class='chooseStore'>";
            foreach (DataRow row in dtPoiInfo.Rows)
            {
                DataTable dtSender = WxPoiHelper.GetSenderByPoiId(row["poi_id"].ToString());
                storelisthtml += string.Format("<li poi_id='{0}'>{1}</li>", row["poi_id"].ToString(), DistributorsBrower.GetDistributorInfo(int.Parse(dtSender.Rows[0]["clientuserid"].ToString())).StoreName);
            }
            storelisthtml += "</ul>";

            litStoreList.Text = storelisthtml;


            //如果手动选择了门店,那么门店列表除了该门店的一律删除
            DataTable dtt = new DataTable();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["poi_id"]))
            {
                dtt=dtPoiInfo.Clone();
                DataRow[] dr = dtPoiInfo.Select("poi_id = '" + this.Page.Request.QueryString["poi_id"] + "'   ");
                for(int i=0;i<dr.Length;i++)
                {
                    dtt.ImportRow((DataRow)dr[i]);
                }
                dtPoiInfo = dtt;
            }



            //获取所有门店的坐标
            string offset = string.Empty;
            foreach (DataRow row in dtPoiInfo.Rows)
            {
                offset += row["latitude"] + "," + row["longitude"] + ";";//增加精度纬度
            }
            offset = offset.TrimEnd(';');
            //将门店坐标放入数组
            string[] offsetList = offset.Split(';');
            //循环判断获取距离,得到配送范围内的门店poi_id
            List<string> poi_id = new List<string>();
            List<double> poi_user_distance = new List<double>();
            for (int i = 0; i < offsetList.Length; i++)
            {
                string[] oa = offsetList[i].Split(',');//获取门店经度,纬度
                double distance = GetDistance(shippingAddress.lat, shippingAddress.lng, Convert.ToDouble(oa[0]), Convert.ToDouble(oa[1]));
                poi_id.Add(dtPoiInfo.Rows[i]["poi_id"].ToString());
                poi_user_distance.Add(distance);
            }

            string matchIds = "";
            string matchDistance = "";
            if (poi_id.Count > 0)
            {
                for (int i = 0; i < poi_id.Count; i++)
                {
                    DataTable dtSender = WxPoiHelper.GetSenderByPoiId(poi_id[i]);
                    foreach (DataRow row in dtSender.Rows)
                    {
                        if (row["clientUserId"].ToString() != "")
                        {
                            matchIds += row["clientUserId"] + ",";
                            matchDistance += poi_user_distance[i] + ",";
                        }
                    }
                }
                matchDistance = matchDistance.TrimEnd(',');
                matchIds = matchIds.TrimEnd(',');
            }

            string[] matchDistanceArrStr = matchDistance.Split(',');
            if (matchDistanceArrStr.Length <= 0)
            {
                return;
            } 
            double[] matchDistanceArr = new double[matchDistanceArrStr.Length];
            for (int i = 0; i < matchDistanceArrStr.Length; i++)
            {
                matchDistanceArr[i] = Convert.ToDouble(matchDistanceArrStr[i]);
            }
            string[] matchIdsArr = matchIds.Split(',');
            int minIndex = GetMinAndIndex(matchDistanceArr);
            hidUserDistance.Value=matchDistanceArr[minIndex].ToString();//获取距离最小的值
            this.litOrderAlert.Text = this.litOrderAlert.Text + "当前配送距离：" + hidUserDistance.Value +"公里";
            hidUserStoreId.Value=matchIdsArr[minIndex].ToString();//获取距离最小的值的门店id
            if (matchIdsArr.Length > 0)
                hidUserStoreName.Value = DistributorsBrower.GetDistributorInfo(int.Parse(matchIdsArr[minIndex])).StoreName;




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
        private int GetMinAndIndex(params double[] pa)
        {
            double[] maxAndMin = new double[2];
            int index = -1;//定义变量存最小值的索引
            if (pa.Length != 0)
            {
                maxAndMin[1] = pa[0];
                index = 0;
                for (int i = 0; i < pa.Length; i++)
                {
                    if (maxAndMin[1] > pa[i])
                    {
                        index = i;
                        maxAndMin[1] = pa[i];
                    }
                }
            }
            return index;
        }

        protected override void OnInit(EventArgs e)
        {

            if (this.SkinName == null)
            {
                this.SkinName = "skin-vPicOrder.html";
            }
            base.OnInit(e);
        }

    }
}

