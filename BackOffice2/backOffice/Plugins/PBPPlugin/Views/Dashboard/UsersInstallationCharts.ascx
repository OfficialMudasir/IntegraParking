<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>
<%@ Import Namespace="integraMobile.Domain.Abstract" %>

<% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

<% Html.Kendo().Splitter()
        .Name("splChartsUsersInstallation")
        .Orientation(SplitterOrientation.Vertical)
        .HtmlAttributes(new { style = "height: 605px;" })
        .Events(ev =>
            ev.Resize("OnSplChartsUsersInstallationResize")
        )                                                                                                                  
        .Panes(verticalPanes =>
        {
            verticalPanes.Add()
                .HtmlAttributes(new { id = "splChartsUsersInstallation1" })
                .Size("50%")
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">                        
                        <div class="div-chartuserinstallationspies">
                        <label><%= resBundle.GetString("PBPPlugin", "Dashboard_UsersInstallations_Title") %> </label>
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartUsersInstallations")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(Kendo.Mvc.UI.ChartLegendPosition.Right).Visible(true)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_UsersInstallations", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnUsersInstallationsChartRequestStart");
                                        events.RequestEnd("OnUsersInstallationsChartRequestEnd");
                                    });                                                                                                
                                })     
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#888888", "#aaaaaa", "#42b7ff", "#444444", "#777777", "#999999", "#42c7ff", "#555555", "#bbbbbb", "#dddddd", "#42d7ff" })
                                .Series(series =>
                                {                                                                                                
                                    series.Pie(model => model.Serie1, model => model.Category);
                                    //series.Pie(model => model.Serie1, model => model.Category).Name("Extends");
                                    //series.Pie(model => model.Serie2, model => model.Category).Name("Refunds");
                                                                                                
                                                                                                
                                })
                                .SeriesDefaults(defaults =>
                                    defaults.Pie()
                                        .Labels(labels =>
                                            labels.Template("#= category #<br>#=kendo.format('{0:P}', percentage)# (#=dataItem.Serie0#)")
                                            .Background("transparent")
                                            .Align(Kendo.Mvc.UI.ChartPieLabelsAlign.Column)                                                
                                            .Visible(false)
                                        )
                                )
                                .ValueAxis(axis => axis.Numeric()
                                    .Labels(labels => labels.Format("{0:N0}"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )
                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:n2}%")    
                                    .Format("{0:N0}")                                                                                            
                                    .Template("#= category #<br>#=kendo.format('{0:P}', percentage)# (#=dataItem.Serie0#)")
                                )
                                .AutoBind(false)
                                .Pannable(pannable => pannable                                    
                                    .Lock(ChartAxisLock.Y)
                                )
                                .Zoomable(zoomable => zoomable
                                    .Mousewheel(mousewheel => mousewheel.Lock(ChartAxisLock.Y))
                                    .Selection(selection => selection.Lock(ChartAxisLock.Y))
                                )
                                /*.Events(events =>
                                {
                                                                                                
                                })*/
                        %>                                                                                                     
                        </div>
                    </div>
                <% });

            verticalPanes.Add()
                .HtmlAttributes(new { id = "splChartsUsersInstallation2" })
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartuserinstallationspies">
                        <label><%= resBundle.GetString("PBPPlugin", "Dashboard_UsersInstallationsFistOperation_Title") %> </label>
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartUsersInstallationsFirstOperation")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(Kendo.Mvc.UI.ChartLegendPosition.Right).Visible(true)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_UsersInstallationsFirstOperation", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnUsersInstallationsFirstOperationChartRequestStart");
                                        events.RequestEnd("OnUsersInstallationsFirstOperationChartRequestEnd");
                                    });                                                                                                
                                })     
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#888888", "#aaaaaa", "#42b7ff", "#444444", "#777777", "#999999", "#42c7ff", "#555555", "#bbbbbb", "#dddddd", "#42d7ff" })
                                .Series(series =>
                                {                                                                                                
                                    series.Pie(model => model.Serie1, model => model.Category);
                                    //series.Pie(model => model.Serie1, model => model.Category).Name("Extends");
                                    //series.Pie(model => model.Serie2, model => model.Category).Name("Refunds");
                                                                                                
                                                                                                
                                })
                                .SeriesDefaults(defaults =>
                                    defaults.Pie()
                                        .Labels(labels =>
                                            labels.Template("#= category #<br>#=kendo.format('{0:P}', percentage)# (#=dataItem.Serie0#)")
                                            .Background("transparent")
                                            .Align(Kendo.Mvc.UI.ChartPieLabelsAlign.Column)                                                
                                            .Visible(false)
                                        )
                                )                                                                                                                        
                                .ValueAxis(axis => axis.Numeric()
                                    .Labels(labels => labels.Format("{0:N0}"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )
                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:n2}%")                                                                                                                                    
                                    .Template("#= category #<br>#=kendo.format('{0:P}', percentage)# (#=dataItem.Serie0#)")
                                )
                                .AutoBind(false)
                                .Pannable(pannable => pannable                                    
                                    .Lock(ChartAxisLock.Y)
                                )
                                .Zoomable(zoomable => zoomable
                                    .Mousewheel(mousewheel => mousewheel.Lock(ChartAxisLock.Y))
                                    .Selection(selection => selection.Lock(ChartAxisLock.Y))
                                )
                                /*.Events(events =>
                                {
                                                                                                
                                })*/
                        %>    
                        </div>

                    </div>
                <% });
                                           
        }).Render();
        %>

<script>

    function OnUsersInstallationsChartRequestStart(e) {
        kendo.ui.progress($("#chartUsersInstallations"), true);
        $("#chartUsersInstallations").attr("inprogress", "true");
    }
    function OnUsersInstallationsChartRequestEnd(e) {
        kendo.ui.progress($("#chartUsersInstallations"), false);
        $("#chartUsersInstallations").attr("inprogress", "false");
    }

    function OnUsersInstallationsFirstOperationChartRequestStart(e) {
        kendo.ui.progress($("#chartUsersInstallationsFirstOperation"), true);
        $("#chartUsersInstallationsFirstOperation").attr("inprogress", "true");
    }
    function OnUsersInstallationsFirstOperationChartRequestEnd(e) {
        kendo.ui.progress($("#chartUsersInstallationsFirstOperation"), false);
        $("#chartUsersInstallationsFirstOperation").attr("inprogress", "false");
    }

   

    function OnSplChartsUsersInstallationResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>