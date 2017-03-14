namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Members;
    using ControlPanel.Sales;
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
    public class VStatistics : VWeiXinOAuthTemplatedWebControl
    {
        private Literal visitCount;
        private Literal memberCount;
        private Literal memberAdd;
        private Literal orderCount;
        private Literal orderToday;
        private Literal visitCountToday;
        private HtmlInputHidden hiddenIsAgent;//是否代理商
        private VshopTemplatedRepeater vStatisticsList;//分销商信息列表

        protected override void AttachChildControls()
        {
            this.visitCount = (Literal)this.FindControl("visitCount");//访问次数
            this.memberCount = (Literal)this.FindControl("memberCount");//会员总数
            this.memberAdd = (Literal)this.FindControl("memberAdd");//今日新增会员
            this.orderCount = (Literal)this.FindControl("orderCount");//订单总数
            this.orderToday = (Literal)this.FindControl("orderToday");//今日订单
            this.visitCountToday = (Literal)this.FindControl("visitCountToday");//今日访问次数
            this.hiddenIsAgent = (HtmlInputHidden)this.FindControl("hiddenIsAgent");//是否代理商
            this.vStatisticsList = (VshopTemplatedRepeater)this.FindControl("vStatisticsList");
            //绑定数据
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DistributorsInfo currentDistributor = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);


            if (currentDistributor == null)
                return;

            if (currentDistributor.IsAgent == 0)
            {
                hiddenIsAgent.Value = "0";
                //访问次数
                string visitCount = DistributorsBrower.GetDistributorVisitCount(currentMember.UserId).ToString();
                this.visitCount.Text = string.Format("<li><i>{0}</i><p>店铺访问量</p></li>", visitCount);
                //会员的总数
                string memberCount = DistributorsBrower.GetDistributorMemberCount(currentMember.UserId).ToString();
                this.memberCount.Text = string.Format("<li><i>{0}</i><p>我的会员总数</p></li>", memberCount);
                //今日新增会员
                string memberAddCount = DistributorsBrower.GetDistributorMemberCount(currentMember.UserId, DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                this.memberAdd.Text = string.Format("<li><i>{0}</i><p>今日新增会员</p></li>", memberAddCount);
                //订单总数
                OrderQuery query = new OrderQuery
                {
                    UserId = new int?(Globals.GetCurrentMemberUserId())
                };
                string orderCount = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                this.orderCount.Text = string.Format("<li><i>{0}</i><p>订单总数</p></li>", orderCount);
                //今日订单
                query.Status = OrderStatus.Today;
                string orderToday = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                this.orderToday.Text = string.Format("<li><i>{0}</i><p>今日订单</p></li>", orderToday);
                //今日点击量
                string visitCountT = DistributorsBrower.GetDistributorVisitCount(currentMember.UserId, DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                this.visitCountToday.Text = string.Format("<li><i>{0}</i><p>今日访问量</p></li>", visitCountT);
            }
            else if (currentDistributor.IsAgent == 1)//代理商载入列表页
            {
                hiddenIsAgent.Value = "1";
                DistributorsQuery queryAgent = new DistributorsQuery
                {
                    PageIndex = 1,
                    PageSize = int.MaxValue,
                    UserId = currentDistributor.UserId,
                    GradeId = 3,
                    AgentPath = currentDistributor.UserId.ToString(),
                };
                DataTable all = DistributorsBrower.GetDownDistributorsAndAgents(queryAgent);//DistributorsBrower.GetAgentDistributorsVisitInfo(currentDistributor.UserId);
                DataTable infoList=new DataTable();
                infoList.Columns.Add("name", typeof(string));
                infoList.Columns.Add("visitCount",typeof(string));
                infoList.Columns.Add("memberCount",typeof(string));
                infoList.Columns.Add("memberAddCount",typeof(string));
                infoList.Columns.Add("orderCount",typeof(string));
                infoList.Columns.Add("orderToday",typeof(string));
                infoList.Columns.Add("visitCountT",typeof(string));
                
                for(int i=0;i<all.Rows.Count;i++)
                {
                    int currentDistributorId=all.Rows[i]["userId"].ToInt();
                    string visitCount = DistributorsBrower.GetDistributorVisitCount(currentDistributorId).ToString();
                    string memberCount = DistributorsBrower.GetDistributorMemberCount(currentDistributorId).ToString();
                    string memberAddCount = DistributorsBrower.GetDistributorMemberCount(currentDistributorId, DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                    OrderQuery query = new OrderQuery
                    {
                        UserId = currentDistributorId
                    };
                    string orderCount = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    query.Status = OrderStatus.Today;
                    string orderToday = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    string visitCountT = DistributorsBrower.GetDistributorVisitCount(currentDistributorId, DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                    string name = all.Rows[i]["StoreName"].ToString();
                    infoList.Rows.Add(name,visitCount, memberCount, memberAddCount, orderCount, orderToday, visitCountT);
                }
                vStatisticsList.DataSource = infoList;
                vStatisticsList.DataBind();
            }

        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Statistics.html";
            }
            base.OnInit(e);
        }

        private void vshoporders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}

