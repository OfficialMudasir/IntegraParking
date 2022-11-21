
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.AppealModel>" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <link href="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"]%>/Content/Appeal_CSS/Welcome_Appeal.css" rel="stylesheet" />


    <div class="container">
        <div class ="form__wrap">
          <div class="form">
            <div class="language">
              <h3 class="mb-4 title"><%=Resources.Appeal_Welcome%></h3>
        

            </div>
            <p><%=Resources.Appeal_TheInformationThatYouSubmit%></p>

            <p> <%=Resources.Appeal_PleaseEnterInTheInformation %></p>
              <br/>
            <div class="formGroup">
                 <% using (Html.BeginForm("Details", "Appeal", FormMethod.Post, new { @role = "form", @class = "form-content", @onsubmit = "this.querySelectorAll('#submitButton').forEach((element) => { element.disabled = true; element.innerText='Searching please wait...';})" }))
                     {
                         
                         %>

                        <!-- TICKET NUMBER -->
                        <div class="form-group">

                            <%= Html.TextBoxFor(fin => fin.TicketNumber, new {
                                    @placeholder = Resources.Fine_TicketNumberPlaceholder,
                                    @required="required",
                                    @type="text",
                                    @id="ticketnumber",
                                    @autocomplete="off",
                                    @class="form-control p-0"})
                                %>
                            <label class="form-control-placeholder p-0" for="">
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="16"
                                    height="16"
                                    fill="currentColor"
                                    class="bi bi-ticket-detailed"
                                    viewBox="0 0 16 16">
                                    <path d="M4 5.5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5Zm0 5a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5ZM5 7a1 1 0 0 0 0 2h6a1 1 0 1 0 0-2H5Z" />
                                    <path d="M0 4.5A1.5 1.5 0 0 1 1.5 3h13A1.5 1.5 0 0 1 16 4.5V6a.5.5 0 0 1-.5.5 1.5 1.5 0 0 0 0 3 .5.5 0 0 1 .5.5v1.5a1.5 1.5 0 0 1-1.5 1.5h-13A1.5 1.5 0 0 1 0 11.5V10a.5.5 0 0 1 .5-.5 1.5 1.5 0 1 0 0-3A.5.5 0 0 1 0 6V4.5ZM1.5 4a.5.5 0 0 0-.5.5v1.05a2.5 2.5 0 0 1 0 4.9v1.05a.5.5 0 0 0 .5.5h13a.5.5 0 0 0 .5-.5v-1.05a2.5 2.5 0 0 1 0-4.9V4.5a.5.5 0 0 0-.5-.5h-13Z" />
                                </svg>
                                <span>Ticket Number</span>
                            </label>
                                
                            <%= Html.ValidationMessageFor(model => model.TicketNumber, "", new { @class = "text-danger" }) %>

                                
                        </div>
                       
                        <br/>

                        <%-- if OpId & InstallationId     --%>
                        <% if (ViewData["OpId"] != null || ViewData["InstallationId"] != null) { %>
                            <!-- LICENSE PLATE	-->

                               <div class="form-group">

                                   <%= Html.TextBoxFor(fin => fin.Plate, new {
                                        @placeholder = Resources.Fine_PlatePlaceholder,
                                        @required="required",
                                        @type="text",
                                        @id="platenumber",
                                        @autocomplete="off",
                                        @class="form-control p-0"})
                                    %>
                                    <label class="form-control-placeholder p-0" for="email">

                                        <%--<img src="https://img.icons8.com/ios/344/sedan.png" style="height: 21px; width: 19px;" />--%>
                                        <%--<svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="16"
                                            height="16"
                                            fill="currentColor"
                                            class="bi bi-headset"
                                            viewBox="0 0 16 16">
                                            <path d="M8 1a5 5 0 0 0-5 5v1h1a1 1 0 0 1 1 1v3a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1V6a6 6 0 1 1 12 0v6a2.5 2.5 0 0 1-2.5 2.5H9.366a1 1 0 0 1-.866.5h-1a1 1 0 1 1 0-2h1a1 1 0 0 1 .866.5H11.5A1.5 1.5 0 0 0 13 12h-1a1 1 0 0 1-1-1V8a1 1 0 0 1 1-1h1V6a5 5 0 0 0-5-5z" />
                                        </svg>--%>

                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-car-front" viewBox="0 0 16 16">
                                          <path d="M4 9a1 1 0 1 1-2 0 1 1 0 0 1 2 0Zm10 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0ZM6 8a1 1 0 0 0 0 2h4a1 1 0 1 0 0-2H6ZM4.862 4.276 3.906 6.19a.51.51 0 0 0 .497.731c.91-.073 2.35-.17 3.597-.17 1.247 0 2.688.097 3.597.17a.51.51 0 0 0 .497-.731l-.956-1.913A.5.5 0 0 0 10.691 4H5.309a.5.5 0 0 0-.447.276Z"/>
                                          <path fill-rule="evenodd" d="M2.52 3.515A2.5 2.5 0 0 1 4.82 2h6.362c1 0 1.904.596 2.298 1.515l.792 1.848c.075.175.21.319.38.404.5.25.855.715.965 1.262l.335 1.679c.033.161.049.325.049.49v.413c0 .814-.39 1.543-1 1.997V13.5a.5.5 0 0 1-.5.5h-2a.5.5 0 0 1-.5-.5v-1.338c-1.292.048-2.745.088-4 .088s-2.708-.04-4-.088V13.5a.5.5 0 0 1-.5.5h-2a.5.5 0 0 1-.5-.5v-1.892c-.61-.454-1-1.183-1-1.997v-.413a2.5 2.5 0 0 1 .049-.49l.335-1.68c.11-.546.465-1.012.964-1.261a.807.807 0 0 0 .381-.404l.792-1.848ZM4.82 3a1.5 1.5 0 0 0-1.379.91l-.792 1.847a1.8 1.8 0 0 1-.853.904.807.807 0 0 0-.43.564L1.03 8.904a1.5 1.5 0 0 0-.03.294v.413c0 .796.62 1.448 1.408 1.484 1.555.07 3.786.155 5.592.155 1.806 0 4.037-.084 5.592-.155A1.479 1.479 0 0 0 15 9.611v-.413c0-.099-.01-.197-.03-.294l-.335-1.68a.807.807 0 0 0-.43-.563 1.807 1.807 0 0 1-.853-.904l-.792-1.848A1.5 1.5 0 0 0 11.18 3H4.82Z"/>
                                        </svg>

                                        
                                        <span>Plate Number</span>
                                    </label>
                                    <%= Html.ValidationMessageFor(model => model.Plate, "", new { @class = "text-danger" }) %>

                                </div>
                            
                        <% } %>
                        <%-- // end if OpId & InstallationId --%>

                       

                        <div class="contestText">
                            

                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-exclamation-triangle" viewBox="0 0 16 16">
                              <path d="M7.938 2.016A.13.13 0 0 1 8.002 2a.13.13 0 0 1 .063.016.146.146 0 0 1 .054.057l6.857 11.667c.036.06.035.124.002.183a.163.163 0 0 1-.054.06.116.116 0 0 1-.066.017H1.146a.115.115 0 0 1-.066-.017.163.163 0 0 1-.054-.06.176.176 0 0 1 .002-.183L7.884 2.073a.147.147 0 0 1 .054-.057zm1.044-.45a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566z"/>
                              <path d="M7.002 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0zM7.1 5.995a.905.905 0 1 1 1.8 0l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995z"/>
                            </svg>
                            <%--<span>Each contested ticket must be submitted individually</span>--%>
                            <span><%=Resources.Appeal_EachContestedTicketMust %></span>
                      </div>
                
                      <button type="submit" id="submitButton" class="btn btn-block btn-primary p-3 signup" value="Appeal" disabled>
                        <%=Resources.Fine_SearchButton %>
                      </button>
                      

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
                                    <%} // ForceTicketNumber  
                                 %>
                        <%=Html.HiddenFor(fin => fin.InstallationList) %>
                        <%=Html.HiddenFor(fin => fin.StandardInstallationList) %>


                     <% } // End Form %>
                     
        
                
            </div>
        
          </div>
                <div class ="contact__details">
                 <div class="contact">
                    <p>
                     <%=Resources.Appeal_ImmediatelyUponYourSubmission %>
                    </p>
                    <p><%=Resources.Appeal_IfThisNoticeIsNotReceived%></p>
                      <a href="https://www.pcisecuritystandards.org" target="_blank">
                            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/2020/card/PCI-DDS-Cert.png" class="PCI-DDS-Cert" alt="<%=Resources.Fine_PCI %>" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>">
                        </a>
                    
            </div>
                    <div class="paymenttypes">
                        <p class="m-0 small text-center align-middle "><span class="align-middle"><%=Resources.Fine_AcceptedPayments %></span>
                        </p>
                        <img src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/credit/tarjetas.jpg" class="payimages" alt="<%=Resources.Fine_PCI %>" style="width:100px" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>"/>

                    </div>
              </div>

            </div>
            <!-- INFO DISCLAIMER -->
            
        </div>
    
    <style type="text/css">
        .errorSpan {
            display: none;
        }

        .loader {
            width: 20px;
            height: 20px;
            border: 5px solid #FFF;
            border-bottom-color: transparent;
            border-radius: 50%;
            display: inline-block;
            box-sizing: border-box;
            animation: rotation 1s linear infinite;
            margin-left: 15px;
        }

        @keyframes rotation {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            let strLabelTicketNumber = $("#lblTicketNumber").text();
            let strLabelPlate = $("#lblPlate").text();
            if (!$("#TicketNumber").attr("placeholder"))
                $("#TicketNumber").attr("placeholder", strLabelTicketNumber);
            if (!$("#Plate").attr("placeholder"))
                $("#Plate").attr("placeholder", strLabelPlate);
        });
        let tickNumText = "";
        let PlateText = "";
        $("#ticketnumber").keyup(function (e) {
            tickNumText = e.target.value;
            if (tickNumText != "" && PlateText != "" && tickNumText.length > 3) {
                $("#submitButton").removeAttr("disabled", "");
            }
            else {
                $("#submitButton").prop('disabled', true);

            }
        });
        $("#platenumber").keyup(function (e) {
            PlateText = e.target.value;
            if (tickNumText != "" && PlateText != "" && PlateText.length > 3) {
                $("#submitButton").removeAttr("disabled", "");
            }
            else {
                $("#submitButton").prop('disabled', true);

            }
        });
        

    </script> 
</asp:Content>