using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class PaymentMethodStModel
    {
        #region Properties
        [JsonProperty("@id")]
        public int id { get; set; }
        public List<PaymentMethodItemModel> pm { get; set; }
        #endregion

        #region Constructor
        public PaymentMethodStModel()
        {
            pm = new List<PaymentMethodItemModel>();
        }
        #endregion
     }
}