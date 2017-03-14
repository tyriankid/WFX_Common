using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class RoleAreaPermissions : AdminPage
	{
        protected System.Web.UI.WebControls.Button btnAdd;
        protected DataTable currentArea;//当前用户的所属区域
        protected HiddenField txtRegionScop;//隐藏域,区域id
        protected HiddenField txtRegionScopName;//隐藏域,区域名称
        
        protected int userId;

        [AdministerCheck(true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            if (!this.IsPostBack)
            {
                userId = int.Parse(Request.QueryString["UserId"]);
                currentArea = ManagerHelper.GetManagerArea(userId);//获取当前用户的所属区域
                ViewState["currentArea"] = currentArea;//并存入临时变量内
                string text = "";
                string text2 = "";
                for(int i=0;i<currentArea.Rows.Count;i++)
                {
                    text = text + currentArea.Rows[i]["RegionId"] + ",";
                    text2 = text2 + currentArea.Rows[i]["RegionName"] + ",";
                }
                text = text.TrimEnd(new char[]
			    {
				    ','
			    });
                text2 = text2.TrimEnd(new char[]
			    {
				    ','
			    });
                    this.txtRegionScop.Value = text;
                    this.txtRegionScopName.Value = text2;
                }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool flag = false;
            int num = 0;
            if (!int.TryParse(base.Request.QueryString["UserID"], out num))
            {
                base.Response.Redirect("managers.aspx");
            }
            //将选中的地址转换成数组
            string[] arrayRegionId =txtRegionScop.Value.ToString().Split(',');
            string[] arrayRegionName = txtRegionScopName.Value.ToString().Split(',');
            currentArea = (DataTable)ViewState["currentArea"];

            for (int i = 0; i < arrayRegionId.Length; i++)
            {
                if (currentArea.Select("RegionID = '" + arrayRegionId[i] + "'").Length == 0)
                {
                    DataRow drNew = currentArea.NewRow();
                    drNew["MRegionID"] = Guid.NewGuid();
                    drNew["UserId"] = Request.QueryString["UserId"];
                    drNew["RegionID"] = arrayRegionId[i];
                    drNew["RegionName"] = arrayRegionName[i];
                    currentArea.Rows.Add(drNew);
                }
            }

            int rowsCount = currentArea.Rows.Count;
            //减少处理
            for (int i = 0; i < rowsCount; i++)
            {
                if (this.txtRegionScop.Value.ToString().ToLower().IndexOf(currentArea.Rows[i]["RegionID"].ToString().ToLower()) == -1)
                    currentArea.Rows[i].Delete();
            }
            //开始对数据库进行更新操作
            string str = "";
            if (DataBaseHelper.CommitDataTable(currentArea, "SELECT * from erp_managersregion") != -1)
            {
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存功能配置菜单生效。", "true");
            }
            else
            {
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存功能配置菜单失败！", "false");
            }
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
            

        }


        

	}
}
