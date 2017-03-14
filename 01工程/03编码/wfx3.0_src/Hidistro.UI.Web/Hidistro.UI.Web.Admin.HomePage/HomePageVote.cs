using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class HomePageVote : AdminPage
	{
        protected HiddenField txtRes;
        protected Button btnSave;
        protected Button btnReset;
        protected Panel panelHomePage;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            if (!IsPostBack)
            {
                BindData();
            }
		}

        private void BindData()
        {
            panelHomePage.Controls.Clear();

            string strVoteId = Request.QueryString["VoteId"].ToString();

            //唯一值 ，投票活动ID
            string selectSql = string.Format("Select * From Yihui_Votes_Model Where VoteId={0} order by ModelSN", strVoteId);
            DataSet ds = DataBaseHelper.GetDataSet(selectSql);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                baseModel.PKID = new Guid(dr["VMID"].ToString());//模块的内容ID
                baseModel.PageSN = dr["ModelSN"] + "";
                panelHomePage.Controls.Add(baseModel);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            //开始保存
            //得到投票活动Id参数值
            string strVoteId = Request.QueryString["VoteId"].ToString();

            string selectSql = string.Format("Select * From Yihui_Votes_Model Where VoteId={0};", strVoteId);
            selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select VMID From Yihui_Votes_Model Where VoteId={0});", strVoteId);
            selectSql += string.Format("Select * From YiHui_Votes_Model_Detail Where VMID in(Select VMID From Yihui_Votes_Model Where VoteId={0});", strVoteId);
            selectSql += string.Format("Select * From Yihui_Votes_Model Where 1=2;");
            selectSql += string.Format("Select * From YiHui_HomePage_Model Where 1=2;");
            selectSql += string.Format("Select * From YiHui_Votes_Model_Detail Where 1=2;");
            selectSql += string.Format("Select * From YiHui_Votes_Model_Result Where VoteId={0}", strVoteId);
            DataSet dsData = DataBaseHelper.GetDataSet(selectSql);

            //清空投票模块表，及模块内容表
            for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
            {
                dsData.Tables[0].Rows[i].Delete();
            }
            for (int i = 0; i < dsData.Tables[1].Rows.Count; i++)
            {
                dsData.Tables[1].Rows[i].Delete();
            }
            for (int i = 0; i < dsData.Tables[2].Rows.Count; i++)
            {
                dsData.Tables[2].Rows[i].Delete();
            }
            for (int i = 0; i < dsData.Tables[6].Rows.Count; i++)
            {
                dsData.Tables[6].Rows[i].Delete();
            }

            string str = txtRes.Value;
            if (str != "")
            {
                str = str.TrimEnd('○');
                string[] strModels = str.Split('○');
                int m = 1;
                foreach (string modelstr in strModels)
                {
                    string[] AttrStrs = modelstr.Split('●');
                    DataRow vmdr = dsData.Tables[3].NewRow();
                    DataRow hpmdr = dsData.Tables[4].NewRow();
                    //构建Votes_Model表
                    Guid vmid = Guid.NewGuid();
                    vmdr["VMID"] = vmid;
                    vmdr["VoteId"] = strVoteId;
                    vmdr["ModelCode"] = AttrStrs[0];
                    vmdr["ModelSN"] = m;
                    //构建Page_Model表
                    hpmdr["PMID"] = Guid.NewGuid();
                    hpmdr["PageID"] = vmid;
                    hpmdr["ModelCode"] = AttrStrs[0];
                    hpmdr["PMID"] = Guid.NewGuid();
                    switch (AttrStrs[0])
                    {
                        case "WenBen":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "TuPian":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "ShuRuKuang":
                            //hpmdr["PMHeight"] = AttrStrs[1];
                            //hpmdr["PMImgDisplay"] = AttrStrs[2];
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];
                            hpmdr["PMStyle"] = AttrStrs[4];
                            hpmdr["PMTop"] = AttrStrs[5];
                            hpmdr["PMContents"] = AttrStrs[6];
                            break;
                        case "ShiJian":
                            //hpmdr["PMHeight"] = AttrStrs[1];
                            //hpmdr["PMImgDisplay"] = AttrStrs[2];
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];
                            //hpmdr["PMStyle"] = AttrStrs[4];
                            hpmdr["PMTop"] = AttrStrs[5];
                            hpmdr["PMContents"] = AttrStrs[6];
                            break;
                        case "XuanXiang":
                            //hpmdr["PMHeight"] = AttrStrs[1];
                            //hpmdr["PMImgDisplay"] = AttrStrs[2];
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];
                            hpmdr["PMStyle"] = AttrStrs[4];
                            hpmdr["PMTop"] = AttrStrs[5];
                            hpmdr["PMContents"] = AttrStrs[6];

                            //保存选项明细
                            string[] strDetail = AttrStrs[6].Split('♦');
                            if (strDetail.Length == 3)
                            {
                                string[] strItems = strDetail[2].Split('♢');
                                foreach (string strItem in strItems)
                                {
                                    string[] strValues = strItem.Split('□');
                                    if (strValues.Length == 3)
                                    {
                                        DataRow vmddr = dsData.Tables[5].NewRow();
                                        vmddr["MDID"] = Guid.NewGuid();
                                        vmddr["VMID"] = vmid;
                                        vmddr["Value"] = strValues[1];
                                        vmddr["Name"] = strValues[1];
                                        vmddr["Scode"] = Convert.ToInt32(strValues[0]);
                                        dsData.Tables[5].Rows.Add(vmddr);
                                    }
                                }
                            }
                            break;
                    }
                    dsData.Tables[3].Rows.Add(vmdr);
                    dsData.Tables[4].Rows.Add(hpmdr);
                    m++;
                }
            }
            DataBaseHelper.CommitDataSet(dsData, selectSql.Split(';'));
            BindData();
        }

        //重置
        protected void btnReset_Click(object sender, EventArgs e)
        {
            BindData();
        //    string strVoteId = Request.QueryString["VoteId"].ToString();
        //    if (!string.IsNullOrEmpty(strVoteId))
        //    {
        //        string selectSql = string.Format("Select * From Yihui_Votes_Model Where VoteId={0};", strVoteId);
        //        selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select VMID From Yihui_Votes_Model Where VoteId={0});", strVoteId);
        //        selectSql += string.Format("Select * From Yihui_Votes_Model Where VoteId={0};", strVoteId);
        //        selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select VMID From Yihui_Votes_Model Where VoteId={0});", strVoteId);
        //        selectSql += string.Format("Select * From Yihui_Votes_Model Where 1=2;");
        //        selectSql += string.Format("Select * From YiHui_HomePage_Model Where 1=2");
        //        DataSet dsData = DataBaseHelper.GetDataSet(selectSql);

        //        for (int i = 0; i < dsData.Tables[2].Rows.Count; i++)
        //        {
        //            dsData.Tables[2].Rows[i].Delete();
        //        }
        //        for (int i = 0; i < dsData.Tables[3].Rows.Count; i++)
        //        {
        //            dsData.Tables[3].Rows[i].Delete();
        //        }
        //        //dsData table1和2 代表初始模板   table2和3 代表还原时需要删除的数据  4和5 代表需要新增的数据
        //        foreach (DataRow dr in dsData.Tables[0].Rows)
        //        {
        //            DataRow OldHpmdr = dsData.Tables[1].Select(" PageID='" + dr["VMID"].ToString() + "'")[0];
        //            DataRow hpdr = dsData.Tables[4].NewRow();
        //            DataRow hpmdr = dsData.Tables[5].NewRow();
        //            hpdr.ItemArray = dr.ItemArray;
        //            hpmdr.ItemArray = OldHpmdr.ItemArray;
        //            Guid pageid = Guid.NewGuid();
        //            hpdr["VMID"] = pageid;
        //            hpmdr["PageID"] = pageid;
        //            hpmdr["PMID"] = Guid.NewGuid();
        //            hpdr["VoteId"] = new Guid(strVoteId);
        //            dsData.Tables[4].Rows.Add(hpdr);
        //            dsData.Tables[5].Rows.Add(hpmdr);
        //        }
        //        DataBaseHelper.CommitDataSet(dsData, selectSql.Split(';'));
        //        BindData();
        //    }
        }
	}
}
