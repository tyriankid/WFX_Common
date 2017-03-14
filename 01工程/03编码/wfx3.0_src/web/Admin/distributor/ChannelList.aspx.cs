using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;

namespace Hidistro.UI.Web.Admin.distributor
{
    
    public partial class ChannelList : AdminPage
    {
        private Guid ChannelId;
        private void BindChannelGrade()
        {
            Channel entity = new Channel();
            Globals.EntityCoding(entity, true);
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            DbQueryResult ChannelListRequest = DistributorGradeBrower.GetChannelListGradeRequest(entity);
            this.rptChannellist.DataSource = ChannelListRequest.Data;
            this.rptChannellist.DataBind();
            this.pager.TotalRecords = ChannelListRequest.TotalRecords;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.BindChannelGrade();
            }
        }

        protected void rptChannelList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
               ChannelId = new Guid(e.CommandArgument.ToString());
                switch(e.CommandName)
              {
                    case"del":
                        DistributorsBrower.DeleteChannel(ChannelId);
                        break;
              }
        }

    }
}
