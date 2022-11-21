using integraMobile.Infrastructure.Logging.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace integraMobile.WS.Tools
{
    public class SettingsApp
    {
        #region Properties private
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(Helpers));
        #endregion

        #region Properties public
        public static String ServicePlatePath
        {
            get {    
                    if(String.IsNullOrEmpty(WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SERVICE_PLATE_PATH])) 
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("No exist web.Config={0}",  ConstantsEntity.SETTINGS_SERVICE_PLATE_PATH));
                        return string.Empty;
                    }
                    else
                    {
                        return WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SERVICE_PLATE_PATH]; 
                    }
                    
                }
        }

        public static String ServicePlateMaxRetry
        {
            get {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SERVICE_PLATE_MAX_RETRY])) 
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("No exist web.Config={0}", ConstantsEntity.SETTINGS_SERVICE_PLATE_MAX_RETRY));
                        return string.Empty;
                    }
                    else
                    {
                        return WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SERVICE_PLATE_MAX_RETRY]; 
                    }
                    
                }
        }

        public static String ShareMessage
        {
            get {
                if (String.IsNullOrEmpty(WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SHARE_MESSAGE])) 
                    {
                        m_Log.LogMessage(LogLevels.logERROR, string.Format("No exist web.Config={0}", ConstantsEntity.SETTINGS_SHARE_MESSAGE));
                        return string.Empty;
                    }
                    else
                    {
                        return WebConfigurationManager.AppSettings[ConstantsEntity.SETTINGS_SHARE_MESSAGE]; 
                    }
                    
                }
        }
        #endregion
    }
}