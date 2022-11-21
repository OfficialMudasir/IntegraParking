<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<backOffice.Models.BalanceTransferDataModel>" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Models" %>

<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="backOffice.Infrastructure.Security" %>
<%@ Import Namespace="PBPPlugin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>
    <%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Title", "Balance Transfers") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/balanceTransfer.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/grid.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content(RouteConfig.BasePath + "Scripts/grid.js?v1.0") %>"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>

    <br />

    <% using (Html.BeginForm("BalanceTransfer", "Tools", FormMethod.Post))
       { %>

    <div class="editor-form">
        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.SourceEmail)%>
            </div>
            <div class="editor-field">
                <%=
                    Html.TextBoxFor(m => m.SourceEmail, new { id="SourceEmail", @class = "k-input k-textbox", style = "width:100%;" })
                %>
                <%= Html.ValidationMessageFor(m => m.SourceEmail) %>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Password) %>
            </div>
            <div class="editor-field">
                <%= Html.PasswordFor(m => m.Password, new { id = "Password", @class = "k-input k-textbox" })%>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.Label(resBundle.GetString("PBPPlugin", "BalanceTransfer.Recipients", "Recipients")) %>
            </div>
            <div class="editor-field">
                <script type="text/x-kendo-tmpl" id="recipients-template">
                    <li style="display:inline;" >
                        # if (recipientAlt) {#
                            <div class="lirecipient lirecipient-alt">
                        # } else { #
                            <div class="lirecipient">
                        #}
                         recipientAlt = !recipientAlt;
                        #
                            <div class="lirecipient-left">
                                <img alt="image" class="k-image" src="<%= Url.Content(RouteConfig.BasePath + "Content/img/tools/user16.png") %>">
                                #: Email #
                            </div>
                            <div class="lirecipient-right">
                                <a class="k-icon removerecipient-icon" href="\#" 
                                   onclick="balanceTransfer_DeleteRecipient('#: Email #'); return false;"
                                   title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Title", "DeleteRecipient_Title") %>"></a>
                            </div>
                        </div>
                    </li>
                </script>
                <div class="list-recipients">
                    <button type="button" id="btn_Addrecipients" class="k-button k-button-icon"
                        onclick="balanceTransfer_AddRecipient(); return false;"
                        title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients") %>">
                        <span class="k-icon k-add"></span><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients", "AddRecipients") %>
                    </button>
                    <button type="button" id="btnRemoveAllRecipients" class="k-button k-button-icon"
                        onclick="balanceTransfer_DeleteAllRecipients(); return false;"
                        title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Title", "DeleteAllRecipients") %>">
                        <span class="k-icon k-delete"></span><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients", "DeleteAllRecipients") %>
                    </button>
                    <span id="RecipientsCount"></span>

                    <%: Html.Kendo().ListView<EmailDataModel>(Model.GetRecipientsModel())
                        .Name("lstRecipients")
                        .TagName("div")
                        .HtmlAttributes(new { id = "lstRecipients" })
                        .ClientTemplateId("recipients-template")
                            //.Selectable()
                        .DataSource(dataSource =>
                        {
                            dataSource.Read(read => read.Action("RecipientsTransfer_Read", "Tools" ));
                            dataSource.PageSize(10);
                            dataSource.Sort(conf => conf.Add("Email"));
                        })
                        //.AutoBind(true)
                        .Pageable(pa => pa.Enabled(true))                        
                    %>
                </div>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Amount) %>
            </div>
            <div class="editor-field">
                <%=
                    Html.Kendo().NumericTextBoxFor(m => m.Amount).Format("n2").Decimals(2)                                                    
                %>
                <%= Html.ValidationMessageFor(m => m.Amount) %>
            </div>
        </div>


        <div class="buttons-container">
            <div class="buttonsleft-container">
            </div>
            <div class="buttonsright-container">
                <button type="button" id="btnTransfer" class="k-button k-button-icon"
                    onclick="balanceTransfer_Transfer(); return false;"
                    title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer", "transfer") %>">
                    <span class="k-icon transfer-icon"></span><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer", "Transfer") %>
                </button>
            </div>

        </div>

    </div>

    <% } %>

    <div id="dlgAddRecipients">

        <div class="addrecipients-grid">
            <%
                var dateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
                var gridDateTimeformat = "{0:" + dateTimeFormat.ShortDatePattern + " " + dateTimeFormat.ShortTimePattern + "}";
            %>

            <% Html.Kendo().Grid<UserDataModel>()
        .Name("gridUsers")
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
            //columns.Bound(p => p.Plates).HtmlAttributes(new { title = "#= grid_IsNull(Plates) #" }).Width("150px");
            columns.Bound(p => p.Balance).ClientTemplate("#=users_FormatAmount(Balance, BalanceCurrencyIsoCode)#").Width("100px");
            columns.ForeignKey(p => p.BalanceCurrencyId, (IQueryable)ViewData["currencies"], "CurrencyID", "Name").Width("60px");
            columns.ForeignKey(p => p.PaymentMeanTypeId, (IList)ViewData["paymentMeanTypes"], "PaymentMeanTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.PaymentMeanSubTypeId, (IQueryable)ViewData["paymentMeanSubTypes"], "PaymentMeanSubTypeId", "Description").Width("100px");
            columns.ForeignKey(p => p.PaymentSuscriptionTypeId, (IList)ViewData["paymentSuscryptionTypes"], "PaymentSuscryptionTypeId", "Description").Width("100px");
        })
        .ToolBar(toolbar =>
        {
            toolbar.Template(() =>
            {                                
            %>
            <div style="float: left;">
                <%= Html.Label(resBundle.GetString("PBPPlugin", "BalanceTransfer.Users", "Users")) %>
            </div>
            <div class="gridFilter">
                <%= Html.Label(resBundle.GetString("PBPPlugin", "BalanceTransfer.Installations", "Installations")) %>
                <% /*if (FormAuthMemberShip.HelperService.InstallationsRoleAllowed("EMAILTOOL_READ").Count >= Model.InstallationsCount)
                   {*/
                       Html.Kendo().DropDownList()
                               .Name("installations")
                               .OptionLabel(resBundle.GetString("PBPPlugin", "BalanceTransfer.InstallationsAll", "InstallationsAll"))
                               .DataTextField("Description")
                               .DataValueField("InstallationId")
                               .AutoBind(false)
                               .Events(e => e.Change("balanceTransfer_installationsChange"))
                               .DataSource(ds =>
                               {
                                   ds.Read("Installations_Read", "Tools", new { role = "BALANCETRANSFERS_WRITE" });
                               }).Render();
                   /*}
                   else
                   {
                       Html.Kendo().DropDownList()
                               .Name("installations")
                               .DataTextField("Description")
                               .DataValueField("InstallationId")
                               .AutoBind(true)
                               .Events(e => e.Change("emailTool_installationsChange"))
                               .DataSource(ds =>
                               {
                                   ds.Read("Installations_Read", "Tools");
                               }).Render();
                   }*/
                %>
                <input id="Users_HdnFilterInfoTitle" type="hidden" value="<%= resBundle.GetString("PBPPlugin", "Grid_FilterInfo_Title", "FilterInfo_Title") %>" />
                <a id="Users_FilterInfo" class="imageToolbar filterDisabled k-icon "
                    href="#" onclick="balanceTransfer_GridUsersClearFilter(); return false;"
                    title=""></a>
            </div>
            <%
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
        .Selectable(a => a.Mode(GridSelectionMode.Multiple))
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
            .Read(read => read.Action("Users_Read", "Tools"))
            .ServerOperation(true)
            .Sort(sort => sort.Add("Username").Ascending())
        )
        .Resizable(resize => resize.Columns(true))
        .Reorderable(reorder => reorder.Columns(true))
        .HtmlAttributes(new { style = "height: 335px;", @class = "grid" })
                   /*.Events(ev =>
                   {
                       ev.DataBound("User_onDataBound");
                       ev.ColumnHide("User_onColumnHide");
                       ev.ColumnShow("User_onColumnShow");
                       ev.ColumnResize("User_onColumnResize");
                       ev.ColumnReorder("User_onColumnReorder");
                   })*/
        .Events(events =>
            {
                //events.Change("onChange");
                events.DataBound("balanceTransfer_GridUsersOnDataBound");
            })
        .ColumnMenu()
        .Render();       
            %>
        </div>

        <div id="divChks">
            <br />
            <input id="chkAllUsers" type="checkbox" onchange="balanceTransfer_SelectAllUsers(this.checked);" /><label for="chkAllUsers"><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.SelectAllUsers", "SelectAllUsers") %></label>
            </br />
            <input id="chkAllFilteredUsers" type="checkbox" onchange="balanceTransfer_SelectAllFilteredUsers(this.checked);" /><label for="chkAllFilteredUsers"><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.SelectAllFilteredUsers", "SelectAllFilteredUsers") %></label>
        </div>

        <div class="addrecipients-buttons">
            <button type="button" class="k-button k-button-icon"
                onclick="$('#dlgAddRecipients').data('kendoWindow').addRecipients = true; $('#dlgAddRecipients').data('kendoWindow').close(); return false;"
                title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Accept", "AddRecipients_Accept") %>">
                <span class="k-icon k-update"></span><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Accept", "AddRecipients_Accept") %>
            </button>

            <button type="button" class="k-button k-button-icon"
                onclick="$('#dlgAddRecipients').data('kendoWindow').addRecipients = false; $('#dlgAddRecipients').data('kendoWindow').close(); return false;"
                title="<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Cancel", "AddRecipients_Cancel") %>">
                <span class="k-icon k-cancel"></span><%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Cancel", "AddRecipients_Cancel") %>
            </button>
        </div>

    </div>

    <script>

        var recipientAlt = false;
        var gridUsersFilter = null;

        $(document).ready(function () {

            $("form").kendoValidator();

            var dlg = $("#dlgAddRecipients");
            if (!dlg.data("kendoWindow")) {
                dlg.kendoWindow({
                    title: "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "Add Recipients") %>",
                    modal: true,
                    width: "800px",
                    visible: false,
                    actions: [
                        //"Pin",
                        "Minimize",
                        "Maximize",
                        "Close"
                    ],
                    open: function () {
                        this.addRecipients = false;
                        $("#chkAllUsers").attr("checked", false);
                        $("#chkAllFilteredUsers").attr("checked", false);
                        $("#chkAllUsers").attr('disabled', false);
                        balanceTransfer_SelectAllUsers(false);
                        balanceTransfer_ResizeGrid();
                    },
                    close: function (e) {
                        if (e.userTriggered) this.addRecipients = false;
                        if (!balanceTransfer_AddRecipientsSave(this.addRecipients)) {
                            e.preventDefault();
                        }
                    }
                });
            }

            var lstRecipients = document.getElementById('lstRecipients');
            lstRecipients.onpaste = function (e) {
                balanceTransfer_PasteRecipients(e);
                return false; // Prevent the default handler from running.
            };
        });

        function balanceTransfer_EditorPaste(e) {
            //e.html = htmlDecode(e.html);
            //$("#body").val("<b>paste</b>");
            //var editor = $("#body").data("kendoEditor");
            //editor.value(e.html);
            //editor.value("<b>paste</b>");

        }
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }

        function balanceTransfer_RefreshListRecipients() {

            recipientAlt = false;
            var lstRecipients = $("#lstRecipients").data("kendoListView");
            lstRecipients.dataSource.read();
            //lstRecipients.refresh();
            lstRecipients.dataSource.page(1);

        }

        function balanceTransfer_SelectAllUsers(select) {
            var grid = $("#gridUsers").data("kendoGrid");
            if (select)
                grid.select("tr[role='row']");
            else
                grid.clearSelection();
        }

        function balanceTransfer_SelectAllFilteredUsers(select) {
            $("#chkAllUsers").attr('disabled', select);
            var grid = $("#gridUsers").data("kendoGrid");
            if (select) {
                grid.clearSelection();
            }
            else {
                //$(".addrecipients-grid *").enable();
                //grid.clearSelection();
            }
        }

        function balanceTransfer_AddRecipient() {

            //alert(kendo.stringify($("#lstRecipients").data("kendoListView").dataSource.data()));

            var dlg = $("#dlgAddRecipients").data("kendoWindow");
            dlg.center();
            dlg.open();

        }

        function balanceTransfer_AddRecipientsSave(save) {

            var bRet = true;

            if (save) {

                kendo.ui.progress($("#lstRecipients"), true);

                var grid = $("#gridUsers").data("kendoGrid");
                var selected = [];
                var applyFilter = ($('#chkAllFilteredUsers:checked').val() ? 1 : 0);

                var rows = grid.select();
                for (var i = 0; i < rows.length; i++) {
                    var data = grid.dataItem(rows[i]);
                    selected.push(data.Email);
                }

                // ***
                /*for (var i = rows.length; i < 70000; i++) {
                    selected[i] = selected[0].split("@")[0] + i + "@" + selected[0].split("@")[1];
                }*/
                // ***
                //alert(selected.join(", "));

                //var lstRecipients = $("#lstRecipients").data("kendoListView");
                //var data = lstRecipients.dataSource.data();

                var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
                   .options.parameterMap({
                       filter: grid.dataSource.filter()
                   });
                //href = href.replace(/filter=([^&]*)/, 'filter=' + (requestObject.filter || '~'));

                $.ajax({
                    timeout: 600000,
                    type: 'POST',
                    url: "<%= Url.Action("AddRecipientsTransfer", "Tools")%>",
                    data: { recipients: kendo.stringify(selected), filter: requestObject.filter, applyFilter: applyFilter },
                    success: function (response) {

                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);

                            if (response.Result == true) {
                                //emailTool_RefreshListRecipients();
                                //emailTool_GetAddRecipientsState();
                                setTimeout(function () {
                                    balanceTransfer_GetAddRecipientsState();
                                }, 2000);
                            }
                            else {
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %> </br>" + response.ErrorInfo, "warning");
                                kendo.ui.progress($("#lstRecipients"), false);
                            }

                        } catch (ex) {
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                            kendo.ui.progress($("#lstRecipients"), false);
                        }

                    },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                        kendo.ui.progress($("#lstRecipients"), false);
                    }
                });

                }

                return bRet;
            }

            function balanceTransfer_GetAddRecipientsState() {

                kendo.ui.progress($("#lstRecipients"), true);

                $.ajax({
                    type: 'GET',
                    url: "<%= Url.Action("GetAddRecipientsTransferStatus", "Tools")%>",
                    data: { plugin: 'PBPPlugin', uniqueId: '<%: Model.UniqueId %>' },
                    success: function (response) {

                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);
                            if (response.RecipientsCount != null) {
                                var msg = "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.StatusInfo", "Emails added: {0}") %>";
                            $("#RecipientsCount").html(msg.replace("{0}", response.RecipientsCount));
                        }
                        if (response.Result == true) {
                            kendo.ui.progress($("#lstRecipients"), false);
                            balanceTransfer_RefreshListRecipients();
                            if (response.AddingRecipientsResult == false)
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %> </br>" + response.ErrorInfo, "warning");
                        }
                        else {
                            setTimeout(function () {
                                balanceTransfer_GetAddRecipientsState();
                            }, 2000);
                        }

                    } catch (ex) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "balanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                        kendo.ui.progress($("#lstRecipients"), false);
                    }

                },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                    kendo.ui.progress($("#lstRecipients"), false);
                }
                });

            }

        function balanceTransfer_AddRecipientsPaste(pasteText) {

                var bRet = true;

                kendo.ui.progress($("#lstRecipients"), true);

                //var selected = pasteText.split(",");

                $.ajax({
                    type: 'POST',
                    url: "<%= Url.Action("AddRecipientsTransferPaste", "Tools")%>",
                    data: { plugin: 'PBPPlugin', recipients: kendo.stringify(pasteText) },
                    success: function (response) {

                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);

                            if (response.Result == true) {
                                setTimeout(function () {
                                    balanceTransfer_GetAddRecipientsState();
                                }, 2000);
                                //emailTool_RefreshListRecipients();
                            }
                            else {
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %> </br>" + response.ErrorInfo, "warning");
                            kendo.ui.progress($("#lstRecipients"), false);
                        }

                    } catch (ex) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                    kendo.ui.progress($("#lstRecipients"), false);
                }

                },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Title", "AddRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.AddRecipients.Error", "AddRecipients_Error") %>");
                    kendo.ui.progress($("#lstRecipients"), false);
                }
                });

                return bRet;
            }

            function balanceTransfer_DeleteRecipient(recipient) {
                balanceTransfer_DeleteRecipientConfirm(recipient);
            }
            function balanceTransfer_DeleteRecipientConfirm(recipient) {

                $.ajax({
                    type: 'POST',
                    url: "<%= Url.Action("DeleteRecipientTransfer", "Tools")%>",
                data: { recipient: recipient },
                success: function (response) {

                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);

                            if (response.Result == true) {
                                balanceTransfer_RefreshListRecipients();
                                balanceTransfer_GetAddRecipientsState();
                            }
                            else {
                                msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Title", "DeleteRecipient_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Error", "DeleteRecipient_Error") %>: " + response.ErrorInfo, "warning");
                            }

                        } catch (ex) {
                            //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.exception);
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Title", "DeleteRecipient_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Error", "DeleteRecipient_Error") %>");
                        }
                    },
                error: function (xhr) {
                    //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.communication);
                    msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Title", "DeleteRecipient_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteRecipient.Error", "DeleteRecipient_Error") %>");
                }
            });

        }

        function balanceTransfer_DeleteAllRecipients() {
            msgboxConfirm("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Title", "DeleteAllRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteEllRecipients.Confirm", "DeleteEllRecipients_Confirm") %>", "balanceTransfer_DeleteAllRecipients_Confirm()");
        }
        function balanceTransfer_DeleteAllRecipients_Confirm() {

            kendo.ui.progress($("#lstRecipients"), true);

            $.ajax({
                type: 'POST',
                url: "<%= Url.Action("DeleteAllRecipientsTransfer", "Tools")%>",
                data: {},
                success: function (response) {

                    kendo.ui.progress($("#lstRecipients"), false);

                    try {
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {
                            balanceTransfer_RefreshListRecipients();
                            balanceTransfer_GetAddRecipientsState();
                        }
                        else {
                            msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Title", "DeleteAllRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Error", "DeleteAllRecipients_Error") %>: " + response.ErrorInfo, "warning");
                        }

                    } catch (ex) {
                        //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.exception);
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Title", "DeleteAllRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Error", "DeleteAllRecipients_Error") %>");
                    }


                },
                error: function (xhr) {
                    //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.communication);
                    msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Title", "DeleteAllRecipients_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.DeleteAllRecipients.Error", "DeleteAllRecipients_Error") %>");
                    kendo.ui.progress($("#lstRecipients"), false);
                }
            });

            }

            function balanceTransfer_Transfer() {
                var validator = $("form").kendoValidator().data("kendoValidator");
                if (validator.validate()) {
                    balanceTransfer_Transfer_Confirm();
                }
            }
            function balanceTransfer_Transfer_Confirm() {

                var sSourceEmail = $("#SourceEmail").val();                
                var sPwd = $("#Password").val();
                var sAmount = $("#Amount").val();
                var fAmount = parseFloat(sAmount.replace(",", ".")).toFixed(2);                

                if (sSourceEmail != "" && sPwd != "" && fAmount > 0) {

                    kendo.ui.progress($(".editor-form"), true);

                    $.ajax({
                        type: 'POST',
                        url: "<%= Url.Action("Transfer", "Tools")%>",
                        data: { sourceEmail: sSourceEmail, password: sPwd, amount: fAmount },
                        success: function (response) {

                            try {

                                if (response.Result == true) {
                                    setTimeout(function () {
                                        balanceTransfer_GetTransferState();
                                    }, 2000);
                                }
                                else {
                                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>: " + response.ErrorInfo, "warning");
                                kendo.ui.progress($(".editor-form"), false);
                            }

                        } catch (ex) {
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>");
                            kendo.ui.progress($(".editor-form"), false);
                        }
                    },
                        error: function (xhr) {
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>");
                        kendo.ui.progress($(".editor-form"), false);
                    }
                    });
                }
                else {
                    if (sSourceEmail == "")
                        msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "BalanceTransferDataModel.SourceEmail", "Source Email")) %>", "warning");
                    else if (sPwd == "")
                        msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "BalanceTransferDataModel.Password", "Password")) %>", "warning");
                    else
                        msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= String.Format(resBundle.GetString("PBPPlugin", "ErrorsMsg_RequiredField", "RequiredField"), resBundle.GetString("PBPPlugin", "BalanceTransferDataModel.Amount", "Amount")) %>", "warning");
                }

            }

            function balanceTransfer_GetTransferState() {

                kendo.ui.progress($(".editor-form"), true);

                $.ajax({
                    type: 'GET',
                    url: "<%= Url.Action("GetTransferStatus", "Tools")%>",
                    data: { plugin: 'PBPPlugin', uniqueId: '<%: Model.UniqueId %>' },
                    success: function (response) {

                        try {

                            if (response.Result == true) {
                                kendo.ui.progress($(".editor-form"), false);

                                if (response.TransferResult) {
                                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Success", "Transfer_Success") %>", "information");
                                    balanceTransfer_DeleteAllRecipients_Confirm();
                                    $("#Password").val("");
                                    $("ul.k-upload-files").remove();
                                }
                                else {
                                    balanceTransfer_RefreshListRecipients();
                                    msgboxAlert("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>: " + response.ErrorInfo, "warning");
                                }
                            }
                            else {
                                setTimeout(function () {
                                    balanceTransfer_GetTransferState();
                                }, 2000);
                            }

                        } catch (ex) {
                            msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>");
                            kendo.ui.progress($(".editor-form"), false);
                        }

                    },
                    error: function (xhr) {
                        msgboxError("<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Title", "Transfer_Title") %>", "<%= resBundle.GetString("PBPPlugin", "BalanceTransfer.Transfer.Error", "Transfer_Error") %>");
                        kendo.ui.progress($(".editor-form"), false);
                    }
                });

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

            /*function onChange(arg) {
    
                //var selected = $.map(this.select(), function (item) {
                //    return $(item).text();
                //});
    
                var selected = [];
                var rows = this.select();
                for (var i = 0; i < rows.length; i++) {
                    var data = this.dataItem(rows[i]);
                    selected.push(data.Email);
                }
                //alert("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
    
            }*/

            function balanceTransfer_GridUsersOnDataBound(e) {

                var grid = $('#gridUsers').data('kendoGrid');

                gridUsersFilter = grid.dataSource.filter();

                grid_ShowFilterInfo('Users', gridUsersFilter);

                balanceTransfer_ResizeGrid();

            }

            function balanceTransfer_GridUsersClearFilter() {
                var grid = $("#gridUsers").data('kendoGrid');
                var dataSource = grid.dataSource;

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

                var state = {
                    page: dataSource.page(),
                    pageSize: dataSource.pageSize(),
                    sort: dataSource.sort(),
                    group: dataSource.group(),
                    filter: null,
                    hiddenColumns: hiddenColumns,
                    columnsWidth: columnsWidth,
                    columnsOrder: columnsOrder
                };

                $("#installations").data("kendoDropDownList").select(0);

                grid.dataSource.query(state);

            }

            function balanceTransfer_installationsChange() {
                var value = this.value(),
                     grid = $("#gridUsers").data("kendoGrid");

                if (value) {
                    grid.dataSource.filter({ field: "InstallationId", operator: "eq", value: parseInt(value) });
                } else {
                    grid.dataSource.filter({});
                }
            }

            function balanceTransfer_ResizeGrid() {
                var gridElement = $("#gridUsers"),
                    dataArea = gridElement.find(".k-grid-content"),
                    gridHeight = gridElement.innerHeight(),
                    otherElements = gridElement.children().not(".k-grid-content"),
                    otherElementsHeight = 0;
                /*otherElements.each(function () {
                    otherElementsHeight += $(this).outerHeight();
                });*/
                otherElementsHeight = 47;
                dataArea.height(gridHeight - otherElementsHeight);
            }

            function balanceTransfer_PasteRecipients(e) {
                var pastedText = undefined;
                if (window.clipboardData && window.clipboardData.getData) { // IE
                    pastedText = window.clipboardData.getData('Text');
                } else if (e.clipboardData && e.clipboardData.getData) {
                    pastedText = e.clipboardData.getData('text/plain');
                }
                //alert(pastedText); // Process and handle text...
                balanceTransfer_AddRecipientsPaste(pastedText);
            }

    </script>

</asp:Content>
