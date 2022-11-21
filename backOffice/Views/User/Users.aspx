<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<UserDataModel>>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Users_Title %>
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

    <% Html.Kendo().Grid<UserDataModel>(Model)
        .Name("gridUser")
        .Columns(columns =>
        {
            columns.Bound(p => p.Username).HtmlAttributes(new { title = "#= grid_IsNull(Username) #" }).Width("135px");
            columns.Bound(p => p.InsertionUTCDate).Format(gridDateTimeformat).Filterable(f => f.UI("grid_DateTimeFilter")).Width("135px");
            columns.Bound(p => p.Email).HtmlAttributes(new { title = "#= grid_IsNull(Email) #" }).Width("120px");
            columns.Bound(p => p.Name).HtmlAttributes(new { title = "#= grid_IsNull(Name) #" }).Width("100px");
            columns.Bound(p => p.Surname1).HtmlAttributes(new { title = "#= grid_IsNull(Surname1) #" }).Width("100px");
            columns.Bound(p => p.Surname2).HtmlAttributes(new { title = "#= grid_IsNull(Surname2) #" }).Width("100px");
            columns.Bound(p => p.DocId).HtmlAttributes(new { title = "#= grid_IsNull(DocId) #" }).Width("75px");
            columns.Bound(p => p.MainPhoneNumber).HtmlAttributes(new { title = "#= grid_IsNull(MainPhoneNumber) #" }).Width("100px");            
            columns.ForeignKey(p => p.MainPhoneCountryId, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("120px");
            columns.Bound(p => p.AlternativePhoneNumber).HtmlAttributes(new { title = "#= grid_IsNull(AlternativePhoneNumber) #" }).Width("100px");
            columns.ForeignKey(p => p.AlternativePhoneCountryID, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("120px");
            columns.Bound(p => p.StreetName).HtmlAttributes(new { title = "#= grid_IsNull(StreetName) #" }).Width("150px");
            columns.Bound(p => p.StreetNumber).Width("50px");
            columns.Bound(p => p.LevelInStreetNumber).Width("50px");
            columns.Bound(p => p.DoorInStreetNumber).Width("50px");
            columns.Bound(p => p.LetterInStreetNumber).Width("50px");
            columns.Bound(p => p.StairInStreetNumber).Width("50px");
            columns.ForeignKey(p => p.CountryId, (IQueryable)ViewData["countries"], "CountryId", "Description").Width("100px");
            columns.Bound(p => p.State).HtmlAttributes(new { title = "#= grid_IsNull(State) #" }).Width("100px");
            columns.Bound(p => p.City).HtmlAttributes(new { title = "#= grid_IsNull(City) #" }).Width("100px");
            columns.Bound(p => p.ZipCode).HtmlAttributes(new { title = "#= grid_IsNull(ZipCode) #" }).Width("100px");
            columns.Bound(p => p.Plates).HtmlAttributes(new { title = "#= grid_IsNull(Plates) #" }).Width("150px");
            columns.Bound(p => p.Balance).ClientTemplate("#=users_FormatAmount(Balance, BalanceCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.BalanceCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.ForeignKey(p => p.PaymentMeanTypeId, (IList)ViewData["paymentMeanTypes"], "PaymentMeanTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.PaymentMeanSubTypeId, (IQueryable)ViewData["paymentMeanSubTypes"], "PaymentMeanSubTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.PaymentSuscriptionTypeId, (IList)ViewData["paymentSuscryptionTypes"], "PaymentSuscryptionTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.Enabled, (IQueryable)ViewData["booleans"], "BooleanId", "Description").Width("50px");
            //columns.Command(cmd => cmd.Custom("Disable").Text("Inhabilitar").Click("users_CmdDisable")).Title("Acciones").Width("100px");
            if ((ConfigurationManager.AppSettings["DisableUser"] ?? "0") == "1")
                columns.Template(o => o).ClientTemplate("#if (Enabled == '1') {#<a class='k-button k-button-icontext k-grid-Disable' href='\\#' onClick='users_CmdDisable(#: UserId #, \"#: Username #\"); return false;' title=\"" + Resources.User_Disable_Button_Title + "\" ><span class='k-icon k-delete'></span>" + Resources.User_Disable_Button_Text + "</a>#}#").Width("100px");
            
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {
                Html.RenderPartial("../Shared/gridFilters", "User");
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
                m.Id(p => p.UserId);
            })
            .Read(read => read.Action("Users_Read", "User", new { gridInitialized = "false" }))
            .ServerOperation(true)
            .Sort(sort => sort.Add("Username").Ascending())
        )        
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 450px;", @class = "grid" })
        .Events(ev =>
        {
            ev.DataBound("UserMain_onDataBound");
            ev.ColumnHide("User_onColumnHide");
            ev.ColumnShow("User_onColumnShow");
            ev.ColumnResize("User_onColumnResize");
            ev.ColumnReorder("User_onColumnReorder");            
        })
        .ColumnMenu()
        .Render();       
    %>

    <script>

        $(document).ready(function () {

            grid_Ready("User");

        });

        function UserMain_onDataBound(e) {
            $(".k-grid-Disable").find("span").addClass("k-icon k-delete");            
            User_onDataBound(e);
        }

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

        function users_CmdDisable(userId, username) {
            var msgConfirm = "<%= Resources.User_Disable_Confirm %>";
            msgConfirm = msgConfirm.replace("{0}", username);
            msgboxConfirm("<%= Resources.User_Disable_Title %>", msgConfirm, "users_CmdDisableConfirm(" + userId + ")");
        }

        function users_CmdDisableConfirm(userId) {

            $.ajax({
                type: 'POST',
                url: '<%= Url.Action("User_Disable", "User")%>',
                data: { userId: userId },
                success: function (response) {

                    try {
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {
                            grid_Refresh("User");
                        }
                        else {
                            msgboxAlert("<%= Resources.User_Disable_Title %>", response.ErrorInfo, "warning");                            
                        }

                    } catch (ex) {
                        msgboxError("<%= Resources.User_Disable_Title %>", "<%= Resources.User_Disable_Error %>");
                    }
                },
                error: function (xhr) {
                    msgboxError("<%= Resources.User_Disable_Title %>", "<%= Resources.User_Disable_Error %>");
                }
            });

        }

    </script>

</asp:Content>

