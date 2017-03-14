using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Core.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Hidistro.ControlPanel.Store;
using System.Collections.Specialized;
using Hidistro.SaleSystem.Vshop;
using Hidistro.Entities.Comments;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Core.Enums;

public partial class Admin_product_ImportOfGoods : AdminPage
{

    private int? categoryId;
    private System.DateTime? endDate;
    private string productCode;
    private string productName;
    private System.DateTime? startDate;
    private int? tagId;
    private int? typeId;
    protected string LocalUrl = string.Empty;
    private string CommoditySource;
    private string CommodityCode;
    private ProductSaleStatus saleStatus = ProductSaleStatus.All;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
        this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
        if (!this.Page.IsPostBack)
        {
            this.dropCategories.IsUnclassified = true;
            this.dropCategories.DataBind();
            this.dropBrandList.DataBind();
            this.dropTagList.DataBind();
            this.dropType.DataBind();
            this.BindProducts();
        }
    }
    private void BindProducts()
    {
        this.LoadParameters();
        ProductQuery entity = new ProductQuery
        {
            Keywords = this.productName,
            ProductCode = this.productCode,
            CategoryId = this.categoryId,
            PageSize = this.pager.PageSize,
            PageIndex = this.pager.PageIndex,
            SortOrder = SortAction.Desc,
            SortBy = "DisplaySequence",
            StartDate = this.startDate,
            BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
            TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
            TypeId = this.typeId,
            EndDate = this.endDate,
            SaleStatus = this.saleStatus,
        };
        if (this.categoryId.HasValue && this.categoryId > 0)
        {
            entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
        }
        Globals.EntityCoding(entity, true);
        DbQueryResult products = ProductHelper.GetProductsAndCode(entity);
        this.grdProducts.DataSource = products.Data;
        this.grdProducts.DataBind();
        this.txtSearchText.Text = entity.Keywords;
        this.dropCategories.SelectedValue = entity.CategoryId;
        this.dropType.SelectedValue = entity.TypeId;
        this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
    }


    private void LoadParameters()
    {
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
        {
            this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CommoditySource"]))
        {
            this.CommoditySource = Globals.UrlDecode(this.Page.Request.QueryString["CommoditySource"]);
        }
        //if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Distributor"]))
        //{
        //    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["Distributor"]);
        //}
        //if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Price"]))
        //{
        //    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["Price"]);
        //}
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CommodityCode"]))
        {
            this.CommodityCode = Globals.UrlDecode(this.Page.Request.QueryString["CommodityCode"]);
        } 
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
        {
            this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
        }
        int result = 0;
        if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
        {
            this.categoryId = new int?(result);
        }
        int num2 = 0;
        if (int.TryParse(this.Page.Request.QueryString["brandId"], out num2))
        {
            this.dropBrandList.SelectedValue = new int?(num2);
        }
        int num3 = 0;
        if (int.TryParse(this.Page.Request.QueryString["tagId"], out num3))
        {
            this.tagId = new int?(num3);
        }
        int num4 = 0;
        if (int.TryParse(this.Page.Request.QueryString["typeId"], out num4))
        {
            this.typeId = new int?(num4);
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
        {
            this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
        }
        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
        {
            this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
        }
        this.txtSearchText.Text = this.productName;
        this.dropCategories.DataBind();
        this.dropCategories.SelectedValue = this.categoryId;
        this.dropTagList.DataBind();
        this.dropTagList.SelectedValue = this.tagId;
        this.calendarStartDate.SelectedDate = this.startDate;
        this.calendarEndDate.SelectedDate = this.endDate;
        this.dropType.SelectedValue = this.typeId;
    }

    private void btnSearch_Click(object sender, System.EventArgs e)
    {
        this.ReloadProductOnSales(true);
    }

    private void ReloadProductOnSales(bool isSearch)
    {
        NameValueCollection queryStrings = new NameValueCollection();
        queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
        if (this.dropCategories.SelectedValue.HasValue)
        {
            queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
        }
        queryStrings.Add("pageSize", this.pager.PageSize.ToString());
        if (!isSearch)
        {
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
        }
        if (this.calendarStartDate.SelectedDate.HasValue)
        {
            queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
        }
        if (this.calendarEndDate.SelectedDate.HasValue)
        {
            queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
        }
        if (this.dropBrandList.SelectedValue.HasValue)
        {
            queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
        }
        if (this.dropTagList.SelectedValue.HasValue)
        {
            queryStrings.Add("tagId", this.dropTagList.SelectedValue.ToString());
        }
        if (this.dropType.SelectedValue.HasValue)
        {
            queryStrings.Add("typeId", this.dropType.SelectedValue.ToString());
        }  
        base.ReloadPage(queryStrings);
    }
}
