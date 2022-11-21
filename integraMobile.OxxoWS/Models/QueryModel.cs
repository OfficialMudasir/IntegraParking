using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Runtime.Serialization;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;

namespace integraMobile.OxxoWS.Models
{
    public enum QueryResult
    {
        success = 0,
        invalid_user = 1,
        // ...
        exception = 99
    }

    [DataContract(Namespace = "")]
    [XmlType(Namespace = "", TypeName = "OLS")]
    public class QueryOutput : ModelBase
    {
        //Log4net Wrapper class
        private static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(QueryOutput));

        public QueryOutput()
        {
            version = "1.0";
            account = "";
            name = "";
            address = "";
            status = "";
            reference = "";
            partial = "";
        }

        #region Properties

        [DataMember]
        public string account { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string reference { get; set; }

        [DataMember]
        public string partial { get; set; }

        [DataMember]
        public Concept[] concepts { get; set; }

        [DataMember]
        public string code { get; set; }
        [IgnoreDataMember]
        [XmlIgnore]
        public QueryResult codeType
        {
            get { return (QueryResult)Convert.ToInt32(this.code); }
            set
            {
                this.code = string.Format("{0:00}", (int)value);
                switch (value)
                {
                    case QueryResult.success: this.message = "Cliente Encontrado"; break;
                    case QueryResult.invalid_user: this.message = "Cliente No Encontrado"; break;

                    case QueryResult.exception: this.message = "Error"; break;
                }
            }
        }

        [DataMember]
        public string message { get; set; }

        [XmlAttribute("version")]
        public string version { get; set; }

        #endregion

        #region Public Methods

        public bool Query(string sClient, ICustomersRepository oCustomersRepository, IInfraestructureRepository oInfraestructureRepository)
        {
            m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Query::Input params:client={0}", sClient));
            string strUserEmail = "";

            try
            {

                USER oUser = null;

                decimal? dUserId = null;
                try
                {
                    dUserId = Convert.ToDecimal(sClient);
                }
                catch (Exception ex)
                {
                    dUserId = null;
                }

                if (dUserId.HasValue && oCustomersRepository.GetUserDataById(ref oUser, dUserId.Value))
                {
                    strUserEmail = oUser.USR_EMAIL;
                    this.account = oUser.USR_ID.ToString();
                    this.name = (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_NAME) ? oUser.CUSTOMER.CUS_NAME : oUser.USR_USERNAME);
                    if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_SURNAME1)) this.name += " " + oUser.CUSTOMER.CUS_SURNAME1;
                    if (!string.IsNullOrEmpty(oUser.CUSTOMER.CUS_SURNAME2)) this.name += " " + oUser.CUSTOMER.CUS_SURNAME2;
                    this.address = string.Format("{0} {1}, {2} {3}, {4}", oUser.CUSTOMER.CUS_STREET, oUser.CUSTOMER.CUS_STREE_NUMBER, oUser.CUSTOMER.CUS_ZIPCODE, oUser.CUSTOMER.CUS_CITY, oUser.CUSTOMER.CUS_STATE);
                    if (this.address.Length > 100) this.address = this.address.Substring(0, 100);
                    
                    this.partial = "P";
                    var oConcepts = new List<Concept>();
                    try
                    {
                        string sCurIsoCode = ConfigurationManager.AppSettings["Currency_IsoCode"] ?? "MXN";
                           
                        var oRechargeValues = oInfraestructureRepository.Currencies.Where(c => c.CUR_ISO_CODE == sCurIsoCode).First()
                                                                        .CURRENCY_RECHARGE_VALUEs.Where(s => s.CURV_VALUE_TYPE == (int)RechargeValuesTypes.rvt_OxxoRecharge)
                                                                        .OrderBy(s => s.CURV_VALUE);
                        var oMinRechargeValue = oRechargeValues.FirstOrDefault();
                        if (oMinRechargeValue != null)
                            oConcepts.Add(new Concept() { description = "amount min", amount = oMinRechargeValue.CURV_VALUE, operation = "min" });
                        var oMaxRechargeValue = oRechargeValues.LastOrDefault();
                        if (oMaxRechargeValue != null)
                            oConcepts.Add(new Concept() { description = "amount max", amount = oMaxRechargeValue.CURV_VALUE, operation = "max" });
                        if (oMinRechargeValue != null)
                            oConcepts.Add(new Concept() { description = "pago minimo", amount = oMinRechargeValue.CURV_VALUE, operation = "+" });

                    }
                    catch (Exception ex)
                    {
                        m_oLog.LogMessage(LogLevels.logERROR, string.Format("OXXO Query::Error getting currency_recharge_values: client={0}::Email={1}", sClient, strUserEmail), ex);
                    }
                    this.concepts = oConcepts.ToArray();

                    this.codeType = QueryResult.success;
                }
                else
                    this.codeType = QueryResult.invalid_user;

            }
            catch (Exception ex)
            {
                this.codeType = QueryResult.exception;
                m_oLog.LogMessage(LogLevels.logERROR, string.Format("OXXO Query::Exception: client={0}::Email={1}", sClient, strUserEmail), ex);
            }

            m_oLog.LogMessage(LogLevels.logDEBUG, string.Format("OXXO Query::Email={0}::QueryOutput={1}",strUserEmail, PrettyXml(Conversions.XmlSerializeToString(this))));

            return (this.codeType == QueryResult.success);
        }

        public static QueryOutput Deserialize(string inboundXML)
        {
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(inboundXML));
            var serializer = new DataContractSerializer(typeof(QueryOutput));
            var request = new QueryOutput();
            request = (QueryOutput)serializer.ReadObject(ms);

            return request;
        }


        #endregion
        
    }

    [DataContract(Namespace = "", Name = "concept")]
    [XmlType(Namespace = "", TypeName = "concept")]
    public class Concept
    {
        [DataMember]
        public string description { get; set; }

        [DataMember]
        public long amount { get; set; }

        [DataMember]
        public string operation { get; set; }
    }
}