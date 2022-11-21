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

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="<%= Url.Content("~/Content/kendo/kendo.common.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.default.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />    
    <link href="<%= Url.Content("~/Content/kendoExt/kendo.ext.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.dataviz.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.dataviz.default.min.css?v2016.3.1118") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/kendo/kendo.blinkay.css?v3.1") %>" rel="stylesheet" type="text/css" />

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

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Main_BttnActivePermits%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% System.Resources.ResourceManager resBundle = Resources.ResourceManager; %>  
<% 
    using (Html.BeginForm("Manage", "Permits", FormMethod.Post, new { onsubmit = "return validateForm()", autocomplete = "off" }))
    {
        %>
        <div class="main_container">        
            <div id="loader"></div>
            <div class="content-wrap">
                <div id="permits-errors" class="row " <% if (string.IsNullOrEmpty(Model.Error)) { %> style="display:none;"<% } %>>
                    <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12">

                        <%-- <div>
                            <img src="<%= Url.Content("~/Content/img/Permits/warning.png")%>" />
                        </div> --%>

                        <div class="alert alert-bky-danger notice">

                            <p style="display:inline-block;"><span class="bky-cancel"></span></p>
                            <p style="display:inline-block;"><%= Model.Error%></p>
                            <% Model.Error = string.Empty; %>

                        </div><!--//.notice.alert-->
                        
                    </div><!--//.col-block-->
                </div><!--//#permits-errors.row-->
                <div class="row">
                    <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-12 col-block">
                        <div class="content">
                            <h3 class="content-header"><%= resBundle.GetString("Permits_Manage")%></h3>
                            <hr>
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
                            <div class="PlateField form-group" id="PlateField1">
                                <%-- <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_01"), resBundle.GetString("PermitsDataModel_Plate")) %></label> --%>
                                <%= Html.Label( "LicensePlate1", string.Format("{0} {1}", resBundle.GetString("Permits_Plate_01"), resBundle.GetString("PermitsDataModel_Plate"))) %>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate1")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField2">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_02"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true)*/
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate2")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField3">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_03"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate3")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField4">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_04"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate4")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField5">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_05"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate5")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField6">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_06"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate6")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField7">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_07"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate7")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField8">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_08"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true) */
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate8")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField9">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_09"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true)*/
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate9")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <div class="PlateField form-group" id="PlateField10">
                                <label><% =string.Format("{0} {1}", resBundle.GetString("Permits_Plate_10"), resBundle.GetString("PermitsDataModel_Plate")) %></label>
                                <%= Html.Kendo().DropDownList()
                                    .AutoBind(false)
                                    /*.AutoClose(true)         
                                    .ClearButton(true)*/
                                    .DataSource(source => {
                                        source.Read(read =>
                                        {
                                            read.Action("GetPlates", "Permits").Data("OptionalPlateToAdd");
                                        })
                                        .ServerFiltering(true)
                                        .Events(e => { e.Error("Generic_Error"); });
                                    })
                                    .DataTextField("LicensePlate")
                                    .DataValueField("id")
                                    .Enable(true)     
                                    .Events(e => { e.DataBound("Plate_Databound").Change("Plate_Changed"); }) 
                                    .Filter(FilterType.Contains)
                                    .HtmlAttributes(new { style = "display: block;"})
                                    /*.MaxSelectedItems(1)*/
                                    .Name("LicensePlate10")
                                    .NoDataTemplateId("noPlateTemplate")
                                    /*.Placeholder(resBundle.GetString("Permits_SelectPlate"))*/
                                %>
                            </div>
                            <hr>
                            <div class="submit-button-little row-buttons">
                                <button type="submit" id="pay-button" class="btn btn-bky-primary"><%= resBundle.GetString("Permits_Save")%></button>
                                <button type="button" id="logout-button" class="btn btn-bky-sec-default"><%= resBundle.GetString("Permits_Logout")%></button>
                            </div>   
                        </div><!--//.content.OLD!-->
                    </div><!--//.col-block-->
                </div><!--//.row-->
            </div><!--//.content-wrap-->
        </div><!--//.main_container-->
        <%
    }  // Html.BeginForm
%>
<script type="text/javascript">

    var Clearing = "";
    var PlateAdded = null;
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

    $(document).ready(function() {
        EnablePlateFields();
    });

    /* ERRORS */

    function Generic_Error(e) {
        if (e == null) {
            $(".notice p").html("<p><%= resBundle.GetString("Permits_Error_Result_Error_Generic")%></p>");
            $(".notice").css("display", "block");
        }
        else {
            $(".notice p").html("<p><%= resBundle.GetString("Permits_Error_Result_Error_Generic")%></p><p>[Error " + e.xhr.status + "] " + e.xhr.statusText + "</p>");
            $(".notice").css("display", "block");
        }
    }

    function Custom_Error(e) {
        $(".notice p").html("<p>" + e + "</p>");
        $(".notice").css("display", "block");
    }

    function Hide_Error() {
        $(".notice").css("display", "none");
    }

    /* HELPERS */

    function Loading(show) {
        kendo.ui.progress($("#loader"), show);
    }

    function ClearItems(combo) {
        if (combo == "Plates") {
            for (i = 1; i <= 10; i++) {
                $("#LicensePlate" + i).data("kendoDropDownList").data([]);
                //$("#LicensePlate" + i).data("kendoDropDownList").trigger("change");
            }
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

    function EnablePlateFields() {
        MaxLicensePlates = <%=Model.MaxLicensePlates%>;
        // We show as much plate fields as defined by tariff
        for (i = 1; i <= MaxLicensePlates; i++) {
            $("#PlateField" + i).css("display", "block");
            $('#LicensePlate'+i).data().kendoDropDownList.dataSource.read();                
        }
        // We hide the others
        for (i = (MaxLicensePlates + 1) ; i <= 10; i++) {
            $("#PlateField" + i).css("display", "none");
        }
    }

    function IsAnyPlateSelected() {
        for (i = 0; i < MaxLicensePlates; i++) {
            if ($("#LicensePlate" + (i+1)).data("kendoDropDownList") != null && $("#LicensePlate" + (i+1)).data("kendoDropDownList").value() != null && $("#LicensePlate" + (i+1)).data("kendoDropDownList").value().length > 0) {
                return true;
            }
        }
        return false;
    }

    function LoadPlates() {
        Hide_Error();
        ClearItems("Plates");

        for (i = 1; i <= 10; i++)
        { 
            var plate = $('#LicensePlate'+i).data().kendoDropDownList;
            plate.dataSource.read();
            plate.enable(true);
        }
    }

    /* FILTERS */

    function filterCities() {
        return {
            CityId: $("#City").val()
        };
    }

    function OptionalPlateToAdd() {
        return {
            PlateToAdd: PlateAdded
        };
    }

    /* CHANGED */

    function Plate_Changed(e) {
        var plate = null;
        if (e.sender != null) {
            plate = e.sender.element.data("kendoDropDownList");
        }
        else if (e.element != null) {
            plate = e.element.data("kendoDropDownList");
        }            
        if (plate != null) {
            var SelectedPlateIsValid = true;
            if (plate.element != null && plate.element.length > 0 && plate.element[0].value != "") {
                for (i = 1; i <= 10; i++) {
                    if (plate.element[0].id != ("LicensePlate" + i)) {
                        if ($("#LicensePlate" + i).val() != null && $("#LicensePlate" + i).val().length > 0) {
                            if ($("#LicensePlate" + i).val() == plate.element[0].value) {
                                SelectedPlateIsValid = false;
                            }
                        }
                    }
                }
            }
            if (!SelectedPlateIsValid) {           
                e.sender.element.data("kendoDropDownList").value([]);
                e.sender.element.data("kendoDropDownList").trigger("change");
                Custom_Error("<%= resBundle.GetString("Permits_PlateAlreadySelected") %>");
            }            
            else {
                $("#"+plate.element[0].id).data("kendoDropDownList").close();
            }
        }
    }

    /* DATABOUND */

    function Plate_Databound(e) {

        var plate = e.sender.element.data("kendoDropDownList");

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

        var SelectedPlate = null;

        switch (e.sender.element[0].id) {
            case "LicensePlate1":
                SelectedPlate = SelectedPlate1;
                SelectedPlate1 = null;
                break;
            case "LicensePlate2":
                SelectedPlate = SelectedPlate2;
                SelectedPlate2 = null;
                break;
            case "LicensePlate3":
                SelectedPlate = SelectedPlate3;
                SelectedPlate3 = null;
                break;
            case "LicensePlate4":
                SelectedPlate = SelectedPlate4;
                SelectedPlate4 = null;
                break;
            case "LicensePlate5":
                SelectedPlate = SelectedPlate5;
                SelectedPlate5 = null;
                break;
            case "LicensePlate6":
                SelectedPlate = SelectedPlate6;
                SelectedPlate6 = null;
                break;
            case "LicensePlate7":
                SelectedPlate = SelectedPlate7;
                SelectedPlate7 = null;
                break;
            case "LicensePlate8":
                SelectedPlate = SelectedPlate8;
                SelectedPlate8 = null;
                break;
            case "LicensePlate9":
                SelectedPlate = SelectedPlate9;
                SelectedPlate9 = null;
                break;
            case "LicensePlate10":
                SelectedPlate = SelectedPlate10;
                SelectedPlate10 = null;
                break;
        }

        if (plate.dataSource.data().length > 0 && PlateAdded != null && WhereAdded == e.sender.element[0].id) {
            plate.value(PlateAdded);
            Plate_Changed(this);                    
            PlateAdded = null;
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

    /* NEW LICENSE PLATE */

    function AddLicensePlate(id, NewPlate) {
        console.log("AddLicensePlate - " + id + " - " + NewPlate);
        if (NewPlate != "") {
            Loading(true);
            $.ajax({
                type: 'POST',
                url: "<%= Url.Action("AddLicensePlate", "Permits") %>",
                data: { LicensePlate: NewPlate, CityId: <%=Model.SelectedCity%> },
                success: function (response) {
                    if (response != 1) {
                        Custom_Error("<%= resBundle.GetString("AddLicensePlate.Title") %>", response[1]);
                    }
                    else {
                        PlateAdded = NewPlate;
                        WhereAdded = id;

                        if (MaxLicensePlates > 0) {  $('#LicensePlate1').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 1) {  $('#LicensePlate2').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 2) {  $('#LicensePlate3').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 3) {  $('#LicensePlate4').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 4) {  $('#LicensePlate5').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 5) {  $('#LicensePlate6').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 6) {  $('#LicensePlate7').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 7) {  $('#LicensePlate8').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 8) {  $('#LicensePlate9').data().kendoDropDownList.dataSource.read(); }
                        if (MaxLicensePlates > 9) { $('#LicensePlate10').data().kendoDropDownList.dataSource.read(); }
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

    /* FORM VALIDATION */

    function validateForm() {
        $("#pay-button").prop("disabled", "disabled");
        if (IsAnyPlateSelected()) {
            return true;
        }
        Custom_Error("<%= resBundle.GetString("Permits_Signup_Required") %>");
        $("#pay-button").removeProp("disabled");
        return false;
    };

    /* LOGOUT */

    $("#logout-button").on("click", function () {
        document.location = "<%= Url.Action("ActivePermits", "Permits") %>";
    });

</script>
</asp:Content>