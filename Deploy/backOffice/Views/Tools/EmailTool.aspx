<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EmailToolDataModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="backOffice.Properties" %>
<%@ Import Namespace="backOffice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.EmailTool_Title %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content("~/Content/emailTool.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + System.Threading.Thread.CurrentThread.CurrentCulture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/grid.js?v1.2") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        //set culture of the Kendo UI
        kendo.culture("<%: System.Threading.Thread.CurrentThread.CurrentCulture  %>");
    </script>

    <!--
    <h2><span class="k-icon emailtool-icon"></span>
        <%= Resources.EmailTool_Title %></h2>-->

    <br/>

    <% using(Html.BeginForm("SendEmails", "Tools", FormMethod.Post)) { %>

    <div class="editor-form">
        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Subject)%>
            </div>
            <div class="editor-field">
                <%=
                    Html.TextBoxFor(m => m.Subject, new { id="subject", @class = "k-input k-textbox", style = "width:100%;" })
                %>
                <%= Html.ValidationMessageFor(m => m.Subject) %>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Body) %>
            </div>
            <div class="editor-field">
                <%=
                    Html.Kendo().EditorFor(m => m.Body)
                                .Tools(tools => {
                                    tools.FontName();
                                    tools.FontSize();
                                    tools.FontColor();
                                    tools.SubScript();
                                    tools.SuperScript();
                                })
                                //.Encode(false)
                                .HtmlAttributes(new { id="body"}).Events( ev => ev.Paste("emailTool_EditorPaste"))                                
                %>
                <%= Html.ValidationMessageFor(m => m.Body) %>
            </div>
        </div>

        <div class="editor-container">
            <div class="editor-label">
                <%= Html.LabelFor(m => m.Recipients) %>
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
                                <img alt="image" class="k-image" src="<%= Url.Content("~/Content/img/tools/user16.png") %>">
                                #: Email #
                            </div>
                            <div class="lirecipient-right">
                                <a class="k-icon removerecipient-icon" href="\#" 
                                   onclick="emailTool_DeleteRecipient('#: Email #'); return false;"
                                   title="<%= Resources.EmailTool_DeleteRecipient_Title %>"></a>
                            </div>
                        </div>
                    </li>
                </script>
                <div class="list-recipients">
                    <button type="button" id="btn_Addrecipients" class="k-button k-button-icon" 
                            onclick="emailTool_AddRecipient(); return false;"
                            title="<%= Resources.EmailTool_AddRecipients_Title %>">
                        <span class="k-icon k-add">  </span><%= Resources.EmailTool_AddRecipients %>
                    </button>
                    <button type="button" id="btnRemoveAllRecipients" class="k-button k-button-icon" 
                            onclick="emailTool_DeleteAllRecipients(); return false;"
                            title="<%= Resources.EmailTool_DeleteAllRecipients %>">
                        <span class="k-icon k-delete">  </span><%= Resources.EmailTool_DeleteAllRecipients %>
                    </button>

                    <%: Html.Kendo().ListView<EmailDataModel>(Model.Recipients)
                        .Name("lstRecipients")
                        .TagName("div")
                        .HtmlAttributes(new { id = "lstRecipients" })
                        .ClientTemplateId("recipients-template")
                            //.Selectable()
                        .DataSource(dataSource =>
                        {
                            dataSource.Read(read => read.Action("Recipients_Read", "Tools"));
                            dataSource.PageSize(10);
                            dataSource.Sort(conf => conf.Add("Email"));
                        })
                        //.AutoBind(true)
                        .Pageable(pa => pa.Enabled(true))                        
                    %>
                </div>
                <div class="list-uploads">
                    <div class="title-uploads">
                        <%= Html.LabelFor(m => m.Attachments) %>                        
                    </div>
                    
                    <%= Html.Kendo().Upload()
                        .Name("uploads")                                                
                        .Async(a => a
                            .Save("SaveAttachment", "Tools")
                            .Remove("RemoveAttachment", "Tools")
                            .AutoUpload(true)
                        )
                    %>
                </div>
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

        <div class="buttons-container">
            <div class="buttonsleft-container">
            </div>
            <div class="buttonsright-container">
                <button type="button" id="btnSendEmails" class="k-button k-button-icon" 
                        onclick="emailTool_SendEmails(); return false;"
                        title="<%= Resources.EmailTool_Send %>">
                    <span class="k-icon sendemail-icon">  </span><%= Resources.EmailTool_Send %>
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
                //Html.RenderPartial("../Shared/gridFilters", "User");
                %>
                <div style="float:left;">
                    <%= Html.Label(Resources.EmailTool_Users) %>
                </div>
                <div class="gridFilter">
                    <%= Html.Label(Resources.EmailTool_Installations) %>
                    <%:Html.Kendo().DropDownList()
                                .Name("installations")
                                .OptionLabel(Resources.EmailTool_InstallationsAll)
                                .DataTextField("Description")
                                .DataValueField("InstallationId")
                                .AutoBind(false)
                                .Events(e => e.Change("emailTool_installationsChange"))
                                .DataSource(ds =>
                                {
                                    ds.Read("Installations_Read", "Tools");
                                })
                             %> 
                    <input id="Users_HdnFilterInfoTitle" type="hidden" value="<%= Resources.Grid_FilterInfo_Title %>" />
                    <a id="Users_FilterInfo" class="imageToolbar filterDisabled k-icon "
                        href="#" onclick="emailTool_GridUsersClearFilter(); return false;"
                        title="">        
                    </a>
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
                events.DataBound("emailTool_GridUsersOnDataBound");
            })
        .ColumnMenu()
        .Render();       
    %>            
        </div>

        <div id="divChks">
            <br />
            <input id="chkAllUsers" type="checkbox" onchange="emailTool_SelectAllUsers(this.checked);"/><label for="chkAllUsers"><%= Resources.EmailTool_SelectAllUsers %></label>
            </br/>
            <input id="chkAllFilteredUsers" type="checkbox" onchange="emailTool_SelectAllFilteredUsers(this.checked);"/><label for="chkAllFilteredUsers"><%= Resources.EmailTool_SelectAllFilteredUsers %></label>
        </div>

        <div class="addrecipients-buttons">
            <button type="button" class="k-button k-button-icon" 
                    onclick="$('#dlgAddRecipients').data('kendoWindow').addRecipients = true; $('#dlgAddRecipients').data('kendoWindow').close(); return false;"
                    title="<%= Resources.EmailTool_AddRecipients_Accept %>">
                <span class="k-icon k-update"></span><%= Resources.EmailTool_AddRecipients_Accept %>
            </button>

            <button type="button" class="k-button k-button-icon" 
                    onclick="$('#dlgAddRecipients').data('kendoWindow').addRecipients = false; $('#dlgAddRecipients').data('kendoWindow').close(); return false;"
                    title="<%= Resources.EmailTool_AddRecipients_Cancel %>">
                <span class="k-icon k-cancel"></span><%= Resources.EmailTool_AddRecipients_Cancel %>
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
                    title: "<%= Resources.EmailTool_AddRecipients_Title %>",
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
                        emailTool_SelectAllUsers(false);
                        emailTool_ResizeGrid();
                    },
                    close: function (e) {
                        if (e.userTriggered) this.addRecipients = false;
                        if (!emailTool_AddRecipientsSave(this.addRecipients)) {
                            e.preventDefault();
                        }
                    }
                });
            }

            var lstRecipients = document.getElementById('lstRecipients');
            lstRecipients.onpaste = function (e) {
                emailTool_PasteRecipients(e);
                return false; // Prevent the default handler from running.
            };
        });

        function emailTool_EditorPaste(e) {
            e.html = htmlDecode(e.html);
            //$("#body").val("<b>paste</b>");
            //var editor = $("#body").data("kendoEditor");
            //editor.value(e.html);
            //editor.value("<b>paste</b>");
            
        }
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }

        function emailTool_RefreshListRecipients() {

            recipientAlt = false;
            var lstRecipients = $("#lstRecipients").data("kendoListView");
            lstRecipients.dataSource.read();
            //lstRecipients.refresh();
            lstRecipients.dataSource.page(1);

        }

        function emailTool_SelectAllUsers(select) {
            var grid = $("#gridUsers").data("kendoGrid");
            if (select)
                grid.select("tr[role='row']");
            else
                grid.clearSelection();
        }

        function emailTool_SelectAllFilteredUsers(select) {
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

        function emailTool_AddRecipient() {

            //alert(kendo.stringify($("#lstRecipients").data("kendoListView").dataSource.data()));

            var dlg = $("#dlgAddRecipients").data("kendoWindow");
            dlg.center();
            dlg.open();

        }

        function emailTool_AddRecipientsSave(save) {

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
                //alert(selected.join(", "));

                //var lstRecipients = $("#lstRecipients").data("kendoListView");
                //var data = lstRecipients.dataSource.data();

                var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
                   .options.parameterMap({
                       filter: grid.dataSource.filter()
                   });                
                //href = href.replace(/filter=([^&]*)/, 'filter=' + (requestObject.filter || '~'));

                $.ajax({
                    type: 'POST',
                    url: "<%= Url.Action("AddRecipients", "Tools")%>",
                    data: { recipients: kendo.stringify(selected), filter: requestObject.filter, applyFilter: applyFilter },
                    success: function (response) {
                        
                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);

                            if (response.Result == true) {
                                emailTool_RefreshListRecipients();
                            }
                            else {
                                msgboxAlert("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %> </br>" + response.ErrorInfo, "warning");
                            }
                            
                        } catch (ex) {                            
                            msgboxError("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %>");
                        }

                        kendo.ui.progress($("#lstRecipients"), false);
                    },
                    error: function (xhr) {                        
                        msgboxError("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %>");
                        kendo.ui.progress($("#lstRecipients"), false);
                    }
                });

            }

            return bRet;
        }

        function emailTool_AddRecipientsPaste(pasteText) {

            var bRet = true;

            kendo.ui.progress($("#lstRecipients"), true);
                
            //var selected = pasteText.split(",");
               
            $.ajax({
                type: 'GET',
                url: "<%= Url.Action("AddRecipients", "Tools")%>",
                data: { recipients: kendo.stringify(pasteText) },
                success: function (response) {

                    try {
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {
                            emailTool_RefreshListRecipients();
                        }
                        else {
                            msgboxAlert("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %> </br>" + response.ErrorInfo, "warning");
                        }

                    } catch (ex) {
                        msgboxError("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %>");
                    }

                    kendo.ui.progress($("#lstRecipients"), false);
                },
                error: function (xhr) {
                    msgboxError("<%= Resources.EmailTool_AddRecipients_Title %>", "<%= Resources.EmailTool_AddRecipients_Error %>");
                    kendo.ui.progress($("#lstRecipients"), false);
                }
            });

            return bRet;
        }

        function emailTool_DeleteRecipient(recipient) {
            emailTool_DeleteRecipientConfirm(recipient);
        }
        function emailTool_DeleteRecipientConfirm(recipient) {

            $.ajax({
                type: 'POST',
                url: "<%= Url.Action("DeleteRecipient", "Tools")%>",
                data: { recipient: recipient},
                success: function (response) {

                    try {
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {
                            emailTool_RefreshListRecipients();
                        }
                        else {
                            msgboxAlert("<%= Resources.EmailTool_DeleteRecipient_Title %>", "<%= Resources.EmailTool_DeleteRecipient_Error %>: " + response.ErrorInfo, "warning");
                        }

                    } catch (ex) {
                        //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.exception);
                        msgboxError("<%= Resources.EmailTool_DeleteRecipient_Title %>", "<%= Resources.EmailTool_DeleteRecipient_Error %>");
                    }
                },
                error: function (xhr) {
                    //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.communication);
                    msgboxError("<%= Resources.EmailTool_DeleteRecipient_Title %>", "<%= Resources.EmailTool_DeleteRecipient_Error %>");
                }
            });

        }

        function emailTool_DeleteAllRecipients() {
            msgboxConfirm("<%= Resources.EmailTool_DeleteEllRecipients_Title %>", "<%= Resources.EmailTool_DeleteEllRecipients_Confirm %>", "emailTool_DeleteAllRecipients_Confirm()");            
        }
        function emailTool_DeleteAllRecipients_Confirm() {
            
            $.ajax({
                type: 'POST',
                url: "<%= Url.Action("DeleteAllRecipients", "Tools")%>",
                    data: { },
                    success: function (response) {

                        try {
                            //var oResponse = JSON.parse(response);
                            //eval("oReponse = " + response);

                            if (response.Result == true) {
                                emailTool_RefreshListRecipients();
                            }
                            else {
                                msgboxAlert("<%= Resources.EmailTool_DeleteEllRecipients_Title %>", "<%= Resources.EmailTool_DeleteAllRecipients_Error %>: " + response.ErrorInfo, "warning");
                            }

                        } catch (ex) {
                            //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.exception);
                            msgboxError("<%= Resources.EmailTool_DeleteEllRecipients_Title %>", "<%= Resources.EmailTool_DeleteAllRecipients_Error %>");
                        }
                    },
                    error: function (xhr) {
                        //msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.communication);
                        msgboxError("<%= Resources.EmailTool_DeleteEllRecipients_Title %>", "<%= Resources.EmailTool_DeleteAllRecipients_Error %>");

                    }
                });

        }

        function emailTool_SendEmails() {
            var validator = $("form").kendoValidator().data("kendoValidator");
            if (validator.validate()) {
                emailTool_SendEmails_Confirm();
            }
        }
        function emailTool_SendEmails_Confirm() {

            var sSubject = $("#subject").val();
            var sBody = $("#body").data("kendoEditor").value();
            var sPwd = $("#Password").val();
            
            if (sSubject != "" && sBody != "") {
                $.ajax({
                    type: 'POST',
                    url: "<%= Url.Action("SendEmails", "Tools")%>",
                    data: { subject: sSubject, body: kendo.htmlEncode(sBody), pwd: sPwd },
                    success: function (response) {

                        try {

                            if (response.Result == true) {
                                msgboxAlert("<%= Resources.EmailTool_SendEmails_Title %>", "<%= Resources.EmailTool_SendEmails_Success %>", "information");
                                emailTool_DeleteAllRecipients_Confirm();                                
                                $("#Password").val("");                                
                                $("ul.k-upload-files").remove();
                            }
                            else {
                                msgboxAlert("<%= Resources.EmailTool_SendEmails_Title %>", "<%= Resources.EmailTool_SendEmails_Error %>: " + response.ErrorInfo, "warning");
                            }

                        } catch (ex) {                            
                            msgboxError("<%= Resources.EmailTool_SendEmails_Title %>", "<%= Resources.EmailTool_SendEmails_Error %>");
                        }
                    },
                    error: function (xhr) {                        
                        msgboxError("<%= Resources.EmailTool_SendEmails_Title %>", "<%= Resources.EmailTool_SendEmails_Error %>");
                    }
                });
            }
            else {
                if (sSubject == "")
                    msgboxAlert("<%= Resources.EmailTool_SendEmails_Title %>", "<%= String.Format(Resources.ErrorsMsg_RequiredField, Resources.EmailToolDataModel_Subject) %>", "warning");
                else
                    msgboxAlert("<%= Resources.EmailTool_SendEmails_Title %>", "<%= String.Format(Resources.ErrorsMsg_RequiredField, Resources.EmailToolDataModel_Body) %>", "warning");
            }

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

        function emailTool_GridUsersOnDataBound(e) {

            var grid = $('#gridUsers').data('kendoGrid');

            gridUsersFilter = grid.dataSource.filter();

            grid_ShowFilterInfo('Users', gridUsersFilter);

            emailTool_ResizeGrid();

        }

        function emailTool_GridUsersClearFilter() {
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

        function emailTool_installationsChange() {
            var value = this.value(),
		 	     grid = $("#gridUsers").data("kendoGrid");

            if (value) {
                grid.dataSource.filter({ field: "InstallationId", operator: "eq", value: parseInt(value) });
            } else {
                grid.dataSource.filter({});
            }
        }

        function emailTool_ResizeGrid() {
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

        function emailTool_PasteRecipients(e) {
            var pastedText = undefined;
            if (window.clipboardData && window.clipboardData.getData) { // IE
                pastedText = window.clipboardData.getData('Text');
            } else if (e.clipboardData && e.clipboardData.getData) {
                pastedText = e.clipboardData.getData('text/plain');
            }
            //alert(pastedText); // Process and handle text...
            emailTool_AddRecipientsPaste(pastedText);
        }

    </script>

</asp:Content>
