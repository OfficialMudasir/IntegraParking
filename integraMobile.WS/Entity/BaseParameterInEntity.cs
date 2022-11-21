using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace integraMobile.WS.Entity
{
    [Serializable]
    public class BaseParameterInEntity
    {
        #region Properties
        public string appvers { get; set;}
    	public string cityID { get; set;}
    	public string cmodel { get; set;}
    	public string cosvers { get; set;}
    	public string date { get; set;}
    	public string IdServiceType { get; set;}
    	public string IMEI { get; set;}
    	public string lang { get; set;}
    	public string OSID  { get; set;}
    	public string SessionID  { get; set;}
    	public string u  { get; set;}
    	public string utc_date  { get; set;}
    	public string utc_offset  { get; set;}
    	public string vers  { get; set;}
        public string WIFIMAC { get; set; }
        #endregion

        #region Constructor
        public BaseParameterInEntity()
        { }
        #endregion
    }
}