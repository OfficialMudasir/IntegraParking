<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<UserSecurityOperationDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.UserSecurityOperation_Title %>
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

    <% Html.Kendo().Grid<UserSecurityOperationDataModel>(Model)
        .Name("gridUserSecurityOperation")
        .Columns(columns =>
        {
            columns.ForeignKey(p => p.OpType, (IQueryable)ViewData["securityOperationTypes"], "Id", "Description").Width("100px");
            columns.ForeignKey(p => p.Status, (IQueryable)ViewData["securityOperationStatus"], "Id", "Description").Width("100px");
            columns.Bound(p => p.UtcDateTime).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Username).HtmlAttributes(new { title = "#= grid_IsNull(Username) #" }).Width("135px");
            columns.Bound(p => p.NewMainPhoneNumber).HtmlAttributes(new { title = "#= grid_IsNull(NewMainPhoneNumber) #" }).Width("100px");
            columns.ForeignKey(p => p.NewMainPhoneCountryId, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("120px");
            columns.Bound(p => p.NewEmail).HtmlAttributes(new { title = "#= grid_IsNull(NewEmail) #" }).Width("120px");
            columns.Bound(p => p.UrlParameter).ClientTemplate("<a href=\"#= userSecurityOperations_Url(UrlPrefix, UrlParameter) #\" target=\"_blank\">#= userSecurityOperations_Url(UrlPrefix, UrlParameter) #</a>").HtmlAttributes(new { title = "#= grid_IsNull(userSecurityOperations_Url(UrlPrefix, UrlParameter)) #" }).Filterable(false).Width("120px");
            columns.Bound(p => p.ActivationCode).HtmlAttributes(new { title = "#= grid_IsNull(ActivationCode) #" }).Width("135px");
            columns.Bound(p => p.ActivationRetries).HtmlAttributes(new { title = "#= grid_IsNull(ActivationRetries) #" }).Width("50px");
            columns.Bound(p => p.LastSendDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "UserSecurityOperation");
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
            .Read(read => read.Action("UserSecurityOperations_Read", "User", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("LastSendDate").Descending())
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("UserSecurityOperation_onDataBound");
            ev.ColumnHide("UserSecurityOperation_onColumnHide");
            ev.ColumnShow("UserSecurityOperation_onColumnShow");
            ev.ColumnResize("UserSecurityOperation_onColumnResize");
            ev.ColumnReorder("UserSecurityOperation_onColumnReorder");
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        $(document).ready(function () {

            grid_Ready("UserSecurityOperation");

        });

        function userSecurityOperations_Url(prefix, parameter) {
            return prefix.replace('{0}', parameter);
        }

    </script>

</asp:Content>
