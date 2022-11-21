using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Offstreet.Test.WS.Data
{
    [Serializable]
    public class WSZoneTar
    {
        public List<WSZone> m_oZones;
        public List<WSZoneOffstreet> m_oZonesOffstreet;
        public List<WSTariff> m_oTariffs;

        public WSZoneTar()
        {

        }

        public WSZoneTar(SortedList oParameters)
        {
            m_oZones = new List<WSZone>();

            if (oParameters.GetValueInt("ZoneTar_num") >= 1)
            {
                for (int i = 0; i < oParameters.GetValueInt("ZoneTar_0_zone_num"); i++)
                {
                    var oZone = new WSZone()
                    {
                        Id = oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_id", i)),
                        Description = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_desc", i)),
                        Literal = oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_lit", i)),
                        NumDesc = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_numdesc", i)),
                        Colour = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_colour", i))
                    };
                    if (oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_subzone_num", i)) > 0)
                    {
                        for (int j = 0; j < oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_subzone_num", i)); j++)
                        {
                            oZone.SubZones.Add(new WSZone()
                            {
                                Id = oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_id", i, j)),
                                Description = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_desc", i, j)),
                                Literal = oParameters.GetValueInt(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_lit", i, j)),
                                NumDesc = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_numdesc", i, j)),
                                Colour = oParameters.GetValueString(string.Format("ZoneTar_0_zone_{0}_subzone_{1}_colour", i, j))
                            });
                        }
                    }
                    m_oZones.Add(oZone);
                }
            }

            m_oZonesOffstreet = new List<WSZoneOffstreet>();
            if (oParameters.GetValueInt("ZoneTarOffstreet_num") >= 1)
            {
                for (int i = 0; i < oParameters.GetValueInt("ZoneTarOffstreet_0_zone_num"); i++)
                {
                    var oZone = new WSZoneOffstreet()
                    {
                        Id = oParameters.GetValueInt(string.Format("ZoneTarOffstreet_0_zone_{0}_id", i)),
                        Description = oParameters.GetValueString(string.Format("ZoneTarOffstreet_0_zone_{0}_desc", i)),
                        Literal = oParameters.GetValueInt(string.Format("ZoneTarOffstreet_0_zone_{0}_lit", i)),
                        NumDesc = oParameters.GetValueString(string.Format("ZoneTarOffstreet_0_zone_{0}_numdesc", i)),
                        Colour = oParameters.GetValueString(string.Format("ZoneTarOffstreet_0_zone_{0}_colour", i)),
                        Occupancy = oParameters.GetValueInt(string.Format("ZoneTarOffstreet_0_zone_{0}_occupancy", i)),
                        OffparkingType = oParameters.GetValueString(string.Format("ZoneTarOffstreet_0_zone_{0}_offparking_type", i))
                    };
                    m_oZonesOffstreet.Add(oZone);
                }
            }

            m_oTariffs = new List<WSTariff>();
            if (oParameters.GetValueInt("InfoTAR_num") >= 1)
            {
                for (int i = 0; i < oParameters.GetValueInt("InfoTAR_0_ad_num"); i++)
                {
                    var oTariff = new WSTariff()
                    {
                        Id = oParameters.GetValueInt(string.Format("InfoTAR_0_ad_{0}_id", i)),
                        Description = oParameters.GetValueString(string.Format("InfoTAR_0_ad_{0}_desc", i)),
                        Literal = oParameters.GetValueInt(string.Format("InfoTAR_0_ad_{0}_lit", i)),
                        Selectable = (oParameters.GetValueInt(string.Format("InfoTAR_0_ad_{0}_sel", i)) == 1)
                    };
                    if (oParameters.GetValueInt(string.Format("InfoTAR_0_ad_{0}_szs_num", i)) > 0)
                    {
                        oTariff.SubZones = oParameters.GetValueString(string.Format("InfoTAR_0_ad_{0}_szs_0_sz", i)).Split('~')
                                                                                                                    .Where(item => !string.IsNullOrEmpty(item))
                                                                                                                    .Select(item => Convert.ToInt32(item))
                                                                                                                    .ToList();
                    }
                    m_oTariffs.Add(oTariff);
                }
            }
                 
        }

        public List<WSZone> Zones
        {
            get { return m_oZones; }
            set { m_oZones = value; }
        }

        public List<WSZoneOffstreet> ZonesOffstreet
        {
            get { return m_oZonesOffstreet; }
            set { m_oZonesOffstreet = value; }
        }

        public List<WSTariff> Tariffs
        {
            get { return m_oTariffs; }
            set { m_oTariffs = value; }
        }

    }

    [Serializable]
    public class WSZone
    {
        private int m_iId;
        private string m_sDescription;
        private int m_iLiteral;
        private string m_sNumDesc;
        private string m_sColour;

        private List<WSZone> m_oSubZones = new List<WSZone>();

        public WSZone()
        {

        }

        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }

        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public int Literal
        {
            get { return m_iLiteral; }
            set { m_iLiteral = value; }
        }

        public string NumDesc
        {
            get { return m_sNumDesc; }
            set { m_sNumDesc = value; }
        }

        public string Colour
        {
            get { return m_sColour; }
            set { m_sColour = value; }
        }

        public List<WSZone> SubZones
        {
            get { return m_oSubZones; }
            set { m_oSubZones = value; }
        }

    }

    [Serializable]
    public class WSZoneOffstreet
    {
        private int m_iId;
        private string m_sDescription;
        private int m_iLiteral;
        private string m_sNumDesc;
        private string m_sColour;

        private int m_iOccupancy;
        private string m_sOffparkingType; 
        
        public WSZoneOffstreet()
        {

        }

        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }

        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public int Literal
        {
            get { return m_iLiteral; }
            set { m_iLiteral = value; }
        }

        public string NumDesc
        {
            get { return m_sNumDesc; }
            set { m_sNumDesc = value; }
        }

        public string Colour
        {
            get { return m_sColour; }
            set { m_sColour = value; }
        }

        public int Occupancy
        {
            get { return m_iOccupancy; }
            set { m_iOccupancy = value; }
        }

        public string OffparkingType
        {
            get { return m_sOffparkingType; }
            set { m_sOffparkingType = value; }
        }

    }

    [Serializable]
    public class WSTariff
    {
        private int m_iId;
        private string m_sDescription;
        private int m_iLiteral;
        private bool m_bSelectable;

        private List<int> m_oSubZones = new List<int>();

        public WSTariff()
        {

        }

        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }

        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public int Literal
        {
            get { return m_iLiteral; }
            set { m_iLiteral = value; }
        }

        public bool Selectable
        {
            get { return m_bSelectable; }
            set { m_bSelectable = value; }
        }

        public List<int> SubZones
        {
            get { return m_oSubZones; }
            set { m_oSubZones = value; }
        }

    }
}