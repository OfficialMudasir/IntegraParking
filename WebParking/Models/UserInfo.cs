using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using WebParking.WS.Data;

namespace WebParking.Models
{
    [Serializable]
    public class UserInfo
    {
        public string User = "";
        public string Pwd = "";
        public string SessionID = "";
        public string Language = (ConfigurationManager.AppSettings["DefaultLanguage"] ?? "es-ES");
        
        public WSUserPlates UserPlates = null;
        public WSUserPreferences UserPreferences = null;
        public WSZoneTar ZoneTar = null;
        public WSQueryParkingOperation QueryParkingOperation = null;

        public string LegalTermsVersion = "";

        public string Cur = "EUR";

        public int CityId = 0;
        public string CityName = "";
        public int Balance = 0;

        public string Plate = "";
        public int GroupId = 0;        
        public int TariffId = 0;
        public bool MadTarInfo = false;
        public int StepIndex = -1;
        public bool ParkConfirmed = false;

        public UserInfo()
        {

        }

        public WSParkingOperationStep CurrentStep()
        {
            WSParkingOperationStep oRet = null;
            if (this.StepIndex >= 0 && this.QueryParkingOperation.Steps.Count > this.StepIndex)
                oRet = this.QueryParkingOperation.Steps[this.StepIndex];
            return oRet;
        }
        public string FormatedAmount(int iCents)
        {
            return (((double)iCents) / 100).ToString("#0.00") + " " + Cur;
        }
    }
}