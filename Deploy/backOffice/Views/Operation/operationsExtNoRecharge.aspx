<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<OperationExtNoRechargeDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Operations_Title %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/grid.js?v1.2") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");
    </script>

    <%
        var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        var gridDateTimeformat = "{0:" + dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern + "}";
    %>

    <% Html.Kendo().Grid(Model)
        .Name("gridOperationExtNoRecharge")
        .Columns(columns =>
        {
            //columns.Bound(p => p.TypeId).ClientTemplate("#=typeEnumValues[TypeId]#").Width("100px").Hidden(true); //.Groupable(false);
            columns.ForeignKey(p => p.TypeId, (IQueryable)ViewData["chargeOperationTypesNoRecharge"], "ChargeOperationTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.MobileOSId, (IQueryable)ViewData["mobileOSs"], "MobileOSId", "Description").Width("100px");
            columns.Bound(p => p.AppVersion).Width("100px");
            columns.Bound(p => p.Installation).Width("150px");
            columns.Bound(p => p.InstallationShortDesc).Width("100px");
            columns.Bound(p => p.DateUTC).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Date).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");            
            columns.Bound(p => p.DateIni).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DateEnd).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Amount).ClientTemplate("#=operationsExt_FormatAmount(Amount, AmountCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.ForeignKey(p => p.AmountCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.AmountFinal).ClientTemplate("#=operationsExt_FormatAmount(AmountFinal, BalanceCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.Bound(p => p.Time).Width("75px");
            columns.Bound(p => p.ChangeApplied).Format("{0:n6}").Width("75px");
            columns.Bound(p => p.Plate).Width("150px");
            columns.Bound(p => p.TicketNumber).Width("150px");
            columns.Bound(p => p.TicketData).Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(TicketData) #" });
            columns.ForeignKey(p => p.SectorId, (IQueryable)ViewData["groups"], "GroupId", (string)ViewData["groupsDescriptionField"]).Width("150px");
            columns.ForeignKey(p => p.TariffId, (IQueryable)ViewData["tariffs"], "TariffId", (string)ViewData["tariffsDescriptionField"]).Width("150px");
            //columns.Bound(p => p.SuscriptionType).Width("150px");
            columns.ForeignKey(p => p.SuscriptionType, (IList)ViewData["paymentSuscryptionTypes"], "PaymentSuscryptionTypeId", "Description").Width("100px");
            columns.Bound(p => p.InsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.ExternalId1).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId1) #" });
            columns.Bound(p => p.ExternalId2).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId2) #" });
            columns.Bound(p => p.ExternalId3).Width("100px").HtmlAttributes(new { title = "#= grid_IsNull(ExternalId3) #" });
            columns.Bound(p => p.Latitude).Format("{0:n6}").Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(Latitude) #" });
            columns.Bound(p => p.Longitude).Format("{0:n6}").Width("150px").HtmlAttributes(new { title = "#= grid_IsNull(Longitude) #" });            
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "OperationExtNoRecharge");
            });
        }
        )
        //.Groupable()
        .Pageable(pager => pager
            .Input(false)
            .Numeric(true)
            .Info(true)
            .PreviousNext(true)
            .Refresh(true)
            .PageSizes(true)            
        )
        .Sortable()
        .Scrollable()
           //.Scrollable(s => s.Height("auto"))
        .Filterable(filterable => filterable
            .Extra(true)
            /*.Operators(operators => operators
                .ForString(str => str.Clear()
                    .Contains("Contains")
                    .StartsWith("Starts with")
                    .IsEqualTo("Is equal to")
                    .IsNotEqualTo("Is not equal to")                    
                )
            )*/
        )
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(10)
            .Model(m =>
            {
                m.Id(p => p.Date);
            })
            .Read(read => read.Action("OperationsExtNoRecharge_Read", "Operation", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("DateUTC").Descending())
            .Events(ev =>
            {
                ev.RequestStart("OperationExtNoRecharge_onRequestStart");
            })            
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("OperationExtNoRecharge_onDataBound");
            ev.ColumnHide("OperationExtNoRecharge_onColumnHide");
            ev.ColumnShow("OperationExtNoRecharge_onColumnShow");
            ev.ColumnResize("OperationExtNoRecharge_onColumnResize");
            ev.ColumnReorder("OperationExtNoRecharge_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        var typeEnumValues = <%= backOffice.Controllers.OperationController.GetChargeOperationsTypeEnum() %>;

        function operationsExt_FormatAmount(amount, currencyIsoCode) {
            if (amount != null) {
                if (currencyIsoCode != null)
                    return kendo.format('{0:0.00} {1}', amount, currencyIsoCode);
                else {
                    if (amount != 0)
                        return kendo.format('{0:0.00}', amount);
                    else
                        return "";
                }
            }
            else
                return "";            
        }

        
        $(document).ready(function () {

            var defaultFilter = { 
                filters: [ { field: "DateUTC", operator: "", value: null, isDefaultFilter: true} ],
                logic: "and"};

            grid_Ready("OperationExtNoRecharge", defaultFilter);
            
        });

    </script>

</asp:Content>

