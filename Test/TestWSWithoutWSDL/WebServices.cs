using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Web;
using System.IO;
using System.Xml.XPath;
using System.Collections;

namespace integraMobile.Infrastructure
{
    /// <summary>
    /// This class is an alternative when you can't use Service References. It allows you to invoke Web Methods on a given Web Service URL.
    /// Based on the code from http://stackoverflow.com/questions/9482773/web-service-without-adding-a-reference
    /// </summary>
    public class WebService
    {
        private SortedList m_outParameters = null;
        private const int DEFAULT_TIMEOUT = 60000;
        public string Url { get; private set; }
        public string Method { get; private set; }
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResponseSOAP = XDocument.Parse("<root/>");
        public XDocument ResultXML = XDocument.Parse("<root/>");
        public string ResultString = String.Empty;
        public int Timeout {get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public SortedList OutParameters { get { return m_outParameters; } }

        public WebService()
        {
            Url = String.Empty;
            Method = String.Empty;
            Timeout = DEFAULT_TIMEOUT;
        }
        public WebService(string baseUrl)
        {
            Url = baseUrl;
            Method = String.Empty;
            Timeout = DEFAULT_TIMEOUT;
        }
        public WebService(string baseUrl, string methodName)
        {
            Url = baseUrl;
            Method = methodName;
            Timeout = DEFAULT_TIMEOUT;
        }

        // Public API

        /// <summary>
        /// Adds a parameter to the WebMethod invocation.
        /// </summary>
        /// <param name="name">Name of the WebMethod parameter (case sensitive)</param>
        /// <param name="value">Value to pass to the paramenter</param>
        public void AddParameter(string name, string value)
        {
            Params.Add(name, value);
        }
      
        public void Invoke()
        {
            Invoke(Method, null, null, true);
        }

        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        public void Invoke(string methodName)
        {
            Invoke(methodName, null, null, true);
        }

        public void Invoke(string strMethodNameRequest, string strMethodNameResponse, string strResponseNameSpace)
        {
            Invoke(strMethodNameRequest, strMethodNameResponse, strResponseNameSpace, true);
        }


        /// <summary>
        /// Cleans all internal data used in the last invocation, except the WebService's URL.
        /// This avoids creating a new WebService object when the URL you want to use is the same.
        /// </summary>
        public void CleanLastInvoke()
        {
            ResponseSOAP = ResultXML = null;
            m_outParameters = null;
            ResultString = Method = String.Empty;
            Params = new Dictionary<string, string>();
        }



        public int GetOutputElementCount(string strPath)
        {
            return GetOutputElementCount(strPath, null);
        }

        public SortedList GetOutputElement(string strPath)
        {
            return GetOutputElement(strPath, null);
        }

        public string GetOutputStringElement(string strPath)
        {
            return GetOutputStringElement(strPath, null);
        }

        public ArrayList GetOutputElementArray(string strPath)
        {
            return GetOutputElementArray(strPath, null);
        }

        /// <summary>
        /// //
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="oSortedList"></param>
        /// <returns></returns>

        public int GetOutputElementCount(string strPath, SortedList oSortedList)
        {
            int iRes=0;

            string [] arrTags= strPath.Split(new char[]{'/'});
            SortedList oCurrSortedList=null;
            if (oSortedList==null)
                oCurrSortedList = OutParameters;
            else
                oCurrSortedList = oSortedList;
            int i=0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (i < arrTags.Count() - 1)
                        {
                            if (oArrParameters.Count == 1)
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                i++;
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }

                        }
                        else
                            iRes = oArrParameters.Count;
                    }
                    else
                    {
                        if (i == arrTags.Count() - 1)
                        {
                            iRes = 1;
                        }
                        break;
                    }
                      

                    i++;

                }
                else
                    break;
            }

            return iRes;
        }


        public SortedList GetOutputElement(string strPath, SortedList oSortedList)
        {
            SortedList oList = null;
            string strTemp = null;

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = null;
            if (oSortedList == null)
                oCurrSortedList = OutParameters;
            else
                oCurrSortedList = oSortedList;

            int i = 0;
            
            while (i<arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (oArrParameters.Count == 1)
                        {
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                strTemp = oArrParameters[0].ToString();
                            }
                        }
                        else
                        {
                            i++;
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }
                            else
                            {
                                strTemp = oArrParameters[Convert.ToInt32(arrTags[i])].ToString();
                            }
                        }
                    }
                    else
                    {
                        strTemp = oCurrSortedList[arrTags[i]].ToString();
                    }

                    i++;

                    if (!string.IsNullOrEmpty(strTemp))
                        break;

                    if (i >= arrTags.Count())
                        oList = oCurrSortedList;
                }
                else
                    break;
            }

            return oList;
        }

        public string GetOutputStringElement(string strPath, SortedList oSortedList)
        {
            string strRes = null;
            string strTemp = "";

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = null;
            if (oSortedList == null)
                oCurrSortedList = OutParameters;
            else
                oCurrSortedList = oSortedList;

            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (oArrParameters.Count == 1)
                        {
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                strTemp = oArrParameters[0].ToString();
                            }

                        }
                        else
                        {
                            i++;
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }
                            else
                            {
                                strTemp = oArrParameters[Convert.ToInt32(arrTags[i])].ToString();
                            }

                        }
                    }
                    else
                    {
                        strTemp = oCurrSortedList[arrTags[i]].ToString();
                    }

                    i++;

          
                    if (i >= arrTags.Count() && (!string.IsNullOrEmpty(strTemp)))
                        strRes = strTemp;
                }
                else
                    break;
            }

            return strRes;
        }

        public ArrayList GetOutputElementArray(string strPath, SortedList oSortedList)
        {
            ArrayList oArray = null;

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = null;
            if (oSortedList == null)
                oCurrSortedList = OutParameters;
            else
                oCurrSortedList = oSortedList;

            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                    if (i < arrTags.Count() - 1)
                    {
                        if (oArrParameters.Count == 1)
                        {
                            oCurrSortedList = (SortedList)oArrParameters[0];
                        }
                        else
                        {
                            i++;
                            oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                        }
                    }
                    else
                        oArray = oArrParameters;

                    i++;

                }
                else
                    break;
            }

            return oArray;
        }

        
        #region Helper Methods

        /// <summary>
        /// Checks if the WebService's URL and the WebMethod's name are valid. If not, throws ArgumentNullException.
        /// </summary>
        /// <param name="methodName">Web Method name (optional)</param>
        private void AssertCanInvoke(string methodName = "")
        {
            if (Url == String.Empty)
                throw new ArgumentNullException("You tried to invoke a webservice without specifying the WebService's URL.");
            if ((methodName == "") && (Method == String.Empty))
                throw new ArgumentNullException("You tried to invoke a webservice without specifying the WebMethod.");
        }

        private void ExtractResult(string methodName, string strNameSpace)
        {
            // Selects just the elements with namespace http://tempuri.org/ (i.e. ignores SOAP namespace)
            XmlNamespaceManager namespMan = new XmlNamespaceManager(new NameTable());
            
            if (string.IsNullOrEmpty(strNameSpace))
                namespMan.AddNamespace("foo", "http://tempuri.org/");
            else
                namespMan.AddNamespace("foo", strNameSpace);

            
            XElement webMethodResult = ResponseSOAP.XPathSelectElement("//foo:" + methodName + "Result", namespMan);
            if (webMethodResult==null)
                webMethodResult = ResponseSOAP.XPathSelectElement("//foo:" + methodName, namespMan);

            // If the result is an XML, return it and convert it to string
            if (webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
            {
                
                ResultXML = XDocument.Parse(Utils.UnescapeString(webMethodResult.FirstNode.ToString()));
                ResultXML = Utils.RemoveNamespaces(ResultXML);
                ResultString = ResultXML.ToString();
            }
            // If the result is a string, return it and convert it to XML (creating a root node to wrap the result)
            else
            {
                ResultString = webMethodResult.FirstNode.ToString();
                try
                {
                    ResultXML = XDocument.Parse(Utils.UnescapeString(ResultString));
                }
                catch
                {
                    ResultXML = XDocument.Parse("<root>"+Utils.UnescapeString(ResultString)+"</root>");

                }
            }

            if (ResultXML!=null)
            {
                FindOutParameters(ResultXML, out m_outParameters);
            }

        }



        protected bool FindOutParameters(XDocument oXML, out SortedList parameters)
        {
            bool bRes=true;
            parameters = new SortedList();
            try
            {
                try
                {                   
                    foreach (XNode oNode in oXML.Nodes())
                    {
                        NodeToParams(oNode, parameters);
                    }
                }
                catch (Exception)
                {
                    bRes = false;
                }
            }
            catch (Exception)
            {
                bRes = false;
            }

            return bRes;
        }

        protected void NodeToParams(XNode oNode, SortedList parameters)
        {
            if (((XElement)oNode).HasElements)
            {
                SortedList oCurrentSortedList = new SortedList();

                if (parameters[((XElement)oNode).Name.ToString()] == null)
                {
                    ArrayList oArray = new ArrayList();
                    oArray.Add(oCurrentSortedList);
                    parameters[((XElement)oNode).Name.ToString()] = oArray;

                }
                else 
                {
                    ArrayList oArray = (ArrayList)parameters[((XElement)oNode).Name.ToString()];
                    oArray.Add(oCurrentSortedList);                                       
                }
                
               
                foreach (XNode oChildNode in ((XElement)oNode).Elements())
                {
                    NodeToParams(oChildNode, oCurrentSortedList);
                }



            }
            else
            {

                if (parameters[((XElement)oNode).Name.ToString()] == null)
                    parameters[((XElement)oNode).Name.ToString()] = ((XElement)oNode).Value.ToString();
                else
                {
                    if (parameters[((XElement)oNode).Name.ToString()].GetType() != typeof(ArrayList))
                    {
                        ArrayList oArray = new ArrayList();
                        oArray.Add(parameters[((XElement)oNode).Name.ToString()]);
                        oArray.Add(((XElement)oNode).Value.ToString());
                        parameters[((XElement)oNode).Name.ToString()] = oArray;
                    }
                    else
                    {
                        ((ArrayList)parameters[((XElement)oNode).Name.ToString()]).Add(((XElement)oNode).Value.ToString());
                    }

                }

            }
        }



        /// <summary>
        /// Invokes a Web Method, with its parameters encoded or not.
        /// </summary>
        /// <param name="methodName">Name of the web method you want to call (case sensitive)</param>
        /// <param name="encode">Do you want to encode your parameters? (default: true)</param>
        private void Invoke(string methodNameRequest, string methodNameResponse, string strResponseNameSpace, bool encode)
        {
            AssertCanInvoke(methodNameRequest);
            string soapStr =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                   xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <{0} xmlns=""http://tempuri.org/"">
                      {1}
                    </{0}>
                  </soap:Body>
                </soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + methodNameRequest + "\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";
            req.Timeout = Timeout;

            if (!string.IsNullOrEmpty(Username))
                req.Credentials = new System.Net.NetworkCredential(Username, Password);

            using (Stream stm = req.GetRequestStream())
            {
                string postValues = "";
                foreach (var param in Params)
                {
                    if (encode) postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.HtmlEncode(param.Key), HttpUtility.HtmlEncode(param.Value));
                    else postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
                }

                soapStr = string.Format(soapStr, methodNameRequest, postValues);
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();
                ResponseSOAP = XDocument.Parse(result);
                if (string.IsNullOrEmpty(methodNameResponse))
                    ExtractResult(methodNameRequest, strResponseNameSpace);
                else
                    ExtractResult(methodNameResponse, strResponseNameSpace);
            }
        }

        /// <summary>
        /// This method should be called before each Invoke().
        /// </summary>
        public virtual void PreInvoke()
        {
            CleanLastInvoke();
            // feel free to add more instructions to this method
        }

        /// <summary>
        /// This method should be called after each (successful or unsuccessful) Invoke().
        /// </summary>
        public virtual void PosInvoke()
        {
            // feel free to add more instructions to this method
        }

        #endregion
    }
}
