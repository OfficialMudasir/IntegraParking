<%@ Page Title="" Language="C#" MasterPageFile="" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html>
<head>
    <meta name="mobile-web-app-capable" content="yes">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
<form action="CC3ReplyPT" method="POST" id="StripeResponse" name="StripeResponse">
   
  <input type="hidden" name="stripeToken" id="stripeToken" />
  <input type="hidden" name="stripeEmail" id="stripeEmail" />
  <input type="hidden" name="stripeErrorCode" id="stripeErrorCode" />

  <script src="https://checkout.stripe.com/v2/checkout.js"></script>
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.js"></script>

  <button id="hiddenButton" style="visibility:hidden;">Purchase</button>

  <script>
      var bPageIsSubmitted=false;

      var token = function (res) {           
          $('#stripeToken').val(res.id);
          $('#stripeEmail').val(res.email);
          bPageIsSubmitted=true;
          $('form').submit();
      };

      var handler = StripeCheckout.configure({
          key: '<%:ViewData["key"]%>',
          image: '<%:ViewData["image"]%>',
          locale: 'auto',
          token: token              
      });

      document.getElementById('hiddenButton').addEventListener('click', function(e) {
          // Open Checkout with further options:

          var closed = function () {   
              if (!bPageIsSubmitted)
              {
                  bPageIsSubmitted=true;
                  $('#stripeErrorCode').val('window_closed');
                  $('form').submit();
              }
          };

          handler.open({
              billingAddress: false,
              amount: <%:ViewData["amount"]%>,
                currency: '<%:ViewData["currency"]%>',
                name: '<%:ViewData["commerceName"]%>',
                description: '<%:ViewData["description"]%>',
                panelLabel: '<%:ViewData["panelLabel"]%>',
                email: '<%:ViewData["email"]%>',
                allowRememberMe: false,
                closed: closed
            });
            e.preventDefault();
        });

        // Close Checkout on page navigation:
        window.addEventListener('popstate', function() {
            handler.close();             
        });

        $(document).ready(function () {
            $('#hiddenButton').click();
            return false;
        });
        
         
  </script>
</form>
</body>
</html>