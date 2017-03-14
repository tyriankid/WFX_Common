<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!--循环块-->
<a >
          <div class="well member-orders-nav">
              <div class="member-orders-content">
                  <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl160" />
                  <div class="info">
                      <div class="name bcolor"><%# Eval("ProductName") %></div>
                      <p class="yj price">￥<b><%# Eval("CountDownPrice", "{0:F2}") %></b><del>￥<%# Eval("SalePrice", "{0:F2}") %></del></p>
                      <p class="num">
                          限购：<em><%#Eval("MaxCount") %></em>件
                      </p>
                  </div>
                  <div class="member-right-box">
                  <a class="qianggou" href="<%# "CountDownProductDetail.aspx?countDownId=" + Eval("CountDownId") %>">马上抢</a>
                  <span class="num">限购数量：<i><%# Eval("MaxCount") == DBNull.Value ? 0 : Eval("MaxCount")%></i></span>
                  </div>
              </div>
              <input type="hidden" name="time" text="<%#Eval("EndDate")%>"/>
              <div class="time-box">还剩：<%#Hidistro.UI.SaleSystem.CodeBehind.VCountDownProductList.FormatDate(Convert.ToDateTime(Eval("EndDate"))) %></div>
          </div>
</a>
        <!--循环块-->
