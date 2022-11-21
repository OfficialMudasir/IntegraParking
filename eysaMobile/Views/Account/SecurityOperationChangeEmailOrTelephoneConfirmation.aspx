<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SecurityOperationChangeEmailOrTelephoneConfirmation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="formulario">
<h1> <%=Resources.ServiceName %> </h1> 
<h2><%=Resources.SecurityOperationChangeEmailOrTelephone_Confirmation%></h2>

<br/>
 <div class="error">
 <p><%= Html.ValidationMessage("CodeExpired")%></p>
 <p><%= Html.ValidationMessage("customersDomainError")%></p>
 </div>

<%if (!Convert.ToBoolean(ViewData["ConfirmationError"]))
  {%>
<div class="div100" style="text-align:left">
 <p>
 <%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText1 %><br />
 <p>
<p class="aclaracion">                                                                                
<%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText2 %><%=ViewData["Email"] %><br />
<%=Resources.SecurityOperationChangeEmailOrTelephone_ConfirmationText3 %><%=ViewData["Telephone"] %><br />
 </p>  

</div>
<%} %>
<p>&nbsp;</p>
<br />
<div class="greenhr"><hr /></div>
<br />
<% using (Html.BeginForm("LogOn", "Home", FormMethod.Get)) { %>    
<input type="submit" value="<%=Resources.Button_Logon%>" class="botonverde" />
<% } %>
</div>




</asp:Content>
