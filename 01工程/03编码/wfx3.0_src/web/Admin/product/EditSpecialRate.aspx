<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditSpecialRate"
    MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    
    <div id="divCommissions" >
        <div class="frame-content" style="width: 100%;">
            <p><h3>熳洁儿小区代 </h3></p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text" id="first" name="txtthird" runat="server" value="0" />%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
            <p>
                <h2>熳洁儿二级客户/电商部门</h2>
            </p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text"  id="second" name="txtsecond" runat="server" value="0" />%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
            <p>
                <h2>熳洁儿三级客户</h2>
            </p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text"  id="third" name="txtthird" runat="server" value="0"/>%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
            <p>
                <h2>芬奈/U美单品牌客户</h2>
            </p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text" id="fourth" name="txtfourth" runat="server" value="0" />%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
            <p>
                <h2>非在册客户/公司内部员工</h2>
            </p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text" id="fifth" name="txtfifth" runat="server" value="0"/>%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
            <p>
                <h2>PC端客户</h2>
            </p>
            <p>
                <span class="frame-span frame-input90" style="width: 160px;">折扣设置：</span>
                <input type="text" id="sixth" name="txtsixth" runat="server" value="0"/>%
                <span style="color: #888888; padding-left: 10px;">输入1-100的数</span>
            </p>
        </div>
        <asp:Button ID="btnSaveRate" runat="server"  Text="保存设置"  CssClass="submit_DAqueding"/>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id='<%=first.ClientID%>']").blur(checkCommission);
            $("input[id='<%=second.ClientID%>']").blur(checkCommission);
            $("input[id='<%=third.ClientID%>']").blur(checkCommission);
            $("input[id='<%=fourth.ClientID%>']").blur(checkCommission);
            $("input[id='<%=fifth.ClientID%>']").blur(checkCommission);
        });

        var checkCommission = function () {
            var num = $(this).val();
            if (!/^[0-9]\d*$/i.test(num)) { //匹配正整数
                //if (!/^[1-9]\d*|0$/i.test(num)) { //匹配正整数
                $(this).val(1);
                alert("佣金格式不正确");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
