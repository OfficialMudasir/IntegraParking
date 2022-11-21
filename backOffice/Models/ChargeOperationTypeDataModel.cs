using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class ChargeOperationTypeDataModel
    {
        [LocalizedDisplayName("ChargeOperationTypeDataModel_ChargeOperationTypeId", NameResourceType = typeof(Resources))]
        public int ChargeOperationTypeId { get; set; }

        [LocalizedDisplayName("ChargeOperationTypeDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<ChargeOperationTypeDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(ChargeOperationsType)).Cast<ChargeOperationsType>()
                         select new ChargeOperationTypeDataModel
                         {
                             ChargeOperationTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("ChargeOperationsType_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static IQueryable<ChargeOperationTypeDataModel> ListNoRecharge()
        {
            //Expression<Func<USER, bool>> predicate
            //var predicate = PredicateBuilder.True<ChargeOperationTypeDataModel>();
            //predicate = predicate.And(a => a.ChargeOperationTypeId != (int) ChargeOperationsType.BalanceRecharge);

            var types = (from d in Enum.GetValues(typeof(ChargeOperationsType)).Cast<ChargeOperationsType>()
                         select new ChargeOperationTypeDataModel
                         {
                             ChargeOperationTypeId = (int)d,
                             Description = Resources.ResourceManager.GetString("ChargeOperationsType_" + (d).ToString())
                         })
                         .Where(a => a.ChargeOperationTypeId != 5)
                         //.Where(predicate)
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("ChargeOperationsType_" + Enum.GetName(typeof(ChargeOperationsType), _type));
        }

    }
}