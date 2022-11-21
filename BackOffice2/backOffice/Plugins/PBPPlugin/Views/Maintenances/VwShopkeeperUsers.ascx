<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MaintenanceViewModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="integraMobile.Domain.Abstract" %>
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
<link href="<%= Url.Content(sAssemblyBasePath + "Content/Maintenance/shopkeeperusers.css?v1.0") %>" rel="stylesheet" type="text/css" />

<script type="text/javascript">

    <%
        var resBundle2 = ResourceBundle.GetInstance(Model.MaintenanceData.Definition.ResourcesAssemblyName);
    %>

    function VwShopkeeperUsers_CmdChangeStatus(userId, username, action ) {
        var msgTitle = "";
        var msgConfirm = "";
        switch (action) {
            case "Approve":                 
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Approve.Title") %>";
                msgConfirm = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Approve.Confirm") %>";
                break;
            case "Refuse": 
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Refuse.Title") %>";
                msgConfirm = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Refuse.Confirm") %>";
                break;
            case "Disable": 
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Disable.Title") %>";
                msgConfirm = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Disable.Confirm") %>";
                break;
        }
        
        msgConfirm = msgConfirm.replace("{0}", username);
        msgboxConfirm(msgTitle, msgConfirm, "VwShopkeeperUsers_CmdChangeStatusConfirm(" + userId + ",'" + action + "')");
    }

    function VwShopkeeperUsers_CmdChangeStatusConfirm(userId, action) {

        kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), true);

        var shopkeeperstatus = <%= (int)ShopKeeperStatus.PendingRequest %>;

        var msgTitle = "";
        var msgError = "";
        switch (action) {
            case "Approve": 
                shopkeeperstatus = <%= (int)ShopKeeperStatus.ShopKeeperUser %>;
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Approve.Title") %>";
                msgError = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Approve.Error") %>";
                break;
            case "Refuse": 
                shopkeeperstatus = <%= (int)ShopKeeperStatus.RegularUser %>;  
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Refuse.Title") %>";
                msgError = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Refuse.Error") %>";
                break;
            case "Disable": 
                shopkeeperstatus = <%= (int)ShopKeeperStatus.RegularUser %>;  
                msgTitle = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Disable.Title") %>";
                msgError = "<%= resBundle2.GetStringJS("PBPPlugin", "VwShopkeeperUsers.Disable.Error") %>";
                break;
        }

        $.ajax({
            type: 'POST',
            url: '<%= Url.Action("ShopkeeperUser_ChangeStatus", "User")%>',
            data: { plugin: 'PBPPlugin', userId: userId, shopkeeperstatus: shopkeeperstatus },
            success: function (response) {

                try {
                    kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                    //var oResponse = JSON.parse(response);
                    //eval("oReponse = " + response);

                    if (response.Result == true) {                            
                        ipsGrid<%: ModelId %>.Refresh();                            
                    }
                    else {
                        msgboxAlert(msgTitle, response.ErrorInfo, "warning");
                    }

                } catch (ex) {
                    msgboxError(msgTitle, msgError);
                }
            },
            error: function (xhr) {
                kendo.ui.progress($("#" + ipsGrid<%: ModelId %>.gridId), false);
                msgboxError(msgTitle, msgError);
            }
        });

    }

</script>
