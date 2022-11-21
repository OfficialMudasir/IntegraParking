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

    var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    var resBundleModel = ResourceBundle.GetInstance(ModelData.Definition.ResourcesAssemblyName);

    var AddAllowed = (ModelData.Access >= AccessLevel.Admin);
    
%>    

<div class="gridFilterLeft">
    <% if (AddAllowed) { %>
        <span class="<%: ModelId%> k-grid-add tbrAdd" href="#" title="<%= resBundle.GetString("Maintenance", "Messages.Add.Title", "Add new register") %>">
            <span class="k-icon k-add">  </span><%= resBundle.GetString("Maintenance", "Messages.Add.Button", "Add") %>
        </span>
    <% } %>

    <span class="tooltip<%: ModelId%> tbrProfiles tooltipProfiles" 
          title="<%= resBundle.GetString("Maintenance", "Messages.Profile.Title", "Profiles") %>">
        <!--<b><span id="span<%: ModelId%>_Profile"><%: (ModelData.CurrentProfile != null ? ModelData.CurrentProfile.Name : "") %></span></b>
        <a id="a<%: ModelId%>_Profile" class="k-icon k-i-custom "
            href="#" onclick="return false;"
            title=""
            >        
        </a>-->
        <span class="k-icon profiles16-icon"></span><%= resBundle.GetString("Maintenance", "Messages.Profile.Title", "Profiles") %>
    </span>

    <span class="tooltip<%: ModelId%> tbrFilter tooltipFilter"
        title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Title", "Filters") %>" >
        <input id="<%: ModelId%>_HdnFilterInfoTitle" type="hidden" value="<%= resBundle.GetString("Maintenance", "Messages.FilterInfo.Title") %>" />
        <!--<a id="<%: ModelId%>_FilterInfo2" class="imageToolbar filterDisabled k-icon "
            href="#" onclick="return false;"
            title="">        
        </a>-->
        <span>
            <span id="<%: ModelId%>_FilterInfo" class="k-icon filterDisabled"></span><%= resBundle.GetString("Maintenance", "Messages.Filter.Title", "Filters") %>
        </span>
    </span>

    <!--<div class="lvProfilesAdd">-->
        <button type="button" id="btn<%: ModelId %>_SaveProfile" class="k-button k-button-icon <%= (ModelData.ModeAdvanced ? "" : "displayNone") %>"
                onclick="ipsGrid<%: ModelId %>.UpdateStateProfile(ipsGrid<%: ModelId %>.currentProfileId, 'mtbrp<%: ModelId %>_RefreshProfiles()'); return false;"
                title="<%= resBundle.GetString("Maintenance", "Messages.ProfilesPanel.SaveProfile.Button.Title", "Save profile") %>" >
            <span class="k-icon save16-icon"></span>
        </button>
        <button type="button" id="btn<%: ModelId %>_AddProfile" class="k-button k-button-icon <%= (ModelData.ModeAdvanced ? "" : "displayNone") %>" 
                onclick="mtbrp<%: ModelId %>_AddProfile(); return false;"
                title="<%= resBundle.GetString("Maintenance", "Messages.ProfilesPanel.AddProfile.Button.Title", "Add new profile") %>">
            <span class="k-icon k-add"></span>
        </button>        
        <%= Html.Kendo().AutoComplete()
            .Name("txt" + ModelId + "_AddProfile")
            .DataTextField("Name")                              
            .Filter("contains")
            .MinLength(3)
            .HtmlAttributes(new { style = "visibility: hidden;", @class = (ModelData.ModeAdvanced ? "" : "displayNone") })
            .DataSource(source => {
                source.Read(read =>
                {
                    read.Action("GetFindProfiles", "Maintenance", new { plugin = "MaintenancePlugin", modelName = ModelName })
                        .Data("mtbr" + ModelId + "_AddProfileOnAdditionalData");
                })
                .ServerFiltering(true);
            })            
            .Events(e => {
                //e.Select("FindUnitOnSelect");
            })
        %>
    <!--</div>-->

    <a id="a<%: ModelId%>ExportXls" class="<%: ModelId%> export imageToolbar exportXls k-icon " 
        href="<%=Url.Action("Export", "Maintenance", new { /*page = 1, pageSize = "~",*/ filter = "~", sort = "~", modelName = ModelName, columns = "~", format = "xls", plugin = "MaintenancePlugin" }) %>" 
        onclick="mtbr<%: ModelId %>_Redirect(this.id); return false;"
        title="<%= resBundle.GetString("Maintenance", "Messages.ExportXls.Title") %>" >
    </a> 

    <a class="<%: ModelId%> export imageToolbar exportPdf k-icon " 
        href="<%=Url.Action("Export", "Maintenance", new { /*page = 1, pageSize = "~",*/ filter = "~", sort = "~", modelName = ModelName, columns = "~", format = "pdf", plugin = "MaintenancePlugin" }) %>" 
        title="<%= resBundle.GetString("Maintenance", "Messages.ExportPdf.Title") %>" >
    </a> 

</div>
<div id="div<%: ModelId%>_Toolbar" class="gridFilterRight">
    
    <h3><%= resBundle.GetString("Maintenance", "Messages.Toolbar.Title", "VISTA:") %></h3>
    <h2><span id="span<%: ModelId %>_CurrentProfile" class="<%: ModelId %>_CurrentProfileName"><%= (ModelData.CurrentProfile != null ? ModelData.CurrentProfile.Name : "") %></span></h2>

    <span id="grid<%: ModelId%>FilterText"></span>
        
    <!--
    <a id="clearState" class="imageToolbar clear k-icon " 
        href="#" onclick="$.cookie(ipsGrid<%: ModelId%>.cookieStateName, null); location.reload(); return false;"
        title="<%= resBundle.GetString("Maintenance", "Messages.ClearState.Title") %>">
    </a> 
    -->
    
</div>

<% Html.RenderPartial(RouteConfig.BasePath + "Views/Shared/MaintenanceToolbarProfiles.ascx", Model); %>
<% Html.RenderPartial(RouteConfig.BasePath + "Views/Shared/MaintenanceToolbarFilters.ascx", Model); %>

<script>

    var Access = 0;

    $(document).ready(function () {

        //ipsGrid<%: ModelId%>.events.onDataBound = function (e) {
            //mtbr<%: ModelId%>_RefreshFilter();
        //};

        $("#btn<%: ModelId%>_SaveProfile").kendoButton({
            enable: false
        });

    });

    function mtbr<%: ModelId%>_RefreshGrid() {
        ipsGrid<%: ModelId%>.LoadState();
        mtbrf<%: ModelId%>_RefreshGridPending(false);        
    }

    function mtbr<%: ModelId %>_Redirect(hrefId) {

        var bRet = false;
        var ipsGrid = ipsGrid<%: ModelId%>;
        if (ipsGrid != null) {

            bRet = ipsGrid.CheckProfileChanges("mtbr<%: ModelId %>_RedirectConfirm('" + hrefId + "')");

            /*var bChanged = ipsGrid.ProfileChanged();
            app_setBeforeUnload(false);
            window.location.href = $("#" + hrefId).attr('href');
            if (bChanged) app_setBeforeUnload(true);*/
            
        }        
        return bRet;
    }
    function mtbr<%: ModelId %>_RedirectConfirm(hrefId) {
        app_setBeforeUnload(false);
        window.location.href = $("#" + hrefId).attr('href');
    }

    function mtbr<%: ModelId %>_AddProfileOnAdditionalData() {
        return {
            find: $("#txt<%: ModelId %>_AddProfile").val()
        };
    }

</script>