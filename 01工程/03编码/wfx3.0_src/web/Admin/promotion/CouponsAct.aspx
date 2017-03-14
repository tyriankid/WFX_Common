<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewCoupons.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CouponsAct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
      <h1>优惠券活动管理 </h1>
     <span>管理店铺新创建的优惠券活动，您添加优惠卷活动，以用来会员分享给其他会员优惠卷</span></div>
    <!-- 添加按钮-->
    <div class="btn">
     <a href="AddCouponAct.aspx" class="submit_jia">添加优惠券活动</a>
    </div>
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">   
     <UI:Grid ID="grdCoupons" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="ID" HeaderStyle-CssClass="table_title" GridLines="None"  SortOrderBy="ID" SortOrder="DESC">
                        <Columns>  
                                <asp:TemplateField HeaderText="领取页背景图" SortExpression="Name" HeaderStyle-CssClass="td_right td_left" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate>
                                        <img src='<%#Eval("BgImg") %>' height="60"/>
                                  </ItemTemplate>
                               </asp:TemplateField>    
                            <asp:TemplateField HeaderText="活动名称" SortExpression="ColValue2" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                     <Hi:SubStringLabel ID="lblColValue2" StrLength="15" StrReplace="..."  Field="ColValue2" runat="server" ></Hi:SubStringLabel>
                                  </ItemTemplate>
                               </asp:TemplateField>       
                               <asp:TemplateField HeaderText="优惠券名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                     <Hi:SubStringLabel ID="lblCouponName" StrLength="60" StrReplace="..."  Field="Name" runat="server" ></Hi:SubStringLabel>
                                  </ItemTemplate>
                               </asp:TemplateField>                  
                                 <asp:TemplateField HeaderText="开始日期" SortExpression="StartTime" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                    <div style="width:120px;"><Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("StartTime")%>' runat="server" ></Hi:FormatedTimeLabel></div>
                                  </ItemTemplate>
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="结束日期" SortExpression="ClosingTime" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                    <div style="width:120px;"><Hi:FormatedTimeLabel ID="lblClosingTimes" Time='<%#Eval("ClosingTime")%>' runat="server" ></Hi:FormatedTimeLabel></div>
                                  </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="满足金额" HeaderStyle-CssClass="td_right td_left">
                                    <itemtemplate>
                                        <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%#Eval("Amount") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                    </itemtemplate>
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="可抵扣金额" HeaderStyle-CssClass="td_right td_left">
                                <itemtemplate>
                                    <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("DiscountValue") %>' runat="server" ></Hi:FormatedMoneyLabel>
                                </itemtemplate>
                              </asp:TemplateField>
                            <asp:BoundField DataField="ColValue1" HeaderText="会员每天发放数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                               <asp:BoundField DataField="SentCount" HeaderText="总数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField>
                               <asp:BoundField DataField="UsedCount" HeaderText="已使用数量" HtmlEncode="false" HeaderStyle-CssClass="td_right td_left"></asp:BoundField> 
                                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width=250>                                    
                                    <ItemTemplate>
                                        <span class="submit_bianji"><asp:HyperLink ID="hyperLink1"  runat="server" visible='<%# IsCouponEnd(Eval("ClosingTime")) %>'   NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddCouponAct.aspx?ID={0}", Eval("ID")))%>'  Text="编辑" /> </span>
			                            <span class="submit_shanchu"><Hi:ImageLinkButton  ID="lkbDelete" runat="server"     CommandName="Delete" Text="删除" OnClientClick="javascript:return confirm('确定要执行改删除操作吗？删除后将不可以恢复')"></Hi:ImageLinkButton> </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                     </UI:Grid>
                    
                    <script type="text/javascript">
                        

                       
                       
                    </script>
                     


</div>

   
                              
      <div class="blank5 clearfix"></div>
   
    <!--数据列表底部功能区域-->
  </div>
</asp:Content>