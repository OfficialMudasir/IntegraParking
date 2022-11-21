using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using Newtonsoft.Json;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.OxxoWS.WS;

namespace integraMobile.OxxoWS.Models
{
    public enum PayResult
    {
        success = 0,
        invalid_user = 1,
        invalid_inputparams = 2,
        already_payed = 3,
        // ...
        exception = 99
    }

    [DataContract(Namespace = "", Name = "OLS")]
    [XmlType(Namespace = "", TypeName = "OLS")]    
    public class PayInput : ModelBase
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(PayInput));

        #region Properties

        [DataMember]
        public string token { get; set; }

        [DataMember]
        public string client { get; set; }

        [DataMember]
        public string tranDate { get; set; }
        [IgnoreDataMember]
        public DateTime? DtTranDate
        {
            get
            {
                DateTime? dt = null;
                if (!string.IsNullOrEmpty(this.tranDate))
                    dt = DateTime.ParseExact(this.tranDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                return dt;
            }
            set
            {
                if (value.HasValue)
                    this.tranDate = value.Value.ToString("yyyyMMddHHmmss");
                else
                    this.tranDate = null;
            }
        }

        [DataMember]
        public int? cashMachine { get; set; }

        [DataMember]
        public string entryMode { get; set; }

        [DataMember]
        public long ticket { get; set; }

        [DataMember]
        public string account { get; set; }

        [DataMember]
        public long amount { get; set; }

        [DataMember]
        public long folio { get; set; }

        [DataMember]
        public string adminDate { get; set; }
        [IgnoreDataMember]
        public DateTime? DtAdminDate
        {
            get
            {
                DateTime? dt = null;
                if (!string.IsNullOrEmpty(this.adminDate))
                    dt = DateTime.ParseExact(this.adminDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                return dt;
            }
            set
            {
                if (value.HasValue)
                    this.adminDate = value.Value.ToString("yyyyMMdd");
                else
                    this.adminDate = null;
            }
        }

        [DataMember]
        public string store { get; set; }

        [DataMember]
        public string partial { get; set; }

        #endregion

        #region Methods

        private bool IsValid()
        {
            bool bRet = true;

            bRet = !string.IsNullOrEmpty(this.token) && !string.IsNullOrEmpty(this.client) && this.DtTranDate.HasValue &&
                   !string.IsNullOrEmpty(this.account) && this.DtAdminDate.HasValue && !string.IsNullOrEmpty(this.store) && !string.IsNullOrEmpty(this.partial);

            return bRet;
        }

        public PayOutput Pay(ICustomersRepository oCustomersRepository)
        {
            PayOutput oRet = new PayOutput();
            string strUserEmail = "";

            try
            {
                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Pay::PayInput={0}", PrettyXml(Conversions.XmlSerializeToString(this))));

                USER oUser = null;

                decimal? dUserId = null;
                try
                {
                    dUserId = Convert.ToDecimal(this.client);
                }
                catch
                {
                    dUserId = null;
                }

                if (dUserId.HasValue && oCustomersRepository.GetUserDataById(ref oUser, dUserId.Value))
                {
                    if (this.IsValid())
                    {
                        if (oUser.USR_ID == Convert.ToDecimal(this.account))
                        {
                            string sUserIdent = oUser.USR_USERNAME + "_" + DateTime.UtcNow.Ticks.ToString();
                            string strCellModel = "";
                            string strOSVersion = "";
                            string strPhoneSerialNumber = "";
                            string strCulture = "en-US";
                            string strAppVersion = "1.5";
                            bool bSessionKeepAlive = true;
                            string sSessionID = "";

                            decimal dInsId = Convert.ToDecimal(ConfigurationManager.AppSettings["InstallationId"] ?? "0");

                            if (oCustomersRepository.StartSession(ref oUser, dInsId, 5, sUserIdent, sUserIdent, sUserIdent, strCellModel,
                                                                  strOSVersion, strPhoneSerialNumber, strCulture, strAppVersion, bSessionKeepAlive, out sSessionID))
                            {

                                strUserEmail = oUser.USR_EMAIL;
                                WSIntegraMobile oWS = new WSIntegraMobile();
                                SortedList oParametersOut = new SortedList();

                                ResultType eResult = oWS.ConfirmRecharge(oUser.USR_USERNAME, sSessionID, Convert.ToInt32(this.amount), this.token, this.cashMachine, this.entryMode, this.ticket, this.folio, this.DtAdminDate.Value, this.store, this.partial, ref oParametersOut);
                                if (eResult == ResultType.Result_OK)
                                {
                                    oRet.auth = Convert.ToInt64(oParametersOut["rechargeId"]);
                                    oRet.amount = this.amount;
                                    oRet.messageTicket = "Pago Realizado";
                                    oRet.account = this.account;
                                    oRet.codeType = PayResult.success;
                                }
                                else if (eResult == ResultType.Result_Error_AlreadyUsed_Recharge_Code)
                                    oRet.codeType = PayResult.already_payed;
                                else
                                    oRet.codeType = PayResult.exception;
                            }
                            else
                                oRet.codeType = PayResult.invalid_user;
                        }
                        else
                            oRet.codeType = PayResult.invalid_user;
                    }
                    else
                        oRet.codeType = PayResult.invalid_inputparams;
                }
                else
                    oRet.codeType = PayResult.invalid_user;
            }
            catch (Exception ex)
            {
                oRet.codeType = PayResult.exception;
                m_oLog.LogMessage(LogLevels.logERROR, "OXXO Pay::Exception", ex); 
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Pay::Email={0}::PayOutput={1}", strUserEmail,PrettyXml(Conversions.XmlSerializeToString(oRet))));

            return oRet;
        }

        public static PayInput Deserialize(string inboundXML)
        {
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(inboundXML));
            var serializer = new DataContractSerializer(typeof(PayInput));
            var request = new PayInput();
            request = (PayInput)serializer.ReadObject(ms);

            return request;
        }

        #endregion

    }

    [DataContract(Namespace = "")]
    [XmlType(Namespace = "", TypeName = "OLS")]
    public class PayOutput
    {
        [DataMember]
        public long auth { get; set; }

        [DataMember]
        public long amount { get; set; }

        [DataMember]
        public string messageTicket { get; set; }

        [DataMember]
        public string account { get; set; }

        [DataMember]
        public string code { get; set; }
        [IgnoreDataMember]
        [XmlIgnore]
        public PayResult codeType
        {            
            get { return (PayResult)Convert.ToInt32(this.code); }
            set
            {
                this.code = string.Format("{0:00}", (int)value);
                switch (value)
                {
                    case PayResult.success: this.errorDesc = "Pago Realizado"; break;
                    case PayResult.invalid_user: this.errorDesc = "Cliente No Encontrado"; break;
                    case PayResult.invalid_inputparams: this.errorDesc = "Parámetros de entrada incorrectos"; break;
                    case PayResult.already_payed: this.errorDesc = "Pago existente"; break;
                    case PayResult.exception: this.errorDesc = "Error"; break;
                }
            }
        }
        [DataMember]
        public string errorDesc { get; set; }

        [XmlAttribute("version")]
        public string version { get; set; }

        public PayOutput()
        {
            version = "1.0";
            messageTicket = "";
            account = "";
        }

    }
}