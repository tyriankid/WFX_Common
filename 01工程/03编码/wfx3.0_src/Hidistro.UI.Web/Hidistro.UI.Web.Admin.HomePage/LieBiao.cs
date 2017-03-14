using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class LieBiao : BaseModel
	{
        public DataTable dt =null;
        public string StyleName = "";
        protected Repeater rptGoods;
        public string[] colors = null;
        public string goodsort = "";//得到商品列表排序
		protected void Page_Load(object sender, System.EventArgs e)
		{
            InitData();
		}
        private void InitData()
        {
            try
            {              
                string[] sortfiles = { " DisplaySequence desc", " VistiCounts desc", " AddedDate desc", " AddedDate", " ShowSaleCounts desc", " ShowSaleCounts" };
                dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Model", " PageID ='" + this.PKID + "'");
                StyleName = dt.Rows[0]["PMStyle"] + "";
                string Top = dt.Rows[0]["PMTop"] + "";
                colors = dt.Rows[0]["PMValue1"].ToString().Split('♦');
                goodsort = dt.Rows[0]["PMContents"].ToString();
                string[] Sorts = GetString(dt.Rows[0]["PMContents"].ToString().Split('♦'));
                string orderby = "";
                string s = "";
                foreach (string sort in Sorts)
                {
                    s += sort;
                    if (sort == "3" && s.Contains("4")) 
                    {
                        continue;
                    }
                    if (sort == "4" && s.Contains("3"))
                    {
                        continue;
                    }
                    if (sort == "5" && s.Contains("6"))
                    {
                        continue;
                    }
                    if (sort == "6" && s.Contains("5"))
                    {
                        continue;
                    }
                    
                    orderby += sortfiles[Convert.ToInt32(sort)-1] + "," ;
                }
                 SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                 DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors();
                 string strWhere = "";
                 if (!masterSettings.EnableStoreProductAuto)
                 {
                     if ((currentDistributors != null) && (currentDistributors.UserId != 0))
                     {

                         strWhere += " and p.ProductId IN (SELECT ProductId FROM Hishop_DistributorProducts WHERE UserId=" + currentDistributors.UserId + ")";
                     }
                 }              
                orderby = orderby.TrimEnd(',');
                string sql = "select top " + Top + " p.*,(select MIN(SalePrice) from Hishop_SKUs where ProductId =p.ProductId) as SalePrice from Hishop_Products as p  where p.SaleStatus=1 and p.ProductId not in (select ProductId from hishop_CutDown where EndDate > GETDATE()) " + strWhere + " order by " + orderby;
                rptGoods.DataSource = DataBaseHelper.GetDataSet(sql);
                rptGoods.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        public static string[] GetString(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (list.IndexOf(values[i].ToLower()) == -1)//对每个成员做一次新数组查询如果没有相等的则加到新数组
                    list.Add(values[i]);
            }
            return list.ToArray();
        } 
	}
}
