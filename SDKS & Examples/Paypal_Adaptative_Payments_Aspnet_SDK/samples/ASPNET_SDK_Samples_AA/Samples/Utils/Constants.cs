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
			public const string CREATEACCOUNTRESPONSE = "CREATEACCOUNTRESPONSE";
            public const string GETVERIFIEDSTATUSRESPONSE = "GETVERIFIEDSTATUSRESPONSE";
            public const string ADDBANKACCOUNTRESPONSE = "ADDBANKACCOUNTRESPONSE";
            public const string ADDPAYMENTCARDRESPONSE = "ADDPAYMENTCARDRESPONSE";
            public const string SETFUNDINGSOURCECONFIRMED = "SETFUNDINGSOURCECONFIRMED";
            public const string PROFILE = "PROFILE";
			public const string FATALEXCEPTION = "FATALEXCEPTION";
		
			
		}
        /// <summary>
        /// QueryString Constants
        /// </summary>
		public class QueryStringConstants
		{
			public const string TYPE = "type";
            
		}

        /// <summary>
        /// ASPXPages Constants
        /// </summary>
		public class ASPXPages
		{
			public const string APIERROR = "APIError.aspx";
			
		}

	}
}
