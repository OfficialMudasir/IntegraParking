<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.Models.UserInfo>" %>
<%@ Import Namespace="WebParking.WS.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.OccupationView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= Url.Content("~/Content/OccupationEM2.css") %>" rel="stylesheet" type="text/css" />    


    <style>

    </style>

    <div class="mainContainer">
    
        <header class="op-parking">
            <a id="backLinkBtn" class="btn-left btn-back" href="#" ></a>
            <span class="valign"></span>
            <h2><%= Resources.OccupationView_Title %></h2>
        </header>


        <div id="divOccupation">

            <div class="cloudCO2"></div> 
            <div class="co2msg">           
            <%= string.Format(Resources.OccupationView_Coe, Model.QueryParkingOperation.Coe/10) %>
            </diV>
            <div class="occupationmsg">
            <%= Resources.OccupationView_Ocu %>
            </div>

            <div id="divOccupationDesc">
                <div class="fieldlabel">
                    <%= string.Format("{0} - {1}", Model.GroupId, Model.ZoneTar.Zones.GetDescription(Model.GroupId)) %>
                </div>
                <div class="fieldvalue">
                    <%= Model.QueryParkingOperation.OcuDesc %>
                </div>
            </div>
            <div class="carcatmsg">
            <%= Resources.OccupationView_Carcat %>
            </div>
            <div id="divCarCatDesc">
                <div class="fieldlabel">
                    <%= Model.Plate %>
                </div>
                <div class="fieldvalue">
                    <%= Model.QueryParkingOperation.CarCatDesc %>
                </div>
            </div>

            <% using (Html.BeginForm("QueryParkingOccupation", "Park", FormMethod.Post))
            { %>

            <div class="chkShowAgain">
                <input type="checkbox" name="madtarinfo" id="madtarinfo" />
                <label for="madtarinfo"><%= Resources.OccupationView_ShowAgain %></label>
            </div>
                

                <div class="occupation-submit">
                    <!--<button class="k-button k-button-icon"                 
                            title="<%= Resources.OccupationView_BackButton_Title %>"
                            onclick="history.back();">
                        <span class="k-icon k-update"></span><%= Resources.OccupationView_BackButton %>
                    </button>-->
                    <button type="submit" class="k-button k-button-icon"                 
                            title="<%= Resources.OccupationView_SubmitButton_Title %>"
                            onclick="$('.loading').show(); return true;">
                        <span class="k-icon k-update"></span><%= Resources.OccupationView_SubmitButton %>
                    </button>
                </div>

            <%  } %>

        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
