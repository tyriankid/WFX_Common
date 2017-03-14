<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("OrderId") %>
    </td>
    <td>
       <%# Eval("StoreName2").ToString()==""?Eval("StoreName").ToString(): Eval ("StoreName2").ToString()%>
    </td>
    <td>
        <span class="money">￥<%#Eval("CommTotal","{0:F2}" )%> </span>
    </td>
</tr>
