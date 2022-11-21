using integraMobile.ExternalWS;
using integraMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace integraMobile.Response
{
    [Serializable]
    public class ResponseSignUpStep2
    {
        #region Properties
        public String signup_guid { get; set; }
        public String username { get; set; }
        public String email { get; set; }
        public ResultType r  { get; set; }
        public int legalterms { get; set; }
        public String legaltermsver  { get; set; }
        public String url1  { get; set; }
        public String url2  { get; set; }
        public SubscriptionTypeModel subscription_type { get; set; }
        public PaymentMethodModel payment_method  { get; set; }
        public String ccprovider  { get; set; }
        public String Paypal_Client_id { get; set; }
        public String Paypal_Environment { get; set; }
        public String creditcall_guid { get; set; }
        public String creditcall_token_url { get; set; }
        public String creditcall_hash_seed_key { get; set; }
        public String stripe_guid { get; set; }
        public String stripe_token_url { get; set; }
        public String stripe_hash_seed_key { get; set; }
        public String iecisa_guid { get; set; }
        public String iecisa_token_url { get; set; }
        public String iecisa_hash_seed_key { get; set; }
        public String moneris_guid { get; set; }
        public String moneris_token_url { get; set; }
        public String moneris_hash_seed_key { get; set; }
        public String transbank_guid { get; set; }
        public String transbank_token_url { get; set; }
        public String transbank_hash_seed_key { get; set; }
        public String payu_guid  { get; set; }
        public String payu_token_url  { get; set; }
        public String payu_hash_seed_key  { get; set; }
        public int per_transaction_minimum_charge_amount { get; set; }
        public String per_transaction_minimum_charge_currency  { get; set; }
        #endregion

        #region Constructor
        public ResponseSignUpStep2()
        {
            subscription_type = new SubscriptionTypeModel();
            payment_method = new PaymentMethodModel();
        }
        #endregion
    }
}