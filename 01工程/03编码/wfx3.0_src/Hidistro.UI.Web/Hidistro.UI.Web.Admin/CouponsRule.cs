using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class CouponsRule : AdminPage
	{
        protected Button btn_Save;
        protected CheckBoxList chk_cate;
        protected HiddenField txtCouponsId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //this.btn_Save = (Button)this.FindControl("btn_Save");
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            if (!IsPostBack)
            {
                if (Request.QueryString["CouponsId"] != null)
                {
                    
                    //this.txtCouponsId=(HiddenField)this.FindControl("txtCouponsId");
                    this.txtCouponsId.Value = Request.QueryString["CouponsId"];
                    bindchkData();
                }
            }
		}

        private void btn_Save_Click(object sender, System.EventArgs e)
        {
            if (txtCouponsId.Value != "")
            {
                DataTable dt =DataBaseHelper.GetDataTable("Hishop_CouponsRule"," CouponsId="+txtCouponsId.Value,"");
                foreach (ListItem item in chk_cate.Items)
                {
                    if (item.Selected)
                    {
                        DataRow[] adddrs = dt.Select(string.Format("CategoryId={0}", item.Value));
                        if (adddrs.Length == 0)
                        {
                            DataRow dr = dt.NewRow();
                            dr["CouponsId"] = txtCouponsId.Value;
                            dr["CategoryId"] = item.Value;
                            dt.Rows.Add(dr);
                        }

                    }
                    else
                    {
                        DataRow[] deldrs = dt.Select(string.Format("CategoryId={0}", item.Value));
                        if (deldrs.Length > 0)
                        {
                            deldrs[0].Delete();
                        }
                    }
                }
                DataBaseHelper.CommitDataTable(dt, "select * from Hishop_CouponsRule where CouponsId = "+txtCouponsId.Value);
                this.ShowMsg("成功删除了选定张优惠券", true);
            }
        }

        private void bindchkData()
        {
            //this.chk_cate = (CheckBoxList)this.FindControl("chk_cate");
            this.chk_cate.DataSource = CatalogHelper.GetSequenceCategories();
            this.chk_cate.DataTextField = "Name";
            this.chk_cate.DataValueField = "CategoryId";
            this.chk_cate.DataBind();
            DataTable dt = DataBaseHelper.GetDataTable("Hishop_CouponsRule", " CouponsId=" + txtCouponsId.Value, "");
            foreach(ListItem item in chk_cate.Items){
                foreach (DataRow dr in dt.Rows)
                {
                    if (item.Value.Equals(dr["CategoryId"].ToString()))
                    {
                        item.Selected = true;
                    }
                }
            }
        }
	}
}
