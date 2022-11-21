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
.Name("splChartsInscriptionsPlatform")
.Orientation(SplitterOrientation.Vertical)
.HtmlAttributes(new { style = "height: 610px;" })
.Events(ev =>
    ev.Resize("OnSplChartsInscriptionsPlatformResize")
)                                                                                                                  
.Panes(verticalPanes =>
{
    verticalPanes.Add()
        .HtmlAttributes(new { id = "splChartsInscriptionsPlatform1" })
        .Size("50%")
        .Collapsible(true)
        .Content(() => { %>
            <div class="pane-content" style="height: 100%;">
                <div class="div-chartinscriptionsplatformcolumns" style="height: 100%;">
                    <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartInscriptionsPlatform")
                        //.Title("Operations"
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Top).Visible(true)
                        )
                        .DataSource(ds => 
                        {
                            ds.Read(read => read.Action("Read_InscriptionsPlatform", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnInscriptionsPlatformChartRequestStart");
                                events.RequestEnd("OnInscriptionsPlatformChartRequestEnd");
                            });                                                                                                
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column().Stack(true)
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                        .Series(series =>
                        {
                            //series.Column(model => model.Total).Name("Operations");
                            series.Column(model => model.Serie0).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.iOS")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie1).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Android")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie2).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.WindowsPhone")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie3).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Web")).Axis("countAxis").Stack("Count");
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
                            .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCountInscriptionsPlatform"))
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
                <div class="div-chartinscriptionsplatformpies">
                    <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartInscriptionsPlatformTotals")
                        //.Title("Operations"
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Bottom).Visible(true)
                        )
                        .DataSource(ds => 
                        {
                            ds.Read(read => read.Action("Read_InscriptionsPlatformTotals", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnInscriptionsPlatformTotalsChartRequestStart");
                                events.RequestEnd("OnInscriptionsPlatformTotalsChartRequestEnd");
                            });                                                                                                
                        })     
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                        .Series(series =>
                        {                                                                                                
                            series.Pie(model => model.Serie0, model => model.Category);
                        })
                        .SeriesDefaults(defaults =>
                            defaults.Pie()
                                    .Labels(labels =>
                                        labels.Template("#= category #<br>#=kendo.toString(value, 'n2')#%")
                                        .Background("transparent")
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
        .HtmlAttributes(new { id = "splChartsInscriptionsPlatform2" })
        .Collapsible(true)
        .Content(() => { %>                                                                                
			<div class="pane-content" style="height: 100%;">
                <div class="div-chartavgs" style="height: 100%;">
                    <%= Html.Kendo().Chart<ChartDataItem>()
                        .Name("chartInscriptionsPlatformAcums")
                        //.Title("Operations"
                        .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                        .Legend(legend => legend
                            .Position(ChartLegendPosition.Top).Visible(true)
                        )
                        .DataSource(ds => 
                        {
                            ds.Read(read => read.Action("Read_InscriptionsPlatformAcums", "Dashboard"));
                            ds.Events(events =>
                            {
                                events.RequestStart("OnInscriptionsPlatformAcumsChartRequestStart");
                                events.RequestEnd("OnInscriptionsPlatformAcumsChartRequestEnd");
                            });                                                                                                
                        })
                        .SeriesDefaults(seriesDefaults =>
                            seriesDefaults.Column().Stack(true)
                        )
                        .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                        .Series(series =>
                        {
                            //series.Column(model => model.Total).Name("Operations");
                            series.Column(model => model.Serie4).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.iOS")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie5).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Android")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie6).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.WindowsPhone")).Axis("countAxis").Stack("Count");
                            series.Column(model => model.Serie7).Name(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Web")).Axis("countAxis").Stack("Count");
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
                            .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCountInscriptionsPlatformAcums"))
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
                                           
}).Render();
%>

<script>

    function OnInscriptionsPlatformChartRequestStart(e) {
        kendo.ui.progress($("#chartInscriptionsPlatform"), true);
        $("#chartInscriptionsPlatform").attr("inprogress", "true");
    }
    function OnInscriptionsPlatformChartRequestEnd(e) {
        kendo.ui.progress($("#chartInscriptionsPlatform"), false);
        $("#chartInscriptionsPlatform").attr("inprogress", "false");
    }

    function OnInscriptionsPlatformTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartInscriptionsPlatformTotals"), true);
        $("#chartInscriptionsPlatformTotals").attr("inprogress", "true");
    }
    function OnInscriptionsPlatformTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartInscriptionsPlatformTotals"), false);
        $("#chartInscriptionsPlatformTotals").attr("inprogress", "false");
    }

    function OnInscriptionsPlatformAcumsChartRequestStart(e) {
        kendo.ui.progress($("#chartInscriptionsPlatformAcums"), true);
        $("#chartInscriptionsPlatformAcums").attr("inprogress", "true");
    }
    function OnInscriptionsPlatformAcumsChartRequestEnd(e) {
        kendo.ui.progress($("#chartInscriptionsPlatformAcums"), false);
        $("#chartInscriptionsPlatformAcums").attr("inprogress", "false");
    }

    function OnSplChartsInscriptionsPlatformResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>