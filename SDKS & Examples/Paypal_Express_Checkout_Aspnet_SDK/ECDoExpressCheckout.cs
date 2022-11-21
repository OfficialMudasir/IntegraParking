/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * DoExpressCheckoutPayment SOAP example; last modified 08MAY23. 
 *
 * Complete an Express Checkout transaction.  
 */
using System;
using com.paypal.sdk.services;
using com.paypal.soap.api;
using com.paypal.sdk.profiles;
/**
 * PayPal .NET SDK sample code
 */
namespace GenerateCodeSOAP
{
	public class ECDoExpressCheckout
	{
		public ECDoExpressCheckout()
		{
		}

		public string ECDoExpressCheckoutCode(string token, string payerID, string paymentAmount, PaymentActionCodeType paymentAction, CurrencyCodeType currencyCodeType)
		{
			CallerServices caller = new CallerServices();

			IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
			/*
			 WARNING: Do not embed plaintext credentials in your application code.
			 Doing so is insecure and against best practices.
			 Your API credentials must be handled securely. Please consider
			 encrypting them for use in any production environment, and ensure
			 that only authorized individuals may view or modify them.
			 */

            // Set up your API credentials, PayPal end point, and API version.
			profile.APIUsername = "sdk-three_api1.sdk.com";
			profile.APIPassword = "QFZCWN5HZM8VBG7Q";
			profile.APISignature = "AVGidzoSQiGWu.lGj3z15HLczXaaAcK6imHawrjefqgclVwBe8imgCHZ";
			profile.Environment="sandbox";
			caller.APIProfile = profile;

			// Create the request object.
			DoExpressCheckoutPaymentRequestType pp_request = new DoExpressCheckoutPaymentRequestType();
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
            // Create the request details object.
			pp_request.DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
			pp_request.DoExpressCheckoutPaymentRequestDetails.Token = token;
			pp_request.DoExpressCheckoutPaymentRequestDetails.PayerID = payerID;
			pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentAction = paymentAction;//Enum for PaymentAction is  PaymentActionCodeType.Sale
		
			pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails = new PaymentDetailsType();

			pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal = new BasicAmountType();

			pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal.currencyID = currencyCodeType;//Enum for currency code is  CurrencyCodeType.USD
			pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal.Value = paymentAmount;

            // Execute the API operation and obtain the response.
			DoExpressCheckoutPaymentResponseType pp_response=new DoExpressCheckoutPaymentResponseType();
			pp_response= (DoExpressCheckoutPaymentResponseType) caller.Call("DoExpressCheckoutPayment", pp_request);
			return pp_response.Ack.ToString();
		}

	}
}
