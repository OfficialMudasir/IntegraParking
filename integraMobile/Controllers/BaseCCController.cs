using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class BaseCCController : Controller
    {
        protected static readonly CLogWrapper m_Log = new CLogWrapper(typeof(BaseCCController));

        protected ICustomersRepository customersRepository;
        protected IInfraestructureRepository infraestructureRepository;

        protected System.Web.HttpServerUtilityBase _server = null;
        protected System.Web.HttpSessionStateBase _session = null;
        protected System.Web.HttpRequestBase _request = null;
        protected System.Web.HttpResponseBase _response = null;

        protected const long BIG_PRIME_NUMBER = 472189635;
        protected const long BIG_PRIME_NUMBER2 = 624159837;

        protected bool bAvoidHashCheck = false;
        protected decimal? Configuration_Id = null;


        /*
        [HttpPost]
        public ActionResult test()
        {
            if (_request == null) _request = Request;

            Logger_AddLogMessage(string.Format("Test: {0}",
                                       _request["r"].ToString()), LogLevels.logINFO);

            HttpResponse response = HttpContext.ApplicationInstance.Response;
            response.Clear();

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body>");
            s.AppendFormat("<form name='form' method='post'>");

            s.AppendFormat("<input type='hidden' name='r' value='{0}' />", _request["r"].ToString());

            s.Append("</form></body></html>");
            response.Write(s.ToString());
            response.End();
            return null;
        }
        */

        protected void RedirectWithData(string url, Dictionary<string, object> postData)
        {
            HttpResponse response = HttpContext.ApplicationInstance.Response;
            response.Clear();

            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", url);
            foreach (string key in postData.Keys)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", key, postData[key]);
            }
            s.Append("</form></body></html>");
            response.Write(s.ToString());
            response.End();
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


        protected string CalculateCryptResult(string strInput, string strHashSeed)
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

                strRes = ByteArrayToString(EncryptStringToBytes_Aes(strInput, _normKey, _iv));



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

        protected static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

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

        protected static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        protected static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        protected string PrettyJSON(string json)
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



        protected void SaveSession(string key)
        {
            Dictionary<string, string> oSessionDict = new Dictionary<string, string>();

            for (int i = 0; i < _session.Count; i++)
            {
                var crntSession = _session.Keys[i];

                Dictionary<string, string> oValueDict = new Dictionary<string, string>();

                if (_session[crntSession] == null)
                {
                    oValueDict["type"] = "null";
                }
                else
                {
                    oValueDict["type"] = _session[crntSession].GetType().ToString();
                }

                oValueDict["value"] = JsonConvert.SerializeObject(_session[crntSession]);

                oSessionDict[crntSession] = JsonConvert.SerializeObject(oValueDict);

            }

            var json = JsonConvert.SerializeObject(oSessionDict);

            Logger_AddLogMessage(string.Format("SaveSession({0}): {1}", key, PrettyJSON(json)), LogLevels.logINFO);

            infraestructureRepository.InsertOrUpdateSessionVariables(key, json);
        }


        protected void LoadSession(string key)
        {

            string jsonSession = infraestructureRepository.GetSessionVariables(key);

            Logger_AddLogMessage(string.Format("LoadSession from database({0}): {1}", key, PrettyJSON(jsonSession)), LogLevels.logINFO);

            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSession);

            foreach (KeyValuePair<string, string> kvValue in values)
            {

                string strType = "";
                string strValue = "";
                var value = JsonConvert.DeserializeObject<Dictionary<string, string>>(kvValue.Value);

                foreach (KeyValuePair<string, string> variable in value)
                {
                    if (variable.Key == "type")
                    {
                        strType = variable.Value;
                    }
                    else
                        strValue = variable.Value;

                }

                if (strType == "null")
                {
                    _session[kvValue.Key] = null;
                }
                else if (strType == "System.Globalization.CultureInfo")
                {
                    _session[kvValue.Key] = new CultureInfo(JsonConvert.DeserializeObject<string>(strValue));
                }
                else
                {
                    _session[kvValue.Key] = JsonConvert.DeserializeObject(strValue, Type.GetType(strType));
                }

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
