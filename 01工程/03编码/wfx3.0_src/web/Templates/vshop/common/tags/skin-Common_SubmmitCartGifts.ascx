<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/Vshop/GiftDetails.aspx?GiftId=" + Eval("GiftId")%>" class="detailLink">
    <div class="box">
        <div class="left">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        </div>
        <div class="right">
            <div class="bcolor name">
                <%# Eval("Name")%></div>
            <div class="specification">
            </div>
            <div class="price text-danger">
                <%# Eval("NeedPoint")%><span class="bcolor">积分 x
                    <%# Eval("Quantity")%></span></div>
        </div>
    </div>
</a>



        <script>
            $(function () {
                var skuInputs = $('.specification input');
                $.each(skuInputs, function (j, input) {
                    var text = '';
                    $.each($(input).val().split(';'), function (i, sku) {
                        if ($.trim(sku))
                            text += '<span class="badge-h">' + sku.split('：')[1] + '</span>';
                    });
                    $(input).parent().html(text);


                });

            });
        
        
        </script>
