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
				<div class="d-flex mb-2">
					<div class="lh-sm"><%= Resources.Parking_Amount_ParkingSessionWillExpire %></div>
					<div class="ms-auto badge bk-badge badge-day-0 mb-2" id="badge"><%= Resources.Parking_Amount_Today %></div>
					</div>
				<hr class="divider2 m-0">
				<div class="d-flex">
					<div class=""><%= Resources.Parking_Total %></div>
					<div class="ms-auto" id="EndDate"></div>
				</div>
				<div class="d-flex">
					<div class="h1 color-primary" id="q_total"></div>
					<div class="h1 ms-auto color-primary" id="EndTime"></div>
				</div>
			</div>
			<div class="d-flex mt-3">
				<div><i class="bky-time p-1 mr-1"></i><%= Resources.Parking_ChooseEndTime %></div>
			</div>
			<div class="text-center d-flex mt-3">
				<button id="btn1" class="btn btn-lg bk-btn-time-select-disabled w-100 align-self-stretch"><span id="btn1_date"></span> <br> 
					<span class="h1" id="btn1_time"></span>
					<div class="h1 mt-3" id="btn1_amount"></div>
					<div class="text-black-50">
						<hr class="divider2">
						<i class="bky-minus mr-2 h2"></i>
					</div>
                    <div id="btn1_index" style="display:none;"></div>
				</button>
				<button id="btn2" class="btn btn-lg bk-btn-time-select w-100 align-self-stretch"><span id="btn2_date"></span> <br> 
					<span class="h1" id="btn2_time"></span>
					<div class="h1 mt-3" id="btn2_amount"></div>
					<div class="">
						<hr class="divider2 color-primary-light">
						<i class="bky-plus mr-2 h2"></i>
					</div>
                    <div id="btn2_index" style="display:none;"></div>
				</button>                
			</div>
			<div class="text-center">
				<button class="w-100-sm btn btn-lg bk-btn-primary mt-5" type="submit" id="PayButton"><%= Resources.Parking_Pay %> <span id="AmountToPay"></span></button>
                <%
                using (Html.BeginForm("ConfirmPayment", "Parking", FormMethod.Post, new { @class = "payment-request", @id="ConfirmPaymentForm" }))
                { 
                %>
                <input type="hidden" name="SelectedStep" id="SelectedStepCF" />
                <button class="w-100-sm btn btn-lg bk-btn-primary mt-5" type="submit" id="ConfirmButton"><%= Resources.Parking_Confirm %></button>
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
    var EndDate = "";
    var IncrementTotal = 0;
    var FreeAvailableMinutes = 0;

    $(".payment-request").on("click", function() {
        $(".css-1ozk4ba").css("display", "block");
    });

    $("#PayButton").on("click", function() {
        $("#P1").css("display", "none");
        $("#P2").css("display", "block");
    });

    <%
    int FreeAvailableMinutes = 0;
    
    foreach (integraMobile.Models.ParkingStep ps in Model.Steps)
    {
        if (ps.QuantityTotal_Clean == 0)
        {
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
    %>
    if (Steps.length > 0) {
        SetStep(0);
    }

    function SetStep(index) {
        var SelectedStep = Steps[index];
        
        if (SelectedStep != null) {
            $("#SelectedStepCC").val(SelectedStep.t);
            $("#SelectedStepCF").val(SelectedStep.t);
            $("#SelectedStepPP").val(SelectedStep.t);
            EndDate = SelectedStep.d;
            IncrementTotal = eval(SelectedStep.t);
            SetPrice(SelectedStep.q, SelectedStep.q_vat, SelectedStep.q_total, SelectedStep.q_total_clean);
            SetBadge(SelectedStep.days, SelectedStep.daysclass);
            SetEndDate();

            if (index > 0) {
                SetPreviousButton(index - 1);
            }
            else {
                DisablePreviousButton();
            }

            if (index < (Steps.length - 1)) {
                SetNextButton(eval(index) + 1);
            }
            else {
                DisableNextButton();
            }
        }
        else {
            Reset();
        }
    }

    function SetNextButton(index) {
        var SelectedStep = Steps[index];
        if (SelectedStep != null) {
            document.getElementById("btn2_amount").innerHTML = SelectedStep.q_total;
            document.getElementById("btn2_date").innerHTML = SelectedStep.d.split(" | ")[0];
            document.getElementById("btn2_time").innerHTML = SelectedStep.d.split(" | ")[1];
            document.getElementById("btn2_index").innerHTML = index;
            $("#btn2").addClass("bk-btn-time-select");
            $("#btn2").removeClass("bk-btn-time-select-disabled");
        }
    }

    function SetPreviousButton(index) {
        var SelectedStep = Steps[index];
        if (SelectedStep != null) {
            document.getElementById("btn1_amount").innerHTML = SelectedStep.q_total;
            document.getElementById("btn1_date").innerHTML = SelectedStep.d.split(" | ")[0];
            document.getElementById("btn1_time").innerHTML = SelectedStep.d.split(" | ")[1];
            document.getElementById("btn1_index").innerHTML = index;
            $("#btn1").addClass("bk-btn-time-select");
            $("#btn1").removeClass("bk-btn-time-select-disabled");
        }
    }

    function DisablePreviousButton() {
        document.getElementById("btn1_amount").innerHTML = "";
        document.getElementById("btn1_date").innerHTML = "";
        document.getElementById("btn1_time").innerHTML = "";
        document.getElementById("btn1_index").innerHTML = "";
        $("#btn1").removeClass("bk-btn-time-select");
        $("#btn1").addClass("bk-btn-time-select-disabled");
    }

    function DisableNextButton() {
        document.getElementById("btn2_amount").innerHTML = "";
        document.getElementById("btn2_date").innerHTML = "";
        document.getElementById("btn2_time").innerHTML = "";
        document.getElementById("btn2_index").innerHTML = "";
        $("#btn2").removeClass("bk-btn-time-select");
        $("#btn2").addClass("bk-btn-time-select-disabled");
    }

    $("#btn1").on("click", function () {
        if ($("#btn1_index").html() != "") {
            SetStep($("#btn1_index").html());
        }
    });

    $("#btn2").on("click", function () {
        if ($("#btn2_index").html() != "") {
            SetStep($("#btn2_index").html());
        }
    });
    
    function SetEndDate() {
        document.getElementById("EndDate").innerHTML = EndDate.split(" | ")[0];
        document.getElementById("EndTime").innerHTML = EndDate.split(" | ")[1];
    }

    function SetPrice(q, q_vat, q_total, q_clean) {
        document.getElementById("q_total").innerHTML = q_total;
        document.getElementById("AmountToPay").innerHTML = q_total;

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
        document.getElementById("q_total").innerHTML = "";
        document.getElementById("AmountToPay").innerHTML = "";
        document.getElementById("EndDate").innerHTML = "--";
        document.getElementById("EndTime").innerHTML = "";
        IncrementTotal = 0;
        EndDate = "";
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
    else    
    { 
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
            event.preventDefault();
            var Step = $('#SelectedStepCC').val();
            grecaptcha.ready(function () {
                grecaptcha.execute('<%= ConfigurationManager.AppSettings["RecaptchaId"] %>', { action: 'CreditCardForm' }).then(function (token) {
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
