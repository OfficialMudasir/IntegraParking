<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html lang="en">

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<title><%=Resources.MercadoPago_CVV_Request_Title %></title>
	<link href="https://fonts.googleapis.com/css?family=Roboto:300,300i,400,400i,500" rel="stylesheet">
    <link rel="stylesheet" href="../Content/css/mercadopago.css">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
</head>

<body>

	<div class="caja">
		<h1 class="title-h1"><%=Resources.Payu_Card_Data %></h1>

		<form method="" id="form-cvv" class="clearfix">

			<div><span class="form-row create-errors"></span></div>

			<div class="form-row first_row card-data">
				<input type="hidden" name="cardNumber" id="cardId" value="<%:ViewData["MercadoPago_cardid"]%>" />
                <div class="form-row">
                    <label for="cvv"><%=Resources.MercadoPago_CVV %></label>
                    <input type="text" id="cvv" required minlength="<%:ViewData["MercadoPago_cvvlength"]%>" maxlength="<%:ViewData["MercadoPago_cvvlength"]%>"/>
                </div>
                <div class="form-row1">
				    <button type="button" id="btnSubmit"><%=Resources.MercadoPago_Confirm %></button>
                    <button id="btnCancel" type="button"><%=Resources.MercadoPago_Cancel %></button>                
                </div>
            </div>
		</form>
	</div>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
	<script src="<%:ViewData["MercadoPago_sdk_url"]%>"></script>
   <script>
       const mp = new MercadoPago('<%:ViewData["MercadoPago_public_key"]%>');


       $("#btnCancel").click(function () {
           fetch("<%:ViewData["MercadoPago_response_url"]%>", {
               method: "POST",
               redirect: "follow",
               headers: {
                   "Content-Type": "application/json",
               },
               body: JSON.stringify({
                   cancel: 1,
                   error: 0
               }),
           }).then(response => {
               if (response.redirected) {
                   window.location.href = response.url;
               }
           }).catch(function (err) {
               console.info(err + " url: " + url);
           });
       })



       $("#btnSubmit").click(function () {

           (async function createToken() {
               try {
                   const token = await mp.createCardToken({
                       cardId: document.getElementById('cardId').value,
                       securityCode: document.getElementById('cvv').value,
                   })

                   fetch("<%:ViewData["MercadoPago_response_url"]%>", {
                       method: "POST",
                       redirect: "follow",
                       headers: {
                           "Content-Type": "application/json",
                       },
                       body: JSON.stringify({
                           cancel: 0,
                           error: 0,
                           token: token.id
                       }),
                   }).then(response => {
                       if (response.redirected) {
                           window.location.href = response.url;
                       }
                   }).catch(function (err) {
                       console.info(err + " url: " + url);
                   });


               } catch (e) {

                   fetch("<%:ViewData["MercadoPago_response_url"]%>", {
                       method: "POST",
                       redirect: "follow",
                       headers: {
                           "Content-Type": "application/json",
                       },
                       body: JSON.stringify({
                           cancel: 0,
                           error: 1,
                           token: ""
                       }),
                   }).then(response => {
                       if (response.redirected) {
                           window.location.href = response.url;
                       }
                   }).catch(function (err) {
                       console.info(err + " url: " + url);
                   });
                   
               }
           })()
       })

   </script>

</body>

</html>
