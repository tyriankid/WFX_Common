namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.VShop;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_PrizeNames : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Activity != null)
            {
                StringBuilder builder = new StringBuilder();
                int iCount=0;
                string strText = string.Empty;
                foreach (PrizeSetting setting in this.Activity.PrizeSettingList)
                {
                    int.TryParse(setting.PrizeName, out iCount);
                    if (iCount > 0) strText = "元红包";
                    builder.AppendFormat("<p>{0}：{1} ({2}名)</p>", setting.PrizeLevel, setting.PrizeName + strText, setting.PrizeNum);
                }
                writer.Write(builder.ToString());
            }
        }

        public LotteryActivityInfo Activity { get; set; }
    }
}

