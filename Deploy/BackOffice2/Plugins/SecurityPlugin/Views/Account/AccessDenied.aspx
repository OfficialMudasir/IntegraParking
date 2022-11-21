<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="backOffice.Infrastructure" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    <%= resBundle.GetString("Security", "AccessDeniedView.Title", "Access denied") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    
    <h2><%= resBundle.GetString("Security", "AccessDeniedView.Title", "Access denied") %></h2>

    <p/>

    <%= resBundle.GetString("Security", "AccessDeniedView.Message", "") %>

</asp:Content>
