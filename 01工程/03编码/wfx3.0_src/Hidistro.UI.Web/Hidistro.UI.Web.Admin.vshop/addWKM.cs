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
    public class addWKM : AdminPage
    {
        protected TextBox txtDescription;
        protected TextBox txtShareTitle;
        protected TextBox txtShareDescription;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        protected System.Web.UI.HtmlControls.HtmlInputHidden subjectInfo;
        protected System.Web.UI.HtmlControls.HtmlInputHidden optionInfo;

        protected void btnAddWKM_Click(object sender, System.EventArgs e)
        {
            HttpFileCollection files = Request.Files;
           
            IList<string> sbjList = subjectInfo.Value.Split(';');//题目列
            IList<string> optList = optionInfo.Value.Split(';');//选项列
            WKMInfo wkmInfo = new WKMInfo();
                wkmInfo.WKMId = Guid.NewGuid();
                wkmInfo.TitleDescription = txtDescription.Text.Trim();
                wkmInfo.ShareTitle = txtShareTitle.Text.Trim();
                wkmInfo.ShareDescription = txtShareDescription.Text.Trim();
                wkmInfo.StartDate = Convert.ToDateTime(calendarStartDate.Text);
                wkmInfo.EndDate = Convert.ToDateTime(calendarEndDate.Text);
            //WKMSubjectInfo wkmSubjectInfo = new WKMSubjectInfo();
            //IList<WKMOptionInfo> wkmOptionInfo = new List<WKMOptionInfo>();
            for (int i = 0; i < sbjList.Count; i++)
            {
                Guid sbjGuid = Guid.NewGuid();
                wkmInfo.SubjectInfo.SubjectContent.Add(sbjList[i]);
                wkmInfo.SubjectInfo.WKMSubjectId.Add(sbjGuid);
                wkmInfo.SubjectInfo.ActivityId.Add(wkmInfo.WKMId);

                wkmInfo.OptionsInfo.Add(new WKMOptionInfo());
                IList<string> optListPerSbj = optList[i].Split('/');
                for (int j = 0; j < optListPerSbj.Count; j++)
                {
                    wkmInfo.OptionsInfo[i].OptionContent.Add(optListPerSbj[j]);
                    wkmInfo.OptionsInfo[i].WKMOptionId.Add(Guid.NewGuid());
                    wkmInfo.OptionsInfo[i].TitleId.Add(wkmInfo.SubjectInfo.WKMSubjectId[i]);
                }
                //图片实体保存到项目内
                if (files[i].ContentLength > 0)
                {
                    string fileName = sbjGuid + Path.GetExtension(files[i].FileName);
                    files[i].SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                    wkmInfo.SubjectInfo.ImgUrl.Add("/Storage/master/topic/" + fileName);
                }
                else
                {
                    wkmInfo.SubjectInfo.ImgUrl.Add("");
                }


            }
            if (PromoteHelper.AddWKM(wkmInfo))
            {
                this.ShowMsgAndReUrl("添加成功！请将活动附加属性设置完毕后,方可推广使用!", true, "setWKMDetail.aspx?id="+wkmInfo.WKMId);
            }
            else
            {
                this.ShowMsg("添加失败！", false);
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

        }
    }
}
