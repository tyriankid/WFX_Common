<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

            <li>
                <a class="imgbox"><img src=<%# Eval("UserHead")%> /></a>
                <a class="nrbox">
                    <div class="tt"><b><%# Eval("StoreName")%></b><span><%# ((DateTime)Eval("CreateTime")).ToShortDateString() %> 注册</span></div>
                    <p><span>贡献销售额<%# Eval("OrderTotal", "{0:F2}")%><strong class="text-muted">(元)</strong></span>
                        <span>贡献佣金<%# Eval("CommTotal", "{0:F2}")%><strong class="text-muted">(元)</strong></span></p>
                </a>
            </li>
        

