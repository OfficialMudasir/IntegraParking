<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html lang="en">

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<title><%=Resources.MercadoPago_Request_Title %></title>
	<link href="https://fonts.googleapis.com/css?family=Roboto:300,300i,400,400i,500" rel="stylesheet">
    <link rel="stylesheet" href="../Content/css/mercadopago.css">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
</head>

<body>

	<div class="caja">

		<h1 class="title-h1"><%=Resources.MercadoPago_Card_Data %></h1>

		<form method="POST" id="form-checkout" class="clearfix">

			<div><span class="form-row create-errors"></span></div>

			<div class="form-row first_row card-data">
                <div class="form-row">
    				<input type="text" name="cardNumber" id="form-checkout__cardNumber" />                
                </div>
                <div class="form-row">
				    <input type="text" name="cardExpirationDate" id="form-checkout__cardExpirationDate" />
                </div>
                <div class="form-row">
				    <input type="text" name="cardholderName" id="form-checkout__cardholderName"/>
                </div>
                <div class="form-row">
				    <input type="email" name="cardholderEmail" id="form-checkout__cardholderEmail" value="<%:ViewData["MercadoPago_email"]%>"/>
                </div>
                <div class="form-row">
				    <input type="text" name="securityCode" id="form-checkout__securityCode" />
                </div>
                <div class="form-row">
                    <div class="row-middle">
				        <select name="issuer" id="form-checkout__issuer"></select>
                    </div>
                    <div class="row-middle">
				        <select name="identificationType" id="form-checkout__identificationType"></select>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="form-row">
				    <input type="text" name="identificationNumber" id="form-checkout__identificationNumber"/>
                </div>
                <div class="form-row">
			    	<select style='display:none;' name="installments" id="form-checkout__installments"></select>
                </div>
                <div class="form-row">
		    		<button type="submit" id="form-checkout__submit"><%=Resources.MercadoPago_Pay %></button>
                    <button id="btnCancel" type="button"><%=Resources.MercadoPago_Cancel %></button>
                </div>
                <div class="form-row">
    				<progress value="0" class="progress-bar"><%=Resources.MercadoPago_Loading %></progress>
                </div>
            </div>

		</form>

	</div>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
	<script src="<%:ViewData["MercadoPago_sdk_url"]%>"></script>
   <script>
       const mp = new MercadoPago('<%:ViewData["MercadoPago_public_key"]%>');
       const cardForm = mp.cardForm({
           amount: "<%:ViewData["MercadoPago_amount"]%>",
           autoMount: true,
           form: {
               id: "form-checkout",
               cardholderName: {
                   id: "form-checkout__cardholderName",
                   placeholder: "<%=Resources.MercadoPago_Card_Holder_Name %>",
               },
               cardholderEmail: {
                   id: "form-checkout__cardholderEmail",
                   placeholder: "<%=Resources.MercadoPago_Card_Holder_Email %>",
               },
               cardNumber: {
                   id: "form-checkout__cardNumber",
                   placeholder: "<%=Resources.MercadoPago_Card_Number %>",
               },
               cardExpirationDate: {
                   id: "form-checkout__cardExpirationDate",
                   placeholder: "<%=Resources.MercadoPago_Card_Expiration_Date %>",
               },
               securityCode: {
                   id: "form-checkout__securityCode",
                   placeholder: "<%=Resources.MercadoPago_Card_Security_Code %>",
               },
               installments: {
                   id: "form-checkout__installments",
                   placeholder: "<%=Resources.MercadoPago_Installments %>",
               },
               identificationType: {
                   id: "form-checkout__identificationType",
                   placeholder: "<%=Resources.MercadoPago_Identification_Type %>",
               },
               identificationNumber: {
                   id: "form-checkout__identificationNumber",
                   placeholder: "<%=Resources.MercadoPago_Identification_Number %>",
               },
               issuer: {
                   id: "form-checkout__issuer",
                   placeholder: "<%=Resources.MercadoPago_Issuer %>",
               },
           },
           callbacks: {
               onFormMounted: error => {
                   if (error) return console.warn("Form Mounted handling error: ", error);
                   console.log("Form mounted");
               },
               onSubmit: event => {
                   event.preventDefault();

                   const {
                       paymentMethodId: payment_method_id,
                       issuerId: issuer_id,
                       cardholderEmail: email,
                       amount,
                       token,
                       installments,
                       identificationNumber,
                       identificationType,
                   } = cardForm.getCardFormData();

                   fetch("<%:ViewData["MercadoPago_response_url"]%>", {
                       method: "POST",
                       redirect: "follow",
                       headers: {
                           "Content-Type": "application/json",
                       },
                       body: JSON.stringify({
                           cancel: 0,
                           token,
                           issuer_id,
                           payment_method_id,
                           transaction_amount: Number(amount),
                           installments: Number(installments),
                           description: "<%:ViewData["MercadoPago_description"]%>",
                           payer: {
                               email,
                               identification: {
                                   type: identificationType,
                                   number: identificationNumber,
                               },
                           },
                       }),
                   }).then(response => {
                       if (response.redirected) {
                           window.location.href = response.url;
                       }
                   }).catch(function (err) {
                           console.info(err + " url: " + url);
                       });
;
               },
               onFetching: (resource) => {
                   console.log("Fetching resource: ", resource);

                   // Animate progress bar
                   const progressBar = document.querySelector(".progress-bar");
                   progressBar.removeAttribute("value");

                   return () => {
                       progressBar.setAttribute("value", "0");
                   };
               }
           },
       });

       

       $("#btnCancel").click(function () {
           //alert("cancel");
           fetch("<%:ViewData["MercadoPago_response_url"]%>", {
               method: "POST",
               redirect: "follow",
               headers: {
                   "Content-Type": "application/json",
               },
               body: JSON.stringify({
                   cancel: 1,         
                   }),
           }).then(response => {
               if (response.redirected) {
                   window.location.href = response.url;
               }
           }).catch(function (err) {
               console.info(err + " url: " + url);
           });
       })

   </script>

</body>

</html>
