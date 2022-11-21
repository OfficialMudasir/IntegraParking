<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OperationFilterModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="System.Globalization" %>

<% using (Html.BeginForm("Main", "Account", FormMethod.Get, new { @class = "operations-filter", @role="form" }))
{ 
%>
    <div class="form-group">
        <%=Html.LabelFor(x => x.Type)%>
        <%=Html.DropDownListFor(x => x.Type, Model.Types ,"",  new Dictionary<string, object> { { "data-autopostback", "true" }, { "class", "form-control" } })%>
    </div>
    <div class="form-group">
        <%=Html.LabelFor(x => x.DateIni)%>
        <%=Html.TextBoxFor(x => x.DateIni, new { @class = "form-control select-date", @onfocus = "blur();" })%>
    </div>
    <div class="form-group">
        <%=Html.LabelFor(x => x.DateEnd)%>
        <%=Html.TextBoxFor(x => x.DateEnd, new { @class = "form-control select-date", @onfocus = "blur();" })%>
    </div>
    <div class="form-group">
        <%=Html.LabelFor(x => x.Plates)%>
        <%=Html.DropDownListFor(x => x.Plate,Model.Plates ,"",  new Dictionary<string, object> { { "data-autopostback", "true" }, { "class", "form-control" } })%>
    </div>
    <div class="form-group row-buttons">
        <button class="btn btn-bky-primary" type="submit"><%=Resources.Account_Op_Filter%></button> 
        &nbsp;
        <a class="account-reset btn" href="Main"><%=Resources.Account_Reset%></a>
    </div>
<% } %>
