using Hidistro.ControlPanel.AdminMenu;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Admin_ManageMenu : Page//Hidistro.UI.ControlPanel.Utility.AdminPage//Page
{
    XmlDocument xmldoc;
    XmlNode xmlnode;
    XmlElement xmlelem;
    DataTable allMenus;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            xmldoc = new XmlDocument();
            xmldoc.Load(GetMasterSettingsFilename("Menu_Select"));
            XmlNode root = xmldoc.SelectSingleNode("Menu");
            this.hiSelectIDS.Value = root.InnerText;

            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            BindData();
        }
    }

    private void BindData()
    {
        allMenus = AdminMenuHelper.GetAllAdminMenus();//获取所有菜单信息
        allMenuList.DataSource = allMenus;
        allMenuList.DataBind();
        allMenus.PrimaryKey = new DataColumn[] { allMenus.Columns["MIID"] };
        ViewState["allMenus"] = allMenus;
    }

    private static string GetMasterSettingsFilename(string fileName)
    {
        HttpContext current = HttpContext.Current;
        return ((current != null) ? current.Request.MapPath("~/Admin/"+fileName+".xml") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Admin\"+fileName+".xml"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string json =hiSelectIDS.Value;
        DataTable dtData = JsonToDataTable((DataTable)ViewState["allMenus"],json);

        string str = "";
        if (DataBaseHelper.CommitDataTable(dtData, "SELECT * from YiHui_MenuInfo") != -1)
        {
            BindData();
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
        
        /*
        SqlDataAdapter SDA = new SqlDataAdapter();
        SqlCommandBuilder SCB = new SqlCommandBuilder(SDA);
        SDA.Update(allMenus);


        //数据
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("ID",typeof(int));
        dtNew.Columns.Add("Name", typeof(string));
        dtNew.Rows.Add(new string[]{"1","T1"});
        dtNew.Rows.Add(new string[] { "2", "T2" });
        dtNew.AcceptChanges();
        */

        //用户
        //dtNew.Rows[0]["Name"] = "T_New";
        //dtNew.Rows[0].RowState


        /*
        string ids = this.hiSelectIDS.Value;
        string str = "";
        if (AdminMenuHelper.UpdateSelectMenus(ids))
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
        */


        /*
        xmldoc = new XmlDocument();
        xmldoc.Load(GetMasterSettingsFilename("Menu_Select"));
        XmlNode root = xmldoc.SelectSingleNode("Menu");
        root.InnerText = this.hiSelectIDS.Value;
        xmldoc.Save(GetMasterSettingsFilename("Menu_Select"));

        string[] arrayID = this.hiSelectIDS.Value.Trim(',').Split(',');
        string[] arrayName = this.hiSelectNameS.Value.Split(',');
        string[] arrayLink = this.hiSelectLinkS.Value.Split(',');
        string strMenuTypeName=string.Empty;

        XmlTextWriter xmlWriter;
        string strFilename = GetMasterSettingsFilename("Menu");

        xmlWriter = new XmlTextWriter(strFilename, Encoding.UTF8);//创建一个xml文档
        xmlWriter.Formatting = Formatting.Indented;
        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("Menu");

        for (int i=0; i < arrayID.Length; i++)
        {
            if (i > 0)
            {
                int j = arrayID[i - 1].Length - arrayID[i].Length + 1;
                for (int k = 0; k < j; k++)
                    xmlWriter.WriteEndElement();
            }
            switch (arrayID[i].Length)
            { 
                case 1:
                    strMenuTypeName="Module";
                    break;
                case 2:
                    strMenuTypeName = "Item";
                    break;
                case 3:
                    strMenuTypeName = "PageLink";
                    break;
            }
            xmlWriter.WriteStartElement(strMenuTypeName);
            xmlWriter.WriteAttributeString("Title", arrayName[i]);
            xmlWriter.WriteAttributeString("Link", arrayLink[i]);
        }
        xmlWriter.Close();
        */
    }


    /// <summary>
    /// 将json转换为DataTable
    /// </summary>
    /// <param name="strJson">得到的json</param>
    /// <returns></returns>
    private DataTable JsonToDataTable(DataTable allMenus,string strJson)
    {
        //转换json格式
        strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
        //取出表名   
        var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        //去除表名   
        strJson = strJson.Substring(strJson.IndexOf("[") + 1);
        strJson = strJson.Substring(0, strJson.IndexOf("]"));

        //获取数据   
        rg = new Regex(@"(?<={)[^}]+(?=})");
        MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split('*');

            string strMIID = strRows[0].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
            DataRow drData = allMenus.Rows.Find(strMIID);
            for (int r = 1; r < strRows.Length; r++)
            {
                string col = strRows[r].Split('#')[0].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                string value = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                drData[col] = value;
            }
        }
        return allMenus;
    }
}