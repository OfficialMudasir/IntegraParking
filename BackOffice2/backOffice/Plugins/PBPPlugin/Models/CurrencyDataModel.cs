using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class CurrencyDataModel
    {
        [LocalizedDisplayNameBundle("CurrencyDataModel_CurrencyID", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public decimal? CurrencyID { get; set; }
        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***        
        [LocalizedDisplayNameBundle("CurrencyDataModel_Name", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Name")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        //[StringLength(10, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***        
        [LocalizedDisplayNameBundle("CurrencyDataModel_IsoCode", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "IsoCode")]
        public string IsoCode { get; set; }

        public static IQueryable<CurrencyDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<CURRENCy, bool>> predicate = null)
        {
            var currencies = (from dom in backOfficeRepository.GetCurrencies(predicate)
                             select new CurrencyDataModel
                             {
                                 CurrencyID = dom.CUR_ID,
                                 Name = dom.CUR_NAME
                             })
                             .OrderBy(e => e.Name)
                             .AsQueryable();
            return currencies;
        }

        public static CurrencyDataModel Get(IBackOfficeRepository backOfficeRepository, decimal currencyId)
        {
            var predicate = PredicateBuilder.True<CURRENCy>();
            predicate = predicate.And(a => a.CUR_ID == currencyId);
            var currencies = List(backOfficeRepository, predicate);
            if (currencies.Count() > 0)
                return currencies.First();
            else
                return new CurrencyDataModel();
        }
        public static CurrencyDataModel Get(IBackOfficeRepository backOfficeRepository, decimal? currencyId)
        {
            decimal id = -1;
            if (currencyId.HasValue) id = currencyId.Value;
            return Get(backOfficeRepository, id);            
        }

        public static CurrencyDataModel Get(IQueryable<CurrencyDataModel> currencies, decimal currencyId)
        {
            var cur = currencies.Where(c => c.CurrencyID == currencyId);
            if (cur.Count() > 0)
                return cur.First();
            else
                return new CurrencyDataModel();
        }

        public static CurrencyDataModel Get(IQueryable<CurrencyDataModel> currencies, decimal? currencyId)
        {
            decimal id = -1;
            if (currencyId.HasValue) id = currencyId.Value;
            return Get(currencies, id);            
        }
            
        
    }
}