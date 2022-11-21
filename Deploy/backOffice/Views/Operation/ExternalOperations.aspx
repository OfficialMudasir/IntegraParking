<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<ExternalOperationDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.ExternalOperations_Title %>
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
        .Name("gridExternalOperation")
        .Columns(columns =>
        {
            columns.Bound(p => p.Id).Width("50px");
            columns.Bound(p => p.Installation).Width("150px");
            columns.Bound(p => p.InstallationShortDesc).Width("100px");
            columns.Bound(p => p.Plate).Width("150px");
            columns.ForeignKey(p => p.ZoneId, (IQueryable)ViewData["groups"], "GroupId", (string)ViewData["groupsDescriptionField"]).Width("150px");
            columns.ForeignKey(p => p.TariffId, (IQueryable)ViewData["tariffs"], "TariffId", (string)ViewData["tariffsDescriptionField"]).Width("150px");
            columns.Bound(p => p.Date).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.IniDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.EndDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DateUTC).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.IniDateUTC).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.EndDateUTC).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Amount).ClientTemplate("#=externalOperations_FormatAmount(Amount, AmountCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.ForeignKey(p => p.AmountCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.Time).Width("75px");
            columns.ForeignKey(p => p.InsertionNotified, (IQueryable)ViewData["booleans"], "BooleanId", "Description").Width("50px");            
            columns.ForeignKey(p => p.EndingNotified, (IQueryable)ViewData["booleans"], "BooleanId", "Description").Width("50px");            
            columns.ForeignKey(p => p.ExternalProviderId, (IQueryable)ViewData["externalProviders"], "ExternalProviderId", "Name").Width("150px");
            columns.ForeignKey(p => p.SourceId, (IQueryable)ViewData["operationSourceTypes"], "OperationSourceTypeId", "Description").Width("100px");
            columns.Bound(p => p.SourceIdentifier).Width("75px");
            columns.ForeignKey(p => p.TypeId, (IQueryable)ViewData["chargeOperationTypes"], "ChargeOperationTypeId", "Description").Width("100px");
            columns.Bound(p => p.InsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.DateUTCOffset).Width("75px");
            columns.Bound(p => p.IniDateUTCOffset).Width("75px");
            columns.Bound(p => p.EndDateUTCOffset).Width("75px");
            columns.Bound(p => p.OperationId1).HtmlAttributes(new { title = "#= grid_IsNull(OperationId1) #" }).Width("50px");
            columns.Bound(p => p.OperationId2).HtmlAttributes(new { title = "#= grid_IsNull(OperationId2) #" }).Width("50px");
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "ExternalOperation");
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
            .Read(read => read.Action("ExternalOperations_Read", "Operation", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("Date").Descending())
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("ExternalOperation_onDataBound");
            ev.ColumnHide("ExternalOperation_onColumnHide");
            ev.ColumnShow("ExternalOperation_onColumnShow");
            ev.ColumnResize("ExternalOperation_onColumnResize");
            ev.ColumnReorder("ExternalOperation_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        function externalOperations_FormatAmount(amount, currencyIsoCode) {
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

            grid_Ready("ExternalOperation");
            
        });

    </script>

</asp:Content>

