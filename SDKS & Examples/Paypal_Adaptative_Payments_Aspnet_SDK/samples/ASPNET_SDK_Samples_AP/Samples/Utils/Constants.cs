using System;

namespace ASPNET_SDK_Samples.Samples
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		public Constants()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
        /// <summary>
        /// Session Constants
        /// </summary>
		public class SessionConstants
		{
			
			public const string FAULT = "FAULT";
            public const string ERRORFAULT = "ERRORFAULT";
			public const string PAYMENTDETAILSRESPONSE = "PAYMENTDETAILSRESPONSE";
			public const string PREAPPROVALRESPONSE = "PREAPPROVALRESPONSE";
			public const string PREAPPROVALDETAILSRESPONSE = "PREAPPROVALDETAILSRESPONSE";
			public const string REFUNDRESPONSE = "REFUNDRESPONSE";
            public const string CANCELPREAPPROVALRESPONSE = "CANCELPREAPPROVALRESPONSE";
            public const string CONVERTCURRENCYRESPONSE = "CONVERTCURRENCYRESPONSE";
            public const string SETPAYMENTOPTIONSRESPONSE = "SETPAYMENTOPTIONSRESPONSE";
            public const string GETPAYMENTOPTIONSRESPONSE = "GETPAYMENTOPTIONSRESPONSE";
            public const string EXECUTEPAYMENTRESPONSE = "EXECUTEPAYMENTRESPONSE";
			
			public const string PROFILE = "PROFILE";
			public const string PAYKEY = "PAYKEY";
			public const string PREAPPROVALKEY = "PREAPPROVALKEY";
			public const string FATALEXCEPTION = "FATALEXCEPTION";
		
			
		}
        /// <summary>
        /// QueryString Constants
        /// </summary>
		public class QueryStringConstants
		{
			
			public const string PAYKEY = "paykey";
			public const string TYPE = "type";
            public const string PREAPPROVALKEY = "preapprovalkey";
			

		}

        /// <summary>
        /// ASPXPages Constants
        /// </summary>
		public class ASPXPages
		{
			public const string APIERROR = "APIError.aspx";
			public const string PAYMENTDETAILS = "PaymentDetails.aspx";
            public const string PREAPPROVALDETAILS = "PreapprovalDetails.aspx";

		}

	}
}
