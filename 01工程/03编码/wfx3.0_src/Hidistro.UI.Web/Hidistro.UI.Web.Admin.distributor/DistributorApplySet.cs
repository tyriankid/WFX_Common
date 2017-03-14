using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.distributor
{
    public class DistributorApplySet : AdminPage
    {
        protected KindeditorControl ApplicationDescription;
        protected System.Web.UI.WebControls.Button btnSave;
        protected KindeditorControl fkContent;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radiorequestoff;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radiorequeston;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioCommissionon;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioCommissionoff;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioProfitoff;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioProfit;

        protected System.Web.UI.WebControls.TextBox txtApplySet;
        protected System.Web.UI.HtmlControls.HtmlInputText txtrequestmoney;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenStoreProducAutoOn;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenStoreProducAutoOff;

        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenAgentProducRangeOn;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenAgentProducRangeOff;

        //店铺信息配置是否开启, 不开启时会员快速成为分销商
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenStoreInfoSetOn;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioOpenStoreInfoSetOff;

        //分销商购买自己商品的特殊优惠选项
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioDistributorCutDefault;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioDistributorCutByCostPrice;
        protected System.Web.UI.HtmlControls.HtmlInputText textDistributorCutByRate;

        //分销商升级类型(按佣金,按销售额)
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioUpgradeComm;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton radioUpgradeOrdersTotal;

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.ApplicationDescription = this.ApplicationDescription.Text.Trim();
            masterSettings.DistributorDescription = this.fkContent.Text;
            masterSettings.MentionNowMoney = this.txtApplySet.Text.Trim();
            masterSettings.IsRequestDistributor = this.radiorequeston.Checked;
            masterSettings.EnableCommission = this.radioCommissionon.Checked;
            masterSettings.EnableProfit = this.radioProfit.Checked;
            masterSettings.EnableStoreProductAuto = this.radioOpenStoreProducAutoOn.Checked;
            masterSettings.EnableAgentProductRange = this.radioOpenAgentProducRangeOn.Checked;
            masterSettings.EnableStoreInfoSet = this.radioOpenStoreInfoSetOn.Checked;
            #region 分销商升级类型
            if (this.radioUpgradeComm.Checked)
            {
                masterSettings.DistributorUpgradeType = this.radioUpgradeComm.Value;
            }
            else if (this.radioUpgradeOrdersTotal.Checked)
            {
                masterSettings.DistributorUpgradeType = this.radioUpgradeOrdersTotal.Value;
            }
            #endregion
            #region 分销商特殊优惠
            //分销商特殊优惠
            if (this.radioDistributorCutDefault.Checked)//默认无优惠
            {
                masterSettings.DistributorCutOff = "default";
            }
            else if (this.radioDistributorCutByCostPrice.Checked)
            {
                masterSettings.DistributorCutOff = "bycostprice";
            }
            else if (this.radioDistributorCutByCostPrice.Checked == false && this.radioDistributorCutDefault.Checked == false)
            {
                string s = this.textDistributorCutByRate.Value.Trim();
                int result = 0;
                if (!int.TryParse(s, out result) || result < 0 || result > 100)
                {
                    this.ShowMsg("请输入大于0,小于100的正整数百分比!", false);
                    return;
                }
                masterSettings.DistributorCutOff = result.ToString();
            }
            //end分销商特殊优惠
            #endregion 

            if (masterSettings.IsRequestDistributor)
            {
                string s = this.txtrequestmoney.Value.Trim();
                int result = 0;
                if (!int.TryParse(s, out result) || result < 0)
                {
                    this.ShowMsg("请输入必须大于等于0的整数申请分销商条件金额", false);
                    return;
                }
                masterSettings.FinishedOrderMoney = result;
            }
            SettingsManager.Save(masterSettings);
            this.ShowMsg("修改成功", true);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.ApplicationDescription.Text = masterSettings.ApplicationDescription;
                this.fkContent.Text = masterSettings.DistributorDescription;
                this.txtApplySet.Text = masterSettings.MentionNowMoney;
                this.txtrequestmoney.Value = masterSettings.FinishedOrderMoney.ToString();
                this.radiorequeston.Checked = true;
                if (!masterSettings.IsRequestDistributor)
                {
                    this.radiorequestoff.Checked = true;
                }              
                if (masterSettings.EnableCommission)
                {
                    this.radioCommissionon.Checked = true;
                }
                else
                {
                    this.radioCommissionoff.Checked = true;
                }

                if (masterSettings.EnableProfit)
                {
                    this.radioProfit.Checked = true;
                }
                else
                {
                    this.radioProfitoff.Checked = true;
                }

                if (masterSettings.EnableStoreProductAuto)
                    this.radioOpenStoreProducAutoOn.Checked = true;
                else
                    this.radioOpenStoreProducAutoOff.Checked = true;

                if (masterSettings.EnableAgentProductRange)
                    this.radioOpenAgentProducRangeOn.Checked = true;
                else
                {
                    this.radioOpenAgentProducRangeOff.Checked = true;
                }

                if (masterSettings.EnableStoreInfoSet)
                    this.radioOpenStoreInfoSetOn.Checked = true;
                else
                    this.radioOpenStoreInfoSetOff.Checked = true;

                if (masterSettings.DistributorUpgradeType == "byComm")
                    this.radioUpgradeComm.Checked = true;
                else if (masterSettings.DistributorUpgradeType == "byOrdersTotal")
                    this.radioUpgradeOrdersTotal.Checked = true;

                //分销商特殊优惠
                if (masterSettings.DistributorCutOff == "default")
                {
                    this.radioDistributorCutDefault.Checked = true;
                    this.radioDistributorCutByCostPrice.Checked = false;
                    this.textDistributorCutByRate.Disabled = true;
                }
                else if (masterSettings.DistributorCutOff == "bycostprice")
                {
                    this.radioDistributorCutDefault.Checked = false;
                    this.radioDistributorCutByCostPrice.Checked = true;
                    this.textDistributorCutByRate.Disabled = true;
                }
                else
                {
                    this.radioDistributorCutDefault.Checked = false;
                    this.radioDistributorCutByCostPrice.Checked = false;
                    this.textDistributorCutByRate.Disabled = false;
                    this.textDistributorCutByRate.Value = masterSettings.DistributorCutOff;
                }
            }
        }
    }
}
