<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<InvoicesListContainerViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="integraMobile.Infrastructure"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Invoices</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<link rel="stylesheet" href="../Content/CSS/operations.css" type="text/css">
<link href="../Content/CSS/jquery/custom-theme/jquery-ui-1.9.2.custom.css" rel="stylesheet">
<script src="../Content/jquery/jquery-1.8.3.js"></script>
<script src="../Content/jquery/jquery-ui-1.9.2.custom.js"></script>
<script src="../Content/jquery/i18n/jquery.ui.datepicker-<%=((CultureInfo)Session["Culture"]).Name.Substring(0, 2)%>.js"></script>
<script src="../Content/jquery/i18n/jquery.ui.datepicker-<%=((CultureInfo)Session["Culture"]).Name%>.js"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<p>&nbsp;</p>
  


<%Html.RenderPartial("InvoicesFilter", Model.InvoicesFilterViewModel); %>
<div class="datagrid" style="clear:left;">

<%  NumberFormatInfo provider = new NumberFormatInfo();
    provider.NumberDecimalSeparator = ".";
%>

    <%= Html.Grid(Model.InvoicesPagedList)
            .Empty(Resources.No_Registries_Found)
            .Sort(Model.GridSortOptions)
            .Columns(column => {
                column.For(x => x.InvoiceNumber)
                    .SortColumnName("InvoiceNumber")
                    .Named(Resources.Account_Invoice_InvoiceNumber);
                
                column.For(x => x.Date)                    
                    .Format("{0:dd/MM/yy}")
                    .SortColumnName("Date")
                    .Named(Resources.Account_Invoice_Date);
                   

                column.For(x => string.Format(provider, "{0:0.00} {1}",x.Amount / 100.0, x.CurrencyIsoCode))
                    .SortColumnName("Amount")
                    .Named(Resources.Account_Invoice_Amount);

                column.For(x => (x.AmountOps.HasValue ? string.Format(provider, "{0:0.00} {1}", x.AmountOps / 100.0, x.CurrencyIsoCode):""))
                    .SortColumnName("AmountOps")
                    .Named(Resources.Account_Invoice_AmountOps);
                
                column.For(x => "<a href=\"" + 
                    Url.RouteUrl(new { Controller = "Account", Action = "Invoice", invoiceID = x.InvoiceId }) + 
                        "\" target=\"_blank\"><img src=\"../Content/img/pdf.jpg\" width=\"14px\" height=\"16px\" style=\"vertical-align:baseline\"></a>").Named("A").Encode(false)
                    .SortColumnName("DownloadURL")
                    .Named(Resources.Account_Invoice_Down_Link);
                                    
    }).RowStart(row => string.Format("<tr {0}>", row.IsAlternate ? "class=\"alt\"" : "")) %>
  <table>
  <tr>
  <td class="tablePager">
  <%= Html.Pager(Model.InvoicesPagedList)
    .First(Resources.Pager_First)
    .Last(Resources.Pager_Last)
    .Next(Resources.Pager_Next)
    .Previous(Resources.Pager_Previous)
    .Format(Resources.Pager_Format)
    .SingleFormat(Resources.Pager_SingleFormat)
    .NumberOfPagesToDisplay(5) %>
  </td>
  </tr>
  </table>
  
</div>


</asp:Content>
