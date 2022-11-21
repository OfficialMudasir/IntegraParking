<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="MaintenancePlugin.Models" %>

<script>


    $(document).ready(function () {
        triCheckbox_Init();        
    });


</script>

<div class="editor-form">

    <ul class="edit-form-errors"></ul>

    <%
        MaintenanceViewModel oMaintenance = (MaintenanceViewModel) ViewData["maintenance"];
        foreach (MaintenanceFieldDataModel oField in oMaintenance.MaintenanceData.Fields)
        {
            %>
            <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelForModel(oField.LocalizedName, new { @for = oField.Mapping })%>                
            </div>
            <div class="editor-field">
                <%=
                    oField.Definition.GetHtmlEditControl(Html, oMaintenance.ModelId)                    
                 %>
            </div>
            </div>
            <%
        }
     %>

</div>