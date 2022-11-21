function IPSFilter(grid, options) {

    var instance = this;

    this._filterAll = null;
    this._grid = grid;
    this._options = options;

    this.Load = function (state) {

        if (state != null)
            instance._filterAll = state.filterAll;
        else
            instance._filterAll = null;

    }

    this.GetFilterAll = function () {
        return instance._filterAll;
    }

    this.SetFilterAll = function (filterAll) {

        instance._filterAll = filterAll;

    }

    this.GetGridFilter = function () {

        var retFilter = null;

        if (instance._filterAll != null) {

            retFilter = JSON.parse(kendo.stringify(instance._filterAll));
            instance.FiltersDelDisableds(retFilter);
            if (retFilter.filters.length == 0)
                retFilter = null;

            if (retFilter != null) 
                instance.ParseFilterDates(retFilter, instance._grid.grid.dataSource.options.schema.model.fields);            
        }

        return retFilter;
    }

    /*this.SetGridFilter = function (filter) {

        var filterAllTmp = null;

        if (filter != null) {
            filterAllTmp = JSON.parse(kendo.stringify(filter));
            instance.SetFiltersEnabled(filterAllTmp, true);
            if (instance._filterAll != null) {
                var lstFiltersAll = instance.FiltersList(instance._filterAll);

                for (var i = 0; i < lstFiltersAll.length; i++) {
                    if (!instance.FilterInFilters(filterAllTmp, lstFiltersAll[i])) {
                        lstFiltersAll[i].enabled = false;                            
                        // posar lstFiltersAll[i] al nivell filterAllTmp en funció del field i sinó a l'arrel                            
                        if (!instance.AddFilterInLevel(filterAllTmp, lstFiltersAll[i])) {
                            filterAllTmp.filters.push(lstFiltersAll[i]);
                        }
                    }
                }                    

            }
        }
        else {
            var lstFilters = instance.FiltersList(instance._filterAll);
            for (var i = 0; i < lstFilters.length; i++)
                lstFilters[i].enabled = false;
            filterAllTmp = { filters: lstFilters, logic: "and" };
        }

        instance._filterAll = filterAllTmp;

    }*/

    this.FiltersDelDisableds = function (filter) {

        var bRet = false;

        if (filter != null) {
            if (filter.filters != null && filter.logic != null) {
                var dels = [];
                for (var i = 0; i < filter.filters.length; i++) {
                    if (filter.filters[i].field != null) {
                        if (filter.filters[i].enabled == false) dels.push(i);
                    }
                    else {
                        if (!instance.FiltersDelDisableds(filter.filters[i])) {
                            dels.push(i);
                        }
                    }
                }
                for (var i = dels.length - 1; i >= 0; i--)
                    filter.filters.splice(dels[i], 1);
                bRet = (filter.filters.length > 0);
            }
        }

        return bRet;
    }

    this.ParseFilterDates = function (filter, fields, parent, index) {
        if (filter.filters) {
            for (var i = 0; i < filter.filters.length; i++) {
                instance.ParseFilterDates(filter.filters[i], fields, filter.filters, i);
            }
        }
        else {
            if (fields[filter.field].type == "date") {
                var now = new Date();
                var to = kendo.parseDate(now.getFullYear() + "/" + ("0" + (now.getMonth() + 1)).slice(-2) + "/" + ("0" + now.getDate()).slice(-2) + " 23:59:59", "yyyy/MM/dd HH:mm:ss");
                var from = null;
                var filterTo = { field: filter.field, operator: "lte", value: to };
                if (filter.operator == "lastweek") {
                    from = kendo.parseDate(now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate(), "yyyy/MM/dd");
                    from.setDate(to.getDate() - 7);
                }
                else if (filter.operator == "lastmonth") {
                    from = kendo.parseDate(now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate(), "yyyy/MM/dd");
                    from.setMonth(to.getMonth() - 1);
                }
                else if (filter.operator == "lastyear") {
                    from = kendo.parseDate(now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate(), "yyyy/MM/dd");
                    from.setFullYear(to.getFullYear() - 1);
                }
                else if (filter.operator == "currentweek") {
                    from = kendo.parseDate(now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate(), "yyyy/MM/dd");
                    var weekDay = to.getDay();
                    weekDay = (weekDay == 0 ? 6 : weekDay - 1);
                    from.setDate(to.getDate() - weekDay);
                }
                else if (filter.operator == "currentmonth") {
                    from = kendo.parseDate(now.getFullYear() + "/" + (now.getMonth() + 1) + "/01", "yyyy/MM/dd");                    
                }
                else if (filter.operator == "currentyear") {
                    from = kendo.parseDate(now.getFullYear() + "/01/01", "yyyy/MM/dd");
                }
                else if (filter.operator == "currentday") {                    
                    from = kendo.parseDate(now.getFullYear() + "/" + ("0" + (now.getMonth() + 1)).slice(-2) + "/" + ("0" + now.getDate()).slice(-2) + " 00:00:00", "yyyy/MM/dd HH:mm:ss");
                    to = kendo.parseDate(now.getFullYear() + "/" + ("0" + (now.getMonth() + 1)).slice(-2) + "/" + ("0" + now.getDate()).slice(-2) + " 23:59:59", "yyyy/MM/dd HH:mm:ss");
                    filterTo = { field: filter.field, operator: "lte", value: to };
                }

                else
                    filter.value = kendo.parseDate(filter.value);
                
                if (from != null) {
                    var filterFrom = { field: filter.field, operator: "gte", value: from };

                    filter.filters = [];
                    filter.logic = "and";
                    filter.filters.push(filterFrom);
                    filter.filters.push(filterTo);
                    filter.field = null;
                    filter.operator = null;
                    filter.value = null;

                    //filter.operator = "gte";
                    //filter.value = from;                    
                    //parent.splice(index + 1, 0, filterTo);
                }
            }
        }
    }

    this.SetFiltersEnabled = function (filter, enabled, filterItem, path) {
        if (filter == null) {
            filter = instance._filterAll;
            path = "1";
        }
        if (filter != null) {
            if (filter.filters != null && filter.logic != null) {
                for (var i = 0; i < filter.filters.length; i++) {
                    if (filter.filters[i] != null)
                        instance.SetFiltersEnabled(filter.filters[i], enabled, filterItem, path + "/" + (i+1));
                }
            }
            else {
                if (filterItem != null) {
                    //if (instance.FiltersEqual(filter, filterItem))
                    if (path == filterItem.path)
                        filter.enabled = enabled;
                }
                else
                    filter.enabled = enabled;
            }
        }
    }

    this.FiltersEqual = function (filter1, filter2) {
        return (filter1.field == filter2.field && filter1.operator == filter2.operator && filter1.value == filter2.value);
    }

    this.FiltersList = function (filter) {
        var list = [];
        if (filter != null) {
            if (filter.filters != null && filter.logic != null) {
                for (var i = 0; i < filter.filters.length; i++)
                    list = list.concat(instance.FiltersList(filter.filters[i]));
            }
            else {
                list.push(JSON.parse(kendo.stringify(filter)));
            }
        }
        return list;
    }

    this.FilterInFilters = function (filter, filterItem) {
        var bRet = false;
        if (filter != null) {
            if (filter.filters != null && filter.logic != null) {
                for (var i = 0; i < filter.filters.length && !bRet; i++) {
                    bRet = instance.FilterInFilters(filter.filters[i], filterItem);
                }
            }
            else {
                bRet = instance.FiltersEqual(filter, filterItem);
            }
        }
        return bRet;
    }


    this.AddFilterInLevel = function (filter, filterItem) {
        var bRet = false;
        if (filter != null) {
            if (filter.filters != null && filter.logic != null) {
                for (var i = 0; i < filter.filters.length && !bRet; i++) {
                    if (filter.filters[i].filters != null) {
                        bRet = instance.AddFilterInLevel(filter.filters[i], filterItem);
                    }
                    else {
                        bRet = (filter.filters[i].field == filterItem.field);
                        if (bRet) {
                            filter.filters.push(filterItem);
                        }
                    }
                }
            }
        }
        return bRet;
    }

    this.ClearFilter = function (filter) {

        var bRet = false;

        if (filter == null) filter = instance._filterAll;

        if (filter != null) {
            if (filter.filters != null) {
                var dels = [];
                for (var i = 0; i < filter.filters.length; i++) {
                    if (!instance.ClearFilter(filter.filters[i])) {
                        dels.push(i);
                    }
                }
                for (var i = dels.length - 1; i >= 0; i--)
                    filter.filters.splice(dels[i], 1);
                bRet = (filter.filters.length > 0);
                if (!bRet) {
                    filter = null;
                }
            }
            else
                bRet = true;
        }

        return bRet;
    }

    this.GetFilterInfo = function (filter) {

        var sInfo = "";

        if (filter.filters != null) {
            for (var i = 0; i < filter.filters.length; i++) {                
                sInfo += "(" + instance.GetFilterInfo(filter.filters[i]) + ")";
                if (i < (filter.filters.length - 1)) sInfo += "\n\r" + instance.LogicDesc(filter.logic) + "\n\r";
            }
        }
        else if (filter.field != null) {
            var sValue = filter.value;
            if (instance._grid.grid.dataSource.options.schema.model.fields[filter.field].type == "date") {
                sValue = kendo.toString(filter.value, instance._options.shortDateTimeFormat);
            }
            else if (instance._grid.grid.dataSource.options.schema.model.fields[filter.field + "_FK"] != null && filter.valueFK != null) {
                sValue = filter.valueFK;
            }
            sInfo = instance._grid.ColumnTitle(filter.field) + " " + instance.OperatorDesc(filter.operator) + " " + sValue;
        }
        return sInfo;
    }

    this.OperatorDesc = function (operator) {
        var desc = operator;
        switch (operator) {
            case "eq": desc = instance._options.messages.operator.eq; break;
            case "neq": desc = instance._options.messages.operator.neq; break;
            case "lt": desc = instance._options.messages.operator.lt; break;
            case "lte": desc = instance._options.messages.operator.lte; break;
            case "gt": desc = instance._options.messages.operator.gt; break;
            case "gte": desc = instance._options.messages.operator.gte; break;
            case "startswith": desc = instance._options.messages.operator.startswith; break;
            case "endswith": desc = instance._options.messages.operator.endswith; break;
            case "contains": desc = instance._options.messages.operator.contains; break;
            case "doesnotcontain": desc = instance._options.messages.operator.doesnotcontain; break;
            case "lastweek": desc = instance._options.messages.operator.lastweek; break;
            case "lastmonth": desc = instance._options.messages.operator.lastmonth; break;
            case "lastyear": desc = instance._options.messages.operator.lastyear; break;
            case "currentweek": desc = instance._options.messages.operator.currentweek; break;
            case "currentmonth": desc = instance._options.messages.operator.currentmonth; break;
            case "currentyear": desc = instance._options.messages.operator.currentyear; break;
            case "currentday": desc = instance._options.messages.operator.currentday; break;
        }
        return desc;
    }

    this.LogicDesc = function (logic) {
        var desc = logic;
        switch (logic) {
            case "and": desc = instance._options.messages.logic.and; break;
            case "or": desc = instance._options.messages.logic.or; break;
        }
        return desc;
    }

    this.GetAllFiltersInfo = function (filters) {

        var sInfo = "";
        var filter = null;

        if (filters != null) {
            sInfo = instance.GetFilterInfo(filters);
        }
        
        return sInfo;
    }

    this.GetTreeNode = function (filter, path, level, parent) {

        var node = null;

        if (filter == null) filter = instance._filterAll;
        if (path == null) path = "";
        if (level == null) level = 1;

        if (filter != null && !filter.internal) {
            
            var fields = instance._grid.options.fields;

            if (path != "") path = path + "/";            
            path = path + level;

            if (filter.filters != null && filter.logic != null) {                
                level = 1;
                var items = [];
                for (var i=0; i < filter.filters.length; i++) {
                    if (filter.filters[i] != null) {
                        var oNode = instance.GetTreeNode(filter.filters[i], path, level, filter);
                        if (oNode != null) items.push(oNode);
                        level = level + 1;
                    }
                }
                if (items.length > 0)                    
                    node = {
                        id: { path: path, pathId: path.split("/").join("-"), logic: filter.logic },
                        path: path,
                        text: instance.LogicDesc(filter.logic),
                        expanded: true, dirty: false, items: items
                    };
            }
            else {                
                node = {
                    id: { path: path, pathId: path.split("/").join("-"), field: filter.field, operator: filter.operator, value: filter.value, logic: parent.logic, type: fields[filter.field].TypeString },
                    path: path,
                    text: /*instance.GetFilterInfo(filter)*/"",
                    checked: (filter.enabled != null ? filter.enabled : true)
                    //imageUrl: instance._options.routeConfig.absoluteBasePath + "Content/images/grid/filter16.png"
                };
            }
        }
        return node;
    }

    this.GetTreeCheckedFilters = function (nodes, checkedFilters) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].hasChildren) {
                instance.GetTreeCheckedFilters(nodes[i].children.view(), checkedFilters);
            }
            else {
                if (nodes[i].checked) 
                    checkedFilters.push(nodes[i].id);
            }
        }
    }

    this.AddFilter = function (filterLogic, filterData, treePath) {

        var bRet = false;

        if (instance._filterAll == null || instance._filterAll.filters == null) {
            instance._filterAll = { filters: [], logic: "and" };
        }
        
        var curFilter = instance._filterAll;
        var iPos = -1;
        if (treePath != null && treePath != "") {

            var arr = treePath.split("/");

            for (var i = 1; i < arr.length; i++) {
                if (curFilter.filters[arr[i] - 1] != null) {
                    if (curFilter.filters[arr[i] - 1].filters != null) {
                        curFilter = curFilter.filters[arr[i] - 1];
                    }
                    else {
                        iPos = arr[i] - 1;
                        i = arr.length;
                    }
                }
                else {
                    //ERROR
                    i = arr.length;
                }
            }

        }

        if (curFilter.filters != null) {
            bRet = true;
            if (curFilter.logic != filterLogic) {
                if (curFilter.filters.length > 0) {
                    var newFilter = { filters: [], logic: filterLogic };
                    if (iPos == -1) {
                        curFilter.filters.push(newFilter);
                    }
                    else
                        curFilter.filters.splice(iPos, 0, newFilter);
                    curFilter = newFilter;
                }
                else
                    curFilter.logic = filterLogic;
            }
            if (iPos == -1) {
                curFilter.filters.push(filterData);
            }
            else
                curFilter.filters.splice(iPos, 0, filterData);
        }

        return bRet;
    }

    this.EditFilter = function (filterLogic, filterData, treePath) {

        var bChanged = false;
        var bRet = false;

        if (instance._filterAll == null || instance._filterAll.filters == null) {
            instance._filterAll = { filters: [], logic: "and" };
            bChanged = true;
        }

        var curFilter = instance._filterAll;
        var iPos = -1;
        if (treePath != null && treePath != "") {

            var arr = treePath.split("/");

            for (var i = 1; i < arr.length; i++) {
                if (curFilter.filters[arr[i] - 1] != null) {
                    if (curFilter.filters[arr[i] - 1].filters != null) {
                        curFilter = curFilter.filters[arr[i] - 1];
                    }
                    else {
                        iPos = arr[i] - 1;
                        i = arr.length;
                    }
                }
                else {
                    //ERROR
                    i = arr.length;
                }
            }

        }

        if (curFilter.filters != null) {
            bRet = true;
            if (filterData == null || filterData.field == "") {
                if (!bChanged) bChanged = (curFilter.logic != filterLogic);
                curFilter.logic = filterLogic;
            }
            else {
                if (!bChanged) bChanged = (curFilter.filters[iPos] == null ||
                                           curFilter.filters[iPos].field != filterData.field ||
                                           curFilter.filters[iPos].operator != filterData.operator ||
                                           curFilter.filters[iPos].value != filterData.value);
                curFilter.filters[iPos] = filterData;
            }
        }

        return bChanged;
    }

    this.DeleteFilter = function (treePath) {

        var bRet = true;

        if (instance._filterAll == null || instance._filterAll.filters == null) {
            instance._filterAll = { filters: [], logic: "and" };
        }

        var curFilter = instance._filterAll;
        var iPos = -1;
        if (treePath != null && treePath != "") {

            var arr = treePath.split("/");

            for (var i = 1; i < arr.length; i++) {
                if (curFilter.filters[arr[i] - 1] != null) {
                    if (curFilter.filters[arr[i] - 1].filters != null) {
                        curFilter = curFilter.filters[arr[i] - 1];
                    }
                    else {
                        iPos = arr[i] - 1;
                        i = arr.length;
                    }
                }
                else {
                    //ERROR
                    i = arr.length;
                }
            }

            if (iPos > -1) {
                curFilter.filters.splice(iPos, 1);
                instance.ClearFilter();
            }
            else {
                curFilter.filters = [];
                instance.ClearFilter();
            }

        }

        return bRet;
    }

    this.DeleteFilterByField = function (field, onlyInternal, curFilter, path) {

        var bRet = false;

        if (instance._filterAll == null || instance._filterAll.filters == null) {
            instance._filterAll = { filters: [], logic: "and" };
        }

        var root = false;
        if (curFilter == null) {
            curFilter = instance._filterAll;
            root = true;
        }
        if (path == null) path = "1";

        var iPos = -1;        
        if (field != null && field != "") {

            for (var i = 0; i < curFilter.filters.length && !bRet; i++) {
                if (curFilter.filters[i].logic == null) {
                    if (curFilter.filters[i].field == field &&
                        (!onlyInternal || curFilter.filters[i].internal)) {
                        //path += "/" + (i + 1);
                        bRet = instance.DeleteFilter(path + "/" + (i + 1));
                    }
                }
                else {
                    bRet = instance.DeleteFilterByField(field, onlyInternal, curFilter.filters[i], path + "/" + (i + 1));
                }
            }

            //if (path != "") bRet = instance.DeleteFilter(path);
            if (bRet && root) bRet = instance.DeleteFilterByField(field, onlyInternal);
        }

        return bRet;
    }
    this.DeleteFilterByInternal = function (curFilter, path) {

        var bRet = false;

        if (instance._filterAll == null || instance._filterAll.filters == null) {
            instance._filterAll = { filters: [], logic: "and" };
        }

        var root = false;
        if (curFilter == null) {
            curFilter = instance._filterAll;
            root = true;
        }
        if (path == null) path = "1";

        var iPos = -1;

        for (var i = 0; i < curFilter.filters.length && !bRet; i++) {
            if (curFilter.filters[i].logic == null) {
                if (curFilter.filters[i].internal) {
                    //path += "/" + (i + 1);
                    bRet = instance.DeleteFilter(path + "/" + (i + 1));
                }
            }
            else {
                bRet = instance.DeleteFilterByInternal(curFilter.filters[i], path + "/" + (i + 1));
            }
        }
            
        if (bRet && root) bRet = instance.DeleteFilterByInternal();

        return bRet;
    }

    this.CleanFiltersInternal = function (filter) {

        if (filter.internal == true) {
            filter = null;
        }
        else {
            if (filter.filters != null) {
                var tmpFilters = [];
                $.each(filter.filters, function (index, value) {
                    if (value.internal == null || !value.internal) {                        
                        if (value.filters != null) {
                            var tmp = instance.CleanFiltersInternal(value);
                            if (tmp != null) tmpFilters.push(tmp);
                        }
                        else
                            tmpFilters.push(value);
                    }
                });
                filter.filters = tmpFilters;
            }            
        }

        if (filter.filters != null && filter.filters.length == 0)
            filter = null;

        return filter;
    };
    this.CleanFiltersValues = function (filter) {
        if (filter != null && filter.filters != null) {            
            $.each(filter.filters, function (index, value) {                
                if (value.filters != null) {
                    instance.CleanFiltersValues(value);                    
                }
                else {
                    value.value = null;
                }                        
            });            
        }
    };

    this.ConvertFiltersToCurrentCulture = function (filter) {
        if (filter != null) {
            if (filter.noculture) {
                filter.noculture = false;
                instance.TransformFilterToCurrentCulture(filter, instance._grid.options.fields);
            }
        }
    };
    this.TransformFilterToCurrentCulture = function (filter, fields) {

        if (filter.filters != null) {
            for (var i = 0; i < filter.filters.length; i++) {
                instance.TransformFilterToCurrentCulture(filter.filters[i], fields);
            }
        }
        else {
            switch (fields[filter.field].TypeString) {
                case "Float":
                    var separator = kendo.culture().numberFormat["."];
                    filter.value = filter.value.replace(".", separator);
                    break;
                case "DateTime": 
                    var dtValue = kendo.toString(kendo.parseDate(filter.value, "yyyy/MM/dd HH:mm:ss"), instance._options.shortDateTimeFormat);
                    filter.value = dtValue;
                    break;
                case "Date":
                    var dtValue = kendo.toString(kendo.parseDate(filter.value, "yyyy/MM/dd"), instance._options.shortDateFormat);
                    filter.value = dtValue;
                    break;
                case "Time":
                    var dtValue = kendo.toString(kendo.parseDate(filter.value, "HH:mm:ss"), instance._options.shortTimeFormat);
                    filter.value = dtValue;
                    break;                                    
            }
        }

    };

    this.ConvertFiltersToNoCulture = function (filter) {
        if (filter != null) {
            if (filter.noculture != true) {
                filter.noculture = true;
                instance.TransformFilterToNoCulture(filter, instance._grid.options.fields);
            }
        }
    };
    this.TransformFilterToNoCulture = function (filter, fields) {

        if (filter.filters != null) {
            for (var i = 0; i < filter.filters.length; i++) {
                instance.TransformFilterToNoCulture(filter.filters[i], fields);
            }
        }
        else {
            switch (fields[filter.field].TypeString) {
                case "Float":
                    var separator = kendo.culture().numberFormat["."];
                    filter.value = filter.value.replace(separator, ".");
                    break;
                case "DateTime":                     
                    filter.value = kendo.toString(kendo.parseDate(filter.value), "yyyy/MM/dd HH:mm:ss");                    
                    break;
                case "Date":
                    filter.value = kendo.toString(kendo.parseDate(filter.value), "yyyy/MM/dd");
                    break;
                case "Time":
                    filter.value = kendo.toString(kendo.parseDate(filter.value), "HH:mm:ss");
                    break;
            }
        }

    };

}