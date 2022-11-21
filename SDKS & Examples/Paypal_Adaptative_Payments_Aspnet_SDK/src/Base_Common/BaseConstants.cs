namespace PayPal.Platform.SDK
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class BaseConstants
	{
		/// <summary>
		/// Modify these values if you want to use your own profile.
		/// </summary>

		/* 
		 *                                                                         *
		 * WARNING: Do not embed plaintext credentials in your application code.   *
		 * Doing so is insecure and against best practices.                        *
		 *                                                                         *
		 * Your API credentials must be handled securely. Please consider          *
		 * encrypting them for use in any production environment, and ensure       *
		 * that only authorized individuals may view or modify them.               *
		 *                                                                         *
		 */
               					

        public const string XPAYPALREQUESTDATAFORMAT = "X-PAYPAL-REQUEST-DATA-FORMAT";
        public const string XPAYPALRESPONSEDATAFORMAT = "X-PAYPAL-RESPONSE-DATA-FORMAT";
        public const string XPAYPALSERVICEVERSION = "X-PAYPAL-SERVICE-VERSION";        
        public const string XPAYPALSECURITYUSERID = "X-PAYPAL-SECURITY-USERID";
        public const string XPAYPALSECURITYPASSWORD = "X-PAYPAL-SECURITY-PASSWORD";
        public const string XPAYPALSECURITYSIGNATURE = "X-PAYPAL-SECURITY-SIGNATURE";
        public const string XPAYPALMESSAGEPROTOCOL = "X-PAYPAL-MESSAGE-PROTOCOL";
        public const string XPAYPALAPPLICATIONID = "X-PAYPAL-APPLICATION-ID";
        public const string XPAYPALDEVICEIPADDRESS = "X-PAYPAL-DEVICE-IPADDRESS";
        public const string XPAYPALSANDBOXEMAILADDRESS = "X-PAYPAL-SANDBOX-EMAIL-ADDRESS";
        public const string XPAYPALREQUESTSOURCE = "X-PAYPAL-REQUEST-SOURCE";        

        //Data Request format specified here
        public const string  RequestDataformat="SOAP11";
        public const string  ResponseDataformat="SOAP11";

        public const string REQUESTMETHOD = "POST";
        public const string PAYPALLOGFILE = "PAYPALLOGFILE";

        public const int DEFAULT_TIMEOUT = 3600000;

        // Change the version only if you re-built the official PayPal SDK
        public const string XPAYPALSOURCE = "DOTNET_SOAP_SDK_V1.1";

        public class ErrorMessages
        {
            public const string PROFILE_NULL = "APIProfile can not be null." ;
            public const string PAYLOAD_NULL = "PayLoad can not be null or empty.";
        }

	}
}
