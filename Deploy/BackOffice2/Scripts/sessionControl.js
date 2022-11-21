
(function ($) {

    var options = {
        // default options for this plugin
        idle: function () { },             // called when the user fails to respond to prompt
        idleLimitSeconds: 15 * 60,        // 15 minutes        
        promptDuration: 1 * 60 * 1000,        // 60 seconds
        requireButtonClick: true,        // when false, mouse/key messages cancel idle prompt

        title: "",
        modal: true,
        width: "400px",
        visible: false,
        actions: [
            "Close"
        ]/*,
        messages: {
            title: gSiteMaster.session.messages.title,
            message: gSiteMaster.session.messages.message,
            buttons: {
                confirm: {
                    text: gSiteMaster.session.messages.buttons.confirm.text,
                    title: gSiteMaster.session.messages.buttons.confirm.title
                },
                logout: {
                    text: gSiteMaster.session.messages.buttons.logout.text,
                    title: gSiteMaster.session.messages.buttons.logout.title
                }
            }
        }*/

    },

        // constants
        PLUGIN_NAME = 'sessionControl',
        IDLE_CHECK_POLLING_INTERVAL = 1000,
        DIALOG_ID = PLUGIN_NAME + '-dialog',
        PROGRESSBAR_CLASS = PLUGIN_NAME + '-progressbar',

        // private variables
        isStarted = false,
        dialogElement,
        progressBarElement,
        progressBarTimerId,
        idlePollTimerId,
        timeOfLastInteraction = new Date();

    // private functions
    function touchTimeOfLastInteraction() {

        timeOfLastInteraction = new Date();
        if (!options.requireButtonClick && !dialogElement.data("kendoWindow").element.is(":hidden")) {            
            dialogElement.data("kendoWindow").close();
        }

    }
    function showDialogIfRequired() {

        if (!dialogElement.data("kendoWindow").element.is(":hidden")) {
            return;
        }
        if (timeOfLastInteraction.getTime() + (options.idleLimitSeconds * 1000) > (new Date()).getTime()) {
            return;
        }        
        dialogElement.data("kendoWindow").center().open();

    }
    function decrementProgressBarValue() {

        var currentValue = progressBarElement.data("kendoProgressBar").value();
        if (currentValue === 0) {

            dialogElement.data("kendoWindow").close();            
            if (typeof (options.idle) === 'function') {
                options.idle();
            }
        } else {
            progressBarElement.data("kendoProgressBar").value(currentValue - 1);
        }

    }
    function startProgressBarTimer() {

        if (progressBarTimerId) {
            return;
        }
        progressBarElement.data("kendoProgressBar").value(100);
        var tickInterval = options.promptDuration / 100;
        progressBarTimerId = window.setInterval(decrementProgressBarValue, tickInterval);

    }
    function stopProgressBarTimer() {

        if (!progressBarTimerId) {
            return;
        }
        window.clearInterval(progressBarTimerId);
        progressBarTimerId = null;

    }
    function repositionDialog() {

        if (dialogElement.data("kendoWindow").element.is(":hidden")) {
            return;
        }
        // HACK: use setTimeout to queue a re-positioning for the next available
        // clock cycle.  without this hack, the positioning calculations appear
        // to use the pre-resized/scrolled dimensions/layout coordinates.
        window.setTimeout(function () {
            var position = dialogElement.data("kendoWindow").wrapper.offset();
            dialogElement.data("kendoWindow").setOptions({ position: position });
        }, 0);

    }
    function start(o) {

        // sanity checks
        if (isStarted) {
            throw 'Already started';
        }

        $.extend(options, o);

        if (options.messages.message instanceof jQuery && options.messages.message.length > 1) {
            throw 'The "message" option can not contain more than 1 element';
        }

        if (options.idleLimitSeconds > 0) {

            if (options.messages.message instanceof jQuery) {
                dialogElement = options.messages.message;
            } else {
                dialogElement = $('<div id="' + DIALOG_ID + '"><span>' + options.messages.message + '<br/></span></div>');
            }

            dialogElement.kendoWindow(options);
            dialogElement.data("kendoWindow").bind('open', function () { this.idle = options.idle; startProgressBarTimer(); });
            dialogElement.data("kendoWindow").bind('close', stopProgressBarTimer);

            progressBarElement = $('<div class="' + PROGRESSBAR_CLASS + '"></div>')
                .appendTo(dialogElement)
                .kendoProgressBar({
                    showStatus: false,
                    animation: false,
                    min: 0,
                    max: 100
                });

            $('<div>' +
                '<button type="button" class="k-button k-button-icon" ' +
                        'onclick="$(this).parent().parent().data(\'kendoWindow\').close(); return false;" ' +
                        'title="' + options.messages.buttons.confirm.title + '"> ' +
                    '<span class="k-icon k-update"></span>' + options.messages.buttons.confirm.text +
                '</button> ' +
                '<button type="button" class="k-button k-button-icon" ' +
                        'onclick="$(this).parent().parent().data(\'kendoWindow\').close(); if (typeof ($(this).parent().parent().data(\'kendoWindow\').idle) === \'function\') { $(this).parent().parent().data(\'kendoWindow\').idle(); } return false;" ' +
                        'title="' + options.messages.buttons.logout.title + '"> ' +
                    '<span class="k-icon logout16-icon"></span>' + options.messages.buttons.logout.text +
                '</button>' +
             '</div>')
                .appendTo(dialogElement);

            $(window.document)
                .bind('mousemove', touchTimeOfLastInteraction)
                .bind('mousedown', touchTimeOfLastInteraction)
                .bind('keydown', touchTimeOfLastInteraction);

            $(window)
                .bind('scroll', repositionDialog)
                .bind('resize', repositionDialog);

            idlePollTimerId = window.setInterval(showDialogIfRequired, IDLE_CHECK_POLLING_INTERVAL);

            isStarted = true;
        }
    }
    function stop() {

        if (!isStarted) {
            throw 'Not started yet';
        }

        if (!dialogElement.data("kendoWindow").element.is(":hidden")) {
            dialogElement.data("kendoWindow").close();
        }

        window.clearInterval(idlePollTimerId);
        idlePollTimerId = null;

        $(window)
            .unbind('scroll', repositionDialog)
            .unbind('resize', repositionDialog);

        $(window.document)
            .unbind('keydown', touchTimeOfLastInteraction)
            .unbind('mousedown', touchTimeOfLastInteraction)
            .unbind('mousemove', touchTimeOfLastInteraction);

        stopProgressBarTimer();
        progressBarElement.data("kendoProgressBar").destroy();
        progressBarElement.remove();

        dialogElement
            .data("kendoWindow")
            .unbind('close', stopProgressBarTimer)
            .unbind('open', startProgressBarTimer)
            .destroy();

        if (!(options.message instanceof jQuery)) {
            dialogElement.remove();
        }

        isStarted = false;

    }

    // public api
    if (!$[PLUGIN_NAME]) {
        $[PLUGIN_NAME] = {
            start: start,
            stop: stop,
            isStarted: function () {
                return isStarted;
            }
        };
    }

}(jQuery));