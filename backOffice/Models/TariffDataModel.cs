using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace backOffice.Models
{
    public class TariffDataModel
    {
        [LocalizedDisplayName("TariffDataModel_TariffId", NameResourceType = typeof(Resources))]
        public decimal? TariffId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_InvalidLengthWithMinimum")]
        [LocalizedDisplayName("TariffDataModel_Description", NameResourceType = typeof(Resources))]
        public string Description { get; set; }

        public string InstallationShortDesc { get; set; }

        public string DescriptionWithInst
        {
            get { return InstallationShortDesc + " - " + Description; }
        }

        public static IQueryable<TariffDataModel> List(IBackOfficeRepository backOfficeRepository)
        {
            var tariffs = (from dom in backOfficeRepository.GetTariffs()
                          select new TariffDataModel
                          {
                              TariffId = dom.TAR_ID,
                              Description = dom.TAR_DESCRIPTION,
                              InstallationShortDesc = dom.INSTALLATION.INS_SHORTDESC
                          })
                          .OrderBy(e => e.Description)
                          .AsQueryable();
            return tariffs;
        }

        public static TariffDataModel Get(decimal tariffId, IBackOfficeRepository backOfficeRepository)
        {
            var groups = List(backOfficeRepository).Where(a => a.TariffId== tariffId);
            if (groups.Count() > 0)
                return groups.First();
            else
                return new TariffDataModel();
        }

        public static TariffDataModel Get(decimal? tariffId, IBackOfficeRepository backOfficeRepository)
        {
            decimal id = -1;
            if (tariffId.HasValue) id = tariffId.Value;
            return Get(id, backOfficeRepository);
        }
    }
}