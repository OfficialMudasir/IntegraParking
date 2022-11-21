using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace integraMobile.OxxoWS.WS.Data
{
    [Serializable]
    public class WSUserPreferences
    {
        private List<WSPrefPlate> m_oPrefPlates;
        private List<WSFavArea> m_oFavAreas;

        private string m_sLastUsedPlate;
        private string m_sMostUsedPlate;
        private int m_iLastUsedZone;
        private int m_iLastTariff;
        private int m_iMostUsedZone;
        private int m_iMostUsedTariff;

        public WSUserPreferences()
        {
        }

        public WSUserPreferences(SortedList oParameters)
        {
            m_oPrefPlates = new List<WSPrefPlate>();
            m_oFavAreas = new List<WSFavArea>();

            if (oParameters.GetValueInt("userDATA_num") >= 1)
            {
                if (oParameters.GetValueInt("userDATA_0_userpreferences_num") >= 1)
                {
                    if (oParameters.GetValueInt("userDATA_0_userpreferences_0_prefplates_num") >= 1)
                    {
                        for (int i = 0; i < oParameters.GetValueInt("userDATA_0_userpreferences_0_prefplates_0_prefplate_num"); i++)
                        {
                            m_oPrefPlates.Add(new WSPrefPlate()
                            {
                                City = oParameters.GetValueInt(string.Format("userDATA_0_userpreferences_0_prefplates_0_prefplate_{0}_city", i)),
                                Plate = oParameters.GetValueString(string.Format("userDATA_0_userpreferences_0_prefplates_0_prefplate_{0}_lp", i))
                            });
                        }
                    }

                    if (oParameters.GetValueInt("userDATA_0_userpreferences_0_favareas_num") >= 1)
                    {
                        for (int i = 0; i < oParameters.GetValueInt("userDATA_0_userpreferences_0_favareas_0_favarea_num"); i++)
                        {
                            m_oFavAreas.Add(new WSFavArea()
                            {
                                City = oParameters.GetValueInt(string.Format("userDATA_0_userpreferences_0_favareas_0_favarea_{0}_city", i)),
                                Group = oParameters.GetValueInt(string.Format("userDATA_0_userpreferences_0_favareas_0_favarea_{0}_sector", i)),
                                Rate = oParameters.GetValueInt(string.Format("userDATA_0_userpreferences_0_favareas_0_favarea_{0}_rate", i))
                            });
                        }
                    }

                    m_sLastUsedPlate = oParameters.GetValueString("userDATA_0_userpreferences_0_lup");
                    m_sMostUsedPlate = oParameters.GetValueString("userDATA_0_userpreferences_0_mup");
                    m_iLastUsedZone = oParameters.GetValueInt("userDATA_0_userpreferences_0_luz");
                    m_iLastTariff = oParameters.GetValueInt("userDATA_0_userpreferences_0_tarifluz");
                    m_iMostUsedZone = oParameters.GetValueInt("userDATA_0_userpreferences_0_muz");
                    m_iMostUsedTariff = oParameters.GetValueInt("userDATA_0_userpreferences_0_tarifmuz");
                }
            }
            else
            {
                var userpreferences = oParameters.GetOutputElement("userDATA/userpreferences");
                if (userpreferences["prefplates"].GetType() == typeof(ArrayList))
                {
                    var preflates = oParameters.GetOutputElementArray("userDATA/userpreferences/prefplates");
                    SortedList oPrefPlate;
                    foreach (SortedList oItem in preflates)
                    {
                        oPrefPlate = (SortedList)((ArrayList)oItem["prefplate"])[0];
                        m_oPrefPlates.Add(new WSPrefPlate()
                        {
                            City = oPrefPlate.GetValueInt("city"),
                            Plate = oPrefPlate.GetValueString("lp")
                        });
                    }
                }

                if (userpreferences["favareas"].GetType() == typeof(ArrayList))
                {
                    var favareas = oParameters.GetOutputElementArray("userDATA/userpreferences/favareas");
                    SortedList oFavArea;
                    foreach (SortedList oItem in favareas)
                    {
                        oFavArea = (SortedList)((ArrayList)oItem["favarea"])[0];
                        m_oFavAreas.Add(new WSFavArea()
                        {
                            City = oFavArea.GetValueInt("city"),
                            Group = oFavArea.GetValueInt("sector"),
                            Rate = oFavArea.GetValueInt("rate")
                        });
                    }
                }

                m_sLastUsedPlate = userpreferences.GetValueString("lup");
                m_sMostUsedPlate = userpreferences.GetValueString("mup");
                m_iLastUsedZone = userpreferences.GetValueInt("luz");
                m_iLastTariff = userpreferences.GetValueInt("tarifluz");
                m_iMostUsedZone = userpreferences.GetValueInt("muz");
                m_iMostUsedTariff = userpreferences.GetValueInt("tarifmuz");

            }
        }

        public List<WSPrefPlate> PrefPlates
        {
            get { return m_oPrefPlates; }
            set { m_oPrefPlates = value; }
        }
        public List<WSFavArea> FavAreas
        {
            get { return m_oFavAreas; }
            set { m_oFavAreas = value; }
        }
        public string LastUsedPlate
        {
            get { return m_sLastUsedPlate; }
            set { m_sLastUsedPlate = value; }
        }
        public string MostUsedPlate
        {
            get { return m_sMostUsedPlate; }
            set { m_sMostUsedPlate = value; }
        }
        public int LastUsedZone
        {
            get { return m_iLastUsedZone; }
            set { m_iLastUsedZone = value; }
        }
        public int LastTariff
        {
            get { return m_iLastTariff; }
            set { m_iLastTariff = value; }
        }
        public int MostUsedZone
        {
            get { return m_iMostUsedZone; }
            set { m_iMostUsedZone = value; }
        }
        public int MostUsedTariff
        {
            get { return m_iMostUsedTariff; }
            set { m_iMostUsedTariff = value; }
        }

        public WSFavArea GetFavArea(int iCityId, List<WSZone> oZones)
        {
            WSFavArea oRet = this.m_oFavAreas.Where(i => i.City == iCityId).FirstOrDefault();

            if (oRet == null)
            {
                if (this.LastUsedZone > 0 && this.LastTariff > 0 && !string.IsNullOrEmpty(oZones.GetDescription(this.LastUsedZone)))
                    oRet = new WSFavArea(oZones) { City = iCityId, Group = this.LastUsedZone, Rate = this.LastTariff };
                else if (this.MostUsedZone > 0 && this.MostUsedTariff > 0 && !string.IsNullOrEmpty(oZones.GetDescription(this.MostUsedZone)))
                    oRet = new WSFavArea(oZones) { City = iCityId, Group = this.MostUsedZone, Rate = this.MostUsedTariff };
            }
            else            
            {
                oRet.Zones = oZones;
            }

            return oRet;
        }

    }

    [Serializable]
    public class WSPrefPlate
    {
        private int m_iCity;
        private string m_sPlate;

        public WSPrefPlate()
        {

        }

        public int City
        {
            get { return m_iCity; }
            set { m_iCity = value; }
        }
        public string Plate
        {
            get { return m_sPlate; }
            set { m_sPlate = value; }
        }

    }

    [Serializable]
    public class WSFavArea
    {
        private int m_iCity;
        private int m_iGroup;
        private int m_iRate;

        private List<WSZone> m_oZones = null;

        public WSFavArea()
        {

        }
        public WSFavArea(List<WSZone> oZones)
        {
            m_oZones = oZones;
        }

        public int City
        {
            get { return m_iCity; }
            set { m_iCity = value; }
        }
        public int Group
        {
            get { return m_iGroup; }
            set { m_iGroup = value; }
        }
        public int Rate
        {
            get { return m_iRate; }
            set { m_iRate = value; }
        }

        public List<WSZone> Zones
        {
            set { m_oZones = value; }
        }

        public string GroupDescription
        {
            get
            {
                string sRet = "";
                if (m_oZones != null)
                {
                    sRet = m_oZones.GetDescription(m_iGroup);
                }
                return sRet;
            }
        }

    }

}