using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.IO;
using System.Configuration;

namespace integraMobile.Infrastructure
{
    public class ReCaptchaHelper
    {
        public static MvcHtmlString ReCAPTCHA(string language)
        {
            MvcHtmlString captchaHtml;

            string strPrivateKey= ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            string strPublicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];


            using (Recaptcha.RecaptchaControl captchaControl = new Recaptcha.RecaptchaControl
            {
                ID = "ReCaptcha",
                Theme = "white",
                Language = language,
                PublicKey = strPublicKey,
                PrivateKey = strPrivateKey
            })
            {
                using (HtmlTextWriter htmlWriter = new HtmlTextWriter(new StringWriter()))
                {
                    captchaControl.RenderControl(htmlWriter);
                    captchaHtml = MvcHtmlString.Create(htmlWriter.InnerWriter.ToString());
                }
            }
            return captchaHtml;
        }
    }
}