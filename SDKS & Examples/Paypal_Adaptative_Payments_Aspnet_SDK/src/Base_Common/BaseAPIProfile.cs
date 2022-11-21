
using System;
using log4net;

namespace PayPal.Platform.SDK
{
	/// <summary>
	/// BaseAPIProfile takes the values that needs to be passed as header.
	/// </summary>
	[Serializable]
	public  class BaseAPIProfile
	{
		/// <summary>
		/// The username used to access the PayPal API
		/// </summary>
		private string apiUsername = string.Empty;

		/// <summary>
		/// The password used to access the PayPal API
		/// </summary>
		private string apiPassword;

		/// <summary>
		/// The name of the entity on behalf of which this profile is issuing calls
		/// </summary>
		private string subject;


		/// <summary>
		///  The PayPal environment (Live, Sadnbox)
		/// </summary>
		private string environment;

		/// <summary>
		///  The connection timeout in milliseconds
		/// </summary>
		private int timeout;


		/// <summary>
		///  The maximum number of retries
		/// </summary>
		private int maximumRetries;


		/// <summary>
		///  The delay time bewteen each retry call in milliseconds
		/// </summary>
		private int delayTime;


		/// <summary>
		/// The API signature used in three-token authentication
		/// </summary>
		private string apiSignature;

		/// <summary>
		/// The username used to access the PayPal API
		/// </summary>
		
		/// <summary>
		/// The certificate used to access the PayPal API
		/// </summary>
		private string certificateFile = string.Empty;
        /// <summary>
        /// The certificate used to access the PayPal API
        /// </summary>
        private byte[] certificate = null ;

		/// <summary>
		/// The privateKeyPassword used
		/// </summary>
		private string privateKeyPassword = "";
		
		/// <summary>
		/// Type of profile used to authenticate
		/// </summary>
		private ProfileType apiProfileType;

        /// <summary>
        /// To specify trust all certificate
        /// </summary>
        private bool isTrustallCertificate;


        private string deviceIpaddress = string.Empty;

        private string sandboxMailAddress = string.Empty;
		/// <summary>
		/// Paypal request and response Data format.
		/// </summary>
		private string XPAYPALREQUESTDATAFORMAT=BaseConstants.XPAYPALREQUESTDATAFORMAT ;
		private string XPAYPALRESPONSEDATAFORMAT=BaseConstants.XPAYPALRESPONSEDATAFORMAT;
        private string XPAYPALSERVICEVERSION = BaseConstants.XPAYPALSERVICEVERSION ;
        private string XPAYPALREQUESTSOURCE = BaseConstants.XPAYPALREQUESTSOURCE ;
        private string XPAYPALAPPLICATIONID = BaseConstants.XPAYPALAPPLICATIONID;

        /// <summary>
        /// Endpoint to Append.
        /// </summary>
        private string EndPointAP;

        private string source;
        
		
        /// <summary>
        /// API Username
        /// </summary>
		public string APIUsername
		{
			get { return this.apiUsername; }
			set { this.apiUsername = value; }
		}


		/// <summary>
		/// The password used to access the PayPal API
		/// </summary>
		public string APIPassword
		{
			get { return this.apiPassword; }
			set { this.apiPassword = value; }
		}


		/// <summary>
		/// The name of the entity on behalf of which this profile is issuing calls
		/// </summary>
		/// <remarks>
		/// This is for Third-Party access
		/// </remarks>
		public string Subject
		{
			get { return this.subject; }
			set { this.subject = value; }
		}


		/// <summary>
		/// The PayPal environment (Live, Sadnbox) 
		/// </summary>
		public string Environment
		{
			get { return this.environment; }
			set { this.environment = value; }
		}


		/// <summary>
		///  The connection timeout in milliseconds
		/// </summary>
		public int Timeout
		{
			get { return this.timeout; }
			set
			{
				if (value >= 0)
					this.timeout = value;
			}
		}


		/// <summary>
		///  The maximum number of retries
		/// </summary>
		public int MaximumRetries
		{
			get { return this.maximumRetries; }
			set { if (value >= 0) this.maximumRetries = value; }
		}


		/// <summary>
		///  The delay time bewteen each retry call in milliseconds
		/// </summary>
		public int DelayTime
		{
			get { return this.delayTime; }
			set { if (value >= 0) this.delayTime = value; }
		}


		/// <summary>
		/// API Signature used to access the PayPal API.  Only used for
		/// profiles set to Three-Token Authentication instead of Client-Side SSL Certificates.
		/// </summary>
		public  string APISignature
		{
			get
			{
				return this.apiSignature;
			}
			set
			{
				this.apiSignature = value;
			}
		}


		/// <summary>
		/// The file-name of the certificate to be used.
		///
		/// </summary>
        public string CertificateFile
		{
			get
			{
				return this.certificateFile;
			}
			set
			{
				this.certificateFile = value;
			}
		}
        /// <summary>
        /// The the certificate to be used.
        /// 
        /// </summary>
        public byte[] Certificate
        {
            get
            {
                return this.certificate;
            }
            set
            {
                this.certificate = value;
            }
        }
	
	
		/// <summary>
		/// Represents Data format of Request.
		/// </summary>
		public  string RequestDataformat
		{
			get
			{
				return this.XPAYPALREQUESTDATAFORMAT;
			}
			set
			{
				this.XPAYPALREQUESTDATAFORMAT = value;
			}
		}

		/// <summary>
		/// Represents Data format of Response
		/// </summary>
		public  string ResponseDataformat
		{
			get
			{
				return this.XPAYPALRESPONSEDATAFORMAT;
			}
			set
			{
				this.XPAYPALRESPONSEDATAFORMAT = value;
			}
		}

		/// <summary>
		/// version of API Service
		/// </summary>
		private  string ServiceVersion
		{
			get
			{
				return this.XPAYPALSERVICEVERSION;
			}
			set
			{
				this.XPAYPALSERVICEVERSION = value;
			}
		}

		/// <summary>
		/// Request Source
		/// </summary>
		private  string RequestSource
		{
			get
			{
				return this.XPAYPALREQUESTSOURCE;
			}
			set
			{
				this.XPAYPALREQUESTSOURCE = value;
			}
		}

        /// <summary>
        /// Application id
        /// </summary>
        public string ApplicationID
        {
            get
            {
                return this.XPAYPALAPPLICATIONID;
            }
            set
            {
                this.XPAYPALAPPLICATIONID = value;
            }
        }
		/// <summary>
		/// End point to be appended.
		/// </summary>
        public string EndPointAppend
        {
            get
            {
                return this.EndPointAP;
            }
            set
            {
                this.EndPointAP = value;
            }
        }
		
		/// <summary>
		/// Represents Profile type (3token or Certificate)
		/// </summary>
		public  ProfileType APIProfileType
		{
			get
			{
				return this.apiProfileType;
			}
			set
			{
				this.apiProfileType = value;
			}
		}
		
		/// <summary>
		/// Password of given Certificate's Private Key
		/// </summary>
		public string PrivateKeyPassword
		{
			get
			{
				return privateKeyPassword;
			}
			set
			{
				privateKeyPassword = value;
			}
		}

        /// <summary>
        /// To trust all certificate
        /// </summary>
        public bool IsTrustAllCertificates
        {
            get
            {
                return isTrustallCertificate;
            }
            set
            {
                isTrustallCertificate = value;
            }
        }

        /// <summary>
        /// For XPAYPALDEVICEIPADDRESS
        /// </summary>
        public string DeviceIpAddress
        {
            get
            {
                return deviceIpaddress;
            }
            set
            {
                deviceIpaddress = value;
            }
        }

        /// <summary>
        /// For Sandbox Dev Central Account
        /// </summary>
        public string SandboxMailAddress
        {
            get
            {
                return sandboxMailAddress;
            }
            set
            {
                sandboxMailAddress = value;
            }
        }

        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
	} 
} // profiles namespace