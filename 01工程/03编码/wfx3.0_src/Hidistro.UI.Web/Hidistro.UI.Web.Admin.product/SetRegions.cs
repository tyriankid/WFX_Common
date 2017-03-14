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
namespace Hidistro.UI.Web.Admin.product
{
    public class SetRegions : AdminPage
	{
        protected System.Web.UI.WebControls.Button btnSave;
        protected DataTable dtProductRegion;//当前用户的所属区域
        protected HiddenField txtRegionScop;//隐藏域,区域id
        protected HiddenField txtRegionScopName;//隐藏域,区域名称
        protected HiddenField txtProductIds;//隐藏域,传递的商品ID集合
        
        protected int userId;

        [AdministerCheck(true)]
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtProductIds.Value = this.Page.Request.QueryString["productIds"];
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            if (!this.IsPostBack)
            {
                //第一次加载暂无内容
            }
        }

        //保存商品区域关系
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //将选中的地址转换成数组
            string[] arrayRegionId = txtRegionScop.Value.ToString().Split(',');
            string[] arrayRegionName = txtRegionScopName.Value.ToString().Split(',');
            //得到选择的商品Id值集合
            string[] arrayProductId = this.txtProductIds.Value.Split(',');

            //构建条件 
            string strSqlwhere = string.Format(@"ProductID in ({0})", this.txtProductIds.Value);

            //得到商品区域关系表结构
            dtProductRegion = DataBaseHelper.GetDataTable("Erp_ProductRegion", strSqlwhere, null);
            //循环构建数据
            if (arrayRegionId.Length == arrayRegionName.Length)
            {
                for (int i = 0; i < arrayProductId.Length; i++)
                {
                    for (int k = 0; k < arrayRegionId.Length; k++)
                    {
                        if (dtProductRegion.Select(string.Format(@"ProductID = '{0}' and RegionID = '{1}'", arrayProductId[i], arrayRegionId[k]), "", DataViewRowState.CurrentRows).Length == 0)
                        {
                            DataRow drNew = dtProductRegion.NewRow();//构建新行
                            drNew["ID"] = Guid.NewGuid();//构建新的Guid值
                            drNew["ProductID"] = arrayProductId[i];//插入商品ID
                            drNew["RegionID"] = arrayRegionId[k];//插入区域ID
                            drNew["RegionName"] = arrayRegionName[k];//插入区域名称
                            dtProductRegion.Rows.Add(drNew);//插入行
                        }
                    }
                }
            }
            //将表提交到数据库
            string str = string.Empty;//定义回调提示语变量
            if (DataBaseHelper.CommitDataTable(dtProductRegion, "SELECT * from Erp_ProductRegion") != -1)
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存商品区域关系生效。", "true");//成功
            else
                str = string.Format("ShowMsg(\"{0}\", {1})", "保存商品区域关系失败！", "false");//失败
            //前端（客户端）弹出提示
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);setTimeout(function(){CloseDialogFrame()},1000);</script>");

        }


        

	}
}
