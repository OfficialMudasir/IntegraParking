using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace integraMobile.Validation
{
    public class ValidateCaptchaAttribute : ActionFilterAttribute
    {
        string CHALLENGE_FIELD_KEY = "recaptcha_challenge_field";
        string RESPONSE_FIELD_KEY = "recaptcha_response_field";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Form[CHALLENGE_FIELD_KEY] == null)
            {
                filterContext.ActionParameters["CaptchaIsValid"] = true;
                return;
            }

            var captchaChallengeValue = filterContext.HttpContext.Request.Form[CHALLENGE_FIELD_KEY];
            var captchaResponseValue = filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY];

            string strPrivateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];

            var captchaValidator = new Recaptcha.RecaptchaValidator
            {
                PrivateKey = strPrivateKey,
                RemoteIP = filterContext.HttpContext.Request.UserHostAddress,
                Challenge = captchaChallengeValue,
                Response = captchaResponseValue
            };

            var recaptchaResponse = captchaValidator.Validate();
            filterContext.ActionParameters["CaptchaIsValid"] = recaptchaResponse.IsValid;


            base.OnActionExecuting(filterContext);
        }
    }
}