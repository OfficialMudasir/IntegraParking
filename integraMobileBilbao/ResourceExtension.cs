using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using System.Resources;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Properties;

namespace integraMobile.Web.Resources
{
    public class ResourceExtension
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(ResourceExtension));

        static SortedList<string, string> m_oLiteralsCache = new SortedList<string, string>();

        public static string GetLiteral(string strLiteral)
        {
            string strResourceString = "";
            string strCulture = "";
            try
            {
                try
                {
                    strCulture = Thread.CurrentThread.CurrentUICulture.Name;
                }
                catch
                {
                }

                if (string.IsNullOrEmpty(strCulture))
                {
                    strCulture = "es-ES";
                }

                try
                {
                    if (m_oLiteralsCache.ContainsKey(strCulture + "_" + strLiteral))
                    {
                        return m_oLiteralsCache[strCulture + "_" + strLiteral];
                    }

                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "", LogLevels.logERROR);
                }


                lock (m_oLiteralsCache)
                {
                    try
                    {
                        if (m_oLiteralsCache.ContainsKey(strCulture + "_" + strLiteral))
                        {
                            return m_oLiteralsCache[strCulture + "_" + strLiteral];
                        }

                        ResourceManager rm = integraMobile.Properties.Resources.ResourceManager;
                        strResourceString = rm.GetString(strLiteral);
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "", LogLevels.logERROR);
                    }


                    string strFile = "";

                    if (string.IsNullOrEmpty(strResourceString))
                    {
                        strFile = strLiteral + ".html";
                    }
                    else if (strResourceString.Contains("FILESYSTEM"))
                    {
                        strFile = strLiteral + ".html";

                        if (strResourceString != "FILESYSTEM")
                        {
                            string[] strResourceStrings = strResourceString.Split(new char[] { ':' });
                            if ((strResourceStrings.Count() == 2) && (strResourceStrings[0] == "FILESYSTEM"))
                            {
                                strCulture = strResourceStrings[1];
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(strFile))
                    {
                        strResourceString = "";
                        string strContentFolderRoot = "";

                        try
                        {
                            if (HttpContext.Current != null)
                            {
                                strContentFolderRoot = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "ResourceExt");
                            }
                            else
                            {
                                strContentFolderRoot = System.IO.Path.Combine((new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.FullName, "ResourceExt");
                            }
                        }
                        catch (Exception e)
                        {
                            Logger_AddLogException(e, "", LogLevels.logERROR);
                        }

                        string strResourceFile = System.IO.Path.Combine(strContentFolderRoot, strCulture, strFile);


                        if (File.Exists(strResourceFile))
                        {
                            strResourceString = File.ReadAllText(strResourceFile, System.Text.Encoding.UTF8);
                        }

                    }

                    m_oLiteralsCache[strCulture + "_" + strLiteral] = strResourceString;
                    Logger_AddLogMessage(string.Format("LITERAL {0} CACHED", strCulture + "_" + strLiteral), LogLevels.logINFO);

                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "", LogLevels.logERROR);
            }

            return strResourceString;
        }

        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }


    }
}
