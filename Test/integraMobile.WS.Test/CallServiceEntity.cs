using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.WS.Test
{
    public class CallServiceEntity
    {

        private integraMobileWS.integraMobileWS Start()
        {
            integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
            oIntegraMobileWS.Url = ConfigurationManager.AppSettings["WSUrl"];
            oIntegraMobileWS.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WSHttpUser"]))
            {
                oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["WSHttpUser"], ConfigurationManager.AppSettings["WSHttpPassword"]);
            }
        }

        private string CallServiceIntegraMobileWS(string methods, string sInJson, COUNTRIES_REDIRECTION countriesRedirection)
        {
            string jsonOut = string.Empty;
            try
            {
                integraMobileWS.integraMobileWS oIntegraMobileWS = Start(countriesRedirection);
                switch (methods)
                {
                    case Tools.METHODS_SIGNUP_STEP1_JSON:
                        jsonOut = oIntegraMobileWS.SignUpStep1JSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_SIGNUP_STEP2_JSON:
                        jsonOut = oIntegraMobileWS.SignUpStep2JSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_QUERY_LOGIN_CITY_JSON:
                        jsonOut = oIntegraMobileWS.QueryLoginCityJSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_VERIFY_LOGIN_EXISTS_JSON:
                        jsonOut = oIntegraMobileWS.VerifyLoginExistsJSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return jsonOut;
        }

          
    }
}
