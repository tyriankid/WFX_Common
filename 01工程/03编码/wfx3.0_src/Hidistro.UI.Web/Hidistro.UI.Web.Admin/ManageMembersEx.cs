using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Members)]
    public class ManageMembersEx : AdminPage
    {
        private bool? approved;
        protected System.Web.UI.WebControls.Button btnExport;
        protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnSendEmail;
        protected System.Web.UI.WebControls.Button btnSendMessage;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected Grid grdMemberList;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdenableemail;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hdenablemsg;
        protected PageSize hrefPageSize;
        protected System.Web.UI.WebControls.Literal litsmscount;
        protected Pager pager;
        protected Pager pager1;
        private int? rankId;
        protected MemberGradeDropDownList rankList;
        private string realName;
        private string storeName;
        private string searchKey;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span1;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span2;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span3;
        protected System.Web.UI.HtmlControls.HtmlGenericControl Span4;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtemailcontent;
        protected System.Web.UI.HtmlControls.HtmlTextArea txtmsgcontent;
        protected System.Web.UI.WebControls.TextBox txtRealName;
        protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.TextBox txtSearchText;
        private int? vipCard = null;
        protected void BindData()
        {
            MemberQuery query = new MemberQuery
            {
                Username = this.searchKey,
                Realname = this.realName,
                StoreName=this.storeName,
                GradeId = this.rankId,
                PageIndex = this.pager.PageIndex,
                IsApproved = this.approved,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            if (this.vipCard.HasValue && this.vipCard.Value != 0)
            {
                query.HasVipCard = new bool?(this.vipCard.Value == 1);
            }
            DbQueryResult members = MemberHelper.GetMembersEx(query);
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
        }
        private void btnSearchButton_Click(object sender, System.EventArgs e)
        {
            this.ReBind(true);
        }
       
        private void ddlApproved_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ReBind(false);
        }
        protected int GetAmount(SiteSettings settings)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(settings.SMSSettings))
            {
                string xml = HiCryptographer.Decrypt(settings.SMSSettings);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                string innerText = document.SelectSingleNode("xml/Appkey").InnerText;
                string postData = "method=getAmount&Appkey=" + innerText;
                string s = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
                int num2;
                if (int.TryParse(s, out num2))
                {
                    num = System.Convert.ToInt32(s);
                }
            }
            return num;
        }
        private SiteSettings GetSiteSetting()
        {
            return SettingsManager.GetMasterSettings(false);
        }
        private void grdMemberList_ReBindData(object sender)
        {
            this.ReBind(false);
        }
        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["rankId"], out result))
                {
                    this.rankId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeName"]))
                {
                    this.storeName = base.Server.UrlDecode(this.Page.Request.QueryString["storeName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
                {
                    this.approved = new bool?(System.Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
                }
                this.rankList.SelectedValue = this.rankId;
                this.txtSearchText.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
                this.txtStoreName.Text = this.storeName;
            }
            else
            {
                this.rankId = this.rankList.SelectedValue;
                this.searchKey = this.txtSearchText.Text;
                this.realName = this.txtRealName.Text.Trim();
                this.storeName = this.txtStoreName.Text.Trim();
            }
        }
        protected override void OnInitComplete(System.EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
            this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.rankList.DataBind();
                this.rankList.SelectedValue = this.rankId;
                this.BindData();
                SiteSettings siteSetting = this.GetSiteSetting();
                if (siteSetting.SMSEnabled)
                {
                    this.litsmscount.Text = this.GetAmount(siteSetting).ToString();
                    this.hdenablemsg.Value = "1";
                }
                if (siteSetting.EmailEnabled)
                {
                    this.hdenableemail.Value = "1";
                }
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
        public string PostData(string url, string postData)
        {
            string str = string.Empty;
            string result;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = (long)bytes.Length;
                using (System.IO.Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.Stream stream2 = response.GetResponseStream())
                    {
                        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                        System.IO.Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else
                        {
                            if (response.ContentEncoding.ToLower() == "deflate")
                            {
                                stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                            }
                        }
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream3, encoding))
                        {
                            result = reader.ReadToEnd();
                            return result;
                        }
                    }
                }
            }
            catch (System.Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            result = str;
            return result;
        }
        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.rankList.SelectedValue.HasValue)
            {
                queryStrings.Add("rankId", this.rankList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("realName", this.txtRealName.Text);
            queryStrings.Add("storeName", this.txtStoreName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}
