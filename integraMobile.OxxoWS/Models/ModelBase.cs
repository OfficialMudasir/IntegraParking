using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.OxxoWS.Models
{
    public class ModelBase
    {

        protected string DataContractSerializeObject<T>(T objectToSerialize)
        {
            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        public static string PrettyXml(string xml)
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

        public static string SerializeToString(Type oObjType, object oObj)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", ""); //Add an empty namespace and empty value            
            XmlSerializer slz = new XmlSerializer(oObjType);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                slz.Serialize(writer, oObj, ns);
            }

            return sb.ToString().Replace("\"utf-16\"", "\"utf-8\"");
        }

    }
}