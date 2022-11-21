using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models
{
    public class InstallationDataModel
    {
        [LocalizedDisplayName("InstallationDataModel_InstallationId", NameResourceType = typeof(Resources))]
        public decimal InstallationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("InstallationDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        [LocalizedDisplayName("InstallationDataModel_ShortDesc", NameResourceType = typeof(Resources))]
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