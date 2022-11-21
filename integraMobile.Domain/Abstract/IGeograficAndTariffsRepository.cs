using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using integraMobile.Infrastructure.PermitsAPI;

namespace integraMobile.Domain.Abstract
{
    #region STRUCT
    public struct stZone
    {
        public int level;
        public decimal dID;
        public string strDescription;
        public decimal dLiteralID;
        public string strShowId;
        public string strColour;
        public IEnumerable<stZone> subzones;
        public IEnumerable<stGPSPolygon> GPSpolygons;
        public GroupType GroupType;
        public float Occupancy;
        public int ParkingType;
        public DateTime dtIniApply;
        public DateTime dtEndApply;
        public stGPSPoint center;
        public bool allowByPassMap;
        public int? permitMaxMonths;
        public int? permitMaxBuyDay;
        public decimal? dMessageLiteralID;
        public string message;
    }

    public struct stTariff
    {
        public decimal dID;
        public string strDescription;
        public decimal dLiteralID;
        public bool bUserSelectable;
        public IEnumerable<decimal> zones;
        public IEnumerable<stTariffZone> tariffZones;
        public TariffType tariffType;
        public int maxPlates;        
        public int permitMaxNum;
        public TariffBehavior tariffBehavior;
        public TariffShopkeeperBehavior tariffShopkeeperBehavior;
        public ulong ulMinVersion;
        public ulong ulMaxVersion;
        public int polygonShow;
        public string polygonColour;
        public int polygonZ;
        public string polygonMapDescription;
        public int? tariffAutoStart;
        public int? tariffRestartTariff;
        public decimal? tariffServiceType;
        public int? tariffTypeOfServiceType;
        public string tarSerLitUnderWheel;
        public string tarSerLitButtonStop;
        public string tarSerLitEndParking;
        public string tarSerLitButtonEndParking;
        public CardPayment_Mode tarCardPaymentMode;
    }

    public struct stTariffZone
    {
        public decimal dID;
        public List<stTariffZoneApplicationPeriods> applicationPeriods;
        public IEnumerable<stGPSPolygon> GPSpolygons;
                  
    }

    public struct stTariffZoneApplicationPeriods
    {
        public bool bUserSelectable;
        public DateTime dtIniApply;
        public DateTime dtEndApply; 
    }

    public struct stGPSPolygon
    {
        public int iPolNumber;
        public IEnumerable<stGPSPoint> GPSpolygon;
    }
    
    public struct stGPSPoint    
    {
        public decimal order;
        public decimal dLatitude;
        public decimal dLongitude;
        public DateTime dtIniApply;
        public DateTime dtEndApply;
    }

    public struct stCityPolygons
    {
        public IEnumerable<stCityPolygon> citipolygon;
    }

    public struct stCityPolygon
    {
        public decimal citiPolygonId;
        public string colour;
        public IEnumerable<stGPSPolygon> polygon;
        public decimal? dMessageLiteralID;
        public string message;
    }
    //public struct stMessage
    //{
    //    public IEnumerable<stLine> line;
    //}

    //public struct stLine
    //{
    //    public string text;
    //    public IEnumerable<String> urls;
    //}

    #endregion

    #region ENUM
    public enum FineWSSignatureType
    {
        fst_test = 0,
        fst_internal = 1,
        fst_standard = 2,
        fst_eysa =3,
        fst_gtechna = 4,
        fst_madidplatform = 5,
        fst_santboi = 6,
        fst_valoriza = 7,
        fst_mifas = 8,
        fst_bsm = 9,
        fst_emisalba = 10,
        fst_bilbao_integration = 11,
        fst_madrid2platform = 12
    }

    public enum ParkWSSignatureType
    {
        pst_test = 0,
        pst_internal = 1,
        pst_standard_time_steps = 2,
        pst_eysa = 3,
        pst_standard_amount_steps = 4,
        pst_bsm = 5,
        pst_bilbao_integration = 6
    }

    public enum UnParkWSSignatureType
    {
        upst_test = 0,
        upst_internal = 1,
        upst_standard = 2,
        upst_eysa = 3,
        upst_bsm = 4,
        upst_bilbao_integration = 5

    }

    public enum OperationConfirmMode
    {
        online = 0,
        offline = 1,
        first_online = 2
    }

    public enum ConfirmParkWSSignatureType
    {
        cpst_test = 0,
        cpst_internal = 1,
        cpst_standard = 2,
        cpst_eysa = 3,
        cpst_gtechna = 4,
        cpst_nocall = 5,
        cpst_standardmadrid = 6,
        cpst_bsm = 7,
        cpst_madridplatform = 8,
        cpst_bilbao_integration = 9,
        cpst_madrid2platform = 10,
        cpst_SIR = 11
    }

    public enum UserReplicationWSSignatureType
    {
        urst_Zendesk = 1,
    }

    public enum ConfirmEntryOffstreetWSSignatureType
    {
        test = 0,
        meypar = 1,
        no_call = 2
    }

    public enum QueryExitOffstreetWSSignatureType
    {
        test = 0,
        meypar = 1,
        iparkcontrol = 2,
        no_call = 3,
        meyparAdventa = 4
    }

    public enum ConfirmExitOffstreetWSSignatureType
    {
        test = 0,
        meypar = 1,
        iparkcontrol = 2,
        no_call = 3,
        meyparAdventa = 4
    }

    public enum StreetSectionsUpdateSignatureType
    {
        no_call = 0,
        Permits = 1,
    }

    public enum RechargeValuesTypes
    {
        rvt_ManualRecharge = 1,
        rvt_AutomaticRecharge = 2,
        rvt_AutomaticRechargeBelow = 3,
        rvt_SignUp = 4,
        rvt_RechargeChangePay = 5,
        rvt_RechargePagatelia = 6,
        rvt_RechargePaypal= 7,
        rvt_BalanceTransfer = 8,
        rvt_OxxoRecharge = 9,
        rvt_ShopKeeperBalanceTransfer = 10
    }

    /// <summary>
    /// 1; No restriction, 2:Start at ticket time, 3: Start at 0:00 of next day
    /// </summary>

    public enum TicketTypeFeaturePeriodType
    {
        NoRestriction = 1,
        StartAtTicketType= 2,
        StartAt00NextDay=3
    }

    public enum TariffType
    {
        RegularTariff = 0,
        PermitTariff = 1,
    }

    public enum TariffBehavior
    {
        Standard = 0,
        StartStop=1,
        StartStopHybrid = 2
    }

   
    public enum TariffShopkeeperBehavior
    {
        AllUsers = 0,
        OnlyUsers = 1,
        OnlyShopkeepers=2
    }

    public enum CardPayment_Mode
    {
        Charge = 0,
        Authorization = 1,
        AuthorizationPreferably = 2
    }

    #endregion

    #region INTERFACE
    public interface IGeograficAndTariffsRepository
    {
        bool getInstallation(decimal? dInstallationId, decimal? dLatitude, decimal? dLongitude, 
                             ref INSTALLATION oInstallation, ref DateTime ?dtInsDateTime);
        bool getInstallation(decimal? dInstallationId, decimal? dLatitude, decimal? dLongitude, decimal? dUserCurrencyId,
                             ref INSTALLATION oInstallation, ref DateTime? dtInsDateTime, out bool bValidCurrency);
        bool getInstallation(decimal? dInstallationId, decimal? dLatitude, decimal? dLongitude, decimal? dUserCurrencyId, bool bGetSuperInstallation,
                             ref INSTALLATION oInstallation, ref DateTime? dtInsDateTime, out bool bValidCurrency);
        bool getInstallationById(decimal? dInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime);
        bool getInstallationByStandardId(string strStandardInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime);
        bool getInstallationByStandardIdWebPortal(string strStandardInstallationId,
                                   ref INSTALLATION oInstallation,
                                   ref DateTime? dtInsDateTime);

        IEnumerable<INSTALLATION> getInstallationsList(decimal? dCurrencyId = null);        
        bool getGroup(decimal? dGroupId, ref GROUP oGroup, ref DateTime? dtgroupDateTime);
        DateTime? getInstallationDateTime(decimal dInstallationId);
        DateTime? ConvertInstallationDateTimeToUTC(decimal dInstallationId, DateTime dtInstallation);
        DateTime? ConvertUTCToInstallationDateTime(decimal dInstallationId, DateTime dtUTC);
        int? GetInstallationUTCOffSetInMinutes(decimal dInstallationId);
        DateTime? getGroupDateTime(decimal dGroupID);
        IEnumerable<stZone> getInstallationGroupHierarchy(decimal dInstallationId, List<GroupType> groupTypes, int? lang, bool filterOnlyPermitRatesGroups);
        IEnumerable<stTariff> getInstallationTariffs(decimal dInstallationId, decimal? lang);
        IEnumerable<stZone> getInstallationGroupHierarchy2(decimal dInstallationId, List<GroupType> groupTypes,bool filterOnlyPermitRatesGroups);
        IEnumerable<stTariff> getInstallationTariffs2(decimal dInstallationId, decimal? lang);
        IEnumerable<stCityPolygon> getInstallationPolygons(decimal dInstallationId, int? lang);


        IEnumerable<stTariff> getGroupTariffs(decimal dGroupId, decimal? lang);
        IEnumerable<stTariff> getGroupTariffs(decimal dGroupId, decimal? dLatitude, decimal? dLongitude, decimal? lang);
        IEnumerable<stTariff> getPlateTariffsInGroup(string strPlate, decimal dGroupId, decimal? dLatitude, decimal? dLongitude, decimal? lang);

        bool GetGroupAndTariffExternalTranslation(int iWSNumber, GROUP oGroup, TARIFF oTariff, ref string strExtGroupId, ref string strExtTarId);
        bool GetGroupAndTariffExternalTranslation(int iWSNumber, decimal dGroupId, decimal dTariffId, ref string strExtGroupId, ref string strExtTarId);
        bool GetGroupAndTariffFromExternalId(int iWSNumber, DateTime dt, INSTALLATION oInstallation, string strExtGroupId, string strExtTarId, ref decimal? dGroupId, ref decimal? dTariffId);
        bool GetStreetSectionFromExternalId(decimal dInstallationId, string sExtStreetSectionId, ref decimal? dStreetSecionId);

        bool GetGroupAndTariffFromExternalId(int iWSNumber, DateTime dt, INSTALLATION oInstallation, string strExtGroupId, string strExtTarId, decimal dInTariffId, ref decimal? dGroupId, ref decimal? dTariffId);

        bool GetGroupAndTariffStepOffsetMinutes(GROUP oGroup, TARIFF oTariff, out int? iOffset);

        bool GetSyncInstallation(long lVersionFrom, out INSTALLATIONS_SYNC[] oArrSync);
        bool GetSyncInstallationGeometry(long lVersionFrom, out INSTALLATIONS_GEOMETRY_SYNC[] oArrSync, int ?iMaxRegistriesToReturn);

        decimal GetSyncInstallationCurrentVersion();
        decimal GetSyncInstallationGeometryCurrentVersion();

        bool getExternalProvider(string strName, ref EXTERNAL_PROVIDER oExternalProvider);


        bool getOffStreetConfiguration(decimal? dGroupId, decimal? dLatitude, decimal? dLongitude, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime);
        bool getOffStreetConfigurationByExtOpsId(string sExtParkingId, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime);
        bool getOffStreetConfigurationByExtOpsId(string sExtParkingId, int iTerminalId, ref GROUPS_OFFSTREET_WS_CONFIGURATION oOffstreetConfiguration, ref DateTime? dtgroupDateTime);
        bool getGroupByExtOpsId(string sExtGroupId, ref GROUP oGroup, ref DateTime? dtgroupDateTime);

        bool GetFinanDistOperator(decimal dFinanDistOperatorId, ref FINAN_DIST_OPERATOR oFinanDistOperator);

        bool getTicketTypePaymentInfo(decimal dInsId, DateTime dtInsDate, DateTime dtTicketDate, string strTicketTypeExtID, out bool bIsPayable, out DateTime? dtMaxPayDate, out int iAmount, out string strDesc1, out string strDesc2);
        bool ExistTicketPayment(decimal dInsId, string strTicketNumber);

        List<decimal> GetChildInstallationsIds(decimal dInstallationId, DateTime? dtNow, bool bIncludeId = true);
        List<INSTALLATION> GetChildInstallations(decimal dInstallationId, DateTime? dtNow, bool bIncludeId = true);
        decimal? GetSuperInstallationId(decimal dInstallationId, DateTime? dtNow = null);
        decimal? GetSuperInstallationId(decimal dInstallationId, DateTime? dtNow, integraMobileDBEntitiesDataContext dbContext);
        INSTALLATION GetSuperInstallation(decimal dInstallationId, DateTime? dtNow = null);
        decimal GetDefaultSourceApp();
        decimal GetSourceApp(string strCode);
        string GetSourceAppCode(decimal dSourceApp);
        string GetSourceAppDescription(decimal dSourceApp);

        List<INSTALLATIONS_GEOMETRY> GetInstallationsGeometries(decimal dInstallationId, int? iMaxRegistriesToReturn = null);

        bool GetStreetSectionsUpdateInstallations(out List<INSTALLATION> oInstallations);

        bool GetInstallationStreets(decimal dInstallationID, out List<STREET> oStreets);

        bool UpdateStreets(decimal dInstallationID, 
                           ref List<StreetData> oInsertStreetsData,
                           ref List<StreetData> oUpdateStreetsData,
                           ref List<StreetData> oDeleteStreetsData);

        //bool GetInstallationsStreetSections(decimal dInstallationID, out List<StreetSectionData> oStreetSectionsData, out Dictionary<int, GridElement> oGrid);

        bool RecreateStreetSectionsGrid(decimal dInstallationID, ref Dictionary<int, GridElement> oGrid);
        bool UpdateStreetSections(decimal dInstallationID, bool bGridRecreated,
                                  ref List<StreetSectionData> oInsertStreetSectionsData,
                                  ref List<StreetSectionData> oUpdateStreetSectionsData,
                                  ref List<StreetSectionData> oDeleteStreetSectionsData);

        bool GetPackageFileData(decimal dInstallationID, out Dictionary<decimal, STREET> oStreets, out List<STREET_SECTION> oStreetSections,
                                out List<STREET_SECTIONS_GRID> oGrid, out int iPackageNextVersion);

        bool GetStreetSectionsExternalIds(decimal dInstallationID, out string[] oStreetSections);

        bool GetStreetSection(decimal dStreetSectionId, out STREET_SECTION oStreetSection);

    }
    #endregion
}
