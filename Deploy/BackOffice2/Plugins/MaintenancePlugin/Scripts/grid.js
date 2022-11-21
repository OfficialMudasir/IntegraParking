function IPSGrid(modelName, modelId, options) {

    var instance = this;

    this.options = options;
    this.events = {};
    if (options != null && options.events != null)
        this.events = options.events;

    this.gridId = 'grid' + modelId;
    this.modelId = modelId;
    this.modelName = modelName;
    this.grid = null;    

    this.cookieStateName = instance.gridId + "State";

    this.firstDataBound = true;
    this.columnMenuInit = false;

    this.currentFilterField = null;

    this.currentProfileId = 0;
    this.currentProfileName = "";
    this.state = null;
    this.originalState = null;
    
    this.filter = null;

    this.inEditMode = 0; //0: none, 1: editing, 2: adding, 3: refreshing, 4,5: deleting

    this.OnSelected = null;

    this.ComboBoxValues = new Hashtable();    

    this.gridHeight = null;

    this.detailEmptyFunc = null;

    this.Ready = function () {

        instance.grid = $('#' + instance.gridId).data('kendoGrid');

        if (instance.grid != null) {

            instance.filter = new IPSFilter(instance,
                                            {
                                                routeConfig: instance.options.routeConfig,
                                                messages: instance.options.messages.filter,
                                                shortDateFormat: instance.options.shortDateFormat,
                                                shortTimeFormat: instance.options.shortTimeFormat,
                                                shortDateTimeFormat: instance.options.shortDateTimeFormat
                                            });

            instance.SetCurrentProfile(options.currentProfile);

            $(document).resize(function () {
                instance.ResizeGrid();
            });
            instance.ResizeGrid();

            $("#" + instance.gridId)
                .find(".k-header-column-menu")
                .click(function (e) {

                    instance.currentFilterField = $(this).parent().attr("data-field");

                    setTimeout(function () { //delay execution to allow the filtering menu to be created                
                        if (instance.columnMenuInit) {
                            $(".k-column-menu[style*='display: block;'] ul:first li:last").remove();
                        }
                        var liItem = '<li class="k-item k-state-default" role="menuitem">' +
                                        $(".k-column-menu[style*='display: block;'] .k-columns-item li > span > input[data-field='" + instance.currentFilterField + "']").parent().parent().html() +
                                        '</li>';
                        //$(".k-column-menu ul:first").append('<li class="k-item k-state-default k-first" role="menuitem"><span class="k-link"><input type="checkbox" data-field="TypeId" data-index="0">Tipo</span></li>');
                        $(".k-column-menu[style*='display: block;'] ul:first").append(liItem);
                        var checked = true;
                        //var state = JSON.parse($.cookie(instance.cookieStateName));                    
                        if (instance.state) {
                            checked = (instance.state.hiddenColumns.indexOf(instance.currentFilterField) == -1);
                        }
                        $(".k-column-menu[style*='display: block;'] ul:first > li:last > span > input").prop("checked", checked);
                        $(".k-column-menu[style*='display: block;'] ul:first > li:last").click(function (e) {
                            $(".k-column-menu[style*='display: block;'] .k-columns-item li > span > input[data-field='" + instance.currentFilterField + "']").click();
                        });
                        instance.columnMenuInit = true;
                    });
                });

            $("#" + instance.gridId).find(".k-pager-info").parent()
                        .append('<div class="grid-load-error"></div>')
            /*.delegate(".approve a", "click", function (e) {
                console.debug(e);
            })*/;

            instance.grid.table.kendoTooltip({
                filter: "td",
                content: function (e) {
                    var target = e.target; // element for which the tooltip is shown
                    return $(target).text();
                },
                show: function (e) {
                    this.popup.element.addClass("grid-tooltip");
                },
                beforeShow: function (e) {
                    if ($(e.target).text() === null || $(e.target).text().trim() === "" || $(e.target).attr("class") =="k-detail-cell") {
                        // don't show the tooltip if the name attribute contains null
                        e.preventDefault();
                    }
                }
            });

        }
    }

    this.LoadState = function () {

        if (instance.grid != null) {

            //var state = JSON.parse($.cookie(instance.cookieStateName));            
            if (instance.state) {

                // Set columns visibility
                for (var iCol = 0; iCol < instance.grid.columns.length; iCol++) {
                    if (instance.state.hiddenColumns.indexOf(instance.grid.columns[iCol].field) == -1)
                        instance.grid.showColumn(instance.grid.columns[iCol].field);
                    else
                        instance.grid.hideColumn(instance.grid.columns[iCol].field);
                }

                // Set columns width
                if (instance.state.columnsWidth != null) {
                    instance.SetColumnsWidth(instance.state.columnsWidth);
                }

                if (instance.state.columnsOrder != null) {
                    var column;
                    for (var iDest = 0; iDest < instance.state.columnsOrder.length; iDest++) {
                        column = null;
                        for (var iCol = 0; iCol < instance.grid.columns.length && column == null; iCol++) {
                            if (instance.grid.columns[iCol].field == instance.state.columnsOrder[iDest]) {
                                column = instance.grid.columns[iCol];
                            }
                        }
                        instance.grid.reorderColumn(iDest, column);
                    }
                }

                instance.state.filter = instance.filter.GetGridFilter();

            }

            instance.grid.dataSource.transport.options.read.url = instance.grid.dataSource.transport.options.read.url.replace("&gridInitialized=false", "&gridInitialized=true");

            instance.grid.dataSource.query(instance.state);

        }

    }

    this.ShowFilterInfo = function (filters) {

        var sInfo = "";
        var filter = null;

        var cnFilterInfo = $("#" + instance.modelId + "_FilterInfo");
        var cnFilterInfoTitle = $("#" + instance.modelId + "_HdnFilterInfoTitle");
        var cnFilterTbr = $(".tbrFilter");

        sInfo = instance.filter.GetAllFiltersInfo(filters);
        if (sInfo != "") {
            cnFilterInfo.addClass("filterEnabled");
            cnFilterInfo.removeClass("filterDisabled");
            cnFilterTbr.addClass("filterEnabled");
            cnFilterTbr.removeClass("filterDisabled");
        }
        else {
            sInfo = cnFilterInfoTitle.val();
            cnFilterInfo.removeClass("filterEnabled");
            cnFilterInfo.addClass("filterDisabled");
            cnFilterTbr.removeClass("filterEnabled");
            cnFilterTbr.addClass("filterDisabled");
        }

        cnFilterInfo.prop("title", sInfo);
        if (cnFilterInfo.parent() != null) cnFilterInfo.parent().prop("title", sInfo);

    }

    this.ColumnTitle = function (field) {
        for (var j = 0; j < instance.grid.columns.length; j++) {
            if (instance.grid.columns[j].field == field)
                return instance.grid.columns[j].title;
        }
        return field;
    }

    this.ClearFilter = function () {
        //var state = JSON.parse($.cookie(instance.cookieStateName));        
        if (instance.state) {
            instance.filter.SetFilterAll(null);
            instance.state.filter = instance.filter.GetGridFilter();            
            //instance.grid.dataSource.query(instance.state);
        }
    }

    this.IsNull = function (value, nullValue) {
        if (nullValue == null) nullValue = "";
        return ((value == null) ? nullValue : value);
    }

    this.Boolean = function (value) {
        return (value == 0 || value == null ? instance.options.messages.boolean.no : instance.options.messages.boolean.yes);
    }

    this.DateTimeFilter = function (control) {
        $(control).kendoDateTimePicker();
    }
    this.DateFilter = function (control) {
        $(control).kendoDatePicker();
    }
    this.TimeFilter = function (control) {
        $(control).kendoTimePicker();
    }

    /*this.ComboBoxFilter = function (control, fkModelName, valueField, textField) {

        $(control).kendoComboBox({
            dataValueField: valueField,
            dataTextField: textField,
            filter: "startswith",
            autoBind: false,
            suggest: true,
            minLength: 1,
            dataSource: {
                serverFiltering: true,
                transport: {
                    serverFiltering: true,
                    read: {
                        type: "GET",
                        url: instance.options.actions.comboBox_read,
                        //dataType: "jsonp",
                        data: {
                            plugin: "MaintenancePlugin",
                            modelName: fkModelName
                        }
                    }
                }
            }
        });
    }*/

    this.FormatCurrency = function (amount, currencyInfo, format) {
        var ret = "";
        if (amount != null) {
            if (format == null) format = '0.00';
            ret = kendo.format('{0:' + format + '}', amount);

            if (currencyInfo != null && currencyInfo != "") {
                var arr = currencyInfo.split("~");
                var pattern = '{0:' + format + '} {1}';
                if (arr.length >= 2) {
                    if (arr[1] == "0")
                        pattern = '{1}{0:' + format + '}';
                    else if (arr[1] == "1")
                        pattern = '{0:' + format + '}{1}';
                    else if (arr[1] == "2")
                        pattern = '{1} {0:' + format + '}';
                    else if (arr[1] == "3")
                        pattern = '{0:' + format + '} {1}';
                }
                //if (arr.length >= 3) amount = amount * parseFloat(arr[2]);
                ret = kendo.format(pattern, amount, currencyInfo.split("~")[0]);
            }
            /*else {
                if (amount != 0)
                    ret = kendo.format('{0:' + format + '}', amount);
            }*/
        }        
        return ret;
    }

    this.FKName = function (fieldName, fieldValue, fieldFKValue) {
        var ret;
        if (fieldFKValue != null && fieldFKValue != undefined && (fieldFKValue != "" || fieldValue == null))
            ret = fieldFKValue;
        else
            eval("ret = instance.FK_" + fieldName + "[" + fieldValue + "]");
        if (ret == null || ret == undefined) ret = "";
        return ret;
    }

    this.OnDataBound = function (e) {

        if (instance.grid == null) instance.grid = $('#' + instance.gridId).data('kendoGrid');

        instance.UpdateExportLinks();

        if (instance.firstDataBound == false) {
            var dataSource = instance.grid.dataSource;  //this.dataSource;

            var hiddenColumns = [];
            for (var iColumn = 0; iColumn < instance.grid.columns.length; iColumn++) {
                if (instance.grid.columns[iColumn].hidden == true) {
                    hiddenColumns.push(instance.grid.columns[iColumn].field);
                }
            }

            var columnsWidth = [];
            for (var iColumn = 0; iColumn < instance.grid.columns.length; iColumn++) {
                if (instance.grid.columns[iColumn].width != undefined && instance.grid.columns[iColumn].width != null) {
                    columnsWidth.push(instance.grid.columns[iColumn].field + ";" + instance.grid.columns[iColumn].width);
                }
            }

            var columnsOrder = [];
            for (var iColumn = 0; iColumn < instance.grid.columns.length; iColumn++) {
                columnsOrder.push(instance.grid.columns[iColumn].field);
            }

            //instance.filter.SetGridFilter(dataSource.filter());

            instance.state = {
                page: 1, //dataSource.page(),
                pageSize: dataSource.pageSize(),
                sort: dataSource.sort(),
                group: dataSource.group(),
                filter: dataSource.filter(),
                filterAll: instance.filter.GetFilterAll(),
                hiddenColumns: hiddenColumns,
                columnsWidth: columnsWidth,
                columnsOrder: columnsOrder
            };

            if (instance.originalState == null) {
                instance.originalState = JSON.parse(kendo.stringify(instance.state));
            }
            else {
                //if (kendo.stringify(instance.originalState) != kendo.stringify(instance.state))
                if (instance.ProfileChanged())
                    app_setBeforeUnload(true);
            }

            //var stringState = kendo.stringify(instance.state);

            instance.ShowFilterInfo(dataSource.filter());

            //$.cookie(instance.cookieStateName, stringState);            

        }
        else {

            instance.firstDataBound = false;            

        }

        if (e.sender._data.length == 0) {            
            var contentDiv = instance.grid.wrapper.children(".k-grid-content"),
            dataTable = contentDiv.children("table");
            if (!dataTable.find("tr").length) {
                dataTable.children("tbody").append("<tr colspan='" + instance.grid.columns.length + "'><td> </td></tr>");
                try {
                    if ($.browser.msie) {
                        dataTable.width(instance.grid.wrapper.children(".k-grid-header").find("table").width());
                        contentDiv.scrollLeft(1);
                    }
                }
                catch (ex) {
                }
            }
        }

        $(".k-grid-delete").off("click");
        $(".k-grid-delete").on("click", function (e) {
            $(this).parent().parent().addClass("deleteTarget_" + instance.gridId);
            msgboxConfirm(instance.options.messages.delete.title, instance.options.messages.delete.confirm, "ipsGrid" + instance.modelId + ".DeleteConfirm();");
            e.preventDefault();
            e.stopPropagation();
        });

        $(".k-grid-add").off("click");
        $(".k-grid-add").on("click", function (e) {
            var Access = 0;
            //$('#' + instance.gridId).data("action", "C");
            //instance.grid.dataSource.transport.options.read.url
        });

        $("#" + instance.gridId).find(".grid-load-error").html("");

        if (instance.detailEmptyFunc != null) {
            this.element.find('tr.k-master-row').each(function () {
                var row = $(this);
                var data = instance.grid.dataSource.getByUid(row.data('uid'));
                // this example will work if ReportId is null or 0 (if the row has no details)
                if (instance.detailEmptyFunc(data)) {
                    row.find('.k-hierarchy-cell a').css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
                }
            });
        }

        try {
            instance.events.onDataBound.call(e);
        }
        catch (ex) {
            //alert(ex.message);
        }
    }

    this.OnColumnHide = function (e) {

        //var state = JSON.parse($.cookie(instance.cookieStateName));        
        if (instance.state) {
            var bExist = false;
            for (var iColHidden = 0; iColHidden < instance.state.hiddenColumns.length && !bExist; iColHidden++) {
                if (e.column.field == instance.state.hiddenColumns[iColHidden])
                    bExist = true;
            }
            if (!bExist) {
                instance.state.hiddenColumns.push(e.column.field);
                //$.cookie(instance.cookieStateName, kendo.stringify(state));                
                instance.UpdateExportLinks();

                if (instance.ProfileChanged())
                    app_setBeforeUnload(true);

            }
        }

    }

    this.OnColumnShow = function (e) {

        //var state = JSON.parse($.cookie(instance.cookieStateName));        
        if (instance.state) {
            var bExist = false;
            for (var iColHidden = 0; iColHidden < instance.state.hiddenColumns.length && !bExist; iColHidden++) {
                if (e.column.field == instance.state.hiddenColumns[iColHidden])
                    bExist = true;
            }
            if (bExist) {
                instance.state.hiddenColumns.splice(iColHidden - 1, 1);
                //$.cookie(instance.cookieStateName, kendo.stringify(state));                                
                instance.UpdateExportLinks();

                if (instance.ProfileChanged())
                    app_setBeforeUnload(true);

            }
        }

    }

    this.OnColumnResize = function (e) {

        //var state = JSON.parse($.cookie(instance.cookieStateName));        
        if (instance.state) {
            var bExist = false;
            for (var iCol = 0; iCol < instance.state.columnsWidth.length && !bExist; iCol++) {
                if (e.column.field == instance.state.columnsWidth[iCol].split(";")[0])
                    bExist = true;
            }
            if (bExist) {
                instance.state.columnsWidth[iCol - 1] = e.column.field + ";" + e.newWidth + "px";
            }
            else
                instance.state.columnsWidth.push(e.column.field + ";" + e.newWidth + "px");
            //$.cookie(instance.cookieStateName, kendo.stringify(state));            
            if (instance.ProfileChanged())
                app_setBeforeUnload(true);
        }

    }

    this.OnColumnReorder = function (e) {
        //var state = JSON.parse($.cookie(instance.cookieStateName));        
        if (instance.state) {
            instance.state.columnsOrder.splice(e.oldIndex, 1);
            instance.state.columnsOrder.splice(e.newIndex, 0, e.column.field);
            //$.cookie(instance.cookieStateName, kendo.stringify(state));            
            instance.UpdateExportLinks();
            if (instance.ProfileChanged())
                app_setBeforeUnload(true);
        }
    }

    this.OnFilterMenuInit = function (e) {
        this.currentFilterField = e.field;
    }

    /*this.OnEdit = function (e) {
        var g = e.sender;        
        
        $(".customCurrency").each(function (index) {
            var cn = $(this).data("kendoNumericTextBox");
            if (cn) {
                var currencyInfo = e.model[$(this).attr("curMapping")]; //g._data[0][$(this).attr("curMapping")];
                var format = $(this).attr("curFormat");
                //cn.setOptions({ format: $(this).attr("curFormat").replace("CUR", curSimbol) });
                var _options = cn.options;
                var _value = $(this).val();
                if (currencyInfo != null && currencyInfo != "") {
                    if (currencyInfo.split("~")[1] == "0")
                        format = 'CUR' + format;
                    else if (currencyInfo.split("~")[1] == "1")
                        format = format + 'CUR';
                    else if (currencyInfo.split("~")[1] == "2")
                        format = 'CUR ' + format;
                    else if (currencyInfo.split("~")[1] == "3")
                        format = format + ' CUR';
                    _options.format = format.replace("CUR", currencyInfo.split("~")[0]);
                }
                cn.destroy();                
                $(this).kendoNumericTextBox(_options);
                $(this).val(_value);                
            }
        });

    }*/

    this.OnEdit = function (e) {

        instance.inEditMode = (e.model.isNew() ? 2 : 1);

        $(".customCurrency").each(function (index) {
            var cn = $(this).data("kendoNumericTextBox");
            if (cn) {
                var currencyInfo = e.model[$(this).attr("curMapping")]; //g._data[0][$(this).attr("curMapping")];
                var format = $(this).attr("curFormat");
                //cn.setOptions({ format: $(this).attr("curFormat").replace("CUR", curSimbol) });
                var _options = cn.options;
                var _value = $(this).val();
                if (currencyInfo != null && currencyInfo != "") {
                    if (currencyInfo.split("~")[1] == "0")
                        format = 'CUR' + format;
                    else if (currencyInfo.split("~")[1] == "1")
                        format = format + 'CUR';
                    else if (currencyInfo.split("~")[1] == "2")
                        format = 'CUR ' + format;
                    else if (currencyInfo.split("~")[1] == "3")
                        format = format + ' CUR';
                    _options.format = format.replace("CUR", currencyInfo.split("~")[0]);
                }
                cn.destroy();
                $(this).kendoNumericTextBox(_options);
                $(this).val(_value);
            }
        });
                
        
        var actionType = (e.model.isNew() ? "Create" : "Modify");
        $(e.container).find("input[data-role=combobox]").each(function () {

            var cmb = $(this).data("kendoComboBox");

            try {
                var fieldFK = $(this).attr("name");
                var fkId = 0;
                var fkDesc = "";
                eval("fkId = e.model." + fieldFK);
                eval("fkDesc = e.model." + fieldFK + "_FK");
                if (fkId == 0 && fkDesc == "") {
                    eval("e.model." + fieldFK + " = null");
                    if (instance.options.installationFilter == "") cmb.dataSource.read();
                }
            }
            catch (ex) {
            }
            
            if (instance.options.installationFilter != "") {
                var urlRead = cmb.dataSource.transport.options.read.url;
                urlRead += "&actionType=" + actionType;
                urlRead += "&installationId=" + (e.model.INS_ID != null ? e.model.INS_ID : 0);
                //urlRead = urlRead.replace("&actionType=Modify", "&actionType=" + actionType);
                cmb.dataSource.transport.options.read.url = urlRead;
                cmb.dataSource.read();
            }

        });

        instance.ComboBoxValues = new Hashtable();

    }

    this.OnCancel = function (e) {
        instance.inEditMode = 0;
        
        setTimeout(function () {
            $(".k-grid-delete").off("click");
            $(".k-grid-delete").on("click", function (e) {
                $(this).parent().parent().addClass("deleteTarget_" + instance.gridId);
                msgboxConfirm(instance.options.messages.delete.title, instance.options.messages.delete.confirm, "ipsGrid" + instance.modelId + ".DeleteConfirm();");
                e.preventDefault();
                e.stopPropagation();
            });
        }, 500);
    }

    this.OnSaveChanges = function (e) {
        var g = e.sender;        
    }

    this.OnSave = function (e) {
        
        var key = "";
        for (var i = 0; i < instance.ComboBoxValues.keys().length; i++) {
            key = instance.ComboBoxValues.keys()[i];
            try {
                eval("e.model." + key + "=" + instance.ComboBoxValues.get(key));
                e.model.dirty = true;
            } catch (ex) { }
        }

    }

    this.OnChange = function (e) {

        /*var selected = $.map(this.select(), function (item) {
            return $(item).text();
        });
        console.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");*/

        var selectedId = instance.grid.dataItem(instance.grid.select()).id;
        console.log("Selected: " + selectedId);

        if (instance.OnSelected != null) {
            instance.OnSelected(instance.grid.dataItem(instance.grid.select()));
        }
        
    }

    this.OnError = function (args) {

        //instance.inEditMode = 0;
        if (args.errors) {
            var message = "Errors:<br>";
            $.each(args.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "<br>";
                    });
                }
            });
            var validationTemplate = kendo.template($("#" + instance.modelId + "_ValidationMessageTemplate").html());

            //msgboxError(instance.options.messages.name, message);

            //instance.grid.dataSource.cancelChanges();

            $("#" + instance.gridId).data().kendoGrid.one('dataBinding', function (e) {
                e.preventDefault();

                $.each(args.errors, function (propertyName) {
                    var renderedTemplate = validationTemplate({ field: propertyName, messages: this.errors });
                    if (instance.grid.editable != null)
                        instance.grid.editable.element.find(".edit-form-errors").append(renderedTemplate);
                    else {
                        try {
                            instance.grid.content.parent().parent().find(".edit-form-errors").append(renderedTemplate);
                        } catch (ex) { }
                    }
                });
            });

            //$(".k-pager-refresh.k-link").click();
        }
        else {
            //$("#" + instance.gridId + ".grid-load-error")
            $("#" + instance.gridId).find(".grid-load-error").html(instance.options.messages.error.loadingdata);
        }

    }

    this.OnRequestStart = function (e) {
        if (instance.inEditMode == 1 || instance.inEditMode == 2)
            kendo.ui.progress($(".k-window"), true);
        else if (instance.inEditMode == 3 || instance.inEditMode == 4 || instance.inEditMode == 5)
            kendo.ui.progress($("#" + instance.gridId), true);
    }

    this.OnRequestEnd = function (e) {
        if (instance.inEditMode == 1 || instance.inEditMode == 2) {
            kendo.ui.progress($(".k-window"), false);
            //instance.inEditMode = 0;
            if (e.response == null || e.response.Errors == null) {
                // Refresh grid data
                instance.inEditMode = 3;
                instance.grid.dataSource.read();
                instance.grid.refresh();
            }
        }
        else if (instance.inEditMode == 3) {
            instance.inEditMode = 0;
            kendo.ui.progress($("#" + instance.gridId), false);
        }
        else if (instance.inEditMode == 4) {
            instance.inEditMode = 0;
            kendo.ui.progress($("#" + instance.gridId), false);
            if (e.response.Errors != null) {
                // Refresh grid data
                instance.inEditMode = 3;
                instance.grid.dataSource.read();
                instance.grid.refresh();
            }
        }
        else if (instance.inEditMode == 5) {
            instance.inEditMode = 4;
            kendo.ui.progress($("#" + instance.gridId), false);
        }

    }

    this.OnComboBoxChange = function (e) {
        if (this.value() && this.selectedIndex == -1) {
            /*if (this.dataSource._data.length > 0) {
                var dt = this.dataSource._data[0];
                this.value(dt[this.options.dataValueField]);
                this.text(dt[this.options.dataTextField]);
            }
            else {*/
                this.value(null);
                this.text("");
            //}
        }        
        instance.ComboBoxValues.put(this.input.context.name, this.value());
    }

    this.OnComboBoxDataBound = function (e) {
        if (this.value() == 0 && this.selectedIndex == -1) {
            this.text("");
        }
    }

    this.UpdateExportLinks = function () {

        // Get the export link as jQuery object
        var $exportLink = $('.' + instance.modelId + '.export');
        if ($exportLink.length > 0) {

            var columns = [];
            for (var iColumn = 0; iColumn < instance.grid.columns.length; iColumn++) {
                if (instance.grid.columns[iColumn].hidden != true) {
                    columns.push(instance.grid.columns[iColumn].field);
                }
            }

            // ask the parameterMap to create the request object for you
            var requestObject = (new kendo.data.transports["aspnetmvc-server"]({ prefix: "" }))
            .options.parameterMap({
                page: instance.grid.dataSource.page(),
                sort: instance.grid.dataSource.sort(),
                filter: instance.grid.dataSource.filter()/*,
            columns: columns*/
            });
            requestObject.columns = columns;

            // Get its 'href' attribute - the URL where it would navigate to
            var href = $exportLink.attr('href');

            // Update the 'page' parameter with the grid's current page
            href = href.replace(/page=([^&]*)/, 'page=' + requestObject.page || '~');

            // Update the 'sort' parameter with the grid's current sort descriptor
            href = href.replace(/sort=([^&]*)/, 'sort=' + requestObject.sort || '~');

            // Update the 'pageSize' parameter with the grid's current pageSize
            href = href.replace(/pageSize=([^&]*)/, 'pageSize=' + instance.grid.dataSource._pageSize);

            //update filter descriptor with the filters applied
            href = href.replace(/filter=([^&]*)/, 'filter=' + (requestObject.filter || '~'));

            href = href.replace(/columns=([^&]*)/, 'columns=' + (requestObject.columns || '~'));

            // Update the 'href' attribute
            $exportLink.attr('href', href);

            $('.' + instance.modelId + '.exportPdf').attr('href', href.replace(/xls/, 'pdf'));

        }
    }

    this.SetColumnsWidth = function (widthColumns) {

        var field, width,
            th, index,
            headerTable = $('#' + instance.gridId + '.k-grid .k-grid-header table'),
            contentTable = $('#' + instance.gridId + '.k-grid .k-grid-content table');


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

    this.SetCurrentProfile = function (profile, load) {

        var btnSaveProfile = $("#btn" + instance.modelId + "_SaveProfile").data("kendoButton");

        if (profile != null) {

            //$("#btn" + instance.modelId + "_SaveProfile").removeAttr('disabled')            
            if (btnSaveProfile != null) btnSaveProfile.enable(true);

            if (load == null) load = true;

            instance.currentProfileId = profile.Id;
            instance.currentProfileName = profile.Name;
            if (profile.Config != null && profile.Config != "") {
                var oProfileConfig = JSON.parse(profile.Config);
                instance.filter.ConvertFiltersToCurrentCulture(oProfileConfig.filterAll);
                instance.state = oProfileConfig;
                var sProfileConfig = kendo.stringify(oProfileConfig);
                instance.originalState = JSON.parse(sProfileConfig);
            }
            else {
                instance.state = null;
                instance.originalState = null;
            }

            if (instance.state == null) {
                instance.state = {
                    page: 1,
                    pageSize: instance.grid.dataSource.pageSize(),
                    sort: instance.grid.dataSource.sort(),
                    group: null,
                    filter: null,
                    filterAll: null,
                    hiddenColumns: [],
                    columnsWidth: [],
                    columnsOrder: []                    
                };                
            }
            
            instance.filter.Load(instance.state);

            app_setBeforeUnload(false);

            if (load == true) instance.LoadState();
        }
        else            
            if (btnSaveProfile != null) btnSaveProfile.enable(false);

    }

    this.LoadProfile = function (profileId, callback) {

        /*var sOriginalState = "";
        var sState = "";
        if (instance.originalState != null) {
            sOriginalState = kendo.stringify(instance.originalState);
            var tmpOriginalState = JSON.parse(sOriginalState);
            tmpOriginalState.filter = null;
            sOriginalState = kendo.stringify(tmpOriginalState);
        }
        if (instance.state != null) {
            sState = kendo.stringify(instance.state);
            var tmpState = JSON.parse(sState);
            tmpState.filter = null;
            sState = kendo.stringify(tmpState);
        }*/

        //if (sOriginalState != sState && sOriginalState != "") {
        if (instance.ProfileChanged()) {
            msgboxConfirm(instance.options.messages.profile.load.title, instance.options.messages.profile.load.lostChanges, instance.options.varname + ".LoadProfileConfirm(" + profileId + ", '" + callback + "');");
        }
        else
            instance.LoadProfileConfirm(profileId, callback);
    }

    this.LoadProfileConfirm = function (profileId, callback) {

        if (profileId == null) profileId = instance.currentProfileId;
        
        $.ajax({
            type: 'POST',
            url: instance.options.actions.loadProfile,
            data: { plugin: 'MaintenancePlugin', modelName: instance.modelName, profileId: profileId },
            success: function (response) {

                try {
                    //var oResponse = JSON.parse(response);
                    //eval("oReponse = " + response);

                    if (response.Result == true) {
                        
                        if (response.Profile != null) {
                            instance.SetCurrentProfile(response.Profile);
                            if (callback != null && callback != "")
                                eval(callback);
                        }
                        else {                            
                            msgboxAlert(instance.options.messages.profile.load.title, instance.options.messages.profile.load.error.invalid, "warning");
                        }
                    }
                    else {                        
                        msgboxAlert(instance.options.messages.profile.load.title, response.ErrorInfo, "warning");
                    }

                } catch (ex) {
                    msgboxError(instance.options.messages.profile.load.title, instance.options.messages.error.exception);
                }
            },
            error: function (xhr) {
                msgboxError(instance.options.messages.profile.load.title, instance.options.messages.error.communication);
            }
        });

    }

    this.AddProfile = function (profileName, callback) {

        if (profileName.trim() != "") {

            var oStateCopy = JSON.parse(kendo.stringify(instance.state));
            instance.filter.ConvertFiltersToNoCulture(oStateCopy.filterAll);

            $.ajax({
                type: 'POST',
                url: instance.options.actions.addProfile,
                data: { plugin: 'MaintenancePlugin', modelName: instance.modelName, profileName: profileName, profileConfig: kendo.stringify(oStateCopy) },
                success: function (response) {

                    try {
                        //var oResponse = JSON.parse(response);
                        //eval("oReponse = " + response);

                        if (response.Result == true) {
                            instance.SetCurrentProfile(response.Profile);
                        }
                        else {
                            msgboxAlert(instance.options.messages.profile.add.title, response.ErrorInfo, "warning");
                        }

                        if (callback != null && callback != "")
                            eval(callback);


                    } catch (ex) {
                        msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.exception);
                    }
                },
                error: function (xhr) {
                    msgboxError(instance.options.messages.profile.add.title, instance.options.messages.error.communication);
                }
            });

        }
        else
            msgboxAlert(instance.options.messages.profile.add.title, instance.options.messages.profile.add.error.invalidName, "warning");

    }

    this.UpdateStateProfile = function (profileId, callback) {
        instance.state.filterAll = instance.filter.GetFilterAll();
        var oStateCopy = JSON.parse(kendo.stringify(instance.state));
        instance.filter.ConvertFiltersToNoCulture(oStateCopy.filterAll);
        instance.UpdateProfile(profileId, "Config", kendo.stringify(oStateCopy), callback);
    }

    this.UpdateDefaultProfile = function (profileId, value, callback) {
        instance.UpdateProfile(profileId, "Default", value, callback);
    }

    this.UpdatePublicProfile = function (profileId, value, callback) {
        instance.UpdateProfile(profileId, "Public", value, callback);
    }

    this.UpdateProfile = function (profileId, param, value, callback) {

        if (profileId == null) profileId = instance.currentProfileId;

        $.ajax({
            type: 'POST',
            url: instance.options.actions.updateProfile,
            data: { plugin: 'MaintenancePlugin', modelName: instance.modelName, profileId: profileId, profileParam: param, profileValue: value },
            success: function (response) {

                try {
                    //var oResponse = JSON.parse(response);
                    //eval("oReponse = " + response);

                    /*if (response.Profiles != null) {
                        instance.profiles = response.Profiles;
                        var sCurName = instance.currentProfile.Name;
                        instance.currentProfile = null;
                        $.each(instance.profiles, function (i, item) {
                            if (instance.profiles[i].Name == sCurName)
                                instance.currentProfile = instance.profiles[i];
                        });
                        if (instance.currentProfile == null) {
                            $.each(instance.profiles, function (i, item) {
                                if (instance.profiles[i].Default == true)
                                    instance.currentProfile = instance.profiles[i];
                            });
                        }
                        
                    }*/

                    if (response.Result == true) {
                        instance.SetCurrentProfile(response.Profile, false);
                    }
                    else {
                        msgboxAlert(instance.options.messages.profile.update.title, response.ErrorInfo, "warning");
                    }

                    if (callback != null && callback != "")
                        eval(callback);

                } catch (ex) {
                    msgboxError(instance.options.messages.profile.update.title, "Error");
                }
            },
            error: function (xhr) {
                msgboxError(instance.options.messages.profile.update.title, "Error");
            }
        });

    }

    this.DeleteProfile = function (profileId, callback) {

        msgboxConfirm(instance.options.messages.profile.delete.title, instance.options.messages.profile.delete.confirm, instance.options.varname + ".DeleteProfileConfirm(" + profileId + ", '" + callback + "');");
        
    }

    this.DeleteProfileConfirm = function (profileId, callback) {

        $.ajax({
            type: 'POST',
            url: instance.options.actions.deleteProfile,
            data: { plugin: 'MaintenancePlugin', modelName: instance.modelName, profileId: profileId },
            success: function (response) {

                try {
                    //var oResponse = JSON.parse(response);
                    //eval("oReponse = " + response);

                    if (response.Result == true) {
                        if (instance.currentProfileId == profileId) {
                            instance.SetCurrentProfile(response.Profile);
                        }
                        if (callback != null && callback != "")
                            eval(callback);
                    }
                    else {
                        msgboxAlert(instance.options.messages.profile.delete.title, response.ErrorInfo, "warning");
                    }

                } catch (ex) {                    
                    msgboxError(instance.options.messages.profile.delete.title, instance.options.messages.error.exception);
                }
            },
            error: function (xhr) {                
                msgboxError(instance.options.messages.profile.delete.title, instance.options.messages.error.communication);
            }
        });

    }

    this.DeleteConfirm = function () {

        instance.inEditMode = 5;
        kendo.ui.progress($("#" + instance.gridId), true);

        var deleteTarget = $(".deleteTarget_" + instance.gridId);        
        instance.grid.removeRow(deleteTarget.closest("tr"));
        //instance.grid.dataSource.sync();
        //var dataItem = instance.grid.dataItem(deleteTarget.closest("tr"));
        //instance.grid.dataSource.remove(dataItem);
        //instance.grid.dataSource.sync();
        deleteTarget.removeClass("deleteTarget_" + instance.gridId);

        /*
        //Selecting Grid
        var gview = $("#grid").data("kendoGrid");
        //Getting selected row
        var dataItem = gview.dataItem(gview.select());
        //Removing Selected row
        gview.dataSource.remove(dataItem);
        //Removing row using index number
        gview.dataSource.remove(0);// 0 is row index
        */
    }

    this.ResizeGrid = function (newHeight) {
        var gridElement = $("#" + instance.gridId);
        var dataArea = gridElement.find(".k-grid-content");
        
        if (newHeight != null) {
            instance.gridHeight = newHeight;
        }

        var newGridHeight = instance.gridHeight;
        if (newGridHeight == null) {
            newGridHeight = $(document).height() - gridElement.offset().top - 10; // - 225;
        }

        var newDataAreaHeight = newGridHeight - 65;

        dataArea.height(newDataAreaHeight);
        gridElement.height(newGridHeight);

        if (instance.grid != null) instance.grid.refresh();
    }

    this.Refresh = function () {
        if (instance.grid != null) {
            instance.grid.dataSource.read();
        }
    }

    this.ProfileChanged = function () {
        if (instance.options.profilesActive) {
            var sOriginalState = "";
            var sState = "";

            if (instance.originalState != null) {
                sOriginalState = kendo.stringify(instance.originalState);
                var tmpOriginalState = JSON.parse(sOriginalState);
                tmpOriginalState.filter = null;
                if (tmpOriginalState.filterAll != null) {
                    tmpOriginalState.filterAll = instance.filter.CleanFiltersInternal(tmpOriginalState.filterAll);
                    instance.filter.CleanFiltersValues(tmpOriginalState.filterAll);
                }

                sOriginalState = "";
                if (tmpOriginalState.columnsOrder != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.columnsOrder);
                if (tmpOriginalState.columnsWidth != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.columnsWidth);
                if (tmpOriginalState.group != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.group);
                if (tmpOriginalState.hiddenColumns != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.hiddenColumns);
                if (tmpOriginalState.sort != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.sort);
                if (tmpOriginalState.filterAll != null) sOriginalState = sOriginalState + kendo.stringify(tmpOriginalState.filterAll);

                //sOriginalState = kendo.stringify(tmpOriginalState);
                //sOriginalState = sOriginalState.replace(",\"filter\":null", "");
            }
            if (instance.state != null) {
                sState = kendo.stringify(instance.state);
                var tmpState = JSON.parse(sState);
                tmpState.filter = null;
                if (tmpState.filterAll != null) {
                    tmpState.filterAll = instance.filter.CleanFiltersInternal(tmpState.filterAll);
                    instance.filter.CleanFiltersValues(tmpState.filterAll);
                }

                sState = "";
                if (tmpState.columnsOrder != null) sState = sState + kendo.stringify(tmpState.columnsOrder);
                if (tmpState.columnsWidth != null) sState = sState + kendo.stringify(tmpState.columnsWidth);
                if (tmpState.group != null) sState = sState + kendo.stringify(tmpState.group);
                if (tmpState.hiddenColumns != null) sState = sState + kendo.stringify(tmpState.hiddenColumns);
                if (tmpState.sort != null) sState = sState + kendo.stringify(tmpState.sort);
                if (tmpState.filterAll != null) sState = sState + kendo.stringify(tmpState.filterAll);

                //sState = kendo.stringify(tmpState);
                //sState = sState.replace(",\"filter\":null", "");
            }

            if (instance.options.mode == "advanced") {
                return (sOriginalState != sState && sOriginalState != "");
            }
            else {
                return false;
            }
        }
        else
            return false;
    };

    this.CheckProfileChanges = function (callback) {
        
        if (instance.ProfileChanged()) {
            msgboxConfirm(instance.options.messages.profile.load.title, instance.options.messages.profile.load.lostChanges, callback);
        }
        else
            eval(callback);

    };

}

kendo.ui.Tooltip.fn._show = function (show) {
    return function (target) {
        var e = {
            sender: this,
            target: target,
            preventDefault: function () {
                this.isDefaultPrevented = true;
            }
        };

        if (typeof this.options.beforeShow === "function") {
            this.options.beforeShow.call(this, e);
        }
        if (!e.isDefaultPrevented) {
            // only show the tooltip if preventDefault() wasn't called..
            show.call(this, target);
        }
    };
}(kendo.ui.Tooltip.fn._show);