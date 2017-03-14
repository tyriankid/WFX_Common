<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="ManageMenu.aspx.cs" Inherits="Admin_ManageMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="zTree/zTreeStyle.css" type="text/css"/>
	<script type="text/javascript" src="zTree/jquery-1.4.4.min.js"></script>
	<script type="text/javascript" src="zTree/jquery.ztree.core-3.5.js"></script>
	<script type="text/javascript" src="zTree/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript" src="../Utility/windows.js"></script>
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen" />
<script type="text/javascript">
		<!--
    var setting = {
        check: {
            enable: true
        },
        data: {
            simpleData: {
                enable: true
            }
        }
    };

    //根据页面表格内的元素拼接字符串传递到隐藏域,由后台继续处理
    function checkSelect()
    {
        var json = "[";
        $('.datalist tr').each(function (i) {
            if (i != 0) {
                var inputCount = $(this).children().children().length;
                json+="{"
                for (var i = 0; i < inputCount; i++) {
                    json += "\"" + $(this).children().children().eq(i).attr("name") + "\":" + "\"" + $(this).children().children().eq(i).val()+"\",";
                }
                json = json.substring(0, json.length - 1) + "}";
            }
        });
        json += "]";
        $("#hiSelectIDS").val(json);
        return true;
    }

	</script>
</head>
    <body onload="loadSelect()">
       <form id="form1" runat="server">
<div class="areacolumn clearfix" >
      <div class="columnright">
          <div class="title">
            <em><img src="images/04.gif" width="32" height="32" /></em>
            <h1>后台模块配置</h1>
            <span>后台权限菜单，配置管理员登录后台后可见的菜单</span>
          </div>

      <div>
      <ul class="btn">
            <asp:Button ID="btnSave" runat="server" Text="保 存" OnClientClick="return checkSelect()" CssClass="submit_DAqueding" OnClick="btnSave_Click" /><br />
            <asp:HiddenField ID="hiSelectIDS" runat="server"  Value=""/>
            <asp:HiddenField ID="hiSelectNameS" runat="server"  Value=""/>
            <asp:HiddenField ID="hiSelectLinkS" runat="server"  Value=""/>
        </ul>
      </div>

          <div class="dataarea" >
<div class="datalist">
<asp:Repeater runat="server" ID="allMenuList" >
            <HeaderTemplate>
                <table border="0" cellspacing="0" cellpadding="0" style="table-layout: fixed">
                    <tr class="table_title">
                        <td width="10%" class="td_right td_left" style="display:none">
                            选择
                        </td>
                        <td width="15%" class="td_right td_left">
                            菜单排序ID
                        </td>
                        <td width="15%" class="td_right td_left">
                            菜单名
                        </td>
                        <td width="40%" class="td_right td_left">
                            菜单URL
                        </td>
                        <td width="40%" class="td_right td_left">
                            图标Url
                        </td>
                        <td width="20%" class="td_right td_left">
                            是否隐藏
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                   <tr>
                       <td>
                           <input type="hidden" name="MIID"value='<%#Eval("MIID") %>' style="width:100%"/>
                           <input type="text" name="Layout" value='<%#Eval("Layout") %>' style="<%# Eval("Layout").ToString().Length<=2?"width:100%;":Eval("Layout").ToString().Length<=4?"margin-left:20px;width:85%;":"margin-left:40px;width:70%;" %>"/>
                       </td>
                       <td>
                           <input type="text" name="MiName" value='<%#Eval("MIName") %>' style="width:100%"/>
                       </td>
                       <td>
                           <input type="text" name="MIUrl" value='<%#Eval("MIUrl") %>' style="width:100%"/>
                       </td>
                       <td>
                           <input type="text" name="IconLink" value='<%#Eval("IconLink") %>' style="width:100%"/>
                       </td>
                       <td>
                           <input type="text" name="visible" value='<%#Eval("visible") %>' style="width:100%;text-align:center"/>
                       </td>
                   </tr>
            </ItemTemplate>
            <FooterTemplate>
                  </table>
            </FooterTemplate>
</asp:Repeater>
</div>
</div>



      </div>
  </div>
        </form>
</body>
  </html>