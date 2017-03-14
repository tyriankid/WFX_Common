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
	public class SkinPage : AdminPage
	{
        protected HiddenField txtRes;
        protected Button btnSave;
        protected Panel panelHomePage;
        protected Repeater rptSkin;
        protected HiddenField txtSkinName;
        protected TextBox txtName;
        protected Button btnAdd;
        protected HiddenField txtSkinID;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.rptSkin.ItemCommand += new RepeaterCommandEventHandler(this.rptSkin_ItemCommand);
            if (!IsPostBack)
            {
                BindData();
            }
		}

        private void BindData()
        {
            //绑定二级页风格列表
            DataTable dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Skin");
            rptSkin.DataSource = dt;
            rptSkin.DataBind();

            //默认绑定第一个风格
            
            if (dt.Rows.Count > 0)
            {
                BindPage(dt.Rows[0]["SkinID"]+"");
                txtSkinName.Value=dt.Rows[0]["Name"]+"";
                txtSkinID.Value = dt.Rows[0]["SkinID"]+"";
            }
        }

        private void BindPage(string SkinID)
        {
            panelHomePage.Controls.Clear();
            DataTable dt = DataBaseHelper.GetDataTable("YiHui_HomePage","SkinID='"+SkinID+"'");
            foreach (DataRow dr in dt.Rows)
            {
                BaseModel baseModel = (BaseModel)this.Page.LoadControl("/admin/HomePage/ModelTag/" + dr["ModelCode"] + ".ascx");
                baseModel.PKID = new Guid(dr["PageID"].ToString());//模块的内容ID
                baseModel.PageSN = dr["PageSN"] + "";
                panelHomePage.Controls.Add(baseModel);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text.Trim();
            txtSkinName.Value = Name;
            txtSkinID.Value=Guid.NewGuid()+"";
            DataTable dt = DataBaseHelper.GetDataTable("YiHui_HomePage_Skin","1=2");
            DataRow dr = dt.NewRow();
            dr["SkinID"] = txtSkinID.Value;
            dr["Name"] = Name;
            dr["SkinType"] = 2;
            dt.Rows.Add(dr);
            DataBaseHelper.CommitDataTable(dt, "select * from YiHui_HomePage_Skin where 1=2");
            rptSkin.DataSource = DataBaseHelper.GetDataTable("YiHui_HomePage_Skin");
            rptSkin.DataBind();
            panelHomePage.Controls.Clear();
        }

        public void Save()
        {
            
            string selectSql = string.Format("Select * From YiHui_HomePage Where SkinID='{0}';", txtSkinID.Value);
            selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select PageID From YiHui_HomePage Where SkinID='{0}');", txtSkinID.Value);
            selectSql += string.Format("Select * From YiHui_HomePage Where 1=2;");
            selectSql += string.Format("Select * From YiHui_HomePage_Model Where 1=2");
            DataSet dsData = DataBaseHelper.GetDataSet(selectSql);

            for (int i = 0; i<dsData.Tables[0].Rows.Count; i++)
            {
                dsData.Tables[0].Rows[i].Delete();
            }
            for (int i = 0; i < dsData.Tables[1].Rows.Count; i++)
            {
                dsData.Tables[1].Rows[i].Delete();
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
                    DataRow hpdr = dsData.Tables[2].NewRow();
                    DataRow hpmdr = dsData.Tables[3].NewRow();
                    hpdr["PageType"] = 1000;
                    hpdr["PageSN"] = m;
                    Guid pageid = Guid.NewGuid();
                    hpdr["PageID"] = pageid;
                    hpdr["ModelCode"] = AttrStrs[0];
                    hpmdr["PageID"] = pageid;
                    hpmdr["ModelCode"] = AttrStrs[0];
                    hpmdr["PMID"] = Guid.NewGuid();
                    hpdr["SkinID"] = txtSkinID.Value;
                    hpmdr["SkinID"] = txtSkinID.Value;
                    switch (AttrStrs[0])
                    {
                        case "DianZhao":

                            break;
                        case "DaoHang":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;;//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "WenBen":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "GunDong":
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
                        case "HuanDeng":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "SouSuo":

                            break;
                        case "ShangPin":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "LieBiao":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            hpmdr["PMValue1"] = AttrStrs[7];//背景颜色和文字颜色
                            break;
                        case "KongBai" :
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                           // hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            //hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "BeiJing":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                           // hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            break;
                        case "VDaoHang":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            hpmdr["PMImgDisplay"] = AttrStrs[2] == "none" ? 2 : 1;//是否显示图片
                            hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;;//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            hpmdr["PMValue1"] = AttrStrs[7];//宽度
                            hpmdr["PMValue2"] = AttrStrs[8];//位置 top 和left
                            break;
                        case "VWenBen":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            //hpmdr["PMValue1"] = AttrStrs[7];//宽度
                            hpmdr["PMValue2"] = AttrStrs[8];//位置 top 和left
                            break;
                        case "VGunDong":
                            hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3];//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            //hpmdr["PMValue1"] = AttrStrs[7];//宽度
                            hpmdr["PMValue2"] = AttrStrs[8];//位置 top 和left
                            break;
                        case "VTuPian":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;//是否显示文字
                            hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            //hpmdr["PMValue1"] = AttrStrs[7];//宽度
                            hpmdr["PMValue2"] = AttrStrs[8];//位置 top 和left
                            break;
                        case "VHuanDeng":
                            //hpmdr["PMHeight"] = AttrStrs[1];//高度
                            //hpmdr["PMImgDisplay"]=AttrStrs[2]=="none"?2:1;//是否显示图片
                            //hpmdr["PMTxtDisplay"] = AttrStrs[3] == "none" ? 2 : 1;//是否显示文字
                            //hpmdr["PMStyle"] = AttrStrs[4];//样式
                            //hpmdr["PMTop"] = AttrStrs[5];//显示个数
                            hpmdr["PMContents"] = AttrStrs[6];//内容
                            //hpmdr["PMValue1"] = AttrStrs[7];//宽度
                            hpmdr["PMValue2"] = AttrStrs[8];//位置 top 和left
                            break;
                    }
                    dsData.Tables[2].Rows.Add(hpdr);
                    dsData.Tables[3].Rows.Add(hpmdr);
                    m++;
                }
            }
            DataBaseHelper.CommitDataSet(dsData, selectSql.Split(';'));
            BindPage(txtSkinID.Value);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            int PageType = 0;
            int delType = 11;
            if (currentManager.UserName.Equals("yihui") && Request.QueryString["t"] == "1")
            {
                return;
            }
            if (currentManager.UserName.Equals("yihui") && Request.QueryString["t"] == "2")
            {
                return;
            }
            if (!currentManager.UserName.Equals("yihui") && Request.QueryString["t"] == "2")
            {
                PageType = 21;
                delType = 31;
            }
            if (!currentManager.UserName.Equals("yihui"))
            {
                string selectSql = string.Format("Select * From YiHui_HomePage Where PageType={0};", PageType);
                selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select PageID From YiHui_HomePage Where PageType={0});", PageType);
                selectSql += string.Format("Select * From YiHui_HomePage Where PageType={0};", delType);
                selectSql += string.Format("Select * From YiHui_HomePage_Model Where PageID in(Select PageID From YiHui_HomePage Where PageType={0});", delType);
                selectSql += string.Format("Select * From YiHui_HomePage Where 1=2;");
                selectSql += string.Format("Select * From YiHui_HomePage_Model Where 1=2");
                DataSet dsData = DataBaseHelper.GetDataSet(selectSql);

                for (int i = 0; i < dsData.Tables[2].Rows.Count; i++)
                {
                    dsData.Tables[2].Rows[i].Delete();
                }
                for (int i = 0; i < dsData.Tables[3].Rows.Count; i++)
                {
                    dsData.Tables[3].Rows[i].Delete();
                }
                //dsData table1和2 代表初始模板   table2和3 代表还原时需要删除的数据  4和5 代表需要新增的数据
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    DataRow OldHpmdr = dsData.Tables[1].Select(" PageID='"+dr["PageID"].ToString()+"'")[0];
                    DataRow hpdr = dsData.Tables[4].NewRow();
                    DataRow hpmdr = dsData.Tables[5].NewRow();
                    hpdr.ItemArray = dr.ItemArray;
                    hpmdr.ItemArray = OldHpmdr.ItemArray;
                    Guid pageid = Guid.NewGuid();
                    hpdr["PageID"] = pageid;
                    hpmdr["PageID"] = pageid;
                    hpmdr["PMID"] = Guid.NewGuid();
                    hpdr["PageType"] = delType;
                    dsData.Tables[4].Rows.Add(hpdr);
                    dsData.Tables[5].Rows.Add(hpmdr);
                }
                DataBaseHelper.CommitDataSet(dsData, selectSql.Split(';'));
                BindData();
            }
        }


        protected void rptSkin_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("skin"))
            {
                txtSkinID.Value = e.CommandArgument+"";
                ((LinkButton)e.Item.FindControl("btnSkin")).CssClass = "";
                BindPage(e.CommandArgument+"");
            }else if(e.CommandName.Equals("del")){
                string SkinID = e.CommandArgument+"";
                string StrSql = "select * from YiHui_HomePage_Skin where SkinID='"+SkinID+"';";
                StrSql += "select * from YiHui_HomePage where SkinID='" + SkinID + "';";
                StrSql += "select * from YiHui_HomePage_Model where SkinID='" + SkinID + "'";
                DataSet dsData = DataBaseHelper.GetDataSet(StrSql);
                dsData.Tables[0].PrimaryKey = new DataColumn[] { dsData.Tables[0].Columns["SkinID"] };
                dsData.Tables[1].PrimaryKey = new DataColumn[] { dsData.Tables[1].Columns["PageID"] };
                dsData.Tables[2].PrimaryKey = new DataColumn[] { dsData.Tables[2].Columns["PMID"] };

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
                DataBaseHelper.CommitDataSet(dsData,StrSql.Split(';'));
                BindData();
            }
        }
	}
}
