/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * DoReauthorization SOAP example; last modified 08MAY23. 
 *
 * Reauthorize a previously authorized payment.  
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
	public class Reauthorization
	{
		public Reauthorization()
		{
		}

		public string ReauthorizationCode(string authorizationId, string value, CurrencyCodeType currencyId)
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
			DoReauthorizationRequestType pp_request = new DoReauthorizationRequestType();
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
			pp_request.AuthorizationID = authorizationId;
			pp_request.Amount = new BasicAmountType();
			pp_request.Amount.Value = value;
			pp_request.Amount.currencyID = currencyId;//Enum for currency code is  CurrencyCodeType.USD

            // Execute the API operation and obtain the response.
            DoReauthorizationResponseType pp_response=new DoReauthorizationResponseType();
			pp_response= (DoReauthorizationResponseType) caller.Call("DoReauthorization", pp_request);
			return pp_response.Ack.ToString();

		}
	}
}
