function ProfileViewModel(options) {

    var instance;

    //---Properties
    this.Options = options;
    this.InstanceName = options.InstanceName;
    this.BindContainers = options.BindContainers;
    this.LoadingContainer = options.LoadingContainer;
    this.NotificationContainer = options.NotificationContainer;

    this.UserData = null;

    this.HasChanges = false;
    this.RequiredMsg = options.Resources.Default.FieldRequired;
    
    this.Events = { OnLoad: options.Events.OnLoad, OnLoadCancel: options.Events.OnLoadCancel, OnSave: options.Events.OnSave, OnCancel: options.Events.OnCancel, OnRemove: options.Events.OnRemove, OnMove: options.Events.OnMove };
    
    this.InBinding = false;

    this.Validator = null;

    // --- public functions
    this.bind = function () {
        /*var staticNotification = $("#staticNotification").kendoNotification({
            appendTo: "#tabs"
        }).data("kendoNotification");*/
        if (!instance.get("InBinding")) {
            instance.set("InBinding", true);
            instance.BindContainers.forEach(function (entry) {
                kendo.bind($(entry), instance);
            });
            instance.set("InBinding", false);
        }
    };

    this.showSaveButton = function () {
        return (instance.get("HasChanges"));
    };

    this.editEnabled = function () {
        return true;
    };

    this.load = function () {
        instance.checkChanges(instance.get("InstanceName") + ".loadConfirm()", instance.get("InstanceName") + ".loadCancel()");
    };
    this.loadConfirm = function () {        
        instance.Loading(true);
        $.ajax({
            type: 'GET',
            url: instance.Options.Actions.Load,
            data: { },
            success: function (response) {

                try {

                    if (response.Result == true) {

                        if (response.Data != null) {
                            instance.set("UserData", response.Data);
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
                instance.Loading(false);
            },
            error: function (xhr) {
                msgboxError(instance.Options.Resources.Load.Title, instance.Options.Resources.Default.CommunicationError);
                instance.Loading(false);
            }
        });
    };
    this.loadCancel = function () {
        if (instance.get("Events").OnLoadCancel != "") eval(instance.get("Events").OnLoadCancel + "(this)");
    };
    this.save = function () {
        if (instance.validate()) {

            instance.Loading(true);

            var oData = instance.get("UserData");

            $.ajax({
                type: 'POST',
                url: instance.Options.Actions.Save,
                data: { plugin: 'SecurityPlugin', entityData: kendo.stringify(oData) },
                success: function (response) {
                    try {
                        if (response.Result == true) {

                            if (response.Data != null) {
                                instance.set("UserData", response.Data);
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
        instance.load();
    };

    this.change = function () {
        instance.set("HasChanges", true);
        instance.bind();
    };
    this.nochange = function () {
        instance.set("HasChanges", false);        
        instance.bind();
    };
    this.checkChanges = function (callbackYes, callbackNo) {
        if (instance.get("HasChanges"))
            msgboxConfirm(instance.Options.Resources.CheckChanges.Title, instance.Options.Resources.CheckChanges.Message, callbackYes, callbackNo);
        else
            eval(callbackYes);
    };

    this.validate = function () {
        var validator = instance.get("Validator");
        if (validator != null) {
            return validator.validate();
            //return true; // validator.validate();
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

