<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Collections" %>
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
<%@ Import Namespace="integraMobile.Domain.Abstract" %>

<% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

<% Html.Kendo().Splitter()
.Name("splChartsInscriptions")
.Orientation(SplitterOrientation.Vertical)
.HtmlAttributes(new { style = "height: 610px;" })
.Events(ev =>
    ev.Resize("OnSplChartsInscriptionsResize")
)                                                                                                                  
.Panes(verticalPanes =>
{
    verticalPanes.Add()
        .HtmlAttributes(new { id = "splChartsInscriptions1" })
        .Size("50%")
        .Collapsible(true)
        .Content(() => { %>
            <div class="pane-content" style="height: 100%;">
                <div class="div-chartcolumns2" style="height: 100%;">
                <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartInscriptions")
                        //.Title("Operations"
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Top).Visible(false)
                        )
                        .DataSource(ds => 
                        {
                            ds.Read(read => read.Action("Read_Inscriptions", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnInscriptionsChartRequestStart");
                                events.RequestEnd("OnInscriptionsChartRequestEnd");
                            });                                                                                                
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column().Stack(true)
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                        .Series(series =>
                        {
                            //series.Column(model => model.Total).Name("Operations");
                            series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieInscriptionsCount"))
                                                                .Axis("countAxis");
                                                                                                    
                        })
                        .CategoryAxis(axis => axis
                            .Categories(model => model.Category)                            
                            .Labels(labels => labels
                                .Rotation(-90)
                                //.Format("{0:dd/MM}")
                                //.Template("#= kendo.toString( toDate(BusinessDate), 'MM/dd/yyyy' )#")
                            )
                            .MajorGridLines(lines => lines.Visible(false))
                            //.AxisCrossingValue(0, 1000000)
                        )
                        .ValueAxis(axis => axis
                            .Numeric("countAxis")
                            .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCountInscriptions"))
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
        .HtmlAttributes(new { id = "splChartsInscriptions2" })
        .Collapsible(true)
        .Content(() => { %>                                                                                
			<div class="pane-content" style="height: 100%;">
                <div class="div-chartavgs" style="height: 100%;">
                <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartInscriptionsAvg")                                                                                            
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" })
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Right)
                            .Labels(labels => labels.Template("#=text#"))
                            .Visible(false)
                        )
                        .DataSource(ds =>
                        {
                            ds.Read(read => read.Action("Read_InscriptionsAvg", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnInscriptionsAvgChartRequestStart");
                                events.RequestEnd("OnInscriptionsAvgChartRequestEnd");
                            });
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column()
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999"/*, "#cccccc"*/ })
                        .Series(series =>
                        {
                            series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_Serie1InscriptionAmountAvg"))
                                                                .Axis("amountAxis")//.Stack("amount")
                                                                .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #=dashboard.filters.CurrencyIsoCode#"));
                            series.Column(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "Dashboard_Serie2InscriptionAmountAvg"))
                                                                .Axis("amountAxis")//.Stack("amount")
                                                                .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #=dashboard.filters.CurrencyIsoCode#"));
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
                            .Numeric("amountAxis")                            
                            .Title("Valor")                                                                                                
                            .Labels(labels => labels/*.Format("{0:n2}")*/.Template("#= kendo.toString(value, 'n0') # #= dashboard.filters.CurrencyIsoCode#"))
                            //.MajorUnit(10000)
                            .Line(line => line.Visible(true))
                        )

                        .Tooltip(tooltip => tooltip
                            .Visible(true)
                            //.Format("{0:N0}")
                            .Template("#= series.name #<br>#= kendo.toString(value, 'n2') #")
                        )
                        .AutoBind(false)
                        .Pannable(pannable => pannable                                    
                            .Lock(ChartAxisLock.Y)
                        )
                        .Zoomable(zoomable => zoomable
                            .Mousewheel(mousewheel => mousewheel.Lock(ChartAxisLock.Y))
                            .Selection(selection => selection.Lock(ChartAxisLock.Y))
                        )
                %>   
                </div>
            </div>																				
        <% });
                                           
}).Render();
%>

<script>

    function OnInscriptionsChartRequestStart(e) {
        kendo.ui.progress($("#chartInscriptions"), true);
        $("#chartInscriptions").attr("inprogress", "true");
    }
    function OnInscriptionsChartRequestEnd(e) {
        kendo.ui.progress($("#chartInscriptions"), false);
        $("#chartInscriptions").attr("inprogress", "false");
    }

    function OnInscriptionsAvgChartRequestStart(e) {
        kendo.ui.progress($("#chartInscriptionsAvg"), true);
        $("#chartInscriptionsAvg").attr("inprogress", "true");
    }
    function OnInscriptionsAvgChartRequestEnd(e) {
        kendo.ui.progress($("#chartInscriptionsAvg"), false);
        $("#chartInscriptionsAvg").attr("inprogress", "false");
    }

    function OnSplChartsInscriptionsResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>