<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div id="cartGifts" class="well shopcart">
<asp:Literal ID="litpromotion" runat="server"></asp:Literal> 

<asp:Repeater ID="rptCartGift" runat="server" DataSource='<%# Eval("LineGifts") %>' >
    <ItemTemplate>
    <hr style="margin:0 0px 0 0px;">
        <div class="goods-box goods-box-shopcart">
          
                <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
                <div class="info">
                     <a href="<%# Globals.ApplicationPath + "/Vshop/GiftDetails.aspx?GiftId=" + Eval("GiftId")%>"> <div class="name bcolor">
                        <%# Eval("Name")%></div></a>

                    <div class="price text-danger">
                        <%# Eval("NeedPoint")%>积分
                    </div>
                    <div class="goods-num">
		                <div name="spSub" class="shopcart-minus">
		                    - 
		                </div>
		                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>' giftid='<%# Eval("GiftId")%>'/>
		                <div name="spAdd" class="shopcart-add">
		                    + 
		                </div>
		            </div>
                </div>
            <a class="link-del" href="javascript:void(0)" name="iDelete" giftid='<%# Eval("GiftId")%>'><span></span></a>

            <!--<div class="goods-num">
                <div name="spSub" class="shopcart-minus">
                    - 
                </div>

                <div name="spAdd" class="shopcart-add">
                    + 
                </div>
            </div>-->
        </div>
    </ItemTemplate>
    
</asp:Repeater>
<div><asp:Literal ID="litline" runat="server"></asp:Literal> </div>

</div>
