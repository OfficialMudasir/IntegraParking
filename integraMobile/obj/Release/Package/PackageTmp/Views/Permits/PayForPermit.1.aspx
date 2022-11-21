<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master" Inherits="System.Web.Mvc.ViewPage<PermitsModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="integraMobile.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>    
    <%= resBundle.GetString("Permits_PayForPermit") %>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Main_BttnPermits%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="<%= Url.Content("~/Content/kendo/kendo.common.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.default.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />    
    <link href="<%= Url.Content("~/Content/kendoExt/kendo.ext.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.dataviz.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.dataviz.default.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.custom.css?v3.1") %>" rel="stylesheet" type="text/css" />

    <script src="<%= Url.Content("~/Scripts/kendo/jquery.min.js?v2016.3.1118") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendo/kendo.all.min.js?v2016.3.1118") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendo/kendo.timezones.min.js?v2016.3.1118") %>"></script>    
    <script src="<%= Url.Content("~/Scripts/kendo/kendo.aspnetmvc.min.js?v2016.3.1118") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendo/jszip.min.js?v1.0") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendoExt/kendo.web.ext.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendo/kendo.iparkme.js?v3.0") %>"></script>

    <%
        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
    %>
    <script src="<%= Url.Content("~/Scripts/kendo/cultures/kendo.culture." + culture + ".min.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/kendo/messages/kendo.messages." + culture + ".min.js") %>"></script>
    <%-- Set the current culture --%>
    <script>
        kendo.culture("<%= culture %>");
    </script>

    <link href="<%= Url.Content("~/Content/Permits.css?rnd=5") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            if (top.location != this.location) {
                $("html, body").css("display", "none");
                top.location = this.location;
            }
            else {
                $("html, body").css("display", "block");
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>  
<% 
    using (Html.BeginForm("PayForPermit", "Permits", FormMethod.Post, new { onsubmit = "return validateForm()", autocomplete = "off" }))
    { 
        %>
        <div id="loader"></div>
        <div class="content-wrap">
            <div class="row">
                <div id="permits-steps" class="col-lg-8 col-lg-offset-2 col-md-12 col-block">      
                    <% 
                        if (Model.FromLogin == true) 
                        { 
                            %>
                            <%--
                            <div class="steps">
                                <div>
                                    <div class="bubble" id="bubble1"><img src="<%= Url.Content("~/Content/img/Permits/1-blue.png")%>" /></div>
                                    <div class="p-arrow p-arrow-light" id="arrow1"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                                    <div class="bubble bubble-little" id="bubble2"><img src="<%= Url.Content("~/Content/img/Permits/2-gray.png")%>" /></div>
                                </div>
                                <div>
                                    <div class="bubble-label label-blue"><%= resBundle.GetString("Permits_PayForPermit")%></div>
                                    <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_Summary")%></div>
                                </div>
                            </div><!--//.steps-->
                            --%>
                            <div class="steps">
                                <div class="step current">
                                    <span class="number">1. &nbsp; </span>
                                    <span class="step-desc"><%= resBundle.GetString("Permits_PayForPermit")%></span>
                                </div>
                                <div class="step disabled">
                                    <span class="number">2. &nbsp; </span>
                                    <span class="step-desc"><%= resBundle.GetString("Permits_Summary")%></span>
                                </div>
                            </div><!--//.steps-->
                            <% 
                        }
                        else
                        {
                            %>
                            <%-- // else old 
                            <div class="decoration-little-little">
                                <div>
                                    <div class="bubble" id="bubble1"><img src="<%= Url.Content("~/Content/img/Permits/1-blue.png")%>" /></div>
                                    <div class="p-arrow p-arrow-light" id="arrow1"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                                    <div class="bubble bubble-little" id="bubble2"><img src="<%= Url.Content("~/Content/img/Permits/2-gray.png")%>" /></div>
                                    <div class="p-arrow p-arrow-light" id="arrow2"><img src="<%= Url.Content("~/Content/img/Permits/arrow.png")%>" /></div>
                                    <div class="bubble bubble-little" id="bubble3"><img src="<%= Url.Content("~/Content/img/Permits/3-gray.png")%>" /></div>
                                </div>
                                <div>
                                    <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_PaymentMethod")%></div>
                                    <div class="bubble-label label-blue"><%= resBundle.GetString("Permits_PayForPermit")%></div>
                                    <div class="bubble-label label-gray"><%= resBundle.GetString("Permits_Summary")%></div>
                                </div>
                            </div>
                            // else old --%>
                            <div class="steps">
                                <div class="step current">
                                    <span class="number">1. &nbsp; </span>
                                    <span class="step-desc"><%= resBundle.GetString("Permits_PaymentMethod")%></span>
                                </div>
                                <div class="step disabled">
                                    <span class="number">2. &nbsp; </span>
                                    <span class="step-desc"><%= resBundle.GetString("Permits_PayForPermit")%></span>
                                </div>
                                <div class="step disabled">
                                    <span class="number">3. &nbsp; </span>
                                    <span class="step-desc"><%= resBundle.GetString("Permits_Summary")%></span>
                                </div>
                            </div><!--//.steps-->                        
                            <% 
                        } // if else Model.FromLogin
                    %>
                </div><!--//#permits-steps-->
            </div><!--//.row-->
            <div class="row">
                <div id="permits-form" class="col-lg-8 col-lg-offset-2 col-md-12 col-block">      

                    <h3 class="content-header">
                        <span>
                            <% 
                                if (Model.FromLogin == false) 
                                {
                                    %>2<% 
                                } else { 
                                    %>1<% 
                                } 
                            %>.
                        </span> <%= resBundle.GetString("Permits_PayForPermit")%>
                    </h3><!--//.content-header-->

                    <div class="alert alert-bky-danger notice"<% if (string.IsNullOrEmpty(Model.Error)) { %> style="display:none;"<% } %>>

                        <p style="display:inline-block;"><span class="bky-cancel"></span></p>
                        <p style="display:inline-block;"><%= Model.Error%></p>
                            <% Model.Error = string.Empty; %>
                    </div><!--//.notice.alert-->

                    <div class="form-group">
                        <label for="PermitsDataModel_City"><% =resBundle.GetString("PermitsDataModel_City") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("City")
                            .HtmlAttributes(new { id ="PermitsDataModel_City" })
                            .DataTextField("Name")
                            .DataValueField("id")
                            .Filter("contains")
                            .OptionLabel(resBundle.GetString("Permits_SelectCity"))
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetCities", "Permits");
                                })
                                .ServerFiltering(false)
                                .Events(e => { e.Error("Generic_Error"); });
                            })
                            .Events(e => { e.DataBound("City_Databound").Change("City_Changed"); })
                        %>
                    </div>
                    <div class="form-group">
                        <label><% =resBundle.GetString("PermitsDataModel_Zone") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("Zone")
                            .HtmlAttributes(new { style = "width:410px;" })
                            .DataTextField("Name")
                            .DataValueField("id")                      
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetZones", "Permits").Data("dataForZones");
                                })
                                .ServerFiltering(false)
                                .Events(e => { e.Error("Generic_Error"); });
                            })  
                            .Enable(false)                    
                            .AutoBind(false)                    
                            .Filter("contains")
                            .Events(e => { e.DataBound("Zone_Databound").Change("Zone_Changed"); }) 
                        %>
                    </div>
                    <div class="form-group">
                        <label><% =resBundle.GetString("PermitsDataModel_Month") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("Month")
                            .HtmlAttributes(new { style = "width:410px;" })
                            .DataTextField("Name")
                            .DataValueField("id")                    
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetMonths", "Permits").Data("dataForMonths");
                                })
                                .ServerFiltering(false)
                                .Events(e => { e.Error("Generic_Error"); });
                            })  
                            .Enable(false)
                            .AutoBind(false)
                            .Events(e => { e.DataBound("Month_Databound").Change("Month_Changed"); }) 
                        %>
                    </div>
                    <div class="form-group">
                        <label><% =resBundle.GetString("PermitsDataModel_Tariff") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("Tariff")
                            .HtmlAttributes(new { style = "width:410px;" })
                            .DataTextField("Name")
                            .DataValueField("id")                    
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetTariffs", "Permits").Data("dataForTariffs");
                                })
                                .ServerFiltering(true)
                                .Events(e => { e.Error("Generic_Error"); });
                            })  
                            .Enable(false)
                            .AutoBind(false)
                            .Filter("contains")
                            .Events(e => { e.DataBound("Tariff_Databound").Change("Tariff_Changed"); }) 
                        %>
                    </div>
                    <div class="form-group">
                        <label><% =resBundle.GetString("Permits_NumPermits") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("NumPermits")
                            .HtmlAttributes(new { style = "width:410px;" })
                            .DataTextField("Name")
                            .DataValueField("id")                    
                            .DataSource(source => {
                                source.Read(read =>
                                {
                                    read.Action("GetNumPermits", "Permits").Data("dataForNumPermits");
                                })                        
                                .Events(e => { e.Error("Generic_Error"); });
                            })  
                            .Enable(false)
                            .AutoBind(false)
                            .Filter("contains")
                            .Events(e => { e.DataBound("NumPermits_Databound").Change("NumPermits_Changed"); }) 
                        %>
                    </div>
                    <script id="noPlateTemplate" type="text/x-kendo-tmpl">
                        # var value = instance.filterInput.val(); #
                        # var id = instance.element[0].id; #
                        # if (value != "<%=resBundle.GetString("Permits_SelectPlate")%>" && value != "") { #
                        <div>
                            <% = resBundle.GetString("Permits_PlateNotFound") %>
                        </div>
                        <br />
                        <button class="k-button" onclick="addNew('#: id #', '#: value #')"><% = resBundle.GetString("Permits_InsertNewPlate") %></button>
                        # }
                        else { #
                        <div>
                            <% = resBundle.GetString("Permits_PlateNotFound2") %>
                        </div>
                        # } #
                    </script>
                    <script>
                        function addNew(id, value) {
                            AddLicensePlate(id, value);
                        }
                    </script>
                    <div id="PermitGroups"></div>
                    <div class="form-group">
                        <label><% =resBundle.GetString("PermitsDataModel_TimeStep") %></label>
                        <%= Html.Kendo().DropDownList()
                            .Name("TimeStep")
                            .HtmlAttributes(new { style = "width:410px; font-size:15px;" })
                            .DataTextField("text")
                            .DataValueField("EndDate")
                            .DataSource(source => {
                                source.Read(read => 
                                { 
                                    read.Action("GetTimeSteps", "Permits").Data("dataForTimeSteps"); 
                                })
                                .ServerFiltering(true)                        
                                .Events(e => { e.Error("Generic_Error"); });                        
                            })
                            .Enable(false)
                            .AutoBind(false)
                            .Filter("contains")
                            .Events(e => { e.DataBound("TimeStep_Databound").Change("TimeStep_Changed"); }) 
                        %>            
                    </div>
                    <div id="Permits_Automatic_Renewal" class="form-group">
                        <% = Html.Kendo().CheckBox().Name("automatic_renewal").Checked(true).Label(resBundle.GetString("Permits_Automatic_Renewal")) %>
                    </div>
                    <div >
                            <p class="alert alert-bky-info">
                                <span class="bky-info"></span> 
                                &nbsp;
                                <% =resBundle.GetString("Permits_Automatic_Renewal_Description") %>
                            </p>
                    </div>
                    <div class="row-buttons">
                        <button type="submit" id="pay-button" class="btn btn-bky-primary" ><%= resBundle.GetString("Permits_Pay")%></button>
                        &nbsp;
                        <button type="button" id="logout-button" class="btn btn-bky-sec-default"><%= resBundle.GetString("Permits_Logout")%></button>
                    </div><!--//.row-buttons-->
                </div>
            </div>
        </div>
        <% 
    } // Html.BeginForm
%>

<script type="text/javascript">

    var NumPermits = 1;

    var Clearing = "";
    var PlateToAdd = null;
    var WhereAdded = null;

    var MaxLicensePlates = 0;
    var LicensePlateValues = [];

    var SelectedPlate1  = <%=Model.SelectedPlate1 != null  ? '\u0022' + Model.SelectedPlate1 + '\u0022'  : "null" %>;
    var SelectedPlate2  = <%=Model.SelectedPlate2 != null  ? '\u0022' + Model.SelectedPlate2 + '\u0022'  : "null" %>;
    var SelectedPlate3  = <%=Model.SelectedPlate3 != null  ? '\u0022' + Model.SelectedPlate3 + '\u0022'  : "null" %>;
    var SelectedPlate4  = <%=Model.SelectedPlate4 != null  ? '\u0022' + Model.SelectedPlate4 + '\u0022'  : "null" %>;
    var SelectedPlate5  = <%=Model.SelectedPlate5 != null  ? '\u0022' + Model.SelectedPlate5 + '\u0022'  : "null" %>;
    var SelectedPlate6  = <%=Model.SelectedPlate6 != null  ? '\u0022' + Model.SelectedPlate6 + '\u0022'  : "null" %>;
    var SelectedPlate7  = <%=Model.SelectedPlate7 != null  ? '\u0022' + Model.SelectedPlate7 + '\u0022'  : "null" %>;
    var SelectedPlate8  = <%=Model.SelectedPlate8 != null  ? '\u0022' + Model.SelectedPlate8 + '\u0022'  : "null" %>;
    var SelectedPlate9  = <%=Model.SelectedPlate9 != null  ? '\u0022' + Model.SelectedPlate9 + '\u0022'  : "null" %>;
    var SelectedPlate10 = <%=Model.SelectedPlate10 != null ? '\u0022' + Model.SelectedPlate10 + '\u0022' : "null" %>;

    <%
    Model.SelectedPlate1  = null;
    Model.SelectedPlate2  = null;
    Model.SelectedPlate3  = null;
    Model.SelectedPlate4  = null;
    Model.SelectedPlate5  = null;
    Model.SelectedPlate6  = null;
    Model.SelectedPlate7  = null;
    Model.SelectedPlate8  = null;
    Model.SelectedPlate9  = null;
    Model.SelectedPlate10 = null;
    %>

    /************************************* ERROR HANDLERS *************************************/

    function Generic_Error(e) {            
        if (e == null) {
            $(".notice p").html("<p><%= resBundle.GetString("Permits_Error_Result_Error_Generic")%></p>");
            $(".notice").css("display", "block");
        }
        else {
            $(".notice p").html("<p><%= resBundle.GetString("Permits_Error_Result_Error_Generic")%></p><p>[Error " + e.xhr.status + "] " + e.xhr.statusText + "</p>");
            $(".notice").css("display", "block");
        }
        $("html, body").animate({
            scrollTop: 0
        }, 1000);
    }

    function Custom_Error(e) {
        $(".notice p").html("<p>" + e + "</p>");
        $(".notice").css("display", "block");
        $("html, body").animate({
            scrollTop: 0
        }, 1000);
    }

    function Hide_Error() {
        $(".notice").css("display", "none");
    }

    /************************************* HELPERS *************************************/

    function Loading(show) {
        kendo.ui.progress($("#loader"), show);
    }

    function TogglePlateStatus(operation) {
        for (i = 1; i <= NumPermits; i++) {
            for (j = 1; j <= MaxLicensePlates; j ++) {
                switch (operation) {
                    case "enable":
                        $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).enable(true);
                        break;
                    case "disable":
                        $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).enable(false);
                        break;
                }
            }
        }
    }

    function ClearItems(combo) {
        if (combo == "Plates") {
            for (i = 1; i <= NumPermits; i++)
            {
                for (j = 1; j <= MaxLicensePlates; j++) {
                    $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).dataSource.data([]);
                    $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).text("");
                    $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value("");                    
                }
            }
            Clearing = "";
        }
        else {
            Clearing = combo;
            var ddl = $("#" + combo).data("kendoDropDownList");
            ddl.dataSource.data([]); // clears dataSource
            ddl.text(""); // clears visible text
            ddl.value(""); // clears invisible value
            ddl.enable(false);
            Clearing = "";
        }
    }

    function AnyPlateIsSelected() {
        var BlocksCompleted = 0;
        for (i = 1; i <= NumPermits; i++)
        {
            var AnySelected = false;
            for (j = 1; j <= MaxLicensePlates; j++) {
                if ($("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j) != null && 
                    $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value() != null && 
                    $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value().length > 0) {
                    BlocksCompleted++;
                    break;
                }
            }                
        }
        if (BlocksCompleted / NumPermits == 1)
        {
            return true;
        }
        return false;        
    }

    function ClearFrom(currentCombo) {
        switch (currentCombo)
        {
            case "Zone":
                ClearItems("Zone");
                ClearItems("Month");
                ClearItems("Tariff");    
                ClearItems("NumPermits");
                ClearItems("Plates");
                ClearItems("TimeStep");
                break;
            case "Month":
                ClearItems("Month");
                ClearItems("Tariff");
                ClearItems("NumPermits");
                ClearItems("Plates");
                ClearItems("TimeStep");
                break;
            case "Tariff":
                ClearItems("Tariff");
                ClearItems("NumPermits");
                ClearItems("Plates");
                ClearItems("TimeStep");
                break;
            case "NumPermits":
                ClearItems("NumPermits");
                ClearItems("Plates");
                ClearItems("TimeStep");
                break;
        }
    }

    function CityIsSelected() {
        return ($("#City").val() != "" && $("#City").val() != 0);            
    }

    function ZoneIsSelected() {
        return ($("#Zone").val() != "" && $("#Zone").val() != 0);
    }

    function MonthIsSelected() {
        return ($('#Month').val() != "" && $('#Month').val() != 0);
    }

    function TariffIsSelected() {
        return ($('#Tariff').val() != "" && $('#Tariff').val() != 0);
    }

    function NumPermitsIsSelected() {
        return ($('#NumPermits').val() != "" && $('#NumPermits').val() != 0);
    }

    /************************************* LOADERS *************************************/

    function LoadPlates() {
        if ($("#Tariff").val() != null) {
            for (i = 0; i < $("#Tariff").data().kendoDropDownList.dataSource._data.length; i++) {
                if ($("#Tariff").data().kendoDropDownList.dataSource._data[i].id == $("#Tariff").val()) {
                    MaxLicensePlates = $("#Tariff").data().kendoDropDownList.dataSource._data[i].MaxLicensePlates;
                    for (i = 1; i <= NumPermits; i++)
                    {
                        // We show as much plate fields as defined by tariff, for each one of the blocks
                        for (j = 1; j <= MaxLicensePlates; j++) {
                            $("#PlateField" + i + "_" + j).css("display", "block");
                            $('#LicensePlate'+i + "_" + j).data("kendoLicensePlate"+i + "_" + j).dataSource.read();
                            $('#LicensePlate'+i + "_" + j).data("kendoLicensePlate"+i + "_" + j).enable(true);
                        }
                        // We hide the others
                        for (j = (MaxLicensePlates + 1) ; j <= 10; j++) {
                            $("#PlateField" + i + "_" + j).css("display", "none");
                        }                        
                    }
                }
            }
        }
    }

    function LoadPlateGroups() {
    
        $("#PermitGroups").html("");

        for (i = 1; i <= NumPermits; i++)
        {
            $("#PermitGroups").append("<div><h3><%=resBundle.GetString("Permits_Permit") %> " + i + "</h3></div>");
            $("#PermitGroups").append("<div class='notice-fixed' style='padding-bottom:10px!important'><%=resBundle.GetString("Permits_OnlyOne") %></div>");

            for (j = 1; j <= 10; j++)
            {
                $("#PermitGroups").append("<div class='PlateField' id='PlateField" + i + "_" + j + "'><label><% =resBundle.GetString("PermitsDataModel_Plate") %> " + j + "</label><div id='LicensePlate" + i + "_" + j + "' name='LicensePlate" + i + "_" + j + "' style='width: 410px; display:inline-block;'></div><input type='hidden' name='LP" + i + "_" + j + "' id='LP" + i + "_" + j + "'></div>");

                var ds = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "GetPlates",
                            dataType: "json",
                            data: OptionalPlateToAdd
                        }                        
                    },
                    serverFiltering: false
                });
                ds.bind("error", Generic_Error);

                $("#LicensePlate" + i + "_" + j).kendoDropDownList({
                    autoBind: false,
                    dataSource: ds,
                    dataTextField: "LicensePlate",
                    dataValueField: "id",                    
                    dataBound: Plate_Databound,
                    change: Plate_Changed,
                    filter: "contains",
                    name: "LicensePlate" + i + "_" + j,
                    noDataTemplate: $("#noPlateTemplate").html()
                });
            }
        }

    }

    function LoadZones() {            
        Hide_Error();
        ClearFrom("Zone");

        if (CityIsSelected()) {                
            var zones = $('#Zone').data("kendoDropDownList");
            zones.dataSource.read(dataForZones());
            zones.enable(true);
        }
    }

    function LoadMonths() {            
        Hide_Error();
        ClearFrom("Month");            

        if (CityIsSelected() && ZoneIsSelected()) {
            var months = $('#Month').data("kendoDropDownList");
            months.dataSource.read(dataForMonths());
            months.enable(true);
        }
    }

    function LoadTariffs() {            
        Hide_Error();
        ClearFrom("Tariff");            

        var tariffs = $('#Tariff').data("kendoDropDownList");
        if (CityIsSelected() && ZoneIsSelected() && MonthIsSelected()) {
            tariffs.dataSource.read(dataForTariffs());
        }
    }

    function LoadNumPermits() {
        Hide_Error();
        ClearFrom("NumPermits");
        
        var numPermits = $("#NumPermits").data("kendoDropDownList");
        if (CityIsSelected() && ZoneIsSelected() && MonthIsSelected() && TariffIsSelected()) {
            numPermits.dataSource.read(dataForNumPermits());
            numPermits.enable(true);
        }
    }

    function LoadTimeSteps() {            
        Hide_Error();
        ClearItems("TimeStep");            

        if (CityIsSelected() && ZoneIsSelected() && MonthIsSelected() && TariffIsSelected() && NumPermitsIsSelected() && AnyPlateIsSelected()) {
            TogglePlateStatus("disable");
            var timestep = $('#TimeStep').data("kendoDropDownList");                
            timestep.dataSource.read(dataForTimeSteps());
        }
    }

    /************************************* DATA *************************************/

    function dataForZones() {            
        return {
            CityId: $("#City").val()
        };
    }

    function dataForMonths() {
        return {
            CityId: $("#City").val(),
            ZoneId: $("#Zone").val()
        };
    }

    function dataForTariffs() {
        return {
            CityId: $("#City").val(),
            ZoneId: $("#Zone").val()
        };
    }

    function dataForNumPermits() {
        return {
            TariffId: $("#Tariff").val()
        };
    }

    function dataForTimeSteps() {
        var data = {
            CityId: $("#City").val(),
            ZoneId: $("#Zone").val(),
            Tariff: $("#Tariff").val(),
            Month: $("#Month").val()
        }

        for (i = 1; i <= NumPermits; i++)
        {
            for (j = 1; j <= 10; j++)
            {
                data["LP"+i+"_"+j] = (MaxLicensePlates > (j-1) && $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j) != null && $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value()  != null && $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value().length > 0)  ? $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).value()  : "";
            }
        }

        return data;
    }

    function OptionalPlateToAdd() {
        return {
            PlateToAdd: PlateToAdd
        };
    }

    /************************************* CHANGED *************************************/

    function City_Changed() {                        
        LoadZones();            
    }

    function Zone_Changed() {   
        if ($("#Zone").val() == -1) {
            document.location = "PaymentMethod?InstallationId=" + $("#City").val();
        }
        else {
            LoadMonths();      
        }
    }

    function Month_Changed() {
        LoadTariffs();
    }

    function Tariff_Changed() {            
        LoadNumPermits();                        
    }

    function NumPermits_Changed() {            
        NumPermits = $("#NumPermits").val();
        LoadPlateGroups();
        LoadPlates();            
    }

    function Plate_Changed(e) {
        ClearItems("TimeStep");

        var plate = null;
        if (e.sender != null) {
            plate = e.sender.element.data("kendo" + e.sender.element[0].id);
        }
        else if (e.element != null) {
            plate = e.element.data("kendo" + e.element[0].id);
        }            

        if (plate != null) {
            var SelectedPlateIsValid = true;
            if (plate.element != null && plate.element.length > 0 && plate.element[0].value != "") {
                for (i = 1; i <= NumPermits; i++)
                {
                    for (j = 1; j <= MaxLicensePlates; j++) {
                        if (plate.element[0].id != ("LicensePlate" + i + "_" + j)) {
                            if ($("#LicensePlate" + i + "_" + j).val() != null && $("#LicensePlate" + i + "_" + j).val().length > 0) {
                                if ($("#LicensePlate" + i + "_" + j).val() == plate.element[0].value) {
                                    SelectedPlateIsValid = false;
                                }
                            }
                        }
                    }
                }
            }
            if (SelectedPlateIsValid) {
                LoadTimeSteps();
                $("#LP"+plate.element[0].id.toString().replace("LicensePlate", "")).val(plate.element[0].value);
                $("#"+plate.element[0].id).data("kendo" + plate.element[0].id).close();
            }
            else {
                e.sender.element.data("kendo" + e.sender.element[0].id).value([]);
                $("#LP"+e.sender.element[0].id.toString().replace("LicensePlate", "")).val("");
                e.sender.element.data("kendo" + e.sender.element[0].id).trigger("change");
                Custom_Error("<%= resBundle.GetString("Permits_PlateAlreadySelected") %>");
            }
        }
    }

    function TimeStep_Changed() {
        Hide_Error();
        if (TariffIsSelected()) {
            if (!$.isNumeric($("#TimeStep").val())) {
                if ($("#TimeStep").val() != "") {
                    Custom_Error($("#TimeStep").val());
                }
                else {
                    Hide_Error();
                }
            }
            else {
                Hide_Error();
            }
        }
    }

    /************************************* DATABOUND *************************************/

    function City_Databound() {
        var city = $('#City').data("kendoDropDownList");
    <%
    if (Model.SelectedCity != null)
    { 
    %>
        city.value(<% = Model.SelectedCity %>);
        City_Changed();
    <%
        Model.SelectedCity = null;
    }
    else 
    {
    %>
        if (city.dataSource.data().length == 1) {
            city.select(1);
            City_Changed();
        }
    <% 
    }
    %>
    }

    function Zone_Databound() {            
        var zones = $('#Zone').data("kendoDropDownList");
    <%
    if (Model.SelectedZone != null)
    { 
    %>
        zones.value(<% = Model.SelectedZone %>);
        Zone_Changed();
        zones.enable(true);
    <%
        Model.SelectedZone = null;
    }
    else 
    {
    %>
        if (zones.dataSource.data().length == 2) {
            zones.select(1);
            Zone_Changed();
            zones.enable(true);
        }
        else if (zones.dataSource.data().length > 0) {
            zones.select(0);
            Zone_Changed();
            zones.enable(true);
        }
        else {
            if (Clearing != "Zone" && $("#City").val() != "") {
                Generic_Error();
            }
        }
    <% 
    }
    %>
    }

    function Month_Databound() {
        var month = $('#Month').data("kendoDropDownList");

        <%
    if (!string.IsNullOrEmpty(Model.SelectedMonth))
    {
        if (Model.SelectedMonth == Model.CurrentMonth)
        {
    %>
        month.value("CURRENT");
        Month_Changed();
        month.enable(true);
    <%
        }
        else 
        { 
    %>            
        for (i = 0; i < $("#Month").data().kendoDropDownList.dataSource._data.length; i++) {
            if ($("#Month").data().kendoDropDownList.dataSource._data[i].id.toString().substring($("#Month").data().kendoDropDownList.dataSource._data[i].id.toString().length-4) == '<%=Model.SelectedMonth%>') {
                month.value($("#Month").data().kendoDropDownList.dataSource._data[i].id.toString());
                Month_Changed();
                month.enable(true);
                break;
            }
        }            
    <%
        }
        Model.SelectedMonth = null;
    }
    else 
    {
    %>
        if (month.dataSource.data().length == 2) {
            month.select(1);
            Month_Changed();
            month.enable(true);
        }
        else if (month.dataSource.data().length > 0) {
            month.select(0);
            Month_Changed();
            month.enable(true);
        }
    <% 
    }
    %>
    }

    function Plate_Databound(e) {
        var plate = e.sender.element.data("kendo" + e.sender.element[0].id);
        var SelectedPlate = null;

        switch (e.sender.element[0].id) {
            case "LicensePlate1_1":
                SelectedPlate = SelectedPlate1;
                SelectedPlate1 = null;
                break;
            case "LicensePlate1_2":
                SelectedPlate = SelectedPlate2;
                SelectedPlate2 = null;
                break;
            case "LicensePlate1_3":
                SelectedPlate = SelectedPlate3;
                SelectedPlate3 = null;
                break;
            case "LicensePlate1_4":
                SelectedPlate = SelectedPlate4;
                SelectedPlate4 = null;
                break;
            case "LicensePlate1_5":
                SelectedPlate = SelectedPlate5;
                SelectedPlate5 = null;
                break;
            case "LicensePlate1_6":
                SelectedPlate = SelectedPlate6;
                SelectedPlate6 = null;
                break;
            case "LicensePlate1_7":
                SelectedPlate = SelectedPlate7;
                SelectedPlate7 = null;
                break;
            case "LicensePlate1_8":
                SelectedPlate = SelectedPlate8;
                SelectedPlate8 = null;
                break;
            case "LicensePlate1_9":
                SelectedPlate = SelectedPlate9;
                SelectedPlate9 = null;
                break;
            case "LicensePlate1_10":
                SelectedPlate = SelectedPlate10;
                SelectedPlate10 = null;
                break;
        }

        if (plate.dataSource.data().length > 0 && PlateToAdd != null && WhereAdded == e.sender.element[0].id) {
            plate.value(PlateToAdd);
            Plate_Changed(this);                    
            PlateToAdd = null;
            WhereAdded = null;
        }   
        else if (SelectedPlate != null) {
            plate.value(SelectedPlate);
            Plate_Changed(this);
        }
        else if (plate.dataSource.data().length == 1) {
            plate.value(plate.dataSource.data()[0]);
            Plate_Changed(this);
        }                      
    }

    function Tariff_Databound() {            
        var tariff = $('#Tariff').data("kendoDropDownList");
    <%
    if (Model.SelectedTariff != null)
    { 
    %>
        tariff.value(<% = Model.SelectedTariff %>);
        Tariff_Changed();
        tariff.enable(true);
    <%
        Model.SelectedTariff = null;
    }
    else 
    {
    %>
        if (tariff.dataSource.data().length == 2) {
            tariff.select(1);
            Tariff_Changed();
            tariff.enable(true);
        }
        else if (tariff.dataSource.data().length > 0) {
            tariff.select(0);
            Tariff_Changed();
            tariff.enable(true);
        }
        else {
            if (Clearing != "Tariff") {
                Generic_Error();
            }
        }
    <% 
    }
    %>
    }

    function NumPermits_Databound() {            
        var numPermits = $('#NumPermits').data("kendoDropDownList");
    <%
    if (Model.SelectedNumPermits != null)
    { 
    %>
        numPermits.value(<% = Model.SelectedNumPermits %>);
        NumPermits_Changed();
        numPermits.enable(true);
    <%
        Model.SelectedNumPermits = null;
    }
    else 
    {
    %>
        if (numPermits.dataSource.data().length == 2) {
            numPermits.select(1);
            NumPermits_Changed();
            numPermits.enable(true);
        }
        else if (numPermits.dataSource.data().length > 0) {
            numPermits.select(0);
            NumPermits_Changed();
            numPermits.enable(true);
        }
        else {
            if (Clearing != "NumPermits") {
                Generic_Error();
            }
        }
    <%
    }
    %>
    }

    function TimeStep_Databound() {
        var timestep = $('#TimeStep').data("kendoDropDownList");
        if (timestep.dataSource.data().length == 2) {
            timestep.select(1);
            TimeStep_Changed();
            timestep.enable(true);
        }
        else if (timestep.dataSource.data().length > 0) {
            timestep.select(0);
            TimeStep_Changed();
            timestep.enable(true);
        }
        else {
            if (Clearing != "TimeStep") {
                Generic_Error();
            }
        }

        TogglePlateStatus("enable");

        if (TariffIsSelected()) {
            if (!$.isNumeric($("#TimeStep").val())) {
                if ($("#TimeStep").val() != "") {
                    Custom_Error($("#TimeStep").val());
                }
                else {
                    Hide_Error();
                }
            }
            else {
                Hide_Error();
            }
        }
    }

    /************************************* NEW LICENSE PLATE *************************************/

    function AddLicensePlate(id, NewPlate) {
        if (NewPlate != "") {
            Loading(true);
            $.ajax({
                type: 'POST',
                url: "<%= Url.Action("AddLicensePlate", "Permits") %>",
                data: { LicensePlate: NewPlate, CityId: $("#City").val() },
                success: function (response) {
                    if (response != 1) {
                        Custom_Error("<%= resBundle.GetString("AddLicensePlate.Title") %>", response[1]);
                    }
                    else {
                        PlateToAdd = NewPlate;
                        WhereAdded = id;

                        for (i = 1; i <= NumPermits; i++) {
                            for (j = 1; j <= MaxLicensePlates; j++) {
                                $("#LicensePlate" + i + "_" + j).data("kendoLicensePlate" + i + "_" + j).dataSource.read();
                            }
                        }
                    }
                    Loading(false);
                },
                error: function (xhr) {
                    Custom_Error("<%= resBundle.GetString("AddLicensePlate.Title") %>", "<%= resBundle.GetString("AddLicensePlate.Error") %>");
                    Loading(false);                            
                }
            });
        }
    }

    /************************************* FORM VALIDATION *************************************/

    function validateForm() {
        $("#pay-button").prop("disabled", "disabled");
        if ($("#City").val() > 0) {                
            if ($("#Zone").val() > 0) {                    
                if (AnyPlateIsSelected()) {                        
                    if ($("#Tariff").val() > 0) {                            
                        if ($("#TimeStep").val() != "") {                                
                            if ($.isNumeric($("#TimeStep").val())) {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        Custom_Error("<%= resBundle.GetString("Permits_Signup_Required") %>");
        $("#pay-button").removeProp("disabled");
        return false;
    };

    /************************************* LOGOUT *************************************/

    $("#logout-button").on("click", function () {
        document.location = "<%= Url.Action("PayForPermit", "Permits") %>";
    });  

</script>
</asp:Content>