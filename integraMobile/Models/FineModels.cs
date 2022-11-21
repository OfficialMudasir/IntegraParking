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

namespace integraMobile.Models
{
    /* This is only working after post */
    [PropertiesMustMatch("Email", "ConfirmEmail", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_EmailsMustMatch")]
    [Serializable]
    public class FineModel
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(RetailerCouponsModel));

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("Fine_TicketNumber", NameResourceType = typeof(Resources))]
        public string TicketNumber { get; set; }
        
        [DataType(DataType.Text)]
        [LocalizedDisplayName("FineModel_Plate", NameResourceType = typeof(Resources))]
        public string Plate { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("FineModel_Email", NameResourceType = typeof(Resources))]
        public string Email { get; set; }
       
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidFormat")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLength")]
        [LocalizedDisplayName("FineModel_ConfirmEmail", NameResourceType = typeof(Resources))]
        public string ConfirmEmail { get; set; }

        [LocalizedDisplayName("FineModel_Installation", NameResourceType = typeof(Resources))]
        public int InstallationId { get; set; }
        public int Total { get; set; }
        public int Result { get; set; }
        public int NumDecimals { get; set; }
        public DateTime TicketDate { get; set; }
        public string FineDetails { get; set; }
        public string AmountCurrencyIsoCode { get; set; }
        public string InstallationShortDesc { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal CurrencyId { get; set; }
        public DateTime PriceCalculationDate { get; set; }
        public int GrpId { get; set; }
        public string AuthId { get; set; }

        public decimal? ForceInstallationId { get; set; }
        public bool BlockInstallationCombo { get; set; }
        public string ForceCulture { get; set; }
        public string ForceTicketNumber { get; set; }

        [DataType(DataType.Text)]
        public string InstallationList { get; set; }
        public string StandardInstallationList { get; set; }

        public int Quantity = 0;
        public int TotalQuantity = 0;
        public int FixedFEE = 0;
        public int PartialFixedFEE = 0;
        public int PartialPercFEE = 0;
        public int QVAT = 0;
        public int QFEE = 0;
        public int PartialVAT1 = 0;
        public int PercFEETopped = 0;
        public int PartialPercFEEVAT = 0;
        public int PartialFixedFEEVAT = 0;
        public decimal PercFEE = 0;
        public decimal PercVAT1 = 0;
        public decimal PercVAT2 = 0;
        public decimal VAT_Percent = 0;
        public IsTAXMode TaxMode = IsTAXMode.IsTax;

        // dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE,

        

        public void Init(IFineRepository fineRepository)
        {
            // Obtenir dades operador i percentatge IVA de l'operador
            Init(fineRepository,null);
        }

        public void Init(IFineRepository fineRepository, int? iId)
        {
            // Obtenir dades operador i percentatge IVA de l'operador
            int iDefaultOperatorId = Int32.Parse(ConfigurationManager.AppSettings["DefaultOperatorID"].ToString());
            integraMobile.Domain.OPERATOR oOperator = fineRepository.GetOperator(iDefaultOperatorId);
            if (iId.HasValue)
            {
                integraMobile.Domain.PAYMENT_TYPE oPaymentType = fineRepository.GetPaymentType(iId.Value);
            }
            else
            {
                integraMobile.Domain.PAYMENT_TYPE oPaymentType = fineRepository.GetPaymentType(1);
            }
        }

        public Dictionary<string, object> ToDictionary(System.Collections.Specialized.NameValueCollection reqParams)
        {
            List<string> lstModelKeys = new List<string>(new string[] { "TicketNumber", "Email", "ConfirmEmail", "Installation" });

            Dictionary<string, object> paramsDic = reqParams.AllKeys
                .Where(p => lstModelKeys.Contains(p))
                .ToDictionary(k => k, k => (object)reqParams[k]);

            return paramsDic;
        }

        public Dictionary<string, object> ToDictionary()
        {
            List<string> lstModelKeys = new List<string>(new string[] { "TicketNumber", "Email", "ConfirmEmail", "Installation" });

            Dictionary<string, object> paramsDic = new Dictionary<string, object>();

            foreach (string sPropertyName in lstModelKeys)
            {
                PropertyInfo propInfo = this.GetType().GetProperty(sPropertyName);
                object objValue = propInfo.GetValue(this, null);
                paramsDic.Add(sPropertyName, objValue);
            }

            return paramsDic;
        }
    }
}