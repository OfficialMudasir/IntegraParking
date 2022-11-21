using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace PayPal.Platform.SDK
{
    /// <summary>
    /// Summary description for XMLEncoder.
    /// </summary>
    public class XMLEncoder
    {
        public XMLEncoder()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string Encode(object obj)
        {
            StringWriter Output = null;
            XmlSerializer serializer = null;

            try
            {
                Output = new StringWriter(new StringBuilder());
                serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(Output, obj);
                return Output.ToString();
            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in XMLEncoder->Encode method", ex);
            }
            finally
            {
                Output = null;
                serializer = null;
            }


        }
        public static object Decode(string xmlEnvelope, Type toType)
        {
            XmlSerializer serializer = null;

            try
            {
                serializer = new XmlSerializer(toType);

                xmlEnvelope = xmlEnvelope.Replace("UTF", "utf").Trim();
                xmlEnvelope = xmlEnvelope.Replace("FaultMessage", "FaultDetailFaultMessage");
                xmlEnvelope = xmlEnvelope.Replace("ns2:", "");
                xmlEnvelope = xmlEnvelope.Replace("xmlns:ns2=\"http://svcs.paypal.com/types/ap\"", "");
                xmlEnvelope = xmlEnvelope.Replace("<parameter />","");
                //xmlEnvelope = xmlEnvelope.Replace("<FaultMessage >", "<FaultMessage>");
                //xmlEnvelope = xmlEnvelope.Replace("xmlns:ns2=\"http://svcs.paypal.com/types/ap\"", "");
                MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(xmlEnvelope));
                return (object)serializer.Deserialize(reader);

                //serializer = new XmlSerializer(toType);
                //xmlEnvelope = xmlEnvelope.Replace("ns2:", "");
                //xmlEnvelope = xmlEnvelope.Replace("xmlns:ns2=\"http://svcs.paypal.com/types/ap\"", " ");
                //StringReader strrdr = new StringReader(xmlEnvelope);
                //XmlTextReader reader = new XmlTextReader(strrdr);
                //return (object)serializer.Deserialize(reader);


            }
            catch (FATALException FATALEx)
            {
                throw FATALEx;
            }
            catch (Exception ex)
            {
                throw new FATALException("Error occurred in XMLEncoder->Decode method", ex);
            }
            finally
            {
                serializer = null;
            }

        }
    }
}

