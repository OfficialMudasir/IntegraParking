using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.WS.Infrastructure
{
    public enum ParkingOperationButtonType
    {
        Increment = 1,
        RateStep = 2,
        RateMaximum = 3,
        Reset = 4
    }

    public class ParkingOperationButton
    {
        public ParkingOperationButton()
        {
            JsonArray = "true";
        }

        [System.Xml.Serialization.XmlAttribute(@"jsonArray")]
        public string JsonArray { get; set; }

        [System.Xml.Serialization.XmlIgnore()]
        public ParkingOperationButtonType Type { get; set; }
        [System.Xml.Serialization.XmlElement(ElementName = "btntype")]
        public int Type_Int
        {
            get { return (int)this.Type; }
            set { this.Type = (ParkingOperationButtonType)value; }
        }

        [System.Xml.Serialization.XmlElement(ElementName = "min")]
        public int? Minutes { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "lit")]
        public string Literal { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "id")]
        public string Id { get; set; }
    }

    public class ParkingOperationButtonsList
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(ParkingOperationButtonsList));

        [System.Xml.Serialization.XmlArrayItem("button")]
        public ParkingOperationButton[] Buttons { get; set; }

        public string ToCustomXml()
        {
            string sXml = "";

            if (this.Buttons != null && this.Buttons.Any())
            {
                sXml = this.XmlSerializeToString();
                sXml = ParkingOperationButtonsList.RemoveNilTrue(sXml);
                int iIniPos = sXml.IndexOf("<Buttons>") + ("<Buttons>").Length;
                int iEndPos = sXml.IndexOf("</Buttons>");
                sXml = sXml.Substring(iIniPos, iEndPos - iIniPos).Replace("jsonArray", "json:Array");
            }
            return sXml;
        }

        public static ParkingOperationButtonsList FromCustomXml(string sXml)
        {
            ParkingOperationButtonsList oRet = null;

            try
            {
                sXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><ParkingOperationButtonsList xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Buttons>{0}</Buttons></ParkingOperationButtonsList>", sXml);
                sXml = sXml.Replace("json:Array=\"true\"", "");
                sXml = sXml.Replace("json:Array='true'", "");
                oRet = (ParkingOperationButtonsList)Conversions.XmlDeserializeFromString(sXml, typeof(ParkingOperationButtonsList));
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "ParkingOperationButtonsList::FromCustomXml", ex);
            }

            return oRet;
        }

        public static String RemoveNilTrue(String xmlContent, String schemePrefix = "xsi")
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDocument.NameTable);

            bool schemeExist = false;

            foreach (XmlAttribute attr in xmlDocument.DocumentElement.Attributes)
            {
                if (attr.Prefix.Equals("xmlns", StringComparison.InvariantCultureIgnoreCase)
                    && attr.LocalName.Equals(schemePrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    nsMgr.AddNamespace(attr.LocalName, attr.Value);
                    schemeExist = true;
                    break;
                }
            }

            // scheme exists - remove nodes
            if (schemeExist)
            {
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//*[@" + schemePrefix + ":nil='true']", nsMgr);

                foreach (XmlNode xmlNode in xmlNodeList)
                    xmlNode.ParentNode.RemoveChild(xmlNode);

                return xmlDocument.InnerXml;
            }
            else
                return xmlContent;
        }
    }
}