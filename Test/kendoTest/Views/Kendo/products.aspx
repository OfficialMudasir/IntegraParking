<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Kendo.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    products
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>products</h2>

    <%: Html.Kendo().Grid<kendoTest.Models.ClientProductViewModel>()
        .Name("grid")
        .Columns(columns =>
        {
            columns.Bound(p => p.ProductName);
            columns.ForeignKey(p => p.Category, (System.Collections.IEnumerable)ViewData["categories"], "CategoryID", "CategoryName")
                .Title("Category").ClientTemplate("#=Category.CategoryName#").EditorTemplateName("ClientCategory").Width(150);
            columns.Bound(p => p.UnitPrice).Width(150);
            columns.Command(command => command.Destroy()).Width(110);
        })
        .ToolBar(toolBar =>
            {
                toolBar.Save();
                toolBar.Create();
            })
        .Editable(editable => editable.Mode(GridEditMode.InCell))
        .Filterable()
        .Groupable()
        .Pageable()     
        .Scrollable()
        .Sortable()
        .HtmlAttributes(new { style = "height:430px;" })    
        .DataSource(dataSource => dataSource
            .Ajax()
            .Batch(true)
            .PageSize(20)
            .ServerOperation(false)
            .Events(events => events.Error("errorHandler"))
            .Model(model =>
            {
                model.Id(p => p.ProductID);
                model.Field(p => p.ProductID).Editable(false);
                model.Field(p => p.CategoryID).DefaultValue(1);       
            })
            .Read(read => read.Action("Products_Read", "Kendo"))
            .Update(update => update.Action("Products_Update", "Kendo"))
            .Create(create => create.Action("Products_Create", "Kendo"))
            .Destroy(destroy => destroy.Action("Products_Destroy", "Kendo"))
        )
    %>
    <script type="text/javascript">
        function errorHandler(e) {
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

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
