<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html lang="en">

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
	<title><%=Resources.MercadoPago_Request_Title %></title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
</head>

<body>

	<script src="<%:ViewData["MercadoPago_sdk_url"]%>"></script>
    <script>
       const mp = new MercadoPago('<%:ViewData["MercadoPago_public_key"]%>', {
           locale: "es-AR",
       });
       // Inicializa el checkout
       const checkout = mp.checkout({
           preference: {
               id: '<%:ViewData["MercadoPago_preference_id"]%>',
           },
           autoOpen: true, // Habilita la apertura automática del Checkout Pro
       });

   </script>

</body>

</html>
