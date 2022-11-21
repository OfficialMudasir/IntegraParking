<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MaintenanceViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="System.Configuration" %>
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

    function CustomerInscriptions_Url(urlParameter) {        
        return '<%= ConfigurationManager.AppSettings["CustomerInscriptions_Url"] ?? "" %>' + urlParameter;
    }

</script>
