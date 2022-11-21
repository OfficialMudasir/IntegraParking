using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace integraMobile.Infrastructure.RedsysAPI
{
   public class RedsysAPIWs
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
        public RedsysAPIWs()
        {

            m_keyvalues = new Dictionary<string, string>();


            cryp = new Cryptogra();
        }


        /// <summary>
        /// Get DS_MERCHANT_ORDER value from XML string DATOSENTRADA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetOrder(string data)
        {
            int posOrderIni = data.IndexOf("<DS_MERCHANT_ORDER>");
            int tamOrderIni = "<DS_MERCHANT_ORDER>".Length;
            int posOrderEnd = data.IndexOf("</DS_MERCHANT_ORDER>");
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
        private string GetParameter(string data, string keyInit, string keyEnd)
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
        /// Generate string with XML response paremeters 
        /// concatenating Ds_Amount, Ds_Order, Ds_MerchantCode, Ds_MerchantCode, Ds_Currency, Ds_Response, Ds_TransactionType and Ds_SecurePayment
        /// utilized into createSignatureResponseHostToHost
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GenerateCadena(string data)
        {
            StringBuilder cadena = new StringBuilder();
            string amount = GetParameter(data, "<Ds_Amount>", "</Ds_Amount>");
            string order =  GetParameter(data,  "<Ds_Order>","</Ds_Order>");
            string code =   GetParameter(data, "<Ds_MerchantCode>", "</Ds_MerchantCode>");
            string currency = GetParameter(data, "<Ds_Currency>", "</Ds_Currency>");
            string response = GetParameter(data, "<Ds_Response>", "</Ds_Response>");
            string type = GetParameter(data, "<Ds_TransactionType>", "</Ds_TransactionType>");
            string secure = GetParameter(data, "<Ds_SecurePayment>", "</Ds_SecurePayment>");
            cadena.Append(amount);
            cadena.Append(order);
            cadena.Append(code);
            cadena.Append(currency);
            cadena.Append(response);
            cadena.Append(type);
            cadena.Append(secure);
            return cadena.ToString();
        }

        /// <summary>
        /// Convert XML response string into Dictionary key/value object 
        /// </summary>
        /// <param name="data"></param>
        public void XMLToDiccionary(string data)
        {
                GetParameterDiccionary(data,"CODIGO","<CODIGO>","</CODIGO>");
                GetParameterDiccionary(data,"Ds_Amount","<Ds_Amount>","</Ds_Amount>");
                GetParameterDiccionary(data,"Ds_Currency","<Ds_Currency>","</Ds_Currency>");
                GetParameterDiccionary(data,"Ds_Order","<Ds_Order>", "</Ds_Order>");
                GetParameterDiccionary(data,"Ds_Signature","<Ds_Signature>","</Ds_Signature>");
                GetParameterDiccionary(data,"Ds_MerchantCode","<Ds_MerchantCode>", "</Ds_MerchantCode>");
                GetParameterDiccionary(data,"Ds_Terminal","<Ds_Terminal>", "</Ds_Terminal>");
                GetParameterDiccionary(data, "Ds_Response", "<Ds_Response>", "</Ds_Response>");
                GetParameterDiccionary(data,"Ds_AuthorisationCode","<Ds_AuthorisationCode>","</Ds_AuthorisationCode>");
                GetParameterDiccionary(data,"Ds_TransactionType","<Ds_TransactionType>", "</Ds_TransactionType>");
                GetParameterDiccionary(data,"Ds_SecurePayment","<Ds_SecurePayment>", "</Ds_SecurePayment>" );
                GetParameterDiccionary(data,"Ds_Language","<Ds_Language>","</Ds_Language>");
                GetParameterDiccionary(data,"Ds_MerchantData", "<Ds_MerchantData>", "</Ds_MerchantData>");
                GetParameterDiccionary(data,"Ds_Card_Country","<Ds_Card_Country>","</Ds_Card_Country>");
                GetParameterDiccionary(data, "Ds_EMV3DS", "<Ds_EMV3DS>", "</Ds_EMV3DS>");
                GetParameterDiccionary(data, "MD", "<MD>", "</MD>");
            
        }
        /// <summary>
        /// Generate DATOSENTRADA XML parameters
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="fuc"></param>
        /// <param name="currency"></param>
        /// <param name="pan"></param>
        /// <param name="cvv2"></param>
        /// <param name="trans"></param>
        /// <param name="terminal"></param>
        /// <param name="expire"></param>
        /// <returns> XML string </returns>
        public string GenerateDatoEntradaXML(string amount, string fuc, string currency, string pan, string cvv2, string trans, string terminal, string expire, string merchant_group)
        {
            // Generate automatically with Customer's platform
            var id = "1234567891";
            StringBuilder cad = new StringBuilder();
            string initentrada = "<DATOSENTRADA>";
            string endentrada = "</DATOSENTRADA>";
            string amountCad = "<DS_MERCHANT_AMOUNT>" + amount + "</DS_MERCHANT_AMOUNT>";
            string idCad = "<DS_MERCHANT_ORDER>" + id + "</DS_MERCHANT_ORDER>";
            string funCad = "<DS_MERCHANT_MERCHANTCODE>" + fuc + "</DS_MERCHANT_MERCHANTCODE>";
            string currencyCad = "<DS_MERCHANT_CURRENCY>" + currency + "</DS_MERCHANT_CURRENCY>";
            string panCad = "<DS_MERCHANT_PAN>"+pan+ "</DS_MERCHANT_PAN>";
            string cvv2Cad = "<DS_MERCHANT_CVV2>" + cvv2 + "</DS_MERCHANT_CVV2>";
            string transCad = "<DS_MERCHANT_TRANSACTIONTYPE>"+ trans + "</DS_MERCHANT_TRANSACTIONTYPE>";
            string terminalCad = "<DS_MERCHANT_TERMINAL>" + terminal + "</DS_MERCHANT_TERMINAL>";
            string expireCad = "<DS_MERCHANT_EXPIRYDATE>" + expire + "</DS_MERCHANT_EXPIRYDATE>";
            string merchantGroupCad = null;

            if (!string.IsNullOrEmpty(merchant_group))
                merchantGroupCad = "<DS_MERCHANT_GROUP>" + merchant_group + "</DS_MERCHANT_GROUP>";            


            cad.Append(initentrada);
            cad.Append(amountCad);
            cad.Append(idCad);
            cad.Append(funCad);
            cad.Append(currencyCad);
            cad.Append(panCad);
            cad.Append(cvv2Cad);
            cad.Append(transCad);
            cad.Append(terminalCad);
            cad.Append(expireCad);
            if (merchantGroupCad != null) cad.Append(merchantGroupCad);
            cad.Append(endentrada);
            return cad.ToString();
        }

        public string GenerateDatoEntradaXMLTrataPeticionNO3DS(string amount, string fuc, string currency, string token, string trans, string terminal, string id, string authcode, string merchant_group, string strCOFTxnID, string strExemption="")
        {
            // Generate automatically with Customer's platform
            //var id = "1234567891";
            StringBuilder cad = new StringBuilder();
            string initentrada = "<DATOSENTRADA>";
            string endentrada = "</DATOSENTRADA>";
            string amountCad = "<DS_MERCHANT_AMOUNT>" + amount + "</DS_MERCHANT_AMOUNT>";
            string idCad = "<DS_MERCHANT_ORDER>" + id + "</DS_MERCHANT_ORDER>";
            string funCad = "<DS_MERCHANT_MERCHANTCODE>" + fuc + "</DS_MERCHANT_MERCHANTCODE>";
            string currencyCad = "<DS_MERCHANT_CURRENCY>" + currency + "</DS_MERCHANT_CURRENCY>";
            string tokenCad = null;
            if (!string.IsNullOrEmpty(token))
                tokenCad = "<DS_MERCHANT_IDENTIFIER>" + token + "</DS_MERCHANT_IDENTIFIER>";            
            string transCad = "<DS_MERCHANT_TRANSACTIONTYPE>" + trans + "</DS_MERCHANT_TRANSACTIONTYPE>";
            string terminalCad = "<DS_MERCHANT_TERMINAL>" + terminal + "</DS_MERCHANT_TERMINAL>";
            string authCodeCad = null;
            if (!string.IsNullOrEmpty(authcode))
                authCodeCad = "<DS_MERCHANT_AUTHORISATIONCODE>" + authcode + "</DS_MERCHANT_AUTHORISATIONCODE>";
            string merchantGroupCad = null;
            if (!string.IsNullOrEmpty(merchant_group))
                merchantGroupCad = "<DS_MERCHANT_GROUP>" + merchant_group + "</DS_MERCHANT_GROUP>";

            string strPSD2Cad = null;
            if (!string.IsNullOrEmpty(strExemption))
            {
                strPSD2Cad = "<DS_MERCHANT_EXCEP_SCA>" + strExemption + "</DS_MERCHANT_EXCEP_SCA>";
                if (strExemption == "MIT")
                {                    
                    strPSD2Cad += "<DS_MERCHANT_COF_INI>N</DS_MERCHANT_COF_INI>";
                    strPSD2Cad += "<DS_MERCHANT_COF_TYPE>C</DS_MERCHANT_COF_TYPE>";
                    strPSD2Cad += "<DS_MECHANT_COF_TXNID>"+strCOFTxnID+"</DS_MECHANT_COF_TXNID>";
                    strPSD2Cad += "<DS_MERCHANT_DIRECTPAYMENT>true</DS_MERCHANT_DIRECTPAYMENT>";
                }
            }

            cad.Append(initentrada);
            cad.Append(amountCad);
            cad.Append(idCad);
            cad.Append(funCad);
            cad.Append(currencyCad);
            if (tokenCad != null) cad.Append(tokenCad);            
            cad.Append(transCad);
            cad.Append(terminalCad);
            if (authCodeCad != null) cad.Append(authCodeCad);
            if (merchantGroupCad != null) cad.Append(merchantGroupCad);
            if (strPSD2Cad != null) cad.Append(strPSD2Cad);
            
            cad.Append(endentrada);
            return cad.ToString();
        }


       

        public string GenerateDatoEntradaXML3DS(string amount, string fuc, string currency, string token, string trans, string terminal, string id, string authcode, string merchant_group, string strCOFTxnID, string s3DSParameters)
        {
            // Generate automatically with Customer's platform
            //var id = "1234567891";
            StringBuilder cad = new StringBuilder();
            string initentrada = "<DATOSENTRADA>";
            string endentrada = "</DATOSENTRADA>";
            string amountCad = "<DS_MERCHANT_AMOUNT>" + amount + "</DS_MERCHANT_AMOUNT>";
            string idCad = "<DS_MERCHANT_ORDER>" + id + "</DS_MERCHANT_ORDER>";
            string funCad = "<DS_MERCHANT_MERCHANTCODE>" + fuc + "</DS_MERCHANT_MERCHANTCODE>";
            string currencyCad = "<DS_MERCHANT_CURRENCY>" + currency + "</DS_MERCHANT_CURRENCY>";
            string tokenCad = null;
            if (!string.IsNullOrEmpty(token))
                tokenCad = "<DS_MERCHANT_IDENTIFIER>" + token + "</DS_MERCHANT_IDENTIFIER>";
            string transCad = "<DS_MERCHANT_TRANSACTIONTYPE>" + trans + "</DS_MERCHANT_TRANSACTIONTYPE>";
            string terminalCad = "<DS_MERCHANT_TERMINAL>" + terminal + "</DS_MERCHANT_TERMINAL>";
            string authCodeCad = null;
            if (!string.IsNullOrEmpty(authcode))
                authCodeCad = "<DS_MERCHANT_AUTHORISATIONCODE>" + authcode + "</DS_MERCHANT_AUTHORISATIONCODE>";
            string merchantGroupCad = null;
            if (!string.IsNullOrEmpty(merchant_group))
                merchantGroupCad = "<DS_MERCHANT_GROUP>" + merchant_group + "</DS_MERCHANT_GROUP>";

            string strPSD2Cad = null;
            if (!string.IsNullOrEmpty(s3DSParameters))
                strPSD2Cad = string.Format("<DS_MERCHANT_EMV3DS>{0}</DS_MERCHANT_EMV3DS>", s3DSParameters);

            strPSD2Cad += "<DS_MERCHANT_COF_INI>N</DS_MERCHANT_COF_INI>";
            strPSD2Cad += "<DS_MERCHANT_COF_TYPE>C</DS_MERCHANT_COF_TYPE>";
            strPSD2Cad += "<DS_MECHANT_COF_TXNID>" + strCOFTxnID + "</DS_MECHANT_COF_TXNID>";
            //strPSD2Cad += "<DS_MERCHANT_DIRECTPAYMENT>true</DS_MERCHANT_DIRECTPAYMENT>";


            cad.Append(initentrada);
            cad.Append(amountCad);
            cad.Append(idCad);
            cad.Append(funCad);
            cad.Append(currencyCad);
            if (tokenCad != null) cad.Append(tokenCad);
            cad.Append(transCad);
            cad.Append(terminalCad);
            if (authCodeCad != null) cad.Append(authCodeCad);
            if (merchantGroupCad != null) cad.Append(merchantGroupCad);
            if (strPSD2Cad != null) cad.Append(strPSD2Cad);
            cad.Append(endentrada);
            return cad.ToString();
        }


        /// <summary>
        /// Concatenate XML DATOSENTRADA string calculated using GenerateDatoEntradaXML with labels "DS_SIGNATUREVERSION" and "DS_SIGNATURE"
        /// </summary>
        /// <param name="dataEntrada"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public string GenerateRequestXML(string dataEntrada, string signature)
        {
            StringBuilder cad = new StringBuilder();
            string initrequest = "<REQUEST>";
            string endrequest = "</REQUEST>";
            string version = "<DS_SIGNATUREVERSION>HMAC_SHA256_V1</DS_SIGNATUREVERSION>";
            string initsignature = "<DS_SIGNATURE>";
            string endsignature = "</DS_SIGNATURE>";
            cad.Append(initrequest);
            cad.Append(dataEntrada);
            cad.Append(version);
            cad.Append(initsignature);
            cad.Append(signature);
            cad.Append(endsignature);
            cad.Append(endrequest);
            return cad.ToString();

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
        ///  Method for paid request generation. This method generates signature using main key and DATOSENTRADA XML string 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inputData"></param>
        /// <returns>string with signature encoded using Base 64</returns>
        public string createMerchantSignatureHostToHost(string key, string inputData)
        {

            // Decode key to byte[]
            byte[] k = Base64Decode2(key);
            // Calculate derivated key by encrypting with 3DES the "DS_MERCHANT_ORDER" with decoded key 
            byte[] derivatedkey = cryp.Encrypt3DES(GetOrder(inputData), k);
            /// Calculate HMAC SHA256 with DATOSENTRADA XML string using derivated key calculated previously
            byte[] hash = cryp.GetHMACSHA256(inputData, derivatedkey);
            // Encode byte[] hash to Base64 String
            string res = Base64Encode2(hash);
            return res;
        }


        /// <summary>
        ///  Method for paid data reception (Response HOST to HOST). Generate the signature with response parameters to check that the signature is valid
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cadena"></param>
        /// <param name="numOrder"></param>
        /// <returns></returns>
        public string createSignatureResponseHostToHost(string key, string cadena, string numOrder)
        {
            // Decode key to byte[]
            byte[] k = Base64Decode2(key);
            // Calculate derivated key by encrypting with 3DES the Order number (numOrder) with decoded key k 
            byte[] derivatedkey = cryp.Encrypt3DES(numOrder, k);
            //Generate string with XML response paremeters cadena and previous derivated key 
            byte[] hash = cryp.GetHMACSHA256(cadena, derivatedkey);
            // Encode byte[] hash to Base64 String
            string res = Base64Encode2(hash);
            return res;
        }
    
    }
}
