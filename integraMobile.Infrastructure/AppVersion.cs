using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure
{
    public class AppUtilities
    {

        public static ulong AppVersion(string strAppVersion)
        {
            ulong ulRes = 0;
            try
            {
                if (!string.IsNullOrEmpty(strAppVersion))
                {

                    char[] cSep = new char[] { '.' };
                    string[] strVersComponents = strAppVersion.Split(cSep);
                    int i = 0;
                    ulong ulMultiplier = Convert.ToUInt64(1000 * 1000) * Convert.ToUInt64(1000 * 1000);
                    foreach (string strComponent in strVersComponents)
                    {
                        ulRes += Convert.ToUInt64(strComponent) * ulMultiplier;
                        ulMultiplier /= 1000;
                        i++;

                        if (i == 5) break;

                    }
                }

            }
            catch
            {
                ulRes = 0;
            }

            return ulRes;

        }
    }
}
