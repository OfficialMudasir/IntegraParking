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


<%-- <a href="<%= Url.Action("Manage", "Permits", new { OperationId = Model.Id, City = Model.CityId, Zone = Model.GrpId, Tariff = Model.TariffId, P1 = P1, P2 = P2, P3 = P3, P4 = P4, P5 = P5, P6 = P6, P7 = P7, P8 = P8, P9 = P9, P10 = P10}) %>"><button type="button" class="permit-pay-button btn btn-danger"><i class="fontawesome-shopping-cart"></i>&nbsp;&nbsp;<% =Resources.Permits_Manage %></button></a> --%>
<a class="btn btn-xs btn-bky-success" href="<%= Url.Action("Manage", "Permits", new { OperationId = Model.Id, City = Model.CityId, Zone = Model.GrpId, Tariff = Model.TariffId, P1 = P1, P2 = P2, P3 = P3, P4 = P4, P5 = P5, P6 = P6, P7 = P7, P8 = P8, P9 = P9, P10 = P10}) %>">
    <i class="bky-edit"></i>
    &nbsp;
    <% =Resources.Permits_Manage %>
</a>