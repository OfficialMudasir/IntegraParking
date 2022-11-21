using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Runtime.Serialization;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Http;
using integraMobile.Domain.Abstract;
using integraMobile.Domain;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.OxxoWS.Models;
using Newtonsoft.Json;

namespace integraMobile.OxxoWS.Controllers
{

    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    [RoutePrefix("service")]
    public class ServiceController : ApiController
    {
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(ServiceController));

        public ICustomersRepository m_oCustomersRepository { get; set; }        
        public IInfraestructureRepository m_oInfraestructureRepository { get; set; }        
        public IGeograficAndTariffsRepository m_oGeograficAndTariffsRepository { get; set; }
        public IBackOfficeRepository m_oBackOfficeRepository { get; set; }

        public ServiceController(ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository, IGeograficAndTariffsRepository oGeograficAndTariffsRepository, IBackOfficeRepository oBackOfficeRepository)
        {
            this.m_oCustomersRepository = oCustomersRepository;
            this.m_oInfraestructureRepository = oInfraestructureRepository;
            this.m_oGeograficAndTariffsRepository = oGeograficAndTariffsRepository;
            this.m_oBackOfficeRepository = oBackOfficeRepository;
        }

        /*[Route("{token}/consulta")]
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Mvc.HttpGet]
        public IHttpActionResult Consulta(string token, string client)
        {
            var oQuery = new QueryOutput();
            oQuery.Query(client, m_oCustomersRepository, m_oInfraestructureRepository);
            return Content(System.Net.HttpStatusCode.OK, oQuery, Configuration.Formatters.XmlFormatter);
        }*/
        [Route("{token}/consulta")]
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Mvc.HttpGet]
        public HttpResponseMessage Consulta(string token, string client)
        {
            if (token!=ConfigurationManager.AppSettings["Token"])
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Token");

            var res = Request.CreateResponse(HttpStatusCode.OK);
            if ((ConfigurationManager.AppSettings["IsProxy"] != null)
                 &&(ConfigurationManager.AppSettings["ProxyURL"] != null)
                 && (ConfigurationManager.AppSettings["IsProxy"] == "1"))
            {
                string strURL = string.Format("{0}/service/{1}/Consulta?client={2}", ConfigurationManager.AppSettings["ProxyURL"], token,client);

                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Query::QueryInput Client={0}", client));

                
                string strResponse=SendGetRequest(strURL);

                if (!string.IsNullOrEmpty(strResponse))
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Query::QueryOutput={0}", ModelBase.PrettyXml(strResponse)));
                    res.Content = new StringContent(strResponse, Encoding.UTF8, "text/xml");        
                }
                else
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Query::Error"));
                    res = Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                
            }
            else
            {
                var oQuery = new QueryOutput();
                oQuery.Query(client, m_oCustomersRepository, m_oInfraestructureRepository);                
                res.Content = new StringContent(ModelBase.SerializeToString(typeof(QueryOutput), oQuery), Encoding.UTF8, "text/xml");                
            }

            return res;
        }

        /*[Route("{token}/pay")]
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Mvc.HttpPost]
        public PayOutput Pay(string token, [FromBody] PayInput payInput)
        {
            return payInput.Pay(m_oCustomersRepository);
        }*/
        [Route("{token}/pay")]
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Pay(string token, [FromBody] PayInput payInput)
        {
            if (token != ConfigurationManager.AppSettings["Token"])
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Token");


            var res = Request.CreateResponse(HttpStatusCode.OK);
            if ((ConfigurationManager.AppSettings["IsProxy"] != null)
                && (ConfigurationManager.AppSettings["ProxyURL"] != null)
                && (ConfigurationManager.AppSettings["IsProxy"] == "1"))
            {
                string strURL = string.Format("{0}/service/{1}/Pay", ConfigurationManager.AppSettings["ProxyURL"],token);
                string strXMLInput = ModelBase.SerializeToString(typeof(PayInput), payInput);
                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Pay::PayInput={0}", ModelBase.PrettyXml(strXMLInput)));
                string strResponse = SendPostRequest(strXMLInput,strURL);

                if (!string.IsNullOrEmpty(strResponse))
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Pay::PayOutput={0}", ModelBase.PrettyXml(strResponse)));
                    res.Content = new StringContent(strResponse, Encoding.UTF8, "text/xml");
                }
                else
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Pay::Error"));
                    res = Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

            }
            else
            {
                var oPayOuput = payInput.Pay(m_oCustomersRepository);
                res.Content = new StringContent(ModelBase.SerializeToString(typeof(PayOutput), oPayOuput), Encoding.UTF8, "text/xml");
            }
                      
            return res;
        }

        /*[Route("{token}/reverse")]
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Mvc.HttpPost]
        public ReverseOutput Reverse(string token, [FromBody] ReverseInput reverseInput)
        {
            return reverseInput.Reverse(m_oCustomersRepository, m_oBackOfficeRepository, m_oInfraestructureRepository);
        }*/
        [Route("{token}/reverse")]
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Reverse(string token, [FromBody] ReverseInput reverseInput)
        {
            if (token != ConfigurationManager.AppSettings["Token"])
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Token");

            var res = Request.CreateResponse(HttpStatusCode.OK);
            if ((ConfigurationManager.AppSettings["IsProxy"] != null)
                && (ConfigurationManager.AppSettings["ProxyURL"] != null)
                && (ConfigurationManager.AppSettings["IsProxy"] == "1"))
            {
                string strURL = string.Format("{0}/service/{1}/Reverse", ConfigurationManager.AppSettings["ProxyURL"],token);
                string strXMLInput = ModelBase.SerializeToString(typeof(ReverseInput), reverseInput);
                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Reverse::ReverseInput={0}", ModelBase.PrettyXml(strXMLInput)));                
                string strResponse = SendPostRequest(strXMLInput, strURL);


                if (!string.IsNullOrEmpty(strResponse))
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Reverse::ReverseOutput={0}", ModelBase.PrettyXml(strResponse)));
                    res.Content = new StringContent(strResponse, Encoding.UTF8, "text/xml");
                }
                else
                {
                    m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("Reverse::Error"));
                    res = Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

            }
            else
            {
                var oReverseOutput = reverseInput.Reverse(m_oCustomersRepository, m_oBackOfficeRepository, m_oInfraestructureRepository);
                res.Content = new StringContent(ModelBase.SerializeToString(typeof(ReverseOutput), oReverseOutput), Encoding.UTF8, "text/xml");
            }
            return res;
        }


        private static string SendGetRequest(string url)
        {
            string strResult = "";

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpRequest.Method = "GET";

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    httpRequest.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                WebResponse response = httpRequest.GetResponse();
                // Display the status.
                HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                if (oWebResponse.StatusDescription == "OK")
                {
                    // Get the stream containing content returned by the server.
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    strResult = reader.ReadToEnd();
                    // Display the content.
                   
                    reader.Close();
                    dataStream.Close();
                }

                response.Close();

            }
            catch (Exception e)
            {
                m_oLog.LogMessage(LogLevels.logERROR, string.Format("SendGetRequest: Error {0}", e.Message));
            }

            return strResult;
        }


        private static string SendPostRequest(string data, string url)
        {
            string strResult = "";
            try
            {
                //Data parameter Example
                //string data = "name=" + value
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = data.Length;

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    httpRequest.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(data);
                streamWriter.Close();

                WebResponse response = httpRequest.GetResponse();
                // Display the status.
                HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                if (oWebResponse.StatusDescription == "OK")
                {
                    // Get the stream containing content returned by the server.
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    strResult = reader.ReadToEnd();
                    // Display the content.

                    reader.Close();
                    dataStream.Close();
                }

                response.Close();
            }
            catch (Exception e)
            {
                m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("SendPostRequest: Error {0}", e.Message));
            }

            return strResult;
        }


        private static string GetWSHttpUser()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSHttpUser"].ToString();
            }
            catch { }
            return sRet;
        }
        private static string GetWSHttpPassword()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSHttpPassword"].ToString();
            }
            catch { }
            return sRet;
        }

    }    
}
