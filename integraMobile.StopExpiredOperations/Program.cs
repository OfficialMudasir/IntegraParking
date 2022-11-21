using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using integraMobile.StopExpiredOperations.WS;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.StopExpiredOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            var m_oLog = new CLogWrapper(typeof(Program));

            m_oLog.LogMessage(LogLevels.logINFO, "Main::Starting...");

            int iMaxOperations = Convert.ToInt32(ConfigurationManager.AppSettings["BlockMaxOperations"].ToString());

            WSIntegraMobile oWS = new WSIntegraMobile();

            ResultType rtRes = ResultType.Result_OK;
            int iStoppedOperations = iMaxOperations;

            while (rtRes == ResultType.Result_OK && iStoppedOperations == iMaxOperations)
            {
                rtRes = oWS.StopExpiredOperations(iMaxOperations, out iStoppedOperations);
            }

            m_oLog.LogMessage(LogLevels.logINFO, "Main::Finish");
        }
    }
}
