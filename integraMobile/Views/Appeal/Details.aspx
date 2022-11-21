<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.AppealReturnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/Appeal_CSS/Details.css" rel="stylesheet" />

    <%-- New Html Changes --%>

    <div class="mainTicketContent">
        <div class="ticket__wrap" style="margin-bottom:70px;">
		     <%--<h2 class="heading__ticket1">Ticket Detail</h2>--%>
		     <h2 class="heading__ticket1"><%=Resources.Details_TicketDetail%></h2>
        </div>
        <ul class="progressbar">
            <%--<li class="active">Ticket(s) </li>--%>
            <li class="active"><%=Resources.Details_Ticketnumber%> </li>
            <%--<li>Personal information </li>--%>
            <li><%=Resources.Details_Personalinformation%> </li>
            <%--<li>Payment </li>--%>
            <li> <%=Resources.Details_Payment%> </li>
        </ul>

        <%using (Html.BeginForm("PersonalInformation", "Appeal", FormMethod.Post, new { @role = "form", @class = "form-content" })) 
            { %> 

                <div class="ticket__wrap  mt-60">
                    <%--<%=Model.SearchedTicket = null %>--%>
                    <%-- if Model is not null --%>
                    <%if (Model.SearchedTicket != null) { %>
                            <h2 class="heading__ticket"><%=Resources._Ticket%> #<%=Model.SearchedTicket.ticketnumber %></h2>
                        
                            <%if (Model.SearchedTicket.InstallationID == Convert.ToInt32(Model.StandardInstallationList)) 
                              { %>
                                    <%-- Hidden Details Of Ticket Details--%>
                                    <div class="ticketDetails" id="detailedTicket_<%=Model.SearchedTicket.ticketnumber %>">
		                                <div class="formbox filledForm">
			                                <div class="col-12 mb20">
				                                <label>Ticket code </label>
				                                <%--<input type="text" value="<%=Model.SearchedTicket.ticketnumber %>">--%>
                                                 <%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketnumber, new { @type="text", @value=Model.SearchedTicket.ticketnumber, @class="form-control p-0"})%>
			                                </div>
			                                <div class="col-12 mb20 ">
				                                <label>Ticket Type</label>
				                                <%--<input type="text" value="FAILED TO PAY FOR PARKING">--%>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.TicketTypeDesc, new {@type="text", @value=Model.SearchedTicket.TicketTypeID + ", " +  (!String.IsNullOrEmpty(Model.SearchedTicket.TicketTypeDesc) ? Model.SearchedTicket.TicketTypeDesc : "NA"),
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
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.ticketlprplate != null ? Model.SearchedTicket.ticketlprplate : Model.SearchedTicket.ticketqueryplate,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-6 mb20">
				                                <label>VIN </label>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketlprplate, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value="-",
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			                                <div class="col-12 mb20">
				                                <label>Vehcile State </label>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.state, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.Address.state,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-12 mb20 ">
				                                <label>Address</label>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.postcode, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.Address.postcode + Model.SearchedTicket.Address.city + Model.SearchedTicket.Address.suburb,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			                                <div class="col-6 mb20">
				                                <label>Postal Code* </label>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.postcode, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.Address.postcode,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-6 mb20">
				                                <label>Province/State*</label>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.Address.state, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.Address.state,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			
			                                <div class="col-12 mb20 ">
				                                <label>Public Notes</label>
			                                    <%= Html.TextAreaFor(fin => fin.SearchedTicket.TickPublicNotes, new {
                                                            @type="text",
                                                            @id="firstname",
                                                            @name="firstname",
                                                            @value=Model.SearchedTicket.TickPublicNotes !=null ? Model.SearchedTicket.TickPublicNotes : "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                                                            @class="form-control p-0"})
                                                        %>
                                            </div>

                                             <%--Collapsaible divs start--%>
                                            <div class="accordion whaccordian" id="accordionExample">
                                              <div class="card">
                                                <div class="card-header" id="headingOne">
                                                  <h2 class="mb-0">
                                                    <button class="btn  btnPhotos form-control p-0" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
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
                                                                                            <%--<label><%=images.ImageName %></label>--%>
                                                                                 <%} %>
                                                                                <%else { %>
                                                                                            <label>No photos available.</label>
                                                                                <%} %>
                                                    
                                                                           <%} %>         
                                                                    <%} %>
                                                      <%else { %>
                                                             <label>No photos available.</label>
                                                      <%} %>
                                                  </div>
                                                </div>
                                              </div>
                                              <div class="card">
                                                <div class="card-header" id="headingTwo">
                                                  <h2 class="mb-0">
                                                    <button class="btn  collapsed btnPreview form-control p-0" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
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
                                                    <button class="btn collapsed btnConversation form-control p-0" type="button" data-toggle="collapse" data-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
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

                                            <div class="col-6 mb20 amount">
				                                <label class="totalamountlabel">Total Amount: </label>
                                                <span>$<%=Model.SearchedTicket.TickBalance %></span>
                                    
			                                </div>
			
			                               <%--<div class="arrowleft"></div>--%>
            </div>
                                        </div>
        
                                
                        
                                <%-- Searched Ticket --%>
                                    <div class="ticket__inner" id="Ticket_<%=Model.SearchedTicket.ticketnumber %>">
                                        <div class="ticket__check">
                                            <label class="form-control">
                                                <%--<input type="checkbox" id="searchedTicket" name="checkbox-checked" value="<%=Model.SearchedTicket.ticketid %>" checked />--%>
                                                <%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketid, new {
                                                @type="checkbox",
                                                @id="searchedTicket_"+Model.SearchedTicket.ticketid,
                                                @name="checkbox-checked",
                                                @value=Model.SearchedTicket.ticketid,
                                                @checked=true})
                                            %>
                                            <%=Html.HiddenFor(fin => fin.SearchedTicket.InstallationID, new
                                                                    {
                                                                        @type = "hidden",
                                                                        @id = "searchedTicket" + Model.SearchedTicket.InstallationID,
                                                                        @name = "checkbox-checked",
                                                                        @value = Model.SearchedTicket.InstallationID,
                                                                    })
                                                            %>
                                            </label>

                                 
                                        </div>
                                        <div class="ticket__contains">
                                            <div class="ticket__details ticket-box-outer">
                                                <%-- new changes --%>
                                                <div class="ticket-box">
                                                    <label>Ticket #</label>
                                                    <p><%=Model.SearchedTicket.ticketnumber %></p>
                                                        <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketnumber, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket_" + Model.SearchedTicket.ticketnumber,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.ticketnumber,
                                                            })
                                                    %>
                                                </div>

                                                <div class="ticket-box">
                                                    <label>Ticket Type</label>
                                                    <p><%=Model.SearchedTicket.TicketTypeID %>, <%=!String.IsNullOrEmpty(Model.SearchedTicket.TicketTypeDesc) ? Model.SearchedTicket.TicketTypeDesc : "NA" %></p>
                                                </div>

                                                <div class="ticket-box">
                                                    <label>Issue date</label>
                                                    <p><%=Model.SearchedTicket.ticketdate.ToShortDateString() %></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketdate, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + Model.SearchedTicket.ticketdate,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.ticketdate,
                                                            })
                                                    %>

                                                </div>
                                                <div class="ticket-box">
                                                    <label>Issue Time</label>
                                                    <p><%=Model.SearchedTicket.ticketdate.ToShortTimeString() %></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketdate, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + Model.SearchedTicket.ticketdate,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.ticketdate,
                                                            })
                                                    %>
                                                </div>
                                                
                                                <div class="ticket-box">
                                                    <label>License Plate</label>
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
                                                <div class="ticket-box">
                                                    <label>VIN</label>
                                                    <p><%=!String.IsNullOrEmpty(Model.SearchedTicket.TickVin) ? Model.SearchedTicket.TickVin : "_"%></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.TickVin, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + Model.SearchedTicket.TickVin,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.TickVin,
                                                            })
                                                    %>
                                                </div>
                                                <div class="ticket-box">
                                                    <label>Location</label>
                                                    <%--<p>1060 Brickell Avenue</p>--%>
                                                    <p><%=Model.SearchedTicket.Address.road%>,<%=Model.SearchedTicket.Address.suburb%></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.suburb, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + Model.SearchedTicket.Address.suburb,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.Address.suburb,
                                                            })
                                                    %>
                                                </div>
                                                <div class="ticket-box">
                                                    <label>Status</label>
                                                    <%--<p>1060 Brickell Avenue</p>--%>
                                                    <p><%=Model.SearchedTicket.TicketStatus %></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.suburb, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + Model.SearchedTicket.Address.suburb,
                                                                @name = "checkbox-checked",
                                                                @value = Model.SearchedTicket.Address.suburb,
                                                            })
                                                    %>
                                                </div>


                                            <%-- Old changes --%>
                                               
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
                                                <a href="javascript:void(0);" id="detailedTicketIcon_<%=Model.SearchedTicket.ticketnumber %>">
                                                    <svg version="1.1" id="Layer_2" xmlns="http://www.w3.org/2000/svg"
                                                        xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 512 512"
                                                        style="enable-background:new 0 0 512 512;" xml:space="preserve">
                                                        <g>
                                                            <g>
                                                                <g>
                                                                    <path
                                                                        d="M510.977,250.975C467.408,148.537,367.321,82.344,256,82.344S44.594,148.535,1.023,250.975                            c-1.364,3.206-1.364,6.827,0,10.033C44.591,363.458,144.677,429.656,256,429.656c111.326,0,211.411-66.198,254.977-168.648                            C512.341,257.802,512.341,254.181,510.977,250.975z M256,404.022c-99.194,0-188.557-57.922-229.191-148.029                            c40.635-90.098,130-148.014,229.191-148.014c99.193,0,188.557,57.916,229.191,148.012                            C444.557,346.098,355.194,404.022,256,404.022z" />
                                                                    <path
                                                                        d="M256,157.825c-54.134,0-98.174,44.041-98.174,98.174c0,54.133,44.041,98.174,98.174,98.174                            c54.133,0,98.174-44.04,98.174-98.174C354.174,201.865,310.133,157.825,256,157.825z M256,328.541                            c-39.998,0-72.54-32.542-72.54-72.54s32.542-72.54,72.54-72.54s72.54,32.542,72.54,72.54S295.998,328.541,256,328.541z" />
                                                                    <path
                                                                        d="M256,203.878c-28.739,0-52.123,23.382-52.123,52.123c0,7.078,5.737,12.817,12.817,12.817                            c7.08,0,12.817-5.739,12.817-12.817c0-14.606,11.882-26.489,26.489-26.489c7.08,0,12.817-5.739,12.817-12.817                            C268.817,209.615,263.078,203.878,256,203.878z" />
                                                                </g>
                                                            </g>
                                                        </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                    </svg></a>
                                            </div>
                                            <div class="ticket__icn">
                                                <a href="javascript:void(0);" id="downloadTicketIcon_<%=Model.SearchedTicket.ticketnumber %>"><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg"
                                                        xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 490 490"
                                                        style="enable-background:new 0 0 490 490;" xml:space="preserve">
                                                        <g>
                                                            <g>
                                                                <g>
                                                                    <path
                                                                        d="M245,0c-9.5,0-17.2,7.7-17.2,17.2v331.2L169,289.6c-6.7-6.7-17.6-6.7-24.3,0s-6.7,17.6,0,24.3l88.1,88.1 				c3.3,3.3,7.7,5,12.1,5c4.4,0,8.8-1.7,12.1-5l88.1-88.1c6.7-6.7,6.7-17.6,0-24.3c-6.7-6.7-17.6-6.7-24.3,0L262,348.4V17.1 				C262.1,7.6,254.5,0,245,0z" />
                                                                    <path
                                                                        d="M462.1,472.9v-99.7c0-9.5-7.7-17.2-17.2-17.2s-17.2,7.7-17.2,17.2v82.6H62.2v-82.6c0-9.5-7.7-17.2-17.1-17.2 				s-17.2,7.7-17.2,17.2v99.7c0,9.5,7.7,17.1,17.2,17.1h399.8C454.4,490,462.1,482.4,462.1,472.9z" />
                                                                </g>
                                                            </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                        </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                    </svg></a>
                                            </div>
                                        </div>
                                        <div class="btns-outer">
                                            <div class="btn__wrap">
                                                     <%--<%= Html.TextBoxFor(fin => fin.SearchedTicket.ticketid, new {
                                                        @type="hidden",
                                                        @name="ticketid",
                                                        @id=Model.SearchedTicket.ticketid,
                                                        @value=Model.SearchedTicket.ticketid,
                                                        })
                                                     %>--%>
                                                    <button class ="btn btn-primary" type="submit" id="btnPay_<%=Model.SearchedTicket.ticketid %>" name="submitButton" value="<%=Model.SearchedTicket.ticketid %>" formaction="TicketPaymentSummary" formmethod="post"><%=Resources.Page_PayNow_Buttontxt%></button>

                                                    <button href="javascript:void(0);" class ="btn btn-secondary" type="submit" id="btnContest_<%=Model.SearchedTicket.ticketid %>" name="submitButton" value="<%=Model.SearchedTicket.ticketid %>" formaction="PersonalInformation" formmethod="post"><%=Resources.Page_Contest_Buttontxt%></button>
                                               </div>
                                            </div>
                                    </div>


                                
                            <%} %>
                            <%else 
                              { %>
                                     <div class="border uk-card uk-margin uk-card-hover">
                                         <div class="uk-card-body">
                                             <div class="s">
                                                <label> Entered Ticket is Out Of Province. </label>
                                             </div>
                                         </div>
                                    </div>
                            <%} %>
                    
                      <%}%>
                     <%else 
                        { %>
                            <%--<div class="ticket__inner">
                                <div class="ticket__check nullticket" style="padding-left:67px;">
                                    <label class="form-control"> Entered Ticket Is Not Available or Already Contested !!!</label>
                                </div>
                            </div>--%>
                                <div class="border uk-card uk-margin uk-card-hover">
                                     <div class="uk-card-body">
                                         <div class="s">
                                            <%--<label> Entered ticket is not available or out of state....</label>--%>
                                            <label> <%=Resources.Details_EnteredTicketIsNotAvailable %></label>
                                         </div>
                                     </div>
                                </div>
                                
                    <%} %>
            
                    <br/>
                    <br/>
                        
                    
                    <%--<%=Model.AllTickets=null %>--%>
                    <%if (Model.AllTickets != null)
                        { %>
                            <%if (Model.SearchedTicket.InstallationID == Convert.ToInt32(Model.StandardInstallationList)) 
                              { %> 
                                    <%--<h2 class="heading__ticket">Other Tickets</h2>--%>
                                    <h2 class="heading__ticket"><%=Resources.Details_OtherTickets%></h2>
                                    <%foreach (var model in Model.AllTickets)
                                 { %>


                                    <%-- Hidden Details for Ticket Details --%>

                                    <div class="ticketDetails" id="detailedTicket_<%=model.ticketnumber %>">
		                                <div class="formbox filledForm">
			                                <div class="col-12 mb20">
				                                <label>Ticket code </label>
                                                 <%= Html.TextBoxFor(ticket => model.ticketnumber, new {
                                                            @type="text",
                                                            @value=model.ticketnumber,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-12 mb20 ">
				                                <label>Ticket Type</label>
				                                <%--<input type="text" value="FAILED TO PAY FOR PARKING">--%>
                                                <%= Html.TextBoxFor(ticket => model.TicketTypeDesc, new {
                                                            @type="text",
                                                            @value= model.TicketTypeID + ", " + (!String.IsNullOrEmpty(model.TicketTypeDesc) ? model.TicketTypeDesc : "NA") ,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			                                <div class="col-6 mb20">
				                                <label>Issue Date </label>
				                                <input type="text" class="form-control p-0" value="<%=model.ticketdate.ToShortDateString() %>">

                                               
			                                </div>
			                                <div class="col-6 mb20">
				                                <label>Issue Time </label>
                                                 <input type="text" class="form-control p-0" value="<%=model.ticketdate.ToShortTimeString() %>">
                                              
			                                </div>

                                            <div class="col-6 mb20">
				                                <label>Vehicle Licence Plate </label>
                                                <%= Html.TextBoxFor(ticket => model.ticketlprplate, new {
                                                            @type="text",
                                                            @value=model.ticketlprplate != null ? model.ticketlprplate : model.ticketqueryplate,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-6 mb20">
				                                <label>VIN </label>
                                                <%= Html.TextBoxFor(fin => model.ticketlprplate, new {
                                                            @type="text",
                                                            @value="-",
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			                                <div class="col-12 mb20">
				                                <label>Vehcile State </label>
                                                <%= Html.TextBoxFor(fin => model.Address.state, new {
                                                            @type="text",
                                                            @value=model.Address.state,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-12 mb20 ">
				                                <label>Address</label>
                                                <%= Html.TextBoxFor(fin => model.Address.postcode, new {
                                                            @type="text",
                                                            @value=model.Address.postcode + model.Address.city + model.Address.suburb,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			                                <div class="col-6 mb20">
				                                <label>Postal Code* </label>
                                                <%= Html.TextBoxFor(fin => model.Address.postcode, new {
                                                            @type="text",
                                                            @value=model.Address.postcode,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			                                <div class="col-6 mb20">
				                                <label>Province/State*</label>
                                                <%= Html.TextBoxFor(fin => model.Address.state, new {
                                                            @type="text",
                                                            @value=model.Address.state,
                                                            @class="form-control p-0"})
                                                        %>
			                                </div>
			
			
			                                <div class="col-12 mb20 ">
				                                <label>Public Notes</label>
			                                    <%= Html.TextAreaFor(fin => model.TickPublicNotes, new {
                                                            @type="text",
                                                            @value=model.TickPublicNotes !=null ? model.TickPublicNotes : "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
                                                            @class="form-control p-0"})
                                                        %>
                                            </div>
			                                <%--Collapsaible divs start--%>
                                            <div class="accordion whaccordian" >
                                              <div class="card">
                                                <div class="card-header">
                                                  <h2 class="mb-0">
                                                    <button class="btn  btnPhotos form-control p-0" type="button" data-toggle="collapse" data-target="#collapseOne<%=model.ticketnumber %>" aria-expanded="true" aria-controls="collapseOne">
                                                      Photos <span class="arrow"></span>
                                                    </button>
                                                  </h2>
                                                </div>

                                                <div id="collapseOne<%=model.ticketnumber %>" class="collapse" aria-labelledby="headingOne" data-parent="#accordionExample">

                                                  <div class="card-body">
                                                      <%if (model.TicketImages != null)
                                                        { %> 
                                                               <%foreach (var images in model.TicketImages) 
                                                                { %>
                                                                     <%if (!String.IsNullOrEmpty(images.base64))
                                                                        { %> 
                                                                                <img class="photos" src="data:image/png;base64,<%=images.base64%>" />
                                                                                <%--<label><%=images.ImageName %></label>--%>
                                                                     <%} %>
                                                                    <%else { %>
                                                                                <label>No photos available.</label>
                                                                    <%} %>
                                                    
                                                                <%} %>         
                                                        <%} %>
                                                       <%else { %>
                                                                  <label>No photos available.</label>
                                                         <%} %>
                                                      
                                                      <%--<img class="photos" src="../../Content/img/cardemo.jpg" />--%>
                                                  </div>
                                                </div>
                                              </div>
                                              <div class="card">
                                                <div class="card-header" id="headingTwo<%=model.ticketnumber %>">
                                                  <h2 class="mb-0">
                                                    <button class="btn  collapsed btnPreview form-control p-0" type="button" data-toggle="collapse" data-target="#collapseTwo<%=model.ticketnumber %>" aria-expanded="false" aria-controls="collapseTwo">
                                                      Ticket Preview <span class="arrow"></span>
                                                    </button>
                                                  </h2>
                                                </div>
                                                <div id="collapseTwo<%=model.ticketnumber %>" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                                  <div class="card-body">
                                                      <%if (!String.IsNullOrEmpty(model.ticketpdfbase64)) 
                                                        {%>
                                                            <canvas class="TicketPreview" id="the-canvas<%=model.ticketnumber %>"></canvas>
                                                            <%=Html.HiddenFor(fin => model.ticketpdfbase64, new
                                                                {
                                                                    @type = "hidden",
                                                                    @id = "pdfimage_" + model.ticketnumber,
                                                                    @name = "pdfimage",
                                                                    @value = model.ticketpdfbase64,
                                                                })
                                                             %>
                                                    <% } %>
                                                      <%else{%> 
                                                                <p id="ticketPreview<%=model.ticketnumber %>" style="margin: 24px 0px 6px 28px;"></p>
                                                        <%} %>
                                                      
                                                  </div>
                                                </div>
                                              </div>
                                              <div class="card">
                                                <div class="card-header" id="headingThree<%=model.ticketnumber %>">
                                                  <h2 class="mb-0">
                                                    <button class="btn collapsed btnConversation form-control p-0" type="button" data-toggle="collapse" data-target="#collapseThree<%=model.ticketnumber %>" aria-expanded="false" aria-controls="collapseThree">
                                                      Conversation <span class="arrow"></span>
                                                    </button>
                                          
                                                  </h2>
                                                </div>
                                                <div id="collapseThree<%=model.ticketnumber %>" class="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                                                  <div class="card-body">
                                                      <p>This is Under Progress</p>
                                                  </div>
                                                </div>
                                              </div>
                                            </div>
                                    <%--Collapsaible divs end--%>
                                            <div class="col-6 mb20 amount">
				                                <label class="totalamountlabel">Total Amount: </label>
                                                <span>$<%=Convert.ToDouble(model.TickBalance) %></span>
                                    
			                                </div>


			                               <%--<div class="arrowleft" id="arrowleft_<%=model.ticketnumber %>"></div>--%>
		
		                                </div>
		
                                    </div>

                                    <%-- Tickets  --%>
                                    <div class="ticket__inner" id="Ticket_<%=model.ticketnumber %>">
                                        <div class="ticket__check">
                                            <label class="form-control">
                                                

                                                 <%= Html.TextBoxFor((e) => model.ticketid, new
                                                        {
                                                            @type = "checkbox",
                                                            @id = "searchedTicket_" + model.ticketid,
                                                            @name = "checkbox-checked",
                                                            @value = model.ticketid,
                                                            @checked = true
                                                        })
                                                %>
                                                 <%=Html.HiddenFor(fin => model.InstallationID, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + model.InstallationID,
                                                            @name = "checkbox-checked",
                                                            @value = model.InstallationID,
                                                        })
                                                %>
                                            </label>

                                        </div>
                                        <div class="ticket__contains">
                                            <div class="ticket__details ticket-box-outer">


                                                <%-- new changes --%>
                                                <div class="ticket-box">
                                                    <label>Ticket #</label>
                                                    <p><%=model.ticketnumber %></p>
                                                        <%=Html.HiddenFor(fin => model.ticketnumber, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket_" + model.ticketnumber,
                                                                @name = "checkbox-checked",
                                                                @value = model.ticketnumber,
                                                            })
                                                    %>
                                                </div>

                                                <div class="ticket-box">
                                                    <label>Ticket Type</label>
                                                    <%--<p>FAILED TO PAY FOR PARKING</p>--%>
                                                    <p><%=model.TicketTypeID %>, <%=!String.IsNullOrEmpty(model.TicketTypeDesc) ? model.TicketTypeDesc : "NA" %></p>
                                                </div>

                                                <div class="ticket-box">
                                                    <label>Issue date</label>
                                                    <p><%=model.ticketdate.ToShortDateString() %></p>
                                                    <%=Html.HiddenFor(fin => model.ticketdate, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.ticketdate,
                                                                @name = "checkbox-checked",
                                                                @value = model.ticketdate,
                                                            })
                                                    %>

                                                </div>
                                                <div class="ticket-box">
                                                    <label>Issue Time</label>
                                                    <p><%=model.ticketdate.ToShortTimeString() %></p>
                                                    <%=Html.HiddenFor(fin => model.ticketdate, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.ticketdate,
                                                                @name = "checkbox-checked",
                                                                @value = model.ticketdate,
                                                            })
                                                    %>
                                                </div>
                                                
                                                <div class="ticket-box">
                                                    <label>License Plate</label>
                                                    <p><%=model.ticketlprplate != null ? model.ticketlprplate : model.ticketqueryplate %></p>

                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.ticketqueryplate, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.ticketqueryplate,
                                                                @name = "checkbox-checked",
                                                                @value = model.ticketqueryplate,
                                                            })
                                                    %>
                                                </div>
                                                <div class="ticket-box">
                                                    <label>VIN</label>
                                                    <p><%=!String.IsNullOrEmpty(model.TickVin) ? model.TickVin : "_"%></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.TickVin, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.TickVin,
                                                                @name = "checkbox-checked",
                                                                @value = model.TickVin,
                                                            })
                                                    %>
                                                </div>
                                                <div class="ticket-box">
                                                    <label>Location</label>
                                                    <%--<p>1060 Brickell Avenue</p>--%>
                                                    <p><%=model.Address.road%>,<%=model.Address.suburb%></p>
                                                    <%=Html.HiddenFor(fin => fin.SearchedTicket.Address.suburb, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.Address.suburb,
                                                                @name = "checkbox-checked",
                                                                @value = model.Address.suburb,
                                                            })
                                                    %>
                                                </div>
                                                <div class="ticket-box">
                                                    <label>Status</label>
                                                    <%--<p>1060 Brickell Avenue</p>--%>
                                                    <p><%=model.TicketStatus %></p>
                                                    <%=Html.HiddenFor(fin => model.Address.suburb, new
                                                            {
                                                                @type = "hidden",
                                                                @id = "searchedTicket" + model.Address.suburb,
                                                                @name = "checkbox-checked",
                                                                @value = model.Address.suburb,
                                                            })
                                                    %>
                                                </div>


                                            <%-- Old changes --%>




                                                
                                            </div>
                                            
                                            <div class="ticket__details ticket__outer">
                                                <div class="ticket__des">
                                                    <label>Amount Due</label>
                                                </div>
                                                <div class="ticket__des">
                                                    <h5>$<%=Convert.ToDouble(model.TickBalance) %></h5>
                                                    <%=Html.HiddenFor(fin => model.TickBalance, new
                                                        {
                                                            @type = "hidden",
                                                            @id = "searchedTicket" + model.TickBalance,
                                                            @name = "checkbox-checked",
                                                            @value = model.TickBalance,
                                                        })
                                                     %>

                                                    
                                                </div>
                                            </div>

                                            
                                        </div>
                                            <div class="ticket__right">
                                            <div class="ticket__icn">
                                                <a href="javascript:void(0);" id="detailedTicketIcon_<%=model.ticketnumber %>">
                                                    <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg"
                                                        xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 512 512"
                                                        style="enable-background:new 0 0 512 512;" xml:space="preserve">
                                                        <g>
                                                            <g>
                                                                <g>
                                                                    <path
                                                                        d="M510.977,250.975C467.408,148.537,367.321,82.344,256,82.344S44.594,148.535,1.023,250.975                            c-1.364,3.206-1.364,6.827,0,10.033C44.591,363.458,144.677,429.656,256,429.656c111.326,0,211.411-66.198,254.977-168.648                            C512.341,257.802,512.341,254.181,510.977,250.975z M256,404.022c-99.194,0-188.557-57.922-229.191-148.029                            c40.635-90.098,130-148.014,229.191-148.014c99.193,0,188.557,57.916,229.191,148.012                            C444.557,346.098,355.194,404.022,256,404.022z" />
                                                                    <path
                                                                        d="M256,157.825c-54.134,0-98.174,44.041-98.174,98.174c0,54.133,44.041,98.174,98.174,98.174                            c54.133,0,98.174-44.04,98.174-98.174C354.174,201.865,310.133,157.825,256,157.825z M256,328.541                            c-39.998,0-72.54-32.542-72.54-72.54s32.542-72.54,72.54-72.54s72.54,32.542,72.54,72.54S295.998,328.541,256,328.541z" />
                                                                    <path
                                                                        d="M256,203.878c-28.739,0-52.123,23.382-52.123,52.123c0,7.078,5.737,12.817,12.817,12.817                            c7.08,0,12.817-5.739,12.817-12.817c0-14.606,11.882-26.489,26.489-26.489c7.08,0,12.817-5.739,12.817-12.817                            C268.817,209.615,263.078,203.878,256,203.878z" />
                                                                </g>
                                                            </g>
                                                        </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                    </svg></a>
                                            </div>
                                            <div class="ticket__icn">
                                                <a href="javascript:void(0);" id="downloadTicketIcon_<%=model.ticketnumber %>">

                                                    <svg version="1.1" id="Capa_2" xmlns="http://www.w3.org/2000/svg"
                                                        xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 490 490"
                                                        style="enable-background:new 0 0 490 490;" xml:space="preserve">
                                                        <g>
                                                            <g>
                                                                <g>
                                                                    <path
                                                                        d="M245,0c-9.5,0-17.2,7.7-17.2,17.2v331.2L169,289.6c-6.7-6.7-17.6-6.7-24.3,0s-6.7,17.6,0,24.3l88.1,88.1 				c3.3,3.3,7.7,5,12.1,5c4.4,0,8.8-1.7,12.1-5l88.1-88.1c6.7-6.7,6.7-17.6,0-24.3c-6.7-6.7-17.6-6.7-24.3,0L262,348.4V17.1 				C262.1,7.6,254.5,0,245,0z" />
                                                                    <path
                                                                        d="M462.1,472.9v-99.7c0-9.5-7.7-17.2-17.2-17.2s-17.2,7.7-17.2,17.2v82.6H62.2v-82.6c0-9.5-7.7-17.2-17.1-17.2 				s-17.2,7.7-17.2,17.2v99.7c0,9.5,7.7,17.1,17.2,17.1h399.8C454.4,490,462.1,482.4,462.1,472.9z" />
                                                                </g>
                                                            </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                            <g> </g>
                                                        </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                        <g> </g>
                                                    </svg></a>
                                            </div>
                                        </div>

                                            <div class="btns-outer">
                                                <div class="btn__wrap">
                                                    <%--<%= Html.TextBoxFor((e) => model.ticketid, new
                                                            {
                                                                @type = "hidden",
                                                                @id = model.ticketid,
                                                                @name="ticketid",
                                                                @value = model.ticketid,
                                                            })
                                                    %>--%>
                                                    <%--<button class ="btn btn-primary" type="submit" id="btnPay_<%=model.ticketid %>" name="submitButton" value="<%=model.ticketid %>" formaction="TicketPaymentSummary" formmethod="post">Pay</button>--%>
                                                    <button class ="btn btn-primary" type="submit" id="btnPay_<%=model.ticketid %>" name="submitButton" value="<%=model.ticketid %>" formaction="TicketPaymentSummary" formmethod="post"><%=Resources.Page_PayNow_Buttontxt%></button>
                                                    <%--<button href="javascript:void(0);" class ="btn btn-secondary" type="submit" id="<%=model.ticketid %>" name="submitButton" value="<%=model.ticketid %>" formaction="PersonalInformation" formmethod="post">Contest</button>--%>
                                                    <button href="javascript:void(0);" class ="btn btn-secondary" type="submit" id="<%=model.ticketid %>" name="submitButton" value="<%=model.ticketid %>" formaction="PersonalInformation" formmethod="post"><%=Resources.Page_Contest_Buttontxt%></button>
                                                </div>
                                            </div>
                                        </div>
                                    <br/>
                    
                                  <%} %>
                            <%} %>
                            
                    <%} %>

                    <%else 
                        { %>
                            <%--<div class="ticket__inner">
                                <div class="ticket__check nullticket" style="padding-left:121px;">
                                    <label class="form-control"> <%=Resources.Details_NoOtherTicketAreAvailable %></label>
                                </div>
                            </div>--%>
                    <%} %>
            
            
                    
           

                    <%if (Model.SearchedTicket != null || Model.AllTickets != null) 
                        { %> 
                             <%if (Model.SearchedTicket.InstallationID == Convert.ToInt32(Model.StandardInstallationList)) 
                               {%> 
                                        <div class="btn__wrap">
                                            <button class ="btn btn-primary" formaction="TicketPaymentSummary" formmethod="post" ><%=Resources.Details_PaySelected %></button>
                                            <%--<button class ="btn btn-primary" type="submit" id="btnPay">Pay Selected</button>--%>
                                        </div>
                            <% }%>
                             <%else 
                               { %>
                                    <div class="btn__wrap">
                                        <button class ="btn btn-primary" type="submit" disabled><%=Resources.Details_PaySelected %></button>
                                        <%--<button class ="btn btn-secondary" type="submit" disabled>Contest</button>--%>
                                    </div>
                             <%} %>
                            

                            
                    <%} %>
                    <%else 
                        { %>
                            <div class="btn__wrap">
                                <button class ="btn btn-primary" type="submit" disabled>Pay Now</button>
                                <button class ="btn btn-secondary" type="submit" disabled>Contest</button>
                            </div>
                    <%} %>
                    
                    
                    

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
                </div>
        <%} %>
        
        </div>

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.0.943/pdf.min.js"></script>
    <script src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"]%>/Content/Appeal_JS/Details.js"></script>
</asp:Content>

