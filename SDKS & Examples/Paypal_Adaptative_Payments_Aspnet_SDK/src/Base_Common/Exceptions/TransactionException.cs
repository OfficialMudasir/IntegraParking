 using System;

namespace PayPal.Platform.SDK
{
	/// <summary>
	/// Summary description for TranactionException.
	/// </summary>
	public class TransactionException: Exception
	{	
		/// <summary>
		/// Acknowledgement string
		/// </summary>
		private string ack;

		/// <summary>
		/// Correlation ID
		/// </summary>
		private string  correlationID;
		/// <summary>
		/// build number
		/// </summary>
		private string build;
		/// <summary>
		/// Transaction error holds Error details
		/// </summary>
        private FaultDetailFaultMessageError[] errorDetails;
       
		/// <summary>
		/// Acknowledgement string
		/// </summary>
		public string Ack
		{
			get{return ack;}
			set{this.ack = value;}
		}
		/// <summary>
		/// Correlation ID
		/// </summary>
		public string CorrelationID
		{
			get{return correlationID;}
			set{this.correlationID = value;}
		}
		/// <summary>
		/// build number
		/// </summary>
		public string Build
		{
			get{return build;}
			set{this.build = value;}
		}
		/// <summary>
		/// Transaction error holds Error details
		/// </summary>
        public FaultDetailFaultMessageError[] ErrorDetails
		{
			get{return errorDetails;}
			set{this.errorDetails = value;}
		}
        
		public TransactionException(PayLoadFromat payLoadFormat, string faultMessage)
		{
			try
			{
				switch(payLoadFormat)
				{
					case PayLoadFromat.SOAP11 :
					{
						/// typecasting fault string to SOAP fault Envelope
						PayPal.Platform.SDK.Fault env = (PayPal.Platform.SDK.Fault)SoapEncoder.Decode(faultMessage, typeof(PayPal.Platform.SDK.Fault));
                        
						/// Constructing TransactionException message
						this.ack = env.detail.FaultMessage.responseEnvelope.ack ;
						this.build = env.detail.FaultMessage.responseEnvelope.build ;
						this.correlationID = env.detail.FaultMessage.responseEnvelope.correlationId ;
                        this.errorDetails = env.detail.FaultMessage.error;
                        break;		
					}
                   case PayLoadFromat.XML:
                    {
                        /// typecasting fault string to SOAP fault Envelope
                        PayPal.Platform.SDK.FaultDetailFaultMessage env = (PayPal.Platform.SDK.FaultDetailFaultMessage)XMLEncoder.Decode(faultMessage, typeof(PayPal.Platform.SDK.FaultDetailFaultMessage));

                        /// Constructing TransactionException message
                        this.ack = env.responseEnvelope.ack;
                        this.build = env.responseEnvelope.build;
                        this.correlationID = env.responseEnvelope.correlationId;
                        this.errorDetails = env.error;
                        break;
                    }
                    case PayLoadFromat.JSON:
                    {
                        /// typecasting fault string to SOAP fault Envelope
                        PayPal.Platform.SDK.Fault env = (PayPal.Platform.SDK.Fault)JSONSerializer.JsonDecode(faultMessage, typeof(PayPal.Platform.SDK.Fault));
                        ///// Constructing TransactionException message
                        this.ack = env.detail.FaultMessage.responseEnvelope.ack;
                        this.build = env.detail.FaultMessage.responseEnvelope.build;
                        this.correlationID = env.detail.FaultMessage.responseEnvelope.correlationId;
                        this.errorDetails = env.detail.FaultMessage.error;
                        break;

                    }

				}
		
			}
			catch(FATALException FATALEx)
			{
				throw FATALEx;
			}
			catch(Exception ex)
			{
				throw new FATALException("Error occurred in TranactionException->Constructor method",ex);
			}
			finally
			{
				
			}
		}
	}
	
}
