<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.Models.UserInfo>" %>
<%@ Import Namespace="WebParking.WS.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ? Resources.ConfirmParkingView_TitleConfirm : Resources.ConfirmParkingView_TitleTicket) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <link href="<%= Url.Content("~/Content/ConfirmParkingEM2.css") %>" rel="stylesheet" type="text/css" />    


    <style>

    </style>

    <div class="mainContainer">


        <header class="op-parking">
            <% if (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ) { // on confirm screen, show back icon%>
                <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <% } %>
            <span class="valign"></span>
            <h2><%= (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ? Resources.ConfirmParkingView_TitleConfirm : Resources.ConfirmParkingView_TitleTicket) %></h2>
        </header>

        <div id="divConfirmParking">

        <% if (ViewContext.RouteData.Values["action"].ToString() == "TicketParking")  // on show tickeet
           {%>
        <div id="ConfirmParking">
            <span id="ConfirmParkingIco"></span>
            <h3 id="ConfirmParkingMsg">Estacionamiento realizado con éxito, gracias.</h3>
        </div>
        <%} %>
        <div id="opPlate" class="opField">
            <span><%= Model.Plate %></span>
            <%/*= (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ? string.Format(Resources.ConfirmParkingView_PlateConfirm, Model.Plate) : string.Format(Resources.ConfirmParkingView_PlateTicket, Model.Plate)) */%>
        </div>
    <%/* old version%>
        <div class="zoneTar resumeItem">
            <div class="fieldlabel">
                <%= Resources.ConfirmParkingView_Zone %>
            </div>
            <div class="fieldvalue">
                <%= Model.ZoneTar.Zones.GetDescription(Model.GroupId) %>
            </div>
            <div class="fieldlabel">
                <%= Resources.ConfirmParkingView_Tariff %>
            </div>
            <div class="fieldvalue">
                <%= Model.ZoneTar.Tariffs.GetDescription(Model.TariffId) %>
            </div>
        </div>
    <%-- end old version*/ %>
        
        <div id="opZoneTariff" class="opField">
            <span id="opZone"> <%= Model.ZoneTar.Zones.GetDescription(Model.GroupId) %></span>
            &nbsp;-&nbsp;
            <span id="opTariff"> <%= Model.ZoneTar.Tariffs.GetDescription(Model.TariffId) %> </span>
        </div>


        <% /* -- Old Version -- %>

        <div class="operationdate">
            <div class="fieldlabel">
                <%= Resources.ConfirmParkingView_IniDate %>
            </div>
            <div class="fieldvalue">
                <span class="spanDate"><%= Model.QueryParkingOperation.InitialDate.ToShortDateString() %></span>
                <span class="spanTime"><%= Model.QueryParkingOperation.InitialDate.ToShortTimeString() %></span>
            </div>
            <div class="fieldlabel">
                <%= Resources.ConfirmParkingView_EndTime %>
                <br />
                <span><%= Model.CurrentStep().D.ToShortTimeString() %></span>
            </div>
            <div class="fieldvalue">
                <%= Model.CurrentStep().D.ToString("dd MMM yyyy") %>
            </div>
            <div class="fieldlabel">
                <%= Resources.ConfirmParkingView_Duration %>
            </div>
            <div class="fieldvalue">
                <%= string.Format("{0} {1}", Model.CurrentStep().T, Resources.MinuteAbrev) %>
            </div>
        </div>

        <% -- End Old Version -- */ %>

        <div id="opIniDate" class="opField">
            <span class="IniLabel"><%= Resources.ConfirmParkingView_IniDate %></span>
            <span class="IniTime"><%= Model.QueryParkingOperation.InitialDate.ToShortTimeString() %></span>
            <span class="IniDate"><%= Model.QueryParkingOperation.InitialDate.ToShortDateString() %></span>
        </div>

        <div id="opEndDate" class=" opField">
            <span class="icoWatch"></span>
            <span class="EndLabel"><%= Resources.ConfirmParkingView_EndTime %></span>
            <span class="EndTime"><%= Model.CurrentStep().D.ToShortTimeString() %></span>
            <span class="EndDate"><%= Model.CurrentStep().D.ToString("dd MMM yyyy") %></span>
        </div>

        <div id="opDuration" class="opField">
            <span id="opDurationLabel"> <%= Resources.ConfirmParkingView_Duration %> </span>
            <span id="opDurationValue"> <%= string.Format("{0} {1}", Model.CurrentStep().T, Resources.MinuteAbrev) %> </span>
        </div>


        <div class="amount">

            <% if (Model.QueryParkingOperation.Layout == 1) { %>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceFeeLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().Q) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceFeeLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QFee) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceVATLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QVat) %>
                </div>
            <% } else if (Model.QueryParkingOperation.Layout == 2) { %>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.QSubTotalLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QSubTotal) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceVATLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QVat) %>
                </div>
            <% } else if (Model.QueryParkingOperation.Layout == 3) { %>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceFeeLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().Q) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceFeeLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QFee) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Resources.ConfirmParkingView_Bonus %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QBonusAmount) %>
                </div>
                <div class="fieldlabel tabulated">
                    <%= Model.QueryParkingOperation.ServiceVATLbl %>
                </div>
                <div class="fieldvalue tabulated">
                    <%= Model.FormatedAmount(Model.CurrentStep().QVat) %>
                </div>
            <% } %>
            <div class="fieldlabel totalLabel">
                <%= (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ? Resources.ConfirmParkingView_AmountConfirm : Resources.ConfirmParkingView_AmountTicket) %>
            </div>
            <div class="fieldvalue totalValue">
                <%= Model.FormatedAmount(Model.CurrentStep().QTotal) %>
            </div>
        </div>


        <% if (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" ) { // on need confirm %>

            <% using (Html.BeginForm("TicketParking", "Park", FormMethod.Post))
                { %>

        

            <div class="confirmParking-submit">
                <button type="submit" class="k-button k-button-icon"                 
                        title="<%= Resources.ConfirmParkingView_SubmitButton_Title %>"
                        style="display:<%= (ViewContext.RouteData.Values["action"].ToString() == "ConfirmParking" && !Model.ParkConfirmed ? "": "none") %>;"
                        onclick="$('.loading').show(); return true;">
                    <span class="k-icon k-update"></span><%= Resources.ConfirmParkingView_SubmitButton %>
                </button>
            </div>

            <%  } // close BeginForm %>


        <% } else if (ViewContext.RouteData.Values["action"].ToString() == "TicketParking") { // else on show ticket %>

           <button type="button" class="k-button k-button-icon k-button-close" title="<%= Resources.ConfirmParkingView_CloseButton_Title %>" onclick="javascript:window.location='<%: Url.Action("Logout", "Park")%>';"  >
                <span id="icoClose"></span>
                <%= Resources.ConfirmParkingView_CloseButton %>
            </button>


        <% } // end ifelse %>

        </div>
            
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
