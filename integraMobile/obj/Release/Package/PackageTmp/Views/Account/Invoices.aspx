<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<InvoicesListContainerViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="integraMobile.Infrastructure"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Account_Main_BttnInvoices%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.core.css?v=2-0-1") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/footable/css/footable.standalone.css") %>" rel="stylesheet" type="text/css">
    <link href="<%= Url.Content("~/Content/js/datepicker/datepicker.css") %>" rel="stylesheet" type="text/css">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%-- Resources.Account_Main_BttnOperations --%> 
    <%=Resources.Account_Main_BttnInvoices%>

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
        <li><a href="<%= Url.Action("Main", "Account") %>" title="<%=Resources.Account_Main_BttnOperations%>"><%=Resources.Account_Main_BttnOperations%></a>
        </li>
        <li><i class="fa fa-lg fa-angle-right"></i>
        </li>
        <li><%=Resources.Account_Main_BttnInvoices%>
        </li>
    </ul>
</div>
--%>
<%-- 
<div class="title-alt">
    <h2><%=Resources.Account_Main_BttnInvoices%></h2>
</div>
--%>

<div class="content-wrap">
    <div class="row">
        <div class="col-md-12 col-block">
            <%Html.RenderPartial("InvoicesFilter", Model.InvoicesFilterViewModel); %>

            <%  NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
            %>

            <%= Html.Grid(Model.InvoicesPagedList).Empty(Resources.No_Registries_Found).Columns(column => {
                column.For(x => x.InvoiceNumber)
                    .Named(Resources.Account_Invoice_InvoiceNumber)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-toggle","true"} });
                column.For(x => x.Date)                    
                    .Format("{0:dd/MM/yy}")
                    .Named(Resources.Account_Invoice_Date)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                column.For(x => string.Format(provider, "{0:0.00} {1}",x.Amount / 100.0, x.CurrencyIsoCode))
                    .Named(Resources.Account_Invoice_Amount)
                            .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"} });
                column.For(x => (x.AmountOps.HasValue ? string.Format(provider, "{0:0.00} {1}", x.AmountOps / 100.0, x.CurrencyIsoCode):""))
                    .SortColumnName("AmountOps")
                    .Named(Resources.Account_Invoice_AmountOps);
        

                column.For(x => "<a href=\"" + Url.RouteUrl(new { Controller = "Account", Action = "Invoice", invoiceID = x.InvoiceId}) + "\" target=\"_blank\" class=\"btn btn-bky-sec-danger\"><i class=\"bky-billing\"></i> Get PDF</a>").Named("A").Encode(false)
                    .Named(Resources.Account_Invoice_Down_Link)
                    .HeaderAttributes(new Dictionary<string,Object> { {"data-hide","phone, tablet_portrait"}, {"data-sort-ignore", "true"} })
                    .Attributes(x => new Dictionary<string, object>() { {"class","invoicing-download"} });             
            }).Columns(col => {}).Attributes(@id => "invoicing-table", @class => "footable invoicing-table") %>

            <div class="row operations-action">
                <div class="col-xs-12">
                    <%= Html.Pager(Model.InvoicesPagedList) %>
                    <%--
                    <ul class="pagination">
                        <li><a href="#">&laquo;</a></li>
                        <li><a href="#">1</a></li>
                        <li><a href="#">2</a></li>
                        <li><a href="#">3</a></li>
                        <li><a href="#">4</a></li>
                        <li><a href="#">5</a></li>
                        <li><a href="#">&raquo;</a></li>
                    </ul>
                    --%>
                </div><!--// .col-xs-12 -->

            </div><!--// .row.operations-action -->
        </div>
    </div><!-- row -->
</div><!-- content-wrap-->

<script type="text/javascript" src="<%= Url.Content("~/Content/js/toggle_close.js") %>"></script>

<script type="text/javascript" src="<%= Url.Content("~/Content/js/footable/js/footable.min.js?v=2-0-1") %>"></script>
<script type="text/javascript" src="<%= Url.Content("~/Content/js/footable/js/footable.sort.js?v=2-0-1") %>"></script>

<%-- try new library
<script type="text/javascript" src="<%= Url.Content("~/Content/js/footable-bootstrap/js/footable.min.js") %>"></script>
--%>
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
