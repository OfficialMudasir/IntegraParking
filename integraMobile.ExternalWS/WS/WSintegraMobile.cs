using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain.Abstract;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using integraMobile.Domain;
using System.Resources;
using System.Threading;
using integraMobile.Domain.Concrete;
using integraMobile.Domain.Helper;
using Newtonsoft.Json;

namespace integraMobile.ExternalWS
{
    public class WSintegraMobile : WSBase
    {
        protected const string OSID_WEB = "5";
        protected const string LANG = "1"; 
        protected const string VERS = "1.0";
        protected const long BIG_PRIME_NUMBER = 472189635;
        protected const long BIG_PRIME_NUMBER2 = 624159837;

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infrastructureRepository;
        private IBackOfficeRepository backOfficeRepository;
        private SQLGeograficAndTariffsRepository geograficAndTariffsRepository;
        private string appVersion = string.Empty;
        private string parkingAppVersion = string.Empty;
        private string parkingAppCode = string.Empty;
        
        #region Dictionaries        

        Dictionary<string, string> dGetSubPayOptions = new Dictionary<string, string>()
        {
            { "IMEI", "" }, 
            { "WIFIMAC", "" }, 
            { "lang", "" }, 
            { "ccode", "" }, 
            { "gps", "" }, 
            { "vers", "" }, 
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" }
        };

        Dictionary<string, string> dChangeSubscriptionTypeInternal = new Dictionary<string, string>() 
        { 
            { "u", "" },
            { "subscription_type", "" },
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" },
        };

        Dictionary<string, string> dQueryLoginCityInternal = new Dictionary<string, string>() { 
            { "u", "" }, 
            { "lang", "" }, 
            { "OSID", "" }, 
            { "IMEI", "" }, 
            { "WIFIMAC", "" }, 
            { "keepsessionalive", "" }, 
            { "cityID", "" }, 
            { "gps", "" }, 
            { "utc_offset", "" }, 
            { "cityVERSION", "" }, 
            { "tarVERSION", "" }, 
            { "InsVersion", "" }, 
            { "InsGeomVersion", "" }, 
            { "appvers", "" }, 
            { "cmodel", "" }, 
            { "cosvers", "" }, 
            { "cserialnumber", "" }, 
            { "pushID", "" }, 
            { "vers", "" }, 
            { "ah", "" }
        };

        Dictionary<string, string> dAddLicensePlate = new Dictionary<string, string>() { 
            { "u", "" }, 
            { "SessionID", "" }, 
            { "license", "" }, 
            { "cityID", "" }, 
            { "vers", "" }, 
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" }
        };

        Dictionary<string, string> dQueryParkingTariffs = new Dictionary<string, string>() { 
            { "u", "" }, 
            { "SessionID", "" }, 
            { "p", "" }, 
            { "d", "" }, 
            { "g", "" }, 
            { "gps", "" }, 
            { "vers", "" }, 
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" }
        };

        Dictionary<string, string> dQueryParkingTariffsGuestUser = new Dictionary<string, string>() { 
            { "d", "" }, 
            { "g", "" }, 
            { "p", "" }, 
            { "appcode", "" }, 
            { "appvers", "" }, 
            { "culture", "" }, 
            { "OSID", "" },
            { "vers", "" }, 
            { "WIFIMAC", "" }, 
            { "guid", "" }, 
            { "ah", "" }
        };

        Dictionary<string, string> dQueryParkingOperationWithTimeSteps = new Dictionary<string, string>() { 
            { "u", "" }, 
            { "SessionID", "" }, 
            { "d", "" }, 
            { "di", "" }, 
            { "g", "" }, 
            { "strse", "" }, 
            { "ad", "" }, 
            { "isshopkeeperoperation", "" }, 
            { "vers", "" }, 
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" }
        };

        Dictionary<string, string> dConfirmParkingOperation = new Dictionary<string, string>() { 
            { "q", "" }, 
            { "d", "" }, 
            { "ed", "" }, 
            { "gps", "" }, 
            { "gpssel", "" }, 
            { "bd", "" }, 
            { "isgpsselected", "" }, 
            { "isshopkeeperoperation", "" }, 
            { "g", "" }, 
            { "postpay", "" }, 
            { "q_without_bon", "" }, 
            { "real_q", "" }, 
            { "q_fee", "" }, 
            { "madtarinfo", "" }, 
            { "ad", "" }, 
            { "t", "" }, 
            { "time_bal_used", "" }, 
            { "q_total", "" }, 
            { "q_vat", "" }, 
            { "appvers", "" }, 
            { "lang", "" }, 
            { "OSID", "" }, 
            { "SessionID", "" }, 
            { "u", "" }, 
            { "vers", "" }, 
            { "WIFIMAC", "" }, 
            { "backofficeUsr", "" },
            { "automatic_renewal", "" },
            { "ah", "" }
        };

        Dictionary<string, string> dCommonStart = new Dictionary<string, string>() 
        { 
            { "SessionId", "" },
            { "OSID", "" },
            { "utc_offset", "" },
            { "utc_date", "" }, 
            { "email", "" },
            { "ccprovider", "" },
            { "subscription_type", "" }
        };

        Dictionary<string, string> dCreditCall = new Dictionary<string, string>() 
        {
            { "ekashu_reference", "" },
            { "ekashu_auth_code", "" },
            { "ekashu_auth_result", "" },
            { "ekashu_card_hash", "" },
            { "ekashu_card_reference", "" },
            { "ekashu_card_scheme", "" }, 
            { "ekashu_date_time_local_fmt", "" },
            { "ekashu_masked_card_number", "" },
            { "ekashu_transaction_id", "" },
            { "ekashu_expires_end_month", "" },
            { "ekashu_expires_end_year", "" }
        };

        Dictionary<string, string> dIecisa = new Dictionary<string, string>() 
        {
            { "iecisa_CF_TicketNumber", "" },
            { "iecisa_CF_AuthCode", "" },
            { "iecisa_CF_Result", "" },
            { "iecisa_CF_TransactionID", "" },
            { "iecisa_TransactionID", "" },
            { "iecisa_CF_Token", "" },
            { "iecisa_CF_PAN", "" },
            { "iecisa_GatewayDate", "" },
            { "iecisa_CF_ExpirationDate", "" }
        };

        Dictionary<string, string> dStripe = new Dictionary<string, string>() 
        {
            { "stripe_customer_id", "" },
            { "stripe_card_reference", "" },
            { "stripe_card_scheme", "" },
            { "stripe_masked_card_number", "" },
            { "stripe_expires_end_month", "" },
            { "stripe_expires_end_year", "" },
            { "stripe_transaction_id", "" },
            { "stripe_date_time_utc", "" }
        };

        Dictionary<string, string> dMoneris = new Dictionary<string, string>()
        {
            { "moneris_reference", "" },
            { "moneris_auth_code", "" },
            { "moneris_auth_result", "" },
            { "moneris_card_hash", "" },
            { "moneris_card_reference", "" },
            { "moneris_card_scheme", "" },
            { "moneris_date_time_local_fmt", "" },
            { "moneris_masked_card_number", "" },
            { "moneris_transaction_id", "" },
            { "moneris_expires_end_month", "" },
            { "moneris_expires_end_year", "" }
        };

        Dictionary<string, string> dTransbank = new Dictionary<string, string>()
        {
            { "transbank_reference", "" },
            { "transbank_auth_code", "" },
            { "transbank_card_hash", "" },
            { "transbank_card_reference", "" },
            { "transbank_card_scheme", "" },
            { "transbank_date_time_local_fmt", "" },
            { "transbank_masked_card_number", "" },
            { "transbank_transaction_id", "" }
        };

        Dictionary<string, string> dPayu = new Dictionary<string, string>()
        {
            { "payu_reference", "" },
            { "payu_auth_code", "" },
            { "payu_card_hash", "" },
            { "payu_card_reference", "" },
            { "payu_card_scheme", "" },
            { "payu_date_time_local_fmt", "" },
            { "payu_masked_card_number", "" },
            { "payu_transaction_id", "" }
        };

        Dictionary<string, string> dCommonEnd = new Dictionary<string, string>()
        {
            { "RAW_CCNUMBER", "" },
            { "RAW_CCEXPDATE", "" },
            { "RAW_CCCVC", "" },
            { "ccchargedquantity", "" },
            { "ccchargerevert", "" },
            { "lang", "" },
            { "vers", "" },
            { "appvers", "" }, 
            { "ah", "" }
        };

        Dictionary<string, string> dModifyOperationPlates = new Dictionary<string, string>()
        {
            { "ope_id", "" },
            { "u", "" }, 
            { "SessionID", "" }, 
            { "p", "" },
            { "p2", "" },
            { "p3", "" },
            { "p4", "" },
            { "p5", "" },
            { "p6", "" },
            { "p7", "" },
            { "p8", "" },
            { "p9", "" },
            { "p10", "" },
            { "appvers", "" }, 
            { "OSID", "" },
            { "ah", "" }
        };

        #endregion

        #region Constructor

        public WSintegraMobile(IBackOfficeRepository _backOfficeRepository, ICustomersRepository _customersRepository, IInfraestructureRepository _infrastructureRepository, SQLGeograficAndTariffsRepository _geograficAndTariffsRepository)
            : base()
        {
            backOfficeRepository = _backOfficeRepository;
            customersRepository = _customersRepository;
            infrastructureRepository = _infrastructureRepository;
            geograficAndTariffsRepository = _geograficAndTariffsRepository;

            m_Log = new CLogWrapper(typeof(WSintegraMobile));
            if (ConfigurationManager.AppSettings.AllKeys.Contains("PermitsAppVersion"))
                appVersion = ConfigurationManager.AppSettings["PermitsAppVersion"].ToString();
            else
                appVersion = "3.6";

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ParkingAppVersion"))
                parkingAppVersion = ConfigurationManager.AppSettings["ParkingAppVersion"].ToString();
            else
                parkingAppVersion = "3.8";

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ParkingAppCode"))
                parkingAppCode = ConfigurationManager.AppSettings["ParkingAppCode"].ToString();
            else
                parkingAppCode = "BLINKAY";
        }
        
        #endregion
        
        #region Aux

        private integraMobile.ExternalWS.integraMobileWS.integraMobileWS initWS()
        {
            integraMobile.ExternalWS.integraMobileWS.integraMobileWS ret = new integraMobile.ExternalWS.integraMobileWS.integraMobileWS();
            ret.Url = GetWSUrl();
            ret.Timeout = GetWSTimeout();

            if (!string.IsNullOrEmpty(GetWSHttpUser()))
            {
                ret.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
            }

            return ret;
        }

        private string GetCodePhone(int CountryId)
        {
            integraMobile.Domain.COUNTRy Country = backOfficeRepository.GetCountries().Where(t => t.COU_ID == CountryId).FirstOrDefault();
            return Country.COU_TEL_PREFIX.Trim();
        }

        public string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string ReturnURL, string strHashSeed, string CancelURL = "", string QFEE = "", string QVAT = "", string Total = "", string ExternalId = "")
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL + Culture + CancelURL + QFEE + QVAT + Total + ExternalId;
            return CalculateHash(strHashString, strHashSeed);
        }

        protected string CalculateHash(string strInput, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;
                HMACSHA256 _hmacsha256 = null;
                int iKeyLength = 64;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }
                _hmacsha256 = new HMACSHA256(_normKey);

                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = null;

                hash = _hmacsha256.ComputeHash(inputBytes);

                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }
            return strRes;
        }

        public string DecryptCryptResult(string strHexByteArray, string strHashSeed)
        {
            string strRes = "";
            try
            {
                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));
                }
                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));
                }
                strRes = DecryptStringFromBytes_Aes(StringToByteArray(strHexByteArray), _normKey, _iv);
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);
            }
            return strRes;
        }

        protected static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        protected static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private string GetOnlyValues(List<Dictionary<string, string>> parameters)
        {
            string result = string.Empty;
            string s = string.Empty;
            foreach (Dictionary<string, string> d in parameters)
            {
                foreach (string value in d.Values)
                {
                    if (value == null) 
                    { 
                        s = string.Empty; 
                    } 
                    else 
                    { 
                        s = value; 
                    }
                    result += s.Replace("<lat>", string.Empty).Replace("</lat>", string.Empty).Replace("<long>", string.Empty).Replace("</long>", string.Empty);
                }
            }
            return result;
        }

        private string GetXML(List<Dictionary<string, string>> parameters)
        {
            string result = string.Empty;
            foreach (Dictionary<string, string> d in parameters)
            {
                foreach (string key in d.Keys)
                {
                    result += string.Format("<{0}>{1}</{0}>", key, d[key]);
                }
            }
            return string.Format("<ipark_in>{0}</ipark_in>", result);
        }

        #endregion        

        #region SignUp        

        public ResultType GetSubPayOptions(decimal? CountryId, out List<int> oOptions, out int oCCProvider, out string oChargeCurrency)
        {
            ResultType rtRes = ResultType.Result_OK;
            List<int> Options = new List<int>() { };
            SortedList parametersOut = new SortedList();
            oCCProvider = 0;
            oChargeCurrency = string.Empty;

            if (CountryId != null)
            {
                try
                {
                    integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                    int ccode = (int)CountryId;
                    string sLatLon;
                    string sLatLonXml = LatLonXml(1, 1, out sLatLon).Replace("<gps>", string.Empty).Replace("</gps>", string.Empty); ;

                    dGetSubPayOptions = new Dictionary<string, string>()
                    {
                        { "IMEI", "" }, 
                        { "WIFIMAC", "" }, 
                        { "lang", "" }, 
                        { "ccode", "" }, 
                        { "gps", "" }, 
                        { "vers", "" }, 
                        { "appvers", "" }, 
                        { "OSID", "" },
                        { "ah", "" }
                    };

                    dGetSubPayOptions["IMEI"] = string.Empty;
                    dGetSubPayOptions["WIFIMAC"] = string.Empty;
                    dGetSubPayOptions["lang"] = LANG;
                    dGetSubPayOptions["ccode"] = ccode.ToString();
                    dGetSubPayOptions["gps"] = sLatLonXml;
                    dGetSubPayOptions["vers"] = VERS;
                    dGetSubPayOptions["appvers"] = appVersion;
                    dGetSubPayOptions["OSID"] = OSID_WEB;
                    dGetSubPayOptions["ah"] = string.Empty;
                    dGetSubPayOptions["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dGetSubPayOptions }));

                    string strIn = GetXML(new List<Dictionary<string, string>>() { dGetSubPayOptions });
                    Logger_AddLogMessage(string.Format("GetSubPayOptions xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                    string strOut = oIntegraMobileWS.GetSubPayOptions(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                    Logger_AddLogMessage(string.Format("GetSubPayOptions xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                    SortedList wsParameters = null;

                    rtRes = FindOutParameters(strOut, out wsParameters);

                    if (parametersOut != null)
                    {
                        foreach (var key in wsParameters.Keys)
                            parametersOut.Add(key, wsParameters[key]);
                    }

                    if (rtRes == ResultType.Result_OK)
                    {
                        rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                        if (rtRes == ResultType.Result_OK)
                        {
                            if (wsParameters.ContainsKey("ccprovider"))
                            {
                                oCCProvider = Convert.ToInt32(wsParameters["ccprovider"]);
                            }
                            if (wsParameters.ContainsKey("per_transaction_minimum_charge_currency"))
                            {
                                oChargeCurrency = wsParameters["per_transaction_minimum_charge_currency"].ToString();
                            }
                            int iNum = Convert.ToInt32(wsParameters["subscription_type_st_num"]);
                            for (int i = 0; i < iNum; i++)
                            {
                                Options.Add(Convert.ToInt32(wsParameters[string.Format("subscription_type_st_{0}", i)].ToString()));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogException(e, "GetSubPayOptions::Exception", LogLevels.logERROR);
                }
            }

            oOptions = Options;

            return rtRes;
        }

        #endregion

        #region SubscriptionType

        public ResultType ChangeSubscriptionTypeInternal(string Username, decimal SubscriptionType)
        {
            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                dChangeSubscriptionTypeInternal = new Dictionary<string, string>() 
                { 
                    { "u", "" },
                    { "subscription_type", "" },
                    { "appvers", "" }, 
                    { "OSID", "" },
                    { "ah", "" },
                };

                dChangeSubscriptionTypeInternal["u"] = string.Empty;
                dChangeSubscriptionTypeInternal["subscription_type"] = string.Empty;
                dChangeSubscriptionTypeInternal["appvers"] = appVersion;
                dChangeSubscriptionTypeInternal["OSID"] = OSID_WEB;
                dChangeSubscriptionTypeInternal["ah"] = string.Empty;
                dChangeSubscriptionTypeInternal["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dChangeSubscriptionTypeInternal }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dChangeSubscriptionTypeInternal });
                Logger_AddLogMessage(string.Format("ChangeSubscriptionTypeInternal xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.ChangeSubscriptionTypeInternal(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("ChangeSubscriptionTypeInternal xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());                    
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ChangeSubscriptionTypeInternal::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        #endregion

        #region PayForPermit        

        public ResultType QueryLoginCityInternal(decimal? CityId, string Username, string text, string utc_offset, out SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;
            parametersOut = new SortedList();

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string sLatLon;
                string sLatLonXml = LatLonXml(1, 1, out sLatLon).Replace("<gps>", string.Empty).Replace("</gps>", string.Empty); ;

                dQueryLoginCityInternal = new Dictionary<string, string>() { 
                    { "u", "" }, 
                    { "lang", "" }, 
                    { "OSID", "" }, 
                    { "IMEI", "" }, 
                    { "WIFIMAC", "" }, 
                    { "keepsessionalive", "" }, 
                    { "cityID", "" }, 
                    { "gps", "" }, 
                    { "utc_offset", "" }, 
                    { "cityVERSION", "" }, 
                    { "tarVERSION", "" }, 
                    { "InsVersion", "" }, 
                    { "InsGeomVersion", "" }, 
                    { "appvers", "" }, 
                    { "cmodel", "" }, 
                    { "cosvers", "" }, 
                    { "cserialnumber", "" }, 
                    { "pushID", "" }, 
                    { "vers", "" }, 
                    { "ah", "" }
                };

                dQueryLoginCityInternal["u"] = Username;
                dQueryLoginCityInternal["lang"] = LANG;
                dQueryLoginCityInternal["OSID"] = OSID_WEB;
                dQueryLoginCityInternal["IMEI"] = string.Empty;
                dQueryLoginCityInternal["WIFIMAC"] = string.Empty;
                dQueryLoginCityInternal["keepsessionalive"] = "1";
                dQueryLoginCityInternal["cityID"] = CityId.ToString();
                dQueryLoginCityInternal["gps"] = sLatLonXml;
                dQueryLoginCityInternal["utc_offset"] = utc_offset;
                dQueryLoginCityInternal["cityVERSION"] = "-1";
                dQueryLoginCityInternal["tarVERSION"] = "-1";
                dQueryLoginCityInternal["InsVersion"] = "-1";
                dQueryLoginCityInternal["InsGeomVersion"] = "-1";
                dQueryLoginCityInternal["appvers"] = appVersion;
                dQueryLoginCityInternal["cmodel"] = string.Empty;
                dQueryLoginCityInternal["cosvers"] = string.Empty;
                dQueryLoginCityInternal["cserialnumber"] = string.Empty;
                dQueryLoginCityInternal["pushID"] = string.Empty;
                dQueryLoginCityInternal["vers"] = VERS;
                dQueryLoginCityInternal["ah"] = string.Empty;
                dQueryLoginCityInternal["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dQueryLoginCityInternal }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dQueryLoginCityInternal });
                Logger_AddLogMessage(string.Format("QueryLoginCityInternal xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.QueryLoginCityInternal(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("QueryLoginCityInternal xmlOut={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());                    
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryLoginCityInternal::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType AddLicensePlate(string Username, string SessionId, string LicensePlate, long CityId)
        {
            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                dAddLicensePlate = new Dictionary<string, string>() { 
                    { "u", "" }, 
                    { "SessionID", "" }, 
                    { "license", "" }, 
                    { "cityID", "" }, 
                    { "vers", "" }, 
                    { "appvers", "" }, 
                    { "OSID", "" },
                    { "ah", "" }
                };

                dAddLicensePlate["u"] = Username;
                dAddLicensePlate["SessionID"] = SessionId;
                dAddLicensePlate["license"] = LicensePlate;
                dAddLicensePlate["cityID"] = CityId.ToString();
                dAddLicensePlate["vers"] = VERS;
                dAddLicensePlate["appvers"] = appVersion;
                dAddLicensePlate["OSID"] = OSID_WEB;
                dAddLicensePlate["ah"] = string.Empty;
                dAddLicensePlate["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dAddLicensePlate }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dAddLicensePlate });
                Logger_AddLogMessage(string.Format("AddLicensePlate xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.AddLicensePlate(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("AddLicensePlate xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "GetSubPayOptions::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType QueryParkingTariffs(string Username, string SessionId, decimal? ZoneId, string text, out SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;

            parametersOut = new SortedList();

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string sLatLon;
                string sLatLonXml = LatLonXml(1, 1, out sLatLon).Replace("<gps>", string.Empty).Replace("</gps>", string.Empty); ;

                dQueryParkingTariffs = new Dictionary<string, string>() { 
                    { "u", "" }, 
                    { "SessionID", "" }, 
                    { "p", "" }, 
                    { "d", "" }, 
                    { "g", "" }, 
                    { "gps", "" }, 
                    { "vers", "" }, 
                    { "appvers", "" }, 
                    { "OSID", "" },
                    { "ah", "" }
                };

                dQueryParkingTariffs["u"] = Username;
                dQueryParkingTariffs["SessionID"] = SessionId;
                dQueryParkingTariffs["p"] = "PERMITS";
                dQueryParkingTariffs["d"] = DateTime.Now.ToString("HHmmssddMMyy");
                dQueryParkingTariffs["g"] = ZoneId.ToString();
                dQueryParkingTariffs["gps"] = sLatLon;
                dQueryParkingTariffs["vers"] = VERS;
                dQueryParkingTariffs["appvers"] = appVersion;
                dQueryParkingTariffs["OSID"] = OSID_WEB;
                dQueryParkingTariffs["ah"] = string.Empty;
                dQueryParkingTariffs["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dQueryParkingTariffs }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dQueryParkingTariffs });
                Logger_AddLogMessage(string.Format("QueryParkingTariffs xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.QueryParkingTariffs(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("QueryParkingTariffs xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryParkingTariffs::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType QueryParkingTariffsGuestUser(string ZoneId, string LicensePlate, string Culture, out dynamic wsParameters, string SessionId = "", string Guid = "")
        {
            ResultType rtRes = ResultType.Result_OK;
            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string curDate = DateTime.Now.ToString("HHmmssddMMyy");

                var dQueryParkingTariffsGuestUser = new
                {
                    ipark_in = new
                    {
                        d = curDate,
                        g = ZoneId,
                        p = LicensePlate,
                        appcode = parkingAppCode,
                        appvers = parkingAppVersion,
                        culture = Culture,
                        OSID = OSID_WEB,
                        vers = VERS,
                        SessionId = SessionId,
                        guid = Guid,
                        ah = CalculateWSHash(curDate + ZoneId + LicensePlate + parkingAppCode + parkingAppVersion + Culture + OSID_WEB + VERS + SessionId + Guid)
                    }
                };

                string strIn = JsonConvert.SerializeObject(dQueryParkingTariffsGuestUser);

                Logger_AddLogMessage(string.Format("dQueryParkingTariffsGuestUser xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.QueryParkingTariffsGuestUserJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("QueryParkingTariffs xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryParkingTariffsGuestUser::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType QueryAvailableTariffsGuestUser(string ZoneId, string Culture, out dynamic wsParameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();                

                string curDate = DateTime.Now.ToString("HHmmssddMMyy");

                var dQueryAvailableTariffsGuestUser = new
                {
                    ipark_in = new
                    {
                        d = curDate,
                        g = ZoneId,
                        appcode = parkingAppCode,
                        appvers = parkingAppVersion,
                        culture = Culture,
                        OSID = OSID_WEB,
                        vers = VERS,
                        SessionId = string.Empty,
                        ah = CalculateWSHash(curDate + ZoneId + parkingAppCode + parkingAppVersion + Culture + OSID_WEB + VERS)
                    }
                };

                string strIn = JsonConvert.SerializeObject(dQueryAvailableTariffsGuestUser);

                Logger_AddLogMessage(string.Format("QueryAvailableTariffsGuestUserJSON xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.QueryAvailableTariffsGuestUserJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("QueryAvailableTariffsGuestUserJSON xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryAvailableTariffsGuestUserJSON::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType QueryParkingOperationWithTimeStepsGuestUser(decimal Group, string Plate, decimal Rate, string SessionID, string User, int Lang, out dynamic wsParameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string curDate = DateTime.Now.ToString("HHmmssddMMyy");

                var dQueryParkingOperationWithTimeSteps = new
                {
                    ipark_in = new
                    {
                        d = curDate,
                        isshopkeeperoperation = "0",
                        g = Convert.ToInt32(Group).ToString(),
                        p = Plate,
                        ad = Convert.ToInt32(Rate).ToString(),
                        appcode = parkingAppCode,
                        appvers = parkingAppVersion,
                        lang = Convert.ToInt32(Lang).ToString(),
                        OSID = OSID_WEB,
                        SessionID = SessionID,
                        u = User,
                        vers = VERS,
                        ah = CalculateWSHash(curDate + "0" + Convert.ToInt32(Group).ToString() + Plate + Convert.ToInt32(Rate).ToString() + parkingAppCode + parkingAppVersion + Convert.ToInt32(Lang).ToString() + OSID_WEB + SessionID + User + VERS)
                    }
                };

                string s = curDate + "0" + Group + Plate + Rate + parkingAppCode + parkingAppVersion + Lang.ToString() + OSID_WEB + SessionID + User + VERS;
                Console.Write(s);

                string strIn = JsonConvert.SerializeObject(dQueryParkingOperationWithTimeSteps);

                Logger_AddLogMessage(string.Format("dQueryParkingOperationWithTimeSteps xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.QueryParkingOperationWithTimeStepsJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("dQueryParkingOperationWithTimeSteps xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryParkingOperationWithTimeStepsGuestUser::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType SendParkingEmailTo(decimal OperationId, string Email, string SessionID, string User, out dynamic wsParameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string curDate = DateTime.Now.ToString("HHmmssddMMyy");

                var dSendParkingEmailTo = new
                {
                    ipark_in = new
                    {
                        d = curDate,
                        appcode = parkingAppCode,
                        appvers = parkingAppVersion,
                        OSID = OSID_WEB,
                        SessionID = SessionID,
                        u = User,
                        vers = VERS,
                        emailrecipient = Email,
                        operationid = OperationId,
                        ah = CalculateWSHash(curDate + parkingAppCode + parkingAppVersion + OSID_WEB + SessionID + User + VERS + Email + OperationId)
                    }
                };

                string strIn = JsonConvert.SerializeObject(dSendParkingEmailTo);

                Logger_AddLogMessage(string.Format("SendParkingEmailTo xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.SendParkingEmailToJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("SendParkingEmailTo xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "SendParkingEmailTo::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType QueryParkingOperationWithTimeSteps(string Username, string SessionId, List<string> PlateCollection, decimal CityId, decimal ZoneId, decimal Tariff, string Month, out SortedList parametersOut, out int FailedPermit)
        {
            ResultType rtRes = ResultType.Result_OK;            
            parametersOut = new SortedList();
            FailedPermit = 0;            

            integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

            string sLatLon;
            string sLatLonXml = LatLonXml(1, 1, out sLatLon).Replace("<gps>", string.Empty).Replace("</gps>", string.Empty);

            DateTime now = DateTime.Now;

            dQueryParkingOperationWithTimeSteps = new Dictionary<string, string>() { 
                { "u", "" }, 
                { "SessionID", "" }, 
                { "d", "" }, 
                { "di", "" }, 
                { "g", "" }, 
                { "strse", "" }, 
                { "ad", "" }, 
                { "isshopkeeperoperation", "" }, 
                { "vers", "" }, 
                { "appvers", "" }, 
                { "OSID", "" },
                { "ah", "" }
            };

            dQueryParkingOperationWithTimeSteps["u"] = Username;
            dQueryParkingOperationWithTimeSteps["SessionID"] = SessionId;
            int blockCount = 1;
            string prefix = "p";
            string plateNumber = "";
            foreach (string PlateBlock in PlateCollection)
            {
                int plateCount = 1;
                foreach (string Plate in PlateBlock.Split(','))
                {
                    if (blockCount > 1)
                    {
                        prefix = string.Format("p{0}_", blockCount);
                        plateNumber = plateCount.ToString();
                    }
                    else if (plateCount > 1) 
                    {
                        plateNumber = plateCount.ToString();
                    }
                    dQueryParkingOperationWithTimeSteps[prefix + plateNumber] = Plate;
                    plateCount++;
                }
                blockCount++;
            }

            /* Server datetime to installation datetime */
            INSTALLATION ins = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                .Where(i => i.INS_ID == CityId)
                .FirstOrDefault();

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ins.INS_TIMEZONE_ID);
            DateTime dtServerTime = DateTime.Now;
            DateTime dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);

            dQueryParkingOperationWithTimeSteps["d"] = dtInsDateTime.ToString("HHmmssddMMyy");
            dQueryParkingOperationWithTimeSteps["di"] = Month == "CURRENT" ? dtInsDateTime.ToString("HHmmssddMMyy") : Month;
            dQueryParkingOperationWithTimeSteps["g"] = ZoneId.ToString();
            dQueryParkingOperationWithTimeSteps["strse"] = string.Empty;
            dQueryParkingOperationWithTimeSteps["ad"] = Tariff.ToString();
            dQueryParkingOperationWithTimeSteps["isshopkeeperoperation"] = "0";
            dQueryParkingOperationWithTimeSteps["vers"] = VERS;
            dQueryParkingOperationWithTimeSteps["appvers"] = appVersion;
            dQueryParkingOperationWithTimeSteps["OSID"] = OSID_WEB;
            dQueryParkingOperationWithTimeSteps["ah"] = string.Empty;
            dQueryParkingOperationWithTimeSteps["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dQueryParkingOperationWithTimeSteps }));

            string strIn = GetXML(new List<Dictionary<string, string>>() { dQueryParkingOperationWithTimeSteps });
            Logger_AddLogMessage(string.Format("QueryParkingOperationWithTimeSteps xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

            string strOut = oIntegraMobileWS.QueryParkingOperationWithTimeSteps(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
            Logger_AddLogMessage(string.Format("QueryParkingOperationWithTimeSteps xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

            SortedList wsParameters = null;

            rtRes = FindOutParameters2(strOut, out wsParameters);

            if (parametersOut != null)
            {
                foreach (var key in wsParameters.Keys)
                    parametersOut.Add(key, wsParameters[key]);
            }

            if (rtRes == ResultType.Result_OK)
            {
                string resultKey;
                for (int i = 1; i <= PlateCollection.Count; i++)
                {
                    if (i == 1)
                    {
                        resultKey = "r";
                    }
                    else 
                    {
                        resultKey = string.Format("r{0}", i);
                    }

                    rtRes = (ResultType)Convert.ToInt32(wsParameters[resultKey].ToString());

                    if (rtRes != ResultType.Result_OK)
                    {
                        FailedPermit = i;
                        return rtRes;
                    }
                }                                        
            }

            return rtRes;
        }

        public ResultType ConfirmParkingOperationGuestUser(decimal Amount, string Date, string EndDate, string InitialDate, decimal g, string LicensePlate,
            decimal q_without_bon, decimal real_q, decimal q_fee, decimal ad, decimal t, decimal time_bal_used, decimal q_total, decimal q_vat, string SessionId, string u,
            int automatic_renewal, string strMonerisMD, string strMonerisCAVV, string strMonerisECI, bool bPaymentInPerson, int Paymeth, int PaymentProvider,
            string moneris_card_reference, string moneris_card_hash, string moneris_card_scheme, string moneris_masked_card_number, string moneris_expires_end_month,
            string moneris_expires_end_year, string moneris_date_time_local_fmt, string moneris_reference, string moneris_transaction_id, string moneris_auth_code,
            string moneris_auth_result, string payu_reference, string payu_auth_code, string payu_card_hash, string payu_card_reference, string payu_card_scheme,
            string payu_date_time_local_fmt, string payu_masked_card_number, string payu_transaction_id, string CardToken, string CardHash, string CardScheme, string CardPAN,
            string CardExpirationDate, string ChargeDateTime, string CardCFAuthCode, string CardCFTicketNumber, string CardCFTransactionID, string CardTransactionID,
            string stripe_card_reference, string stripe_card_scheme, string stripe_customer_id, string stripe_date_time_utc, string stripe_expires_end_month,
            string stripe_expires_end_year, string stripe_masked_card_number, string stripe_transaction_id, string paypal_PayerID, string paypal_paymentId, string paypal_token, 
            string bsredsys_card_reference, string bsredsys_card_hash, string bsredsys_card_scheme, string bsredsys_masked_card_number, string bsredsys_expires_end_month,
            string bsredsys_expires_end_year, string bsredsys_date_time_local_fmt, string bsredsys_reference, string bsredsys_transaction_id, string bsredsys_auth_code,
            string bsredsys_auth_result, out string str3DSUrl, out dynamic wsParameters)
        {

            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();
            str3DSUrl = "";
            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                var dConfirmParkingOperation = new
                {
                    ipark_in = new
                    {
                        q = Amount.ToString("0.#"),
                        d = Date,
                        ed = EndDate,
                        gps = string.Empty,
                        gpssel = string.Empty,
                        bd = InitialDate,
                        isgpsselected = "false",
                        isshopkeeperoperation = "0",
                        g = g.ToString("0.#"),
                        postpay = "0",
                        q_without_bon = q_without_bon.ToString("0.#"),
                        real_q = real_q.ToString("0.#"),
                        q_fee = q_fee.ToString("0.#"),
                        madtarinfo = string.Empty,
                        ad = ad.ToString("0.#"),
                        t = t.ToString("0.#"),
                        time_bal_used = time_bal_used.ToString("0.#"),
                        q_total = q_total.ToString("0.#"),
                        q_vat = q_vat.ToString("0.#"),
                        appvers = parkingAppVersion,
                        lang = LANG,
                        OSID = OSID_WEB,
                        SessionID = SessionId,
                        u = u,
                        vers = VERS,
                        WIFIMAC = string.Empty,
                        backofficeUsr = string.Empty,
                        automatic_renewal = automatic_renewal.ToString(),
                        p = LicensePlate,
                        moneris_md = strMonerisMD,
                        moneris_cavv = strMonerisCAVV,
                        moneris_eci = strMonerisECI,
                        payment_in_person = "0",
                        paymeth = Paymeth.ToString(),
                        ccprovider = PaymentProvider.ToString(),
                        moneris_card_reference = moneris_card_reference,
                        moneris_card_hash = moneris_card_hash,
                        moneris_card_scheme = moneris_card_scheme,
                        moneris_masked_card_number = moneris_masked_card_number,
                        moneris_expires_end_month = moneris_expires_end_month,
                        moneris_expires_end_year = moneris_expires_end_year,
                        moneris_date_time_local_fmt = moneris_date_time_local_fmt,
                        moneris_reference = moneris_reference,
                        moneris_transaction_id = moneris_transaction_id,
                        moneris_auth_code = moneris_auth_code,
                        moneris_auth_result = moneris_auth_result,
                        payu_reference = payu_reference,
                        payu_auth_code = payu_auth_code,
                        payu_card_hash = payu_card_hash,
                        payu_card_reference = payu_card_reference,
                        payu_card_scheme = payu_card_scheme,
                        payu_date_time_local_fmt = payu_date_time_local_fmt,
                        payu_masked_card_number = payu_masked_card_number,
                        payu_transaction_id = payu_transaction_id,
                        CardToken = CardToken,
                        CardHash = CardHash,
                        CardScheme = CardScheme,
                        CardPAN = CardPAN,
                        CardExpirationDate = CardExpirationDate,
                        ChargeDateTime = ChargeDateTime,
                        CardCFAuthCode = CardCFAuthCode,
                        CardCFTicketNumber = CardCFTicketNumber,
                        CardCFTransactionID = CardCFTransactionID,
                        CardTransactionID = CardTransactionID,
                        stripe_card_reference = stripe_card_reference,
                        stripe_card_scheme = stripe_card_scheme,
                        stripe_customer_id = stripe_customer_id,
                        stripe_date_time_utc = stripe_date_time_utc,
                        stripe_expires_end_month = stripe_expires_end_month,
                        stripe_expires_end_year = stripe_expires_end_year,
                        stripe_masked_card_number = stripe_masked_card_number,
                        stripe_transaction_id = stripe_transaction_id,
                        paypal_PayerID = paypal_PayerID,
                        paypal_paymentId = paypal_paymentId,
                        paypal_token = paypal_token,
                        bsredsys_card_reference = bsredsys_card_reference,
                        bsredsys_card_hash = bsredsys_card_hash,
                        bsredsys_card_scheme = bsredsys_card_scheme,
                        bsredsys_masked_card_number = bsredsys_masked_card_number,
                        bsredsys_expires_end_month = bsredsys_expires_end_month,
                        bsredsys_expires_end_year = bsredsys_expires_end_year,
                        bsredsys_date_time_local_fmt = bsredsys_date_time_local_fmt,
                        bsredsys_reference = bsredsys_reference,
                        bsredsys_transaction_id = bsredsys_transaction_id,
                        bsredsys_auth_code = bsredsys_auth_code,
                        bsredsys_auth_result = bsredsys_auth_result,
                        ah = CalculateWSHash(Amount.ToString("0.#") + Date + EndDate + InitialDate + "false" + "0" + g.ToString("0.#") + "0" + q_without_bon.ToString("0.#") +
                                             real_q.ToString("0.#") + q_fee.ToString("0.#") + ad.ToString("0.#") + t.ToString("0.#") + time_bal_used.ToString("0.#") +
                                             q_total.ToString("0.#") + q_vat.ToString("0.#") + parkingAppVersion + LANG + OSID_WEB + SessionId + u + VERS + automatic_renewal.ToString() +
                                             LicensePlate + strMonerisMD + strMonerisCAVV + strMonerisECI + "0" + Paymeth.ToString() + PaymentProvider.ToString() + moneris_card_reference + moneris_card_hash +
                                             moneris_card_scheme + moneris_masked_card_number + moneris_expires_end_month + moneris_expires_end_year + moneris_date_time_local_fmt +
                                             moneris_reference + moneris_transaction_id + moneris_auth_code + moneris_auth_result + payu_reference + payu_auth_code + payu_card_hash +
                                             payu_card_reference + payu_card_scheme + payu_date_time_local_fmt + payu_masked_card_number + payu_transaction_id + CardToken + CardHash +
                                             CardScheme + CardPAN + CardExpirationDate + ChargeDateTime + CardCFAuthCode + CardCFTicketNumber + CardCFTransactionID + CardTransactionID +
                                             stripe_card_reference + stripe_card_scheme + stripe_customer_id + stripe_date_time_utc + stripe_expires_end_month + stripe_expires_end_year +
                                             stripe_masked_card_number + stripe_transaction_id + paypal_PayerID + paypal_paymentId + paypal_token + bsredsys_card_reference + bsredsys_card_hash +
                                             bsredsys_card_scheme + bsredsys_masked_card_number + bsredsys_expires_end_month + bsredsys_expires_end_year + bsredsys_date_time_local_fmt + bsredsys_reference + 
                                             bsredsys_transaction_id + bsredsys_auth_code + bsredsys_auth_result)
                    }
                };

                string strIn = JsonConvert.SerializeObject(dConfirmParkingOperation);

                Logger_AddLogMessage(string.Format("ConfirmParkingOperationJSON xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.ConfirmParkingOperationJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("ConfirmParkingOperationJSON xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());

                if (rtRes == ResultType.Result_3DS_Validation_Needed)
                {
                    str3DSUrl = wsParameters["ThreeDSURL"].ToString();

                }

            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ConfirmParkingOperation::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType ConfirmParkingOperationGuestUser(decimal Amount, string Date, string EndDate, string InitialDate, decimal g, string LicensePlate,
            decimal q_without_bon, decimal real_q, decimal q_fee, decimal ad, decimal t, decimal time_bal_used, decimal q_total, decimal q_vat, string SessionId, string u, 
            out dynamic wsParameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();

            wsParameters = null;

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                var dConfirmParkingOperation = new
                {
                    ipark_in = new
                    {
                        actualgps = string.Empty,
                        q = Amount.ToString("0.#"),
                        d = Date,
                        ed = EndDate,
                        gps = string.Empty,
                        gpssel = string.Empty,
                        bd = InitialDate,
                        isgpsselected = "false",
                        isshopkeeperoperation = "0",
                        g = g.ToString("0.#"),
                        p = LicensePlate,
                        postpay = "0",
                        q_without_bon = q_without_bon.ToString("0.#"),
                        real_q = real_q.ToString("0.#"),
                        q_fee = q_fee.ToString("0.#"),
                        madtarinfo = string.Empty,
                        ad = ad.ToString("0.#"),
                        t = t.ToString("0.#"),
                        time_bal_used = time_bal_used.ToString("0.#"),
                        q_total = q_total.ToString("0.#"),
                        q_vat = q_vat.ToString("0.#"),
                        appcode = parkingAppCode,
                        appvers = parkingAppVersion,
                        lang = LANG,
                        OSID = OSID_WEB,
                        SessionID = SessionId,
                        u = u,
                        vers = VERS,
                        WIFIMAC = string.Empty,
                        ah = CalculateWSHash(Amount.ToString("0.#") + Date + EndDate + InitialDate + "false" + "0" + g.ToString("0.#") + LicensePlate + "0" + q_without_bon.ToString("0.#") +
                                             real_q.ToString("0.#") + q_fee.ToString("0.#") + ad.ToString("0.#") + t.ToString("0.#") + time_bal_used.ToString("0.#") +
                                             q_total.ToString("0.#") + q_vat.ToString("0.#") + parkingAppCode + parkingAppVersion + LANG + OSID_WEB + SessionId + u + "1.0")
                    }
                };

                string strIn = JsonConvert.SerializeObject(dConfirmParkingOperation);

                Logger_AddLogMessage(string.Format("ConfirmParkingOperationJSON xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.ConfirmParkingOperationJSON(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("ConfirmParkingOperationJSON xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                wsParameters = JsonConvert.DeserializeObject(strOut);

                rtRes = (ResultType)Convert.ToInt32(wsParameters["ipark_out"]["r"].ToString());
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ConfirmParkingOperation::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType ConfirmParkingOperation(decimal Amount, string Date, string EndDate, string InitialDate, decimal g, List<string> PlateCollection, decimal q_without_bon, decimal real_q, decimal q_fee, decimal ad, decimal t, 
                                                 decimal time_bal_used, decimal q_total, decimal q_vat, string SessionId, string u, int automatic_renewal, 
                                                 string  strMonerisMD, string strMonerisCAVV,string strMonerisECI,bool bPaymentInPerson, out string str3DSUrl)
        {
            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();
            str3DSUrl = "";

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                string sLatLon;
                string sLatLonXml = LatLonXml(1, 1, out sLatLon).Replace("<gps>", string.Empty).Replace("</gps>", string.Empty); ;

                dConfirmParkingOperation = new Dictionary<string, string>() { 
                    { "q", "" }, 
                    { "d", "" }, 
                    { "ed", "" }, 
                    { "gps", "" }, 
                    { "gpssel", "" }, 
                    { "bd", "" }, 
                    { "isgpsselected", "" }, 
                    { "isshopkeeperoperation", "" }, 
                    { "g", "" }, 
                    { "postpay", "" }, 
                    { "q_without_bon", "" }, 
                    { "real_q", "" }, 
                    { "q_fee", "" }, 
                    { "madtarinfo", "" }, 
                    { "ad", "" }, 
                    { "t", "" }, 
                    { "time_bal_used", "" }, 
                    { "q_total", "" }, 
                    { "q_vat", "" }, 
                    { "appvers", "" }, 
                    { "lang", "" }, 
                    { "OSID", "" }, 
                    { "SessionID", "" }, 
                    { "u", "" }, 
                    { "vers", "" }, 
                    { "WIFIMAC", "" }, 
                    { "backofficeUsr", "" },
                    { "automatic_renewal", "" },
                    { "moneris_md", "" },
                    { "moneris_cavv", "" },
                    { "moneris_eci", "" },
                    { "payment_in_person", "" },
                    { "ah", "" }
                };

                dConfirmParkingOperation["q"] = Amount.ToString("0.#");
                dConfirmParkingOperation["d"] = Date;
                dConfirmParkingOperation["ed"] = EndDate;
                dConfirmParkingOperation["gps"] = string.Empty;
                dConfirmParkingOperation["gpssel"] = string.Empty;
                dConfirmParkingOperation["bd"] = InitialDate;
                dConfirmParkingOperation["isgpsselected"] = "false";
                dConfirmParkingOperation["isshopkeeperoperation"] = "0";
                dConfirmParkingOperation["g"] = g.ToString("0.#");

                int blockCount = 1;
                string prefix = "p";
                string plateNumber = "";
                foreach (string PlateBlock in PlateCollection)
                {
                    int plateCount = 1;
                    foreach (string Plate in PlateBlock.Split(','))
                    {
                        if (blockCount > 1)
                        {
                            prefix = string.Format("p{0}_", blockCount);
                            plateNumber = plateCount.ToString();
                        }
                        else if (plateCount > 1)
                        {
                            plateNumber = plateCount.ToString();
                        }
                        dConfirmParkingOperation[prefix + plateNumber] = Plate;
                        plateCount++;
                    }
                    blockCount++;
                }

                dConfirmParkingOperation["postpay"] = "0";
                dConfirmParkingOperation["q_without_bon"] = q_without_bon.ToString("0.#");
                dConfirmParkingOperation["real_q"] = real_q.ToString("0.#");
                dConfirmParkingOperation["q_fee"] = q_fee.ToString("0.#");
                dConfirmParkingOperation["madtarinfo"] = "1";
                dConfirmParkingOperation["ad"] = ad.ToString("0.#");
                dConfirmParkingOperation["t"] = t.ToString("0.#");
                dConfirmParkingOperation["time_bal_used"] = time_bal_used.ToString("0.#");
                dConfirmParkingOperation["q_total"] = q_total.ToString("0.#");
                dConfirmParkingOperation["q_vat"] = q_vat.ToString("0.#");
                dConfirmParkingOperation["appvers"] = appVersion;
                dConfirmParkingOperation["lang"] = LANG;
                dConfirmParkingOperation["OSID"] = OSID_WEB;
                dConfirmParkingOperation["SessionID"] = SessionId;
                dConfirmParkingOperation["u"] = u;
                dConfirmParkingOperation["vers"] = VERS;
                dConfirmParkingOperation["WIFIMAC"] = string.Empty;
                dConfirmParkingOperation["backofficeUsr"] = string.Empty;
                dConfirmParkingOperation["automatic_renewal"] = automatic_renewal.ToString();
                dConfirmParkingOperation["ah"] = string.Empty;
                

                if (!string.IsNullOrEmpty(strMonerisMD) && !string.IsNullOrEmpty(strMonerisCAVV) && !string.IsNullOrEmpty(strMonerisECI))
                {
                    dConfirmParkingOperation["moneris_md"] = strMonerisMD;
                    dConfirmParkingOperation["moneris_cavv"] = strMonerisCAVV;
                    dConfirmParkingOperation["moneris_eci"] = strMonerisECI;
                }

                if (!bPaymentInPerson)
                {
                    dConfirmParkingOperation["payment_in_person"] = bPaymentInPerson?"1":"0";
                }

                dConfirmParkingOperation["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dConfirmParkingOperation }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dConfirmParkingOperation });
                Logger_AddLogMessage(string.Format("ConfirmParkingOperation xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.ConfirmParkingOperation(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("ConfirmParkingOperation xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {                    
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());
                    if (rtRes == ResultType.Result_3DS_Validation_Needed)
                    {
                        str3DSUrl = wsParameters["ThreeDSURL"].ToString();

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ConfirmParkingOperation::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        public ResultType ModifyOperationPlates(decimal OperationId, string SessionId, string u, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10)
        {
            ResultType rtRes = ResultType.Result_OK;
            SortedList parametersOut = new SortedList();

            try
            {
                integraMobile.ExternalWS.integraMobileWS.integraMobileWS oIntegraMobileWS = initWS();

                dModifyOperationPlates = new Dictionary<string, string>()
                {
                    { "ope_id", "" },
                    { "u", "" }, 
                    { "SessionID", "" }, 
                    { "p", "" },
                    { "p2", "" },
                    { "p3", "" },
                    { "p4", "" },
                    { "p5", "" },
                    { "p6", "" },
                    { "p7", "" },
                    { "p8", "" },
                    { "p9", "" },
                    { "p10", "" },
                    { "appvers", "" }, 
                    { "OSID", "" },
                    { "ah", "" }
                };

                dModifyOperationPlates["ope_id"] = OperationId.ToString();
                dModifyOperationPlates["u"] = u;
                dModifyOperationPlates["SessionID"] = SessionId;
                dModifyOperationPlates["p"] = p1;
                dModifyOperationPlates["p2"] = p2;
                dModifyOperationPlates["p3"] = p3;
                dModifyOperationPlates["p4"] = p4;
                dModifyOperationPlates["p5"] = p5;
                dModifyOperationPlates["p6"] = p6;
                dModifyOperationPlates["p7"] = p7;
                dModifyOperationPlates["p8"] = p8;
                dModifyOperationPlates["p9"] = p9;
                dModifyOperationPlates["p10"] = p10;
                dModifyOperationPlates["appvers"] = appVersion;
                dModifyOperationPlates["OSID"] = OSID_WEB;
                dModifyOperationPlates["ah"] = string.Empty;
                dModifyOperationPlates["ah"] = CalculateWSHash(GetOnlyValues(new List<Dictionary<string, string>>() { dModifyOperationPlates }));

                string strIn = GetXML(new List<Dictionary<string, string>>() { dModifyOperationPlates });
                Logger_AddLogMessage(string.Format("ModifyOperationPlates xmlIn={0}", PrettyXml(strIn)), LogLevels.logDEBUG);

                string strOut = oIntegraMobileWS.ModifyOperationPlates(strIn).Replace("\r\n  ", "").Replace("\r\n ", "").Replace("\r\n", "");
                Logger_AddLogMessage(string.Format("ModifyOperationPlates xmlOut ={0}", PrettyXml(strOut)), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ModifyOperationPlates::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }

        #endregion

        #region NonWS

        public class City
        {
            public decimal id { get; set; }
            public string Name { get; set; }
            public int Enabled { get; set; }
            public decimal? Child { get; set; }
            public DateTime IniApplyDate { get; set; }
            public DateTime EndApplyDate { get; set; }
            public INSTALLATION Installation { get; set; }
        }

        public List<City> GetCities(string text, decimal dCurID)
        {
            /* Allowed Installations */
            //List<int> installationsAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed(RequiredRole);

            List<decimal> InstallationsWithPermitTariffs = new List<decimal>();

            List<City> superInstallationsAllowed = backOfficeRepository.GetSuperInstallations(PredicateBuilder.True<SUPER_INSTALLATION>())
                                                    .Select(i => new City()
                                                    {
                                                        id = i.SINS_SUPER_INS_ID,
                                                        Name = i.INSTALLATION1.INS_DESCRIPTION,
                                                        Enabled = i.INSTALLATION1.INS_ENABLED,
                                                        Child = i.SINS_CHILD_INS_ID,
                                                        IniApplyDate = i.SINS_INI_APPLY_DATE,
                                                        EndApplyDate = i.SINS_END_APPLY_DATE,
                                                        Installation = i.INSTALLATION
                                                    })
                                                    .Where(i => (i.Name.ToLower().Contains(text.ToLower()) || string.IsNullOrEmpty(text)) &&
                                                                i.Enabled == 1 &&
                                                                DateTime.Now >= i.IniApplyDate &&
                                                                DateTime.Now <= i.EndApplyDate &&
                                                                i.Installation.CURRENCy.CUR_ID == dCurID)
                                                    .ToList();

            // For every superinstallation, we check if they have any tariff with tar_type = 1, and then, if any of these tariffs is valid at the moment.
            foreach (City SuperInstallation in superInstallationsAllowed)
            {
                if (SuperInstallation.Installation != null)
                {
                    if (!InstallationsWithPermitTariffs.Contains(SuperInstallation.Installation.INS_ID))
                    {
                        foreach (TARIFF tariff in SuperInstallation.Installation.TARIFFs)
                        {
                            if (tariff.TAR_TYPE == 1)
                            {
                                foreach (TARIFFS_IN_GROUP tig in tariff.TARIFFS_IN_GROUPs)
                                {                                    
                                    if (tig.TARGR_INI_APPLY_DATE <= DateTime.UtcNow && tig.TARGR_END_APPLY_DATE >= DateTime.UtcNow)
                                    {
                                        if (!InstallationsWithPermitTariffs.Contains(SuperInstallation.Installation.INS_ID))
                                        {
                                            InstallationsWithPermitTariffs.Add(SuperInstallation.Installation.INS_ID);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (InstallationsWithPermitTariffs.Contains(SuperInstallation.Installation.INS_ID))
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // Now we get only the superinstallations with any valid permit-type tariff in any of their installations
            superInstallationsAllowed = superInstallationsAllowed.Where(i => InstallationsWithPermitTariffs.Contains((decimal)i.Child)).ToList();

            /* All subinstallations from enabled and allowed superinstallations */
            List<decimal?> subInstallationIds = superInstallationsAllowed.Select(t => t.Child).ToList();

            /* Installations enabled and allowed, excluding subinstallations */
            List<City> installations = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                                            .Select(i => new City()
                                            {
                                                id = i.INS_ID,
                                                Name = i.INS_DESCRIPTION,
                                                Enabled = i.INS_ENABLED,
                                                Installation = i
                                            })
                                            .Where(i => (i.Name.ToLower().Contains(text.ToLower()) || string.IsNullOrEmpty(text)) &&
                                                i.Enabled == 1 &&
                                                /*installationsAllowed.Contains(Convert.ToInt32(i.id)) &&*/
                                                !subInstallationIds.Contains(i.id))
                                            .ToList();

            List<INSTALLATION> oLstCompatibleInstallations = (List<INSTALLATION>)geograficAndTariffsRepository.getInstallationsList(dCurID);
            // For every installation, we check if they have any tariff with tar_type = 1, and then, if any of these tariffs is valid at the moment.
            foreach (City city in installations)
            {
                if (oLstCompatibleInstallations.Where(r => r.INS_ID == city.id).Count() == 1)
                {
                    if (!InstallationsWithPermitTariffs.Contains(city.Installation.INS_ID))
                    {
                        foreach (TARIFF tar in city.Installation.TARIFFs)
                        {
                            if (tar.TAR_TYPE == 1)
                            {
                                foreach (TARIFFS_IN_GROUP tig in tar.TARIFFS_IN_GROUPs)
                                {
                                    if (tig.TARGR_INI_APPLY_DATE <= DateTime.UtcNow && tig.TARGR_END_APPLY_DATE >= DateTime.UtcNow)
                                    {
                                        if (!InstallationsWithPermitTariffs.Contains(city.Installation.INS_ID))
                                        {
                                            InstallationsWithPermitTariffs.Add(city.Installation.INS_ID);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (InstallationsWithPermitTariffs.Contains(city.Installation.INS_ID))
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // Now we get only the installations with any valid permit-type tariff
            installations = installations.Where(i => InstallationsWithPermitTariffs.Contains(i.id)).ToList();

            /* Installations enabled and allowed union Super-installations enabled and allowed */
            List<City> installationsAndSuperInstallations = installations.Union(superInstallationsAllowed).GroupBy(t => t.id).Select(g => g.First()).OrderBy(t => t.Name).ToList();

           

            return installationsAndSuperInstallations;
        }

        #endregion

    }
}