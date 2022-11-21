<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebParking.WS.Data.WSFavArea>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.SelectorFavView_Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= Url.Content("~/Content/SelectorFavEM2.css") %>" rel="stylesheet" type="text/css" />    


    <style>
        .mainContainer {
            
        }
    </style>

    <div class="mainContainer">

        <header class="op-parking">
            <a class="btn-left btn-back" href="javascript:window.history.back();"></a>
            <h2><%= Resources.SelectorFavView_Title %></h2>
        </header>


        <div id="divSelectorFav">
        <ul>
            <li class="itemFavLoc">
                <a href="<%= Url.Action("ZoneSelected", "Park") %>?groupId=<%= Model.Group %>" onclick="$('.loading').show();">
                    <%// <img src="<%= Url.Content("~/Content/images/ParkingFavouriteLocation.png") %/>" style="height: 48px;"/> %>
                    <span><%= string.Format(Resources.SelectorFavView_FavouriteLocation, Model.GroupDescription) %></span>
                </a>
            </li>
            <li class="itemOtherLoc">
                <a href="<%= Url.Action("Zones", "Park") %>" onclick="$('.loading').show();">
                    <%// <img src="<%= Url.Content("~/Content/images/ZoneAndSector.png") %/>" style="height: 48px;" /> %>
                    <span><%= Resources.SelectorFavView_ZoneSelector %></span>
                </a>
            </li>
        </ul>
        </div>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
