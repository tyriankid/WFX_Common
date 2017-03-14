using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.SaleSystem.CodeBehind;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WxPayAPI;
namespace Hidistro.UI.Web.API
{
    /*
     与商品相关的无刷新操作
     */
    public class ProductsHandler : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private IDictionary<string, string> jsondict = new Dictionary<string, string>();

        
        public void ProcessRequest(System.Web.HttpContext context)
        {
            string text = context.Request["action"];
            switch (text)
            {
                case "getProductsAsyc":
                    this.getProductsAsyc(context);
                    break;
                case "GetQuickOrderSKUSelector":
                    this.GetQuickOrderSKUSelector(context);
                    break;
                case "GetQuickOrderSKUSelectorWithoutBtn":
                    this.GetQuickOrderSKUSelectorWithoutBtn(context);
                    break;
                case "GetShoppingCartGoodNum":
                    this.GetShoppingCartGoodNum(context);
                    break;
                case "GetShoppingCartTotal":
                    this.GetShoppingCartTotal(context);
                    break;
                case "ToggleProductSoldOut":
                    this.ToggleProductSoldOut(context);
                    break;
                case "sendPaySuccessMsg":
                    this.sendPaySuccessMsg(context);
                    break;
                case "getMemberCenterOrderNums":
                    this.getMemberCenterOrderNums(context);
                    break;
            }
        }
        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("error1.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// 获取个人中心代付款和配送中的订单数量以便于展示
        /// </summary>
        /// <param name="context"></param>
        private void getMemberCenterOrderNums(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string type = context.Request["type"];
            int[] result = MemberProcessor.GetMemberNums(MemberProcessor.GetCurrentMember().UserId);
            context.Response.Write("{\"waitPay\":\"" + result[0] + "\",\"paid\":\""+result[1]+"\"}");
        }

        /// <summary>
        /// 支付成功微信提醒
        /// </summary>
        /// <param name="context"></param>
        private void sendPaySuccessMsg(System.Web.HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                string orderId = context.Request["orderid"];
                string uid = context.Request["uid"];
                WriteLog("进入");
                string result = string.Empty;
                WriteLog("参数:" + orderId + " " + uid);
                MemberInfo member = MemberProcessor.GetMember(int.Parse(uid));
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                TemplateMessage templateMessage = new TemplateMessage();
                templateMessage.Url = "";//单击URL
                templateMessage.TemplateId = "Rx4a6JWjSRlIwctN0SuwGa82cjpzBnpZQWdwHz9CWQE";//Globals.GetMasterSettings(true).WX_Template_01;// "b1_ARggaBzbc5owqmwrZ15QPj9Ksfs0p5i64C6MzXKw";//消息模板ID
                templateMessage.Touser = member.OpenId;//用户OPENID


                TemplateMessage.MessagePart[] messateParts = new TemplateMessage.MessagePart[]{
                                                new TemplateMessage.MessagePart{Name = "first",Value = "您好，微信支付已成功！"},
                                                new TemplateMessage.MessagePart{Name = "keyword1",Value = orderInfo.OrderId},
                                                new TemplateMessage.MessagePart{Name = "keyword2",Value = orderInfo.GetTotal().ToString("F2")+"元"},
                                                new TemplateMessage.MessagePart{Name = "keyword3",Value = orderInfo.ModeName},
                                                new TemplateMessage.MessagePart{Name = "keyword4",Value = orderInfo.PayDate.ToString()},
                                                new TemplateMessage.MessagePart{Name = "remark",/*Color = "#FF0000"*/Value ="多谢您的惠顾！下单成功，正在等待接单，请留意接单通知！"}};
                templateMessage.Data = messateParts;
                TemplateApi.SendMessage(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), templateMessage);
                result = "ok";
                context.Response.Write("{\"result\":\"" + result + "\"}");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
            
        }

        /// <summary>
        /// 商品状态改为售罄
        /// </summary>
        /// <param name="context"></param>
        private void ToggleProductSoldOut(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int num = 0;
            int productId = int.TryParse(context.Request["productid"],out num)?num:0;
            string type = context.Request["type"];

            string result = string.Empty;
            if (ProductHelper.UpdateProductSaleStatus(productId.ToString(),type=="售罄"?ProductSaleStatus.SoldOut:ProductSaleStatus.OnSale))
            {
                result = type=="售罄"?"商品已改为售罄！补货后请及时将状态改回！":"商品已补货！可以正常下单了！";
            }
            else
            {
                result = "出错了";
            }
            context.Response.Write("{\"result\":\""+result+"\"}");
        }

        /// <summary>
        /// 根据分类id获取商品信息
        /// </summary>
        /// <param name="context"></param>
        private void getProductsAsyc(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int currentCategoryId = -1;
            IList<ProductInfo> dtMobileProducts = new List<ProductInfo>();
            try
            {
                int categoryId = Convert.ToInt32(context.Request["CategoryId"]);
                

                Hidistro.Core.HiCache.Remove("DataCache-CategoriesRange");//清除分类缓存
                //获取手机端所有商品的分类
                DataTable dtMobileCagegories = CategoryBrowser.GetCategoriesByRange(2);
                if (categoryId == 0)
                {
                    if (dtMobileCagegories.Rows.Count > 0)
                    {
                        //获取手机端商品的第一个分类id
                        int FirstCategoryID = Convert.ToInt32(dtMobileCagegories.Rows[0]["categoryId"]);
                        dtMobileProducts = ProductBrowser.GetProductList(FirstCategoryID);
                        currentCategoryId = FirstCategoryID;
                    }
                }
                else
                {
                    dtMobileProducts = ProductBrowser.GetProductList(categoryId);
                    currentCategoryId = categoryId;
                }
                CategoryInfo currentCategory = CatalogHelper.GetCategory(currentCategoryId);
                string backHtml = "";
                backHtml += string.Format("<div role='categoryProducts' CategoryId='{0}'>", currentCategoryId);
                foreach (ProductInfo info in dtMobileProducts)
                {
                    backHtml += string.Format("<div class='goods' role='productItem'  ProductId='{0}' ProductName='{1}' BuyPrice='{2}' SkuCounts='{3}' Quantity='{4}' Stock='{5}' SkuId='{6}'>", info.ProductId, info.ProductName, info.DefaultSku.SalePrice, info.Skus.Count, 0, info.Stock, info.SkuId);
                    backHtml += " <div id='slides'><div class='swiper-container productImg'><div class='swiper-wrapper'>";
                    //循环增加商品轮播图
                    string locationUrl = "javascript:;";
                    SlideImage[] imageArray = new SlideImage[] { new SlideImage(info.ImageUrl1, locationUrl), new SlideImage(info.ImageUrl2, locationUrl), new SlideImage(info.ImageUrl3, locationUrl), new SlideImage(info.ImageUrl4, locationUrl), new SlideImage(info.ImageUrl5, locationUrl) };
                    foreach (SlideImage image in imageArray)
                    {
                        if (!string.IsNullOrWhiteSpace(image.ImageUrl))
                            backHtml += "<div class='swiper-slide'><img src='" + image.ImageUrl + "' /></div>";
                    }
                    backHtml += "</div></div></div><div class='info'>";
                    backHtml += "<span class='title'>" + info.ProductName + "</span>";
                    backHtml += "<p class='price'>￥" + info.DefaultSku.SalePrice.ToString("F2") + "<i class='fa fa-plus-circle'></i></p>";
                    backHtml += "</div></div>";
                }
                backHtml += "</div>";
                //<li id='product_<%# Eval('ProductId') %>' ProductId='<%#Eval('ProductId') %>' ProductName='<%# Eval('ProductName') %>' BuyPrice='<%# Eval('SalePrice') %>' SkuCounts='<%# Eval('skuCounts') %>' Quantity='<%# Eval('Quantity') %>' Stock='<%# Eval('stock') %>' SkuId='<%# Eval('SkuId') %>'>
                //<div id='slides'><div class='swiper-container productImg'><div class='swiper-wrapper'>
                //            <hi:vshoptemplatedrepeater id='rptProductImages' templatefile='/Tags/skin-Common_QuickOrderSlide.ascx' runat='server' />
                //        </div></div></div>
                //<div class='info'>
                //    <span class='title'><%# Eval('ProductName') %></span>
                //    <p class='price'>￥<%# Eval('SalePrice','{0:F0}') %><i class='fa fa-plus-circle'></i></p>
                //</div></li>

                //System.Threading.Thread.Sleep(3000);
                context.Response.Write("{\"success\":true,\"backHtml\":\"" + backHtml + "\",\"categoryBannerImgUrl\":\"" + currentCategory.MetaKeywords + "\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\":true,\"errMsg\":\"" + ex.Message + "\"}");
            }
            finally
            {
                dtMobileProducts = null;
            }
            
        }


        /// <summary>
        /// 动态获取规格选择器
        /// </summary>
        private void GetQuickOrderSKUSelector(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = Convert.ToInt32(context.Request["ProductId"]);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), productId);
            DataTable skus = ProductBrowser.GetSkus(productId);
            StringBuilder builder = new StringBuilder();

            IList<string> list = new List<string>();
            builder.Append("<ul>");
            builder.Append("<li><div class='imgbox' style='background:url(" + product.ThumbnailUrl180 + ") no-repeat center center;'></div><div class='infobox'><p>" + product.ProductName + "</p><p role='skuPrice'>￥" + product.DefaultSku.SalePrice.ToString("F2") + "</p></div><a class='close' ><i class='fa fa-times-circle'></i></a></li>");
            if ((skus != null) && (skus.Rows.Count > 0))
            {
                foreach (DataRow row in skus.Rows)
                {
                    if (!list.Contains((string)row["AttributeName"]))
                    {
                        list.Add((string)row["AttributeName"]);
                        builder.Append("<li>");
                        builder.AppendFormat("<div class='spec_title'>{0}</div><input type='hidden' name='skuCountname' AttributeName='{0}' id='skuContent_{1}' />", row["AttributeName"], row["AttributeId"]);
                        builder.AppendFormat("<div id='skuRow_{0}' class='spec-kind'>", row["AttributeId"]);
                        
                        IList<string> list2 = new List<string>();
                        foreach (DataRow row2 in skus.Rows)
                        {
                            if ((string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0) && !list2.Contains((string)row2["ValueStr"]))
                            {
                                string str = string.Concat(new object[] { "skuValueId_", row["AttributeId"], "_", row2["ValueId"] });
                                list2.Add((string)row2["ValueStr"]);
                                builder.AppendFormat("<div id='{0}' role='skuBtn' AttributeId='{1}' ValueId='{2}'>{3}</div>", new object[] { str, row["AttributeId"], row2["ValueId"], (row2["ImageUrl"].ToString() != "") ? ("<img src='" + row2["ImageUrl"] + "' width='50px' height='35px'></img>") : row2["ValueStr"] });
                            }
                        }
                        builder.Append("</div>");
                        builder.Append("</li>");
                    }
                }
            }
            else
            {
                
            }
            builder.Append("<li><div class='spec_title'>数量</div><div class='count'><div><span role='minus'>-</span><input type='text' disabled='true' role='orderQuantity' value='0' /><span role='add'>+</span></div></div></li>");
            builder.Append("<li><div class='shopping_cart'><a role='addToCart'>加入购物车</a><a role='buy'>立即购买</a></div></li>");
            builder.Append("</ul>");
            string backAttrs = string.Format("\"success\":true,\"selector\":\"{0}\"", builder);
            context.Response.Write("{" + backAttrs + "}");

        }


        /// <summary>
        /// 动态获取规格选择器(不带两个按钮的)
        /// </summary>
        private void GetQuickOrderSKUSelectorWithoutBtn(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = Convert.ToInt32(context.Request["ProductId"]);
            ProductInfo product = ProductBrowser.GetProduct(MemberProcessor.GetCurrentMember(), productId);
            DataTable skus = ProductBrowser.GetSkus(productId);
            StringBuilder builder = new StringBuilder();

            IList<string> list = new List<string>();
            builder.Append("<ul>");
            builder.Append("<li><div class='imgbox' style='background:url(" + product.ThumbnailUrl180 + ") no-repeat center center;'></div><div class='infobox'><p  role='productName'>" + product.ProductName + "</p><p role='skuPrice'>￥" + product.DefaultSku.SalePrice.ToString("F0") + "</p></div><a class='close' OnClick='quickHide()'><i class='fa fa-times-circle'></i></a></li>");
            if ((skus != null) && (skus.Rows.Count > 0))
            {
                foreach (DataRow row in skus.Rows)
                {
                    if (!list.Contains((string)row["AttributeName"]))
                    {
                        list.Add((string)row["AttributeName"]);
                        builder.Append("<li>");
                        builder.AppendFormat("<div class='spec_title'>{0}</div><input type='hidden' name='skuCountname' AttributeName='{0}' id='skuContent_{1}' />", row["AttributeName"], row["AttributeId"]);
                        builder.AppendFormat("<div id='skuRow_{0}' class='spec-kind'>", row["AttributeId"]);

                        IList<string> list2 = new List<string>();
                        foreach (DataRow row2 in skus.Rows)
                        {
                            if ((string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0) && !list2.Contains((string)row2["ValueStr"]))
                            {
                                string str = string.Concat(new object[] { "skuValueId_", row["AttributeId"], "_", row2["ValueId"] });
                                list2.Add((string)row2["ValueStr"]);
                                builder.AppendFormat("<div id='{0}' role='skuBtn' AttributeId='{1}' ValueId='{2}'>{3}</div>", new object[] { str, row["AttributeId"], row2["ValueId"], (row2["ImageUrl"].ToString() != "") ? ("<img src='" + row2["ImageUrl"] + "' width='50px' height='35px'></img>") : row2["ValueStr"] });
                            }
                        }
                        builder.Append("</div>");
                        builder.Append("</li>");
                    }
                }
            }
            else
            {

            }
            builder.Append("<li><div class='count'><div><span role='minus'>-</span><input type='text' disabled='true' role='orderQuantity' value='0' /><span role='add'>+</span></div></div></li>");
            builder.Append("</ul>");
            string backAttrs = string.Format("\"success\":true,\"selector\":\"{0}\"", builder);
            context.Response.Write("{" + backAttrs + "}");

        }

        /// <summary>
        /// 获取购物车商品数量
        /// </summary>
        /// <param name="context"></param>
        private void GetShoppingCartGoodNum(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"success\":true,\"num\":\"" + ShoppingCartProcessor.GetShoppingCartNum() + "\"}");
        }

        /// <summary>
        /// 获取购物车商品总价
        /// </summary>
        /// <param name="context"></param>
        private void GetShoppingCartTotal(System.Web.HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"success\":true,\"totalprice\":\"" + ShoppingCartProcessor.GetShoppingCartTotal() + "\"}");
        }

    }
}
