using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Function;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Core;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.Products)]
    public class AgentProducts : AdminPage
	{
		protected ImageLinkButton btnDelete;
		//protected System.Web.UI.WebControls.Button btnSearch;
        protected System.Web.UI.WebControls.Button btnBuy;
		protected Grid grdProducts;
        private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
        protected string LocalUrl = string.Empty;
        protected HiddenField hiSkuIds;//隐藏域,传递的商品ID集合
        protected HiddenField hiValue;//隐藏域,传递的商品ID集合
        protected HiddenField hiProductId;//隐藏域,传递的商品ID集合
        protected System.Web.UI.WebControls.Literal litTitle;//提示语
        protected System.Web.UI.WebControls.DropDownList userAddress;
        protected System.Web.UI.WebControls.DropDownList userGiveMode;
        protected System.Web.UI.WebControls.DropDownList userPayMode;
        protected System.Web.UI.WebControls.TextBox txtOrderRemark;

		private void BindProducts()
		{
			this.LoadParameters();

            //得到当前登录用户所在区域
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();//当前登录用户信息

            ////得到商品区域关系表结构
            //string strSkuIds = string.Empty;
            //DataTable dtAgentProduct = DataBaseHelper.GetDataTable("Erp_AgentProduct", string.Format("UserId = '{0}'", currentManager.UserId), null);
            //foreach(DataRow dr in dtAgentProduct.Rows)
            //{
            //    strSkuIds += "'" + dr["SkuId"].ToString() + "',";
            //}
            //strSkuIds = strSkuIds.TrimEnd(',');
            //string strWhere = string.IsNullOrEmpty(strSkuIds) ? "1=2" : "SkuId in (" + strSkuIds + ")";
            string strWhere = string.Format(@"UserId = {0}", currentManager.UserId);
            DataTable dtProduct = ManagerHelper.GetAgentProduct(strWhere);
            if (dtProduct.Rows.Count == 0)
            {
                this.litTitle.Text = "当前用户无订货商品";
                this.litTitle.Visible = true;
            }
            this.grdProducts.DataSource = dtProduct;
            this.grdProducts.DataBind();

            //ProductQuery entity = new ProductQuery
            //{
            //    Keywords = this.productName,
            //    ProductCode = this.productCode,
            //    CategoryId = this.categoryId,
            //    PageSize = this.pager.PageSize,
            //    PageIndex = this.pager.PageIndex,
            //    SortOrder = SortAction.Desc,
            //    SortBy = "DisplaySequence",
            //    StartDate = this.startDate,
            //    BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
            //    TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
            //    TypeId = this.typeId,
            //    SaleStatus = this.saleStatus,
            //    EndDate = this.endDate,
            //    SkuIds = strSkuIds
            //};
            //if (this.categoryId.HasValue && this.categoryId > 0)
            //{
            //    entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            //}
            //Globals.EntityCoding(entity, true);
            //DbQueryResult products = ProductHelper.GetProductsByAgent(entity);
            //this.grdProducts.DataSource = products.Data;
            //this.grdProducts.DataBind();
            //this.txtSearchText.Text = entity.Keywords;
            //this.txtSKU.Text = entity.ProductCode;
            //this.dropCategories.SelectedValue = entity.CategoryId;
            //this.dropType.SelectedValue = entity.TypeId;
            //this.pager1.TotalRecords = (this.pager.TotalRecords = products.TotalRecords);
		}
        //private void btnCancelFreeShip_Click(object sender, System.EventArgs e)
        //{
        //    throw new System.NotImplementedException();
        //}
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的商品", false);
            }
            else
            {
                //得到当前登录用户所在区域
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();//当前登录用户信息

                string[] strIds = str.Split(',');
                str = string.Empty;
                for (int i = 0; i < strIds.Length; i++)
                {
                    if (i == strIds.Length - 1)
                        str += "'" + strIds[i] + "'";
                    else
                        str += "'" + strIds[i] + "',";
                }
                if (ManagerHelper.DeleteAgentProduct(str, currentManager.UserId))
                {
                    this.ShowMsg("成功删除了选择的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("删除商品失败，未知错误", false);
                }
            }
        }
        //private void btnInStock_Click(object sender, System.EventArgs e)
        //{
        //    string str = base.Request.Form["CheckBoxGroup"];
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        this.ShowMsg("请先选择要入库的商品", false);
        //    }
        //    else
        //    {
        //        if (ProductHelper.InStock(str) > 0)
        //        {
        //            this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
        //            this.BindProducts();
        //        }
        //        else
        //        {
        //            this.ShowMsg("入库商品失败，未知错误", false);
        //        }
        //    }
        //}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}

        /// <summary>
        /// 提交到订货列表按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuy_Click(object sender, System.EventArgs e)
        {
            
            this.btnBuy.Enabled = false;//禁用当前按钮
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();//当前登录用户信息
            if (currentManager != null /*&& currentManager.ClientUserId > 0*/)
            {
                
                string strSkuIds = string.Empty;//存储所有订购商品的SkuId，用于操作后清除待选列表Erp_AgentProduct表数据
                int shipaddressId = 0;//送货地址Id
                int givemodeId = 0;//配送方式Id
                int paymodeId = 0;//支付方式Id

                ShoppingCartInfo shoppingCart = null;

                OrderInfo orderInfo = new OrderInfo();
                MemberInfo currentMember = MemberHelper.GetMember(currentManager.ClientUserId);
                foreach (System.Web.UI.WebControls.GridViewRow row in this.grdProducts.Rows)
                {
                    //decimal total = 0;
                    decimal price = 0;//单价
                    int resultNum = 0;//数量

                    System.Web.UI.WebControls.HiddenField txtboxvalue = (System.Web.UI.WebControls.HiddenField)row.FindControl("hiValue");//得到SkuId
                    //System.Web.UI.WebControls.HiddenField txtboxproduct = (System.Web.UI.WebControls.HiddenField)row.FindControl("hiProductId");//得到ProductId
                    System.Web.UI.WebControls.Literal litSalePrice = (System.Web.UI.WebControls.Literal)row.FindControl("litSalePrice");

                    if (int.TryParse(txtboxvalue.Value.Trim(), out resultNum) && decimal.TryParse(litSalePrice.Text, out price))
                    {
                        string skuId = this.grdProducts.DataKeys[row.RowIndex].Value.ToString();
                        strSkuIds += "'" + skuId + "',";//累加SkuId值并用'',''分割
                        if (!string.IsNullOrEmpty(skuId))
                        {
                            //首先将商品插入购物车
                            //后台订单的购物车处理
                            int pcUserid = 0;
                            if (Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.RegionalFunction && ManagerHelper.GetCurrentManager() != null)
                            {
                                int categoryId = CategoryBrowser.GetCategoryIdBySkuId(skuId);
                                pcUserid = currentManager.UserId;
                                ShoppingCartProcessor.AddLineItemPC(skuId, resultNum, categoryId, pcUserid);
                            }


                            //total += price * resultNum;//计算合计
                            //生成订单项

                            //shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(currentMember, price, skuId, resultNum);
                            shoppingCart = ShoppingCartProcessor.GetShoppingCart(currentManager.UserId);
                            if (shoppingCart != null && shoppingCart.LineItems != null && shoppingCart.LineItems.Count > 0)
                            {
                                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                                {
                                    LineItemInfo info3 = new LineItemInfo
                                    {
                                        SkuId = info2.SkuId,
                                        ProductId = info2.ProductId,
                                        SKU = info2.SKU,
                                        Quantity = info2.Quantity,
                                        ShipmentQuantity = info2.ShippQuantity,
                                        ItemCostPrice = new SkuDao().GetSkuItem(info2.SkuId).CostPrice,
                                        ItemListPrice = info2.MemberPrice,
                                        ItemAdjustedPrice = info2.AdjustedPrice,
                                        ItemDescription = info2.Name,
                                        ThumbnailsUrl = info2.ThumbnailUrl40,
                                        ItemWeight = info2.Weight,
                                        SKUContent = info2.SkuContent,
                                        PromotionId = info2.PromotionId,
                                        PromotionName = info2.PromotionName,
                                        MainCategoryPath = info2.MainCategoryPath
                                    };
                                    orderInfo.LineItems.Add(info3.SkuId, info3);
                                }
                            }
                            else
                            {
                                this.ShowMsg("订单生成失败。", true);
                                this.btnBuy.Enabled = true;//启用当前按钮
                                return;
                            }
                        }
                    }
                }
                //一个商品数量都没输入，则退出
                if(string.IsNullOrEmpty(strSkuIds))
                {
                    this.ShowMsg("请输入商品数量。", true);
                    this.btnBuy.Enabled = true;//启用当前按钮
                    return;
                }

                //送货地址
                if (int.TryParse(this.userAddress.SelectedValue, out shipaddressId))
                {
                    ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shipaddressId,Convert.ToInt32("99999" + currentManager.UserId.ToString()));//(shipaddressId, currentMember.UserId);
                    if (shippingAddress != null)
                    {
                        //this.userAddress.SelectedItem.Text
                        orderInfo.ShippingRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, "，");
                        orderInfo.RegionId = shippingAddress.RegionId;
                        orderInfo.Address = shippingAddress.Address;
                        orderInfo.ZipCode = shippingAddress.Zipcode;
                        orderInfo.ShipTo = shippingAddress.ShipTo;
                        orderInfo.TelPhone = shippingAddress.TelPhone;
                        orderInfo.CellPhone = shippingAddress.CellPhone;
                        MemberProcessor.SetDefaultShippingAddress(shipaddressId, Convert.ToInt32("99999" + currentManager.UserId.ToString()));
                    }
                }
                //配送方式
                if (int.TryParse(this.userGiveMode.SelectedValue, out givemodeId))
                {
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(givemodeId, true);
                    if (shippingMode != null)
                    {
                        orderInfo.ShippingModeId = shippingMode.ModeId;
                        orderInfo.ModeName = shippingMode.Name;
                        if (shoppingCart.LineItems.Count != shoppingCart.LineItems.Count((ShoppingCartItemInfo a) => a.IsfreeShipping))
                        {
                            orderInfo.AdjustedFreight = (orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCart.Weight, shippingMode));
                        }
                        else
                        {
                            orderInfo.AdjustedFreight = (orderInfo.Freight = 0m);
                        }
                    }
                }
                //支付方式
                if (int.TryParse(this.userPayMode.SelectedValue, out paymodeId))
                {
                    orderInfo.PaymentTypeId = paymodeId;
                    switch (paymodeId)
                    {
                        //case -1:
                        //case 0:
                        //    {
                        //        orderInfo.PaymentType = "货到付款";
                        //        orderInfo.Gateway = "hishop.plugins.payment.podrequest";
                        //        break;
                        //    }
                        //case 88:
                        //    {
                        //        orderInfo.PaymentType = "微信支付";
                        //        orderInfo.Gateway = "hishop.plugins.payment.weixinrequest";
                        //        break;
                        //    }
                        case 99:
                            {
                                orderInfo.PaymentType = "线下付款";
                                orderInfo.Gateway = "hishop.plugins.payment.offlinerequest";
                                break;
                            }
                        default:
                            {
                                PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(paymodeId);
                                if (paymentMode != null)
                                {
                                    orderInfo.PaymentTypeId = paymentMode.ModeId;
                                    orderInfo.PaymentType = paymentMode.Name;
                                    orderInfo.Gateway = paymentMode.Gateway;
                                }
                                break;
                            }
                    }

                }
                orderInfo.OrderId = this.GenerateOrderId(currentManager.UserId);//生成ID
                orderInfo.OrderDate = System.DateTime.Now;//当前时间
                //基本信息
                orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
                orderInfo.RefundStatus = RefundStatus.None;
                orderInfo.ShipToDate = "时间不限";
                orderInfo.ReferralUserId = 0;//订单的所属分销ID,没有就设置为0
                
                //代理商用户相关信息
                orderInfo.UserId = Convert.ToInt32("99999" + currentManager.UserId.ToString());//currentMember.UserId;
                orderInfo.Username = currentManager.UserName;//currentMember.UserName;
                orderInfo.EmailAddress = currentManager.Email;//currentMember.Email;

                //orderInfo.RealName = currentMember.RealName;
                orderInfo.RealName = currentManager.AgentName;//存储用户后台昵称

                //orderInfo.QQ = currentMember.QQ;
                orderInfo.Remark = this.txtOrderRemark.Text;//得到前端TextBox值
                orderInfo.OrderSource = 1;//来源代理商采购
                this.SetOrderItemStatus(orderInfo);

                

                if (ShoppingProcessor.CreatOrder(orderInfo))
                {
                    ShoppingCartProcessor.ClearShoppingCartPC();
                    //订单生成成功后清空
                    strSkuIds = strSkuIds.TrimEnd(',');
                    //清除已经订购的商品在订购列表中
                    ProductBrowser.DeleteAgentProduct(strSkuIds, currentManager.UserId);

                    this.ShowMsg("订单生成成功，请尽快完成支付。", true);
                }
                else
                {
                    this.ShowMsg("订单生成失败。", true);
                }

                //HiCache.Remove("DataCache-Categories");//刷前台缓存
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("当前登录用户不是前端用户升级而来，无法进行生成订单操作。", true);
            }
            this.btnBuy.Enabled = true;//启用当前按钮
        }

        /// <summary>
        /// 计算代理商订单ID
        /// </summary>
        /// <param name="userId">后台用户Id</param>
        /// <returns>订单ID</returns>
        private string GenerateOrderId(int userId)
        {
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            return System.DateTime.Now.ToString("yyyyMMdd") + str + "_" + userId.ToString();
        }

        /// <summary>
        /// 设置订单子项状态
        /// </summary>
        /// <param name="order"></param>
        public void SetOrderItemStatus(OrderInfo order)
        {
            System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
            LineItemInfo lineItemInfo = new LineItemInfo();
            foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
            {
                lineItemInfo = current.Value;
                lineItemInfo.OrderItemsStatus = OrderStatus.WaitBuyerPay;
            }
        }

        
        /// <summary>
        /// Grid绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
                System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litCostPrice");
                System.Web.UI.WebControls.Literal literal3 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litRegionName");
                System.Web.UI.WebControls.Literal literal4 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSkuId");

				if (literal.Text == "1")
				{
					literal.Text = "出售中";
				}
				else
				{
					if (literal.Text == "2")
					{
						literal.Text = "下架区";
					}
					else
					{
						literal.Text = "仓库中";
					}
				}
                //if (string.IsNullOrEmpty(literal2.Text))
                //{
                //    literal2.Text = "-";
                //}
                if (!string.IsNullOrEmpty(literal3.Text))
                {
                    DataTable dtProductRegion = (DataTable)ViewState["dtProductRegion"];
                    DataRow[] drPr = dtProductRegion.Select(string.Format(@"ProductID = '{0}'", literal3.Text), "", DataViewRowState.CurrentRows);
                    literal3.Text = string.Empty;
                    foreach (DataRow dr in drPr)
                    {
                        literal3.Text += "<div style=\"float: left;\">" + dr["RegionName"].ToString() + "</div>\r\n";
                    }
                }
                if (!string.IsNullOrEmpty(literal4.Text) && ViewState["dtSkuItem"] != null)
                {
                    DataTable dtSkuItems = (DataTable)ViewState["dtSkuItem"];
                    string strProductId = ((System.Data.DataRowView)(e.Row.DataItem)).Row["ProductId"].ToString();
                    DataRow[] dritems = dtSkuItems.Select(string.Format(@"SkuId = '{0}' and ProductId = '{1}'", literal4.Text, strProductId), "", DataViewRowState.CurrentRows);
                    literal4.Text = string.Empty;
                    foreach (DataRow dr in dritems)
                    {
                        literal4.Text += "<div style=\"float: left;\">" + dr["AttributeName"].ToString() + "：" + dr["ValueStr"].ToString() + "</div>\r\n";
                    }
                }

			}
		}
        
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
			//this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			if (!this.Page.IsPostBack)
			{
                DataTable dtSkuItems = ProductBrowser.GetSkuItems();
                ViewState["dtSkuItem"] = dtSkuItems;//存储到全局值

                DataTable dtProductRegion = ManagerHelper.GetProductRegion(string.Empty);
                ViewState["dtProductRegion"] = dtProductRegion;//存储到全局值

                DataTable dtRegion = ManagerHelper.GetRegion(string.Empty);

                //得到当前登录用户所在区域
                ListItem itemDefault = new ListItem("请选择送货地址", "", true);
                userAddress.Items.Add(itemDefault);
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();//当前登录用户信息
                IList<ShippingAddressInfo> shippingAddress = MemberProcessor.GetShippingAddresses(Convert.ToInt32("99999" + currentManager.UserId.ToString()));//(currentManager.ClientUserId);
                if (shippingAddress != null && shippingAddress.Count > 0)
                {
                    foreach (ShippingAddressInfo info in shippingAddress)
                    {
                        string strText = info.ShipTo + info.CellPhone + GetRegionName(dtRegion, info.RegionId) + info.Address;
                        ListItem item = new ListItem(strText, info.ShippingId.ToString(), true);
                        userAddress.Items.Add(item);
                    }
                }

                //添加配送方式下拉框
                ListItem itemGiveDefault = new ListItem("请选择支付方式", "", true);
                userGiveMode.Items.Add(itemGiveDefault);
                IList<ShippingModeInfo> shippingModes = ShoppingProcessor.GetShippingModes();
                if (shippingModes != null && shippingModes.Count > 0)
                {
                    foreach (ShippingModeInfo info in shippingModes)
                    {
                        ListItem itemGive = new ListItem(info.Name, info.ModeId.ToString(), true);
                        userGiveMode.Items.Add(itemGive);
                    }
                }

                //添加支付方式下拉框
                ListItem itemPayDefault = new ListItem("请选择支付方式", "", true);
                userPayMode.Items.Add(itemPayDefault);
                ListItem itemPayNotLine = new ListItem("线下支付", "99", true);
                userPayMode.Items.Add(itemPayNotLine);
                //得到数据库中的支付方式列表
                IList<PaymentModeInfo> paymentMode = ShoppingProcessor.GetPaymentModes();
                if (paymentMode != null && paymentMode.Count > 0)
                {
                    foreach (PaymentModeInfo info in paymentMode)
                    {
                        ListItem itemPay = new ListItem(info.Name, info.ModeId.ToString(), true);
                        userPayMode.Items.Add(itemPay);
                    }
                }

                //设置订单说明默认值
                this.txtOrderRemark.Text = "代理商采购订单";

                //绑定订货列表
				this.BindProducts();

			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}

        /// <summary>
        /// 根据Regionid得到ReginName值
        /// </summary>
        /// <param name="dtRegion">表</param>
        /// <param name="regionid">区域Id</param>
        /// <returns></returns>
        private string GetRegionName(DataTable dtRegion, int regionid)
        {
            string strReturn = string.Empty;
            if (regionid != 0)
            {
                DataRow[] drRows = dtRegion.Select(string.Format(@"ProvinceID = '{0}' and CityID = 0 and CountyID = 0", regionid), "", DataViewRowState.CurrentRows);
                if (drRows.Length > 0)
                {
                    strReturn = drRows[0]["RegionName"].ToString();
                }
                else
                {
                    drRows = dtRegion.Select(string.Format(@"CityID = '{0}' and CountyID = 0", regionid), "", DataViewRowState.CurrentRows);
                    if (drRows.Length > 0)
                    {
                        DataRow[] drParent = dtRegion.Select(string.Format(@"ProvinceID = '{0}' and CityID = 0 and CountyID = 0", drRows[0]["ProvinceID"].ToString()), "", DataViewRowState.CurrentRows);
                        if (drParent.Length > 0)
                            strReturn += drParent[0]["RegionName"].ToString();
                        strReturn += drRows[0]["RegionName"].ToString();
                    }
                    else
                    {
                        drRows = dtRegion.Select(string.Format(@"CountyID = '{0}'", regionid), "", DataViewRowState.CurrentRows);
                        if (drRows.Length > 0)
                        {
                            DataRow[] drParent1 = dtRegion.Select(string.Format(@"ProvinceID = '{0}' and CityID = 0 and CountyID = 0", drRows[0]["ProvinceID"].ToString()), "", DataViewRowState.CurrentRows);
                            DataRow[] drParent2 = dtRegion.Select(string.Format(@"CityID = '{0}' and CountyID = 0", drRows[0]["CityID"].ToString()), "", DataViewRowState.CurrentRows);
                            if (drParent1.Length > 0 && drParent2.Length > 0)
                            {
                                strReturn += drParent1[0]["RegionName"].ToString();
                                strReturn += drParent2[0]["RegionName"].ToString();
                            }
                            strReturn += drRows[0]["RegionName"].ToString();
                        }
                    }
                }
            }
            return strReturn;
        }

		private void ReloadProductOnSales(bool isSearch)
		{
			NameValueCollection queryStrings = new NameValueCollection();
			
            //if (!isSearch)
            //{
            //    queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            //}
            //if (this.calendarStartDate.SelectedDate.HasValue)
            //{
            //    queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            //}
            //if (this.calendarEndDate.SelectedDate.HasValue)
            //{
            //    queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            //}
			//queryStrings.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
			base.ReloadPage(queryStrings);
		}
	}
}
