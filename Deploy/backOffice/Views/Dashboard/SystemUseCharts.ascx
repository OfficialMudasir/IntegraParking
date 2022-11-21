<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>

<% Html.Kendo().Splitter()
        .Name("splChartsOperationsUser")
        .Orientation(SplitterOrientation.Vertical)
        .HtmlAttributes(new { style = "height: 605px;" })
        .Events(ev =>
            ev.Resize("OnSplChartsOperationsUserResize")
        )                                                                                                                  
        .Panes(verticalPanes =>
        {
            verticalPanes.Add()
                .HtmlAttributes(new { id = "splChartsOperationsUser1" })
                .Size("50%")
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartcolumns">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperationsUser")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_OperationsUser", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsUserChartRequestStart");
                                        events.RequestEnd("OnOperationsUserChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(true)
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    //series.Column(model => model.Total).Name("Operations");
                                    series.Column(model => model.Serie0).Name(string.Format(Resources.Dashboard_SerieOperationsUser, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()))).Stack("users");
                                    series.Column(model => model.Serie1).Name(string.Format(Resources.Dashboard_SerieOperationsUser, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()))).Stack("users");
                                    series.Column(model => model.Serie2).Name(string.Format(Resources.Dashboard_SerieOperationsUser, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()))).Stack("users");
                                    series.Column(model => model.Serie3).Name(string.Format(Resources.Dashboard_SerieOperationsUser, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.TicketPayment.ToString()))).Stack("users");
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
                                    .Title(Resources.Dashboard_YAxisCount)
                                    .Labels(labels => labels.Format("{0:N2}"))
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
                .HtmlAttributes(new { id = "splChartsOperationsUser2" })
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartavgs">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartOperationsUserAll")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(false)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_OperationsUserAll", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnOperationsUserAllChartRequestStart");
                                        events.RequestEnd("OnOperationsUserAllChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(true)
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    //series.Column(model => model.Total).Name("Operations");
                                    series.Column(model => model.Serie0).Name(string.Format(Resources.Dashboard_SerieOperationsUserAll, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()))).Stack("allusers");
                                    series.Column(model => model.Serie1).Name(string.Format(Resources.Dashboard_SerieOperationsUserAll, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()))).Stack("allusers");
                                    series.Column(model => model.Serie2).Name(string.Format(Resources.Dashboard_SerieOperationsUserAll, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()))).Stack("allusers");
                                    series.Column(model => model.Serie3).Name(string.Format(Resources.Dashboard_SerieOperationsUserAll, Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.TicketPayment.ToString()))).Stack("allusers");
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
                                    .Title(Resources.Dashboard_YAxisCount)
                                    .Labels(labels => labels.Format("{0:N3}"))
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

    function OnOperationsUserChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsUser"), true);
        $("#chartOperationsUser").attr("inprogress", "true");
    }
    function OnOperationsUserChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsUser"), false);
        $("#chartOperationsUser").attr("inprogress", "false");
    }

    function OnOperationsUserAllChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsUserAll"), true);
        $("#chartOperationsUserAll").attr("inprogress", "true");
    }
    function OnOperationsUserAllChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsUserAll"), false);
        $("#chartOperationsUserAll").attr("inprogress", "false");
    }

    function OnSplChartsOperationsUserResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>