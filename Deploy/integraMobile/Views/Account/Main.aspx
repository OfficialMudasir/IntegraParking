<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<OperationsListContainerViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="integraMobile.Infrastructure"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%-- Resources.Account_Main_BttnOperations --%>
    <%=Resources.Account_HisOperations%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.core.css?v=2-0-1") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.standalone.css") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/datepicker/datepicker.css") %>" rel="stylesheet" type="text/css">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- Resources.Account_Main_BttnOperations --%>
    <%=Resources.Account_HisOperations%>
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
        <li><%=Resources.Account_Main_BttnOperations%>
        </li>
    </ul>
</div>
--%>

<%--
<div class="title-alt" class="container-fluid .col-block">
    <h3><%=Resources.Account_HisOperations%></h3>
</div>
--%>

<div id="content-main">
    <div class="row">
        <div class="col-md-12 col-block">


            <%Html.RenderPartial("OperationsFilter", Model.OperationsFilterViewModel); %>

            <%  NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
            %>

            <%= Html.Grid(Model.OperationsPagedList).Columns(column => {
                column
                    .For(x => x.Type)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-toggle","true"} })
                    .Named(Resources.Account_Op_Operation);
                column
                    .For(x => x.Installation)
                    .Named(Resources.Account_Op_Installation)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => x.Sector)
                    .Named(Resources.Account_Op_Zone)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => x.Tariff)
                    .Named(Resources.Account_Op_Tariff)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column.For(x => x.Plates)
                    .Named(Resources.PermitsDataModel_Plates)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });                               
                column
                    .For(x => x.Date)                    
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .Named(Resources.Account_Op_Date)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => string.Format(provider, "{0:0.00} {1}",x.Amount / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Op_Amount)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => string.Format(provider, "{0:0.00} {1}", x.AmountFEE / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Op_AmountFEE)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => string.Format(provider, "{0:0.00} {1}", x.AmountVAT / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Op_AmountVAT)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => string.Format(provider, "{0:0.00} {1}", x.AmountTotal / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Op_AmountTotal)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                /* column
                    .For(x => string.Format(provider, "{0:0.000000}", x.ChangeApplied))                    
                    .Named(Resources.Account_Op_ChangeApplied)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} }); */                        
                column
                    .For(x => x.DateIni)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .Named(Resources.Account_Op_Start_Date)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => x.DateEnd)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .Named(Resources.Account_Op_End_Date)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                column
                    .For(x => x.Time)
                    .Named(Resources.Account_Op_Duration)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
                column
                    .For(x => x.TicketNumber)
                    .Named(Resources.Account_Op_TicketNumber)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} }); 
                column
                    .For(x => x.TicketData)
                    .Named(Resources.Account_Op_TicketData)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","all"} });
            }).Columns(col => {}).Attributes(@id => "operations-table", @class => "footable operations-table") %>

            <div class="row operations-action">
                <div class="col-md-6">

                    <%= Html.Pager(Model.OperationsPagedList) .First(Resources.Pager_First)
                    .Last(Resources.Pager_Last)
                    .Next(Resources.Pager_Next)
                    .Previous(Resources.Pager_Previous)
                    .Format(Resources.Pager_Format)
                    .SingleFormat(Resources.Pager_SingleFormat)
                    .NumberOfPagesToDisplay(5) %>

                </div><!--// .col-xs-6-->

                <%Html.RenderPartial("GenerateLinks", Model.OperationsFilterViewModel); %>

            </div><!--// .row.operation-actions -->




        </div><!--// .col-sm-12-->

    </div><!--// .row -->
</div><!--// #content-main -->

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
</script>

</asp:Content>
