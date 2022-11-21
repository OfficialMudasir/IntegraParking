using integraMobile.Controllers;
using integraMobile.ExternalWS;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Properties;
using integraMobile.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace integraMobile.Helper
{
    public class Cookies
    {
        #region Methods Public

        /// <summary>
        /// For IPS-344
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            return (cookie != null ? cookie.Value : null);
        }

        public static void Delete(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

        /// <summary>
        /// For IPS-344
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, string value)
        {
            bool cookieNew = false;
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                cookieNew = true;
            }
            cookie.Domain = "blinkay.app";
            cookie.Path = "/";
            cookie.Secure = false;
            cookie.Value = value;

            cookie.Expires = DateTime.Now.AddMinutes(2);
            if (cookieNew)
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }

       #endregion
    }
}