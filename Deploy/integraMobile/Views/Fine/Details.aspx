<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Blinkay.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.FineModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- NEW FORM -->
	<div id="page-content">
	    <!-- CONTENT CONTAINER HD -->
		<div id="form-win-container" class=" container-xl">

			<!-- WINDOW TITTLE	-->
			<div id="form-title" class="row justify-content-center">
				<div id="form-win-header" class="col">

					<h1 class="title-portal my-3 my-lg-4">
						<!-- TITLE ICON // -->
						<svg version="1.1" id="title-icon" xmlns="http://www.w3.org/2000/svg" x="0" y="0" viewbox="0 0 64 64" xml:space="preserve">

							<path class="title-icon-path" d="M61.4 11.4C60 10 58.1 9.1 56 8.9h-.8c-1.8 0-3.5.7-4.6 1.9l-4.4 4.5-17.1 17.1c-.4.4-.6.9-.6 1.4v8.4c0 1.1.9 2 2 2h8.6c.5 0 1-.2 1.4-.6l18.4-18.2 2.9-3c1.4-1.2 2.2-3 2.2-4.9v-.8c-.2-2.1-1.2-3.9-2.6-5.3zM38.3 40.3h-5.8v-5.6l16.6-16.6 5.7 5.9-16.5 16.3zm20.9-21L57.6 21 52 15.2l1.6-1.6c.5-.5 1.2-.7 1.9-.7h.3c1.1.1 2.1.6 2.8 1.4.8.7 1.3 1.7 1.4 2.8v.3c.1.7-.3 1.4-.8 1.9zM21.2 39.7h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM21.2 27.4h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM35 17c0-1.1-.9-2-2-2H13c-1.1 0-2 .9-2 2s.9 2 2 2h20c1.1 0 2-.9 2-2z" />
							<path class="title-icon-path" d="M50.8 40.3c-1.1 0-2 .9-2 2v17.4c-1.1-.5-2-1.1-2.8-1.8-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2C30 59.2 28.3 60 26.4 60s-3.6-.9-4.8-2.2c-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2c-.8.8-1.7 1.4-2.7 1.8H4V4h46.8c1.1 0 2-.9 2-2s-.9-2-2-2H2C.9 0 0 .9 0 2v60c0 1.1.9 2 2 2 2.9 0 5.6-1.3 7.4-3.2 1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2s5.6-1.2 7.4-3.2c1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2 1.1 0 2-.9 2-2V42.3c-.1-1.1-.9-2-2-2z" />
						</svg>
						<!-- // TITLE ICON -->
						<%=Resources.Fine_Header%></h1>
				</div>
			</div>

			<% 
			string modelErrors = string.Empty;
			foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState)
			{
				foreach (ModelError modelError in keyValuePair.Value.Errors)
				{
					modelErrors += "<p><img src='../Content/img/2020/check-error.png'>&nbsp;" + modelError.ErrorMessage + "</p>";
				}
			}
			%>

			<!-- STEPS -->
			<div id="form-win-steps" class="row">
				<div class="col form-step"><%=Resources.Fine_SearchParameters%></div>
				<div class="col form-step form-step-selected"><%=Resources.Fine_Payment%></div>
				<div class="col form-step"><%=Resources.Fine_PaymentDone%></div>
			</div>
			<%-- // END STEPS --%>

            <%-- <!-- FORM WIN INTRO --> --%>
			<%-- <div id="form-win-intro" class="row"> --%>
				<%-- if else  not modelErrors --%>
				<% if (!string.IsNullOrEmpty(modelErrors)) { %>
					<div id="form-win-intro " class="row form-error">

						<div class="row justify-content-center align-items-center">
							<p class="col col-hd-8 text-center my-3">								
								<% =modelErrors %>
							</p>
						</div><%-- row modelError --%>
						<%-- <div class="w-100"></div> --%>
						<div class="row justify-content-center align-items-center">
							<% using ( Html.BeginForm("Details", "Fine", FormMethod.Post, new { @class ="col"}) ) { %>   
							<input class="btn btn-secondary w-50 p-3"  name="submitButton" type="submit" value="<%=Resources.Fine_SearchButton%>">
							<% } %>
						</div>
					</div><%-- // form-win-intro.form-error --%>
				<% } else { 
					if (Model.Result > 0 && string.IsNullOrEmpty(modelErrors)) { %>
						<div id="form-win-intro" class="row form-results">
							<div class="col-12 justify-content-center align-items-center">
								<p class="text-center small my-3"><% =Resources.Fine_NonRefundable %></p>
								<p class="text-center small  m-0" id="CustomInfo"></p>
							</div>
						</div>
					<% } // end if Model.Result
				} // end if else  not modelErrors %>
				<%-- end if else  not modelErrors --%>
			<%-- </div><!----// .form-win-intro --> --%>

			<%-- INI FORM-CONTENT // --%>
				<% if (Model.Result > 0 && string.IsNullOrEmpty(modelErrors)) { %>
					<% using (Html.BeginForm("Payment", "Fine", FormMethod.Post, new { @class ="form-content col-12 col-lg-8 p-0 align-self-center" })) { %>
									
						<!-- FORM CONTENT --->
						<%-- <div class="row"> --%>
							<%-- <div class="col"> --%>
								<!-- TABLE RESUME-->
								<div class=" table-responsive-md">
									<table id="table-resume-list" class="table small">
										<thead>
											<tr>
												<th scope="col"><%=Resources.Fine_TicketNumber %></th>
												<th scope="col"><%=Resources.Fine_Plate %></th>
												<th scope="col"><%=Resources.Account_Invoice_Date %></th>
												<th scope="col"><%=Resources.Account_Invoice_PDF_Amount %></th>
											</tr>
										</thead>
										<tbody>
											<tr>
												<th scope="row">
													<%=Model.TicketNumber %>
												</th>
												<td><%=Model.Plate %></td>
												<td><%=Model.TicketDate %></td>
												<% if (Model.AmountCurrencyIsoCode == "$") { %>
												<td class="text-right"><% =string.Format("{1} {0:0.00}", Convert.ToDouble(Model.Total) / 100, Model.AmountCurrencyIsoCode) %></td>
												<% } else { %>
												<td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(Model.Total) / 100, Model.AmountCurrencyIsoCode) %></td>
												<% } %>
											</tr>
										</tbody>
										<tfoot>
											<tr>
												<td colspan="2">

												</td>
												<td colspan="2" class="p-0">
													<!--	TABLE TOTALS 	//	-->
													<table id="table-resume-totals" class="table">
														<tbody>
															<tr>
																<td><%=Resources.Fine_FEE%>:</td>
																<% if (Model.AmountCurrencyIsoCode == "$") { %>
																<td class="text-right"><% =string.Format("{1} {0:0.00}", Convert.ToDouble(Model.QFEE) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } else { %>
																<td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(Model.QFEE) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } %>
															</tr>
															<tr>
																<td><%=Resources.Fine_PartialVAT1%>:</td>
																<% if (Model.AmountCurrencyIsoCode == "$") { %>
																<td class="text-right"><% =string.Format("{1} {0:0.00}", Convert.ToDouble(Model.QVAT) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } else { %>
																<td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(Model.QVAT) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } %>
															</tr>
														</tbody>
														<tfoot>
															<tr>
																<td><%=Resources.Fine_Amount%>:</td>
																<% if (Model.AmountCurrencyIsoCode == "$") { %>
																<td class="text-right"><% =string.Format("{1} {0:0.00}", Convert.ToDouble(Model.TotalQuantity) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } else { %>
																<td class="text-right"><% =string.Format("{0:0.00} {1}", Convert.ToDouble(Model.TotalQuantity) / 100, Model.AmountCurrencyIsoCode) %></td>
																<% } %>
															</tr>
														</tfoot>
													</table>

													<!--//		TABLE TOTALS-->


												</td><!-- //#table-resume totals -->
											</tr>
										</tfoot>
									</table>
								</div><!-- // # form-table-resume -->
							<%-- </div>// .col  --%>
						<%-- </div><!-- // .row --> --%>

						<!-- FORM SUBMIT BUTTONS	-->
						<div id="form-submit-buttons" class="row justify-content-center align-items-center">
							<%if (Model != null && Model.TotalQuantity!=0) { %>
								<div class="col col-lg-6">
									<button class="btn btn-block btn-primary p-3" name="submitButton" value="Credit" type="submit"><%: Resources.Fine_PayWithCreditCard %></button>
								</div>
							<%} else {%>
							    <div class="col col-lg-4  d-none d-lg-inline-block">
									<input class="btn btn-block btn-primary p-3" name="submitButton" type="submit" value="<%=Resources.Fine_Close%>">
								</div>
								<div class="col col-lg-4">
									<button class="btn btn-block btn-primary p-3" disabled type="submit"><%: Resources.Fine_PayWithCreditCard %></button>
								</div>
								
							<%} // if else Model.TotalQuantity %>

							<%if (Session["PaymentPayPalButton"] != null && !string.IsNullOrEmpty(Session["PaymentPayPalButton"].ToString())) 
                            { %>
                                <%if (Model != null && Model.TotalQuantity!=0) 
                                    { %>
								    <div class="col col-lg-4">
									    <button class="btn btn-block btn-info btn-paypal p-2" name="submitButton" value="PayPal" type="submit"><%: Resources.Fine_PayWith %> <b class="btn-paypal-icon">PayPal</b> </button>
								    </div>
                            <%} 
                              else 
                              {%>
                                <div class="col col-lg-4">
									    <button class="btn btn-block btn-info btn-paypal p-2" disabled value="PayPal" type="submit"><%: Resources.Fine_PayWith %> <b class="btn-paypal-icon">PayPal</b> </button>
								    </div>
                                <%} 
                              }%>
							 <%--// PaymentPayPalButton --%>

						</div> <%-- form-submit-buttons --%>
					<% } // Html.BeginForm %>
				<% } else if (string.IsNullOrEmpty(modelErrors)) { %>
					<!-- FORM CONTENT ALT -->
					<div class="form-content row justify-content-center content-search">
						<div class="d-flex flex-column justify-content-center ">
							<p class="text-center col col-lg-6 align-self-center my-5">								
								<%
								switch (Model.Result.ToString())
								{
									case "-5":
										if (Model.Email == ConfigurationManager.AppSettings["TICKET_PAYMENT_NON_USER_NO_EMAIL_PLACEHOLDER"].ToString())
										{
											Response.Write(Resources.Result_Error_Fine_Number_Not_Found_For_Plate);
										}
										else
										{
											Response.Write(Resources.Result_Error_Fine_Number_Not_Found);
										} 
										break;
									case "-6": 
										Response.Write(Resources.Result_Error_Fine_Type_Not_Payable);
										break;
									case "-7":
										Response.Write(Resources.Result_Error_Fine_Payment_Period_Expired);
										break;
									case "-8":
										Response.Write(Resources.Result_Error_Fine_Number_Already_Paid);
										break;
									default:
										Response.Write(Resources.Permits_Error_Result_Error_Generic);
										break;
								} // switch         
								%>
							</p>
							<%-- <div class="w-100"></div> --%>
							<div class="text-center col col-lg-6 align-self-center">
								<% using ( Html.BeginForm("Details", "Fine", FormMethod.Post,  new { @class ="btn-back"}) ) { %>   
								<input class="btn btn-primary btn-block p-3"  name="submitButton" type="submit" value="<%=Resources.Fine_SearchButton%>">
								<% } %>
							</div><%-- text-center --%>
						</div>
					</div><!-- // .form-content row justify-content-center content-search --> 
					<%--  END FORM CONTENT ALT  --%>
				<% } // end if else modelErrors  %>
			<%-- // END  FORM-CONTENT --%>


			<!-- INFO DISCLAIMER -->
			<div id="form-payment--disclaimer" class="col  text-center mb-4">
				<hr class="w-75 d-none d-hd-block">
				<div class="row justify-content-center align-items-center">
					<div class="col-12 col-lg-4 align-items-center justify-content-center">
						<a href="https://www.pcisecuritystandards.org" target="_blank">
							<img class="PCI-DDS-Cert"  src="../Content/img/2020/card/PCI-DDS-Cert.png" alt="<%=Resources.Fine_PCI %>" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>">
						</a>
					</div><%-- col  --%>
					<div class="col-12 col-lg-4 align-items-center justify-content-center">
						<p class="m-0 small text-center align-middle"><span class="align-middle"><%=Resources.Fine_AcceptedPayments %></span>
							<i class="fab fa-cc-visa align-middle" data-toggle="tooltip" data-placement="bottom" title="Visa"></i>
							<i class="fab fa-cc-mastercard align-middle" data-toggle="tooltip" data-placement="bottom" title="Master Card"></i>
							<% if (Session["PayPal_Enabled"] != null && (bool)Session["PayPal_Enabled"] == true) { %>
							<i class="fab fa-cc-paypal align-middle" data-toggle="tooltip" data-placement="bottom" title="PayPal"></i>						
							<% } %>
						</p>
					</div><%-- col  --%>
				</div><%-- row  --%>
			</div><!-- // .form-payment--disclaimer -->
		</div><!-- // #form-win-container-->
	</div>
</asp:Content>
