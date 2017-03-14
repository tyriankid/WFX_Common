using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Core;
using Hidistro.UI.ControlPanel.Utility;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class jumpPage : AdminPage
	{
        protected System.Web.UI.HtmlControls.HtmlInputHidden adminType;
        protected System.Web.UI.HtmlControls.HtmlInputHidden isDutyExist;
        protected Button btnQuickDutyOn;

        public int loginId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                loginId = currentManager.UserId;
                string currentRolename = ManagerHelper.GetRole(currentManager.RoleId).RoleName;
                switch (Page.Request.QueryString["action"])
                {   
                    case "sswk"://爽爽挝啡,跳转到pc点餐页面
                        switch (currentRolename)
                        {
                            case "超级管理员":
                            case "门店发货":
                                adminType.Value="admin";
                                break;
                            case "活动账号":
                                adminType.Value="activity";
                                break;
                        }
                        break;
                }
                if (adminType.Value != "admin")
                {
                    isDutyExist.Value = ManagerHelper.isDutyExist(currentManager.UserId) ? "1" : "0";
                }
                else
                {
                    isDutyExist.Value = "1";
                }
			}
            this.btnQuickDutyOn.Click += new System.EventHandler(this.btnDutyOn_Click);
		}

        private void btnDutyOn_Click(object sender, System.EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            string msg = ManagerHelper.DutyOn(currentManager.UserId);
            if (msg == "打卡成功")
            {
                this.ShowMsgAndReUrl("打卡成功!",true,Request.Url.AbsoluteUri);
            }
            else
            {
                this.ShowMsg(msg, false);
            }
        }
	}
}
