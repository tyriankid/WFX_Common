namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyVantages : VWeiXinOAuthTemplatedWebControl
    {
        private Pager pager;

        protected override void AttachChildControls()
        {
            this.pager = (Pager)this.FindControl("pager");
            LoadGiftTop();
            int num2;
            int num3;
            string url = this.Page.Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
            {
                this.Page.Response.Redirect(url);
            }
            string str2 = this.Page.Request.QueryString["status"];
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "1";
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            
        }


        /// <summary>
        /// 绑定TOP类控件
        /// </summary>
        private void BindGiftTop(Control c)
        {
            //声明变量
            object dtGiftTop = null;

            //找到控件后,获取对应的数据集
            string topName = c.ID.Substring(c.ID.IndexOf("_") + 1);
            switch (topName)
            {
                case "Hot":
                    dtGiftTop = ProductBrowser.GetGifts(4);
                    break;
                case "All":
                    GiftQuery query = new GiftQuery
                    {
                        Page = new Pagination {  
                            PageSize= this.pager.PageSize,
                            PageIndex = this.pager.PageIndex,
                            SortBy = "Giftid",
                            SortOrder = SortAction.Desc
                        }
                    };
                    dtGiftTop = ProductBrowser.GetGifts(query).Data;
                    break;
                default:
                    //dtProductTop = ProductBrowser.GetHomeProductTop(3, pt);
                    break;
            }

            //数据集处理
            switch (SettingsManager.GetMasterSettings(true).VTheme.ToLower())
            {
                default:
                    break;
            }
            //绑定
            (c as VshopTemplatedRepeater).DataSource = dtGiftTop;
            (c as VshopTemplatedRepeater).DataBind();
        }




        /// <summary>
        /// 加载TOP类控件数据
        /// </summary>
        private void LoadGiftTop()
        {
            foreach (Control c in base.Controls)
            {
                if (c is VshopTemplatedRepeater)
                {
                    if (c.ID.IndexOf("rptGiftTop_") > -1)
                    {
                        BindGiftTop(c);
                    }
                }
                foreach (Control c2 in c.Controls)
                {
                    if (c2 is VshopTemplatedRepeater)
                    {
                        if (c2.ID.IndexOf("rptGiftTop_") > -1)
                        {
                            BindGiftTop(c2);
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyVantages.html";
            }
            base.OnInit(e);
        }
    }
}

