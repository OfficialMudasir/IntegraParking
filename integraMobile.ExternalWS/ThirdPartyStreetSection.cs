using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain;
using integraMobile.Infrastructure.PermitsAPI;

namespace integraMobile.ExternalWS
{
    public class ThirdPartyStreetSection : ThirdPartyBase
    {
        private const double EARTH_RADIUS_KM = 6378.1370;
        private const decimal GRID_SIZE_KM = 0.2M;

        public ThirdPartyStreetSection() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyStreetSection));
        }

        public bool PermitsGetInstallations(INSTALLATION oInstallation, out PermitsErrorResponse oErrorResponse, out List<InstallationData> oInstallationsData, out long lEllapsedTime)
        {
            bool bRes = false;
            oErrorResponse = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;
            Exception oNotificationEx = null;
            oInstallationsData = new List<InstallationData>();

            try
            {
                string sBaseUrl = oInstallation.INS_STREET_SECTION_UPDATE_WS_URL;
                string sUser = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_USER;
                string sPassword = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_PASSWORD;

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    if (this.PermitsToken(sBaseUrl, sUser, sPassword, out sAccessToken))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                        sBaseUrl = string.Format(sBaseUrl + "{0}installations", (!sBaseUrl.EndsWith("/") ? "/" : ""));

                        int iPage = 0;
                        int iPageSize = 250;
                        int iReturnedCount = iPageSize;

                        string sUrl = "";

                        bRes = true;

                        while (iReturnedCount >= iPageSize && bRes)
                        {
                            sUrl = string.Format(sBaseUrl + "?Page={0}&PageSize={1}", iPage++, iPageSize);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                            request.Method = "GET";
                            request.ContentType = "application/json";
                            request.Accept = "application/json";
                            request.Timeout = Get3rdPartyWSTimeout();
                            request.Headers.Add(string.Format("Authorization: Bearer {0}", sAccessToken));

                            Logger_AddLogMessage(string.Format("PermitsGetInstallations request.url={0}, Authorization: Bearer {1}", sUrl, sAccessToken), LogLevels.logINFO);

                            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            //{
                            //    streamWriter.Write(json);
                            //    streamWriter.Flush();
                            //    streamWriter.Close();
                            //}

                            if (watch != null)
                            {
                                watch.Stop();
                                watch = null;
                            }

                            watch = Stopwatch.StartNew();

                            try
                            {
                                using (WebResponse response = request.GetResponse())
                                {
                                    bAuthRetry = false;

                                    using (Stream strReader = response.GetResponseStream())
                                    {
                                        lEllapsedTime += watch.ElapsedMilliseconds;

                                        if (strReader == null)
                                        {
                                            bRes = false;
                                        }
                                        using (StreamReader objReader = new StreamReader(strReader))
                                        {
                                            string responseBody = objReader.ReadToEnd();

                                            Logger_AddLogMessage(string.Format("PermitsGetInstallations response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                            dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                            if (oResponse != null)
                                            {
                                                iReturnedCount = oResponse.Count;

                                                foreach (var oItem in oResponse)
                                                {
                                                    oInstallationsData.Add(new InstallationData
                                                    {
                                                        id = oItem.id.Value,
                                                        description = oItem.description.Value,
                                                        remarks = oItem.remarks?.Value,
                                                        externalId = oItem.externalId.Value
                                                    });
                                                }

                                                bRes = true;
                                            }
                                            else
                                                bRes = false;
                                        }
                                    }
                                }
                            }
                            catch (WebException ex)
                            {
                                if (watch != null)
                                    lEllapsedTime = watch.ElapsedMilliseconds;
                                

                                if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    if (bAuthRetry.Value == false)
                                    {
                                        m_sPermitsAccessToken = null;
                                        bAuthRetry = true;
                                    }
                                    else
                                        bAuthRetry = false;
                                }
                                else
                                {
                                    bAuthRetry = false;
                                    if (ex.Response != null)
                                        oErrorResponse = PermitsErrorResponse.Load((HttpWebResponse)ex.Response);
                                    else
                                        oErrorResponse = new PermitsErrorResponse()
                                        {
                                            status = ex.Status.ToString(),
                                            timeout = (ex.Status == WebExceptionStatus.Timeout)
                                        };
                                    //rtRes = oErrorResponse.GetResultType();
                                }
                                bRes = false;
                                Logger_AddLogException(ex, "PermitsGetInstallations::WebException", LogLevels.logERROR);
                            }
                            catch (Exception ex)
                            {
                                if (watch != null)
                                    lEllapsedTime = watch.ElapsedMilliseconds;

                                bAuthRetry = false;
                                bRes = false;
                                Logger_AddLogException(ex, "PermitsGetInstallations::Exception", LogLevels.logERROR);
                            }
                        }

                    }
                    else
                    {
                        //rtRes = ResultType.Result_Error_InvalidAuthentication;
                        bRes = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (watch != null)
                    lEllapsedTime = watch.ElapsedMilliseconds;

                oNotificationEx = ex;
                bRes = false;
                Logger_AddLogException(ex, "PermitsGetInstallations::Exception", LogLevels.logERROR);
            }

            return bRes;
        }

        public bool PermitsGetInstallationGroups(INSTALLATION oInstallation, out PermitsErrorResponse oErrorResponse, out List<GroupData> oGroupsData, out long lEllapsedTime)
        {
            bool bRes = false;
            oErrorResponse = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;
            Exception oNotificationEx = null;
            oGroupsData = new List<GroupData>();

            try
            {
                List<InstallationData> oInstallationsData = null;

                bRes = this.PermitsGetInstallations(oInstallation, out oErrorResponse, out oInstallationsData, out lEllapsedTime);
                if (bRes)
                {
                    lEllapsedTime = 0;

                    string sPermitsInstallationId = oInstallationsData.Where(i => i.externalId == oInstallation.INS_PERMITS_INS_ID).Select(i => i.id).FirstOrDefault();

                    string sBaseUrl = oInstallation.INS_STREET_SECTION_UPDATE_WS_URL;
                    string sUser = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_USER;
                    string sPassword = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_PASSWORD;

                    string sAccessToken;

                    bool? bAuthRetry = null;
                    while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                    {
                        if (!bAuthRetry.HasValue)
                            bAuthRetry = false;

                        if (this.PermitsToken(sBaseUrl, sUser, sPassword, out sAccessToken))
                        {
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            sBaseUrl = string.Format(sBaseUrl + "{0}installations/{1}/zones", (!sBaseUrl.EndsWith("/") ? "/" : ""), sPermitsInstallationId);

                            int iPage = 0;
                            int iPageSize = 250;
                            int iReturnedCount = iPageSize;

                            string sUrl = "";

                            bRes = true;

                            while (iReturnedCount >= iPageSize && bRes)
                            {
                                sUrl = string.Format(sBaseUrl + "?Page={0}&PageSize={1}", iPage++, iPageSize);

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                                request.Method = "GET";
                                request.ContentType = "application/json";
                                request.Accept = "application/json";
                                request.Timeout = Get3rdPartyWSTimeout();
                                request.Headers.Add(string.Format("Authorization: Bearer {0}", sAccessToken));

                                Logger_AddLogMessage(string.Format("PermitsGetInstallationGroups request.url={0}, Authorization: Bearer {1}", sUrl, sAccessToken), LogLevels.logINFO);

                                if (watch != null)
                                {
                                    watch.Stop();
                                    watch = null;
                                }

                                watch = Stopwatch.StartNew();

                                try
                                {
                                    using (WebResponse response = request.GetResponse())
                                    {
                                        bAuthRetry = false;

                                        using (Stream strReader = response.GetResponseStream())
                                        {
                                            lEllapsedTime += watch.ElapsedMilliseconds;

                                            if (strReader == null)
                                            {
                                                bRes = false;
                                            }
                                            using (StreamReader objReader = new StreamReader(strReader))
                                            {
                                                string responseBody = objReader.ReadToEnd();

                                                Logger_AddLogMessage(string.Format("PermitsGetInstallationGroups response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                                dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                                if (oResponse != null)
                                                {
                                                    iReturnedCount = oResponse.Count;

                                                    foreach (var oItem in oResponse)
                                                    {
                                                        oGroupsData.Add(new GroupData
                                                        {
                                                            id = oItem.id.Value,
                                                            description = oItem.description.Value,
                                                            externalId = oItem.externalId.Value
                                                        });
                                                    }

                                                    bRes = true;
                                                }
                                                else
                                                    bRes = false;
                                            }
                                        }
                                    }
                                }
                                catch (WebException ex)
                                {
                                    if (watch != null)
                                        lEllapsedTime = watch.ElapsedMilliseconds;


                                    if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        if (bAuthRetry.Value == false)
                                        {
                                            m_sPermitsAccessToken = null;
                                            bAuthRetry = true;
                                        }
                                        else
                                            bAuthRetry = false;
                                    }
                                    else
                                    {
                                        bAuthRetry = false;
                                        if (ex.Response != null)
                                            oErrorResponse = PermitsErrorResponse.Load((HttpWebResponse)ex.Response);
                                        else
                                            oErrorResponse = new PermitsErrorResponse()
                                            {
                                                status = ex.Status.ToString(),
                                                timeout = (ex.Status == WebExceptionStatus.Timeout)
                                            };
                                        //rtRes = oErrorResponse.GetResultType();
                                    }
                                    bRes = false;
                                    Logger_AddLogException(ex, "PermitsGetInstallationGroups::WebException", LogLevels.logERROR);
                                }
                                catch (Exception ex)
                                {
                                    if (watch != null)
                                        lEllapsedTime = watch.ElapsedMilliseconds;

                                    bAuthRetry = false;
                                    bRes = false;
                                    Logger_AddLogException(ex, "PermitsGetInstallationGroups::Exception", LogLevels.logERROR);
                                }
                            }

                        }
                        else
                        {
                            //rtRes = ResultType.Result_Error_InvalidAuthentication;
                            bRes = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (watch != null)
                    lEllapsedTime = watch.ElapsedMilliseconds;

                oNotificationEx = ex;
                bRes = false;
                Logger_AddLogException(ex, "PermitsGetInstallationGroups::Exception", LogLevels.logERROR);
            }

            return bRes;
        }

        public bool PermitsGetStreets(INSTALLATION oInstallation, out PermitsErrorResponse oErrorResponse, out List<StreetData> oStreetsData, out long lEllapsedTime)
        {
            bool bRes = false;
            oErrorResponse = null;
            lEllapsedTime = 0;            
            Stopwatch watch = null;
            Exception oNotificationEx = null;
            oStreetsData = new List<StreetData>();

            try
            {
                List<InstallationData> oInstallationsData = null;

                bRes = this.PermitsGetInstallations(oInstallation, out oErrorResponse, out oInstallationsData, out lEllapsedTime);
                if (bRes)
                {
                    lEllapsedTime = 0;

                    string sPermitsInstallationId = oInstallationsData.Where(i => i.externalId == oInstallation.INS_PERMITS_INS_ID).Select(i => i.id).FirstOrDefault();

                    string sBaseUrl = oInstallation.INS_STREET_SECTION_UPDATE_WS_URL;
                    string sUser = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_USER;
                    string sPassword = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_PASSWORD;

                    string sAccessToken;

                    bool? bAuthRetry = null;
                    while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                    {
                        if (!bAuthRetry.HasValue)
                            bAuthRetry = false;

                        if (this.PermitsToken(sBaseUrl, sUser, sPassword, out sAccessToken))
                        {
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            sBaseUrl = string.Format(sBaseUrl + "{0}installations/{1}/streets", (!sBaseUrl.EndsWith("/") ? "/" : ""), sPermitsInstallationId);

                            int iPage = 0;
                            int iPageSize = 250;
                            int iReturnedCount = iPageSize;

                            string sUrl = "";

                            bRes = true;

                            while (iReturnedCount >= iPageSize && bRes)
                            {
                                sUrl = string.Format(sBaseUrl + "?Page={0}&PageSize={1}", iPage++, iPageSize);

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                                request.Method = "GET";
                                request.ContentType = "application/json";
                                request.Accept = "application/json";
                                request.Timeout = Get3rdPartyWSTimeout();
                                request.Headers.Add(string.Format("Authorization: Bearer {0}", sAccessToken));

                                Logger_AddLogMessage(string.Format("PermitsGetStreets request.url={0}, Authorization: Bearer {1}", sUrl, sAccessToken), LogLevels.logINFO);

                                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                                //{
                                //    streamWriter.Write(json);
                                //    streamWriter.Flush();
                                //    streamWriter.Close();
                                //}

                                if (watch != null)
                                {
                                    watch.Stop();
                                    watch = null;
                                }

                                watch = Stopwatch.StartNew();

                                try
                                {
                                    using (WebResponse response = request.GetResponse())
                                    {
                                        bAuthRetry = false;

                                        using (Stream strReader = response.GetResponseStream())
                                        {
                                            lEllapsedTime += watch.ElapsedMilliseconds;

                                            if (strReader == null)
                                            {
                                                bRes = false;
                                            }
                                            using (StreamReader objReader = new StreamReader(strReader))
                                            {
                                                string responseBody = objReader.ReadToEnd();

                                                Logger_AddLogMessage(string.Format("PermitsGetStreets response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                                dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                                if (oResponse != null)
                                                {
                                                    iReturnedCount = oResponse.Count;

                                                    foreach (var oItem in oResponse)
                                                    {
                                                        oStreetsData.Add(new StreetData
                                                        {
                                                            Id = oItem.id.Value,
                                                            Description = oItem.description.Value,
                                                            Deleted = false
                                                        });
                                                    }

                                                    bRes = true;
                                                }
                                                else
                                                    bRes = false;
                                            }
                                        }
                                    }
                                }
                                catch (WebException ex)
                                {
                                    if (watch != null)
                                        lEllapsedTime = watch.ElapsedMilliseconds;

                                    if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        if (bAuthRetry.Value == false)
                                        {
                                            m_sPermitsAccessToken = null;
                                            bAuthRetry = true;
                                        }
                                        else
                                            bAuthRetry = false;
                                    }
                                    else
                                    {
                                        bAuthRetry = false;
                                        if (ex.Response != null)
                                            oErrorResponse = PermitsErrorResponse.Load((HttpWebResponse)ex.Response);
                                        else
                                            oErrorResponse = new PermitsErrorResponse()
                                            {
                                                status = ex.Status.ToString(),
                                                timeout = (ex.Status == WebExceptionStatus.Timeout)
                                            };
                                        //rtRes = oErrorResponse.GetResultType();
                                    }
                                    bRes = false;
                                    Logger_AddLogException(ex, "PermitsGetStreets::WebException", LogLevels.logERROR);
                                }
                                catch (Exception ex)
                                {
                                    if (watch != null)
                                        lEllapsedTime = watch.ElapsedMilliseconds;

                                    bAuthRetry = false;
                                    bRes = false;
                                    Logger_AddLogException(ex, "PermitsGetStreets::Exception", LogLevels.logERROR);
                                }
                            }

                        }
                        else
                        {
                            //rtRes = ResultType.Result_Error_InvalidAuthentication;
                            bRes = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (watch != null)
                    lEllapsedTime = watch.ElapsedMilliseconds;

                oNotificationEx = ex;                
                bRes = false;
                Logger_AddLogException(ex, "PermitsGetStreets::Exception", LogLevels.logERROR);
            }

            return bRes;
        }

        public bool PermitsGetStreetSections(INSTALLATION oInstallation, out PermitsErrorResponse oErrorResponse, out List<StreetSectionData> oStreetSectionsData, out Dictionary<int, GridElement> oGrid, out long lEllapsedTime)
        {
            bool bRes = false;
            oErrorResponse = null;
            lEllapsedTime = -1;
            Stopwatch watch = null;
            Exception oNotificationEx = null;
            oStreetSectionsData = new List<StreetSectionData>();
            oGrid = new Dictionary<int, GridElement>();

            try
            {
                List<GroupData> oGroupsData = null;

                bRes = this.PermitsGetInstallationGroups(oInstallation, out oErrorResponse, out oGroupsData, out lEllapsedTime);
                if (bRes)
                {
                    lEllapsedTime = 0;

                    string sBaseUrl = oInstallation.INS_STREET_SECTION_UPDATE_WS_URL;
                    string sUser = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_USER;
                    string sPassword = oInstallation.INS_STREET_SECTION_UPDATE_WS_HTTP_PASSWORD;

                    string sAccessToken;

                    bool? bAuthRetry = null;
                    while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                    {
                        if (!bAuthRetry.HasValue)
                            bAuthRetry = false;

                        if (this.PermitsToken(sBaseUrl, sUser, sPassword, out sAccessToken))
                        {
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            oStreetSectionsData = new List<StreetSectionData>();
                            oGrid = new Dictionary<int, GridElement>();

                            decimal xmin = decimal.MaxValue;
                            decimal xmax = decimal.MinValue;
                            decimal ymin = decimal.MaxValue;
                            decimal ymax = decimal.MinValue;

                            foreach (var oDBGroup in oInstallation.GROUPs.Where(g => !string.IsNullOrEmpty(g.GRP_PERMITS_EXT_ID)))
                            {
                                string sPermitsGroupId = oGroupsData.Where(g => g.externalId == oDBGroup.GRP_PERMITS_EXT_ID).Select(g => g.id).FirstOrDefault();
                                if (!string.IsNullOrEmpty(sPermitsGroupId))
                                {
                                    sBaseUrl = string.Format(sBaseUrl + "{0}zones/{1}/street-sections", (!sBaseUrl.EndsWith("/") ? "/" : ""), sPermitsGroupId);

                                    int iPage = 0;
                                    int iPageSize = 250;
                                    int iReturnedCount = iPageSize;

                                    string sUrl = "";

                                    bRes = true;

                                    while (iReturnedCount >= iPageSize && bRes)
                                    {
                                        sUrl = string.Format(sBaseUrl + "?Geometry=true&Page={0}&PageSize={1}", iPage++, iPageSize);

                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
                                        request.Method = "GET";
                                        request.ContentType = "application/json";
                                        request.Accept = "application/json";
                                        request.Timeout = Get3rdPartyWSTimeout();
                                        request.Headers.Add(string.Format("Authorization: Bearer {0}", sAccessToken));

                                        Logger_AddLogMessage(string.Format("PermitsGetStreetSections request.url={0}, Authorization: Bearer {1}", sUrl, sAccessToken), LogLevels.logINFO);

                                        //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                                        //{
                                        //    streamWriter.Write(json);
                                        //    streamWriter.Flush();
                                        //    streamWriter.Close();
                                        //}

                                        try
                                        {
                                            using (WebResponse response = request.GetResponse())
                                            {
                                                bAuthRetry = false;

                                                using (Stream strReader = response.GetResponseStream())
                                                {
                                                    if (strReader == null)
                                                    {
                                                        bRes = false;
                                                    }
                                                    using (StreamReader objReader = new StreamReader(strReader))
                                                    {
                                                        string responseBody = objReader.ReadToEnd();

                                                        Logger_AddLogMessage(string.Format("PermitsGetStreetSections response.json={0}", PrettyJSON(responseBody)), LogLevels.logINFO);

                                                        dynamic oResponse = JsonConvert.DeserializeObject(responseBody);

                                                        if (oResponse != null)
                                                        {
                                                            iReturnedCount = oResponse.Count;

                                                            foreach (var oItem in oResponse)
                                                            {
                                                                var oSection = new StreetSectionData
                                                                {
                                                                    Id = oItem.id.Value,
                                                                    Description = "",
                                                                    Zone = oDBGroup.GRP_PERMITS_EXT_ID,
                                                                    Street = oItem.streetId.Value,
                                                                    StreetNumberFrom = (int?)oItem.from?.Value,
                                                                    StreetNumberTo = (int?)oItem.to?.Value,
                                                                    Side = (int?)oItem.side?.Value,
                                                                    ReservedSpace = (int)(oItem.reservedSpaces?.Value ?? 0),
                                                                    Colour = "FF034FC7", //"6495ED",
                                                                    Deleted = false,
                                                                    oGridElements = new Dictionary<int, GridElement>()
                                                                };

                                                                if (oItem.geometry != null)
                                                                {
                                                                    oSection.GeometryType = oItem.geometry.type?.Value;
                                                                    oSection.GeometryCoordinates = new List<MapPoint>();
                                                                    foreach (var oPolygon in oItem.geometry.coordinates)
                                                                    {
                                                                        foreach (var oPoint in oPolygon)
                                                                        {
                                                                            var oMapPoint = new MapPoint()
                                                                            {
                                                                                x = Convert.ToDecimal(oPoint[0].Value),
                                                                                y = Convert.ToDecimal(oPoint[1].Value)
                                                                            };

                                                                            oSection.GeometryCoordinates.Add(oMapPoint);

                                                                            if (oMapPoint.x < xmin) xmin = oMapPoint.x;
                                                                            if (oMapPoint.x > xmax) xmax = oMapPoint.x;
                                                                            if (oMapPoint.y < ymin) ymin = oMapPoint.y;
                                                                            if (oMapPoint.y > ymax) ymax = oMapPoint.y;
                                                                        }
                                                                    }
                                                                }

                                                                bool bAddSection = true;
                                                                if (oItem.parkingRates != null)
                                                                {
                                                                    var oParkingRates = new List<ParkingRate>();
                                                                    foreach (var oRate in oItem.parkingRates)
                                                                    {
                                                                        oParkingRates.Add(new ParkingRate() { id = oRate.id.Value, name = oRate.name.Value });
                                                                    }

                                                                    bAddSection = !oParkingRates.Where(r => r.name.ToUpper().Contains("PROHIBIDO ESTACIONAR")).Any();                                                                                                                                        
                                                                }

                                                                if (bAddSection)
                                                                    oStreetSectionsData.Add(oSection);
                                                            }

                                                            bRes = true;
                                                        }
                                                        else
                                                            bRes = false;
                                                    }
                                                }
                                            }
                                        }
                                        catch (WebException ex)
                                        {
                                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                                            {
                                                if (bAuthRetry.Value == false)
                                                {
                                                    m_sPermitsAccessToken = null;
                                                    bAuthRetry = true;
                                                }
                                                else
                                                    bAuthRetry = false;
                                            }
                                            else
                                            {
                                                bAuthRetry = false;
                                                if (ex.Response != null)
                                                    oErrorResponse = PermitsErrorResponse.Load((HttpWebResponse)ex.Response);
                                                else
                                                    oErrorResponse = new PermitsErrorResponse()
                                                    {
                                                        status = ex.Status.ToString(),
                                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                                    };
                                                //rtRes = oErrorResponse.GetResultType();
                                            }
                                            bRes = false;
                                            Logger_AddLogException(ex, "PermitsGetStreetSections::WebException", LogLevels.logERROR);
                                        }
                                        catch (Exception ex)
                                        {
                                            bAuthRetry = false;
                                            bRes = false;
                                            Logger_AddLogException(ex, "PermitsGetStreetSections::Exception", LogLevels.logERROR);
                                        }
                                    }
                                }

                            }

                            if (bRes && oStreetSectionsData.Any())
                            {

                                oStreetSectionsData = oStreetSectionsData.OrderBy(r => r.Id).ToList();

                                MapPoint[] oLstContainerPolygon = new MapPoint[4];

                                oLstContainerPolygon[0] = new MapPoint() { x = xmin, y = ymax };
                                oLstContainerPolygon[1] = new MapPoint() { x = xmax, y = ymax };
                                oLstContainerPolygon[2] = new MapPoint() { x = xmax, y = ymin };
                                oLstContainerPolygon[3] = new MapPoint() { x = xmin, y = ymin };

                                List<MapPoint> oXGridPoints = new List<MapPoint>();
                                List<MapPoint> oYGridPoints = new List<MapPoint>();

                                decimal dXDistance = GetDistanceKM(oLstContainerPolygon[0], oLstContainerPolygon[1]);
                                decimal dYDistance = GetDistanceKM(oLstContainerPolygon[1], oLstContainerPolygon[2]);
                                decimal dGridSize = GRID_SIZE_KM;
                                decimal dXDistanceProp = dGridSize / dXDistance;
                                decimal dYDistanceProp = dGridSize / dYDistance;

                                oLstContainerPolygon[0].x -= (oLstContainerPolygon[1].x - oLstContainerPolygon[0].x) * dXDistanceProp;
                                oLstContainerPolygon[0].y += (oLstContainerPolygon[0].y - oLstContainerPolygon[3].y) * dYDistanceProp;
                                oLstContainerPolygon[1].x += (oLstContainerPolygon[1].x - oLstContainerPolygon[0].x) * dXDistanceProp;
                                oLstContainerPolygon[1].y += (oLstContainerPolygon[0].y - oLstContainerPolygon[3].y) * dYDistanceProp;
                                oLstContainerPolygon[2].x += (oLstContainerPolygon[1].x - oLstContainerPolygon[0].x) * dXDistanceProp;
                                oLstContainerPolygon[2].y -= (oLstContainerPolygon[0].y - oLstContainerPolygon[3].y) * dYDistanceProp;
                                oLstContainerPolygon[3].x -= (oLstContainerPolygon[1].x - oLstContainerPolygon[0].x) * dXDistanceProp;
                                oLstContainerPolygon[3].y -= (oLstContainerPolygon[0].y - oLstContainerPolygon[3].y) * dYDistanceProp;

                                dXDistance = GetDistanceKM(oLstContainerPolygon[0], oLstContainerPolygon[1]);
                                dYDistance = GetDistanceKM(oLstContainerPolygon[1], oLstContainerPolygon[2]);
                                dXDistanceProp = dGridSize / dXDistance;
                                dYDistanceProp = dGridSize / dYDistance;



                                oXGridPoints.Add(oLstContainerPolygon[0]);

                                int i = 1;
                                decimal dTempDistance = 0;
                                while (dTempDistance < dXDistance)
                                {
                                    decimal dNextX = oLstContainerPolygon[0].x + (oLstContainerPolygon[1].x - oLstContainerPolygon[0].x) * i * dXDistanceProp;
                                    oXGridPoints.Add(new MapPoint() { x = dNextX, y = oLstContainerPolygon[0].y });
                                    dTempDistance = GetDistanceKM(oXGridPoints[0], oXGridPoints[i]);
                                    i++;
                                }


                                oYGridPoints.Add(oLstContainerPolygon[0]);
                                i = 1;
                                dTempDistance = 0;
                                while (dTempDistance < dYDistance)
                                {
                                    decimal dNextY = oLstContainerPolygon[0].y - (oLstContainerPolygon[0].y - oLstContainerPolygon[3].y) * i * dYDistanceProp;
                                    oYGridPoints.Add(new MapPoint() { x = oLstContainerPolygon[0].x, y = dNextY });
                                    dTempDistance = GetDistanceKM(oYGridPoints[0], oYGridPoints[i]);
                                    i++;
                                }


                                int id = 1;

                                i = 0;
                                int j;
                                while (i < oXGridPoints.Count() - 1)
                                {
                                    j = 0;
                                    while (j < oYGridPoints.Count() - 1)
                                    {

                                        List<MapPoint> oPolygon = new List<MapPoint>();
                                        oPolygon.Add(new MapPoint() { x = oXGridPoints[i].x, y = oYGridPoints[j].y });
                                        oPolygon.Add(new MapPoint() { x = oXGridPoints[i + 1].x, y = oYGridPoints[j].y });
                                        oPolygon.Add(new MapPoint() { x = oXGridPoints[i + 1].x, y = oYGridPoints[j + 1].y });
                                        oPolygon.Add(new MapPoint() { x = oXGridPoints[i].x, y = oYGridPoints[j + 1].y });
                                        oGrid.Add(id, new GridElement()
                                        {
                                            id = id,
                                            description = string.
                                                Format("Grid({0},{1})", i, j),
                                            Polygon = oPolygon,
                                            ReferenceCount = 0,
                                            LstStreetSections = new List<StreetSectionData>(),
                                            x = i,
                                            y = j,
                                            maxX = oXGridPoints.Count() - 2,
                                            maxY = oYGridPoints.Count() - 2
                                        });
                                        id++;
                                        j++;
                                    }
                                    i++;
                                }


                                foreach (StreetSectionData oData in oStreetSectionsData.Where(r => !r.Deleted))
                                {

                                    foreach (MapPoint oPoint in oData.GeometryCoordinates)
                                    {
                                        bool bInside = false;
                                        int iId = -1;

                                        foreach (KeyValuePair<int, GridElement> entry in oGrid)
                                        {
                                            bInside = IsPointInsidePolygon(oPoint, entry.Value.Polygon);
                                            if (bInside)
                                            {
                                                iId = entry.Key;
                                                break;
                                            }
                                        }

                                        if (!bInside)
                                        {
                                            Console.Write("Error");
                                        }
                                        else
                                        {
                                            if (!oData.oGridElements.ContainsKey(iId))
                                            {
                                                oGrid[iId].ReferenceCount++;
                                                oGrid[iId].LstStreetSections.Add(oData);
                                                oData.oGridElements[iId] = oGrid[iId];
                                            }
                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            //rtRes = ResultType.Result_Error_InvalidAuthentication;
                            bRes = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = ex;
                bRes = false;
                Logger_AddLogException(ex, "PermitsGetStreetSections::Exception", LogLevels.logERROR);
            }

            return bRes;
        }

        private bool IsPointInsidePolygon(MapPoint p, List<MapPoint> Polygon)
        {
            decimal dAngle = 0;

            try
            {

                for (int i = 0; i < Polygon.Count; i++)
                {
                    System.Windows.Vector v1 = new System.Windows.Vector(Convert.ToDouble(Polygon[i].x - p.x), Convert.ToDouble(Polygon[i].y - p.y));
                    System.Windows.Vector v2 = new System.Windows.Vector(Convert.ToDouble(Polygon[(i + 1) % Polygon.Count].x - p.x),
                                           Convert.ToDouble(Polygon[(i + 1) % Polygon.Count].y - p.y));

                    dAngle = dAngle + Convert.ToDecimal((System.Windows.Vector.AngleBetween(v1, v2) * Math.PI / 180));

                }
            }
            catch (Exception e)
            {

                dAngle = 0;
            }


            return (Math.Abs(Convert.ToDouble(dAngle)) > Math.PI);
        }

        private decimal GetDistanceKM(MapPoint A, MapPoint B)
        {
            double aStartLat = ConvertToRadians(A.y);
            double aStartLong = ConvertToRadians(A.x);
            double aEndLat = ConvertToRadians(B.y);
            double aEndLong = ConvertToRadians(B.x);

            double distance = Math.Acos(Math.Sin(aStartLat) * Math.Sin(aEndLat)
                    + Math.Cos(aStartLat) * Math.Cos(aEndLat)
                    * Math.Cos(aEndLong - aStartLong));

            return Convert.ToDecimal(EARTH_RADIUS_KM * distance);
        }

        private double ConvertToRadians(decimal angle)
        {
            return (Math.PI / 180) * Convert.ToDouble(angle);
        }
    }

}
