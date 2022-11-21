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
using VehicleType.WS.Domain.Abstract;
using VehicleType.WS.Domain;
using VehicleType.WS.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

namespace VehicleType.WS.Controllers
{

    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]    
    public class VehicleTypeController : ApiController
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(VehicleTypeController));

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        private const long BIG_PRIME_NUMBER = 2147483647;

        private IVehicleTypeRepository oRepository = null;

        private enum ResultType
        {
            Result_OK = 1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_NotFound = -2,
            Result_Error_Generic = -3,
        }

        public class VehicleTypeRequest
        {
            public string Plate { get; set; }
            public string AuthHash { get; set; }

        }

        public class VehicleTypeResponse
        {
            public int Result { get; set; }
            public string Data { get; set; }
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

        public VehicleTypeController(IVehicleTypeRepository Repository)
        {
            this.oRepository = Repository;
        }

        [Route("vehicletype/{sPlate}/{sAuthHash}")]
        public VehicleTypeResponse Get(string sPlate, string sAuthHash)
        {

            return GetVehicleType(new VehicleTypeRequest()
                     {
                         Plate = sPlate,
                         AuthHash = sAuthHash
                     });                                       
        }

        [Route("vehicletype")]
        public VehicleTypeResponse Post(VehicleTypeRequest oRequest)
        {
            return GetVehicleType(oRequest);
        }



        protected VehicleTypeResponse GetVehicleType(VehicleTypeRequest oRequest)
        {

            VehicleTypeResponse oResult = null;

            try
            {
                InitializeStatic();

                if (!IsHashCorrect(oRequest))
                {
                    Logger_AddLogMessage(string.Format("GetVehicleType {0} --> Error {1}", oRequest.Plate, (int)ResultType.Result_Error_InvalidAuthenticationHash), LogLevels.logERROR);

                    oResult = new VehicleTypeResponse()
                    {
                        Result = (int)ResultType.Result_Error_InvalidAuthenticationHash,
                    };

                }
                else
                {

                    string sVehicleType = null;
                    if (oRepository.GetVehicleType(oRequest.Plate, out sVehicleType))
                    {
                        Logger_AddLogMessage(string.Format("GetVehicleType {0} --> {1}", oRequest.Plate, sVehicleType), LogLevels.logINFO);

                        oResult = new VehicleTypeResponse()
                        {
                            Result = (int)(sVehicleType != null ? ResultType.Result_OK : ResultType.Result_Error_NotFound),
                            Data = sVehicleType
                        };
                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("GetVehicleType {0} --> Error {1}", oRequest.Plate, (int)ResultType.Result_Error_Generic), LogLevels.logERROR);

                        oResult = new VehicleTypeResponse()
                        {
                            Result = (int)ResultType.Result_Error_Generic,
                        };
                    }
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, string.Format("GetVehicleType::Exception for vehicle {0}", oRequest.Plate), LogLevels.logERROR);
                oResult = new VehicleTypeResponse()
                {
                    Result = (int)ResultType.Result_Error_Generic,
                };
                                    
            }

            try
            {
                string json = JsonConvert.SerializeObject(oResult);
                //Logger_AddLogMessage(string.Format("GetVehicleType Result.json={0}", PrettyJSON(json)), LogLevels.logINFO);                            
            }
            catch{}


            return oResult;

        }


        protected void InitializeStatic()
        {
            if (_hmacsha256 == null)
            {
                int iKeyLength = 64;

                if (_hMacKey == null)
                {
                    _hMacKey = System.Configuration.ConfigurationManager.AppSettings["HashKey"];
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


        protected bool IsHashCorrect(VehicleTypeRequest oRequest)
        {
            if ((System.Configuration.ConfigurationManager.AppSettings["CheckHash"] ?? "1") == "1")
                return (CalculateHash(oRequest.Plate) == oRequest.AuthHash);
            else
                return true;
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
