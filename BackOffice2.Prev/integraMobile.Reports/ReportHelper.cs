using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using PIC.Infrastructure.Logging;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Security;

namespace integraMobile.Reports
{
    public static class ReportHelper
    {
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(ReportHelper));

        public static string CurrentPlugin = "";

        public static bool ApplyResources(this Telerik.Reporting.Report oReport)
        {
            bool bRet = false;
            m_oLog.LogMessage(LogLevels.logDEBUG, "ApplyResources", ">>");

            try
            {
                ResourceBundle resBundle = ResourceBundle.GetInstance();
                if (resBundle != null)
                {
                    resBundle.LocaleRoot = "integraMobile.Reports.Locale";
                    resBundle.AddResourceFile(oReport.Name);
                    if (!string.IsNullOrEmpty(CurrentPlugin))
                        resBundle.Locale = ResourceBundle.GetInstance(CurrentPlugin).Locale;
                    else
                        resBundle.Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

                    //this.textBox1.Value = resBundle.GetString("Finantial", this.textBox1.Name + ".Value");
                    foreach (FieldInfo oField in oReport.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                    {
                        if (oField.FieldType.IsSubclassOf(typeof(ReportItemBase)))
                        {
                            ReportItemBase oItem = (ReportItemBase)oField.GetValue(oReport);
                            if (oItem.GetType().GetProperty("Value") != null && oItem.GetType().GetProperty("Value").PropertyType == typeof(string))
                            {
                                try
                                {
                                    string sLiteral = resBundle.GetString(oReport.Name, string.Format("{0}.Value", oItem.Name), (string)oItem.GetType().GetProperty("Value").GetValue(oItem, null));
                                    oItem.GetType().GetProperty("Value").SetValue(oItem, sLiteral);
                                }
                                catch (Exception ex)
                                {
                                    m_oLog.LogMessage(LogLevels.logWARN, "ApplyResources", string.Format("Resources value for '{0}.{1}.Value' couldn't be loaded", oReport.Name, oItem.Name), ex);
                                }
                            }
                        }
                    }

                    bRet = true;
                }
                else
                    m_oLog.LogMessage(LogLevels.logWARN, "ApplyResources", "Resources bundle couldn't be loaded");
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ApplyResources", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "ApplyResources", "<<");

            return bRet;
        }

        public static bool ApplyCurrency(this Telerik.Reporting.Report oReport, string sCurrencyISOCode)
        {
            bool bRet = false;
            m_oLog.LogMessage(LogLevels.logDEBUG, "ApplyCurrency", ">>");

            try
            {
                var oCultures = Helper.CultureInfoFromCurrencyISO(sCurrencyISOCode);
                if (oCultures.Count > 0)
                {
                    oReport.Culture = oCultures[0];
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ApplyCurrency", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "ApplyCurrency", "<<");

            return bRet;
        }

        public static bool ConfigureInstallationsDatasource(this Telerik.Reporting.Report oReport, SqlDataSource oInstallationsDataSource, bool bIncludeRechargesInstallation = true)
        {
            bool bRet = false;
            m_oLog.LogMessage(LogLevels.logDEBUG, "ConfigureInstallationsDatasource", ">>");

            try
            {

                if (FormAuthMemberShip.HelperService != null)
                {
                    string sRechargesInstallation = "*** Instalación recargas ***";
                    ResourceBundle resBundle = ResourceBundle.GetInstance();
                    if (resBundle != null)
                    {
                        resBundle.LocaleRoot = "integraMobile.Reports.Locale";
                        resBundle.AddResourceFile(oReport.Name);
                        if (!string.IsNullOrEmpty(CurrentPlugin))
                            resBundle.Locale = ResourceBundle.GetInstance(CurrentPlugin).Locale;
                        else
                            resBundle.Locale = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                        sRechargesInstallation = resBundle.GetString(oReport.Name, "Installation.Recharge.Name", sRechargesInstallation);
                    }

                    string sInstallationIds = "";
                    if (FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports." + oReport.Name))
                    {
                        var oInstallations = FormAuthMemberShip.HelperService.InstallationsFeatureAllowed("FinantialReports." + oReport.Name, AccessLevel.Read);
                        foreach (var item in oInstallations)
                        {
                            sInstallationIds += "," + item.ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(sInstallationIds))
                    {
                        if (bIncludeRechargesInstallation)
                            sInstallationIds = "0" + sInstallationIds;
                        else
                            sInstallationIds = sInstallationIds.Substring(1);
                    }
                    else
                        sInstallationIds = "-1";
                    oInstallationsDataSource.SelectCommand = "SELECT INS_ID, INS_DESCRIPTION\r\nFROM INSTALLATIONS\r\nWHERE INS_ENABLED = 1 AND INS_ID IN (" + sInstallationIds + ")\r\n";
                    if (bIncludeRechargesInstallation)
                    {
                        oInstallationsDataSource.SelectCommand += "UNION\r\n" +
                                                                  "SELECT 0, \'" + sRechargesInstallation + "\' WHERE 0 IN (" + sInstallationIds + ")";

                    }
                }
                bRet = true;
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ConfigureInstallationsDatasource", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "ConfigureInstallationsDatasource", "<<");

            return bRet;
        }

        public static object GetMonthName(Int32 iMonth)
        {
            string sRet = iMonth.ToString();
            m_oLog.LogMessage(LogLevels.logDEBUG, "GetMonthName", ">>");

            try
            {
                string sLocale = "";
                if (!string.IsNullOrEmpty(CurrentPlugin))
                    sLocale = ResourceBundle.GetInstance(CurrentPlugin).Locale;
                else
                    sLocale = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                var oCultureInfo = System.Globalization.CultureInfo.GetCultureInfo(sLocale);
                if (oCultureInfo != null)
                    sRet = oCultureInfo.DateTimeFormat.GetMonthName(iMonth);

            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "GetMonthName", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "GetMonthName", "<<");

            return sRet;
        }

        public static int GetMonthMinutes(Int32 iYear, Int32 iMonth)
        {
            int iRet = 0;
            m_oLog.LogMessage(LogLevels.logDEBUG, "GetMonthMinutes", ">>");

            try
            {
                DateTime dtIni = new DateTime(iYear, iMonth, 1);
                DateTime dtEnd = dtIni.AddMonths(1);

                TimeSpan oDiff = dtEnd.Subtract(dtIni);
                iRet = Convert.ToInt32(oDiff.TotalMinutes);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "GetMonthMinutes", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "GetMonthMinutes", "<<");

            return iRet;
        }

        public static bool ReportAccess(this Telerik.Reporting.Report oReport)
        {
            bool bRet = false;
            m_oLog.LogMessage(LogLevels.logDEBUG, "ReportAccess", ">>");

            try
            {
                if (FormAuthMemberShip.HelperService != null)
                {
                    bRet = FormAuthMemberShip.HelperService.FeatureReadAllowed("FinantialReports." + oReport.Name);
                }
                else
                    bRet = true;
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ReportAccess", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "ReportAccess", "<<");

            return bRet;
        }

        public static bool ReportAccess(this Telerik.Reporting.Report oReport, string sFeatureName)
        {
            bool bRet = false;
            m_oLog.LogMessage(LogLevels.logDEBUG, "ReportAccess", ">>");

            try
            {
                if (FormAuthMemberShip.HelperService != null)
                {
                    bRet = FormAuthMemberShip.HelperService.FeatureReadAllowed(sFeatureName);
                }
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ReportAccess", "Exception", ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, "ReportAccess", "<<");

            return bRet;
        }

    }
}
