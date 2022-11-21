using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class MobileOSDataModel
    {
        private static readonly ResourceBundle resBundle = ResourceBundle.GetInstance();

        //[LocalizedDisplayName("MobileOSDataModel_Id", NameResourceType = typeof(Resources))]
        public int MobileOSId { get; set; }

        //[LocalizedDisplayName("MobileOSDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public static IQueryable<MobileOSDataModel> List()
        {
            var types = (from d in Enum.GetValues(typeof(MobileOS)).Cast<MobileOS>()
                         select new MobileOSDataModel
                         {
                             MobileOSId = (int)d,
                             Description = resBundle.GetString("Maintenance", "TypeEnums.MobileOSs." + (d).ToString(), (d).ToString())
                         })
                        .OrderBy(e => e.Description)
                        .AsQueryable();
            return types;
        }

        public static string GetTypeIdString(int _type)
        {
            return resBundle.GetString("Maintenance", "TypeEnums.MobileOSs." + Enum.GetName(typeof(MobileOS), _type).ToString(), Enum.GetName(typeof(MobileOS), _type).ToString());
        }

    }
}