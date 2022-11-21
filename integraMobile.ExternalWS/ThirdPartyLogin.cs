using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Dynamic;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain.Abstract;
using Ninject;
using Newtonsoft.Json;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace integraMobile.ExternalWS
{
   
    public class ThirdPartyLogin : ThirdPartyBase
    {
        
        public ThirdPartyLogin() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyUser));
        }

        public ResultType BlinkaySuiteAppSecurityAuthentication(string sUrl, string sHashKey, string sHttpUser, string sHttpPwd, string sProv, string sUsername, string sPwd, 
                                                                ref SortedList parametersOut, ref List<INSTALLATION> oInstallations, ref Dictionary<decimal, List<string>> oInsRoles)
        {
            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
             

                PIC_WS.PIC_WS oPICWs = new PIC_WS.PIC_WS();
                oPICWs.Url = sUrl;
                oPICWs.Timeout = Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(sHttpUser))
                {
                    oPICWs.Credentials = new System.Net.NetworkCredential(sHttpUser, sHttpPwd);
                }


                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateStandardWSHash(sHashKey,
                                                      string.Format("{0}{1}{2}", 
                                                      sUsername, sPwd, sProv));

                strMessage = string.Format("<ipark_in><username>{0}</username><pwd>{1}</pwd><prov>{2}</prov><ah>{3}</ah></ipark_in>",
                                           sUsername, sPwd, sProv, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("BlinkaySuiteAppSecurityAuthentication xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = oPICWs.BlinkaySuiteAppSecurityAuthentication(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("BlinkaySuiteAppSecurityAuthentication xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                    parametersOut["r"] = Convert.ToInt32(rtRes);

                    if (rtRes == ResultType.Result_OK)
                    {
                        //parametersOut["roles"] = wsParameters
                        parametersOut["id"] = wsParameters["id"];
                        parametersOut["name"] = wsParameters["name"];
                        parametersOut["surname1"] = wsParameters["surname1"];
                        parametersOut["surname2"] = wsParameters["surname2"];
                        parametersOut["email"] = wsParameters["email"];
                        parametersOut["cou_code"] = wsParameters["cou_code"];
                        parametersOut["tel_cou_code"] = wsParameters["tel_cou_code"];
                        parametersOut["telephone"] = wsParameters["telephone"];
                        parametersOut["culture_lang"] = wsParameters["culture_lang"];
                        parametersOut["badgeNumber"] = wsParameters["badgeNumber"];

                        if (wsParameters["installations_ins_num"] != null && Convert.ToInt32(wsParameters["installations_ins_num"]) > 0 &&
                            wsParameters["roles_ins_roles_num"] != null && Convert.ToInt32(wsParameters["roles_ins_roles_num"]) == Convert.ToInt32(wsParameters["installations_ins_num"]))
                        {                                                        
                            Domain.INSTALLATION oIns = null;
                            string sKey = "";
                            int iKeyIndex = 0;
                            string sRole = "";
                            for (int iIndex = 0; iIndex < Convert.ToInt32(wsParameters["roles_ins_roles_num"]); iIndex++)
                            {
                                oIns = infraestructureRepository.Installations.Where(i => i.INS_STANDARD_CITY_ID == wsParameters[string.Format("installations_ins_{0}_insId", iIndex)].ToString()).FirstOrDefault();
                                if (oIns != null)
                                {
                                    oInstallations.Add(oIns);
                                    if (!oInsRoles.ContainsKey(oIns.INS_ID))
                                        oInsRoles.Add(oIns.INS_ID, new List<string>());
                                    sKey = "roles_ins_roles_{0}_role";
                                    iKeyIndex = 0;
                                    while (wsParameters[string.Format(sKey, iIndex)] != null)
                                    {
                                        if (!string.IsNullOrEmpty(wsParameters[string.Format(sKey, iIndex)].ToString()))
                                        {
                                            sRole = wsParameters[string.Format(sKey, iIndex)].ToString();
                                            if (!oInsRoles[oIns.INS_ID].Contains(sRole))
                                                oInsRoles[oIns.INS_ID].Add(sRole);
                                        }
                                        iKeyIndex += 1;
                                        sKey = "roles_ins_roles_{0}_role_" + iKeyIndex.ToString();
                                    }
                                }
                            }
                        }
                    }

                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BlinkaySuiteAppSecurityAuthentication::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

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
    }
}
