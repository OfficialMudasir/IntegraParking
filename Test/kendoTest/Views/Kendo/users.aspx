<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Kendo.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="kendoTest.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    users
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendoTest/users.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users</h2>
    
<% Html.Kendo().Grid<UserAdminDataModel>()
    .Name("Grid")
    .Columns(columns =>
    {
        columns.Bound(p => p.Username); //.Groupable(false);
        columns.Bound(p => p.Email);
        columns.Bound(p => p.Country).ClientTemplate("${Country == null || Country.CountryID == null ? '' : Country.Description}");
        //columns.ForeignKey(p => p.Country, (IEnumerable<CountryDataModel>)ViewData["countries"], "CountryID", "Description").ClientTemplate("${Country == null || Country.CountryID == null ? '' : Country.Description}"/*"#=Currency.CurrencyName#"*/).EditorTemplateName("CountrySelector").Width(150);
        columns.Command(command => { command.Edit(); command.Destroy(); }).Width(160);      
    })
    /*.ToolBar(toolbar =>
    {
        toolbar.Template(() =>
        {
            Html.RenderPartial("filters");
        });
    }
    )*/
    .ToolBar(toolbar => toolbar.Create())
    .Editable(editable => editable.Mode(GridEditMode.PopUp).TemplateName("UserPopup"))
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
       //.Filterable()
    .DataSource(dataSource => dataSource
        .Ajax()
        .PageSize(10)
        .Events(events => events.Error("error_handler"))        
        .Model(model =>
        {
            model.Id(p => p.UserID);
            model.Field(p => p.UserID).Editable(false);
            model.Field(p => p.CountryID).DefaultValue(((CountryDataModel)ViewData["defaultCountry"]).CountryID);
        })        
        .Create(update => update.Action("Users_Create", "Kendo"))
        .Read(read => read.Action("Users_Read", "Kendo"))
        .Update(update => update.Action("Users_Update", "Kendo"))
        .Destroy(update => update.Action("Users_Destroy", "Kendo"))
        .ServerOperation(true)
        .Sort(sort => sort.Add("Username").Ascending()) // <-- initial sort expression
    )
    .Resizable(resize => resize.Columns(true))
    .Reorderable(reorder => reorder.Columns(true))
    .HtmlAttributes(new { style = "height: 530px;" })
    //.Events(ev => ev.DataBound("onDataBound"))
    .Render();
%>

<script type="text/javascript">

    var users_countryFlagUrl = '<%= Url.Action("CountryFlagImage") %>';

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

</asp:Content>
