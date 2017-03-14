using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Hidistro.SaleSystem.Vshop;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Data;
using Hidistro.ControlPanel.Members;
using System.Text.RegularExpressions;


namespace Hidistro.UI.Web.Admin.distributor
{
    public class UnderDistributorManage : AdminPage
	{
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;
        private string startDate="";
        private string endDate="";
		protected System.Web.UI.WebControls.Button btnSearchButton;
        protected System.Web.UI.WebControls.Button btnDistributorTree;
		private string CellPhone = "";
		protected System.Web.UI.WebControls.DropDownList DrGrade;
		protected System.Web.UI.WebControls.DropDownList DrStatus;
		private string Grade = "0";
		private string MicroSignal = "";
		protected Pager pager;
		private string RealName = "";
		protected System.Web.UI.WebControls.Repeater reDistributor;
		private string Status = "0";
		private string StoreName = "";
        private string IsAgent = "0";
		protected System.Web.UI.WebControls.TextBox txtStoreName;
        protected System.Web.UI.WebControls.HiddenField hiIsAgent;
        public string TypeTitle = "������";
        protected System.Web.UI.WebControls.TextBox txtReferralUserId;
        protected HiddenField hidTopUserId;//���ϼ��û�id������
        private int distributorId = 0;
        protected Literal litShowInfo;
        protected Button btnExport;
		private void BindData()
		{
            if (!string.IsNullOrEmpty(this.startDate))
            {
                this.calendarStartDate.SelectedDate = Convert.ToDateTime(this.startDate);
            }
            if (!string.IsNullOrEmpty(this.endDate))
            {
                this.calendarEndDate.SelectedDate = Convert.ToDateTime(this.endDate);
            }
			DistributorsQuery entity = new DistributorsQuery
			{
				GradeId = int.Parse(this.Grade),
				StoreName = this.StoreName,
				CellPhone = this.CellPhone,
				RealName = this.RealName,
				MicroSignal = this.MicroSignal,
				ReferralStatus = int.Parse(this.Status),
                IsAgent = int.Parse(this.IsAgent),
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc,
				SortBy = "userid"
			};
			Globals.EntityCoding(entity, true);
			//DbQueryResult distributors = VShopHelper.GetDistributors(entity);
            DistributorsInfo topDistributor = DistributorsBrower.GetDistributorInfo(this.distributorId);
            hidTopUserId.Value = distributorId.ToString();
            if (topDistributor != null)
            {
                DataTable dt = new DataTable();
                /*
                if (topDistributor.IsAgent == 1)
                {
                    litShowInfo.Text = "<p style='color:red'>����ǰ��ѯ���û���" + topDistributor.StoreName + "���Ǵ�����,������������������Ϣ</p>";
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        AgentPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//�������������̺ʹ�����
                    };
                    dt = DistributorsBrower.GetDownDistributorsAndA(dquery);
                }
                else
                {
                    litShowInfo.Text = "<p style='color:red'>����ǰ��ѯ���û���" + topDistributor.StoreName + "���Ƿ�����,���������������ڵ�������Ϣ</p>";
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        ReferralPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//��������������
                    };
                    dt = DistributorsBrower.GetThreeDownDistributors(dquery);
                }
                 */ 
                //dt.Rows.Add(topDistributor.UserName, topDistributor.UserHead, topDistributor.UserId, topDistributor.IsAgent, topDistributor.StoreName);
                DataTable dtDownDistributor = DistributorsBrower.GetDownDistributor(this.distributorId,this.startDate,this.endDate);
                dt = dtDownDistributor.Clone();//�������ݼ��ṹ
                if (!dt.Columns.Contains("HasChild")) {
                    dt.Columns.Add("HasChild", typeof(int));
                }
                /*
                if (!dt.Columns.Contains("ParentStoreName"))
                {
                    dt.Columns.Add("ParentStoreName", typeof(string));
                }
                 */ 
                DataRow[] rows = dtDownDistributor.Select(string.Format("ReferralPath='{0}' or ReferralPath like '%|{0}' or userid={0}", this.distributorId), "userid");
                foreach (DataRow row in rows)  // ����ѯ�Ľ����ӵ�dt�У� 
                {
                    DataRow dr=dt.Rows.Add(row.ItemArray);
                    dr["HasChild"] = dtDownDistributor.Select(string.Format("ReferralPath='{0}' or ReferralPath like '%|{0}'", dr["userid"]), "userid").Length > 0 ? 1 : 0;
                    //dr["ParentStoreName"] = topDistributor.StoreName;
                } 

                this.reDistributor.DataSource = dt;
                this.reDistributor.DataBind();
                this.pager.TotalRecords = dt.Rows.Count;
            }
            else//�����ǰû�д�ֵ,����ʾ����һ�������̵���Ϣ
            {
                DataTable firstDistributors = DistributorsBrower.GetFirstDistributors(this.startDate,this.endDate);
                DataTable dt = firstDistributors.Clone();//�������ݼ��ṹ
                if (!dt.Columns.Contains("HasChild"))
                {
                    dt.Columns.Add("HasChild", typeof(int));
                }
                foreach (DataRow row in firstDistributors.Rows)  // ����ѯ�Ľ����ӵ�dt�У� 
                {
                    DataRow dr = dt.Rows.Add(row.ItemArray);
                    dr["HasChild"] = Convert.ToInt32(row["childCount"]) > 0? 1 : 0;
                }
                this.reDistributor.DataSource = dt;
                this.reDistributor.DataBind();
                this.pager.TotalRecords = firstDistributors.Rows.Count;
                litShowInfo.Text = "<p style='color:red'>���������ж���������������Լ���������Ϣ����" + firstDistributors.Rows.Count + "����¼</p>";
            }
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
        //�鿴��ϵͼ��ť
        private void btnDistributorTree_Click(object sender, System.EventArgs e)
        {
            string parameter=string.Empty;
            if (Convert.ToInt32(this.Page.Request.QueryString["distributorId"]) != 0)
            {
                parameter = "?TopUserId=" + Convert.ToInt32(this.Page.Request.QueryString["distributorId"]);
            }
            this.Page.Response.Redirect("DistributorsTree.aspx" + parameter);
        }
        private void btnExport_Click(object sender, System.EventArgs e)
        {
            DataTable dt = null;
            if ( hidTopUserId.Value != "0")
            {
                dt = DistributorsBrower.GetDistributor(Convert.ToInt32(hidTopUserId.Value), this.calendarStartDate.SelectedDate.ToString(), this.calendarEndDate.SelectedDate.ToString());
            }
            else//�����ǰû�д�ֵ,����ʾ����һ�������̵���Ϣ
            {
                dt = DistributorsBrower.GetAllFirstDis(this.startDate, this.endDate);
            }

            DataTable NewDt = DistributorsBrower.Export(dt);
            //����������ֶ�
            NewDt.Columns.Add("ProfitTotal");
            foreach (DataRow row in NewDt.Rows)
            {
                row["ProfitTotal"] = Convert.ToDouble(row["OrderTotal"]) - Convert.ToDouble(row["CostTotal"]) - Convert.ToDouble(row["CommTotal"]);
            }
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            fields.Add("StoreName");
            list2.Add("������");
            fields.Add("username");
            list2.Add("΢���ǳ�");
            fields.Add("CommTotal");
            list2.Add("Ӷ���ܶ�");
            fields.Add("OrderTotal");
            list2.Add("�����ܶ�");
            fields.Add("CostTotal");
            list2.Add("�����ɱ��ܶ�");
            fields.Add("ProfitTotal");
            list2.Add("����");
            fields.Add("pname");
            list2.Add("�����ϼ�����");
            fields.Add("isAgent");
            list2.Add("�Ƿ������");
           

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (System.Data.DataRow row in NewDt.Rows)
            {
                foreach (string str2 in fields)
                {
                    if (str2 == "isAgent")
                    {
                        builder.Append(Convert.ToInt32(row[str2]) ==1?"��":"��").Append(",");
                    }
                    else
                    {
                        builder.Append(row[str2]).Append(",");
                    }
                    if (str2 == fields[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=Distributors "+DateTime.Now.ToShortDateString()+".csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
				{
					this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
				{
					this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
				{
					this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
				{
					this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
				{
					this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
				{
					this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
				}
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isagent"]))
                {
                    this.IsAgent = base.Server.UrlDecode(this.Page.Request.QueryString["isagent"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DistributorId"]))
                {
                    this.distributorId=Convert.ToInt32(this.Page.Request.QueryString["distributorId"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
                {
                    this.startDate =this.Page.Request.QueryString["StartDate"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
                {
                    this.endDate = this.Page.Request.QueryString["EndDate"];
                }

				this.txtStoreName.Text = this.StoreName;

                this.hiIsAgent.Value = this.IsAgent;
                TypeTitle = (this.IsAgent == "1") ? "������" : "������";
			}
			else
			{
                this.StoreName = this.txtReferralUserId.Text;
                this.IsAgent = this.hiIsAgent.Value;
                TypeTitle = (this.IsAgent == "1") ? "������" : "������";
			}
		}


        


		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!string.IsNullOrEmpty(base.Request["action"]) && base.Request["action"] == "SearchKey")
            {
                string allDistributorsName = string.Empty;
                if (!string.IsNullOrEmpty(base.Request["keyword"]))
                {
                    allDistributorsName = Hidistro.ControlPanel.Members.MemberHelper.GetDistributorsName(base.Request["keyword"]);
                }
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"data\":[" + allDistributorsName + "]}");
                base.Response.End();
            }
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            //this.btnDistributorTree.Click += new System.EventHandler(this.btnDistributorTree_Click);
			this.LoadParameters();

            if (!Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableStoreProductAuto && Hidistro.Core.SettingsManager.GetMasterSettings(false).EnableAgentProductRange)
            {
                ViewState["IsSetProductRange"] = "1";
            }
            else
            {
                ViewState["IsSetProductRange"] = "0";
            }
            
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}
		private void ReBind(bool isSearch)
		{
            string distributorname = this.txtReferralUserId.Text;
            int referruserId = MemberHelper.IsExiteDistributorNames(distributorname);
			NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("DistributorId", referruserId.ToString());
            queryStrings.Add("StoreName", distributorname);
            queryStrings.Add("IsAgent", this.hiIsAgent.Value);
			queryStrings.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(queryStrings);
		}

        [System.Web.Services.WebMethod]
        public static string GetUnderDistributorTr(string currentDistributorId,string startDate="",string endDate="")
        {
            
            DistributorsInfo currentDistributor = DistributorsBrower.GetDistributorInfo(Convert.ToInt32(currentDistributorId));
            DataTable dtDownDistributor=DistributorsBrower.GetDownDistributor(Convert.ToInt32(currentDistributorId),startDate,endDate);
            DataTable dt = dtDownDistributor.Clone();//�������ݼ��ṹ
            if (!dt.Columns.Contains("HasChild"))
            {
                dt.Columns.Add("HasChild", typeof(int));
            }
            if (!dt.Columns.Contains("ParentStoreName"))
            {
                dt.Columns.Add("ParentStoreName", typeof(string));
            }
            DataRow[] rows = dtDownDistributor.Select(string.Format("ReferralPath='{0}' or ReferralPath like '%|{0}' ", currentDistributorId), "userid");
            foreach (DataRow row in rows)  // ����ѯ�Ľ����ӵ�dt�У� 
            {
                DataRow dr = dt.Rows.Add(row.ItemArray);
                dr["HasChild"] = dtDownDistributor.Select(string.Format("ReferralPath='{0}' or ReferralPath like '%|{0}'", dr["userid"]), "userid").Length > 0 ? 1 : 0;
                dr["ParentStoreName"] = currentDistributor.StoreName;
            } 
            string inHtml = string.Empty;
            if (dt.Rows.Count > 0)
            {
                string buttonHtml = string.Empty;
                foreach (DataRow row in dt.Rows)
                {
                    int userId =Convert.ToInt32(row["UserId"]);
                    buttonHtml = string.Format("<span class='submit_bianji' ><a href='DistributorDetails.aspx?UserId=" + row["UserId"].ToString() + "&IsAgent=" + row["isagent"].ToString() + "' >��ϸ</a></span><span class='submit_bianji'><a href='CommissionsList.aspx?UserId=" + row["UserId"] + "' >Ӷ����ϸ</a></span><a href='" + Globals.GetAdminAbsolutePath(string.Format("/sales/ManageOrder.aspx?ReferralUserId={0}", row["UserId"].ToString())) + "'>���ж���</a>");
                    inHtml += string.Format("<tr id='{0}' {9} pid='{1}'><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{10}</td><td>{6}</td><td>{7}</td><td style='text-align:center'>{8}</td></tr>",
                        row["UserId"], currentDistributorId, row["StoreName"], GetText(row["UserName"].ToString()).Trim(), ((decimal)row["CommTotal"]).ToString("F2"), ((decimal)row["OrderTotal"]).ToString("F2"), currentDistributor.StoreName, Convert.ToInt32(row["isagent"]) == 1 ? "��" : "��", buttonHtml, Convert.ToInt32(row["HasChild"]) == 1 ? "hasChild='true'" : "", ((decimal)row["CostTotal"]).ToString("F2"));
                }
            }
            return ("{\"trHtml\":\""+inHtml+"\"}");
        }

        /// <summary>
        /// ��ȡ��ǰ��ɫ�����в˵�
        /// </summary>
        [System.Web.Services.WebMethod]
        public static string GetZnodes(string TopUserId)
        {
            string znodes = string.Empty;
            DistributorsInfo topDistributor = DistributorsBrower.GetDistributorInfo(Convert.ToInt32(TopUserId));
            if (topDistributor != null)
            {
                DataTable dt = new DataTable();
                if (topDistributor.IsAgent == 1)
                {
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        AgentPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//�������������̺ʹ�����
                    };
                    dt = DistributorsBrower.GetDownDistributorsAndA(dquery);
                }
                else
                {
                    DistributorsQuery dquery = new DistributorsQuery
                    {
                        PageIndex = 1,
                        PageSize = int.MaxValue,
                        ReferralPath = topDistributor.UserId.ToString(),
                        GradeId = 99,//��������������
                    };
                    dt = DistributorsBrower.GetThreeDownDistributors(dquery);
                }
                
                if (dt != null)//�����ǰ��ɫ���п���ʹ�õĲ˵�
                {
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["UserId"] };
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string userid = dt.Rows[i]["userid"].ToString();
                        string isagent = dt.Rows[i]["isagent"].ToString();
                        string name = dt.Rows[i]["storename"].ToString() + " " + (dt.Rows[i]["isagent"].ToString() == "1" ? "(������)" : "(������)");
                        string storename = dt.Rows[i]["username"].ToString();
                        string pid = dt.Rows[i]["ParentUserId"].ToString();
                        znodes += "{\"id\":\"" + userid + "\",\"isagent\":\"" + isagent + "\",\"name\":\"" + name + "\",\"open\":\"true\",\"storename\":\"" + storename + "\",\"pid\":\""+pid+"\"},";
                    }
                    znodes = znodes.TrimEnd(',');
                }
            }
            else
            {
                return "������";
            }
            return znodes;
        }

        /// <summary>
        /// ����ǩת����TEXT
        /// </summary>
        public static string GetText(string strHtml)
        {
            if (strHtml == "")
            {
                strHtml = "";
            }
            else
            {
                strHtml = Regex.Replace(strHtml, "<[^>]*>", "");
                //�滻�ո�
                //strHtml = Regex.Replace(strHtml,"\\s+", " ");
                strHtml = Regex.Replace(strHtml, "&nbsp;+", "");
                strHtml = Regex.Replace(strHtml, "'+", "");
                strHtml = Regex.Replace(strHtml, "\r\n+", "");
                strHtml = Regex.Replace(strHtml, " ", "");
            }
            return strHtml;
        }

	}
}
