using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Globalization;


namespace Offstreet.Test.WS.Data
{
    [Serializable]
    public class WSQueryCarExitforPayment
    {
        public decimal GroupId; // <g>
        public string OpeId; // <ope_id> Operation unique ID: number/barcode/ticket ID </ope_id>
        public int OpeIdType; // <ope_id_type> 1: MEYPAR id, 2: I@ id QR </ope_id_type>
        public string Plate; // <p>plate</p>
        public int Op; // <op> 1: there is an overpayment because user did not leave in the courtesy time/ 0: first payment >/op>
        public int Q; // <q>Amount of money paid in Cents</q>
        public int? Qch; // <qch>Amount q in user currency</qch>
        public decimal? Chng; // <chng>change applied to transform city currency to user currency</chng>
        public int T; // <t>Time in minutes obtained paying <q> cents</t>
        public DateTime BeginDate; // <bd>Initial date (in format hh24missddMMYY) of the parking:  exacty is the <d> tag value provided in NotifyCarEntry method</bd>  Mandotory
        public DateTime EndDate; // <ed>End date (in format hh24missddMMYY) </ed> Mandatory
        public int TarId; // <tar_id> tariff id that could be different from the entry one because user parked in  another place</tar_id>
        public DateTime MaxExitDate; // <med> maximum exit date for leaving the parking once has been paid  </med>

        public WSQueryCarExitforPayment()
        {

        }

        public WSQueryCarExitforPayment(SortedList oParameters)
        {
            this.GroupId = oParameters.GetValueDecimal("g");
            this.OpeId = oParameters.GetValueString("ope_id");
            this.OpeIdType = oParameters.GetValueInt("ope_id_type");
            this.Plate = oParameters.GetValueString("p");
            this.Op = oParameters.GetValueInt("op");
            this.Q = oParameters.GetValueInt("q");
            this.Qch = oParameters.GetValueIntNullable("qch");
            this.Chng = oParameters.GetValueIntNullable("chng");
            this.T = oParameters.GetValueInt("t");
            this.BeginDate = oParameters.GetValueDateTime("bd");
            this.EndDate = oParameters.GetValueDateTime("ed");
            this.TarId = oParameters.GetValueInt("tar_id");
            this.MaxExitDate = oParameters.GetValueDateTime("med");
        }
    }

}