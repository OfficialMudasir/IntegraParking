<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OperationFilterModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="System.Globalization" %>

<div class="col-md-6 text-right row-buttons">
    
    <a href="<%=Url.Action("MainExport", "Account", new {
        @Type = Model.SelectedType, 
        DateIni = Model.CurrentDateIni, 
        DateEnd = Model.CurrentDateEnd, 
        Plate = Model.SelectedPlate, 
        Column = Model.CurrentGridSortOptions.Column, 
        Direction = Model.CurrentGridSortOptions.Direction, 
        format = "pdf" })%>" class="btn btn-bky-sec-danger"><i class="bky-operation"></i> <%=Resources.Account_GetPDF%></a> 

    <a href="<%=Url.Action("MainExport", "Account", new { 
        @Type = Model.SelectedType, 
        DateIni = Model.CurrentDateIni, 
        DateEnd = Model.CurrentDateEnd, 
        Plate = Model.SelectedPlate, 
        Column = Model.CurrentGridSortOptions.Column, 
        Direction = Model.CurrentGridSortOptions.Direction, 
        format = "xls" })%>" class="btn btn-bky-sec-success"><i class="bky-operation"></i> <%=Resources.Account_GetExcel%></a>
</div>