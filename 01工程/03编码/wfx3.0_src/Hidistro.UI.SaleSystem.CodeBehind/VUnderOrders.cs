namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VUnderOrders : VWeiXinOAuthTemplatedWebControl
    {
        private Literal orderTotalPrice;//下属订单总额
        private Literal orderTotalCount;//下属订单总数
        private Literal underDistributorsCount;//下属总数
        private VshopTemplatedRepeater vshoporders;
        private HtmlInputHidden txtTotal;


        protected override void AttachChildControls()
        {
            this.orderTotalPrice = (Literal)this.FindControl("orderTotalPrice");//下属订单总额
            this.orderTotalCount = (Literal)this.FindControl("orderTotalCount");//下属订单总数
            this.underDistributorsCount = (Literal)this.FindControl("underDistributorsCount");//下属总数
            this.vshoporders = (VshopTemplatedRepeater) this.FindControl("vshoporders");
            this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
            //当前分销商信息
            DistributorsInfo currentDistributor = null;
            try
            {
                currentDistributor = DistributorsBrower.GetUserIdDistributors(MemberProcessor.GetCurrentMember().UserId);
            }
            catch //如果当前用户未登录或者是不是分销商引发为空异常,则跳转到个人中心
            {
                this.Page.Response.Redirect("MemberCenter.aspx");
            }
            PageTitle.AddSiteNameTitle("店铺订单");
            int result = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);

            int num2;
            int num3;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 5;
            }
            OrderQuery query = new OrderQuery {
                UserId = currentDistributor.UserId,
                PageIndex = num2,
                PageSize = num3,
                SortBy="OrderDate ",
                SortOrder = SortAction.Desc,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus),result.ToString()),
                customKeyword = new System.Collections.Generic.List<string> { },
            };
            if (this.Page.Request.QueryString["status"] != null || this.Page.Request.QueryString["keywords"] != null)
            {
                query.StartDate = null;
                query.EndDate = null;
            }

            //获取搜索关键字
            if (this.Page.Request.QueryString["keywords"] != null)
            {

                string[] keywords = this.Page.Request.QueryString["keywords"].ToString().Split(',');
                for (int i = 0; i < keywords.Length; i++)
                {
                    int result1;
                    if (keywords[i].Length == 15 && int.TryParse(keywords[0], out result1))//如果长度是15位并且是数字就是订单
                        query.OrderId = keywords[0];
                    else
                    {
                        query.customKeyword.Add(keywords[i]);//不然的话加入自定义查找列表内
                    }
                }
            }
            //获取搜索日期区间
            if (this.Page.Request.QueryString["dateStart"] != null)
            {
                DateTime dateStart = DateTime.MinValue;
                DateTime.TryParse(this.Page.Request.QueryString["dateStart"].ToString(), out dateStart);
                query.StartDate = dateStart;
                
            }
            if (this.Page.Request.QueryString["dateEnd"] != null)
                query.EndDate = Convert.ToDateTime(this.Page.Request.QueryString["dateEnd"].ToString());

            
            DbQueryResult userRedPagerList = DistributorsBrower.GetUnderOrders(query);
            //循环获取上一级分销商的名字
            
            foreach(DataRow row in ((DataTable)userRedPagerList.Data).Rows)
            {
                DistributorsInfo parentDistributor = DistributorsBrower.GetDistributorInfo(int.Parse(row["referralUserId"].ToString()));//上一级分销商信息
                if (parentDistributor != null)
                    row["ParentName"] = parentDistributor.StoreName.ToString();
                else
                    row["ParentName"] = "暂无上级负责人";
            }

            //开始绑定
            this.vshoporders.DataSource = userRedPagerList.Data;
            this.txtTotal.SetWhenIsNotNull(userRedPagerList.TotalRecords.ToString());
            //订单总数
            this.orderTotalCount.Text = userRedPagerList.TotalRecords.ToString();
            query.PageIndex = 1;
            query.PageSize = int.MaxValue;
            //订单金额总数
            DataTable total = (DataTable)DistributorsBrower.GetUnderOrders(query).Data;
            decimal totalPrice = 0M;
            for (int i = 0; i < total.Rows.Count; i++)
            {
                totalPrice += decimal.Parse(total.Rows[i]["OrderTotal"].ToString());
            }
            this.orderTotalPrice.Text=totalPrice.ToString("0.00");
            //下属总数
            DistributorsQuery dquery = new DistributorsQuery
            {
                PageIndex = 1,
                PageSize = 0x2710,
                AgentPath = currentDistributor.UserId.ToString(),
                GradeId = 3,//所有分销商
            };

            this.underDistributorsCount.Text =  DistributorsBrower.GetDownDistributorsAndAgents(dquery).Rows.Count.ToString();
            this.vshoporders.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-UnderOrders.html";
            }
            base.OnInit(e);
        }

        private void vshoporders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
           
        }
    }
}

