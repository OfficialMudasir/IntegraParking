<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>
<%@ Import Namespace="integraMobile.Domain.Abstract" %>

<% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

<% Html.Kendo().Splitter()
        .Name("splChartsInvitations")
        .Orientation(SplitterOrientation.Vertical)
        .HtmlAttributes(new { style = "height: 605px;" })
        .Events(ev =>
            ev.Resize("OnSplChartsInvitationsResize")
        )                                                                                                                  
        .Panes(verticalPanes =>
        {
            verticalPanes.Add()
                .HtmlAttributes(new { id = "splInvitationsCharts1" })
                .Size("50%")
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartcolumns" style="height: 100%;">
                        <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartInvitations")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(true)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_Invitations", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnInvitationsChartRequestStart");
                                        events.RequestEnd("OnInvitationsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column().Stack(true)
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    series.Column(model => model.Serie0).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Sended")).Stack("Stacked1");
                                    series.Column(model => model.Serie1).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Accepted")).Stack("Stacked2");
                                    series.Column(model => model.Serie2).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Pending")).Stack("Stacked2");
                                    series.Column(model => model.Serie3).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_AcceptedTotal")).Stack("Stacked3");
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
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCount_Invitations"))
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
                .HtmlAttributes(new { id = "splCharts2" })
                .Collapsible(true)
                .Content(() => { %>
                    <div class="pane-content" style="height: 100%;">
                        <div class="div-chartavgs" style="height: 100%;">
                            <%= Html.Kendo().Chart<ChartDataItem>()
                                .Name("chartInvitationsAcums")
                                //.Title("Operations"
                                .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                                .Legend(legend => legend
                                    .Position(ChartLegendPosition.Top).Visible(true)
                                )
                                .DataSource(ds => 
                                {
                                    ds.Read(read => read.Action("Read_InvitationsAcums", "Dashboard"));
                                    ds.Events(events =>
                                    {
                                        events.RequestStart("OnInvitationsAcumsChartRequestStart");
                                        events.RequestEnd("OnInvitationsAcumsChartRequestEnd");
                                    });                                                                                                
                                })
                                .SeriesDefaults(seriesDefaults =>
                                    seriesDefaults.Column()
                                )
                                .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                                .Series(series =>
                                {
                                    series.Column(model => model.Serie4).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Acums_Sended"));
                                    series.Column(model => model.Serie5).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Acums_Accepted"));
                                    series.Column(model => model.Serie6).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Acums_Pending"));
                                    series.Column(model => model.Serie7).Name(resBundle.GetString("PBPPlugin", "Dashboard_Invitations_Acums_AcceptedTotal"));
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
                                    .Title(resBundle.GetString("PBPPlugin", "Dashboard_YAxisCount_InvitationsAcum"))
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

    function OnInvitationsChartRequestStart(e) {
        kendo.ui.progress($("#chartInvitations"), true);
        $("#chartInvitations").attr("inprogress", "true");
    }
    function OnInvitationsChartRequestEnd(e) {
        kendo.ui.progress($("#chartInvitations"), false);
        $("#chartInvitations").attr("inprogress", "false");
    }

    /*function OnOperationsTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsTotals"), true);
        $("#chartOperationsTotals").attr("inprogress", "true");
    }
    function OnOperationsTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsTotals"), false);
        $("#chartOperationsTotals").attr("inprogress", "false");
    }*/

    function OnInvitationsAcumsChartRequestStart(e) {
        kendo.ui.progress($("#chartInvitationsAcums"), true);
        $("#chartInvitationsAcums").attr("inprogress", "true");
    }
    function OnInvitationsAcumsChartRequestEnd(e) {
        kendo.ui.progress($("#chartInvitationsAcums"), false);
        $("#chartInvitationsAcums").attr("inprogress", "false");
    }

    /*function OnOperationsAvgTotalsChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), true);
        $("#chartOperationsAvgTotals").attr("inprogress", "true");
    }
    function OnOperationsAvgTotalsChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsAvgTotals"), false);
        $("#chartOperationsAvgTotals").attr("inprogress", "false");
    }*/

    function OnSplChartsInvitationsResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>