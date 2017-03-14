
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.agent
{
    public partial class DutyShift : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDutyInfo();
            }
            this.btnDutyOn.Click += new EventHandler(this.btnDutyOn_Click);
            this.btnDutyOff.Click += new EventHandler(this.btnDutyOff_Click);
        }

        /// <summary>
        /// 获取当前用户当天的打卡信息
        /// </summary>
        private void BindDutyInfo()
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            DataTable dtDuty = ManagerHelper.GetDutyInfo(currentManager.UserId);
            if (dtDuty.Rows.Count > 0)
            {
                string orderTotal = dtDuty.Rows[0]["OrderTotal"] == DBNull.Value ? "0" : ((decimal)dtDuty.Rows[0]["OrderTotal"]).ToString("F2");
                string orderCount = dtDuty.Rows[0]["OrderCount"].ToString();
                string timeLoginIn = ((DateTime)dtDuty.Rows[0]["LoginTime"]).ToString("HH:mm");
                string timeNow = DateTime.Now.ToString("HH:mm");
                string timeMinus = TimeMinus((DateTime)dtDuty.Rows[0]["LoginTime"], DateTime.Now);


                litUserInfo.Text = string.Format("<td>{0}</td><td>{1}</td>", currentManager.UserName, DateTime.Now);
                litTimeInfo.Text = string.Format("<td>{0} - {1}</td> <td id='DutyHours'>{2}</td>", timeLoginIn, timeNow, timeMinus);
                litOrderInfo.Text = string.Format("<td>￥{0}</td>", orderTotal);
                litSaleInfo.Text = string.Format("<td>{0}单</td>", orderCount);
                litSaleInfo.Text += string.Format("<td>￥{0}/{1}单</td>", orderTotal, orderCount);
            }
        }

        private string TimeMinus(DateTime d1, DateTime d2)
        {
            TimeSpan ts = d2.Subtract(d1);
            string timespan = (ts.Days.ToString() == "0" ? "" : ts.Days.ToString() + "天")
            + (ts.Hours.ToString() == "0" ? "" : ts.Hours.ToString() + "小时")
            + (ts.Minutes.ToString() == "0" ? "" : ts.Minutes.ToString() + "分钟");
            return timespan;
        }

        private void btnDutyOn_Click(object sender, System.EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            string msg=ManagerHelper.DutyOn(currentManager.UserId);
            if (msg == "打卡成功")
            {
                this.ShowMsgAndReUrl("打卡成功!", true, "DutyShift.aspx");
            }
            else
            {
                this.ShowMsg(msg, false);
            } 
        }

        private void btnDutyOff_Click(object sender, System.EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            string msg = ManagerHelper.DutyOff(currentManager.UserId);
            if (msg == "关班成功")
            {
                this.ShowMsgAndReUrl("关班成功!", true, "DutyShift.aspx");
            }
            else
            {
                this.ShowMsg(msg, false);
            }
        }

    }

}
