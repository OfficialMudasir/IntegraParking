function utilsDeepCopy(obj) {
    if (obj != null) {
        if (Object.prototype.toString.call(obj) === '[object Array]') {
            var out = [], i = 0, len = obj.length;
            for (; i < len; i++) {
                out[i] = arguments.callee(obj[i]);
            }
            return out;
        }
        if (utilsIsDate(obj)) {
            return new Date(obj.getTime());
        }
        if (typeof obj === 'object') {
            var out = {}, i;
            for (i in obj) {
                out[i] = arguments.callee(obj[i]);
            }
            return out;
        }
    }
    return obj;
}

function utilsFilterDates(oData, minutesToAdd) {

    for (var property in oData) {
        if (oData[property] != null) {
            if (utilsIsDate(oData[property])) {
                var min = minutesToAdd;
                if (min == null) {
                    min = -(oData[property].getTimezoneOffset());
                }
                oData[property] = new Date(oData[property].getTime() + (min * 60000));
            }
            else if (Object.prototype.toString.call(oData[property]) === '[object Array]') {
                for (var i = 0; i < oData[property].length; i++) {
                    arguments.callee(oData[property][i], minutesToAdd);
                }
            }
            else if (typeof oData === 'object') {
                arguments.callee(oData[property], minutesToAdd);
            }
        }
    }

}

function utilsParseDates(oData) {
    for (var property in oData) {
        if (oData[property] != null) {
            if (typeof oData[property] === 'string') {
                if (oData[property].substr(0, 6) == "/Date(")
                    oData[property] = kendo.parseDate(oData[property]);
            }
            else {
                if (Object.prototype.toString.call(oData[property]) === '[object Array]') {
                    for (var i = 0; i < oData[property].length; i++) {
                        arguments.callee(oData[property][i]);
                    }
                }
                else if (typeof oData === 'object') {
                    arguments.callee(oData[property]);
                }
            }
        }
    }
}

function utilsIsDate(date) {
    return (date != null && (typeof date.getTime === 'function'));
    //return ( (new Date(date) !== "Invalid Date" && !isNaN(new Date(date)) ));
}

function utilsStringify(obj) {
    var objCopy = utilsDeepCopy(obj);
    utilsFilterDates(objCopy);
    return kendo.stringify(objCopy);
}