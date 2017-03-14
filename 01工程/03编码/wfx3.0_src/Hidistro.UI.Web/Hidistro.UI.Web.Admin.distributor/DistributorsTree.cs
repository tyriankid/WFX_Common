using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class DistributorsTree : AdminPage
	{
        protected int topUserId = 0;//查询的最上级用户id
        protected HiddenField hidTopUserId;//最上级用户id隐藏域
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                topUserId = Convert.ToInt32(Request.QueryString["TopUserId"]);
                hidTopUserId.Value = topUserId.ToString();
            }
        }

        /// <summary>
        /// 获取当前角色的所有菜单
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string GetZnodes(string TopUserId)
        {
            string znodes = string.Empty;
            DistributorsInfo topDistributor = DistributorsBrower.GetDistributorInfo(Convert.ToInt32(TopUserId));
            if (topDistributor != null)
            {
                DataTable dt = new DataTable();
                if (topDistributor.IsAgent == 1)
                {
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        AgentPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//所有下属分销商和代理商
                    };
                    dt = DistributorsBrower.GetDownDistributorsAndA(dquery);
                }
                else
                {
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        ReferralPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//所有三级分销商
                    };
                    dt = DistributorsBrower.GetThreeDownDistributors(dquery);
                }
                
                if (dt != null)//如果当前角色下有可以使用的菜单
                {
                    //先加一行自己
                    dt.Rows.Add(topDistributor.UserName, topDistributor.UserHead, topDistributor.UserId, topDistributor.IsAgent, topDistributor.StoreName);
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["UserId"] };
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string userid = dt.Rows[i]["userid"].ToString();
                        string isagent = dt.Rows[i]["isagent"].ToString();
                        string name = dt.Rows[i]["username"].ToString() + " " + dt.Rows[i]["isagent"].ToString()=="1"?"(代理商)":"(分销商)";
                        string storename = dt.Rows[i]["storename"].ToString();
                        string pid = dt.Rows[i]["ParentUserId"].ToString();
                        znodes += "{\"id\":\"" + userid + "\",\"isagent\":\"" + isagent + "\",\"name\":\"" + name + "\",\"open\":\"true\",\"storename\":\"" + storename + "\",\"pid\":\""+pid+"\"},";
                    }
                    znodes = znodes.TrimEnd(',');
                }
            }
            else
            {
                return "出错了";
            }
            //if (currentMenus != null)//如果当前角色下有可以使用的菜单
            //{
            //    for (int i = 0; i < currentMenus.Rows.Count; i++)
            //    {
            //        string dataid=currentMenus.Rows[i]["MIID"].ToString();
            //        string id = currentMenus.Rows[i]["Layout"].ToString();
            //        string pid = id.Length <= 2 ? "00" : id.Substring(0, id.Length - 2);//上级id,如果当前layout是一级菜单,则为00,否则就是去掉后两位.(向前一级)
            //        string name = currentMenus.Rows[i]["MIName"].ToString();
            //        string link = currentMenus.Rows[i]["MiUrl"].ToString();
            //        znodes += "{\"id\":\"" + id + "\",\"pId\":\"" + pid + "\",\"name\":\"" + name + "\",\"open\":\"true\",\"Link\":\"" + link + "\",\"DataId\":\"" + dataid + "\" },";
            //    }
            //    znodes = znodes.TrimEnd(',');
                
            //}
            return znodes;
        }


        


        

	}
}
