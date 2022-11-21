using System;
using log4net;
using log4net.Config;


// Configure log4net using the .config file
[assembly: log4net.Config.DOMConfigurator(Watch = true)]

namespace integraMobile.Infrastructure.Logging.Tools
{
    public enum LogLevels
    {
        logDEBUG = 0,
        logINFO  = 1,
        logWARN  = 2,
        logERROR = 3,
        logFATAL = 4
    };

	/// <summary>
	/// Summary description for CLogWrapper.
	/// </summary>
	public class CLogWrapper
	{
        #region -- Member variables --
        
        // Define a static logger variable so that it references the Logger instance
        private static ILog m_log = null; 

        #endregion 

        #region -- Constructor / Destructor  --

        public CLogWrapper(System.Type objType)
		{
            try
            {
                m_log = LogManager.GetLogger(objType.Name);
            }
            catch
            {
                
            }
        }


        #endregion 

        #region -- Trace functions --

        /// <summary>
        /// Traces the specified message
        /// </summary>
        public void LogMessage(LogLevels nLevel, string strMessage)
        {
            switch (nLevel)
            {
                case LogLevels.logDEBUG: if (m_log.IsDebugEnabled) { m_log.Debug(strMessage); }		break;
                case LogLevels.logINFO:  if (m_log.IsInfoEnabled ) { m_log.Info(strMessage);  }		break;
                case LogLevels.logWARN:  if (m_log.IsWarnEnabled)  { m_log.Warn(strMessage);  }		break;
                case LogLevels.logERROR: if (m_log.IsErrorEnabled) { m_log.Error(strMessage); }		break;
                case LogLevels.logFATAL: if (m_log.IsFatalEnabled) { m_log.Fatal(strMessage); }		break;
                default: /* DEBUG */	 if (m_log.IsDebugEnabled) { m_log.Debug(strMessage); }		break;
            }
        }


        /// <summary>
        /// Traces the specified message and the exception information
        /// </summary>
        public void LogMessage(LogLevels nLevel, string strMessage, Exception excObject)
        {
            string strException = excObject.GetType().ToString();;

            strMessage = strException + " : " + strMessage;
            switch (nLevel)
            {
                case LogLevels.logDEBUG: if (m_log.IsDebugEnabled) { m_log.Debug(strMessage, excObject); }	break;
                case LogLevels.logINFO:  if (m_log.IsInfoEnabled ) { m_log.Info(strMessage,  excObject); }	break;
                case LogLevels.logWARN:  if (m_log.IsWarnEnabled)  { m_log.Warn(strMessage,  excObject); }	break;
                case LogLevels.logERROR: if (m_log.IsErrorEnabled) { m_log.Error(strMessage, excObject); }	break;
                case LogLevels.logFATAL: if (m_log.IsFatalEnabled) { m_log.Fatal(strMessage, excObject); }	break;
                default: /* DEBUG */	 if (m_log.IsDebugEnabled) { m_log.Debug(strMessage, excObject); }	break;
            }
        }

        #endregion     
    }

}
