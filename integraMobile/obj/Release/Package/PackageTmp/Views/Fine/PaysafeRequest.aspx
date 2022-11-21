<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCtype html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Paysafe Redirect</title>    
    <script type="text/jscript" src = "https://hosted.paysafe.com/js/v1/latest/paysafe.min.js"></script>
    <!-- <%:ViewData["paysafe_include_js_url"]%> -->

    <script type="text/jscript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/jscript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.15.0/jquery.validate.min.js"></script>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.min.css" />

    <link rel="stylesheet" href="../Content/css/Paysafe.css">

    <meta name="viewport" content="width=device-width, initial-scale=1">

</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 col-md-4">
                <!-- CREDIT CARD FORM STARTS HERE -->
                <div class="panel panel-default credit-card-box">
                    <div class="panel-heading display-table" >
                        <div class="row display-tr" >
                            <h3 class="panel-title display-td" ><%=Resources.Paysafe_PaymentDetails %></h3>
                            <div class="display-td" >                            
                                <img class="img-responsive pull-right" src="https://developer.paysafe.com/fileadmin/content/logos/accepted_cards_by_paysafejs.png">
                            </div>
                        </div>                    
                    </div>
                    <div class="panel-body">
                        <form role="form" id="payment-form" method="POST" action="javascript:void(0);">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="cardNumber"><%=Resources.Paysafe_CardNumber %></label>
                                        <div class="input-group">
                                            <div 
                                                class="form-control"
                                                id="cardNumber"
                                            > </div>
                                            <span class="input-group-addon"><i class="fa fa-credit-card"></i></span>
                                        </div>
                                    </div>                            
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-7 col-md-7">
                                    <div class="form-group">
                                        <label for="cardExpiry"><span class="hidden-xs"><%=Resources.Paysafe_CardExpDate %></span><span class="visible-xs-inline">EXP</span> DATE</label>
                                        <div 
                                            class="form-control" 
                                            id="cardExpiry"
                                        ></div>
                                    </div>
                                </div>
                                <div class="col-xs-5 col-md-5 pull-right">
                                    <div class="form-group">
                                        <label for="cardCVC"><%=Resources.Paysafe_CardCVC %></label>
                                        <div 
                                            class="form-control"
                                            id="cardCVC"
                                        ></div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <label for="zip"><%=Resources.Paysafe_ZipCode %></label>                                        
                                        <input id="zip" name="zip" type="text" class="form-control" maxlength="10" required autocomplete="off" />
                                    </div>                            
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <button class="pay btn btn-success btn-lg btn-block" type="button"><%=Resources.Paysafe_PayButton %></button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <button class="cancel btn btn-cancel btn-lg btn-block" type="button"><%=Resources.Paysafe_CancelButton %></button>
                                </div>
                            </div>
                            <div class="row" style="display:none;">
                                <div class="col-xs-12">
                                    <p class="payment-errors"></p>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>            
                <!-- CREDIT CARD FORM ENDS HERE -->
            
            
            </div>            
                
        </div>
    </div>

    <script type="text/jscript">

        // Base 64 encoded version of the Single-Use-Token API key.
        // Create the key below by concatenating the API username and password
        // separated by a colon and Base 64 encoding the result
        var apiKey = "<%:ViewData["paysafe_api_key"]%>";
        var $form = $('#payment-form');
        $form.find('.pay').prop('disabled', true);
        var options = {

            // select the Paysafe test / sandbox environment
            environment: "<%:ViewData["paysafe_environment"]%>",

            // set the CSS selectors to identify the payment field divs above
            // set the placeholder text to display in these fields
            fields: {
                cardNumber: {
                    selector: "#cardNumber",
                    placeholder: "<%=Resources.Paysafe_CardNumber_Holder %>"
                },
                expiryDate: {
                    selector: "#cardExpiry",
                    placeholder: "<%=Resources.Paysafe_CardExpDate_Holder %>"
                },
                cvv: {
                    selector: "#cardCVC",
                    placeholder: "<%=Resources.Paysafe_CardCVC_Holder %>"
                }
            }
        };

        // initalize the hosted iframes using the SDK setup function
        paysafe.fields.setup(apiKey, options, function (instance, error) {

            if (error) {
                console.log(error);
                //window.location = "<%:ViewData["paysafe_url_failure"]%>";
            } else {

                var payButton = $form.find('.pay');
                var cancelButton = $form.find('.cancel');

                console.log(payButton);

                instance.fields("cvv cardNumber expiryDate").valid(function (eventInstance, event) {
                    $(event.target.containerElement).closest('.form-control').removeClass('error').addClass('success');

                    if (paymentFormReady()) {
                        $form.find('.pay').prop('disabled', false);
                    }
                });

                instance.fields("cvv cardNumber expiryDate").invalid(function (eventInstance, event) {
                    $(event.target.containerElement).closest('.form-control').removeClass('success').addClass('error');
                    if (!paymentFormReady()) {
                        $form.find('.pay').prop('disabled', true);
                    }
                });

                instance.fields.cardNumber.on("FieldValueChange", function (instance, event) {
                    console.log(instance.fields.cardNumber);

                    if (!instance.fields.cardNumber.isEmpty()) {
                        var cardBrand = instance.getCardBrand().replace(/\s+/g, '');
                        console.log(cardBrand);

                        switch (cardBrand) {
                            case "AmericanExpress":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-amex');
                                break;
                            case "MasterCard":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-mastercard');
                                break;
                            case "Visa":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-visa');
                                break;
                            case "Diners":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-diners-club');
                                break;
                            case "JCB":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-jcb');
                                break;
                            case "Maestro":
                                $form.find($(".fa")).removeClass('fa-credit-card').addClass('fa-cc-discover');
                                break;
                        }
                    }
                    else {
                        $form.find($(".fa")).removeClass().addClass('fa fa-credit-card');
                    }
                });

                payButton.bind("click", function (event) {
                    instance.tokenize(function (instance, error, result) {
                        if (error) {
                            console.log(error);
                            $form.find('.pay').html('<%=Resources.Paysafe_PayButton %>').prop('disabled', false);
                            /* Show Paysafe errors on the form */
                            $form.find('.payment-errors').text(error.displayMessage);
                            $form.find('.payment-errors').closest('.row').show();
                            //window.location = "<%:ViewData["paysafe_url_failure"]%>";
                        } else {
                            /* Visual feedback */
                            $form.find('.pay').html('<%=Resources.Paysafe_PayButton_Processing %> <i class="fa fa-spinner fa-pulse"></i>');
                            /* Hide Paysafe errors on the form */
                            $form.find('.payment-errors').closest('.row').hide();
                            $form.find('.payment-errors').text("");

                            // response contains token          
                            console.log(result.token);

                            window.location = "<%:ViewData["paysafe_url_success"]%>" + "?token=" + result.token + "&paymentMethod=Cards&zip=" + $form.find("#zip").val();
                        }
                    });
                });

                cancelButton.bind("click", function (event) {
                    window.location = "<%:ViewData["paysafe_url_failure"]%>" + "?cancel=1";
                });
                }
        });

            paymentFormReady = function () {
                if ($form.find('#cardNumber').hasClass("success") &&
                    $form.find('#cardExpiry').hasClass("success") &&
                    $form.find('#cardCVC').hasClass("success") &&
                    $form.find('#zip').hasClass("success")) {
                    return true;
                } else {
                    return false;
                }
            }

            $('#zip').on('input', function () {
                var input = $(this);
                if (input.val()) {
                    input.removeClass("error").addClass("success");
                    if (paymentFormReady()) {
                        $form.find('.pay').prop('disabled', false);
                    }
                }
                else {
                    input.removeClass("success").addClass("error");
                    if (!paymentFormReady()) {
                        $form.find('.pay').prop('disabled', true);
                    }
                }
            });

    </script>
</body>
</html>
