<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.UserData_RemoveUserAccount %>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.UserData_RemoveUserAccount%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div class="row">
    <div id="paper-top">
        <div class="col-sm-12">
            <h2 class="tittle-content-header">
                <span><%=Resources.ServiceName %>
                </span>
            </h2>

        </div>
    </div>
</div>
--%>
<%--
<div id="breadcrumb-wrapper" class="row">
    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Index", "Home") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.UserData_RemoveUserAccount %>
        </li>
    </ul>
</div>
--%>
<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
                <h3><%=Resources.UserData_DeleteUserTitle%></h3>

                    <div class="alert alert-bky-success">
                        <span class="bky-ok"></span>
                        &nbsp;
                        <%=Resources.UserData_DeleteUserEnd_Remark%> 
                    </div>

                    <div class="row-buttons">
                        <a href="<%=Url.Action("Index", "Home", null)%>" class="btn btn-bky-primary"><%=Resources.Button_Logon %></a>
                    </div>
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

</asp:Content>
