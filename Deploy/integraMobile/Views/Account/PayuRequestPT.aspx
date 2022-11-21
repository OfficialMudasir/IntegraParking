<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html lang="en">

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<title>PAYU Ejemplo</title>
	<link href="https://fonts.googleapis.com/css?family=Roboto:300,300i,400,400i,500" rel="stylesheet">
    <link rel="stylesheet" href="../Content/css/payu.css">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />


</head>

<body>
	<!-- crea toquen con la funcion  setCardDetails  -->
	<div class="caja">
		<h1 class="title-h1"><%=Resources.Payu_Card_Data %></h1>

		<form method="POST" id="create-form" class="clearfix" >
			<div><span class="form-row create-errors"></span></div>
			<div class="form-row first_row">
				<div class="card">
					<label for="number"> <span><%=Resources.Credit_Card %></span>
                  </label>
				</div>
				<input id="number" name="number" type="hidden" size="30" payu-content="number" autocomplete="off">
				<div id="number_container">
					<input id="number_mask" name="number_mask" type="text" size="30" placeholder="Número de la tarjeta" autocomplete="off">
					<input id="number_fake" name="number_fake" type="number" size="30" autocomplete="off">
				</div>
				<div id="mylistID">
					<div onmouseover="payU.showLabel(false)" style="visibility: hidden; position: relative;">
						<div id="_div_class_7897"><%=Resources.Payu_Card_Choose_Franchise %></div>
						<div id="_list_class_4567" class="allowHover">
							<input type="radio" value="260" name="line-style" id="VISA"><label for="VISA"></label>
							<input type="radio" value="261" name="line-style" id="MASTERCARD"><label for="MASTERCARD"></label>
							<input type="radio" value="139" name="line-style" id="AMEX"><label for="AMEX"></label>
						</div>
					</div>
				</div>
			</div>
            <div class="form-row">
                <div id="eventlog"></div>
            </div>
			<div class="form-row card-data">
				<div class="form-row-col col-75 expiracion">
					<label for="exp_month"><%=Resources.Payu_Card_Exp_Date %></label>
					<input id="exp_month" name="exp_month" type="hidden" size="2" payu-content="exp_month" placeholder="<%=Resources.Payu_Card_Month_Format %>" size="2" maxlength="2" required autocomplete="off">
					<select id="exp_month2" name="exp_month2" required autocomplete="off" placeholder="<%=Resources.Payu_Card_Month_Format %>">
						<option value="01">01</option>
						<option value="02">02</option>
						<option value="03">03</option>
						<option value="04">04</option>
						<option value="05">05</option>
						<option value="06">06</option>
						<option value="07">07</option>
						<option value="08">08</option>
						<option value="09">09</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
					</select>
					<span> / </span>
					<input id="exp_year" name="exp_year" type="hidden" size="4" payu-content="exp_year" placeholder="<%=Resources.Payu_Card_Year_Format %>" size="4" maxlength="4" required autocomplete="off">
					<select id="exp_year2" name="exp_year2" required autocomplete="off" placeholder="<%=Resources.Payu_Card_Year_Format %>"></select>				
				</div>
				<div class="form-row-col col-25">
					<label for="cvc"><span><%=Resources.Payu_Card_CVC %></span></label>
					<input id="cvc" name="cvc" type="number" size="4" payu-content="cvc" size="4" maxlength="4" required autocomplete="off">
				</div>
			</div>
			<div class="form-row">
				<label> <span><%=Resources.Payu_Card_Name %></span></label>
				<input id="name_card" name="name_card" type="text" size="30" payu-content="name_card" autocomplete="off">
			</div>
			<div class="form-row">
				<div class="documento">
					<label><%=Resources.Payu_Card_Id %></label>
					<input id="document" name="document" type="text" size="25" payu-content="document" autocomplete="off">
				</div>
			</div>
			<input payu-content="payer_id" value="<%:ViewData["payer_id"]%>" type="hidden">

			<div class="clear"></div>
			<div class="form-row">
				<button id="btnCancel" type="button">Cancelar</button>
				<button type="submit" class="submit">Aceptar</button>

			</div>
		</form>
	</div>

	<!-- CODE & LIBRARIES 	-->
	<!-- lib payu.js -->
	<script type="text/javascript" src="https://gateway.payulatam.com/ppp-web-gateway/javascript/PayU.js"></script>
	<!-- jQuery -->
	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
	<script type="text/javascript">
	    payU.setURL('<%:ViewData["api_url"]%>');
	    payU.setPublicKey('<%:ViewData["public_key"]%>');
	    payU.setAccountID('<%:ViewData["account_id"]%>');
	    payU.setListBoxID("mylistID");
	    payU.setLanguage("'<%:ViewData["lang"]%>'"); // optional
	    payU.getPaymentMethods();

	    var currMonth=<%:ViewData["month"]%>;
        var currYear=<%:ViewData["year"]%>;

	    $(document).ready(function() {
		
	        $("#exp_month2").val(pad(currMonth,2));
	        $("#exp_month").val(pad(currMonth,2));
			
	        for (i=currYear; i<currYear+15; i++) {
	            $("#exp_year2").append($("<option>", { value: i, text: i }));
	        }
	        $("#exp_year2").val(currYear);
	        $("#exp_year").val(currYear);
			
	    });
		
	    $("#exp_month2, #exp_year2").on("change", function() {
	        if (parseInt($("#exp_month2").val()) < currMonth && parseInt($("#exp_year2").val()) == currYear) {
	            $("#exp_year2").val(currYear+1);
	        }
	        $("#exp_month").val($("#exp_month2").val());
	        $("#exp_year").val($("#exp_year2").val());
	    });
		
	    // function de respuesta
	    var responseHandler = function (response) {
	        var $form = $('#create-form');

	        if (response.error) {
	            // Show the errors on the form
	            $form.find('.create-errors').text(response.error);
	            $form.find('button').prop('disabled', false);
	        } else {
	            // token contains id, last4, and card type
	            var vtoken = response.token;
	            var vpayer_id = response.payer_id;
	            var vdocument = response.document;
	            var vname = response.name;
	            var vmethod = response.method;
	            var vccard = $("#number").val();
	            var vpan = "";
	            if (vccard.length == 15) {
	                vpan = "***********" + vccard.substr(vccard.length-4, 4);
	            }
	            else if (vccard.length == 16) {
	                vpan = "************" + vccard.substr(vccard.length-4, 4);
	            }

	            //alert(JSON.stringify(response));
	            $.redirectPost("<%:ViewData["response_url"]%>",
                    {
                        cancel: 0,
                        token: vtoken,
                        payer_id: vpayer_id,
                        document: vdocument,
                        name: vname,
                        method: vmethod,
                        check: '<%:ViewData["check"]%>',
                        pan: vpan
                    });

                    //$form.find('button').prop('disabled', false);
                }
        };

            jQuery(function ($) {
                $('#create-form').submit(function (event) {
                    /*
                                    if ($(".create-errors").length) {
                                        $(".create-errors").hidde();
                                    }
                    */
                    // I@Tga :: Clean errors
                    $(".create-errors").text("");


                    var $form = $(this);

                    // Disable the submit button to prevent repeated clicks
                    $form.find('button').prop('disabled', true);

                    payU.createToken(responseHandler, $form);

                    // Prevent the form from submitting with the default action
                    return false;
                });
            });


            $(document).ready(function () {
                $("#btnCancel").click(function () {
                    //alert("cancel");
                    $.redirectPost("<%:ViewData["response_url"]%>",
				{
				    cancel: 1,
				    check: '<%:ViewData["check"]%>'
				});

                    $form.find('button').prop('disabled', false);

                });
            });


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
		
        var numberLimit = 16;
        var cvcLimit = 3;
        var backFromMonth = false;
        var cardNumberSeparator = " ";
		
        // When card is American Express, card length is 15 and cvc length is 4
        // When card is VISA or Mastercard, card length is 16 and cvc length is 3
        function setLimits() {
            if ($("#AMEX").is(':checked')) {
                numberLimit = 15;
                cvcLimit = 4;
            }
            else {
                numberLimit = 16;
                cvcLimit = 3;				
            }
        }
		
        // Takes the value in input#number and places it, formatted, into input#number_mask
        function ComposeMask() {			
            var number = $("#number").val();
            var numberMasked = "";
						
            if ($("#AMEX").is(':checked')) {
                if (number.length > 10) {
                    // Three blocks for American Express cards (4+6+5)
                    numberMasked = number.substring(0,4) + cardNumberSeparator + number.substring(4,10) + cardNumberSeparator + number.substring(10);	
                }
                else if (number.length > 4) {
                    // First two blocks for American Express cards (4+6)
                    numberMasked = number.substring(0,4) + cardNumberSeparator + number.substring(4);	
                }
                else if (number.length > 0) {
                    // First block for American Express cards (4)
                    numberMasked = number;
                }						
            }
            else {
                if (number.length > 12) {
                    // Four blocks for non-American Express cards (4+4+4+4)
                    numberMasked = number.substring(0,4) + cardNumberSeparator + number.substring(4,8) + cardNumberSeparator + number.substring(8,12) + cardNumberSeparator + number.substring(12);	
                }
                else if (number.length > 8) {
                    // First three blocks for non-American Express cards (4+4+4)
                    numberMasked = number.substring(0,4) + cardNumberSeparator + number.substring(4,8) + cardNumberSeparator + number.substring(8);	
                }
                else if (number.length > 4) {
                    // First two blocks for non-American Express cards (4+4)
                    numberMasked = number.substring(0,4) + cardNumberSeparator + number.substring(4);	
                }
                else if (number.length > 0) {
                    // First block for non-American Express cards (4)
                    numberMasked = number;
                }			
            }			

            $("#number_mask").val(numberMasked);
        }		
		
        // We don't want the input#number_mask to get focus. Focus must be on input#number_fake in order to show numeric keyboard on devices
        $("#number_mask").on("focus", function(e) {
            e.preventDefault();
            $("#number_fake").focus();
        });
		
        // On focus on input#number_fake, we set a class to fake focus on input#number_mask when in fact it is not focused
        $("#number_fake").on("focus", function() {
            $("#number_mask").addClass("fake_focus");
        });
		
        // When focus is lost, we also remove the class from input#number_mask
        $("#number_fake").on("blur", function() {
            $("#number_mask").removeClass("fake_focus");
        });

        $("#number_fake").on("keyup", function(e) {			
            // Here we avoid the deletion of the last number when coming back from input#exp_month with backspace key
            if (!(e.keyCode === 8 && backFromMonth)) {					    
                if ($.isNumeric(String.fromCharCode(e.keyCode))) {
                    // If a number is entered, we append it to input#number (if length < max)
                    if ($("#number").val().length < numberLimit) {
                        $("#number").val($("#number").val()+String.fromCharCode(e.keyCode));
                    }
                }
                else if (e.keyCode == 8) { // backspace
                    // If backspace key is pressed, we delete the last number from input#number
                    var currentNumber = $("#number").val();
                    $("#number").val(currentNumber.substring(0,currentNumber.length-1));
                }
                else if (e.keyCode == 46) { // delete
                    // If delete key is pressed, we delete the whole number from input#number
                    $("#number").val('');				
                }
                // In any case, we clear input#number_fake to show only the content of input#number_mask
                $("#number_fake").val('');				
                setLimits();					
                var x = payU.validateCard($("#number").val());
                ComposeMask();
                if ($("#number").val().length == numberLimit) {
                    // Jump to next input
                    $("#exp_month2").focus();
                }				
            }
            else {
                backFromMonth = false;
            }
        });
		
        $("#document").on("keydown", function(e) {
            if (e.keyCode === 8 && $("#document").val() === "") {
                // Jump to previous input if backspace is pressed and current input is empty
                e.preventDefault();
                $("#name_card").focus();
            }
        });
		
        $("#name_card").on("keydown", function(e) {
            if (e.keyCode === 8 && $("#name_card").val() === "") {
                // Jump to previous input if backspace is pressed and current input is empty
                e.preventDefault();
                $("#cvc").focus();
            }
        });
		
        $("#name_card").on("keyup", function(e) {
            var name = $("#name_card").val()
            var name_replaced = name.replace(/[^ a-zA-Z]/, ""); // numbers
            name_replaced = name_replaced.replace(/á/, "a");
            name_replaced = name_replaced.replace(/é/, "e");
            name_replaced = name_replaced.replace(/í/, "i");
            name_replaced = name_replaced.replace(/ó/, "o");
            name_replaced = name_replaced.replace(/ú/, "u");
            name_replaced = name_replaced.replace(/Á/, "A");
            name_replaced = name_replaced.replace(/É/, "E");
            name_replaced = name_replaced.replace(/Í/, "I");
            name_replaced = name_replaced.replace(/Ó/, "O");
            name_replaced = name_replaced.replace(/Ú/, "U");
            name_replaced = name_replaced.replace(/à/, "a");
            name_replaced = name_replaced.replace(/è/, "e");
            name_replaced = name_replaced.replace(/ì/, "i");
            name_replaced = name_replaced.replace(/ò/, "o");
            name_replaced = name_replaced.replace(/ù/, "u");
            name_replaced = name_replaced.replace(/À/, "A");
            name_replaced = name_replaced.replace(/È/, "E");
            name_replaced = name_replaced.replace(/Ì/, "I");
            name_replaced = name_replaced.replace(/Ò/, "O");
            name_replaced = name_replaced.replace(/Ù/, "U");
            $("#name_card").val(name_replaced);
        });
		
        $("#cvc").on("keydown", function(e) {
            if (e.keyCode === 8 && $("#cvc").val() === "") {
                // Jump to previous input if backspace is pressed and current input is empty
                e.preventDefault();
                $("#exp_year2").focus();
            }
            else {
                if ($("#cvc").val().length == cvcLimit) {
                    e.preventDefault();
                }
            }
        });
		
        $("#exp_year").on("keydown", function(e) {
            if (e.keyCode === 8 && $("#exp_year").val() === "") {
                // Jump to previous input if backspace is pressed and current input is empty
                e.preventDefault();
                $("#exp_month2").focus();
            }
        });
		
        $("#exp_month").on("keydown", function(e) {
            if (e.keyCode === 8 && $("#exp_month").val() === "") {
                // Jump to previous input if backspace is pressed and current input is empty
                e.preventDefault();
                backFromMonth = true;
                $("#number_fake").focus();
            }
        });

        $("#exp_month").on("keyup", function(e) {
            if ($(this).val().length === 2 && e.keyCode !== 8) {
                // Jump to next input if month length is completed
                $("#exp_year").focus();
            }
        });

        $("#exp_year").on("keyup", function(e) {
            if ($(this).val().length === 4 && e.keyCode !== 8) {
                // Jump to next input if year length is completed
                $("#cvc").focus();
            }
        });

        $("#cvc").on("keyup", function(e) {
            setLimits();
            if ($(this).val().length === cvcLimit && e.keyCode !== 8) {
                // Jump to next input if month length is completed
                $("#name_card").focus();
            }
        });
		
        function pad(n, width, z) {
            z = z || '0';
            n = n + '';
            return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
        }
		
	</script>

</body>

</html>
