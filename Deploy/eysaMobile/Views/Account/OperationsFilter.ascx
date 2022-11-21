<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OperationFilterModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="System.Globalization" %>
<%
    var htmlAttributes = new Dictionary<string, object> { { "data-autopostback", "true" }, { "class", "inputelementsw100" } }; 
%>
<%
    using (Html.BeginForm("Main", "Account", FormMethod.Get, new { id = "operationSearch" }))
{ %>

 <script>
     $(function () {
         $.datepicker.setDefaults($.datepicker.regional[""]);
         if ($.datepicker.regional["<%=((CultureInfo)Session["Culture"]).Name%>"]==null)
         {
             $("#DateIni").datepicker($.datepicker.regional["<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>"]);
             $("#DateEnd").datepicker($.datepicker.regional["<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>"]);
         }
         else
         {
             $("#DateIni").datepicker($.datepicker.regional["<%=((CultureInfo)Session["Culture"]).Name%>"]);
             $("#DateEnd").datepicker($.datepicker.regional["<%=((CultureInfo)Session["Culture"]).Name%>"]);

         }

     });
 </script>

<div id="operationsfiltercontainer">
  <div class="operationsfilterfield">
    <p><%=Html.LabelFor(x => x.DateIni)%>:</p>
     <%=Html.TextBoxFor(x => x.DateIni, new { @class = "inputelementsw100" })%>
  </div>
  <div class="operationsfilterfield">
    <p><%=Html.LabelFor(x => x.DateEnd)%>:</p>
     <%=Html.TextBoxFor(x => x.DateEnd, new { @class = "inputelementsw100" })%>
  </div>
  <div class="operationsfilterfield">
    <p><%=Html.LabelFor(x => x.Type)%>:</p>
    <%=Html.DropDownListFor(x => x.Type,Model.Types ,"",  htmlAttributes)%>
  </div>
  <div class="operationsfilterfield">
    <p><%=Html.LabelFor(x => x.Plate)%>:</p>
    <%=Html.DropDownListFor(x => x.Plate,Model.Plates ,"",  htmlAttributes)%>
  </div>
  <div class="operationsfilterfield">
    <input type="submit" name="filter" value="<%=Resources.Account_Op_Filter%>"" 
     class="botongris" style="margin-left:0; display:inline; float:left; width:auto; margin-top:19px; padding:3px; padding-left:8px;padding-right:8px;" />
  </div>
    <div class="operationsfilterExport">
        <a class="export imageToolbar exportXls " 
            href="<%=Url.Action("MainExport", "Account", new { @Type = Model.SelectedType, DateIni = Model.CurrentDateIni, DateEnd = Model.CurrentDateEnd, Plate = Model.SelectedPlate, Column = Model.CurrentGridSortOptions.Column, Direction = Model.CurrentGridSortOptions.Direction, format = "xls" })%>" 
            title="<%= Resources.Account_Op_ExportXls_Title %>" >
        </a> 
        <a class="export imageToolbar exportPdf " 
            href="<%=Url.Action("MainExport", "Account", new { @Type = Model.SelectedType, DateIni = Model.CurrentDateIni, DateEnd = Model.CurrentDateEnd, Plate = Model.SelectedPlate, Column = Model.CurrentGridSortOptions.Column, Direction = Model.CurrentGridSortOptions.Direction, format = "pdf" })%>" 
            title="<%= Resources.Account_Op_ExportPdf_Title %>" >
        </a> 
    </div>
</div>
<%} %>