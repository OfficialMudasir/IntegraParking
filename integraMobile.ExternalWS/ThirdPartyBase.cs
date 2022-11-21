using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;



namespace integraMobile.ExternalWS
{
    public class ThirdPartyBase
    {
        private const long BIG_PRIME_NUMBER = 2147483647;
        protected static string _xmlTagName = "ipark";
        protected const string OUT_SUFIX = "_out";
        protected const int DEFAULT_WS_TIMEOUT = 5000; //ms        

        private IKernel m_kernel = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }
        [Inject]
        public IRetailerRepository retailerRepository { get; set; }
        
        //Log4net Wrapper class
        protected static CLogWrapper m_Log; // = new CLogWrapper(typeof(ThirdPartyOperation));

        protected Notifications m_notifications;

        protected static MadridPlatform.AuthSession m_oMadridPlatformAuthSession = null;
        protected static int m_iMadridPlatformOpenSessions = 0;
        protected static DateTime? m_dtMadridPlatformStartSession = null;
        protected static readonly object m_oLocker = new object();

        protected static string m_sMadridAccessToken = null;
        protected static DateTime? m_dtMadridAccessTokenStart = null;
        protected static int? m_iMadridTokenExpirationSeconds = null;
        protected static readonly object m_oMadridLocker = new object();

        protected static string m_sBSMAccessToken = null;
        protected static DateTime? m_dtBSMAccessTokenStart = null;
        protected static int? m_iBSMTokenExpirationSeconds = null;
        protected static readonly object m_oBSMLocker = new object();

        protected static string m_sPermitsAccessToken = null;
        protected static DateTime? m_dtPermitsAccessTokenStart = null;
        protected static int? m_iPermitsTokenExpirationSeconds = null;
        protected static readonly object m_oPermitsLocker = new object();

        protected static string m_sSIRAccessToken = null;
        protected static DateTime? m_dtSIRAccessTokenStart = null;
        protected static int? m_iSIRTokenExpirationSeconds = null;
        protected static readonly object m_oSIRLocker = new object();

        // Pagatelia 
        protected static string _hMacKey = null;
        protected static byte[] _normKey = null;
        protected static HMACSHA256 _hmacsha256 = null;

        public ThirdPartyBase()
        {
            m_kernel = new StandardKernel(new integraMobileThirdPartyConfirmModule());
            m_kernel.Inject(this);

            m_notifications = NotificationsFactory.CreateNotifications();
        }

        public ThirdPartyBase(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, IGeograficAndTariffsRepository oGeograficAndTariffsRepository, IRetailerRepository oRetailerRepository)
        {
            customersRepository = oCustomersRepository;
            infraestructureRepository = oInfraestructureRepository;
            geograficAndTariffsRepository = oGeograficAndTariffsRepository;
            retailerRepository = oRetailerRepository;
        }

        protected int Get3rdPartyWSTimeout()
        {
            return infraestructureRepository.GetRateWSTimeout();
        }

        protected string CalculateGtechnaWSHash(string strMACKey, string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
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
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

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
                Logger_AddLogException(e, "CalculateWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
        }


        protected string CalculateEysaWSHash(string strMACKey, string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
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
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

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
                Logger_AddLogException(e, "CalculateEysaWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
        }


        protected string CalculateStandardWSHash(string strMACKey, string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
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
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

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
                Logger_AddLogException(e, "CalculateStandardWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
        }

        protected string CalculateBilbaoWSHash(string strMACKey, string strInput)
        {
            
            return CalculateStandardWSHash(strMACKey, strInput);
        }
        protected string EncryptSantBoi(string strKey, string strSecretIV, string strInput)
        {
            string strRes = "";

            try
            {

                string strHashKey = getStringHashSha256(strKey).Substring(0,32);
                string strIVKey = getStringHashSha256(strSecretIV).Substring(0,16);

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashKey);
                byte[] IVBytes = System.Text.Encoding.UTF8.GetBytes(strIVKey);


                // Instantiate a new Aes object to perform string symmetric encryption
                
                Aes encryptor = Aes.Create();

                encryptor.Mode = CipherMode.CBC;
                //encryptor.KeySize = 256;
                //encryptor.BlockSize = 128;
                //encryptor.Padding = PaddingMode.Zeros;

                // Set key and IV
                encryptor.Key = keyBytes;
                encryptor.IV = IVBytes;

                // Instantiate a new MemoryStream object to contain the encrypted bytes
                MemoryStream memoryStream = new MemoryStream();

                // Instantiate a new encryptor from our Aes object
                ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

                // Instantiate a new CryptoStream object to process the data and write it to the 
                // memory stream
                CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

                // Convert the plainText string into a byte array
                byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(strInput);

                // Encrypt the input plaintext string
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);

                // Complete the encryption process
                cryptoStream.FlushFinalBlock();

                // Convert the encrypted data from a MemoryStream to a byte array
                byte[] cipherBytes = memoryStream.ToArray();

                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();

                string strTemp = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
                byte[] byTemp = System.Text.Encoding.UTF8.GetBytes(strTemp);

                // Convert the encrypted byte array to a base64 encoded string
                strRes = Convert.ToBase64String(byTemp, 0, byTemp.Length); 
               
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "EncryptSantBoi::Exception", LogLevels.logERROR);
            }

            return strRes;
        }

        private string getStringHashSha256(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }


        private byte[] getHashSha256(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            
            return hash;
        }


        private static void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }

        private static void PagateliaInitializeStatic()
        {

            int iKeyLength = 64;

            if (_hMacKey == null)
            {
                _hMacKey = ConfigurationManager.AppSettings["PagateliaWsAuthHashKey"].ToString();
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

            if (_hmacsha256 == null)
            {
                _hmacsha256 = new HMACSHA256(_normKey);
            }

        }



        public string CalculatePagateliaWsHash(string strInput)
        {
            string strRes = "";
            try
            {
                PagateliaInitializeStatic();

                if (_hmacsha256 != null)
                {
                    byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                    byte[] hash = _hmacsha256.ComputeHash(inputBytes);

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
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculatePagateliaWsHash::Exception", LogLevels.logERROR);
            }


            return strRes;
        }

        protected ResultType FindOutParameters(string xmlIn, out SortedList parameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            parameters = new SortedList();


            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    if (xmlIn.StartsWith("<?xml"))
                    {
                        xmlInDoc.LoadXml(xmlIn);
                    }
                    else
                    {
                        xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);
                    }

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        switch (Node.Name)
                        {
                            default:

                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        int i = 0;
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {
                                            if (!ChildNode.ChildNodes[0].HasChildNodes)
                                            {
                                                if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] == null)
                                                {
                                                    parameters[Node.Name + "_" + ChildNode.Name] = ChildNode.InnerText.Trim();
                                                }
                                                else
                                                {
                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] = ChildNode.InnerText.Trim();
                                                }
                                            }
                                            else
                                            {
                                                int j = 0;
                                                foreach (XmlNode ChildNode2 in ChildNode.ChildNodes)
                                                {
                                                    if (!ChildNode2.HasChildNodes)
                                                    {
                                                        if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] == null)
                                                        {
                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] = ChildNode2.InnerText.Trim();
                                                        }
                                                        else
                                                        {
                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString()] = ChildNode2.InnerText.Trim();
                                                        }
                                                    }
                                                    else
                                                    {

                                                        if (!ChildNode2.ChildNodes[0].HasChildNodes)
                                                        {


                                                            if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] == null)
                                                            {
                                                                parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] = ChildNode2.InnerText.Trim();
                                                            }
                                                            else
                                                            {
                                                                parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString()] = ChildNode2.InnerText.Trim();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            int k = 0;
                                                            foreach (XmlNode ChildNode3 in ChildNode2.ChildNodes)
                                                            {
                                                                if (!ChildNode3.HasChildNodes)
                                                                {
                                                                    if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name] == null)
                                                                    {
                                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name] = ChildNode3.InnerText.Trim();
                                                                    }
                                                                    else
                                                                    {
                                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name + "_" + k.ToString()] = ChildNode3.InnerText.Trim();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (!ChildNode3.ChildNodes[0].HasChildNodes)
                                                                    {

                                                                        if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name] == null)
                                                                        {
                                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name] = ChildNode3.InnerText.Trim();
                                                                        }
                                                                        else
                                                                        {
                                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name + "_" + k.ToString()] = ChildNode3.InnerText.Trim();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        int l = 0;
                                                                        foreach (XmlNode ChildNode4 in ChildNode3.ChildNodes)
                                                                        {

                                                                            /*if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] == null)
                                                                            {
                                                                                parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] = ChildNode4.InnerText.Trim();
                                                                            }
                                                                            else
                                                                            {
                                                                                parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name + "_" + l.ToString()] = ChildNode4.InnerText.Trim();
                                                                            }*/
                                                                            if (!ChildNode4.HasChildNodes)
                                                                            {
                                                                                if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] == null)
                                                                                {
                                                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] = ChildNode4.InnerText.Trim();
                                                                                }
                                                                                else
                                                                                {
                                                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name + "_" + l.ToString()] = ChildNode4.InnerText.Trim();
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!ChildNode4.ChildNodes[0].HasChildNodes)
                                                                                {

                                                                                    if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] == null)
                                                                                    {
                                                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name] = ChildNode4.InnerText.Trim();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + /*j.ToString() + "_" +*/ ChildNode3.Name + "_" + k.ToString() + "_" + ChildNode4.Name + "_" + l.ToString()] = ChildNode4.InnerText.Trim();
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    int m = 0;
                                                                                    foreach (XmlNode ChildNode5 in ChildNode4.ChildNodes)
                                                                                    {

                                                                                        if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name + "_" + ChildNode4.Name + "_" + l.ToString() + "_" + ChildNode5.Name] == null)
                                                                                        {
                                                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name + "_" + ChildNode4.Name + "_" + l.ToString() + "_" + ChildNode5.Name] = ChildNode5.InnerText.Trim();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString() + "_" + ChildNode3.Name + "_" + ChildNode4.Name + "_" + l.ToString() + "_" + ChildNode5.Name + "_" + m.ToString()] = ChildNode4.InnerText.Trim();
                                                                                        }


                                                                                    }

                                                                                }
                                                                                l++;
                                                                            }

                                                                        }

                                                                    }
                                                                    k++;
                                                                }
                                                            }

                                                        }
                                                        j++;
                                                    }
                                                }
                                            }
                                            i++;
                                            parameters[Node.Name + "_" + ChildNode.Name + "_num"] = i;
                                        }
                                    }
                                    else
                                    {
                                        parameters[Node.Name] = Node.InnerText.Trim();
                                    }
                                }
                                else
                                {
                                    parameters[Node.Name] = null;
                                }

                                break;

                        }

                    }

                    if (Nodes.Count == 0)
                    {
                        Logger_AddLogMessage(string.Format("FindParameters: Bad Input XML: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logERROR);
                        rtRes = ResultType.Result_Error_Generic;

                    }


                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindInputParameters: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindInputParameters::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }

        protected ResultType Convert_ResultTypeStandardParkingWS_TO_ResultType(ResultTypeStandardParkingWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeStandardParkingWS.ResultSP_ParkingWarning_HighPollutionEpisode_Level2:
                    rtResultType = ResultType.Result_ParkingWarning_HighPollutionEpisode_Level2;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_ParkingWarning_HighPollutionEpisode_Level1:
                    rtResultType = ResultType.Result_ParkingWarning_HighPollutionEpisode_Level1;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_InvalidAuthenticationHash:
                    rtResultType = ResultType.Result_Error_InvalidAuthenticationHash;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_ParkingMaximumTimeUsed:
                    rtResultType = ResultType.Result_Error_ParkingMaximumTimeUsed;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_NotWaitedReentryTime:
                    rtResultType = ResultType.Result_Error_NotWaitedReentryTime;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_RefundNotPossible:
                    rtResultType = ResultType.Result_Error_RefundNotPossible;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Fine_Number_Not_Found:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Fine_Type_Not_Payable:
                    rtResultType = ResultType.Result_Error_Fine_Type_Not_Payable;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Fine_Payment_Period_Expired:
                    rtResultType = ResultType.Result_Error_Fine_Payment_Period_Expired;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Fine_Number_Already_Paid:
                    rtResultType = ResultType.Result_Error_Fine_Number_Already_Paid;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Generic:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Invalid_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Missing_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Invalid_City:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Invalid_Group:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Invalid_Tariff:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Tariff_Not_Available:
                    rtResultType = ResultType.Result_Error_Tariffs_Not_Available;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_InvalidExternalProvider:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_OperationAlreadyExist:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_CrossSourceExtensionNotPossible:
                    rtResultType = ResultType.Result_Error_CrossSourceExtensionNotPossible;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_RestrictedTariff:
                    rtResultType = ResultType.Result_Error_RestrictedTariff;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_Tariff_Not_Required:
                    rtResultType = ResultType.Result_Error_Tariff_Not_Required;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_PlateInBlackList:
                    rtResultType = ResultType.Result_Error_PlateInBlackList;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_NoParkingRequired_ZeroEmissions:
                    rtResultType = ResultType.Result_Error_NoParkingRequired_ZeroEmissions;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_ParkingRefused_PollutionEpisode:
                    rtResultType = ResultType.Result_Error_ParkingRefused_PollutionEpisode;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_PermitNotInRenewalPeriod:
                    rtResultType = ResultType.Result_Error_PermitNotInRenewalPeriod;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_PermitAlreadyExist:
                    rtResultType = ResultType.Result_Error_PermitAlreadyExist;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_InvalidOperationId:
                    rtResultType = ResultType.Result_Error_OperationNotFound;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_PermitMonthNotAllowed:
                    rtResultType = ResultType.Result_Error_PermitMonthNotAllowed;
                    break;               
                case ResultTypeStandardParkingWS.ResultSP_Error_RestrictedTariffWithSpecificMessage:
                    rtResultType = ResultType.Result_Error_RestrictedTariffWithSpecificMessage;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_ParkingRefused_WithoutCategory:
                    rtResultType = ResultType.Result_Error_ParkingRefused_WithoutCategory;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_MaxParkingOpsByUser:
                    rtResultType = ResultType.Result_Error_ParkingRefused_MaxParkingOpsByUser;
                    break;
                case ResultTypeStandardParkingWS.ResultSP_Error_MaxParkingOpsInPeriod:
                    rtResultType = ResultType.Result_Error_ParkingRefused_MaxParkingOpsInPeriod;
                    break;
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_ResultTypeBilbaoIntegrationParkingWS_TO_ResultType(ResultTypeBilbaoIntegrationParkingWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_InvalidAuthenticationHash:
                    rtResultType = ResultType.Result_Error_InvalidAuthenticationHash;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_ParkingMaximumTimeUsed:
                    rtResultType = ResultType.Result_Error_ParkingMaximumTimeUsed;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_NotWaitedReentryTime:
                    rtResultType = ResultType.Result_Error_NotWaitedReentryTime;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Generic:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Invalid_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Missing_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Invalid_City:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Invalid_Group:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Invalid_Tariff:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_Tariff_Not_Available:
                    rtResultType = ResultType.Result_Error_Tariffs_Not_Available;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_InvalidExternalProvider:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeBilbaoIntegrationParkingWS.ResultSP_Error_CrossSourceExtensionNotPossible:
                    rtResultType = ResultType.Result_Error_CrossSourceExtensionNotPossible;
                    break;
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_ResultTypePICWS_TO_ResultType(ResultTypePICWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypePICWS.ResultPICWS_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_InvalidAuthenticationHash:
                    rtResultType = ResultType.Result_Error_InvalidAuthenticationHash;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Generic:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Missing_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_City:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_Group:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_Unit:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_AlarmType:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_InvalidExternalProvider:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_Invalid_Status:
                    rtResultType = ResultType.Result_Error_Invalid_Status;
                    break;
                case ResultTypePICWS.ResultPICWS_Error_CollectingAlreadyExists:
                    rtResultType = ResultType.Result_Error_CollectingAlreadyExists;
                    break;                                        
                case ResultTypePICWS.ResultPICWS_Error_TicketPaymentAlreadyExist:
                    rtResultType = ResultType.Result_Error_TicketPaymentAlreadyExist;
                    break;                    
                case ResultTypePICWS.ResultPICWS_Error_TicketNotPayable:
                    rtResultType = ResultType.Result_Error_Fine_Type_Not_Payable;
                    break;                    
                case ResultTypePICWS.ResultPICWS_Error_InvalidUserOrPassword:
                    rtResultType = ResultType.Result_Error_InvalidAuthentication;
                    break;

                default:
                    break;
            }


            return rtResultType;
        }
     
        protected ResultType Convert_ResultTypeStandardFineWS_TO_ResultType(ResultTypeStandardFineWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeStandardFineWS.Ok:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeStandardFineWS.InvalidAuthenticationHashExternalService:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.InvalidAuthenticationExternalService:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.InvalidAuthentication:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.TicketNotFound:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;                    
                case ResultTypeStandardFineWS.TicketNumberNotFound:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeStandardFineWS.TicketTypeNotPayable:
                    rtResultType = ResultType.Result_Error_Fine_Type_Not_Payable;
                    break;
                case ResultTypeStandardFineWS.TicketPaymentPeriodExpired:
                    rtResultType = ResultType.Result_Error_Fine_Payment_Period_Expired;
                    break;
                case ResultTypeStandardFineWS.TicketAlreadyCancelled:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeStandardFineWS.TicketAlreadyAnulled:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeStandardFineWS.TicketAlreadyRemitted:
                    rtResultType = ResultType.Result_Error_Fine_Payment_Period_Expired;
                    break;
                case ResultTypeStandardFineWS.TicketAlreadyPaid:
                    rtResultType = ResultType.Result_Error_Fine_Number_Already_Paid;
                    break;
                case ResultTypeStandardFineWS.TicketNotClosed:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeStandardFineWS.InvalidPaymentAmount:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;                
                case ResultTypeStandardFineWS.InstallationNotFound:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.InvalidExternalProvider:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;                                               
                case ResultTypeStandardFineWS.InvalidAuthenticationHash:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.ErrorDetected:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeStandardFineWS.GenericError:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                default:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
            }


            return rtResultType;
        }


        protected ResultType Convert_ResultTypeSantBoiFineWS_TO_ResultType(ResultTypeSantBoiFineWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeSantBoiFineWS.Ok:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeSantBoiFineWS.TicketNotFound:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                case ResultTypeSantBoiFineWS.TicketAlreadyPaid:
                    rtResultType = ResultType.Result_Error_Fine_Number_Already_Paid;
                    break;
                case ResultTypeSantBoiFineWS.GenericError:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                default:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
            }

            return rtResultType;
        }

        protected ResultType Convert_ResultTypeMeyparOffstreetWS_TO_ResultType(ResultTypeMeyparOffstreetWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Generic:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Invalid_Id:
                    rtResultType = ResultType.Result_Error_Invalid_Id;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Invalid_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Missing_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_OperationNotFound:
                    rtResultType = ResultType.Result_Error_OperationNotFound;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_OperationAlreadyClosed:
                    rtResultType = ResultType.Result_Error_OperationAlreadyClosed;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Max_Multidiscount_Reached:
                    rtResultType = ResultType.Result_Error_Max_Multidiscount_Reached;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_Discount_NotAllowed:
                    rtResultType = ResultType.Result_Error_Discount_NotAllowed;
                    break;
                case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_InvoiceGeneration:
                    rtResultType = ResultType.Result_Error_Offstreet_InvoiceGeneration;
                    break;
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_ResultTypeMeyparAdventaOffstreetWS_TO_ResultType(ResultTypeMeyparAdventaOffstreetWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeMeyparAdventaOffstreetWS.Ok:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Error_Generico:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Soporte_Invalido:
                    rtResultType = ResultType.Result_Error_Invalid_Id;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Soporte_No_Encontrado:
                    rtResultType = ResultType.Result_Error_OperationNotFound;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Parametros_Entrada_Incorrectos:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Faltan_Parametros_Entrada:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypeMeyparAdventaOffstreetWS.Operacion_No_Encontrada:
                    rtResultType = ResultType.Result_Error_OperationNotFound;
                    break;
                //case ResultTypeMeyparAdventaOffstreetWS.ResultMOffstreet_Error_OperationAlreadyClosed:
                //    rtResultType = ResultType.Result_Error_OperationAlreadyClosed;
                //    break;
                //case ResultTypeMeyparAdventaOffstreetWS..ResultMOffstreet_Error_Max_Multidiscount_Reached:
                //    rtResultType = ResultType.Result_Error_Max_Multidiscount_Reached;
                //    break;
                case ResultTypeMeyparAdventaOffstreetWS.Descuento_No_Encontrado:
                    rtResultType = ResultType.Result_Error_Discount_NotAllowed;
                    break;
                //case ResultTypeMeyparOffstreetWS.ResultMOffstreet_Error_InvoiceGeneration:
                //    rtResultType = ResultType.Result_Error_Offstreet_InvoiceGeneration;
                //    break;
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_ResultTypeIParkControlOffstreetWS_TO_ResultType(ResultTypeIParkControlOffstreetWS oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case ResultTypeIParkControlOffstreetWS.Result_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InvalidAuthentication:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_PlatePaymentBlacklist:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_UnkownParkingOperation:
                    rtResultType = ResultType.Result_Error_Invalid_Id;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_ParkingOperationExit:
                    rtResultType = ResultType.Result_Error_OperationAlreadyClosed;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_ParkingOperationMissmatched:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_ParkingOperationPaid:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_TariffNotFound:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_ParkingOpQueryExpired:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_PlatePaymentWithSpecialGrants:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_GroupNotFound:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InstallationNotFound:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InvalidExternalProvider:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_PaymentError:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_PaymentDenied:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_CurrencyNotFound:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_CouponUnknown:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_CouponAlreadyUsed:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_CouponCanNotBeApplied:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_CouponsMissmatched:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InstallationServicesMissmatched:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InvalidInputParameters:
                     rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_MissingInputParameters:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_InvalidAuthenticationHash:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_ErrorDetected:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;
                case ResultTypeIParkControlOffstreetWS.Result_Error_Generic:
                     rtResultType = ResultType.Result_Error_Generic;
                    break;            
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_MadridPlatformAuthResult_TO_ResultType(int iAuthResult)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (iAuthResult)
            {
                case 0: rtResultType = ResultType.Result_OK; break;                                     // Autoricado
                case 1: rtResultType = ResultType.Result_Error_Fine_Payment_Period_Expired; break;      // Fuera de periodo
                case 2: rtResultType = ResultType.Result_Error_Fine_Type_Not_Payable; break;            // No anulable
                case 3: rtResultType = ResultType.Result_Error_Fine_Number_Not_Found; break;            // No existe
                case 4: rtResultType = ResultType.Result_Error_Fine_Number_Already_Paid; break;         // Ya abonada
                case 5: rtResultType = ResultType.Result_Error_Fine_Number_Not_Found; break;            // Invalidada
                case 6: rtResultType = ResultType.Result_Error_Generic; break;                          // Error servidor
                case 99: rtResultType = ResultType.Result_Error_Generic; break;                         // Excepción servidor
                case 100: rtResultType = ResultType.Result_Error_Generic; break;                         // Servidor fuera de línea
                case 103: rtResultType = ResultType.Result_Error_Generic; break;                         // Rechazado por protocolo de contaminación activado
            }

            return rtResultType;
        }

        protected ResultType Convert_ResultTypePagateliaWS_TO_ResultType(short iExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (iExtResultType)
            {
                case 1:
                    rtResultType = ResultType.Result_OK;
                    break;
                case -9:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case -19:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case -20:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case -26:
                    rtResultType = ResultType.Result_Error_Invalid_User;
                    break;
                case -27:
                    rtResultType = ResultType.Result_Error_User_Not_Logged;
                    break;
                case -29:
                    rtResultType = ResultType.Result_Error_Invalid_Payment_Mean;
                    break;
                case -30:
                    rtResultType = ResultType.Result_Error_Invalid_Recharge_Code;
                    break;
                case -40:
                    rtResultType = ResultType.Result_Error_UserAccountBlocked;
                    break;
                case -41:
                    rtResultType = ResultType.Result_Error_UserAccountNotAproved;
                    break;
                case -50:
                    rtResultType = ResultType.Result_Error_UserBalanceNotEnough;
                    break;
                case -51:
                    rtResultType = ResultType.Result_Error_UserAmountDailyLimitReached;
                    break;
                case -60:
                    rtResultType = ResultType.Result_Error_AmountNotValid;
                    break;
                default:
                    break;
            }


            return rtResultType;
        }

        protected ResultType Convert_ResultTypeMifasEsExpedienteAnulableWS_TO_ResultType(Int32 oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                //1 Expediente existente i anulable
                case 1:
                    rtResultType = ResultType.Result_OK;
                    break;
                //0 Expediente no existente
                case 0:
                    rtResultType = ResultType.Result_Error_Fine_Number_Not_Found;
                    break;
                //-1 Error en la comprobación
                case -1:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                //-2 Expediente no anulable
                case -2:
                    rtResultType = ResultType.Result_Error_Fine_Type_Not_Payable;
                    break;
                //-3 Expediente ya anulado
                case -3:
                    rtResultType = ResultType.Result_Error_Fine_Number_Already_Paid;
                    break;
                default:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
            }

            return rtResultType;
        }

        protected ResultType Convert_ResultTypeEmisalba_TO_ResultType(int iError, string sErrorMessage)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            if (iError == 0)
                rtResultType = ResultType.Result_OK;
            else
            {
                // Check sErrorMessage ??
                rtResultType = ResultType.Result_Error_Generic;
            }

            return rtResultType;
        }

        protected int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, INSTALLATION oInstallation, USER oUser, out double dChangeApplied, out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeApplied = 1;
            dChangeFee = 0;


            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    int iFactor = infraestructureRepository.GetCurrenciesFactorDifference((int)oInstallation.INS_CUR_ID, (int)oUser.USR_CUR_ID);

                    double dConvertedValue = CCurrencyConvertor.ConvertCurrency(Convert.ToDouble(iQuantity),
                                              oInstallation.CURRENCy.CUR_ISO_CODE,
                                              oUser.CURRENCy.CUR_ISO_CODE, out dChangeApplied);
                    if (dConvertedValue < 0)
                    {
                        Logger_AddLogMessage(string.Format("ChangeQuantityFromInstallationCurToUserCur::Error Converting {0} {1} to {2} ", iQuantity, oInstallation.CURRENCy.CUR_ISO_CODE, oUser.CURRENCy.CUR_ISO_CODE), LogLevels.logERROR);
                        return ((int)ResultType.Result_Error_Generic);
                    }
                    dConvertedValue = dConvertedValue * Math.Pow(10, (double)iFactor);
                    dChangeFee = Convert.ToDouble(infraestructureRepository.GetChangeFeePerc()) * dConvertedValue / 100;
                    iResult = Convert.ToInt32(dConvertedValue - dChangeFee + 0.5);
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }

        protected int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, INSTALLATION oInstallation, USER oUser)
        {
            int iResult = iQuantity;
            double dChangeApplied = 1;
            double dChangeFee = 0;


            try
            {

                iResult = ChangeQuantityFromInstallationCurToUserCur(iQuantity, oInstallation, oUser, out dChangeApplied, out dChangeFee);


            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }




        protected int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, double dChangeToApply, INSTALLATION oInstallation, USER oUser, out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeFee = 0;

            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    int iFactor = infraestructureRepository.GetCurrenciesFactorDifference((int)oInstallation.INS_CUR_ID, (int)oUser.USR_CUR_ID);
                    double dConvertedValue = Convert.ToDouble(iQuantity) * dChangeToApply;
                    dConvertedValue = dConvertedValue * Math.Pow(10, (double)iFactor);

                    dConvertedValue = Math.Round(dConvertedValue, 4);

                    dChangeFee = Convert.ToDouble(infraestructureRepository.GetChangeFeePerc()) * dConvertedValue / 100;
                    iResult = Convert.ToInt32(Math.Round(dConvertedValue - dChangeFee, MidpointRounding.AwayFromZero));
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }


        protected int ChangeQuantityFromInstallationCurToUserCur(int iQuantity, double dChangeToApply, decimal dINSCurID, decimal dUserCurID , out double dChangeFee)
        {
            int iResult = iQuantity;
            dChangeFee = 0;

            try
            {

                if (dINSCurID != dUserCurID)
                {
                    int iFactor = infraestructureRepository.GetCurrenciesFactorDifference((int)dINSCurID, (int)dUserCurID);                  
                    double dConvertedValue = Convert.ToDouble(iQuantity) * dChangeToApply;
                    dConvertedValue = dConvertedValue * Math.Pow(10, (double)iFactor);

                    dConvertedValue = Math.Round(dConvertedValue, 4);

                    decimal dChangeFeePerc = infraestructureRepository.GetChangeFeePerc();
                    dChangeFee = Convert.ToDouble(dChangeFeePerc) * dConvertedValue / 100;
                    iResult = Convert.ToInt32(Math.Round(dConvertedValue - dChangeFee, MidpointRounding.AwayFromZero));
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "ChangeQuantityFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return iResult;
        }



        protected double GetChangeToApplyFromInstallationCurToUserCur(INSTALLATION oInstallation, USER oUser)
        {
            double dResult = 1.0;


            try
            {

                if (oInstallation.INS_CUR_ID != oUser.USR_CUR_ID)
                {
                    dResult = CCurrencyConvertor.GetChangeToApply(oInstallation.CURRENCy.CUR_ISO_CODE,
                                              oUser.CURRENCy.CUR_ISO_CODE);
                    if (dResult < 0)
                    {
                        Logger_AddLogMessage(string.Format("GetChangeToApplyFromInstallationCurToUserCur::Error getting change from {0} to {1} ", oInstallation.CURRENCy.CUR_ISO_CODE, oUser.CURRENCy.CUR_ISO_CODE), LogLevels.logERROR);
                        return ((int)ResultType.Result_Error_Generic);
                    }
                }

            }
            catch (Exception e)
            {
                dResult = -1.0;
                Logger_AddLogException(e, "GetChangeToApplyFromInstallationCurToUserCur::Exception", LogLevels.logERROR);
            }

            return dResult;
        }

        protected string PrettyXml(string xml)
        {

            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }

        #region Madrid definitions

        protected bool MadridPlatfomStartSession(MadridPlatform.PublishServiceV12Client oService, out MadridPlatform.AuthSession oAuthSession, int? iWSTimeout, out long lEllapsedTime)
        {
            bool bRet = false;
            oAuthSession = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;
            watch = Stopwatch.StartNew();
            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();

            try
            {
                lock (m_oLocker)
                {
                    AddTLS12Support();
                    /*if (m_oMadridPlatformAuthSession == null)
                    {
                        Logger_AddLogMessage("MadridPlatfomStartSession - Starting session ... ", LogLevels.logDEBUG);
                        MadridPlatform.AuthLoginResponse oResponse = null;
                        int iRetry = 0;
                        while ((oResponse == null || oResponse.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK) && iRetry < 3)
                        {
                            if (iRetry > 0) Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Retrying start session {0} ...", iRetry), LogLevels.logWARN);
                            oResponse = oService.startSession(oService.ClientCredentials.UserName.UserName, oService.ClientCredentials.UserName.Password, "es");
                            iRetry += 1;
                        }
                        if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                        {
                            bRet = true;
                            m_oMadridPlatformAuthSession = oResponse.Result;
                            m_dtMadridPlatformStartSession = DateTime.UtcNow;
                            oAuthSession = oResponse.Result;
                            m_iMadridPlatformOpenSessions += 1;
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session started successfully: Status='{0}', sessionId='{1}', userName='{2}'", oResponse.Status.ToString(), m_oMadridPlatformAuthSession.sessionId, m_oMadridPlatformAuthSession.userName), LogLevels.logDEBUG);
                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Error starting session: Status='{0}', errorDetails='{1}'", oResponse.Status.ToString(), oResponse.errorDetails), LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        bRet = true;
                        oAuthSession = m_oMadridPlatformAuthSession;
                        m_iMadridPlatformOpenSessions += 1;
                        Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session reused: sessionId='{0}', userName='{1}'", m_oMadridPlatformAuthSession.sessionId, m_oMadridPlatformAuthSession.userName), LogLevels.logDEBUG);
                    }
                    Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Sessions count: {0}", m_iMadridPlatformOpenSessions), LogLevels.logDEBUG);*/



                    if (m_oMadridPlatformAuthSession == null)
                    {
                        Logger_AddLogMessage("MadridPlatfomStartSession - Starting session ... ", LogLevels.logDEBUG);
                        MadridPlatform.AuthLoginResponse oResponse = null;
                        int iRetry = 0;
                        while ((oResponse == null || oResponse.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK) && iRetry < 3)
                        {
                            oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout-(int)watch.ElapsedMilliseconds);
                           
                            if (iRetry > 0) Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Retrying start session {0} ...", iRetry), LogLevels.logWARN);
                            oResponse = oService.startSession(oService.ClientCredentials.UserName.UserName, oService.ClientCredentials.UserName.Password, "es");
                            iRetry += 1;
                        }
                        if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                        {
                            bRet = true;
                            m_oMadridPlatformAuthSession = oResponse.Result;
                            m_dtMadridPlatformStartSession = DateTime.UtcNow;
                            oAuthSession = oResponse.Result;
                            m_iMadridPlatformOpenSessions += 1;
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session started successfully: Status='{0}', sessionId='{1}', userName='{2}'", oResponse.Status.ToString(), m_oMadridPlatformAuthSession.sessionId, m_oMadridPlatformAuthSession.userName), LogLevels.logDEBUG);
                        }
                        else
                        {
                            string sErrorInfo = string.Format("MadridPlatfomStartSession - Error starting session: Status='{0}', errorDetails='{1}', authError='{2}'", oResponse.Status.ToString(), oResponse.errorDetails);
                            if (oResponse.authError.HasValue)
                                sErrorInfo = string.Format("{0}, authError='{1}'", sErrorInfo, oResponse.authError.Value.ToString());
                            Logger_AddLogMessage(sErrorInfo, LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        if (m_dtMadridPlatformStartSession.HasValue && m_dtMadridPlatformStartSession < DateTime.UtcNow.AddDays(-1))
                        {
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session expired: sessionId='{0}', userName='{1}'", m_oMadridPlatformAuthSession.sessionId, m_oMadridPlatformAuthSession.userName), LogLevels.logDEBUG);
                            MadridPlatfomEndSession(oService, m_oMadridPlatformAuthSession, iWSLocalTimeout - (int)watch.ElapsedMilliseconds);
                            bRet = this.MadridPlatfomStartSession(oService, out oAuthSession,iWSTimeout, out lEllapsedTime);
                        }
                        else
                        {
                            bRet = true;
                            oAuthSession = m_oMadridPlatformAuthSession;
                            //m_iMadridPlatformOpenSessions += 1;
                            Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Session reused: sessionId='{0}', userName='{1}'", m_oMadridPlatformAuthSession.sessionId, m_oMadridPlatformAuthSession.userName), LogLevels.logDEBUG);
                        }
                    }
                    Logger_AddLogMessage(string.Format("MadridPlatfomStartSession - Sessions count: {0}", m_iMadridPlatformOpenSessions), LogLevels.logDEBUG);
                }
            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "MadridPlatfomStartSession::Exception", LogLevels.logERROR);
            }

            lEllapsedTime = watch.ElapsedMilliseconds;

            return bRet;
        }

        protected bool MadridPlatfomEndSession(MadridPlatform.PublishServiceV12Client oService, MadridPlatform.AuthSession oAuthSession,int iWSTimeout)
        {
            bool bRet = false;


            lock (m_oLocker)
            {
                bool bClosing = false;          
                try
                {
                    AddTLS12Support();
                    /*if (m_iMadridPlatformOpenSessions <= 1)
                    {
                        Logger_AddLogMessage("MadridPlatfomEndSession - Ending session ... ", LogLevels.logDEBUG);
                        m_iMadridPlatformOpenSessions = 0;
                        bClosing = true;
                        var oResponse = oService.endSession(oAuthSession);
                        if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                        {
                            Logger_AddLogMessage("MadridPlatfomEndSession - Session ended successfully.", LogLevels.logDEBUG);
                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Error ending session: Status='{0}', errorDetails='{1}'", oResponse.Status.ToString(), oResponse.errorDetails), LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        m_iMadridPlatformOpenSessions -= 1;
                        bRet = true;
                    }
                    Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Sessions count: {0}", m_iMadridPlatformOpenSessions), LogLevels.logDEBUG);*/

                    //if (m_iMadridPlatformOpenSessions <= 1)
                    //{
                    Logger_AddLogMessage("MadridPlatfomEndSession - Ending session ... ", LogLevels.logDEBUG);
                    m_iMadridPlatformOpenSessions = 0;
                    bClosing = true;
                    oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSTimeout);
                    var oResponse = oService.endSession(oAuthSession);
                    if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                    {
                        Logger_AddLogMessage("MadridPlatfomEndSession - Session ended successfully.", LogLevels.logDEBUG);
                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Error ending session: Status='{0}', errorDetails='{1}'", oResponse.Status.ToString(), oResponse.errorDetails), LogLevels.logERROR);
                    }
                    /*}
                    else
                    {
                        m_iMadridPlatformOpenSessions -= 1;
                        bRet = true;
                    }*/
                    Logger_AddLogMessage(string.Format("MadridPlatfomEndSession - Sessions count: {0}", m_iMadridPlatformOpenSessions), LogLevels.logDEBUG);

                }
                catch (Exception ex)
                {
                    Logger_AddLogException(ex, "MadridPlatfomEndSession::Exception", LogLevels.logERROR);
                }
                finally
                {
                    if (bClosing)
                    {
                        bRet = true;
                        m_oMadridPlatformAuthSession = null;
                    }
                }
            }
            return bRet;
        }

        protected bool MadridPlatfomEndSession()
        {
            bool bRet = false;

            lock (m_oLocker)
            {
                m_oMadridPlatformAuthSession = null;
                m_dtMadridPlatformStartSession = null;
                m_iMadridPlatformOpenSessions = 0;
                Logger_AddLogMessage("MadridPlatfomEndSession - Expirated session closed.", LogLevels.logDEBUG);
            }
            return bRet;
        }

        protected bool MadridToken(string sUrl, string sClientId, string sClientSecret, out string sAccessToken)
        {
            bool bRet = false;
            sAccessToken = "";

            try
            {
                lock (m_oMadridLocker)
                {
                    if (!m_iMadridTokenExpirationSeconds.HasValue)
                    {
                        m_iMadridTokenExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["MadridTokenExpirationSeconds"] ?? "1440");
                    }

                    if (string.IsNullOrEmpty(m_sMadridAccessToken) ||
                        (m_dtMadridAccessTokenStart.HasValue && m_dtMadridAccessTokenStart.Value < DateTime.UtcNow.AddSeconds(-m_iMadridTokenExpirationSeconds.Value)))
                    {
                        m_sMadridAccessToken = null;

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            ((sender, certificate, chain, sslPolicyErrors) => true);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        Encoding encoding = new UTF8Encoding();
                        string sParametersIn = "grant_type=client_credentials";
                        sParametersIn += "&client_id=" + sClientId;
                        sParametersIn += "&client_secret=" + sClientSecret;

                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/x-www-form-urlencoded";
                        oRequest.Timeout = Get3rdPartyWSTimeout();

                        Logger_AddLogMessage(string.Format("MadridToken request.url={0}, request.body={1}", sUrl, sParametersIn), LogLevels.logINFO);


                        byte[] byteArray = Encoding.UTF8.GetBytes(sParametersIn);

                        oRequest.ContentLength = byteArray.Length;
                        Stream dataStream = oRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        try
                        {

                            WebResponse response = oRequest.GetResponse();
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            if (oWebResponse.StatusDescription == "OK")
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("MadridToken response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                sAccessToken = oResponse["access_token"];
                                m_sMadridAccessToken = sAccessToken;
                                m_dtMadridAccessTokenStart = DateTime.UtcNow;
                                if (oResponse["expires_in"] != null)
                                {
                                    m_iMadridTokenExpirationSeconds = Convert.ToInt32(oResponse["expires_in"]);
                                }

                                bRet = true;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (Exception e)
                        {
                            bRet = false;
                            Logger_AddLogException(e, "MadridToken::Exception", LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        sAccessToken = m_sMadridAccessToken;
                        bRet = true;
                    }

                }
            }
            catch (Exception e)
            {
                bRet = false;
                Logger_AddLogException(e, "MadridToken::Exception", LogLevels.logERROR);
            }

            return bRet;
        }

        // ***
        public bool Madrid2AllowedZone(GROUP oGroup, int iWSNumber)
        {
            bool bRet = false;

            if (oGroup != null)
            {
                var oPhyZones = (ConfigurationManager.AppSettings["Madrid2AllowedZones"] ?? "").Split(',').ToList().Select(z => z.Trim());

                string sExtId = "";
                switch (iWSNumber)
                {
                    case 0: sExtId = oGroup.GRP_QUERY_EXT_ID; break;
                    case 1: sExtId = oGroup.GRP_EXT1_ID; break;
                    case 2: sExtId = oGroup.GRP_EXT2_ID; break;
                    case 3: sExtId = oGroup.GRP_EXT3_ID; break;
                    case 4: sExtId = oGroup.GRP_ID_FOR_EXT_OPS; break;
                }
                bRet = oPhyZones.Any(z => z == sExtId);
            }

            return bRet;
        }
        public bool Madrid2AllowedZone(string sGroupExtId)
        {
            bool bRet = false;

            var oPhyZones = (ConfigurationManager.AppSettings["Madrid2AllowedZones"] ?? "").Split(',').ToList().Select(z => z.Trim());
            
            bRet = oPhyZones.Any(z => z == sGroupExtId);
            
            return bRet;
        }
        public void Madrid2Params(out string sUrl, out string sClientId, out string sClientSecret, out string sTokenUrl, out string sCodSystem, out string sCodGeoZone, out string sCodCity)
        {
            sUrl = ConfigurationManager.AppSettings["Madrid2Url"] ?? "https://sermobile.madmovilidad.es/mobilegeneralagg/api/v1";
            sClientId = ConfigurationManager.AppSettings["Madrid2ClientId"] ?? "_Blinkay98_";
            sClientSecret = ConfigurationManager.AppSettings["Madrid2ClientSecret"] ?? "8dd8d979-3476-db30-7315-e2120f97b958";
            sTokenUrl = ConfigurationManager.AppSettings["Madrid2TokenUrl"] ?? "https://seridentity.madmovilidad.es/connect/token";
            sCodSystem = ConfigurationManager.AppSettings["CodeSystem"] ?? "0001";
            sCodGeoZone = ConfigurationManager.AppSettings["CodeGeoZone"] ?? "01";
            sCodCity = ConfigurationManager.AppSettings["CodeCity"] ?? "01";
        }
        // ***

        protected string TimeSpanToString(TimeSpan oTimeSpan)
        {
            string sRet = XmlConvert.ToString(oTimeSpan);

            try
            {
                //PT11H41M57S
                //PT20M
                if (sRet.StartsWith("PT"))
                {
                    int iH = 0;
                    int iM = 0;
                    int iS = 0;
                    int iPos = 1;
                    int iPos2 = 0;
                    if (sRet.Contains("H"))
                    {
                        iPos = sRet.IndexOf("H");
                        iH = Convert.ToInt32(sRet.Substring(2, iPos - 2));
                    }
                    if (sRet.Contains("M"))
                    {
                        iPos2 = sRet.IndexOf("M");
                        iM = Convert.ToInt32(sRet.Substring(iPos + 1, iPos2 - (iPos + 1)));
                        iPos = iPos2;
                    }
                    if (sRet.Contains("S"))
                    {
                        iPos2 = sRet.IndexOf("S");
                        iS = Convert.ToInt32(sRet.Substring(iPos + 1, iPos2 - (iPos + 1)));
                    }
                    sRet = string.Format("PT{0:00}H{1:00}M{2:00}S", iH, iM, iS);
                }
            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "TimeSpanToString::Exception", LogLevels.logERROR);
            }

            return sRet;
        }

        #endregion

        protected bool BSMToken(string sBaseUrl, string sUserKey, string sUserPwd, int? iWSTimeout, out string sAccessToken , out long lEllapsedTime)
        {
            bool bRet = false;
            sAccessToken = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            try
            {
                lock (m_oBSMLocker)
                {
                    if (!m_iBSMTokenExpirationSeconds.HasValue)
                    {
                        m_iBSMTokenExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["BSMTokenExpirationSeconds"] ?? "3600");
                    }

                    if (string.IsNullOrEmpty(m_sBSMAccessToken) ||
                        (m_dtBSMAccessTokenStart.HasValue && m_dtBSMAccessTokenStart.Value < DateTime.UtcNow.AddSeconds(-m_iBSMTokenExpirationSeconds.Value)))
                    {
                        m_sBSMAccessToken = null;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                            ((sender, certificate, chain, sslPolicyErrors) => true);

                        string sUrl = sBaseUrl.Remove(sBaseUrl.IndexOf("api/") + 3) + "/token";
                        WebRequest oRequest = WebRequest.Create(sUrl);
                        String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(sUserKey + ":" + sUserPwd));
                        oRequest.Headers.Add("Authorization", "Basic " + encoded);

                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/x-www-form-urlencoded";                        
                        oRequest.Timeout = iWSTimeout??Get3rdPartyWSTimeout();

                        string sParametersIn = "grant_type=client_credentials";

                        Logger_AddLogMessage(string.Format("BSMToken request.url={0}, request.body={1}", sUrl, sParametersIn), LogLevels.logINFO);


                        byte[] byteArray = Encoding.UTF8.GetBytes(sParametersIn);

                        oRequest.ContentLength = byteArray.Length;
                        watch = Stopwatch.StartNew();
                        Stream dataStream = oRequest.GetRequestStream();                        
                        dataStream.Write(byteArray, 0, byteArray.Length);                        
                        dataStream.Close();                        

                        try
                        {

                            WebResponse response = oRequest.GetResponse();                            
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            lEllapsedTime = watch.ElapsedMilliseconds;
                            if (oWebResponse.StatusDescription == "OK")
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("BSMToken response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                sAccessToken = oResponse["access_token"];
                                m_sBSMAccessToken = sAccessToken;
                                m_dtBSMAccessTokenStart = DateTime.UtcNow;
                                if (oResponse["expires_in"] != null)
                                {
                                    m_iBSMTokenExpirationSeconds = Convert.ToInt32(oResponse["expires_in"]);
                                }

                                bRet = true;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (Exception e)
                        {
                            if ((watch != null) && (lEllapsedTime == 0))
                            {
                                lEllapsedTime = watch.ElapsedMilliseconds;
                            }
                            bRet = false;
                            Logger_AddLogException(e, "BSMToken::Exception", LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        sAccessToken = m_sBSMAccessToken;
                        bRet = true;
                    }

                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                bRet = false;
                Logger_AddLogException(e, "BSMToken::Exception", LogLevels.logERROR);
            }

            return bRet;
        }

        protected bool PermitsToken(string sBaseUrl, string sUserKey, string sUserPwd, out string sAccessToken)
        {
            bool bRet = false;
            sAccessToken = "";

            try
            {
                lock (m_oPermitsLocker)
                {
                    if (!m_iPermitsTokenExpirationSeconds.HasValue)
                    {
                        m_iPermitsTokenExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["PermitsTokenExpirationSeconds"] ?? "86400");
                    }

                    if (string.IsNullOrEmpty(m_sPermitsAccessToken) ||
                        (m_dtPermitsAccessTokenStart.HasValue && m_dtPermitsAccessTokenStart.Value < DateTime.UtcNow.AddSeconds(-m_iPermitsTokenExpirationSeconds.Value)))
                    {
                        m_sPermitsAccessToken = null;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        string sUrl = sBaseUrl + (!sBaseUrl.EndsWith("/") ? "/" : "") + "login";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                        string json = $"{{\"user\": \"{sUserKey}\", \"password\": \"{sUserPwd}\"}}";
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        request.Accept = "application/json";
                        request.Timeout = Get3rdPartyWSTimeout();

                        Logger_AddLogMessage(string.Format("PermitsToken request.url={0}, request.body={1}", sBaseUrl, json), LogLevels.logINFO);

                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        try
                        {
                            using (WebResponse response = request.GetResponse())
                            {
                                using (Stream strReader = response.GetResponseStream())
                                {
                                    if (strReader == null)
                                    {
                                        sAccessToken = string.Empty;
                                    }
                                    using (StreamReader objReader = new StreamReader(strReader))
                                    {
                                        string responseBody = objReader.ReadToEnd();

                                        Logger_AddLogMessage(string.Format("PermitsToken response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                        dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                        if (oResponse.GetType() == typeof(string))
                                        {
                                            sAccessToken = (string)oResponse;
                                            m_sPermitsAccessToken = sAccessToken;
                                            m_dtPermitsAccessTokenStart = DateTime.UtcNow;

                                            bRet = true;
                                        }
                                        else
                                        {
                                            sAccessToken = string.Empty;
                                            bRet = false;
                                        }
                                    }
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            sAccessToken = string.Empty;
                            bRet = false;
                            Logger_AddLogException(ex, "PermitsToken::Exception", LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        sAccessToken = m_sPermitsAccessToken;
                        bRet = true;
                    }
                }
            }
            catch (Exception e)
            {
                bRet = false;
                Logger_AddLogException(e, "PermitsToken::Exception", LogLevels.logERROR);
            }

            return bRet;
        }

        protected bool SIRToken(string sBaseUrl, string sUserKey, string sUserPwd, out string sAccessToken)
        {
            bool bRet = false;
            sAccessToken = "";

            try
            {
                lock (m_oSIRLocker)
                {
                    if (!m_iSIRTokenExpirationSeconds.HasValue)
                    {
                        m_iSIRTokenExpirationSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["SIRTokenExpirationSeconds"] ?? "1800");
                    }

                    if (string.IsNullOrEmpty(m_sSIRAccessToken) ||
                        (m_dtSIRAccessTokenStart.HasValue && m_dtSIRAccessTokenStart.Value < DateTime.UtcNow.AddSeconds(-m_iSIRTokenExpirationSeconds.Value)))
                    {
                        m_sSIRAccessToken = null;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        string sUrl = sBaseUrl + (!sBaseUrl.EndsWith("/") ? "/" : "") + "Login";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                        string json = $"{{\"user\": \"{sUserKey}\", \"pass\": \"{sUserPwd}\"}}";
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        request.Accept = "application/json";
                        request.Timeout = Get3rdPartyWSTimeout();

                        Logger_AddLogMessage(string.Format("SIRToken request.url={0}, request.body={1}", sUrl, json), LogLevels.logINFO);

                        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        try
                        {
                            using (WebResponse response = request.GetResponse())
                            {
                                using (Stream strReader = response.GetResponseStream())
                                {
                                    if (strReader == null)
                                    {
                                        sAccessToken = string.Empty;
                                    }
                                    using (StreamReader objReader = new StreamReader(strReader))
                                    {
                                        string responseBody = objReader.ReadToEnd();

                                        Logger_AddLogMessage(string.Format("SIRToken response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                        dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                        if (oResponse != null)
                                        {
                                            sAccessToken = oResponse["token"];                                            
                                            m_sSIRAccessToken = sAccessToken;
                                            m_dtSIRAccessTokenStart = DateTime.UtcNow;

                                            bRet = true;
                                        }
                                        else
                                        {
                                            sAccessToken = string.Empty;
                                            bRet = false;
                                        }
                                    }
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            sAccessToken = string.Empty;
                            bRet = false;
                            Logger_AddLogException(ex, "SIRToken::Exception", LogLevels.logERROR);
                        }
                    }
                    else
                    {
                        sAccessToken = m_sSIRAccessToken;
                        bRet = true;
                    }
                }
            }
            catch (Exception e)
            {
                bRet = false;
                Logger_AddLogException(e, "SIRToken::Exception", LogLevels.logERROR);
            }

            return bRet;
        }

        public static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }

        public static void AddTLS11Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls11) == 0) //Enable TLs 1.1
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls) == 0) //Enable TLs 1.0
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
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

    #region Emisalba definitions

    public class EmisalbaExternUser
    {
        public string user { get; set; }
        public string password { get; set; }
        public string empresa { get; set; }
    }

    public class EmisalbaRequest
    {
        public string action { get; set; }
        public EmisalbaRequestMsgBase message { get; set; }
    }

    public class EmisalbaRequestMsgBase
    {
        public EmisalbaExternUser ExternUser { get; set; }
    }

    public class EmisalbaResponse<T>
    {
        public string error { get; set; }
        public T message { get; set; }
    }

    public class EmisalbaResponseMsgBase
    {
        public string error { get; set; }
    }

    #endregion

    #region Permits definitions

    public class PermitsErrorResponse
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(PermitsErrorResponse));

        public string errorCode { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string[] errors { get; set; }
        public bool timeout { get; set; }

        public PermitsErrorResponse()
        {
            timeout = false;
        }

        public static PermitsErrorResponse Load(HttpWebResponse oResponse)
        {
            PermitsErrorResponse oRet = new PermitsErrorResponse();

            string sJsonResponse = "";

            try
            {
                using (var stream = oResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    sJsonResponse = reader.ReadToEnd();
                }
                oRet = JsonConvert.DeserializeObject<PermitsErrorResponse>(sJsonResponse);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "PermitsErrorResponse::Load Exception", ex);
                oRet.timeout = true;
            }

            if (oResponse != null)
            {
                try
                {
                    oRet.timeout = (oResponse.StatusCode == HttpStatusCode.RequestTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            m_oLog.LogMessage(LogLevels.logINFO, string.Format("PermitsErrorResponse::Load response={0}", PrettyJSON(sJsonResponse)));

            return oRet;
        }

        public ResultType GetResultType()
        {
            ResultType eRet = ResultType.Result_Error_Generic;

            if (this.errorCode == "error_15")
                eRet = ResultType.Result_Error_InvalidTicketId;
            // "error_152": "S'ha trobat un tiquet actiu"
            // "error_153": "NomÃ©s es pot reemborsar un mÃ xim del 100% de lâ€™import original"
            // "error_158": "Alguns dels parÃ metres obligatoris no estan informats"
            // "error_163": "El tiquet ja estÃ  parat"
            // "error_165": "MatrÃ­cula no vÃ lida",
            else if (this.errorCode == "error_213")
                //eRet = ResultType.Result_Error_Park_Min_Time_Not_Exceeded;
                eRet = ResultType.Result_Error_Generic;
            else if (this.timeout)
                //eRet = ResultType.Result_Error_BSM_Not_Answering;
                eRet = ResultType.Result_Error_Generic;

            return eRet;
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
    }

    #endregion

    #region SIR definitions

    public class SIRBase
    {
        public static DateTime? GetDateTimeFromStringFormat(string strDateTime)
        {
            DateTime? dt = null;
            try
            {
                if (!string.IsNullOrEmpty(strDateTime))
                {
                    if (strDateTime.Length == 24)
                        dt = DateTime.ParseExact(strDateTime, "yyyy-MM-ddTHH:mm:ss.fffZ",
                                                 System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                throw;
            }

            return dt;
        }

        public static string GetDateTimeStringFromDateFormat(DateTime? dtDateTime, string sFormat = "yyyy-MM-ddTHH:mm:ss.fffZ")
        {
            string sRet = null;
            if (dtDateTime.HasValue)
            {
                sRet = dtDateTime.Value.ToString(sFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            return sRet;
        }

    }

    public class SIRResponse<T> : SIRBase
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(SIRResponse<T>));

        public bool success { get; set; }
        public bool existsErrorMessages { get; set; }
        public SIRErrorMessage[] messages { get; set; }
        public T data { get; set; }

        public bool timeout { get; set; }

        public SIRResponse()
        {
            timeout = false;
        }

        public static SIRResponse<T> Load(HttpWebResponse oResponse)
        {
            SIRResponse<T> oRet = new SIRResponse<T>();

            string sJsonResponse = "";

            try
            {
                using (var stream = oResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    sJsonResponse = reader.ReadToEnd();
                }
                oRet = JsonConvert.DeserializeObject<SIRResponse<T>>(sJsonResponse);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "SIRResponse::Load Exception", ex);
                oRet.timeout = true;
            }

            if (oResponse != null)
            {
                try
                {
                    oRet.timeout = (oResponse.StatusCode == HttpStatusCode.RequestTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            m_oLog.LogMessage(LogLevels.logINFO, string.Format("SIRResponse::Load response={0}", PrettyJSON(sJsonResponse)));

            return oRet;
        }
        
        //public ResultType GetResultType()
        //{
        //    ResultType eRet = ResultType.Result_Error_Generic;
        //
        //    if (this.errorCode == "error_15")
        //        eRet = ResultType.Result_Error_InvalidTicketId;
        //    // "error_152": "S'ha trobat un tiquet actiu"
        //    // "error_153": "NomÃ©s es pot reemborsar un mÃ xim del 100% de lâ€™import original"
        //    // "error_158": "Alguns dels parÃ metres obligatoris no estan informats"
        //    // "error_163": "El tiquet ja estÃ  parat"
        //    // "error_165": "MatrÃ­cula no vÃ lida",
        //    else if (this.errorCode == "error_213")
        //        //eRet = ResultType.Result_Error_Park_Min_Time_Not_Exceeded;
        //        eRet = ResultType.Result_Error_Generic;
        //    else if (this.timeout)
        //        //eRet = ResultType.Result_Error_BSM_Not_Answering;
        //        eRet = ResultType.Result_Error_Generic;
        //
        //    return eRet;
        //}

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
    }

    public class SIRErrorMessage
    {
        public SIRException ex { get; set; }
        public string text { get; set; }
        public bool esError { get; set; }
        public object tag { get; set; }
    }

    public class SIRException
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }
    #endregion

}
