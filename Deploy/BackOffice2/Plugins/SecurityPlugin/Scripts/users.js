function SecurityViewModel(options) {

    var instance;

    //---Properties
    this.Options = options;
    this.InstanceName = options.InstanceName;
    this.BindContainers = options.BindContainers;
    this.LoadingContainer = options.LoadingContainer;
    this.NotificationContainer = options.NotificationContainer;

    this.EntityType = "";
    this.SecurityId = -1;
    this.PrevSecurityId = -1;
    this.UserData = null;
    this.GroupData = null;    
    this.AllRoles = null;
    this.EditableInstallations = null;
    this.HasChanges = false;
    this.RolesChanged = false;
    this.RequiredMsg = options.Resources.Default.FieldRequired;

    this.Events = { OnLoad: options.Events.OnLoad, OnLoadCancel: options.Events.OnLoadCancel, OnSave: options.Events.OnSave, OnCancel: options.Events.OnCancel, OnRemove: options.Events.OnRemove, OnMove: options.Events.OnMove };

    this.AccessLevel = options.AccessLevel;

    this.InBinding = false;

    this.Validator = null;
    this.ValidatorUser = null;
    this.ValidatorGroup = null;

    // --- public functions
    this.bind = function () {
        /*var staticNotification = $("#staticNotification").kendoNotification({
            appendTo: "#tabs"
        }).data("kendoNotification");*/
        if (!instance.get("InBinding")) {
            instance.set("InBinding", true);
            instance.BindContainers.forEach(function(entry) {
                kendo.bind($(entry), instance);
            });
            instance.set("InBinding", false);
        }
    };

    this.showUserForm = function() {
        return instance.get("UserData") !== null;
    };
    this.showGroupForm = function () {
        return instance.get("GroupData") !== null;
    };
    this.showSaveButton = function () {
        return (instance.get("AccessLevel") > 3 && instance.get("EntityType") !== "" && instance.get("HasChanges"));
    };
    this.showNewButton = function () {
        return (instance.get("AccessLevel") >= 9 && (instance.get("SecurityId") == 0 || instance.get("EntityType") == "G") && !instance.get("HasChanges"));
    };
    this.showRemoveButton = function () {
        return (instance.get("AccessLevel") >= 9 && instance.get("EntityType") !== "" && instance.get("SecurityId") > 0 && !instance.get("HasChanges"));
    };

    this.editEnabled = function () {
        return (instance.get("AccessLevel") > 3);
    };
    this.editEnabled2 = function () {
        var oRoles = [];
        var oInstallations = [];
        var oCurUserInstallations = instance.get("EditableInstallations");

        try {
            if (instance.get("EntityType") == "U") {
                oRoles = instance.get("UserData").AllowedRoles;
            }
            else {
                oRoles = instance.get("GroupData").AllowedRoles;
            }
        }
        catch (e) {}

        var sInstallation = "";
        $.each(oRoles, function (i, item) {
            sInstallation = item.split("#")[0];
            if (oInstallations.indexOf(sInstallation) == -1) {
                oInstallations.push(parseInt(sInstallation));
            }
        });

        var bEditEnabled = false;
        if (oCurUserInstallations != null && oCurUserInstallations.length > 0) {
            bEditEnabled = true;
            $.each(oInstallations, function (i, item) {
                if (oCurUserInstallations.indexOf(item) == -1)
                    bEditEnabled = false;
            });
        }

        return bEditEnabled;
    };
        
    this.load = function (id) {
        instance.checkChanges(instance.get("InstanceName") + ".loadConfirm(" + id + ")", instance.get("InstanceName") + ".loadCancel()");
    };
    this.loadConfirm = function (id) {        
        if (id > 0) {            
            instance.Loading(true);
            $.ajax({
                type: 'GET',
                url: instance.Options.Actions.Load,
                data: { plugin: 'SecurityPlugin', securityId: id },
                success: function (response) {

                    try {

                        if (response.Result == true) {

                            if (response.Data != null) {
                                instance.set("EntityType", response.Data.EntityType);
                                instance.set("SecurityId", id);
                                instance.set("PrevSecurityId", 0);
                                instance.set("GroupData", response.Data.GroupData);
                                instance.set("UserData", response.Data.UserData);                                
                                instance.set("AllRoles", response.Data.AllRoles);
                                instance.set("EditableInstallations", response.Data.EditableInstallations);
                                instance.nochange();
                                //instance.bind();
                                instance.validate();                                
                                if (instance.get("Events").OnLoad != "") eval(instance.get("Events").OnLoad + "(instance)");
                            }
                            else {
                                msgboxAlert(instance.Options.Resources.Load.Title, instance.Options.Resources.Default.InvalidId, "warning");
                            }
                        }
                        else {
                            msgboxAlert(instance.Options.Resources.Load.Title, response.ErrorInfo, "warning");
                        }

                    } catch (ex) {
                        msgboxError(instance.Options.Resources.Load.Title, instance.Options.Resources.Load.Exception);
                    }
                    //instance.Loading(false);
                },
                error: function (xhr) {
                    msgboxError(instance.Options.Resources.Load.Title, instance.Options.Resources.Default.CommunicationError);
                    instance.Loading(false);
                }
            });
        }
        else {
            instance.set("EntityType", "");
            instance.set("SecurityId", 0);
            instance.set("PrevSecurityId", 0);
            instance.set("GroupData", null);
            instance.set("UserData", null);            
            instance.set("AllRoles", null);
            instance.set("EditableInstallations", null);
            instance.nochange();
            //instance.bind();
            instance.validate();            
            if (instance.get("Events").OnLoad != "") eval(instance.get("Events").OnLoad + "(instance)");
        }
    };
    this.loadCancel = function () {
        if (instance.get("Events").OnLoadCancel != "") eval(instance.get("Events").OnLoadCancel + "(this)");
    };
    this.save = function () {
        if (instance.get("RolesChanged") == true && instance.get("EntityType") == "G")
            msgboxYesNo(instance.Options.Resources.Save.Title, instance.Options.Resources.Save.ApplyNewRolesToChilds, instance.get("InstanceName") + ".saveConfirm(1)", instance.get("InstanceName") + ".saveConfirm(0)");
        else
            instance.saveConfirm(1);
    };
    this.saveConfirm = function (iApplyNewRolesToChilds) {
        if (instance.validate()) {

            instance.Loading(true);
            
            var oData = null;
            if (instance.get("EntityType") == "G")
                oData = instance.get("GroupData");
            else
                oData = instance.get("UserData");
                
            $.ajax({
                type: 'POST',
                url: instance.Options.Actions.Save,
                data: { plugin: 'SecurityPlugin', securityId: instance.get("SecurityId"), entityType: instance.get("EntityType"), entityData: kendo.stringify(oData), iApplyNewRolesToChilds: iApplyNewRolesToChilds },
                success: function (response) {
                    try {
                        if (response.Result == true) {
                                                                
                            if (response.Data != null) {
                                instance.set("EntityType", response.Data.EntityType);
                                instance.set("GroupData", response.Data.GroupData);
                                instance.set("UserData", response.Data.UserData);                                
                                instance.set("AllRoles", response.Data.AllRoles);
                                instance.set("EditableInstallations", response.Data.EditableInstallations);
                                if (instance.get("SecurityId") == -1) {
                                    var path = "";
                                    if (instance.get("EntityType") == "G")
                                        path = instance.get("GroupData").Path;
                                    else if (instance.get("EntityType") == "U")
                                        path = instance.get("UserData").Path;
                                    if (path != "") {
                                        var arrPath = path.split(",");
                                        instance.set("SecurityId", arrPath[arrPath.length - 1]);
                                    }
                                }
                                instance.nochange();
                                //instance.bind();
                                instance.validate();
                                instance.showNotification(instance.Options.Resources.Save.Notification);
                                if (instance.get("Events").OnSave != "") eval(instance.get("Events").OnSave + "(instance)");
                            }
                            else {
                                msgboxAlert(instance.Options.Resources.Save.Title, instance.Options.Resources.Default.Invalid.Id, "warning");
                            }
                        }
                        else {
                            msgboxAlert(instance.Options.Resources.Save.Title, response.ErrorInfo, "warning");
                        }
                    } catch (ex) {
                        msgboxError(instance.Options.Resources.Save.Title, instance.Options.Resources.Save.Exception);
                    }                    
                    instance.Loading(false);
                },
                error: function (xhr) {
                    msgboxError(instance.Options.Resources.Save.Title, instance.Options.Resources.Default.CommunicationError);
                    instance.Loading(false);
                }
            });

        }
        else
            msgboxAlert(instance.Options.Resources.Save.Title, instance.Options.Resources.Default.InvalidData, "warning");            
    };
    this.cancel = function () {            
        instance.nochange();
        if (instance.get("PrevSecurityId") > 0)
            instance.load(instance.get("PrevSecurityId"));
        else
            instance.load(instance.get("SecurityId"));
    };
    this.remove = function () {
        msgboxConfirm(instance.Options.Resources.Remove.Title, instance.Options.Resources.Remove.AskConfirm, instance.get("InstanceName") + ".removeConfirm()");
    };
    this.removeConfirm = function () {

        instance.Loading(true);

        $.ajax({
            type: 'GET',
            url: instance.Options.Actions.Remove,
            data: { plugin: 'SecurityPlugin', securityId: instance.get("SecurityId") },
            success: function (response) {
                try {
                    if (response.Result == true) {
                        instance.set("EntityType", "");
                        instance.set("GroupData", null);
                        instance.set("UserData", null);                        
                        instance.set("AllRoles", null);
                        instance.set("EditableInstallations", null);
                        //instance.nochange();
                        instance.bind();
                        //instance.validate();
                        instance.showNotification(instance.Options.Resources.Remove.Notification);
                        if (instance.get("Events").OnRemove != "") eval(instance.get("Events").OnRemove + "(instance)");
                    }
                    else {
                        msgboxAlert(instance.Options.Resources.Remove.Title, response.ErrorInfo, "warning");
                    }
                } catch (ex) {
                    msgboxError(instance.Options.Resources.Remove.Title, instance.Options.Resources.Remove.Exception);
                }
                instance.Loading(false);
            },
            error: function (xhr) {
                msgboxError(instance.Options.Resources.Remove.Title, instance.Options.Resources.Default.CommunicationError);
                instance.Loading(false);
            }
        });

    };
    this.newGroup = function () {
        instance.checkChanges(instance.get("InstanceName") + ".newConfirm('G')");
    };
    this.newUser = function () {
        instance.checkChanges(instance.get("InstanceName") + ".newConfirm('U')");
    };
    this.newConfirm = function (type) {
        var instance = this;
        $.ajax({
            type: 'GET',
            url: instance.Options.Actions.New,
            data: { plugin: 'SecurityPlugin', entityType: type, parentId: instance.get("SecurityId") },
            success: function (response) {

                try {
                    //var oResponse = JSON.parse(response);
                    //eval("oReponse = " + response);

                    if (response.Result == true) {

                        if (response.Data != null) {
                            instance.set("EntityType", response.Data.EntityType);
                            instance.set("PrevSecurityId", instance.get("SecurityId"))
                            instance.set("SecurityId", -1);
                            instance.set("GroupData", response.Data.GroupData);
                            instance.set("UserData", response.Data.UserData);                            
                            instance.set("AllRoles", response.Data.AllRoles);
                            instance.set("EditableInstallations", response.Data.EditableInstallations);
                            instance.change();
                            //instance.bind();
                            instance.validate();                            
                            if (instance.get("Events").OnLoad != "") eval(instance.get("Events").OnLoad + "(instance)");
                        }
                        else {
                            msgboxAlert(instance.Options.Resources.New.Title, instance.Options.Resources.Default.InvalidData, "warning");
                        }
                    }
                    else {
                        msgboxAlert(instance.Options.Resources.New.Title, response.ErrorInfo, "warning");
                    }

                } catch (ex) {
                    msgboxError(instance.Options.Resources.New.Title, instance.Options.Resources.New.Exception);
                }
            },
            error: function (xhr) {
                msgboxError(instance.Options.Resources.New.Title, instance.Options.Resources.Default.CommunicationError);
            }
        });

    };

    this.move = function (srcSecurityId, dstSecurityId) {
        msgboxConfirm(instance.Options.Resources.Move.Title, instance.Options.Resources.Move.Confirm, instance.get("InstanceName") + ".moveConfirm(" + srcSecurityId + ", " + dstSecurityId + ")");
    };
    this.moveConfirm = function (srcSecurityId, dstSecurityId) {
        if (instance.validate()) {

            instance.Loading(true);

            var oData = null;
            if (instance.get("EntityType") == "G")
                oData = instance.get("GroupData");
            else
                oData = instance.get("UserData");

            $.ajax({
                type: 'POST',
                url: instance.Options.Actions.Move,
                data: { plugin: 'SecurityPlugin', srcSecurityId: srcSecurityId, dstSecurityId: dstSecurityId },
                success: function (response) {
                    try {
                        if (response.Result == true) {

                            if (response.Data != null) {
                                instance.set("EntityType", response.Data.EntityType);
                                instance.set("GroupData", response.Data.GroupData);
                                instance.set("UserData", response.Data.UserData);
                                instance.set("AllRoles", response.Data.AllRoles);
                                instance.set("EditableInstallations", response.Data.EditableInstallations);
                                if (instance.get("SecurityId") == -1) {
                                    var path = "";
                                    if (instance.get("EntityType") == "G")
                                        path = instance.get("GroupData").Path;
                                    else if (instance.get("EntityType") == "U")
                                        path = instance.get("UserData").Path;
                                    if (path != "") {
                                        var arrPath = path.split(",");
                                        instance.set("SecurityId", arrPath[arrPath.length - 1]);
                                    }
                                }
                                instance.nochange();
                                //instance.bind();
                                instance.validate();
                                instance.showNotification(instance.Options.Resources.Move.Notification);
                                if (instance.get("Events").OnMove != "") eval(instance.get("Events").OnMove + "(instance)");
                            }
                            else {
                                msgboxAlert(instance.Options.Resources.Move.Title, instance.Options.Resources.Default.Invalid.Id, "warning");
                            }
                        }
                        else {
                            msgboxAlert(instance.Options.Resources.Move.Title, response.ErrorInfo, "warning");
                        }
                    } catch (ex) {
                        msgboxError(instance.Options.Resources.Move.Title, instance.Options.Resources.Move.Exception);
                    }
                    instance.Loading(false);
                },
                error: function (xhr) {
                    msgboxError(instance.Options.Resources.Move.Title, instance.Options.Resources.Default.CommunicationError);
                    instance.Loading(false);
                }
            });

        }
        else
            msgboxAlert(instance.Options.Resources.Move.Title, instance.Options.Resources.Default.InvalidData, "warning");
    };

    this.setRoles = function (roles) {
        var sOldRoles = "";
        var oData = null;
        if (instance.get("EntityType") == "G")
            oData = instance.get("GroupData");
        else
            oData = instance.get("UserData");

        sOldRoles = oData.AllowedRoles.join(",");
        oData.AllowedRoles = roles;
        if (sOldRoles != roles.join(",")) {
            instance.change();
            instance.set("RolesChanged", true);
        }
    };

    this.change = function () {
        instance.set("HasChanges", true);
        instance.bind();            
    };
    this.nochange = function () {
        instance.set("HasChanges", false);
        instance.set("RolesChanged", false);
        instance.bind();
    };
    this.checkChanges = function (callbackYes, callbackNo) {
        if (instance.get("HasChanges"))
            msgboxConfirm(instance.Options.Resources.CheckChanges.Title, instance.Options.Resources.CheckChanges.Message, callbackYes, callbackNo);
        else
            eval(callbackYes);
    };
    this.checkRolesChanges = function (callbackYes, callbackNo) {
        if (instance.get("RolesChanged"))
            msgboxConfirm(instance.Options.Resources.CheckChanges.Title, instance.Options.Resources.CheckChanges.Message, callbackYes, callbackNo);
        else
            eval(callbackYes);
    };

    this.validate = function () {
        var validator = instance.get("Validator");
        if (validator != null) {
            validator.validate();
            return true; // validator.validate();
        }
        else
            return true;
    };
                
    this.showNotification = function (message) {
        var notif = $(instance.NotificationContainer).data("kendoNotification");
        notif.show(message, "info");
        //var container = $(notif.options.appendTo);
        //container.scrollTop(container[0].scrollHeight);
    };

    this.Loading = function (show) {
        kendo.ui.progress($(instance.LoadingContainer), show);
    };

    instance = kendo.observable(this);
    return instance;
}

function RoleChange(roleId, roleStatus, setParent) {

    var cnRole = $("#" + roleId);
    if (cnRole != null) {

        var rol_status = parseInt(cnRole.attr("rol_status"));
        var rol_type = cnRole.attr("rol_type");
        if (rol_status != -1 || roleStatus != null) {

            // Get new role status
            if (roleStatus == null)
                rol_status = (rol_status == 1 ? 0 : 1);
            else
                rol_status = roleStatus;
            // Set current role status
            if (rol_type == "A") {
                SetRoleStatus(roleId, rol_status);
                SetRoleStatus("W" + roleId.substr(1), 1);
                SetRoleStatus("R" + roleId.substr(1), 1);
            }
            else if (rol_type == "W") {
                SetRoleStatus("A" + roleId.substr(1), 0);
                SetRoleStatus(roleId, rol_status);
                SetRoleStatus("R" + roleId.substr(1), 1);
            }
            else if (rol_type == "R") {
                SetRoleStatus("A" + roleId.substr(1), 0);
                SetRoleStatus("W" + roleId.substr(1), 0);
                SetRoleStatus(roleId, rol_status);
            }
            // Set childen status
            $("button[rol_parent='" + roleId + "']").each(function () {
                RoleChange(this.id, rol_status, false);
            });
            // Set parents status
            if (setParent == null) setParent = true;
            if (setParent) {
                SetRoleParent("A" + roleId.substr(1));
                SetRoleParent("W" + roleId.substr(1));
                SetRoleParent("R" + roleId.substr(1));
            }
        }

        if (roleStatus == null) {
            viewModel.setRoles(GetAllowedRoles());
        }
    }

}

function SetRoleParent(roleId) {
    var cnRole = $("#" + roleId);
    if (cnRole != null) {
        if (parseInt(cnRole.attr("rol_status")) != -1) {
            var rol_parent = cnRole.attr("rol_parent");
            if (rol_parent != null && rol_parent.length > 1) {
                var rol_status = -1;
                $("button[rol_parent='" + rol_parent + "']").each(function () {
                    var childStatus = parseInt($(this).attr("rol_status"));
                    if (childStatus != -1) {
                        if (rol_status == -1) rol_status = childStatus;
                        if (childStatus == 0) {
                            rol_status = (rol_status > 0 ? 2 : 0);
                        }
                        else if (childStatus == 1) {
                            rol_status = (rol_status == 1 ? 1 : 2);
                        }
                        else {
                            rol_status = 2;
                        }
                    }
                });

                SetRoleStatus(rol_parent, rol_status);

                SetRoleParent(rol_parent);
            }
        }
    }
}

function SetRoleStatus(roleId, roleStatus) {
    var cnRole = $("#" + roleId);
    if (cnRole != null) {
        if (parseInt(cnRole.attr("rol_status")) != -1) {
            var rol_type = cnRole.attr("rol_type");
            cnRole.attr("rol_status", roleStatus);
            cnRole.removeClass("k-state-selected");
            cnRole.removeClass("k-state-selected2");
            var cssRole = "";
            if (roleStatus == 1)
                cssRole = "k-state-selected";
            else if (roleStatus == 2)
                cssRole = "k-state-selected2";
            if (cssRole != "") cnRole.addClass(cssRole);

            var titleRole = "";
            var key = "treeRolesOptions.Resources.RoleNode.";
            var key = key + (rol_type == "A" ? "Admin" : (rol_type == "W" ? "Write" : "Read")) + ".";
            var key = key + (roleStatus == 0 ? "Disabled" : (roleStatus == 1 ? "Enabled" : "Partial"));
            eval("titleRole = " + key + ";");
            cnRole.prop("title", titleRole);
        }
    }
}

function GetAllowedRoles() {
    var roles = [];

    $("button[rol_name!=''][rol_status='1']").each(function () {
        roles.push($(this).attr("rol_name"));
    });

    return roles;
}
