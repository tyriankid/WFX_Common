namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Config;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VCountDownProductList : VWeiXinOAuthTemplatedWebControl
    {
        int num;
        int num2;
        int num3;
        private Literal litItemParams;
        private VshopTemplatedRepeater vcountdownproducts;//抢购商品列表
        private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            this.vcountdownproducts = (VshopTemplatedRepeater)this.FindControl("vcountdownproducts");
            this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
            this.litItemParams = (Literal)this.FindControl("litItemParams");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            ProductBrowseQuery query =new ProductBrowseQuery
            {
                PageIndex=num,
                PageSize=num2,
                SortBy="CountDownId",
                SortOrder = SortAction.Desc,
            };
            DbQueryResult drSource = ProductBrowser.GetCountDownProductList(query);
            num3 = drSource.TotalRecords;
            txtTotal.SetWhenIsNotNull(num3.ToString());
            this.vcountdownproducts.DataSource = drSource.Data;
            this.vcountdownproducts.DataBind();

            #region 微信分享内容配置
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            DistributorsInfo userIdDistributors = new DistributorsInfo();
            userIdDistributors = DistributorsBrower.GetUserIdDistributors(base.referralId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                PageTitle.AddSiteNameTitle(userIdDistributors.StoreName);
            }
            string str3 = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
            {
                str3 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
            }
            string str4 = "";
            string str5 = (userIdDistributors == null) ? masterSettings.SiteName : userIdDistributors.StoreName;
            if (!string.IsNullOrEmpty(masterSettings.DistributorBackgroundPic))
            {
                str4 = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[] { '|' })[0];
            }
            string strDes = masterSettings.ShopHomeDescription;
            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.BrandShow)
            {
                strDes = "低价抢正品，马上有优惠，就在考拉萌购！";
            }

            this.litItemParams.Text = str3 + "|" + masterSettings.ShopHomeName + "|" + strDes + "$";
            this.litItemParams.Text = string.Concat(new object[] { this.litItemParams.Text, str4, "|好店推荐之", str5, "商城|"+strDes+"|", HttpContext.Current.Request.Url });
            #endregion
        }


        public static string FormatDate(DateTime endTime)
        {
            TimeSpan ts=endTime-DateTime.Now;
            return string.Format("<b>{0}</b>天<b>{1}</b>小时<b>{2}</b>分<b>{3}</b>秒",ts.Days,ts.Hours,ts.Minutes,ts.Seconds);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCountDownProductList.html";
            }
            base.OnInit(e);
        }
    }
}

