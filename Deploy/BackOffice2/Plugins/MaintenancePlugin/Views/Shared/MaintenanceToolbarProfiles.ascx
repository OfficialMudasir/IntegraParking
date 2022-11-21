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

<link href="<%= Url.Content(RouteConfig.BasePath + "Content/profiles.css?v2.0") %>" rel="stylesheet" type="text/css" />

<%
    var ModelId = Model.ModelId;
    var ModelName = Model.MaintenanceData.Name;
    var ModelData = Model.MaintenanceData;

    var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    var resBundleModel = ResourceBundle.GetInstance(ModelData.Definition.ResourcesAssemblyName);    
%>    

<%:Html.Kendo().Tooltip()    
    //.For("#div" + ModelId + "_Toolbar")    
    .For(".tooltip" + ModelId + ".tooltipProfiles")
    //.Filter(".tooltipProfiles")
    .ContentTemplateId("template" + ModelId + "_ProfilesTooltip")
    //.ContentHandler("LoadTooltip")
    .Position(TooltipPosition.Bottom)    
    .AutoHide(false)
    .ShowOn(TooltipShowOnEvent.Click)
    .Events(ev =>
        ev.Show("mtbrp" + ModelId + "_LoadProfiles")
     )
    .Width(400)
    .Height(200)
%>

<script id="template<%: ModelId %>_ProfilesTooltip" type="text/x-kendo-template">
    
    <!--<p>#=target.data('title')#</p>-->
    <br/>
    <div class="divFindProfiles">
        <input id="txt<%: ModelId%>_FindProfile"/>
    </div>

    <div id="lv<%: ModelId %>_Profiles" class="lvProfiles"></div>
    <!--<div id="pagerProfiles" class="k-pager-wrap"></div>-->

</script>

<script id="template<%: ModelId %>_Profile" type="text/x-kendo-tmpl">
    <li style="display:inline;" >
        # if (Id != ipsGrid<%: ModelId %>.currentProfileId) {
            if (ipsGrid<%: ModelId%>.profileAlt) {#
                <div id="li<%: ModelId %>_Profiles_#:Id#" class="liProfile ProfileAlt">
            #} else {#
                <div id="li<%: ModelId %>_Profiles_#:Id#" class="liProfile">
            #}
         } else {#
            <div id="li<%: ModelId %>_Profiles_#:Id#" class="liProfile ProfileSelected">
        #}
        ipsGrid<%: ModelId%>.profileAlt = !ipsGrid<%: ModelId%>.profileAlt;
        #
        
            <div class="liProfileLeft">
                <b><span style="cursor:pointer;" onclick="ipsGrid<%: ModelId %>.LoadProfile(#:Id#, 'mtbrp<%: ModelId %>_SetCurrentProfile()'); return false;">
                        #:Name#
                   </span>
                </b> #:Description# &nbsp; &nbsp;
            </div>
            <div class="liProfileRight">
                # if ("<%: (ModelData.Definition.DefaultProfileMandatory ? "1":"0") %>" == "1") { if (Default == true) { #
                    <a href="" onclick="return false;"
                       class="imageProfilesList profileDefault#:Default#" style="cursor:default;"></a> 
                # } else { #
                    <a href="" onclick="ipsGrid<%: ModelId %>.UpdateDefaultProfile(#:Id#, !#:Default#, 'mtbrp<%: ModelId %>_RefreshProfiles()'); return false;" 
                       class="imageProfilesList profileDefault#:Default#"></a> 
                # }} #
                <a href="" onclick="ipsGrid<%: ModelId %>.UpdatePublicProfile(#:Id#, !#:Public#, 'mtbrp<%: ModelId %>_RefreshProfiles()'); return false;" 
                   class="imageProfilesList profilePublic#:Public#"></a>         
                <a href="" onclick="ipsGrid<%: ModelId %>.UpdateStateProfile(#:Id#, 'mtbrp<%: ModelId %>_RefreshProfiles()'); return false;" 
                   class=" imageProfilesList profileSave" ></a> 
                <a href="" onclick="if (ipsGrid<%: ModelId %>.currentProfileId != #:Id#) { ipsGrid<%: ModelId %>.LoadProfile(#:Id#, 'mtbrp<%: ModelId %>_SetCurrentProfile(true)'); } else { mtbrf<%: ModelId%>_ShowFiltersTooltip(); } return false;" 
                   class=" imageProfilesList profileFilter"
                   title="<%= resBundle.GetString("Maintenance", "Messages.Profile.ShowFilter.Button.Title", "Load profile and show filters") %>" ></a>    
                <a href="" onclick="ipsGrid<%: ModelId %>.DeleteProfile(#:Id#, 'mtbrp<%: ModelId %>_RefreshProfiles()'); return false;" 
                   class=" imageProfilesList profileDelete <%= (ModelData.ModeAdvanced ? "" : "displayNone") %>" 
                   title="<%= resBundle.GetString("Maintenance", "Messages.Profile.Delete.Button.Title", "Delete profile") %>" ></a>
            </div>
        </div>
    </li>
</script>

<script>

    $(document).ready(function () {

        $("#txt<%: ModelId %>_AddProfile").closest(".k-widget").addClass("k-textbox k-space-right")
                                            .append('<span class="k-icon k-i-tick"' +
                                                          'onclick="ipsGrid<%: ModelId %>.AddProfile($(\'\#txt<%: ModelId %>_AddProfile\').val(), \'mtbrp<%: ModelId%>_RefreshProfilesAdd()\'); return false;"' +
                                                          'title="<%= resBundle.GetString("Maintenance", "Messages.ProfilesPanel.AddProfile.Button.Title", "Add new profile") %>"></span>');

    });

    function mtbrp<%: ModelId%>_LoadProfiles(e) {

        var tooltipFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip");
        if (tooltipFilter != null) tooltipFilter.hide();

        //if ($("#div<%: ModelId%>_Toolbar").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").data("kendoListView") == null) {
        if ($(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").data("kendoListView") == null) {
            

            var urlGetProfiles = ipsGrid<%: ModelId%>.options.actions.getProfiles;

            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        type: 'GET',
                        //dataType: "jsonp",
                        url: urlGetProfiles,
                        data: {
                            plugin: 'MaintenancePlugin',
                            modelName: '<%: ModelName%>'
                        }
                    }
                }/*,
                pageSize: 15*/
            });

            /*$("#pagerProfiles").kendoPager({
                dataSource: dataSource
            });*/        
        
            ipsGrid<%: ModelId%>.profileAlt = false;

            //$("#lvProfiles").kendoListView({
            //e.find("#lvProfiles").kendoListView({        
            //$("#div<%: ModelId%>_Toolbar").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").kendoListView({
            $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").kendoListView({
                template: kendo.template($("#template<%: ModelId %>_Profile").html()),
                autoBind: false,
                dataSource: dataSource,
                selectable: false
            });

            var lvProfiles = $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles");
            lvProfiles.attr("style", "min-height: " + (lvProfiles.parent().parent().height() - 67) + "px !important");

            dataSource.read();

            $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content.find("#txt<%: ModelId %>_FindProfile").kendoAutoComplete({
                dataTextField: "Name",
                filter: "contains",
                minLength: 2,
                dataSource: {                
                    serverFiltering: true,
                    transport: {
                        read: {
                            type: 'GET',
                            url: "<%= Url.Action("GetFindProfiles", "Maintenance", new { plugin = "MaintenancePlugin", modelName = ModelName } )%>",
                            data: mtbrp<%: ModelId %>_FindProfileOnAdditionalData
                        }
                    }
                },
                select: mtbrp<%: ModelId %>_FindProfileOnSelect                
            });

            $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content.find("#txt<%: ModelId %>_FindProfile").closest(".k-widget").addClass("k-textbox k-space-right").append('<span class="k-icon k-i-search"></span>');

        }
    }

    function mtbrp<%: ModelId%>_RefreshProfiles() {
        //$("#div<%: ModelId%>_Toolbar").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").data("kendoListView").dataSource.read();
        var tooltipProfiles = $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content;
        if (tooltipProfiles != null) {
            var lvProfiles = tooltipProfiles.find("#lv<%: ModelId %>_Profiles").data("kendoListView");
            if (lvProfiles != null)
                lvProfiles.dataSource.read();
            else
                mtbrp<%: ModelId%>_LoadProfiles();
        }
        mtbrp<%: ModelId %>_SetCurrentProfile();
    }

    function mtbrp<%: ModelId%>_RefreshProfilesAdd() {
        $("#txt<%: ModelId %>_AddProfile").val(""); 
        mtbrp<%: ModelId %>_RefreshProfiles();
    }

    function mtbrp<%: ModelId%>_SetCurrentProfile(bShowFiltersTooltip) {
        try {
            //$("#div<%: ModelId%>_Toolbar").data("kendoTooltip").content.find("#lv<%: ModelId %>_Profiles").find(".ProfileSelected").removeClass("ProfileSelected");
            var tooltipProfiles = $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip").content;
            if (tooltipProfiles != null) tooltipProfiles.find("#lv<%: ModelId %>_Profiles").find(".ProfileSelected").removeClass("ProfileSelected");

            $("#li<%: ModelId %>_Profiles_" + ipsGrid<%: ModelId %>.currentProfileId).addClass("ProfileSelected");

            //$("#span<%: ModelId%>_Profile").html(ipsGrid<%: ModelId %>.currentProfileName);
            //$("#txt<%: ModelId%>_AddProfile").val(ipsGrid<%: ModelId %>.currentProfileName);
            //$("#span<%: ModelId%>_CurrentProfile").html(ipsGrid<%: ModelId %>.currentProfileName);
            $(".<%: ModelId %>_CurrentProfileName").each(function (index) { $(this).html(ipsGrid<%: ModelId %>.currentProfileName); } );

            mtbrf<%: ModelId%>_RefreshFilter();

            if (bShowFiltersTooltip) {                
                mtbrf<%: ModelId%>_ShowFiltersTooltip();
            }

        }
        catch (ex) {
        }
    }


    function mtbrp<%: ModelId %>_FindProfileOnAdditionalData() {
        return {
            find: $("#txt<%: ModelId %>_FindProfile").val()
        };
    }

    function mtbrp<%: ModelId %>_FindProfileOnSelect(e) {        
        var oDataItem = this.dataItem(e.item.index());
        if (oDataItem != null) {
            ipsGrid<%: ModelId %>.LoadProfile(oDataItem.Id, 'mtbrp<%: ModelId %>_SetCurrentProfile()');
        }
    }

    function mtbrp<%: ModelId %>_AddProfile() {
        if ($("#txt<%: ModelId %>_AddProfile").parent().css('visibility') == 'hidden') { 
            $("#txt<%: ModelId %>_AddProfile").parent().css('visibility', ''); 
            $("#txt<%: ModelId %>_AddProfile").css('visibility', ''); 
            $("#btn<%: ModelId %>_AddProfile").addClass('k-state-active'); 
        } 
        else { 
            $("#txt<%: ModelId %>_AddProfile").parent().css('visibility', 'hidden'); 
            $('#txt<%: ModelId %>_AddProfile').css('visibility', 'hidden'); 
            $("#btn<%: ModelId %>_AddProfile").removeClass('k-state-active'); 
        }
    }

</script>
