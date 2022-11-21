<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.FineModel>" %>

<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
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
    dynamic j = Json.Decode(ViewData["Result"].ToString());    
    if (j.result == "succeeded")
    {
        %>
            <div class="content-wrap">
                <div class="row">
                    <div class="col-sm-12 col-block">
                        <h3><%=Resources.Fine_TicketPaymentSuccess_1%></h3>
                        <table class="table">
                            <tr>
                                <th ><%=Resources.Fine_Plate%>:</td>
                                <th class="text-right"><% =j.PayTickets_Plate %></td>
                            </tr>
                            <tr>
                                <td ><%=Resources.Fine_Total%>:</td>
                                <td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_Total) / 100, j.PayTickets_AmountCurrencyIsoCode) %></td>
                            </tr>
                            <tr>
                                <td ><%=Resources.Fine_FEE%>:</td>
                                <td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_QFEE) / 100, j.PayTickets_AmountCurrencyIsoCode) %></td>
                            </tr>
                            <tr>
                                <td ><%=Resources.Fine_PartialVAT1%>:</td>
                                <td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_QVAT) / 100, j.PayTickets_AmountCurrencyIsoCode) %></td>
                            </tr>
                            <tr>
                                <th ><%=Resources.Fine_Amount%>:</td>
                                <th class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_TotalQuantity) / 100, j.PayTickets_AmountCurrencyIsoCode) %></td>
                            </tr>
                        </table>
                        <% 
                            if (Session["strRechargeEmailSubject"] != null && Session["strRechargeEmailBody"] != null)
                            {
                                %>
                                    <iframe src="SuccessMail" style="width:400px; height:300px; overflow:hidden; border:none;"></iframe>
                                <% 
                            }
                        %>
                        <div class="row-buttons">
                            <input class="btn btn-bky-primary" type="button" value="<%=Resources.Fine_TicketPayment_MainMenu%>" onclick="location.href = 'Fine';" />
                        </div><!--//.row-buttons-->
                    </div><!--//.col-block-->
                </div><!--//.row-->
            </div><!--//.content-wrap / if -->
        <%        
    }
    else 
    { 
        %>
            <div class="content-wrap">
                <div class="row abonos">
                    <div class="col-md-12 col-block">
                        <div class="alert alert-bky-danger">
                            <h3><%=Resources.Fine_TicketPaymentFailureTitle%></h3>
                            <hr>
                            <%=Resources.Fine_TicketPaymentFailure_1%>
                            <hr>
                            <%=Resources.Fine_TicketPaymentFailure_3%>
                            <hr>
                            <%=j.errorMessage%>
                        </div><!--//.alert-->
                        <div class="row-buttons">
                            <button type="button" value="<%=Resources.Fine_TicketPayment_MainMenu%>" class="btn btn-bky-primary" onclick="location.href = 'Fine';"></button>                            
                        </div>                        
                    </div><!--//.col-block-->
                </div><!--//.row-->
            </div><!--//.content-wrap / else -->
        <%        
    } // if else j.result
%>
<script type="text/javascript">
    history.pushState(null, null, location.href);
    window.onpopstate = function () {
        history.go(1);
    };
</script>
</asp:Content>
