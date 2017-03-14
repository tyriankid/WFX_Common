using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    [ParseChildren(true)]
    public class VWGWIndex : VshopTemplatedWebControl
    {
        public Panel panelHomePage;
        private Literal litItemParams;
        protected override void AttachChildControls()
        {
            this.panelHomePage = (Panel)this.FindControl("panelHomePage");
            this.litItemParams = (Literal)this.FindControl("litItemParams");

           
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            //PageTitle.AddSiteNameTitle(masterSettings.SiteName);

            


            panelHomePage.Controls.Clear();
            string selectSql = string.Format("Select * From YiHui_HomePage Where PageType={0} order by PageSN", 31);
            DataSet ds = DataBaseHelper.GetDataSet(selectSql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                ds = DataBaseHelper.GetDataSet("Select * From YiHui_HomePage Where PageType=21 order by PageSN");
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                baseModel.PKID = new Guid(dr["PageID"].ToString());//模块的内容ID
                baseModel.PageSN = dr["PageSN"] + "";
                panelHomePage.Controls.Add(baseModel);
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VWGWIndex.html";
            }
            base.OnInit(e);
        }

      
     
    }
}
