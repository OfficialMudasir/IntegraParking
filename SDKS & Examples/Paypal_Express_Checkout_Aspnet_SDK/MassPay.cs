/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * MassPay SOAP example; last modified 08MAY23. 
 *
 * Pay one or more recipients.  
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
	public class MassPay
	{
		MassPayRequestType pp_request = new MassPayRequestType();
		MassPayRequestItemType MassItemReq=new MassPayRequestItemType();

		public MassPay()
		{
		}

		public string MassPayCode(string EmailSubject,ReceiverInfoCodeType receiverType,string[] ReceiverEmail, string[] value, string[] UniqueId,string[] note,CurrencyCodeType[] currencyId,int Count)
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
			pp_request.MassPayItem= new MassPayRequestItemType[Count];
			pp_request.Version="51.0";

            // Add request-specific fields to the request.
			MassPayResponseType pp_response=new MassPayResponseType();

			for (int i=0;i<Count;i++)
			{

				pp_request.MassPayItem[i]=new MassPayRequestItemType();

				pp_request.MassPayItem[i].ReceiverEmail=ReceiverEmail[i];
				pp_request.MassPayItem[i].Amount = new BasicAmountType();
				pp_request.MassPayItem[i].Amount.Value = value[i];
				pp_request.MassPayItem[i].Amount.currencyID=currencyId[i];
				pp_request.MassPayItem[i].UniqueId=UniqueId[i];
				pp_request.MassPayItem[i].Note=note[i];

			}
			
			pp_request.EmailSubject=EmailSubject;
			pp_request.ReceiverType=receiverType;//Enum for ReceiverType is ReceiverInfoCodeType.EmailAddress

            // Execute the API operation and obtain the response.
			pp_response= (MassPayResponseType) caller.Call("MassPay", pp_request);
			return pp_response.Ack.ToString();

		}
	}
}
