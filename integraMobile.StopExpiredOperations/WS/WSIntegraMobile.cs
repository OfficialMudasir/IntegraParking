using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Linq;
using System.Web;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.StopExpiredOperations.WS
{
    public class WSIntegraMobile : WSBase
    {

        public WSIntegraMobile()
            : base()
        {
            m_Log = new CLogWrapper(typeof(WSIntegraMobile));
        }


        public ResultType StopExpiredOperations(int iNumMaxOperations, out int iStoppedOperationsCount)
        {            
            ResultType rtRes = ResultType.Result_OK;
            iStoppedOperationsCount = 0;

            string sXmlIn = "";
            string sXmlOut = "";            

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                DateTime dt = DateTime.UtcNow;

                strAuthHash = CalculateWSHash(string.Format("{0:HHmmssddMMyy}{1}{2}", dt, iNumMaxOperations, strvers));

                strMessage = string.Format("<ipark_in><d>{0:HHmmssddMMyy}</d><num_op>{1}</num_op><vers>{2}</vers><ah>{3}</ah></ipark_in>",
                                           dt, iNumMaxOperations, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("StopExpiredOperations xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.StopExpiredOperations(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("StopExpiredOperations xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {
                        iStoppedOperationsCount = Convert.ToInt32(wsParameters["num_stopped_op"]);
                    }

                }

            }
            catch (Exception e)
            {                
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "StopExpiredOperations::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

    }
}