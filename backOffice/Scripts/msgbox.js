function msgboxConfirm(title, message, callback_Yes, callback_No) {

    $.when(kendo.ui.ExtOkCancelDialog.show({
        title: title,
        message: message,
        icon: "k-ext-question"
    })
    ).done(function (response) {
        if (response.button == "OK") {
            if (callback_Yes != null && callback_Yes != "")
                eval(callback_Yes);
        }
        else {
            if (callback_No != null && callback_No != "")
                eval(callback_No);
        }
    });

    /*
    var kendoWindow = $("<div />").kendoWindow({
        title: title,
        resizable: false,
        modal: true
    });

    kendoWindow.data("kendoWindow")
        .content($("#msgboxConfirmDialog").html())
        .center().open();

    kendoWindow.find(".confirm-title").html(message);

    kendoWindow.find(".confirm-yes,.confirm-no")
               .click(function () {
                   if ($(this).hasClass("confirm-yes")) {
                       if (callback_Yes != null && callback_Yes != "")
                           eval(callback_Yes);
                   }
                   else {
                       if (callback_No != null && callback_No != "")
                           eval(callback_No);
                   }

                   kendoWindow.data("kendoWindow").close();
               })
                .end();
    */

}

function msgboxError(title, message) {
    msgboxAlert(title, message, "error");
}
function msgboxInformation(title, message) {
    msgboxAlert(title, message, "information");
}

function msgboxAlert(title, message, icon) {

    if (icon == null && icon == "") icon = "information";

    icon = (icon == "information" ? "k-ext-information" : (icon == "question" ? "k-ext-question" : (icon == "warning" ? "k-ext-warning" : "k-ext-error")));
    /*Information: k-ext-information
    Question: k-ext-question
    Warning: k-ext-warning
    Error: k-ext-error*/

    $.when(kendo.ui.ExtAlertDialog.show({
        title: title,
        message: message,
        icon: icon
    })
    ).done(function () {
        //console.log("User clicked the OK button");
    });
}
