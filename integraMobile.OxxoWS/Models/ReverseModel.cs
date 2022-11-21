using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Runtime.Serialization;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.Threading;
using Newtonsoft.Json;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.OxxoWS.WS;
using integraMobile.OxxoWS.Properties;

namespace integraMobile.OxxoWS.Models
{
    public enum ReverseResult
    {
        success = 0,
        invalid_recharge_id = 1,
        invalid_inputparams = 2,
        invalid_recharge_status = 3,
        // ...
        exception = 99
    }

    [DataContract(Namespace = "", Name = "OLS")]
    [XmlType(Namespace = "", TypeName = "OLS")]
    public class ReverseInput : ModelBase
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(ReverseInput));

        #region Properties

        [DataMember]
        public string token { get; set; }

        [DataMember]
        public long folio { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        public long? lAuth { get; set; }

        [DataMember]
        public string auth
        {
            get
            {
                if (lAuth.HasValue)
                    return lAuth.Value.ToString();
                else
                    return null;
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    lAuth = null;
                else
                    lAuth = Convert.ToInt64(value);
            }
        }

        #endregion

        #region Methods

        private bool IsValid()
        {
            bool bRet = true;

            bRet = (!string.IsNullOrEmpty(this.token) && this.folio > 0 && ((this.lAuth.HasValue && this.lAuth > 0) || !this.lAuth.HasValue));

            return bRet;
        }

        public ReverseOutput Reverse(ICustomersRepository oCustomersRepository, IBackOfficeRepository oBackOfficeRepository, IInfraestructureRepository oInfraestructureRepository)
        {
            ReverseOutput oRet = new ReverseOutput();
            string strUserEmail = "";

            try
            {
                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Reverse::ReverseInput={0}", PrettyXml(Conversions.XmlSerializeToString(this))));

                var oPredicate = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGE>();
                oPredicate = oPredicate.And(c => c.CUSPMR_OXXO_FOLIO.HasValue && c.CUSPMR_OXXO_FOLIO.Value == this.folio);
                if (this.lAuth.HasValue)
                    oPredicate = oPredicate.And(c => c.CUSPMR_ID == this.lAuth.Value);

                var oRecharge = oBackOfficeRepository.GetCustomerRecharges(oPredicate).FirstOrDefault();
                if (oRecharge != null)
                {
                    strUserEmail = oRecharge.USER.USR_EMAIL;
                    if (oRecharge.CUSPMR_TRANS_STATUS == 0)
                    {
                        if (this.IsValid())
                        {
                            object oObjDeleted = null;
                            int iBalanceBefore = 0;
                            USER oUser = null;
                            OPERATIONS_DISCOUNT oDiscountDeleted = null;
                            bool bIsHisOperation = false;
                            bool bErrorAccess = false;

                            if (oBackOfficeRepository.DeleteOperation(ChargeOperationsType.BalanceRecharge, oRecharge.CUSPMR_ID, out oObjDeleted, out iBalanceBefore, out oUser, out oDiscountDeleted, out bIsHisOperation, null, out bErrorAccess, PaymentMeanRechargeStatus.Cancelled))
                            {
                                oRecharge = (CUSTOMER_PAYMENT_MEANS_RECHARGE)oObjDeleted;

                                oRet.idReverse = Convert.ToInt64(oRecharge.CUSPMR_ID);
                                oRet.amount = Convert.ToInt64(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED);
                                oRet.auth = Convert.ToInt64(oRecharge.CUSPMR_ID);
                                oRet.account = oRecharge.CUSPMR_USR_ID.ToString();

                                oRet.codeType = ReverseResult.success;
                                oRet.messageTicket = "Reverso Realizado";

                                SendNotification(oCustomersRepository, oInfraestructureRepository, oRecharge, oUser);                                
                            }
                            else
                                oRet.codeType = ReverseResult.exception;
                        }
                        else
                            oRet.codeType = ReverseResult.invalid_inputparams;
                    }
                    else
                        oRet.codeType = ReverseResult.invalid_recharge_status;
                }
                else
                    oRet.codeType = ReverseResult.invalid_recharge_id;
            }
            catch (Exception ex)
            {
                oRet.codeType = ReverseResult.exception;
                m_oLog.LogMessage(LogLevels.logERROR, "OXXO Reverse::Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Reverse::Email={0}::ReverseOutput={1}", strUserEmail,PrettyXml(Conversions.XmlSerializeToString(this))));

            return oRet;
        }

        public static ReverseInput Deserialize(string inboundXML)
        {
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(inboundXML));
            var serializer = new DataContractSerializer(typeof(ReverseInput));
            var request = new ReverseInput();
            request = (ReverseInput)serializer.ReadObject(ms);

            return request;
        }

        public static string DataContractSerializeObject<T>(T objectToSerialize)
        {
            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        private bool SendNotification(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge, USER oUser)
        {
            bool bRet = false;


            if ((PaymentSuscryptionType)oRecharge.CUSPMR_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPrepay)
            {
                string culture = oUser.USR_CULTURE_LANG;
                CultureInfo ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

                int iQuantity = oRecharge.CUSPMR_AMOUNT;
                decimal dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                decimal dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                decimal dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                int iPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                int iFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                int iPartialVAT1;
                int iPartialPercFEE;
                int iPartialFixedFEE;
                int iPartialPercFEEVAT;
                int iPartialFixedFEEVAT;

                int iTotalQuantity = oCustomersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                iQFEE += iFixedFEE;
                int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                int iQSubTotal = iQuantity + iQFEE;

                int iLayout = 0;
                if (iQFEE != 0 || iQVAT != 0)
                {
                    OPERATOR oOperator = oCustomersRepository.GetDefaultOperator();
                    if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                }


                string sLayoutSubtotal = "";
                string sLayoutTotal = "";

                string sCurIsoCode = oInfraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));

                if (iLayout == 2)
                {
                    sLayoutSubtotal = string.Format(Resource.Email_LayoutSubtotal,
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(iQSubTotal) / 100, sCurIsoCode),
                                                    (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                    (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                    string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, sCurIsoCode));
                }
                else if (iLayout == 1)
                {
                    sLayoutTotal = string.Format(Resource.Email_LayoutTotal,
                                                 string.Format("{0:0.00} {1}", Convert.ToDouble(iQuantity) / 100, sCurIsoCode),
                                                 string.Format("{0:0.00} {1}", Convert.ToDouble(iQFEE) / 100, sCurIsoCode),
                                                 (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                 (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                 string.Format("{0:0.00} {1}", Convert.ToDouble(iQVAT) / 100, sCurIsoCode));
                }

                string strRechargeEmailSubject = Resource.ReverseRecharge_EmailHeader;
                /*
                    ID: {0}<br>
                 *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                 *  Cantidad Recargada: {2} 
                 */
                string strRechargeEmailBody = string.Format(Resource.ReverseRecharge_EmailBody,
                    oRecharge.CUSPMR_ID,
                    oRecharge.CUSPMR_DATE,
                    string.Format("{0:0.00} {1}", Convert.ToDouble(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) / 100,
                                                  oInfraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                    string.Format("{0:0.00} {1}", Convert.ToDouble(oUser.USR_BALANCE + oRecharge.CUSPMR_AMOUNT) / 100,
                                        oInfraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                    string.Format("{0:0.00} {1}", Convert.ToDouble(oUser.USR_BALANCE) / 100,
                                        oInfraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                    ConfigurationManager.AppSettings["EmailSignatureURL"],
                    ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                    sLayoutSubtotal, sLayoutTotal);


                bRet = SendEmail(oCustomersRepository, oInfraestructureRepository, ref oUser, strRechargeEmailSubject, strRechargeEmailBody);

            }

            return bRet;
        }

        private bool SendEmail(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, ref USER oUser, string strEmailSubject, string strEmailBody)
        {
            bool bRes = true;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                            ((sender, certificate, chain, sslPolicyErrors) => true);

                long lSenderId = oInfraestructureRepository.SendEmailTo(oUser.USR_EMAIL, strEmailSubject, strEmailBody);

                if (lSenderId > 0)
                {
                    oCustomersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }

        #endregion

    }

    [DataContract(Namespace = "")]
    [XmlType(Namespace = "", TypeName = "OLS")]
    public class ReverseOutput
    {
        [DataMember]
        public long idReverse { get; set; }

        [DataMember]
        public long amount { get; set; }

        [DataMember]
        public long auth { get; set; }

        [DataMember]
        public string account { get; set; }

        [DataMember]
        public string code { get; set; }
        [IgnoreDataMember]
        [XmlIgnore]
        public ReverseResult codeType
        {
            get { return (ReverseResult)Convert.ToInt32(this.code); }            
            set
            {
                this.code = string.Format("{0:00}", (int)value);
                switch (value)
                {
                    case ReverseResult.success: this.errorDesc = "Reverso Realizado"; break;
                    case ReverseResult.invalid_recharge_id: this.errorDesc = "Recarga no encontrada"; break;
                    case ReverseResult.invalid_inputparams: this.errorDesc = "Parámetros de entrada incorrectos"; break;
                    case ReverseResult.invalid_recharge_status: this.errorDesc = "Estado de la recarga no válido"; break;
                    case ReverseResult.exception: this.errorDesc = "Error"; break;
                }
            }
        }
        [DataMember]
        public string errorDesc { get; set; }

        [DataMember]
        public string messageTicket { get; set; }

        [XmlAttribute("version")]
        public string version { get; set; }

        public ReverseOutput()
        {
            version = "1.0";
            account = "";
            messageTicket = "";
        }

    }
}