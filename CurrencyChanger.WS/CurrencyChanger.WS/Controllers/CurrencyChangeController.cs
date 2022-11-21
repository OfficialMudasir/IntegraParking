using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Globalization;
using System.Web.Security;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography;
using CurrencyChanger.WS.Domain.Abstract;
using CurrencyChanger.WS.Domain;
using CurrencyChanger.WS.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

namespace CurrencyChanger.WS.Controllers
{

    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]    
    public class CurrencyChangeController : ApiController
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CurrencyChangeController));

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;

        private ICurrencyChangeRepository currencyChangeRepository = null;
        private enum ChangeProviders
        {
            Unknown=0,
            CurrencyLayer=1,
        }

        private enum ResultType
        {
            Result_OK=1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_Invalid_UTCTime = -2,
            Result_Error_Getting_Change_From_Provider = -3,
            Result_Error_Generic = -4,
        }


        public class ChangeRequest
        {
            public string SrcIsoCode { get; set; }
            public string DstIsoCode { get; set; }
            public string UTCTime { get; set; }
            public string AuthHash { get; set; }

        }

        public class ChangeResponse
        {
            public class CChangeData
            {
                public string SrcIsoCode { get; set; }
                public string DstIsoCode { get; set; }
                public double Change { get; set; }
                public DateTime AdquisitionUTCDateTime { get; set; }
            }

            public int Result {get;set;}
            public CChangeData ChangeData { get; set; }
            

        }

        public CurrencyChangeController(ICurrencyChangeRepository currencyChangeRepository)
        {
            this.currencyChangeRepository = currencyChangeRepository;
        }

        [Route("currencychange/{strSrcIsoCode}/{strDstIsoCode}/{strUTCTime}/{strAuthHash}")]
        public ChangeResponse Get(string strSrcIsoCode, string strDstIsoCode, string strUTCTime, string strAuthHash)
        {

            return GetChange(new ChangeRequest()
                     {
                         SrcIsoCode = strSrcIsoCode,
                         DstIsoCode = strDstIsoCode,
                         UTCTime = strUTCTime,
                         AuthHash = strAuthHash
                     });                                       
        }

        [Route("currencychange")]
        public ChangeResponse Post(ChangeRequest oRequest)
        {
            return GetChange(oRequest);
        }



        protected ChangeResponse GetChange(ChangeRequest oRequest)
        {

            ChangeResponse oResult = null;

            try
            {
                InitializeStatic();

                if (!IsHashCorrect(oRequest))
                {
                    oResult = new ChangeResponse()
                    {
                        Result = (int)ResultType.Result_Error_InvalidAuthenticationHash,
                    };

                }
                else
                {
                    bool bISCorrectDate = false;


                    try
                    {
                        DateTime dt = DateTime.ParseExact(oRequest.UTCTime,"yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);

                        double dTotalSeconds = (DateTime.UtcNow - dt).TotalSeconds;

                        if (Math.Abs(dTotalSeconds) <= currencyChangeRepository.GetParameterValue("MAX_UTC_DIFF_TIME", 180))
                        {
                            bISCorrectDate = true;
                        }

                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "GetChange::Exception", LogLevels.logERROR);                                        
                    }



                    if (!bISCorrectDate)
                    {
                        oResult = new ChangeResponse()
                        {
                            Result = (int)ResultType.Result_Error_Invalid_UTCTime,
                        };
                    }
                    else
                    {

                        if (oRequest.SrcIsoCode != oRequest.DstIsoCode)
                        {

                            bool bGetChange = false;
                            CURRENCY_CHANGE oChange = null;
                            SortedList<string, double> oOtherCurrenciesChanges = null;
                            if (currencyChangeRepository.GetCurrencyChange(oRequest.SrcIsoCode, oRequest.DstIsoCode, ref oChange, ref oOtherCurrenciesChanges))
                            {
                                if (oChange == null)
                                {
                                    bGetChange = true;
                                }
                                else
                                {
                                    int iChangeTTL = currencyChangeRepository.GetParameterValue("CHANGE_TTL", 1800);

                                    if ((DateTime.UtcNow - oChange.CUCH_CHANGE_UTC_DATE).TotalSeconds > iChangeTTL)
                                    {
                                        bGetChange = true;
                                    }
                                }

                            }
                            else
                            {
                                bGetChange = true;
                            }


                            if (bGetChange)
                            {
                                double dChange = 0;
                                if (getCurrencyChange(oRequest.SrcIsoCode, oRequest.DstIsoCode, ref oOtherCurrenciesChanges, ref dChange))
                                {
                                    oResult = new ChangeResponse()
                                    {
                                        Result = (int)ResultType.Result_OK,
                                        ChangeData = new ChangeResponse.CChangeData()
                                        {
                                            SrcIsoCode = oRequest.SrcIsoCode,
                                            DstIsoCode = oRequest.DstIsoCode,
                                            Change = dChange,
                                            AdquisitionUTCDateTime = DateTime.UtcNow,
                                        }

                                    };

                                    currencyChangeRepository.SetCurrencyChange(oRequest.SrcIsoCode, oRequest.DstIsoCode, dChange, ref oOtherCurrenciesChanges);

                                }
                                else
                                {
                                    oResult = new ChangeResponse()
                                    {
                                        Result = (int)ResultType.Result_Error_Getting_Change_From_Provider,
                                    };

                                }

                            }
                            else
                            {
                                oResult = new ChangeResponse()
                                {
                                    Result = (int)ResultType.Result_OK,
                                    ChangeData = new ChangeResponse.CChangeData()
                                    {
                                        SrcIsoCode = oRequest.SrcIsoCode,
                                        DstIsoCode = oRequest.DstIsoCode,
                                        Change = Convert.ToDouble(oChange.CUCH_CHANGE),
                                        AdquisitionUTCDateTime = oChange.CUCH_CHANGE_UTC_DATE,
                                    }
                                };
                            }
                        }
                        else
                        {
                            oResult = new ChangeResponse()
                            {
                                Result = (int)ResultType.Result_OK,
                                ChangeData = new ChangeResponse.CChangeData()
                                {
                                    SrcIsoCode = oRequest.SrcIsoCode,
                                    DstIsoCode = oRequest.DstIsoCode,
                                    Change = 1.0,
                                    AdquisitionUTCDateTime = DateTime.UtcNow,
                                }
                            };

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GetChange::Exception", LogLevels.logERROR);
                oResult = new ChangeResponse()
                {
                    Result = (int)ResultType.Result_Error_Generic,
                };
                                    
            }

            try
            {
                string json = JsonConvert.SerializeObject(oResult);
                Logger_AddLogMessage(string.Format("GetChange Result.json={0}", PrettyJSON(json)), LogLevels.logINFO);                            
            }
            catch{}


            return oResult;

        }


        protected bool getCurrencyChange(string strSrcIsoCode, string strDstIsoCode, ref SortedList<string, double> oOtherCurrenciesChanges, ref double dChange)
        {
            bool bRes = false;
            ChangeProviders eProvider = ChangeProviders.Unknown;
            
            try
            {
       
                if (Enum.TryParse(currencyChangeRepository.GetParameterValue("CHANGE_PROVIDER"), out eProvider))
                {
                    switch (eProvider)
                    {
                        case ChangeProviders.CurrencyLayer:

                            bRes=getCurrencyChangeCurrencyLayer(strSrcIsoCode, strDstIsoCode, ref oOtherCurrenciesChanges, ref dChange);
                            break;
                        default:
                            break;
                    }



                }
            }
            catch
            {
                bRes = false;
            }


            return bRes;
        }


        protected bool getCurrencyChangeCurrencyLayer(string strSrcIsoCode, string strDstIsoCode, ref SortedList<string,double> oOtherCurrenciesChanges, 
                                                      ref double dChange)
        {
            bool bRes = false;

            try
            {
                string strURL=currencyChangeRepository.GetParameterValue("SERVICE_URL");
                if (!string.IsNullOrEmpty(strURL))
                {
                    string strDstIsoCodes = strDstIsoCode;
                    foreach (KeyValuePair<string, double> oTuple in oOtherCurrenciesChanges)
                    {
                        strDstIsoCodes += "," + oTuple.Key;
                    }

                    strURL += string.Format("&currencies={1}&source={0}&format=1", strSrcIsoCode, strDstIsoCodes);

                    WebRequest request = WebRequest.Create(strURL);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Timeout = currencyChangeRepository.GetParameterValue("REMOTE_CALL_TIMEOUT", 5000);

                    Logger_AddLogMessage(string.Format("getCurrencyChangeCurrencyLayer request.url={0}", strURL), LogLevels.logINFO);

                    request.ContentLength = 0;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    // Close the Stream object.
                    dataStream.Close();
                    // Get the response.
                    string strChange = "";

                    try
                    {

                        WebResponse response = request.GetResponse();
                        // Display the status.
                        HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                        if (oWebResponse.StatusDescription == "OK")
                        {
                            // Get the stream containing content returned by the server.
                            dataStream = response.GetResponseStream();
                            // Open the stream using a StreamReader for easy access.
                            StreamReader reader = new StreamReader(dataStream);
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();
                            // Display the content.

                            Logger_AddLogMessage(string.Format("getCurrencyChangeCurrencyLayer response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                            // Clean up the streams.


                            dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                            var oQuotes = oResponse["quotes"];
                            strChange = oQuotes[strSrcIsoCode+strDstIsoCode];
                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";

                            dChange = Convert.ToDouble(strChange, numberFormatProvider);

                            SortedList<string,double> oTempOtherCurrenciesChanges= new SortedList<string,double>();

                            foreach (KeyValuePair<string, double> oTuple in oOtherCurrenciesChanges)
                            {
                                strChange = oQuotes[strSrcIsoCode + oTuple.Key];
                                oTempOtherCurrenciesChanges.Add(oTuple.Key, Convert.ToDouble(strChange, numberFormatProvider));
                            }
                            
                            oOtherCurrenciesChanges = oTempOtherCurrenciesChanges;

                            bRes = true;

                            Logger_AddLogMessage(string.Format("getCurrencyChangeCurrencyLayer Src Currency={0}; Dst Currency={1}; Change={2}; ", strSrcIsoCode, strDstIsoCode, dChange), LogLevels.logINFO);

                            reader.Close();
                            dataStream.Close();
                        }

                        response.Close();
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "getCurrencyChangeCurrencyLayer::Exception", LogLevels.logERROR);
                    }




                }



            }
            catch
            {
                bRes = false;
            }

            return bRes;

        }


        protected void InitializeStatic()
        {
            if (_hmacsha256 == null)
            {
                int iKeyLength = 64;

                if (_hMacKey == null)
                {
                    _hMacKey = currencyChangeRepository.GetParameterValue("HASH_KEY");
                }

                if (_normKey == null)
                {
                    byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(_hMacKey);
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
                }


                _hmacsha256 = new HMACSHA256(_normKey);
            }
            

        }


        protected bool IsHashCorrect(ChangeRequest oRequest)
        {           
            return  (CalculateHash(oRequest.SrcIsoCode + oRequest.DstIsoCode + oRequest.UTCTime) == oRequest.AuthHash);
        }

        protected string CalculateHash(string strInput)
        {
            string strRes = "";
            try
            {

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

        protected static string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }


        protected static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        protected static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

    }
}
