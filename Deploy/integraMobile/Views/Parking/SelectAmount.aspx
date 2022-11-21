<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_Parking.Master" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= Resources.Parking_SelectAmount %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <main class="container-fluid" id="P1">
    <div class="row">
      <div class="content col-md-9 col-lg-6 p-3">
        <h1 class="h5"><i class="bky-time p-1 mr-2"></i> <%= Resources.Parking_Amount_SelectTime %></h1>
        <hr class="divider1 neg-margin">
        <% 
            if (!string.IsNullOrEmpty(Model.Error))
            {         
        %>
        <div class="error-feedback">
            <p id="error-text"><%= Model.Error %></p>
        </div>
        <%
            }
        %>
        <div class="bk-card-secondary p-3 rounded-xs">
            <div><i class="bky-ini-time mr-2"></i> <span id="CurrentDate"></span></div>
            <hr class="divider2 mt-0 mb-2">
            <div class="d-flex align-items-center pt-2 pb-2">
                <div class="text-muted line-height"><%= Resources.Parking_Amount_ParkingSessionWillExpire %></div>
                <p class="ms-auto badge bk-badge" id="badge"></p>
            </div>
            <div class="d-flex">
                <div id="EndDate">--</div>
                <div class="h1 ms-auto" id="EndTime"></div>
            </div>
        </div>
        <div class="d-flex mt-3 color-red">
            <div id="FreeParkingRemaining"><%= Resources.Parking_RemainingFreeParking %></div>
            <div class="ms-auto mr-1" id="FreeParkingRemainingAmount"></div>
        </div>
        <div class="text-center d-flex  mt-3">
            <% 
            int i = 0;
            foreach (integraMobile.Models.ParkingButton pb in Model.Buttons)
            {
                if (i % 3 == 0)
                {
            %>
        </div>
        <div class="text-center d-flex">
            <%
                }
            %>
            <button class="btn btn-lg bk-btn-select w-100 align-self-stretch" type="submit" onclick="<%= pb.Function %>"><%= pb.Text %></button>
            <%
                i++;                
            } 
            %>
        </div>
        <div class="text-center mt-3">
            <p id="PressButtonsTip"><%= Resources.Parking_Amount_PressButtons %></p>
            <table class="table table-borderless table-sm" id="Amounts" style="display:none;">
                <tbody>                    
                    <!--<tr>
                        <td><i class="bky-payment"></i> </td>
                        <td scope="row" class="text-left">
                            <%= Model.LabelParking %>
                        </td>
                        <td class="text-right" id="q"></td>
                    </tr>
                    <tr id="vat_hidden">
                        <td></td>
                        <td scope="row" class="text-left">
                            <%= Model.LabelFee %>
                        </td>
                        <td class="text-right" id="q_vat"></td>
                    </tr>-->
                    <tr>
                        <td><i class="bky-payment"></i> </td>
                        <th class="text-left"><%= Model.LabelTotal %></th>
                        <td class="text-right">
                            <strong id="q_total"></strong>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="text-center" id="PayButtons">
            <button class="btn btn-lg btn-block mt-4 bk-btn-primary w-100-sm" type="submit" id="PayButton"><%= Resources.Parking_Pay %> <span id="AmountToPay"></span></button>
            <%
            using (Html.BeginForm("ConfirmPayment", "Parking", FormMethod.Post, new { @class = "payment-request", @id="ConfirmPaymentForm" }))
            { 
            %>
            <input type="hidden" name="SelectedStep" id="SelectedStepCF" />
            <button class="btn btn-lg btn-block mt-4 bk-btn-primary w-100-sm" type="submit" id="ConfirmButton"><%= Resources.Parking_Confirm %></button>
            <%
            }
            %>
        </div>
      </div>
    </div>
  </main>
  <main class="container-fluid" id="P2">
    <div class="row">
        <div class="content col-md-9 col-lg-6 p-3">
            <h1 class="h5"><i class="bky-cards p-1 mr-2"></i> <%= Resources.Parking_PaymentMethods %></h1>
            <hr class="divider1 neg-margin">
            <button class="btn btn-lg btn-block mt-4 btn-black w-100-sm" type="submit" id="PayWithGoogle">Google Pay button here</button>
            <button class="btn btn-lg btn-block mt-4 btn-black w-100-sm" type="submit" id="PayWithApple">Apple Pay button here</button>
            <% 
            using (Html.BeginForm("PayWithCreditCard", "Parking", FormMethod.Post, new { @class = "payment-request", @id="CreditCardForm" }))
            {
            %>
            <input type="hidden" name="SelectedStep" id="SelectedStepCC" />
            <div class="g-recaptcha" data-sitekey="<%= ConfigurationManager.AppSettings["RecaptchaId"] %>" style="margin-top:20px;"></div>
            <button class="btn btn-lg btn-block mt-4 bk-btn-primary w-100-sm" type="submit" id="PayWithCreditCard"><%= Resources.Parking_DebitCreditCard %></button>
            <%
            }
            using (Html.BeginForm("PayWithPayPal", "Parking", FormMethod.Post, new { @class = "payment-request", @id="PayPalForm" }))
            { 
            %>
            <input type="hidden" name="SelectedStep" id="SelectedStepPP" />   
            <div class="g-recaptcha" data-sitekey="<%= ConfigurationManager.AppSettings["RecaptchaId"] %>" style="margin-top:20px;"></div>
            <button class="btn btn-lg btn-block mt-4 bk-btn-primary btn-paypal w-100-sm" type="submit" id="PayWithPayPal">
                <img srcset="../Content/Parking/img/paypal.png 1x, ../Content/Parking/img/paypal@2x.png 2x" src="../Content/Parking/img/paypal.png" width="71" height="20" alt="PayPal">
            </button>
            <%
            }
            %>
        </div>
    </div>
  </main>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

<script type="text/javascript">

    var Steps = [];
    var CurrentDate = "<%= Model.InitialDate %>";
    var EndDate = "";
    var IncrementTotal = 0;
    var FreeAvailableMinutes = 0;

    document.getElementById("CurrentDate").innerHTML = CurrentDate;

    $(".payment-request").on("click", function() {
        $(".css-1ozk4ba").css("display", "block");
    });

    $("#PayButton").on("click", function() {
        $("#P1").css("display", "none");
        $("#P2").css("display", "block");
    });

    <%
    int MaxT = 0;
    bool FreeAvailable = false;
    int zeroes = 0;
    int FreeAvailableMinutes = 0;
    
    foreach (integraMobile.Models.ParkingStep ps in Model.Steps)
    {
        MaxT = ps.Time;
        if (ps.QuantityTotal_Clean == 0)
        {
            zeroes++;
            if (zeroes == 2)
            {
                FreeAvailable = true;
            }
            FreeAvailableMinutes = ps.Time;
        }
    %>
    Steps.push({
        "t": "<%= ps.Time %>",
        "q": "<%= ps.Quantity %>",
        "d": "<%= ps.EndDate %>",
        "q_vat": "<%= ps.QuantityVat %>",
        "q_total": "<%= ps.QuantityTotal %>",
        "days": "<%= ps.Days %>",
        "daysclass": "<%= ps.DaysClass %>",
        "q_total_clean": "<%= ps.QuantityTotal_Clean %>"
    });

    <%
    }
    if (Model.FreeTimeTariff == true)
    { 
    %>
    var FreeAvailable = true;
    FreeAvailableMinutes = <%= FreeAvailableMinutes %>;
    $("#FreeParkingRemainingAmount").css("display", "block");
    $("#FreeParkingRemainingAmount").html(timeConvert(<%= FreeAvailableMinutes %>));
    $("#FreeParkingRemaining").css("display", "flex");
    <%
    }
    else 
    {
    %>
    var FreeAvailable = false;
    <%
    }
    %>
    var MaxT = <%= MaxT %>;    

    function timeConvert(n) {
        if (n < 0) {
            return "0 Min.";
        }
        else {
            var num = n;
            var hours = (num / 60);
            var rhours = Math.floor(hours);
            var minutes = (hours - rhours) * 60;
            var rminutes = Math.round(minutes);
            var returnValue = "";

            if (rminutes == 0 && rhours == 0) {
                returnValue = "0 Min.";
            }
            else if (rminutes == 0 && rhours == 1) {
                returnValue = rhours + " Hr.";
            }
            else if (rminutes == 0 && rhours > 1) {
                returnValue = rhours + " Hrs.";
            }
            else if (rminutes == 1 && rhours == 0) {
                returnValue = rminutes + " Min.";
            }
            else if (rminutes == 1 && rhours == 1) {
                returnValue = rhours + " Hr. " + rminutes + " Min.";
            }
            else if (rminutes == 1 && rhours > 1) {
                returnValue = rhours + " Hrs. " + rminutes + " Min.";
            }
            else if (rminutes > 1 && rhours == 0) {
                returnValue = rminutes + " Min.";
            }
            else if (rminutes > 1 && rhours == 1) {
                returnValue = rhours + " Hr. " + rminutes + " Min.";
            }
            else if (rminutes > 1 && rhours > 1) {
                returnValue = rhours + " Hrs. " + rminutes + " Min.";
            }

            return returnValue;
        }
    }

    function Increment(minutes) {

        IncrementTotal += eval(minutes);

        if (IncrementTotal > MaxT) {
            RateMaximum();
        }
        else {
            var SelectedStep = null;
            Steps.forEach(function(e) {
                if (e.t >= IncrementTotal) {
                    if (SelectedStep == null) {
                        SelectedStep = e;
                    }
                }
            });
            if (SelectedStep != null) {
                $("#SelectedStepCC").val(SelectedStep.t);
                $("#SelectedStepCF").val(SelectedStep.t);
                $("#SelectedStepPP").val(SelectedStep.t);
                EndDate = SelectedStep.d;
                IncrementTotal = eval(SelectedStep.t);
                SetPrice(SelectedStep.q, SelectedStep.q_vat, SelectedStep.q_total, SelectedStep.q_total_clean);
                SetBadge(SelectedStep.days, SelectedStep.daysclass);
                SetEndDate();

                <%
                if (Model.FreeTimeTariff == true)
                { 
                %>
                $("#FreeParkingRemainingAmount").css("display", "block");
                $("#FreeParkingRemainingAmount").html(timeConvert(FreeAvailableMinutes - SelectedStep.t));
                <%
                }
                %>
            }        
            else {
                IncrementTotal -= eval(minutes);
            }
        }
    }

    function RateStep(minutes) {

        if (eval(minutes) == 0) {
            Reset();
        }
        else {
            IncrementTotal = eval(minutes);

            var SelectedStep = null;
            Steps.forEach(function(e) {
                if (eval(e.t) == eval(IncrementTotal)) {
                    if (SelectedStep == null) {
                        SelectedStep = e;
                    }
                }
            });
            if (SelectedStep != null) {
                $("#SelectedStepCC").val(SelectedStep.t);
                $("#SelectedStepCF").val(SelectedStep.t);
                $("#SelectedStepPP").val(SelectedStep.t);
                EndDate = SelectedStep.d;
                IncrementTotal = eval(SelectedStep.t);
                SetPrice(SelectedStep.q, SelectedStep.q_vat, SelectedStep.q_total, SelectedStep.q_total_clean);
                SetBadge(SelectedStep.days, SelectedStep.daysclass);
                SetEndDate();

                <%
                if (Model.FreeTimeTariff == true)
                { 
                %>
                $("#FreeParkingRemainingAmount").css("display", "block");
                $("#FreeParkingRemainingAmount").html(timeConvert(FreeAvailableMinutes - SelectedStep.t));
                <%
                }
                %>
            }
            else {
                Reset();
            }
        }
    }

    function RateMaximum() {
        var SelectedStep = null;
        Steps.forEach(function(e) {
            SelectedStep = e;
        });
        if (SelectedStep != null) {
            $("#SelectedStepCC").val(SelectedStep.t);
            $("#SelectedStepCF").val(SelectedStep.t);
            $("#SelectedStepPP").val(SelectedStep.t);
            EndDate = SelectedStep.d;
            IncrementTotal = eval(SelectedStep.t);
            SetPrice(SelectedStep.q, SelectedStep.q_vat, SelectedStep.q_total, SelectedStep.q_total_clean);
            SetBadge(SelectedStep.days, SelectedStep.daysclass);
            SetEndDate();

            <%
            if (Model.FreeTimeTariff == true)
            { 
            %>
            $("#FreeParkingRemainingAmount").css("display", "block");
            $("#FreeParkingRemainingAmount").html(timeConvert(FreeAvailableMinutes - SelectedStep.t));
            <%
            }
            %>
        }
        else {
            Reset();
        }
    }
    
    function SetEndDate() {
        document.getElementById("EndDate").innerHTML = EndDate.split(" | ")[0];
        document.getElementById("EndTime").innerHTML = EndDate.split(" | ")[1];
    }

    function SetPrice(q, q_vat, q_total, q_clean) {
        //document.getElementById("q").innerHTML = q;
        //document.getElementById("q_vat").innerHTML = q_vat;
        document.getElementById("q_total").innerHTML = q_total;
        document.getElementById("AmountToPay").innerHTML = q_total;
        document.getElementById("Amounts").style.display = "table";
        //document.getElementById("PayButtons").style.display = "block";
        document.getElementById("PressButtonsTip").style.display = "none";

        if (q_clean == 0) {
            $("#PayButton").css("display", "none");
            $("#ConfirmButton").css("display", "inline-block");
        }
        else {
            $("#PayButton").css("display", "inline-block");
            $("#ConfirmButton").css("display", "none");
        }
    }

    function SetBadge(value, color) {
        $("#badge").html(value);
        $("#badge").removeClass("badge-day-0");
        $("#badge").removeClass("badge-day-1");
        $("#badge").removeClass("badge-day-more");
        $("#badge").addClass(color);
    }

    function Reset() {   
        document.getElementById("badge").innerHTML = "";
        //document.getElementById("q").innerHTML = "";
        //document.getElementById("q_vat").innerHTML = "";
        document.getElementById("q_total").innerHTML = "";
        document.getElementById("AmountToPay").innerHTML = "";
        document.getElementById("Amounts").style.display = "none";
        document.getElementById("EndDate").innerHTML = "--";
        document.getElementById("EndTime").innerHTML = "";
        //document.getElementById("PayButtons").style.display = "none";
        document.getElementById("PressButtonsTip").style.display = "block";
        IncrementTotal = 0;
        EndDate = "";
        <%
        if (Model.FreeTimeTariff == true)
        { 
        %>
        $("#FreeParkingRemainingAmount").css("display", "block");
        $("#FreeParkingRemainingAmount").html(timeConvert(FreeAvailableMinutes));
        <%
        }
        %>
    }
    <%
    if (string.IsNullOrEmpty(Model.Error) && Model.Tariffs.Count > 1)
    {         
    %>
    var backTarget = "SelectRate";
    <%
    }
    else 
    {
    %>
    var backTarget = "Plates";
    <%
    }
    %>

    function Back() {
        if ($("#P2").css("display") == "block") {
            $("#P1").css("display","block");
            $("#P2").css("display","none");
        }
        else {
            document.location = backTarget;
        }
    }   

    <% 
    if (Model.CreditCardEnabled)
    {
    %>
    document.getElementById("PayWithCreditCard").style.display = "block";
    document.getElementById("CreditCardForm").style.display = "block";
    <%
    }
    else { 
    %>
    document.getElementById("PayWithCreditCard").style.display = "none";
    document.getElementById("CreditCardForm").style.display = "none";
    <%
    }
    if (Model.PayPalEnabled)
    {
    %>
    document.getElementById("PayWithPayPal").style.display = "block";
    document.getElementById("PayPalForm").style.display = "block";
    <%
    }
    else { 
    %>
    document.getElementById("PayWithPayPal").style.display = "none";
    document.getElementById("PayPalForm").style.display = "none";
    <%
    }
    %>
    /*
    $('#CreditCardForm').submit(function () {
        if ($("#RecaptchaToken").length == 0) {
            // we stoped it
            event.preventDefault();
            var Step = $('#SelectedStepCC').val();        
            // needs for recaptacha ready
            grecaptcha.ready(function () {
                // do request for recaptcha token
                // response is promise with passed token
                grecaptcha.execute('<%= ConfigurationManager.AppSettings["RecaptchaId"] %>', { action: 'CreditCardForm' }).then(function (token) {
                    // add token to form
                    $('#CreditCardForm').prepend('<input type="hidden" id="RecaptchaToken" name="RecaptchaToken" value="' + token + '">');
                    $('#CreditCardForm').submit();
                });;
            });
        }
    });
    */

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
