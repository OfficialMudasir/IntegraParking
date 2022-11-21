using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class CountriesModel
    {
        #region Propierties
        public List<Country> country { get; set; }
        #endregion

        #region Constructor
        public CountriesModel()
        {
            country = new List<Country>();
        }
        #endregion

        [Serializable]
        public class Country
        {
            #region Propierties
            public Int32 idcountry { get; set; }
            public List<Questions> questions { get; set; }
            #endregion

            #region Constructor
            public Country()
            {
                questions = new List<Questions>();
            }
            #endregion
        }
        
        [Serializable]
        public class Questions
        {
            #region Propierties
            public List<Question> question { get; set; }
            #endregion

            #region Constructor
            public Questions()
            {
                question = new List<Question>();
            }
            #endregion
        }
        
        [Serializable]
        public class Question
        {
            #region Propierties
            public Int32 idversion { get; set; }
            public String literal { get; set; }
            public Int32 mandatory { get; set; }
            public Urls urls { get; set; }
            #endregion

            #region Constructor
            public Question()
            {
                urls = new Urls();
            }
            #endregion
        }
        
        [Serializable]
        public class Urls
        {
            #region Propierties
            public List<String> url { get; set; }
            #endregion

            #region Constructor
            public Urls()
            {
                url = new List<string>();
            }
            #endregion
        }
    }
}