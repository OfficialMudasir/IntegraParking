<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Resources.ForgotPassword_Title%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- <%= Resources.ServiceName %> --%>
    <%=Resources.ForgotPassword_Title%>
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
        <li><%=Resources.ForgotPassword_Title%>
        </li>
    </ul>
</div>
--%>

<%-- 
<div class="title-alt">
    <h6><%=Resources.ForgotPassword_Title%></h6>
</div>
 --%>

<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <div class="alert alert-bky-success text-center">
                <div>
                    <span class="bky-done big-icon" ></span> 
                </div>
                <p><%=Resources.ForgotPasswordEnd_Remark%></p>
            </div>
            <p class="text-center">
                <a href="<%=Url.Action("Index", "Home", null)%>" class="btn btn-bky-primary">
                    <%=Resources.Button_Logon %>
                </a>
            </p>
        </div>
    </div>
</div>
</asp:Content>