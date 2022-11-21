<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<CustomerInscriptionDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.CustomerInscriptions_Title %>
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

    <% Html.Kendo().Grid<CustomerInscriptionDataModel>(Model)
        .Name("gridCustomerInscription")
        .Columns(columns =>
        {
            columns.Bound(p => p.Name).HtmlAttributes(new { title = "#= grid_IsNull(Name) #" }).Width("135px");
            columns.Bound(p => p.Surname1).HtmlAttributes(new { title = "#= grid_IsNull(Surname1) #" }).Width("100px");
            columns.Bound(p => p.Surname2).HtmlAttributes(new { title = "#= grid_IsNull(Surname2) #" }).Width("100px");
            columns.Bound(p => p.DocId).HtmlAttributes(new { title = "#= grid_IsNull(DocId) #" }).Width("75px");
            columns.Bound(p => p.MainPhoneNumber).HtmlAttributes(new { title = "#= grid_IsNull(MainPhoneNumber) #" }).Width("100px");
            columns.ForeignKey(p => p.MainPhoneCountryId, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("120px");
            columns.Bound(p => p.AlternativePhoneNumber).HtmlAttributes(new { title = "#= grid_IsNull(AlternativePhoneNumber) #" }).Width("100px");
            columns.ForeignKey(p => p.AlternativePhoneCountryID, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("120px");
            columns.Bound(p => p.Email).HtmlAttributes(new { title = "#= grid_IsNull(Email) #" }).Width("120px");
            columns.Bound(p => p.ActivationCode).HtmlAttributes(new { title = "#= grid_IsNull(ActivationCode) #" }).Width("80px");
            columns.Bound(p => p.Url).ClientTemplate("<a href=\"#= Url #\" target=\"_blank\">#= Url #</a>").HtmlAttributes(new { title = "#= grid_IsNull(Url) #" }).Width("120px");
            columns.Bound(p => p.ActivationRetries).HtmlAttributes(new { title = "#= grid_IsNull(ActivationRetries) #" }).Width("80px");            
            columns.Bound(p => p.LastSentTime).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Culture).HtmlAttributes(new { title = "#= grid_IsNull(Culture) #" }).Width("80px");
            columns.Bound(p => p.CustomerId).HtmlAttributes(new { title = "#= grid_IsNull(CustomerId) #" }).Width("100px");            
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "CustomerInscription");
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
                m.Id(p => p.Id);
            })
            .Read(read => read.Action("CustomerInscriptions_Read", "CustomerInscription", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("LastSentTime").Descending())
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("CustomerInscription_onDataBound");
            ev.ColumnHide("CustomerInscription_onColumnHide");
            ev.ColumnShow("CustomerInscription_onColumnShow");
            ev.ColumnResize("CustomerInscription_onColumnResize");
            ev.ColumnReorder("CustomerInscription_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        $(document).ready(function () {

            grid_Ready("CustomerInscription");

        });

        function users_FormatAmount(amount, currencyIsoCode) {
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

    </script>

</asp:Content>
