using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Function;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Login : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button btnAdminLogin;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected HeadContainer HeadContainer1;
		protected SmallStatusMessage lblStatus;
		//private readonly string licenseMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的微分销系统未经官方授权，无法登录后台管理。请联系微分销官方购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "Vshop/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2014 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";
		//private readonly string noticeMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的微分销系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "Vshop/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2014 hishop.com.cn all Rights Reserved. 本产品资源均为 Hishop 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";
		protected PageTitle PageTitle1;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.TextBox txtAdminName;
		protected System.Web.UI.WebControls.TextBox txtAdminPassWord;
		protected System.Web.UI.WebControls.TextBox txtCode;
        protected System.Web.UI.HtmlControls.HtmlInputHidden specialHideShow;

		private string verifyCodeKey = "VerifyCode";
		private string ReferralLink
		{
			get
			{
				return this.ViewState["ReferralLink"] as string;
			}
			set
			{
				this.ViewState["ReferralLink"] = value;
			}
		}
		private void btnAdminLogin_Click(object sender, System.EventArgs e)
		{
			if (!Globals.CheckVerifyCode(this.txtCode.Text.Trim()))
			{
				this.ShowMessage("验证码不正确");
			}
			else
			{
				ManagerInfo manager = ManagerHelper.GetManager(this.txtAdminName.Text);
				if (manager == null)
				{
					this.ShowMessage("无效的用户信息");
				}
                else if(manager.State == 1)
                {
                    this.ShowMessage("该账号未激活");
                }
				else
				{
					if (manager.Password != HiCryptographer.Md5Encrypt(this.txtAdminPassWord.Text))
					{
						this.ShowMessage("密码不正确");
					}
					else
					{
						System.Web.HttpCookie cookie = new System.Web.HttpCookie("Vshop-Manager")
						{
							Value = manager.UserId.ToString(),
                            Expires = System.DateTime.Now.AddDays(1.0)//Expires = System.DateTime.Now.AddDays(1.0) 
						};
						System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
						this.Page.Response.Redirect("Default.aspx", true);
					}
				}
			}
		}
		private bool CheckVerifyCode(string verifyCode)
		{
			return base.Request.Cookies[this.verifyCodeKey] != null && string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string VisitDistributorID = Hidistro.Core.HiCache.Get(string.Format("DataCache-VisitDistributor-{0}", "333")) as string;
            if (!string.IsNullOrEmpty(VisitDistributorID))
            {

            }


			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string verifyCode = base.Request["code"];
				string str2;
				if (!this.CheckVerifyCode(verifyCode))
				{
					str2 = "0";
				}
				else
				{
					str2 = "1";
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
				base.Response.Write("}");
				base.Response.End();
			}
			if (!this.Page.IsPostBack)
			{
				Uri urlReferrer = this.Context.Request.UrlReferrer;
				if (urlReferrer != null)
				{
					this.ReferralLink = urlReferrer.ToString();
				}
				this.txtAdminName.Focus();
				PageTitle.AddSiteNameTitle("后台登录");

                string path = Server.MapPath("~/config/CustomConfig.xml");

                /* 阿黛尔艺丝*/
                /*DataSet dsAde = new DataSet();
                dsAde.DataSetName = "adele";
                DataTable dtAde = new DataTable();
                dtAde.TableName = "DistributorType";
                dtAde.Columns.Add("name", typeof(string));
                dtAde.Columns.Add("showfootmanage", typeof(string));
                dtAde.Columns.Add("showfootapply", typeof(string));
                dtAde.Columns.Add("showbutton", typeof(string));
                dtAde.Rows.Add(new string[] { "VIP", "VIP", "VIP", "马上成为VIP" });
                dsAde.Tables.Add(dtAde);
                dsAde.WriteXml(path, XmlWriteMode.IgnoreSchema);
                */

                /*竞芳菲(同欣美容)*/
                /*DataSet dsJFF = new DataSet();
                dsJFF.DataSetName = "jingff";
                DataTable dtJFF = new DataTable();
                dtJFF.TableName = "isReturnChangeGoodsOn";
                dtJFF.Columns.Add("value", typeof(string));
                dtJFF.Rows.Add(new string[] { "true" });
                dsJFF.Tables.Add(dtJFF);
                dsJFF.WriteXml(path, XmlWriteMode.IgnoreSchema);*/

                /*齐品汇{特殊的5类分销商返佣机制}*/
                /*
                DataSet dsQiph = new DataSet();
                dsQiph.DataSetName = "Qipinhui";
                DataTable dtQiph = new DataTable();
                dtQiph.TableName = "Configs";
                dtQiph.Columns.Add("isQipinhui", typeof(string));
                dtQiph.Rows.Add(new string[] { "true" });
                dsQiph.Tables.Add(dtQiph);
                dsQiph.WriteXml(path, XmlWriteMode.IgnoreSchema);
                /*

                /*爱弗瑞{天使返利规则定制}*/
                /*DataSet dsAfr = new DataSet();
                dsAfr.DataSetName = "aifr";
                DataTable dtAFF = new DataTable();
                dtAFF.TableName = "Configs";
                dtAFF.Columns.Add("isAll", typeof(string));
                dtAFF.Rows.Add(new string[] { "true" });
                dsAfr.Tables.Add(dtAFF);
                dsAfr.WriteXml(path, XmlWriteMode.IgnoreSchema);
                */
         
                /*迪蔓国际{文字、Logo等定制处理}*/
                /*
                DataSet dsDiman = new DataSet();
                dsDiman.DataSetName = "diman";
                DataTable dtDiman = new DataTable();
                dtDiman.TableName = "Configs";
                dtDiman.Columns.Add("myCashText", typeof(string));
                dtDiman.Columns.Add("myDistributorText", typeof(string));
                dtDiman.Columns.Add("showbutton", typeof(string));
                dtDiman.Columns.Add("isLogoOn", typeof(string));
                dtDiman.Columns.Add("isDistributorDescriptionOn", typeof(string));
                dtDiman.Columns.Add("customInputs", typeof(string));
                dtDiman.Rows.Add(new string[] { "我的返利", "我的分店", "马上注册微店", "false", "false", "<div class=\"login-name\"> <span class=\"glyphicon glyphicon-phone\"></span><input type=\"text\" class=\"\" id=\"username\" placeholder=\"请输入手机号\"/>  </div> <div class=\"login-name\"><span class=\"glyphicon glyphicon-user\"></span><input type=\"password\" class=\"\" id=\"pass\" placeholder=\"请输入真实姓名\"/>  </div><script>$(function () {$(\".btn-apply\").click(function () {if ($(\"#username\").val() == \"\" || $(\"#pass\").val() == \"\") {alert_h(\"请将信息填写完整！\");return false; } });}); </script>" });
                dsDiman.Tables.Add(dtDiman);
                dsDiman.WriteXml(path, XmlWriteMode.IgnoreSchema);
                */

                /*玖信健佳{后台代理商的分区域相关功能}*/
                /*
                DataSet dsJXJJ = new DataSet();
                dsJXJJ.DataSetName = "JXJJ";
                DataTable dtJXJJ = new DataTable();
                dtJXJJ.TableName = "Configs";
                dtJXJJ.Columns.Add("regionalFunction", typeof(bool));
                dtJXJJ.Rows.Add(new string[] { "true" });
                dsJXJJ.Tables.Add(dtJXJJ);
                dsJXJJ.WriteXml(path, XmlWriteMode.IgnoreSchema);
                 */ 

                /*默认配置*/
                /*
                DataSet dsDefault = new DataSet();
                dsDefault.DataSetName = "default";
                DataTable dtDefault = new DataTable();
                dtDefault.TableName = "Configs";
                dtDefault.Columns.Add("isQuickGetCashOn", typeof(bool));
                dtDefault.Rows.Add(new string[] { "true" });
                dsDefault.Tables.Add(dtDefault);
                dsDefault.WriteXml(path, XmlWriteMode.IgnoreSchema);
                 */
                Hidistro.Core.Entities.SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                //对特殊用户的特殊功能进行相应的隐藏显示
                if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction)
                {
                    this.specialHideShow.Value = "jxjj";//玖信健佳
                }
			}
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string str = "1";
			if (!string.IsNullOrEmpty(str))
			{
				/*switch (System.Convert.ToInt32(str.Replace("{\"state\":\"", "").Replace("\"}", "")))
				{
				case -1:
					writer.Write(this.noticeMsg);
					return;
				case 0:
					writer.Write(this.licenseMsg);
					return;
				}*/
			}
			base.Render(writer);
		}
		private void ShowMessage(string msg)
		{
			this.lblStatus.Text = msg;
			this.lblStatus.Success = false;
			this.lblStatus.Visible = true;
		}
	}
}
