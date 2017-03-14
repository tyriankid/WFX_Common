namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    public class RefundPlugins : PluginContainer
    {
        private static volatile RefundPlugins instance = null;
        private static readonly object LockHelper = new object();

        private RefundPlugins()
        {
        }

        public override PluginItem GetPluginItem(string fullName)
        {
            return base.GetPluginItem("RefundRequest", fullName);
        }

        public override PluginItemCollection GetPlugins()
        {
            return base.GetPlugins("RefundRequest");
        }

        public static RefundPlugins Instance()
        {
            if (instance == null)
            {
                lock (LockHelper)
                {
                    if (instance == null)
                    {
                        instance = new RefundPlugins();
                    }
                }
            }
            instance.VerifyIndex();
            return instance;
        }

        protected override string IndexCacheKey
        {
            get
            {
                return "plugin-refund-index";
            }
        }

        protected override string PluginLocalPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/plugins/refund");
            }
        }

        protected override string PluginVirtualPath
        {
            get
            {
                return (Utils.ApplicationPath + "/plugins/refund");
            }
        }

        protected override string TypeCacheKey
        {
            get
            {
                return "plugin-refund-type";
            }
        }
    }
}

