using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.Reports.Invoicing
{
    public static class InvoiceHelper
    {
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(InvoiceHelper));

        public static bool ApplyResources(this Telerik.Reporting.Report oReport)
        {
            return true;
        }

        public static bool ApplyCurrency(this Telerik.Reporting.Report oReport, string sCurrencyISOCode)
        {
            bool bRet = false;            

            try
            {
                var oCultures = CCurrencyConvertor.CultureInfoFromCurrencyISO(sCurrencyISOCode);
                if (oCultures.Count > 0)
                {
                    oReport.Culture = oCultures[0];
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ApplyCurrency: ", ex);
            }

            return bRet;
        }

    }
}