namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrders : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater rptOrders;
        private Repeater rptordergifts;//礼品列表
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员订单");
            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
            OrderQuery query = new OrderQuery();
            switch (result)
            {
                case 1:
                    query.Status = OrderStatus.WaitBuyerPay;
                    break;

                case 3:
                    query.Status = OrderStatus.SellerAlreadySent;
                    break;
            }

            DataSet userOrder = MemberProcessor.GetUserOrder(Globals.GetCurrentMemberUserId(), query);
            this.rptOrders = (VshopTemplatedRepeater) this.FindControl("rptOrders");
            this.rptOrders.ItemDataBound += new RepeaterItemEventHandler(this.rptOrders_ItemDataBound);
            this.rptOrders.DataSource = userOrder;
            this.rptOrders.DataBind();
            /*
            string orderids=string.Empty;
            foreach(DataRow dr in userOrder.Tables[0].Rows)
                orderids+=string.Format("{0},",dr["orderid"].ToString());
            orderids=orderids.TrimEnd(',');
            */
        }
        private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                
                /*DataSet userOrderGift = GiftProcessor.GetUserOrderGift(Globals.GetCurrentMemberUserId(),);
                this.rptordergifts = (Repeater)e.Item.Controls[0].FindControl("rptordergifts");
                this.rptordergifts.DataSource = userOrderGift;
                this.rptordergifts.DataBind();*/
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrders.html";
            }
            base.OnInit(e);
        }
    }
}

