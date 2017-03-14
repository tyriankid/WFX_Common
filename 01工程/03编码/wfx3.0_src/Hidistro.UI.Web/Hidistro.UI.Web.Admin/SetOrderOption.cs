using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class SetOrderOption : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnOK;
        protected RadioButtonList rdoSpecialOrderAmount;
        protected System.Web.UI.WebControls.TextBox txtSpecialValue1;
        protected System.Web.UI.WebControls.TextBox txtSpecialValue2;
		protected System.Web.UI.WebControls.TextBox txtCloseOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCloseOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtFinishOrderDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtFinishOrderDaysTip;
		protected System.Web.UI.WebControls.TextBox txtShowDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShowDaysTip;
		protected System.Web.UI.WebControls.TextBox txtTaxRate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTaxRateTip;
        protected System.Web.UI.WebControls.TextBox txtOrderRemindTime;
        protected System.Web.UI.HtmlControls.HtmlGenericControl txtOrderRemindTimeTip;
        protected YesNoRadioButtonList radEnableOrderRemind;
		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			int num;
			int num2;
			int num3;
			decimal num4;
            decimal num5;
			if (this.ValidateValues(out num, out num2, out num3, out num4))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                //masterSettings.freeShippingAmount = num5;
				masterSettings.OrderShowDays = num;
				masterSettings.CloseOrderDays = num2;
				masterSettings.FinishOrderDays = num3;
				masterSettings.TaxRate = num4;
                masterSettings.EnableOrderRemind = radEnableOrderRemind.SelectedValue;
                masterSettings.OrderRemindTime = txtOrderRemindTime.Text.Trim();

                masterSettings.SpecialOrderAmountType = rdoSpecialOrderAmount.SelectedValue;
                masterSettings.SpecialValue1 = string.IsNullOrEmpty(txtSpecialValue1.Text.Trim()) ? 0M : Convert.ToDecimal(txtSpecialValue1.Text.Trim());
                masterSettings.SpecialValue2 = string.IsNullOrEmpty(txtSpecialValue2.Text.Trim()) ? 0M : Convert.ToDecimal(txtSpecialValue2.Text.Trim());

				if (this.ValidationSettings(masterSettings))
				{
					Globals.EntityCoding(masterSettings, true);
					SettingsManager.Save(masterSettings);
					this.SavaKuaidi100Key();
					this.ShowMsg("保存成功", true);
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this.txtShowDays.Text = masterSettings.OrderShowDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtCloseOrderDays.Text = masterSettings.CloseOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtFinishOrderDays.Text = masterSettings.FinishOrderDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.txtTaxRate.Text = masterSettings.TaxRate.ToString(System.Globalization.CultureInfo.InvariantCulture);
                int rdoIndex = -1;
                switch(masterSettings.SpecialOrderAmountType.ToString(System.Globalization.CultureInfo.InvariantCulture))
                {
                    case "freeShipping":
                        rdoIndex = 1;
                        break;
                    case "addShipping":
                        rdoIndex = 2;
                        break;
                    default:
                        rdoIndex = 0;
                        break;
                }
                this.rdoSpecialOrderAmount.SelectedIndex = rdoIndex;
                this.txtSpecialValue1.Text = masterSettings.SpecialValue1.ToString("F2");
                this.txtSpecialValue2.Text = masterSettings.SpecialValue2.ToString("F2");
                this.radEnableOrderRemind.SelectedValue = masterSettings.EnableOrderRemind;
                this.txtOrderRemindTime.Text = masterSettings.OrderRemindTime;
				XmlDocument document = new XmlDocument();
				string filename = System.Web.HttpContext.Current.Request.MapPath("~/Express.xml");
				document.Load(filename);
				document.SelectSingleNode("companys");
			}
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
		}
		private void SavaKuaidi100Key()
		{
			XmlDocument document = new XmlDocument();
			string filename = System.Web.HttpContext.Current.Request.MapPath("~/Express.xml");
			document.Load(filename);
			document.SelectSingleNode("companys");
			document.Save(filename);
		}
        private bool ValidateValues(out int showDays, out int closeOrderDays, out int finishOrderDays, out decimal taxRate)
		{
			string str = string.Empty;
			if (!int.TryParse(this.txtShowDays.Text, out showDays))
			{
				str += Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtCloseOrderDays.Text, out closeOrderDays))
			{
				str += Formatter.FormatErrorMessage("过期几天自动关闭订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtFinishOrderDays.Text, out finishOrderDays))
			{
				str += Formatter.FormatErrorMessage("发货几天自动完成订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!decimal.TryParse(this.txtTaxRate.Text, out taxRate))
			{
				str += Formatter.FormatErrorMessage("订单发票税率不能为空,为非负数字,范围在0-100之间");
			}


			bool result;
			if (!string.IsNullOrEmpty(str))
			{
				this.ShowMsg(str, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		private bool ValidationSettings(SiteSettings setting)
		{
			ValidationResults results = Validation.Validate<SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			string msg = string.Empty;
			if (!results.IsValid)
			{
				foreach (ValidationResult result in (System.Collections.Generic.IEnumerable<ValidationResult>)results)
				{
					msg += Formatter.FormatErrorMessage(result.Message);
				}
				this.ShowMsg(msg, false);
			}
			return results.IsValid;
		}
	}
}
