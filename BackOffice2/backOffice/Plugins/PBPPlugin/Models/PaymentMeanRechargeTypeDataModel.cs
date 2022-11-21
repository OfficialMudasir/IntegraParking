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
    public class PaymentMeanRechargeTypeDataModel
    {
        [LocalizedDisplayNameBundle("PaymentMeanRechargeTypeDataModel_PaymentMeanRechargeTypeId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public int PaymentMeanRechargeTypeId { get; set; }

        [LocalizedDisplayNameBundle("PaymentMeanRechargeTypeDataModel_Description", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Description")]
        public string Description { get; set; }

        private static ResourceBundle resBundle = ResourceBundle.GetInstance();

        public static IQueryable<PaymentMeanRechargeTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(PaymentMeanRechargeType)).Cast<PaymentMeanRechargeType>()
                         select new PaymentMeanRechargeTypeDataModel
                         {
                             PaymentMeanRechargeTypeId = (int)d,
                             Description = resBundle.GetString("Maintenance", "TypeEnums.PaymentMeanRechargeTypes." + (d).ToString(), d.ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            try {
                return resBundle.GetString("Maintenance", "TypeEnums.PaymentMeanRechargeTypes." + Enum.GetName(typeof(PaymentMeanRechargeType), _type), Enum.GetName(typeof(PaymentMeanRechargeType), _type));
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