/*
 * Copyright 2005, 2008 PayPal, Inc. All Rights Reserved.
 *
 * DoDirectPayment SOAP example; last modified 08MAY23. 
 *
 * Process a credit card payment.  
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
	public class DoDirectPayment
	{
		public DoDirectPayment()
		{
		}
		public string DoDirectPaymentCode(string paymentAmount, string buyerLastName, string buyerFirstName, string buyerAddress1, string buyerAddress2, string buyerCity, string buyerState, string buyerZipCode, string creditCardType, string creditCardNumber, string CVV2, int expMonth, int expYear, PaymentActionCodeType paymentAction)
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
			DoDirectPaymentRequestType pp_Request = new DoDirectPaymentRequestType();
			pp_Request.Version="51.0";

			// Add request-specific fields to the request.
			// Create the request details object.
			pp_Request.DoDirectPaymentRequestDetails = new DoDirectPaymentRequestDetailsType();

			pp_Request.DoDirectPaymentRequestDetails.IPAddress = "10.244.43.106";
			pp_Request.DoDirectPaymentRequestDetails.MerchantSessionId = "1X911810264059026";
			pp_Request.DoDirectPaymentRequestDetails.PaymentAction = paymentAction;
			
			pp_Request.DoDirectPaymentRequestDetails.CreditCard = new CreditCardDetailsType();
			
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardNumber = creditCardNumber;	
			switch (creditCardType)
			{
				case "Visa":
					pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Visa;
					break;
				case "MasterCard":
					pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.MasterCard;
					break;
				case "Discover":
					pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Discover;
					break;
				case "Amex":
					pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Amex;
					break;
			}
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CVV2 = CVV2;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpMonth = expMonth;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpYear = expYear;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpMonthSpecified = true; 
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpYearSpecified = true; 
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner = new PayerInfoType();
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Payer = "";
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerID = "";
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerStatus = PayPalUserStatusCodeType.unverified;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerCountry = CountryCodeType.US;

			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address = new AddressType();
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Street1 = buyerAddress1;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Street2 = buyerAddress2;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CityName = buyerCity;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.StateOrProvince= buyerState;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.PostalCode = buyerZipCode;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CountryName = "USA";
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Country = CountryCodeType.US;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CountrySpecified = true;
		
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName = new PersonNameType();
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName.FirstName = buyerFirstName;
			pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName.LastName = buyerLastName;
			pp_Request.DoDirectPaymentRequestDetails.PaymentDetails = new PaymentDetailsType();
			pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal = new BasicAmountType();
			// NOTE: The only currency supported by the Direct Payment API at this time is US dollars (USD).

			pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal.currencyID = CurrencyCodeType.USD;
			pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal.Value = paymentAmount;

            // Execute the API operation and obtain the response.
			DoDirectPaymentResponseType pp_response =new DoDirectPaymentResponseType();
			pp_response= (DoDirectPaymentResponseType) caller.Call("DoDirectPayment", pp_Request);
			return pp_response.Ack.ToString();
		}
	}
}
