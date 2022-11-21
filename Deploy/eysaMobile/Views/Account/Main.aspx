<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Account.Master"  Inherits="System.Web.Mvc.ViewPage<OperationsListContainerViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="integraMobile.Models" %>
<%@ Import Namespace="integraMobile.Infrastructure"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Main Menu</asp:Content>

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

<%Html.RenderPartial("OperationsFilter", Model.OperationsFilterViewModel); %>
  
<div class="datagrid" style="clear:left;">

<%  NumberFormatInfo provider = new NumberFormatInfo();
    provider.NumberDecimalSeparator = ".";
%>

    <%= Html.Grid(Model.OperationsPagedList)
            .Empty(Resources.No_Registries_Found)
            .Sort(Model.GridSortOptions)
            .Columns(column => {
                column.For(x => x.Type)
                    .SortColumnName("TypeId")
                    .Named(Resources.Account_Op_Operation);

                column.For(x => x.Installation)
                    .SortColumnName("Installation")
                    .Named(Resources.Account_Op_Installation);

                column.For(x => x.Sector)
                    .SortColumnName("Sector")
                    .Named(Resources.Account_Op_Zone);

                column.For(x => x.Tariff)
                    .SortColumnName("Tariff")
                    .Named(Resources.Account_Op_Tariff);


                column.For(x => x.Plate)
                    .SortColumnName("Plate")
                    .Named(Resources.Account_Op_LicensePlate);                                 
                
                column.For(x => x.Date)                    
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .SortColumnName("Date")
                    .Named(Resources.Account_Op_Date);
                   

                column.For(x => string.Format(provider, "{0:0.00} {1}",x.Amount / 100.0, x.CurrencyIsoCode))
                    .SortColumnName("Amount")
                    .Named(Resources.Account_Op_Amount);

                column.For(x => string.Format(provider, "{0:0.00} {1}", x.AmountFEE / 100.0, x.CurrencyIsoCode))
                    .SortColumnName("AmountFEE")
                    .Named(Resources.Account_Op_AmountFEE);

                column.For(x => string.Format(provider, "{0:0.00} {1}", x.AmountBONUS / 100.0, x.CurrencyIsoCode))
                                    .SortColumnName("AmountBONUS")
                                    .Named(Resources.Account_Op_AmountBONUS);                

                column.For(x => string.Format(provider, "{0:0.00} {1}", x.AmountVAT / 100.0, x.CurrencyIsoCode))
                    .SortColumnName("AmountVAT")
                    .Named(Resources.Account_Op_AmountVAT);

                column.For(x => string.Format(provider, "{0:0.00} {1}", x.AmountTotal / 100.0, x.CurrencyIsoCode))
                    .SortColumnName("AmountTotal")
                    .Named(Resources.Account_Op_AmountTotal);
                
                /*column.For(x => string.Format(provider, "{0:0.000000}", x.ChangeApplied))                    
                    .SortColumnName("ChangeApplied")
                    .Named(Resources.Account_Op_ChangeApplied);    */                                    

                column.For(x => x.DateIni)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .SortColumnName("DateIni")
                    .Named(Resources.Account_Op_Start_Date);

                column.For(x => x.DateEnd)
                    .Format("{0:dd/MM/yy HH:mm:ss}")
                    .SortColumnName("DateEnd")
                    .Named(Resources.Account_Op_End_Date);

                column.For(x => x.Time)
                    .SortColumnName("Time")
                    .Named(Resources.Account_Op_Duration);
                    
                column.For(x => x.TicketNumber)
                    .SortColumnName("TicketNumber")
                    .Named(Resources.Account_Op_TicketNumber);
                    
                column.For(x => x.TicketData)
                    .SortColumnName("TicketData")
                    .Named(Resources.Account_Op_TicketData);

                column.For(x => x.AdditionalUser)
                                    .SortColumnName("AdditionalUser")
                                    .Named(Resources.Account_Op_AdditionalUser);                
                    
    }).RowStart(row => string.Format("<tr {0}>", row.IsAlternate ? "class=\"alt\"" : "")) %>
  
  <table>
  <tr>
  <td class="tablePager">
  <%= Html.Pager(Model.OperationsPagedList)
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
