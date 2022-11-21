<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Blinkay.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.FineModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<!-- NEW FORM -->
<div id="page-content">
	<div class="container">
		<div id="form-win-container" class=" col-md-12 col-lg-10 p-4 mb-4">

			<!-- WINDOW TITTLE	-->
			<div id="form-title" class="row justify-content-center p-2 my-2">
				<div id="form-win-header" class="col">

					<h1 class="title-portal">
						<!-- TITLE ICON // -->
						<svg version="1.1" id="title-icon" xmlns="http://www.w3.org/2000/svg" x="0" y="0" viewbox="0 0 64 64" xml:space="preserve">

							<path class="title-icon-path" d="M61.4 11.4C60 10 58.1 9.1 56 8.9h-.8c-1.8 0-3.5.7-4.6 1.9l-4.4 4.5-17.1 17.1c-.4.4-.6.9-.6 1.4v8.4c0 1.1.9 2 2 2h8.6c.5 0 1-.2 1.4-.6l18.4-18.2 2.9-3c1.4-1.2 2.2-3 2.2-4.9v-.8c-.2-2.1-1.2-3.9-2.6-5.3zM38.3 40.3h-5.8v-5.6l16.6-16.6 5.7 5.9-16.5 16.3zm20.9-21L57.6 21 52 15.2l1.6-1.6c.5-.5 1.2-.7 1.9-.7h.3c1.1.1 2.1.6 2.8 1.4.8.7 1.3 1.7 1.4 2.8v.3c.1.7-.3 1.4-.8 1.9zM21.2 39.7h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM21.2 27.4h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM35 17c0-1.1-.9-2-2-2H13c-1.1 0-2 .9-2 2s.9 2 2 2h20c1.1 0 2-.9 2-2z" />
							<path class="title-icon-path" d="M50.8 40.3c-1.1 0-2 .9-2 2v17.4c-1.1-.5-2-1.1-2.8-1.8-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2C30 59.2 28.3 60 26.4 60s-3.6-.9-4.8-2.2c-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2c-.8.8-1.7 1.4-2.7 1.8H4V4h46.8c1.1 0 2-.9 2-2s-.9-2-2-2H2C.9 0 0 .9 0 2v60c0 1.1.9 2 2 2 2.9 0 5.6-1.3 7.4-3.2 1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2s5.6-1.2 7.4-3.2c1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2 1.1 0 2-.9 2-2V42.3c-.1-1.1-.9-2-2-2z" />
						</svg>
						<!-- // TITLE ICON -->
						<%=Resources.Fine_Header%></h1>
				</div>
			</div>

			<!-- STEPS -->
			<div id="form-win-steps" class="row justify-content-center p-4">
				<div class="col-md p-2 form-step"><%=Resources.Fine_SearchParameters%></div>
				<div class="col-md p-2 form-step"><%=Resources.Fine_Payment%></div>
				<div class="col-md form-step form-step-selected"><%=Resources.Fine_PaymentDone%></div>
			</div>


<%
if (ViewData["Result"] != null) {        
    dynamic j = Json.Decode(ViewData["Result"].ToString());    
    if (j.result != null && j.result == "succeeded")
    {
%>
			<!-- FORM INTRO -->
			<div id="form-win-intro" class="row p-2">
				<div class="col-12 my-4">
					<p class="text-center">
						<img src="../Content/img/2020/check-done.png" alt="done!"> &nbsp;
						<%=Resources.Fine_TicketPaymentOK%></p>
				</div>
			</div>

			<!-- FORM CONTENT --->
			<!-- TABLE RESUME-->
			<div class="row justify-content-center p-4 table-responsive-md">
				<table id="table-resume-list" class="table  ">
					<thead>
						<tr>
							<th scope="col"><%=Resources.Fine_TicketNumber%></th>
							<th scope="col"><%=Resources.Fine_Plate%></th>
							<th scope="col">&nbsp;</th>
							<th scope="col"><%=Resources.Account_Invoice_PDF_Amount %></th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<th scope="row">
								<%=Session["TicketNumber"]%>
							</th>
							<td><% =(j.PayTickets_Plate != null ? j.PayTickets_Plate.ToString() : "") %></td>
							<td>&nbsp;</td>
                            <% if (j.PayTickets_AmountCurrencyIsoCode == "$")
                               { %>
							<td class="text-right"><% =(j.PayTickets_Total != null ? string.Format("{1} {0:0.00}", Convert.ToDouble(j.PayTickets_Total) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                            <% } else { %>
							<td class="text-right"><% =(j.PayTickets_Total != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_Total) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                            <% } %>
						</tr>
					</tbody>
					<tfoot>
						<td colspan="2"></td>
						<td colspan="2" class="p-0">
							<!--	TABLE TOTALS 	//	-->
							<table id="table-resume-totals" class="table">
								<tbody>
									<tr>
										<td><%=Resources.Fine_FEE%>:</td>
                                        <% if (j.PayTickets_AmountCurrencyIsoCode == "$")
                                           { %>
										<td class="text-right"><% =(j.PayTickets_QFEE != null ? string.Format("{1} {0:0.00}", Convert.ToDouble(j.PayTickets_QFEE) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } else { %>
                                        <td class="text-right"><% =(j.PayTickets_QFEE != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_QFEE) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } %>
									</tr>
									<tr>
										<td><%=Resources.Fine_PartialVAT1%>:</td>
                                        <% if (j.PayTickets_AmountCurrencyIsoCode == "$")
                                           { %>
										<td class="text-right"><% =(j.PayTickets_QVAT != null ? string.Format("{1} {0:0.00}", Convert.ToDouble(j.PayTickets_QVAT) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } else { %>
										<td class="text-right"><% =(j.PayTickets_QVAT != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_QVAT) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } %>
									</tr>
								</tbody>
								<tfoot>
									<tr>
										<td><%=Resources.Fine_Amount%>:</td>
                                        <% if (j.PayTickets_AmountCurrencyIsoCode == "$")
                                           { %>
										<td class="text-right"><% =(j.PayTickets_TotalQuantity != null ? string.Format("{1} {0:0.00}", Convert.ToDouble(j.PayTickets_TotalQuantity) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } else { %>
										<td class="text-right"><% =(j.PayTickets_TotalQuantity != null ? string.Format("{0:0.00} {1}", Convert.ToDouble(j.PayTickets_TotalQuantity) / 100, j.PayTickets_AmountCurrencyIsoCode) : "") %></td>
                                        <% } %>
									</tr>
								</tfoot>
							</table>
						</td><!-- //#table-resume totals -->
					</tfoot>
				</table>
			</div><!-- // # form-table-resume -->
			<!-- FORM RESUME TEXT -->
			<div id="form-win-resume" class="row p-2">
				<div class="col-12">
					<p class="text-center"><%=Resources.Fine_ProvideInfo %></p>
				</div>
				<!-- Force next columns to break to new line -->
				<div class="w-100"></div>
                <div class="col-12 text-center">
                    <% 
                    using (Html.BeginForm("BSRedsysResult", "Fine", FormMethod.Post))
                    { 
                    %>
					<input class="btn btn-secondary w-25" name="submitButton" type="submit" value="<%=Resources.Fine_Close%>">
                    <% 
                    }
                    %>
					<hr class="w-75 my-5">
				</div>
			</div>
			
            <% if (Session["strRechargeEmailSubject"] != null && Session["strRechargeEmailBody"] != null) { %><iframe src="SuccessMail" style="width:100%; height:500px; overflow:hidden; border:none;" scrolling="no"></iframe><% } %>
    <%        
    }
    else 
    { 
    %>
			<!-- FORM INTRO -->
			<div id="form-win-intro" class="row p-2">
				<div class="col-12 my-4">
					<p class="text-center">

						<img src="../Content/img/2020/check-error.png" alt="error!"> &nbsp;
						<%=Resources.Fine_TicketPaymentFailureText%>
					</p>
				</div>
				<div class="w-100"></div>
				<div class="col-12 text-center">
                    <% using (Html.BeginForm("BSRedsysResult", "Fine", FormMethod.Post))
                    { 
                    %>   
                    <input class="btn btn-secondary w-25"  name="submitButton" type="submit" value="<%=Resources.Fine_SearchButton%>" "/>
                    <% 
                    }
                    %>
				</div>
            </div>
<%        
    }
}
%>
        </div>
    </div>
</div>
<script type="text/javascript">
    history.pushState(null, null, location.href);
    window.onpopstate = function () {
        history.go(1);
    };
</script>
</asp:Content>
