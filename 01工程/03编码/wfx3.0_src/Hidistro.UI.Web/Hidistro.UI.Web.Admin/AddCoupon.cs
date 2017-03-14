using ASPNET.WebControls;
using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class AddCoupon : AdminPage
	{
		protected System.Web.UI.WebControls.Button btnAddCoupons;
		protected WebCalendar calendarEndDate;
		protected WebCalendar calendarStartDate;
		protected System.Web.UI.WebControls.TextBox txtAmount;
		protected System.Web.UI.WebControls.TextBox txtCouponName;
		protected System.Web.UI.WebControls.TextBox txtDiscountValue;
		protected System.Web.UI.WebControls.TextBox txtNeedPoint;
        protected System.Web.UI.WebControls.DropDownList DDLservice;
        protected System.Web.UI.WebControls.DropDownList DDLcategory;
        protected HtmlInputHidden hidIsSanZuo;

		private void btnAddCoupons_Click(object sender, System.EventArgs e)
		{
			string msg = string.Empty;
			decimal? nullable;
			decimal num;
			int num2;
			if (this.ValidateValues(out nullable, out num, out num2))
			{
				if (!this.calendarStartDate.SelectedDate.HasValue)
				{
					this.ShowMsg("请选择开始日期！", false);
				}
				else
				{
					if (!this.calendarEndDate.SelectedDate.HasValue)
					{
						this.ShowMsg("请选择结束日期！", false);
					}
					else
					{
						if (this.calendarStartDate.SelectedDate.Value.CompareTo(this.calendarEndDate.SelectedDate.Value) >= 0)
						{
							this.ShowMsg("开始日期不能晚于结束日期！", false);
						}
						else
						{
                            CouponInfo target = new CouponInfo();
                            if (CustomConfigHelper.Instance.IsSanzuo || (CustomConfigHelper.Instance.AutoShipping && CustomConfigHelper.Instance.AnonymousOrder))
                            {                      
                                target.Name = this.txtCouponName.Text;
								target.ClosingTime = this.calendarEndDate.SelectedDate.Value;
								target.StartTime = this.calendarStartDate.SelectedDate.Value;
                                if (this.DDLservice.SelectedValue != "未选择")
                                {
                                    target.SenderId = Convert.ToInt32(this.DDLservice.SelectedValue);
                                }
                                else {
                                    target.SenderId = 0;
                                }
                                if (this.DDLcategory.SelectedValue != "未选择")
                                {
                                    target.CategoryId = Convert.ToInt32(this.DDLcategory.SelectedValue);
                                }
                                else {
                                    target.CategoryId = 0;
                                }
								target.Amount = nullable;
								target.DiscountValue = num;
                                target.NeedPoint = num2;
                            }
                            else
                            {
                                target.Name = this.txtCouponName.Text;
                                target.ClosingTime = this.calendarEndDate.SelectedDate.Value;
                                target.StartTime = this.calendarStartDate.SelectedDate.Value;
                                target.Amount = nullable;
                                target.DiscountValue = num;
                                target.NeedPoint = num2;
                            }

							ValidationResults results = Validation.Validate<CouponInfo>(target, new string[]
							{
								"ValCoupon"
							});
							if (!results.IsValid)
							{
								using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator = ((System.Collections.Generic.IEnumerable<ValidationResult>)results).GetEnumerator())
								{
									if (enumerator.MoveNext())
									{
										ValidationResult result = enumerator.Current;
										msg += Formatter.FormatErrorMessage(result.Message);
										this.ShowMsg(msg, false);
										return;
									}
								}
							}


							string lotNumber = string.Empty;
                            //创建优惠券
							CouponActionStatus status = CouponHelper.CreateCoupon(target, 0, out lotNumber);

                            //创建微信卡包优惠券
                            //读取配置信息
                            if (false) //是否同时将生成的优惠券创建到到微信卡券内
                            {
                                Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
                                //CouponInfo coupon1 = CouponHelper.GetCoupon(couponId);
                                WXCouponInfo wxCoupon = new WXCouponInfo();
                                wxCoupon.card.card_type = "GENERAL_COUPON";
                                wxCoupon.card.general_coupon.base_info.code_type = "CODE_TYPE_TEXT";
                                //wxCoupon.logo_url = Globals.DomainName + masterSettings.DistributorLogoPic;
                                wxCoupon.card.general_coupon.base_info.logo_url = "http://yihui.ewaywin.com/Storage/data/DistributorLogoPic/20151125173959_6308.jpg";
                                wxCoupon.card.general_coupon.base_info.brand_name = masterSettings.SiteName;
                                wxCoupon.card.general_coupon.base_info.title = "满" + target.Amount.ToString() + "减" + target.DiscountValue.ToString("0.00");
                                wxCoupon.card.general_coupon.base_info.sub_title = this.txtCouponName.Text;
                                wxCoupon.card.general_coupon.base_info.color = "Color100";
                                wxCoupon.card.general_coupon.base_info.notice = "购买商品时使用";
                                wxCoupon.card.general_coupon.base_info.description = target.Description;
                                wxCoupon.card.general_coupon.base_info.sku.quantity = 200;
                                wxCoupon.card.general_coupon.base_info.date_info.type = "DATE_TYPE_FIX_TIME_RANGE";
                                wxCoupon.card.general_coupon.base_info.date_info.begin_timestamp = DateTimeToUnixTimestamp(target.StartTime);
                                wxCoupon.card.general_coupon.base_info.date_info.end_timestamp = DateTimeToUnixTimestamp(target.ClosingTime);
                                wxCoupon.card.general_coupon.base_info.use_custom_code = false;
                                wxCoupon.card.general_coupon.default_detail = target.Name;                        
                                string json = Newtonsoft.Json.JsonConvert.SerializeObject(wxCoupon, Newtonsoft.Json.Formatting.Indented);
                                string access_token = Access_token.GetAccess_token();
                                //string access_token = "12j5h9jB21EEtpBkJKCtG3K10pZFXKAjDuM_CXJmvea6TdU_0YGCEaumhkClH0Wd3sq12901e6hqoA9VvTxemCSNUxtSoCwXSHrZ1Elb_v0CSQcAAAXNL";
                                Hidistro.UI.Web.API.wx.Post("https://api.weixin.qq.com/card/create?access_token=" + access_token, json);
                            }

							if (status != CouponActionStatus.UnknowError)
							{
								CouponActionStatus couponActionStatus = status;
								if (couponActionStatus != CouponActionStatus.DuplicateName)
								{
									if (couponActionStatus != CouponActionStatus.CreateClaimCodeError)
									{
										this.ShowMsg("添加优惠券成功", true);
										this.RestCoupon();
									}
									else
									{
										this.ShowMsg("生成优惠券号码错误", false);
									}
								}
								else
								{
									this.ShowMsg("已经存在相同的优惠券名称", false);
								}
							}
							else
							{
								this.ShowMsg("未知错误", false);
							}
						}
					}
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddCoupons.Click += new System.EventHandler(this.btnAddCoupons_Click);
            if (CustomConfigHelper.Instance.IsSanzuo == true || CustomConfigHelper.Instance.BusinessName == "爽爽挝啡")
            {
                hidIsSanZuo.Value = "1";
                List<CategoryQuery> category = new List<CategoryQuery>();
                category = CouponHelper.GetHishop_Categories();
                DDLcategory.Items.Add("未选择");
                foreach (CategoryQuery ca in category)
                {
                    ListItem item = new ListItem();
                    item.Text = ca.Name;
                    item.Value = ca.CategoryId.ToString();
                    DDLcategory.Items.Add(item);
                }
                List<CategoryQuery> clientuser = new List<CategoryQuery>();
                clientuser = CouponHelper.Getaspnet_ManagersClientUserId();
                DDLservice.Items.Add("未选择");
                foreach (CategoryQuery ct in clientuser)
                {
                    ListItem item = new ListItem();
                    item.Text = ct.UserName;
                    item.Value = ct.ClientUserId.ToString();
                    DDLservice.Items.Add(item);
                }
            }
		}
		private void RestCoupon()
		{
			this.txtCouponName.Text = string.Empty;
			this.txtAmount.Text = string.Empty;
			this.txtDiscountValue.Text = string.Empty;
		}
		private bool ValidateValues(out decimal? amount, out decimal discount, out int needPoint)
		{
			string str = string.Empty;
			amount = new decimal?(0m);
			if (!string.IsNullOrEmpty(this.txtAmount.Text.Trim()))
			{
				decimal num;
				if (decimal.TryParse(this.txtAmount.Text.Trim(), out num))
				{
					amount = new decimal?(num);
				}
				else
				{
					str += Formatter.FormatErrorMessage("满足金额必须为0-1000万之间");
				}
			}
			if (!int.TryParse(this.txtNeedPoint.Text.Trim(), out needPoint))
			{
				str += Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
			}
			if (!decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discount))
			{
				str += Formatter.FormatErrorMessage("可抵扣金额必须在0.01-1000万之间");
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



        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
	}
}
