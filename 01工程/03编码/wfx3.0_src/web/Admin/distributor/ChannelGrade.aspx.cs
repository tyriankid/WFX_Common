
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{		
    public partial class ChannelGrade : AdminPage
	{
        private Guid channelId;
		private void BaindCahnnel()
		{

            Channel ChannelInfo = DistributorGradeBrower.GetChannelListGrade(channelId);
            if (ChannelInfo != null)
            {
                this.txtRemark.Text = ChannelInfo.Remark;
                this.txtName.Text = ChannelInfo.ChannelName;
            }
		}
        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            if (channelId == Guid.Empty)
            {
                //增加;
                Channel channel = new Channel();
                channel.ChannelName = this.txtName.Text;
                channel.Remark = this.txtRemark.Text;
                if (DistributorGradeBrower.InsertChannelList(channel))
                {
                    this.ShowMsg("增加成功！", true);
                }
                else {
                    this.ShowMsg("增加失败！", false);
                }
            }
            else { 
                //修改
                Channel ChannelInfo = DistributorGradeBrower.GetChannelListGrade(channelId);
                ChannelInfo.Remark = this.txtName.Text;
                ChannelInfo.ChannelName = this.txtRemark.Text;
                if (DistributorGradeBrower.UpdateChannelList(ChannelInfo))
                {
                    this.ShowMsg("修改成功！", true);
                }
                else {
                    this.ShowMsg("修改失败！", true);
                }
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (this.Page.Request.QueryString["Id"] != null)
            {
                channelId =new Guid(this.Page.Request.QueryString["Id"]);
            }
			this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
			if (!this.Page.IsPostBack)
			{
                this.BaindCahnnel();
			}
		}
	}
}