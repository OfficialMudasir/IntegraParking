using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;
using System.Configuration;
using integraMobile.Domain;

namespace integraMobile.Infrastructure
{
    public class InstallationDropDownHelper
    {
        public static MvcHtmlString InstallationDropDown(Array arrInstallations, string strFlagPath, string strSelectedOption)
        {
            MvcHtmlString optionsHtml;
            StringBuilder sbHtml = new StringBuilder();

            string strDefaultInstallationCode = ConfigurationManager.AppSettings["DefaultInstallationCode"];

            bool selected = false;

            foreach (INSTALLATION Installation in arrInstallations)
            {
                if (
                    (!String.IsNullOrEmpty(strSelectedOption) && strSelectedOption == Installation.INS_ID.ToString()) ||
                    (String.IsNullOrEmpty(strSelectedOption) && Installation.INS_ID.ToString() == strDefaultInstallationCode) ||
                    arrInstallations.Length == 1
                    )
                {
                    selected = true;
                    sbHtml.AppendFormat("<option value=\"{0}\" SELECTED>{1}</option>\n", Installation.INS_ID, Installation.INS_DESCRIPTION);
                }
                else
                {
                    sbHtml.AppendFormat("<option value=\"{0}\">{1}</option>\n", Installation.INS_ID, Installation.INS_DESCRIPTION);
                }
            }

            if (!selected) {
                sbHtml.Insert(0, "<option disabled selected></option>");
            }

            optionsHtml = MvcHtmlString.Create(sbHtml.ToString());

            return optionsHtml;
        }        
    }
}