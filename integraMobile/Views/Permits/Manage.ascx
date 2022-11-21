<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PermitRowModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%-- <a href="<%= Url.Action("Manage", "Permits", new { SelectedCity = Model.CityId, SelectedZone = Model.GrpId, SelectedPlates = Model.Plates, SelectedTariff = Model.TariffId }) %>"><button type="button" class="permit-pay-button btn btn-danger"><i class="fontawesome-shopping-cart"></i>&nbsp;&nbsp;<% =Resources.Permits_Manage %></button></a> --%>
<a  class="permit-pay-button btn btn-danger" href="<%= Url.Action("Manage", "Permits", new { SelectedCity = Model.CityId, SelectedZone = Model.GrpId, SelectedPlates = Model.Plates, SelectedTariff = Model.TariffId }) %>">
    <i class="fontawesome-shopping-cart"></i>
    <% =Resources.Permits_Manage %>
</a> 