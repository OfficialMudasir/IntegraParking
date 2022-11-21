/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * SetExpressCheckout SOAP example; last modified 08MAY23. 
 *
 * Initiate an Express Checkout transaction.  
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
	public class ECSetExpressCheckout
	{
		public ECSetExpressCheckout()
		{
		}

		public string ECSetExpressCheckoutCode(string paymentAmount, string returnURL, string cancelURL, PaymentActionCodeType paymentAction, CurrencyCodeType currencyCodeType)
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
			SetExpressCheckoutRequestType pp_request = new SetExpressCheckoutRequestType();
			pp_request.Version="51.0";	

            // Add request-specific fields to the request.
			// Create the request details object.
			pp_request.SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();
			pp_request.SetExpressCheckoutRequestDetails.PaymentAction = paymentAction;//Enum for PaymentAction is  PaymentActionCodeType.Sale
			pp_request.SetExpressCheckoutRequestDetails.PaymentActionSpecified = true;

			pp_request.SetExpressCheckoutRequestDetails.OrderTotal = new BasicAmountType();

			pp_request.SetExpressCheckoutRequestDetails.OrderTotal.currencyID = currencyCodeType;//Enum for currency code is  CurrencyCodeType.USD
			pp_request.SetExpressCheckoutRequestDetails.OrderTotal.Value = paymentAmount;
		
			pp_request.SetExpressCheckoutRequestDetails.CancelURL = cancelURL;
			pp_request.SetExpressCheckoutRequestDetails.ReturnURL = returnURL;

            // Execute the API operation and obtain the response.
			SetExpressCheckoutResponseType pp_response=new SetExpressCheckoutResponseType();
			pp_response= (SetExpressCheckoutResponseType) caller.Call("SetExpressCheckout", pp_request);
			return pp_response.Ack.ToString();
		}

	}
}
