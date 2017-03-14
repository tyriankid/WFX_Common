using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditProductCategory)]
	public class EditCategory : AdminPage
	{
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.WebControls.Button btnSaveCategory;
		private int categoryId;
		protected ProductTypeDownList dropProductTypes;
		protected KindeditorControl fckNotes1;
		protected KindeditorControl fckNotes2;
		protected KindeditorControl fckNotes3;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liParentCategroy;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liRewriteName;
		protected System.Web.UI.WebControls.TextBox txtCategoryName;
		protected System.Web.UI.WebControls.TextBox txtPageDesc;
		protected System.Web.UI.WebControls.TextBox txtPageKeyTitle;
		protected System.Web.UI.WebControls.TextBox txtPageKeyWords;
		protected System.Web.UI.WebControls.TextBox txtRewriteName;
		protected System.Web.UI.WebControls.TextBox txtSKUPrefix;
        protected FileUpload bannerImgUpload;//分类大图
        protected Image bannerImg;

		private void BindCategoryInfo(CategoryInfo categoryInfo)
		{
			if (categoryInfo != null)
			{
				this.txtCategoryName.Text = categoryInfo.Name;
				this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
				this.txtSKUPrefix.Text = categoryInfo.SKUPrefix;
				this.txtRewriteName.Text = categoryInfo.RewriteName;
				this.txtPageKeyTitle.Text = categoryInfo.MetaTitle;
				//this.txtPageKeyWords.Text = categoryInfo.MetaKeywords;
                bannerImg.ImageUrl = categoryInfo.MetaKeywords;
				this.txtPageDesc.Text = categoryInfo.MetaDescription;
				this.fckNotes1.Text = categoryInfo.Notes1;
				this.fckNotes2.Text = categoryInfo.Notes2;
				this.fckNotes3.Text = categoryInfo.Notes3;
				this.imgPic.ImageUrl = categoryInfo.IconUrl;
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			this.imgPic.ImageUrl = string.Empty;
			this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
		}
		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
			if (category == null)
			{
				this.ShowMsg("编缉商品分类错误,未知", false);
			}
			else
			{
				string str = string.Empty;
				if (this.fileUpload.HasFile)
				{
					try
					{
						str = VShopHelper.UploadTopicImage(this.fileUpload.PostedFile);
					}
					catch
					{
					}
				}
				category.IconUrl = str;
				category.Name = this.txtCategoryName.Text;
				category.SKUPrefix = this.txtSKUPrefix.Text;
				category.RewriteName = this.txtRewriteName.Text;
				category.MetaTitle = this.txtPageKeyTitle.Text;
				//category.MetaKeywords = this.txtPageKeyWords.Text; //将keywords换成了分类大图的imgUrl.
                //保存背景图
                string backimgUrl = bannerImg.ImageUrl;
                if (!string.IsNullOrEmpty(bannerImgUpload.FileName) && bannerImgUpload.FileContent.Length > 0)
                {
                    string fileName = category.CategoryId + Path.GetExtension(bannerImgUpload.FileName);
                    bannerImgUpload.SaveAs(Server.MapPath("/Storage/master/topic/") + fileName);
                    backimgUrl = ("/Storage/master/topic/" + fileName);
                }
                else if (string.IsNullOrEmpty(bannerImg.ImageUrl))
                {
                    backimgUrl = ("");
                }
                category.MetaKeywords = backimgUrl;

				category.MetaDescription = this.txtPageDesc.Text;
				category.AssociatedProductType = this.dropProductTypes.SelectedValue;
				category.Notes1 = this.fckNotes1.Text;
				category.Notes2 = this.fckNotes2.Text;
				category.Notes3 = this.fckNotes3.Text;
				if (category.Depth > 1)
				{
					CategoryInfo info2 = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
					if (string.IsNullOrEmpty(category.Notes1))
					{
						category.Notes1 = info2.Notes1;
					}
					if (string.IsNullOrEmpty(category.Notes2))
					{
						category.Notes2 = info2.Notes2;
					}
					if (string.IsNullOrEmpty(category.Notes3))
					{
						category.Notes3 = info2.Notes3;
					}
				}
				ValidationResults results = Validation.Validate<CategoryInfo>(category, new string[]
				{
					"ValCategory"
				});
				string msg = string.Empty;
				if (!results.IsValid)
				{
					foreach (ValidationResult result in (System.Collections.Generic.IEnumerable<ValidationResult>)results)
					{
						msg += Formatter.FormatErrorMessage(result.Message);
					}
					this.ShowMsg(msg, false);
				}
				else
				{
					CategoryActionStatus categoryActionStatus = CatalogHelper.UpdateCategory(category);
					if (categoryActionStatus != CategoryActionStatus.Success)
					{
						if (categoryActionStatus != CategoryActionStatus.UpdateParentError)
						{
							this.ShowMsg("编缉商品分类错误,未知", false);
						}
						else
						{
							this.ShowMsg("不能自己成为自己的上级分类", false);
						}
					}
					else
					{
                        //保存成功后清空分类缓存
                        HiCache.Remove("DataCache-CategoriesRange");
						base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
					}
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
				this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
				if (!this.Page.IsPostBack)
				{
					CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
					this.dropProductTypes.DataBind();
					this.dropProductTypes.SelectedValue = category.AssociatedProductType;
					if (category == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(category, false);
						this.BindCategoryInfo(category);
						if (category.Depth > 1)
						{
							this.liRewriteName.Style.Add("display", "none");
						}
					}
				}
			}
		}
	}
}
