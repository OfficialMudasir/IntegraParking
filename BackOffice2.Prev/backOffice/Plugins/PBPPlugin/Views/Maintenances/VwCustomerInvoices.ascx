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

    function invoices_ExportInvoice(invoiceId) {
                    
    var url = '<%= Url.Action("Export", "Invoice", new { plugin = "PBPPlugin", invoiceId = "INVOICEIDPARAM" })%>';
        url = url.replace("INVOICEIDPARAM", invoiceId);

        window.open(url, '_blank');

    }

</script>

<style scoped="scoped">

    .k-grid tbody .k-button {
        min-width:0px
    }
    .k-grid-InvoiceExport span {
        background-image: Url(../Plugins/PBPPlugin/Content/img/grid/exportPdf.png);
        width:16px;
        height:16px;            
        display:inline-block;            
        background-size: contain;
        /*margin-top: 1px;*/
    }

</style>
