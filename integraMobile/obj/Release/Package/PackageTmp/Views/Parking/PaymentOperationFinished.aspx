<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_Plates %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <main class="container-fluid">
        <div class="row">
            <div class="col-md-9 col-lg-6 p-3">
                <h1 class="h5"><i class="bky-tarif p-1 mr-2"></i> Payment Successful</h1>
                <hr>
                <div class="error-feedback">
                    <p id="error-text"></p>
                </div>
            </div>
        </div>
    </main>

</asp:Content>