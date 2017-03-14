using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Config;
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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddProducts)]
	public class AddProduct : ProductBasePage
	{
		protected System.Web.UI.WebControls.Button btnAdd;
		private int categoryId;
		protected System.Web.UI.WebControls.CheckBox ChkisfreeShipping;
		protected System.Web.UI.WebControls.CheckBox chkSkuEnabled;
		protected System.Web.UI.WebControls.CheckBox ckbIsDownPic;
		protected BrandCategoriesDropDownList dropBrandCategories;
        protected BrandCategoriesCheckBoxList BrandCategories;
		protected ProductTypeDownList dropProductTypes;
		protected KindeditorControl editDescription;
		protected System.Web.UI.HtmlControls.HtmlGenericControl l_tags;
		protected System.Web.UI.WebControls.Literal litCategoryName;
		protected ProductTagsLiteral litralProductTag;
		protected System.Web.UI.WebControls.HyperLink lnkEditCategory;
		protected System.Web.UI.WebControls.RadioButton radInStock;
		protected System.Web.UI.WebControls.RadioButton radOnSales;
		protected System.Web.UI.WebControls.RadioButton radUnSales;
		protected Script Script1;
		protected Script Script2;
		protected TrimTextBox txtAttributes;
		protected TrimTextBox txtCostPrice;
		protected TrimTextBox txtMarketPrice;
		protected TrimTextBox txtMemberPrices;
		protected TrimTextBox txtProductCode;
		protected TrimTextBox txtProductName;
		protected TrimTextBox txtProductTag;
		protected TrimTextBox txtSalePrice;
		protected TrimTextBox txtShortDescription;
		protected TrimTextBox txtSku;
		protected TrimTextBox txtSkus;
		protected TrimTextBox txtStock;
		protected TrimTextBox txtUnit;
		protected TrimTextBox txtWeight;
        protected ProductFlashUpload ucFlashUpload1;
        protected DropDownList DDLRange;//商品显示范围
        //protected ImageUploader uploader1;
        //protected ImageUploader uploader2;
        //protected ImageUploader uploader3;
        //protected ImageUploader uploader4;
        //protected ImageUploader uploader5;
        protected System.Web.UI.HtmlControls.HtmlGenericControl spDropDownList;
        protected System.Web.UI.HtmlControls.HtmlGenericControl spCheckBox;
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			decimal num3;
			decimal? nullable;
			decimal? nullable2;
			int num4;
			decimal? nullable3;
			int num5;
			if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num3, out nullable, out nullable2, out num4, out nullable3, out num5))
			{
				if (!this.chkSkuEnabled.Checked)
				{
					if (num3 <= 0m)
					{
						this.ShowMsg("商品一口价必须大于0", false);
						return;
					}
					if (nullable.HasValue && nullable.Value >= num3)
					{
						this.ShowMsg("商品成本价必须小于商品一口价", false);
						return;
					}
				}
				string text = this.editDescription.Text;
				if (this.ckbIsDownPic.Checked)
				{
					text = base.DownRemotePic(text);
				}
                
                string str1 = this.ucFlashUpload1.Value.Trim();
                //this.ucFlashUpload1.Value = str1;
                string[] strArrays = str1.Split(new char[] { ',' });
                string[] strArrays1 = new string[] { "", "", "", "", "" };
                string[] strArrays2 = strArrays1;
                for (int i = 0; i < (int)strArrays.Length && i < 5; i++)
                {
                    strArrays2[i] = strArrays[i];
                }
                ProductInfo target = null;
                    string strChecks= "";
                    if (CustomConfigHelper.Instance.BrandShow)
                    {
                        for (int i = 0; i < this.BrandCategories.Items.Count; i++)
                        {
                            bool a = this.BrandCategories.Items[i].Selected;
                            if (a)
                            {
                                strChecks += this.BrandCategories.Items[i].Value + ",";
                            }
                        }
                        target = new ProductInfo
                        {
                            CategoryId = this.categoryId,
                            TypeId = this.dropProductTypes.SelectedValue,
                            ProductName = this.txtProductName.Text,
                            ProductCode = this.txtProductCode.Text,
                            MarketPrice = nullable2,
                            Unit = this.txtUnit.Text,
                            Range = Convert.ToInt32(this.DDLRange.SelectedValue),
                            ImageUrl1 = strArrays2[0],
                            ImageUrl2 = strArrays2[1],
                            ImageUrl3 = strArrays2[2],
                            ImageUrl4 = strArrays2[3],
                            ImageUrl5 = strArrays2[4],
                            ThumbnailUrl40 = strArrays2[0].Replace("/images/", "/thumbs40/40_"),
                            ThumbnailUrl60 = strArrays2[0].Replace("/images/", "/thumbs60/60_"),
                            ThumbnailUrl100 = strArrays2[0].Replace("/images/", "/thumbs100/100_"),
                            ThumbnailUrl160 = strArrays2[0].Replace("/images/", "/thumbs160/160_"),
                            ThumbnailUrl180 = strArrays2[0].Replace("/images/", "/thumbs180/180_"),
                            ThumbnailUrl220 = strArrays2[0].Replace("/images/", "/thumbs220/220_"),
                            ThumbnailUrl310 = strArrays2[0].Replace("/images/", "/thumbs310/310_"),
                            ThumbnailUrl410 = strArrays2[0].Replace("/images/", "/thumbs410/410_"),
                            ShortDescription = this.txtShortDescription.Text,
                            IsfreeShipping = this.ChkisfreeShipping.Checked,
                            Description = (!string.IsNullOrEmpty(text) && text.Length > 0) ? text : null,
                            AddedDate = System.DateTime.Now,
                            BrandId =strChecks.TrimEnd(','),
                            MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
                        };
                    }
                    else
                    {
                        target = new ProductInfo
                         {
                             CategoryId = this.categoryId,
                             TypeId = this.dropProductTypes.SelectedValue,
                             ProductName = this.txtProductName.Text,
                             ProductCode = this.txtProductCode.Text,
                             MarketPrice = nullable2,
                             Unit = this.txtUnit.Text,
                             Range = Convert.ToInt32(this.DDLRange.SelectedValue),
                             ImageUrl1 = strArrays2[0],
                             ImageUrl2 = strArrays2[1],
                             ImageUrl3 = strArrays2[2],
                             ImageUrl4 = strArrays2[3],
                             ImageUrl5 = strArrays2[4],
                             ThumbnailUrl40 = strArrays2[0].Replace("/images/", "/thumbs40/40_"),
                             ThumbnailUrl60 = strArrays2[0].Replace("/images/", "/thumbs60/60_"),
                             ThumbnailUrl100 = strArrays2[0].Replace("/images/", "/thumbs100/100_"),
                             ThumbnailUrl160 = strArrays2[0].Replace("/images/", "/thumbs160/160_"),
                             ThumbnailUrl180 = strArrays2[0].Replace("/images/", "/thumbs180/180_"),
                             ThumbnailUrl220 = strArrays2[0].Replace("/images/", "/thumbs220/220_"),
                             ThumbnailUrl310 = strArrays2[0].Replace("/images/", "/thumbs310/310_"),
                             ThumbnailUrl410 = strArrays2[0].Replace("/images/", "/thumbs410/410_"),
                             ShortDescription = this.txtShortDescription.Text,
                             IsfreeShipping = this.ChkisfreeShipping.Checked,
                             Description = (!string.IsNullOrEmpty(text) && text.Length > 0) ? text : null,
                             AddedDate = System.DateTime.Now,
                             BrandId = this.dropBrandCategories.SelectedValue.ToString(),
                             MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|"
                         };

                    }
				ProductSaleStatus onSale = ProductSaleStatus.OnSale;
				if (this.radInStock.Checked)
				{
					onSale = ProductSaleStatus.OnStock;
				}
				if (this.radUnSales.Checked)
				{
					onSale = ProductSaleStatus.UnSale;
				}
				if (this.radOnSales.Checked)
				{
					onSale = ProductSaleStatus.OnSale;
				}
				target.SaleStatus = onSale;
                target.StoreId = ManagerHelper.GetCurrentManager().ClientUserId; //pro辣特殊需求,不同门店可以上架不同商品,商品扩展storeid存的是当前门店管理员的clientid,对应分销商表的userid

				System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> attrs = null;
				System.Collections.Generic.Dictionary<string, SKUItem> skus;
				if (this.chkSkuEnabled.Checked)
				{
					target.HasSKU = true;
					skus = base.GetSkus(this.txtSkus.Text);
				}
				else
				{
					System.Collections.Generic.Dictionary<string, SKUItem> dictionary3 = new System.Collections.Generic.Dictionary<string, SKUItem>();
					SKUItem item = new SKUItem
					{
						SkuId = "0",
						SKU = this.txtSku.Text,
						SalePrice = num3,
						CostPrice = nullable.HasValue ? nullable.Value : 0m,
						Stock = num4,
						Weight = nullable3.HasValue ? nullable3.Value : 0m
					};
					dictionary3.Add("0", item);
					skus = dictionary3;
					if (this.txtMemberPrices.Text.Length > 0)
					{
						base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
					}
				}
				if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
				{
					attrs = base.GetAttributes(this.txtAttributes.Text);
				}
				ValidationResults validateResults = Validation.Validate<ProductInfo>(target, new string[]
				{
					"AddProduct"
				});
				if (!validateResults.IsValid)
				{
					this.ShowMsg(validateResults);
				}
				else
				{
					System.Collections.Generic.IList<int> tagsId = new System.Collections.Generic.List<int>();
					if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
					{
						string str2 = this.txtProductTag.Text.Trim();
						string[] strArray;
						if (str2.Contains(","))
						{
							strArray = str2.Split(new char[]
							{
								','
							});
						}
						else
						{
							strArray = new string[]
							{
								str2
							};
						}
						string[] array = strArray;
						for (int i = 0; i < array.Length; i++)
						{
							string str3 = array[i];
							tagsId.Add(System.Convert.ToInt32(str3));
						}
					}

                    //获取当前子账号门店,以判断子门店是否编辑过商品,若点击保存,商品直接被设置为待审核状态
                    ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                    int storeId = ManagerHelper.getClientUserIdBySenderId(currentManager.UserId);
                    //如果当前子门店编辑过,商品设置为待审核状态 爽爽挝啡2.0
                    if (storeId > 0 && CustomConfigHelper.Instance.AutoShipping && CustomConfigHelper.Instance.AnonymousOrder)
                    {
                        target.ReviewState = 1;//待审核
                    }
                    //如果当前子门店编辑过,商品设置为待审核状态 爽爽挝啡2.0
                    if (storeId > 0 && CustomConfigHelper.Instance.AutoShipping && CustomConfigHelper.Instance.AnonymousOrder)
                    {
                        target.ReviewState = 1;//待审核
                    }
					switch (ProductHelper.AddProduct(target, skus, attrs, tagsId))
					{
					case ProductActionStatus.Success:
						this.ShowMsg("添加商品成功", true);
						base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", this.categoryId, target.ProductId)), true);
						return;
					case ProductActionStatus.DuplicateName:
						this.ShowMsg("添加商品失败，商品名称不能重复", false);
						return;
					case ProductActionStatus.DuplicateSKU:
						this.ShowMsg("添加商品失败，商家编码不能重复", false);
						return;
					case ProductActionStatus.SKUError:
						this.ShowMsg("添加商品失败，商家编码不能重复", false);
						return;
					case ProductActionStatus.AttributeError:
						this.ShowMsg("添加商品失败，保存商品属性时出错", false);
						return;
					case ProductActionStatus.ProductTagEroor:
						this.ShowMsg("添加商品失败，保存商品标签时出错", false);
						return;
					}
					this.ShowMsg("添加商品失败，未知错误", false);
				}
			}
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				base.DoCallback();
			}
			else
			{
				if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					if (!this.Page.IsPostBack)
					{
						this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
						CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
						if (category == null)
						{
							base.GotoResourceNotFound();
						}
						else
						{
							if (!string.IsNullOrEmpty(this.litralProductTag.Text))
							{
								this.l_tags.Visible = true;
							}
							this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(System.Globalization.CultureInfo.InvariantCulture);
							this.dropProductTypes.DataBind();
							this.dropProductTypes.SelectedValue = category.AssociatedProductType;
                            if (CustomConfigHelper.Instance.BrandShow)
                            {
                                this.BrandCategories.Visible =true;
                                this.dropBrandCategories.Visible = false;
                                spDropDownList.Visible = false;
                                this.BrandCategories.DataBind();
                            }
                            else
                            {
                                spCheckBox.Visible = false;
                                this.dropBrandCategories.Visible = true;
                                this.BrandCategories.Visible = false;
                                this.dropBrandCategories.DataBind();
                            }
							this.txtProductCode.Text = (this.txtSku.Text = category.SKUPrefix + new System.Random(System.DateTime.Now.Millisecond).Next(1, 99999).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(5, '0'));
						}
					}
				}
			}
		}
		private bool ValidateConverts(bool skuEnabled, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight, out int lineId)
		{
			string str = string.Empty;
			costPrice = new decimal?(0m);
			marketPrice = new decimal?(0m);
			weight = new decimal?(0m);
			stock = (lineId = 0);
			salePrice = 0m;
			if (this.txtProductCode.Text.Length > 20)
			{
				str += Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
			}
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
			{
				decimal num;
				if (decimal.TryParse(this.txtMarketPrice.Text, out num))
				{
					marketPrice = new decimal?(num);
				}
				else
				{
					str += Formatter.FormatErrorMessage("请正确填写商品的市场价");
				}
			}
			if (!skuEnabled)
			{
				if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
				{
					str += Formatter.FormatErrorMessage("请正确填写商品一口价");
				}
				if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
				{
					decimal num2;
					if (decimal.TryParse(this.txtCostPrice.Text, out num2))
					{
						costPrice = new decimal?(num2);
					}
					else
					{
						str += Formatter.FormatErrorMessage("请正确填写商品的成本价");
					}
				}
				if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
				{
					str += Formatter.FormatErrorMessage("请正确填写商品的库存数量");
				}
				if (!string.IsNullOrEmpty(this.txtWeight.Text))
				{
					decimal num3;
					if (decimal.TryParse(this.txtWeight.Text, out num3))
					{
						weight = new decimal?(num3);
					}
					else
					{
						str += Formatter.FormatErrorMessage("请正确填写商品的重量");
					}
				}
			}
			bool result;
			if (!string.IsNullOrEmpty(str))
			{
				this.ShowMsg(str, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
