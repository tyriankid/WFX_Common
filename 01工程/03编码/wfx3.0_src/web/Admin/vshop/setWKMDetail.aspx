<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="setWKMDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.setWKMDetail" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
        <div class="title">
        <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1>设置微信答题活动附加属性</h1>
        <span>设置微信答题活动的背景图和匹配度描述等信息</span>
        </div>  
        <div class="formitem validator2">
        <ul>
            <li><span class="formitemtitle Pw_128">活动背景图：</span>
                <asp:FileUpload ID="backImgUpload" runat="server" />
                <div style="padding:3px"><a name="imgLink"><asp:Image ID="backImg" runat="server" style="height:50px"/></a></div>
                <p id="txtKeywordTip"> 推荐图片尺寸480*960</p>
            </li>
            <li><span class="formitemtitle Pw_128">活动Logo：</span>
                <asp:FileUpload ID="logoImgUpload" runat="server" />
                <div style="padding:3px"><a name="imgLink"><asp:Image ID="logoImg" runat="server" style="height:50px"/></a></div>
                <p id="P4"> 推荐图片尺寸60*60</p>
            </li>
            <li><span class="formitemtitle Pw_128">微信分享图标：</span>
                <asp:FileUpload ID="wxImgUpload" runat="server" />
                <div style="padding:3px"><a name="imgLink"><asp:Image ID="wxImg" runat="server" style="height:50px"/></a></div>
                <p id="P5"> 推荐图片尺寸60*60</p>
            </li>
            <li><span class="formitemtitle Pw_128">广告图1：</span>
                <asp:FileUpload ID="adImgUpload1" runat="server" /> 广告链接:<asp:TextBox ID="txtAd1" runat="server" style="width:300px;height:20px"></asp:TextBox>
                <div style="padding:3px"><a name="imgLink"><asp:Image ID="adImg1" runat="server" style="height:35px"/></a><asp:Button ID="deleteAd1" runat="server" Text="删除" onclick="deleteAd1_Click"/></div>
                <p id="P1"> 推荐图片尺寸250*50</p>
            </li>
            <li><span class="formitemtitle Pw_128">广告图2：</span> 
                <asp:FileUpload ID="adImgUpload2" runat="server" /> 广告链接:<asp:TextBox ID="txtAd2" runat="server" style="width:300px;height:20px"></asp:TextBox>
                <div style="padding:3px"><a name="imgLink"><asp:Image ID="adImg2" runat="server" style="height:35px"/></a><asp:Button ID="deleteAd2" runat="server" Text="删除" onclick="deleteAd2_Click"/></div>
                <p id="P2"> 推荐图片尺寸250*50</p>
            </li>
            <li><span class="formitemtitle Pw_128">版权信息：</span> 
                <asp:TextBox ID="txtCopyright" runat="server" style="width:300px;height:auto" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_128">引导关注页地址：</span> 
                <asp:TextBox ID="txtGuidePageUrl" runat="server" style="width:500px;height:20px"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_128"><em >*</em>增加匹配度描述：</span>
                <input type="button" id="addSubject" value="增加一个"/>
                <input type="button" id="reSet" value="清除"/>
                <p id="P3"> 默认有5个匹配度区间,可自定义区间数量和匹配度描述</p>
            </li>
            <li id="matchInfoArea" style="display:none">
                <table id="matchInfoTable">

                </table>
            </li>
        </ul>
        <ul class="btn Pa_100 clearfix">
            <asp:Button ID="btnAddWKM" runat="server" OnClientClick="return PageIsValid();"
                Text="保 存" CssClass="submit_DAqueding" onclick="btnAddWKM_Click"/>
        </ul>
        </div>
    </div>
    </div>
    <input type="hidden" runat="server" id="mId" /><!--匹配guid区间-->
    <input type="hidden" runat="server" id="mStart" /><!--匹配度起始区间-->
    <input type="hidden" runat="server" id="mEnd" /><!--匹配结束始区间-->
    <input type="hidden" runat="server" id="mDes" /><!--匹配描述-->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#addSubject").click(function () { addSubject() }); 
            $("#reSet").click(function () { $("#matchInfoTable").html("") });
            $("a[name=imgLink]").click(function () { window.open($(this).find("img").attr("src")) });//点击图片可以跳转预览
            $("a[name=delete]").click(function () {
                var parent = $(this).closest("li");
                parent.find("input").val("");
                parent.find("img").remove();
            });
            //页面载入时,默认给五个区间
            for (var i = 0; i < 5; i++) {
                addSubject();
            }
            //并且默认设置区间值
            $("tr[name=matchInfo]").each(function (e) {
                $(this).find("input[name=start]").val((e * 20 + 1) == 1 ? 0 : (e * 20 + 1));
                $(this).find("input[name=end]").val((e + 1) * 20);
            });
            //载入匹配描述列表
            loadMatchInfoList();
        });

        function loadMatchInfoList() {

            var mStartList = $("#mStart").val().split(';');
            var mEndList = $("#mEnd").val().split(';');
            var mDesList = $("#mDes").val().split(';');
            var mIdList = $("#mId").val().split(';');
            if (mStartList.length == 1)
                return;
            //先清除,在根据列表数量增加
            $("#matchInfoTable").html("");
            for (var i = 0; i < mIdList.length; i++) {
                addSubject();
            }
            $("tr[name=matchInfo]").each(function (e) {
                $(this).attr("id", mIdList[e]);
                $(this).find("input[name=start]").val(mStartList[e]);
                $(this).find("input[name=end]").val(mEndList[e]);
                $(this).find("input[name=des]").val(mDesList[e]);
            });
        }

        function addSubject() {
            $("#matchInfoArea").show();
            var sbjCount = $("tr[name=matchInfo]").length + 1;
            var inputHtml = "<tr name='matchInfo' id='" + guid() + "' >";
            inputHtml += "<td><input type='text' name='start' id='mStart_" + sbjCount + "' style='width:30px;height:25px' />% 至 </td>  "; //开始区间
            inputHtml += "<td><input type='text' name='end' id='mEnd_" + sbjCount + "' style='width:30px;height:25px' />%</td>"; //结束区间
            inputHtml += "<td>描述:<input type='text' name='des' id='mDes_" + sbjCount + "' style='width:500px;height:25px' /></td>"
            inputHtml += "</tr>";
            $("#matchInfoTable").append(inputHtml);
        }

        function PageIsValid() {
            var flag = true;
            var mStart = "";
            var mEnd = "";
            var mDes = "";
            var mId = "";
            if ($("tr[name=matchInfo]").length == 1) {
                alert("至少填写两条匹配度信息!");
                return false;
            }
            $("tr[name=matchInfo]").each(function (e) {
                var currentMstart=$(this).find("input[name=start]").val().trim();
                var currentMend = $(this).find("input[name=end]").val().trim();
                var currentMdes = $(this).find("input[name=des]").val().trim();
                var currentMid = $(this).attr("id");
                if ((currentMstart != "" && currentMend != "" && currentMdes != "")) {
                    mStart += currentMstart + ";";
                    mEnd += currentMend + ";";
                    mDes += currentMdes + ";";
                    mId += currentMid + ";";
                }
                else {
                    alert("请将每行的信息填写完整!");
                    flag = false;
                }
            });
            mStart = mStart.substr(0, mStart.length - 1);
            mEnd = mEnd.substr(0, mEnd.length - 1);
            mDes = mDes.substr(0, mDes.length - 1);
            mId = mId.substr(0, mId.length - 1);
            $("#mStart").val(mStart);
            $("#mEnd").val(mEnd);
            $("#mDes").val(mDes);
            $("#mId").val(mId);
            return flag;
        }

        function getSum(num1, num2) { return (parseInt(num1) + parseInt(num2)); }
        function getX(num1, num2) { return (parseInt(num1) * parseInt(num2)); }
        function guid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    </script>
</asp:Content>
