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
using integraMobile.Domain.Concrete;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class CountryDataModel
    {

        public CountryDataModel()
        {
        }
        public CountryDataModel(COUNTRy domCountry)
        {
            CountryID = domCountry.COU_ID;
            Description = domCountry.COU_DESCRIPTION;
            Code = domCountry.COU_CODE;
            TelPrefix = domCountry.COU_TEL_PREFIX;
            Currency = new CurrencyDataModel()
            {
                CurrencyID = domCountry.CURRENCy.CUR_ID,
                Name = domCountry.CURRENCy.CUR_NAME
            };
        }

        public decimal? CountryID { get; set; }
        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***        
        [DisplayName("Descrption")]
        [UIHint("TextEditorTemplate")]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        ///[StringLength(10, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***        
        [DisplayName("Code")]
        [UIHint("TextEditorTemplate")]
        public string Code { get; set; }

        [DataType(DataType.PhoneNumber)]
        //[StringLength(10, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***        
        [DisplayName("Telephon prefix")]
        [UIHint("TextEditorTemplate")]
        public string TelPrefix { get; set; }

        public decimal? CurrencyID { get; set; }

        [UIHint("Currency")]
        public CurrencyDataModel Currency { get; set; }

        public static IQueryable<CountryDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<COUNTRy, bool>> predicate = null)
        {
            if (predicate == null) predicate = PredicateBuilder.True<COUNTRy>();
            var countries = (from dom in backOfficeRepository.GetCountries(predicate)
                              select new CountryDataModel
                              {
                                  CountryID = dom.COU_ID,
                                  Description = dom.COU_DESCRIPTION
                              })
                             .OrderBy(e => e.Description)
                             .AsQueryable();
            return countries;
        }

        public static CountryDataModel Get(IBackOfficeRepository backOfficeRepository, decimal countryId)
        {
            var predicate = PredicateBuilder.True<COUNTRy>();
            predicate = predicate.And(a => a.COU_ID == countryId);
            var countries = List(backOfficeRepository, predicate);
            if (countries.Count() > 0)
                return countries.First();
            else
                return new CountryDataModel();
        }
        public static CountryDataModel Get(IBackOfficeRepository backOfficeRepository, decimal? countryId)
        {
            if (countryId.HasValue)
                return Get(backOfficeRepository, countryId.Value);
            else
                return new CountryDataModel();
        }

        public bool SetDomain(SQLBackOfficeRepository dataRepository, bool remove = false)
        {
            bool bRes = true;

            try
            {
                COUNTRy oCountry = new COUNTRy();
                if (this.CountryID.HasValue) oCountry.COU_ID = this.CountryID.Value;
                oCountry.COU_DESCRIPTION = this.Description;
                oCountry.COU_CODE = this.Code;
                oCountry.COU_TEL_PREFIX = this.TelPrefix;
                if (this.Currency != null)
                    oCountry.COU_CUR_ID = this.Currency.CurrencyID;
                else
                    oCountry.COU_CUR_ID = null;

                if (!remove)
                {
                    bRes = dataRepository.UpdateCountry(ref oCountry);
                    if (bRes) this.CountryID = oCountry.COU_ID;
                }
                else
                    bRes = dataRepository.DeleteCountry(ref oCountry);

            }
            catch (Exception e)
            {
                bRes = false;

            }

            return bRes;
        }

    }
}