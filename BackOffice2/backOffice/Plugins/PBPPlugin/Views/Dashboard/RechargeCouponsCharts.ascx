<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>
<%@ Import Namespace="integraMobile.Domain.Abstract" %>

<% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

<% Html.Kendo().Splitter()
        .Name("splChartsRechargeCoupons")
        .Orientation(SplitterOrientation.Vertical)
        .HtmlAttributes(new { style = "height: 605px;" })
        .Events(ev =>
            ev.Resize("OnSplChartsRechargeCouponsResize")
        )                                                                                                                  
        .Panes(verticalPanes =>
        {
            verticalPanes.Add()
                .HtmlAttributes(new { id = "splRechargeCouponsCharts1" })
                .Size("50%")
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartcolumns" style="height: 100%;">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartRechargeCoupons")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_RechargeCoupons", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnRechargeCouponsChartRequestStart");
                                        events.RequestEnd("OnRechargeCouponsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(true)
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_RechargeCoupons_Purchased")).Stack("Stacked1");
                                    series.Column(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "Dashboard_RechargeCoupons_Used")).Stack("Stacked2");
                                })
                                .CategoryAxis(axis => axis
                                    .Categories(model => model.Category)                            
                                    .Labels(labels => labels
                                        .Rotation(-90)
                                        //.Format("{0:dd/MM}")
                                        //.Template("#= kendo.toString( toDate(BusinessDate), 'MM/dd/yyyy' )#")
                                    )
                                    .MajorGridLines(lines => lines.Visible(false))
                            
                                )
                                .ValueAxis(axis => axis
                                    .Numeric()
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCount_RechargeCoupons"))
                                    .Labels(labels => labels.Format("{0:N0}"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )                                                                                            
                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:N0}")
                                    .Template("#= series.name #<br>#= value #")
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
                .HtmlAttributes(new { id = "splRechargeCouponsCharts2" })
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartavgs" style="height: 100%;">
                            <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartRechargeCouponsAcums")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_RechargeCouponsAcums", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnRechargeCouponsAcumsChartRequestStart");
                                        events.RequestEnd("OnRechargeCouponsAcumsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column()
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    series.Column(model => model.Serie2).Name(resBundle.GetString("PBPPlugin", "Dashboard_RechargeCoupons_Acums_Purchased"));
                                    series.Column(model => model.Serie3).Name(resBundle.GetString("PBPPlugin", "Dashboard_RechargeCoupons_Acums_Used"));
                                })
                                .CategoryAxis(axis => axis
                                    .Categories(model => model.Category)                            
                                    .Labels(labels => labels
                                        .Rotation(-90)
                                        //.Format("{0:dd/MM}")
                                        //.Template("#= kendo.toString( toDate(BusinessDate), 'MM/dd/yyyy' )#")
                                    )
                                    .MajorGridLines(lines => lines.Visible(false))
                            
                                )
                                .ValueAxis(axis => axis
                                    .Numeric()
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCount_RechargeCouponsAcum"))
                                    .Labels(labels => labels.Format("{0:N0}"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )                                                                                            
                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:N0}")
                                    .Template("#= series.name #<br>#= value #")
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

    function OnRechargeCouponsChartRequestStart(e) {
        kendo.ui.progress($("#chartRechargeCoupons"), true);
        $("#chartRechargeCoupons").attr("inprogress", "true");
    }
    function OnRechargeCouponsChartRequestEnd(e) {
        kendo.ui.progress($("#chartRechargeCoupons"), false);
        $("#chartRechargeCoupons").attr("inprogress", "false");
    }

    /*function OnOperationsTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsTotals"), true);
        $("#chartOperationsTotals").attr("inprogress", "true");
    }
    function OnOperationsTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsTotals"), false);
        $("#chartOperationsTotals").attr("inprogress", "false");
    }*/

    function OnRechargeCouponsAcumsChartRequestStart(e) {
        kendo.ui.progress($("#chartRechargeCouponsAcums"), true);
        $("#chartRechargeCouponsAcums").attr("inprogress", "true");
    }
    function OnRechargeCouponsAcumsChartRequestEnd(e) {
        kendo.ui.progress($("#chartRechargeCouponsAcums"), false);
        $("#chartRechargeCouponsAcums").attr("inprogress", "false");
    }

    /*function OnOperationsAvgTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), true);
        $("#chartOperationsAvgTotals").attr("inprogress", "true");
    }
    function OnOperationsAvgTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), false);
        $("#chartOperationsAvgTotals").attr("inprogress", "false");
    }*/

    function OnSplChartsRechargeCouponsResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>