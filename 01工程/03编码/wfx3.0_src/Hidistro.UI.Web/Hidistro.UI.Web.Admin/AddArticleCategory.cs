﻿namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ArticleCategories)]
    public class AddArticleCategory : AdminPage
    {
        protected Button btnSubmitArticleCategory;
        protected FileUpload fileUpload;
        protected TextBox txtArticleCategoryiesDesc;
        protected TextBox txtArticleCategoryiesName;

        private void AddNewCategory(ArticleCategoryInfo category)
        {
            if (ArticleHelper.CreateArticleCategory(category))
            {
                if (this.Page.Request.QueryString["source"] == "add")
                {
                    this.CloseWindow();
                }
                else
                {
                    this.Reset();
                    this.ShowMsg("成功添加了一个文章分类", true);
                }
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (this.fileUpload.HasFile)
            {
                try
                {
                    str = ArticleHelper.UploadArticleImage(this.fileUpload.PostedFile);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            ArticleCategoryInfo target = new ArticleCategoryInfo {
                Name = this.txtArticleCategoryiesName.Text.Trim(),
                IconUrl = str,
                Description = this.txtArticleCategoryiesDesc.Text.Trim()
            };
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleCategoryInfo>(target, new string[] { "ValArticleCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                this.AddNewCategory(target);
            }
            else
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitArticleCategory.Click += new EventHandler(this.btnSubmitArticleCategory_Click);
        }

        private void Reset()
        {
            this.txtArticleCategoryiesName.Text = string.Empty;
            this.txtArticleCategoryiesDesc.Text = string.Empty;
        }
    }
}

