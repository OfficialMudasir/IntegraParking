using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using System.Reflection;
using System.Drawing;
using System.Xml;
using integraMobile.Properties;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Infrastructure.QrCodeNet.Rendering;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain;

namespace integraMobile.Models
{
    [Serializable]
    public class ParkingModel
    {
        public int PlateIndex { get; set; }
        public int Group { get; set; }
        public bool PreSelectionDone { get; set; }
        public string GroupText { get; set; }
        public decimal Installation { get; set; }
        public string InstallationText { get; set; }
        public string Plate { get; set; }
        public decimal Rate { get; set; }
        public string RateText { get; set; }
        public ParkingStep Step { get; set; }
        public List<ParkingTariff> Tariffs = new List<ParkingTariff>();
        public List<ParkingButton> Buttons = new List<ParkingButton>();
        public List<ParkingStep> Steps = new List<ParkingStep>();
        public string Culture { get; set; }
        public ParkingLanguage LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public string SessionID { get; set; }
        public string Guid { get; set; }
        public string User { get; set; }
        public string InitialDate { get; set; }
        public string InitialDate_Clean { get; set; }
        public string LabelParking { get; set; }
        public string LabelFee { get; set; }
        public string LabelFeeVat { get; set; }
        public string LabelTotal { get; set; }
        public string Error { get; set; }
        public PaymentReturnData PaymentReturn { get; set; }
        public PaymentMeanCreditCardProviderType PaymentProvider { get; set; }
        public ParkingPaymentParams PaymentParams { get; set; }
        public string HashSeed { get; set; }
        public bool CreditCardEnabled { get; set; }
        public bool PayPalEnabled { get; set; }
        public string Email { get; set; }
        public string MaskedCardNumber { get; set; }
        public int Paymeth { get; set; }
        public string Theme { get; set; }
        public bool FreeTimeTariff { get; set; }
        public long OperationId { get; set; }
        public int Layout { get; set; }

        public Dictionary<string, object> ToDictionary(System.Collections.Specialized.NameValueCollection reqParams)
        {
            List<string> lstModelKeys = new List<string>(new string[] { "TicketNumber", "Email", "ConfirmEmail", "Installation" });

            Dictionary<string, object> paramsDic = reqParams.AllKeys
                .Where(p => lstModelKeys.Contains(p))
                .ToDictionary(k => k, k => (object)reqParams[k]);

            return paramsDic;
        }
    }


    [Serializable()]
    public class ParkingPaymentParams
    {
        public string Email { get; set; }
        public int Amount { get; set; }
        public string CurrencyISOCODE { get; set; }
        public string Description { get; set; }
        public string UTCDate { get; set; }
        public string ExternalId { get; set; }
        public string Culture { get; set; }
        public ParkingPaymentProviderData CreditCard { get; set; }
        public ParkingPaymentProviderData PayPal { get; set; }
        public string Hash { get; set; }
        public string ReturnURL { get; set; }
        public string CancelURL { get; set; }
    }

    [Serializable()]
    public class ParkingPaymentProviderData
    {
        public string RequestURL { get; set; }
        public string Guid { get; set; }
        public string HashSeed { get; set; }
    }

    [Serializable]
    public class ParkingTariff
    {
        public decimal Id { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class ParkingButton
    {
        public ParkingButtonType Type { get; set; }
        public int Minutes { get; set; }
        public string Text { get; set; }
        public string Function { get; set; }
    }

    [Serializable]
    public class ParkingStep
    {
        public int Time { get; set; }
        public string TimeFormatted { get; set; }
        public int TimeBalanceUsed { get; set; }
        public string Quantity { get; set; }
        public string QuantityVat { get; set; }
        public string QuantityPlusVat { get; set; }
        public string QuantityReal { get; set; }
        public string QuantityWithoutBon { get; set; }
        public string QuantityFee { get; set; }
        public string QuantityFeeVat { get; set; }
        public string QuantityTotal { get; set; }
        public decimal Quantity_Clean { get; set; }
        public decimal QuantityVat_Clean { get; set; }
        public decimal QuantityPlusVat_Clean { get; set; }
        public decimal QuantityReal_Clean { get; set; }
        public decimal QuantityWithoutBon_Clean { get; set; }
        public decimal QuantityFee_Clean { get; set; }
        public decimal QuantityFeeVat_Clean { get; set; }
        public decimal QuantityTotal_Clean { get; set; }
        public string InitialDate { get; set; }
        public string EndDate { get; set; }
        public string EndDate_Clean { get; set; }
        public string MinimumTime { get; set; }
        public string Days { get; set; }
        public string DaysClass { get; set; }
    }

    public enum ParkingButtonType
    {
        Increment = 1,
        RateStep = 2,
        RateMaximum = 3
    }

    public enum ParkingLanguage
    {
        esES = 1,
        enUS = 2,
        frFR = 3,
        caES = 4,
        esMX = 5,
        euES = 6,
        itIT = 7
    }

}