/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * SetCustomerBillingAgreement SOAP example; last modified 08MAY23. 
 *
 * Sets up a billing agreement for recurring payments.  
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
	public class SetCustomerBillingAgreement
	{
		public SetCustomerBillingAgreement()
		{
		}

		public string SetCustomerBillingAgreementCode(string returnURL, string cancelURL,  string Description)
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
			SetCustomerBillingAgreementRequestType pp_request=new SetCustomerBillingAgreementRequestType();
			pp_request.SetCustomerBillingAgreementRequestDetails =new SetCustomerBillingAgreementRequestDetailsType();
			SetCustomerBillingAgreementResponseType pp_response=new SetCustomerBillingAgreementResponseType();
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
			pp_request.SetCustomerBillingAgreementRequestDetails.CancelURL=cancelURL;
			pp_request.SetCustomerBillingAgreementRequestDetails.ReturnURL=returnURL;

			pp_request.SetCustomerBillingAgreementRequestDetails.BillingAgreementDetails=new BillingAgreementDetailsType();

			pp_request.SetCustomerBillingAgreementRequestDetails.BillingAgreementDetails.BillingAgreementDescription=Description;
			pp_request.SetCustomerBillingAgreementRequestDetails.BillingAgreementDetails.BillingType= BillingCodeType.RecurringPayments;
			
			// Execute the API operation and obtain the response.
			pp_response= (SetCustomerBillingAgreementResponseType) caller.Call("SetCustomerBillingAgreement", pp_request);
			return pp_response.Ack.ToString();
		}

	}
}
