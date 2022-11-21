<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.UserData_ChangeTelOrEmailTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%=Resources.Account_Main_BttnUserData%> --%>
    <%=Resources.UserData_ChangeTelOrEmailTitle%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div id="breadcrumb-wrapper" class="row">
    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="UserData" title="<%=Resources.Account_Main_BttnUserData%>"><%=Resources.Account_Main_BttnUserData%></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.UserData_ChangeTelOrEmailTitle %>
        </li>
    </ul>
</div>
--%>
<div class="content-wrap">
    <div class="row">
        <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-block">

                    <div class="alert alert-bky-info">
                        <span class="bky-info"></span> 
                        &nbsp; 
                        <%=Resources.UserData_ChangeEmailOrMobileEnd_Remark%>
                    </div>

                    <div class="row-buttons">
                        <a class="btn btn-bky-primary" href="UserData"><%=Resources.Pager_Previous%></a>
                    </div>

        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->
</asp:Content>