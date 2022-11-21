<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DashboardDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>
<%@ Import Namespace="backOffice.Models.Dashboard" %>

<% Html.Kendo().Splitter()
    .Name("splChartsToday")
    .Orientation(SplitterOrientation.Vertical)
    .HtmlAttributes(new { style = "height: 605px;" })
    .Events(ev =>
        ev.Resize("OnSplChartsTodayResize")
    )                                                                                                                  
    .Panes(verticalPanes =>
    {
        verticalPanes.Add()
            .HtmlAttributes(new { id = "splChartsToday1" })
            .Size("150px")
            .Collapsible(true)
            .Content(() => { %>
                <div class="pane-content" style="height: 100%;">
                    <table border="0" style="width:100%;">
                        <tr>
                            <td colspan="3" class="openum-title-container">
                                <span id="spanParkingsTitle" class="equal"><%= Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()) %></span>
                            </td>
                            <td colspan="3" class="openum-title-container">
                                <span id="spanExtensionsTitle" class="equal"><%= Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()) %></span>
                            </td>
                            <td colspan="3" class="openum-title-container">
                                <span id="spanRefundsTitle" class="equal"><%= Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()) %></span>
                            </td>
                            <td colspan="3" class="openum-title-container">
                                <span id="spanTicketsTitle" class="equal"><%= Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.TicketPayment.ToString()) %></span>
                            </td>
                            <td colspan="3" class="openum-title-container">
                                <span id="spanRechargesTitle" class="equal"><%= Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.BalanceRecharge.ToString()) %></span>
                            </td>
                            <td rowspan="3" class="openum-refresh-container">
                                <button type="button" id="btnRefreshToday" class="k-button k-button-icon" 
                                        onclick="ChartsRefresh();return false;"
                                        title="">
                                    <span class="k-icon k-i-refresh"></span>
                                </button>
                            </td>
                        </tr>
                        <tr>
                            <% string[] opeTypes = new string[] { "Parkings", "Extensions", "Refunds", "Tickets", "Recharges" };
                               foreach (string type in opeTypes) {
                            %>
                                <td colspan="3" class="openum-detail-container">
                                    <table border="0" style="width:100%;">
                                        <tr>
                                            <td class="openum-detail-title-container openum-detail-title-container-left">
                                                <span id="span<%:type%>PrevWeekTitle"></span>
                                            </td>
                                            <td class="openum-detail-title-container openum-detail-title-container-middle">
                                                <span id="span<%:type%>PrevDayTitle"></span>
                                            </td>
                                            <td class="openum-detail-title-container openum-detail-title-container-right">
                                                <span id="span<%:type%>CurDayTitle"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="openum-detail-value-container openum-detail-value-container-left">
                                                <span id="span<%:type%>PrevWeekValue"></span>
                                            </td>
                                            <td class="openum-detail-value-container openum-detail-value-container-middle">
                                                <span id="span<%:type%>PrevDayValue"></span>
                                            </td>
                                            <td class="openum-detail-value-container openum-detail-value-container-right">
                                                <span id="span<%:type%>CurDayValue"></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            <% } %>
                        </tr>
                        <tr>
                            <% foreach (string type in opeTypes) {
                            %>
                                <td colspan="3" class="openum-chart">
                                    <%= Html.Kendo().Sparkline<TodayData>()
                                            .Name("chart" + type + "Today")
                                            .HtmlAttributes(new { style = "height: 100%; width: 80%;" }) 
                                            .Type(SparklineType.Bullet)
                                            .ValueAxis(axis => axis
                                                .Numeric()
                                                .Min(0)
                                                //.Max(7000)
                                                //.PlotBands(bands => {
                                                //    bands.Add().From(0).To(22).Color("#787878").Opacity(0.3);                                                
                                                //})
                                            )
                                            .Tooltip(tooltip => tooltip
                                                .Visible(true)
                                            )
                                            .DataSource(ds => 
                                            {
                                                ds.Read(read => read.Action("Read_Today", "Dashboard"));
                                                ds.Events(events =>
                                                {
                                                    events.RequestStart("On" + type + "TodayChartRequestStart");
                                                    events.RequestEnd("On" + type + "TodayChartRequestEnd");
                                                });
                                            })                                        
                                            .SeriesColors(new string[] {/*"#42a7ff", "#666666", "#999999"*/ "#70B5DD", "#1083C7", "#1C638D"})
                                            .Series( series => {
                                                if (type == "Parkings")
                                                {
                                                    series.Bar("ParkingsPrevWeek").Name("PrevWeek")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("1");
                                                    series.Bar("ParkingsPrevDay").Name("PrevDay")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("2");
                                                    series.Bar("ParkingsCurDay").Name("Today")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("3");
                                                }
                                                else
                                                {
                                                    series.Bar("PrevWeek").Name("PrevWeek")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("1");
                                                    series.Bar("PrevDay").Name("PrevDay")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("2");
                                                    series.Bar("CurDay").Name("Today")
                                                                                .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("3");
                                                }
                                                
                                                /*switch (type)
                                                {
                                                    case "Parkings":
                                                        {
                                                            series.Bar(o => o.PrevWeek).Name("PrevWeek")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("1");
                                                            series.Bar(o => o.PrevDay).Name("PrevDay")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("2");
                                                            series.Bar(o => o.CurDay).Name("Today")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("3");
                                                            break;
                                                        }
                                                    case "Parkings":
                                                        {
                                                            series.Bar(o => o.PrevWeek).Name("PrevWeek")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("1");
                                                            series.Bar(o => o.PrevDay).Name("PrevDay")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("2");
                                                            series.Bar(o => o.CurDay).Name("Today")
                                                                                        .Tooltip(tooltip => tooltip.Template("#= value #")).Stack("3");
                                                            break;
                                                        }
                                                        
                                                }*/
                                                
                                            })
                                            .AutoBind(false)       
                                            .RenderAs(RenderingMode.Canvas)
                                    %>
                                </td>
                            <% } %>
                        </tr>
                    </table>

                </div>
            <% });

        verticalPanes.Add()
            .HtmlAttributes(new { id = "splChartsToday2" })
            .Collapsible(true)
            .Content(() => { %>
                <div class="pane-content" style="height: 96%;">
                    <%= Html.Kendo().Chart<ChartDataItem>()
                            .Name("chartOperationsToday")
                            //.Title("Operations"
                            .HtmlAttributes(new { style = "height: 100%; width: 100%;" }) 
                            .Legend(legend => legend
                                .Position(ChartLegendPosition.Top).Visible(false)
                            )
                            .DataSource(ds => 
                            {
                                ds.Read(read => read.Action("Read_OperationsToday", "Dashboard"));
                                ds.Events(events =>
                                {
                                    events.RequestStart("OnOperationsTodayChartRequestStart");
                                    events.RequestEnd("OnOperationsTodayChartRequestEnd");
                                });                                                                                                
                            })
                            .SeriesDefaults(seriesDefaults =>
                                seriesDefaults.Column().Stack(true)
                            )
                            .SeriesColors(new string[] { "#42a7ff", "#666666", "#999999", "#cccccc" })
                            .Series(series =>
                            {
                                //series.Column(model => model.Total).Name("Operations");
                                series.Column(model => model.Serie0).Name(Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingOperation.ToString()));
                                series.Column(model => model.Serie1).Name(Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ExtensionOperation.ToString()));
                                series.Column(model => model.Serie2).Name(Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.ParkingRefund.ToString()));
                                series.Column(model => model.Serie3).Name(Resources.ResourceManager.GetString("ChargeOperationsTypes_" + ChargeOperationsType.TicketPayment.ToString()));
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
            <% });
                                           
    }).Render();
    %>

<script>

    function OnParkingsTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartParkingsToday"), true);
        $("#chartParkingsToday").attr("inprogress", "true");
    }
    function OnParkingsTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartParkingsToday"), false);
        $("#chartParkingsToday").attr("inprogress", "false");
        var data = e.response; //$("#chartParkingsToday").data("kendoSparkline").dataSource.data();
        if (data.length > 0) {
            //$("#chartParkingsToday").data("kendoSparkline").options.valueAxis.plotBands[0].to = data[0].Serie2;
            SetOperationTypeCss("Parkings", data);
            SetOperationTypeCss("Extensions", data);
            SetOperationTypeCss("Refunds", data);
            SetOperationTypeCss("Tickets", data);
            SetOperationTypeCss("Recharges", data);
        }
    }

    function OnExtensionsTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartExtensionsToday"), true);
        $("#chartExtensionsToday").attr("inprogress", "true");
    }
    function OnExtensionsTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartExtensionsToday"), false);
        $("#chartExtensionsToday").attr("inprogress", "false");
        var data = e.response;
        if (data.length > 0) {
            //$("#chartExtensionsToday").data("kendoSparkline").options.valueAxis.plotBands[0].to = data[0].Serie2;
            SetOperationTypeCss("Extensions", data);
        }
    }

    function OnRefundsTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartRefundsToday"), true);
        $("#chartRefundsToday").attr("inprogress", "true");
    }
    function OnRefundsTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartRefundsToday"), false);
        $("#chartRefundsToday").attr("inprogress", "false");
        var data = e.response;
        if (data.length > 0) {
            //$("#chartRefundsToday").data("kendoSparkline").options.valueAxis.plotBands[0].to = data[0].Serie2;
            SetOperationTypeCss("Refunds", data);
        }
    }

    function OnTicketsTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartTicketsToday"), true);
        $("#chartTicketsToday").attr("inprogress", "true");
    }
    function OnTicketsTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartTicketsToday"), false);
        $("#chartTicketsToday").attr("inprogress", "false");
        var data = e.response;
        if (data.length > 0) {
            //$("#chartTicketsToday").data("kendoSparkline").options.valueAxis.plotBands[0].to = data[0].Serie2;
            SetOperationTypeCss("Tickets", data);
        }
    }

    function OnRechargesTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartRechargesToday"), true);
        $("#chartRechargesToday").attr("inprogress", "true");
    }
    function OnRechargesTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartRechargesToday"), false);
        $("#chartRechargesToday").attr("inprogress", "false");
        var data = e.response;
        if (data.length > 0) {
            //$("#chartRechargesToday").data("kendoSparkline").options.valueAxis.plotBands[0].to = data[0].Serie2;
            SetOperationTypeCss("Recharges", data);
        }
    }

    function SetOperationTypeCss(opeType, data) {

        var CurDay = 0;
        eval('CurDay = data[0].' + opeType + 'CurDay');
        var PrevDay = 0;
        eval('PrevDay = data[0].' + opeType + 'PrevDay');
        var PrevWeek = 0;
        eval('PrevWeek = data[0].' + opeType + 'PrevWeek');

        var span = "span" + opeType + "CurDayValue";
        $("#" + span).removeClass("higher"); $("#" + span).removeClass("lower");
        $($("#" + span).parent()).removeClass("higher-container"); $($("#" + span).parent()).removeClass("lower-container");
        var cls = "equal";
        if (CurDay > PrevDay)
            cls = "higher";
        else if (CurDay < PrevDay)
            cls = "lower";
        $("#" + span).addClass(cls);
        $($("#" + span).parent()).addClass(cls + "-container");
        $("#" + span).text(CurDay);
        $("#span" + opeType + "CurDayTitle").html(data[0].CurDayDate);
        $("#chart" + opeType + "Today").data("kendoSparkline").options.series[2].name = data[0].CurDayDateLong;

        span = "span" + opeType + "PrevDayValue";
        $("#" + span).removeClass("higher"); $("#" + span).removeClass("lower");
        $($("#" + span).parent()).removeClass("higher-container-small"); $($("#" + span).parent()).removeClass("lower-container-small");
        var cls = "equal";
        if (PrevDay > PrevWeek)
            cls = "higher";
        else if (PrevDay < PrevWeek)
            cls = "lower";
        $("#" + span).addClass(cls);
        $($("#" + span).parent()).addClass(cls + "-container-small");
        $("#" + span).text(PrevDay);
        $("#span" + opeType + "PrevDayTitle").html(data[0].PrevDayDate);
        $("#chart" + opeType + "Today").data("kendoSparkline").options.series[1].name = data[0].PrevDayDateLong;

        span = "span" + opeType + "PrevWeekValue";
        $("#" + span).removeClass("higher"); $("#" + span).removeClass("lower");
        $($("#" + span).parent()).removeClass("higher-container"); $($("#" + span).parent()).removeClass("lower-container");
        var cls = "equal";
        /*if (data[0].Serie1 > data[0].Serie2)
            cls = "higher";
        else if (data[0].Serie1 < data[0].Serie2)
            cls = "lower";*/
        $("#" + span).addClass(cls);
        $($("#" + span).parent()).addClass(cls + "-container");
        $("#" + span).text(PrevWeek);
        $("#span" + opeType + "PrevWeekTitle").html(data[0].PrevWeekDate);
        $("#chart" + opeType + "Today").data("kendoSparkline").options.series[0].name = data[0].PrevWeekDateLong;

        if (opeType != "Parkings") {
            var dataSource = $("#chart" + opeType + "Today").data("kendoSparkline").dataSource;
            var dataItem = dataSource.at(0);
            if (dataSource != null) dataSource.remove(dataItem);
            dataSource.add({ PrevWeek: PrevWeek, PrevDay: PrevDay, CurDay: CurDay });
        }
    }

    function OnOperationsTodayChartRequestStart(e) {
        kendo.ui.progress($("#chartOperationsToday"), true);
        $("#chartOperationsToday").attr("inprogress", "true");
    }
    function OnOperationsTodayChartRequestEnd(e) {
        kendo.ui.progress($("#chartOperationsToday"), false);
        $("#chartOperationsToday").attr("inprogress", "false");
    }

    function OnSplChartsTodayResize() {
        if (dashboard.ready) setTimeout("ChartsRefresh(false)", 500);
    }

</script>