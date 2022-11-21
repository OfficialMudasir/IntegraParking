/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * DoCapture SOAP example; last modified 08MAY23. 
 *
 * Capture a payment.  
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
	public class DoCapture
	{
		public DoCapture()
		{
		}

		public string DoCaptureCode(string authorizationId, string note, string value, CurrencyCodeType currencyId, string invoiceId)
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

			DoCaptureRequestType pp_request = new DoCaptureRequestType();
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
			pp_request.AuthorizationID = authorizationId;
			pp_request.Note = note;
			pp_request.Amount = new BasicAmountType();
			pp_request.Amount.Value = value;
			pp_request.Amount.currencyID = currencyId;
			pp_request.InvoiceID = invoiceId;

            // Execute the API operation and obtain the response.
			DoCaptureResponseType pp_response =new DoCaptureResponseType();
			pp_response= (DoCaptureResponseType)  caller.Call("DoCapture", pp_request);
			return pp_response.Ack.ToString();
		}
	}
}
