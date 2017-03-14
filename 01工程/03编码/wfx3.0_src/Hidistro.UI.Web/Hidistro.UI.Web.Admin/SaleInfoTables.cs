using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
    public class SaleInfoTables : AdminPage
    {
        protected DropDownList ddlSenders;
        protected Button btnExport;
        protected Button btnSerch;
        protected Button btnProductTotalExport;
        protected Literal litTables;
        protected WebCalendar calendarStartDate;
        protected WebCalendar calendarEndDate;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            ddlSenders.SelectedIndexChanged += new EventHandler(this.ddlSenders_SelectedIndexChanged);
            this.btnExport.Click += new EventHandler(this.orderStatistics);
            this.btnProductTotalExport.Click += new EventHandler(this.btnProductTotalExport_click);
            this.btnSerch.Click += new EventHandler(this.btnSerch_click);
            if (!this.Page.IsPostBack)
            {
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                if (ManagerHelper.GetRole(currentManager.RoleId).RoleName == "超级管理员")
                {
                    DataTable dtSenders = ManagerHelper.GetAllManagers();
                    foreach (DataRow row in dtSenders.Rows)
                    {
                        ListItem item = new ListItem
                        {
                            Text = row["UserName"].ToString() + "：" + row["agentName"].ToString(),
                            Value = row["UserId"].ToString(),
                        };
                        if (!string.IsNullOrEmpty(row["clientUserId"].ToString()) && row["clientUserId"].ToString() != "0")
                            ddlSenders.Items.Add(item);
                    }
                    ddlSenders.SelectedIndex = 0;
                    if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
                    {
                        loadTables("", "", Convert.ToInt32(ddlSenders.SelectedValue));
                    }
                    else {
                        ddlSenders.Visible = false;
                    }
                }
                else
                {
                    ddlSenders.Items.Clear();
                    ListItem item = new ListItem
                    {
                        Text = currentManager.UserName + "：" + currentManager.AgentName,
                        Value = currentManager.UserId .ToString(),
                    };
                    ddlSenders.Items.Add(item);
                    ddlSenders.SelectedIndex = 0;
                    ddlSenders.Visible = false;
                    loadTables("","",currentManager.UserId);
                }
            }
        }

        private void btnSerch_click(object sender, EventArgs e)
        {
            string starttime = "";
            string endtime = "";
            if (calendarStartDate.SelectedDate.HasValue)
            {
                starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }
            if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
            {
                loadTables(starttime, endtime, Convert.ToInt32(ddlSenders.SelectedValue));
            }           
        }
      
        void ddlSenders_SelectedIndexChanged(object sender, EventArgs e)
        {
            string starttime="";
            string endtime="";
            if (calendarStartDate.SelectedDate.HasValue)
            {
                starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }        
            loadTables(starttime, endtime,Convert.ToInt32(ddlSenders.SelectedValue));
        }

        private void loadTables(string StartTime="",string EndTime="",int sender=0)
        {
            DataSet ds = SalesHelper.GetSaleInfoTables(StartTime, EndTime,sender);
            /*
        <div class='back back2'>
            <div class='backtt clear'><h1>3</h1><p>今日流量</p></div>
            <ul class='three_row clear'>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
                <li><a href=''><span>代付款</span><b>4</b></a></li>
            </ul>
        </div>
             */
            string htmls = string.Empty;
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].Rows.Count == 0) continue; //跳出本次循环
                string tableName = string.Empty;
                switch (i)
                {
                    case 0:
                        tableName = "总计";
                        break;
                    case 1:
                        tableName = "PC端统计";
                        break;
                    case 2:
                        tableName = "微信端统计";
                        break;
                    case 3:
                        tableName = "今日总计";
                        break;
                    case 4:
                        tableName = "今日PC端统计";
                        break;
                    case 5:
                        tableName = "今日微信端统计";
                        break;
                    case 6:
                        tableName = "pc端微信扫码支付总计";
                        break;
                    case 7:
                        tableName = "今日pc端微信扫码支付统计";
                        break;                
                }
                htmls += "<div class='back back" + (i + 1) + "'>";
                htmls += "<div class='backtt clear'><h1>￥" + ((decimal)ds.Tables[i].Rows[0]["OrderTotal"]).ToString("F2") + "</h1><p>" + tableName + "</p></div>";
                htmls += "<ul class='three_row clear'>";

                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "账号", ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName);
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "订单总数", ds.Tables[i].Rows[0]["orderCount"]);
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "订单总额", ((decimal)ds.Tables[i].Rows[0]["OrderTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "优惠券总额", ((decimal)ds.Tables[i].Rows[0]["CouponTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "优惠总额", ((decimal)ds.Tables[i].Rows[0]["DiscountTotal"]).ToString("F2"));
                htmls += string.Format("<li><a href='javascript:void(0)'><span>{0}</span><b>{1}</b></a></li>", "商品总件数", ds.Tables[i].Rows[0]["productTotal"]);
                htmls += "</ul></div>";
            }
            litTables.Text = htmls;
        }

        /// <summary>
        /// 商品销售数量导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnProductTotalExport_click(object sender, EventArgs e)
        {
            string starttime = "";
            string endtime = "";
            int send = 0;
            string  modename ="";
            //获取时间的值
            if(calendarStartDate.SelectedDate.HasValue)
            {
                 starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }           
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }
            if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
            {
                send = Convert.ToInt32(ddlSenders.SelectedValue);
                ManagerInfo manager = ManagerHelper.GetManager(send);
                modename = ManagerHelper.getPcOrderStorenameByClientuserid(manager.ClientUserId);
            }
            //获取数据源
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            DataTable dt = new DataTable();
            DataTable data = SalesHelper.GetProductQuantity(starttime, endtime, send, modename);
            //商品数量(购买的数量减去送的数量)
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (!Convert.IsDBNull(data.Rows[i]["GiveQuantity"]))
                {
                    int quantity = Convert.ToInt32(data.Rows[i]["Quantity"]) - Convert.ToInt32(data.Rows[i]["GiveQuantity"]);
                    data.Rows[i]["Quantity"] = quantity;
                }
            }
            fields.Add("ProductName");
            list2.Add("商品姓名");
            fields.Add("Quantity");
            list2.Add("商品总数量");           
            //表格循环
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string str in list2)
            {
                builder.Append(str + ",");
                if (str == list2[list2.Count - 1])
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                    builder.Append("\r\n");
                }
            }
            foreach (DataRow row in data.Rows)
            {
                foreach (string str2 in fields)
                {
                    if (row.Table.Rows[0]["" + str2 + ""] == null || row.Table.Rows[0]["" + str2 + ""].ToString() == "")
                    {
                        switch (str2)
                        {
                            case "ProductName":
                                builder.Append(row[str2] = 0).Append(",");
                                break;
                            case "Quantity":
                                builder.Append(row[str2] = 0).Append(",");
                                break;
                        }
                    }
                    else
                    {
                        builder.Append(row[str2]).Append(",");
                    }
                    if (str2 == fields[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
            }
            //时间格式化
            string sj ="";
            string sj2 = "";
            if (calendarStartDate.SelectedDate.HasValue && calendarEndDate.SelectedDate.HasValue)
            {
                DateTime dte = Convert.ToDateTime(starttime);
                DateTime dte2 = Convert.ToDateTime(endtime);
                sj = dte.ToString("yyyy-MM-dd").Replace("-", "/");
                sj2 = dte2.ToString("yyyy-MM-dd").Replace("-", "/");
            }
            //导出到表格
            string FileName = "";
            if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
            {
                 FileName = "" + ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName + "";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + "" + sj + "" + sj2 + ".csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }
        /// <summary>
        /// 导出Excel表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void orderStatistics(object sender, EventArgs e)
        {
            string starttime = "";
            string endtime = "";
            int managerId = 0;
            if (calendarStartDate.SelectedDate.HasValue)
            {
                starttime = this.calendarStartDate.SelectedDate.Value.ToString();
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                endtime = this.calendarEndDate.SelectedDate.Value.ToString();
            }
            if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
            { 
                managerId =Convert.ToInt32(ddlSenders.SelectedValue);
            }
            #region 循环条件
            System.Collections.Generic.IList<string> fields = new System.Collections.Generic.List<string>();
            System.Collections.Generic.IList<string> list2 = new System.Collections.Generic.List<string>();
            DataSet orderInfo = SalesHelper.GetSaleInfoTables(starttime, endtime, managerId);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            string tableName = string.Empty;
            fields.Add("tableName"); list2.Add("");
            fields.Add("orderCount"); list2.Add("订单总数");
            fields.Add("OrderTotal"); list2.Add("订单总额");
            fields.Add("CouponTotal"); list2.Add("优惠券总额");
            fields.Add("DiscountTotal"); list2.Add("优惠总额");
            fields.Add("productTotal"); list2.Add("商品总件数");
            #endregion
            for (int i = 0; i< orderInfo.Tables.Count; i++)
            {
                if (orderInfo.Tables[i].Rows.Count == 0) continue;
                switch (i)
                {
                   #region tableName循环
                    case 0:
                        tableName = "总计";
                        break;
                    case 1:
                        tableName = "PC端统计";
                        break;
                    case 2: 
                        tableName = "微信端统计";
                        break;
                    case 3:
                        tableName = "今日总计";
                        break;
                    case 4:
                        tableName = "今日PC端统计";
                        break;
                    case 5:
                        tableName = "今日微信端统计";
                        break;
                    case 6:
                        tableName = "pc端微信扫码支付总计";
                        break;
                    case 7:
                        tableName = "今日pc端微信扫码支付统计";
                        break;
                    #endregion
                }
            //表格循环
            DataTable dataTable = orderInfo.Tables[i];
            //新增表头
            dataTable.Columns.Add("tableName");
            dataTable.Rows[0]["tableName"] = tableName;
            int rowNumber = dataTable.Rows.Count;
            if (rowNumber == 0)
               {
                 this.ShowMsg("没有任何数据可以导入到Excel文件！", false);
                 return;
               }
             foreach (string str in list2)
                {
                   builder.Append(str + ",");
                   if (str == list2[list2.Count - 1])
                      {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                       }
                }
             #region 表格带值循环,将内容赋给builder
             foreach (DataRow row in dataTable.Rows)
             {
                 foreach (string str2 in fields)
                 {
                     if (row.Table.Rows[0][""+ str2 + ""] == null||row.Table.Rows[0]["" + str2 + ""].ToString() =="")
                     {
                         switch (str2)
                         {
                             #region 内容非空判断
                             case "orderCount":
                                 builder.Append(row[str2] = 0).Append(",");                             
                                 break;
                             case "OrderTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "CouponTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "DiscountTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             case "productTotal":
                                 builder.Append(row[str2] = 0).Append(",");
                                 break;
                             #endregion
                         }
                     }
                     else
                     {
                         builder.Append(row[str2]).Append(",");
                     }
                     if (str2 == fields[list2.Count - 1])
                     {
                         builder = builder.Remove(builder.Length - 1, 1);
                         builder.Append("\r\n");
                     }
                   }
             }
             #endregion
            }
            #region 通过builder导出Excel"
            string FileName = "";
            if (!string.IsNullOrEmpty(ddlSenders.SelectedValue))
            {
                 FileName = "" + ManagerHelper.GetManager(Convert.ToInt32(ddlSenders.SelectedValue)).UserName + "";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".csv");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
            #endregion
        }
    }
}

