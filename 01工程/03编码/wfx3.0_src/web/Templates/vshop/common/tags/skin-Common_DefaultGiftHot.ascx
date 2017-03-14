<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li>
	<a href='<%# Globals.ApplicationPath + "/Vshop/GiftDetails.aspx?GiftId=" + Eval("GiftId")%>'>
       <img src="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>">
	</a>
</li>