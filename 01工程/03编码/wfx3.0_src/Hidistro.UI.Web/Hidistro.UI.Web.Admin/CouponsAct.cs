using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
    public class CouponsAct : AdminPage
	{
		protected Grid grdCoupons;
		private void BindCoupons()
		{
			this.grdCoupons.DataSource = CouponHelper.GetCouponsAct("");
			this.grdCoupons.DataBind();
		}


       

		
		private void grdCoupons_ReBindData(object sender)
		{
			this.BindCoupons();
		}
		private void grdCoupons_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int ID = (int)this.grdCoupons.DataKeys[e.RowIndex].Value;
			if (CouponHelper.deleteCouponsAct(ID))
			{
				this.BindCoupons();
				this.ShowMsg("成功删除了优惠卷活动", true);
			}
			else
			{
				this.ShowMsg("删除优惠券活动失败", false);
			}
		}
		protected bool IsCouponEnd(object endtime)
		{
			return System.Convert.ToDateTime(endtime).CompareTo(System.DateTime.Now) > 0;
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdCoupons.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdCoupons_RowDeleting);
			this.grdCoupons.ReBindData += new Grid.ReBindDataEventHandler(this.grdCoupons_ReBindData);
			if (!this.Page.IsPostBack)
			{
				this.BindCoupons();
			}
		}
	}
}
