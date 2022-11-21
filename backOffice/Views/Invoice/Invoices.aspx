<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<InvoiceDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Invoices_Title %>
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

    <% Html.Kendo().Grid<InvoiceDataModel>(Model)
        .Name("gridInvoice")
        .Columns(columns =>
        {            
            //columns.ForeignKey(p => p.UserId, (IQueryable)ViewData["users"], "UserId", "Username").Width("120px");
            columns.Bound(p => p.Username).Width("120px");
            columns.Bound(p => p.InvoiceNumber).ClientTemplate("#= InvoiceNumberFormatted #").Width("150px");
            columns.Bound(p => p.Date).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Amount).ClientTemplate("#=invoices_FormatAmount(Amount, CurrencyIsoCode)#").Width("100px")/*.Format("{0:0.##}")*/;
            columns.ForeignKey(p => p.CurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.Command(command => command.Custom("InvoiceExport").Text(" ").Click("invoices_ExportInvoice")
                                            .HtmlAttributes(new { title = Resources.InvoiceDataModel_DownloadURL } )
                                              /*.HtmlAttributes(new { @class = "k-icon k-edit" })*/);
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "Invoice");
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
                m.Id(p => p.InvoiceId);
            })
            .Read(read => read.Action("Invoices_Read", "Invoice", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("Date").Descending())
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("Invoice_onDataBound");
            ev.ColumnHide("Invoice_onColumnHide");
            ev.ColumnShow("Invoice_onColumnShow");
            ev.ColumnResize("Invoice_onColumnResize");
            ev.ColumnReorder("Invoice_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        function invoices_FormatAmount(amount, currencyIsoCode) {
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

            grid_Ready("Invoice");

        });

        function invoices_ExportInvoice(e) {

            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            
            var url = '<%= Url.Action("Export", "Invoice", new { invoiceId = "INVOICEIDPARAM" })%>';
            url = url.replace("INVOICEIDPARAM", dataItem.InvoiceId);

            window.open(url, '_blank');

        }

    </script>

    <style scoped="scoped">

        .k-grid tbody .k-button {
            min-width:0px
        }
        .k-grid-InvoiceExport span {
            background-image: Url(../Content/img/grid/exportPdf.png);
            width:16px;
            height:16px;            
            display:inline-block;            
            background-size: contain;
            /*margin-top: 1px;*/
        }

    </style>

</asp:Content>

