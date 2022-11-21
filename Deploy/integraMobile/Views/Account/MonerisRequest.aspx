<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!doctype html>
<html lang="en">

<head>
	<!-- Required meta tags -->
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

	<!-- Blinkay CSS -->
   
	<link rel="stylesheet" href="../Content/css/2020/theme-Blinkay.css">
	<link rel="stylesheet" href="../Content/css/2020/blinkay-bs4.min.css">
	<link rel="stylesheet" href="../Content/css/2020/blinkay-moneris.css">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" integrity="sha384-B4dIYHKNBt8Bc12p+WXckhzcICo0wtJAoU8YZTY5qE0Id1GSseTk6S+L3BlXeVIU" crossorigin="anonymous">

	<title></title>
</head>

<body>
	<div class="container">
		<header>
			<h1></h1>
		</header>

		<div class="row">
			<div class="col small">
				<p><%=Resources.Moneris_HT_Text1 %></p>
			</div>

		</div>
		<!--// .row -->
		<div class="row">
			<!--		//////-->


			<!-- ALERT:: modes = 1.ℹ - alert-info; 2.⚠ - alert-warning 3.🛑 - alert-danger 4.✔ - alert-success -->
			<div id="errorAlert" class="col alert alert-danger alert-dismissible fade show" role="alert">
				<strong>⚠ <%=Resources.Moneris_HT_Error %></strong>
                <span id="spanerror" class="small"></span>
				<button type="button" class="close" onClick=doCloseError() aria-label="Close">
					<span aria-hidden="true">&times;</span>
				</button>
			</div>
		</div>
		<!--               form-->
		<div id="monerisResponse" class="row"></div>
		<iframe id=monerisFrame src="<%:ViewData["moneris_hosted_tokenization_url"]%>?id=<%:ViewData["moneris_hosted_tokenization_id"]%>&
        css_body=background:white;&
        css_textbox=border-width:0px; outline:none transparent; border-bottom:2px solid %23E7E7E7;font-family:Saira, sans-serif;margin-top:0.5em;margin-bottom:1em; padding:0.2em 0; font-size:14pt;&
        pmmsg=true&
        display_labels=1&
        css_textbox_pan=width:100%;font-family:Saira,Arial,sans-serif;font-size:12pt;max-width:25ch;&
        css_textbox_exp=width:33%;font-family:Saira,Arial,sans-serif;font-size:12pt;max-width:4ch;&
        css_textbox_cvd=width:33%;font-family:Saira,Arial,sans-serif;font-size:12pt;max-width:4ch;&
        enable_exp=1&
        enable_cvd=1&
        css_input_label=color:%23979797;font-family:Saira,Arial,sans-serif;font-size:15pt;&
        css_label_pan=&
        css_label_exp=&
        css_label_cvd=&
        pan_label=<%=HttpUtility.UrlEncode(Resources.Moneris_HT_CardNumber,Encoding.GetEncoding("ISO-8859-1")) %>&
        exp_label=<%=HttpUtility.UrlEncode(Resources.Moneris_HT_ExpiryDate,Encoding.GetEncoding("ISO-8859-1")) %>&
        cvd_label=<%=HttpUtility.UrlEncode(Resources.Moneris_HT_CVD,Encoding.GetEncoding("ISO-8859-1")) %>" frameborder='0' width="200px" height="200px"></iframe>



		<!--		------->
		<div class="row justify-content-center  px-4">
			<div class="col-12 col-sm-6 col-md-4 my-2 my-md-0">
				<input id="btnsubmit" type=button onClick=doMonerisSubmit() value="<%=Resources.Moneris_HT_ButRegisterCard %>" class="btn btn-block btn-primary p-2">
			</div>
			<div class="col-12 col-sm-6 col-md-4 my-2 my-md-0">
				<input id="btncancel" type=button onClick=doCancel() value="<%=Resources.Moneris_HT_ButCancel %>" class="btn btn-block btn-primary p-2">
			</div>
		</div><!--	//	.row .justify-content-center  .px-4-->
		<!--		------->
		<!-- INFO FOOTER -->


      
		<div class="row">

			<div class="col small" >
            <hr class="w-75">

				<p><%=Resources.Moneris_HT_Text2 %></p>
			</div>

		</div>

        
		<div id="form-payment--disclaimer" class="col-12 text-center mt-4">
            
            <hr class="w-75">

			<p class="small text-center align-middle">
                <a href="https://www.pcisecuritystandards.org" target="_blank">
                            <img src="../Content/img/2020/card/PCI-DDS-Cert.png" class="PCI-DDS-Cert" alt="PCI Security Certified" data-toggle="tooltip" data-placement="bottom" title="PCI Security Certified">
                </a>
                <span class="align-middle">&nbsp;&nbsp;</span>
                <span class="align-middle"><%=Resources.Moneris_HT_AcceptedForms %></span>
				<nobr>
					<i class="fab fa-cc-visa align-middle" data-toggle="tooltip" data-placement="bottom" title="Visa"></i>
					<i class="fab fa-cc-mastercard align-middle" data-toggle="tooltip" data-placement="bottom" title="Masterd Card"></i>
					<i class="fab fa-cc-amex align-middle" data-toggle="tooltip" data-placement="bottom" title="American Express"></i>
					<i class="fab fa-cc-paypal align-middle" data-toggle="tooltip" data-placement="bottom" title="PayPal"></i>
					<i class="fab fa-cc-stripe align-middle" data-toggle="tooltip" data-placement="bottom" title="Stripe"></i>
				</nobr>
			</p>
		</div><!-- // .form-payment--disclaimer -->





		<!--

Mandatory Variables

    Id -> Required - Provided by the Hosted Tokenization profile configuration tool in the MRC.

    css_body -> Required - CSS applied to the body.  By default margin and padding is set to 0.

    css_textbox -> Required - CSS applied to all text boxes in general.


Optional variables

    pmmsg -> Recommended - Forces form to only accept message of 'tokenize'.

    display_labels -> Optional – 0 for no labels, 1 for traditional labels, 2 for place holder labels.

    css_textbox_pan -> Optional - CSS applied to the pan text box specifically.

    css_textbox_exp -> Optional - CSS applied to the expiry date text box specifically.

    css_textbox_cvd -> Optional - CSS applied to the CVD text box specifically.

    enable_exp -> Optional - Must be set to 1 for expiry date text box to be displayed (Format: MMYY)

    enable_cvd -> Optional - Must be set to 1 for CVD text box to be displayed

    css_input_label -> Optional – CSS for input labels

    css_label_pan -> Optional – CSS for card number label

    css_label_exp -> Optional – CSS for expiry date label

    css_label_cvd -> Optional – CSS for CVD label

    pan_label -> Optional – text for card number label (default is “Card Number”)

    exp_label -> Optional – text for expiry date label (default is “Expiry Date”)

    cvd_label -> Optional – text for CVD label (default is “CVD”)

-->
	</div><!-- // .container -->

	<!-- Optional JavaScript -->
	<!-- jQuery first, then Popper.js, then Bootstrap JS -->
	<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
	<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js" integrity="sha384-OgVRvuATP1z7JjHLkuOU7Xw704+h835Lr+6QL9UvYjZE3Ipu6Tp75j7Bh/kR0JKI" crossorigin="anonymous"></script>

	<!--	MONERIS SCRIPTS-->
	<script>


	    $(document).ready(function () {

	        $("#spanerror").text("");
	        $("#btnsubmit").prop('disabled', false);
	        $("#btncancel").prop('disabled', false);
	    });

	    function doMonerisSubmit() {
	        // Disable the submit button to prevent repeated clicks
	        $("#btnsubmit").prop('disabled', true);
	        $("#btncancel").prop('disabled', true);
	        var monFrameRef = document.getElementById('monerisFrame').contentWindow;
	        monFrameRef.postMessage('tokenize', '<%:ViewData["moneris_hosted_tokenization_url"]%>');
	        //change link according to table above 
	        return false;
        }


        function doCancel() {
            $("#btnsubmit").prop('disabled', true);
            $("#btncancel").prop('disabled', true);
            var vResponseCode = "914";
            var vDataKey = "";
            var vErrorMessage = "transaction_cancelled";
            var vBin = "";


            $.redirectPost("<%:ViewData["response_url"]%>",
                                            {
                                                ResponseCode: vResponseCode,
                                                DataKey: vDataKey,
                                                ErrorMessage: vErrorMessage,
                                                Bin: vBin,
                                            });
        }


        function doCloseError() {
            $("#errorAlert").hide();
        }

        var respMsg = function (e) {
            var respData = eval("(" + e.data + ")");
            ///document.getElementById("monerisResponse").innerHTML = e.origin + " SENT " + " - " +
            //alert(respData.responseCode + "-" + respData.dataKey + "-" + respData.errorMessage);
            //document.getElementById("monerisFrame").style.display = 'none';


            var vResponseCode = respData.responseCode;
            var vDataKey = respData.dataKey;
            var vErrorMessage = respData.errorMessage;
            var vBin = respData.bin;

            if (vResponseCode == "001") {


                $.redirectPost("<%:ViewData["response_url"]%>",
                                               {
                                                   ResponseCode: vResponseCode,
                                                   DataKey: vDataKey,
                                                   ErrorMessage: vErrorMessage,
                                                   Bin: vBin,
                                               });
            }
            else {

                vResponseCode = vResponseCode + '';
                var errorCodes = vResponseCode.split(',');
                var errorMsg = "";

                errorCodes.sort();

                for (var i = 0; i < errorCodes.length; i++) {

                    switch (errorCodes[i]) {

                        case "940":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Invalid_Profile1 %>";
                            }
                            break;
                        case "941":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Error_Generating_Token %>";
                            }
                            break;
                        case "942":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Invalid_Profile2 %>";
                            }
                            break;
                        case "943":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Invalid_Card_Number %>";
                            }
                            break;
                        case "944":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Invalid_Date %>";
                            }
                            break;
                        case "945":
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Invalid_CVD %>";
                            }
                            break;
                        default:
                            {
                                errorMsg = errorMsg + "<br>- <%=Resources.Moneris_HT_Error_Unknown %>";
                            }
                            break;
                    }
                }

                $("#errorAlert").show();
                $("#spanerror").html(errorMsg);
                $("#btnsubmit").prop('disabled', false);
                $("#btncancel").prop('disabled', false);
            }


        }

        window.onload = function () {
            if (window.addEventListener) {
                window.addEventListener("message", respMsg, false);
            } else {
                if (window.attachEvent) {
                    window.attachEvent("onmessage", respMsg);
                }
            }
        }


        // jquery extend function
        $.extend({
            redirectPost: function (location, args) {
                var form = $('<form></form>');
                form.attr("method", "post");
                form.attr("action", location);

                $.each(args, function (key, value) {
                    var field = $('<input></input>');

                    field.attr("type", "hidden");
                    field.attr("name", key);
                    field.attr("value", value);

                    form.append(field);
                });
                $(form).appendTo('body').submit();
            }
        });

	</script>

	<!--BLINKAY SCRIPTS-->
	<script>
	    // Event On close alert Error
	    $('#errorAlert').on('closed.bs.alert', function () {
	        // do something...
	        //ej. location.reload();

	    });

	    // tooltips
	    $(function () {
	        $('[data-toggle="tooltip"]').tooltip()
	    });
	</script>
</body></html>
