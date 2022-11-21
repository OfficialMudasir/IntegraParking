/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * TransactionSearch SOAP example; last modified 08MAY23. 
 *
 * Search your account history for transactions that meet the criteria you specify.  
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
	public class TransactionSearch
	{
		public TransactionSearch()
		{
		}

		public string TransactionSearchnCode(DateTime startDate, DateTime endDate,string transactionId)
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
			TransactionSearchRequestType concreteRequest = new TransactionSearchRequestType();			
			concreteRequest.Version="51.0";

            // Add request-specific fields to the request.
			concreteRequest.StartDate = startDate;
			concreteRequest.EndDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
			concreteRequest.EndDateSpecified = true;
			concreteRequest.TransactionID=transactionId;

            // Execute the API operation and obtain the response.
			TransactionSearchResponseType pp_response=new TransactionSearchResponseType();
			pp_response= (TransactionSearchResponseType) caller.Call("TransactionSearch", concreteRequest);
			return pp_response.Ack.ToString();

		}
	}
}
