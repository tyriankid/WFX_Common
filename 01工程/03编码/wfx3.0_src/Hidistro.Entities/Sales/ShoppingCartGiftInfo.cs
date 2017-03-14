namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartGiftInfo
    {
        public decimal CostPrice { get; set; }

        public int GiftId { get; set; }

        public string Name { get; set; }

        public int NeedPoint { get; set; }

        public int PromoType { get; set; }

        public int Quantity { get; set; }

        public int SubPointTotal
        {
            get
            {
                if (this.PromoType <= 0)
                {
                    return (this.NeedPoint * this.Quantity);
                }
                return 0;
            }
        }

        public string ThumbnailUrl100 { get; set; }

        public string ThumbnailUrl40 { get; set; }

        public string ThumbnailUrl60 { get; set; }

        public int UserId { get; set; }

        public int Stock { get; set; }
    }
}

