<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<integraMobile.Models.ParkingModel>" %>

<html>
    <head>
        <script type="text/javascript">
            function onLoadSubmit() {
                document.PaymentMethodForm.submit();
            }

            $(document).ready(function () {
                browser.history.deleteUrl({ url: window.location.href });
            });

        </script>
    </head>
    <body onload="onLoadSubmit()">    
        <form id="PaymentMethodForm" name="PaymentMethodForm" action="<% = Model.PaymentParams.PayPal.RequestURL %>" method="post">
            <input type="hidden" name="Guid" id="Guid" value="<% = Model.PaymentParams.PayPal.Guid %>" />
            <input type="hidden" name="Email" id="Email" value="<% = Model.PaymentParams.Email %>" />
            <input type="hidden" name="Amount" id="Amount" value="<% = Model.Step.Quantity_Clean %>" />
            <input type="hidden" name="CurrencyISOCODE" id="CurrencyISOCODE" value="<% = Model.PaymentParams.CurrencyISOCODE %>" />
            <input type="hidden" name="Description" id="Description" value="<% = Model.PaymentParams.Description %>" />
            <input type="hidden" name="UTCDate" id="UTCDate" value="<% = Model.PaymentParams.UTCDate %>" />
            <input type="hidden" name="Culture" id="Culture" value="<% = Model.PaymentParams.Culture %>" />
            <input type="hidden" name="ReturnURL" id="ReturnURL" value="<% = Model.PaymentParams.ReturnURL %>" />
            <input type="hidden" name="CancelURL" id="CancelURL" value="<% = Model.PaymentParams.CancelURL %>" />
            <input type="hidden" name="QFEE" id="QFEE" value="<%= Model.Step.QuantityFee_Clean %>" />
            <input type="hidden" name="QVAT" id="QVAT" value="<%= Model.Step.QuantityVat_Clean %>" />
            <input type="hidden" name="Total" id="Total" value="<% = Model.Step.QuantityTotal_Clean %>" />
            <input type="hidden" name="ExternalId" id="ExternalId" value="<% = Model.PaymentParams.ExternalId %>" />
            <input type="hidden" name="Hash" id="Hash" value="<% = Model.PaymentParams.Hash %>" />
        </form>
    </body>
</html>