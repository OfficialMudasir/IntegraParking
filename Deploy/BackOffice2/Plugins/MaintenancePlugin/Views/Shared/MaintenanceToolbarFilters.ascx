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

<link href="<%= Url.Content(RouteConfig.BasePath + "Content/filters.css?v2.0") %>" rel="stylesheet" type="text/css" />

<%
    var ModelId = Model.ModelId;
    var ModelName = Model.MaintenanceData.Name;
    var ModelData = Model.MaintenanceData;

    var resBundle = ResourceBundle.GetInstance("MaintenancePlugin");
    var resBundleModel = ResourceBundle.GetInstance(ModelData.Definition.ResourcesAssemblyName);
    
%>    

<%:Html.Kendo().Tooltip()    
    //.For("#div" + ModelId + "_Toolbar")    
    .For(".tooltip" + ModelId + ".tooltipFilter")
    //.Filter(".tooltipFilter")
    .ContentTemplateId("template" + ModelId + "_FilterTooltip")
    //.ContentHandler("LoadTooltip")
    .Position(TooltipPosition.Bottom)    
    .AutoHide(false)
    .ShowOn(TooltipShowOnEvent.Click)
    .Events(ev =>
        ev.Show("mtbrf" + ModelId + "_LoadFilter")
     )
    .Width(400)
    .Height(200)    
%>

<script id="template<%: ModelId %>_FilterTooltip" type="text/x-kendo-template">
    
    <!--<p>#=target.data('title')#</p>-->
    <br/>
    <div class="treeFilterActions">
        <div class="filterActions">
            <button type="button" id="btn<%: ModelId %>_RefreshFilter" class="k-button k-button-icon btnRefreshFilter" 
                    onclick="mtbrf<%: ModelId%>_RefreshFilter(true); return false;"
                    title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Refresh.Button.Title", "Refresh filters") %>">
                <span class="k-icon k-i-refresh"></span>
            </button>
            <button type="button" id="btn<%: ModelId %>_DeleteFilter" class="k-button k-button-icon btnDeleteFilter <%= (ModelData.ModeAdvanced ? "" : "displayNone") %>" 
                    onclick="mtbrf<%: ModelId %>_ClearFilter(); return false;"
                    title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Delete.Button.Title", "Delete all filters") %>">
                <span class="k-icon k-delete"></span>
            </button>

            <button type="button" id="btn<%: ModelId %>_AddFilter" class="k-button k-button-icon btnAddFilter <%= (ModelData.ModeAdvanced ? "" : "displayNone") %>" 
                    onclick="mtbrf<%: ModelId %>_FilterAdd('', 'and'); return false;"
                    title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Add.Button.Title", "Add filter") %>">
                <span class="k-icon k-add"></span>
            </button>

            <button type="button" id="btn<%: ModelId %>_ApplyFilter" class="k-button k-button-icon btnApplyFilter" 
                    onclick="mtbr<%: ModelId%>_RefreshGrid(); return false;"
                    title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Apply.Button.Title", "Get data applying filter") %>">
                <span class="k-icon k-i-refresh"></span><span id="lbl<%: ModelId %>_ApplyFilter"><%= resBundle.GetString("Maintenance", "Messages.Filter.Apply.Button", "Apply filter") %></span>
            </button>
        </div>
        <div class="filterCurrentProfile">
            <h3><%= resBundle.GetString("Maintenance", "Messages.Filter.CurrentProfile.Title", "Profile filters:") %></h3>
            <h2><span class="<%: ModelId %>_CurrentProfileName"><%= (ModelData.CurrentProfile != null ? ModelData.CurrentProfile.Name : "") %></span></h2>
        </div>        
    </div>

    <div id="tree<%: ModelId %>_Filter" class="treeFilter"></div>

    <ul id="contextmenu<%: ModelId %>_FilterLogic" style="z-index: 10003;">
    </ul>

    <ul id="contextmenu<%: ModelId %>_FilterValue" style="z-index: 10003;">
    </ul>


</script>

<script id="template<%: ModelId %>_FilterTreeNode" type="text/x-kendo-template">
    
    <div class="FilterTreeNode #: (item.hasChildren ? 'filterLogic' : 'filterValue') #" path="#= item.id.path #">

        <div class="FilterTreeNodeLeft #: (item.hasChildren ? 'filterLogic' : 'filterValue') #">
            
            <!--<span # if (item.hasChildren) { # class='filterLogic' # } else { # class='filterValue' # } # path='#= item.id.path #'>#= item.text #</span>-->
            <!--<span class="#: (item.hasChildren ? 'filterLogic' : 'filterValue') #" path="#= item.id.path #">#= item.text #</span>-->

            # if (item.hasChildren) { #
                    
                <!--<span path="#= item.id.path #">#= item.text #</span>-->

                <input id="ddlLogics<%: ModelId %>_#=item.id.pathId#" class="FilterTreeNode-ddlLogics" />
                <script>
                $("\#ddlLogics<%: ModelId %>_#=item.id.pathId#").kendoDropDownList({
                    dataTextField: "name",
                    dataValueField: "id",
                    dataSource: [
                        { id: "and", name: "<%= resBundle.GetString("Maintenance", string.Format("Messages.Filter.Logic.{0}", Enum.GetName(typeof(Kendo.Mvc.FilterCompositionLogicalOperator), Kendo.Mvc.FilterCompositionLogicalOperator.And)), "And") %>"},
                        { id: "or", name: "<%= resBundle.GetString("Maintenance", string.Format("Messages.Filter.Logic.{0}", Enum.GetName(typeof(Kendo.Mvc.FilterCompositionLogicalOperator), Kendo.Mvc.FilterCompositionLogicalOperator.Or)), "Or") %>" }
                    ],
                    value: "#=item.id.logic#",
                    select: function (e) { mtbrf<%: ModelId %>_FilterLogicOnSelect2(e, '#=item.id.pathId#'); }  
                });
                <\/script>
                </div>

                <div class="FilterTreeNodeRight filterLogic">
                       
                    <div class="<%= (ModelData.ModeAdvanced ? "" : "displayNone") %>">             
                        <span class="k-icon k-link addFilterAnd-icon" 
                              onclick="mtbrf<%: ModelId %>_FilterAdd('#=item.id.pathId#', 'and'); return false;"
                              title="<%= resBundle.GetString("Maintenance", "Filter.Menu.AddFilter.And", "Add 'and' filter") %>"></span>
                        <span class="k-icon k-link addFilterOr-icon" 
                              onclick="mtbrf<%: ModelId %>_FilterAdd('#=item.id.pathId#', 'or'); return false;"
                              title="<%= resBundle.GetString("Maintenance", "Filter.Menu.AddFilter.Or", "Add 'or' filter") %>"></span>
                        <span class="k-icon k-link k-delete" 
                              onclick="mtbrf<%: ModelId %>_DeleteFilter('#=item.id.path#'); return false;"
                              title="<%= resBundle.GetString("Maintenance", "Filter.Menu.DeleteFilter", "Delete filter") %>"></span>
                    </div>
            
            # } else { #
        
                <input id="hdnLogics<%: ModelId %>_#=item.id.pathId#" type="hidden" value="#=item.id.logic#" />

                <input id="cmbFilterFields<%: ModelId %>_#=item.id.pathId#" class="FilterTreeNode-cmbFilterFields" />
                <script>
                $("\#cmbFilterFields<%: ModelId %>_#=item.id.pathId#").kendoComboBox({
                    dataTextField: "Name",
                    dataValueField: "Mapping",
                    dataSource: <%= ModelData.GetFieldsJson() %>,
                    value: "#=item.id.field#",
                    select: function (e) { mtbrf<%: ModelId %>_FilterFieldOnSelect2(e, '#=item.id.pathId#'); }
                });
                <\/script>
                </div>

                <div class="FilterTreeNodeRight filterValue">
                <div style="float:left;">
                <%        
                    foreach (MaintenanceFieldDataModel oField in ModelData.Fields)
                    {
                        %>
                        <div id="div<%: ModelId %>_#:item.id.pathId#_<%: oField.Mapping %>_Filter" class="filters<%: ModelId %>_#:item.id.pathId# filter-field2" style="display: #: (item.id.field == '<%: oField.Mapping %>' ? '' : 'none') #;">

                            <%=
                                oField.Definition.GetFilterControlTemplate(Html, ModelId, "item.id.pathId")
                            %>
                        </div>
                        <script>
                            $("\#div<%: ModelId %>_#:item.id.pathId#_<%: oField.Mapping %>_Filter").kendoValidator();
                        <\/script>
                        <%
                    }
                 %>
                </div>
                <div style="float:right;" class="<%= (ModelData.ModeAdvanced ? "" : "displayNone") %>">
                    <span class="k-icon k-link addFilterAnd-icon" 
                            onclick="mtbrf<%: ModelId %>_FilterAdd('#=item.id.pathId#', 'and'); return false;"
                            title="<%= resBundle.GetString("Maintenance", "Filter.Menu.InsertFilter.And", "Insert 'and' filter") %>"></span>
                    <span class="k-icon k-link addFilterOr-icon" 
                            onclick="mtbrf<%: ModelId %>_FilterAdd('#=item.id.pathId#', 'or'); return false;"
                            title="<%= resBundle.GetString("Maintenance", "Filter.Menu.InsertFilter.Or", "Insert 'or' filter") %>"></span>
                    <span class="k-icon k-link k-delete" 
                            onclick="mtbrf<%: ModelId %>_DeleteFilter('#=item.id.path#'); return false;"
                            title="<%= resBundle.GetString("Maintenance", "Filter.Menu.DeleteFilter", "Delete filter") %>"></span>
                </div>

            # } #

        </div>

    </div>

</script>


<script>

    $(document).ready(function () {

        var dlg = $("#dlg<%: ModelId %>_Filter");
        if (!dlg.data("kendoWindow")) {
            dlg.kendoWindow({                
                title: "<%= resBundle.GetString("Maintenance", "Messages.Filter.WindowTitle", "Filter") %>",
                modal: true,
                width: "400px",
                visible: false,
                actions: [
                    //"Pin",
                    "Minimize",
                    "Maximize",
                    "Close"
                ],
                open: function () {
                    this.saveFilter = false;
                },
                close: function (e) {
                    if (e.userTriggered) this.saveFilter = false;
                    if (!mtbrf<%: ModelId %>_FilterConfirm(this.saveFilter)) {
                        e.preventDefault();
                    }
                }
            });
        }

        <%  foreach (MaintenanceFieldDataModel oField in ModelData.Fields)
            { %>
                //$("#div<%: ModelId %>_<%: oField.Mapping %>_Filter").kendoValidator();
                $(".filter-field").kendoValidator();                
        <%  } %>        

    });

    function mtbrf<%: ModelId%>_ShowFiltersTooltip() {
        var tooltipFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip");
        if (tooltipFilter != null) tooltipFilter.show($(".tooltipFilter"));
    }

    function mtbrf<%: ModelId%>_LoadFilter(e) {
        kendo.ui.progress($(".treeFilterActions"), true);        
        setTimeout(mtbrf<%: ModelId%>_LoadFilterConfirm, 50);
    }
    function mtbrf<%: ModelId%>_LoadFilterConfirm() {

        var tooltipProfiles = $(".tooltip<%: ModelId%>.tooltipProfiles").data("kendoTooltip");
        if (tooltipProfiles != null) tooltipProfiles.hide();

        var treeFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content.find("#tree<%: ModelId %>_Filter");        

        if (treeFilter != null && treeFilter.data("kendoTreeView") == null) {
                        
            var ipsGrid = ipsGrid<%: ModelId%>;
            var treeData = [];            
            var root = ipsGrid.filter.GetTreeNode();
            if (root != null) treeData.push(root);
            
            if (root == null)
                $("#btn<%: ModelId %>_AddFilter").show();
            else
                $("#btn<%: ModelId %>_AddFilter").hide();

            var dataSource = new kendo.data.DataSource({
                data: treeData
            });
            
            treeFilter.kendoTreeView({
                checkboxes: {
                    checkChildren: true,
                    template:"# if(!item.hasChildren){# <input type='checkbox'  name='checkedFilters_<%: ModelId %>[#= item.id.pathId #]' value='true' # if (item.checked) {# checked='true' #}# onchange='mtbrf<%: ModelId %>_CheckFilter(this)' />#}#"
                    //template: kendo.template($("#template<%: ModelId %>_FilterTreeNode").html())
                },
                dataSource: treeData,
                //template: "<span # if (item.hasChildren) { # class='filterLogic' # } else { # class='filterValue' # } # path='#= item.id.path #'>#= item.text #</span>"
                template: kendo.template($("#template<%: ModelId %>_FilterTreeNode").html())                
            });
            
            treeFilter.attr("style", "min-height: " + (treeFilter.parent().parent().height() - 67) + "px !important");
            
            dataSource.read();
            
            /*var contextMenuFilterLogic = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content.find("#contextmenu<%: ModelId %>_FilterLogic");
            contextMenuFilterLogic.kendoExtContextMenu({
                width: "175px",
                targets: "#tree<%: ModelId %>_Filter .k-item .filterLogic",
                items: [
                    {
                        id: "addFilter",
                        text: "<%= resBundle.GetString("Maintenance", "Filter.Menu.AddFilter", "Add filter") %>",
                        iconCss: "k-add"
                    },
                    {
                        id: "editFilter",
                        text: "<%= resBundle.GetString("Maintenance", "Filter.Menu.EditFilter", "Edit filter") %>",
                        iconCss: "k-edit"
                    },
                    {
                        separator: true
                    },
                    {
                        id: "deleteFilter",
                        text: "<%= resBundle.GetString("Maintenance", "Filter.Menu.DeleteFilter", "Delete filter") %>",
                        iconCss: "k-delete"
                    }
                ],
                itemSelect: function (e) {
                    mtbrf<%: ModelId %>_MenuFilter($(e.item).attr("id"), $(e.target).attr("path"));
                }
            });

            var contextMenuFilterValue = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content.find("#contextmenu<%: ModelId %>_FilterValue");            
            contextMenuFilterValue.kendoExtContextMenu({
                width: "175px",
                targets: "#tree<%: ModelId %>_Filter .k-item .filterValue",
                items: [
                    {
                        id: "editFilter",
                        text: "<%= resBundle.GetString("Maintenance", "Filter.Menu.EditFilter", "Edit filter") %>",
                        iconCss: "k-edit"
                    },
                    {
                        separator: true
                    },
                    {
                        id: "deleteFilter",
                        text: "<%= resBundle.GetString("Maintenance", "Filter.Menu.DeleteFilter", "Delete filter") %>",
                        iconCss: "k-delete"
                    }
                ],
                itemSelect: function (e) {

                    mtbrf<%: ModelId %>_MenuFilter($(e.item).attr("id"), $(e.target).attr("path"));

                }
            });*/

            treeFilter.data("kendoTreeView").checkEventEnabled = false;

            treeFilter.data("kendoTreeView").dataSource.bind("change", function() {

                if (treeFilter.data("kendoTreeView").checkEventEnabled == true) {

                    var ipsGrid = ipsGrid<%: ModelId%>;
                
                    var checkedFilters = [];
                    ipsGrid.filter.GetTreeCheckedFilters(treeFilter.data("kendoTreeView").dataSource.view(), checkedFilters);                    
                    

                    ipsGrid.filter.SetFiltersEnabled(null, false);
                    for (var i=0; i<checkedFilters.length; i++) {
                        ipsGrid.filter.SetFiltersEnabled(null, true, checkedFilters[i]);
                    }

                    treeFilter.data("kendoTreeView").checkEventEnabled = false;

                    //ipsGrid.LoadState();
                    mtbrf<%: ModelId%>_RefreshGridPending();
                }
            });

            mtbrf<%: ModelId%>_RefreshGridPending(false);
        }
        //else
        //    mtbr<%: ModelId%>_RefreshFilter();

        kendo.ui.progress($(".treeFilterActions"), false);

    }

    function mtbrf<%: ModelId%>_RefreshFilter(bRefreshGrid) {
        kendo.ui.progress($(".treeFilterActions"), true);
        setTimeout(mtbrf<%: ModelId%>_RefreshFilterConfirm, 50, bRefreshGrid);
    }
    function mtbrf<%: ModelId%>_RefreshFilterConfirm(bRefreshGrid) {

        var content = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content;
        if (content != null) {
            var treeview = content.find("#tree<%: ModelId %>_Filter").data("kendoTreeView");        
            if (treeview != null) {

                var ipsGrid = ipsGrid<%: ModelId%>;
                var treeData = [];            
                var root = ipsGrid.filter.GetTreeNode();
                if (root != null) treeData.push(root);

                if (root == null)
                    $("#btn<%: ModelId %>_AddFilter").show();
                else
                    $("#btn<%: ModelId %>_AddFilter").hide();

                treeview.dataSource.data(treeData);
            
                //treeview.refresh();
                if (bRefreshGrid == null) bRefreshGrid = false;
                if (bRefreshGrid == true) {
                    mtbr<%: ModelId%>_RefreshGrid();
                }
            }
        }

        kendo.ui.progress($(".treeFilterActions"), false);
        
    }

    var b<%: ModelId%>_RefreshGridPending = false;

    function mtbrf<%: ModelId%>_RefreshGridPending(bShow) {
        if (bShow == null) bShow = true;
        if (bShow) {
            b<%: ModelId%>_RefreshGridPending = true;
            mtbrf<%: ModelId%>_RefreshGridPendingBlink();            
        }
        else {
            b<%: ModelId%>_RefreshGridPending = false;            
        }
    }
    function mtbrf<%: ModelId%>_RefreshGridPendingBlink() {
        if (b<%: ModelId%>_RefreshGridPending)
            $("#lbl<%: ModelId %>_ApplyFilter").fadeTo(500, 0.1).fadeTo(1000, 1.0, mtbrf<%: ModelId%>_RefreshGridPendingBlink);

    }

    function mtbrf<%: ModelId%>_ClearFilter() {
        ipsGrid<%: ModelId %>.ClearFilter();
        mtbrf<%: ModelId%>_RefreshFilter();
        mtbrf<%: ModelId%>_RefreshGridPending();
    }

    function mtbrf<%: ModelId %>_CheckFilter(chk) {

        var treeFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content.find("#tree<%: ModelId %>_Filter");        
        if (treeFilter != null)
            treeFilter.data("kendoTreeView").checkEventEnabled = true;

    }

    function mtbrf<%: ModelId %>_MenuFilter(menuOption, selectedPath) {

        var curId = null;
        var tree = $(".tooltip<%: ModelId %>.tooltipFilter").data("kendoTooltip").content.find("#tree<%: ModelId %>_Filter").data("kendoTreeView");
        if (tree != null) {
            var itemSelected = tree.dataItem("[path='" + selectedPath + "']");
            if (itemSelected) {
                //alert(itemSelected.id + " " + itemSelected.id.path);
                curId = itemSelected.id;
            }
        }

        if (menuOption == "addFilter") {
            mtbrf<%: ModelId %>_ShowFilter(curId.path, curId.logic, null, null, null, true);
        }
        else if (menuOption == "editFilter") {
            mtbrf<%: ModelId %>_ShowFilter(curId.path, curId.logic, curId.field, curId.operator, curId.value);
        }
        else if (menuOption == "deleteFilter") {
            mtbrf<%: ModelId %>_DeleteFilter(curId.path);
        }

    }

    function mtbrf<%: ModelId %>_AddFilter(selectedNodePath) {

        mtbrf<%: ModelId %>_ShowFilter(selectedNodePath);

    }

    function mtbrf<%: ModelId %>_ShowFilter(selectedNodePath, logic, field, operator, value, addMode) {

        $("#hdn<%: ModelId %>_FilterPath").val(selectedNodePath);
        var bAdd = ((logic==null && field == null) || addMode);
        $("#hdn<%: ModelId %>_FilterAdd").val((bAdd?"1":"0"));

        if (!bAdd && field == null)
            $("#div<%: ModelId %>_FilterContainer").hide();
        else
            $("#div<%: ModelId %>_FilterContainer").show();

        var ddlLogics = $("#ddlLogics<%: ModelId %>").data("kendoDropDownList");
        if (ddlLogics != null) {
            if (logic == null) logic = "and";
            ddlLogics.select(function(dataItem) {
                return dataItem.id === logic;
            });
            ddlLogics.enable(field == null);
        }

        var cmbFields = $("#cmb<%: ModelId %>_FilterFields").data("kendoComboBox");
        if (cmbFields != null) {
            $(".filters<%: ModelId %>.filter-field").hide();
            if (field != null) {
                cmbFields.select(function(dataItem) {
                    return dataItem.Mapping === field;
                });                
                $("#div<%: ModelId %>_" + field + "_Filter").show();
            }
            else {
                cmbFields.value(null);                
            }
        }

        var ddlOperators = $("#ddlOperators<%: ModelId %>_" + field).data("kendoDropDownList");
        if (ddlOperators != null) {

            var datePicker = $("#" + field).data("kendoDateTimePicker");
            if (datePicker == null) datePicker = $("#" + field).data("kendoDatePicker");
            if (datePicker != null) datePicker.enable();
            
            if (operator != null) {
                ddlOperators.select(function(dataItem) {
                    return dataItem.id === operator;
                });
                if (datePicker != null) {
                    if (operator == "lastweek" || operator == "lastmonth" || operator == "lastyear" ||
                        operator == "currentweek" || operator == "currentmonth" || operator == "currentyear" || operator == "currentday") {                        
                        datePicker.enable(false);
                        datePicker.value(null);
                    }
                }
            }
            else
                ddlOperators.select(0);
        }

        var cnValue = $("#" + field);
        if (cnValue != null) {
            cnValue.val(value);
            var cmb = cnValue.data("kendoComboBox");
            if (cmb != null) cmb.value(value);
        }

        var dlg = $("#dlg<%: ModelId %>_Filter").data("kendoWindow");
        dlg.center();
        dlg.open();

    }

    function mtbrf<%: ModelId %>_DeleteFilter(selectedNodePath) {

        var ipsGrid = ipsGrid<%: ModelId%>;
        if (ipsGrid != null) {

            ipsGrid.filter.DeleteFilter(selectedNodePath);

            var tooltipFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip");
            if (tooltipFilter != null) tooltipFilter.show($(".tooltipFilter"));

            mtbrf<%: ModelId%>_RefreshFilter();
            //ipsGrid.LoadState();
            mtbrf<%: ModelId%>_RefreshGridPending();            

        }

    }

    function mtbrf<%: ModelId %>_FilterConfirm(save) {

        var bShowTooltip = true;

        if (save) {
            
            var bValid = true;
            if (bValid) {

                var bAdd = ($("#hdn<%: ModelId %>_FilterAdd").val() == "1");
                var selectedPath = $("#hdn<%: ModelId %>_FilterPath").val();
                var filterLogic = "and";
                var filterData = {field: "", operator:"", value:""};

                var ddlLogics = $("#ddlLogics<%: ModelId %>").data("kendoDropDownList");
                if (ddlLogics != null) {
                    var iLogicIndex = ddlLogics.select();
                    if (iLogicIndex > -1) {
                        filterLogic = ddlLogics.dataItem(iLogicIndex).id;
                    }
                    else {
                        // Logic required
                        bValid = false;
                    }
                }
                else {
                    // Invalid logic
                    bValid = false;
                }

                if ($("#div<%: ModelId %>_FilterContainer").css("display") != "none") {

                    var cmbFields = $("#cmb<%: ModelId %>_FilterFields").data("kendoComboBox");
                    if (cmbFields != null) {
                        var iFieldIndex = cmbFields.select();
                        if (iFieldIndex > -1) {
                            filterData.field = cmbFields.dataItem(iFieldIndex).Mapping;

                        }
                        else {
                            // Field required
                            bValid = false;
                        }
                    }
                    else {
                        // Invalid field
                        bValid = false;
                    }

                    var ddlOperators = $("#ddlOperators<%: ModelId %>_" + filterData.field).data("kendoDropDownList");
                    if (ddlOperators != null) {
                        var iOperatorIndex = ddlOperators.select();
                        if (iOperatorIndex > -1) {
                            filterData.operator = ddlOperators.dataItem(iOperatorIndex).id;
                        }
                        else {
                            // Operator required
                            bValid = false;
                        }
                    }
                    else {
                        // Invalid operator
                        bValid = false;
                    }

                    var validator = $("#div<%: ModelId %>_" + filterData.field + "_Filter").data("kendoValidator");                    
                    if (validator.validate()) {
                        var cnValue = $("#" + filterData.field);
                        if (cnValue != null) {
                            if (cnValue.attr("class") != "tri-checkbox")
                                filterData.value = cnValue.val();
                            else
                                filterData.value = (cnValue[0].checked ? 1 : 0);
                            if (filterData.value == null) {
                                // Value required
                                bValid = false;
                            }
                            else {
                                var cmb = cnValue.data("kendoComboBox");
                                if (cmb != null) {
                                    filterData.valueFK = cmb.text();
                                }
                            }
                        }
                        else {
                            // Invalid value
                            bValid = false;
                        }
                    }
                    else
                        bValid = false;
                }
            }

            if (bValid == true) {

                var ipsGrid = ipsGrid<%: ModelId%>;
                if (ipsGrid != null) {

                    if (selectedPath == null || selectedPath == "") {
                        var tree = $(".tooltip<%: ModelId %>.tooltipFilter").data("kendoTooltip").content.find("#tree<%: ModelId %>_Filter").data("kendoTreeView");
                        if (tree != null) {
                            var itemSelected = tree.dataItem(tree.select());                        
                            if (itemSelected) {
                                //alert(itemSelected.id + " " + itemSelected.id.path);
                                selectedPath = itemSelected.id.path;
                            }
                        }
                    }

                    if (bAdd)
                        ipsGrid.filter.AddFilter(filterLogic, filterData, selectedPath);
                    else
                        ipsGrid.filter.EditFilter(filterLogic, filterData, selectedPath);

                    mtbrf<%: ModelId%>_RefreshFilter();
                    //ipsGrid.LoadState();                    
                    mtbrf<%: ModelId%>_RefreshGridPending();

                }

            }
            else {
                bShowTooltip = false;
            }

        }
        
        if (bShowTooltip) {
            var tooltipFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip");
            if (tooltipFilter != null) tooltipFilter.show($(".tooltipFilter"));
        }

        return bShowTooltip;
    }

    function mtbrf<%: ModelId %>_FilterUpdate(filterId, filterLogic, filterField, filterOperator, filterValue, filterValueFK) {

        var bValid = true;

        var ipsGrid = ipsGrid<%: ModelId%>;

        var bAdd = false; //($("#hdn<%: ModelId %>_FilterAdd").val() == "1");
        var selectedPath = selectedPath = filterId.split("-").join("/");        
        var filterData = {field: "", operator:"", value:""};


        if (filterLogic == null) {
            filterLogic = "and";
            var ddlLogics = $("#ddlLogics<%: ModelId %>_" + filterId).data("kendoDropDownList");
            if (ddlLogics != null) {
                var iLogicIndex = ddlLogics.select();
                if (iLogicIndex > -1) {
                    filterLogic = ddlLogics.dataItem(iLogicIndex).id;
                }
                else {
                    // Logic required
                    bValid = false;
                }
            }
            else {
                var hdnLogics = $("#hdnLogics<%: ModelId %>_" + filterId);
                if (hdnLogics != null)
                    filterLogic = hdnLogics.val();
                else
                    // Invalid logic
                    bValid = false;
            }
        }
        
        if (filterField == null) {
            var cmbFields = $("#cmbFilterFields<%: ModelId %>_" + filterId);
            if (cmbFields != null) {            
                cmbFields = cmbFields.data("kendoComboBox");
                if (cmbFields != null) {
                    var iFieldIndex = cmbFields.select();
                    if (iFieldIndex > -1) {
                        filterData.field = cmbFields.dataItem(iFieldIndex).Mapping;
                    }
                    else                        
                        bValid = false; // Field required
                }
                //else                     
                //    bValid = false; // Invalid field
            }
            //else
            //    bValid = false;
        }
        else
            filterData.field = filterField;

        if (filterData.field != null && filterData.field != "") {

            if (filterOperator == null) {
                var ddlOperators = $("#ddlOperators<%: ModelId %>_" + filterId + "_" + filterData.field).data("kendoDropDownList");
                if (ddlOperators != null) {
                    var iOperatorIndex = ddlOperators.select();
                    if (iOperatorIndex > -1) {
                        filterData.operator = ddlOperators.dataItem(iOperatorIndex).id;
                    }
                    else                     
                        bValid = false;  // Operator required
                }
                else                 
                    bValid = false; // Invalid operator
            }
            else
                filterData.operator = filterOperator;

            if (filterValue == null) {
                var validator = $("#div<%: ModelId %>_" + filterId + "_" + filterData.field + "_Filter").data("kendoValidator");
                if (validator != null && validator.validate() && ipsGrid != null) {
                    var pref = "";
                    if (ipsGrid.options.fields[filterData.field].FKMaintenanceId == null) {
                        switch (ipsGrid<%: ModelId %>.options.fields[filterData.field].TypeString) {
                            case "Text": {  pref = "txt"; break; }
                            case "DateTime": {  pref = "dtp"; break; }
                            case "Date": {  pref = "dp"; break; }
                            case "Time": {  pref = "tp"; break; }
                            case "Integer": {  pref = "itxt"; break; }
                            case "Float": {  pref = "ftxt"; break; }
                            case "Boolean": {  pref = "chk"; break; }
                        }
                    }
                    else {
                        pref = "cmb";
                    }
                    var cnValue = $("#" + pref + "<%: ModelId %>_" + filterId + "_" + filterData.field);
                    if (cnValue != null) {
                        if (cnValue.attr("class") != "tri-checkbox")
                            filterData.value = cnValue.val();
                        else
                            filterData.value = (cnValue[0].checked ? 1 : 0);
                        if (filterData.value == null) {
                            // Value required
                            bValid = false;
                        }
                        else {
                            var cmb = cnValue.data("kendoComboBox");
                            if (cmb != null) {
                                filterData.valueFK = cmb.text();
                            }
                        }
                    }
                    else {
                        // Invalid value
                        bValid = false;
                    }
                }
                else
                    bValid = false;
            }
            else {
                filterData.value = filterValue;
                if (filterValueFK != null) filterData.valueFK = filterValueFK;
            }

        }

        if (bValid == true) {
            
            if (ipsGrid != null) {

                var bChanged = true;
                if (bAdd)
                    ipsGrid.filter.AddFilter(filterLogic, filterData, selectedPath);
                else
                    bChanged = ipsGrid.filter.EditFilter(filterLogic, filterData, selectedPath);

                //mtbrf<%: ModelId%>_RefreshFilter();
                //ipsGrid.LoadState();                    
                if (bChanged) mtbrf<%: ModelId%>_RefreshGridPending();

                if (bChanged) {
                    //$("input[name='checkedFilters[" + filterId + "]']").is(":checked")
                    $("input[name='checkedFilters_<%: ModelId %>[" + filterId + "]']").prop("checked", true);                    
                    mtbrf<%: ModelId %>_CheckFilter();
                    mtbrf<%: ModelId %>_ChangeCheckFilter();
                }
            }

        }

    }

    function mtbrf<%: ModelId %>_FilterAdd(filterId, filterLogic) {

        var bValid = true;

        var ipsGrid = ipsGrid<%: ModelId%>;
        
        var selectedPath = selectedPath = filterId.split("-").join("/");        
        var filterData = {field: "", operator:"", value:""};

        
        if (filterLogic == null) {
            var ddlLogics = $("#ddlLogics<%: ModelId %>_" + filterId).data("kendoDropDownList");
            if (ddlLogics != null) {
                var iLogicIndex = ddlLogics.select();
                if (iLogicIndex > -1) {
                    filterLogic = ddlLogics.dataItem(iLogicIndex).id;
                }
                else {
                    // Logic required
                    bValid = false;
                }
            }
            else {
                var hdnLogics = $("#hdnLogics<%: ModelId %>_" + filterId);
                if (hdnLogics != null && hdnLogics.length > 0)
                    filterLogic = hdnLogics.val();
                //else
                // Invalid logic
                //bValid = false;
            }
        }
        
        filterData.field = ipsGrid.options.fields[Object.keys(ipsGrid.options.fields)[0]].Mapping;
        filterData.operator = "eq";
        filterData.value = "";


        if (bValid == true) {
            
            if (ipsGrid != null) {

                ipsGrid.filter.AddFilter(filterLogic, filterData, selectedPath);

                mtbrf<%: ModelId%>_RefreshFilter();
                //ipsGrid.LoadState();                    
                mtbrf<%: ModelId%>_RefreshGridPending();

            }

        }

    }

    function mtbrf<%: ModelId %>_FilterLogicOnSelect2(e, filterId) {        
        var dataItem = e.sender.dataItem(e.item.index());
        if (dataItem != null) {
            mtbrf<%: ModelId %>_FilterUpdate(filterId, dataItem.id, null, null, null, null);
        }
    }

    function mtbrf<%: ModelId %>_FilterFieldOnSelect(e) {        
        var dataItem = this.dataItem(e.item.index());
        if (dataItem != null) {
            $(".filters<%: ModelId %>.filter-field").hide();        
            $("#div<%: ModelId %>_" + dataItem.Mapping + "_Filter").show();
        }
    }

    function mtbrf<%: ModelId %>_FilterFieldOnSelect2(e, filterId) {        
        var dataItem = e.sender.dataItem(e.item.index());
        if (dataItem != null) {
            $(".filters<%: ModelId %>_" + filterId + ".filter-field2").hide();
            $("#div<%: ModelId %>_" + filterId + "_" + dataItem.Mapping + "_Filter").show();

            mtbrf<%: ModelId %>_FilterUpdate(filterId, null, dataItem.Mapping, null, null, null);
        }
    }

    function mtbrf<%: ModelId %>_FilterOperatorOnSelect(e) {        
        var dataItem = this.dataItem(e.item.index());
        if (dataItem != null) {
            var ddlId = e.item[0].parentNode.id;
            var field = ddlId.split("_")[1];

            var datePicker = $("#" + field).data("kendoDateTimePicker");
            if (datePicker == null) datePicker = $("#" + field).data("kendoDatePicker");
            if (datePicker != null) {
                if (dataItem.id == "lastweek" || dataItem.id == "lastmonth" || dataItem.id == "lastyear" ||
                    dataItem.id == "currentweek" || dataItem.id == "currentmonth" || dataItem.id == "currentyear" || dataItem.id == "currentday") {                    
                    datePicker.enable(false);
                    datePicker.value(null);
                }
                else {
                    datePicker.enable();
                }
            }
        }
    }

    function mtbrf<%: ModelId %>_FilterOperatorOnSelect2(e, filterId) {        
        var dataItem = e.sender.dataItem(e.item.index());
        if (dataItem != null) {
            var field = "dtp<%: ModelId %>_" + filterId + "_" + e.item[0].parentNode.id.split("_")[2]; 

            var datePicker = $("#" + field).data("kendoDateTimePicker");
            if (datePicker == null) datePicker = $("#" + field).data("kendoDatePicker");
            if (datePicker != null) {
                if (dataItem.id == "lastweek" || dataItem.id == "lastmonth" || dataItem.id == "lastyear" ||
                    dataItem.id == "currentweek" || dataItem.id == "currentmonth" || dataItem.id == "currentyear" || dataItem.id == "currentday") {                    
                    datePicker.enable(false);
                    datePicker.value(null);
                }
                else {
                    datePicker.enable();
                }
            }

            mtbrf<%: ModelId %>_FilterUpdate(filterId, null, null, dataItem.id, null, null);
        }
    }

    function mtbrf<%: ModelId %>_FilterCmbValueOnSelect2(e, filterId) {
        var dataItem = e.sender.dataItem(e.item.index());
        if (dataItem != null) {
            var ValueId = dataItem[$("#" + e.item[0].parentNode.id.replace("_listbox","")).attr("valuefield")];
            var ValueText = dataItem[$("#" + e.item[0].parentNode.id.replace("_listbox","")).attr("textvalue")];
            mtbrf<%: ModelId %>_FilterUpdate(filterId, null, null, null, ValueId, ValueText);
        }
    }

    function mtbrf<%: ModelId %>_ChangeCheckFilter() {

        var treeFilter = $(".tooltip<%: ModelId%>.tooltipFilter").data("kendoTooltip").content.find("#tree<%: ModelId %>_Filter");

        treeFilter.data("kendoTreeView").dataSource.data()[0].dirty = true;
        //treeFilter.data("kendoTreeView").dataSource.sync();
    }

</script>

<div id="dlg<%: ModelId %>_Filter">

    <input id="hdn<%: ModelId %>_FilterPath" type="hidden" value="" />
    <input id="hdn<%: ModelId %>_FilterAdd" type="hidden" value="1" />

    <div class="filter-logic-container">
        <%= Html.Kendo().DropDownList()
                .Name(string.Format("ddlLogics{0}", ModelId))
                .DataValueField("id")
                .DataTextField("name")
                .SelectedIndex(0)
                .BindTo(new System.Collections.Generic.List<dynamic>() {
                            new {id = "and", name = resBundle.GetString("Maintenance", string.Format("Messages.Filter.Logic.{0}", Enum.GetName(typeof(Kendo.Mvc.FilterCompositionLogicalOperator), Kendo.Mvc.FilterCompositionLogicalOperator.And)), "And") },
                            new {id = "or", name = resBundle.GetString("Maintenance", string.Format("Messages.Filter.Logic.{0}", Enum.GetName(typeof(Kendo.Mvc.FilterCompositionLogicalOperator), Kendo.Mvc.FilterCompositionLogicalOperator.Or)), "Or") }
                        })
                .HtmlAttributes(new { id = string.Format("ddlLogics{0}", ModelId) })        
        %>
    </div>

    <div id="div<%: ModelId %>_FilterContainer" class="filter-container" style="display:none;">
        <div class="filter-label">
            <h3><%= resBundle.GetString("Maintenance", "Messages.Filter.FieldSelect", "Field:") %></h3>
            <%= Html.Kendo().ComboBox()
                  .Name("cmb" + ModelId + "_FilterFields")
                  //.Filter("contains")
                  //.Placeholder("Select fabric...")
                  .DataTextField("Name")
                  .DataValueField("Mapping")          
                  .BindTo(ModelData.GetFields())
                  .HtmlAttributes(new { id = "cmb" + ModelId + "_FilterFields"})
                  .Events(ev => ev.Select("mtbrf" + ModelId + "_FilterFieldOnSelect"))
            %>
        </div>
        <%        
            foreach (MaintenanceFieldDataModel oField in ModelData.Fields)
            {
                %>
                <div id="div<%: ModelId %>_<%: oField.Mapping %>_Filter" class="filters<%: ModelId %> filter-field" style="display: none;">
                    <%=
                        oField.Definition.GetHtmlFilterControl(Html, ModelId)                    
                        %>
                </div>
                <%
            }
         %>
    </div>

    <div class="filter-buttons">
        <button type="button" class="k-button k-button-icon" 
                onclick="$('#dlg<%: ModelId %>_Filter').data('kendoWindow').saveFilter = true; $('#dlg<%: ModelId %>_Filter').data('kendoWindow').close(); return false;"
                title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Accept.Button.Title", "Accept") %>">
            <span class="k-icon k-update"></span><%= resBundle.GetString("Maintenance", "Messages.Filter.Accept.Button", "Accept") %>
        </button>

        <button type="button" class="k-button k-button-icon" 
                onclick="$('#dlg<%: ModelId %>_Filter').data('kendoWindow').saveFilter = false; $('#dlg<%: ModelId %>_Filter').data('kendoWindow').close(); return false;"
                title="<%= resBundle.GetString("Maintenance", "Messages.Filter.Cancel.Button.Title", "Cancel") %>">
            <span class="k-icon k-cancel"></span><%= resBundle.GetString("Maintenance", "Messages.Filter.Cancel.Button", "Cancel") %>
        </button>
    </div>

</div>