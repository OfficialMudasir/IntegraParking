<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.AppealReturnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    PersonalInformation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <link href="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/Appeal_CSS/PersonalInformation.css" rel="stylesheet" />

    <%-- New Html Changes --%>

    <div class="mainTicketContent">
	            <div class="ticket__wrap">
		            <%--<h2 class="heading__ticket1">Contest Ticket</h2>--%>
		            <h2 class="heading__ticket1"><%=Resources.PersonalInformation_ContestTicket%></h2>
		            <p class="mb40 fs-13 mt-0 ">
		            <%--The information submitted here with will be remotely by a Parking Court judge for purpose of rendering a decision with respect to the ticket.--%>
                        <%=Resources.PersonalInformation_ContestTicket_Note%>
		            </p>
                </div>
                <ul class="progressbar">
		            <%--<li class="active">Ticket(s)</li>--%>
		            <li class="active"><%=Resources.Details_Ticketnumber%></li>
		            <%--<li class="active">Personal information</li>--%>
		            <li class="active"><%=Resources.Details_Personalinformation%></li>
		            <%--<li id="case">State the case</li>--%>
		            <li id="case"><%=Resources.Details_State_Case%></li>
	            </ul>
             
              <%--<%using (Html.BeginForm("SaveAppealInformation", "Appeal", FormMethod.Post, new { @role = "form", @class = "form-content" }))--%>
             <%using (Html.BeginForm(new { @role = "form", @class = "form-content", @onsubmit = "this.querySelectorAll('#Uploads').forEach((element) => { element.disabled = true; element.innerText='Contesting please wait...';})" }))
                 {%>
                
                        <%if (Model.SearchedTicket != null) 
                            { %>
                             <div class="ticket__wrap">
                                 <%--<h2 class="heading__ticket">Ticket</h2>--%>
                                 <h2 class="heading__ticket"><%=Resources._Ticket%></h2>
                                 <%-- Hidden Details --%>

                                <div class="ticketDetails">
		
		                            <div class="formbox filledForm">
			                            <div class="col-12 mb20">
				                            <label>Ticket code </label>
				                            
                                            <%=Html.TextBoxFor(model => model.SearchedTicket.ticketnumber, new
                                                    {
                                                        @id="TicketNumberSingle",
                                                        @class="form-control p-0",
                                                        @type = "text",
                                                        @Value = Model.SearchedTicket.ticketnumber
                                                    })
                                                %>
                                            <%= Html.HiddenFor(fin => fin.SearchedTicket.ticketnumber, new {
                                                        @type="hidden",
                                                        @id="ticketnumber",
                                                        @value=Model.SearchedTicket.ticketnumber,
                                                        @class="form-control p-0"})
                                                    %>
                                            <%= Html.HiddenFor(fin => fin.SearchedTicket.InstallationID, new {
                                                        @type="hidden",
                                                        @id="installationId",
                                                        @value=Model.SearchedTicket.InstallationID,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			                            <div class="col-12 mb20 ">
				                            <label>Ticket Type</label>
				                            <%--<input type="text" value="FAILED TO PAY FOR PARKING">--%>
                                            <%= Html.TextBoxFor(ticket => Model.SearchedTicket.TicketTypeDesc, new {
                                                            @type="text",
                                                            @value= Model.SearchedTicket.TicketTypeID + ", " + (!String.IsNullOrEmpty(Model.SearchedTicket.TicketTypeDesc) ? Model.SearchedTicket.TicketTypeDesc : "NA") ,
                                                            @class="form-control p-0"})
                                                        %>
                                    
			                            </div>
			
			                            <div class="col-6 mb20">
				                            <label>Issue Date </label>
                                             <input type="text" class="form-control p-0" value="<%=Model.SearchedTicket.ticketdate.ToShortDateString() %>">
                                    
			                            </div>
			                            <div class="col-6 mb20">
				                            <label>Issue Time </label>
                                             <input type="text" class="form-control p-0" value="<%=Model.SearchedTicket.ticketdate.ToShortTimeString() %>">
			                            </div>

                                        <div class="col-6 mb20">
				                            <label>Vehicle Licence Plate </label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketlprplate, new {
                                                        @type="text",
                                                        @id="platenumber",
                                                        @value=Model.SearchedTicket.ticketlprplate != null ? Model.SearchedTicket.ticketlprplate : Model.SearchedTicket.ticketqueryplate,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			                            <div class="col-6 mb20">
				                            <label>VIN </label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketlprplate, new {
                                                        @type="text",
                                                        @value="-",
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			
			                            <div class="col-12 mb20">
				                            <label>Vehcile State </label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.state, new {
                                                        @type="text",
                                                        @value=Model.SearchedTicket.Address.state,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			                            <div class="col-12 mb20 ">
				                            <label>Address</label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.postcode, new {
                                                        @type="text",
                                                        @value=Model.SearchedTicket.Address.postcode + Model.SearchedTicket.Address.city + Model.SearchedTicket.Address.suburb,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			
			                            <div class="col-6 mb20">
				                            <label>Postal Code* </label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.postcode, new {
                                                        @type="text",
                                                        @value=Model.SearchedTicket.Address.postcode,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			                            <div class="col-6 mb20">
				                            <label>Province/State*</label>
                                            <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.state, new {
                                                        @type="text",
                                                        @value=Model.SearchedTicket.Address.state,
                                                        @class="form-control p-0"})
                                                    %>
			                            </div>
			
			
			                            <div class="col-12 mb20 ">
				                            <label>Public Notes</label>
			                                <%= Html.TextAreaFor(fin => fin.SearchedTicket.TickPublicNotes, new {
                                                        @type="text",
                                                        @value=Model.SearchedTicket.TickPublicNotes !=null ? Model.SearchedTicket.TickPublicNotes : "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                                                        @class="form-control p-0"})
                                                    %>
                                        </div>
                                        <%--Collapsaible divs start--%>
                                            <div class="accordion whaccordian" id="accordionExample">
                                              <div class="card">
                                                <div class="card-header" id="headingOne">
                                                  <h2 class="mb-0">
                                                    <button class="btn  btnPhotos" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                      Photos <span class="arrow"></span>
                                                    </button>
                                                  </h2>
                                                </div>

                                                <div id="collapseOne" class="collapse" aria-labelledby="headingOne" data-parent="#accordionExample">
                                                  <div class="card-body">
                                          
                                                      <%if (Model.SearchedTicket.TicketImages != null)
                                                                    { %> 
                                                                           <%foreach (var images in Model.SearchedTicket.TicketImages) 
                                                                            { %>
                                                                                 <%if (!String.IsNullOrEmpty(images.base64))
                                                                                    { %> 
                                                                                            <img class="photos" src="data:image/png;base64,<%=images.base64%>" />
                                                                                            <label><%=images.ImageName %></label>
                                                                                 <%} %>
                                                                                <%else { %>
                                                                                            <label>No photos available.</label>
                                                                                <%} %>
                                                    
                                                                           <%} %>         
                                                                    <%} %>
                                                      <%else { %>
                                                             <label>No Image Available for this Ticket</label>
                                                      <%} %>
                                                  </div>
                                                </div>
                                              </div>
                                              <div class="card">
                                                <div class="card-header" id="headingTwo">
                                                  <h2 class="mb-0">
                                                    <button class="btn  collapsed btnPreview" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                                      Ticket Preview <span class="arrow"></span>
                                                    </button>
                                                  </h2>
                                                </div>
                                                <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                                  <div class="card-body">
                                                      <%if (!String.IsNullOrEmpty(Model.SearchedTicket.ticketpdfbase64)) 
                                                                    {%>
                                                                        <canvas class="TicketPreview" id="the-canvas<%=Model.SearchedTicket.ticketnumber %>"></canvas>
                                                                        <%=Html.HiddenFor(fin => Model.SearchedTicket.ticketpdfbase64, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @id = "pdfimage_" + Model.SearchedTicket.ticketnumber,
                                                                                @name = "pdfimage",
                                                                                @value = Model.SearchedTicket.ticketpdfbase64,
                                                                            })
                                                                         %>
                                                   <% } %>
                                                      <%else{%> 
                                                                <p id="ticketPreview<%=Model.SearchedTicket.ticketnumber %>" style="margin: 24px 0px 6px 28px;"></p>
                                                      <%} %>
                                         
                                                  </div>
                                                </div>
                                              </div>
                                              <div class="card">
                                                <div class="card-header" id="headingThree">
                                                  <h2 class="mb-0">
                                                    <button class="btn collapsed btnConversation" type="button" data-toggle="collapse" data-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                                      Conversation <span class="arrow"></span>
                                                    </button>
                                          
                                                  </h2>
                                                </div>
                                                <div id="collapseThree" class="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                                                  <div class="card-body">
                                                      <p>This is Under Progress</p>
                                                  </div>
                                                </div>
                                              </div>
                                            </div>
                                         <%--Collapsaible divs end--%>
			
			                           <div class="arrowleft"></div>
		
		                            </div>
		
                                </div>
                                 <div class="ticket__inner" id="Ticket">
                                     <div class="ticket__check">
                                          <%=Html.HiddenFor(fin => fin.SearchedTicket.InstallationID, new
                                                        {
                                                            @type = "hidden",
                                                            
                                                        })
                                                %>
                                     </div>
                                     <div class="ticket__contains">
                                         <div class="ticket__details">
                                             <div class="ticket__des">
                                                 <label>Ticket #</label>
                                                 <%--<p>123456</p>--%>
                                                 <p><%=Model.SearchedTicket.ticketnumber %></p>

                                                 
                                                 <%=Html.HiddenFor(model => model.SearchedTicket.ticketnumber, new
                                                        {   
                                                            @type = "hidden",
                                                            @id= "SearchedNumber", @Value = Model.SearchedTicket.ticketnumber
                                                        })
                                                %>
                                                  
                                                 <%=Html.HiddenFor(fin => fin.ticketnumber, new
                                                        {
                                                            @type = "hidden",
                                                        })
                                                %>
                                                 <%=Html.HiddenFor(fin => fin.Plate, new
                                                        {
                                                            @type = "hidden",
                                                        })
                                                %>

                                                 <input type="hidden" id="searchedTicketNumber" value="<%=ViewData["TicketNumber"]%>" />
                                                 <%--<%=Html.HiddenFor(fin => fin.SearchedTicket.ticketnumber, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicketNumber",
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.ticketnumber,
                                                        })
                                                %>--%>
                                             </div>
                                             <div class="ticket__des">
                                                 <label>Issue date</label>
                                                 <%--<p>13/03/22 9:00AM</p>--%>
                                                 <p><%=Model.SearchedTicket.ticketdate %></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketdate, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.ticketdate,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.ticketdate,
                                                        })
                                                %>
                                             </div>
                                         </div>
                                         <div class="ticket__details">
                                             <div class="ticket__des">
                                                 <label>Ticket Type</label>
                                                 <%--<p>FAILED TO PAY FOR PARKING</p>--%>
                                                 <p><%=Model.SearchedTicket.TicketTypeID %>, <%=!String.IsNullOrEmpty(Model.SearchedTicket.TicketTypeDesc) ? Model.SearchedTicket.TicketTypeDesc : "NA" %></p>

                                             </div>
                                         </div>
                                         <div class="ticket__details ticket__info">
                                             <div class="ticket__des">
                                                 <label>License Plate</label>
                                                 <%--<p>123456</p>--%>
                                                 <p><%=Model.SearchedTicket.ticketlprplate != null ? Model.SearchedTicket.ticketlprplate : Model.SearchedTicket.ticketqueryplate %></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketqueryplate, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.ticketqueryplate,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.ticketqueryplate,
                                                        })
                                                %>
                                             </div>
                                             <div class="ticket__des">
                                                 <label>State</label>
                                                 <%--<p>123456</p>--%>
                                                 <p><%=Model.SearchedTicket.Address.state %></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.state, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.Address.state,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.Address.state,
                                                        })
                                                %>
                                             </div>
                                             <div class="ticket__des">
                                                 <label>VIN</label>
                                                 <p>_</p>
                                             </div>
                                         </div>
                                         <div class="ticket__details ticket__info">
                                             <div class="ticket__des">
                                                 <label>Address</label>
                                                 <%--<p>1060 Brickell Avenue</p>--%>
                                                 <p><%=Model.SearchedTicket.Address.road%>,<%=Model.SearchedTicket.Address.suburb%></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.road, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.Address.road,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.Address.road,
                                                        })
                                                %>
                                                 <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.suburb, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.Address.suburb,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.Address.suburb,
                                                        })
                                                %>
                                             </div>
                                             <div class="ticket__des">
                                                 <label>Postal code</label>
                                                 <%--<p>3312</p>--%>
                                                 <p><%=Model.SearchedTicket.Address.postcode %></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.postcode, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.Address.postcode,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.Address.postcode,
                                                        })
                                                %>
                                             </div>
                                             <div class="ticket__des">
                                                 <label>Province/State</label>
                                                 <%--<p>3312</p>--%>
                                                 <p><%=Model.SearchedTicket.Address.postcode%>,<%=Model.SearchedTicket.Address.state%></p>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.state, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.Address.state,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.Address.state,
                                                        })
                                                %>
                                             </div>
                                         </div>
                                         <div class="ticket__details ticket__outer">
                                             <div class="ticket__des">
                                                 <label>Amount Due</label>
                                             </div>
                                             <div class="ticket__des">
                                                 <h5>$<%=Convert.ToDouble(Model.SearchedTicket.TickBalance) %></h5>
                                                  <%=Html.HiddenFor(fin => fin.SearchedTicket.TickBalance, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + Model.SearchedTicket.TickBalance,
                                                            @name = "checkbox-checked",
                                                            @value = Model.SearchedTicket.TickBalance,
                                                        })
                                                %>
                                                 
                                             </div>
                                         </div>
                                     </div>
                                     <div class="ticket__right">
                                         <div class="ticket__icn">
                                             <a href="#" id="detailedTicketIcon_<%=Model.SearchedTicket.ticketnumber %>">
                                                 <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 512 512" style="enable-background: new 0 0 512 512;" xml:space="preserve">
                                                     <g>
                                                         <g>
                                                             <g>
                                                                 <path d="M510.977,250.975C467.408,148.537,367.321,82.344,256,82.344S44.594,148.535,1.023,250.975                            c-1.364,3.206-1.364,6.827,0,10.033C44.591,363.458,144.677,429.656,256,429.656c111.326,0,211.411-66.198,254.977-168.648                            C512.341,257.802,512.341,254.181,510.977,250.975z M256,404.022c-99.194,0-188.557-57.922-229.191-148.029                            c40.635-90.098,130-148.014,229.191-148.014c99.193,0,188.557,57.916,229.191,148.012                            C444.557,346.098,355.194,404.022,256,404.022z" />
                                                                 <path d="M256,157.825c-54.134,0-98.174,44.041-98.174,98.174c0,54.133,44.041,98.174,98.174,98.174                            c54.133,0,98.174-44.04,98.174-98.174C354.174,201.865,310.133,157.825,256,157.825z M256,328.541                            c-39.998,0-72.54-32.542-72.54-72.54s32.542-72.54,72.54-72.54s72.54,32.542,72.54,72.54S295.998,328.541,256,328.541z" />
                                                                 <path d="M256,203.878c-28.739,0-52.123,23.382-52.123,52.123c0,7.078,5.737,12.817,12.817,12.817                            c7.08,0,12.817-5.739,12.817-12.817c0-14.606,11.882-26.489,26.489-26.489c7.08,0,12.817-5.739,12.817-12.817                            C268.817,209.615,263.078,203.878,256,203.878z" />
                                                             </g>
                                                         </g>
                                                     </g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                 </svg></a>
                                         </div>
                                         <div class="ticket__icn">
                                             <a href="#"  id="downloadTicketIcon_<%=Model.SearchedTicket.ticketnumber %>">
                                                 <svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 490 490" style="enable-background: new 0 0 490 490;" xml:space="preserve">
                                                     <g>
                                                         <g>
                                                             <g>
                                                                 <path d="M245,0c-9.5,0-17.2,7.7-17.2,17.2v331.2L169,289.6c-6.7-6.7-17.6-6.7-24.3,0s-6.7,17.6,0,24.3l88.1,88.1 				c3.3,3.3,7.7,5,12.1,5c4.4,0,8.8-1.7,12.1-5l88.1-88.1c6.7-6.7,6.7-17.6,0-24.3c-6.7-6.7-17.6-6.7-24.3,0L262,348.4V17.1 				C262.1,7.6,254.5,0,245,0z" />
                                                                 <path d="M462.1,472.9v-99.7c0-9.5-7.7-17.2-17.2-17.2s-17.2,7.7-17.2,17.2v82.6H62.2v-82.6c0-9.5-7.7-17.2-17.1-17.2 				s-17.2,7.7-17.2,17.2v99.7c0,9.5,7.7,17.1,17.2,17.1h399.8C454.4,490,462.1,482.4,462.1,472.9z" />
                                                             </g>
                                                         </g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                         <g></g>
                                                     </g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                     <g></g>
                                                 </svg></a>
                                         </div>
                                     </div>
                                 </div>

                                 <%-- Check Ticket Status Is Already Contested or Approved or Cancelled--%>
                                     <%if (Model.SearchedTicket.TicketStatusID == 9 && Model.SearchedTicket.TicketStatus == "Pending")
                                       { %> 
                                            <div class="ticket__inner" style ="display:flex; flex-direction:column">
                                                <div class="ticket__check nullticket" style="padding-left:15px;">
                                                    
                                                    <div class="alert alert-danger" style="border-radius:26px;">
                                                        
                                                        <label> Selected Ticket is already contested, Please check your email.</label>
                                                    </div>
                                                </div>
                                                
                                                <div class="ticket__check nullticket" style="padding-left:15px;">
                                                    
                                                    <%= Html.HiddenFor(fin => fin.StandardInstallationList, new
                                                                {
                                                                    @type = "hidden",
                                                                    @autocomplete = "off",
                                                                    @class = "form-control p-0",
                                                                    @value = Model.StandardInstallationList
                                                                })
                                                        %>
                                                         <%= Html.HiddenFor(fin => fin.InstallationList, new
                                                                {
                                                                    @type = "hidden",
                                                                    @autocomplete = "off",
                                                                    @class = "form-control p-0",
                                                                    @value = Model.InstallationList
                                                                })
                                                        %>
                                                        <button class="btn btn-secondary1" name="submitButton" value="Back" formaction="Details" formmethod="POST"><%=Resources.PersonalInformation_BackToTickets_Buttontxt%></button>
                                                </div>
                                            </div>
                                            
                                            
                                     <%} %>
                                     <%else if(Model.SearchedTicket.TicketStatusID == 10 && Model.SearchedTicket.TicketStatus == "Approved")
                                         {%>
                                            <div class="ticket__inner" style ="">
                                                <div class="ticket__check nullticket" style="padding-left:15px;">
                                                    
                                                    <div class="alert alert-success" style="border-radius:26px;">
                                                        
                                                        <label>Ticket selected for contest is Approved.</label>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                    <% } %>
                                     <%else if(Model.SearchedTicket.TicketStatusID == 11 && Model.SearchedTicket.TicketStatus == "Rejected")
                                             {%>
                                                <div class="ticket__inner" style ="">
                                                    <div class="ticket__check nullticket" style="padding-left:15px;">
                                                    
                                                        <div class="alert alert-info" style="border-radius:26px;">
                                                        
                                                            <label>Ticket selected for contest is Rejected.</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            
                                        <% } %>
                                     <%else 
                                         {%>
                                             <%-- Personal Information --%>
                                             <div class="ticket__wrap mt-60" id="personalInformation">
                                                 <%--<h2 class="heading__ticket">Personal Information</h2>--%>
                                                 <h2 class="heading__ticket"><%=Resources.PersonalInformation_PersonalInformation %></h2>
                                                 <%--<p class="mb40 fs-13 mt-0 ">1) The Pittsburg Parking Court's email notification process is optional and voluntary. A ticketholder has the option to recive notification of adjusdiction either (a) via first class mall, </p>--%>
                                                 <p class="mb40 fs-13 mt-0 "><%=Resources.PersonalInformation_PersonalInformation_Note %></p>


                                                 <div class="formbox">
                                                     <div class="col-12 mb20">
                                                         <%--<label>First Name* </label>--%>
                                                         <label><%=Resources.PersonalInformation_FirstName%> </label>
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
                                                         <span class="form_error" id="firstNameErrorMsg" style="color:red"></span>
                                                     </div>
                                                     <div class="col-12 mb20 ">
                                                         <%--<label>Last Name* </label>--%>
                                                         <label><%=Resources.PersonalInformation_LastName%> </label>

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
                                                         <span class="form_error" id="lastNameErrorMsg" style="color:red"></span>
                                                     </div>

                                                     <div class="col-12  mb20">
                                                         <%--<label>Address* </label>--%>
                                                         <label><%=Resources.PersonalInformation_Address %> </label>
                                                         <%= Html.TextBoxFor(fin => fin.AppealData.Address, new
{
    @placeholder = "@Churchill-laan 266/III, 1078 GA AMSTERDAM, Netherlands",
    //@required = "required",
    @type = "text",
    @id = "Appeal_Address",
    @name = "Appeal_Address",
    @autocomplete = "off",
    @class = "form-control p-0"
})
                                                        %>
                                                         <span class="form_error" id="addressErrorMsg" style="color:red"></span>

                                                     </div>

                                                     <div class="col-6 mb20">
                                                         <%--<label>Email* </label>--%>
                                                         <label><%=Resources.PersonalInformation_Email %></label>
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
                                                         <span class="form_error" id="invalid_email" style="color:red; font-size:11px;">Email is not valid</span>
                                                         <span class="form_error" id="empty_email" style="color:red; font-size:11px;">Email can not be empty</span>
                                                     </div>
                                                     <div class="col-6 mb20">
                                                         <label><%=Resources.PersonalInformation_EmailConfirmation %> </label>
                                                         <%--<label>Email Confirmation* </label>--%>
                                                         <%= Html.TextBoxFor(fin => fin.AppealData.Email, new
                                                                {
                                                                    @placeholder = "example@gmail.com",
                                                                    //@required = "required",
                                                                    @type = "email",
                                                                    @id = "confirmEmail",
                                                                    @name = "confirmEmail",
                                                                    @autocomplete = "off",
                                                                    @class = "form-control p-0"
                                                                })
                                                        %>

                                                         <span class="form_error" id="invalid_confirmEmail" style="color:red; font-size:11px;">Confirm email is not valid</span>
                                                         <span class="form_error" id="doesnot_Match" style="color:red; font-size:11px;">Email doesn't match</span>
                                                         <span class="form_error" id="empty_emailconfirm" style="color:red; font-size:11px;">Confirm email can not be empty</span>


                                                     </div>
                                                        <%= Html.ValidationMessageFor(fin => fin.AppealData.Email, "", new { @class = "text-danger emailError" }) %>
                                            
                                                     <div class="col-6 mb20">
                                                         <%--<label style="font-weight: bold">Notification of adjudication <span style="font-weight: normal">(Select One)</span></label>--%>
                                                         <label style="font-weight: bold"><%=Resources.PersonalInformation_Note%> <span style="font-weight: normal"><%=Resources.PersonalInformation_SelectOne%></span></label>
                                                     </div>

                                                     <div class="col-6 mb20">
                                                         <label class="labelCheckbox">
                                                             <input type="radio" id="adMail" name="Mail" value="First Class Mail">
                                                             <%=Resources.PersonalInformation_Cb1%></label>
                                                         <label class="labelCheckbox">
                                                             <input type="radio" id="EleMail" name="Mail" checked="checked">
                                                             <%=Resources.PersonalInformation_Cb2%></label>
                                                     </div>
                                                     <div class="col-12  mb20 btns">

                                                         <%if (Request.UrlReferrer != null)
                                                             {%> 
                                                                 <%--<input  class="btn btn-secondary1" type="submit" name="Back" value="Back" />--%>
                                                                        <%--<button class="btn btn-secondary1" type="submit" onclick="history.go(-1); return false;">Back to tickets</button>--%>
                                                         <%= Html.HiddenFor(fin => fin.StandardInstallationList, new
                                                                {
                                                                    @type = "hidden",
                                                                    @autocomplete = "off",
                                                                    @class = "form-control p-0",
                                                                    @value = Model.StandardInstallationList
                                                                })
                                                        %>
                                                         <%= Html.HiddenFor(fin => fin.InstallationList, new
                                                                {
                                                                    @type = "hidden",
                                                                    @autocomplete = "off",
                                                                    @class = "form-control p-0",
                                                                    @value = Model.InstallationList
                                                                })
                                                        %>
                                                                        <%--<button class="btn btn-secondary1" name="submitButton" value="Back" formaction="Details" formmethod="post">Back to tickets</button>--%>
                                                                        <button class="btn btn-secondary1" name="submitButton" value="Back" formaction="Details" formmethod="post"><%=Resources.PersonalInformation_BackToTickets_Buttontxt%></button>
                                                                
                                                              
                                                         <% } %>
                                                         <%--<button class="btn btn-primary btnContinue" id="btnContinue" disabled>Continue</button>--%>
                                                         <button class="btn btn-primary btnContinue" id="btnContinue" disabled><%=Resources.PersonalInformation_Continue_Buttontxt%></button>
                                                     </div>
                                                 </div>

                                     

                                             </div>

                                             <%-- Please state Your case --%>
                                             <div class="ticket__wrap mt-60" id="stateCase">
		                                        <%--<h2 class="heading__ticket">Please state your case </h2>--%>
		                                        <h2 class="heading__ticket"><%=Resources.PersonalInformation_PleaseStateYourCase%></h2>
		
		                                        <%--<p>Briefly explain the relevant facts supporting your request for dismissal of the ticket. you may attach any evidence,photographs or information necessary to support your position. (2000 character limit) </p>--%>
		                                        <p><%=Resources.PersonalInformation_PleaseStateYourCase_Note%> </p>
		
		                                        <div class="formbox">
			                                        <div class="col-12 mb20">
				                                        <%--<label>Reason*  </label>--%>
				                                        <label><%=Resources.PersonalInformation_Reason%>  </label>
                                                        <%=Html.DropDownListFor(fin => fin.AppealData.AppealReason, new List<SelectListItem> {
                                                                                       new SelectListItem { Value = "Parking" , Text = "Parking" },
                                                                                       new SelectListItem { Value = "Non Parking" , Text = "Non Parking" },
                                                                                       new SelectListItem { Value = "Failed to prove" , Text = "Failed to prove" }
                                                                                    },
                                                                                        "Select a reason",
                                                                                        new
                                                                                        {
                                                                                            @class = "form-control p-0",
                                                                                            @name = "reason",
                                                                                            @id = "reason"

                                                                                        })%>
			                                        </div>
		
			
			                                        <div class="col-12 mb20 ">
				                                        <%--<label>Description*</label>--%>
				                                        <label><%=Resources.PersonalInformation_Description%></label>
                                                        <%= Html.TextAreaFor(fin => fin.AppealData.AppealDesc, new
                                                            {
                                                                @placeholder = "example@gmail.com",
                                                                //@required = "required",
                                                                @id = "desc",
                                                                @name = "desc",
                                                                @autocomplete = "off",
                                                                @class = "form-control p-0",
                                                                @maxlength="2000",
                                                            })
                                                            %>
                                                        <label id="chars" style="font-size: 10px; text-align: right;">2000</label>
			                                        </div>
			
                                                    
			                                        <div class="col-12 mb20 ">
				                                        <%--<label>Upload the files to provide proof</label>--%>
				                                        <label><%=Resources.PersonalInformation_UploadTheFiles%></label>


				                                        <label class="drag dropSection" id="dropOnMe" name="picture" draggable="false">
                                                            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/dummy.png" />
					                                        <span>Drag your or <strong>browse</strong></span>
                                                            <%--<input class="Fileinput" type="file" id="pictureInput" name="picture" multiple />--%>
                                                            <%= Html.TextBoxFor(fin => fin.AppealData.AppealImageList, new
                                                                    {
                                                                        @type = "file",
                                                                        @id = "pictureInput",
                                                                        @name = "AppealImageList",
                                                                        @class = "Fileinput",
                                                                        @multiple = true
                                                                    })
                                                            %>
                                                
                                                            <span class="draggableFile" draggable="false">
                                                                <ol draggable="false" id="myFileList"></ol>
                                                            </span>
				                                        </label>
                                                        <%--<input id="upload" draggable="false" type="button"
                                                            value="Upload Selected Files" />--%>
			                                        </div>

			                                        <div class="col-12 mb20 ">
				                                        <div class="upload-row">
                                                            <%--<%= Html.HiddenFor(fin => fin.AppealData.AppealImageList, new {
                                                                @type="file",
                                                                @id="appealName",
                                                                @class="Fileinput",
                                                                @name="AppealImageList",
                                                                @multiple=true})
                                                            %>--%>
                                               
				                                        </div>

				
				                                        <%--<p>Note: Only the file extension GIF, JPG, BMP,TIF, DDC,WAV are allowed and total uploaded file size must be less than 10 MB </p>--%>
				                                        <p><%=Resources.PersonalInformation_Note2%></p>
			                                        </div>
			
			
			                                        <div class="col-12 mb20  text-center">
				                                            <%= Html.TextBoxFor(fin => fin.AppealData.AppealImageLists, new
                                                                    {
                                                                        @type = "hidden",
                                                                        @id = "pictureInput1",
                                                                        @name = "pictureInput1",
                                                                        @class = "Fileinput",
                                                                        @multiple = true
                                                                    })
                                                            %>
				                                        <%--<button class="btn btn-primary" id="upload" type="submit">Submit</button>--%>
				                                        <button 
                                                            class="btn btn-primary" 
                                                            formaction="SaveAppealInformation" 
                                                            id="Uploads" 
                                                            formmethod="post"
                                                            type="submit"><%=Resources.PersonalInformation_Submit%></button>
			                                        </div>
			
			
		
		                                        </div>
		
                                            </div>
                                            
                                            
                                    <% } %>

                             </div>

                            <%} %>
                        <%else 
                          {%> 
                                <div class="ticket__inner">
                                    <div class="ticket__check nullticket" style="padding-left:15px;">
                                        <label class="form-control"> Selected Ticket Is Not Available or Already Contested !!!</label>
                                    </div>
                                </div>
                        <%} %>
                       
             <% } %>
            
         </div>

    
                    <br/>
                    <br/>
                    <div class="contact__details">
                        <div class="contact">
                            <p>
                                 <%=Resources.Appeal_ImmediatelyUponYourSubmission %>
                            </p>
                            <p><%=Resources.Appeal_IfThisNoticeIsNotReceived%></p><br/>
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


                
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places"></script> 
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?sensor=false&libraries=places&key=<API_Key>"></script>--%>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.0.943/pdf.min.js"></script>
    <script type="text/javascript" src="http://cdn.jsdelivr.net/jquery.cookie/1.3.1/jquery.cookie.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.1.0/knockout-min.js" type="text/javascript"></script>
    <link href="dropzone/dropzone.css" rel="stylesheet" type="text/css" />

    <%--<script src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/Appeal_JS/PersonalInformation.js"></script>--%>

    <script>

        $(document).ready(function () {
            // Hide some info forms
            $(".ticketDetails").hide();
            $("#stateCase").hide();
            $("#invalid_email").hide();
            $("#invalid_confirmEmail").hide();
            $("#doesnot_Match").hide();
            $("#empty_email").hide();
            $("#empty_emailconfirm").hide();

            // Detect Device
            var isMobile = {
                Android: function () {
                    return navigator.userAgent.match(/Android/i);
                },
                BlackBerry: function () {
                    return navigator.userAgent.match(/BlackBerry/i);
                },
                iOS: function () {
                    return navigator.userAgent.match(/iPhone|iPad|iPod/i);
                },
                Opera: function () {
                    return navigator.userAgent.match(/Opera Mini/i);
                },
                Web: function () {
                    return navigator.userAgent.match(/Chrome/i);
                },
                Windows: function () {
                    return navigator.userAgent.match(/IEMobile/i);
                },
                None: function () {
                    if (!(isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows() || isMobile.Web())) return "None"
                },
                any: function () {
                    return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows() || isMobile.Web() || isMobile.None());
                }
            };

            // Appeal information validations
            let firstName = "";
            let lastName = "";
            let Appeal_Address = "";
            let email = "";
            let confirmEmail = "";
            let appealReason = "";
            let appealDesc = "";
            let EmailEqual = false;

            let AppealData = {
                FirstName: "",
                LastName: "",
                Address: "",
                Email: "",
                AppealReason: "",
                AppealDesc: "",
                Device: isMobile.any()[0]
            };
            let InstallationDetails = {
                ticketnumber: $("#searchedTicketNumber").val(),
                plate: $("#platenumber").val(),
                InstallationId: $("#installationId").val(),
            }
            $("#firstname").change(function (e) {
                firstName = e.target.value;
                AppealData.FirstName = firstName;
                if (firstName == "") {
                    var first = document.getElementById("firstNameErrorMsg")
                    first.innerText = "First name can not be empty";
                    first.style.color = "red";
                    first.style.fontSize = "11px"
                    first.blur();
                    $("#btnContinue").prop('disabled', true);
                }
                else {
                    $("#firstNameErrorMsg").text("");
                }

            });
            $("#lastname").change(function (e) {
                lastName = e.target.value;
                AppealData.LastName = lastName;
                if (lastName == "") {
                    var first = document.getElementById("lastNameErrorMsg")
                    first.innerText = "Last name can not be empty";
                    first.style.color = "red";
                    first.style.fontSize = "11px"
                    first.blur();
                    $("#btnContinue").prop('disabled', true);


                }
                else {
                    $("#lastNameErrorMsg").text("");
                }
            });
            $("#Appeal_Address").change(function (e) {
                Appeal_Address = e.target.value;
                AppealData.Address = Appeal_Address;
                if (Appeal_Address == "") {
                    var first = document.getElementById("addressErrorMsg")
                    first.innerText = "Address can not be empty";
                    first.style.color = "red";
                    first.style.fontSize = "11px"
                    first.blur();
                    $("#btnContinue").prop('disabled', true);


                }
                else {
                    $("#addressErrorMsg").text("");
                }
            });
            $("#email").on("change keypress keyup keydown",function (e) {
                email = e.target.value;
                if (email == "") {
                    $("#btnContinue").prop('disabled', true);
                    $("#empty_email").show();
                    $("#invalid_email").hide();

                }
                else if (!IsEmail(email) && email != "") {
                    $("#invalid_email").show();
                    $("#empty_email").hide();
                    $("#btnContinue").prop('disabled', true);

                }
                else {
                    $("#invalid_email").hide();
                    $("#empty_email").hide();
                    AppealData.Email = email;
                }

            });
            

            $("#confirmEmail").on('change keypress keyup keydown',function (e) {
                confirmEmail = e.target.value;
                if (confirmEmail == "") {
                    $("#empty_emailconfirm").show();
                    $("#invalid_confirmEmail").hide();
                }
                else if (!IsEmail(confirmEmail) && confirmEmail!="") {
                    $("#invalid_confirmEmail").show();
                    $("#doesnot_Match").hide();
                    $("#empty_emailconfirm").hide();
                    $("#btnContinue").prop('disabled', true);

                }
                else if (confirmEmail != email) {
                    $("#invalid_confirmEmail").hide();
                    $("#doesnot_Match").show();
                    $("#empty_emailconfirm").hide();
                    $("#btnContinue").prop('disabled', true);

                }
                else {
                    $("#invalid_confirmEmail").hide();
                    $("#doesnot_Match").hide();
                    $("#empty_emailconfirm").hide();

                    EmailEqual = true;
                    if (firstName != "" && lastName != "" && Appeal_Address != "" && email != "" && confirmEmail != "" && IsEmail(email) && IsEmail(confirmEmail) && EmailEqual) {
                        $("#btnContinue").removeAttr("disabled");
                    }
                }

            });

            $("#confirmEmail").bind('copy paste cut', function (e) {
                e.preventDefault(); 
            });

            $("#reason").change(function () {
                appealReason = $(this).children("option:selected").val();
                AppealData.AppealReason = appealReason;


            });
            $("#desc").change(function (e) {
                appealDesc = e.target.value;
                AppealData.AppealDesc = appealDesc

            });
            $(".formbox").change(function (e) {
                if (firstName != "" && lastName != "" && Appeal_Address != "" && email != "" && confirmEmail != "" && IsEmail(email) && IsEmail(confirmEmail) && EmailEqual) {
                    $("#btnContinue").removeAttr("disabled");
                }
            });

            $("#btnContinue").click((e) => {
                $("#stateCase").show();
                $("#personalInformation").hide();
                $("#case").addClass("active");
                e.preventDefault();
            })


            

            // Validate Email Address
            function IsEmail(email) {
                var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!regex.test(email)) {
                    return false;
                } else {
                    return true;
                }
            }

            //Appeal Images drag and Drop as well as On change
            const fileInput = document.getElementById("pictureInput");
            let myFiles = {};
            // if you expect files by default, make this disabled
            // we will wait until the last file being processed
            let isFilesReady = true;
            let AppealImageList = [];


            fileInput.addEventListener("change", async (event) => {
                debugger;
                // clean up earliest items
                myFiles = {};
                // set state of files to false until each of them is processed
                isFilesReady = false;

                // this is to get the input name attribute, in our case it will yield as "picture"
                // I'm doing this because I want you to use this code dynamically
                // so if you change the input name, the result also going to effect
                const inputKey = fileInput.getAttribute("name");
                var files = event.srcElement.files;

                const filePromises = Object.entries(files).map((item) => {
                    return new Promise((resolve, reject) => {
                        const [index, file] = item;
                        const reader = new FileReader();
                        reader.readAsBinaryString(file);

                        reader.onload = function (event) {
                            // if it's multiple upload field then set the object key as picture[0], picture[1]
                            // otherwise just use picture
                            const fileKey = `${inputKey}${files.length > 1 ? `[${index}]` : ""
                                }`;
                            // Convert Base64 to data URI
                            // Assign it to your object
                            myFiles[fileKey] = `data:${file.type};base64,${btoa(
                                event.target.result
                            )}`;
                            file.imagename = file.name;
                            file.date = file.lastModified;
                            file.fileSize = file.size;
                            file.ImageId = $("#searchedTicketNumber").val() + "-" + file.size;
                            file.ticketnumber = $("#searchedTicketNumber").val();
                            file.cityid = $("#installationId").val();
                            file.image = `data:${file.type};base64,${btoa(
                                event.target.result
                            )}`;
                            AppealImageList.push(file);
                            displayImages(file);
                            resolve();
                        };
                        reader.onerror = function () {
                            console.log("can't read the file");
                            reject();
                        };
                    });
                });

                Promise.all(filePromises)
                    .then(() => {
                        console.log("ready to submit");
                        isFilesReady = true;

                        // demo purposes only
                        var json = JSON.stringify(myFiles, undefined, 2);
                        // /demo purposes only
                    })
                    .catch((error) => {
                        console.log(error);
                        console.log("something wrong happened");
                    });
            });

            // upoad image functionality

            if (typeof (window.FileReader) == 'undefined') {
                alert('Browser does not support HTML5 file uploads!');
            }
            dropOnMe.addEventListener("drop", dropHandler, false);

            dropOnMe.addEventListener("dragover", function (ev) {
                $("#dropOnMe").css("background-color", "lightgoldenrodyellow;");
                ev.preventDefault();
            }, false);

            function dropHandler(ev) {
                // Prevent default processing.
                ev.preventDefault();

                // Get the file(s) that are dropped.
                var filelist = ev.dataTransfer.files;
                if (!filelist) return;  // if null, exit now


                // get image base 64

                const filePromises = Object.entries(filelist).map((item) => {
                    return new Promise((resolve, reject) => {
                        const [index, file] = item;
                        const reader = new FileReader();
                        reader.readAsBinaryString(file);

                        reader.onload = function (event) {
                            // if it's multiple upload field then set the object key as picture[0], picture[1]
                            // otherwise just use picture
                            const fileKey = `picture${filelist.length > 1 ? `[${index}]` : ""
                                }`;
                            // Convert Base64 to data URI
                            // Assign it to your object
                            myFiles[fileKey] = `data:${file.type};base64,${btoa(
                                event.target.result
                            )}`;
                            file.imagename = file.name;
                            file.date = file.lastModified;
                            file.fileSize = file.size;
                            file.ImageId = $("#searchedTicketNumber").val() + "-" + file.size;
                            file.ticketnumber = $("#searchedTicketNumber").val();
                            file.cityid = $("#installationId").val();
                            file.image = `data:${file.type};base64,${btoa(
                                event.target.result
                            )}`;
                            AppealImageList.push(file);
                            console.log("image File List : ", AppealImageList);
                            displayImages(file);
                            resolve();
                        };
                        reader.onerror = function () {
                            console.log("can't read the file");
                            reject();
                        };
                    });
                });

                Promise.all(filePromises)
                    .then(() => {
                        console.log("ready to submit");
                        isFilesReady = true;

                        // demo purposes only
                        var json = JSON.stringify(myFiles, undefined, 2);

                        // /demo purposes only
                    })
                    .catch((error) => {
                        console.log(error);
                        console.log("something wrong happened");
                    });

            }

            $("form").submit(function (e) {
                $("#pictureInput1").val(JSON.stringify(AppealImageList));
                $("#Uploads").text("Contesting please wait....");
                $("#Uploads").prop("disabled", true)
            });

           

            //Display images

            function displayImages(image) {
                if (image != null) {
                    var uploadImageOuter = document.createElement("div");
                    uploadImageOuter.setAttribute("class", "upload-img-outer");
                    uploadImageOuter.setAttribute("id", "imageouter" + image.lastModified);

                    var closeBtn = document.createElement("span");
                    closeBtn.setAttribute("class", "file_remove");
                    closeBtn.setAttribute("id", "rmImage_" + image.lastModified);
                    closeBtn.textContent = "X";

                    var uploadimg = document.createElement("div");
                    uploadimg.setAttribute("class", "upload-img");
                    uploadimg.setAttribute("id", "uploadimage" + image.lastModified);

                    var imgoverlay = document.createElement("div");
                    imgoverlay.setAttribute("class", "img-overlay");
                    imgoverlay.setAttribute("id", "imageoverlay" + image.lastModified);
                    imgoverlay.innerText = image.name;

                    var img = document.createElement('img');
                    img.setAttribute("id", "image" + image.lastModified);
                    img.src = image.image;

                    

                    //var appealName = document.getElementById("appealName");
                    //appealName.setAttribute("value", AppealImageList);
                    //appealName.name = AppealImageList;

                    //var appealBase64 = document.getElementById("appealBase64");
                    //appealBase64.setAttribute("value", image.base64);
                    //appealBase64.name = image.base64;

                    $(".upload-row").append(uploadImageOuter);
                    $("#imageouter" + image.lastModified).append(uploadimg);
                    $("#imageouter" + image.lastModified).append(closeBtn);
                    $("#uploadimage" + image.lastModified).append(img);
                    $("#uploadimage" + image.lastModified).append(imgoverlay);
                }
            }
            dropOnMe.addEventListener("dragend", function (ev) {
                $("#dropOnMe").css("background-color", "lightgray;");
                $("#dropOnMe").text("");
                $("upload").click(function () { });
                ev.preventDefault();
            }, false);


            $(document).on('click', '[id^=rmImage_]', function (e) {
                debugger;
                let downloadId = this.id;
                let idNumber = this.id.split('_').pop();
                $("#imageouter" + idNumber).remove();

                //remaining is to pop image element from Appeal ImageList
                AppealImageList = removeByAttr(AppealImageList, 'lastModified', idNumber);
            });

            var removeByAttr = function (arr, attr, value) {
                debugger;
                for (let i = 0; i < arr.length; i++) {
                    if (arr[i][attr].toString() === value) {
                        arr.splice(i, 1);
                    }
                }
                return arr;
            }
        });
        $(document).on('click', '[id^=downloadTicketIcon_]', function () {
            debugger;
            let downloadId = this.id;
            let idNumber = this.id.split('_').pop();
            let pdfid = 'pdfimage_' + idNumber;
            let pdfElement = $("#" + pdfid).val();
            let AllPdfImageIds = event.target.querySelectorAll("[id^=pdfimage_]");

            if (pdfElement != undefined) {
                let linkSource = `data:application/octet-stream;base64,${pdfElement}`;
                let downloadLink = document.createElement("a");
                downloadLink.href = linkSource;
                downloadLink.download = `BlinkayTicket_${idNumber}.pdf`;
                downloadLink.click();
            }
            else {
                $('#exampleModalCenter').modal('show');
                $('#ErrorMessage').text("Ticket copy is not available...!");
            }
        });

        

        $(document).on('click', '[id^=detailedTicketIcon_]', function (e) {
            debugger;
            idNumber = this.id.split('_').pop();
            var ticketId = 'Ticket_' + idNumber;
            var eyeBtnId = 'detailedTicketIcon_' + idNumber
            var detaildTicketId = 'detailedTicket_' + idNumber;
            arrowleft = 'arrowleft_' + idNumber;

            if (idNumber != null) {
                $("#Ticket").hide();
                $(".ticketDetails").show();
            }

            // Ticket Preview
            let pdfid = 'pdfimage_' + idNumber;
            let pdfElement = $("#" + pdfid).val();
            if (pdfElement != undefined) {
                let linkSource = `data:application/octet-stream;base64,${pdfElement}`;
                fetch(linkSource)
                    .then(res => res.blob())
                    .then(blob => {
                        let file = new File([blob], `BlinkayTicket_${idNumber}.pdf`, { type: "application/octet-stream" });

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            showInCanvas(e.target.result);
                        }
                        reader.readAsDataURL(file);

                        function convertDataURIToBinary(dataURI) {
                            var BASE64_MARKER = ';base64,';
                            var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
                            var base64 = dataURI.substring(base64Index);
                            var raw = window.atob(base64);
                            var rawLength = raw.length;
                            var array = new Uint8Array(new ArrayBuffer(rawLength));

                            for (i = 0; i < rawLength; i++) {
                                array[i] = raw.charCodeAt(i);
                            }
                            return array;
                        }
                        function showInCanvas(url) {
                            'use strict';
                            var pdfAsArray = convertDataURIToBinary(url);
                            pdfjsLib.getDocument(pdfAsArray).then(function (pdf) {
                                pdf.getPage(1).then(function (page) {
                                    var scale = 1.5;
                                    var viewport = page.getViewport(scale);
                                    var canvas = document.getElementById('the-canvas' + idNumber);
                                    var context = canvas.getContext('2d');
                                    canvas.height = viewport.height;
                                    canvas.width = viewport.width;
                                    var renderContext = {
                                        canvasContext: context,
                                        viewport: viewport
                                    };
                                    page.render(renderContext);
                                });
                            });
                        }
                    });
            }
            else {
                var canvas = document.getElementById('ticketPreview' + idNumber);
                canvas.innerText = "Ticket Preview is not Available !!!";
                canvas.innerHTML.blink();
            }


        });

        $(".arrowleft").click((e) => {
            $("#Ticket").show();
            $(".ticketDetails").hide();
        })

        var maxLength = 2000;
        $('#desc').keyup(function () {
            var length = $(this).val().length;
            var length = maxLength - length;
            $('#chars').text(length);
        });
    </script>
    
</asp:Content>
