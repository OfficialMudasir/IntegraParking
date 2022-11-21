using System;
using fastJSON;

namespace integraMobile.WS.WebAPI.Actions.Meypar
{
    public enum NotificationTypeEnum
    {
        TicketMovement = 0,
        SubscriberMomevent = 1
    }

    public enum VehicleTypeEnum
    {
        Unknow = 0,
        Car = 1,
        Motorbike = 44
    }

    public class StandardNotificationBody : BaseRequest<SupportValidationBody>
    {
        #region Properties

        [JsonField("ParkingId")]
        public string ParkingId { get; set; }

        [JsonField("SystemCode")]
        public int SystemCode { get; set; }

        [JsonField("TicketId")]
        public string TicketId { get; set; }

        [JsonField("ExternalId")]
        public string ExternalId { get; set; }

        [JsonField("TerminalId")]
        public int TerminalId { get; set; }

        [JsonInclude(false)]
        public DateTime? TicketCreation_LocalDate { get; set; }
        [JsonField("TicketCreationLocalDate")]
        public string TicketCreation_LocalDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.TicketCreation_LocalDate);
            }
            set
            {
                this.TicketCreation_LocalDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonInclude(false)]
        public DateTime? TicketCreation_UTCDate { get; set; }
        [JsonField("TicketCreationUTCDate")]
        public string TicketCreation_UTCDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.TicketCreation_UTCDate);
            }
            set
            {
                this.TicketCreation_UTCDate = GetDateTimeFromStringFormat(value);
            }
        }


        [JsonField("TicketNumber")]
        public int TicketNumber { get; set; }

        [JsonField("SourceZone")]
        public int SourceZone { get; set; }
        [JsonField("DestinationZone")]
        public int DestinationZone { get; set; }

        [JsonField("OperationTerminal")]
        public int OperationTerminal { get; set; }
        [JsonField("OperationTerminalName")]
        public string OperationTerminalName { get; set; }

        [JsonInclude(false)]
        public DateTime? Operation_LocalDate { get; set; }
        [JsonField("OperationLocalDate")]
        public string Operation_LocalDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.Operation_LocalDate);
            }
            set
            {
                this.Operation_LocalDate = GetDateTimeFromStringFormat(value);
            }
        }
        [JsonInclude(false)]
        public DateTime? Operation_UTCDate { get; set; }
        [JsonField("OperationUTCDate")]
        public string Operation_UTCDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.Operation_UTCDate);
            }
            set
            {
                this.Operation_UTCDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonField("LicensePlate")]
        public string LicensePlate { get; set; }

        [JsonInclude(false)]
        public VehicleTypeEnum Vehicle_Type { get; set; }
        [JsonField("VehicleType")]
        public int Vehicle_Type_Int
        {
            get { return (int)this.Vehicle_Type; }
            set { this.Vehicle_Type = (VehicleTypeEnum)value; }
        }

        [JsonInclude(false)]
        public NotificationTypeEnum Notification_Type { get; set; }
        [JsonField("NotificationType")]
        public int Notification_Type_Int
        {
            get { return (int)this.Notification_Type; }
            set { this.Notification_Type = (NotificationTypeEnum)value; }
        }

        [JsonInclude(false)]
        public TerminalDeviceTypeEnum Device_Type { get; set; }
        [JsonField("DeviceType")]
        public int Device_Type_Int
        {
            get { return (int)this.Device_Type; }
            set { this.Device_Type = (TerminalDeviceTypeEnum)value; }
        }

        [JsonField("Version")]
        public string Version { get; set; }

        [JsonInclude(false)]
        public DateTime? Event_LocalDate { get; set; }
        [JsonField("EventLocalDate")]
        public string Event_LocalDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.Event_LocalDate);
            }
            set
            {
                this.Event_LocalDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonInclude(false)]
        public DateTime? Event_UTCDate { get; set; }
        [JsonField("EventUTCDate")]
        public string Event_UTCDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.Event_UTCDate);
            }
            set
            {
                this.Event_UTCDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonField("TotalAmount")]
        public double TotalAmount { get; set; }

        [JsonField("TaxPercent")]
        public double TaxPercent { get; set; }

        [JsonField("Currency")]
        public int? Currency { get; set; }

        [JsonInclude(false)]
        public DateTime? StayStart_LocalDate { get; set; }
        [JsonField("StayStartLocalDate")]
        public string StayStart_LocalDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.StayStart_LocalDate);
            }
            set
            {
                this.StayStart_LocalDate = GetDateTimeFromStringFormat(value);
            }
        }
        [JsonInclude(false)]
        public DateTime? StayStart_UTCDate { get; set; }
        [JsonField("StayStartUTCDate")]
        public string StayStart_UTCDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.StayStart_UTCDate);
            }
            set
            {
                this.StayStart_UTCDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonInclude(false)]
        public DateTime? StayEnd_LocalDate { get; set; }
        [JsonField("StayEndLocalDate")]
        public string StayEnd_LocalDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.StayEnd_LocalDate);
            }
            set
            {
                this.StayEnd_LocalDate = GetDateTimeFromStringFormat(value);
            }
        }
        [JsonInclude(false)]
        public DateTime? StayEnd_UTCDate { get; set; }
        [JsonField("StayEndUTCDate")]
        public string StayEnd_UTCDate_String
        {
            get
            {
                return GetDateTimeStringFromDateFormat(this.StayEnd_UTCDate);
            }
            set
            {
                this.StayEnd_UTCDate = GetDateTimeFromStringFormat(value);
            }
        }

        [JsonField("Historic")]
        public bool Historic { get; set; }

        #endregion
    }
}