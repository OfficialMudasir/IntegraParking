<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PermitRowModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<% 
    string P1 = string.Empty, P2 = string.Empty, P3 = string.Empty, P4 = string.Empty, P5 = string.Empty, P6 = string.Empty, P7 = string.Empty, P8 = string.Empty, P9 = string.Empty, P10 = string.Empty;
    List<string> P = new List<string>();
    P = Model.Plates.Split(new[] {", "}, StringSplitOptions.None).ToList();
    if (P.Count > 0) P1 = P[0];
    if (P.Count > 1) P2 = P[1];
    if (P.Count > 2) P3 = P[2];
    if (P.Count > 3) P4 = P[3];
    if (P.Count > 4) P5 = P[4];
    if (P.Count > 5) P6 = P[5];
    if (P.Count > 6) P7 = P[6];
    if (P.Count > 7) P8 = P[7];
    if (P.Count > 8) P9 = P[8];
    if (P.Count > 9) P10 = P[9];
%>
<% 
    if (Model.RenewAutomatically || Model.PaymentDisabled) 
    { 
        %>
        <%-- <button type="button" class="permit-pay-button btn btn-danger" disabled="disabled"><i class="fontawesome-shopping-cart"></i>&nbsp;&nbsp;<% =Resources.Permits_Pay %></button> --%>
        <button type="button" class="permit-pay-button btn btn-xs btn-bky-default" disabled="disabled">
            <i class="bky-permit-operations"></i> &nbsp; <% =Resources.Permits_Pay %>
        </button>
        <% 
    } 
    else 
    { 
        %>
        <%-- <a href="<%= Url.Action("PayForPermit", "Permits", new { SelectedCity = Model.CityId, SelectedZone = Model.GrpId, SelectedMonth = ((DateTime)Model.DateIni).ToString("MMyy"), SelectedNumPermits = 1, SelectedPlate1 = P1, SelectedPlate2 = P2, SelectedPlate3 = P3, SelectedPlate4 = P4, SelectedPlate5 = P5, SelectedPlate6 = P6, SelectedPlate7 = P7, SelectedPlate8 = P8, SelectedPlate9 = P9, SelectedPlate10 = P10, SelectedTariff = Model.TariffId }) %>"><button type="button" class="permit-pay-button btn btn-danger"><i class="fontawesome-shopping-cart"></i>&nbsp;&nbsp;<% =Resources.Permits_Pay %></button></a> --%>
        <a class="permit-pay-button btn btn-xs btn-bky-danger" href="<%= Url.Action("PayForPermit", "Permits", new { SelectedCity = Model.CityId, SelectedZone = Model.GrpId, SelectedMonth = ((DateTime)Model.DateIni).ToString("MMyy"), SelectedNumPermits = 1, SelectedPlate1 = P1, SelectedPlate2 = P2, SelectedPlate3 = P3, SelectedPlate4 = P4, SelectedPlate5 = P5, SelectedPlate6 = P6, SelectedPlate7 = P7, SelectedPlate8 = P8, SelectedPlate9 = P9, SelectedPlate10 = P10, SelectedTariff = Model.TariffId }) %>">
            <i class="bky-permit-operations"></i> &nbsp; <% =Resources.Permits_Pay %>
        </a>
        <% 
    } 
%>
