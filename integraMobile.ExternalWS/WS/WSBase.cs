using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Text;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.WS.Data;

namespace integraMobile.ExternalWS
{
    public class WSBase
    {
        protected const int DEFAULT_WS_TIMEOUT = 5000; //ms        
        private const long BIG_PRIME_NUMBER = 2147483647;
        protected static string _xmlTagName = "ipark";
        protected const string OUT_SUFIX = "_out";
        protected const int OSID = 5; // Web

        //Log4net Wrapper class
        protected static CLogWrapper m_Log;

        protected Dictionary<string, int> m_oLanguages;


        public WSBase()
        {
            m_oLanguages = new Dictionary<string, int>() { { "es-ES", 1 }, 
                                                           { "en-US", 2 }, 
                                                           { "fr-CA", 3 }, 
                                                           { "ca-ES", 4 }, 
                                                           { "es-MX", 1 } };
            
        }

        protected int GetWSTimeout()
        {
            int iRes = DEFAULT_WS_TIMEOUT;
            try
            {
                iRes = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"].ToString());
            }
            catch
            {
                iRes = DEFAULT_WS_TIMEOUT;
            }

            return iRes;

        }

        protected string GetWSUrl()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSUrl"].ToString();
            }
            catch { }
            return sRet;
        }
        protected string GetWSAuthHashKey()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSAuthHashKey"].ToString();
            }
            catch { }
            return sRet;
        }        
        protected string GetWSHttpUser()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSHttpUser"].ToString();
            }
            catch { }
            return sRet;
        }
        protected string GetWSHttpPassword()
        {
            string sRet = "";
            try
            {
                sRet = ConfigurationManager.AppSettings["WSHttpPassword"].ToString();
            }
            catch { }
            return sRet;
        }

        protected string CalculateWSHash(string strInput)
        {
            string strRes = "";
            int iKeyLength = 64;
            byte[] normMACKey = null;
            HMACSHA256 oMACsha256 = null;

            try
            {
                string strMACKey = GetWSAuthHashKey();

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strMACKey);
                normMACKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    normMACKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }

                oMACsha256 = new HMACSHA256(normMACKey);


                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                byte[] hash = oMACsha256.ComputeHash(inputBytes); ;

                if (hash.Length >= 8)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = hash.Length - 8; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    strRes = sb.ToString();
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateStandardWSHash::Exception", LogLevels.logERROR);
            }

            return strRes;
        }

        protected ResultType FindOutParameters(string xmlIn, out SortedList parameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            parameters = new SortedList();


            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    if (xmlIn.StartsWith("<?xml"))
                    {
                        xmlInDoc.LoadXml(xmlIn);
                    }
                    else
                    {
                        xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);
                    }

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        switch (Node.Name)
                        {
                            default:

                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        int i = 0;
                                        string sPrevChildNodeName = "";
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {
                                            if (sPrevChildNodeName != ChildNode.Name) i = 0;
                                            sPrevChildNodeName = ChildNode.Name;

                                            if (ChildNode.ChildNodes.Count > 0)
                                            {
                                                if (!ChildNode.ChildNodes[0].HasChildNodes)
                                                {
                                                    //if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] == null)
                                                    //{
                                                    //    parameters[Node.Name + "_" + ChildNode.Name] = ChildNode.InnerText.Trim();
                                                    //}
                                                    //else
                                                    //{
                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString()] = ChildNode.InnerText.Trim();
                                                    //}
                                                    for (int a = 0; a < ChildNode.Attributes.Count; a++)
                                                    {
                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode.Attributes[a].Name] = ChildNode.Attributes[a].InnerText;
                                                    }
                                                }
                                                else
                                                {
                                                    int j = 0;
                                                    foreach (XmlNode ChildNode2 in ChildNode.ChildNodes)
                                                    {
                                                        //if (parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] == null)
                                                        //{
                                                        //    parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name] = ChildNode2.InnerText.Trim();
                                                        //}
                                                        //else
                                                        //{
                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_" + j.ToString()] = ChildNode2.InnerText.Trim();
                                                        //}
                                                        j += 1;
                                                        parameters[Node.Name + "_" + ChildNode.Name + "_" + i.ToString() + "_" + ChildNode2.Name + "_num"] = j;
                                                    }
                                                }
                                                i++;
                                                parameters[Node.Name + "_" + ChildNode.Name + "_num"] = i;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        parameters[Node.Name] = Node.InnerText.Trim();
                                    }
                                }
                                else
                                {
                                    parameters[Node.Name] = null;
                                }

                                break;

                        }

                    }

                    if (Nodes.Count == 0)
                    {
                        Logger_AddLogMessage(string.Format("FindParameters: Bad Input XML: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logERROR);
                        rtRes = ResultType.Result_Error_Generic;

                    }


                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindInputParameters: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindInputParameters::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }

        protected ResultType FindOutParameters2(string xmlIn, out SortedList parameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            parameters = new SortedList();


            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    if (xmlIn.StartsWith("<?xml"))
                    {
                        xmlInDoc.LoadXml(xmlIn);
                    }
                    else
                    {
                        xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);
                    }

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode oNode in Nodes)
                    {
                        NodeToParams(oNode, "", parameters);
                    }
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindOutParameters2: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindOutParameters2::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }
        protected void NodeToParams(XmlNode oNode, string sPrefix, SortedList parameters)
        {
            if (oNode.HasChildNodes && oNode.ChildNodes[0].HasChildNodes)
            {
                int iNodeIndex = 0;
                if (parameters.ContainsKey(sPrefix + oNode.Name + "_num"))
                    iNodeIndex = Convert.ToInt32(parameters[sPrefix + oNode.Name + "_num"]);

                //int i = 0;
                foreach (XmlNode oChildNode in oNode.ChildNodes)
                {
                    /*if (!parameters.ContainsKey(sPrefix + oNode.Name + "_" + oChildNode.Name + "_num")) i = 0;
                    NodeToParams(oChildNode, sPrefix + oNode.Name + "_" + i.ToString() + "_", parameters);                    
                    i += 1;
                    parameters[sPrefix + oNode.Name + "_" + oChildNode.Name + "_num"] = i;*/
                    //if (!parameters.ContainsKey(sPrefix + oNode.Name + "_" + oChildNode.Name + "_num")) i = 0;
                    if (oChildNode.NodeType == XmlNodeType.Text)
                    {
                        if (parameters.ContainsKey(sPrefix + oNode.Name))
                            parameters[sPrefix + oNode.Name] += "~";
                        parameters[sPrefix + oNode.Name] = (parameters[sPrefix + oNode.Name] ?? "") + oNode.InnerText.Trim();

                    }
                    else
                    {
                        NodeToParams(oChildNode, sPrefix + oNode.Name + "_" + iNodeIndex.ToString() + "_", parameters);
                    }
                    //i += 1;

                }

                iNodeIndex += 1;
                parameters[sPrefix + oNode.Name + "_num"] = iNodeIndex;

            }
            else if (oNode.HasChildNodes && oNode.ChildNodes.Count>0)
            {              
                //int i = 0;
                foreach (XmlNode oChildNode in oNode.ChildNodes)
                {

                    if (oChildNode.NodeType == XmlNodeType.Text)
                    {
                        if (parameters.ContainsKey(sPrefix + oNode.Name))
                            parameters[sPrefix + oNode.Name] += "~";
                        parameters[sPrefix + oNode.Name] = (parameters[sPrefix + oNode.Name] ?? "") + oNode.InnerText.Trim();

                    }
                    else
                    {

                        NodeToParams(oChildNode, sPrefix + oNode.Name + "_0_", parameters);
                    }

                }
            }
            else
            {
                if (parameters.ContainsKey(sPrefix + oNode.Name))
                    parameters[sPrefix + oNode.Name] += "~";
                parameters[sPrefix + oNode.Name] = (parameters[sPrefix + oNode.Name] ?? "") + oNode.InnerText.Trim();
            }
        }

        protected ResultType FindOutParameters3(string xmlIn, out SortedList parameters)
        {
            ResultType rtRes = ResultType.Result_OK;
            parameters = new SortedList();


            try
            {
                var xmlInDoc = XDocument.Parse(/*Utils.UnescapeString(*/xmlIn/*)*/);
                xmlInDoc = Utils.RemoveNamespaces(xmlInDoc);
                
                try
                {
                    foreach (XNode oNode in xmlInDoc.Nodes())
                    {
                        NodeToParams(oNode, parameters);
                    }

                    parameters = parameters.GetOutputElement("ipark_out");
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindOutParameters3: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindOutParameters3::Exception", LogLevels.logERROR);

            }


            return rtRes;
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

        protected string PrettyXml(string xml)
        {

            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }

        protected string LatLonXml(decimal? dLatitude, decimal? dLongitude, out string sLatLon)
        {
            string sXml = "";
            sLatLon = "";
            if (dLatitude.HasValue)
            {
                sXml += string.Format("<lat>{0}</lat>", dLatitude.Value);
                sLatLon += string.Format("{0}", dLatitude.Value);
            }
            if (dLongitude.HasValue)
            {
                sXml += string.Format("<long>{0}</long>", dLongitude.Value);
                sLatLon += string.Format("{0}", dLongitude.Value);
            }
            
            if (!string.IsNullOrEmpty(sXml))
                sXml = "<gps>" + sXml + "</gps>";
            
            return sXml;
        }

        protected static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }


        protected static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }


    }
}