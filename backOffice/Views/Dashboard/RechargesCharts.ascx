<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>

<% Html.Kendo().Splitter()
.Name("splChartsRecharges")
.Orientation(SplitterOrientation.Vertical)
.HtmlAttributes(new { style = "height: 610px;" })
.Events(ev =>
    ev.Resize("OnSplChartsRechargesResize")
)                                                                                                                  
.Panes(verticalPanes =>
{
    verticalPanes.Add()
        .HtmlAttributes(new { id = "splChartsRecharges1" })
        .Size("50%")
        .MinSize("100px")
        .Collapsible(true)
        .Content(() => { %>
            <div class="pane-content" style="height: 100%;">
                <div class="div-chartcolumns" style="height: 100%;">
                <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartRecharges")
                        //.Title("Operations"
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Top).Visible(false)
                        )
                        .DataSource(ds => 
                        {
                            ds.Read(read => read.Action("Read_Recharges", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnRechargesChartRequestStart");
                                events.RequestEnd("OnRechargesChartRequestEnd");
                            });                                                                                               
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column().Stack(true)
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                        .Series(series =>
                        {
                            //series.Column(model => model.Total).Name("Operations");
                            series.Column(model => model.Serie0).Name(Resources.Dashboard_SerieRechargesCount)
                                                                .Axis("countAxis");
                            series.Line(model => model.Serie1).Name(Resources.Dashboard_SerieRechargesAmount)
                                                                .Axis("amountAxis")
                                                                .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #=dashboard.filters.CurrencyIsoCode#")).Width(1);
                                                                                                    
                        })
                        .CategoryAxis(axis => axis
                            .Categories(model => model.Category)                            
                            .Labels(labels => labels
                                .Rotation(-90)
                                //.Format("{0:dd/MM}")
                                //.Template("#= kendo.toString( toDate(BusinessDate), 'MM/dd/yyyy' )#")
                            )
                            .MajorGridLines(lines => lines.Visible(false))
                            .AxisCrossingValue(0, 1000000)
                        )
                        .ValueAxis(axis => axis
                            .Numeric("amountAxis")
                            .Title(Resources.Dashboard_YAxisAmount)                                                                                                
                            .Labels(labels => labels/*.Format("{0:n2}")*/.Template("#= kendo.toString(value, 'n0') # #= dashboard.filters.CurrencyIsoCode#"))
                            //.MajorUnit(10000)
                            .Line(line => line.Visible(true))
                        )
                        .ValueAxis(axis => axis
                            .Numeric("countAxis")
                            .Title(Resources.Dashboard_YAxisCount)
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
                        /*.Events(events =>
                        {
                                                                                                
                        })*/
                %>   
                </div>
            </div>
        <% });

    verticalPanes.Add()
        .HtmlAttributes(new { id = "splChartsRecharges2" })
        .Collapsible(true)
        .Content(() => { %>                                                                                
			<div class="pane-content" style="height: 100%;">
                <div class="div-chartavgs" style="height: 100%;">
                <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartRechargesAvg")                                                                                            
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" })
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Right)
                            .Labels(labels => labels.Template("#=text#"))
                            .Visible(false)
                        )
                        .DataSource(ds =>
                        {
                            ds.Read(read => read.Action("Read_RechargesAvg", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnRechargesAvgChartRequestStart");
                                events.RequestEnd("OnRechargesAvgChartRequestEnd");
                            });
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column()
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999"/*, "#cccccc"*/ })
                        .Series(series =>
                        {
                            series.Column(model => model.Serie0).Name(Resources.Dashboard_SerieBalanceRechargeAmountAvg)
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
                            .Title(Resources.Dashboard_YAxisAmountAvg)                                                                                                
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
                %>   
                </div>
            </div>																				
        <% });
                                           
}).Render();
%>


<script>

    function OnRechargesChartRequestStart(e) {
        kendo.ui.progress($("#chartRecharges"), true);
        $("#chartRecharges").attr("inprogress", "true");
    }
    function OnRechargesChartRequestEnd(e) {
        kendo.ui.progress($("#chartRecharges"), false);
        $("#chartRecharges").attr("inprogress", "false");
    }

    function OnRechargesAvgChartRequestStart(e) {
        kendo.ui.progress($("#chartRechargesAvg"), true);
        $("#chartRechargesAvg").attr("inprogress", "true");
    }
    function OnRechargesAvgChartRequestEnd(e) {
        kendo.ui.progress($("#chartRechargesAvg"), false);
        $("#chartRechargesAvg").attr("inprogress", "false");
    }

    function OnSplChartsRechargesResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>