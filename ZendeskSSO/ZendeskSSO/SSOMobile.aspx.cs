using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using integraMobile.Infrastructure.Logging.Tools;
using System.Web.Script.Serialization;
using Ninject;
using Ninject.Web;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using System.Xml;
using Newtonsoft.Json;
using ZendeskSSO.Helper;
using Newtonsoft.Json.Linq;

namespace ZendeskSSO
{
    public partial class SSOMobile : System.Web.UI.Page
    {
              
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SSOMobile));

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strToken = Request.Params["user_token"];
            //string strToken = "fe90933c77159c88a339117ffb9e9c885d9dcbc66384ac88";

            Logger_AddLogMessage(string.Format("Zendesk SSO Mobile Request Params => user_token = {0}", strToken), LogLevels.logINFO);
            
            Dictionary<string, object> payload = null;
            bool bSessionId=false;
            if ((!string.IsNullOrEmpty(strToken))&&(customersRepository.GetUserFromOpenSessionSSO(strToken, ref payload)))
            {
                bSessionId = true;
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)t.TotalSeconds;
                payload.Add("iat", timestamp);
                payload.Add("jti", System.Guid.NewGuid().ToString());
                var json = new JavaScriptSerializer().Serialize(payload);

                string token = JWT.JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["SharedMobileSDKKey"], JWT.JwtHashAlgorithm.HS256);

                Response.ContentType = "application/json";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                var jsonResponseDict = new Dictionary<string, object>() { { "jwt", token } };

                string strResponse = new JavaScriptSerializer().Serialize(jsonResponseDict);

                Logger_AddLogMessage(string.Format("JWT Mobile Payload => {0}; Response Generated = {1}", json, strResponse), LogLevels.logINFO);
                Response.Write(strResponse);
            }
            else
            {
                //Buscamos si el SessionId esta en algun servidor Externo
                if (!string.IsNullOrEmpty(strToken))
                {
                    string xmlOut = "";
                    string xmlIn = "";
                    string jsonOut = string.Empty;
                    List<COUNTRIES_REDIRECTION> oListCountriesRedirection = infraestructureRepository.GetCountriesRedirections();
                    foreach (COUNTRIES_REDIRECTION country in oListCountriesRedirection)
                    {

                        ZendeskSSO.Request.TokenZendeskRequest oTokenZendeskResquest = ZendeskSSO.Request.TokenZendeskRequest.getRequest(strToken);
                        string sInJson = Tools.ToJsonRequest(oTokenZendeskResquest);
                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJson);
                        xmlIn = doc.InnerXml;
                        m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(xmlIn)));

                        jsonOut = CallToServerTokenZendesk(sInJson, jsonOut, country);

                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        Logger_AddLogMessage(string.Format("Country Redirection in => {0}; Out = {1}", sInJson, jsonOut), LogLevels.logINFO);

                        Dictionary<string, object> payloadServer = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonOut);


                    if (payloadServer.Keys.Contains("r") && payloadServer["r"].ToString().Equals("1"))
                        {

                            JObject jObject2 = JObject.Parse(jsonOut);
                            payload = new Dictionary<string, object>();

                            //name
                            if (jObject2["name"] != null)
                            {
                                payload.Add("name", jObject2["name"].ToString());
                            }                            //email
                            if (jObject2["email"] != null)
                            {
                                payload.Add("email", jObject2["email"].ToString());
                            }
                            //external_id
                            if (jObject2["external_id"] != null)
                            {
                                payload.Add("external_id", jObject2["external_id"].ToString());
                            }

                            //alias
                            if (jObject2["alias"] != null)
                            {
                                payload.Add("alias", jObject2["alias"].ToString());
                            }

                            //phone
                            if (jObject2["phone"]!=null)
                            {
                                payload.Add("phone", jObject2["phone"].ToString());
                            }


                            //tags
                            if (jObject2["tags"] != null)
                            {
                                payload.Add("tags", jObject2["tags"].ToString());
                            }

                            /*
                            //tags
                            if (jObject2["tags"] != null)
                            {
                                string stags = jObject2["tags"].ToString();
                                JArray jObject4 = JArray.Parse(stags);
                                string sReturnTags = "";
                                foreach (var o in jObject4)
                                {

                                    if (!string.IsNullOrEmpty(sReturnTags))
                                        sReturnTags += ",";

                                    sReturnTags += o.ToString();
                                    

                                }
                               
                               payload.Add("tags", sReturnTags);
                                
                            }*/

                            //role
                            if (jObject2["role"] != null)
                            {
                                payload.Add("role", jObject2["role"].ToString());
                            }

                            //user_fields
                            if (jObject2["user_fields"] != null)
                            {
                                string sUserFields = jObject2["user_fields"].ToString();

                                JObject jObject3 = JObject.Parse(sUserFields); ;
                                Dictionary<string, object> payloadUserFields = new Dictionary<string, object>();

                                 
                                 
                                //telfono_fijo
                                if (jObject3["telfono_fijo"] != null)
                                {
                                    payloadUserFields.Add("telfono_fijo", jObject3["telfono_fijo"].ToString());
                                }


                                //dni_pasaporte
                                if (jObject3["dni_pasaporte"] != null)
                                {
                                    payloadUserFields.Add("dni_pasaporte", jObject3["dni_pasaporte"].ToString());
                                }


                                //provincia
                                if (jObject3["provincia"] != null)
                                {
                                    payloadUserFields.Add("provincia", jObject3["provincia"].ToString());
                                }


                                //ciudad
                                if (jObject3["ciudad"] != null)
                                {
                                    payloadUserFields.Add("ciudad", jObject3["ciudad"].ToString());
                                }



                                //matriculas
                                if (jObject3["matriculas"] != null)
                                {
                                    payloadUserFields.Add("matriculas", jObject3["matriculas"].ToString());
                                }



                                //alta
                                if (jObject3["alta"] != null)
                                {
                                    payloadUserFields.Add("alta", jObject3["alta"].ToString());
                                }

                                //tipo_de_suscripcion
                                if (jObject3["tipo_de_suscripcion"] != null)
                                {
                                    payloadUserFields.Add("tipo_de_suscripcion", jObject3["tipo_de_suscripcion"].ToString());
                                }


                                //saldo_actual
                                if (jObject3["saldo_actual"] != null)
                                {
                                    payloadUserFields.Add("saldo_actual", jObject3["saldo_actual"].ToString());
                                }


                                //version
                                if (jObject3["version"] != null)
                                {
                                    payloadUserFields.Add("version", jObject3["version"].ToString());
                                }



                                //sistema_operativo
                                if (jObject3["sistema_operativo"] != null)
                                {
                                    payloadUserFields.Add("sistema_operativo", jObject3["sistema_operativo"].ToString());
                                }



                                //modelo_dispositvo
                                if (jObject3["modelo_dispositvo"] != null)
                                {
                                    payloadUserFields.Add("modelo_dispositvo", jObject3["modelo_dispositvo"].ToString());
                                }

                                //numero_serie_dispositivo
                                if (jObject3["numero_serie_dispositivo"] != null)
                                {
                                    payloadUserFields.Add("numero_serie_dispositivo", jObject3["numero_serie_dispositivo"].ToString());
                                }


                                //estado
                                if (jObject3["estado"] != null)
                                {
                                    payloadUserFields.Add("estado", jObject3["estado"].ToString());
                                }



                                if (payloadUserFields != null && payloadUserFields.Count > 0)
                                {
                                    payload.Add("user_fields", payloadUserFields);
                                }


                            }


                            bSessionId = true;
                            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                            int timestamp = (int)t.TotalSeconds;
                            payload.Add("iat", timestamp);
                            payload.Add("jti", System.Guid.NewGuid().ToString());
                            var json = new JavaScriptSerializer().Serialize(payload);

                            Logger_AddLogMessage(string.Format("Payload json={0}", json), LogLevels.logINFO);


                            string token = JWT.JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["SharedMobileSDKKey"], JWT.JwtHashAlgorithm.HS256);

                            Logger_AddLogMessage(string.Format("Payload token={0}", token), LogLevels.logINFO);
                            Response.ContentType = "application/json";
                            Response.ContentEncoding = System.Text.Encoding.UTF8;
                            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                            var jsonResponseDict = new Dictionary<string, object>() { { "jwt", token } };

                            string strResponse = new JavaScriptSerializer().Serialize(jsonResponseDict);
                            Logger_AddLogMessage(string.Format("JWT Mobile Payload => {0}; Response Generated = {1}", json, strResponse), LogLevels.logINFO);
                            Response.Write(strResponse);
                            break;
                        }
                    }
                }
                
            }
            if (bSessionId)
            {
                Response.Flush();
            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                Logger_AddLogMessage(string.Format("Zendesk SSO Mobile user_token = {0} UNAUTHORIZED!!!!!", strToken), LogLevels.logINFO);
            }
          
        }

        private static string CallToServerTokenZendesk(string jsonIn, string jsonOut, COUNTRIES_REDIRECTION countriesRedirections)
        {
            integraMobileWS.integraMobileWS oExternalIntegraMobileWS = StarExternalIntegraMobileWS(countriesRedirections);
            if (oExternalIntegraMobileWS != null)
            {
                try
                {
                    jsonOut = oExternalIntegraMobileWS.QueryTokenZendeskJSON(jsonIn);
                }
                catch (Exception)
                {
                    //xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                    Logger_AddLogMessage(string.Format("CallToServerSignUpStep2::Error: jsonIn={0}, jsonOut={1}", jsonIn, jsonOut), LogLevels.logERROR);
                }
            }
            return jsonOut;
        }

        private static integraMobileWS.integraMobileWS StarExternalIntegraMobileWS(COUNTRIES_REDIRECTION countriesRedirections)
        {
            integraMobileWS.integraMobileWS oIntegraMobileWS = null;
            if (!string.IsNullOrEmpty(countriesRedirections.COURE_COUNTRY_REDIRECTION_HTTP_USER) && !string.IsNullOrEmpty(countriesRedirections.COURE_COUNTRY_REDIRECTION_PASSWORD))
            {
                oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                int iwsurl = countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL.IndexOf('/', countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1);
                string ws_url = string.Empty;
                if (iwsurl != -1)
                {
                    ws_url = countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL.Remove(countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1, 1);
                }
                else
                {
                    ws_url = countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL;
                }
                oIntegraMobileWS.Url = ws_url;//countriesRedirections.COURE_COUNTRY_REDIRECTION_WS_URL;
                oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(countriesRedirections.COURE_COUNTRY_REDIRECTION_HTTP_USER, countriesRedirections.COURE_COUNTRY_REDIRECTION_PASSWORD);
            }
            return oIntegraMobileWS;
        }
        
        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }
    }

}