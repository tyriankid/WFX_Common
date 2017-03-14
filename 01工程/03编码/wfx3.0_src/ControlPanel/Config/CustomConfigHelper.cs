namespace Hidistro.ControlPanel.Config
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Function;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Caching;


    public class CustomConfigHelper
    {
        private static CustomConfigHelper _instance;
        public static CustomConfigHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CustomConfigHelper();
                    string pathCustomConfig = HttpContext.Current.Server.MapPath("~/config/CustomConfig.xml");
                    if (File.Exists(pathCustomConfig))
                    {
                        DataSet dsCustomConfig = new DataSet();
                        dsCustomConfig.ReadXml(pathCustomConfig);

                        //通用处理
                        if (dsCustomConfig.Tables.Contains("default"))
                        {
                            //是否开启快速提现,默认开启
                            if (dsCustomConfig.Tables["default"].Columns.Contains("isQuickGetCashOn"))
                                _instance.isQuickGetCashOn = dsCustomConfig.Tables["default"].Rows[0]["isQuickGetCashOn"].ToBool();
                            //是否开启门店配送功能
                            if (dsCustomConfig.Tables["default"].Columns.Contains("AutoShipping"))
                                _instance._autoShipping = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["AutoShipping"].ToString());
                            //是否开启匿名点餐功能
                            if (dsCustomConfig.Tables["default"].Columns.Contains("AnonymousOrder"))
                                _instance._anonymousOrder = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["AnonymousOrder"].ToString());
                            //是否开启后台采购上的登录过滤功能
                            if (dsCustomConfig.Tables["default"].Columns.Contains("IsBuyerNeedsToBeActive"))
                                _instance._isBuyerNeedsToBeActive = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["IsBuyerNeedsToBeActive"].ToString());
                            //商家名称
                            if (dsCustomConfig.Tables["default"].Columns.Contains("BusinessName"))
                                _instance._businessName = dsCustomConfig.Tables["default"].Rows[0]["BusinessName"].ToString();
                            //消费者选择服务门店功能
                            if (dsCustomConfig.Tables["default"].Columns.Contains("SelectServerAgent"))
                                _instance._selectServerAgent = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["SelectServerAgent"].ToString());
                            //订单生成时,需要按当天订单数量排序的位数,不能大于7位,默认为0
                            if (dsCustomConfig.Tables["default"].Columns.Contains("OrderIdSortNumCount"))
                                _instance._orderIdSortNumCount = int.Parse(dsCustomConfig.Tables["default"].Rows[0]["OrderIdSortNumCount"].ToString());
                            //是否开启分类
                            if (dsCustomConfig.Tables["default"].Columns.Contains("ClassSkip"))
                                _instance._classSkip = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["ClassSkip"].ToString());
                            //是否三作咖啡
                            if (dsCustomConfig.Tables["default"].Columns.Contains("IsSanZuo"))
                                _instance._isSanZuo = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["IsSanZuo"].ToString());
                            //是否水泽堂
                            if (dsCustomConfig.Tables["default"].Columns.Contains("IsSZT"))
                                _instance._SztShow = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["IsSZT"].ToString());
                            //是否51跟单
                            if (dsCustomConfig.Tables["default"].Columns.Contains("Is51gendan"))
                                _instance._is51gendan = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["Is51gendan"].ToString());
                            //是否pro辣
                            if (dsCustomConfig.Tables["default"].Columns.Contains("IsProLa"))
                                _instance._isProLa = bool.Parse(dsCustomConfig.Tables["default"].Rows[0]["IsProLa"].ToString());
                            
                        }

                        //定制处理
                        switch (dsCustomConfig.DataSetName)
                        {
                            case "adele":
                                DataRow drDistributorType = dsCustomConfig.Tables["DistributorType"].Rows[0];
                                if (dsCustomConfig.Tables["DistributorType"].Columns.Contains("name"))
                                    _instance.distributorType_Name = drDistributorType["name"].ToString();
                                if (dsCustomConfig.Tables["DistributorType"].Columns.Contains("showfootmanage"))
                                    _instance.distributorType_Showfootmanage = drDistributorType["showfootmanage"].ToString();
                                if (dsCustomConfig.Tables["DistributorType"].Columns.Contains("showfootapply"))
                                    _instance.distributorType_Showfootapply = drDistributorType["showfootapply"].ToString();
                                if (dsCustomConfig.Tables["DistributorType"].Columns.Contains("showbutton"))
                                    _instance.distributorType_Showbutton = drDistributorType["showbutton"].ToString();
                                break;
                            case "jingff"://特殊情况:是否屏蔽退换货功能
                                DataRow isReturnChangeGoods = dsCustomConfig.Tables["isReturnChangeGoodsOn"].Rows[0];
                                if (dsCustomConfig.Tables["isReturnChangeGoodsOn"].Columns.Contains("value"))
                                    _instance.isReturnChangeGoodsOn = isReturnChangeGoods["value"].ToString();
                                break;
                            case "diman":
                                DataRow dtDiman = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("showbutton"))
                                    _instance.distributorType_Showbutton = dtDiman["showbutton"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("myCashText"))
                                    _instance.myCashText = dtDiman["myCashText"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("myDistributorText"))
                                    _instance.myDistributorText = dtDiman["myDistributorText"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("isLogoOn"))
                                    _instance.isLogoOn = dtDiman["isLogoOn"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("isDistributorDescriptionOn"))
                                    _instance.isDistributorDescriptionOn = dtDiman["isDistributorDescriptionOn"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("customInputs"))
                                    _instance.customInputs = dtDiman["customInputs"].ToString();
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("applicationDescriptionTitle"))
                                    _instance.applicationDescriptionTitle = dtDiman["applicationDescriptionTitle"].ToString();
                                break;
                            case "aifr"://是否开启天使全利返佣
                                DataRow drAifr = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("isAll"))
                                    _instance.isAgentRebateAll = bool.Parse(drAifr["isAll"].ToString());
                                break;
                            case "Qipinhui"://齐品汇的天使特殊返佣规则和禁止返佣的四个openid
                                DataRow dtQipn = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("isQipinhui"))
                                    _instance.isQipinhui = bool.Parse(dtQipn["isQipinhui"].ToString());
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("noCommOpenids"))
                                    _instance.noCommOpenids = dtQipn["noCommOpenids"].ToString();
                                break;
                            case "JXJJ"://玖信健佳
                                DataRow dtJXJJ = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("regionalFunction"))
                                    _instance.regionalFunction = bool.Parse(dtJXJJ["regionalFunction"].ToString());
                                break;

                            case"SZT"://水泽堂
                                DataRow dtSZT = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("SztShow"))
                                    _instance._SztShow = bool.Parse(dtSZT["SztShow"].ToString());
                                break;

                            case "JXT"://锦欣铜
                                DataRow dtJXT = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("DistributorOrderJump"))
                                    _instance._distributorOrderJump = bool.Parse(dtJXT["DistributorOrderJump"].ToString());
                                break;
                            case "JZ"://轿子洗衣
                                DataRow dtJZ = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("CouponRecharge"))
                                    _instance._couponRecharge = bool.Parse(dtJZ["CouponRecharge"].ToString());
                                break;
                            case "JZT"://九州通
                                DataRow KLMG = dsCustomConfig.Tables["Configs"].Rows[0];
                                if (dsCustomConfig.Tables["Configs"].Columns.Contains("BrandShow"))
                                    _instance._brandShow = bool.Parse(KLMG["BrandShow"].ToString());
                                break;
                        }
                    }

                }
                return _instance;
            }
        }
        /// <summary>
        /// 水泽堂
        /// </summary>
        private bool _SztShow = false;
        public bool SztShow
        {
            get { return _SztShow; }
        }

        private bool _brandShow = false;
        /// <summary>
        /// 卡拉萌购商品品牌展示
        /// </summary>
        public bool BrandShow
        {
            get { return _brandShow; }
        }
        private string distributorType_Name = "";
        public string DistributorType_Name
        {
            get { return distributorType_Name; }
        }

        private string distributorType_Showfootmanage = "店铺管理";
        public string DistributorType_Showfootmanage
        {
            get { return distributorType_Showfootmanage; }
        }

        private string distributorType_Showfootapply = "申请开店";
        public string DistributorType_Showfootapply
        {
            get { return distributorType_Showfootapply; }
        }
        private string distributorType_Showbutton = "马上成为分销商";
        public string DistributorType_Showbutton
        {
            get { return distributorType_Showbutton; }
        }

        private string isReturnChangeGoodsOn = "true";
        /// <summary>
        /// 获取 是否打开退换货功能，默认为true
        /// </summary>
        public string IsReturnChangeGoodsOn
        {
            get { return isReturnChangeGoodsOn; }
        }

        private string myCashText = "我的佣金";
        public string MyCashText
        {
            get { return myCashText; }
        }

        private string myDistributorText = "我的下属";
        public string MyDistributorText
        {
            get { return myDistributorText; }
        }

        private string isLogoOn = "true";
        public string IsLogoOn
        {
            get { return isLogoOn; }
        }

        private string isDistributorDescriptionOn = "true";
        public string IsDistributorDescriptionOn
        {
            get { return isDistributorDescriptionOn; }
        }

        private string customInputs = "";
        public string CustomInputs
        {
            get { return customInputs; }
        }


        private bool isAgentRebateAll = false;
        /// <summary>
        /// 获取 是否开启天使全利返佣,默认为false
        /// </summary>
        public bool IsAgentRebateAll
        {
            get { return isAgentRebateAll; }
        }

        private string applicationDescriptionTitle = "分销商描述";
        /// <summary>
        /// 成为分销商页面的title
        /// </summary>
        public string ApplicationDescriptionTitle
        {
            get { return applicationDescriptionTitle; }
        }

        private bool isQipinhui = false;
        /// <summary>
        /// 获取 是否开启齐品汇的代理商特殊返佣规则,默认为false
        /// </summary>
        public bool IsQipinhui
        {
            get { return isQipinhui; }
        }

        private string noCommOpenids = "";
        /// <summary>
        /// 不参加返佣的天使的openid
        /// </summary>
        public string NoCommOpenids
        {
            get { return noCommOpenids; }
        }

        private bool regionalFunction = false;
        /// <summary>
        /// 区域功能是否展示(截止20151124,区域功能仅对玖信健佳生效)
        /// </summary>
        public bool RegionalFunction
        {
            get { return regionalFunction; }
        }

        private bool isQuickGetCashOn = true;
        /// <summary>
        /// 是否打开快捷支付
        /// </summary>
        public bool IsQuickGetCashOn
        {
            get { return isQuickGetCashOn; }
        }

        private bool _autoShipping = false;
        /// <summary>
        /// 是否自动打街道选择功能(提交订单时除了省市区更要精确到街道配送范围,同时分销商后台登录订单过滤发货功能,和分销商配置功能,和分类页面快速提交订单功能的同时控制)
        /// 截止2015-12-25仅对咖啡shop18210打开
        /// </summary>
        public bool AutoShipping
        {
            get { return _autoShipping; }
        }

        private bool _anonymousOrder = false;
        /// <summary>
        /// 匿名点餐功能,当pc端的用户登录直接点餐页面时(截止20151229目前仅有ProductSearchbuy页面)无需登录便可以直接通过选择门店进行点餐.
        /// 此功能目前和AutoShipping功能同时开启,否则产生的订单都是总店的
        /// </summary>
        public bool AnonymousOrder
        {
            get { return _anonymousOrder; }
        }

        private bool _distributorOrderJump = false;
        /// <summary>
        /// 分销商在自己店铺购买的订单挂在上一级分销商的店铺下,若没有上一级分销商,则为总店 (Order主表的ReferralUserId字段的处理)
        /// </summary>
        public bool DistributorOrderJump
        {
            get { return _distributorOrderJump; }
        }

        private bool _couponRecharge = false;
        /// <summary>
        /// 是否开启充值赠送优惠券
        /// </summary>
        public bool CouponRecharge
        {
            get { return _couponRecharge; }
        }

        private bool _isBuyerNeedsToBeActive = false;
        /// <summary>
        /// 是否需要激活member(采购商)才可以登录系统(用于爽爽挝啡后台采购系统的登陆过滤)
        /// </summary>
        public bool IsBuyerNeedsToBeActive
        {
            get { return _isBuyerNeedsToBeActive; }
        }

        private string _businessName = "";
        /// <summary>
        /// 商家名称(例如 三作咖啡,爽爽挝啡,迪蔓国际)(目前用于门店匹配的要求开店标语)
        /// </summary>
        public string BusinessName
        {
            get { return _businessName; }
        }

        private bool _selectServerAgent = false;
        /// <summary>
        /// 消费者购买时可以选择服务的门店,如果选了门店订单归属该未选择的门店,仅服务门店拿佣金
        /// 如果服务门店选择无,则按正常三级返佣(三级之外不参与返佣,如无限代理)
        /// </summary>
        public bool SelectServerAgent
        {
            get { return _selectServerAgent; }
        }

        private int _orderIdSortNumCount =0;
        /// <summary>
        /// 订单生成时,需要按当天订单数量排序的位数,不能大于7位,默认为0
        /// </summary>
        public int OrderIdSortNumCount
        {
            get { return _orderIdSortNumCount; }
        }
        /// <summary>
        /// 分类/所有跳转
        /// </summary>
        private bool _classSkip =true;
        public bool ClassSkip
        {
            get { return _classSkip; }
        }

        /// <summary>
        /// 三作咖啡专用开关(去掉分类按钮,门店匹配页面跳转首页)
        /// </summary>
        private bool _isSanZuo = false;
        public bool IsSanzuo
        {
            get { return _isSanZuo; }
        }
        public string IsOrderLeading()
        {
            string strHeaders = "原始订单编号,商品编号,商品数量,成本价,商品单价,商品描述,订单产生时间,付款时间,发货时间,收货时间,收货人姓名,收货人手机,备注,物流公司,送货地区,详细地址";
            return (strHeaders);
        }
        /// <summary>
        /// 计算代理商订单ID
        /// </summary>
        /// <param name="userId">后台用户Id</param>
        /// <returns>订单ID</returns>
        public string GenerateOrderId(int userId)
        {
            string str = string.Empty;
            System.Random random = new System.Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str += ((char)(48 + (ushort)(num % 10))).ToString();
            }
            str = System.DateTime.Now.ToString("yyyyMMdd") + str;
            if (userId > 0) str=str + "_" + userId.ToString();
            return str;
        }
        /// <summary>
        /// 九州通电子面单打印
        /// </summary>
        private bool _isjiuzhoutong = false;
        public bool IsJiuZhouTong
        {
            get { return _isjiuzhoutong;}
        }
        public string GenerateOrderIdByOriginOrderid(string originId)
        {
            byte[] result = Encoding.Default.GetBytes(originId);    //tbPass为输入密码的文本框  
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return  System.DateTime.Now.ToString("yyyyMMdd") + BitConverter.ToString(output).Replace("-", "").Substring(0,7);
        }

        /// <summary>
        /// 51跟单特殊返佣,分销商在自己店铺购买商品时,三级分佣往上推一层
        /// </summary>
        private bool _is51gendan = false;
        public bool Is51gendan
        {
            get { return _is51gendan; }
        }

        /// <summary>
        /// pro辣特殊需求开关
        /// </summary>
        private bool _isProLa = false;
        public bool IsProLa
        {
            get { return _isProLa; }
        }
    }
}

