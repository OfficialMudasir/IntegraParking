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

    function operationsExt_CmdDelete(typeId, operationId, username) {

        var msgConfirm = "<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Confirm") %>";

        var typeInfo = "";
        if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ParkingOperation %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.ParkingOperation").ToLower() %>";                                                                 
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ExtensionOperation %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.ExtensionOperation").ToLower() %>";
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ParkingRefund %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.ParkingRefund").ToLower() %>";
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.TicketPayment %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.TicketPayment").ToLower() %>";
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.BalanceRecharge %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.BalanceRecharge").ToLower() %>";
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.ServiceCharge %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.ServiceCharge").ToLower() %>";
        else if (typeId == <%= (int) integraMobile.Domain.Abstract.ChargeOperationsType.Discount %>)
            typeInfo = "<%= resBundle2.GetString("Maintenance", "TypeEnums.ChargeOperationTypes.Discount").ToLower() %>";
        
        msgConfirm = msgConfirm.replace("{0}", typeInfo);
        msgConfirm = msgConfirm.replace("{1}", operationId);
        msgConfirm = msgConfirm.replace("{2}", username);
        msgboxConfirm("<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Title") %>", msgConfirm, "operationsExt_CmdDeleteConfirm(" + typeId + ", " + operationId + ")");

    }
    function operationsExt_CmdDeleteConfirm(typeId, operationId) {

        kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), true);

        $.ajax({
            type: 'GET',
            url: '<%= Url.Action("OperationExt_Delete", "Operation")%>',
            data: { plugin: 'PBPPlugin', typeId: typeId, operationId: operationId },
            success: function (response) {
                    
                try {
                    kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                    if (response.Result == true) {
                        msgboxAlert("<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Title") %>", "<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Success") %>", "information");                                                        
                    }
                    else {
                        msgboxAlert("<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Title") %>", response.ErrorInfo, "error");                                                        
                    }
                    ipsGrid<%: ModelId %>.Refresh();                    

                } catch (ex) {
                    msgboxError("<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Title") %>", "<%= resBundle2.GetString("Maintenance", "OperationExt_Delete_Error") %>");
                }
            },
            error: function (xhr) {
                kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                msgboxError("<%= resBundle2.GetString("PBPPlugin", "OperationExt_Delete_Title") %>", "<%= resBundle2.GetString("Maintenance", "OperationExt_Delete_Error") %>");
            }
        });

    }

    $(document).ready(function(){
        
        $(".<%: ModelId%>.export.imageToolbar.exportXls.k-icon").attr("href", '<%=Url.Action("Export", "Operation", new { filter = "~", sort = "~", modelName = Model.MaintenanceData.Name, columns = "~", format = "xls", plugin = "PBPPlugin" }) %>');
        $(".<%: ModelId%>.export.imageToolbar.exportPdf.k-icon").attr("href", '<%=Url.Action("Export", "Operation", new { filter = "~", sort = "~", modelName = Model.MaintenanceData.Name, columns = "~", format = "xls", plugin = "PBPPlugin" }) %>');

    });

</script>