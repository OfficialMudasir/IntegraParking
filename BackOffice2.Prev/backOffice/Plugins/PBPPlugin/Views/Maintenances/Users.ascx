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

    function users_CmdDisable(userId, username) {
        var msgConfirm = "<%= resBundle2.GetString("PBPPlugin", "User_Disable_Confirm") %>";
            msgConfirm = msgConfirm.replace("{0}", username);
            msgboxConfirm("<%= resBundle2.GetString("PBPPlugin", "User_Disable_Title") %>", msgConfirm, "users_CmdDisableConfirm(" + userId + ")");
        }

        function users_CmdDisableConfirm(userId) {

            kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), true);

            $.ajax({
                type: 'POST',
                url: '<%= Url.Action("User_Disable", "User")%>',
                data: { plugin: 'PBPPlugin', userId: userId },
                success: function (response) {

                    try {
                        kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {                            
                            ipsGrid<%: ModelId %>.Refresh();                            
                        }
                        else {
                            msgboxAlert("<%= resBundle2.GetString("PBPPlugin", "User_Disable_Title") %>", response.ErrorInfo, "warning");
                        }

                    } catch (ex) {
                        msgboxError("<%= resBundle2.GetString("PBPPlugin", "User_Disable_Title") %>", "<%= resBundle2.GetString("PBPPlugin", "User_Disable_Error") %>");
                    }
                },
                error: function (xhr) {
                    kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                    msgboxError("<%= resBundle2.GetString("PBPPlugin", "User_Disable_Title") %>", "<%= resBundle2.GetString("PBPPlugin", "User_Disable_Error") %>");
                }
            });

            }

</script>
