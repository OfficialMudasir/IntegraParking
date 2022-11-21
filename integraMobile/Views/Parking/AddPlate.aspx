<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_AddPlate %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <main class="container-fluid">
        <div class="row">
            <div class="content col-md-9 col-lg-6 p-3">
                <h1 class="h5"><i class="bky-car-add p-1 mr-2"></i> <%= Resources.Parking_EditPlate_AddPlate %></h1>
                <hr class="divider1">
                <p><%= Resources.Parking_EditPlate_Tip %></p>
                <div class="form-group mb-3">
                    <input type="text" class="form-control" placeholder="<%= Resources.Parking_AddPlate_NewPlate %>" id="PlateField">
                    <div class="error-feedback">
                        <p id="error-text"></p>
                    </div>
                </div>
                <div class="text-center">
                    <button class="w-100-sm btn btn-lg bk-btn-primary mt-5" type="submit" onclick="AddPlate();" id="AddPlate"><%= Resources.Parking_EditPlate_AddVehicle %></button>
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
