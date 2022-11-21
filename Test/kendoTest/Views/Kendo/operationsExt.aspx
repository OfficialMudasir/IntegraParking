<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Kendo.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<OperationExtModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="kendoTest.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Operations Ext
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");        
    </script>

    <h2>Operations Ext</h2>

    <p/>    

   
    <% Html.Kendo().Grid(Model)
        .Name("gridOperationsExt")
        .Columns(columns =>
        {
            //columns.Bound(p => p.TypeId).ClientTemplate("#=typeEnumValues[TypeId]#").Width("100px").Hidden(true); //.Groupable(false);
            columns.ForeignKey(p => p.TypeId, (System.Collections.IEnumerable)ViewData["chargeOperationTypes"], "ChargeOperationTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.UserId, (System.Collections.IEnumerable)ViewData["users"], "UserID", "Username").Width("100px");
            columns.Bound(p => p.Installation).Width("150px");
            columns.Bound(p => p.InstallationShortDesc).Width("100px");
            columns.Bound(p => p.Date).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            columns.Bound(p => p.DateIni).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            columns.Bound(p => p.DateEnd).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            columns.Bound(p => p.Amount).ClientTemplate("#=operationsExt_FormatAmount(Amount, AmountCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.ForeignKey(p => p.AmountCurrencyId, (System.Collections.IEnumerable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.AmountFinal).ClientTemplate("#=operationsExt_FormatAmount(AmountFinal, AmountCurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.Bound(p => p.Time).Width("75px");
            columns.Bound(p => p.BalanceBefore).Width("100px");
            columns.ForeignKey(p => p.BalanceCurrencyId, (System.Collections.IEnumerable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Bound(p => p.ChangeApplied).Format("{0:n6}").Width("75px");
            columns.Bound(p => p.Plate).Width("150px");
            columns.Bound(p => p.TicketNumber).Width("150px");
            columns.Bound(p => p.TicketData).Width("150px");
            columns.ForeignKey(p => p.SectorId, (System.Collections.IEnumerable)ViewData["groups"], "GroupId", "Description").Width("150px");
            columns.ForeignKey(p => p.TariffId, (System.Collections.IEnumerable)ViewData["tariffs"], "TariffId", "Description").Width("150px");
            columns.Bound(p => p.SuscriptionType).Width("150px");
            columns.Bound(p => p.InsertionUTCDate).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            
            //public decimal? RechargeId { get; set; }
            columns.Bound(p => p.RechargeDate).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            columns.Bound(p => p.RechargeAmount).ClientTemplate("#=operationsExt_FormatAmount(RechargeAmount, RechargeAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.RechargeAmountCurrencyId, (System.Collections.IEnumerable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            //public string RechargeAmountCurrencyIsoCode { get; set; }
            columns.Bound(p => p.RechargeBalanceBefore).ClientTemplate("#=operationsExt_FormatAmount(RechargeBalanceBefore, RechargeAmountCurrencyIsoCode)#").Width("100px");
            columns.Bound(p => p.RechargeInsertionUTCDate).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");

            //public decimal? DiscountId { get; set; }
            columns.Bound(p => p.DiscountDate).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");
            columns.Bound(p => p.DiscountAmount).ClientTemplate("#=operationsExt_FormatAmount(DiscountAmount, DiscountAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.DiscountAmountCurrencyId, (System.Collections.IEnumerable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            //public string DiscountAmountCurrencyIsoCode { get; set; }
            columns.Bound(p => p.DiscountAmountFinal).ClientTemplate("#=operationsExt_FormatAmount(DiscountAmountFinal, DiscountAmountCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.DiscountBalanceCurrencyId, (System.Collections.IEnumerable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            //public string DiscountBalanceCurrencyIsoCode { get; set; }
            columns.Bound(p => p.DiscountBalanceBefore).ClientTemplate("#=operationsExt_FormatAmount(DiscountBalanceBefore, DiscountBalanceCurrencyIsoCode)#").Width("100px");
            columns.Bound(p => p.DiscountChangeApplied).Format("{0:n6}").Width("150px");
            columns.Bound(p => p.DiscountInsertionUTCDate).Format("{0:dd/MM/yy HH:mm:ss}").Width("135px");

            columns.ForeignKey(p => p.ServiceChargeTypeId, (System.Collections.IEnumerable)ViewData["serviceChargeTypes"], "ServiceChargeId", "Description").Width("100px");
            
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("filters", "gridOperationsExt");
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
            .Extra(false)
            .Operators(operators => operators
                .ForString(str => str.Clear().Contains("Contains"))
            )
        )
        .DataSource(dataSource => dataSource
            .Ajax()
            .PageSize(10)
            .Model(m =>
            {
                m.Id(p => p.Date);
            })
            .Read(read => read.Action("UserOperationsExt_Read", "Kendo"))
            .ServerOperation(true)
            .Sort(sort => sort.Add("Date").Ascending()) // <-- initial sort expression
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;" })
        .Events(ev =>
        {
            ev.DataBound("operationsExt_onDataBound");
            ev.ColumnHide("operationsExt_onColumnHide");
            ev.ColumnShow("operationsExt_onColumnShow");
            ev.ColumnResize("operationsExt_onColumnResize");
            ev.ColumnReorder("operationsExt_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        var typeEnumValues = <%= kendoTest.Controllers.KendoController.GetTypesEnum() %>;

        function operationsExt_FormatAmount(amount, currencyIsoCode) {
            if (amount != null) {
                return kendo.format('{0:0.##} {1}', amount, currencyIsoCode);
            }
            else
                return "";
            
        }

        $(document).ready(function () {

            var grid = $('#gridOperationsExt').data('kendoGrid');

            var state = JSON.parse($.cookie("gridOperationsExtState"));
            if (state) {

                // Set columns visibility
                for (var iCol = 0; iCol < grid.columns.length; iCol++) {
                    if (state.hiddenColumns.indexOf(grid.columns[iCol].field) == -1)
                        grid.showColumn(grid.columns[iCol].field);
                    else
                        grid.hideColumn(grid.columns[iCol].field);
                }

                // Set columns width
                if (state.columnsWidth != null) {
                    operationsExt_SetColumnsWidth(state.columnsWidth);
                }

                if (state.columnsOrder != null) {
                    var column;
                    for (var iDest = 0; iDest < state.columnsOrder.length; iDest++) {
                        column = null;
                        for (var iCol = 0; iCol < grid.columns.length && column==null; iCol++) {
                            if (grid.columns[iCol].field == state.columnsOrder[iDest]) {
                                column = grid.columns[iCol];
                            }
                        }
                        grid.reorderColumn(iDest, column);
                    }
                }
                    
                // Set grid sort, filter, group, page and pagesize
                if (state.filter) {
                    operationsExt_ParseFilterDates(state.filter, grid.dataSource.options.schema.model.fields);
                }
                grid.dataSource.query(state);
                //grid.dataSource.filter(state.filter);

            }
            else {
                grid.dataSource.read();
            }

        });

        var operationsExt_FirstDataBound = true;

        function operationsExt_onDataBound(e) {
            
            operationsExt_UpdateExportLinks();

            if (operationsExt_FirstDataBound == false) {                
                var grid = $('#gridOperationsExt').data('kendoGrid'); // this
                var dataSource = grid.dataSource;  //this.dataSource;

                var hiddenColumns = [];
                for (var iColumn = 0; iColumn < grid.columns.length; iColumn++) {
                    if (grid.columns[iColumn].hidden == true) {
                        hiddenColumns.push(grid.columns[iColumn].field);
                    }
                }                

                var columnsWidth = [];
                for (var iColumn = 0; iColumn < grid.columns.length; iColumn++) {
                    if (grid.columns[iColumn].width != undefined && grid.columns[iColumn].width != null) {
                        columnsWidth.push(grid.columns[iColumn].field + ";" + grid.columns[iColumn].width);
                    }
                }

                var columnsOrder = [];
                for (var iColumn = 0; iColumn < grid.columns.length; iColumn++) {
                    columnsOrder.push(grid.columns[iColumn].field);
                }

                var state = kendo.stringify({
                    page: dataSource.page(),
                    pageSize: dataSource.pageSize(),
                    sort: dataSource.sort(),
                    group: dataSource.group(),
                    filter: dataSource.filter(),
                    hiddenColumns: hiddenColumns,
                    columnsWidth: columnsWidth,
                    columnsOrder: columnsOrder
                });                

                $("#gridOperationsExtFilterText").text(dataSource.filter()); // TODO: convertir filter a text per mostrar per pantalla.

                $.cookie("gridOperationsExtState", state);                
            }
            else
                operationsExt_FirstDataBound = false;
        }

        function operationsExt_onColumnHide(e) {
            
            var grid = $('#gridOperationsExt').data('kendoGrid');

            var state = JSON.parse($.cookie("gridOperationsExtState"));
            if (state) {
                var bExist = false;
                for (var iColHidden = 0; iColHidden < state.hiddenColumns.length && !bExist; iColHidden++) {
                    if (e.column.field == state.hiddenColumns[iColHidden])
                        bExist = true;
                }
                if (!bExist) {
                    state.hiddenColumns.push(e.column.field);
                    $.cookie("gridOperationsExtState", kendo.stringify(state));
                    operationsExt_UpdateExportLinks();
                }
            }            

        }

        function operationsExt_onColumnShow(e) {
                        
            var grid = $('#gridOperationsExt').data('kendoGrid');

            var state = JSON.parse($.cookie("gridOperationsExtState"));
            if (state) {
                var bExist = false;
                for (var iColHidden = 0; iColHidden < state.hiddenColumns.length && !bExist; iColHidden++) {
                    if (e.column.field == state.hiddenColumns[iColHidden])
                        bExist = true;
                }
                if (bExist) {
                    state.hiddenColumns.splice(iColHidden - 1, 1);                        
                    $.cookie("gridOperationsExtState", kendo.stringify(state));
                    operationsExt_UpdateExportLinks();
                }
            }            

        }

        function operationsExt_onColumnResize(e) {

            var state = JSON.parse($.cookie("gridOperationsExtState"));
            if (state) {
                var bExist = false;
                for (var iCol = 0; iCol < state.columnsWidth.length && !bExist; iCol++) {
                    if (e.column.field == state.columnsWidth[iCol].split(";")[0])
                        bExist = true;
                }
                if (bExist) {
                    state.columnsWidth[iCol - 1] = e.column.field + ";" + e.newWidth + "px";
                }
                else
                    state.columnsWidth.push(e.column.field + ";" + e.newWidth + "px");
                $.cookie("gridOperationsExtState", kendo.stringify(state));
            }

        }

        function operationsExt_onColumnReorder(e) {
            var state = JSON.parse($.cookie("gridOperationsExtState"));
            if (state) {
                state.columnsOrder.splice(e.oldIndex, 1);
                state.columnsOrder.splice(e.newIndex, 0, e.column.field);
                $.cookie("gridOperationsExtState", kendo.stringify(state));
                operationsExt_UpdateExportLinks();
            }
        }

        function operationsExt_UpdateExportLinks() {

            var grid = $('#gridOperationsExt').data('kendoGrid');

            var columns = [];
            for (var iColumn = 0; iColumn < grid.columns.length; iColumn++) {
                if (grid.columns[iColumn].hidden != true) {
                    columns.push(grid.columns[iColumn].field);
                }
            }

            // ask the parameterMap to create the request object for you
            var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
            .options.parameterMap({
                page: grid.dataSource.page(),
                sort: grid.dataSource.sort(),
                filter: grid.dataSource.filter(),
                columns: columns
            });

            // Get the export link as jQuery object
            var $exportLink = $('.export');

            // Get its 'href' attribute - the URL where it would navigate to
            var href = $exportLink.attr('href');

            // Update the 'page' parameter with the grid's current page
            href = href.replace(/page=([^&]*)/, 'page=' + requestObject.page || '~');

            // Update the 'sort' parameter with the grid's current sort descriptor
            href = href.replace(/sort=([^&]*)/, 'sort=' + requestObject.sort || '~');

            // Update the 'pageSize' parameter with the grid's current pageSize
            href = href.replace(/pageSize=([^&]*)/, 'pageSize=' + grid.dataSource._pageSize);

            //update filter descriptor with the filters applied
            href = href.replace(/filter=([^&]*)/, 'filter=' + (requestObject.filter || '~'));

            href = href.replace(/columns=([^&]*)/, 'columns=' + (requestObject.columns || '~'));

            // Update the 'href' attribute
            $exportLink.attr('href', href);

            $('#exportPdf').attr('href', href.replace(/xls/, 'pdf'));

        }

        function operationsExt_ParseFilterDates(filter, fields) {
            if (filter.filters) {
                for (var i = 0; i < filter.filters.length; i++) {
                    operationsExt_ParseFilterDates(filter.filters[i], fields);
                }
            }
            else {
                if (fields[filter.field].type == "date") {
                    filter.value = kendo.parseDate(filter.value);
                }
            }
        }

        function operationsExt_SetColumnsWidth(widthColumns) {

            var field, width,
                th, index,
                headerTable = $('#gridOperationsExt.k-grid .k-grid-header table'),
                contentTable = $('#gridOperationsExt.k-grid .k-grid-content table');


            for (var i = 0; i < widthColumns.length; i++) {
                field = widthColumns[i].split(";")[0];
                width = widthColumns[i].split(";")[1];
                th = $(headerTable).find('th[data-field="' + field + '"]');
                if (th.length) {
                    index = $.inArray(th[0], th.parent().children(":visible"));
                    col = headerTable.find('colgroup col:eq(' + index + ')')
                        .add(contentTable.find('colgroup col:eq(' + index + ')'))
                    ;
                    col.css('width', width);
                }
            }

        }        

    </script>

    <style scoped="scoped">
        #Grid .k-toolbar
        {
            min-height: 27px;
            padding: 1.3em;
        }
        .type-label
        {
            vertical-align: middle;
            padding-right: .5em;
        }
        #type
        {
            vertical-align: middle;
        }
        .filter {
            float: right;
        }

        .k-grid tbody tr{
            height: 25px;
        }
 
        .k-grid td{
            white-space: nowrap;
        }

    </style>

</asp:Content>

