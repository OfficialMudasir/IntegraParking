<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_AddPlate %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<main class="container-fluid">
    <div class="row">
        <div class="content col-md-9 col-lg-6 p-3">
        <h1 class="h5"><i class="bky-tarif p-1 mr-2"></i> <%= Resources.Parking_Pay %></h1>
        <hr class="divider1">
            <div class="bk-card bk-card-grid align-items-center p-4">
                <div class="bk-card-grid-item-title bk-item-text">
                    <p class="h4"><a href=""><%= Resources.Parking_PayWithoutRegistration %></a></p>
                </div>
                <div class="bk-card-grid-item-icons bk-item-right">
                    <img class="ml-2" srcset="./img/visa-icon.png 1x, ./img/visa-icon@2x.png 2x" src="./img/visa-icon.png" width="49" height="49" alt="visa"></a>
                    <img class="ml-2" srcset="./img/mastercard-icon.png 1x, ./img/mastercard-icon@2x.png 2x" src="./img/mastercard-icon.png" width="49" height="49" alt="mastercard"></a>
                    <img class="ml-2" srcset="./img/amex-icon.png 1x, ./img/amex-icon@2x.png 2x" src="./img/amex-icon.png" width="49" height="49" alt="amex"></a>
                    <img class="ml-2" srcset="./img/paypal-icon.png 1x, ./img/paypal-icon@2x.png 2x" src="./img/paypal-icon.png" width="49" height="49" alt="paypal"></a>
                </div>
                <div class="bk-card-grid-item-content bk-item-text">
                    <p><strong><%= Resources.Parking_PayAsOccasionalParker %></strong></p>
                    <ol>
                        <li><%= Resources.Parking_PayAsOccasionalParker_P1 %></li>
                        <li><%= Resources.Parking_PayAsOccasionalParker_P2 %></li>
                    </ol>
                </div>
            </div>
            <div class="bk-card bk-card-grid align-items-center p-4">
                <div class="bk-card-grid-item-title bk-item-text">
                <p class="h4"><a href=""><%= Resources.Parking_PayDownloadBlinkayApp %></a></p>
                </div>
                <div class="bk-card-grid-item-icons bk-item-right">
                <img class="ml-2" srcset="./img/ios-icon.png 1x, ./img/ios-icon@2x.png 2x" src="./img/ios-icon.png" width="49" height="49" alt="ios"></a>
                <img class="ml-2" srcset="./img/android-icon.png 1x, ./img/android-icon@2x.png 2x" src="./img/android-icon.png" width="49" height="49" alt="android"></a>
                </div>
                <div class="bk-card-grid-item-content bk-item-text">
                <p><strong><%= Resources.Parking_PayAsARegularParker %></strong></p>
                    <ol>
                        <li><%= Resources.Parking_PayAsARegularParker_LI1 %></li>
                        <li><%= Resources.Parking_PayAsARegularParker_LI2 %></li>
                        <li><%= Resources.Parking_PayAsARegularParker_LI3 %></li>
                        <li><%= Resources.Parking_PayAsARegularParker_LI4 %></li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
</main>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            browser.history.deleteUrl({ url: window.location.href });
        });

        $("#AddPlate").on("click", function () {
            $(".css-1ozk4ba").css("display", "block");
        });

        function AddPlate() {
            if ($("#PlateField").val() != "") {
                var StoredPlates = JSON.parse(localStorage.getItem("Plates"));
                if (StoredPlates == null) {
                    StoredPlates = [];
                }
                if (!StoredPlates.includes($("#PlateField").val().toString().trim())) {
                    StoredPlates.push($("#PlateField").val());
                    localStorage.setItem("Plates", JSON.stringify(StoredPlates));
                    document.location = "SelectRate?p=" + $("#PlateField").val();
                }
                else {
                    $("#error-text").html("<%= Resources.Parking_Error_PlateAlreadyExists %>");
                }
            }
            else {
                $("#error-text").html("<%= Resources.Parking_Error_PlateNotValid %>");
            }
        }

        function Back() {
            document.location = "Plates";
        }

    </script>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Theme" runat="server">
    <%
        if (!string.IsNullOrEmpty(Model.Theme)) {
    %>
    <link href="../Content/Parking/css/<%= Model.Theme %>.css?v=0.1" rel="stylesheet" type="text/css">
    <%
        } 
    %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Logo" runat="server">
    <%
        if (!string.IsNullOrEmpty(Model.Theme)) {
    %>
    <a class="navbar-brand text-center me-0 px-3 flex-grow-1" href="Plates"><img srcset="../Content/Parking/img/banff-logo.png 1x, ../Content/Parking/img/<%= Model.Theme %>-logo@2x.png 2x" src="../Content/Parking/img/<%= Model.Theme %>-logo.png" width="110" height="40" alt="<%= Model.Theme.ToUpper() %> LOGO"></a>
    <%
        } 
        else {
            Response.Write("<a class=\"navbar-brand text-center me-0 px-3 flex-grow-1\" href=\"Plates\">" + Resources.Parking_PayParking + "</a>");
        }
    %>
</asp:Content>
