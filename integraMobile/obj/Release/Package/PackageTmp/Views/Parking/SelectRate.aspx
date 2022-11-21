<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_SelectRate %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <main class="container-fluid">
    <div class="row">
      <div class="content col-md-9 col-lg-6 p-3">
        <h1 class="h5"><i class="bky-tarif p-1 mr-2"></i> <%= Resources.Parking_Rates_SelectRate %></h1>
        <hr>
        <% 
            if (Model.Tariffs == null || Model.Tariffs.Count == 0)
            {         
        %>
        <div class="error-feedback">
            <p id="error-text"><%= Resources.Parking_Error_NoRates_Description %></p>
        </div>
        <%
            }
            else 
            {
                foreach (integraMobile.Models.ParkingTariff pt in Model.Tariffs)
                { 
        %>
        <div class="bk-card align-items-center border-right-5 <!--border-green--> rate-block" onclick="SelectRate(<%= pt.Id %>)">
            <div class="bk-card-item bk-item-icon"><i class="bky-info p-3"></i></div>
            <div class="bk-card-item bk-item-divisor bk-item-text">(<%= pt.Description %>)</div>
        </div>          
        <%
                }
            }    
        %>
      </div>
    </div>
  </main>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">
        
    $(document).ready(function () {
        if (localStorage.getItem("GUID") == null) {
            localStorage.setItem("GUID", "<%= Model.Guid %>");
        }
    });

	function Back() {
	    document.location = "Plates";
	}

	function SelectRate(rateId) {
	    $(".css-1ozk4ba").css("display", "block");
	    document.location = "SelectAmount?r=" + rateId;
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
