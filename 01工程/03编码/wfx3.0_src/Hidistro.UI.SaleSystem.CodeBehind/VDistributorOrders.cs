namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Config;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorOrders : VWeiXinOAuthTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private VshopTemplatedRepeater vshoporders;

        protected override void AttachChildControls()
        {
            this.vshoporders = (VshopTemplatedRepeater) this.FindControl("vshoporders");
            this.litfinishnum = (Literal) this.FindControl("litfinishnum");
            this.litallnum = (Literal) this.FindControl("litallnum");
            PageTitle.AddSiteNameTitle("店铺订单");
            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
            DistributorsInfo currentDistributor = DistributorsBrower.GetCurrentDistributors();
            int SelectAgentId=0;
            if (currentDistributor.IsAgent == 1 && CustomConfigHelper.Instance.SelectServerAgent)
            {
                SelectAgentId = currentDistributor.UserId;
            }
            OrderQuery query = new OrderQuery {
                UserId = new int?(Globals.GetCurrentMemberUserId()),
                selectAgentId = SelectAgentId,
            };
            if (result != 5)
            {
                query.Status = OrderStatus.Finished;
                this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                query.Status = OrderStatus.All;
                this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
            }
            else
            {
                this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                query.Status = OrderStatus.Finished;
                this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
            }
            this.vshoporders.ItemDataBound += new RepeaterItemEventHandler(this.vshoporders_ItemDataBound);
            DataSet dsData = DistributorsBrower.GetDistributorOrder(query);
            SetCloseOrderState(dsData);
            this.vshoporders.DataSource = dsData;
            this.vshoporders.DataBind();

        }

        /// <summary>
        /// 关闭状态的订单,设置基订单明细状态。
        /// 隐藏[改价]按钮
        /// </summary>
        private void SetCloseOrderState(DataSet dsData)
        {
            DataRow[] rows = dsData.Tables[0].Select("OrderStatus=4");
            foreach (DataRow dr in rows)
            {
                foreach (DataRow row in dr.GetChildRows("OrderItems"))
                {
                    row["OrderItemsStatus"] = dr["OrderStatus"];
                }
            }
        }


        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorOrders.html";
            }
            base.OnInit(e);
        }

        private void vshoporders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable table = ((DataView) DataBinder.Eval(e.Item.DataItem, "OrderItems")).ToTable();
            //增加判断,如果table有数据,才会开始转换
            if (table.Rows.Count > 0)
            {
                decimal num = (decimal)table.Compute("sum(ItemAdjustedCommssion)", "true");
                decimal num2 = (decimal)table.Compute("sum(itemsCommission)", "true");
                FormatedMoneyLabel label = (FormatedMoneyLabel)e.Item.Controls[0].FindControl("lbladjustsum");
                DataRow drOrder = ((DataRowView)e.Item.DataItem).Row;
                label.Text = num.ToString("F2");
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litCommission");
                if (CustomConfigHelper.Instance.SelectServerAgent)
                {
                    literal.Text = (10 / 100M * Convert.ToDecimal(drOrder["Amount"].ToString())).ToString("F2");
                }
                else{
                    literal.Text = (num2 - num).ToString("F2");
                }
                

                Literal litSendOrderGoods = (Literal)e.Item.Controls[0].FindControl("litSendOrderGoods");
                if (drOrder["OrderStatus"].ToString() == "2")
                {
                    string strModeName = (drOrder["ModeName"] != DBNull.Value) ? drOrder["ModeName"].ToString() : "";
                    string strRealModeName = (drOrder.Table.Columns.Contains("RealModeName") && drOrder["RealModeName"] != DBNull.Value) ? drOrder["RealModeName"].ToString() : "";
                    if (strRealModeName.IndexOf("自提") > -1 || (strRealModeName == "" && strModeName.IndexOf("自提") > -1))
                    {
                        litSendOrderGoods.Visible = true;
                        litSendOrderGoods.Text = string.Format("<a class='link link-color' role='button' onclick=SendOrderGoods('{0}','{1}')>发货</a>"
                            , drOrder["OrderId"].ToString(), drOrder["ShippingModeId"].ToString());
                    }
                }
            }
            else
            {
                Literal literal = (Literal)e.Item.Controls[0].FindControl("litCommission");
                literal.Text ="0";
            }
        }
    }
}

