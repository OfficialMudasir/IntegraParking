using integraMobile.Infrastructure.Logging.Tools;
using System.Configuration;

namespace integraMobile.Infrastructure
{
    public static class ConfigurationModel
    {
        #region Constructor
        private static readonly string TEXT_END_POINT = "PAYPAL_API_ENDPOINT";
        private static readonly string TEXT_PAYPAL_API_ENVIRONMENT = "PAYPAL_API_ENVIRONMENT";
        //private static readonly string TEXT_CLIENT_ID = "PAYPAL_API_CLIENT_ID";
        //private static readonly string TEXT_CLIENT_SECRET = "PAYPAL_API_CLIENT_SECRET";
        private static readonly string TEXT_CONNECTION_TIME_OUT = "PAYPAL_CONNECTION_TIMEOUT";
        private static readonly string TEXT_REQUEST_RETRIES = "PAYPAL_REQUEST_RETRIES";
        #endregion

        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(ConfigurationModel));

        private const string DEFAULT_CONNECTION_TIME_OUT = "5000";
        private const string DEFAULT_REQUEST_RETRIES = "2";

        public static string PaypalApiEndPoint
        {
            get
            {
                if (CheckWebConfig(TEXT_END_POINT))
                {
                    return ConfigurationManager.AppSettings[TEXT_END_POINT];
                }
                return string.Empty;
            }
        }

        //public static string PaypalApiClientId
        //{
        //    get
        //    {
        //        if (CheckWebConfig(TEXT_CLIENT_ID))
        //        {
        //            return ConfigurationManager.AppSettings[TEXT_CLIENT_ID];
        //        }
        //        return string.Empty;
        //    }
        //}

        //public static string PaypalApiClientSecret
        //{
        //    get
        //    {
        //        if (CheckWebConfig(TEXT_CLIENT_SECRET))
        //        {
        //            return ConfigurationManager.AppSettings[TEXT_CLIENT_SECRET];
        //        }
        //        return string.Empty;
        //    }
        //}

        public static string PaypalApiEnvironment
        {
            get
            {
                if (CheckWebConfig(TEXT_PAYPAL_API_ENVIRONMENT))
                {
                    return ConfigurationManager.AppSettings[TEXT_PAYPAL_API_ENVIRONMENT];
                }
                return string.Empty;
            }
        }

        public static string PaypalConnectionTimeout
        {
            get
            {
                if (CheckWebConfig(TEXT_CONNECTION_TIME_OUT))
                {
                    return ConfigurationManager.AppSettings[TEXT_CONNECTION_TIME_OUT];
                }
                else
                {
                    return DEFAULT_CONNECTION_TIME_OUT;
                }
            }
        }

        public static string PaypalRequestRetries
        {
            get
            {
                if (CheckWebConfig(TEXT_REQUEST_RETRIES))
                {
                    return ConfigurationManager.AppSettings[TEXT_REQUEST_RETRIES];
                }
                else
                {
                    return DEFAULT_REQUEST_RETRIES;
                }
            }
        }
        #endregion

        private static bool CheckWebConfig(string cadenaConfig)
        {
            bool bCheck = true;

            if (!string.IsNullOrEmpty(cadenaConfig) && string.IsNullOrEmpty(ConfigurationManager.AppSettings[cadenaConfig]))
            {
                m_Log.LogMessage(LogLevels.logERROR, "ConfigurationModel::CheckWebConfig::Missing in the webconfig: " + cadenaConfig);
                bCheck = false;
            }
            return bCheck;
        }
    }
}
