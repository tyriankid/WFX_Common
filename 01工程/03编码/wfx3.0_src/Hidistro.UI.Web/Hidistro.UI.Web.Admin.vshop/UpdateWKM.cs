using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.vshop
{
    public class UpdateWKM : AdminPage
    {
        protected TextBox txtDescription;
        protected TextBox txtShareTitle;
        protected TextBox txtShareDescription;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.HtmlControls.HtmlInputHidden subjectInfo;
        protected System.Web.UI.HtmlControls.HtmlInputHidden optionInfo;

        private Guid activityId;

        protected void btnUpdateWKM_Click(object sender, System.EventArgs e)
        {
            WKMInfo wkmInfo = PromoteHelper.GetWKMInfo(activityId);
            HttpFileCollection files = Request.Files;
            IList<string> sbjList = subjectInfo.Value.Split(';');//题目列
            IList<string> optList = optionInfo.Value.Split(';');//选项列

            wkmInfo.TitleDescription = txtDescription.Text.Trim();
            wkmInfo.ShareTitle = txtShareTitle.Text.Trim();
            wkmInfo.ShareDescription = txtShareDescription.Text.Trim();
            wkmInfo.StartDate = this.calendarStartDate.SelectedDate;
            wkmInfo.EndDate = this.calendarEndDate.SelectedDate;
            for (int i = 0; i < sbjList.Count; i++)
            {
                if (wkmInfo.SubjectInfo.SubjectContent.Count < i + 1 && !string.IsNullOrEmpty(sbjList[i])) //添加了题目
                {
                    wkmInfo.SubjectInfo.SubjectContent.Add(sbjList[i]);
                    wkmInfo.SubjectInfo.WKMSubjectId.Add(Guid.NewGuid());
                    wkmInfo.SubjectInfo.ActivityId.Add(wkmInfo.WKMId);
                    wkmInfo.SubjectInfo.ImgUrl.Add("");

                    wkmInfo.OptionsInfo.Add(new WKMOptionInfo());
                }
                else if (wkmInfo.SubjectInfo.SubjectContent.Count >= i + 1 && !string.IsNullOrEmpty(sbjList[i]))
                {
                    wkmInfo.SubjectInfo.SubjectContent[i] = sbjList[i];
                }
                IList<string> optListPerSbj = optList[i].Split('/');
                for (int j = 0; j < optListPerSbj.Count; j++)
                {
                    if (wkmInfo.OptionsInfo[i].OptionContent.Count<j+1 && !string.IsNullOrEmpty(optListPerSbj[j]))//如果是添加了题目选项
                    {
                        wkmInfo.OptionsInfo[i].OptionContent.Add(optListPerSbj[j]);
                        wkmInfo.OptionsInfo[i].WKMOptionId.Add(Guid.NewGuid());
                        wkmInfo.OptionsInfo[i].TitleId.Add(wkmInfo.SubjectInfo.WKMSubjectId[i]);
                    }
                        
                    else if (wkmInfo.OptionsInfo[i].OptionContent.Count >= j+1 && !string.IsNullOrEmpty(optListPerSbj[j]))
                        wkmInfo.OptionsInfo[i].OptionContent[j]=optListPerSbj[j];
                }
                //图片实体保存到项目内
                if (files[i].ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(wkmInfo.SubjectInfo.ImgUrl[i]) && File.Exists(Server.MapPath(wkmInfo.SubjectInfo.ImgUrl[i])))
                    {
                        File.Delete(Server.MapPath(wkmInfo.SubjectInfo.ImgUrl[i]));//先删除之前的物理图片
                    }
                    string fileName = wkmInfo.SubjectInfo.WKMSubjectId[i] + Path.GetExtension(files[i].FileName);
                    files[i].SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                    wkmInfo.SubjectInfo.ImgUrl[i]=("/Storage/master/topic/" + fileName);
                }
            }
            if (PromoteHelper.UpdateWKM(wkmInfo))
            {
                this.ShowMsgAndReUrl("编辑成功！", true, "ManageWhoKnowMe.aspx");
            }
            else
            {
                this.ShowMsg("编辑失败！", false);
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

        public void pageInit()
        {
            WKMInfo wkmInfo = PromoteHelper.GetWKMInfo(activityId);
            this.txtDescription.Text = wkmInfo.TitleDescription;
            this.txtShareTitle.Text = wkmInfo.ShareTitle;
            this.txtShareDescription.Text = wkmInfo.ShareDescription;
            this.calendarStartDate.SelectedDate = Convert.ToDateTime(wkmInfo.StartDate);
            this.calendarEndDate.SelectedDate = Convert.ToDateTime(wkmInfo.EndDate);
            for(int i=0;i<wkmInfo.SubjectInfo.SubjectContent.Count;i++)
            {
                this.subjectInfo.Value += wkmInfo.SubjectInfo.SubjectContent[i]+";";
                for (int j = 0; j < wkmInfo.OptionsInfo[i].OptionContent.Count;j++ )
                {
                    this.optionInfo.Value += wkmInfo.OptionsInfo[i].OptionContent[j] + "/";
                }
                optionInfo.Value = optionInfo.Value.TrimEnd('/');
                optionInfo.Value += ";";
            }
            subjectInfo.Value = subjectInfo.Value.Trim(';');
            optionInfo.Value = optionInfo.Value.Trim(';');
        }
    }
}
