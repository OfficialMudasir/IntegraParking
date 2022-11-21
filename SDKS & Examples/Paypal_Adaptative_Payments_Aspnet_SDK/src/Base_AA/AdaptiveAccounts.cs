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
using PayPal.Services.Private.AA;
using PayPal.Platform.SDK;

namespace PayPal.Platform.SDK
{
    /// <summary>
    /// AdaptiveAccounts Wrapper class
    /// </summary>
    public class AdaptiveAccounts : CallerServices
    {
        private const string aaEndpoint = "AdaptiveAccounts/";
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
        /// Calls CreateAccount Platform API for the given CreateAccountRequest and returns CreateAccountResponse 
        /// </summary>
        /// <param name="request">CreateAccountRequest</param>
        /// <returns>CreateAccountResponse</returns>
        public CreateAccountResponse CreateAccount(CreateAccountRequest request)
        {
            CreateAccountResponse CAResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "CreateAccount";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.CreateAccountResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.CreateAccountResponse))
                    {
                        CAResponse = (PayPal.Services.Private.AA.CreateAccountResponse)obj;
                    }
                    string name = Enum.GetName(CAResponse.responseEnvelope.ack.GetType(), CAResponse.responseEnvelope.ack);

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
                        CAResponse = (PayPal.Services.Private.AA.CreateAccountResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.CreateAccountResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        CAResponse = (PayPal.Services.Private.AA.CreateAccountResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.CreateAccountResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.CreateAccountResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.CreateAccountResponse))
                        {
                            CAResponse = (PayPal.Services.Private.AA.CreateAccountResponse)obj;
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
            return CAResponse;

        }


        public GetVerifiedStatusResponse GetVerifiedStatus(GetVerifiedStatusRequest request)
        {
            GetVerifiedStatusResponse GetVerifiedStatusResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "GetVerifiedStatus";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.GetVerifiedStatusResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.GetVerifiedStatusResponse))
                    {
                        GetVerifiedStatusResponse = (PayPal.Services.Private.AA.GetVerifiedStatusResponse)obj;
                    }
                    string name = Enum.GetName(GetVerifiedStatusResponse.responseEnvelope.ack.GetType(), GetVerifiedStatusResponse.responseEnvelope.ack);

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
                        GetVerifiedStatusResponse = (PayPal.Services.Private.AA.GetVerifiedStatusResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.GetVerifiedStatusResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GetVerifiedStatusResponse = (PayPal.Services.Private.AA.GetVerifiedStatusResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.GetVerifiedStatusResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.CreateAccountResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.GetVerifiedStatusResponse))
                        {
                            GetVerifiedStatusResponse = (PayPal.Services.Private.AA.GetVerifiedStatusResponse)obj;
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
            return GetVerifiedStatusResponse;

        }


        public AddBankAccountResponse AddBankAccount(AddBankAccountRequest request)
        {
            AddBankAccountResponse AddBankAccountResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "AddBankAccount";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.AddBankAccountResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.AddBankAccountResponse))
                    {
                        AddBankAccountResponse = (PayPal.Services.Private.AA.AddBankAccountResponse)obj;
                    }
                    string name = Enum.GetName(AddBankAccountResponse.responseEnvelope.ack.GetType(), AddBankAccountResponse.responseEnvelope.ack);

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
                        AddBankAccountResponse = (PayPal.Services.Private.AA.AddBankAccountResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.AddBankAccountResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        AddBankAccountResponse = (PayPal.Services.Private.AA.AddBankAccountResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.AddBankAccountResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.AddBankAccountResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.AddBankAccountResponse))
                        {
                            AddBankAccountResponse = (PayPal.Services.Private.AA.AddBankAccountResponse)obj;
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
            return AddBankAccountResponse;

        }



        public AddPaymentCardResponse AddPaymentCard(AddPaymentCardRequest request)
        {
            AddPaymentCardResponse AddPaymentCardResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "AddPaymentCard";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.AddPaymentCardResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.AddPaymentCardResponse))
                    {
                        AddPaymentCardResponse = (PayPal.Services.Private.AA.AddPaymentCardResponse)obj;
                    }
                    string name = Enum.GetName(AddPaymentCardResponse.responseEnvelope.ack.GetType(), AddPaymentCardResponse.responseEnvelope.ack);

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
                        AddPaymentCardResponse = (PayPal.Services.Private.AA.AddPaymentCardResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.AddPaymentCardResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        AddPaymentCardResponse = (PayPal.Services.Private.AA.AddPaymentCardResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.AddPaymentCardResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.AddPaymentCardResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.AddPaymentCardResponse))
                        {
                            AddPaymentCardResponse = (PayPal.Services.Private.AA.AddPaymentCardResponse)obj;
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
            return AddPaymentCardResponse;

        }

        public SetFundingSourceConfirmedResponse SetFundingSourceConfirmed(SetFundingSourceConfirmedRequest request)
        {
            SetFundingSourceConfirmedResponse SetFundingSourceConfirmedResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "SetFundingSourceConfirmed";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse))
                    {
                        SetFundingSourceConfirmedResponse = (PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse)obj;
                    }
                    string name = Enum.GetName(SetFundingSourceConfirmedResponse.responseEnvelope.ack.GetType(), SetFundingSourceConfirmedResponse.responseEnvelope.ack);
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
                        SetFundingSourceConfirmedResponse = (PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        SetFundingSourceConfirmedResponse = (PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse))
                        {
                            SetFundingSourceConfirmedResponse = (PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse)obj;
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
            return SetFundingSourceConfirmedResponse;

        }

        /// <summary>
        /// Returns the user agreement
        /// </summary>
        /// <param name="request">GetUserAgreementRequest</param>
        /// <returns>GetUserAgreementResponse</returns>
        public GetUserAgreementResponse GetUserAgreement(GetUserAgreementRequest request)
        {
            GetUserAgreementResponse GetUserAgreementResponse = null;
            PayLoad = null;

            try
            {
                APIProfile.EndPointAppend = aaEndpoint + "GetUserAgreement";

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
                    object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.GetUserAgreementResponse));
                    if (obj.GetType() == typeof(PayPal.Services.Private.AA.GetUserAgreementResponse))
                    {
                        GetUserAgreementResponse = (PayPal.Services.Private.AA.GetUserAgreementResponse)obj;
                    }
                    string name = Enum.GetName(GetUserAgreementResponse.responseEnvelope.ack.GetType(), GetUserAgreementResponse.responseEnvelope.ack);
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
                        GetUserAgreementResponse = (PayPal.Services.Private.AA.GetUserAgreementResponse)SoapEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.GetUserAgreementResponse));
                    }
                    else if (APIProfile.RequestDataformat == "XML")
                    {
                        GetUserAgreementResponse = (PayPal.Services.Private.AA.GetUserAgreementResponse)XMLEncoder.Decode(res.ToString(), typeof(PayPal.Services.Private.AA.GetUserAgreementResponse));
                    }
                    else
                    {
                        object obj = JSONSerializer.JsonDecode(res.ToString(), typeof(PayPal.Services.Private.AA.GetUserAgreementResponse));
                        if (obj.GetType() == typeof(PayPal.Services.Private.AA.SetFundingSourceConfirmedResponse))
                        {
                            GetUserAgreementResponse = (PayPal.Services.Private.AA.GetUserAgreementResponse)obj;
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

                throw new FATALException("Error occurred in AdapativeAccounts -> GetUserAgreement method.", ex);
            }
            return GetUserAgreementResponse;

        }

    }
}
