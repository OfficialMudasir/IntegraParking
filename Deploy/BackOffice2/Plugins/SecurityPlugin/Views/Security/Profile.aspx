<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Profile</title>
    <link href="<%= Url.Content(RouteConfig.BasePath + "Content/profile.css?v1.1") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content(RouteConfig.BasePath + "Scripts/profile.js?v1.0") %>"></script>

    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    <% var resBundleBackOffice = ResourceBundle.GetInstance("backOffice"); %>

    <script type="text/javascript">
    
        var modelOptionsProfile = {
            InstanceName: "viewProfileModel",
            BindContainers: [".profile-form"],
            LoadingContainer: ".profile-form",
            NotificationContainer: "#spnProfileNotification",
            Actions: {
                Load: "<%= Url.Action("ProfileRead", "Security", new { plugin = "SecurityPlugin" }) %>",
                Save: "<%= Url.Action("ProfileSave", "Security", new { plugin = "SecurityPlugin" }) %>"
            },
            Events: {
                OnLoad: "ViewProfileModel_OnLoad",
                OnLoadCancel: "",
                OnSave: "", OnCancel: "",
                OnRemove: "",
                OnMove: ""
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
                    Title: "<%= resBundle.GetString("Security", "ProfileView.Load.Title", "Load") %>",
                    Exception: "<%= resBundle.GetString("Security", "ProfileView.Load.Exception", "Error loading data") %>"
                },
                Save: {
                    Title: "<%= resBundle.GetString("Security", "ProfileView.Save.Title", "Save") %>",
                    Exception: "<%= resBundle.GetString("Security", "ProfileView.Save.Exception", "Error saving") %>",
                    Notification: "<%= resBundle.GetString("Security", "ProfileView.Save.Notification", "Data saved") %>"                    
                },
                CheckChanges: {
                    Title: "<%= resBundle.GetString("Security", "ProfileView.CheckChanges.Title", "Pending Changes") %>",
                    Message: "<%= resBundle.GetString("Security", "ProfileView.CheckChanges.Message", "Discard pending changes?") %>"
                }
            }            
        };

        var viewProfileModel = new ProfileViewModel(modelOptionsProfile);

        $(document).ready(function () {
           
            viewProfileModel.bind();
            viewProfileModel.Validator = $(".profile-form").kendoValidator({
                rules: {
                    matches: function (input) {

                        var matches = input.data('matches');
                        // if the `data-matches attribute was found`
                        if (matches) {
                            // get the input to match
                            var match = $(matches);
                            // trim the values and check them
                            if ($.trim(input.val()) === $.trim(match.val())) {
                                // the fields match
                                return true;
                            } else {
                                // the fields don't match - validation fails
                                return false;
                            }
                        }
                        // don't perform any match validation on the input
                        return true;
                    }
                },
                messages: {                    
                    matches: function (input) {
                        return input.data("matchesMsg");
                    }
                }
            }).data("kendoValidator");
            //viewProfileModel.Validator.validate();

            viewProfileModel.load();

            $("#Profile_MainTelCountry").kendoComboBox({
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
            $("#Profile_SecondTelCountry").kendoComboBox({
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

            $("#Profile_Language").kendoComboBox({
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

            $("#Profile_Country").kendoComboBox({
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

            $("#Profile_Currency").kendoComboBox({
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

            $("#Profile_MaintenanceMode").kendoComboBox({
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
                },
                change: function (e) {
                    if (this.value() && this.selectedIndex == -1) {
                        viewProfileModel.get("UserData").MaintenanceMode = null;
                    }
                }
            });

        });

        function ViewProfileModel_OnLoad(model) {

            $("#User_PwdConfirm").val(model.get("UserData").Pwd);
            model.validate();

        }

    </script>
</head>
<body>
    <% var resBundle = ResourceBundle.GetInstance("SecurityPlugin"); %>
    <% var resBundleBackOffice = ResourceBundle.GetInstance("backOffice"); %>

    <div>
        <div id="divProfile" class="profile-form k-edit-form-container"  >            
            <div id="divProfileNotification">                
                <%= Html.Kendo().Notification()
                    .Name("spnProfileNotification")
                    .AppendTo("#divProfileNotification")
                    /*.Stacking(NotificationStackingSettings.Down)            
                    .Button(true)*/
                %>                
            </div>

            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Username", resBundle.GetString("Security", "UsersView.User.Username", "User name")) %>
                </div>
                <div class="editor-field">
                    <%= /*Html.TextBoxFor(x => x.Username, new { @class = "k-input k-textbox" }) */
                        Html.TextBox("User_Username", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Username, events: { change: change }, attr: { data-required-msg: RequiredMsg }, enabled: editEnabled"} , { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" }, { "tabindex", "1" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Username") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Name", resBundle.GetString("Security", "UsersView.User.Name", "Name")) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("User_Name", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Name, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Name") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Surname1", resBundle.GetString("Security", "UsersView.User.Surname1", "Surname1")) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("User_Surname1", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Surname1, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Surname1") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Surname2", resBundle.GetString("Security", "UsersView.User.Surname2", "Surname2")) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("User_Surname2", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Surname2, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Surname2") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Pwd", resBundle.GetString("Security", "UsersView.User.Password", "Password")) %>
                </div>
                <div class="editor-field">
                    <%= Html.Password("User_Pwd", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Pwd, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" }, { "data-max-msg", "please enter a number less than 100" }, { "min", "1" }, { "max", "100" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Pwd") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_PwdConfirm", resBundle.GetString("Security", "UsersView.User.PasswordConfirm", "Conirm Password")) %>
                </div>
                <div class="editor-field">
                    <%= Html.Password("User_PwdConfirm", null, new Dictionary<string, Object> { { "data-bind", "events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { "required", "" }, { "data-max-msg", "please enter a number less than 100" }, { "min", "1" }, { "max", "100" }, { "data-matches", "#User_Pwd"}, { "data-matches-msg", resBundle.GetString("Security", "UsersView.User.PasswordConfirm.MatchMessage", "The passwords do not match") } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_PwdConfirm") %>
                </div>
            </div>
            
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Email", resBundle.GetString("Security", "UsersView.User.Email", "Email")) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("User_Email", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Email, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" }, { (backOffice.Infrastructure.Security.FormAuthMemberShip.MembershipService.RequiresUniqueEmail()?"required":"norequired"), "" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Email") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("User_Email2", resBundle.GetString("Security", "UsersView.User.Email2", "Secondary Email")) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextBox("User_Email2", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.Email2, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                    <br />
                    <%= Html.ValidationMessage("User_Email2") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_MainTel", resBundle.GetString("Security", "UsersView.User.MainTel", "Main Phone Number")) %>
                </div>
                <div class="editor-field">
                    <div class="editor-label sub">
                        <input id="Profile_MainTelCountry"
                                data-bind="value: UserData.MainTelCouId, events: { change: change }, enabled: editEnabled"
                                style="width: 100%;"/>
                        <%= Html.ValidationMessage("Profile_MainTelCountry") %>
                    </div>
                    <div class="editor-field sub2">
                        <div class="editor-field sub">
                            <%= Html.TextBox("Profile_MainTel", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.MainTel, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                            <%= Html.ValidationMessage("Profile_MainTel") %>
                        </div>
                    </div>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_SecondTel", resBundle.GetString("Security", "UsersView.User.SecondTel", "Secondary Phone Number")) %>
                </div>
                <div class="editor-field">
                    <div class="editor-label sub">
                        <input id="Profile_SecondTelCountry"
                                data-bind="value: UserData.SecondTelCouId, events: { change: change }, enabled: editEnabled"
                                style="width: 100%;"/>
                        <%= Html.ValidationMessage("Profile_SecondTelCountry") %>
                    </div>
                    <div class="editor-field sub2">
                        <div class="editor-field sub">
                            <%= Html.TextBox("Profile_SecondTel", null, new Dictionary<string, Object> { { "data-bind", "value: UserData.SecondTel, events: { change: change }, attr: { validationMessage: RequiredMsg}, enabled: editEnabled" }, { "class", "k-input k-textbox" }, { "style", "width:100%;" } }) %>
                            <%= Html.ValidationMessage("Profile_SecondTel") %>
                        </div>
                    </div>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_Language", resBundle.GetString("Security", "UsersView.User.Language", "Language")) %>
                </div>
                <div class="editor-field">
                    <input id="Profile_Language"                                                       
                            data-bind="value: UserData.LanId, events: { change: change }, enabled: editEnabled"
                            data_val_required="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                            data-required-msg="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                            
                            style="width: 100%;"/>
                    <br />
                    <%= Html.ValidationMessage("Profile_Language", new { style = "z-index: 0;" })%>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_Country", resBundle.GetString("Security", "UsersView.User.Country", "Country")) %>
                </div>
                <div class="editor-field">
                    <input id="Profile_Country"
                            data-bind="value: UserData.CouId, events: { change: change }, enabled: editEnabled"                                                       
                            style="width: 100%;"/>
                    <br />
                    <%= Html.ValidationMessage("Profile_Country") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_Currency", resBundle.GetString("Security", "UsersView.User.Currency", "Currency")) %>
                </div>
                <div class="editor-field">
                    <input id="Profile_Currency"
                            data-bind="value: UserData.CurId, events: { change: change }, enabled: editEnabled"
                            style="width: 100%;"/>
                    <br />
                    <%= Html.ValidationMessage("Profile_Currency") %>
                </div>
            </div>
            <div class="editor-container">
                <div class="editor-label">
                    <%= Html.Label("Profile_MaintenanceMode", resBundle.GetString("Security", "UsersView.User.MaintenanceMode", "Maintenance Mode")) %>
                </div>
                <div class="editor-field">
                    <input id="Profile_MaintenanceMode"                                                       
                            data-bind="value: UserData.MaintenanceMode, events: { change: change }, enabled: editEnabled"
                            data_val_required="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                            data-required-msg="<%= resBundleBackOffice.GetString("Common", "Default.Field.Required", "Field required") %>"
                            
                            style="width: 100%;"/>
                    <br />
                    <%= Html.ValidationMessage("Profile_MaintenanceMode", new { style = "z-index: 0;" })%>
                </div>
            </div>
            <div id="divProfileToolbar" class="editor-container">
                <div class="editor-field">

                    <button type="button" id="btnSave" class="k-button k-button-icon"
                            data-bind="click: save, enabled: HasChanges, visible: showSaveButton"
                            title="<%= resBundle.GetString("Security", "ProfileView.Button.Save.Title", "Save pending changes") %>">
                        <span class="k-icon save16-icon">  </span><%= resBundle.GetString("Security", "ProfileView.Button.Save", "Save") %>
                    </button>
                    <button type="button" id="btnCancel" class="k-button k-button-icon"
                            data-bind="click: cancel, enabled: HasChanges, visible: showSaveButton"
                            title="<%= resBundle.GetString("Security", "ProfileView.Button.Cancel.Title", "Cancel last changes") %>">
                        <span class="k-icon cancel16-icon">  </span><%= resBundle.GetString("Security", "ProfileView.Button.Cancel", "Cancel") %>
                    </button>

                </div>
            </div>
        </div>

    </div>
</body>
</html>
