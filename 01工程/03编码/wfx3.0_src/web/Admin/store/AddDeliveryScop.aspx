<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddDeliveryScop.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddDeliveryScop" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function doSave() {
            // var RegionId = $("").val();
            var RegionId = $("#regionSelectorValue").val();

            var RegionName = $("#regionSelectorName").val();
            if (RegionId == "" || RegionId == undefined) {
                alert("请选择一个区域");
            }
            else {
                artDialog.open.origin.SaveDeliveryScop(RegionId, RegionName);
                art.dialog.close();
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="datafrom">
        <div class="formitem validator1">
            <ul style="margin: 20px;">
                <li><span class="formitemtitle Pw_198">选择配送区域：</span><p></p>
                </li>
                <li>
                    <Hi:RegionSelector ID="dropRegion" runat="server" IsShift="false" ProvinceWidth="180" CityWidth="150" CountyWidth="150" />
                </li>
            </ul>
            <br />
            <br />
            <br />
            <ul class="btntf Pa_198 clear">
                <input type="button" id="saveBtn" onclick="doSave()" class="submit_DAqueding inbnt" value="保存" />
            </ul>
        </div>
    </div>
</asp:Content>
