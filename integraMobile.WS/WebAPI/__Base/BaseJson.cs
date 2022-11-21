using System;
using fastJSON;

namespace integraMobile.WS.WebAPI
{
    public class BaseJson
    {
        private static JSONParameters _jsonParameters;
        protected static JSONParameters JsonParameters
        {
            get
            {
                if (_jsonParameters != null)
                    return _jsonParameters;

                _jsonParameters = new JSONParameters
                {
                    InlineCircularReferences = true,
                    EnableAnonymousTypes = true,
                    UseFastGuid = true,
                    UseExtensions = false,
                    UsingGlobalTypes = false,
                    SerializeNullValues = false,
                    ShowReadOnlyProperties = true,
                    UseValuesOfEnums = true,
                    KVStyleStringDictionary = false
                };

                return _jsonParameters;
            }
        }

        public static DateTime? GetDateTimeFromStringFormat(string strDateTime)
        {
            DateTime? dt = null;
            try
            {
                if (!string.IsNullOrEmpty(strDateTime))
                {
                    if (strDateTime.Length == 12)
                        dt = DateTime.ParseExact(strDateTime, "HHmmssddMMyy",
                                                 System.Globalization.CultureInfo.InvariantCulture);
                    else if (strDateTime.Length == 17)
                        dt = DateTime.ParseExact(strDateTime, "yyyyMMddHHmmssfff",
                                                 System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                throw;
            }

            return dt;
        }

        public static string GetDateTimeStringFromDateFormat(DateTime? dtDateTime, string sFormat = "yyyyMMddHHmmssfff")
        {
            string sRet = null;
            if (dtDateTime.HasValue)
            {
                dtDateTime.Value.ToString(sFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            return sRet;
        }
    }
}
