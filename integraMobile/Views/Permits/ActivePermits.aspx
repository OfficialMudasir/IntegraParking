<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<PermitListContainerViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="integraMobile.Infrastructure"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Main_BttnActivePermits%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.core.css?v=2-0-1") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.standalone.css") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/datepicker/datepicker.css") %>" rel="stylesheet" type="text/css">
    <%-- OLD BEFORE BLINKAY --
    <style type="text/css">
        .permit-pay-button {
            padding:1px 5px!important;
            font-size:11px!important;
            line-height:1.5!important;
            border-radius:3px!important;
        }
    </style>
    --%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Account_Main_BttnActivePermits%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%--
<div id="breadcrumb-wrapper" class="row">

    <ul id="breadcrumb">
        <li>
            <span class="entypo-home"></span>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.SiteMaster_Home %>"><%=Resources.SiteMaster_Home %></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.Account_Main_BttnActivePermits%>
        </li>
    </ul>
</div>
--%>
<%--
<div class="title-alt">
    <h6><%=Resources.Account_Main_BttnActivePermits%></h6>
</div>
--%>
<div class="content-wrap">
    <!-- ROW CONTENT -->  
    <div class="row">
        <div class="col-sm-12 col-block">

            <!-- ROW FILTER -->
            <%Html.RenderPartial("PermitsFilter", Model.PermitFilterViewModel); %>

            <%  
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
            %>

            <%= Html.Grid(Model.PermitPagedList).Columns(column => {
                column
                    .For(x => x.GrpDescription)
                    .HeaderAttributes(new Dictionary<string, Object> { { "data-toggle", "true" } })
                    .Named(Resources.PermitsDataModel_Zone);
                column
                    .For(x => x.Tariff)
                    .HeaderAttributes(new Dictionary<string, Object> {  {"data-hide","phone"} })
                    .Named(Resources.PermitsDataModel_Tariff);
                column.For(x => x.Plates)
                    .Named(Resources.PermitsDataModel_Plates)
                    .HeaderAttributes(new Dictionary<string, Object> { { "data-toggle", "true" } });                               
                column
                    .For(x => x.DateIni)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .Named(Resources.Account_Op_Start_Date)
                    .HeaderAttributes(new Dictionary<string, Object> {  {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => x.DateEnd)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .Named(Resources.Account_Op_End_Date)
                    .HeaderAttributes(new Dictionary<string, Object> {  {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => string.Format(provider, "{0:0.00} {1}", x.Amount / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Op_Amount)
                    .HeaderAttributes(new Dictionary<string, Object> { {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => Html.Partial("RenewAutomatically", x))
                    .Named(Resources.Permits_Automatic_Renewal)
                    .HeaderAttributes(new Dictionary<string, Object> { {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => Html.Partial("PayButton", x))
                    .Named(Resources.Permits_Pay)
                    .HeaderAttributes(new Dictionary<string, Object> { {"data-hide","phone"} });
                column
                    .For(x => Html.Partial("ManageButton", x))
                    .Named(Resources.Permits_Manage)
                    .HeaderAttributes(new Dictionary<string, Object> { {"data-hide","phone, tablet_portrait"} });
            
            }).Columns(col => { }).Attributes(@id => "operations-table", @class => "footable operations-table")%>


            <div class="row operations-action">
                <div class="col-md-6">

                    <%= Html.Pager(Model.PermitPagedList) .First(Resources.Pager_First)
                    .Last(Resources.Pager_Last)
                    .Next(Resources.Pager_Next)
                    .Previous(Resources.Pager_Previous)
                    .Format(Resources.Pager_Format)
                    .SingleFormat(Resources.Pager_SingleFormat)
                    .NumberOfPagesToDisplay(5) %>

                </div>

                <%Html.RenderPartial("GenerateLinks", Model.PermitFilterViewModel); %>

            </div><!--//.operations-action-->
        </div><!--//.col-block-->
    </div><!--//.row-->
</div><!--//.content-wrap-->

<script type="text/javascript" src="<%= Url.Content("~/Content/js/toggle_close.js") %>"></script>
<script type="text/javascript" src="<%= Url.Content("~/Content/js/footable/js/footable.min.js?v=2-0-1") %>"></script>
<script type="text/javascript" src="<%= Url.Content("~/Content/js/footable/js/footable.sort.js?v=2-0-1") %>"></script>
<script type="text/javascript" src="<%= Url.Content("~/Content/js/datepicker/bootstrap-datepicker.min.js") %>"></script>
<% if (((CultureInfo)Session["Culture"]).Name.Substring(0, 2) != "en") { %>
<script type="text/javascript" src="<%= Url.Content("~/Content/js/datepicker/locales/") %>bootstrap-datepicker.<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>.js"></script>
<% } %>
<script type="text/javascript">
    $(function() {
        $('.footable').footable();
    });
    $('#DateIni').datepicker({
        language: '<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>',
        autoclose: true
    });
    $('#DateEnd').datepicker({
        language: '<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>',
        autoclose: true
    });
    $(".RenewAutomatically").on("change", function () {
        /*if (this.checked || $(this).prop("checked")) {
            document.location = "UpdateRenewalStatus?OperationId=" + this.value + "&CheckedStatus=1";
        }
        else {
            document.location = "UpdateRenewalStatus?OperationId=" + this.value + "&CheckedStatus=0";
        }*/
    });
    function AutoRenewal(Status, Value) {
        if (Status.toLowerCase() == "true") {
            document.location = "UpdateRenewalStatus?OperationId=" + Value + "&CheckedStatus=0";
        }
        else {
            document.location = "UpdateRenewalStatus?OperationId=" + Value + "&CheckedStatus=1";
        }
    }
</script>

</asp:Content>
