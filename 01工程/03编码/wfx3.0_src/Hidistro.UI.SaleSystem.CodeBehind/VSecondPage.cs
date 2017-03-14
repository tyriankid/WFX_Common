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
    public class VSecondPage : VWeiXinOAuthTemplatedWebControl
    {
        public Panel panelHomePage;
        protected override void AttachChildControls()
        {
            this.panelHomePage = (Panel)this.FindControl("panelHomePage");
            panelHomePage.Controls.Clear();
            if (this.Page.Request.QueryString["SkinID"] != null)
            {
                string selectSql = string.Format("Select * From YiHui_HomePage Where SkinID='{0}' order by PageSN", this.Page.Request.QueryString["SkinID"]);
                DataSet ds = DataBaseHelper.GetDataSet(selectSql);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                    baseModel.PKID = new Guid(dr["PageID"].ToString());//模块的内容ID
                    baseModel.PageSN = dr["PageSN"] + "";
                    panelHomePage.Controls.Add(baseModel);
                }

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
