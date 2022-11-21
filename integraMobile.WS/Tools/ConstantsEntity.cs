using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Tools
{
    public class ConstantsEntity
    {
        #region Constants
        /// <summary>
        /// {\"ipark_in\":
        /// </summary>
        public static readonly String TEXT_I_PARK_IN = "{\"ipark_in\":";
        /// <summary>
        /// {\"ipark_out\":
        /// </summary>
        public static readonly String TEXT_I_PARK_OUT = "{\"ipark_out\":";
        #endregion

        #region Constants Parameter

        /// <summary>
        /// "g"  --> Group Id
        /// </summary>
        public static readonly String PARAMETER_GROUP_ID = "g";
        /// <summary>
        /// "IdServiceType" 
        /// </summary>
        public static readonly String PARAMETER_ID_SERVICE_TYPE = "IdServiceType";
        /// <summary>
        /// "cityID" 
        /// </summary>
        public static readonly String PARAMETER_CITY_ID = "cityID";
        /// <summary>
        /// "license" 
        /// </summary>
        public static readonly String PARAMETER_LICENSE = "license";
        /// <summary>
        /// "u" --> User 
        /// </summary>
        public static readonly String PARAMETER_U = "u";
        /// <summary>
        /// "SessionID" 
        /// </summary>
        public static readonly String PARAMETER_SESSION_ID = "SessionID";
        /// <summary>
        /// "TypeOfServiceType"  
        /// </summary>
        public static readonly String PARAMETER_TYPE_OF_SERVICE_TYPE = "TypeOfServiceType";
        /// <summary>
        /// "appvers"
        /// </summary>
        public static readonly String PARAMETER_APP_VERS = "appvers";
        /// <summary>
        /// "vers"
        /// </summary>
        public static readonly String PARAMETER_VERS = "vers";
        /// <summary>
        /// "lang"
        /// </summary>
        public static readonly String PARAMETER_LANG = "lang";
        
        /// <summary>
        /// "tarVERSION"
        /// </summary>
        public static readonly String PARAMETER_TAR_VERSION = "tarVERSION";
        
        #endregion

        #region Constants Settings
        public static readonly String SETTINGS_SERVICE_PLATE_PATH = "ServicePlatePath";
        public static readonly String SETTINGS_SERVICE_PLATE_MAX_RETRY = "ServicePlateMaxRetry";
        public static readonly String SETTINGS_SHARE_MESSAGE = "sharemessage";
        #endregion
    }
}