using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Ninject;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.ExternalWS
{
    public class NotificationsFactory
    {
        private static CLogWrapper m_Log = new CLogWrapper(typeof(NotificationsFactory));



        public static Notifications CreateNotifications()
        {
            Notifications oRet = null;

            try
            {
                string sConfigFile = "";
                try
                {
                    sConfigFile = Convert.ToString(ConfigurationManager.AppSettings["ErrorNoficationsConfig"]);
                }
                catch { }

                //m_Log.LogMessage(LogLevels.logINFO, string.Format("CreateNotifications::Config Path: {0}", sConfigFile));

                if (File.Exists(sConfigFile))
                {
                    oRet = (Notifications)Conversions.JsonDeserializeFromFile(sConfigFile, typeof(Notifications));
                }

                if (oRet == null)
                {
                    oRet = new Notifications();

                    MethodNotifications _method = null;

                    Type[] classTypes = { typeof(ThirdPartyOperation), typeof(ThirdPartyFine) };
                    foreach (Type oClassType in classTypes)
                    {
                        MethodInfo[] methodsInfo = oClassType.GetMethods();
                        foreach (MethodInfo oMethodInfo in methodsInfo)
                        {
                            if (oMethodInfo.IsPublic && oMethodInfo.DeclaringType.Name == oClassType.Name)
                            {
                                _method = new MethodNotifications();
                                _method.ClassName = oMethodInfo.DeclaringType.Name;
                                _method.MethodName = oMethodInfo.Name;
                                _method.MailsList.Add("noreply@integraparking.com");
                                _method.ErrorTypesList.Add("Result_Error_Generic");
                                oRet.MethodsList.Add(_method);
                            }
                        }
                    }

                    Conversions.JsonSerializeToFile(oRet, sConfigFile);
                }

                
                /*string jsonNotifications = "";
                if (ConfigurationManager.AppSettings["Notifications"] != null)
                    jsonNotifications = ConfigurationManager.AppSettings["Notifications"].ToString();

                if (!string.IsNullOrWhiteSpace(jsonNotifications))
                    oRet = (Notifications)Conversions.JsonDeserializeFromString(jsonNotifications, typeof(Notifications));
                else
                {

                    MethodNotifications _method = null;

                    Type[] classTypes = { typeof(ThirdPartyOperation), typeof(ThirdPartyFine) };
                    foreach (Type oClassType in classTypes)
                    {
                        MethodInfo[] methodsInfo = oClassType.GetMethods();
                        foreach (MethodInfo oMethodInfo in methodsInfo)
                        {
                            if (oMethodInfo.IsPublic && oMethodInfo.DeclaringType.Name == oClassType.Name)
                            {
                                _method = new MethodNotifications();
                                _method.ClassName = oMethodInfo.DeclaringType.Name;
                                _method.MethodName = oMethodInfo.Name;
                                _method.MailsList.Add("hbusque@integraparking.com");
                                _method.ErrorTypesList.Add("Result_Error_Generic");
                                oRet.MethodsList.Add(_method);
                            }
                        }
                    }
                */
                    /*Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    jsonNotifications = Conversions.JsonSerializeToString(oRet);
                    if (ConfigurationManager.AppSettings["Notifications"] == null)
                        config.AppSettings.Settings.Add("Notifications", jsonNotifications);
                    else
                        config.AppSettings.Settings["Notifications"].Value = jsonNotifications;
                    config.Save(ConfigurationSaveMode.Full);*/

                /*}*/
            }
            catch (Exception e)
            {                
                m_Log.LogMessage(LogLevels.logERROR, "CreateNotifications::Exception", e);
            }

            return oRet;
        }
    }

    [Serializable]
    public class Notifications
    {
        private static CLogWrapper m_Log = new CLogWrapper(typeof(Notifications));

        private List<MethodNotifications> _methods = new List<MethodNotifications>();

        [Inject]
        [JsonIgnore]
        public IInfraestructureRepository infraestructureRepository { get; set; }

        private IKernel m_kernel = null;
        
        [JsonIgnore]
        public List<MethodNotifications> MethodsList
        {
            get { return _methods; }
            set { _methods = value; }
        }
        public MethodNotifications[] Methods
        {
            get { return _methods.ToArray(); }
            set { _methods = (value != null ? value.ToList() : new List<MethodNotifications>()); }
        }

        public Notifications()
        {
            m_kernel = new StandardKernel(new integraMobileThirdPartyConfirmModule());
            m_kernel.Inject(this);
        }

        public void Notificate(MethodInfo oMethodInfo, ResultType resultType, string sParamsIn, string sParamsOut, bool bXmlParams, Exception oNotificationEx = null)
        {
            try
            {
                foreach (MethodNotifications oMethod in _methods)
                {
                    if (oMethod.ClassName == oMethodInfo.DeclaringType.Name && oMethod.MethodName == oMethodInfo.Name)
                    {
                        foreach (string sErrorType in oMethod.ErrorTypesList)
                        {
                            if (sErrorType == Enum.GetName(typeof(ResultType), resultType) || oNotificationEx != null)
                            {
                                string sSubject = string.Format("{0} ({1}) : ERROR", oMethod.MethodName, oMethod.MethodExternalName);
                                if (bXmlParams)
                                {
                                    sParamsIn = sParamsIn.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                                    sParamsOut= sParamsOut.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                                }
                                string sBody = "";
                                if (oNotificationEx == null) sBody = sErrorType + "<br><br>";
                                sBody += sParamsIn + "<br><br>";
                                if (!string.IsNullOrWhiteSpace(sParamsOut)) sBody += sParamsOut + "<br><br>";
                                if (oNotificationEx != null) sBody += oNotificationEx.Message + "<br>" + oNotificationEx.StackTrace;

                                infraestructureRepository.SendEmailToMultiRecipients(oMethod.MailsList, sSubject, sBody, null, Domain.integraSenderWS.EmailPriority.VeryLow);

                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "Notificate::Exception", e);
            }
        }
    }

    [Serializable]
    public class MethodNotifications
    {
        private string _className;
        private string _methodName;
        private string _methodExternalName;
        private List<string> _mails = new List<string>();
        private List<string> _errorTypes = new List<string>();

        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        public string MethodExternalName
        {
            get { return _methodExternalName; }
            set { _methodExternalName = value; }
        }

        [JsonIgnore]
        public List<string> MailsList
        {
            get { return _mails; }
            set { _mails = value; }
        }
        public string[] Mails
        {
            get { return _mails.ToArray(); }
            set { _mails = (value != null ? value.ToList() : new List<string>()); }
        }

        [JsonIgnore]
        public List<string> ErrorTypesList
        {
            get { return _errorTypes; }
            set { _errorTypes = value; }
        }
        public string[] ErrorTypes
        {
            get { return _errorTypes.ToArray(); }
            set { _errorTypes = (value != null ? value.ToList() : new List<string>()); }
        }

    }
}
