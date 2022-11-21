<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SecurityPlugin.Models.SecurityUsersModel>" %>
<%@ Import Namespace="System.Globalization" %>
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
<%@ Import Namespace="SecurityPlugin" %>
<%@ Import Namespace="SecurityPlugin.Models" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/users.css?v2.0") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/treeHelper.js?v1.0") %>"></script>
    <script src="<%= Url.Content(RouteConfig.BasePath + "Scripts/users.js?v1.6") %>"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    <%= resBundle.GetString("Security", "UsersView.Title", "Users") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
<% var resBundleBackOffice = ResourceBundle.GetInstance("backOffice"); %>
    
<% Html.Kendo().Splitter()
      .Name("splMain")
      .Orientation(SplitterOrientation.Horizontal)
      .HtmlAttributes(new { style = "" })
      .Panes(verticalPanes =>
      {
          verticalPanes.Add()
              .HtmlAttributes(new { id = "left-pane" })
              .Size("250px")
              .Scrollable(false)
              .Collapsible(false)
              .Content(() =>
              { %>

                <div class="treeview-container" style="height: 100%;">
                    
                    <div class="treeSecurity-top">                        

                        <%= Html.Kendo().AutoComplete()
                            .Name("txtFindUser")
                            .DataTextField("UserName")
                            .Filter("contains")
                            .MinLength(2)
                            .HtmlAttributes(new { @class = "findUser" })
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetFindUsers", "Security", new { plugin = "SecurityPlugin" })
                                        .Data("FindUserOnAdditionalData");
                                })
                                .ServerFiltering(true);
                            })
                            .Events(e => {
                                e.Select("FindUserOnSelect");
                            })
                            .Template("<img src=\"#:data.ImageUrl#\" alt=\"#:data.UserName#\" />#:data.UserName#")
                        %>

                    </div>

                    <%=Html.Kendo().TreeView()                    
                        .Name("treeSecurity")                       
                        .DataTextField("name")
                        .DataImageUrlField("imageUrl")
                        .HtmlAttributes(new { style = "height: 100%;" })
                        .DataSource(source =>
                        {
                            source.Read(read => read.Action("TreeSecurityData", "Security", new { plugin = "SecurityPlugin" }));
                        })
                        .DragAndDrop(true)
                        .Events(ev => ev.Select("OnSecuritySelect")
                                        .DataBound("OnSecurityDataBound")
                                        .Drag("OnSecurityDrag")
                                        .DragEnd("OnSecurityDragEnd")
                                        .Drop("OnSecurityDrop"))
                    %>
                </div>

           <% });

          verticalPanes.Add()
              .HtmlAttributes(new { id = "right-pane" })
              .Collapsible(false)              
              .Content(() =>
              { %>              

                <%= Html.Kendo().Notification()
                    .Name("spnNotification")
                    .Stacking(NotificationStackingSettings.Down)
                    .Events(e => e.Show("onNotificationShow"))
                    .Button(true)
                %>

                <% Html.Kendo().TabStrip()
                      .Name("tabs")                      
                      .HtmlAttributes(new { id = "tabs", @class = "form-class" })
                      .Items(tabstrip =>
                      {
                            tabstrip.Add().Text(resBundle.GetString("Security", "UsersView.Title.General", "General"))
                                .Selected(true)
                                .HtmlAttributes(new { id = "tabGeneral", tabIndex = 0 })                                
                                .Content(() =>
                                {
                                %>
                                <!--<div id="divButtons">
                                    <div id="divToolbar"></div>
                                </div>-->
                                <!--<span id="spnNotification"></span>-->
                                <form id="frmGeneral" class="k-block k-info-colored" style="height: 100%; overflow: auto; ">
                                    <div id="divUser" class="editor-form k-edit-form-container" data-bind="visible: showUserForm" >
                                        <!--<div class="k-header k-shadow"> User Information </div>-->
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Username", resBundle.GetString("Security", "UsersView.User.Username", "User name")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Username", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Username, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2"} , { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" } }) %>
                                                <%= Html.ValidationMessage("User_Username") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Name", resBundle.GetString("Security", "UsersView.User.Name", "Name")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Name", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Name, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" } }) %>
                                                <%= Html.ValidationMessage("User_Name") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Surname1", resBundle.GetString("Security", "UsersView.User.Surname1", "Surname1")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Surname1", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Surname1, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                <%= Html.ValidationMessage("User_Surname1") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Surname2", resBundle.GetString("Security", "UsersView.User.Surname2", "Surname2")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Surname2", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Surname2, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                <%= Html.ValidationMessage("User_Surname2") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Pwd", resBundle.GetString("Security", "UsersView.User.Password", "Password")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.Password("User_Pwd", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Pwd, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" }, { "data-max-msg", "please enter a number less than 100" }, { "min", "1" }, { "max", "100" } }) %>
                                                <%= Html.ValidationMessage("User_Pwd") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Email", resBundle.GetString("Security", "UsersView.User.Email", "Email")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Email", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Email, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { (backOffice.Infrastructure.Security.FormAuthMemberShip.MembershipService.RequiresUniqueEmail()?"required":"norequired"), "" } }) %>
                                                <%= Html.ValidationMessage("User_Email") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Email2", resBundle.GetString("Security", "UsersView.User.Email2", "Secondary Email")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("User_Email2", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Email2, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                <%= Html.ValidationMessage("User_Email2") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_MainTel", resBundle.GetString("Security", "UsersView.User.MainTel", "Main Phone Number")) %>
                                            </div>
                                            <div class="editor-field">
                                                <div class="editor-label sub">
                                                    <input id="User_MainTelCountry"
                                                           data-bind="value: UserData.MainTelCouId, events: { change: change }, enabled: editEnabled2"
                                                           style="width: 100%;"/>
                                                    <%= Html.ValidationMessage("User_MainTelCountry") %>
                                                </div>
                                                <div class="editor-field sub2">
                                                    <div class="editor-field sub">
                                                        <%= Html.TextBox("User_MainTel", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.MainTel, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                        <%= Html.ValidationMessage("User_MainTel") %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_SecondTel", resBundle.GetString("Security", "UsersView.User.SecondTel", "Secondary Phone Number")) %>
                                            </div>
                                            <div class="editor-field">
                                                <div class="editor-label sub">
                                                    <input id="User_SecondTelCountry"
                                                           data-bind="value: UserData.SecondTelCouId, events: { change: change }, enabled: editEnabled2"
                                                           style="width: 100%;"/>
                                                    <%= Html.ValidationMessage("User_SecondTelCountry") %>
                                                </div>
                                                <div class="editor-field sub2">
                                                    <div class="editor-field sub">
                                                        <%= Html.TextBox("User_SecondTel", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.SecondTel, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                        <%= Html.ValidationMessage("User_SecondTel") %>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Enabled", resBundle.GetString("Security", "UsersView.User.Enabled", "Enabled")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.CheckBox("User_Email2", new Dictionary<string, Object> { { "data-bind", "checked: UserData.Enabled, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2" } }) %>
                                                <%= Html.ValidationMessage("User_Enabled") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Language", resBundle.GetString("Security", "UsersView.User.Language", "Language")) %>
                                            </div>
                                            <div class="editor-field">
                                                <input id="User_Language"                                                       
                                                       data-bind="value: UserData.LanId, events: { change: change }, enabled: editEnabled2"
                                                       data_val_required="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                                                       
                                                       style="width: 100%;"/>
                                                <%= Html.ValidationMessage("User_Language", new { style = "z-index: 0;" })%>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Country", resBundle.GetString("Security", "UsersView.User.Country", "Country")) %>
                                            </div>
                                            <div class="editor-field">
                                                <input id="User_Country"
                                                       data-bind="value: UserData.CouId, events: { change: change }, enabled: editEnabled2"                                                       
                                                       style="width: 100%;"/>
                                                <%= Html.ValidationMessage("User_Country") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_Currency", resBundle.GetString("Security", "UsersView.User.Currency", "Currency")) %>
                                            </div>
                                            <div class="editor-field">
                                                <input id="User_Currency"
                                                       data-bind="value: UserData.CurId, events: { change: change }, enabled: editEnabled2"
                                                       style="width: 100%;"/>
                                                <%= Html.ValidationMessage("User_Currency") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("User_MaintenanceMode", resBundle.GetString("Security", "UsersView.User.MaintenanceMode", "Maintenance Mode")) %>
                                            </div>
                                            <div class="editor-field">
                                                <input id="User_MaintenanceMode"                                                       
                                                        data-bind="value: UserData.MaintenanceMode, events: { change: change }, enabled: editEnabled2"
                                                        data_val_required="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                                                        data-required-msg="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                                                        
                                                        style="width: 100%;"/>
                                                <br />
                                                <%= Html.ValidationMessage("User_MaintenanceMode", new { style = "z-index: 0;" })%>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="divGroup" class="editor-form k-edit-form-container" data-bind="visible: showGroupForm" >
                                        <!--<div class="k-header k-shadow"> Group Information </div>-->
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("Group_Name", resBundle.GetString("Security", "UsersView.Group.Name", "Name")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("Group_Name", null, new Dictionary<string, Object> { { "data-bind", "value: GroupData.Name, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled2"} , { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" } }) %>
                                                <%= Html.ValidationMessage("Group_Name") %>
                                            </div>
                                        </div>
                                        <div class="editor-container">
                                            <div class="editor-label">
                                                <%= Html.Label("Group_Description", resBundle.GetString("Security", "UsersView.Group.Description", "Description")) %>
                                            </div>
                                            <div class="editor-field">
                                                <%= Html.TextBox("Group_Description", null, new Dictionary<string, Object> { { "data-bind", "value: GroupData.Description, events: { change: change }, enabled: editEnabled2" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                                                <%= Html.ValidationMessage("Group_Description") %>
                                            </div>
                                        </div>
                                    </div>

                                </form>
                                

                             <% });
                                       
                            tabstrip.Add().Text(resBundle.GetString("Security", "UsersView.Title.Roles", "Roles"))
                                .HtmlAttributes(new { id = "tabRoles", tabIndex = 0 })                                
                                .Content(() =>
                                    { %>
    
                                        <%= Html.Kendo().ToolBar()
                                            .Name("tbrTreeRolesType")
                                            .Items(items => {
                                                items.Add().Type(CommandType.ButtonGroup).Buttons(buttons =>
                                                {
                                                    buttons.Add().Text(resBundle.GetString("Security", "UsersView.TreeRolesType.ByInstallation", "Roles by installation")).Id("treeRolesType0").Togglable(true).Group("treeRolesType").SpriteCssClass("k-tool-icon treeRolesType1-icon").Selected(true);
                                                    buttons.Add().Text(resBundle.GetString("Security", "UsersView.TreeRolesType.ByFeature", "Roles by feature")).Id("treeRolesType1").Togglable(true).Group("treeRolesType").SpriteCssClass("k-tool-icon treeRolesType2-icon");
                                                });
                                            })
                                            .Events(e => e.Toggle("TbrTreeRolesTypeOnToggle"))
                                        %>

                                        <script id="treeRoles-template" type="text/kendo-ui-template">
                                            <div style="width: #:item.width#/*400px*/;">
                                            <div style="display: block; float: left; width: 300px;"> #:item.name# </div>
                                            <div style="display: block; /*width: 100px;*/ text-align: right;">
                                            #if(item.statusA == -1) {#
                                                <button id="A#:item.id#" type="button" class="k-button k-button-icon k-state-hidden"
                                                        rol_type="A" rol_name="#:item.roleA#" rol_parent="A#:item.parentId#" rol_status="#:item.statusA#"><span class="k-icon admin-icon"></span></button>
                                            #} else { if(item.statusA == 0) {#
                                                <button id="A#:item.id#" type="button" class="k-button k-button-icon #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Disabled.Title", "Administration permission disabled") %>"
                                                        rol_type="A" rol_name="#:item.roleA#" rol_parent="A#:item.parentId#" rol_status="#:item.statusA#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon admin-icon"></span></button>
                                            #} else { if(item.statusA == 1) {#
                                                <button id="A#:item.id#" type="button" class="k-button k-button-icon k-state-selected #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Enabled.Title", "Administration permission enabled") %>"
                                                        rol_type="A" rol_name="#:item.roleA#" rol_parent="A#:item.parentId#" rol_status="#:item.statusA#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon admin-icon"></span></button>
                                            #} else {#
                                                <button id="A#:item.id#" type="button" class="k-button k-button-icon k-state-selected2 #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Partial.Title", "Administration permission partially enabled") %>"
                                                        rol_type="A" rol_name="#:item.roleA#" rol_parent="A#:item.parentId#" rol_status="#:item.statusA#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon admin-icon"></span></button>
                                            #}}}#
                                            #if(item.statusW == -1) {#
                                                <button id="W#:item.id#" type="button" class="k-button k-button-icon k-state-hidden"
                                                        rol_type="W" rol_name="#:item.roleW#" rol_parent="W#:item.parentId#" rol_status="#:item.statusW#"><span class="k-icon write-icon"></span></button>
                                            #} else { if(item.statusW == 0) {#
                                                <button id="W#:item.id#" type="button" class="k-button k-button-icon #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Disabled.Title", "Write permission disabled") %>"
                                                        rol_type="W" rol_name="#:item.roleW#" rol_parent="W#:item.parentId#" rol_status="#:item.statusW#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon write-icon"></span></button>
                                            #} else { if(item.statusW == 1) {#
                                                <button id="W#:item.id#" type="button" class="k-button k-button-icon k-state-selected #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Enabled.Title", "Write permission enabled") %>"
                                                        rol_type="W" rol_name="#:item.roleW#" rol_parent="W#:item.parentId#" rol_status="#:item.statusW#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon write-icon"></span></button>
                                            #} else {#
                                                <button id="W#:item.id#" type="button" class="k-button k-button-icon k-state-selected2 #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Partial.Title", "Write permission partially enabled") %>"
                                                        rol_type="W" rol_name="#:item.roleW#" rol_parent="W#:item.parentId#" rol_status="#:item.statusW#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon write-icon"></span></button>
                                            #}}}#
                                            #if(item.statusR == -1) {#
                                                <button id="R#:item.id#" type="button" class="k-button k-button-icon k-state-hidden"
                                                        rol_type="R" rol_name="#:item.roleR#" rol_parent="R#:item.parentId#" rol_status="#:item.statusR#"><span class="k-icon read-icon"></span></button>
                                            #} else { if(item.statusR == 0) {#
                                                <button id="R#:item.id#" type="button" class="k-button k-button-icon #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Disabled.Title", "Read permission disabled") %>"
                                                        rol_type="R" rol_name="#:item.roleR#" rol_parent="R#:item.parentId#" rol_status="#:item.statusR#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon read-icon"></span></button>
                                            #} else { if(item.statusR == 1) {#
                                                <button id="R#:item.id#" type="button" class="k-button k-button-icon k-state-selected #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Enabled.Title", "Read permission enabled") %>"
                                                        rol_type="R" rol_name="#:item.roleR#" rol_parent="R#:item.parentId#" rol_status="#:item.statusR#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon read-icon"></span></button>
                                            #} else {#
                                                <button id="R#:item.id#" type="button" class="k-button k-button-icon k-state-selected2 #if (!item.editEnabled) {#k-state-disabled2#}#" title="<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Partial.Title", "Read permission partially enabled") %>"
                                                        rol_type="R" rol_name="#:item.roleR#" rol_parent="R#:item.parentId#" rol_status="#:item.statusR#" onclick="#if (item.editEnabled) {# RoleChange(this.id); #}#"><span class="k-icon read-icon"></span></button>
                                            #}}}#
                                            </div>
                                            </div>
                                        </script>
    

                                        <div class="pane-content" style="height: 100%;">                                            
                                            <%=Html.Kendo().TreeView()
                                                .Name("treeRoles")
                                                .DataTextField("name")
                                                .DataImageUrlField("imageUrl")
                                                .HtmlAttributes(new { @class = "demo-section" })    
                                                /*.Checkboxes(checkboxes => checkboxes
                                                    .Name("checkedRoles")
                                                    .CheckChildren(true)
                                                    .TemplateId("treeRoles2-template")
                                                )*/
                                                .TemplateId("treeRoles-template")                                                
                                                .AutoBind(false)
                                                .LoadOnDemand(false)
                                                .DataSource(source =>
                                                {
                                                    source.Read(read => read.Action("TreeRolesData", "Security", new { plugin = "SecurityPlugin" }).Type(HttpVerbs.Post).Data("TreeRolesAddSecurityId"))
                                                          .Model(model => model.Id("id").Children("items").HasChildren("hasChildren"));
                                                })
                                                .Events(ev =>
                                                    ev.DataBound("OnRolesDataBound")
                                                    )
                                            %>                                            
                                        </div>                                                       
                                    <% });
                                       

                      }).Render();
                %>

           <% });
      })
      .Render();
%>

<script type="text/javascript">
    
    var modelOptions = {
        InstanceName: "viewModel",
        BindContainers: [".form-class", "#divToolbar"],
        LoadingContainer: "#tabs",
        NotificationContainer: "#spnNotification",
        Actions: {
            Load: "<%= Url.Action("SecurityEntityRead", "Security") %>",
            Save: "<%= Url.Action("SecurityEntitySave", "Account", new { plugin = "SecurityPlugin" }) %>",
            Remove: "<%= Url.Action("SecurityEntityRemove", "Security") %>",
            New: "<%= Url.Action("SecurityEntityNew", "Security") %>",
            Move: "<%= Url.Action("SecurityEntityMove", "Security") %>"
        },
        Events: {
            OnLoad: "ViewModel_OnLoad",
            OnLoadCancel: "ViewModel_OnLoadCancel",
            OnSave: "ViewModel_OnSave", OnCancel: "",
            OnRemove: "ViewModel_OnRemove",
            OnMove: "ViewMovel_OnMove"
        },
        Validations: {
            RequiredMsg: "<%= resBundleBackOffice.GetString("Common", "Default.Field.Required2", "Field required") %>"
        },
        Resources: {
            Default: {
                InvalidId: "<%= resBundleBackOffice.GetString("Common", "Default.Invalid.Id", "Invalid id") %>",
                FieldRequired: "<%= resBundleBackOffice.GetString("Common", "Default.Field.Required2", "Field required") %>",
                CommunicationError: "<%= resBundleBackOffice.GetString("Common", "Default.Communication.Error", "Communication error") %>",
                InvalidData: "<%= resBundleBackOffice.GetString("Common", "Default.Invalid.Data", "Invalid data") %>"
            },
            Load: { 
                Title: "<%= resBundle.GetString("Security", "UsersView.Load.Title", "Load") %>",
                Exception: "<%= resBundle.GetString("Security", "UsersView.Load.Exception", "Error loading data") %>"
            },
            Save: {
                Title: "<%= resBundle.GetString("Security", "UsersView.Save.Title", "Save") %>",
                Exception: "<%= resBundle.GetString("Security", "UsersView.Save.Exception", "Error saving") %>",
                Notification: "<%= resBundle.GetString("Security", "UsersView.Save.Notification", "Data saved") %>",
                ApplyNewRolesToChilds: "<%= resBundle.GetString("Security", "UsersView.Save.ApplyNewRolesToChilds", "Apply new roles permissions to child groups and users?") %>"
            },
            Remove: {
                Title: "<%= resBundle.GetString("Security", "UsersView.Remove.Title", "Remove") %>",
                Exception: "<%= resBundle.GetString("Security", "UsersView.Remove.Exception", "Error removing") %>",
                AskConfirm: "<%= resBundle.GetString("Security", "UsersView.Remove.AskConfirm", "Are you sure?") %>",
                Notification: "<%= resBundle.GetString("Security", "UsersView.Remove.Notification", "Removed successfully") %>"
            },
            New: {
                Title: "<%= resBundle.GetString("Security", "UsersView.New.Title", "New") %>",
                Exception: "<%= resBundle.GetString("Security", "UsersView.New.Exception", "Error generating new data") %>"
            },
            Move: {
                Title: "<%= resBundle.GetString("Security", "UsersView.Move.Title", "Move") %>",
                Exception: "<%= resBundle.GetString("Security", "UsersView.Move.Exception", "Error moving element") %>",
                Notification: "<%= resBundle.GetString("Security", "UsersView.Move.Notification", "Element moved") %>",
                Confirm: "<%= resBundle.GetString("Security", "UsersView.Move.Confirm", "Move selected element?") %>"
            },
            CheckChanges: {
                Title: "<%= resBundle.GetString("Security", "UsersView.CheckChanges.Title", "Pending Changes") %>",
                Message: "<%= resBundle.GetString("Security", "UsersView.CheckChanges.Message", "Discard pending changes?") %>"
            }
        },
        AccessLevel: <%= (int)FormAuthMemberShip.HelperService.FeatureLevelAccessAllowed("Security") %>
    };

    var treeRolesOptions = {
        Actions: {
            Read: "<%= Url.Action("TreeRolesData", "Security", new { plugin = "SecurityPlugin" }) %>",
            Read2: "<%= Url.Action("TreeRoles2Data", "Security", new { plugin = "SecurityPlugin" }) %>"
        },
        Resources: {
            RoleNode: {
                Admin: {
                    Enabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Enabled.Title", "Administration permission enabled") %>",
                    Disabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Disabled.Title", "Administration permission disabled") %>",
                    Partial: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Admin.Partial.Title", "Administration permission partially enabled") %>"
                },
                Write: {
                    Enabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Enabled.Title", "Write permission enabled") %>",
                    Disabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Disabled.Title", "Write permission disabled") %>",
                    Partial: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Write.Partial.Title", "Write permission partially enabled") %>"
                },
                Read: {
                    Enabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Enabled.Title", "Read permission enabled") %>",
                    Disabled: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Disabled.Title", "Read permission disabled") %>",
                    Partial: "<%= resBundle.GetString("Security", "UsersView.RoleNode.Read.Partial.Title", "Read permission partially enabled") %>"
                }
            }
        }
    };

    var viewModel = new SecurityViewModel(modelOptions);

    var bSecurityRootExpanded = false;
    var sSecurityExpandPath = "<%= Model.Path %>";
    var arrCurrentPath = ["0"];
            
    $(window).resize(resizeAll);

    function resizeAll() {
        var iDiff = ($("#splMain").position().top + 3);
        $("#splMain").css("height", $(window).height() - iDiff /*231*/);
        $("#treeSecurity").height($(window).height() - iDiff);        
        $("#tabs").height($(window).height() - (iDiff+5));
        $("#tabs").children(".k-content").height($(window).height() - (iDiff+50));
        $("#treeRoles").height($(window).height() - (iDiff+106));        
    }

    

    $(document).ready(function () {
        
        resizeAll();

        var splitter = $("#splMain").data("kendoSplitter");
        splitter.size(".k-pane:first", "15%");
        splitter.trigger("resize");

        var sToolbar = '<div id="divToolbar" style="float: right">' +
                            '<button type="button" id="btnNewGroup" class="k-button k-button-icon" ' +
                                    'data-bind="click: newGroup, visible: showNewButton" ' +
                                    'title="<%= resBundle.GetString("Security", "UsersView.Button.NewGroup.Title", "Create new group") %>"> ' +
                                '<span class="k-icon add16-icon">  </span><%= resBundle.GetString("Security", "UsersView.Button.NewGroup", "New Group") %> ' +
                            '</button>' +
                            '<button type="button" id="btnNewUser" class="k-button k-button-icon" ' +
                                    'data-bind="click: newUser, visible: showNewButton" ' +
                                    'title="<%= resBundle.GetString("Security", "UsersView.Button.NewUser.Title", "Create new user") %>"> ' +
                                '<span class="k-icon add16-icon">  </span><%= resBundle.GetString("Security", "UsersView.Button.NewUser", "New User") %> ' +
                            '</button>' +
                            '<button type="button" id="btnSave" class="k-button k-button-icon" ' +
                                    'data-bind="click: save, enabled: HasChanges, visible: showSaveButton" ' +
                                    'title="<%= resBundle.GetString("Security", "UsersView.Button.Save.Title", "Save pending changes") %>"> ' +
                                '<span class="k-icon save16-icon">  </span><%= resBundle.GetString("Security", "UsersView.Button.Save", "Save") %> ' +
                            '</button>' +
                            '<button type="button" id="btnCancel" class="k-button k-button-icon" ' +
                                    'data-bind="click: cancel, enabled: HasChanges, visible: showSaveButton" ' +
                                    'title="<%= resBundle.GetString("Security", "UsersView.Button.Cancel.Title", "Cancel last changes") %>"> ' +
                                '<span class="k-icon cancel16-icon">  </span><%= resBundle.GetString("Security", "UsersView.Button.Cancel", "Cancel") %> ' +
                            '</button>' +
                            '<button type="button" id="btnRemove" class="k-button k-button-icon" ' +
                                    'data-bind="click: remove, visible: showRemoveButton" ' +
                                    'title="<%= resBundle.GetString("Security", "UsersView.Button.Remove.Title", "Remove current user or group") %>"> ' +
                                '<span class="k-icon remove16-icon">  </span><%= resBundle.GetString("Security", "UsersView.Button.Remove", "Remove") %> ' +
                            '</button>' +
                       '</div>';
        $("#tabs ul").append(sToolbar);

        /*$("#divToolbar").kendoToolBar({
            resizable: true,
            items: [
                {
                    template: '<button type="button" id="btnNewGroup" class="k-button k-button-icon" ' +
                                   'data-bind="click: newGroup, visible: showNewButton" ' +
                                   'title="%= resBundle.GetString("Security", "UsersView.Button.NewGroup.Title", "Create new group") %>"> ' +
                               '<span class="k-icon add16-icon">  </span>%= resBundle.GetString("Security", "UsersView.Button.NewGroup", "New Group") %> ' +
                           '</button>',
                    overflow: "never"
                },
                {
                    template: '<button type="button" id="btnNewUser" class="k-button k-button-icon" ' +
                                   'data-bind="click: newUser, visible: showNewButton" ' +
                                   'title="%= resBundle.GetString("Security", "UsersView.Button.NewUser.Title", "Create new user") %>"> ' +
                               '<span class="k-icon add16-icon">  </span>%= resBundle.GetString("Security", "UsersView.Button.NewUser", "New User") %> ' +
                           '</button>',
                    overflow: "never"
                },
                { type: "separator" },
                {
                    template: '<button type="button" id="btnSave" class="k-button k-button-icon" ' +
                                   'data-bind="click: save, enabled: HasChanges, visible: showSaveButton" ' +
                                   'title="%= resBundle.GetString("Security", "UsersView.Button.Save.Title", "Save pending changes") %>"> ' +
                                '<span class="k-icon save16-icon">  </span>%= resBundle.GetString("Security", "UsersView.Button.Save", "Save") %> ' +
                           '</button>',
                    overflow: "never"
                },
                {
                    template: '<button type="button" id="btnCancel" class="k-button k-button-icon" ' +
                                   'data-bind="click: cancel, enabled: HasChanges, visible: showSaveButton" ' +
                                   'title="%= resBundle.GetString("Security", "UsersView.Button.Cancel.Title", "Cancel last changes") %>"> ' +
                                '<span class="k-icon cancel16-icon">  </span>%= resBundle.GetString("Security", "UsersView.Button.Cancel", "Cancel") %> ' +
                           '</button>',
                    overflow: "never"
                },
                { type: "separator" },
                {
                    template: '<button type="button" id="btnRemove" class="k-button k-button-icon" ' +
                                   'data-bind="click: remove, visible: showRemoveButton" ' +
                                   'title="%= resBundle.GetString("Security", "UsersView.Button.Remove.Title", "Remove current user or group") %>"> ' +
                               '<span class="k-icon remove16-icon">  </span>%= resBundle.GetString("Security", "UsersView.Button.Remove", "Remove") %> ' +
                           '</button>',
                    overflow: "never"
                }
            ]
        });*/

        /*$("#divButtons").kendoWindow({
            position: {
                top: $("#tabs").offset().top,
                left: $("#tabs").offset().left + $("#tabs").width() - 450
            },
            pinned: true,
            resizable: true,
            actions: [],
            width: 450,
            title: false
        });*/
        
        $("#User_MainTelCountry").kendoComboBox({
            placeholder: "",
            dataTextField: "CouDescription",
            dataValueField: "CouId",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("Countries_Read", "Security", new { plugin = "SecurityPlugin" } ) %>",
                    }
                }
            }
        });
        $("#User_SecondTelCountry").kendoComboBox({
            placeholder: "",
            dataTextField: "CouDescription",
            dataValueField: "CouId",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("Countries_Read", "Security", new { plugin = "SecurityPlugin" } ) %>",
                    }
                }
            }
        });

        $("#User_Language").kendoComboBox({
            placeholder: "",
            dataTextField: "LanDescription",
            dataValueField: "LanId",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("Languages_Read", "Security", new { plugin = "SecurityPlugin" } ) %>",
                    }
                }
            }
        });

        $("#User_Country").kendoComboBox({
            placeholder: "",
            dataTextField: "CouDescription",
            dataValueField: "CouId",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("Countries_Read", "Security", new { plugin = "SecurityPlugin" } ) %>",
                    }
                }
            }
        });

        $("#User_Currency").kendoComboBox({
            placeholder: "",
            dataTextField: "CurName",
            dataValueField: "CurId",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("Currencies_Read", "Security", new { plugin = "SecurityPlugin" } ) %>",
                    }
                }
            }
        });
        
        $("#User_MaintenanceMode").kendoComboBox({
            placeholder: "",
            dataTextField: "Description",
            dataValueField: "Id",
            filter: "contains",
            autoBind: false,
            minLength: 2,
            valuePrimitive: true,
            dataSource: {
                type: "jsonp",
                serverFiltering: true,
                transport: {
                    read: {
                        url: "<%= Url.Action("MaintenanceFunctionalityMode_Read", "Maintenance", new { plugin = "MaintenancePlugin" } ) %>"
                    }
                }
            }
        });

        //viewModel.Validator = $("#frmGeneral").kendoValidator().data("kendoValidator");
        /*viewModel.Validator = $("#frmGeneral").kendoValidator({
            messages: {
                // defines a message for the 'custom' validation rule
                minLength: "Invalid minimum length",

                // overrides the built-in message for the required rule
                required: "My custom required message",

                // overrides the built-in message for the email rule
                // with a custom function that returns the actual message
                email: function (input) {
                    return input.data("emailMessage");
                }
            },
            rules: {
                minLength: function (input) {
                    if (input.is("[name=User_Pwd]")) {
                        return input.val().length >= 4;
                    }
                    return true;
                }
            }
        });*/        

        viewModel.bind();
        viewModel.Validator = $(".form-class").kendoValidator().data("kendoValidator");
        //viewModel.Validator.validate();
        
        $("#txtFindUser").closest(".k-widget").addClass("k-textbox k-space-right").append('<span class="k-icon k-i-search"></span>');

    });

    function onNotificationShow(e) {
        if (!$("." + e.sender._guid)[1]) {
            var element = e.element.parent(),
                eWidth = element.width(),
                eHeight = element.height(),
                //wWidth = $(window).width(),
                //wHeight = $(window).height(),
                wWidth = $("#tabs").width(),
                wHeight = $("#tabs").height(),
                wTop = $("#tabs").offset().top,
                wLeft = $("#tabs").offset().left,
                newTop, newLeft;

            newLeft = Math.floor(wWidth / 2 - eWidth / 2);
            newTop = Math.floor(wHeight / 2 - eHeight / 2);

            newTop = wTop - $("#tabs").scrollTop();
            //newLeft = wLeft + Math.floor(wWidth / 2);
            newLeft = $("#tabRoles").offset().left + 100 /*+ $("#tabRoles").width*/;


            e.element.parent().css({ top: newTop, left: newLeft });
        }
    }

    var openingPath = false;

    /*function openPath(treeview, path, triggerSelect) {
      
        var ds = treeview.dataSource;
        var node = ds.get(path[0])
      
        if (triggerSelect == null) triggerSelect = true;

        // skip already expanded and loaded nodes
        while (path.length > 1 && (node.expanded || node.loaded())) {
            node.set("expanded", true);
            path.shift();
            node = ds.get(path[0]);
        }
      
        // if there are levels to expand, expand them
        if (path.length > 1) {
        
            // listen to the change event to know when the node has been loaded
            ds.bind("change", function expandLevel(e) {
                var id = e.node && e.node.id;
          
                // proceed if the change is caused by the last fetching
                if (id == path[0]) {
                    path.shift();
                    ds.unbind("change", expandLevel);
            
                    openPath(treeview, path, triggerSelect);
                }
            });

            ds.get(path[0]).set("expanded", true);
        } else {
            // otherwise select
            node = treeview.findByUid(ds.get(path[0]).uid);
            treeview.select(node);
            if (triggerSelect) treeview.trigger("select", { node: node });
            openingPath = false;
        }
    }*/

    function Tree_ChangeNodeText(treeview, newText, node) {
        var bRet = false;
        if (node == null) node = treeview.select();
        if (node != null) {
            treeview.text(node, newText);
            bRet = true;
        }
        return bRet;
    }

    function Tree_Refresh(treeview) {        
        treeview.dataSource.read();
    }

    /*function Tree_CheckAll(treeviewName) {
        $("#treeRoles")            
            .each(
              function () {
                  var e = $('#treeRoles').data('kendoTreeView').dataItem(this);
                  e.set("selected", !1);
                  delete e.selected;
              });
    }*/

    function TreeRolesAddSecurityId() {
        var parentGroupId = 0;
        var allowedRoles;
        var entityType = viewModel.get("EntityType");
        if (entityType == "G") {
            parentGroupId = viewModel.get("GroupData").ParentId;
            allowedRoles = viewModel.get("GroupData").AllowedRoles;
        }
        else if (entityType == "U") {
            parentGroupId = viewModel.get("UserData").GroupId;
            allowedRoles = viewModel.get("UserData").AllowedRoles;
        }
        return {
            securityId: viewModel.get("SecurityId"), // securityIdSelected
            parentGroupId: parentGroupId,
            allowedRoles: kendo.stringify(allowedRoles),
            allRoles: kendo.stringify(viewModel.get("AllRoles")),
            editableInstallations: kendo.stringify(viewModel.get("EditableInstallations"))
        };        
    }

    function OnSecurityDataBound(e) {
        
        if (sSecurityExpandPath != "" && openingPath == false) {
            openingPath = true;
            var treeview = $('#treeSecurity').data('kendoTreeView');
            treeview.setOptions({ animation: false });
            treeview.collapse(".k-item");
            //var arrPath = sPath.split(",");
            //openPath(treeview, arrPath, true);
            var arrPath = [];
            var arrSecurityExpandPath = sSecurityExpandPath.split(",");
            if (arrSecurityExpandPath.length > 0 && arrSecurityExpandPath[0] != "0") arrPath.push("0");
            arrPath = arrPath.concat(arrSecurityExpandPath);
            sSecurityExpandPath = "";            
            treeNode(treeview, arrPath, "Tree_SelectNodeCallback");
            treeview.setOptions({ animation: kendo.ui.TreeView.fn.options.animation });
        }
        else if (!bSecurityRootExpanded) {
            bSecurityRootExpanded = true;
            var treeview = $('#treeSecurity').data('kendoTreeView');
            if (viewModel.get("SecurityId") > 0) {
                var arrPath = ["0"];
                if (viewModel.get("EntityType") == "G") {                    
                    arrPath = arrPath.concat(viewModel.get("GroupData").Path.split(","));
                }
                else if (viewModel.get("EntityType") == "U") {
                    arrPath = arrPath.concat(viewModel.get("UserData").Path.split(","));
                }
                treeNode(treeview, arrPath, "Tree_SelectNodeCallback");
            }
            else {
                var ds = treeview.dataSource;
                ds.get(0).set("expanded", true);
            }
        }
        
    }
    
    //var SecurityIdPrev = -1;    
    //var pendingRoles = [];
    var bRolesRootExpanded = false;
    function OnRolesDataBound(e) {
        viewModel.Loading(false);
        this.expand('.k-item:first');
        /*if (!bRolesRootExpanded) {
            bRolesRootExpanded = true;
            this.expand('.k-item');
        }*/
        /*if (e.node != null) {
            var _hasChildren = $('#treeRoles').data('kendoTreeView').dataItem(e.node).hasChildren;
            var _id = $('#treeRoles').data('kendoTreeView').dataItem(e.node).id;
            var _parentId = $('#treeRoles').data('kendoTreeView').dataItem(e.node).parentId;
            
            for (var i = 0; i < pendingRoles.length; i++) {
                if (pendingRoles[i] == _parentId) {
                    pendingRoles.splice(i, 1);
                    i = pendingRoles.length;
                }
            }

            if (hasChildren)
                pendingRoles.push(_id);

            console.log(id + "-" + parentId + "-" + hasChildren);
            console.log(pendingRoles.join(','));

        }*/
        /*var SecurityId = viewModel.get("SecurityId");
        if (SecurityId > 0 && SecurityId != SecurityIdPrev) {
            SecurityIdPrev = SecurityId;
            var treeview = $('#treeRoles').data('kendoTreeView');
            var ds = treeview.dataSource;
            if (ds.get("ROOT") != null) ds.get("ROOT").set("expanded", true);
        }*/
    }

    function OnSecuritySelect(e) {        

        var data = $("#treeSecurity").data("kendoTreeView").dataItem(e.node);        
        //securityIdSelected = data.id;
        viewModel.load(data.id);
        
    }
    
    /*function checkedNodeIds(nodes, checkedNodes) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].id != "ROOT" && nodes[i].checked) {
                checkedNodes.push(nodes[i].name);
            }

            if (nodes[i].hasChildren) {
                checkedNodeIds(nodes[i].children.view(), checkedNodes);
            }
        }
    }

    // show checked node IDs on datasource change
    function onRoleCheck() {
        var checkedNodes = [],
            treeView = $("#treeRoles").data("kendoTreeView"),
            message;

        checkedNodeIds(treeView.dataSource.view(), checkedNodes);

        viewModel.setRoles(checkedNodes);
    }*/

    function ViewModel_OnLoad(model) {
        
        arrCurrentPath = ["0"];
        var oData = null;
        if (model.get("EntityType") == "G")
            oData = model.get("GroupData");
        else if (model.get("EntityType") == "U")
            oData = model.get("UserData");
        if (oData != null) {
            arrCurrentPath = arrCurrentPath.concat(oData.Path.split(","));
        }        
        if (model.get("SecurityId") == -1) {
            $("#tabs").data("kendoTabStrip").select(0);
        }

        // Load roles        
        LoadTreeRoles();

    }

    function ViewModel_OnLoadCancel(model) {

        var arrPath = ["0"];
        var oData = null;
        if (model.get("EntityType") == "G")
            oData = model.get("GroupData");
        else if (model.get("EntityType") == "U")
            oData = model.get("UserData");
        if (oData != null) {
            arrPath = arrPath.concat(oData.Path.split(","));
        }
        var treeview = $('#treeSecurity').data('kendoTreeView');        
        treeSelectNode(treeview, treeNode(treeview, arrPath), false);
    }
    function ViewModel_OnSave(model) {
        var sNodeText = "";
        var arrPath = ["0"];
        if (model.get("EntityType") == "G") {
            sNodeText = model.get("GroupData").Name;
            arrPath = arrPath.concat(model.get("GroupData").Path.split(","));
        }
        else if (model.get("EntityType") == "U") {
            sNodeText = model.get("UserData").Username;
            arrPath = arrPath.concat(model.get("UserData").Path.split(","));            
        }
        var treeview = $('#treeSecurity').data('kendoTreeView');        
        if (model.get("PrevSecurityId") != model.get("SecurityId")) {
            bSecurityRootExpanded = false;
            Tree_Refresh(treeview);            
            sSecurityExpandPath = arrPath.join(",");
        }
        else
            Tree_ChangeNodeText(treeview, sNodeText, treeNode(treeview, arrPath));
    }

    function ViewModel_OnRemove(model) {
        var treeview = $('#treeSecurity').data('kendoTreeView');
        bSecurityRootExpanded = false;
        Tree_Refresh(treeview);
        if (arrCurrentPath.length > 1) arrCurrentPath.splice(arrCurrentPath.length - 1, 1);
        sSecurityExpandPath = arrCurrentPath.join(",");
    }
   
    function ViewMovel_OnMove(model) {
        var treeview = $('#treeSecurity').data('kendoTreeView');
        bSecurityRootExpanded = false;
        Tree_Refresh(treeview);
        if (arrCurrentPath.length > 1) arrCurrentPath.splice(arrCurrentPath.length - 1, 1);
        sSecurityExpandPath = arrCurrentPath.join(",");
        //model.load(model.id)
    }

    /*function ExpandAll(e) {
        var treeview = $('#treeRoles2').data('kendoTreeView');
        var ds = treeview.dataSource;
        var item = null;

        if (e.action == "itemchange")
            item = e;
        else if (e.id != null)
            item = e;
        else if (e.node != null)
            item = treeview.dataItem(e.node);

        if (item != null) {
            if (item.action != "itemchange" && !item.loaded()) {
                if (item.hasChildren) {
                    console.log("bind " + item.id);
                    ds.bind("change", function expandLevel(e) {
                        var id = e.node && e.node.id;
                        if (id == item.id) {
                            console.log("unbind " + item.id);
                            ds.unbind("change", expandLevel);
                            ExpandAll(e);
                        }
                    });
                    try {
                        ds.get(item.id).set("expanded", true);
                    } catch (ex) { console.error("expand error " + item.id); }
                }
            }
            else {                
                //try {
                    if (item.items != null && item.items.length > 0) {
                        for (var i = 0; i < item.items.length; i++) {
                            ExpandAll(item.items[i]);
                        }
                    }
                //} catch (ex) { }
            }
        }
        else {
            if (ds.get("ROOT") != null) {
                console.log("bind ROOT");
                ds.bind("change", function expandLevel(e) {
                    var id = e.node && e.node.id;
                    if (id == "ROOT") {
                        console.log("unbind ROOT");
                        ds.unbind("change", expandLevel);
                        ExpandAll(e);
                    }
                });
                ds.get("ROOT").set("expanded", true);
            }
        }
    }*/

    function OnSecurityDrag(e) {
        if (!viewModel.get("HasChanges")) {
            var data = $("#treeSecurity").data("kendoTreeView").dataItem(e.sourceNode);
            var newData = $("#treeSecurity").data("kendoTreeView").dataItem(e.dropTarget);
            console.log("Drag src:" + data.Type + " dst:" + (newData!=null?newData.Type:"NULL"));
            if (newData != null && newData.Type == "G") {
                e.setStatusClass('k-add');
            } else {
                e.setStatusClass('k-denied');
            }
        }
        else {
            e.setStatusClass('k-denied');
        }
    }

    function OnSecurityDragEnd(e) {

    }

    function OnSecurityDrop(e) {
        if (!viewModel.get("HasChanges")) {
            var data = $("#treeSecurity").data("kendoTreeView").dataItem(e.sourceNode);
            var newData = $("#treeSecurity").data("kendoTreeView").dataItem(e.destinationNode);
            if (newData == null || newData.Type != "G") {
                e.setValid(false);
            }
            else {
                if (e.valid) {
                    /* Do your adding here or do it in a drop function elsewhere if you want the receiver to dictate. */
                    console.log("Dropped " + data.id + " " + newData.id);

                    viewModel.move(data.id, newData.id);
                }            
            }
        }

        e.preventDefault();
        /*
        // do not allow changing levels
        if (data.IsParent != newData.IsParent && e.dropPosition != "over") {
            e.setValid(false);
        }
        // do not allow adding to children
        if (!newData.IsParent && e.dropPosition == "over") {
            e.setValid(false);
        }
        */
    }

    function LoadTreeRoles() {

        viewModel.Loading(true);

        var tree = $("#treeRoles").data("kendoTreeView");
        bRolesRootExpanded = false;

        var toolbar = $("#tbrTreeRolesType").data("kendoToolBar");
        var tbrSelected = toolbar.getSelectedFromGroup("treeRolesType");
        if (tbrSelected.index() == 0)
            tree.dataSource.options.transport.read.url = treeRolesOptions.Actions.Read;
        else if (tbrSelected.index() == 1)
            tree.dataSource.options.transport.read.url = treeRolesOptions.Actions.Read2;
                
        tree.dataSource.read();
        
    }

    function TbrTreeRolesTypeOnToggle(e) {
        if (e.group == "treeRolesType") {

            if (viewModel.get("PrevSecurityId") >= 0) {
                //viewModel.checkRolesChanges("LoadTreeRoles()", "ToggleTreeRolesType('treeRolesType" + ((e.target.index()+1)%2) + "')");
                LoadTreeRoles();
            }

        } 
    }

    function ToggleTreeRolesType(toggleId) {
        var toolbar = $("#tbrTreeRolesType").data("kendoToolBar");
        toolbar.toggle("#" + toggleId, true);
    }

    function FindUserOnAdditionalData() {
        return {
            find: $("#txtFindUser").val()
        };
    }
    function FindUserOnSelect(e) {
        var oDataItem = this.dataItem(e.item.index());

        var arrTreePath = treePathToTreePath(oDataItem.UserPath);

        var treeview = $('#treeSecurity').data('kendoTreeView');        
        treeNode(treeview, arrTreePath, "Tree_SelectNodeCallback");
    }

    function Tree_SelectNodeCallback(node, triggerSelect) {
        var treeview = $('#treeSecurity').data('kendoTreeView');
        treeSelectNode(treeview, node, triggerSelect);
    }

</script>

</asp:Content>

