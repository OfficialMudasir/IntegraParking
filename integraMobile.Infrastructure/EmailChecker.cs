using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using integraMobile.Infrastructure.ZeroBounce;

namespace integraMobile.Infrastructure
{
    

    public class EmailChecker
    {
        public enum EmailCheckResult
        {
            Valid = 1,
            Invalid = 2,
            Unknown = 3
        }

        protected const int DEFAULT_WS_TIMEOUT = 5000; //ms        

        static public EmailCheckResult Check(string strEmail)
        {
            EmailCheckResult eResult = EmailCheckResult.Unknown;

            try
            {

                ZeroBounceAPI oApi = new ZeroBounce.ZeroBounceAPI();

                oApi.ApiKey = ConfigurationManager.AppSettings["ZeroBounceAPIKey"].ToString();
                oApi.EmailToValidate = strEmail;
                oApi.ReadTimeOut = Get3rdPartyWSTimeout();
                oApi.RequestTimeOut = Get3rdPartyWSTimeout();
                ZeroBounceCreditsModel oResultCredits = oApi.GetCredits();

                if (Convert.ToInt32(oResultCredits.credits) > 0)
                {
                    ZeroBounceResultsModel oResult = oApi.ValidateEmail();

                    if (!string.IsNullOrEmpty(oResult.status))
                    {
                        switch (oResult.status)
                        {
                            case "Valid":
                                {
                                    if (!strEmail.Contains("unknown@integraparking.com"))
                                    {
                                        eResult = EmailCheckResult.Valid;
                                    }
                                }
                                break;
                            case "Invalid":
                            case "Spamtrap":
                            case "Abuse":
                            case "DoNotMail ":
                                eResult = EmailCheckResult.Invalid;
                                break;
                            case "Catch-All":
                            case "Unknown":
                            default:
                                eResult = EmailCheckResult.Unknown;
                                break;

                        }
                    }
                }
            }
            catch
            {
                
            }

            return eResult;
        }


        static protected int Get3rdPartyWSTimeout()
        {
            int iRes = DEFAULT_WS_TIMEOUT;
            try
            {
                iRes = Convert.ToInt32(ConfigurationManager.AppSettings["3rdPartyWSTimeout"].ToString());
            }
            catch
            {
                iRes = DEFAULT_WS_TIMEOUT;
            }

            return iRes;

        }

    }
}
