using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace integraMobile.OxxoWS.WS.Data
{
    public class WSCities
    {
        public Dictionary<int, string> Cities;

        public WSCities()
        {
            Cities = new Dictionary<int, string>();
        }
        public WSCities(SortedList oParameters)
        {

        }

        public List<WSCity> GetCities()
        {
            return Cities.Select(i => new WSCity() { Id = i.Key, Description = i.Value }).ToList();
        }
    }

    public class WSCity
    {
        public int Id;
        public string Description;
    }
}