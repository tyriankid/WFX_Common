namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMyConsultations : VWeiXinOAuthTemplatedWebControl
    {
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            ProductConsultationAndReplyQuery consultationQuery = new ProductConsultationAndReplyQuery {
                UserId = currentMember.UserId,
                IsCount = true,
                PageIndex = num,
                PageSize = num2,
                SortBy = "ConsultationId",
                SortOrder = SortAction.Desc
            };
            DataTable productConsultations = (DataTable)ProductBrowser.GetProductConsultations(consultationQuery).Data;
            //为查询出来的问答列表增加商品图
            productConsultations.Columns.Add("ThumbnailsUrl");//图
            for (int i = 0; i < productConsultations.Rows.Count; i++)
            {
                int productId = Convert.ToInt32(productConsultations.Rows[i]["ProductId"]);
                Hidistro.Entities.Commodities.ProductInfo info=ProductBrowser.GetProduct(currentMember, productId);
                productConsultations.Rows[i]["ThumbnailsUrl"] = info.ThumbnailUrl60;
            }
            this.rptProducts.DataSource = productConsultations;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(productConsultations.Rows.Count.ToString());
            PageTitle.AddSiteNameTitle("商品咨询");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyConsultations.html";
            }
            base.OnInit(e);
        }
    }
}

