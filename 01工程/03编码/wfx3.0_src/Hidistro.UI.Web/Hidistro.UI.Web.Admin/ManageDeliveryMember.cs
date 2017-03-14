using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]

    public class ManageDeliveryMember : AdminPage
    {

        private int userId;
        protected System.Web.UI.WebControls.Button btnCreate;
        protected System.Web.UI.WebControls.TextBox txtUserName;
        protected System.Web.UI.WebControls.TextBox txtPhone;
        protected System.Web.UI.WebControls.DropDownList DDLSex;
        protected System.Web.UI.WebControls.DropDownList DDLState;
        protected System.Web.UI.WebControls.DropDownList DDLStore;

        //2015-11-17日修改
        protected System.Web.UI.WebControls.Literal litTitle;
        protected System.Web.UI.WebControls.Panel PanelID;

        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (this.userId > 0)
            {
                DeliveryMemberInfo deliveryMember = StoreHelper.GetDeliveryMemberInfo(this.userId);
                deliveryMember.UserName = this.txtUserName.Text.Trim();
                deliveryMember.Phone = this.txtPhone.Text.Trim();
                deliveryMember.Sex = DDLSex.SelectedValue.ToInt();
                deliveryMember.State = DDLState.SelectedValue.ToInt();
                deliveryMember.StoreId = DDLStore.SelectedValue.ToInt();
                if (deliveryMember.StoreId == 0)
                {
                    this.ShowMsg("请选择所属门店！", false); return;
                }
                if (StoreHelper.CreateDeliveryMember(deliveryMember, false))
                {
                    this.ShowMsgAndReUrl("成功编辑了一个配送员", true, "DeliveryUserList.aspx");
                }
                else
                {
                    this.ShowMsg("编辑失败!", false);
                }
            }
            else
            {
                DeliveryMemberInfo deliveryMember = new DeliveryMemberInfo
                {
                    UserName = this.txtUserName.Text.Trim(),
                    Phone = this.txtPhone.Text.Trim(),
                    Sex = DDLSex.SelectedValue.ToInt(),
                    State = DDLState.SelectedValue.ToInt(),
                    StoreId = DDLStore.SelectedValue.ToInt(),
                    AddTime = DateTime.Now,
                };
                if (deliveryMember.StoreId == 0)
                {
                    this.ShowMsg("请选择所属门店！", false); return;
                }
                if (StoreHelper.CreateDeliveryMember(deliveryMember, true))
                {
                    this.ShowMsgAndReUrl("成功添加了一个配送员", true, "DeliveryUserList.aspx");
                }
                else
                {
                    this.ShowMsg("添加失败!", false);
                }
            }

            
        }

        private void Bind()
        {
            DeliveryMemberInfo deliveryMember = StoreHelper.GetDeliveryMemberInfo(this.userId);
            this.txtUserName.Text = deliveryMember.UserName;
            this.txtPhone.Text = deliveryMember.Phone;
            this.DDLSex.SelectedValue = deliveryMember.Sex.ToString();
            this.DDLState.SelectedValue = deliveryMember.State.ToString();
            this.DDLStore.SelectedValue = deliveryMember.StoreId.ToString();
            /*
            {
                UserName = this.txtUserName.Text.Trim(),
                Phone = this.txtPhone.Text.Trim(),
                Sex = DDLSex.SelectedValue.ToInt(),
                State = DDLState.SelectedValue.ToInt(),
                StoreId = DDLStore.SelectedValue.ToInt(),
                AddTime = DateTime.Now,
            };
             */ 
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            this.userId = this.Page.Request.QueryString["id"].ToInt();
            if (!this.Page.IsPostBack)
            {
                DataTable dtStoreList = WxPoiHelper.GetPoiListInfo();
                DDLStore.Items.Add("请选择");
                foreach (DataRow row in dtStoreList.Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = row["storeName"].ToString();
                    item.Value = row["storeid"].ToString();
                    DDLStore.Items.Add(item);
                }

                if (this.userId > 0)
                {
                    this.Bind();
                }
            }

        }


    }
}
