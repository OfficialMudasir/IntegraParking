using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace integraMobile.Infrastructure.RedsysAPI
{
   public class RedsysAPISoap
    {

        /// <summary>
        /// Attribute Dictionary key/value
        /// </summary>
        private Dictionary<string, string> m_keyvalues;


        /// <summary>
        /// Attribute Cryptografy class
        /// </summary>
        private Cryptogra cryp;


        /// <summary>
        /// Constructor
        /// </summary>
        public RedsysAPISoap()
        {

            m_keyvalues = new Dictionary<string, string>();

            cryp = new Cryptogra();
        }

       /// <summary>
       /// Get Order number
       /// </summary>
       /// <param name="data"></param>
       /// <returns></returns>
        private string GetOrder(string data)
        {
            int posOrderIni = data.IndexOf("<Ds_Order>");
            int tamOrderIni = "<Ds_Order>".Length;
            int posOrderEnd = data.IndexOf("</Ds_Order>");
            string result = data.Substring(posOrderIni + tamOrderIni, posOrderEnd - (posOrderIni + tamOrderIni));

            return result;
        }

        /// <summary>
        /// Get parameter from XML string according to key
        /// </summary>
        /// <param name="data">string with XML</param>
        /// <param name="keyInit"> Init key label</param>
        /// <param name="keyEnd"> End key label </param>
        /// <returns></returns>
        public string GetParameter(string data, string keyInit, string keyEnd)
        {
            int posKeyIni = data.IndexOf(keyInit);
            int tamKeyIni = keyInit.Length;
            int posKeyEnd = data.IndexOf(keyEnd);

            
            if (posKeyIni != -1 || posKeyEnd != -1)
            {
                return data.Substring(posKeyIni + tamKeyIni, posKeyEnd - (posKeyIni + tamKeyIni));
            }

            return "";
          }

        /// <summary>
        ///  Add key/value to dictionary from string XML using init and end labels
        /// </summary>
        /// <param name="data">String with XML </param>
        /// <param name="key"> Key of dictionary </param>
        /// <param name="keyInit"> Init key label </param>
        /// <param name="keyEnd"> End key label</param>
        private void GetParameterDiccionary(string data, string key, string keyInit, string keyEnd)
        {
      
            int posIni = data.IndexOf(keyInit);
            int tamIni = keyInit.Length;
            int posEnd = data.IndexOf(keyEnd);

            if ( posIni != -1 || posEnd != -1)
            {
                string res = data.Substring(posIni + tamIni, posEnd - (posIni + tamIni));
                m_keyvalues.Add(key, res);
            }
        }

  
        /// <summary>
        /// Convert XML request string into Dictionary key/value object 
        /// </summary>
        /// <param name="data"></param>
        public void XMLToDiccionary(string data)
        {
                GetParameterDiccionary(data,"Fecha","<Fecha>","</Fecha>");
                GetParameterDiccionary(data,"Hora","<Hora>","</Hora>");
                GetParameterDiccionary(data, "SecurePayment", "<Ds_SecurePayment>", "</Ds_SecurePayment>");
                GetParameterDiccionary(data, "Card_Country", "<Ds_Card_Country>", "</Ds_Card_Country>");
                GetParameterDiccionary(data,"Amount","<Ds_Amount>", "</Ds_Amount>");
                GetParameterDiccionary(data,"Currency","<Ds_Currency>","</Ds_Currency>");
                GetParameterDiccionary(data,"Order","<Ds_Order>", "</Ds_Order>");
                GetParameterDiccionary(data, "MerchantCode", "<Ds_MerchantCode>", "</Ds_MerchantCode>");
                GetParameterDiccionary(data,"Terminal","<Ds_Terminal>","</Ds_Terminal>");
                GetParameterDiccionary(data,"Response","<Ds_Response>", "</Ds_Response>" );
                GetParameterDiccionary(data,"MerchantData","<Ds_MerchantData>","</Ds_MerchantData>");
                GetParameterDiccionary(data, "TransactionType", "<Ds_TransactionType>", "</Ds_TransactionType>");
                GetParameterDiccionary(data, "ConsumerLanguage", "<Ds_ConsumerLanguage>", "</Ds_ConsumerLanguage>");
                GetParameterDiccionary(data, "AuthorisationCode", "<Ds_AuthorisationCode>", "</Ds_AuthorisationCode>");
            
        }
      

        /// <summary>
        /// Get value from dictionary using key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDictionary(string key)
        {

            if (m_keyvalues.ContainsKey(key))
            {
               return m_keyvalues[key];
            }
            return null;
        }



        /// <summary>
        /// Decode Base64 string to byte[]
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        private byte[] Base64Decode2(string encodedData)
        {

            try
            {
                byte[] encodedDataAsBytes
              = System.Convert.FromBase64String(encodedData);

                return encodedDataAsBytes;
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }


        /// <summary>
        /// Encode byte[] to base64 string
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        private string Base64Encode2(byte[] toEncode)
        {
            try
            {
                string returnValue
                      = System.Convert.ToBase64String(toEncode);
                return returnValue;
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message);
            }
        }

       /// <summary>
       /// Get XML between labels Request y  /Request
       /// </summary>
       /// <param name="xml"></param>
       /// <returns></returns>
        private string GetMessage(string xml)
        {
            int posKeyIni = xml.IndexOf("<Message>");
            int tamKeyIni = "<Message>".Length;
            int posKeyEnd = xml.IndexOf("</Request>");
            int tamKeyEnd = "</Request>".Length;
            int tamSignature = "<Signature>".Length;

            if (posKeyIni != -1 || posKeyEnd != -1)
            {
                return xml.Substring(posKeyIni + tamKeyIni, posKeyEnd + tamKeyEnd - tamSignature + 2);
            }

            return "";

        }

        /// <summary>
        ///  Method for paid request generation. This method generates signature using main key and received notification message
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputData"></param>
        /// <returns>string with signature encoded using Base 64</returns>
        public string createMerchantSignatureNotifSOAPRequest(string key, string inputData)
        {

            // Get contain between labels <Request Ds_Version="0.0"> and </Request>
            string request = GetMessage(inputData);
            // Decode key to byte[]
            byte[] k = Base64Decode2(key);
            // Calculate derivated key by encrypting with 3DES the "DS_MERCHANT_ORDER" with decoded key 
            byte[] derivatedkey = cryp.Encrypt3DES(GetOrder(inputData), k);
            /// Calculate HMAC SHA256 with DATOSENTRADA XML string using derivated key calculated previously
            byte[] hash = cryp.GetHMACSHA256(request, derivatedkey);
            // Encode byte[] hash to Base64 String
            string res = Base64Encode2(hash);
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetOrderNotifSOAP(string data)
        {
            return GetParameter(data,"<Ds_Order>","</Ds_Order>");

        }
        /// <summary>
        ///  Method for paid data reception (Response HOST to HOST). Generate the signature with response parameters to check that the signature is valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cadena"></param>
        /// <param name="numOrder"></param>
        /// <returns></returns>
        public string createSignatureNotifSOAPResponse(string key, string response, string numOrder)
        {
            // Decode key to byte[]
            byte[] k = Base64Decode2(key);
            // Calculate derivated key by encrypting with 3DES the Order number (numOrder) with decoded key k 
            byte[] derivatedkey = cryp.Encrypt3DES(numOrder, k);
            //Generate string with XML response paremeters cadena and previous derivated key 
            byte[] hash = cryp.GetHMACSHA256(response, derivatedkey);
            // Encode byte[] hash to Base64 String
            string res = Base64Encode2(hash);
            return res;
        }
    
    }
}
