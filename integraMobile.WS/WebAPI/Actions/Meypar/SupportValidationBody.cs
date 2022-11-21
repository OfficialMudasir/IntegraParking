using System;
using fastJSON;

namespace integraMobile.WS.WebAPI.Actions.Meypar
{
    public enum TerminalDeviceTypeEnum
    {
        ESP_Entry = 1,
        ESP_Exit = 2
    }

    public enum ProductTitleTypeEnum
    {
        Ticket = 0,
        Subscriber = 1
    }

    public enum SupportTypeEnum
    {
        Plate = 0,
        Qr = 1,
        Mifare = 2
    }

    public class SupportValidationBody : BaseRequest<SupportValidationBody>
    {
        #region Properties

        [JsonField("ParkingId")]
        public string ParkingId { get; set; }

        [JsonField("SystemCode")]
        public int SystemCode { get; set; }

        [JsonField("SupportId")]
        public string SupportId { get; set; }

        private SupportTypeEnum m_eSupportType;

        [JsonInclude(false)]
        public SupportTypeEnum Support_Type { get; set; }
        [JsonField("SupportType")]
        public int Support_Type_Int
        {
            get { return (int)this.Support_Type; }
            set { this.Support_Type = (SupportTypeEnum)value; }
        }

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


        [JsonField("OperationTerminal")]
        public int OperationTerminal { get; set; }

        [JsonField("OperationTerminalName")]
        public string OperationTerminalName { get; set; }

        [JsonField("LicensePlate")]
        public string LicensePlate { get; set; }

        [JsonInclude(false)]
        public ProductTitleTypeEnum Product_TitleType { get; set; }
        [JsonField("ProductTitleType")]
        public int Product_TitleType_Int
        {
            get { return (int)this.Product_TitleType; }
            set { this.Product_TitleType = (ProductTitleTypeEnum)value; }
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


        [JsonField("Historic")]
        public bool Historic { get; set; }

        #endregion
    }
}