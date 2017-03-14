namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class DeliveryMemberInfo
    {
        public DeliveryMemberInfo()
        {
            this.AddTime = DateTime.Now;
        }
        public virtual string UserName { get; set; }

        public virtual int DeliveryUserId { get; set; }

        public virtual string Phone { get; set; }

        public virtual int DeliveryState { get; set; }

        public virtual int StoreId { get; set; }

        public virtual int Sex { get; set; }

        public virtual int State { get; set; }

        public virtual DateTime AddTime { get; set; }

    }
}

