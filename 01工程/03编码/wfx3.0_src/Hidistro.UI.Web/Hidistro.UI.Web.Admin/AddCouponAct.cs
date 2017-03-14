using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class AddCouponAct : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnAddCoupons;
		
		protected System.Web.UI.WebControls.TextBox txtColValue;
        protected System.Web.UI.WebControls.DropDownList ddlCoupons;
        protected UpImg upBgImg;
        protected HiddenField txtID;
        protected TextBox txtCouponActName;

		private void btnAddCoupons_Click(object sender, System.EventArgs e)
		{
            if(upBgImg.UploadedImageUrl==""){
                this.ShowMsg("请上传背景图片",false);
                return;
            }
            Hidistro.Entities.Promotions.CouponsAct ca = null;
            if (txtID.Value != "")
            {
                ca = CouponHelper.GetCouponsAct(Convert.ToInt32(txtID.Value));
            }
            else
            {
                ca = new Entities.Promotions.CouponsAct();
            }
            ca.ColValue2 = txtCouponActName.Text.Trim();
            ca.CouponsID = Convert.ToInt32(ddlCoupons.SelectedValue);
            ca.CreateTime = DateTime.Now;
            ca.BgImg = upBgImg.UploadedImageUrl;
            ca.ColValue1 = Convert.ToInt32(txtColValue.Text.Trim());
            if (txtID.Value != "")
            {
                if (CouponHelper.UpdateConponsAct(ca))
                {
                    this.ShowMsg("修改优惠卷活动成功！",true);
                }
                else
                {
                    this.ShowMsg("修改优惠卷活动失败！", false);
                }
            }
            else
            {
                if (CouponHelper.AddCouponsAct(ca))
                {
                    this.ShowMsg("新增优惠卷活动成功！", true);
                }
                else
                {
                    this.ShowMsg("新增优惠卷活动失败！", false);
                }
            }
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCoupons.Click += new System.EventHandler(this.btnAddCoupons_Click);
            if (!IsPostBack)
            {
                ddlCoupons.DataSource = CouponHelper.GetAllCoupons();
                ddlCoupons.DataTextField = "Name";
                ddlCoupons.DataValueField = "CouponId";
                ddlCoupons.DataBind();
                if (Request.QueryString["ID"] != null)
                {
                    int ID = Convert.ToInt32(Request.QueryString["ID"]);
                    txtID.Value = ID + "";
                    Hidistro.Entities.Promotions.CouponsAct ca = CouponHelper.GetCouponsAct(ID);
                    txtCouponActName.Text = ca.ColValue2;
                    ddlCoupons.SelectedValue = ca.CouponsID + "";
                    upBgImg.UploadedImageUrl = ca.BgImg;
                    txtColValue.Text = ca.ColValue1+"";
                }
                else
                {
                    txtColValue.Text = "0";
                }
            }
		}
	
		


        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
	}
}
