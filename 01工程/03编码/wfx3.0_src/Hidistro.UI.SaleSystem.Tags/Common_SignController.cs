namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    /*
     须引用common里面的sign.css
     */
    public class Common_SignController : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            string mainArea = string.Empty;
            //获取当前月份的第一天和最后一天
            DateTime now = DateTime.Now;
            int firstDay = new DateTime(now.Year, now.Month, 1).Day;
            int endDay = (new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1)).Day;
            //得到月份第一天的星期数
            DayOfWeek firstDayOfWeek = new DateTime(now.Year, now.Month, 1).DayOfWeek;
            //获取当前用户当月的签到信息
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DataTable signDt = PromoteHelper.GetUserSignInfo(currentMember.UserId);
            //获取当前用户签到的所有dayOfMonth
            IList<int> signedDay =new List<int>();
            foreach(DataRow signInfo in signDt.Rows)
            {
                signedDay.Add(((DateTime)signInfo["SignTime"]).Day);
            }
             

            //拼接控件主体html
            mainArea = string.Format(@"
		    <div class='qian_month'>{0}</div>
		    <div class='qian_tablebox'>
                <table class='qian_table'>
             	        <tr>
					        <th>日</th>
					        <th>一</th>
					        <th>二</th>
					        <th>三</th>
					        <th>四</th>
					        <th>五</th>
					        <th>六</th>
				        </tr>
             ",now.Year+"年"+now.Month+"月");
            //总行数
            int weekCount = 5;
            //如果当前月份最后一天是28号,并且第一天是星期日,那么当月只有4个星期,只有四行.
            if (endDay == 28 && firstDayOfWeek == DayOfWeek.Monday)
                weekCount = 4;
            for (int all = 0; all < weekCount; all++)
            {
                mainArea += "<tr>";
                //第一行的特殊处理
                if (all == 0)
                {
                    //第一天前的空格数循环
                    for (int i = 0; i < (int)firstDayOfWeek; i++)
                    {
                        mainArea += "<td></td>";
                    }
                    for (int o = 0; o < 7 - (int)firstDayOfWeek; o++)
                    {
                        string isSigned = "";
                        if (firstDay < now.Day)//漏签和已签的样式动态赋值
                        {
                            isSigned = signedDay.Contains(firstDay) ? "class='qianed'" : "class='qianlou'";
                        }
                        else if (firstDay == now.Day)
                        {
                            isSigned = signedDay.Contains(firstDay) ? "class='qianed'" : "";
                        }
                        mainArea += string.Format("<td><span dayVal='{0}' {1}>{0}</span></td>", firstDay, isSigned);
                        firstDay++;
                    }
                }
                //剩余的行
                else
                {
                    for (int j = 0; j < 7 ; j++)
                    {
                        if (firstDay <= endDay)
                        {
                            string isSigned = "";
                            if (firstDay < now.Day)//漏签和已签的样式动态赋值
                            {
                                isSigned = signedDay.Contains(firstDay) ? "class='qianed'" : "class='qianlou'";
                            }
                            else if (firstDay == now.Day)
                            {
                                isSigned = signedDay.Contains(firstDay) ? "class='qianed'" : "";
                            }
                            mainArea += string.Format("<td><span dayVal='{0}' {1}>{0}</span></td>", firstDay, isSigned);
                            firstDay++;
                        }
                        else
                        {
                            mainArea += "<td></td>";
                        }
                    }
                }
                mainArea += "</tr>";
            }
            mainArea += "</table></div>";
            mainArea += string.Format("<div class='qian_jilu'>已结连续签到<b role='currentContinuitySignCounts'>{0}</b>天</div>", PromoteHelper.GetContinuitySignCounts(currentMember.UserId));

            writer.Write(mainArea);
        }

    }
}

