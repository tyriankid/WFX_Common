<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<tr>
    <td>
        <%# Eval("RequestTime","{0:d}")%>
    </td>
    <td>
        <%# Eval("Amount","{0:F2}") %>
    </td>
    <td>
        <%# Eval("IsCheck").Equals(true) ? (Eval("Remark").ToString().IndexOf("不通过")>-1 ?"未通过": "已完成") : "待审核"%>
    </td>
</tr>
