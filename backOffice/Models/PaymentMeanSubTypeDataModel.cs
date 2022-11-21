using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class PaymentMeanSubTypeDataModel
    {
        [LocalizedDisplayName("PaymentMeanSubTypeDataModel_PaymentMeanSubTypeId", NameResourceType = typeof(Resources))]
        public int PaymentMeanSubTypeId { get; set; }

        [LocalizedDisplayName("PaymentMeanSubTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<PaymentMeanSubTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentMeanSubType)).Cast<PaymentMeanSubType>()
                         select new PaymentMeanSubTypeDataModel
                         {
                             PaymentMeanSubTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("PaymentMeanSubType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try {
                return Resources.ResourceManager.GetString("PaymentMeanSubType_" + Enum.GetName(typeof(PaymentMeanSubType), _type));
            }
            catch
            {
                return "";
            }
        }
        public static string GetTypeIdString(int? _type)
        {
            if (_type.HasValue)
                return GetTypeIdString(_type.Value);
            else
                return "";
        }
    }
}