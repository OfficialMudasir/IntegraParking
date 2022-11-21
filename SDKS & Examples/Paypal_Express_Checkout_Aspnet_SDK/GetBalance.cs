/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
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
	/// <summary>
	/// Summary description for GetBalance.
	/// </summary>
	public class GetBalance
	{
		public GetBalance()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string GetBalanceCode()
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
			profile.APIUsername = "sdk-three_api1.sdk.com";
			profile.APIPassword = "QFZCWN5HZM8VBG7Q";
			profile.APISignature = "AVGidzoSQiGWu.lGj3z15HLczXaaAcK6imHawrjefqgclVwBe8imgCHZ";
			profile.Environment="sandbox";
			caller.APIProfile = profile;

			GetBalanceRequestType pp_request=new GetBalanceRequestType();
			pp_request.Version="51.0";
			GetBalanceResponseType pp_response=new GetBalanceResponseType();
			pp_response= (GetBalanceResponseType) caller.Call("GetBalance", pp_request);
			return pp_response.Ack.ToString();
		}

	}
}
