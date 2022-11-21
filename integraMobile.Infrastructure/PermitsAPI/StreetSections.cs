using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure.PermitsAPI
{
    public class MapPoint
    {

        private const decimal MAX_DIFF = 0.000000000001M;
        public decimal x { get; set; }
        public decimal y { get; set; }



        public bool IsEqual(MapPoint oCmpPoint)
        {
            bool bRes = false;

            bRes = Math.Abs(x - oCmpPoint.x) < MAX_DIFF;

            if (bRes)
                bRes = Math.Abs(y - oCmpPoint.y) < MAX_DIFF;


            return bRes;
        }

    };

    public class InstallationData
    {
        public string id { get; set; }
        public string description { get; set; }
        public string remarks { get; set; }
        public string externalId { get; set; }
    }

    public class StreetData
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }

        public bool isEqual(StreetData oCmpStreetData)
        {
            return (this.Id == oCmpStreetData.Id &&
                    this.Description == oCmpStreetData.Description &&
                    this.Deleted == oCmpStreetData.Deleted);
        }
    }

    public class GroupData
    {
        public string id { get; set; }
        public string description { get; set; }
        public string externalId { get; set; }
    }

    public class StreetSectionData
    {


        public string Id { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string StreetFrom { get; set; }
        public string StreetTo { get; set; }
        public int? StreetNumberFrom { get; set; }
        public int? StreetNumberTo { get; set; }
        public int? Side { get; set; }
        public string Zone { get; set; }
        public string SectorType { get; set; }
        public List<string> Tariffs { get; set; }
        public bool Deleted { get; set; }
        public int ReservedSpace { get; set; }
        public string GeometryType { get; set; }
        public List<MapPoint> GeometryCoordinates { get; set; }
        public string Colour { get; set; }
        public Dictionary<int, GridElement> oGridElements { get; set; }
 

        public bool isEqual(StreetSectionData oCmpSSD)
        {
            bool bRes = false;

            bRes = ((GeometryCoordinates?.Count() ?? 0) == (oCmpSSD.GeometryCoordinates?.Count() ?? 0) &&
                    (oGridElements?.Count ?? 0) == (oCmpSSD.oGridElements?.Count ?? 0) &&
                    (Tariffs?.Count() ?? 0) == (oCmpSSD.Tariffs?.Count() ?? 0));

            if (bRes)
            {
                bRes = ((Id == oCmpSSD.Id) &&
                    (Description == oCmpSSD.Description) &&
                    (Street == oCmpSSD.Street) &&
                    (StreetFrom == oCmpSSD.StreetFrom) &&
                    (StreetTo == oCmpSSD.StreetTo) &&
                    (StreetNumberFrom == oCmpSSD.StreetNumberFrom) &&
                    (StreetNumberTo == oCmpSSD.StreetNumberTo) &&
                    (Side == oCmpSSD.Side) &&
                    (Zone == oCmpSSD.Zone) &&
                    (Colour == oCmpSSD.Colour) &&
                    (Deleted == oCmpSSD.Deleted));


                int i = 0;

                while (i < GeometryCoordinates.Count() && bRes)
                {
                    bRes = GeometryCoordinates[i].IsEqual(oCmpSSD.GeometryCoordinates[i]);
                    i++;
                }


                if (bRes && Tariffs != null)
                {
                    i = 0;
                    while (i < Tariffs.Count() && bRes)
                    {
                        bRes = (Tariffs[i] == oCmpSSD.Tariffs[i]);
                        i++;
                    }

                }

                if (bRes)
                {
                    foreach (KeyValuePair<int, GridElement> entry in oGridElements.OrderBy(r => r.Key))
                    {

                        bRes = oCmpSSD.oGridElements[entry.Key] != null;
                        if (!bRes)
                        {
                            break;
                        }
                        else
                        {
                            bRes = (oCmpSSD.oGridElements[entry.Key].id == entry.Value.id);
                            if (!bRes)
                                break;
                        }

                    }
                }

            }

            return bRes;

        }

    };

    public class GridElement
    {
        public int id { get; set; }
        public string description { get; set; }
        public List<MapPoint> Polygon { get; set; }
        public int ReferenceCount { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int maxX { get; set; }
        public int maxY { get; set; }
        public List<StreetSectionData> LstStreetSections { get; set; }

        public bool IsEqual(GridElement oCmpElem)
        {
            bool bRes = false;

            bRes = Polygon.Count() == oCmpElem.Polygon.Count();

            if (bRes)
            {
                bRes = ((id == oCmpElem.id) &&
                    (description == oCmpElem.description) &&
                    (x == oCmpElem.x) &&
                    (y == oCmpElem.y) &&
                    (maxX == oCmpElem.maxX) &&
                    (maxY == oCmpElem.maxY));

                if (bRes)
                {
                    int i = 0;

                    while (i < Polygon.Count() && bRes)
                    {
                        bRes = Polygon[i].IsEqual(oCmpElem.Polygon[i]);
                        i++;
                    }

                }

            }

            return bRes;
        }



    }

    public class ParkingRate
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
