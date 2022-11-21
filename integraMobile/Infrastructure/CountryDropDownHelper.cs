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
    public class CountryDropDownHelper
    {
        public static MvcHtmlString CountryDropDown(Array arrCountries,string strFlagPath, string strSelectedOption)
        {
            MvcHtmlString optionsHtml;
            StringBuilder sbHtml = new StringBuilder();
           
            string strDefaultCountryCode = ConfigurationManager.AppSettings["DefaultCountryCode"];

            foreach (COUNTRy Country in arrCountries)
            {
                if (((!String.IsNullOrEmpty(strSelectedOption)) &&(strSelectedOption == Country.COU_ID.ToString())) || 
                    ((String.IsNullOrEmpty(strSelectedOption)) && (Country.COU_CODE == strDefaultCountryCode)))
                    sbHtml.AppendFormat("<option value=\"{0}\" title=\"{3}{1}.GIF\" SELECTED>{2}</option>\n",
                          Country.COU_ID, Country.COU_CODE, Country.COU_DESCRIPTION, strFlagPath);
                else
                    sbHtml.AppendFormat("<option value=\"{0}\" title=\"{3}{1}.GIF\">{2}</option>\n",
                        Country.COU_ID, Country.COU_CODE, Country.COU_DESCRIPTION, strFlagPath);

            }
            optionsHtml = MvcHtmlString.Create(sbHtml.ToString());
           
            return optionsHtml;
        }
    }
}