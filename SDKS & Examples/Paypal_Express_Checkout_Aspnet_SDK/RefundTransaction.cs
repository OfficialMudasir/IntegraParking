/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * RefundTransaction SOAP example; last modified 08MAY23. 
 *
 * Issue a refund for a prior transaction.  
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
	public class RefundTransaction
	{
		public RefundTransaction()
		{
		}

		public string RefundTransactionCode(String refundType , String transactionId,String amount, String note)
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
			RefundTransactionRequestType concreteRequest = new RefundTransactionRequestType();
			concreteRequest.Version="51.0";			

            // Add request-specific fields to the request.
			if((amount !=  null && amount.Length > 0) && (refundType.Equals("Partial"))) 
			{
					
				BasicAmountType amtType = new BasicAmountType();
				amtType.Value=amount;
				amtType.currencyID= CurrencyCodeType.USD;
				concreteRequest.Amount=amtType;
				concreteRequest.RefundType = RefundType.Partial;
			}
			else
			{
				concreteRequest.RefundType = RefundType.Full;
			}
			concreteRequest.RefundTypeSpecified = true;
			concreteRequest.TransactionID = transactionId;
			concreteRequest.Memo= note;

            // Execute the API operation and obtain the response.
			RefundTransactionResponseType pp_response=new RefundTransactionResponseType();
			pp_response= (RefundTransactionResponseType) caller.Call("RefundTransaction", concreteRequest);
			return pp_response.Ack.ToString();

		}
	}
}
