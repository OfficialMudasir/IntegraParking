<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Ajax" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.WebPages" %>

<%@ Import Namespace="Telerik.Reporting" %>
<%@ Import Namespace="Telerik.ReportViewer.Mvc" %>
<%@ Import Namespace="Kendo.Mvc.UI" %>

<%@ Import Namespace="backOffice.Infrastructure" %>
<%@ Import Namespace="PBPPlugin" %>
<%@ Import Namespace="integraMobile.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <% var resBundle = ResourceBundle.GetInstance("PBPPlugin"); %>
    <%= resBundle.GetString("PBPPlugin", "Reports.Title", "Informes") %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="https://netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css" rel="stylesheet" />

    <!--<link href="https://cdn.kendostatic.com/2013.2.918/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="https://cdn.kendostatic.com/2013.2.918/styles/kendo.blueopal.min.css" rel="stylesheet" />-->

    <style>
        #reportViewer1 {
            position: absolute;
            left: 20px;
            right: 20px;
            top: 200px;
            bottom: 5px;
            overflow: hidden;
            font-family: Verdana, Arial;
        }

        /*#reportViewer1 .listviewitem:first-child {
            pointer-events: none;
        }*/
    </style>
    
    <!--    -->
    <!--<link href="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/styles/kendo.common.min.css") %>" rel="stylesheet" type="text/css" />    -->
    <link href="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/styles/telerikReportViewer.css") %>" rel="stylesheet" type="text/css" />
    <!--<link href="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/styles/kendo.blueopal.min.css") %>" rel="stylesheet" type="text/css" />    -->

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2></h2>
    
    <%
        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
        //var oParams = new Telerik.Reporting.ParameterCollection();
        //oParams.Add("CurrentCulture", culture.Name);        
    %>

    <%: Html.TelerikReporting().ReportViewer()
        // Each report viewer must have an id - it will be used by the initialization script 
        // to find the element and initialize the report viewer.
        .Id("reportViewer1")
        // The url of the service which will provide the report viewer with reports.
        // The service must be properly configured so that the report viewer can 
        // successfully communicate with the server. 
        // For more information on how to configure the service please check http://www.telerik.com/help/reporting/telerik-reporting-rest-conception.html.
        .ServiceUrl(Url.Content("~/api/reports/"))
        // The url for the report viewer template. The template can be edited - 
        // new functionalities can be added and unneeded ones can be removed.
        // For more information please check http://www.telerik.com/help/reporting/html5-report-viewer-templates.html.
        //.TemplateUrl(Url.Content(RouteConfig.BasePath + "ReportViewer/templates/telerikReportViewerTemplate-8.2.14.1204." + culture + ".html"))
        .TemplateUrl(Url.Content(RouteConfig.BasePath + "ReportViewer/templates/telerikReportViewerTemplate.html"))
        // Strongly typed ReportSource - TypeReportSource or UriReportSource.
        //.ReportSource("Finantial/FinantialReportsIndex.trdx")        
        .ReportSource(new TypeReportSource() { TypeName = typeof(integraMobile.Reports.Finantial.FinantialReportsIndex).AssemblyQualifiedName })
        // Specifies whether the viewer is in interactive or print preview mode.
        // PRINT_PREVIEW - Displays the paginated report as if it is printed on paper. Interactivity is not enabled.
        // INTERACTIVE - Displays the report in its original width and height witn no paging. Additionally interactivity is enabled.
        .ViewMode(ViewModes.INTERACTIVE)
        // Sets the scale mode of the viewer.
        // Three modes exist currently:
        // FIT_PAGE - The whole report will fit on the page (will zoom in or out), regardless of its width and height.
        // FIT_PAGE_WIDTH - The report will be zoomed in or out so that the width of the screen and the width of the report match.
        // SPECIFIC - Uses the scale to zoom in and out the report.
        .ScaleMode(ScaleModes.SPECIFIC)        
        // Zoom in and out the report using the scale
        // 1.0 is equal to 100%, i.e. the original size of the report
        .Scale(1.0)
        // Sets whether the viewer’s client session to be persisted between the page’s refreshes(ex. postback). 
        // The session is stored in the browser’s sessionStorage and is available for the duration of the page session.
        .PersistSession(false)
        // Sets the print mode of the viewer.
        .PrintMode(PrintMode.AutoSelect) %>

    <!--kendo.all.min.js can be used as well instead of kendo.web.min.js and kendo.mobile.min.js-->    
    <!--<script src="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/js/telerikReportViewer-8.2.14.1204." + System.Threading.Thread.CurrentThread.CurrentCulture + ".js?v1.0") %>" ></script>-->
    <script src="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/js/kendo.subset.2015.3.930.min.js?v1.0") %>" ></script>
    <script src="<%= Url.Content(RouteConfig.BasePath + "ReportViewer/js/telerikReportViewer-9.2.15.1216." + System.Threading.Thread.CurrentThread.CurrentCulture + ".js?v1.0") %>" ></script>

    <!--<script src="http://cdn.kendostatic.com/2013.2.918/js/kendo.web.min.js"></script>-->
    <!--kendo.mobile.min.js - optional, if gestures/touch support is required-->
    <!--<script src="http://cdn.kendostatic.com/2013.2.918/js/kendo.mobile.min.js"></script>-->

    <script type="text/javascript">
        /*$(document).ready(function () {
        $("#reportViewer1")
              .telerik_ReportViewer({
                  parameterEditors: [
                      {
                          match: function (parameter) {
                              return Boolean(parameter.availableValues) && !parameter.multivalue;
                          },

                          createEditor: function (placeholder, options) {
                              var dropDownElement = $(placeholder).htmll('<div></div>'),
                                        parameter,
                                        valueChangedCallback = options.parameterChanged,
                                        dropDownList;

                              function onChange() {
                                  var val = dropDownList.value();
                                  valueChangedCallback(parameter, val);
                              }

                              return {
                                  beginEdit: function (param) {

                                      parameter = param;

                                      $(dropDownElement).kendoDropDownList({
                                          dataTextField: "name",
                                          dataValueField: "value",
                                          dataSource: parameter.availableValues,
                                          change: onChange
                                      });

                                      dropDownList = $(dropDownElement).data("kendoDropDownList");
                                  }
                              };
                          }
                      }]
              });
        });*/

</script>

</asp:Content>

