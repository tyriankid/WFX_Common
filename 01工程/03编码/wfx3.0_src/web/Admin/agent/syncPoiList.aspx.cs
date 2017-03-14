
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.agent
{
    public partial class syncPoiList : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindPoiInfo();
            }
        }

        /// <summary>
        /// 获取微信门店信息
        /// </summary>
        private void BindPoiInfo()
        {
            DataTable dtPoiInfo = WxPoiHelper.GetPoiListInfo();
            dlPoiList.DataSource = dtPoiInfo;
            dtPoiInfo.Columns.Add("available_state_name");
            foreach (DataRow row in dtPoiInfo.Rows)
            {
                switch (Convert.ToInt32(row["available_state"]))
                {
                    case 1:
                        row["available_state_name"] = "系统错误";
                        break;
                    case 2:
                        row["available_state_name"] = "审核中";
                        break;
                    case 3:
                        row["available_state_name"] = "审核通过";
                        break;
                    case 4:
                        row["available_state_name"] = "审核驳回";
                        break;
                }
            }
            dlPoiList.DataBind();
        }
        /// <summary>
        /// 单击同步门店按钮事件
        /// </summary>
        protected void btnSyncPoiInfos_Click(object sender, EventArgs e)
        {
            try
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
                List<PoiInfoList> poiInfoList = JsonHelper.JsonToList<PoiInfoList>(obj3["business_list"].ToString());
                if (poiInfoList.Count <= 0)
                {
                    this.ShowMsg("尚未添加微信门店", false);
                    return;
                }
                if (WxPoiHelper.SyncPoiListInfo(poiInfoList))
                {
                    this.ShowMsgAndReUrl("同步成功!", true, Request.Url.AbsoluteUri);
                }
                else
                {
                    this.ShowMsg("同步失败", false);
                }
            }
            catch (Exception ex)
            {
                this.ShowMsg(ex.Message, false);
            }

        }


        [System.Web.Services.WebMethod]
        public static string getSenderDiv(string poi_id)
        {
            string ddlHtml = string.Empty;
            /*
             <select>
              <option value ="volvo">Volvo</option>
              <option value ="saab">Saab</option>
              <option value="opel">Opel</option>
              <option value="audi">Audi</option>
            </select>
             */
            try
            {
                DataTable dtSenders = ManagerHelper.GetAllManagers();
                ddlHtml += "<select id='ddlSender'>";
                foreach (DataRow row in dtSenders.Rows)
                {
                    ddlHtml += string.Format("<option value ='{0}'>{1}:{2}</option>", row["UserId"], row["UserName"], row["AgentName"]);
                }
                ddlHtml += "</select>";
                ddlHtml += string.Format("<a href='javascript:void(0)' poi_id='{0}' id='btnBind'>确定</a>",poi_id);
                return ddlHtml;
            }
            catch
            {
                return "err";
            }
        }

        [System.Web.Services.WebMethod]
        public static string bindSender(string poi_id,string sender)
        {
            string result = string.Empty;
            if (WxPoiHelper.BindSender(poi_id, sender))
            {
                result = "ok";
            }
            else
            {
                result = "err";
            }
            return result;
        }
    }

}
