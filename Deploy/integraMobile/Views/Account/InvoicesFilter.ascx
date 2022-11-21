﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<InvoiceFilterModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="System.Globalization" %>

<% using (Html.BeginForm("Invoices", "Account", FormMethod.Get, new { @class = "operations-filter", @role="form" }))
{ 
%>
    <div class="form-group">
        <%=Html.LabelFor(x => x.DateIni)%>
        <%=Html.TextBoxFor(x => x.DateIni, new { @class = "form-control select-date", @onfocus = "blur();" })%>
    </div>
    <div class="form-group">
        <%=Html.LabelFor(x => x.DateEnd)%>
        <%=Html.TextBoxFor(x => x.DateEnd, new { @class = "form-control select-date", @onfocus = "blur();" })%>
    </div>
    <div class="form-group row-buttons">
        <button class="btn btn-bky-primary" type="submit"><%=Resources.Account_Op_Filter%></button> 
        &nbsp;
        <a class="account-reset btn" href="Invoices"><%=Resources.Account_Reset%></a>
    </div>
<% } %>
