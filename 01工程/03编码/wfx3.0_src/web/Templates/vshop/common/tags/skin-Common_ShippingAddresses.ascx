<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well address-box" shippingId="<%#Eval("ShippingId") %>">
    <div class="font-xl">
        <span>收货人：<%#Eval("ShipTo")%></span><em><%#Eval("CellPhone")%></em>
            <div>
                <a onclick='UpdateShipping(<%#Eval("ShippingId") %>)' href="javascript:void(0)"><span
                    class="glyphicon glyphicon-pencil"></span></a><a href="javascript:void(0)" onclick='DeleteShippingAddress(<%#Eval("ShippingId") %>,this)'>
                        <span class="glyphicon glyphicon-trash"></span></a>
            </div>
    </div>
    <div class="font-m">
        收货地址：<Hi:RegionAllName ID="regionname" runat="server" RegionId='<%#Eval("RegionId") %>'></Hi:RegionAllName> <%#Eval("Address")%></div>
</div>
