<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.AppealReturnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PersonalInformation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <link href="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/Appeal_CSS/PersonalInformation.css" rel="stylesheet" />

    <%-- New Html Changes --%>

    <div class="mainTicketContent">
	            <div class="ticket__wrap">
		            <h2 class="heading__ticket1">Payment</h2>
		            <p class="mb40 fs-13 mt-0 ">Items marked with a asterisk (*) are required <br>
		            The information submitted here with will be remotely by a Parking Court judge for purpose of rendering a decision with respect to the ticket.</p>
                </div>
                <ul class="progressbar">
		            <li class="active">Ticket(s)</li>
		            <li class="active">Personal information</li>
		            <li id="case">Payment</li>
	            </ul>
             
              <%--<%using (Html.BeginForm("SaveAppealInformation", "Appeal", FormMethod.Post, new { @role = "form", @class = "form-content" }))--%>
             <%using (Html.BeginForm(new { @role = "form", @class = "form-content"}))
                 {%>
                
                        
                             <div class="ticket__wrap">
                                 <h2 class="heading__ticket">Tickets to pay</h2>
                                 <%-- Hidden Details --%>

                                <div class="ticketDetails">
		
		                            <div class="formbox filledForm">
			                            <div class="col-12 mb20">
                                            
                                            <div class="ticket__inner" style ="justify-content:center; padding-bottom: 0px;">
                                                <div class="ticket__check nullticket" style="padding-left:15px;">
                                                    
                                                    <div class="alert" style="border-radius:26px;background-color:#034fc7; color:#fff" >
                                                        
                                                        <h3 style="color:#fff">Payment module is in progress</h3>
                                                    </div>
                                                </div>
                                            </div>
			                            </div>
		
		                            </div>
		
                                </div>
                                 

                                 
                                    
                                             <%-- Personal Information --%>
                                             <div class="ticket__wrap mt-60" id="personalInformation">
                                                 <h2 class="heading__ticket">Personal Information</h2>
                                                 <p class="mb40 fs-13 mt-0 ">1) The Pittsburg Parking Court's email notification process is optional and voluntary. A ticketholder has the option to recive notification of adjusdiction either (a) via first class mall, </p>


                                                 <div class="formbox">
                                                     <div class="col-12 mb20">
                                                         <label>First Name* </label>
                                                         <%= Html.TextBoxFor(fin => fin.AppealData.FirstName, new
                                                            {
                                                                @placeholder = "@Jhon",
                                                                //@required = "required",
                                                                @type = "text",
                                                                @id = "firstname",
                                                                @autocomplete="off",
                                                                @class = "form-control p-0"
                                                            })
                                                        %>
                                                     </div>
                                                     <div class="col-12 mb20 ">
                                                         <label>Last Name* </label>

                                                         <%= Html.TextBoxFor(fin => fin.AppealData.LastName, new
{
    @placeholder = "@Cenea",
    //@required = "required",
    @type = "text",
    @id = "lastname",
    @autocomplete="off",
    @class = "form-control p-0"
})
                                                        %>
                                                     </div>

                                                    

                                                     <div class="col-12 mb20">
                                                         <label>Email* </label>
                                                          <%= Html.TextBoxFor(fin => fin.AppealData.Email, new
{
    @placeholder = "example@gmail.com",
    //@required = "required",
    @type = "email",
    @id = "email",
    @name = "email",
    @autocomplete = "off",
    @class = "form-control p-0"
})
                                                        %>
                                                     </div>
                                                     <div class="col-12 mb20">
                                                         <label>Telephone* </label>
                                                         <input type="tel" class="form-control p-0"  autocomplete = "off" placeholder="Telephone"/>
                                                         

                                                     </div>
                                                     
                                                     <div class="col-12  mb20 btns" style="justify-content: end">

                                                         <button class="btn btn-primary btnContinue">Proceed to Pay</button>
                                                     </div>
                                                 </div>

                                     

                                             </div>

                                            
                                            
                                            
                                    

                             </div>

                          
                        
                       
             <% } %>
            
         </div>

    
                    <br/>
                    <br/>
                    <div class="contact__details">
                        <div class="contact">
                            <p>
                                Immediately upon your submission you will receive email confirmation
                                  that your on-line hearing request was received.
                            </p>
                            <p>If this notice is not received please call 412-560-7222</p><br/>
                            <a href="https://www.pcisecuritystandards.org" target="_blank">
                                <img src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/2020/card/PCI-DDS-Cert.png" class="PCI-DDS-Cert" alt="<%=Resources.Fine_PCI %>" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>">
                            </a>

                        </div>

                        <div class="paymenttypes">
                            <p class="m-0 small text-center align-middle ">
                                <span class="align-middle"><%=Resources.Fine_AcceptedPayments %></span>
                            </p>

                            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/credit/tarjetas.jpg" class="payimages" alt="<%=Resources.Fine_PCI %>" style="width:100px" data-toggle="tooltip" data-placement="bottom" title="<%=Resources.Fine_PCI %>" />

                        </div>
                    </div>


</asp:Content>
