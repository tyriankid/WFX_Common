
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.agent
{
    public partial class DutyShiftList : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnQueryLogs.Click += new System.EventHandler(this.btnQueryLogs_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindDutyInfo();
            }
        }

        /// <summary>
        /// 获取当前用户当天的打卡信息
        /// </summary>
        private void BindDutyInfo()
        {
            DateTime? dateStart=null;
            DateTime? dateEnd=null;
            int managerId = 0;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
            {
                dateStart = Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
            {
                dateEnd = Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["managerId"]))
            {
                managerId = Convert.ToInt16(base.Server.UrlDecode(this.Page.Request.QueryString["managerId"]));
            }
            dlstDutyList.DataSource = ManagerHelper.GetDutyInfoList(dateStart, dateEnd, managerId);
            dlstDutyList.DataBind();
        }

        private void btnQueryLogs_Click(object sender, System.EventArgs e)
        {
            this.ReloadManagerLogs(true);
        }

        private void ReloadManagerLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.calenderFromDate.SelectedDate.HasValue)
            {
                queryStrings.Add("dateStart", this.calenderFromDate.SelectedDate.ToString());
            }
            if (this.calenderToDate.SelectedDate.HasValue)
            {
                queryStrings.Add("dateEnd", this.calenderToDate.SelectedDate.ToString());
            }
            base.ReloadPage(queryStrings);
        }


    }

}
