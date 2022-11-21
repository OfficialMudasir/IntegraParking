<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MaintenanceDataModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="MaintenancePlugin.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%
        var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    %>
    <%= resBundle.GetString("Resources", "MainMenu_Units") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%
        var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    %>

    <h2><%= resBundle.GetString("Resources", "MainMenu_Test") %></h2>

    <%
        Html.RenderPartial("../../Plugins/MaintenancePlugin/Views/Shared/Maintenance", Model);
    %>

</asp:Content>

