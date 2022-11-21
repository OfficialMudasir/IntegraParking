using integraMobile.Controllers;
using integraMobile.ExternalWS;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Properties;
using integraMobile.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace integraMobile.Helper
{
    public class Tools
    {
        #region Constants
        private const string TAG_CARRIAGERETURN_NEWLINE_TAG = "\r\n\t";
        private const string TAG_CARRIAGERETURN_NEWLINE = "\r\n";
        private const string ENCABEZADO_XML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        //private const string TEXT_I_PARK_OUT = "{\"ipark_out\":";

        public const int CONST_OSID = 5;
        public const string CONST_APPVERS = "2.8";
        public const string TEXT_I_PARK_OUT = "{\"ipark_out\":";
        public const string METHODS_SIGNUP_STEP2_JSON = "SignUpStep2JSON";
        public const string METHODS_SIGNUP_STEP1_JSON = "SignUpStep1JSON";
        public const string METHODS_QUERY_LOGIN_CITY_JSON = "QueryLoginCityJSON";
        public const string METHODS_VERIFY_LOGIN_EXISTS_JSON = "VerifyLoginExistsJSON";
        public const string METHODS_FORGET_PASSWORD_JSON = "ForgetPasswordJSON";
        public const string TAG_VERS = "1.0";

        
        public const string PATH_HOME = "Home/";
        public const string PATH_LOGON = "LogOn";
        #endregion

        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CreditCallController));
        private const long BIG_PRIME_NUMBER = 2147483647;
        static string _xmlTagName = "ipark";
        private const string IN_SUFIX = "_in";
        private const string OUT_SUFIX = "_out";
        static string _hMacKey = null;
        static byte[] _normKey = null;
        static HMACSHA256 _hmacsha256 = null;
        #endregion

        #region Methods Public
        public static string CalculateWSHash(string strInput, String sGetWSAuthHashKey)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {
                string strMACKey = sGetWSAuthHashKey;

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

                m_Log.LogMessage(LogLevels.logERROR, "CalculateWSHash::", e);
            }

            return strRes;
        }

        public static string GenerateXML(SortedList parameters, Boolean bIn)
        {
            string strRes = "";
            try
            {
                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + (bIn ? IN_SUFIX : OUT_SUFIX));
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + (bIn ? IN_SUFIX : OUT_SUFIX));

                foreach (DictionaryEntry item in parameters)
                {
                    try
                    {
                        XmlElement node = xmlOutDoc.CreateElement(item.Key.ToString());
                        node.InnerXml = item.Value.ToString().Trim();
                        rootNode.AppendChild(node);
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "GenerateXML::Exception",  e);
                    }
                }

                strRes = xmlOutDoc.OuterXml;

                if (!bIn && parameters["r"] != null)
                {
                    try
                    {
                        int ir = Convert.ToInt32(parameters["r"].ToString());
                        ResultType rt = (ResultType)ir;
                        if (ir < 0)
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "GenerateXML::" + string.Format("Error = {0}", rt.ToString()));
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GenerateXML::Exception", e);
            }

            return strRes;
        }

        public static string GenerateXML(SortedList parametersOut, Boolean bIn, List<string> lstArrayNodes = null)
        {
            string strRes = "";
            String sTAG = (bIn ? IN_SUFIX : OUT_SUFIX);
            try
            {
                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + sTAG);

                XmlAttribute jsonNS = xmlOutDoc.CreateAttribute("xmlns", "json", "http://www.w3.org/2000/xmlns/");
                jsonNS.Value = "http://james.newtonking.com/projects/json";


                xmlOutDoc.AppendChild(root);
                root.Attributes.Append(jsonNS);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + sTAG);

                foreach (DictionaryEntry item in parametersOut)
                {
                    try
                    {
                        XmlElement node = xmlOutDoc.CreateElement(item.Key.ToString());

                        if (lstArrayNodes != null)
                        {
                            if (lstArrayNodes.Contains(item.Key.ToString()))
                            {
                                //node.Attributes.Append(jsonNS);
                                node.SetAttribute("xmlns:json", @"http://james.newtonking.com/projects/json");
                            }

                        }

                        node.InnerXml = item.Value.ToString().Trim();

                        rootNode.AppendChild(node);
                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("GenerateXMLOuput::Exception item={0}; value={1}", item.Key.ToString(), item.Value.ToString()), e);
                    }
                }

                strRes = xmlOutDoc.OuterXml;

                if (parametersOut["r"] != null)
                {
                    try
                    {
                        int ir = Convert.ToInt32(parametersOut["r"].ToString());
                        ResultType rt = (ResultType)ir;

                        if (ir < 0)
                        {
                            m_Log.LogMessage(LogLevels.logERROR,string.Format("Error = {0}", rt.ToString()));
                        }
                    }
                    catch
                    {

                    }


                }

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GenerateXMLOuput::Exception", e);

            }


            return strRes;
        }

        public static string PrettyXml(string xml)
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

                return TAG_CARRIAGERETURN_NEWLINE_TAG + stringBuilder.ToString().Replace(TAG_CARRIAGERETURN_NEWLINE, TAG_CARRIAGERETURN_NEWLINE_TAG) + TAG_CARRIAGERETURN_NEWLINE;
            }
            catch
            {
                return TAG_CARRIAGERETURN_NEWLINE_TAG + xml + TAG_CARRIAGERETURN_NEWLINE;
            }
        }

        public static String ConvertXMLToJson(String sXML)
        {
            String sJson = String.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                sXML = sXML.Replace(ENCABEZADO_XML, "");
                doc.LoadXml(sXML);

                sJson = JsonConvert.SerializeXmlNode(doc);
                sJson = sJson.Replace(TEXT_I_PARK_OUT, "");
                sJson = sJson.Remove(sJson.Length - 1, 1);
            }
            catch(Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConvertXMLToJson::", ex);
            }
            return sJson;
        }

        public static T ConvertXMLJsonToObject<T>(String strOut) where T : new()
        {
            T Object = new T();
            try
            {
                String sJson = ConvertXMLToJson(strOut);

                Object = JsonConvert.DeserializeObject<T>(sJson);  
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConvertXMLJsonToObject::", ex);
            }
            return Object;
        }

        public static String ToJsonRequest(Object request) 
        {
            String sjson = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(sjson);
            String hashSource = concatValues(json);
            
            String hash = CalculateWSHash(hashSource, ConfigurationManager.AppSettings["WSAuthHashKey"]);
            json.Add("ah", hash);
           
            Newtonsoft.Json.Linq.JObject root = new Newtonsoft.Json.Linq.JObject();
            root.Add("ipark_in", json);

            return root.ToString();
        }
   
        private static String concatValues(Newtonsoft.Json.Linq.JObject json)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in json)
            {
                if (item.Value.Children().Count() > 0)
                {
                    foreach (JToken item2 in item.Value.Children())
                    {
                        if (item2.GetType().Name.Equals("JObject"))
                        {
                            builder.Append(concatValues((JObject)item2));
                        }
                        else if (item2.Children().Count() > 0)
                        {
                            builder.Append(concatValues(new JObject(item2)));
                        }
                        else
                        {
                           
                            
                        }
                    }
                }
                else
                {
                    builder.Append(item.Value);
                }
                //Console.WriteLine("{0} {1} {2} {3}\n", item.id, item.displayName,
                //    item.slug, item.imageUrl);

            }


            //foreach (Map.Entry<String, JsonElement> entry : json.Value()) 
            //{
            //    JsonElement node = entry.getValue();
            //    if (node.isJsonPrimitive()) 
            //    {
            //        builder.append(node.getAsString());
            //    } 
            //    else if (node.isJsonObject()) 
            //    {
            //        builder.append(concatValues(node.getAsJsonObject()));
            //    } 
            //    else if (node.isJsonArray()) 
            //    {
            //        JsonArray nodeArray = node.getAsJsonArray();
            //        for (JsonElement nodeElement : nodeArray) 
            //        {
            //            if (nodeElement.isJsonObject())
            //            {
            //                builder.append(concatValues(nodeElement.getAsJsonObject()));
            //            }
            //    }
            //}
         //   }
            return builder.ToString();
        }

        public static string Message(ResultType resultType)
        {

            switch (resultType)
            {
                //case ResultType.Result_ERROR_EXTENSION_IS_NOT_POSSIBLE:
                //    return Resources.upr_error_ope_extensionnotpossible;

                    
                //case ResultType.Result_ERROR_EXTENSION_IS_NOT_POSSIBLE:
                //    return Resources.upr_error_ope_extensionnotpossible;

                //case ResultType.Result_ERROR_PLATE_HAS_NOT_WAITED_ENOUGH:
                //    return Resources.upr_error_ope_platehastowait;

                //case ResultType.Result_ERROR_PLATE_NO_RIGHTS_UNPARKING:
                //    return Resources.error_plate_no_rights_unparking;

                //case ResultType.Result_ERROR_FINE_NUMBER_NOT_FOUND:
                //    return Resources.pt_summary_error_fine_number_not_found;

                //case ResultType.Result_ERROR_FINE_TYPE_IS_NOT_PAYABLE:
                //    return Resources.pt_summary_error_fine_type_is_not_payable;

                //case ResultType.Result_ERROR_PAYMENT_PERIOD_HAS_EXPIRED:
                //    return Resources.pt_summary_error_payment_period_has_expired;

                //case ResultType.Result_ERROR_FINE_NUMBER_ALREADY_PAID:
                //    return Resources.pt_summary_error_fine_number_already_paid;

                //case ResultType.Result_ERROR_INVALID_AUTHENTICATION:
                //    return Resources.ul_error_fail;

                //case ResultType.Result_ERROR_SESSION_EXPIRED:
                //    return Resources.error_session_expired;

                //case ResultType.Result_ERROR_RECHARGE_INVALID_PAYMENT_MEAN:
                //case ResultType.Result_ERROR_NOT_ENOUGH_BALANCE:
                //    return Resources.urech_summary_error_invalid_payment_mean;

                //case ResultType.Result_ERROR_RECHARGE_PAYMENT_FAILED:
                //    return Resources.urech_summary_error_payment_failed;

                //case ResultType.Result_ERROR_TARIFFS_NOT_AVAILABLE:
                //    return Resources.upt_error_tariffs_not_available;

                //case ResultType.Result_ERROR_REGISTER_NEW_WRONG_FNAME:
                //    return Resources.urs_error_first_name;

                //case ResultType.Result_ERROR_REGISTER_NEW_WRONG_LNAME:
                //    return Resources.urs_error_last_name;

                case ResultType.Result_Error_Invalid_Country_Code:
                    return Resources.urs_error_country_code;

                //case ResultType.Result_ERROR_REGISTER_NEW_WRONG_CELL_NUMBER:
                //    return Resources.urs_error_cell_number;

                case ResultType.Result_Error_Invalid_Email_Address:
                    return Resources.urs_error_email;

                //case ResultType.Result_ERROR_REGISTER_NEW_CELL_NUMBER_EXISTS:
                //    return Resources.urs_error_cell_number_exists;

                case ResultType.Result_Error_Email_Already_Exist:
                    return Resources.urs_error_email_exists;

                //case ResultType.Result_ERROR_RESIDENT_PARKING_EXHAUSTED:
                //    return Resources.upsr_error_resident_parking_exhausted;

                //case ResultType.Result_ERROR_OPERATION_EXPIRED:
                //    return Resources.upr_error_ope_expired;

                //case ResultType.Result_ERROR_INVALID_TICKET_ID:
                //    return Resources.upr_error_ope_invalidticket_id;

                //case ResultType.Result_ERROR_EXPIRED_TICKET_ID:
                //    return Resources.upr_error_ope_expiredticket_id;

                //case ResultType.Result_ERROR_OPERATION_NOTFOUND:
                //    return Resources.upr_error_ope_notfound;

                //case ResultType.Result_ERROR_OPERATION_ALREADYCLOSED:
                //    return Resources.upr_error_ope_alreadyclosed;

                //case ResultType.Result_ERROR_OPERATION_ALREADYEXISTS:
                //    return Resources.upr_error_ope_alreadyexists;

                //case ResultType.Result_ERROR_OPERATION_CONFIRMALREADYEXECUTING:
                //    return Resources.upr_error_ope_confirmexecuting;

                //case ResultType.Result_ERROR_USER_NOT_EXIST:
                //    return Resources.ul_error_fail;

                //case ResultType.Result_ERROR_INVALID_USER_RECEIVER_EMAIL:
                //    return Resources.upr_error_ope_invalidemail;

                //case ResultType.Result_ERROR_INVALID_USER_RECEIVER_EMAIL_DISABLED:
                //    return Resources.upr_error_ope_invalidemail_disabled;
                ////    break;

                /************************************************************************************/
                /***********************Estos Resultado se usan para el login ***********************/
                /************************************************************************************/
                case ResultType.Result_Error_User_Is_Not_Activated:         /*-63*/
                    return Resources.Home_GetUserDataError;
                case ResultType.Result_Error_Invalid_User:                  /*-26*/
                    return Resources.Home_BadUserNameOrPassword;
                case ResultType.Result_Error_InvalidAuthentication:         /*-11*/
                    return Resources.Home_BadUserNameOrPassword;
                case ResultType.Result_Error_User_Not_Logged:               /*-27*/
                    return Resources.Home_GetUserDataError;
                case ResultType.Result_Error_Generic:                       /*-9*/
                    return Resources.Home_GetUserDataError;
                case ResultType.Result_Error_InvalidAuthenticationHash:     /*-1*/
                    return Resources.Home_GetUserDataError;
                case ResultType.Result_Error_Missing_Input_Parameter:        /*-20*/
                    return Resources.Home_GetUserDataError;

                /************************************************************************************/
                /***********************Estos Resultado se usan para el login ***********************/
                /************************************************************************************/

                //case ResultType.Result_ERROR_PLATE_IS_ASSIGNED_TO_ANOTHER_USER:
                //    return Resources.upis_plate_assigned_to_assigned_to_another_user;

                //case ResultType.Result_ERROR_MISSING_INPUT_PARAMETER:
                //    return Resources.
            }
            return String.Empty;
        }


        public static void RedirectWithData(NameValueCollection data, string url)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", url);
            foreach (string key in data)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", key, data[key]);
            }
            s.Append("</form></body></html>");
            response.Write(s.ToString());
            response.End();
        }

        public static string RemeveTagIparkOut(string strOut)
        {
            strOut = strOut.Replace(TEXT_I_PARK_OUT, "");
            strOut = strOut.Remove(strOut.Length - 1, 1);
            return strOut;
        }

        public static string CreatingJsonInVerifyLoginExistsRequest(string username, string password)
        {
            string sInJsonVerify = string.Empty;
            VerifyLoginExistsRequest oVerifyLoginExistsRequest = new VerifyLoginExistsRequest();
            oVerifyLoginExistsRequest = oVerifyLoginExistsRequest.getRequest(username, password);
            sInJsonVerify = Tools.ToJsonRequest(oVerifyLoginExistsRequest);
            return sInJsonVerify;
        }

        public static string CreatingJsonInVerifySingUpExistsRequest(int ilang, int iConsOsid, string email, string vr)
        {
            string sInJsonVerify = string.Empty;
            VerifySignUpExistsRequest oVerifySignUpExistsRequest = new VerifySignUpExistsRequest();
            oVerifySignUpExistsRequest = oVerifySignUpExistsRequest.getRequest(ilang, iConsOsid, email, vr);
            sInJsonVerify = Tools.ToJsonRequest(oVerifySignUpExistsRequest);
            return sInJsonVerify;
        }


        public static void QuickLog(string text)
        {
            var path1 = AppDomain.CurrentDomain.BaseDirectory;
            string filename = path1+"Logs\\log-" + DateTime.Now.ToShortDateString() + ".log";
            string dirPath = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }
        #endregion
    }
}