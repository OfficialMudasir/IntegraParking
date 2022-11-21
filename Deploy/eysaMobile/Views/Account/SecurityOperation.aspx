<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SecurityOperation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
 <div class="error">
 <p><%= Html.ValidationMessage("ConfirmationCodeError")%></p>
 </div>
</div>

</asp:Content>
