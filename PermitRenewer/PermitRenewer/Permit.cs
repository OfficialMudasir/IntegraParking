using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermitRenewer
{
    class Permit
    {
        public decimal OperationId { get; set; }       
        public List<PlateCollection> Plates { get; set; }

        public Permit()
        {
            Plates = new List<PlateCollection>();
        }
    }

    class PermitCollection
    {
        /* Common fields >> */
        public decimal? RelationId { get; set; }
        public string UserName { get; set; }
        public decimal? InstallationId { get; set; }
        public decimal UserCurrencyId { get; set; }
        public decimal? GroupId { get; set; }
        public string UTC_Offset { get; set; }
        public decimal? TariffId { get; set; }
        public string InstallationTimeZone { get; set; }
        /* << Common fields */
        public List<Permit> Permits { get; set; }

        public PermitCollection()
        {
            Permits = new List<Permit>();
        }
    }

    class PlateCollection
    {
        public string Plate1 { get; set; }
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }
        public string Plate4 { get; set; }
        public string Plate5 { get; set; }
        public string Plate6 { get; set; }
        public string Plate7 { get; set; }
        public string Plate8 { get; set; }
        public string Plate9 { get; set; }
        public string Plate10 { get; set; }

        public PlateCollection(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10)
        {
            Plate1 = p1;
            Plate2 = p2;
            Plate3 = p3;
            Plate4 = p4;
            Plate5 = p5;
            Plate6 = p6;
            Plate7 = p7;
            Plate8 = p8;
            Plate9 = p9;
            Plate10 = p10;
        }
    }
}
