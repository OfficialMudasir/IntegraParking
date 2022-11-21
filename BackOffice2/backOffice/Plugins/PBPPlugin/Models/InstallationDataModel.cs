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
    public class InstallationDataModel
    {
        [LocalizedDisplayNameBundle("InstallationDataModel_InstallationId", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Id")]
        public decimal InstallationId { get; set; }

        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        [DataType(DataType.Text)]
        //[StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")] //***
        [LocalizedDisplayNameBundle("InstallationDataModel_Description", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Description")]
        public string Description { get; set; }

        [LocalizedDisplayNameBundle("InstallationDataModel_ShortDesc", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "ShortDesc")]
        public string ShortDesc { get; set; }


        public static IQueryable<InstallationDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var predicate = PredicateBuilder.True<INSTALLATION>();
            return List(backOfficeRepository, predicate);
        }


        public static IQueryable<InstallationDataModel> List(IBackOfficeRepository backOfficeRepository, Expression<Func<INSTALLATION, bool>> predicate)
        {
            var groups = (from dom in backOfficeRepository.GetInstallations(predicate)
                          select new InstallationDataModel
                          {
                              InstallationId = dom.INS_ID,
                              Description = dom.INS_DESCRIPTION,
                              ShortDesc = dom.INS_SHORTDESC
                          })
                         .OrderBy(e => e.Description)
                         .AsQueryable();
            return groups;
        }

    }
}