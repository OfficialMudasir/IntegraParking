<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_Plates %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <main class="container-fluid">
        <div class="row">
            <div class="content col-md-9 col-lg-6 p-3">
                <h1 class="h5"><i class="bky-tarif p-1 mr-2"></i> <%= Resources.Parking_Plates_SelectPlate %></h1>
                <hr class="divider1">
                <div class="error-feedback">
                    <p id="error-text"><%= Model.Error %></p>
                </div>
                <a href="#" id="AddPlate"><button class="btn btn-lg bk-btn-secondary" type="button"><i class="bky-car-add mr-2"></i> <%= Resources.Parking_Plates_AddPlate %></button></a>
                <div id="PlateBlocks"></div>                
            </div>
        </div>
    </main>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script type="text/javascript">

        var StoredPlates = null;

        $("#AddPlate,.EditPlate").on("click", function () {
            $(".css-1ozk4ba").css("display", "block");
        });

        $(document).ready(function () {

            $(".css-1ozk4ba").css("display", "block");

            $("#GoBack").css("display", "none");            

            if (localStorage.getItem("Plates") == null) {
                document.location = "AddPlate";
            }
            else {
                WritePlateBlocks();
            }

            $(".css-1ozk4ba").css("display", "none");
            
        });

        function WritePlateBlocks() {
            $("#PlateBlocks").html("");
            StoredPlates = JSON.parse(localStorage.getItem("Plates"));
            StoredPlates.forEach(AddPlateBlock);
        }

        $("#AddPlate").on("click", function () {
            document.location = "AddPlate?g=" + localStorage.getItem("GUID")
        });

        function AddPlateBlock(value, index, array) {
            var BlockTemplate = '<div class="bk-card align-items-center plate-block" onclick="SelectPlate(' + index + ')"><div class="bk-card-item bk-item-icon"><div class="col-auto p-0 bk-card-left-icons"><div class="mb-2"><a class="EditPlate" href="#" onclick="EditPlate(' + index + ', event)"><i class="bky-edit p-3"></i></a></div><div class="hr-bottom"></div><div class="mt-2"><a href="#" onclick="DeletePlate(' + index + ', event)" class="DeletePlate"><i class="bky-trash p-3"></i></a></div></div></div><div class="bk-card-item bk-item-text h4" onclick="SelectPlate(' + index + ')">' + value + '</div><div class="bk-card-item bk-item-right p-4" onclick="SelectPlate(' + index + ')"><i class="<%= Model.Theme %>-car-select h1"></i></div></div>';
            $("#PlateBlocks").html($("#PlateBlocks").html() + BlockTemplate);
        }

        function SelectPlate(i) {
            $(".css-1ozk4ba").css("display", "block");
            localStorage.setItem("CurrentPlate", StoredPlates[i])
            var g = "";
            if (localStorage.getItem("GUID") != null) {
                g = localStorage.getItem("GUID");
            }
            document.location = "SelectRate?p=" + StoredPlates[i] + "&g=" + g;
        }

        function EditPlate(i, e) {
            if (!e) var e = window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();

            document.location = "EditPlate?i=" + i;
        }

        function DeletePlate(i, e) {           
            if (!e) var e = window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();

            if (StoredPlates == null || StoredPlates.length < (i - 1)) {
                $("#error-text").html("<%= Resources.Parking_Error_PlateNotFound %>");
            }
            else {
                if (confirm("<%= Resources.Parking_Plates_DeletePlate %>" + " " + StoredPlates[i] + "?")) {
                    StoredPlates = StoredPlates.filter(function (value, index, arr) {
                        return index != i;
                    });
                    localStorage.setItem("Plates", JSON.stringify(StoredPlates));
                    WritePlateBlocks();
                }
            }
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
