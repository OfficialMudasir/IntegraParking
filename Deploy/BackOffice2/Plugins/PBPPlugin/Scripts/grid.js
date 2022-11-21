/* grid.js - v1.2 */

function grid_ShowFilterInfo(model, filters) {

    var sInfo = "";
    var filter = null;

    var cnFilterInfo = $("#" + model + "_FilterInfo");    
    var cnFilterInfoTitle = $("#" + model + "_HdnFilterInfoTitle");

    if (filters != null) {
        for (var i = 0; i < filters.filters.length; i++) {
            filter = filters.filters[i];
            sInfo += grid_ColumnTitle(model, filter.field) + " " + grid_OperatorDesc(model, filter.operator) + " " + filter.value;
            if (i < (filters.filters.length - 1)) sInfo += "\n\r" + filters.logic + "\n\r";
        }
        cnFilterInfo.addClass("filterEnabled");
        cnFilterInfo.removeClass("filterDisabled");
    }
    else {
        sInfo = cnFilterInfoTitle.val();
        cnFilterInfo.removeClass("filterEnabled");
        cnFilterInfo.addClass("filterDisabled");
    }

    cnFilterInfo.prop("title", sInfo);

}

function grid_ColumnTitle(model, field) {
    var grid = $("#grid" + model).data('kendoGrid');
    for (var j = 0; j < grid.columns.length; j++) {
        if (grid.columns[j].field == field)
            return grid.columns[j].title;
    }
    return field;
}

function grid_OperatorDesc(model, operator) {
    var desc = operator;
    try {
        var grid = $("#grid" + model).data('kendoGrid');
        switch (operator) {
            case "eq": desc = "="; break;
            case "neq": desc = "!="; break;
            case "lt": desc = "<"; break;
            case "lte": desc = "<="; break;
            case "gt": desc = ">"; break;
            case "gte": desc = ">="; break;
            case "startswith":
                if (grid.options.filterable.operators.string.startswith != null)
                    desc = grid.options.filterable.operators.string.startswith;
                break;
            case "endswith":
                if (grid.options.filterable.operators.string.endswith != null)
                    desc = grid.options.filterable.operators.string.endswith;
                break;
            case "contains":
                if (grid.options.filterable.operators.string.contains != null)
                    desc = grid.options.filterable.operators.string.contains;
                break;
        }
    }
    catch (ex) {

    }
    return desc;
}

function grid_ClearFilter(model) {
    var grid = $("#grid" + model).data('kendoGrid');
    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        state.filter = null;
        grid.dataSource.query(state);
    }
}

function grid_IsNull(value, nullValue) {
    if (nullValue == null) nullValue = "";
    return ((value == null) ? nullValue : value);
}

function grid_DateTimeFilter(control) {
    $(control).kendoDateTimePicker();
}

var columnMenuInit = false;

function grid_Ready(model, defaultFilter) {

    var grid = $('#grid' + model).data('kendoGrid');

    $(window).resize(function () {
        grid_ResizeGrid(model);
    });

    grid_ResizeGrid(model);

    if (defaultFilter == null)
        grid.dataSource.transport.options.read.url = grid.dataSource.transport.options.read.url.replace("gridInitialized=false", "gridInitialized=true");

    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {

        // Set columns visibility
        for (var iCol = 0; iCol < grid.columns.length; iCol++) {
            if (state.hiddenColumns.indexOf(grid.columns[iCol].field) == -1)
                grid.showColumn(grid.columns[iCol].field);
            else
                grid.hideColumn(grid.columns[iCol].field);
        }

        // Set columns width
        if (state.columnsWidth != null) {
            grid_SetColumnsWidth(model, state.columnsWidth);
        }

        if (state.columnsOrder != null) {
            var column;
            for (var iDest = 0; iDest < state.columnsOrder.length; iDest++) {
                column = null;
                for (var iCol = 0; iCol < grid.columns.length && column == null; iCol++) {
                    if (grid.columns[iCol].field == state.columnsOrder[iDest]) {
                        column = grid.columns[iCol];
                    }
                }
                grid.reorderColumn(iDest, column);
            }
        }

        /*if (state.filter == null && defaultFilter != null) {
            state.filter = defaultFilter;
        }*/

        // Set grid sort, filter, group, page and pagesize
        if (state.filter) {
            grid_ParseFilterDates(state.filter, grid.dataSource.options.schema.model.fields);
        }

        grid.dataSource.query(state);
        //grid.dataSource.filter(state.filter);

    }
    else {
        /*if (defaultFilter != null) {
            state = {
                page: grid.dataSource.page(),
                pageSize: grid.dataSource.pageSize(),
                sort: grid.dataSource.sort(),
                group: grid.dataSource.group(),
                filter: defaultFilter
            };
            grid_ParseFilterDates(state.filter, grid.dataSource.options.schema.model.fields);
            grid.dataSource.query(state);
        }
        else
            grid.dataSource.read();*/
        if (defaultFilter == null) grid.dataSource.read();
    }

    $("#grid" + model)
        .find(".k-header-column-menu")
        .click(function (e) {

            var field = $(this).parent().attr("data-field");

            setTimeout(function () { //delay execution to allow the filtering menu to be created                
                if (columnMenuInit) {                    
                    if ($($(".k-column-menu[style*='display: block;'] ul:first li:last")[0]).attr("role") != "option")
                        $(".k-column-menu[style*='display: block;'] ul:first li:last").remove();                    
                }
                var liItem = '<li class="k-item k-state-default" role="menuitem">' +
                                $(".k-column-menu[style*='display: block;'] .k-columns-item li > span > input[data-field='" + field + "']").parent().parent().html() +
                                '</li>';
                //$(".k-column-menu ul:first").append('<li class="k-item k-state-default k-first" role="menuitem"><span class="k-link"><input type="checkbox" data-field="TypeId" data-index="0">Tipo</span></li>');
                $(".k-column-menu[style*='display: block;'] ul:first").append(liItem);
                var checked = true;
                var state = JSON.parse($.cookie("grid" + model + "State"));
                if (state) {
                    var grid = $('#grid' + model).data('kendoGrid');
                    checked = (state.hiddenColumns.indexOf(field) == -1);
                }
                $(".k-column-menu[style*='display: block;'] ul:first > li:last > span > input").prop("checked", checked);
                $(".k-column-menu[style*='display: block;'] ul:first > li:last").click(function (e) {
                    $(".k-column-menu[style*='display: block;'] .k-columns-item li > span > input[data-field='" + field + "']").click();
                });
                columnMenuInit = true;
            });
        });

}

function grid_onDataBound(model, firstDataBound, e) {

    grid_UpdateExportLinks(model);

    if (firstDataBound == false) {

        var grid = $('#grid' + model).data('kendoGrid'); // this
        var dataSource = grid.dataSource;  //this.dataSource;

        //if (dataSource.filter() != null)
        //    grid.dataSource.transport.options.read.url = grid.dataSource.transport.options.read.url.replace("gridInitialized=false", "gridInitialized=true");

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

        var state = kendo.stringify({
            page: dataSource.page(),
            pageSize: dataSource.pageSize(),
            sort: dataSource.sort(),
            group: dataSource.group(),
            filter: dataSource.filter(),
            hiddenColumns: hiddenColumns,
            columnsWidth: columnsWidth,
            columnsOrder: columnsOrder
        });

        grid_ShowFilterInfo(model, dataSource.filter());

        $.cookie("grid" + model + "State", state);
    }
    else
        firstDataBound = false;

    if (e.sender._data.length == 0) {
        var grid = $('#grid' + model).data('kendoGrid');
        var contentDiv = grid.wrapper.children(".k-grid-content"),
        dataTable = contentDiv.children("table");
        if (!dataTable.find("tr").length) {
            dataTable.children("tbody").append("<tr colspan='" + grid.columns.length + "'><td> </td></tr>");
            try {
                if ($.browser.msie) {
                    dataTable.width(grid.wrapper.children(".k-grid-header").find("table").width());
                    contentDiv.scrollLeft(1);
                }
            }
            catch (ex) {
            }
        }
    }
}

function grid_onColumnHide(model, e) {

    var grid = $('#grid' + model).data('kendoGrid');

    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        var bExist = false;
        for (var iColHidden = 0; iColHidden < state.hiddenColumns.length && !bExist; iColHidden++) {
            if (e.column.field == state.hiddenColumns[iColHidden])
                bExist = true;
        }
        if (!bExist) {
            state.hiddenColumns.push(e.column.field);
            $.cookie("grid" + model + "State", kendo.stringify(state));
            grid_UpdateExportLinks(model);
        }
    }

}

function grid_onColumnShow(model, e) {

    var grid = $('#grid' + model).data('kendoGrid');

    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        var bExist = false;
        for (var iColHidden = 0; iColHidden < state.hiddenColumns.length && !bExist; iColHidden++) {
            if (e.column.field == state.hiddenColumns[iColHidden])
                bExist = true;
        }
        if (bExist) {
            state.hiddenColumns.splice(iColHidden - 1, 1);
            $.cookie("grid" + model + "State", kendo.stringify(state));
            grid_UpdateExportLinks(model);
        }
    }

}

function grid_onColumnResize(model, e) {

    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        var bExist = false;
        for (var iCol = 0; iCol < state.columnsWidth.length && !bExist; iCol++) {
            if (e.column.field == state.columnsWidth[iCol].split(";")[0])
                bExist = true;
        }
        if (bExist) {
            state.columnsWidth[iCol - 1] = e.column.field + ";" + e.newWidth + "px";
        }
        else
            state.columnsWidth.push(e.column.field + ";" + e.newWidth + "px");
        $.cookie("grid" + model + "State", kendo.stringify(state));
    }

}

function grid_onColumnReorder(model, e) {
    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        state.columnsOrder.splice(e.oldIndex, 1);
        state.columnsOrder.splice(e.newIndex, 0, e.column.field);
        $.cookie("grid" + model + "State", kendo.stringify(state));
        grid_UpdateExportLinks(model);
    }
}

function grid_UpdateExportLinks(model) {

    var grid = $('#grid' + model).data('kendoGrid');

    var columns = [];
    for (var iColumn = 0; iColumn < grid.columns.length; iColumn++) {
        if (grid.columns[iColumn].hidden != true) {
            columns.push(grid.columns[iColumn].field);
        }
    }

    // ask the parameterMap to create the request object for you
    var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
    .options.parameterMap({
        page: grid.dataSource.page(),
        sort: grid.dataSource.sort(),
        filter: grid.dataSource.filter(),
        columns: columns
    });

    // Get the export link as jQuery object
    var $exportLink = $('.' + model + '.export');

    // Get its 'href' attribute - the URL where it would navigate to
    var href = $exportLink.attr('href');

    // Update the 'page' parameter with the grid's current page
    href = href.replace(/page=([^&]*)/, 'page=' + requestObject.page || '~');

    // Update the 'sort' parameter with the grid's current sort descriptor
    href = href.replace(/sort=([^&]*)/, 'sort=' + requestObject.sort || '~');

    // Update the 'pageSize' parameter with the grid's current pageSize
    href = href.replace(/pageSize=([^&]*)/, 'pageSize=' + grid.dataSource._pageSize);

    //update filter descriptor with the filters applied
    href = href.replace(/filter=([^&]*)/, 'filter=' + (requestObject.filter || '~'));

    href = href.replace(/columns=([^&]*)/, 'columns=' + (requestObject.columns || '~'));

    // Update the 'href' attribute
    $exportLink.attr('href', href);

    $('.' + model + '.exportPdf').attr('href', href.replace(/xls/, 'pdf'));

}

function grid_ParseFilterDates(filter, fields) {
    if (filter.filters) {
        for (var i = 0; i < filter.filters.length; i++) {
            grid_ParseFilterDates(filter.filters[i], fields);
        }
    }
    else {
        if (fields[filter.field].type == "date") {
            if (filter.isDefaultFilter == true) {
                var now = new Date();
                var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                filter.value = kendo.parseDate(today);
                filter.operator = "gte";
            }
            else
                filter.value = kendo.parseDate(filter.value);
        }
    }
}

function grid_SetColumnsWidth(model, widthColumns) {

    var field, width,
        th, index,
        headerTable = $('#grid' + model + '.k-grid .k-grid-header table'),
        contentTable = $('#grid' + model + '.k-grid .k-grid-content table');


    for (var i = 0; i < widthColumns.length; i++) {
        field = widthColumns[i].split(";")[0];
        width = widthColumns[i].split(";")[1];
        th = $(headerTable).find('th[data-field="' + field + '"]');
        if (th.length) {
            index = $.inArray(th[0], th.parent().children(":visible"));
            col = headerTable.find('colgroup col:eq(' + index + ')')
                .add(contentTable.find('colgroup col:eq(' + index + ')'))
            ;
            col.css('width', width);
        }
    }

}

function grid_Refresh(model) {
    var grid = $("#grid" + model).data('kendoGrid');
    var state = JSON.parse($.cookie("grid" + model + "State"));
    if (state) {
        //state.filter = null;
        grid.dataSource.query(state);
    }
}

function grid_ResizeGrid (model) {
    var gridElement = $("#grid" + model);
    var dataArea = gridElement.find(".k-grid-content");

    var newGridHeight = $(document).height() - 200;
    var newDataAreaHeight = newGridHeight - 65;

    dataArea.height(newDataAreaHeight);
    gridElement.height(newGridHeight);

    $("#grid" + model).data('kendoGrid').refresh();
}

function grid_onRequestStart(model, e) {

    var grid = $('#grid' + model).data('kendoGrid'); // this
    var dataSource = grid.dataSource;  //this.dataSource;

    if (dataSource.filter() != null) {
        grid.dataSource.transport.options.read.url = grid.dataSource.transport.options.read.url.replace("gridInitialized=false", "gridInitialized=true");
    }
    else {
        grid.dataSource.transport.options.read.url = grid.dataSource.transport.options.read.url.replace("gridInitialized=true", "gridInitialized=false");
    }

}