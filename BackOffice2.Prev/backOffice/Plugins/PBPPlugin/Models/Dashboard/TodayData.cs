using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backOffice.Models.Dashboard
{
    public class TodayData
    {
        public string DataType { get; set; }

        public long CurDay { get; set; }
        public long PrevDay { get; set; }
        public long PrevWeek { get; set; }

        public long ParkingsCurDay { get; set; }
        public long ParkingsPrevDay { get; set; }
        public long ParkingsPrevWeek { get; set; }

        public long ExtensionsCurDay { get; set; }
        public long ExtensionsPrevDay { get; set; }
        public long ExtensionsPrevWeek { get; set; }

        public long RefundsCurDay { get; set; }
        public long RefundsPrevDay { get; set; }
        public long RefundsPrevWeek { get; set; }

        public long TicketsCurDay { get; set; }
        public long TicketsPrevDay { get; set; }
        public long TicketsPrevWeek { get; set; }

        public long RechargesCurDay { get; set; }
        public long RechargesPrevDay { get; set; }
        public long RechargesPrevWeek { get; set; }

        public string CurDayDate { get; set; }
        public string PrevDayDate { get; set; }
        public string PrevWeekDate { get; set; }

        public string CurDayDateLong { get; set; }
        public string PrevDayDateLong { get; set; }
        public string PrevWeekDateLong { get; set; }

    }
}