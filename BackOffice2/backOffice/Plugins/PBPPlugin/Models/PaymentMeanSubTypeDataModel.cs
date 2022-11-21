using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class PaymentMeanSubTypeDataModel
    {
        [LocalizedDisplayNameBundle("PaymentMeanSubTypeDataModel_PaymentMeanSubTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public int PaymentMeanSubTypeId { get; set; }

        [LocalizedDisplayNameBundle("PaymentMeanSubTypeDataModel_Description", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Description")]
        public string Description { get; set; }

        private static ResourceBundle resBundle = ResourceBundle.GetInstance();

        public static IQueryable<PaymentMeanSubTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentMeanSubType)).Cast<PaymentMeanSubType>()
                         select new PaymentMeanSubTypeDataModel
                         {
                             PaymentMeanSubTypeId = (int)d,
                             Description = resBundle.GetString("PBPPlugin", "PaymentMeanSubType_" + (d).ToString(), d.ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try {
                return resBundle.GetString("PBPPlugin", "PaymentMeanSubType_" + Enum.GetName(typeof(PaymentMeanSubType), _type), Enum.GetName(typeof(PaymentMeanSubType), _type));
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