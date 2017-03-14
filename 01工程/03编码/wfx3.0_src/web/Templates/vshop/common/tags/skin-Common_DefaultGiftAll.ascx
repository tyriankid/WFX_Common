<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

		            <li>
						<a href='<%# Globals.ApplicationPath + "/Vshop/GiftDetails.aspx?GiftId=" + Eval("GiftId")%>'>
							<img src="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>">
							<div class="inter-pro-info">
								<p class="inter-pro-tit"><%#Eval("Name") %></p>
								<p class="inter-pro-price"><span><%#Eval("NeedPoint") %></span>积分</p>
								<p class="inter-pro-macket-price">市场参考价：<%#Eval("MarketPrice","{0:F2}") %>元</p>
							</div>
						</a>
					</li>