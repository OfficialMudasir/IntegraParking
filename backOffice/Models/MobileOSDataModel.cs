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
    public class MobileOSDataModel
    {
        [LocalizedDisplayName("MobileOSDataModel_Id", NameResourceType = typeof(Resources))]
        public int MobileOSId { get; set; }

        [LocalizedDisplayName("MobileOSDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<MobileOSDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(MobileOS)).Cast<MobileOS>()
                         select new MobileOSDataModel
                         {
                             MobileOSId = (int)d,
                             Description = Resources.ResourceManager.GetString("MobileOS_" + (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return Resources.ResourceManager.GetString("MobileOS_" + Enum.GetName(typeof(MobileOS), _type));
        }

    }
}