using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class InvoiceDataModel
    {
        [LocalizedDisplayName("InvoiceDataModel_UserId", NameResourceType = typeof(Resources))]
        public decimal? UserId { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_UserId", NameResourceType = typeof(Resources))]
        public string Username { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_InvoiceId", NameResourceType = typeof(Resources))]
        public decimal InvoiceId { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_InvoiceNumber", NameResourceType = typeof(Resources))]
        public int InvoiceNumber { get; set; }
        public string InvoiceNumberFormatted { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_Date", NameResourceType = typeof(Resources))]
        public DateTime? Date { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_Amount", NameResourceType = typeof(Resources))]
        public double Amount { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_CurrencyId", NameResourceType = typeof(Resources))]
        public decimal CurrencyId { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_CurrencyIsoCode", NameResourceType = typeof(Resources))]
        public string CurrencyIsoCode { get; set; }
        [LocalizedDisplayName("InvoiceDataModel_DownloadURL", NameResourceType = typeof(Resources))]
        public string DownloadURL { get; set; }

        public string UserId_FK { get; set;}
        public string CurrencyId_FK { get; set; }

        public static IQueryable<InvoiceDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();
            return List(backOfficeRepository, predicate, true);
        }

        public static IQueryable<InvoiceDataModel> List(IBackOfficeRepository backOfficeRepository, bool loadFKs)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INVOICE>();
            return List(backOfficeRepository, predicate, loadFKs);
        }

        public static IQueryable<InvoiceDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<CUSTOMER_INVOICE, bool>> predicate, bool loadFKs)
        {
            IQueryable<InvoiceDataModel> modelInvoices;            

            if (loadFKs)
            {
                modelInvoices = from domInv in backOfficeRepository.GetInvoices(predicate)
                                    select new InvoiceDataModel
                                    {
                                        UserId = domInv.CUSTOMER.USER.USR_ID,
                                        Username = domInv.CUSTOMER.USER.USR_USERNAME,
                                        InvoiceId = domInv.CUSINV_ID,
                                        InvoiceNumber = Convert.ToInt32(domInv.CUSINV_INV_NUMBER),
                                        InvoiceNumberFormatted = string.Format(domInv.OPERATOR.OPR_INVOICE_NUMBER_FORMAT, Convert.ToInt32(domInv.CUSINV_INV_NUMBER), domInv.CUSINV_INV_DATE),
                                        Date = domInv.CUSINV_INV_DATE,
                                        Amount = Convert.ToDouble(domInv.CUSINV_INV_AMOUNT / 100.0),
                                        CurrencyId = domInv.CUSINV_CUR_ID,
                                        CurrencyIsoCode = domInv.CURRENCy.CUR_ISO_CODE,
                                        UserId_FK = domInv.CUSTOMER.USER.USR_USERNAME,
                                        CurrencyId_FK = domInv.CURRENCy.CUR_NAME
                                    };
            }
            else
            {
                modelInvoices = from domInv in backOfficeRepository.GetInvoices(predicate)
                                select new InvoiceDataModel
                                {
                                    UserId = domInv.CUSTOMER.USER.USR_ID,
                                    Username = domInv.CUSTOMER.USER.USR_USERNAME,
                                    InvoiceId = domInv.CUSINV_ID,
                                    InvoiceNumber = Convert.ToInt32(domInv.CUSINV_INV_NUMBER),
                                    InvoiceNumberFormatted = string.Format(domInv.OPERATOR.OPR_INVOICE_NUMBER_FORMAT, Convert.ToInt32(domInv.CUSINV_INV_NUMBER), domInv.CUSINV_INV_DATE),
                                    Date = domInv.CUSINV_INV_DATE,
                                    Amount = Convert.ToDouble(domInv.CUSINV_INV_AMOUNT / 100.0),
                                    CurrencyId = domInv.CUSINV_CUR_ID,
                                    CurrencyIsoCode = domInv.CURRENCy.CUR_ISO_CODE
                                };
            }

            return modelInvoices.AsQueryable();
        }
    }
}