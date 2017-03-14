<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseLink.aspx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.ChooseLink" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="js/jquery-2.1.4.min.js" type="text/javascript" charset="utf-8"></script>
    <title></title>
    <style>
        table {
            margin-top:20px;
            width:100%;
            font-family:"Microsoft YaHei";
            font-size:14px;
            text-align:left;
        }
        table th:first-child{
            padding-left:10px;
            width:80%;
        }
        table th:last-child{
            text-align:right;
            padding-right:18px;
        }
        table td{
            border-bottom:1px solid #c7c7c7;
        }
        table th{
            border-bottom:2px solid #F54E4E;
        }
        table td a{
            float:right;
            color:#fff;
            display:block;
            width:50px;
            height:25px;
            text-align:center;
            line-height:25px;
            border-radius:3px;
            background:#D9534F;
            text-decoration:none;
            margin:7px;
        }
        table td a:hover{
            background:#4e5f6d;
        }
        #Panel1 {
            margin:10px 0;
            text-align: center;
        }
        #Panel1 select,#Panel1 input{
            padding:3px 5px;
        }
        #Panel1 input:nth-child(2){
            margin:0 15px;
        }
    </style>
    <script type="text/javascript">
        function xq(id) {
            var t = $("#txtt").val();
            if (t == "1") {
                window.parent.setUrl('/Vshop/WgwArticleDetails.aspx?ArticleId=' + id);
            }else if(t=="3"){
                window.parent.setUrl('/Vshop/SecondPage.aspx?SkinID=' + id);
            }else {
                window.parent.setUrl('/Vshop/WgwArticles.aspx?CategoryId=' + id);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="txtt" runat="server" />
        <asp:Panel ID="Panel1" runat="server">
            <asp:DropDownList ID="ddltype" CssClass="" runat="server"></asp:DropDownList>
            <asp:TextBox ID="txtname" CssClass="" runat="server"></asp:TextBox>
            <asp:Button ID="btnsearch" CssClass="" runat="server" Text="查询" />
        </asp:Panel>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <th>名称</th><th>操作</th>
            </tr>
        
    <asp:repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td style="text-indent:10px;"><%#Eval("Name") %></td><td><a href="javascript:" onclick="xq('<%#Eval("ID") %>')">选取</a></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
            </table>
    </div>
    </form>
</body>
</html>

