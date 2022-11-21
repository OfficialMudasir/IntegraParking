<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Kendo.Master" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="kendoTest.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    countries
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Countries</h2>

<% Html.Kendo().Grid<CountryDataModel>()
    .Name("Grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.CountryID).Width(100);
        columns.Bound(p => p.Description);
        columns.Bound(p => p.Code).Width(50);
        columns.Bound(p => p.TelPrefix).Width(125);        
        columns.ForeignKey(p => p.Currency, (IEnumerable<CurrencyDataModel>)ViewData["currencies"], "CurrencyID", "Name").ClientTemplate("${Currency == null || Currency.CurrencyID == null ? '' : Currency.Name}"/*"#=Currency.CurrencyName#"*/).EditorTemplateName("CurrencySelectorOptional").Width(150);
        columns.Command(command => { command.Edit().Text(" ").CancelText(" ").UpdateText(" "); command.Destroy().Text(" "); }).Width(100);
    })
    .ToolBar(toolbar => {
        toolbar.Create();        
    })
    .Editable(editable => editable.Mode(GridEditMode.InLine))
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
    .DataSource(dataSource => dataSource
        .Ajax()
        .PageSize(10)
        .Events(events => events.Error("error_handler"))
        .Model(model =>
        {
            model.Id(p => p.CountryID);
            model.Field(p => p.CountryID).Editable(false);
            model.Field(p => p.Currency).DefaultValue(((CurrencyDataModel)ViewData["defaultCurrency"]));
        })
        .Create(update => update.Action("Countries_Create", "Kendo"))
        .Read(read => read.Action("Countries_Read", "Kendo"))
        .Update(update => update.Action("Countries_Update", "Kendo"))
        .Destroy(update => update.Action("Countries_Destroy", "Kendo"))
        .ServerOperation(true)
        .Sort(sort => sort.Add("Description").Ascending()) // <-- initial sort expression
    )
    .Resizable(resize => resize.Columns(true))
    .Reorderable(reorder => reorder.Columns(true))
    .HtmlAttributes(new { style = "height: 530px;" })
    //.Events(ev => ev.DataBound("onDataBound"))
    .Render();
%>

<script type="text/javascript">
    function error_handler(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }
</script>

<style>
    .k-grid tbody .k-button {
        min-width: 22px;
        width: 35px;
    }
</style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
