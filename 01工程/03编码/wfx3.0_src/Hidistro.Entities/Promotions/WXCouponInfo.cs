namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;


    [HasSelfValidation]
    public class WXCouponInfo
    {
        public class Card
        {
            public string card_type { get; set; }
            public class GENERAL_COUPON
            {
                public class Base_info
                {
                    public string logo_url { get; set; }

                    public string code_type { get; set; }

                    public string brand_name { get; set; }

                    public string title { get; set; }

                    public string sub_title { get; set; }

                    public string color { get; set; }

                    public string notice { get; set; }

                    public string description { get; set; }

                    public struct Sku
                    {
                        public int quantity { get; set; }
                    }
                    public Sku sku = new Sku();

                    public struct Date_info
                    {
                        public string type { get; set; }
                        public long begin_timestamp { get; set; }
                        public long end_timestamp { get; set; }
                    }
                    public Date_info date_info = new Date_info();

                    public bool use_custom_code { get; set; }

                }
                public Base_info base_info = new Base_info();

                public string default_detail { get; set; }
            }
            public GENERAL_COUPON general_coupon = new GENERAL_COUPON();

            
        }
        public Card card = new Card();
    }
}

