/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * GetExpressCheckoutDetails SOAP example; last modified 08MAY23. 
 *
 * Get information about an Express Checkout transaction.  
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
	public class ECGetExpressCheckout
	{
		public ECGetExpressCheckout()
		{
		}

		public string ECGetExpressCheckoutCode(string token)
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
			GetExpressCheckoutDetailsRequestType pp_request = new GetExpressCheckoutDetailsRequestType();
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
			pp_request.Token = token;

            // Execute the API operation and obtain the response.
			GetExpressCheckoutDetailsResponseType pp_response=new GetExpressCheckoutDetailsResponseType();
			pp_response= (GetExpressCheckoutDetailsResponseType) caller.Call("GetExpressCheckoutDetails", pp_request);
			return pp_response.Ack.ToString();

		}

	}
}
