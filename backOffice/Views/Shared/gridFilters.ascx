<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="backOffice.Properties" %>

<div class="gridFilter">
    <div style="display:none;">
        <label class="dateIni-label" for="dateIni"> Resources.Account_Op_Start_Date :</label>
        <%=Html.Kendo().DatePicker()
                .Name("dateIni")
                .Value("")
                .HtmlAttributes(new { style = "width:150px" })
                .Events(e => e.Change("filters_dateIniChange"))
        %>

        <label class="dateEnd-label" for="dateEnd">Resources.Account_Op_End_Date:</label>
        <%=Html.Kendo().DatePicker()
                .Name("dateEnd")
                .Value("")
                .HtmlAttributes(new { style = "width:150px" })
                .Events(e => e.Change("filters_dateEndChange"))
        %>

        <label class="type-label" for="type">Resources.Account_Op_Operation:</label>
        <%=Html.Kendo().DropDownList()
                .Name("types")
                .OptionLabel("All")
                .DataTextField("Text")
                .DataValueField("Value")
                .AutoBind(false)
                .Events(e => e.Change("filters_typesChange"))
                .DataSource(ds =>
                    {
                        ds.Read("Filter_Types", "Kendo");
                    })
        %> 
        <label class="plate-label" for="type">Resources.Account_Op_LicensePlate:</label>
        <%=Html.Kendo().DropDownList()
                .Name("plates")
                .OptionLabel("All")
                .DataTextField("Text")
                .DataValueField("Value")
                .AutoBind(false)
                .Events(e => e.Change("filters_platesChange"))
                .DataSource(ds =>
                    {
                        ds.Read("Filter_Plates", "Kendo");
                    })
        %>
    </div>

    <span id="grid<%: Model%>FilterText"></span>

    <a class="<%: Model%> export imageToolbar exportXls k-icon " 
        href="<%=Url.Action("Export", "Grid", new { /*page = 1, pageSize = "~",*/ filter = "~", sort = "~", model = Model, columns = "~", format = "xls" }) %>" 
        title="<%= Resources.Grid_ExportXls_Title %>" >
    </a> 

    <a class="<%: Model%> export imageToolbar exportPdf k-icon " 
        href="<%=Url.Action("Export", "Grid", new { /*page = 1, pageSize = "~",*/ filter = "~", sort = "~", model = Model, columns = "~", format = "pdf" }) %>" 
        title="<%= Resources.Grid_ExportPdf_Title %>" >
    </a> 
    
    <a id="clearState" class="imageToolbar clear k-icon " 
        href="#" onclick="$.cookie('grid<%: Model%>State', null); location.reload(); return false;"
        title="<%= Resources.Grid_ClearState_Title %>">
    </a> 

    <input id="<%: Model%>_HdnFilterInfoTitle" type="hidden" value="<%= Resources.Grid_FilterInfo_Title %>" />
    <a id="<%: Model%>_FilterInfo" class="imageToolbar filterDisabled k-icon "
        href="#" onclick="grid_ClearFilter('<%: Model%>'); return false;"
        title="">        
    </a>

</div>

    <script>

        var filters_DateIni = null;
        var filters_DateEnd = null;
        var filters_Type = null;
        var filters_Plate = null;
        function filters_dateIniChange() {
            var value = this.value();
            if (value) {
                filters_DateIni = { field: "Date", operator: "gte", value: value };
            } else {
                filters_DateIni = null;
            }
            filters_filterGrid();
        }
        function filters_dateEndChange() {
            var value = this.value();
            if (value) {
                filters_DateEnd = { field: "Date", operator: "lte", value: value };
            } else {
                filters_DateEnd = null;
            }
            filters_filterGrid();
        }
        function filters_typesChange() {
            var value = this.value()
            if (value) {
                //grid.dataSource.filter({ field: "TypeId", operator: "eq", value: parseInt(value) });
                filters_Type = { field: "TypeId", operator: "eq", value: parseInt(value) };
            } else {
                //grid.dataSource.filter({});
                filters_Type = null;
            }
            filters_filterGrid();
        }
        function filters_platesChange() {
            var value = this.value();
            if (value) {
                filters_Plate = { field: "PlateId", operator: "eq", value: parseInt(value) };
            } else {
                filters_Plate = null;
            }
            filters_filterGrid();
        }

        function filters_filterGrid() {            
            var grid = $("#grid<%: Model%>").data("kendoGrid");
            if (grid) {
                var filters = [];
                if (filters_DateIni != null) filters.push(filters_DateIni);
                if (filters_DateEnd != null) filters.push(filters_DateEnd);
                if (filters_Type != null) filters.push(filters_Type);
                if (filters_Plate != null) filters.push(filters_Plate);
                var filter = {};
                if (filters.length > 0) filter = { logic: "and", filters: filters };
                grid.dataSource.filter(filter);
            }
        }

    </script>

    <script>
        /*function ExportXls() {
            var dataSource = $("#Grid").data("kendoGrid").dataSource;
            var filteredDataSource = new kendo.data.DataSource({
                data: dataSource.data(),
                filter: dataSource.filter()
            });

            filteredDataSource.read();
            var data = filteredDataSource.view();

            var result = "data:application/vnd.ms-excel,";

            result += "<table><tr><th>Type</th><th>Installation</th><th>Amount</th><th>Date</th></tr>";

            for (var i = 0; i < data.length; i++) {
                result += "<tr>";

                result += "<td>";
                result += data[i].Type;
                result += "</td>";

                result += "<td>";
                result += data[i].Installation;
                result += "</td>";

                result += "<td>";
                result += data[i].Amount;
                result += "</td>";

                result += "<td>";
                result += kendo.format("{0:MM/dd/yyyy}", data[i].Date);
                result += "</td>";

                result += "</tr>";
            }

            result += "</table>";
            if (window.navigator.msSaveBlob) {
                window.navigator.msSaveBlob(new Blob([result]), 'export.xls');
            } else {
                window.open(result);
            }

        }*/
    </script>

    <script>


    </script>

    <script>

        var <%: Model%>_FirstDataBound = true;

        function <%: Model%>_onDataBound(e) {
            grid_onDataBound("<%: Model%>", <%: Model%>_FirstDataBound, e);
            <%: Model%>_FirstDataBound = false;
        }

        function <%: Model%>_onColumnHide(e) {
            grid_onColumnHide("<%: Model%>", e);            
        }

        function <%: Model%>_onColumnShow(e) {
            grid_onColumnShow("<%: Model%>", e);            
        }

        function <%: Model%>_onColumnResize(e) {
            grid_onColumnResize("<%: Model%>", e);            
        }

        function <%: Model%>_onColumnReorder(e) {
            grid_onColumnReorder("<%: Model%>", e);            
        }

        function <%: Model%>_onRequestStart(e) {
            grid_onRequestStart("<%: Model%>", e);
        }

    </script>



