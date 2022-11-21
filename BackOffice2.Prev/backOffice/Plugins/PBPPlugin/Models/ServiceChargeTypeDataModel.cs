using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class ServiceChargeTypeDataModel
    {
        private static readonly ResourceBundle resBundle = ResourceBundle.GetInstance();

        //[LocalizedDisplayName("ServiceChargeTypeDataModel_ServiceChargeId", NameResourceType = typeof(Resources))]
        public int ServiceChargeId { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        //[DataType(DataType.Text)]
        //[StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        //[LocalizedDisplayName("ServiceChargeTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<ServiceChargeTypeDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var types = (from d in Enum.GetValues(typeof(ServiceChargeType)).Cast<ServiceChargeType>()
                         select new ServiceChargeTypeDataModel
                         {
                             ServiceChargeId = (int) d,
                             Description = resBundle.GetString("Maintenance", "TypeEnums.ServiceChargeType." + (d).ToString(), (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return resBundle.GetString("Maintenance", "TypeEnums.ServiceChargeType." + Enum.GetName(typeof(ServiceChargeType), _type).ToString(), Enum.GetName(typeof(ServiceChargeType), _type).ToString());
        }
        public static string GetTypeIdString(int? _type)
        {
            if (_type.HasValue)
                return resBundle.GetString("Maintenance", "TypeEnums.ServiceChargeType." + Enum.GetName(typeof(ServiceChargeType), _type).ToString(), Enum.GetName(typeof(ServiceChargeType), _type).ToString());
            else
                return "";
        }

    }
}