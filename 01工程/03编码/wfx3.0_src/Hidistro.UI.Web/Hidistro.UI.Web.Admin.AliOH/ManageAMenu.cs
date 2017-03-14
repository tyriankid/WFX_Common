using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.App_Code;
using Hishop.Alipay.OpenHome;
using Hishop.Alipay.OpenHome.AlipayOHException;
using Hishop.Alipay.OpenHome.Request;
using Hishop.Alipay.OpenHome.Response;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Domain.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.AliOH
{
	public class ManageAMenu : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected Grid grdMenu;
		private void BindData(ClientType clientType)
		{
            this.grdMenu.DataSource = VShopHelper.GetMenus(clientType);
			this.grdMenu.DataBind();
		}
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
            if (string.IsNullOrEmpty(SettingsManager.GetMasterSettings(false).AliOHAppId))
            {
                base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='AliOHServerConfig.aspx'</script>");
            }
            else
            {
                IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(ClientType.AliOH);
                List<Hishop.Alipay.OpenHome.Model.Button> list2 = new List<Hishop.Alipay.OpenHome.Model.Button>();
                foreach (MenuInfo info in initMenus)
                {
                    if ((info.Chilren == null) || (info.Chilren.Count == 0))
                    {
                        Hishop.Alipay.OpenHome.Model.Button item = new Hishop.Alipay.OpenHome.Model.Button
                        {
                            name = info.Name,
                            actionParam = string.IsNullOrEmpty(info.Url) ? "http://javasript:;" : info.Url,
                            actionType = info.Type
                        };
                        list2.Add(item);
                    }
                    else
                    {
                        Hishop.Alipay.OpenHome.Model.Button button2 = new Hishop.Alipay.OpenHome.Model.Button
                        {
                            name = info.Name
                        };
                        List<Hishop.Alipay.OpenHome.Model.Button> list3 = new List<Hishop.Alipay.OpenHome.Model.Button>();
                        foreach (MenuInfo info2 in info.Chilren)
                        {
                            Hishop.Alipay.OpenHome.Model.Button button3 = new Hishop.Alipay.OpenHome.Model.Button
                            {
                                name = info2.Name,
                                actionParam = string.IsNullOrEmpty(info2.Url) ? "http://javasript:;" : info2.Url,
                                actionType = info2.Type
                            };
                            list3.Add(button3);
                        }
                        button2.subButton = list3;
                        list2.Add(button2);
                    }
                }
                Hishop.Alipay.OpenHome.Model.Menu menu = new Hishop.Alipay.OpenHome.Model.Menu
                {
                    button = list2
                };
                AlipayOHClient client = AliOHClientHelper.Instance(base.Server.MapPath("~/"));
                bool flag = false;
                try
                {
                    AddMenuRequest request = new AddMenuRequest(menu);
                    client.Execute<MenuAddResponse>(request);
                    this.ShowMsg("保存到服务窗成功！", true);
                    flag = true;
                }
                catch (AliResponseException)
                {
                }
                catch (Exception exception)
                {
                    this.ShowMsg("保存到服务窗失败，失败原因：" + exception.Message, false);
                    flag = true;
                }
                if (!flag)
                {
                    try
                    {
                        UpdateMenuRequest request2 = new UpdateMenuRequest(menu);
                        client.Execute<MenuUpdateResponse>(request2);
                        this.ShowMsg("保存到服务窗成功！", true);
                    }
                    catch (Exception exception2)
                    {
                        this.ShowMsg("保存到服务窗失败，失败原因：" + exception2.Message, false);
                    }
                }
            }
		}
		private SingleButton BuildMenu(MenuInfo menu)
		{
			SingleButton result;
			switch (menu.BindType)
			{
			case BindType.Key:
				result = new SingleClickButton
				{
					name = menu.Name,
					key = menu.MenuId.ToString()
				};
				break;
			case BindType.Topic:
			case BindType.HomePage:
			case BindType.ProductCategory:
			case BindType.ShoppingCar:
			case BindType.OrderCenter:
			case BindType.MemberCard:
			case BindType.Url:
				result = new SingleViewButton
				{
					name = menu.Name,
					url = menu.Url
				};
				break;
			default:
				result = new SingleClickButton
				{
					name = menu.Name,
					key = "None"
				};
				break;
			}
			return result;
		}
		private void grdMenu_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int menuId = (int)this.grdMenu.DataKeys[rowIndex].Value;
			if (e.CommandName == "Fall")
			{
				VShopHelper.SwapMenuSequence(menuId, false);
			}
			else
			{
				if (e.CommandName == "Rise")
				{
					VShopHelper.SwapMenuSequence(menuId, true);
				}
				else
				{
					if (e.CommandName == "DeleteMenu")
					{
						if (VShopHelper.DeleteMenu(menuId))
						{
							this.ShowMsg("成功删除了指定的分类", true);
						}
						else
						{
							this.ShowMsg("分类删除失败，未知错误", false);
						}
					}
				}
			}
			this.BindData(ClientType.AliOH);
		}
		private void grdMenu_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ParentMenuId");
				string str = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Name").ToString();
				if (num == 0)
				{
					str = "<b>" + str + "</b>";
				}
				if (num > 0)
				{
					str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
				}
				System.Web.UI.WebControls.Literal literal = e.Row.FindControl("lblCategoryName") as System.Web.UI.WebControls.Literal;
				literal.Text = str;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdMenu.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdMenu_RowCommand);
			this.grdMenu.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdMenu_RowDataBound);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			if (!this.Page.IsPostBack)
			{
                this.BindData(ClientType.AliOH);
			}
		}
		protected string RenderInfo(object menuIdObj)
		{
			ReplyInfo reply = ReplyHelper.GetReply((int)menuIdObj);
			string result;
			if (reply != null)
			{
				result = reply.Keys;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
