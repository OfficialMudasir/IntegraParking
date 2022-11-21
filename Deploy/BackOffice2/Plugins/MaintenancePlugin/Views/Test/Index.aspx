<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>


<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="<%= Url.Content("~/Plugins/MaintenancePlugin/Content/grid.css") %>" rel="stylesheet" type="text/css" />    

    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%
        var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    %>

<h2>    
    <%= resBundle.GetString("Resources", "MainMenu_Test") %>
</h2>

    <a href="<%: Url.RouteUrl(new { controller = "Test", action = "Index", plugin = "Test2Plugin" })%>"> Link </a>


    <%
        var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        var gridDateTimeformat = "{0:" + dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern + "}";
    %>

        <%= Html.Kendo().DatePicker()
                .Name("dateIni")
                .Value("")
                .HtmlAttributes(new { style = "width:150px" })
        %>

        <a id="FilterInfo" class="imageToolbar filterDisabled k-icon "
        href="#" onclick="return false;"
        title="">        

    </a>

</asp:Content>

