<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Blinkay.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.FineModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!-- NEW FORM -->
<div id="page-content">
    <!-- CONTENT CONTAINER HD -->
    <%-- <div > --%>
        <div id="form-win-container" class="container-xl">

            <!-- WINDOW TITTLE	-->
            <div id="form-title" class="row justify-content-center  ">
                <div id="form-win-header" class="col">

                    <h1 class="title-portal my-3 my-lg-4">
                        <!-- TITLE ICON // -->
                        <svg version="1.1" id="title-icon" xmlns="http://www.w3.org/2000/svg" x="0" y="0" viewbox="0 0 64 64" xml:space="preserve">

                            <path class="title-icon-path" d="M61.4 11.4C60 10 58.1 9.1 56 8.9h-.8c-1.8 0-3.5.7-4.6 1.9l-4.4 4.5-17.1 17.1c-.4.4-.6.9-.6 1.4v8.4c0 1.1.9 2 2 2h8.6c.5 0 1-.2 1.4-.6l18.4-18.2 2.9-3c1.4-1.2 2.2-3 2.2-4.9v-.8c-.2-2.1-1.2-3.9-2.6-5.3zM38.3 40.3h-5.8v-5.6l16.6-16.6 5.7 5.9-16.5 16.3zm20.9-21L57.6 21 52 15.2l1.6-1.6c.5-.5 1.2-.7 1.9-.7h.3c1.1.1 2.1.6 2.8 1.4.8.7 1.3 1.7 1.4 2.8v.3c.1.7-.3 1.4-.8 1.9zM21.2 39.7h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM21.2 27.4h-8c-1.1 0-2 .9-2 2s.9 2 2 2h8c1.1 0 2-.9 2-2s-.9-2-2-2zM35 17c0-1.1-.9-2-2-2H13c-1.1 0-2 .9-2 2s.9 2 2 2h20c1.1 0 2-.9 2-2z" />
                            <path class="title-icon-path" d="M50.8 40.3c-1.1 0-2 .9-2 2v17.4c-1.1-.5-2-1.1-2.8-1.8-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2C30 59.2 28.3 60 26.4 60s-3.6-.9-4.8-2.2c-1.9-2-4.5-3.2-7.4-3.2s-5.6 1.2-7.4 3.2c-.8.8-1.7 1.4-2.7 1.8H4V4h46.8c1.1 0 2-.9 2-2s-.9-2-2-2H2C.9 0 0 .9 0 2v60c0 1.1.9 2 2 2 2.9 0 5.6-1.3 7.4-3.2 1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2s5.6-1.2 7.4-3.2c1.2-1.4 2.9-2.2 4.8-2.2s3.6.9 4.8 2.2c1.9 2 4.5 3.2 7.4 3.2 1.1 0 2-.9 2-2V42.3c-.1-1.1-.9-2-2-2z" />
                        </svg>
                        <!-- // TITLE ICON -->
                        <!-- TITLE TEXT -->
                        <%=Resources.Fine_Header%>
                    </h1>
                </div>
            </div>

            <!-- STEPS -->
            <div id="form-win-steps" class="row">
                <div class="col form-step form-step-selected"><%=Resources.Fine_SearchParameters%></div>
                <div class="col form-step"><%=Resources.Fine_Payment%></div>
                <div class="col form-step"><%=Resources.Fine_PaymentDone%></div>
            </div>

            <!-- FORM INTRO -->
            <div id="form-win-intro" class="row">
                <div class="col">
                    <p class="text-center m-0 p-0"><%=Resources.Fine_Fill %></p>
                </div>
            </div>

            <!-- FORM CONTENT --->
            <% using (Html.BeginForm("Details", "Fine", FormMethod.Post, new { @role = "form",@class ="form-content" })) { 
                if (ViewData["OpId"] == null && ViewData["InstallationId"] == null) { %>
                        <div class="row justify-content-center">                    
                            <div class="col col-lg-8  form-group">
                                <label for="City" class="col-form-label d-none d-md-block"><%=Resources.Fine_City %></label>
                                <select id="City" size="1" class="form-control form-control-md" onchange="showValue(this.value)" name="InstallationId" <% = (Model.ForceInstallationId != null && Model.BlockInstallationCombo == true) ? "disabled" : "" %>>
                                    <%= InstallationDropDownHelper.InstallationDropDown (
                                            (Array)ViewData["InstallationsOptionList"], 
                                            "../Content/img/banderas/", 
                                            (Model.ForceInstallationId != null ? Model.ForceInstallationId.ToString() : (String) Resources.DefaultInstallationCode)
                                        )%>
                                </select>
                            </div>
                        </div>
                <% } // END If no installation or operator specified
                %>

                <!-- TICKET NUMBER -->
                <div class="row justify-content-center">
                    <div class="col col-lg-8  form-group label2placeholder">
                        <%=Html.LabelFor(fin => fin.TicketNumber, new { @id="lblTicketNumber", @class= "lbl-switch col-form-label  d-none d-md-block" })%>
                        <%= Html.TextBoxFor(fin => fin.TicketNumber, new {
                            @placeholder = Resources.Fine_TicketNumberPlaceholder,
                            @required="required",
                            @type="text",
                            @class="form-control"})
                        %>
                        <small class="form-text text-black-50"><%=Resources.Fine_TicketNumberTip %></small>
                    </div>
                </div>

                <%-- if OpId & InstallationId     --%>
                <% if (ViewData["OpId"] != null || ViewData["InstallationId"] != null) { %>
                    <!-- LICENSE PLATE	-->
                    <div class="row justify-content-center">
                        <div class="col col-lg-8  form-group label2placeholder">
                            <%=Html.LabelFor(fin => fin.Plate, new { @id="lblPlate", @class= "lbl-switch col-form-label  d-none d-md-block" })%>
                            <%= Html.TextBoxFor(fin => fin.Plate, new {
                                @placeholder = Resources.Fine_PlatePlaceholder,
                                @required="required",
                                @type="text",
                                @class="form-control form-control-md"})
                            %>
                            <small class="form-text text-black-50"><%=Resources.Fine_PlateTip %></small>
                        </div><!-- // .form-group -->
                    </div>
                <% } %>
                <%-- // end if OpId & InstallationId --%>

                <%-- if OpId & InstallationId = null  // --%>
                <% if (ViewData["OpId"] == null && ViewData["InstallationId"] == null) { %>
                    <div class="row justify-content-center ">
                        <div class="col-12 col-lg-4 form-group">
                            <%=Html.LabelFor(fin => fin.Email, new { @class= "col-form-label d-none d-hd-block" })%>
                            <%= Html.TextBoxFor(fin => fin.Email, new {
                                @placeholder = Resources.FineModel_WriteYourEmail,
                                @required="required",
                                @type="email",
                                @class="form-control form-control-md" })
                            %>
                            <small class="form-text text-black-50"><%=Resources.Fine_EmailTip %></small>
                        </div><!-- // .form-group -->
                        <div class="col-12 col-lg-4 form-group">
                            <%=Html.LabelFor(fin => fin.ConfirmEmail, new { @class= "col-form-label d-none d-hd-block" })%>
                            <%= Html.TextBoxFor(fin => fin.ConfirmEmail, new {
                                @placeholder = Resources.FineModel_WriteYourConfirmEmail,
                                @required="required",
                                @type="email",
                                @class="form-control form-control-md" })
                            %>
                            <small class="form-text text-black-50"><%=Resources.Fine_ConfirmEmailTip %></small>
                        </div><!-- // .form-group -->
                    </div>
                <% } %>
                <%-- // END if OpId & InstallationId = null --%>

                <!-- FORM AGREE-->
                <div id="form-win-agree" class="row  justify-content-center">
                    <div class=" col-lg-8 col-sm-12 form-group form-check" id="form-win-agree-int">
                        <input autocomplete="off" class="form-check-input " type="checkbox" value="" id="termsAndCond" required pattern="" title="<%=Resources.Fine_Agree %>">
                        <label class="form-check-label" for="termsAndCond">
                            <%=Resources.Fine_AgreeText %>							
                        </label>
                        <div class="invalid-feedback">
                            <%=Resources.Fine_Agree %>
                        </div>
                    </div><!-- // .form-group.form-check -->
                </div><!-- //#form-win-agree.row -->

            <!--//		TABLE TOTALS-->
            <% 
            
            %>

                <!-- FORM SUBMIT BUTTONS	-->
                <div class="row justify-content-center align-items-center">
                    <div class="col-12 col-lg-4  d-none d-lg-inline-block">
                        <button class="btn btn-block btn-secondary p-3" type="reset"><%=Resources.Fine_ClearForm %></button>
                    </div>
                    <div class="col-12 col-lg-4">
                        <button class="btn btn-block btn-primary p-3" type="submit"><%=Resources.Fine_SearchButton %></button>
                    </div>
                </div>
                <%  if (Model.ForceInstallationId != null) { %>
                        <input class="form-control" data-val="true" id="ForceInstallationId" name="ForceInstallationId" value="<%=Model.ForceInstallationId%>" type="hidden" />
                <%  } // ForceInstallationId  %>

                <%  if (Model.ForceTicketNumber != "") {
                        Model.TicketNumber = Model.ForceTicketNumber;
                        %>
                            <script type="text/javascript">
                                $(document).ready(function () {
                                    $("#TicketNumber").val("<%=Model.ForceTicketNumber%>");
                                    $("#TicketNumber").prop("readonly", "readonly");
                                    $("#TicketNumber").trigger("change");
                                    // TGA@Blinkay
                                    $("#TicketNumber").attr("placeholder", "Type fine");
                                });
                            </script>
                <%  } // ForceTicketNumber  %>
                <%=Html.HiddenFor(fin => fin.InstallationList) %>
                <%=Html.HiddenFor(fin => fin.StandardInstallationList) %>

            <% } // End Form %>
            <%-- // END FORM CONTENT --%>

            <!-- INFO DISCLAIMER -->
            <div id="form-payment--disclaimer" class="text-center mb-4">
                <hr class="w-75 d-none d-hd-block">
                <div class="row my-0 my-lg-2 justify-content-center align-items-center">
                    <div class=" col-12 col-lg-4 align-items-center justify-content-center  ">
                        <a href="https://www.pcisecuritystandards.org" target="_blank">
                            <img src="../Content/img/2020/card/PCI-DDS-Cert.png" class="PCI-DDS-Cert" alt="<%=Resources.Fine_PCI %>" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>">
                        </a>
                    </div>
                    <div class="col-12 col-lg-4 align-items-center justify-content-center">
                        <p class="m-0 small text-center align-middle "><span class="align-middle"><%=Resources.Fine_AcceptedPayments %></span>
                            <i class="fab fa-cc-visa align-middle" data-toggle="tooltip" data-placement="bottom" title="Visa"></i>
                            <i class="fab fa-cc-mastercard align-middle" data-toggle="tooltip" data-placement="bottom" title="Master Card"></i>
                            <% if (Session["PayPal_Enabled"] != null && (bool)Session["PayPal_Enabled"] == true) { %>
                            <i class="fab fa-cc-paypal align-middle" data-toggle="tooltip" data-placement="bottom" title="PayPal"></i>						
                            <% } %>
                        </p>
                    </div>
                </div>
            </div><!-- // .form-payment--disclaimer -->
        </div><!-- // #form-win-container-->
    <%-- </div><!-- // .container-xl --> --%>
</div><!--// #page-content  -->
<%-- // TGA@Blinkay --%>

<script type="text/javascript">
    $(document).ready(function () {
        let strLabelTicketNumber = $("#lblTicketNumber").text();   
        let strLabelPlate = $("#lblPlate").text();   
        if( !$("#TicketNumber").attr("placeholder") )
            $("#TicketNumber").attr("placeholder", strLabelTicketNumber );
        if( !$("#Plate").attr("placeholder") )
            $("#Plate").attr("placeholder", strLabelPlate );
    });
</script> 
</asp:Content>