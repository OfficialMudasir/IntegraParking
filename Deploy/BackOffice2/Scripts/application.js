function app_addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            if (oldonload) {
                oldonload();
            }
            func();
        }
    }
}

function app_addUnloadEvent(func) {
    var oldonunload = window.onunload;
    if (typeof window.onunload != 'function') {
        window.onunload = func;
    } else {
        window.onunload = function () {
            if (oldonunload) {
                oldonunload();
            }
            func();
        }
    }
}