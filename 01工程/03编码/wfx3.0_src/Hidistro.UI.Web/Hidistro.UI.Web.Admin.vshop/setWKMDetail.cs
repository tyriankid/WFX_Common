using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class setWKMDetail : AdminPage
    {
        protected FileUpload backImgUpload;//活动背景图
        protected Image backImg;
        protected FileUpload logoImgUpload;//logo
        protected Image logoImg;
        protected FileUpload wxImgUpload;//微信分享图标
        protected Image wxImg;
        protected FileUpload adImgUpload1;//广告图1
        protected Image adImg1;
        protected FileUpload adImgUpload2;//广告图2
        protected Image adImg2;
        protected TextBox txtAd1;//广告链接1
        protected TextBox txtAd2;//广告链接2
        protected TextBox txtCopyright;//版权信息
        protected TextBox txtGuidePageUrl;//引导关注页地址

        protected Button deleteAd1;
        protected Button deleteAd2;

        protected System.Web.UI.HtmlControls.HtmlInputHidden mStart;
        protected System.Web.UI.HtmlControls.HtmlInputHidden mEnd;
        protected System.Web.UI.HtmlControls.HtmlInputHidden mDes;
        protected System.Web.UI.HtmlControls.HtmlInputHidden mId;

        private Guid activityId;

        protected void deleteAd1_Click(object sender, System.EventArgs e)
        {
            adImg1.ImageUrl = "";
            txtAd1.Text = "";
        }
        protected void deleteAd2_Click(object sender, System.EventArgs e)
        {
            adImg2.ImageUrl = "";
            txtAd2.Text = "";
        }

        protected void btnAddWKM_Click(object sender, System.EventArgs e)
        {
            //保存背景图
            string backimgUrl = backImg.ImageUrl;
            if (!string.IsNullOrEmpty(backImgUpload.FileName) && backImgUpload.FileContent.Length > 0 )
            {
                string fileName = activityId + Path.GetExtension(backImgUpload.FileName);
                backImgUpload.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                backimgUrl=("/Storage/master/topic/" + fileName);
            }
            else if (string.IsNullOrEmpty(backImg.ImageUrl))
            {
                backimgUrl=("");
            }

            //保存logo
            string logoImgUrl = logoImg.ImageUrl;
            if (!string.IsNullOrEmpty(logoImgUpload.FileName) && logoImgUpload.FileContent.Length > 0)
            {
                string fileName = activityId + "logo_" + Path.GetExtension(logoImgUpload.FileName);
                logoImgUpload.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                logoImgUrl = ("/Storage/master/topic/" + fileName);
            }
            else if (string.IsNullOrEmpty(logoImg.ImageUrl))
            {
                logoImgUrl = "";
            }

            //保存微信分享图标
            string wxImgUrl = wxImg.ImageUrl;
            if (!string.IsNullOrEmpty(wxImgUpload.FileName) && wxImgUpload.FileContent.Length > 0)
            {
                string fileName = activityId + "WX_" + Path.GetExtension(wxImgUpload.FileName);
                wxImgUpload.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                wxImgUrl = ("/Storage/master/topic/" + fileName);
            }
            else if (string.IsNullOrEmpty(wxImg.ImageUrl))
            {
                wxImgUrl = "";
            }
            
            //保存广告图1
            IList<string> adImgUrlList=new List<string>();
            IList<string> adLinkList = new List<string>();
            if (!string.IsNullOrEmpty(adImgUpload1.FileName) && adImgUpload1.FileContent.Length > 0 )//&& !string.IsNullOrEmpty(txtAd1.Text.Trim()))
            {
                string fileName = activityId +"ad1" + Path.GetExtension(adImgUpload1.FileName);
                adImgUpload1.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                adImgUrlList.Add("/Storage/master/topic/" + fileName);
                adLinkList.Add(txtAd1.Text.Trim());
            }
            else if (string.IsNullOrEmpty(adImg1.ImageUrl))
            {
                adLinkList.Add("");
                adImgUrlList.Add("");
            }
            //保存广告图2
            if (!string.IsNullOrEmpty(adImgUpload2.FileName) && adImgUpload2.FileContent.Length > 0 )//&& !string.IsNullOrEmpty(txtAd2.Text.Trim()) )
            {
                string fileName = activityId + "ad2" + Path.GetExtension(adImgUpload2.FileName);
                adImgUpload2.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                adImgUrlList.Add("/Storage/master/topic/" + fileName);
                adLinkList.Add(txtAd2.Text.Trim());
            }
            else if (string.IsNullOrEmpty(adImg2.ImageUrl))
            {
                adLinkList.Add("");
                adImgUrlList.Add("");
            }
            //保存匹配度区间描述信息
            IList<string> mStartList=mStart.Value.Split(';');
            IList<string> mEndList = mEnd.Value.Split(';');
            IList<string> mDesList = mDes.Value.Split(';');
            IList<string> mIdList = mId.Value.Split(';');

            DataTable dt1 = PromoteHelper.getWKMMatchInfoList(activityId);
            dt1.PrimaryKey = new DataColumn[] { dt1.Columns["id"] };
            DataTable dt2 = dt1.Clone();
            dt2.PrimaryKey = new DataColumn[] { dt2.Columns["id"] };
            if (mId.Value != "")
            {
                for (int i = 0; i < mIdList.Count; i++)
                {
                    DataRow drNew = dt2.NewRow();
                    drNew["id"] = mIdList[i];
                    drNew["matchRateStart"] = mStartList[i];
                    drNew["matchRateEnd"] = mEndList[i];
                    drNew["Description"] = mDesList[i];
                    drNew["activityId"] = activityId;
                    dt2.Rows.Add(drNew);
                }
            }

            int count = dt1.Rows.Count;//DB
            dt1.PrimaryKey = new DataColumn[] { dt1.Columns["ID"] };
            dt2.PrimaryKey = new DataColumn[] { dt2.Columns["ID"] };
            dt1 = DataBaseHelper.GetDtDifferent(dt1, dt2);//根据两个表的不同生成新的dt1,用于整表提交


            if (PromoteHelper.setGuidePageUrl(txtGuidePageUrl.Text, activityId) && PromoteHelper.addWxImgUrl(activityId, wxImgUrl) && PromoteHelper.addLogoImgUrl(activityId, logoImgUrl) && PromoteHelper.setWKMCopyRight(txtCopyright.Text, activityId) && PromoteHelper.addBackImgUrl(activityId, backimgUrl) && PromoteHelper.addAdImgUrl(activityId, adImgUrlList, adLinkList) && DataBaseHelper.CommitDataTable(dt1, "SELECT * from WKM_MatchInfo where activityid='" + activityId + "'") != -1)//PromoteHelper.setMatchInfoList(activityId, mStartList, mEndList, mDesList)) //保存背景图片地址
                {
                    this.ShowMsgAndReUrl("保存成功！", true, "ManageWhoKnowMe.aspx");
                }
                else
                {
                    this.ShowMsg("保存失败！", false);
                }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            activityId = new Guid(base.Request.QueryString["id"]);
            
            if (!this.Page.IsPostBack)
            {
                this.pageInit();
            }
        }

        private void pageInit()
        {
            //匹配度列表
            DataTable dtMatchInfoList = PromoteHelper.getWKMMatchInfoList(activityId);
            foreach (DataRow row in dtMatchInfoList.Rows)
            {
                mStart.Value += row["MatchRateStart"].ToString()+";";
                mEnd.Value += row["MatchRateEnd"].ToString() + ";";
                mDes.Value += row["Description"].ToString() + ";";
                mId.Value += row["Id"].ToString() + ";";
            }
            mStart.Value = mStart.Value.TrimEnd(';');
            mEnd.Value = mEnd.Value.TrimEnd(';');
            mDes.Value = mDes.Value.TrimEnd(';');
            mId.Value = mId.Value.TrimEnd(';');

            //广告图片内容
            DataTable dtAdImgs = PromoteHelper.getAdImgAndUrls(activityId);
            adImg1.ImageUrl = dtAdImgs.Rows[0]["adImgUrl1"].ToString();
            txtAd1.Text = dtAdImgs.Rows[0]["adLink1"].ToString();
            adImg2.ImageUrl = dtAdImgs.Rows[0]["adImgUrl2"].ToString();
            txtAd2.Text = dtAdImgs.Rows[0]["adLink2"].ToString();

            //活动背景图和logo和wx分享图
            DataTable dtImg = PromoteHelper.getBackImgUrl(activityId);
            backImg.ImageUrl = dtImg.Rows[0]["backImgUrl"].ToString();
            logoImg.ImageUrl = dtImg.Rows[0]["logoUrl"].ToString();
            wxImg.ImageUrl = dtImg.Rows[0]["ShareImgUrl"].ToString();

            //copyright信息和guidepageurl
            txtCopyright.Text = PromoteHelper.getWKMCopyRight(activityId);
            txtGuidePageUrl.Text = PromoteHelper.getGuidePageUrl(activityId);
        }
    }
}
