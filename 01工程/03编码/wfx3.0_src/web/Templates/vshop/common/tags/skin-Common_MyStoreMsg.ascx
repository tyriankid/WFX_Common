<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="item">
    <div class="itemcon">
        <div class="itemtit">
            <img src="<%#Eval("UserHead")==DBNull.Value?"/Templates/vshop/common/images/user.png":Eval("UserHead") %>" height="60" width="60" />
            <span><%#Eval("UserName") %></span>
            <asp:LinkButton ID="btndel" CommandArgument='<%#Eval("ID") %>' CommandName="del" OnClientClick="return confirm('确定要删除吗？')" runat="server">删除</asp:LinkButton>
        </div>
        <div class="msgcon"><%#Eval("MessaegeCon") %></div>
        <div class="msgtim"><%#Eval("MsgTime") %></div>
    </div>
</div>