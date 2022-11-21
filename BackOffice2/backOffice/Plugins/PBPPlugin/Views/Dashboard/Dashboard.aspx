<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardDataModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>
<%@ Import Namespace="PBPPlugin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>
    <%= resBundle.GetString("PBPPlugin", "Dashboard_Title") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/dashboard.css?v1.2") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/Noty/jquery.noty.packaged.min.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");
    </script>

    <% Html.Kendo().Splitter()
       .Name("splMain")
       .Orientation(SplitterOrientation.Vertical)
       .HtmlAttributes(new { style = "height: 760px;" })
       .Panes(mainPanes =>
       {
           mainPanes.Add()
                    .HtmlAttributes(new { id = "splMainTop" })
                    .Size("85px")
                    .Resizable(false)
                    .Collapsible(false)
                    .Content(() =>
                    { %>
                        <div class="pane-top" style="padding-top: 10px;">
                            <table border="0" style="width:100%;">
                                <tr>
                                    <td style="padding-left: 10px;">
                                        <%= Html.LabelFor(m => m.DateGroup)%>
                                    </td>
                                    <td>
                                        <%= Html.Kendo().DropDownList()
                                              .Name("ddlDateGroups")
                                              .HtmlAttributes(new { style = "width: 250px" })
                                              .DataTextField("Name")
                                              .DataValueField("id")
                                              .DataSource(source => {
                                                  source.Read(read =>
                                                  {
                                                      read.Action("GetDateGroupTypes", "Dashboard", new { plugin = "PBPPlugin" });
                                                  }); 
                                              })
                                              .Events(events => events.DataBound("OnDateGroupsDataBound")
                                                                      .Change("OnDateGroupsChange"))
                                        %>
                                        <%= Html.LabelFor(m => m.DateGroupPattern)%>
                                        <%= Html.CheckBoxFor<DashboardDataModel>(m => m.DateGroupPattern, new { id = "chkDateGroupPattern" }) %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.DateFilter)%>
                                        <%= Html.Kendo().DropDownList()
                                                .Name("ddlDateFilters")
                                                .HtmlAttributes(new { style = "width: 175px" })
                                                .DataTextField("Name")
                                                .DataValueField("id")
                                                .DataSource(source => {
                                                    source.Read(read =>
                                                    {
                                                        read.Action("GetDateFilterTypes", "Dashboard", new { plugin = "PBPPlugin" });
                                                    }); 
                                                })
                                                .Events(events => events.DataBound("OnDateFiltersDataBound")
                                                                        .Change("OnDateFiltersChange"))
                                        %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.CustomIniDate)%>
                                        <%= Html.Kendo().DateTimePicker()
                                                .Name("txtDateIni")
                                                .Value(Model.CustomIniDate)
                                                .Max(Model.CustomEndDate)
                                                .Events(e => e.Change("OnDateIniChange"))
                                                .Enable(Model.DateFilter == DateFilterType.custom)
                                        %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.CustomEndDate)%>
                                        <%= Html.Kendo().DateTimePicker()
                                                .Name("txtDateEnd")
                                                .Value(Model.CustomEndDate)
                                                .Min(Model.CustomIniDate)
                                                .Events(e => e.Change("OnDateEndChange"))
                                                .Enable(Model.DateFilter == DateFilterType.custom)
                                        %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.CustomIniTime)%>
                                        <%= Html.Kendo().TimePicker()
                                                .Name("txtTimeIni")
                                                .Format("HH:mm")
                                                .Value(Model.CustomIniTime)
                                                .Max(Model.CustomEndTime)
                                                .Events(e => e.Change("OnTimeIniChange"))                                                
                                        %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.CustomEndTime)%>
                                        <%= Html.Kendo().TimePicker()
                                                .Name("txtTimeEnd")
                                                .Format("HH:mm")
                                                .Value(Model.CustomEndTime)
                                                .Min(Model.CustomIniTime)
                                                .Events(e => e.Change("OnTimeEndChange"))
                                        %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 10px;">
                                        <%= Html.LabelFor(m => m.CurrencyId)%>
                                    </td>
                                    <td>
                                        <%= Html.Kendo().DropDownListFor(m => m.CurrencyId)
                                                    .Name("ddlCurrencies")
                                                    .HtmlAttributes(new { style = "width: 250px" })
                                                    .DataTextField("Name")
                                                    .DataValueField("id")
                                                    .DataSource(source => {
                                                        source.Read(read =>
                                                        {
                                                            read.Action("GetCurrencies", "Dashboard", new { plugin = "PBPPlugin" });
                                                        }); 
                                                    })
                                                    .Events(events => events.Change("OnCurrenciesChange"))
                                        %>
                                    </td>
                                    <td>
                                        <%= Html.LabelFor(m => m.UserInstallation)%>
                                        <%= Html.Kendo().DropDownList()
                                                .Name("ddlUserInstallations")
                                                .HtmlAttributes(new { style = "width: 175px" })
                                                .DataTextField("Name")
                                                .DataValueField("id")
                                                .DataSource(source => {
                                                    source.Read(read =>
                                                    {
                                                        read.Action("GetUserInstallationTypes", "Dashboard", new { plugin = "PBPPlugin" });
                                                    }); 
                                                })
                                                .Events(events => events.DataBound("OnUserInstallationsDataBound")
                                                                        .Change("OnUserInstallationsChange"))
                                        %>
                                    </td>
                                </tr>
                            </table>

                            <div class="editor-container" style="display:none;">
                                <div class="editor-label">                                  
                                    <%= Html.LabelFor(m => m.Installations)%>
                                </div>
                                <div class="editor-field">
                                   <%= Html.Kendo().MultiSelect()
                                           .Name("mslInstallations")
                                           .Placeholder("Seleccione las instalaciones...")
                                           .DataTextField("Name")
                                           .DataValueField("id")
                                           .Filter("contains")
                                           .DataSource(source => {
                                              source.Read(read =>
                                              {
                                                  read.Action("Read_TreeGroups", "Dashboard", new { plugin = "PBPPlugin", id = "" });
                                              })
                                              .ServerFiltering(true);
                                           }).Value(Model.Installations)
                                           .Events(events => events.Change("OnInstallationsChange"))
                                    %>
                                </div>
                            </div>
                            
                        </div>
                    <% });
                       
           mainPanes.Add()
                    .HtmlAttributes(new { id = "splMainContent" })                    
                    .Collapsible(false)
                    .Content(() => { %>
                        <div class="pane-content" style="height: 100%;">                            

                            <% Html.Kendo().Splitter()
                                .Name("splContent")                                            
                                .Orientation(SplitterOrientation.Horizontal)
                                .HtmlAttributes(new { style = "height: 670px;" })
                                .Events(ev =>
                                    ev.Resize("OnSplContentResize")
                                )
                                .Panes(contentPanes =>
                                {
                                    contentPanes.Add()
                                        .HtmlAttributes(new { id = "splContentLeft" })
                                        .Size("200px")
                                        .Collapsible(true)                            
                                        .Content(() => { %>
                                            <div class="pane-content" style="height: 98%;">                                                
                                                <% Html.Kendo().TreeView()
                                                    .Name("treeInstallations")
                                                    .HtmlAttributes(new { @class = "demo-section" })                                        
                                                    .Checkboxes(checkboxes => 
                                                        checkboxes.Name("checkedNodes")
                                                                    .CheckChildren(true)
                                                                    /*.Template("<input type='checkbox' name='checkedNodes' value='#= item.id #' #if(item.checked) { # checked # } # onclick='TreeInstallationsChanged();' />")*/
                                                    )
                                                    //.CheckboxTemplate("<input type='checkbox' name='checkedNodes' value='#= item.id #' #if(item.checked) { # checked # } if(item.indeterminate) { # data-indeterminate # } #/>")
                                                    .DataTextField("Name")                                        
                                                    //.ExpandAll(true)
                                                    .DataSource(dataSource => dataSource
                                                        .Read(read => read
                                                            .Action("Read_TreeGroups", "Dashboard", new { plugin = "PBPPlugin" })
                                                        )                                            
                                                    )
                                                    .Events(events =>
                                                        events.Check("TreeInstallationsOnUnitCheck")
                                                              .DataBound("treeInstallationsOnDataBound")
                                                    )
                                                    .Render();
                                                %>                    

                                            </div>
                                        <% });
    
                                    contentPanes.Add()
                                        .HtmlAttributes(new { id = "splContentCenter" })
                                        //.Size("500px")
                                        .Content(() => { %>
                                            <div id="divNotify"></div>
                                            <div class="pane-charts" style="height: 99%;">

                                                <% Html.Kendo().TabStrip()
                                                    .Name("tabContent")
                                                    .HtmlAttributes(new { style = "height: 100%;" })
                                                    .Events(ev => ev.Select("OnTabContentSelect")
                                                                    .Activate("OnTabContentActivate"))
                                                    .Items(tabstrip =>
                                                    {
                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabToday_Text"))
                                                            .Selected(true)
                                                            .HtmlAttributes(new {id = "tabToday", tabIndex = 0})
                                                            .Content(() => 
                                                            { %>
                                                                <div class="pane-content" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/TodayCharts.ascx", Model); %>
                                                                </div>                                                       
                                                        <% });

                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabOperations_Text"))                                                            
                                                            .HtmlAttributes(new {id = "tabOperations", tabIndex = 1})
                                                            .Content(() => 
                                                            { %>
                                                                <div class="pane-content" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/OperationsCharts.ascx", Model); %>
                                                                </div>                                                       
                                                        <% });
                                                                                                   
                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabRecharges_Text"))
                                                            .HtmlAttributes(new { id = "tabRecharges", tabIndex = 2 })
                                                            .Content(() => { %>
                                                                <div class="pane-charts" style="height: 100%;">                                                        
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/RechargesCharts.ascx", Model); %>
                                                                </div>
                                                        <% });

                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabInscriptions_Text"))
                                                            .HtmlAttributes(new { id = "tabInscriptions", tabIndex = 3 })
                                                            .Content(() => { %>
                                                                <div class="pane-charts" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/InscriptionsCharts.ascx", Model); %>
                                                                </div>
                                                        <% });

                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabInscriptionsPlatform_Text"))
                                                            .HtmlAttributes(new { id = "tabInscriptionsPlatform", tabIndex = 4 })
                                                            .Content(() => { %>
                                                                <div class="pane-charts" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/InscriptionsPlatformCharts.ascx", Model); %>
                                                                </div>
                                                        <% });
                                                        
                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabUserInstallations_Text"))
                                                            .HtmlAttributes(new { id = "tabUsersInstallations", tabIndex = 5 })
                                                            .Content(() => { %>
                                                                <div class="pane-charts" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/UsersInstallationCharts.ascx", Model); %>
                                                                </div>
                                                        <% });

                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabSystemUse_Text"))                                    
                                                            .HtmlAttributes(new {id = "tabSystemUse", tabIndex = 6})
                                                            .Content(() => 
                                                            { %>
                                                                <div class="pane-content" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/SystemUseCharts.ascx", Model); %>
                                                                </div>                                                       
                                                        <% });
                                                           
                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabInvitations_Text"))                                                            
                                                            .HtmlAttributes(new {id = "tabInvitations", tabIndex = 7})
                                                            .Content(() => 
                                                            { %>
                                                                <div class="pane-content" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/InvitationsCharts.ascx", Model); %>
                                                                </div>                                                       
                                                        <% });                                                           
                                   
                                                        tabstrip.Add().Text(resBundle.GetString("PBPPlugin", "Dashboard_TabRechargeCoupons_Text"))                                                            
                                                            .HtmlAttributes(new {id = "tabRechargeCoupons", tabIndex = 8})
                                                            .Content(() => 
                                                            { %>
                                                                <div class="pane-content" style="height: 100%;">
                                                                    <% Html.RenderPartial(RouteConfig.BasePath + "Views/Dashboard/RechargeCouponsCharts.ascx", Model); %>
                                                                </div>                                                       
                                                        <% });
                                                                                                                      
                                                })
                                                .Render();
                                                %>
                                            </div>
                                        
                                    <% });

                            }).Render(); %>

                </div>
                        
    
                <% });
          
       })
       .Render();
     %>

    <script>

        var dashboard = {
            actions: {                
                today_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "All", filters = "filters" })%>",
                parkingstoday_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "Parkings", filters = "filters" })%>",
                extensionstoday_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "Extensions", filters = "filters" })%>",
                refundstoday_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "Refunds", filters = "filters" })%>",
                ticketstoday_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "Tickets", filters = "filters" })%>",
                rechargestoday_read: "<%= Url.Action("Read_Today", "Dashboard", new { opeType = "Recharges", filters = "filters" })%>",
                operationstoday_read: "<%= Url.Action("Read_OperationsToday", "Dashboard", new { filters = "filters" })%>",
                operations_read: "<%= Url.Action("Read_Operations", "Dashboard", new { filters = "filters" })%>",
                operationstotals_read: "<%= Url.Action("Read_OperationsTotals", "Dashboard", new { filters = "filters" })%>",
                operationsavg_read: "<%= Url.Action("Read_OperationsAvg", "Dashboard", new { filters = "filters" })%>",
                operationsavgtotals_read: "<%= Url.Action("Read_OperationsAvgTotals", "Dashboard", new { filters = "filters" })%>",
                recharges_read: "<%= Url.Action("Read_Recharges", "Dashboard", new { filters = "filters" })%>",
                rechargesavg_read: "<%= Url.Action("Read_RechargesAvg", "Dashboard", new { filters = "filters" })%>",
                inscriptions_read: "<%= Url.Action("Read_Inscriptions", "Dashboard", new { filters = "filters" })%>",
                inscriptionsavg_read: "<%= Url.Action("Read_InscriptionsAvg", "Dashboard", new { filters = "filters" })%>",
                inscriptionsPlatform_read: "<%= Url.Action("Read_InscriptionsPlatform", "Dashboard", new { filters = "filters" })%>",
                inscriptionsPlatformTotals_read: "<%= Url.Action("Read_InscriptionsPlatformTotals", "Dashboard", new { filters = "filters" })%>",
                inscriptionsPlatformAcums_read: "<%= Url.Action("Read_InscriptionsPlatformAcums", "Dashboard", new { filters = "filters" })%>",
                usersinstallations_read: "<%= Url.Action("Read_UsersInstallations", "Dashboard", new { filters = "filters" })%>",
                usersinstallationsfirstoperation_read: "<%= Url.Action("Read_UsersInstallationsFirstOperation", "Dashboard", new { filters = "filters" })%>",
                operationsuser_read: "<%= Url.Action("Read_OperationsUser", "Dashboard", new { filters = "filters" })%>",
                operationsuserall_read: "<%= Url.Action("Read_OperationsUserAll", "Dashboard", new { filters = "filters" })%>",
                invitations_read: "<%= Url.Action("Read_Invitations", "Dashboard", new { filters = "filters" })%>",
                invitationsAcums_read: "<%= Url.Action("Read_InvitationsAcums", "Dashboard", new { filters = "filters" })%>",
                rechargeCoupons_read: "<%= Url.Action("Read_RechargeCoupons", "Dashboard", new { filters = "filters" })%>",
                rechargeCouponsAcums_read: "<%= Url.Action("Read_RechargeCouponsAcums", "Dashboard", new { filters = "filters" })%>"
            },
            filters: { 
                Installations: <%= Model.InstallationsInit %>,
                //groups: [],
                DateGroup: <%: (int) Model.DateGroup %>,
                DateGroupPattern: <%: (Model.DateGroupPattern?"true":"false") %>,
                DateFilter: <%: (int) Model.DateFilter %>,
                CustomIniDate: new Date(Date.parse('<%: Model.CustomIniDate.ToString("yyyy/MM/dd HH:mm") %>', "yyyy/mm/dd HH:mm")),
                CustomEndDate: new Date(Date.parse('<%: Model.CustomEndDate.ToString("yyyy/MM/dd HH:mm") %>', "yyyy/mm/dd HH:mm")),
                CustomIniTime: "<%: Model.CustomIniTime %>",
                CustomEndTime: "<%: Model.CustomEndTime %>",
                CurrencyId: <%: Model.CurrencyId %>,
                CurrencyIsoCode: "<%= Model.CurrencyIsoCode %>",
                UserInstallation: <%= (int)Model.UserInstallation %>
            },
            tabs: [ { id: "tabToday", filtered: false },
                    { id: "tabOperations", filtered: false },
                    { id: "tabRecharges", filtered: false },
                    { id: "tabInscriptions", filtered: false },
                    { id: "tabInscriptionsPlatform", filtered: false },
                    { id: "tabUsersInstallations", filtered: false },
                    { id: "tabSystemUse", filtered: false },
                    { id: "tabInvitations", filtered: false },
                    { id: "tabRechargeCoupons", filtered: false }],
            tabSelected: "tabToday",
            tabSelectedIndex: 0,
            ready: false,
            noty: null
        };

        $(document).ready(function () {

            // show checked node IDs on datasource change
            /*$("#treeInstallations").data("kendoTreeView").dataSource.bind("change", function () {
                //function OnTreeInstallationsChange(e) {
                var checkedNodes = [],
                    //treeView = $("#treeInstallations").data("kendoTreeView"),
                    message;

                checkedNodeIds(treeView.dataSource.view(), checkedNodes);

                if (checkedNodes.length > 0) {
                    message = "IDs of checked nodes: " + checkedNodes.join(",");
                } else {
                    message = "No nodes checked.";
                }
                
                console.log(message);
            });*/

            dashboard.ready = true;

            $('#chkDateGroupPattern').change(function() {
                var isChecked = $(this).is(':checked');
                dashboard.filters.DateGroupPattern = isChecked;
                //ChartsRefresh(); 
                NotifyRefresh();
            });

            ChartsRefresh(true);            

            setTimeout(function () {
                $("#splMain").data("kendoSplitter")._resize();
                //$("#splChartsOperations").data("kendoSplitter")._resize();
                $("#splChartsToday").data("kendoSplitter")._resize();                
            }, 500);

            $("#txtTimeIni").data("kendoTimePicker").enable(false);
            $("#txtTimeEnd").data("kendoTimePicker").enable(false);
            
            $("#ddlCurrencies").data("kendoDropDownList").value(dashboard.filters.CurrencyId);
            
            dashboard_SetControls(dashboard.tabSelected);
        });

        function OnDateGroupsDataBound(e) {

            var ddlDateGroups = $("#ddlDateGroups").data("kendoDropDownList");
            //ddlDateGroups.select(3);
            ddlDateGroups.select(function (dataItem) {
                return dataItem.id === <%: (int) Model.DateGroup %>;
            });

        }

        function OnDateFiltersDataBound(e) {

            var ddlDateGroups = $("#ddlDateFilters").data("kendoDropDownList");
            //ddlDateGroups.select(3);
            ddlDateGroups.select(function (dataItem) {
                return dataItem.id === <%: (int) Model.DateFilter %>;
            });

        }
        
        function OnUserInstallationsDataBound(e) {

            var ddl = $("#ddlUserInstallations").data("kendoDropDownList");
            //ddlDateGroups.select(3);
            ddl.select(function (dataItem) {
                return dataItem.id === <%: (int) Model.UserInstallation %>;
            });
        }

        /*function TreeInstallationsChanged() {
            dashboard.filters.Installations = [];
            var treeInstallations = $("#treeInstallations").data("kendoTreeView");
            var items = treeInstallations.getCheckedItems();
            if (items != null) {
                for (var i = 0; i < items.length; i++) {
                    dashboard.filters.Installations.push(items[i].id);
                }
            }
            //ChartsRefresh(false, true);
            NotifyRefresh();
        }*/

        function TreeInstallationsOnUnitCheck(e) {
            var treeView = $("#treeInstallations").data("kendoTreeView");
        
            dashboard.filters.Installations = [];
            var checkedNodes = [];
            getCheckedNodeIds(treeView.dataSource.view(), checkedNodes);
            if (checkedNodes != null) {
                for (var i = 0; i < checkedNodes.length; i++) {
                    dashboard.filters.Installations.push(checkedNodes[i].id);
                }                
            }

            NotifyRefresh();
        };

        function getCheckedNodeIds(nodes, checkedNodes) {
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].checked) {
                    checkedNodes.push(nodes[i]);
                }

                if (nodes[i].hasChildren) {
                    getCheckedNodeIds(nodes[i].children.view(), checkedNodes);
                }
            }
        };


        function OnSplContentResize() {
            if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
        }

        function OnTabContentSelect(e) {
            setTimeout( function () {
                $(e.contentElement).find(".k-splitter").each( function () {
                    $(this).data("kendoSplitter")._resize();
                    ChartsRefresh(false);                    
                });
            }, 1500 );

            if (dashboard.tabSelected != e.item.id) {
                dashboard.tabSelected = e.item.id;
                dashboard.tabSelectedIndex = $(e.item).attr("tabIndex");
                ChartsRefresh(false);
            }

            $("#txtTimeIni").data("kendoTimePicker").enable(e.item.id != "tabToday");
            $("#txtTimeEnd").data("kendoTimePicker").enable(e.item.id != "tabToday");

            dashboard_SetControls(e.item.id);
        }
        function OnTabContentActivate(e) {
            ChartsRefresh(false);
        }

        function OnInstallationsChange() {
            var value = this.value();
        }

        function OnDateGroupsChange() {
            var value = this.value();            
            dashboard.filters.dateGroup = value;
            //ChartsRefresh();
            NotifyRefresh();
        }

        function OnDateFiltersChange() {
            var value = this.value();
            dashboard.filters.dateFilter = value;
            //ChartsRefresh();
            NotifyRefresh();

            var txtDateIni = $("#txtDateIni").data("kendoDateTimePicker");
            txtDateIni.enable((value == <%: (int) DateFilterType.custom %>));
            var txtDateEnd = $("#txtDateEnd").data("kendoDateTimePicker");
            txtDateEnd.enable((value == <%: (int) DateFilterType.custom %>));

        }

        function OnDateIniChange() {
            var endPicker = $("#txtDateEnd").data("kendoDateTimePicker"),
                startDate = this.value();

            if (startDate) {
                //var endDate = new Date(startDate);
                //endDate.setDate(startDate.getDate() + 1);
                //endPicker.min(endDate);
                endPicker.min(startDate);
            }

            dashboard.filters.CustomIniDate = startDate;
            //ChartsRefresh();
            NotifyRefresh();
        }

        function OnDateEndChange() {
            var startPicker = $("#txtDateIni").data("kendoDateTimePicker"),
                endDate = this.value();

            if (endDate) {
                //var startDate = new Date(endDate);
                //startDate.setDate(endDate.getDate() - 1);
                //startPicker.max(startDate);
                startPicker.max(endDate);
            }

            dashboard.filters.CustomEndDate = endDate;
            //ChartsRefresh();
            NotifyRefresh();
        }

        function OnTimeIniChange() {
            var endPicker = $("#txtTimeEnd").data("kendoTimePicker"),
                startTime = this.value();

            if (startTime) {
                endPicker.min(startTime);
            }

            var sHour = startTime.getHours().toString();
            if (sHour.length < 2) sHour = "0" + sHour;
            var sMinute = startTime.getMinutes().toString();
            if (sMinute.length < 2) sMinute = "0" + sMinute;

            dashboard.filters.CustomIniTime = sHour + ":" + sMinute;
            //ChartsRefresh();
            NotifyRefresh();
        }

        function OnTimeEndChange() {
            var startPicker = $("#txtTimeIni").data("kendoTimePicker"),
                endTime = this.value();

            if (endTime) {
                startPicker.max(endTime);
            }

            var sHour = endTime.getHours().toString();
            if (sHour.length < 2) sHour = "0" + sHour;
            var sMinute = endTime.getMinutes().toString();
            if (sMinute.length < 2) sMinute = "0" + sMinute;
            
            dashboard.filters.CustomEndTime = sHour + ":" + sMinute;
            //ChartsRefresh();
            NotifyRefresh();
        }

        function OnCurrenciesChange() {
            var value = this.value();            
            dashboard.filters.CurrencyId = value;
            dashboard.filters.CurrencyIsoCode = this.dataItem().IsoCode;
            //ChartsRefresh(true);
            NotifyRefresh();
        }

        function OnUserInstallationsChange() {
            var value = this.value();            
            dashboard.filters.UserInstallation = value;
            //ChartsRefresh();
            NotifyRefresh();
        }

        // function that gathers IDs of checked nodes
        /*function checkedNodeIds(nodes, checkedNodes) {
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].checked) {
                    checkedNodes.push(nodes[i].id);
                }

                if (nodes[i].hasChildren) {
                    checkedNodeIds(nodes[i].children.view(), checkedNodes);
                }
            }
        }*/

        //function ChartsRefresh(excludeTotals, reloadData) {
        function ChartsRefresh(reloadData, reloadDataOperations) {
            
            if (dashboard.ready) {

                if (reloadData == null) reloadData = true;
                if (reloadDataOperations == null) reloadDataOperations = false;

                var chart = null;                
                var chartNumName = "", chartTotalsName = "", chartAvgName = "", chartAvgTotalsName = "";
                var chartNumNameType = "kendoChart", chartTotalsNameType = "kendoChart", chartAvgNameType = "kendoChart", chartAvgTotalsNameType = "kendoChart";
                var urlNumRead, urlTotalsRead, urlAvgRead, urlAvgTotalsRead;
                var chartsExtra = new Array();
                var urlsExtraRead = new Array();
                var chartDataReloaded = false;

                if (dashboard.tabSelected == "tabToday") {
                    chartNumName = "";
                    chartNumNameType = "kendoSparkline";
                    chartTotalsName = "";
                    chartAvgName = "chartOperationsToday";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.today_read;
                    urlNumTotalsRead = "";
                    urlAvgRead = dashboard.actions.operationstoday_read;
                    urlAvgTotalsRead = "";
                    chartsExtra[0] = "chartParkingsToday";
                    //chartsExtra[1] = "chartExtensionsToday";
                    //chartsExtra[2] = "chartRefundsToday";
                    //chartsExtra[3] = "chartTicketsToday";
                    //chartsExtra[4] = "chartRechargesToday";
                    urlsExtraRead[0] = dashboard.actions.today_read;
                    //urlsExtraRead[1] = dashboard.actions.extensionstoday_read;
                    //urlsExtraRead[2] = dashboard.actions.refundstoday_read;
                    //urlsExtraRead[3] = dashboard.actions.ticketstoday_read;
                    //urlsExtraRead[4] = dashboard.actions.rechargestoday_read;
                }
                else if (dashboard.tabSelected == "tabOperations") {
                    chartNumName = "chartOperations";
                    chartTotalsName = "chartOperationsTotals";
                    chartAvgName = "chartOperationsAvg";
                    chartAvgTotalsName = "chartOperationsAvgTotals";
                    urlNumRead = dashboard.actions.operations_read;
                    urlNumTotalsRead = dashboard.actions.operationstotals_read;
                    urlAvgRead = dashboard.actions.operationsavg_read;
                    urlAvgTotalsRead = dashboard.actions.operationsavgtotals_read;
                }
                else if (dashboard.tabSelected == "tabRecharges") {
                    chartNumName = "chartRecharges";
                    chartTotalsName = "chartRechargesTotals";
                    chartAvgName = "chartRechargesAvg";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.recharges_read;
                    urlNumTotalsRead = dashboard.actions.rechargestotals_read;
                    urlAvgRead = dashboard.actions.rechargesavg_read;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabInscriptions") {
                    chartNumName = "chartInscriptions";
                    chartTotalsName = "chartInscriptionsTotals";
                    chartAvgName = "chartInscriptionsAvg";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.inscriptions_read;
                    urlNumTotalsRead = dashboard.actions.inscriptionstotals_read;
                    urlAvgRead = dashboard.actions.inscriptionsavg_read;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabInscriptionsPlatform") {
                    chartNumName = "chartInscriptionsPlatform";
                    chartTotalsName = "chartInscriptionsPlatformTotals";
                    chartAvgName = "chartInscriptionsPlatformAcums";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.inscriptionsPlatform_read;
                    urlNumTotalsRead = dashboard.actions.inscriptionsPlatformTotals_read;
                    urlAvgRead = dashboard.actions.inscriptionsPlatformAcums_read;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabUsersInstallations") {
                    chartNumName = "chartUsersInstallations";
                    chartTotalsName = "";
                    chartAvgName = "chartUsersInstallationsFirstOperation";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.usersinstallations_read;
                    urlNumTotalsRead = "";
                    urlAvgRead = dashboard.actions.usersinstallationsfirstoperation_read;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabSystemUse") {
                    chartNumName = "chartOperationsUser";
                    chartTotalsName = "";
                    chartAvgName = "chartOperationsUserAll";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.operationsuser_read;
                    urlNumTotalsRead = "";
                    urlAvgRead = dashboard.actions.operationsuserall_read;;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabInvitations") {
                    chartNumName = "chartInvitations";
                    chartTotalsName = "";
                    chartAvgName = "chartInvitationsAcums";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.invitations_read;
                    urlNumTotalsRead = "";
                    urlAvgRead = dashboard.actions.invitationsAcums_read;
                    urlAvgTotalsRead = "";
                }
                else if (dashboard.tabSelected == "tabRechargeCoupons") {
                    chartNumName = "chartRechargeCoupons";
                    chartTotalsName = "";
                    chartAvgName = "chartRechargeCouponsAcums";
                    chartAvgTotalsName = "";
                    urlNumRead = dashboard.actions.rechargeCoupons_read;
                    urlNumTotalsRead = "";
                    urlAvgRead = dashboard.actions.rechargeCouponsAcums_read;
                    urlAvgTotalsRead = "";
                }

                chart = $("#" + chartNumName).data(chartNumNameType);
                if (reloadData || !dashboard.tabs[dashboard.tabSelectedIndex].filtered || (reloadDataOperations && (dashboard.tabSelected == "tabToday" || dashboard.tabSelected == "tabOperations" || dashboard.tabSelected == "tabSystemUse"))) {
                    if (chart != null) {
                        chart.dataSource.transport.options.read.url = urlNumRead.replace("filters=filters", "filters=" + kendo.stringify(dashboard.filters));
                        chart.dataSource.read();
                    }
                    chartDataReloaded = true;
                }
                else                     
                    if (chart != null) {
                        chart.refresh();
                        if ($("#" + chartNumName).attr("inprogress") == "true")
                            kendo.ui.progress($("#" + chartNumName), true);
                    }

                chart = $("#" + chartTotalsName).data(chartTotalsNameType);            
                if (reloadData || !dashboard.tabs[dashboard.tabSelectedIndex].filtered || (reloadDataOperations && (dashboard.tabSelected == "tabToday" || dashboard.tabSelected == "tabOperations" || dashboard.tabSelected == "tabSystemUse"))) {
                    if (chart != null) {
                        chart.dataSource.transport.options.read.url = urlNumTotalsRead.replace("filters=filters", "filters=" + kendo.stringify(dashboard.filters));
                        chart.dataSource.read();
                    }
                    chartDataReloaded = true;
                }
                else
                    if (chart != null) {
                        chart.refresh();
                        if ($("#" + chartTotalsName).attr("inprogress") == "true")
                            kendo.ui.progress($("#" + chartTotalsName), true);
                    }

                chart = $("#" + chartAvgName).data(chartAvgNameType);            
                if (reloadData || !dashboard.tabs[dashboard.tabSelectedIndex].filtered || (reloadDataOperations && (dashboard.tabSelected == "tabToday" || dashboard.tabSelected == "tabOperations" || dashboard.tabSelected == "tabSystemUse"))) {
                    if (chart != null) {
                        chart.dataSource.transport.options.read.url = urlAvgRead.replace("filters=filters", "filters=" + kendo.stringify(dashboard.filters));
                        chart.dataSource.read();
                    }
                    chartDataReloaded = true;
                }
                else
                    if (chart != null) {
                        chart.refresh();
                        if ($("#" + chartAvgName).attr("inprogress") == "true")
                            kendo.ui.progress($("#" + chartAvgName), true);
                    }

                chart = $("#" + chartAvgTotalsName).data(chartAvgTotalsNameType);            
                if (reloadData || !dashboard.tabs[dashboard.tabSelectedIndex].filtered || (reloadDataOperations && (dashboard.tabSelected == "tabToday" || dashboard.tabSelected == "tabOperations" || dashboard.tabSelected == "tabSystemUse"))) {
                    if (chart != null) {
                        chart.dataSource.transport.options.read.url = urlAvgTotalsRead.replace("filters=filters", "filters=" + kendo.stringify(dashboard.filters));
                        chart.dataSource.read();
                    }
                    chartDataReloaded = true;
                }
                else
                    if (chart != null) {
                        chart.refresh();
                        if ($("#" + chartAvgTotalsName).attr("inprogress") == "true")
                            kendo.ui.progress($("#" + chartAvgTotalsName), true);
                    }

                for (var i=0; i<chartsExtra.length; i++) {
                    chart = $("#" + chartsExtra[i]).data("kendoSparkline");
                    if (reloadData || !dashboard.tabs[dashboard.tabSelectedIndex].filtered || (reloadDataOperations && (dashboard.tabSelected == "tabToday" || dashboard.tabSelected == "tabOperations" || dashboard.tabSelected == "tabSystemUse"))) {
                        if (chart != null) {
                            chart.dataSource.transport.options.read.url = urlsExtraRead[i].replace("filters=filters", "filters=" + kendo.stringify(dashboard.filters));
                            chart.dataSource.read();
                        }
                        chartDataReloaded = true;
                    }
                    else                     
                        if (chart != null) {
                            chart.refresh();
                            if ($("#" + chartsExtra[i]).attr("inprogress") == "true")
                                kendo.ui.progress($("#" + chartsExtra[i]), true);
                        }
                }

                if (reloadData) {
                    dashboard.tabs[0].filtered = false;
                    dashboard.tabs[1].filtered = false;
                    dashboard.tabs[2].filtered = false;
                    dashboard.tabs[3].filtered = false;
                    dashboard.tabs[4].filtered = false;
                    dashboard.tabs[5].filtered = false;
                    dashboard.tabs[6].filtered = false;
                    dashboard.tabs[7].filtered = false;
                }
                if (reloadDataOperations) {
                    dashboard.tabs[0].filtered = false;
                    dashboard.tabs[3].filtered = false;
                }
                if (chartDataReloaded)
                    dashboard.tabs[dashboard.tabSelectedIndex].filtered = true;

            }

        }

        function minutes2String(minutes) {
            var min_num = parseInt(minutes, 10);
            var hours   = Math.floor(min_num / 60);            
            var minutes = min_num - (hours * 60);

            if (hours   < 10) {hours   = "0"+hours;}
            if (minutes < 10) {minutes = "0"+minutes;}            
            var time    = hours+':'+minutes;
            return time;
        }

        /*kendo.ui.TreeView.prototype.getCheckedItems = (function(){

            function getCheckedItems(){
                var nodes = this.dataSource.view();
                return getCheckedNodes(nodes);
            }

            function getCheckedNodes(nodes){
                var node, childCheckedNodes;
                var checkedNodes = [];

                for (var i = 0; i < nodes.length; i++) {
                    node = nodes[i];
                    if (node.checked) {
                        checkedNodes.push(node);
                    }
        
                    // to understand recursion, first
                    // you must understand recursion
                    if (node.hasChildren) {
                        childCheckedNodes = getCheckedNodes(node.children.view());
                        if (childCheckedNodes.length > 0){
                            checkedNodes = checkedNodes.concat(childCheckedNodes);
                        }
                    }

                }

                return checkedNodes;
            }

            return getCheckedItems;
        })();*/

        function SplPanExpandCollapse (splId, panIndex) {
            var panes = $("#" + splId).children();
            var pane = panes[panIndex];
            if (!pane) return;

            $("#" + splId).data("kendoSplitter").toggle(pane, $(pane).width() <= 0);
        }

        Date.prototype.toJSON = function () 
        {
            return kendo.toString(this, "yyyy-MM-ddTHH:mm:ss");
        }; 

        function NotifyRefresh() {
            if (dashboard.noty == null) {
                dashboard.noty = $("#divNotify").noty({
                    text        : "<%= resBundle.GetString("PBPPlugin", "Dashboard_FilterNotify_Text") %>",
                    type        : "information",
                    dismissQueue: true,
                    layout      : "topCenter",
                    theme       : 'defaultTheme',
                    maxVisible  : 30,
                    buttons     : [
                        {addClass: 'btn btn-primary k-button', text: '<%= resBundle.GetString("PBPPlugin", "Dashboard_FilterNotify_ApplyButton") %>', onClick: function ($noty) {
                            $noty.close();
                            dashboard.noty = null;
                            ChartsRefresh();
                            //noty({dismissQueue: true, force: true, layout: layout, theme: 'defaultTheme', text: 'You clicked "Ok" button', type: 'success'});
                        }
                        },
                        {addClass: 'btn btn-danger k-button', text: '<%= resBundle.GetString("PBPPlugin", "Dashboard_FilterNotify_CancelButton") %>', onClick: function ($noty) {
                            $noty.close();
                            dashboard.noty = null;
                            //noty({dismissQueue: true, force: true, layout: layout, theme: 'defaultTheme', text: 'You clicked "Cancel" button', type: 'error'});
                        }
                        }
                    ]
                });
            }
        }

        var InstallationsRootExpanded = false;
        function treeInstallationsOnDataBound() {
            if (!InstallationsRootExpanded) {
                InstallationsRootExpanded = true;
                this.expand('.k-item');
            }
            else
                dashboard_SetControls(dashboard.tabSelected);
        }

        function dashboard_SetControls(tab) {
            $("#ddlUserInstallations").data("kendoDropDownList").enable(tab == "tabInscriptions" || tab == "tabInscriptionsPlatform");

            var treeview = $('#treeInstallations').data('kendoTreeView');
            treeview.enable(treeview.findByText("<%= resBundle.GetString("PBPPlugin", "Dashboard_UnknownInstallation", "Unknown") %>"),(tab == "tabInscriptions" || tab == "tabInscriptionsPlatform"));
        }

    </script>

</asp:Content>
