<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<InvoiceFilterModel>" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="System.Globalization" %>
<%
    var htmlAttributes = new Dictionary<string, object> { { "data-autopostback", "true" }, { "class", "inputelementsw100" } }; 
%>
<%
    using (Html.BeginForm("Invoices", "Account", FormMethod.Get, new { id = "invoicesSearch" }))
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
    <input type="submit" name="filter" value="<%=Resources.Account_Op_Filter%>"" 
     class="botongris" style="margin-left:0; display:inline; float:left; width:auto; margin-top:19px; padding:3px; padding-left:8px;padding-right:8px;" />
  </div>
</div>
<%} %>