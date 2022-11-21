<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_Plates %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <main class="container-fluid" id="P1">
        <div class="row">
            <div class="content col-md-9 col-lg-6 p-3">
                <div class="text-center m-5 mt-3">
                   <p><strong><%= Resources.Parking_ParkingSessionRegistered %></strong></p>
                   <p><strong><%= Resources.Parking_YouDontNeedReceipt %></strong></p>
                </div>
                <div class="d-flex justify-content-evenly align-items-center">
                    <div class="text-center">
                        <div class=""><%= Resources.Parking_Amount_Today %> <br> <%= Model.InitialDate.Replace(" | ", "</div><div><strong>") %>
                        </strong></div>
                    </div>
                    <div class="text-center">
                        <i class="bky-time"></i>
                    </div>
                    <div class="text-center">
                        <div class=""><%= Model.Step.Days %> <br> <%= Model.Step.EndDate.Replace(" | ", "</div><div><strong>") %>
                        </strong></div>
                    </div>
                </div>
                <hr class="divider1">
                <i class="bky-loaction mr-3"></i> <strong><%= Model.GroupText %></strong>
                <hr class="divider1">
                <i class="bky-car-select mr-3"></i> <%= Model.Plate %>
                <hr class="divider1">
                <i class="bky-tarif mr-3"></i> <%= Model.RateText %>
                <hr class="divider1">
                <% 
                switch (Model.Layout)
                {   
                    case 5:
                %>
                <table class="table table-borderless table-sm">
                    <tbody>
                        <tr>
                            <td width="30"><i class="bky-payment"></i> </td>
                            <td scope="row" class="text-left">
                                <!--"ServiceParkingLbl": "Parking (Rate+GST/PST):",-->
                                <%= Model.LabelParking %>
                            </td>
                            <td class="text-right">
                                <!--"q_plus_vat": "1200",-->
                                <%= Model.Step.QuantityPlusVat %>
                            </td>
                        </tr>
                        <tr>
                            <td width="30"><i class="bky-payment"></i> </td>
                            <td scope="row" class="text-left">
                                <!--"ServiceFeeLbl": "Service Fee:",-->
                                <%= Model.LabelFee %>
                            </td>
                            <td class="text-right">
                                <!--"q_fee_plus_vat": "35",-->
                                <%= Model.Step.QuantityFeeVat %>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <hr class="divider1">
                <div class="text-center mt-3">
                    <table class="table table-borderless table-sm color-primary">
                        <tbody>
                            <tr>
                                <td width="30"><i class="bky-card-select"></i> </td>
                                <td scope="row" class="text-left">
                                    <%= Model.LabelTotal %>
                                </td>
                                <td class="text-right h5">
                                    <%= Model.Step.QuantityTotal %>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <%                        
                        break; 
                    case 0:
                    default:
                %>
                <table class="table table-borderless table-sm">
                    <tbody>
                        <tr>
                            <td width="30"><i class="bky-payment"></i> </td>
                            <td scope="row" class="text-left">
                                <%= Model.LabelParking %>
                            </td>
                            <td class="text-right">
                                <%= Model.Step.Quantity %>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <hr class="divider1">
                <div class="text-center mt-3">
                    <table class="table table-borderless table-sm color-primary">
                        <tbody>
                            <tr>
                                <td width="30"><i class="bky-card-select"></i> </td>
                                <td scope="row" class="text-left">
                                    <%= Model.LabelTotal %>
                                </td>
                                <td class="text-right h5">
                                    <%= Model.Step.QuantityTotal %>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <%
                        break;
                } 
                %>
                <div class="text-center">
                    <!--<button class="w-100-sm btn btn-lg bk-btn-secondary mt-3" type="submit" onclick="captureImage()"><i class="bky-picture"></i> <%= Resources.Parking_SaveScreenshot %></button>-->
                    <button class="w-100-sm btn btn-lg bk-btn-secondary mt-3" type="submit" onclick="mailto()"><i class="bky-mail-send"></i> <%= Resources.Parking_SendByEmail %></button>                        
                </div>
            </div>
        </div>
    </main>

      <main class="container-fluid" id="P2">
        <div class="row">
            <div class="content col-md-9 col-lg-6 p-3">
                <h1 class="h5"><i class="bky-car-add p-1 mr-2"></i> <%= Resources.Parking_EnterEmailAddress %></h1>
                <hr class="divider1">
                <div class="form-group mb-3">
                    <input type="text" class="form-control" placeholder="<%= Resources.Parking_EmailPlaceholder %>" id="EmailField">
                    <div class="error-feedback">
                        <p id="error-text"></p>
                    </div>
                </div>
                <div class="text-center">
                    <button class="w-100-sm btn btn-lg bk-btn-primary mt-5" type="submit" onclick="SendMail();" id="SendMail"><%= Resources.Parking_Send %></button>
                </div>
            </div>
        </div>
    </main>

    <main class="container-fluid" id="P3">
        <div class="row">
            <div class="content col-md-9 col-lg-6 p-3 m-4">
                <div class="text-center">
                    <i class="icon icon-picto-email color-primary picto-big"></i>
                </div>
                <div class="text-center color-primary">
                   <p><strong><%= Resources.Parking_EmailSentCorrectly %></strong></p>
                   <p><strong><%= Resources.Parking_CheckYourInbox %></strong></p>
                </div>
            </div>
        </div>
    </main>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="../Content/js/parking/html2canvas.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#GoBack").css("display", "none");
        });

        function captureImage() {
            var container = document.body;
            html2canvas(container, {
                scale: 2
            }).then(function (canvas) {
                var link = document.createElement("a");
                document.body.appendChild(link);
                link.download = "html_image.png";
                link.href = canvas.toDataURL("image/png");
                link.target = '_blank';
                link.click();
            });
        }

        function mailto() {
            $("#P1").css("display", "none");
            $("#P2").css("display", "block");
            $("#P3").css("display", "none");
            $("#GoBack").css("display", "inline-block");
        }

        function Back() {
            $("#P1").css("display", "block");
            $("#P2").css("display", "none");
            $("#P3").css("display", "none");
            $("#GoBack").css("display", "none");
        }

        function SendMail() {
            $(".css-1ozk4ba").css("display", "block");
            $.ajax({
                type: "POST",
                url: "SendMail",
                data: { Email : $("#EmailField").val() },
                success: function (data) {
                    if (data.Success == "0") {
                        $("#error-text").html(data.Error);
                        $("#P1").css("display", "none");
                        $("#P2").css("display", "block");
                        $("#P3").css("display", "none");
                    }
                    else {
                        $("#error-text").html("");
                        $("#EmailField").val("");
                        $("#P1").css("display", "none");
                        $("#P2").css("display", "none");
                        $("#P3").css("display", "block");
                    }
                },
                error: function (xhr, textStatus, error) {
                    $("#error-text").html(xhr.statusText + " | " + textStatus + " | " + error);
                    $("#P1").css("display", "none");
                    $("#P2").css("display", "block");
                    $("#P3").css("display", "none");
                },
                complete: function () {
                    $(".css-1ozk4ba").css("display", "none");                    
                }
            });
        }

    </script>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Theme" runat="server">
    <%
        if (!string.IsNullOrEmpty(Model.Theme))
        {
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
