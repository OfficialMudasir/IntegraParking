<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.AppealReturnModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    SaveAppealInformation
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	 
    <link href="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/Appeal_CSS/Save_Appeal_Information.css" rel="stylesheet" />
	<div class="mainTicketContent">
		<div class="ticket__wrap  succesfully">
            <%using (Html.BeginForm(new { @role = "form", @class = "form-content" }))
              {%>
                    <%if (Model.AppealData.AppealResponse != null) 
                      { %>
                            <%if (Model.AppealData.AppealResponse.Result == 1 || Model.AppealData.AppealResponse.Result == -22)
                                { %>
                                    <div class="success">
                                        <img class="h50"  src="<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Content/img/success.png"><br>
			                            <%--<h4>The contest has been submitted successfully </h4>--%>
			                            <h4><%=Resources.SaveAppealInformation_TheContestHasBeen%> </h4>
			                            <%--<p>You will recieve a response to your request in a few days.</p>--%>
			                            <p><%=Resources.SaveAppealInformation_YouWillRecieve%></p>
			                        </div>
                            <%} %>
                            <%else 
                              { %>
                                    <div class="error-content">
                                        <img class="h50"  src="https://www.pngitem.com/pimgs/b/164-1646946_error-png.png"><br>
			                            <%--<h4 style="color:red">The contest has not submitted successfully, </h4>--%>
			                            <h4 style="color:red"><%=Resources.SaveAppealInformation_TheContestHasNot%> </h4>
			                            <%--<h4 style="color:red">please try again.</h4>--%>
			                            <h4 style="color:red"><%=Resources.SaveAppealInformation_PleaseTryAgain%></h4>
			                        </div>
                            <%} %>
                    <%} %>


            
			            <div class="backBtn">
                            <%--<button class="btn btn-primary" id="anotherSearch">Search another ticket</button>--%>
                            <button class="btn btn-primary" id="anotherSearch"><%=Resources.SaveAppealInformation_SearchAnotherTicket%></button>
				            <%--<button class="btn btn-secondary1" id="btnBackToTickets" onclick="history.go(-1); return false;">Back to tickets</button>--%>

                

                                                                     <%if (Request.UrlReferrer != null)
                                                                         {%> 
                                                                             
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
                            
                                                                     <%= Html.HiddenFor(fin => fin.SearchedTicket.InstallationID, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>
                            
                                                                     <%= Html.HiddenFor(fin => fin.SearchedTicket.ticketqueryplate, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>
                            
                                                                     <%= Html.HiddenFor(fin => fin.SearchedTicket.ticketlprplate, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>
                            
                                                                     <%= Html.HiddenFor(fin => fin.ForceInstallationId, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>

                                                                     <%= Html.HiddenFor(fin => fin.ticketnumber, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>      

                
                                                                     <%= Html.HiddenFor(fin => fin.SearchedTicket.ticketnumber, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.SearchedTicket.ticketnumber
                                                                            })
                                                                    %> 
                                                                     <%= Html.HiddenFor(fin => fin.Plate, new
                                                                            {
                                                                                @type = "hidden",
                                                                                @autocomplete = "off",
                                                                                @class = "form-control p-0",
                                                                                @value = Model.InstallationList
                                                                            })
                                                                    %>
                                        
                                            <button class="btn btn-secondary1" name="submitButton" value="Back" formaction="Details" formmethod="post"><%=Resources.SaveAppealInformation_BackToTheTickets%></button>
                                                                
                                                              
                                                                     <% } %>
                                                         

			            </div>


			
            <% } %>
            
		    
		</div>

        

	</div>
    <script type = "text/javascript" src = "http://cdn.jsdelivr.net/jquery.cookie/1.3.1/jquery.cookie.js"></script>
    <script>

        $(document).ready(function () {
            debugger;
            const params = new URLSearchParams(window.location.search)
            
            $("#anotherSearch").click(function (e) {
                //var data = e.target.val()
                $.ajax({
                    type: "POST",
                    url: "<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Appeal/RedirectToView",
                    data: {
                        data: JSON.stringify("AnotherSearch"),
                    },
                    dataType: "json",
                    success: function (result) {
                        if (result.success) {
                            //alert(result.message);
                            window.location.replace("<%=System.Configuration.ConfigurationManager.AppSettings["WebBaseURL"] %>/Appeal/Appeal?Org=Canmore&InstallationId=40064&Culture=en-US");
                        }
                        else {
                            e.preventDefault();
                            alert("There was an in contest the ticket !!!");
                        }
                    },
                    error: function (e) {
                        alert("There was error uploading files!");
                        e.preventDefault();

                    }
                });
            })

        });

    </script>

</asp:Content>

