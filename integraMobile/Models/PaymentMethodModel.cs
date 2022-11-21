using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.Models
{
    [Serializable]
    public class PaymentMethodModel
    {
        #region Properties
        public List<PaymentMethodStModel> st { get; set; }
        #endregion

        #region Constructor
        public PaymentMethodModel()
        {
            st = new List<PaymentMethodStModel>();
        }
        #endregion
    }
}