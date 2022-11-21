using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Newtonsoft.Json;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Infrastructure
{
    public class CCurrencyConvertor
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CCurrencyConvertor));

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;

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

            public int Result { get; set; }
            public CChangeData ChangeData { get; set; }


        }

        private enum ResultType
        {
            Result_OK = 1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_Invalid_UTCTime = -2,
            Result_Error_Getting_Change_From_Provider = -3,
            Result_Error_Generic = -4,
        }

        public static double ConvertCurrency(double dQuantityFrom, string strISOCodeFrom, string strISOCodeTo, out double dChangeApplied)
        {
            double dRes = -1.0;
            dChangeApplied = -1.0;
            try
            {
                
                if (strISOCodeFrom != strISOCodeTo)
                {
                    dChangeApplied = GetChangeToApply( strISOCodeFrom, strISOCodeTo);
                    if (dChangeApplied > 0)
                    {
                        dRes = dQuantityFrom * dChangeApplied;
                        dRes = Math.Round(dRes, 4);
                    }
                    else
                    {
                        dRes = -1.0;
                    }
                }
                else
                {
                    dChangeApplied = 1.0;
                    dRes = dQuantityFrom;
                }

            }
            catch(Exception e)
            {
                dRes = -1.0;
                m_Log.LogMessage(LogLevels.logERROR, "ConvertCurrency: ", e);
            }

            return dRes;
        }

        public static double GetChangeToApply(string strISOCodeFrom, string strISOCodeTo)
        {
            double dChangeToApply = -1.0;
           
            try
            {
                if (strISOCodeFrom != strISOCodeTo)
                {

                    InitializeStatic();

                    string strURL = ConfigurationManager.AppSettings["CurrencyServiceURL"] ?? "https://ws.iparkme.com/CurrencyChanger.WS/currencychange";

                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true);


                    WebRequest request = WebRequest.Create(strURL);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["CurrencyServiceTimeout"] ?? "5000"); ;

                    DateTime dt = DateTime.UtcNow;
                    string strUTCTime = dt.ToString("yyyyMMddHHmmssfff");

                    ChangeRequest oRequest = new ChangeRequest()
                    {
                        SrcIsoCode = strISOCodeFrom,
                        DstIsoCode = strISOCodeTo,
                        UTCTime = strUTCTime,
                        AuthHash = CalculateHash(strISOCodeFrom + strISOCodeTo + strUTCTime),
                    };


                    var json = JsonConvert.SerializeObject(oRequest);

                    byte[] byteArray = Encoding.UTF8.GetBytes(json);

                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();

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

                            ChangeResponse oResponse = (ChangeResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(ChangeResponse));

                            if (((ResultType)oResponse.Result) == ResultType.Result_OK)
                            {
                                dChangeToApply = oResponse.ChangeData.Change;
                            }

                            reader.Close();
                            dataStream.Close();
                        }

                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GetChangeToApply: ", ex);
                    }
                }
                else
                {
                    dChangeToApply = 1.0;
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetChangeToApply: ", ex);
            }

            m_Log.LogMessage(LogLevels.logINFO, string.Format("GetChangeToApply: {0}->{1} : {2}", strISOCodeFrom, strISOCodeTo, dChangeToApply));

            return dChangeToApply;
        }



        public static double ConvertCurrency(double lQuantityFrom, string strISOCodeFrom, string strISOCodeTo)
        {
            double dRes = -1;
            try
            {
                double dChangeApplied;

                dRes = ConvertCurrency(lQuantityFrom, strISOCodeFrom, strISOCodeTo, out dChangeApplied);

            }
            catch
            {
                dRes = -1;
            }

            return dRes;
        }


        

        public static List<CultureInfo> CultureInfoFromCurrencyISO(string isoCode)
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            List<CultureInfo> Result = new List<CultureInfo>();
            foreach (CultureInfo ci in cultures)
            {
                RegionInfo ri = new RegionInfo(ci.LCID);
                if (ri.ISOCurrencySymbol == isoCode)
                {
                    if (!Result.Contains(ci))
                        Result.Add(ci);
                }
            }
            return Result;
        }


        protected static void InitializeStatic()
        {
            if (_hmacsha256 == null)
            {
                int iKeyLength = 64;

                if (_hMacKey == null)
                {
                    _hMacKey = ConfigurationManager.AppSettings["CurrencyServiceHashSeed"] ??  @"2_)V6RQu\6ZZa9R~L>CQ)z?G";
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

        protected static string CalculateHash(string strInput)
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
                Console.Write(e.Message);
            }


            return strRes;
        }

    }
}
