<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PermitRowModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<% 
if (Model.AutoRenewalDisabled == true)
{    
%>
<input type="checkbox" class="RenewAutomatically" name="RenewAutomatically" disabled="disabled" /> <%= Resources.Permits_Automatic_Renewal %>
<%
}
else 
{
%>
<input type="checkbox" class="RenewAutomatically" name="RenewAutomatically" value="<% =Model.Id %>" <% if (Model.RenewAutomatically) { %>checked="checked"<% } %> onclick="AutoRenewal('<% =Model.RenewAutomatically %>','<% =Model.Id %>')" /> <%= Resources.Permits_Automatic_Renewal %>
<%
}
%>