using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductCategory)]
    public class EditSpecialRate : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnSaveRate;
        protected HtmlInputText first;//在册经营本品牌客户
        protected HtmlInputText second;//在册未经营本品牌客户
        protected HtmlInputText third;//公司内部无实体经销商
        protected HtmlInputText fourth;//非在册客户
        protected HtmlInputText fifth;//公司电商部门
        protected HtmlInputText sixth;//公司电商部门


        private void BindData()
        {
                string[] rentArry = CatalogHelper.GetSpecialCategoryRent(int.Parse( Request.QueryString["categroyId"])).Split(',');
                if (rentArry.Length == 6)//六种情况写死
                {
                    first.Value = rentArry[0];
                    second.Value = rentArry[1];
                    third.Value = rentArry[2];
                    fourth.Value = rentArry[3];
                    fifth.Value = rentArry[4];
                    sixth.Value = rentArry[5];
                }
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            string rates = first.Value + "," + second.Value + "," + third.Value + "," + fourth.Value + "," + fifth.Value + ","+sixth.Value;
            //将5个比例写入数据库
            if (CatalogHelper.UpdateSpecialCategoryRent(int.Parse(Request.QueryString["categroyId"]), rates))
            {
                this.ShowMsg("成功保存了出货折扣！", true);
            }
        }
        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnSaveRate.Click += new System.EventHandler(this.btnSave_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}
