using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayPal.Services.Private.AP;



namespace PayPal.Platform.SDK
{
    /// <summary>
    /// AdapativePayments Wrapper class
    /// </summary>
    public class AdapativePayments : CallerServices
    {
        private const string apEndpoint = "AdaptivePayments/";
        private string result = string.Empty;
        private TransactionException lastError = null;
        private object res = string.Empty;

        /// <summary>
        /// Returns "SUCCESS" if API request retuns Success response, else returns "FAILURE"
        /// </summary>
        public string isSuccess
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        /// LastError
        /// </summary>
        public TransactionException LastError
        {
            get
            {
                return this.lastError;
            }

        }

        /// <summary>
        /// LastResponse
        /// </summary>
        public string LastResponse
        {
            get
            {
                return this.res.ToString();
            }
        }

        /// <summary>
        /// Calls Pay Platform API for the given PayRequest and returns PayResponse 
        /// </summary>
        /// <param name="request">PayRequest</param>
        /// <returns>PayResponse</returns>
        public PayResponse pay(PayRequest request)
        {

            PayResponse PResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = apEndpoint + "Pay";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();


                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PayResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.PayResponse))
                    {
                        PResponse = (PayPal.Services.Private.AP.PayResponse)obj;
                    }
                    string name = Enum.GetName(PResponse.responseEnvelope.ack.GetType(), PResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }

                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {
                    this.result = "FAILURE";

                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }


                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        PResponse = (PayPal.Services.Private.AP.PayResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PayResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        PResponse = (PayPal.Services.Private.AP.PayResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PayResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PayResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.PayResponse))
                        {
                            PResponse = (PayPal.Services.Private.AP.PayResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }

            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments ->  method.", ex);
            }
            return PResponse;
        }

        /// <summary>
        /// Calls Pay Platform API for the given PayRequest and returns SePayOptionsResponse 
        /// </summary>
        /// <param name="request">PayRequest</param>
        /// <returns>PayResponse</returns>
        public SetPaymentOptionsResponse SetPaymentOptions(SetPaymentOptionsRequest request)
        {

            SetPaymentOptionsResponse SPResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = apEndpoint + "SetPaymentOptions";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();


                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse))
                    {
                        SPResponse = (PayPal.Services.Private.AP.SetPaymentOptionsResponse)obj;
                    }
                    string name = Enum.GetName(SPResponse.responseEnvelope.ack.GetType(), SPResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }

                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {
                    this.result = "FAILURE";

                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }


                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        SPResponse = (PayPal.Services.Private.AP.SetPaymentOptionsResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        SPResponse = (PayPal.Services.Private.AP.SetPaymentOptionsResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.SetPaymentOptionsResponse))
                        {
                            SPResponse = (PayPal.Services.Private.AP.SetPaymentOptionsResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }

            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments ->  method.", ex);
            }
            return SPResponse;
        }
        
        /// <summary>
        /// Calls Pay Platform API for the given SetPaymentOptions and returns PayOptionsResponse 
        /// </summary>
        /// <param name="request">PayRequest</param>
        /// <returns>PayResponse</returns>
        public GetPaymentOptionsResponse GetPaymentOptions(GetPaymentOptionsRequest request)
        {

            GetPaymentOptionsResponse GPResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = apEndpoint + "GetPaymentOptions";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();


                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse))
                    {
                        GPResponse = (PayPal.Services.Private.AP.GetPaymentOptionsResponse)obj;
                    }
                    string name = Enum.GetName(GPResponse.responseEnvelope.ack.GetType(), GPResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }

                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {
                    this.result = "FAILURE";

                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }


                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        GPResponse = (PayPal.Services.Private.AP.GetPaymentOptionsResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GPResponse = (PayPal.Services.Private.AP.GetPaymentOptionsResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetPaymentOptionsResponse))
                        {
                            GPResponse = (PayPal.Services.Private.AP.GetPaymentOptionsResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }

            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments ->  method.", ex);
            }
            return GPResponse;
        }
        /// <summary>
        /// Calls Pay Platform API for the given ExecutePayment and returns PayOptionsResponse 
        /// </summary>
        /// <param name="request">PayRequest</param>
        /// <returns>PayResponse</returns>
        public ExecutePaymentResponse ExecutePayment(ExecutePaymentRequest request)
        {

             ExecutePaymentResponse EPResponse = null;
             PayLoad = null;

             try
             {
                 APIProfile.EndPointAppend = apEndpoint + "ExecutePayment";
                 if (APIProfile.RequestDataformat == "SOAP11")
                 {
                     PayLoad = SoapEncoder.Encode(request);
                 }
                 else if (APIProfile.RequestDataformat == "XML")
                 {
                     PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                 }
                 else
                 {
                     PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                 }
                 res = CallAPI();


                 if (APIProfile.RequestDataformat == "JSON")
                 {
                     object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ExecutePaymentResponse));
                     if (obj.GetType() == typeof(PayPal.Services.Private.AP.ExecutePaymentResponse))
                     {
                         EPResponse = (PayPal.Services.Private.AP.ExecutePaymentResponse)obj;
                     }
                     string name = Enum.GetName(EPResponse.responseEnvelope.ack.GetType(), EPResponse.responseEnvelope.ack);

                     if (name == "Failure")
                     {
                         this.result = "FAILURE";
                         TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                         this.lastError = tranactionEx;
                     }
                 }

                 else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                 {
                     this.result = "FAILURE";

                     if (APIProfile.RequestDataformat == "SOAP11")
                     {
                         TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                         this.lastError = tranactionEx;
                     }
                     else if (APIProfile.RequestDataformat == "XML")
                     {
                         TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                         this.lastError = tranactionEx;
                     }
                     else
                     {
                         TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                         this.lastError = tranactionEx;
                     }


                 }
                 else
                 {
                     if (APIProfile.RequestDataformat == "SOAP11")
                     {
                         EPResponse = (PayPal.Services.Private.AP.ExecutePaymentResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ExecutePaymentResponse));
                     }
                     else if (APIProfile.RequestDataformat == "XML")
                     {
                         EPResponse = (PayPal.Services.Private.AP.ExecutePaymentResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ExecutePaymentResponse));
                     }
                     else
                     {
                         object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ExecutePaymentResponse));
                         if (obj.GetType() == typeof(PayPal.Services.Private.AP.ExecutePaymentResponse))
                         {
                             EPResponse = (PayPal.Services.Private.AP.ExecutePaymentResponse)obj;
                         }
                     }
                     this.result = "SUCCESS";

                 }

             }
             catch (FATALException FATALEx)
             {
                 throw FATALEx;
             }
             catch (Exception ex)
             {

                 throw new FATALException("Error occurred in AdapativePayments ->  method.", ex);
             }
            
             return EPResponse;
        }

        /// <summary>
        /// TCalls PreapprovalDetails Platform API for the given PreapprovalDetailsRequest and returns PreapprovalDetailsResponse
        /// </summary>
        /// <param name="request">PreapprovalDetailsRequest</param>
        /// <returns>PreapprovalDetailsResponse</returns>
        public PreapprovalDetailsResponse preapprovalDetails(PreapprovalDetailsRequest request)
        {
            PreapprovalDetailsResponse PDResponse = null;
            PayLoad = null;
            try
            {

                APIProfile.EndPointAppend = apEndpoint + "PreapprovalDetails";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();
                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse))
                    {
                        PDResponse = (PayPal.Services.Private.AP.PreapprovalDetailsResponse)obj;
                    }
                    string name = Enum.GetName(PDResponse.responseEnvelope.ack.GetType(), PDResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        PDResponse = (PayPal.Services.Private.AP.PreapprovalDetailsResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        PDResponse = (PayPal.Services.Private.AP.PreapprovalDetailsResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.PreapprovalDetailsResponse))
                        {
                            PDResponse = (PayPal.Services.Private.AP.PreapprovalDetailsResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";
                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments -> PreapprovalDetails method.", ex);
            }
            return PDResponse;
        }
        /// <summary>
        /// Calls Refund Platform API for the given RefundRequest and returns RefundResponse
        /// </summary>
        /// <param name="request">RefundRequest</param>
        /// <returns>RefundResponse</returns>
        public RefundResponse refund(RefundRequest request)
        {
            RefundResponse RResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "Refund";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();
                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.RefundResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.RefundResponse))
                    {
                        RResponse = (PayPal.Services.Private.AP.RefundResponse)obj;
                    }
                    string name = Enum.GetName(RResponse.responseEnvelope.ack.GetType(), RResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        RResponse = (PayPal.Services.Private.AP.RefundResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.RefundResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        RResponse = (PayPal.Services.Private.AP.RefundResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.RefundResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.RefundResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.RefundResponse))
                        {
                            RResponse = (PayPal.Services.Private.AP.RefundResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments -> Refund method.", ex);
            }
            return RResponse;
        }
        /// <summary>
        /// Calls PaymentDetails Platform API for the given PaymentDetailsRequest and returns PaymentDetailsResponse
        /// </summary>
        /// <param name="request">PaymentDetailsRequest</param>
        /// <returns>PaymentDetailsResponse</returns>
        public PaymentDetailsResponse paymentDetails(PaymentDetailsRequest request)
        {
            PaymentDetailsResponse PDResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "PaymentDetails";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();
                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PaymentDetailsResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.PaymentDetailsResponse))
                    {
                        PDResponse = (PayPal.Services.Private.AP.PaymentDetailsResponse)obj;
                    }
                    string name = Enum.GetName(PDResponse.responseEnvelope.ack.GetType(), PDResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }

                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        PDResponse = (PayPal.Services.Private.AP.PaymentDetailsResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PaymentDetailsResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        PDResponse = (PayPal.Services.Private.AP.PaymentDetailsResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PaymentDetailsResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PaymentDetailsResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.PaymentDetailsResponse))
                        {
                            PDResponse = (PayPal.Services.Private.AP.PaymentDetailsResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";
                }

            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments -> PaymentDetails method.", ex);
            }
            return PDResponse;
        }
        /// <summary>
        /// Calls Preapproval Platform API for the given PreapprovalRequest and returns PreapprovalResponse
        /// </summary>
        /// <param name="request">PreapprovalRequest</param>
        /// <returns>PreapprovalResponse</returns>
        public PreapprovalResponse preapproval(PreapprovalRequest request)
        {
            PreapprovalResponse PResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "Preapproval";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();
                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.PreapprovalResponse))
                    {
                        PResponse = (PayPal.Services.Private.AP.PreapprovalResponse)obj;
                    }
                    string name = Enum.GetName(PResponse.responseEnvelope.ack.GetType(), PResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        PResponse = (PayPal.Services.Private.AP.PreapprovalResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        PResponse = (PayPal.Services.Private.AP.PreapprovalResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.PreapprovalResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.PreapprovalResponse))
                        {
                            PResponse = (PayPal.Services.Private.AP.PreapprovalResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";
                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {

                throw new FATALException("Error occurred in AdapativePayments -> Preapproval method.", ex);
            }
            return PResponse;
        }
        /// <summary>
        /// Calls CancelPreapproval Platform API for the given CancelPreapprovalRequest and returns CancelPreapprovalResponse
        /// </summary>
        /// <param name="request">CancelPreapprovalRequest</param>
        /// <returns>CancelPreapprovalResponse</returns>
        public CancelPreapprovalResponse CancelPreapproval(CancelPreapprovalRequest request)
        {
            CancelPreapprovalResponse CPResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "CancelPreapproval";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();
                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse))
                    {
                        CPResponse = (PayPal.Services.Private.AP.CancelPreapprovalResponse)obj;
                    }
                    string name = Enum.GetName(CPResponse.responseEnvelope.ack.GetType(), CPResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        CPResponse = (PayPal.Services.Private.AP.CancelPreapprovalResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        CPResponse = (PayPal.Services.Private.AP.CancelPreapprovalResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse));
                    }
                    else
                    {

                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.CancelPreapprovalResponse))
                        {
                            CPResponse = (PayPal.Services.Private.AP.CancelPreapprovalResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> CancelPreapproval method.", ex);

            }
            return CPResponse;
        }
        /// <summary>
        /// Calls ConvertCurrency Platform API for the given ConvertCurrencyRequest and returns ConvertCurrencyResponse
        /// </summary>
        /// <param name="request">ConvertCurrencyRequest</param>
        /// <returns>ConvertCurrencyResponse</returns>
        public ConvertCurrencyResponse ConvertCurrency(ConvertCurrencyRequest request)
        {
            ConvertCurrencyResponse CCResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "ConvertCurrency";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse))
                    {
                        CCResponse = (PayPal.Services.Private.AP.ConvertCurrencyResponse)obj;
                    }
                    string name = Enum.GetName(CCResponse.responseEnvelope.ack.GetType(), CCResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        CCResponse = (PayPal.Services.Private.AP.ConvertCurrencyResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        CCResponse = (PayPal.Services.Private.AP.ConvertCurrencyResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.ConvertCurrencyResponse))
                        {
                            CCResponse = (PayPal.Services.Private.AP.ConvertCurrencyResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> ConvertCurrency method.", ex);
            }
            return CCResponse;
        }
        /// <summary>
        /// Calls GetAllowedFundingSources Platform API for the given GetAllowedFundingSourcesRequest 
        /// and returns GetAllowedFundingSourcesResponse
        /// </summary>
        /// <param name="request">GetAllowedFundingSourcesRequest</param>
        /// <returns>GetAllowedFundingSourcesResponse</returns>
        public GetAllowedFundingSourcesResponse GetAllowedFundingSources(GetAllowedFundingSourcesRequest request)
        {
            GetAllowedFundingSourcesResponse GAFSResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "GetAllowedFundingSources";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse))
                    {
                        GAFSResponse = (PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse)obj;
                    }
                    string name = Enum.GetName(GAFSResponse.responseEnvelope.ack.GetType(), GAFSResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        GAFSResponse = (PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GAFSResponse = (PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse))
                        {
                            GAFSResponse = (PayPal.Services.Private.AP.GetAllowedFundingSourcesResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> GetAllowedFundingSources method.", ex);
            }
            return GAFSResponse;
        }

        /// <summary>
        /// Calls ConfirmPreapproval Platform API for the given ConfirmPreapprovalRequest 
        /// and returns ConfirmPreapprovalResponse
        /// </summary>
        /// <param name="request">ConfirmPreapprovalRequest</param>
        /// <returns>ConfirmPreapprovalResponse</returns>
        public ConfirmPreapprovalResponse ConfirmPreapproval(ConfirmPreapprovalRequest request)
        {
            ConfirmPreapprovalResponse CPResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "ConfirmPreapproval";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse))
                    {
                        CPResponse = (PayPal.Services.Private.AP.ConfirmPreapprovalResponse)obj;
                    }
                    string name = Enum.GetName(CPResponse.responseEnvelope.ack.GetType(), CPResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        CPResponse = (PayPal.Services.Private.AP.ConfirmPreapprovalResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        CPResponse = (PayPal.Services.Private.AP.ConfirmPreapprovalResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.ConfirmPreapprovalResponse))
                        {
                            CPResponse = (PayPal.Services.Private.AP.ConfirmPreapprovalResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> ConfirmPreapproval method.", ex);
            }
            return CPResponse;
        }

        
        /// <summary>
        /// Calls GetAvailableShippingAddresses Platform API for the given GetAvailableShippingAddressesRequest 
        /// and returns GetAvailableShippingAddressesResponse
        /// </summary>
        /// <param name="request">GetAvailableShippingAddressesRequest</param>
        /// <returns>GetAvailableShippingAddressesResponse</returns>
        public GetAvailableShippingAddressesResponse GetAvailableShippingAddresses(GetAvailableShippingAddressesRequest request)
        {
            GetAvailableShippingAddressesResponse GASAResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "GetAvailableShippingAddresses";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse))
                    {
                        GASAResponse = (PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse)obj;
                    }
                    string name = Enum.GetName(GASAResponse.responseEnvelope.ack.GetType(), GASAResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        GASAResponse = (PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GASAResponse = (PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse))
                        {
                            GASAResponse = (PayPal.Services.Private.AP.GetAvailableShippingAddressesResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> GetAvailableShippingAddresses method.", ex);
            }
            return GASAResponse;
        }
                
        
        /// <summary>
        /// Calls GetShippingAddresses Platform API for the given GetShippingAddressesRequest 
        /// and returns GetShippingAddressesResponse
        /// </summary>
        /// <param name="request">GetShippingAddressesRequest</param>
        /// <returns>GetShippingAddressesResponse</returns>
        public GetShippingAddressesResponse GetShippingAddresses(GetShippingAddressesRequest request)
        {
            GetShippingAddressesResponse GSAResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "GetShippingAddresses";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse))
                    {
                        GSAResponse = (PayPal.Services.Private.AP.GetShippingAddressesResponse)obj;
                    }
                    string name = Enum.GetName(GSAResponse.responseEnvelope.ack.GetType(), GSAResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        GSAResponse = (PayPal.Services.Private.AP.GetShippingAddressesResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GSAResponse = (PayPal.Services.Private.AP.GetShippingAddressesResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetShippingAddressesResponse))
                        {
                            GSAResponse = (PayPal.Services.Private.AP.GetShippingAddressesResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> GetShippingAddresses method.", ex);
            }
            return GSAResponse;
        }


        /// <summary>
        /// Calls GetFundingPlans Platform API for the given GetFundingPlansRequest 
        /// and returns GetFundingPlansResponse
        /// </summary>
        /// <param name="request">GetFundingPlansRequest</param>
        /// <returns>GetFundingPlansResponse</returns>
        public GetFundingPlansResponse GetFundingPlans(GetFundingPlansRequest request)
        {
            GetFundingPlansResponse GFPResponse = null;
            PayLoad = null;
            try
            {
                APIProfile.EndPointAppend = apEndpoint + "GetFundingPlans";
                if (APIProfile.RequestDataformat == "SOAP11")
                {
                    PayLoad = SoapEncoder.Encode(request);
                }
                else if (APIProfile.RequestDataformat == "XML")
                {
                    PayLoad = PayPal.Platform.SDK.XMLEncoder.Encode(request);
                }
                else
                {
                    PayLoad = PayPal.Platform.SDK.JSONSerializer.ToJavaScriptObjectNotation(request);
                }
                res = CallAPI();

                if (APIProfile.RequestDataformat == "JSON")
                {
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetFundingPlansResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetFundingPlansResponse))
                    {
                        GFPResponse = (PayPal.Services.Private.AP.GetFundingPlansResponse)obj;
                    }
                    string name = Enum.GetName(GFPResponse.responseEnvelope.ack.GetType(), GFPResponse.responseEnvelope.ack);

                    if (name == "Failure")
                    {
                        this.result = "FAILURE";
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else if (res.ToString().ToUpper().Replace("<ACK>FAILURE</ACK>", "").Length != res.ToString().Length)
                {

                    this.result = "FAILURE";
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.SOAP11, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.XML, res.ToString());
                        this.lastError = tranactionEx;
                    }
                    else
                    {
                        TransactionException tranactionEx = new TransactionException(PayLoadFromat.JSON, res.ToString());
                        this.lastError = tranactionEx;
                    }
                }
                else
                {
                    if (APIProfile.RequestDataformat == "SOAP11")
                    {
                        GFPResponse = (PayPal.Services.Private.AP.GetFundingPlansResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetFundingPlansResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GFPResponse = (PayPal.Services.Private.AP.GetFundingPlansResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AP.GetFundingPlansResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AP.GetFundingPlansResponse));

                        if (obj.GetType() == typeof(PayPal.Services.Private.AP.GetFundingPlansResponse))
                        {
                            GFPResponse = (PayPal.Services.Private.AP.GetFundingPlansResponse)obj;
                        }
                    }
                    this.result = "SUCCESS";

                }
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in AdapativePayments -> GetFundingPlans method.", ex);
            }
            return GFPResponse;
        }

    }
}
