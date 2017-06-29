using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public class SelectProduct : AdminPage
    {
        protected System.Web.UI.WebControls.Button btnSaveInfo;
        protected Repeater SelectedProducts;
        protected int storeId =0;

        private void BindProduct()
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();

            this.SelectedProducts.DataSource = ProductHelper.GetStoreProductBaseInfo(currentManager.ClientUserId);
            this.SelectedProducts.DataBind();
        }

        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
    }
}
