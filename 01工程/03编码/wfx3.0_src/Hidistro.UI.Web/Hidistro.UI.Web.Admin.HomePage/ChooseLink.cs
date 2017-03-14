using ASPNET.WebControls;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.HomePage
{
    public class ChooseLink : AdminPage
	{
        protected HiddenField txtt;
        protected Repeater Repeater1;
        protected DropDownList ddltype;
        protected TextBox txtname;
        protected Button btnsearch;
        protected Panel Panel1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.btnsearch.Click += new EventHandler(btnsearch_Click);
            if (!IsPostBack)
            {
                string t = Request.QueryString["t"];
                txtt.Value = t;
                if (t.Equals("1"))
                {
                    Panel1.Visible = true;
                    ddltype.DataSource = DataBaseHelper.GetDataSet("select CategoryId as ID,Name as Name from Hishop_ArticleCategories").Tables[0];
                    ddltype.DataTextField = "Name";
                    ddltype.DataValueField = "ID";
                    ddltype.DataBind();
                    ddltype.Items.Insert(0,new ListItem("请选择","0"));
                }
                else
                {
                    Panel1.Visible = false;
                }
                InitData();
            }
		}
        private void InitData()
        {
            DataSet ds = null;
            if (txtt.Value == "1")
            {
                string str = " where 1=1";
                if (!ddltype.SelectedValue.Equals("0"))
                {
                    str += " and CategoryId=" + ddltype.SelectedValue;
                }
                str += " and Title like '%"+txtname.Text.Trim()+"%'";
                ds = DataBaseHelper.GetDataSet("select ArticleId as ID,Title as Name from Hishop_Articles "+str);
            }
            else if (txtt.Value == "3")
            {
                ds = DataBaseHelper.GetDataSet("select SkinID as ID,Name as Name from YiHui_HomePage_Skin");
            }
            else
            {
                ds = DataBaseHelper.GetDataSet("select CategoryId as ID,Name as Name from Hishop_ArticleCategories");
            }
            Repeater1.DataSource = ds.Tables[0];
            Repeater1.DataBind();
            
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            InitData();
        }
	}
}
