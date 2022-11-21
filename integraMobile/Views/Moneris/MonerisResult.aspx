<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.FineModel>" %>

<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Resources.Fine_TicketPaymentSuccessTitle%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentTitle" runat="server">
    <%=Resources.Fine_TicketPaymentSuccessTitle%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--
    <div class="row">
        <div id="paper-top">
            <div class="col-sm-12">
                <h2 class="tittle-content-header">
                    <span><%=Resources.Fine_TicketPaymentSuccessTitle%></span>
                </h2>
            </div>
        </div>
    </div>
--%>
    <%
        if (ViewData["Result"] != null) 
        {        
            dynamic j = Json.Decode(ViewData["Result"].ToString());   

            if (j.result != null && j.result == "succeeded")
            {
                %>
                    <div class="content-wrap">
                        <div class="row">
                            <div class="col-sm-12 col-block">
                                    <h3><%=Resources.Fine_TicketPaymentSuccess_1%></h3>
                                    <table class="table">
                                        <tr>
                                            <th><%=Resources.Fine_Plate%>:</td>
                                            <th class="text-right"><% =(j.PayTickets_Plate != null ? j.PayTickets_Plate.ToString() : "") %></td>
                                        </tr>
                                        <tr>
                                            <td ><%=Resources.Fine_Total%>:</td>
                                            <td  class="text-right"><% =(j.PayTickets_Total != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_Total) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        </tr>
                                        <tr>
                                            <td ><%=Resources.Fine_FEE%>:</td>
                                            <td  class="text-right"><% =(j.PayTickets_PercFEE != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_PercFEE) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        </tr>
                                        <tr>
                                            <td ><%=Resources.Fine_PartialVAT1%>:</td>
                                            <td  class="text-right"><% =(j.PayTickets_PartialVAT1 != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_PartialVAT1) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        </tr>
                                        <tr style="font-weight: bold;">
                                            <th  ><%=Resources.Fine_Amount%>:</td>
                                            <th  class="text-right"><% =(j.PayTickets_TotalQuantity != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_TotalQuantity) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        </tr>
                                    </table>
                                    <div class="row-buttons">
                                        <input class="btn btn-bky-primary" type="button" value="<%=Resources.Fine_TicketPayment_MainMenu%>" onclick="location.href = 'Fine';" />
                                    </div>
                            </div><!--//.col-block-->
                        </div><!--//.row-->
                    </div><!--//.content-wrap-->
                <%        
            }
            else 
            { 
                %>
                    <div class="content-wrap">
                        <div class="row">
                            <div class="col-sm-12 col-block">
                                    <h3><%=Resources.Fine_TicketPaymentFailureTitle%></h3>
                                    <div class="alert alert-bky-danger" >
                                        <span class="bky-info"></span> 
                                        &nbsp; 
                                        <%=Resources.Fine_TicketPaymentFailure_1%>
                                    </div>
                                    <div class="alert alert-bky-danger" >
                                        <span class="bky-info"></span> 
                                        &nbsp; 
                                        <%=Resources.Fine_TicketPaymentFailure_3%>
                                    </div>
                                    <div class="alert alert-bky-danger" >
                                        <span class="bky-info"></span> 
                                        &nbsp; 
                                        <%=j.errorMessage%>
                                    </div>
                                    <div class="row-buttons">
                                        <input type="button" value="<%=Resources.Fine_TicketPayment_MainMenu%>" class="btn btn-bky-primary" onclick="location.href = 'Fine';"/>                            
                                    </div>                        
                            </div><!--//.col-block-->
                        </div><!--//.row-->
                    </div><!--//.content-wrap-->
                <%        
            } // if else j.result
        } // if ViewData["Result"] 
    %>
</asp:Content>
