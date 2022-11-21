using System;
using log4net;

namespace PayPal.Platform.SDK
{
	/// <summary>
	/// Custom FATALException holds Short and Long messages.
	/// </summary>
	public class FATALException : Exception
	{
		#region Priavte Members
        private static readonly ILog log = LogManager.GetLogger("PAYPALLOGFILE");
		/// <summary>
		/// Short message
		/// </summary>
		private string FATALExMessage ;
		/// <summary>
		/// Long message
		/// </summary>
		private string FATALExpLongMessage ;

		#endregion 

		#region Constructors

		public FATALException(string FATALExceptionMessage, Exception exception)
		{
			/// Logging the exception.
			if (log.IsInfoEnabled)
			{	
				log.Info(FATALExceptionMessage, exception);
			}
				
			this.FATALExMessage = FATALExceptionMessage;
			this.FATALExpLongMessage = exception.Message ;
		}

		#endregion
			
		#region Public Properties
		
		/// <summary>
		/// Short message.
		/// </summary>
		public string FATALExceptionMessage
		{
			get
			{
				return FATALExMessage; 
			}
			set
			{
				FATALExMessage = value;
			}
		}
		
		/// <summary>
		/// Long message
		/// </summary>
		public string FATALExceptionLongMessage
		{
			get
			{
				return FATALExpLongMessage; 
			}
			set
			{
				FATALExpLongMessage = value;
			}
		}

		#endregion
	}

}
