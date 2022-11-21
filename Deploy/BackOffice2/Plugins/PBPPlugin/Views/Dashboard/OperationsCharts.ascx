<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>
<%@ Import Namespace="integraMobile.Domain.Abstract" %>

<% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

<% Html.Kendo().Splitter()
        .Name("splChartsOperations")
        .Orientation(SplitterOrientation.Vertical)
        .HtmlAttributes(new { style = "height: 605px;" })
        .Events(ev =>
            ev.Resize("OnSplChartsOperationsResize")
        )                                                                                                                  
        .Panes(verticalPanes =>
        {
            verticalPanes.Add()
                .HtmlAttributes(new { id = "splCharts1" })
                .Size("50%")
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartoperationscolumns">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperations")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_Operations", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsChartRequestStart");
                                        events.RequestEnd("OnOperationsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(true)
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    //series.Column(model => model.Total).Name("Operations");
                                    series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()));
                                    series.Column(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()));
                                    series.Column(model => model.Serie2).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()));
                                    series.Column(model => model.Serie3).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.TicketPayment.ToString()));
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
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCount"))
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
                        <div class="div-chartoperationspies">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperationsTotals")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(Kendo.Mvc.UI.ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_OperationsTotals", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsTotalsChartRequestStart");
                                        events.RequestEnd("OnOperationsTotalsChartRequestEnd");
                                    });                                                                                                
                                })     
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {                                                                                                
                                    series.Pie(model => model.Serie0, model => model.Category);
                                    //series.Pie(model => model.Serie1, model => model.Category).Name("Extends");
                                    //series.Pie(model => model.Serie2, model => model.Category).Name("Refunds");
                                                                                                
                                                                                                
                                })
                                .SeriesDefaults(defaults =>
                                    defaults.Pie()
                                            .Labels(labels =>
                                                labels.Template("#= category #<br>#=kendo.toString(value, 'n2')#%")
                                                .Background("transparent")
                                                .Align(Kendo.Mvc.UI.ChartPieLabelsAlign.Column)                                                
                                                .Visible(false)
                                            )
                                )
                                /*.CategoryAxis(axis => axis
                                    .Categories(model => model.Category)                            
                                    .Labels(labels => labels
                                        .Rotation(-90)
                                        //.Format("{0:dd/MM}")
                                        //.Template("#= kendo.toString( toDate(BusinessDate), 'MM/dd/yyyy' )#")
                                    )
                                    .MajorGridLines(lines => lines.Visible(false))
                            
                                )*/                                                                                         
                                .ValueAxis(axis => axis.Numeric()
                                    .Labels(labels => labels.Format("{0:N0}"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )
                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:n2}%")                                                                                                
                                    .Template("#= category #<br>#=kendo.toString(value, 'n2')#%")
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
                .HtmlAttributes(new { id = "splCharts2" })
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartoperationsavgs">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperationsAvg")
                        //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" })
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Right)
                                    .Labels(labels => labels.Template("#=text#"))
                                    .Visible(false)
                                )
                                .DataSource(ds =>
                                {
                                    ds.Read(read => read.Action("Read_OperationsAvg", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsAvgChartRequestStart");
                                        events.RequestEnd("OnOperationsAvgChartRequestEnd");
                                    });
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column()
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999"/*, "#cccccc"*/ })
                                .Series(series =>
                                {                                                                                                                                                                                                
                                    //series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieOperationsAmountAvg"))
                                    //                                    .Axis("amountAxis").Stack("amountTotal")
                                    //                                    .Tooltip(tooltip => tooltip.Template("#= series.name #: #= kendo.toString(value, 'n2') #€"));
                                    series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieParkingOperationsAmountAvg"))
                                                                        .Axis("amountAxis").Stack("amount")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #=dashboard.filters.CurrencyIsoCode#"));
                                    series.Column(model => model.Serie2).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieExtensionOperationsAmountAvg"))
                                                                        .Axis("amountAxis").Stack("amount")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #= dashboard.filters.CurrencyIsoCode#"));
                                    series.Column(model => model.Serie4).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieParkingRefundsAmountAvg"))
                                                                        .Axis("amountAxis").Stack("amount")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #= dashboard.filters.CurrencyIsoCode#"));
                                                                                                
                                    series.Line(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieParkingOperationsTimeAvg"))
                                                                        .Axis("timeAxis")//.Stack("time")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= minutes2String(value) #")).Width(1);
                                    series.Line(model => model.Serie3).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieExtensionOperationsTimeAvg"))
                                                                        .Axis("timeAxis")//.Stack("time")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= minutes2String(value) #")).Width(1);
                                    series.Line(model => model.Serie5).Name(resBundle.GetString("PBPPlugin", "Dashboard_SerieParkingRefunsTimeAvg"))
                                                                        .Axis("timeAxis")//.Stack("time")
                                                                        .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= minutes2String(value) #")).Width(1);
                                                                                                                                                                                                
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
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisAmountAvg"))
                                    .Labels(labels => labels/*.Format("{0:n2}")*/.Template("#= kendo.toString(value, 'n2') # #= dashboard.filters.CurrencyIsoCode#"))
                                    //.MajorUnit(10000)
                                    .Line(line => line.Visible(true))
                                )
                                .ValueAxis/*ValueAxis*/(axis => axis
                                    .Numeric("timeAxis")
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisTimeAvg"))
                                    .Labels(labels => labels.Template("#= minutes2String(value) #"))                                                                                                
                                    //.Color("#ec5e0a")                                                                                                
                                    //.Max(60)
                                )

                                .Tooltip(tooltip => tooltip
                                    .Visible(true)
                                    //.Format("{0:N0}")
                                    .Template("#= series.name #<br>#= kendo.toString(value, 'n2') #")
                                )
                                .AutoBind(false)
                        %>   
                        </div>
                        <div class="div-chartoperationsavgstotals">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperationsAvgTotals")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Bottom).Visible(true)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_OperationsAvgTotals", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsAvgTotalsChartRequestStart");
                                        events.RequestEnd("OnOperationsAvgTotalsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(false)
                                    .Tooltip(tooltip => tooltip.Template("#= series.name #<br>#= kendo.toString(value, 'n2') # #=dashboard.filters.CurrencyIsoCode#"))
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()))
                                                                        .Axis("amountAxis");
                                    series.Column(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()))
                                                                        .Axis("amountAxis");
                                    series.Column(model => model.Serie2).Name(resBundle.GetString("PBPPlugin", "ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()))
                                                                        .Axis("amountAxis");
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
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisAmountAvg"))
                                    .Labels(labels => labels/*.Format("{0:n2}")*/.Template("#= kendo.toString(value, 'n2') # #= dashboard.filters.CurrencyIsoCode#"))
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
                                           
        }).Render();
        %>

<script>

    function OnOperationsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperations"), true);
        $("#chartOperations").attr("inprogress", "true");
    }
    function OnOperationsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperations"), false);
        $("#chartOperations").attr("inprogress", "false");
    }

    function OnOperationsTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsTotals"), true);
        $("#chartOperationsTotals").attr("inprogress", "true");
    }
    function OnOperationsTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsTotals"), false);
        $("#chartOperationsTotals").attr("inprogress", "false");
    }

    function OnOperationsAvgChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsAvg"), true);
        $("#chartOperationsAvg").attr("inprogress", "true");
    }
    function OnOperationsAvgChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsAvg"), false);
        $("#chartOperationsAvg").attr("inprogress", "false");
    }

    function OnOperationsAvgTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), true);
        $("#chartOperationsAvgTotals").attr("inprogress", "true");
    }
    function OnOperationsAvgTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), false);
        $("#chartOperationsAvgTotals").attr("inprogress", "false");
    }

    function OnSplChartsOperationsResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>