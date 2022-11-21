using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Globalization;
using System.Xml.XPath;
using integraMobile.Infrastructure.Logging.Tools;

namespace integraMobile.ExternalWS
{

    public class ThirdPartyGeo : ThirdPartyBase
    {

        public ThirdPartyGeo()
            : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyGeo));
        }


        public bool ReverseGeoLoc(double latitude, double longitude, string api_key,
            out string Address_country_code,
            out string Address_country_name,
            out string Address_administrative_area_level_1,
            out string Address_administrative_area_level_2,
            out string Address_administrative_area_level_3,
            out string Address_colloquial_area,
            out string Address_locality,
            out string Address_sublocality,
            out string Address_neighborhood)
        {

            bool bRes = false;
            Address_country_code = "";
            Address_country_name = "";
            Address_administrative_area_level_1 = "";
            Address_administrative_area_level_2 = "";
            Address_administrative_area_level_3 = "";
            Address_colloquial_area = "";
            Address_locality = "";
            Address_sublocality = "";
            Address_neighborhood = "";

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + 
                    latitude.ToString(CultureInfo.InvariantCulture) + 
                    "," + 
                    longitude.ToString(CultureInfo.InvariantCulture) + 
                    "&sensor=false"+
                    "&key=" + api_key);

                XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                if (element.InnerText == "ZERO_RESULTS")
                {
                    Logger_AddLogMessage("ReverseGeoLoc::No data available for the specified location", LogLevels.logERROR);
                    return bRes;
                }
                else
                {

                    element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");

                    string longname = "";
                    string shortname = "";
                    string typename = "";
                    bool fHit = false;


                    XmlNodeList xnList = doc.SelectNodes("//GeocodeResponse/result/address_component");
                    foreach (XmlNode xn in xnList)
                    {
                        try
                        {
                            longname = xn["long_name"].InnerText;
                            shortname = xn["short_name"].InnerText;
                            typename = xn["type"].InnerText;


                            fHit = true;
                            switch (typename)
                            {
                                //Add whatever you are looking for below
                                case "country":
                                    {
                                        Address_country_name = longname;
                                        Address_country_code = shortname;
                                        break;
                                    }

                                case "locality":
                                    {
                                        Address_locality = longname;
                                        //Address_locality = shortname; //Om Longname visar sig innehålla konstigheter kan man använda shortname istället
                                        break;
                                    }

                                case "sublocality":
                                    {
                                        Address_sublocality = longname;
                                        break;
                                    }

                                case "neighborhood":
                                    {
                                        Address_neighborhood = longname;
                                        break;
                                    }

                                case "colloquial_area":
                                    {
                                        Address_colloquial_area = longname;
                                        break;
                                    }

                                case "administrative_area_level_1":
                                    {
                                        Address_administrative_area_level_1 = longname;
                                        break;
                                    }

                                case "administrative_area_level_2":
                                    {
                                        Address_administrative_area_level_2 = longname;
                                        break;
                                    }

                                case "administrative_area_level_3":
                                    {
                                        Address_administrative_area_level_3 = longname;
                                        break;
                                    }

                                default:
                                    fHit = false;
                                    break;
                            }


                            if (fHit)
                            {
                                Logger_AddLogMessage("ReverseGeoLoc::\tL: " + longname + "\tS:" + shortname, LogLevels.logERROR);
                            }
                        }

                        catch (Exception e)
                        {
                            //Node missing either, longname, shortname or typename
                            fHit = false;
                            Logger_AddLogException(e, "ReverseGeoLoc::Exception", LogLevels.logERROR);
                           
                        }


                    }
                    bRes = true;
                    return bRes;
                }

            }
            catch (Exception ex)
            {
                Logger_AddLogException(ex, "ReverseGeoLoc::Exception", LogLevels.logERROR);
                return bRes; ;
            }
        }

    }
}