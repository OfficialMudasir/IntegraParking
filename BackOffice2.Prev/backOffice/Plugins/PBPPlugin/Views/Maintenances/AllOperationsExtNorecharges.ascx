<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MaintenanceViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Infrastructure.Security" %>
<%@ Import Namespace="MaintenancePlugin" %>
<%@ Import Namespace="MaintenancePlugin.Models" %>

<% 
    var ModelId = Model.ModelId;
    var ModelName = Model.MaintenanceData.Name;
    var ModelData = Model.MaintenanceData;

    var sAssemblyBasePath = (!string.IsNullOrEmpty(ModelData.Definition.ResourcesAssemblyName) ? RouteConfig.BasePath.Replace("/MaintenancePlugin/", "/" + ModelData.Definition.ResourcesAssemblyName + "/") : RouteConfig.BasePath);
%>
<link href="<%= Url.Content(sAssemblyBasePath + "Content/Maintenance/grid.css?v1.0") %>" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    <%
        var resBundle2 = ResourceBundle.GetInstance(Model.MaintenanceData.Definition.ResourcesAssemblyName);
    %>


    $(document).ready(function(){
        
        $(".<%: ModelId%>.export.imageToolbar.exportXls.k-icon").attr("href", '<%=Url.Action("Export", "Operation", new { filter = "~", sort = "~", modelName = Model.MaintenanceData.Name, columns = "~", format = "xls", plugin = "PBPPlugin" }) %>');
        $(".<%: ModelId%>.export.imageToolbar.exportPdf.k-icon").attr("href", '<%=Url.Action("Export", "Operation", new { filter = "~", sort = "~", modelName = Model.MaintenanceData.Name, columns = "~", format = "xls", plugin = "PBPPlugin" }) %>');

    });

</script>