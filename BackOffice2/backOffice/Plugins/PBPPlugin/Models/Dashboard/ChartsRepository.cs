using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Globalization;
using System.Data.Objects.SqlClient;
using System.Configuration;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models.Dashboard
{

    public enum DateGroupType {
        hour = 0,
        weekday = 1,
        day = 2,
        week = 3,
        month = 4,
        quarter = 5,
        half = 6,
        year = 7
    }

    public enum DateFilterType {
        lastWeek,
        lastMonth,
        lastQuarter,
        lastHalf,
        lastYear,
        currentWeek,
        currentMonth,
        currentQuarter,
        currentHalf,
        currentYear,
        custom
    }

    public enum UserInstallationType
    {
        FirstOperation,
        LadaKey
    }

    public class ChartsRepository
    {

        #region Public Static Methods

        public static IList<ChartDataItem> OperationsTodayData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_MINUTE>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, "00:00", DateTime.Now.ToString("HH:mm"));

                var operations = (from o in backOfficeRepository.GetDbOperationsMinute(predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, DB_OPERATIONS_MINUTE>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.TICKETPAYMENTS_COUNT ?? 0)))
                               .ToList();
            }

            return oRet;
        }

        public static TodayData TodayData(IBackOfficeRepository backOfficeRepository, string opeType, string[] installations)
        {
            TodayData oRechargesTodayData = null;
            if (opeType == "All")
            {
                oRechargesTodayData = TodayData(backOfficeRepository, "Recharges", installations);
            }

            // Current day
            var predicate = PredicateBuilder.True<DB_OPERATIONS_MINUTE>();

            if (opeType != "Recharges")
            {
                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
            }

            DateTime dtCurDay = DateTime.Now.Date;

            // ***
            /*var lastOperation = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              orderby o.HOUR descending
                              select o).First();
            dtCurDay = (lastOperation.HOUR ?? new DateTime(2014, 3, 5));*/
            //dtCurDay = new DateTime(2014, 1, 23);
            // ***

            // apply date filter
            
            predicate = FilterDate(predicate, DateFilterType.custom, new DateTime(dtCurDay.Year, dtCurDay.Month, dtCurDay.Day, 0, 0, 0), new DateTime(dtCurDay.Year, dtCurDay.Month, dtCurDay.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                                   "00:00", DateTime.Now.ToString("HH:mm"));

            var operations = (from o in backOfficeRepository.GetDbOperationsMinute(predicate)
                              select o).AsQueryable();

            long lCurDay = 0;
            long lParkingsCurDay = 0;
            long lExtensionsCurDay = 0;
            long lRefundsCurDay = 0;
            long lTicketsCurDay = 0;
            long lRechargesCurDay = 0;
            switch (opeType)
            {
                case "Parkings": lCurDay = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0); break;
                case "Extensions": lCurDay = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0); break;
                case "Refunds": lCurDay = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0); break;
                case "Tickets": lCurDay = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0); break;
                case "Recharges": lCurDay = (operations.Sum(o => o.RECHARGES_COUNT) ?? 0); break;
                case "All":
                    lParkingsCurDay = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0);
                    lExtensionsCurDay = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0);
                    lRefundsCurDay = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0);
                    lTicketsCurDay = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0);                    
                    lRechargesCurDay = oRechargesTodayData.CurDay;
                    break;
            }

            // Previous day
            predicate = PredicateBuilder.True<DB_OPERATIONS_MINUTE>();

            if (opeType != "Recharges")
            {
                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
            }

            // apply date filter
            DateTime dtPrevDay = dtCurDay.AddDays(-1).Date;
            predicate = FilterDate(predicate, DateFilterType.custom, new DateTime(dtPrevDay.Year, dtPrevDay.Month, dtPrevDay.Day, 0, 0, 0), new DateTime(dtPrevDay.Year, dtPrevDay.Month, dtPrevDay.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                                   "00:00", DateTime.Now.ToString("HH:mm"));

            operations = (from o in backOfficeRepository.GetDbOperationsMinute(predicate)
                          select o).AsQueryable();

            long lPrevDay = 0;
            long lParkingsPrevDay = 0;
            long lExtensionsPrevDay = 0;
            long lRefundsPrevDay = 0;
            long lTicketsPrevDay = 0;
            long lRechargesPrevDay = 0;
            switch (opeType)
            {
                case "Parkings": lPrevDay = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0); break;
                case "Extensions": lPrevDay = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0); break;
                case "Refunds": lPrevDay = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0); break;
                case "Tickets": lPrevDay = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0); break;
                case "Recharges": lPrevDay = (operations.Sum(o => o.RECHARGES_COUNT) ?? 0); break;
                case "All":
                    lParkingsPrevDay = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0);
                    lExtensionsPrevDay = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0);
                    lRefundsPrevDay = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0);
                    lTicketsPrevDay = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0);
                    lRechargesPrevDay = oRechargesTodayData.PrevDay;
                    break;
            }

            // Previous week
            predicate = PredicateBuilder.True<DB_OPERATIONS_MINUTE>();

            if (opeType != "Recharges")
            {
                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
            }

            // apply date filter
            DateTime dtPrevWeek = dtCurDay.AddDays(-7).Date;
            predicate = FilterDate(predicate, DateFilterType.custom, new DateTime(dtPrevWeek.Year, dtPrevWeek.Month, dtPrevWeek.Day, 0, 0, 0), new DateTime(dtPrevWeek.Year, dtPrevWeek.Month, dtPrevWeek.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                                   "00:00", DateTime.Now.ToString("HH:mm"));

            operations = (from o in backOfficeRepository.GetDbOperationsMinute(predicate)
                          select o).AsQueryable();

            long lPrevWeek = 0;
            long lParkingsPrevWeek = 0;
            long lExtensionsPrevWeek = 0;
            long lRefundsPrevWeek = 0;
            long lTicketsPrevWeek = 0;
            long lRechargesPrevWeek = 0;
            switch (opeType)
            {
                case "Parkings": lPrevWeek = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0); break;
                case "Extensions": lPrevWeek = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0); break;
                case "Refunds": lPrevWeek = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0); break;
                case "Tickets": lPrevWeek = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0); break;
                case "Recharges": lPrevWeek = (operations.Sum(o => o.RECHARGES_COUNT) ?? 0); break;
                case "All":
                    lParkingsPrevWeek = (operations.Sum(o => o.PARKINGS_COUNT) ?? 0);
                    lExtensionsPrevWeek = (operations.Sum(o => o.EXTENSIONS_COUNT) ?? 0);
                    lRefundsPrevWeek = (operations.Sum(o => o.REFUNDS_COUNT) ?? 0);
                    lTicketsPrevWeek = (operations.Sum(o => o.TICKETPAYMENTS_COUNT) ?? 0);
                    lRechargesPrevWeek = oRechargesTodayData.PrevWeek;
                    break;
            }

            string[] days = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            string[] daysLong = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DayNames;
            //((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.Value.DayOfWeek + 1 : o.HOUR.Value.DayOfWeek))

            return new TodayData { DataType = opeType,
                                   CurDay = lCurDay, PrevDay = lPrevDay, PrevWeek = lPrevWeek,
                                   ParkingsCurDay = lParkingsCurDay,
                                   ParkingsPrevDay = lParkingsPrevDay, 
                                   ParkingsPrevWeek = lParkingsPrevWeek,
                                   ExtensionsCurDay = lExtensionsCurDay,
                                   ExtensionsPrevDay = lExtensionsPrevDay,
                                   ExtensionsPrevWeek = lExtensionsPrevWeek,
                                   RefundsCurDay = lRefundsCurDay,
                                   RefundsPrevDay = lRefundsPrevDay,
                                   RefundsPrevWeek = lRefundsPrevWeek,
                                   TicketsCurDay = lTicketsCurDay,
                                   TicketsPrevDay = lTicketsPrevDay,
                                   TicketsPrevWeek = lTicketsPrevWeek,
                                   RechargesCurDay = lRechargesCurDay,
                                   RechargesPrevDay = lRechargesPrevDay,
                                   RechargesPrevWeek = lRechargesPrevWeek,
                                   CurDayDate = days[(int)(/*dtCurDay.DayOfWeek == DayOfWeek.Sunday ? dtCurDay.DayOfWeek + 1 : */dtCurDay.DayOfWeek)] + "<br>" + dtCurDay.ToString("dd-MM"),
                                   PrevDayDate = days[(int)(/*dtPrevDay.DayOfWeek == DayOfWeek.Sunday ? dtPrevDay.DayOfWeek + 1 : */dtPrevDay.DayOfWeek)] + "<br>" + dtPrevDay.ToString("dd-MM"),
                                   PrevWeekDate = days[(int)(/*dtPrevWeek.DayOfWeek == DayOfWeek.Sunday ? dtPrevWeek.DayOfWeek + 1 : */dtPrevWeek.DayOfWeek)] + "<br>" + dtPrevWeek.ToString("dd-MM"),
                                   CurDayDateLong = daysLong[(int)(/*dtCurDay.DayOfWeek == DayOfWeek.Sunday ? dtCurDay.DayOfWeek + 1 : */dtCurDay.DayOfWeek)] + " " + dtCurDay.ToString("dd-MM"),
                                   PrevDayDateLong = daysLong[(int)(/*dtPrevDay.DayOfWeek == DayOfWeek.Sunday ? dtPrevDay.DayOfWeek + 1 : */dtPrevDay.DayOfWeek)] + " " + dtPrevDay.ToString("dd-MM"),
                                   PrevWeekDateLong = daysLong[(int)(/*dtPrevWeek.DayOfWeek == DayOfWeek.Sunday ? dtPrevWeek.DayOfWeek + 1 : */dtPrevWeek.DayOfWeek)] + " " + dtPrevWeek.ToString("dd-MM")
            };
        }

        /*public static IList<ChartDataItem> OperationsData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
            
            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter
            ChargeOperationsType[] types = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund, ChargeOperationsType.TicketPayment };
            predicate = predicate.And(o => types.Contains((ChargeOperationsType)o.OPE_TYPE));


            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o).AsQueryable();


            IQueryable<IGrouping<string, ALL_OPERATIONS_EXT>> grouped = null;

            ChargeOperationsType[] typesIniDate = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund };

            switch (dateGroup)
            {
                case DateGroupType.day:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + (((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + ((((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + ((((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString());
                    break;
            }



            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count(o => o.OPE_TYPE == (int) ChargeOperationsType.ParkingOperation),
                                                                                      g.Count(o => o.OPE_TYPE == (int) ChargeOperationsType.ExtensionOperation),
                                                                                      g.Count(o => o.OPE_TYPE == (int) ChargeOperationsType.ParkingRefund),
                                                                                      g.Count(o => o.OPE_TYPE == (int) ChargeOperationsType.TicketPayment)))
                          .ToList();

            //oRet = (from o in backOfficeRepository.GetOperations(predicate)
            //        group o by o.OPE_UTC_DATE.Date into g
            //        orderby g.Key
            //        select new ChartDataItem(g.Key.ToString(), g.Count(), null)).ToList();

            return oRet;
        }*/

        public static IList<ChartDataItem> OperationsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_HOUR>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            var operations = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                                select o).AsQueryable();

            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                        g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                        g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                        g.Sum(o => o.TICKETPAYMENTS_COUNT ?? 0)))
                           .ToList();

            return oRet;
        }
        public static IList<ChartDataItem> OperationsDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                            g.Sum(o => o.TICKETPAYMENTS_COUNT ?? 0)))
                               .ToList();
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> OperationsTotalsData(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter
            ChargeOperationsType[] types = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund, ChargeOperationsType.TicketPayment };
            predicate = predicate.And(o => types.Contains((ChargeOperationsType)o.OPE_TYPE));

            var totalCount = backOfficeRepository.GetOperationsExt(predicate).Count();

            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              group o by o.OPE_TYPE into g
                              let i = g.Key
                              orderby i
                              select new ChartDataItem((ChargeOperationsType)g.Key, (float) (g.Count() * 100) / totalCount)
                             ).AsQueryable();

            oRet = operations.ToList();

            // ****************
            //oRet[0] = new ChartDataItem(ChargeOperationsType.ExtensionOperation, oRet[0].Total - 25);
            //oRet.Add(new ChartDataItem(ChargeOperationsType.ExtensionOperation, 25));
            // ****************

            return oRet;
        }*/

        /*public static IList<ChartDataItem> OperationsTotalsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_HOUR>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            var operations = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              select o).AsQueryable();

            var totalParkings = operations.Sum(o => o.PARKINGS_COUNT ?? 0);
            var totalExtensions = operations.Sum(o => o.EXTENSIONS_COUNT ?? 0);
            var totalRefunds = operations.Sum(o => o.REFUNDS_COUNT ?? 0);
            var totalTicketsPayments = operations.Sum(o => o.TICKETPAYMENTS_COUNT ?? 0);
            var total = totalParkings + totalExtensions + totalRefunds + totalTicketsPayments;

            oRet = new List<ChartDataItem>();
            oRet.Add(new ChartDataItem(ChargeOperationsType.ParkingOperation, (totalParkings * 100) / total));
            oRet.Add(new ChartDataItem(ChargeOperationsType.ExtensionOperation, (totalExtensions * 100) / total));
            oRet.Add(new ChartDataItem(ChargeOperationsType.ParkingRefund, (totalRefunds * 100) / total));
            oRet.Add(new ChartDataItem(ChargeOperationsType.TicketPayment, (totalTicketsPayments * 100) / total));

            return oRet;
        }*/
        public static IList<ChartDataItem> OperationsTotalsDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                int totalParkings = 0;
                int totalExtensions = 0;
                int totalRefunds = 0;
                int totalTicketsPayments = 0;
                int total = 0;

                if (operations.Count() > 0)
                {
                    totalParkings = operations.Sum(o => o.PARKINGS_COUNT ?? 0);
                    totalExtensions = operations.Sum(o => o.EXTENSIONS_COUNT ?? 0);
                    totalRefunds = operations.Sum(o => o.REFUNDS_COUNT ?? 0);
                    totalTicketsPayments = operations.Sum(o => o.TICKETPAYMENTS_COUNT ?? 0);
                    total = totalParkings + totalExtensions + totalRefunds + totalTicketsPayments;
                }

                oRet = new List<ChartDataItem>();
                oRet.Add(new ChartDataItem(ChargeOperationsType.ParkingOperation, (total > 0 ? (totalParkings * 100) / total : 0)));
                oRet.Add(new ChartDataItem(ChargeOperationsType.ExtensionOperation, (total > 0 ? (totalExtensions * 100) / total : 0)));
                oRet.Add(new ChartDataItem(ChargeOperationsType.ParkingRefund, (total > 0 ? (totalRefunds * 100) / total : 0)));
                oRet.Add(new ChartDataItem(ChargeOperationsType.TicketPayment, (total > 0 ? (totalTicketsPayments * 100) / total : 0)));
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> OperationsAvgData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter
            ChargeOperationsType[] types = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund };
            predicate = predicate.And(o => types.Contains((ChargeOperationsType)o.OPE_TYPE));

            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o).AsQueryable();

            IQueryable<IGrouping<string, ALL_OPERATIONS_EXT>> grouped = null;

            ChargeOperationsType[] typesIniDate = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund };

            switch (dateGroup)
            {
                case DateGroupType.hour:
                case DateGroupType.weekday:
                case DateGroupType.day:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + (((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + ((((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString() + "-" + ((((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = operations.GroupBy(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Year.ToString());
                    break;
            }

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);
            
            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation),
                                                                                      g.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation),
                                                                                      g.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund),
                                                                                      new float[] { (g.Sum(o => (float) ((o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ? 
                                                                                                                    (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? -(o.OPE_AMOUNT ?? 0) * curChanges["EUR"] : 
                                                                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? -(o.OPE_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                                    -(o.OPE_AMOUNT ?? 0))))) 
                                                                                                                    : 0))) / 100),
                                                                                                    g.Sum(o => (float)((o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ? o.OPE_TIME : 0))) },
                                                                                      new float[] { (g.Sum(o => (float)((o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ? 
                                                                                                                    (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? -o.OPE_AMOUNT.Value * curChanges["EUR"] : 
                                                                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? -o.OPE_AMOUNT.Value * curChanges["USD"] :
                                                                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? -o.OPE_AMOUNT.Value * curChanges["CAD"] :
                                                                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? -o.OPE_AMOUNT.Value * curChanges["GBP"] :
                                                                                                                    -o.OPE_AMOUNT.Value)))) 
                                                                                                                    : 0))) / 100),
                                                                                                    g.Sum(o => (float)((o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ? o.OPE_TIME : 0))) },
                                                                                      new float[] { (g.Sum(o => (float)((o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund ? 
                                                                                                                    (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? o.OPE_AMOUNT.Value * curChanges["EUR"] : 
                                                                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? o.OPE_AMOUNT.Value * curChanges["USD"] :
                                                                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? o.OPE_AMOUNT.Value * curChanges["CAD"] :
                                                                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? o.OPE_AMOUNT.Value * curChanges["GBP"] :
                                                                                                                    o.OPE_AMOUNT.Value)))) 
                                                                                                                    : 0))) / 100),
                                                                                                    g.Sum(o => (float)((o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund ? o.OPE_TIME : 0))) }))
                          .ToList();

            return oRet;
        }*/

        public static IList<ChartDataItem> OperationsAvgDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_HOUR>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter
            // ...

            var operations = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              select o).AsQueryable();
            
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                      g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                      g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                      new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.PARKINGS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.PARKINGS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.PARKINGS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.PARKINGS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.PARKINGS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.PARKINGS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.PARKINGS_TIME??0) },
                                                                                      new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.EXTENSIONS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.EXTENSIONS_TIME??0) },
                                                                                      new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.REFUNDS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.REFUNDS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.REFUNDS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.REFUNDS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.REFUNDS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.REFUNDS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.REFUNDS_TIME??0) }))
                          .ToList();

            return oRet;
        }
        public static IList<ChartDataItem> OperationsAvgDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                // apply operation type filter
                // ...

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                CURRENCy oCurrency = null;
                Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                          g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                          g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                          new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.PARKINGS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.PARKINGS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.PARKINGS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.PARKINGS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.PARKINGS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.PARKINGS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.PARKINGS_TIME??0) },
                                                                                          new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.EXTENSIONS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.EXTENSIONS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.EXTENSIONS_TIME??0) },
                                                                                          new float[] { (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.REFUNDS_AMOUNT??0) * curChanges["EUR"] : 
                                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.REFUNDS_AMOUNT??0) * curChanges["USD"] : 
                                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.REFUNDS_AMOUNT??0) * curChanges["CAD"] : 
                                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.REFUNDS_AMOUNT??0) * curChanges["GBP"] :
                                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.REFUNDS_AMOUNT??0) * curChanges["MXN"] :
                                                                                                                            o.REFUNDS_AMOUNT??0)))))) / 100,
                                                                                                    g.Sum(o => o.REFUNDS_TIME??0) }))
                              .ToList();
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> OperationsAvgTotalsData(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter
            ChargeOperationsType[] types = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund };
            predicate = predicate.And(o => types.Contains((ChargeOperationsType)o.OPE_TYPE));

            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o).AsQueryable();

            //IQueryable<IGrouping<int, ALL_OPERATIONS_EXT>> grouped = operations.GroupBy(o => o.OPE_TYPE);

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            int iCount0 = operations.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation);
            int iCount1 = operations.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation);
            int iCount2 = operations.Count(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
            float fSum0 = 0;
            if (iCount0 > 0)            
                fSum0 = (float)operations.Sum(o => (o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ?
                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? -(o.OPE_AMOUNT ?? 0) * curChanges["EUR"] :
                                                             (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["USD"] :
                                                              (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["CAD"] :
                                                               (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? -(o.OPE_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                -(o.OPE_AMOUNT ?? 0))))) : 0)) / 100;
            
            float fSum1 = 0;
            if (iCount1 > 0)
                fSum1 = (float) operations.Sum(o => (o.OPE_TYPE == (int) ChargeOperationsType.ExtensionOperation ? 
                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? -(o.OPE_AMOUNT ?? 0) * curChanges["EUR"] :
                                                             (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["USD"] :
                                                              (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? -(o.OPE_AMOUNT ?? 0) * curChanges["CAD"] :
                                                               (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? -(o.OPE_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                -(o.OPE_AMOUNT ?? 0))))) : 0)) / 100;

            float fSum2 = 0;
            if (iCount2 > 0)
                fSum2 = (float) operations.Sum(o => (o.OPE_TYPE == (int) ChargeOperationsType.ParkingRefund ?
                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.OPE_AMOUNT ?? 0) * curChanges["EUR"] :
                                                             (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.OPE_AMOUNT ?? 0) * curChanges["USD"] :
                                                              (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.OPE_AMOUNT ?? 0) * curChanges["CAD"] :
                                                               (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.OPE_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                (o.OPE_AMOUNT ?? 0))))) : 0)) / 100;

            oRet.Add(new ChartDataItem("", new int[] { iCount0, iCount1, iCount2 }, new float[] { fSum0 }, new float[] { fSum1 }, new float[] { fSum2 }));

            return oRet;
        }*/

        public static IList<ChartDataItem> OperationsAvgTotalsDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                //IQueryable<IGrouping<int, ALL_OPERATIONS_EXT>> grouped = operations.GroupBy(o => o.OPE_TYPE);

                CURRENCy oCurrency = null;
                Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

                int iCount0 = 0;
                int iCount1 = 0;
                int iCount2 = 0;
                float fSum0 = 0;
                float fSum1 = 0;
                float fSum2 = 0;

                if (operations.Count() > 0)
                {
                    iCount0 = operations.Sum(o => o.PARKINGS_COUNT ?? 0);
                    iCount1 = operations.Sum(o => o.EXTENSIONS_COUNT ?? 0);
                    iCount2 = operations.Sum(o => o.REFUNDS_COUNT ?? 0);

                    if (iCount0 > 0)
                        fSum0 = (float)operations.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.PARKINGS_AMOUNT ?? 0) * curChanges["EUR"] :
                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.PARKINGS_AMOUNT ?? 0) * curChanges["USD"] :
                                                             (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.PARKINGS_AMOUNT ?? 0) * curChanges["CAD"] :
                                                              (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.PARKINGS_AMOUNT ?? 0) * curChanges["GBP"] :
                                                               (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.PARKINGS_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                (o.PARKINGS_AMOUNT ?? 0))))))) / 100;


                    if (iCount1 > 0)
                        fSum1 = (float)operations.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.EXTENSIONS_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.EXTENSIONS_AMOUNT ?? 0) * curChanges["USD"] :
                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.EXTENSIONS_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.EXTENSIONS_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.EXTENSIONS_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                         (o.EXTENSIONS_AMOUNT ?? 0))))))) / 100;

                    if (iCount2 > 0)
                        fSum2 = (float)operations.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.REFUNDS_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.REFUNDS_AMOUNT ?? 0) * curChanges["USD"] :
                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.REFUNDS_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.REFUNDS_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.REFUNDS_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                         (o.REFUNDS_AMOUNT ?? 0))))))) / 100;
                }
                oRet.Add(new ChartDataItem("", new int[] { iCount0, iCount1, iCount2 }, new float[] { fSum0 }, new float[] { fSum1 }, new float[] { fSum2 }));
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> RechargesData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter            
            predicate = predicate.And(o => o.OPE_TYPE == (int)ChargeOperationsType.BalanceRecharge);


            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o).AsQueryable();


            IQueryable<IGrouping<string, ALL_OPERATIONS_EXT>> grouped = null;

            switch (dateGroup)
            {
                case DateGroupType.day:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + ((o.OPE_DATE.Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + o.OPE_DATE.Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + (((o.OPE_DATE.Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + (((o.OPE_DATE.Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString());
                    break;
            }

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count(),
                                                                                      (float) g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? o.OPE_AMOUNT.Value * curChanges["EUR"] : 
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? o.OPE_AMOUNT.Value * curChanges["USD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? o.OPE_AMOUNT.Value * curChanges["CAD"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? o.OPE_AMOUNT.Value * curChanges["GBP"] :
                                                                                                          o.OPE_AMOUNT.Value))))) / 100)) 

                          .ToList();

            //oRet = (from o in backOfficeRepository.GetOperations(predicate)
            //        group o by o.OPE_UTC_DATE.Date into g
            //        orderby g.Key
            //        select new ChartDataItem(g.Key.ToString(), g.Count(), null)).ToList();

            return oRet;
        }*/

        public static IList<ChartDataItem> RechargesDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_HOUR>();

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);


            var operations = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              select o).AsQueryable();
            
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.RECHARGES_COUNT ?? 0),
                                                                                      (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_AMOUNT ?? 0)))))) / 100))
                          .ToList();

            return oRet;
        }
        public static IList<ChartDataItem> RechargesDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);


                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                CURRENCy oCurrency = null;
                Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), 
                                                                                          g.Sum(o => o.RECHARGES_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_REGULAR_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_AUTOMATIC_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_USERCREATION_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_CHANGEPM_COUNT ?? 0),
                                                                                          (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                             (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                              (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                               (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                                (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                                  o.RECHARGES_AMOUNT ?? 0)))))) / 100))
                              .ToList();
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> RechargesAvgData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<ALL_OPERATIONS_EXT>();

            // apply installations an groups filter
            //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            // apply operation type filter            
            predicate = predicate.And(o => o.OPE_TYPE == (int) ChargeOperationsType.BalanceRecharge);

            var operations = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o).AsQueryable();

            IQueryable<IGrouping<string, ALL_OPERATIONS_EXT>> grouped = null;

            switch (dateGroup)
            {
                case DateGroupType.day:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + ((o.OPE_DATE.Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + o.OPE_DATE.Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + (((o.OPE_DATE.Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString() + "-" + (((o.OPE_DATE.Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = operations.GroupBy(o => o.OPE_DATE.Value.Year.ToString());
                    break;
            }

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count(),
                                                                                      (int) 0,
                                                                                      (int) 0,
                                                                                      new float[] { (g.Sum(o => (float)
                                                                                                                    (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? o.OPE_AMOUNT.Value * curChanges["EUR"] : 
                                                                                                                     (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? o.OPE_AMOUNT.Value * curChanges["USD"] :
                                                                                                                      (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? o.OPE_AMOUNT.Value * curChanges["CAD"] :
                                                                                                                       (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? o.OPE_AMOUNT.Value * curChanges["GBP"] :
                                                                                                                    o.OPE_AMOUNT.Value))))) / 100) },
                                                                                      new float[] { 0 },
                                                                                      new float[] { 0 }))
                          .ToList();

            return oRet;
        }*/

        public static IList<ChartDataItem> RechargesAvgDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_HOUR>();

            // apply installations an groups filter
            //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);


            var operations = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              select o).AsQueryable();
            
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.RECHARGES_COUNT ?? 0),
                                                                                      (int)0,
                                                                                      (int)0,
                                                                                      new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_AMOUNT ?? 0)))))) / 100 },
                                                                                      new float[] { 0 },
                                                                                      new float[] { 0 }))
                          .ToList();

            return oRet;
        }
        public static IList<ChartDataItem> RechargesAvgDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();

                // apply installations an groups filter
                //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);


                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                CURRENCy oCurrency = null;
                Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

                
                //List<ChartDataItem> oRet2 = ordered.Select("new ChartDataItem(it.Key.ToString(), it.Sum(RECHARGES_COUNT))", "it").AsQueryable().Cast<ChartDataItem>().ToList();
                
                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), 
                                                                                          g.Sum(o => o.RECHARGES_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_REGULAR_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_AUTOMATIC_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_USERCREATION_COUNT ?? 0),
                                                                                          g.Sum(o => o.RECHARGES_CHANGEPM_COUNT ?? 0),
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_AMOUNT ?? 0)))))) / 100 },
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_REGULAR_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_REGULAR_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_REGULAR_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_REGULAR_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_REGULAR_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_REGULAR_AMOUNT ?? 0)))))) / 100 },
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AUTOMATIC_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AUTOMATIC_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AUTOMATIC_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AUTOMATIC_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AUTOMATIC_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_AUTOMATIC_AMOUNT ?? 0)))))) / 100 },
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_USERCREATION_AMOUNT ?? 0)))))) / 100 },
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_CHANGEPM_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_CHANGEPM_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_CHANGEPM_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_CHANGEPM_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_CHANGEPM_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_CHANGEPM_AMOUNT ?? 0)))))) / 100 }))
                              .ToList();
            }

            return oRet;
        }

        public static IList<ChartDataItem> InscriptionsData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<CUSTOMER_INSCRIPTION>();

            // apply installations an groups filter
            //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterLastSentDate(predicate, dateFilter, iniDate, endDate);

            // apply created user filter
            predicate = predicate.And(o => o.CUISINS_CUS_ID.HasValue);


            var inscriptions = (from o in backOfficeRepository.GetCustomerInscriptions(predicate)
                              select o).AsQueryable();


            IQueryable<IGrouping<string, CUSTOMER_INSCRIPTION>> grouped = null;

            switch (dateGroup)
            {
                case DateGroupType.day:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Year.ToString() + "-" + ((o.CUSINS_LAST_SENT_DATE.Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Year.ToString() + "-" + o.CUSINS_LAST_SENT_DATE.Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Year.ToString() + "-" + (((o.CUSINS_LAST_SENT_DATE.Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Year.ToString() + "-" + (((o.CUSINS_LAST_SENT_DATE.Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = inscriptions.GroupBy(o => o.CUSINS_LAST_SENT_DATE.Value.Year.ToString());
                    break;
            }

            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count()))
                          .ToList();

            return oRet;
        }

        public static IList<ChartDataItem> InscriptionsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime, UserInstallationType eUserInstallation)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<VW_INSCRIPTIONS_HOUR>();

                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations, eUserInstallation);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var inscriptions = (from o in backOfficeRepository.GetVwInscriptionsHour(predicate)
                                    select o).AsQueryable();


                IQueryable<IGrouping<string, VW_INSCRIPTIONS_HOUR>> grouped = null;
                IOrderedQueryable<IGrouping<string, VW_INSCRIPTIONS_HOUR>> ordered = null;

                switch (dateGroup)
                {
                    case DateGroupType.hour:
                        if (!dateGroupPattern)
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Hour.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.weekday:
                        if (!dateGroupPattern)
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + o.HOUR.Value.Month.ToString() + "/" + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.Value.DayOfWeek + 1 : o.HOUR.Value.DayOfWeek)).ToString());
                        }
                        else
                            grouped = inscriptions.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.Value.DayOfWeek + 1 : o.HOUR.Value.DayOfWeek)).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.day:
                        if (!dateGroupPattern)
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Date.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Day.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.week:
                        if (!dateGroupPattern)
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.DayOfYear / 7).ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = inscriptions.GroupBy(o => (o.HOUR.Value.DayOfYear / 7).ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.month:
                        if (!dateGroupPattern)
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + o.HOUR.Value.Month.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Month.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.quarter:
                        if (!dateGroupPattern)
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                        else
                            grouped = inscriptions.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.half:
                        if (!dateGroupPattern)
                            grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                        else
                            grouped = inscriptions.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.year:
                        //if (!dateGroupPattern)
                        grouped = inscriptions.GroupBy(o => o.HOUR.Value.Year.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                }

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(i => i.INSCRIPTIONS_COUNT ?? 0)))
                              .ToList();
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> InscriptionsAvgData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<CUSTOMER_PAYMENT_MEANS_RECHARGE>();

            // apply installations an groups filter
            //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate);

            predicate = predicate.And(t => t.USER.USR_ENABLED == 1 && 
                                           (new int[] { (int)PaymentMeanRechargeStatus.Committed }).Contains(t.CUSPMR_TRANS_STATUS) && 
                                           t.CUSPMR_SUSCRIPTION_TYPE == 1);

            integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();

            var recharges = (from o in backOfficeRepository.GetCustomerRecharges(predicate, dbContext)
                             select o)
                            //.Where(r => dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Where(r2 => r2.USER.USR_ENABLED == 1 &&
                            //                                                                   (new int[] { (int)PaymentMeanRechargeStatus.Committed }).Contains(r2.CUSPMR_TRANS_STATUS) && r2.CUSPMR_SUSCRIPTION_TYPE == 1 &&
                            //                                                                   r2.CUSPMR_USR_ID == r.CUSPMR_USR_ID && 
                            //                                                                   r2.CUSPMR_UTC_DATE < r.CUSPMR_UTC_DATE).Count() == 0)
                            .AsQueryable();
            
            IQueryable<IGrouping<string, CUSTOMER_PAYMENT_MEANS_RECHARGE>> grouped = null;

            switch (dateGroup)
            {
                case DateGroupType.day:
                    grouped = recharges.GroupBy(t => t.CUSPMR_UTC_DATE.Value.Date.ToString());
                    break;
                case DateGroupType.week:
                    grouped = recharges.GroupBy(o => o.CUSPMR_UTC_DATE.Value.Year.ToString() + "-" + ((o.CUSPMR_UTC_DATE.Value.DayOfYear / 7) + 1).ToString());
                    break;
                case DateGroupType.month:
                    grouped = recharges.GroupBy(o => o.CUSPMR_UTC_DATE.Value.Year.ToString() + "-" + o.CUSPMR_UTC_DATE.Value.Month.ToString());
                    break;
                case DateGroupType.quarter:
                    grouped = recharges.GroupBy(o => o.CUSPMR_UTC_DATE.Value.Year.ToString() + "-" + (((o.CUSPMR_UTC_DATE.Value.Month - 1) / 3) + 1).ToString());
                    break;
                case DateGroupType.half:
                    grouped = recharges.GroupBy(o => o.CUSPMR_UTC_DATE.Value.Year.ToString() + "-" + (((o.CUSPMR_UTC_DATE.Value.Month - 1) / 6) + 1).ToString());
                    break;
                case DateGroupType.year:
                    grouped = recharges.GroupBy(o => o.CUSPMR_UTC_DATE.Value.Year.ToString());
                    break;
            }

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = grouped.OrderBy(g => g.Key)
                          .Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Count(r => dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Where(r2 => r2.USER.USR_ENABLED == 1 &&
                                                                                               (new int[] { (int)PaymentMeanRechargeStatus.Committed }).Contains(r2.CUSPMR_TRANS_STATUS) && r2.CUSPMR_SUSCRIPTION_TYPE == 1 &&
                                                                                               r2.CUSPMR_USR_ID == r.CUSPMR_USR_ID &&
                                                                                               r2.CUSPMR_UTC_DATE < r.CUSPMR_UTC_DATE).Count() == 0),
                                                                                      g.Select(r => r.CUSPMR_USR_ID).Distinct().Count(),
                                                                                      0,
                                                                                      new float[] { (float) g.Where(r => dbContext.CUSTOMER_PAYMENT_MEANS_RECHARGEs.Where(r2 => r2.USER.USR_ENABLED == 1 &&
                                                                                               (new int[] { (int)PaymentMeanRechargeStatus.Committed }).Contains(r2.CUSPMR_TRANS_STATUS) && r2.CUSPMR_SUSCRIPTION_TYPE == 1 &&
                                                                                               r2.CUSPMR_USR_ID == r.CUSPMR_USR_ID && 
                                                                                               r2.CUSPMR_UTC_DATE < r.CUSPMR_UTC_DATE).Count() == 0)
                                                                                                .Sum(o => (o.CUSPMR_CUR_ID == 50 ? o.CUSPMR_AMOUNT * curChanges["EUR"] :
                                                                                                           (o.CUSPMR_CUR_ID == 150 ? o.CUSPMR_AMOUNT * curChanges["USD"] :
                                                                                                            (o.CUSPMR_CUR_ID == 28 ? o.CUSPMR_AMOUNT * curChanges["CAD"] :
                                                                                                             (o.CUSPMR_CUR_ID == 22 ? o.CUSPMR_AMOUNT * curChanges["GBP"] :
                                                                                                          o.CUSPMR_AMOUNT))))) / 100},
                                                                                      new float[] { (float) g.Sum(o => (o.CUSPMR_CUR_ID == 50 ? o.CUSPMR_AMOUNT * curChanges["EUR"] :
                                                                                                           (o.CUSPMR_CUR_ID == 150 ? o.CUSPMR_AMOUNT * curChanges["USD"] :
                                                                                                            (o.CUSPMR_CUR_ID == 28 ? o.CUSPMR_AMOUNT * curChanges["CAD"] :
                                                                                                             (o.CUSPMR_CUR_ID == 22 ? o.CUSPMR_AMOUNT * curChanges["GBP"] :
                                                                                                          o.CUSPMR_AMOUNT))))) / 100}, 
                                                                                      new float[] {}))
                          .ToList();

            return oRet;
        }*/

        /*public static IList<ChartDataItem> InscriptionsAvgDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_RECHARGES_HOUR>();

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
            
            var recharges = (from o in backOfficeRepository.GetVwRechargesHour(predicate)
                             select o)
                            .AsQueryable();

            IQueryable<IGrouping<string, VW_RECHARGES_HOUR>> grouped = null;
            IOrderedQueryable<IGrouping<string, VW_RECHARGES_HOUR>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {                        
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + o.HOUR.Value.Month.ToString() + "/" + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.Value.DayOfWeek + 1 : o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = recharges.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.Value.DayOfWeek + 1 : o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    if (!dateGroupPattern)
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.DayOfYear / 7).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = recharges.GroupBy(o => (o.HOUR.Value.DayOfYear / 7).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = recharges.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = recharges.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                        grouped = recharges.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            CURRENCy oCurrency = null;
            Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),  
                                                                                      g.Where(r => r.PREVRECHARGES_COUNT == 0).Select(r => r.OPE_USR_ID).Distinct().Count(),                                                                                        
                                                                                      g.Select(r => r.OPE_USR_ID).Distinct().Count(),
                                                                                      0,
                                                                                      new float[] { (float)g.Sum(o => ((o.PREVRECHARGES_COUNT??0) == 0 ? 
                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                             o.RECHARGES_AMOUNT ?? 0)))) : 0)) / 100 },
                                                                                      new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                             o.RECHARGES_AMOUNT ?? 0))))) / 100 },
                                                                                      new float[] { }))
                          .ToList();

            return oRet;
        }*/
        public static IList<ChartDataItem> InscriptionsAvgDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, decimal currencyId, string iniTime, string endTime, UserInstallationType eUserInstallation)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_RECHARGES_HOUR>();

                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations, eUserInstallation);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var recharges = (from o in backOfficeRepository.GetDbRechargesHour(predicate)
                                 select o)
                                .AsQueryable();

                IQueryable<IGrouping<string, DB_RECHARGES_HOUR>> grouped = null;
                IOrderedQueryable<IGrouping<string, DB_RECHARGES_HOUR>> ordered = null;

                switch (dateGroup)
                {
                    case DateGroupType.hour:
                        if (!dateGroupPattern)
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Hour.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.weekday:
                        if (!dateGroupPattern)
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Year.ToString() + "/" + o.HOUR.Month.ToString() + "/" + o.HOUR.Day.ToString() + "-" + ((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.DayOfWeek + 1 : o.HOUR.DayOfWeek)).ToString());
                        }
                        else
                            grouped = recharges.GroupBy(o => ((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? o.HOUR.DayOfWeek + 1 : o.HOUR.DayOfWeek)).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.day:
                        if (!dateGroupPattern)
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Date.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Day.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.week:
                        if (!dateGroupPattern)
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Year.ToString() + "-" + (o.HOUR.DayOfYear / 7).ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = recharges.GroupBy(o => (o.HOUR.DayOfYear / 7).ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.month:
                        if (!dateGroupPattern)
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Year.ToString() + "-" + o.HOUR.Month.ToString());
                            ordered = grouped.OrderBy(g => g.Key);
                        }
                        else
                        {
                            grouped = recharges.GroupBy(o => o.HOUR.Month.ToString());
                            ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                        }
                        break;
                    case DateGroupType.quarter:
                        if (!dateGroupPattern)
                            grouped = recharges.GroupBy(o => o.HOUR.Year.ToString() + "-" + (((o.HOUR.Month - 1) / 3) + 1).ToString());
                        else
                            grouped = recharges.GroupBy(o => (((o.HOUR.Month - 1) / 3) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.half:
                        if (!dateGroupPattern)
                            grouped = recharges.GroupBy(o => o.HOUR.Year.ToString() + "-" + (((o.HOUR.Month - 1) / 6) + 1).ToString());
                        else
                            grouped = recharges.GroupBy(o => (((o.HOUR.Month - 1) / 6) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                    case DateGroupType.year:
                        //if (!dateGroupPattern)
                        grouped = recharges.GroupBy(o => o.HOUR.Year.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        break;
                }

                CURRENCy oCurrency = null;
                Dictionary<string, double> curChanges = GetCurrencyChanges(backOfficeRepository, currencyId, predicate, out oCurrency);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),
                                                                                          //g.Where(r => r.PREVRECHARGES_COUNT == 0).Select(r => r.OPE_USR_ID).Distinct().Count(),
                                                                                          g.Where(r => r.RECHARGES_USERCREATION_COUNT > 0).Select(r => r.OPE_USR_ID).Distinct().Count(),
                                                                                          g.Select(r => r.OPE_USR_ID).Distinct().Count(),
                                                                                          0,
                                                                                          /*new float[] { (float)g.Sum(o => ((o.PREVRECHARGES_COUNT??0) == 0 ? 
                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                             o.RECHARGES_AMOUNT ?? 0)))) : 0)) / 100 },*/
                                                                                          new float[] { (float)g.Sum(o => ((o.RECHARGES_USERCREATION_COUNT??0) > 0 ? 
                                                                                                        (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_USERCREATION_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_USERCREATION_AMOUNT ?? 0))))) : 0)) / 100 },
                                                                                          new float[] { (float)g.Sum(o => (o.OPE_AMOUNT_CUR_ISO_CODE == "EUR" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["EUR"] :
                                                                                                         (o.OPE_AMOUNT_CUR_ISO_CODE == "USD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["USD"] :
                                                                                                          (o.OPE_AMOUNT_CUR_ISO_CODE == "CAD" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["CAD"] :
                                                                                                           (o.OPE_AMOUNT_CUR_ISO_CODE == "GBP" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["GBP"] :
                                                                                                            (o.OPE_AMOUNT_CUR_ISO_CODE == "MXN" ? (o.RECHARGES_AMOUNT ?? 0) * curChanges["MXN"] :
                                                                                                              o.RECHARGES_AMOUNT ?? 0)))))) / 100 },
                                                                                          new float[] { }))
                              .ToList();
            }

            return oRet;
        }

        public static IList<ChartDataItem> InscriptionsPlatformDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime, UserInstallationType eUserInstallation)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<VW_INSCRIPTIONS_PLATFORM_HOUR>();

                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations, eUserInstallation);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var inscriptions = (from o in backOfficeRepository.GetVwInscriptionsPlatformHour(predicate)
                                   select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, VW_INSCRIPTIONS_PLATFORM_HOUR>> ordered = GroupOrder(inscriptions, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (float)g.Sum(o => o.INSCRIPTIONS_IOS_COUNT ?? 0),
                                                                                                            (float)g.Sum(o => o.INSCRIPTIONS_ANDROID_COUNT ?? 0),
                                                                                                            (float)g.Sum(o => o.INSCRIPTIONS_WINDOWSPHONE_COUNT ?? 0),
                                                                                                            (float)g.Sum(o => o.INSCRIPTIONS_WEB_COUNT ?? 0))
                                     ).ToList();

            }

            return oRet;
        }

        public static IList<ChartDataItem> InscriptionsPlatformTotalsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime, UserInstallationType eUserInstallation)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<VW_INSCRIPTIONS_PLATFORM_HOUR>();

                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations, eUserInstallation);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var inscriptions = (from o in backOfficeRepository.GetVwInscriptionsPlatformHour(predicate)
                                    select o).AsQueryable();

                int totaliOS = 0;
                int totalAndroid = 0;
                int totalWindowsPhone = 0;
                int totalWeb = 0;
                int total = 0;

                if (inscriptions.Count() > 0)
                {
                    totaliOS = inscriptions.Sum(o => o.INSCRIPTIONS_IOS_COUNT ?? 0);
                    totalAndroid = inscriptions.Sum(o => o.INSCRIPTIONS_ANDROID_COUNT ?? 0);
                    totalWindowsPhone = inscriptions.Sum(o => o.INSCRIPTIONS_WINDOWSPHONE_COUNT ?? 0);
                    totalWeb = inscriptions.Sum(o => o.INSCRIPTIONS_WEB_COUNT ?? 0);
                    total = totaliOS + totalAndroid + totalWindowsPhone + totalWeb;
                }

                ResourceBundle resBundle = ResourceBundle.GetInstance();

                oRet = new List<ChartDataItem>();
                oRet.Add(new ChartDataItem(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.iOS"), (total > 0 ? (totaliOS * 100) / total : 0)));
                oRet.Add(new ChartDataItem(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Android"), (total > 0 ? (totalAndroid * 100) / total : 0)));
                oRet.Add(new ChartDataItem(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.WindowsPhone"), (total > 0 ? (totalWindowsPhone * 100) / total : 0)));
                oRet.Add(new ChartDataItem(resBundle.GetString("Maintenance", "TypeEnums.MobileOSs.Web"), (total > 0 ? (totalWeb * 100) / total : 0)));
            }

            return oRet;
        }

        public static IList<ChartDataItem> InscriptionsPlatformDataAcums(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime, UserInstallationType eUserInstallation)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<VW_INSCRIPTIONS_PLATFORM_HOUR>();

                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations, eUserInstallation);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var inscriptions = (from o in backOfficeRepository.GetVwInscriptionsPlatformHour(predicate)
                                    select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, VW_INSCRIPTIONS_PLATFORM_HOUR>> ordered = GroupOrder(inscriptions, dateGroup, dateGroupPattern);

                oRet = ordered.ToList().Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i==0), g.Sum(o => o.INSCRIPTIONS_IOS_COUNT ?? 0),
                                                                                                                         g.Sum(o => o.INSCRIPTIONS_ANDROID_COUNT ?? 0),
                                                                                                                         g.Sum(o => o.INSCRIPTIONS_WINDOWSPHONE_COUNT ?? 0),
                                                                                                                         g.Sum(o => o.INSCRIPTIONS_WEB_COUNT ?? 0))
                                     ).ToList();

            }

            return oRet;
        }

        public static IList<ChartDataItem> UsersInstallationsData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<USER>();

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var oInstallationsList = installations.Where(i => i.StartsWith("I")).Select(i => Convert.ToDecimal(i.Substring(1))).ToList();

                predicate = predicate.And(u => u.USR_ENABLED == 1 &&                             
                                               ((u.INSTALLATION_AUTOMATIC_ASSIGNATION == null && oInstallationsList.Contains(0)) || (u.INSTALLATION_AUTOMATIC_ASSIGNATION.INSTALLATION == null && oInstallationsList.Contains(0)) ||
                                                oInstallationsList.Contains(u.INSTALLATION_AUTOMATIC_ASSIGNATION.INAA_INS_ID.Value)));



                var users = backOfficeRepository.GetUsers(predicate).AsQueryable();

                int iTotalUsers = users.Count();

                //oRet = users.GroupBy(u => u.INSTALLATION_AUTOMATIC_ASSIGNATION)
                //            .Select(g => new ChartDataItem((g.Key != null && g.Key.INSTALLATION != null ? g.Key.INAA_DESCRIPTION : "Otros"), 
                //                                           g.Count(), (g.Count() * 100)/iTotalUsers))
                //            .ToList();

                ResourceBundle resBundle = ResourceBundle.GetInstance();
                var sOthers = resBundle.GetString("PBPPlugin", "Dashboard_UsersInstallations_Others", "Others");

                oRet = users.GroupBy(u => (u.INSTALLATION_AUTOMATIC_ASSIGNATION != null && u.INSTALLATION_AUTOMATIC_ASSIGNATION.INSTALLATION != null ? u.INSTALLATION_AUTOMATIC_ASSIGNATION.INAA_DESCRIPTION : sOthers))
                            .Select(g => new ChartDataItem(g.Key, 
                                                           g.Count(), (float)(g.Count() * 100)/(float)iTotalUsers))
                            .ToList();
                
                /*IOrderedQueryable<IGrouping<string, OperationsUsers>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),
                    // Number of users that make an operation in this period
                                                                                          g.Sum(o => o.USERS_COUNT ?? 0), //g.Select(o => o.OPE_USR_ID).Distinct().Count(), //.Sum(o => o.USERS_COUNT ?? 0),
                                                                                          0,
                                                                                          0,
                                                                                          new float[] {
                                                                                                    (float) g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.RECHARGES_COUNT ?? 0)},
                                                                                          new float[] { },
                                                                                          new float[] { }))
                               .ToList();*/
            }

            return oRet;
        }

        public static IList<ChartDataItem> UsersInstallationsFirstOperationData(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<USER>();

                // apply date filter
                predicate = FilterDateUserOperativeDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

                var oInstallationsList = installations.Where(i => i.StartsWith("I")).Select(i => Convert.ToDecimal(i.Substring(1))).ToList();

                predicate = predicate.And(u => u.USR_ENABLED == 1 &&
                                               ((u.INSTALLATION == null && oInstallationsList.Contains(0)) ||
                                                (u.USR_FIRST_OPERATION_INS_ID.HasValue && oInstallationsList.Contains(u.USR_FIRST_OPERATION_INS_ID.Value))));

                var users = backOfficeRepository.GetUsers(predicate).AsQueryable();

                int iTotalUsers = users.Count();

                ResourceBundle resBundle = ResourceBundle.GetInstance();
                var sNoInstallation = resBundle.GetString("PBPPlugin", "Dashboard_UsersInstallationsFirstOperation_NoInstallation", "No Installation");

                oRet = users.GroupBy(u => u.INSTALLATION)
                            .Select(g => new ChartDataItem((g.Key != null ? g.Key.INS_DESCRIPTION : sNoInstallation),
                                                           g.Count(), (float)(g.Count() * 100) / (float)iTotalUsers))
                            .ToList()
                            .OrderBy(g => g.Category)
                            .ToList();

                
            }

            return oRet;
        }

        /*public static IList<ChartDataItem> OperationsUserDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_USER_HOUR>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            var operations = (from o in backOfficeRepository.GetVwOperationsUserHour(predicate)
                              select o).AsQueryable();

            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_USER_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), 
                                                                                      // Number of users that make an operation in this period
                                                                                      g.Select(o => o.OPE_USR_ID).Distinct().Count(), //.Sum(o => o.USERS_COUNT ?? 0),
                                                                                      0,
                                                                                      0,
                                                                                      new float[] {
                                                                                                    (float) g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0)},
                                                                                      new float[] { },
                                                                                      new float[] { }))
                           .ToList();

            return oRet;
        }*/
        public static IList<ChartDataItem> OperationsUserDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();
                var predicateUsers = PredicateBuilder.True<DB_OPERATIONS_USERS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
                predicateUsers = FilterInstallationsAndGroups(backOfficeRepository, predicateUsers, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                predicateUsers = FilterDate(predicateUsers, dateFilter, iniDate, endDate, iniTime, endTime);

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  join u in backOfficeRepository.GetDbOperationsUsersHour(predicateUsers)
                                  on new { o.HOUR, o.OPE_INS_ID, o.GRP_ID } equals new { u.HOUR, u.OPE_INS_ID, u.GRP_ID }
                                  select new OperationsUsers()
                                  {
                                      HOUR = o.HOUR,
                                      PARKINGS_COUNT = o.PARKINGS_COUNT,
                                      EXTENSIONS_COUNT = o.EXTENSIONS_COUNT,
                                      REFUNDS_COUNT = o.REFUNDS_COUNT,
                                      RECHARGES_COUNT = o.RECHARGES_COUNT,
                                      USERS_COUNT = u.USERS_COUNT,
                                      USERS_ALL_COUNT = u.USERS_ALL_COUNT
                                  }).AsQueryable();

                IOrderedQueryable<IGrouping<string, OperationsUsers>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),
                    // Number of users that make an operation in this period
                                                                                          g.Sum(o => o.USERS_COUNT ?? 0), //g.Select(o => o.OPE_USR_ID).Distinct().Count(), //.Sum(o => o.USERS_COUNT ?? 0),
                                                                                          0,
                                                                                          0,
                                                                                          new float[] {
                                                                                                    (float) g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.RECHARGES_COUNT ?? 0)},
                                                                                          new float[] { },
                                                                                          new float[] { }))
                               .ToList();
            }

            return oRet;
        }

        public static IList<ChartDataItem> OperationsUserAllDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {            
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            var predicate = PredicateBuilder.True<VW_OPERATIONS_USER_HOUR>();

            // apply installations an groups filter
            predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

            // apply date filter
            predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);

            var operations = (from o in backOfficeRepository.GetVwOperationsUserHour(predicate)
                              select o).AsQueryable();

            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_USER_HOUR>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

            oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), 
                                                                                      // Number total of enabled users in this period
                                                                                      10 /*g.First().USERS_ALL_COUNT ?? 0*/, //.Sum(o => o.USERS_ALL_COUNT ?? 0),
                                                                                      0,
                                                                                      0,
                                                                                      new float[] {
                                                                                                    (float) g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0)},
                                                                                      new float[] { },
                                                                                      new float[] { }))
                           .ToList();

            return oRet;
        }
        public static IList<ChartDataItem> OperationsUserAllDataViews2(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                var predicate = PredicateBuilder.True<DB_OPERATIONS_HOUR>();
                var predicateUsers = PredicateBuilder.True<DB_OPERATIONS_USERS_HOUR>();

                // apply installations an groups filter
                predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);
                predicateUsers = FilterInstallationsAndGroups(backOfficeRepository, predicateUsers, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                predicateUsers = FilterDate(predicateUsers, dateFilter, iniDate, endDate, iniTime, endTime);

                var operations = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                                  join u in backOfficeRepository.GetDbOperationsUsersHour(predicateUsers)
                                  on new { o.HOUR, o.OPE_INS_ID, o.GRP_ID } equals new { u.HOUR, u.OPE_INS_ID, u.GRP_ID }
                                  select new OperationsUsers()
                                  {
                                      HOUR = o.HOUR,
                                      PARKINGS_COUNT = o.PARKINGS_COUNT,
                                      EXTENSIONS_COUNT = o.EXTENSIONS_COUNT,
                                      REFUNDS_COUNT = o.REFUNDS_COUNT,
                                      RECHARGES_COUNT = o.RECHARGES_COUNT,
                                      USERS_COUNT = u.USERS_COUNT,
                                      USERS_ALL_COUNT = u.USERS_ALL_COUNT
                                  }).AsQueryable();

                IOrderedQueryable<IGrouping<string, OperationsUsers>> ordered = GroupOrder(operations, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),
                    // Number total of enabled users in this period
                                                                                          (int)g.Average(o => o.USERS_ALL_COUNT).Value /*g.First().USERS_ALL_COUNT ?? 0*/, //.Sum(o => o.USERS_ALL_COUNT ?? 0),
                                                                                          0,
                                                                                          0,
                                                                                          new float[] {
                                                                                                    (float) g.Sum(o => o.PARKINGS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.REFUNDS_COUNT ?? 0),
                                                                                                    (float) g.Sum(o => o.EXTENSIONS_COUNT ?? 0)},
                                                                                          new float[] { },
                                                                                          new float[] { }))
                               .ToList();
            }

            return oRet;
        }

        public static IList<ChartDataItem> InvitationsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<Select_DB_INVITATIONS_HOURResult>();

                // apply installations an groups filter
                //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                DateTime xIni;
                DateTime xEnd;
                GetDateRange(dateFilter, iniDate, endDate, out xIni, out xEnd);

                /*if (string.IsNullOrEmpty(iniTime)) iniTime = "00:00";
                if (string.IsNullOrEmpty(endTime)) endTime = "23:59";
                xIni = new DateTime(xIni.Year, xIni.Month, xIni.Day, Convert.ToInt32(iniTime.Split(':')[0]), Convert.ToInt32(iniTime.Split(':')[1]), 0);
                xEnd = new DateTime(xEnd.Year, xEnd.Month, xEnd.Day, Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);*/

                var invitations = (from o in backOfficeRepository.GetDbInvitationsHour(xIni, xEnd, predicate)
                                  select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, Select_DB_INVITATIONS_HOURResult>> ordered = GroupOrder(invitations, dateGroup, dateGroupPattern);

                switch (dateGroup)
                {
                    case DateGroupType.hour:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HOUR ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HOUR ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HOUR_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HOUR_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.day:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.weekday:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEKDAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEKDAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.week:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEK ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEK ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEK_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEK_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();                           
                        }
                        break;
                    case DateGroupType.month:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_MONTH ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_MONTH ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_MONTH_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_MONTH_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.quarter:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_QUARTER ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_QUARTER ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_QUARTER_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_QUARTER_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.half:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HALF ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HALF ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HALF_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HALF_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.year:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_YEAR ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_YEAR ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_YEAR_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_YEAR_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                }

            }

            return oRet;
        }
        public static IList<ChartDataItem> InvitationsDataAcums(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<Select_DB_INVITATIONS_HOURResult>();

                // apply installations an groups filter
                //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                DateTime xIni;
                DateTime xEnd;
                GetDateRange(dateFilter, iniDate, endDate, out xIni, out xEnd);

                /*if (string.IsNullOrEmpty(iniTime)) iniTime = "00:00";
                if (string.IsNullOrEmpty(endTime)) endTime = "23:59";
                xIni = new DateTime(xIni.Year, xIni.Month, xIni.Day, Convert.ToInt32(iniTime.Split(':')[0]), Convert.ToInt32(iniTime.Split(':')[1]), 0);
                xEnd = new DateTime(xEnd.Year, xEnd.Month, xEnd.Day, Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);*/

                var invitations = (from o in backOfficeRepository.GetDbInvitationsHour(xIni, xEnd, predicate)
                                   select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, Select_DB_INVITATIONS_HOURResult>> ordered = GroupOrder(invitations, dateGroup, dateGroupPattern);

                /*oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i==0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                         g.Sum(o => o.ACCEPTED ?? 0),
                                                                                                                         g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED ?? 0),
                                                                                                                         g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                ).ToList();*/
                switch (dateGroup)
                {
                    case DateGroupType.hour:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HOUR ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HOUR ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HOUR_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HOUR_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.day:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.weekday:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_DAY ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEKDAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEKDAY_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.week:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEK ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEK ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_WEEK_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_WEEK_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.month:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_MONTH ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_MONTH ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_MONTH_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_MONTH_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.quarter:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_QUARTER ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_QUARTER ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_QUARTER_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_QUARTER_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.half:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i==0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HALF ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HALF ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(),(i==0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_HALF_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_HALF_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                    case DateGroupType.year:
                        {
                            if (!dateGroupPattern)
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_YEAR ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_YEAR ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                            else
                                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i == 0), g.Sum(o => o.SENDED ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_YEAR_PART ?? 0),
                                                                                                                            g.Sum(o => o.SENDED ?? 0) - g.Sum(o => o.ACCEPTED_YEAR_PART ?? 0),
                                                                                                                            g.Sum(o => o.ACCEPTED_TOTAL ?? 0))
                                ).ToList();
                        }
                        break;
                }
            }

            return oRet;
        }

        public static IList<ChartDataItem> RechargeCouponsDataViews(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<Select_DB_RECHARGE_COUPONS_HOURResult>();

                // apply installations an groups filter
                //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                DateTime xIni;
                DateTime xEnd;
                GetDateRange(dateFilter, iniDate, endDate, out xIni, out xEnd);

                /*if (string.IsNullOrEmpty(iniTime)) iniTime = "00:00";
                if (string.IsNullOrEmpty(endTime)) endTime = "23:59";
                xIni = new DateTime(xIni.Year, xIni.Month, xIni.Day, Convert.ToInt32(iniTime.Split(':')[0]), Convert.ToInt32(iniTime.Split(':')[1]), 0);
                xEnd = new DateTime(xEnd.Year, xEnd.Month, xEnd.Day, Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);*/

                var rechargeCoupons = (from o in backOfficeRepository.GetDbRechargeCouponsHour(xIni, xEnd, predicate)
                                       select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, Select_DB_RECHARGE_COUPONS_HOURResult>> ordered = GroupOrder(rechargeCoupons, dateGroup, dateGroupPattern);

                oRet = ordered.Select(g => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), g.Sum(o => o.PURCHASED ?? 0),
                                                                                                            g.Sum(o => o.USED ?? 0))
                                ).ToList();

            }

            return oRet;
        }
        public static IList<ChartDataItem> RechargeCouponsDataAcums(IBackOfficeRepository backOfficeRepository, string[] installations, DateGroupType dateGroup, bool dateGroupPattern, DateFilterType dateFilter, DateTime? iniDate, DateTime? endDate, string iniTime, string endTime)
        {
            List<ChartDataItem> oRet = new List<ChartDataItem>();

            if (backOffice.Infrastructure.Security.FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {

                var predicate = PredicateBuilder.True<Select_DB_RECHARGE_COUPONS_HOURResult>();

                // apply installations an groups filter
                //predicate = FilterInstallationsAndGroups(backOfficeRepository, predicate, installations);

                // apply date filter
                predicate = FilterDate(predicate, dateFilter, iniDate, endDate, iniTime, endTime);
                DateTime xIni;
                DateTime xEnd;
                GetDateRange(dateFilter, iniDate, endDate, out xIni, out xEnd);

                /*if (string.IsNullOrEmpty(iniTime)) iniTime = "00:00";
                if (string.IsNullOrEmpty(endTime)) endTime = "23:59";
                xIni = new DateTime(xIni.Year, xIni.Month, xIni.Day, Convert.ToInt32(iniTime.Split(':')[0]), Convert.ToInt32(iniTime.Split(':')[1]), 0);
                xEnd = new DateTime(xEnd.Year, xEnd.Month, xEnd.Day, Convert.ToInt32(endTime.Split(':')[0]), Convert.ToInt32(endTime.Split(':')[1]), 0);*/

                var invitations = (from o in backOfficeRepository.GetDbRechargeCouponsHour(xIni, xEnd, predicate)
                                   select o).AsQueryable();

                IOrderedQueryable<IGrouping<string, Select_DB_RECHARGE_COUPONS_HOURResult>> ordered = GroupOrder(invitations, dateGroup, dateGroupPattern);

                oRet = ordered.Select((g, i) => new ChartDataItem(dateGroup, dateGroupPattern, g.Key.ToString(), (i==0), g.Sum(o => o.PURCHASED ?? 0),
                                                                                                                         g.Sum(o => o.USED ?? 0))
                ).ToList();

            }

            return oRet;
        }

        #endregion

        #region Private Static Methods

        private static bool GetInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, string[] installations, out Dictionary<decimal, List<decimal>> insGrpIds)
        {
            bool bRet = false;            
            insGrpIds = new Dictionary<decimal, List<decimal>>();

            if (installations != null)
            {
                // apply installations filter
                if (installations.Length > 0)
                {
                    decimal dIns = 0;
                    decimal dGrp = 0;
                    foreach (string sId in installations)
                    {
                        if (sId.StartsWith("I"))
                        {
                            dIns = decimal.Parse(sId.Substring(1));
                            if (!insGrpIds.ContainsKey(dIns))
                                insGrpIds.Add(dIns, new List<decimal>());
                        }
                        else if (sId.StartsWith("G"))
                        {
                            dGrp = decimal.Parse(sId.Substring(1));
                            //if (!grpIds.Contains(dGrp))
                            //{
                            var groups = backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>().And(g => g.GRP_ID == dGrp));
                                if (groups != null && groups.Count() > 0)
                                {
                                    GROUP oGroup = groups.First();
                                    dIns = oGroup.GRP_INS_ID;

                                    if (!insGrpIds.ContainsKey(dIns))
                                        insGrpIds.Add(dIns, new List<decimal>());

                                    if (!insGrpIds[dIns].Contains(oGroup.GRP_ID))
                                        insGrpIds[dIns].Add(oGroup.GRP_ID);

                                    var grpChilds = GetGroupChilds(oGroup);
                                    foreach (decimal dGrpChild in grpChilds)
                                    {
                                        if (!insGrpIds[dIns].Contains(dGrpChild))
                                            insGrpIds[dIns].Add(dGrpChild);
                                    }
                                }
                            //}
                        }
                        
                    }
                }
                bRet = true;
            }

            return bRet;
        }

        private static Expression<Func<OPERATION, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<OPERATION, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))            
            {
                var predicateAllIns = PredicateBuilder.False<OPERATION>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<OPERATION>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<OPERATION>();
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {                            
                            predicateGrps = predicateGrps.Or(o => o.OPE_GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);                
            }

            return predicate;
        }

        private static Expression<Func<ALL_OPERATIONS_EXT, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, string[] installations)
        {            
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID.HasValue);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<ALL_OPERATIONS_EXT>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<ALL_OPERATIONS_EXT>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<ALL_OPERATIONS_EXT>();
                        predicateGrps = predicateGrps.Or(o => o.OPE_TYPE != (int)ChargeOperationsType.ParkingOperation);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);

                /*// apply installations filter
                predicate = predicate.And(o => insIds.Contains(o.OPE_INS_ID.Value));

                // apply groups filter
                if (grpIds.Count() > 0)
                {
                    predicate = predicate.And(o => o.GRP_ID.HasValue && grpIds.Contains(o.GRP_ID.Value));
                }*/
            }

            return predicate;
        }

        private static Expression<Func<TICKET_PAYMENT, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<TICKET_PAYMENT, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<TICKET_PAYMENT>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<TICKET_PAYMENT>();
                    predicateIns = predicateIns.And(o => o.TIPA_INS_ID == dIns); // apply installation filter
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }

        private static Expression<Func<VW_OPERATIONS_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID.HasValue);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<VW_OPERATIONS_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<VW_OPERATIONS_HOUR>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<VW_OPERATIONS_HOUR>();
                        predicateGrps = predicateGrps.Or(o => !o.GRP_ID.HasValue);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<DB_OPERATIONS_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID != 0);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<DB_OPERATIONS_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<DB_OPERATIONS_HOUR>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<DB_OPERATIONS_HOUR>();
                        predicateGrps = predicateGrps.Or(o => o.GRP_ID == 0);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }

        private static Expression<Func<VW_OPERATIONS_MINUTE, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID.HasValue);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<VW_OPERATIONS_MINUTE>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<VW_OPERATIONS_MINUTE>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<VW_OPERATIONS_MINUTE>();
                        predicateGrps = predicateGrps.Or(o => !o.GRP_ID.HasValue);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<DB_OPERATIONS_MINUTE, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID != 0);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<DB_OPERATIONS_MINUTE>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<DB_OPERATIONS_MINUTE>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps = PredicateBuilder.False<DB_OPERATIONS_MINUTE>();
                        predicateGrps = predicateGrps.Or(o => o.GRP_ID == 0);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }

        private static Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID.HasValue);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns =PredicateBuilder.False<VW_OPERATIONS_USER_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns =PredicateBuilder.True<VW_OPERATIONS_USER_HOUR>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps =PredicateBuilder.False<VW_OPERATIONS_USER_HOUR>();
                        predicateGrps = predicateGrps.Or(o => !o.GRP_ID.HasValue);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate, string[] installations)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            predicate = predicate.And(o => o.OPE_INS_ID != 0);

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns =PredicateBuilder.False<DB_OPERATIONS_USERS_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns =PredicateBuilder.True<DB_OPERATIONS_USERS_HOUR>();
                    predicateIns = predicateIns.And(o => o.OPE_INS_ID == dIns); // apply installation filter
                    if (insGrpIds[dIns].Count > 0)
                    {
                        var predicateGrps =PredicateBuilder.False<DB_OPERATIONS_USERS_HOUR>();
                        predicateGrps = predicateGrps.Or(o => o.GRP_ID == 0);
                        foreach (decimal dGrp in insGrpIds[dIns])
                        {
                            predicateGrps = predicateGrps.Or(o => o.GRP_ID == dGrp); // apply group filter
                        }
                        predicateIns = predicateIns.And(predicateGrps);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate, string[] installations, UserInstallationType eUserInstallation)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                //if (!insGrpIds.ContainsKey(0))
                //    predicate = predicate.And(o => o.USR_FIRST_OPERATION_INS_ID.HasValue);

                var predicateAllIns = PredicateBuilder.False<VW_INSCRIPTIONS_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<VW_INSCRIPTIONS_HOUR>();
                    if (eUserInstallation == UserInstallationType.FirstOperation)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.USR_FIRST_OPERATION_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => !o.USR_FIRST_OPERATION_INS_ID.HasValue);
                    }
                    else if (eUserInstallation == UserInstallationType.LadaKey)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.INAA_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => !o.INAA_INS_ID.HasValue);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate, string[] installations, UserInstallationType eUserInstallation)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                //if (!insGrpIds.ContainsKey(0))
                //    predicate = predicate.And(o => o.USR_FIRST_OPERATION_INS_ID.HasValue);

                var predicateAllIns = PredicateBuilder.False<VW_INSCRIPTIONS_PLATFORM_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<VW_INSCRIPTIONS_PLATFORM_HOUR>();
                    if (eUserInstallation == UserInstallationType.FirstOperation)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.USR_FIRST_OPERATION_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => !o.USR_FIRST_OPERATION_INS_ID.HasValue);
                    }
                    else if (eUserInstallation == UserInstallationType.LadaKey)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.INAA_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => !o.INAA_INS_ID.HasValue);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }
        private static Expression<Func<DB_RECHARGES_HOUR, bool>> FilterInstallationsAndGroups(IBackOfficeRepository backOfficeRepository, Expression<Func<DB_RECHARGES_HOUR, bool>> predicate, string[] installations, UserInstallationType eUserInstallation)
        {
            Dictionary<decimal, List<decimal>> insGrpIds = null;

            if (GetInstallationsAndGroups(backOfficeRepository, installations, out insGrpIds))
            {
                var predicateAllIns = PredicateBuilder.False<DB_RECHARGES_HOUR>();
                foreach (decimal dIns in insGrpIds.Keys)
                {
                    var predicateIns = PredicateBuilder.True<DB_RECHARGES_HOUR>();
                    if (eUserInstallation == UserInstallationType.FirstOperation)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.USER.USR_FIRST_OPERATION_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => !o.USER.USR_FIRST_OPERATION_INS_ID.HasValue);
                    }
                    else if (eUserInstallation == UserInstallationType.LadaKey)
                    {
                        if (dIns > 0)
                            predicateIns = predicateIns.And(o => o.USER.INSTALLATION_AUTOMATIC_ASSIGNATION != null && o.USER.INSTALLATION_AUTOMATIC_ASSIGNATION.INAA_INS_ID == dIns); // apply installation filter                        
                        else
                            predicateIns = predicateIns.And(o => o.USER.INSTALLATION_AUTOMATIC_ASSIGNATION == null || !o.USER.INSTALLATION_AUTOMATIC_ASSIGNATION.INAA_INS_ID.HasValue);
                    }
                    predicateAllIns = predicateAllIns.Or(predicateIns);
                }

                predicate = predicate.And(predicateAllIns);
            }

            return predicate;
        }

        private static List<decimal> GetGroupChilds(GROUP oParent)
        {
            List<decimal> oRet = new List<decimal>();
            foreach (var oGrpH in oParent.GROUPS_HIERARCHies1)
            {
                oRet.Add(oGrpH.GRHI_GPR_ID_CHILD);
                oRet.AddRange(GetGroupChilds(oGrpH.GROUP));
            }
            return oRet;
        }

        private static void GetDateRange(DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, out DateTime iniDate, out DateTime endDate)
        {
            DateTime xNow = DateTime.Now;

            endDate = xNow;

            switch (dateFilter)
            {
                case DateFilterType.currentWeek:
                    iniDate = xNow.Date.AddDays(-Conversions.Date2DayOfWeek(xNow));
                    break;
                case DateFilterType.currentMonth:
                    iniDate = new DateTime(xNow.Year, xNow.Month, 1);
                    break;
                case DateFilterType.currentQuarter:
                    iniDate = new DateTime(DateTime.Now.Year, (((xNow.Month - 1) / 3) * 3) + 1, 1);
                    break;
                case DateFilterType.currentHalf:
                    iniDate = new DateTime(DateTime.Now.Year, (((xNow.Month - 1) / 6) * 6) + 1, 1);
                    break;
                case DateFilterType.currentYear:
                    iniDate = new DateTime(DateTime.Now.Year, 1, 1);
                    break;

                case DateFilterType.lastWeek:
                    iniDate = xNow.AddDays(-7);
                    break;
                case DateFilterType.lastMonth:
                    iniDate = xNow.AddMonths(-1);
                    break;
                case DateFilterType.lastQuarter:
                    iniDate = xNow.AddMonths(-3);
                    break;
                case DateFilterType.lastHalf:
                    iniDate = xNow.AddMonths(-6);
                    break;
                case DateFilterType.lastYear:
                    iniDate = xNow.AddYears(-1);
                    break;

                case DateFilterType.custom:
                    if (iniCustomDate.HasValue)
                        iniDate = iniCustomDate.Value;
                    else
                        iniDate = xNow.Date;
                    if (endCustomDate.HasValue)
                        endDate = endCustomDate.Value;
                    break;

                default:
                    iniDate = xNow.Date;
                    endDate = xNow;
                    break;
            }

        }

        private static Expression<Func<OPERATION, bool>> FilterIniDate(Expression<Func<OPERATION, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            return predicate.And(o => o.OPE_INIDATE >= xIni && o.OPE_INIDATE <= xEnd);
        }

        /*private static Expression<Func<ALL_OPERATIONS_EXT, bool>> FilterDate(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            return predicate.And(o => o.OPE_DATE >= xIni && o.OPE_DATE <= xEnd);
        }*/

        private static Expression<Func<TICKET_PAYMENT, bool>> FilterDate(Expression<Func<TICKET_PAYMENT, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            return predicate.And(o => o.TIPA_DATE >= xIni && o.TIPA_DATE <= xEnd);
        }

        private static Expression<Func<CUSTOMER_INSCRIPTION, bool>> FilterLastSentDate(Expression<Func<CUSTOMER_INSCRIPTION, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            return predicate.And(o => o.CUSINS_LAST_SENT_DATE >= xIni && o.CUSINS_LAST_SENT_DATE <= xEnd);
        }

        private static Expression<Func<ALL_OPERATIONS_EXT, bool>> FilterDate(Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            ChargeOperationsType[] typesIniDate = { ChargeOperationsType.ParkingOperation, ChargeOperationsType.ExtensionOperation, ChargeOperationsType.ParkingRefund };

            return predicate.And(o => (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE) >= xIni &&
                                      (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE) <= xEnd &&
                                      (((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Hour * 60) + (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      (((typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Hour * 60) + (typesIniDate.Contains((ChargeOperationsType)o.OPE_TYPE) ? o.OPE_INIDATE : o.OPE_DATE).Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<VW_OPERATIONS_HOUR, bool>> FilterDate(Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd && 
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<DB_OPERATIONS_HOUR, bool>> FilterDate(Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<VW_OPERATIONS_MINUTE, bool>> FilterDate(Expression<Func<VW_OPERATIONS_MINUTE, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.MINUTE >= xIni && o.MINUTE <= xEnd &&
                                      ((o.MINUTE.Value.Hour * 60) + o.MINUTE.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.MINUTE.Value.Hour * 60) + o.MINUTE.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<DB_OPERATIONS_MINUTE, bool>> FilterDate(Expression<Func<DB_OPERATIONS_MINUTE, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.MINUTE >= xIni && o.MINUTE <= xEnd &&
                                      ((o.MINUTE.Hour * 60) + o.MINUTE.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.MINUTE.Hour * 60) + o.MINUTE.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> FilterDate(Expression<Func<VW_INSCRIPTIONS_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> FilterDate(Expression<Func<VW_INSCRIPTIONS_PLATFORM_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<USER, bool>> FilterDate(Expression<Func<USER, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            xIni = TimeZoneInfo.ConvertTimeToUtc(xIni);
            xEnd = TimeZoneInfo.ConvertTimeToUtc(xEnd);
            
            return predicate.And(o => o.USR_INSERT_UTC_DATE >= xIni && o.USR_INSERT_UTC_DATE <= xEnd &&
                                      ((o.USR_INSERT_UTC_DATE.Hour * 60) + o.USR_INSERT_UTC_DATE.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.USR_INSERT_UTC_DATE.Hour * 60) + o.USR_INSERT_UTC_DATE.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<USER, bool>> FilterDateUserOperativeDate(Expression<Func<USER, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            xIni = TimeZoneInfo.ConvertTimeToUtc(xIni);
            xEnd = TimeZoneInfo.ConvertTimeToUtc(xEnd);

            return predicate.And(o => o.USR_OPERATIVE_UTC_DATE.HasValue &&
                                      o.USR_OPERATIVE_UTC_DATE >= xIni && o.USR_OPERATIVE_UTC_DATE <= xEnd &&
                                      ((o.USR_OPERATIVE_UTC_DATE.Value.Hour * 60) + o.USR_OPERATIVE_UTC_DATE.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.USR_OPERATIVE_UTC_DATE.Value.Hour * 60) + o.USR_OPERATIVE_UTC_DATE.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<VW_RECHARGES_HOUR, bool>> FilterDate(Expression<Func<VW_RECHARGES_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<DB_RECHARGES_HOUR, bool>> FilterDate(Expression<Func<DB_RECHARGES_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> FilterDate(Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate)
        {
            DateTime xIni;
            DateTime xEnd;

            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);

            return predicate.And(t => t.CUSPMR_UTC_DATE.HasValue && t.CUSPMR_UTC_DATE.Value >= xIni && t.CUSPMR_UTC_DATE.Value <= xEnd);
        }

        private static Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> FilterDate(Expression<Func<VW_OPERATIONS_USER_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> FilterDate(Expression<Func<DB_OPERATIONS_USERS_HOUR, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Hour * 60) + o.HOUR.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> FilterDate(Expression<Func<Select_DB_INVITATIONS_HOURResult, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }
        private static Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> FilterDate(Expression<Func<Select_DB_RECHARGE_COUPONS_HOURResult, bool>> predicate, DateFilterType dateFilter, DateTime? iniCustomDate, DateTime? endCustomDate, string iniTime, string endTime)
        {
            DateTime xIni;
            DateTime xEnd;
            GetDateRange(dateFilter, iniCustomDate, endCustomDate, out xIni, out xEnd);
            return predicate.And(o => o.HOUR >= xIni && o.HOUR <= xEnd &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) >= (Conversions.Time2Seconds(iniTime) / 60) &&
                                      ((o.HOUR.Value.Hour * 60) + o.HOUR.Value.Minute) <= (Conversions.Time2Seconds(endTime) / 60));
        }

        private static IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> GroupOrder(IQueryable<VW_OPERATIONS_HOUR> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> grouped = null;
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_HOUR>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => g.Key);                        
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {                                                
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString() + "/" + (o.HOUR.Value.Day < 10 ? "0" : "") + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {                        
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }
        private static IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> GroupOrder(IQueryable<DB_OPERATIONS_HOUR> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> grouped = null;
            IOrderedQueryable<IGrouping<string, DB_OPERATIONS_HOUR>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Year.ToString() + "/" + (o.HOUR.Month < 10 ? "0" : "") + o.HOUR.Month.ToString() + "/" + (o.HOUR.Day < 10 ? "0" : "") + o.HOUR.Day.ToString() + "-" + ((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Year.ToString() + "-" + (((o.HOUR.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Year.ToString() + "-" + (o.HOUR.Month < 10 ? "0" : "") + o.HOUR.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Year.ToString() + "-" + (((o.HOUR.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Year.ToString() + "-" + (((o.HOUR.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.HOUR.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static IOrderedQueryable<IGrouping<string, VW_OPERATIONS_MINUTE>> GroupOrder(IQueryable<VW_OPERATIONS_MINUTE> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, VW_OPERATIONS_MINUTE>> grouped = null;
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_MINUTE>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        //grouped = operations.GroupBy(o => o.MINUTE.Value.ToString());
                        grouped = operations.GroupBy(o => new DateTime(o.MINUTE.Value.Year, o.MINUTE.Value.Month, o.MINUTE.Value.Day, o.MINUTE.Value.Hour, 0, 0).ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString() + "/" + (o.MINUTE.Value.Month < 10 ? "0" : "") + o.MINUTE.Value.Month.ToString() + "/" + (o.MINUTE.Value.Day < 10 ? "0" : "") + o.MINUTE.Value.Day.ToString() + "-" + ((o.MINUTE.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.MINUTE.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.MINUTE.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.MINUTE.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString() + "-" + (((o.MINUTE.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.MINUTE.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.MINUTE.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString() + "-" + (o.MINUTE.Value.Month < 10 ? "0" : "") + o.MINUTE.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString() + "-" + (((o.MINUTE.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.MINUTE.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString() + "-" + (((o.MINUTE.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.MINUTE.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.MINUTE.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }
        private static IOrderedQueryable<IGrouping<string, DB_OPERATIONS_MINUTE>> GroupOrder(IQueryable<DB_OPERATIONS_MINUTE> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, DB_OPERATIONS_MINUTE>> grouped = null;
            IOrderedQueryable<IGrouping<string, DB_OPERATIONS_MINUTE>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        //grouped = operations.GroupBy(o => o.MINUTE.Value.ToString());
                        grouped = operations.GroupBy(o => new DateTime(o.MINUTE.Year, o.MINUTE.Month, o.MINUTE.Day, o.MINUTE.Hour, 0, 0).ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Year.ToString() + "/" + (o.MINUTE.Month < 10 ? "0" : "") + o.MINUTE.Month.ToString() + "/" + (o.MINUTE.Day < 10 ? "0" : "") + o.MINUTE.Day.ToString() + "-" + ((o.MINUTE.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.MINUTE.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.MINUTE.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.MINUTE.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Year.ToString() + "-" + (((o.MINUTE.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.MINUTE.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.MINUTE.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Year.ToString() + "-" + (o.MINUTE.Month < 10 ? "0" : "") + o.MINUTE.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.MINUTE.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.MINUTE.Year.ToString() + "-" + (((o.MINUTE.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.MINUTE.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.MINUTE.Year.ToString() + "-" + (((o.MINUTE.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.MINUTE.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.MINUTE.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static IOrderedQueryable<IGrouping<string, VW_OPERATIONS_USER_HOUR>> GroupOrder(IQueryable<VW_OPERATIONS_USER_HOUR> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, VW_OPERATIONS_USER_HOUR>> grouped = null;
            IOrderedQueryable<IGrouping<string, VW_OPERATIONS_USER_HOUR>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {                                                
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString() + "/" + (o.HOUR.Value.Day < 10 ? "0" : "") + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {                        
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }
        private static IOrderedQueryable<IGrouping<string, OperationsUsers>> GroupOrder(IQueryable<OperationsUsers> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, OperationsUsers>> grouped = null;
            IOrderedQueryable<IGrouping<string, OperationsUsers>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString() + "/" + (string)(o.HOUR.Month < 10 ? "0" : "") + (string)o.HOUR.Month.ToString() + "/" + (string)(o.HOUR.Day < 10 ? "0" : "") + (string)o.HOUR.Day.ToString() + "-" + (string)((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString() + "-" + (string)(((o.HOUR.DayOfYear / 7) + 1) < 10 ? "0" : "") + (string)((o.HOUR.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => (string)((o.HOUR.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString() + "-" + (o.HOUR.Month < 10 ? "0" : "") + (string)o.HOUR.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => (string)o.HOUR.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString() + "-" + (string)(((o.HOUR.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (string)(((o.HOUR.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString() + "-" + (string)(((o.HOUR.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (string)(((o.HOUR.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => (string)o.HOUR.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static IOrderedQueryable<IGrouping<string, Select_DB_INVITATIONS_HOURResult>> GroupOrder(IQueryable<Select_DB_INVITATIONS_HOURResult> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, Select_DB_INVITATIONS_HOURResult>> grouped = null;
            IOrderedQueryable<IGrouping<string, Select_DB_INVITATIONS_HOURResult>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString() + "/" + (o.HOUR.Value.Day < 10 ? "0" : "") + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Date.ToString());
                        //ordered = grouped.OrderBy(g => g.Key);
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static IOrderedQueryable<IGrouping<string, Select_DB_RECHARGE_COUPONS_HOURResult>> GroupOrder(IQueryable<Select_DB_RECHARGE_COUPONS_HOURResult> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, Select_DB_RECHARGE_COUPONS_HOURResult>> grouped = null;
            IOrderedQueryable<IGrouping<string, Select_DB_RECHARGE_COUPONS_HOURResult>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.ToString());
                        //IQueryable<IGrouping<dynamic, VW_OPERATIONS_HOUR>> grouped2 = operations.GroupBy(o => new { a = o.HOUR.Value, o.OPE_INS_ID });
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString() + "/" + (o.HOUR.Value.Day < 10 ? "0" : "") + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Date.ToString());
                        //ordered = grouped.OrderBy(g => g.Key);
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    /*grouped = operations.GroupBy(o => (((1461 * (o.OPE_UTC_DATE.Year + 4800
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12)) / 4
                                                        + (367 * (o.OPE_UTC_DATE.Month - 2 - 12 *
                                                                ((o.OPE_UTC_DATE.Month - 14) / 12))) / 12
                                                        - (3 * ((o.OPE_UTC_DATE.Year + 4900
                                                        + (o.OPE_UTC_DATE.Month - 14) / 12) / 100)) / 4
                                                        + o.OPE_UTC_DATE.Day - 32075) / 7).ToString());*/
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static IOrderedQueryable<IGrouping<string, VW_INSCRIPTIONS_PLATFORM_HOUR>> GroupOrder(IQueryable<VW_INSCRIPTIONS_PLATFORM_HOUR> operations, DateGroupType dateGroup, bool dateGroupPattern)
        {
            IQueryable<IGrouping<string, VW_INSCRIPTIONS_PLATFORM_HOUR>> grouped = null;
            IOrderedQueryable<IGrouping<string, VW_INSCRIPTIONS_PLATFORM_HOUR>> ordered = null;

            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.ToString());                        
                        ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Hour.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "/" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString() + "/" + (o.HOUR.Value.Day < 10 ? "0" : "") + o.HOUR.Value.Day.ToString() + "-" + ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    }
                    else
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)o.HOUR.Value.DayOfWeek)).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Date.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                        //ordered = grouped.OrderBy(g => Convert.ToDateTime(g.Key));
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Day.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.week:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.DayOfYear / 7) + 1) < 10 ? "0" : "") + ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => ((o.HOUR.Value.DayOfYear / 7) + 1).ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (o.HOUR.Value.Month < 10 ? "0" : "") + o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => g.Key);
                    }
                    else
                    {
                        grouped = operations.GroupBy(o => o.HOUR.Value.Month.ToString());
                        ordered = grouped.OrderBy(g => Convert.ToInt32(g.Key));
                    }
                    break;
                case DateGroupType.quarter:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 3) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.half:
                    if (!dateGroupPattern)
                        grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString() + "-" + (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    else
                        grouped = operations.GroupBy(o => (((o.HOUR.Value.Month - 1) / 6) + 1).ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
                case DateGroupType.year:
                    //if (!dateGroupPattern)
                    grouped = operations.GroupBy(o => o.HOUR.Value.Year.ToString());
                    ordered = grouped.OrderBy(g => g.Key);
                    break;
            }

            return ordered;
        }

        private static string TruncateDate(DateTime oDate, DateGroupType groupType)
        {
            string sRet = "";

            switch (groupType)
            {
                case DateGroupType.day:
                    sRet = oDate.Date.ToString(); //.ToShortDateString();
                    break;
                case DateGroupType.week:
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    sRet = ciCurr.Calendar.GetWeekOfYear(oDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
                    break;
                case DateGroupType.month:
                    sRet = oDate.ToString("MMMM");
                    break;
                case DateGroupType.quarter:
                    if (oDate.Month <= 3)
                        sRet = "Primer cuatrimestre";
                    else if (oDate.Month <= 6)
                        sRet = "Segundo cuatrimestre";
                    else if (oDate.Month <= 9)
                        sRet = "Tercer cuatrimestre";
                    else
                        sRet = "Cuarto cuatrimestre";
                    break;
                case DateGroupType.half:
                    if (oDate.Month <= 6)
                        sRet = "Primera mitad del año";
                    else
                        sRet = "Segunda mitad";
                    break;
                case DateGroupType.year:
                    sRet = oDate.Year.ToString();
                    break;

            }
            return sRet;
        }

        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<ALL_OPERATIONS_EXT, bool>> predicate, out CURRENCy oCurrency)
        {            
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            var currencies = (from o in backOfficeRepository.GetOperationsExt(predicate)
                              select o.OPE_AMOUNT_CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in currencies)
            {
                //if (curIsoCode == "EUR" && oCurrency.CUR_ISO_CODE == "USD")
                //    curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : 1.35940));
                //else
                    curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }

        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<CUSTOMER_PAYMENT_MEANS_RECHARGE, bool>> predicate, out CURRENCy oCurrency)
        {
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            integraMobileDBEntitiesDataContext dbContext = new integraMobileDBEntitiesDataContext();
            var currencies = (from o in backOfficeRepository.GetCustomerRecharges(predicate, dbContext)
                              select o.CUSPMR_CUR_ID).Distinct();
            var curIsoCodes = (from o in backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => currencies.Contains(c.CUR_ID)), dbContext)
                               select o.CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in curIsoCodes)
            {
                curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }

        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<VW_OPERATIONS_HOUR, bool>> predicate, out CURRENCy oCurrency)
        {
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            var currencies = (from o in backOfficeRepository.GetVwOperationsHour(predicate)
                              select o.OPE_AMOUNT_CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in currencies)
            {
                curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }

        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<DB_OPERATIONS_HOUR, bool>> predicate, out CURRENCy oCurrency)
        {
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            var currencies = (from o in backOfficeRepository.GetDbOperationsHour(predicate)
                              select o.OPE_AMOUNT_CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in currencies)
            {
                curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }

        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<VW_RECHARGES_HOUR, bool>> predicate, out CURRENCy oCurrency)
        {
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            var currencies = (from o in backOfficeRepository.GetVwRechargesHour(predicate)
                              select o.OPE_AMOUNT_CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in currencies)
            {
                curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }
        private static Dictionary<string, double> GetCurrencyChanges(IBackOfficeRepository backOfficeRepository, decimal currencyId, Expression<Func<DB_RECHARGES_HOUR, bool>> predicate, out CURRENCy oCurrency)
        {
            Dictionary<string, double> curChanges = new Dictionary<string, double>();
            oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ID == currencyId)).FirstOrDefault();

            var currencies = (from o in backOfficeRepository.GetDbRechargesHour(predicate)
                              select o.OPE_AMOUNT_CUR_ISO_CODE).Distinct();
            foreach (string curIsoCode in currencies)
            {
                curChanges.Add(curIsoCode, (curIsoCode == oCurrency.CUR_ISO_CODE ? 1 : CCurrencyConvertor.GetChangeToApply(curIsoCode, oCurrency.CUR_ISO_CODE)));
            }
            foreach (string curIsoCode in new string[] { "EUR", "USD", "CAD", "GBP", "MXN" })
            {
                if (!curChanges.ContainsKey(curIsoCode))
                    curChanges.Add(curIsoCode, 1);
            }
            return curChanges;
        }

        private static int WeekNumber(DateTime xDate)
        {            
            int iMonthZeller = (xDate.Month ==1?11:(xDate.Month == 2?12:xDate.Month-2));
            int iD = Convert.ToInt32( xDate.Year.ToString().Substring(xDate.Year.ToString().Length-2));
            int iC = Convert.ToInt32(xDate.Year.ToString().Substring(0,2));
            return xDate.Day + ((13*iMonthZeller-1)/5) + iD + (iD/4) + (iC/4) - 2*iC;
        }

        private static int GetWeekNumber(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        } 
        #endregion

    }
}