using System;
using System.Text ;
using System.Xml.Serialization ;
using System.IO ;

namespace PayPal.Platform.SDK
{
	/// <summary>
	/// Summary description for SoapEncoder.
	/// </summary>
	public class SoapEncoder
	{
		public SoapEncoder()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Encodes/converts the given object into SOAP Envelope format
		/// </summary>
		/// <param name="obj">object needs to be encoded</param>
		/// <returns>SOAP Encoded string</returns>
		public static string Encode(object obj)
		{
			string SoapRequest, xml = string.Empty ;
			StringWriter Output = null;
			XmlSerializer serializer = null;

			try
			{
				/// Initializing the XMLSerializer.
				Output = new StringWriter(new StringBuilder());
				serializer = new XmlSerializer(obj.GetType());

				/// Serializing object into XML.
				serializer.Serialize(Output, obj);

				/// Constructing the SOAP Outer Envelope format.
				SoapRequest = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body>";

				/// Adding SOAP Envelope to Serialized XML string.
				xml = Output.ToString().Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
				xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
				xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Trim();
				xml=xml.Replace("\r\n","");
				xml = xml.Replace(" ", "");
				SoapRequest = SoapRequest + xml.Trim();
				SoapRequest = SoapRequest + @"</soap:Body></soap:Envelope>";
						
				return SoapRequest;
			}
			catch(FATALException FATALEx)
			{
				throw FATALEx;
			}
			catch(Exception ex)
			{
				throw new FATALException("Error occurred in SoapEncoder->Encode method",ex);
			}
			finally
			{
				Output = null;
				serializer = null;
			}


		}

		/// <summary>
		/// Decodes/converts the given SOAP Envelope to given object
		/// </summary>
		/// <param name="soapEnvelope">SOAP envelope needs to be Decoced</param>
		/// <param name="toType">type of object to be converted/decoded from given SAOP Envelope</param>
		/// <returns>Given type Object</returns>
		public static object Decode(string soapEnvelope, Type toType)
		{
			XmlSerializer serializer = null;

			try
			{
				/// Initializing the XMLSerializer.
				serializer = new XmlSerializer(toType);
				
				/// Removing SOAP outer Envelope
				soapEnvelope = soapEnvelope.Replace("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">", "");
				soapEnvelope = soapEnvelope.Replace("<soapenv:Header />", "");
				soapEnvelope = soapEnvelope.Replace("<soapenv:Header/>", "");
				soapEnvelope = soapEnvelope.Replace("<soapenv:Body>","");
				soapEnvelope = soapEnvelope.Replace("</soapenv:Body></soapenv:Envelope>","");
				soapEnvelope = soapEnvelope.Replace("xmlns:ns2=\"http://svcs.paypal.com/types/ap\"","");
				soapEnvelope = soapEnvelope.Replace("ns2:","");
				soapEnvelope = soapEnvelope.Replace("soapenv:","");
				soapEnvelope = soapEnvelope.Replace("ns3:","");
				soapEnvelope = soapEnvelope.Replace("xmlns:ns2=\"http://svcs.paypal.com/types/ap\"","");
				
                /// Deserializing and Returning the XML
                MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(soapEnvelope));
				return (object)serializer.Deserialize(reader);
						
			}
			catch(FATALException FATALEx)
			{
				throw FATALEx;
			}
			catch(Exception ex)
			{
				throw new FATALException("Error occurred in SoapEncoder->Decode method",ex);
			}
			finally
			{
				serializer = null;
			}

		}
	}
}
