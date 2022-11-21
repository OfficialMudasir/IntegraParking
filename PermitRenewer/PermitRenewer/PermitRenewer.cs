using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;
using integraMobile.Domain.Helper;
using integraMobile.ExternalWS;
using System.Collections;
using integraMobile.Domain.Concrete;

namespace PermitRenewer
{
    class PermitRenewer
    {
        #region Member Variables

        private IKernel m_kernel = null;
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(PermitRenewer));
        private integraMobile.ExternalWS.WSintegraMobile wswi = null;
        private string SessionId = string.Empty;
        List<decimal> ValidGroups = null;
        List<Tariff> ValidTariffs = null;
        List<string> ValidPlates = null;
        List<TimeStep> ValidTimeSteps = null;

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IBackOfficeRepository backofficeRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infrastructureRepository { get; set; }
        [Inject]
        public SQLGeograficAndTariffsRepository geographicRepository { get; set; }

        #endregion

        #region Constructor / Destructor

        public PermitRenewer()
		{
            m_kernel = new StandardKernel(new PermitRenewerModule());
            m_kernel.Inject(this);

            wswi = new integraMobile.ExternalWS.WSintegraMobile(backofficeRepository, customersRepository, infrastructureRepository, geographicRepository);

        }

        #endregion 

        #region Thread Body

        public void Main()
		{
            Log(LogLevels.logDEBUG, ">> PermitRenewer::Main");

            List<OPERATION> PermitOperationsToRenew = null;
            List<PermitCollection> PermitsToRenew = null;

            /* Set status NULL to 0 on renewable permits */
            customersRepository.UpdateOperationsToRenew(out PermitOperationsToRenew);
            Log(LogLevels.logDEBUG, string.Format("PermitRenewer::UpdateOperationsToRenew. {0} operations selected", PermitOperationsToRenew.Count));
            if (PermitOperationsToRenew.Count > 0)
            {
                /* Group operations in permits */
                PermitsToRenew = GroupPermits(PermitOperationsToRenew);
                Log(LogLevels.logDEBUG, string.Format("PermitRenewer::GroupPermits. {0} operations groupped into {1} permits", PermitOperationsToRenew.Count, PermitsToRenew.Count));
                /* Renew permits */
                RenewPermits(PermitsToRenew);
            }            
            Log(LogLevels.logDEBUG, "<< PermitRenewer::Main");
        }

		#endregion

        #region Permits process

        private List<PermitCollection> GroupPermits(List<OPERATION> PermitOperationsToRenew)
        {
            Log(LogLevels.logDEBUG, ">> PermitRenewer::GroupPermits");

            List<PermitCollection> PermitsGroupped = new List<PermitCollection>();

            try
            {
                foreach (OPERATION op in PermitOperationsToRenew)
                {
                    Permit p = new Permit();
                    p.OperationId = op.OPE_ID;
                    PlateCollection pc = new PlateCollection(op.USER_PLATE.USRP_PLATE,
                                                             op.USER_PLATE1 != null ? op.USER_PLATE1.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE2 != null ? op.USER_PLATE2.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE3 != null ? op.USER_PLATE3.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE4 != null ? op.USER_PLATE4.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE5 != null ? op.USER_PLATE5.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE6 != null ? op.USER_PLATE6.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE7 != null ? op.USER_PLATE7.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE8 != null ? op.USER_PLATE8.USRP_PLATE : string.Empty,
                                                             op.USER_PLATE9 != null ? op.USER_PLATE9.USRP_PLATE : string.Empty);
                    p.Plates.Add(pc);
                    PermitCollection ExistingPermitGroup = null;
                    ExistingPermitGroup = PermitsGroupped.Where(pg => pg.RelationId == op.OPE_REL_OPE_ID).FirstOrDefault();
                    if (ExistingPermitGroup != null)
                    {
                        ExistingPermitGroup.Permits.Add(p);
                    }
                    else
                    {
                        PermitCollection pg = new PermitCollection();
                        pg.RelationId = op.OPE_ID;
                        pg.UserName = op.USER.USR_USERNAME;
                        pg.InstallationId = op.OPE_INS_ID;
                        pg.UserCurrencyId = op.USER.CURRENCy.CUR_ID;
                        pg.GroupId = op.OPE_GRP_ID;
                        pg.UTC_Offset = op.OPE_DATE_UTC_OFFSET.ToString();
                        pg.TariffId = op.OPE_TAR_ID;
                        pg.InstallationTimeZone = op.INSTALLATION.INS_TIMEZONE_ID;
                        pg.Permits.Add(p);
                        PermitsGroupped.Add(pg);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(LogLevels.logERROR, string.Format("PermitRenewer::GroupPermits - {0}", ex.Message));
            }
            Log(LogLevels.logDEBUG, "<< PermitRenewer::GroupPermits");

            return PermitsGroupped;
        }

        private void RenewPermits(List<PermitCollection> PermitsToRenew)
        {
            Log(LogLevels.logDEBUG, ">> PermitRenewer::RenewPermits");
            try
            {
                ResultType res;
                foreach (PermitCollection p in PermitsToRenew)
                {

                    if (ValidTimeSteps == null)
                        ValidTimeSteps = new List<TimeStep>();
                    else
                        ValidTimeSteps.Clear();

                    if (InstallationIsValid(p.InstallationId, p.UserCurrencyId, out res))
                    {
                        if (SessionIsValid(p.InstallationId, p.UserName, p.UTC_Offset, out res))
                        {
                            if (GroupIsValid(p.GroupId, out res))
                            {
                                if (TariffIsValid(p.TariffId, p.UserName, p.GroupId, out res))
                                {
                                    if (NumPermitsIsValid(p.TariffId, p.Permits.Count, out res))
                                    {
                                        bool PermitIsValid = false;
                                        foreach (Permit p2 in p.Permits)
                                        {
                                            if (PlatesAreValid(p2.Plates, out res))
                                            {
                                                if (NumPlatesIsValid(p.TariffId, p2.Plates, out res))
                                                {
                                                    PermitIsValid = true;
                                                }
                                                else
                                                {
                                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Number of plates not valid", p.RelationId));
                                                    PermitIsValid = false;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Plates not valid", p.RelationId));
                                                PermitIsValid = false;
                                                break;
                                            }
                                        }
                                        if (PermitIsValid)
                                        {
                                            List<string> PlateCollection;
                                            ResultType res2 = TimeStepIsValid(p.UserName, p.Permits, p.InstallationId, p.GroupId, p.TariffId, p.InstallationTimeZone, out PlateCollection);
                                            if (res2 == ResultType.Result_OK)
                                            {
                                                ResultType res3 = RenewSinglePermit(p.GroupId, PlateCollection, p.TariffId, p.UserName);
                                                if (res3 == ResultType.Result_OK)
                                                {
                                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} renewed succesfully", p.RelationId));
                                                    SetPermitRenewalCompleted(p.RelationId);
                                                }
                                                else
                                                {
                                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} renewal failed [{1}]", p.RelationId, res3.ToString()));
                                                    SetPermitRenewalFailed(p.RelationId, res3);
                                                }
                                            }
                                            else
                                            {
                                                if (res2 == ResultType.Result_Error_PermitAlreadyExist)
                                                {
                                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable [{1}]", p.RelationId, res2.ToString()));
                                                    SetPermitNotRenewable(p.RelationId, res2);
                                                }
                                                else
                                                {
                                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} renewal failed [{1}]", p.RelationId, res2.ToString()));
                                                    SetPermitRenewalFailed(p.RelationId, res2);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable [{1}]", p.RelationId, res.ToString()));
                                            SetPermitNotRenewable(p.RelationId, res);
                                        }
                                    }
                                    else
                                    {
                                        Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Number of permits not valid", p.RelationId));
                                        SetPermitNotRenewable(p.RelationId, res);
                                    }
                                }
                                else
                                {
                                    Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Tariff not valid", p.RelationId));
                                    SetPermitNotRenewable(p.RelationId, res);
                                }
                            }
                            else
                            {
                                Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Group not valid", p.RelationId));
                                SetPermitNotRenewable(p.RelationId, res);
                            }
                        }
                        else
                        {
                            Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Session not set", p.RelationId));
                            SetPermitNotRenewable(p.RelationId, res);
                        }
                    }
                    else
                    {
                        Log(LogLevels.logINFO, string.Format("PermitRenewer::RenewPermits - Permit with RelationId = {0} is not renewable - Installation not valid", p.RelationId));
                        SetPermitNotRenewable(p.RelationId, res);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(LogLevels.logERROR, string.Format("PermitRenewer::RenewPermits - {0}", ex.Message));
            }
            Log(LogLevels.logDEBUG, "<< PermitRenewer::RenewPermits");
        }

        private ResultType RenewSinglePermit(decimal? GroupId, List<string> PlateCollection, decimal? TariffId, string UserName)
        {
            Log(LogLevels.logDEBUG, ">> PermitRenewer::RenewSinglePermit");
            TimeStep ts = ValidTimeSteps[0];
            string str3DSUrl = "";
            ResultType res = wswi.ConfirmParkingOperation(ts.Amount,
                        DateTime.Now.ToString("HHmmssddMMyy"),
                        ts.EndDate,
                        ts.InitialDate,
                        (decimal)GroupId,
                        PlateCollection,
                        ts.AmountWithoutBon,
                        ts.RealAmount,
                        ts.AmountFee,
                        (decimal)TariffId,
                        ts.MinimumTime,
                        ts.TimeBalanceUsed,
                        ts.AmountTotal,
                        ts.AmountVat,
                        SessionId,
                        UserName,
                        1,
                        "",
                        "",
                        "",
                        false,
                        out str3DSUrl);

            Log(LogLevels.logDEBUG, "<< PermitRenewer::RenewSinglePermit");

            return res;
        }

        private void SetPermitNotRenewable(decimal? RelationId, ResultType ErrorCode)
        {
            customersRepository.UpdatePermitStatus(RelationId, PermitAutoRenewalStatus.RenewalImpossible, (int)ErrorCode);
        }

        private void SetPermitRenewalFailed(decimal? RelationId, ResultType ErrorCode)
        {
            customersRepository.UpdatePermitStatus(RelationId, PermitAutoRenewalStatus.RenewalFailed, (int)ErrorCode);
        }

        private void SetPermitRenewalCompleted(decimal? RelationId)
        {
            customersRepository.UpdatePermitStatus(RelationId, PermitAutoRenewalStatus.RenewalCompleted);
        }

        #endregion

        #region Helpers

        private int CountPlatesInCollection(PlateCollection pc)
        { 
            int PlatesCount = 0;
            if (!string.IsNullOrEmpty(pc.Plate1)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate2)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate3)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate4)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate5)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate6)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate7)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate8)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate9)) PlatesCount++;
            if (!string.IsNullOrEmpty(pc.Plate10)) PlatesCount++;
            return PlatesCount;
        }

        private string GetNextMonth(string InstallationTimezone)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(InstallationTimezone);
            DateTime dtServerTime = DateTime.Now;
            DateTime dtInsDateTime = TimeZoneInfo.ConvertTime(dtServerTime, TimeZoneInfo.Local, tzi);

            DateTime d = new DateTime(dtInsDateTime.Year, dtInsDateTime.Month, 1); // First day of current month
            d = d.AddMonths(1); // First day of next month
            return d.ToString("000000ddMMyy");
        }

        private void Log(LogLevels nLevel, string strMessage)
        {
            Console.WriteLine(strMessage);
            m_Log.LogMessage(nLevel, strMessage);
        }

        #endregion

        #region Call to WS

        private ResultType QueryLoginCityInternal(decimal? InstallationId, string Username, string UTC_Offset)
        {
            SortedList parametersOut = new SortedList();
            List<decimal> PermitTypeTariffs = new List<decimal>();
            ValidGroups = new List<decimal>();
            ValidPlates = new List<string>();

            ResultType res = wswi.QueryLoginCityInternal(InstallationId, Username, string.Empty, UTC_Offset, out parametersOut);

            if (res == ResultType.Result_OK)
            {
                if (parametersOut.ContainsKey("SessionID"))
                {
                    SessionId = parametersOut["SessionID"].ToString();
                }

                int hNum = Convert.ToInt32(parametersOut["InfoTAR_0_ad_num"]);
                if (hNum > 0)
                {
                    for (int i = 0; i < hNum; i++)
                    {
                        if (Convert.ToInt32(parametersOut[string.Format("InfoTAR_0_ad_{0}_type", i)]) == 1)
                        {
                            int kNum = Convert.ToInt32(parametersOut[string.Format("InfoTAR_0_ad_{0}_szs_0_sz_num", i)]);
                            for (int k = 0; k < kNum; k++)
                            {
                                PermitTypeTariffs.Add(Convert.ToDecimal(parametersOut[string.Format("InfoTAR_0_ad_{0}_szs_0_sz_{1}_id", i, k)]));
                            }
                        }
                    }
                }

                int iNum = Convert.ToInt32(parametersOut["ZoneTar_0_zone_num"]);                
                for (int i = 0; i < iNum; i++)
                {
                    int jNum = Convert.ToInt32(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_num", i)]);
                    if (jNum > 0)
                    {
                        for (int j = 0; j < jNum; j++)
                        {
                            if (PermitTypeTariffs.Contains(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_id", i, j)])))
                            {
                                ValidGroups.Add(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_subzone_{1}_id", i, j)]));
                            }
                        }
                    }
                    else
                    {
                        if (PermitTypeTariffs.Contains(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_id", i)])))
                        {
                            ValidGroups.Add(Convert.ToDecimal(parametersOut[string.Format("ZoneTar_0_zone_{0}_id", i)]));
                        }
                    }
                }

                iNum = Convert.ToInt32(parametersOut["userDATA_0_userlp_0_lp_num"]);
                for (int i = 0; i < iNum; i++)
                {
                    if (parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString().Contains("~"))
                    {
                        foreach (string p in parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString().Split('~'))
                        {
                            ValidPlates.Add(p);                            
                        }
                    }
                    else
                    {
                        ValidPlates.Add(parametersOut[string.Format("userDATA_0_userlp_0_lp_{0}_plate", i)].ToString());
                    }
                }
            }

            return res;
        }

        private ResultType QueryParkingTariffs(string UserName, string SessionId, decimal? GroupId)
        {
            ValidTariffs = new List<Tariff>();
            SortedList parametersOut = new SortedList();

            ResultType res = wswi.QueryParkingTariffs(UserName, SessionId, GroupId, string.Empty, out parametersOut);

            if (res == ResultType.Result_OK)
            {
                int iNum = Convert.ToInt32(parametersOut["ltar_ad_num"]);
                for (int i = 0; i < iNum; i++)
                {
                    if (Convert.ToDecimal(parametersOut[string.Format("ltar_ad_{0}_type_4", i)]) == 1)
                    {
                        if (parametersOut.ContainsKey(string.Format("ltar_ad_{0}_id_0", i)) &&
                            parametersOut.ContainsKey(string.Format("ltar_ad_{0}_maxplates_5", i)) &&
                            parametersOut.ContainsKey(string.Format("ltar_ad_{0}_permitmaxbuyonce_8", i)))
                        {
                            ValidTariffs.Add(new Tariff
                            {
                                id = Convert.ToDecimal(parametersOut[string.Format("ltar_ad_{0}_id_0", i)]),
                                MaxLicensePlates = Convert.ToInt32(parametersOut[string.Format("ltar_ad_{0}_maxplates_5", i)]),
                                MaxBuyOnce = Convert.ToInt32(parametersOut[string.Format("ltar_ad_{0}_permitmaxbuyonce_8", i)])
                            });
                        }
                        else
                        {
                            Log(LogLevels.logERROR, string.Format("PermitRenewer::QueryParkingTariffs - Expected property missing for tariff number {0}", (i + 1)));
                        }
                    }
                }
            }

            return res;
        }

        private ResultType QueryParkingOperationWithTimeSteps(string UserName, List<Permit> Permits, decimal InstallationId, decimal GroupId, decimal TariffId, string InstallationTimeZone, out List<string> PlateCollection)
        {
            SortedList parametersOut = new SortedList();
            int failedPermit = 0;
            string Month = GetNextMonth(InstallationTimeZone);

            PlateCollection = new List<string>();
            foreach (Permit p in Permits)
            {
                string PlateBlock = string.Empty;
                foreach (PlateCollection pc in p.Plates)
                {
                    if (!string.IsNullOrEmpty(pc.Plate1)) PlateBlock += string.Format(",{0}", pc.Plate1);
                    if (!string.IsNullOrEmpty(pc.Plate2)) PlateBlock += string.Format(",{0}", pc.Plate2);
                    if (!string.IsNullOrEmpty(pc.Plate3)) PlateBlock += string.Format(",{0}", pc.Plate3);
                    if (!string.IsNullOrEmpty(pc.Plate4)) PlateBlock += string.Format(",{0}", pc.Plate4);
                    if (!string.IsNullOrEmpty(pc.Plate5)) PlateBlock += string.Format(",{0}", pc.Plate5);
                    if (!string.IsNullOrEmpty(pc.Plate6)) PlateBlock += string.Format(",{0}", pc.Plate6);
                    if (!string.IsNullOrEmpty(pc.Plate7)) PlateBlock += string.Format(",{0}", pc.Plate7);
                    if (!string.IsNullOrEmpty(pc.Plate8)) PlateBlock += string.Format(",{0}", pc.Plate8);
                    if (!string.IsNullOrEmpty(pc.Plate9)) PlateBlock += string.Format(",{0}", pc.Plate9);
                    if (!string.IsNullOrEmpty(pc.Plate10)) PlateBlock += string.Format(",{0}", pc.Plate10);
                }
                if (!string.IsNullOrEmpty(PlateBlock))
                {
                    PlateCollection.Add(PlateBlock.Substring(1));
                }
            }

            ResultType res = wswi.QueryParkingOperationWithTimeSteps(UserName, SessionId, PlateCollection, InstallationId, GroupId, TariffId, Month, out parametersOut, out failedPermit);

            if (res == ResultType.Result_OK)
            {
                int iNum = Convert.ToInt32(parametersOut["steps_0_step_num"]);
                int i = iNum - 1;

                string _d = parametersOut[string.Format("steps_0_step_{0}_d", i)].ToString();
                decimal _q = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q", i)]);
                decimal _q_fee = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_fee", i)]);
                decimal _q_total = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_total", i)]);
                decimal _q_vat = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_vat", i)]);
                decimal _q_without_bon = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_q_without_bon", i)]);
                decimal _real_q = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_real_q", i)]);
                decimal _t = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_t", i)]);
                decimal _time_bal_used = Convert.ToDecimal(parametersOut[string.Format("steps_0_step_{0}_time_bal_used", i)]);
                string _di = parametersOut["di"].ToString();

                TimeStep ts = new TimeStep();
                ts.Amount = _q;
                ts.AmountFee = _q_fee;
                ts.AmountTotal = _q_total;
                ts.AmountVat = _q_vat;
                ts.AmountWithoutBon = _q_without_bon;
                ts.EndDate = _d;
                ts.InitialDate = _di;
                ts.MinimumTime = _t;
                ts.RealAmount = _real_q;
                ts.TimeBalanceUsed = _time_bal_used;
                ValidTimeSteps.Add(ts);
            }

            return res;
        }

        #endregion

        #region Validators

        private bool InstallationIsValid(decimal? InstallationId, decimal UserCurrencyId, out ResultType res)
        {
            List<WSintegraMobile.City> Cities = wswi.GetCities(string.Empty, UserCurrencyId);
            if (Cities.Where(c => c.id == InstallationId || (c.Child != null && c.Child == InstallationId)).Count() > 0)
            {
                res = ResultType.Result_OK;
                return true;
            }
            res = ResultType.Result_Error_Invalid_City;
            return false;
        }

        private bool SessionIsValid(decimal? InstallationId, string UserName, string UTC_Offset, out ResultType res)
        {
            SessionId = null;            
            res = QueryLoginCityInternal(InstallationId, UserName, UTC_Offset);
            return !string.IsNullOrEmpty(SessionId);
        }

        private bool GroupIsValid(decimal? GroupId, out ResultType res)
        {
            res = ResultType.Result_OK;
            bool r = ValidGroups.Contains((decimal)GroupId);
            if (!r)
                res = ResultType.Result_Error_Generic;
            return r;
        }

        private bool TariffIsValid(decimal? TariffId, string UserName, decimal? GroupId, out ResultType res)
        {
            res = QueryParkingTariffs(UserName, SessionId, GroupId);
            int c = ValidTariffs.Where(t => t.id == (decimal)TariffId).Count();
            if (c == 0)
                res = ResultType.Result_Error_Tariffs_Not_Available;

            return (c > 0);
        }

        private bool NumPermitsIsValid(decimal? TariffId, int NumPermitsToRenew, out ResultType res)
        {
            res = ResultType.Result_OK;
            int MaxBuyOnceForTariff = ValidTariffs.Where(t => t.id == (decimal)TariffId).Select(s => s.MaxBuyOnce).FirstOrDefault();

            Log(LogLevels.logINFO, string.Format("PermitRenewer::NumPermitsIsValid - Trying to renew {0} permits with TariffId = {1}. MaxBuyOnce is: {2}", NumPermitsToRenew, TariffId, MaxBuyOnceForTariff));

            if (NumPermitsToRenew > MaxBuyOnceForTariff)
                res = ResultType.Result_Error_Generic;

            return (MaxBuyOnceForTariff >= NumPermitsToRenew);
        }

        private bool NumPlatesIsValid(decimal? TariffId, List<PlateCollection> Plates, out ResultType res)
        {
            res = ResultType.Result_OK;
            int MaxPlatesForTariff = ValidTariffs.Where(t => t.id == (decimal)TariffId).Select(s => s.MaxLicensePlates).FirstOrDefault();
            foreach (PlateCollection pc in Plates)
            {
                if (CountPlatesInCollection(pc) > MaxPlatesForTariff)
                {
                    res = ResultType.Result_Error_Invalid_Plate;
                    return false;
                }
            }
            return true;
        }

        private ResultType TimeStepIsValid(string UserName, List<Permit> Permits, decimal? InstallationId, decimal? GroupId, decimal? TariffId, string InstallationTimeZone, out List<string> PlateCollection)
        {
            ResultType res = QueryParkingOperationWithTimeSteps(UserName, Permits, (decimal)InstallationId, (decimal)GroupId, (decimal)TariffId, InstallationTimeZone, out PlateCollection);
            if (res == ResultType.Result_OK)
            {
                if (ValidTimeSteps.Count == 1)
                {
                    return res;
                }
                else
                {
                    return ResultType.Result_Error_Generic;
                }
            }
            return res;
        }

        private bool PlatesAreValid(List<PlateCollection> Plates, out ResultType res)
        {
            res = ResultType.Result_Error_Invalid_Plate;
            foreach (PlateCollection pc in Plates)
            {
                if (!string.IsNullOrEmpty(pc.Plate1) && !ValidPlates.Contains(pc.Plate1)) return false;
                if (!string.IsNullOrEmpty(pc.Plate2) && !ValidPlates.Contains(pc.Plate2)) return false;
                if (!string.IsNullOrEmpty(pc.Plate3) && !ValidPlates.Contains(pc.Plate3)) return false;
                if (!string.IsNullOrEmpty(pc.Plate4) && !ValidPlates.Contains(pc.Plate4)) return false;
                if (!string.IsNullOrEmpty(pc.Plate5) && !ValidPlates.Contains(pc.Plate5)) return false;
                if (!string.IsNullOrEmpty(pc.Plate6) && !ValidPlates.Contains(pc.Plate6)) return false;
                if (!string.IsNullOrEmpty(pc.Plate7) && !ValidPlates.Contains(pc.Plate7)) return false;
                if (!string.IsNullOrEmpty(pc.Plate8) && !ValidPlates.Contains(pc.Plate8)) return false;
                if (!string.IsNullOrEmpty(pc.Plate9) && !ValidPlates.Contains(pc.Plate9)) return false;
                if (!string.IsNullOrEmpty(pc.Plate10) && !ValidPlates.Contains(pc.Plate10)) return false;
            }
            res = ResultType.Result_OK;
            return true;
        }

        #endregion

        #region HelperClasses

        private class Tariff
        {
            public decimal id { get; set; }
            public int MaxLicensePlates { get; set; }
            public int MaxBuyOnce { get; set; }
        }

        private class TimeStep
        {
            public decimal Amount { get; set; }             // q
            public string EndDate { get; set; }             // d
            public string InitialDate { get; set; }         // di
            public decimal AmountWithoutBon { get; set; }   // q_without_bon
            public decimal RealAmount { get; set; }         // real_q
            public decimal AmountFee { get; set; }          // q_fee
            public decimal MinimumTime { get; set; }        // t
            public decimal TimeBalanceUsed { get; set; }    // time_bal_used
            public decimal AmountTotal { get; set; }        // q_total
            public decimal AmountVat { get; set; }          // q_vat
        }

        #endregion

    }
}
