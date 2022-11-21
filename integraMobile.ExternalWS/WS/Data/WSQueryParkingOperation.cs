using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Globalization;
//using WebParking.Properties;

namespace integraMobile.WS.Data
{
    [Serializable]
    public class WSQueryParkingOperation
    {
        public string CityShortDesc;
        public int UTCOffset;
        public int Tariff; //<ad>

        public bool ForceDisp; //<forcedisp>
        public string IdOp; // <idP>
        public decimal Coe; //<coe>
        public string CarCatDesc; //<carcat_desc> car category description</ carcat_desc >
        public string OcuDesc; //<ocu_desc> % occupation description </ocu>

        public int Layout; //<layout>0:no fee, 1: detail fee, 2: summary fee, 3: bonification layout </layout>
        public bool IsServiceCost; //<IsServiceCost> 1: yes / 0: no </IsServiceCost> -- to be used in the summary screen
        public string ServiceParkingLbl; //<ServiceParkingLbl> TASA: </ServiceParkingLbl>
        public string ServiceFeeLbl; //<ServiceFeeLbl> Service Cost: </ ServiceFeeLbl >
        public string ServiceVATLbl; //<ServiceVATLbl> Service VAT: </ ServiceVATLbl >
        public string QSubTotalLbl; //<q_subtotalLbl> Subtotal: </ q_subtotalLbl >
        public string ServiceTotalLbl; //<ServiceTotalLbl> Total: </ ServiceTotalLbl >

        public int Btn1Step; //<btn1step>first step amount in min, for example: 15</btn1step>
        public string Btn1StepLit; //<btn1steplit>+15m</btn1steplit>
        public int Btn2Step; //<btn2step> second step amount in min, for example: 30</btn2step>
        public string Btn2StepLit; //<btn2steplit>+30m</btn2steplit>
        public int Btn3Step; //<btn3step>third step amount in min, could be the maximum, for example 1440</btn3step>
        public string Btn3StepLit; //<btn3steplit>max</btn3steplit>
        public int Q1; //<q1>minimum amount to pay in Cents</q1>
        public int Q2; //<q2>maximum amount to pay in Cents</q2>
        public int T1; //<t1>minimum amount of time to park in minutes</q1>
        public int T2; //<t2> minimum amount of time to park in minutes </q2>
        public DateTime InitialDate; //<di>Initial date (in format hh24missddMMYY) of the parking: the same as the input date if the operation is a first parking, or the date of the end of parking operations chain if the operation is an extension</di>

        public int BonusAmount; //<bonusamount> 0 - 100</bonusamount> Amount to be applied to the bonus.
        public string BonusId; //<bonusid> unique bonus identifier></ bonusid > Identification to the bonus.
        public string DiscountUrl; //<discountURL> additional information to be shown in the bonus summary </discountURL>

        public List<WSParkingOperationStep> Steps = new List<WSParkingOperationStep>();

        public int OperationType; //<o>Operation Type: 1: First parking: 2: extension</o>
        public int Aq; //<aq>Amount of Cents accumulated in the current parking chain (first parking plus all the extensions) linked to the current operation</aq>
        public int At; //<at> Amount of minutes accumulated in the current parking chain (first parking plus all the extensions) linked to the current operation </at>
        //If city currency is different from user currency
        public decimal? Chng; //<chng>change applied to transform city currency to user currency</chng>
        public int Qch1; //<qch1>minimum amount to pay in user currency</qch1>
        public int Qch2; //<qch2>maximum amount to pay in user currency</qch2>

        public WSQueryParkingOperation()
        {

        }

        public WSQueryParkingOperation(SortedList oParameters)
        {
            this.CityShortDesc = oParameters.GetValueString("cityShortDesc");
            this.UTCOffset = oParameters.GetValueInt("utc_offset");
            this.Tariff = oParameters.GetValueInt("ad");

            this.ForceDisp = oParameters.GetValueInt("forcedisp") == 1;
            this.IdOp = oParameters.GetValueString("idP");
            this.Coe = oParameters.GetValueDecimal("coe");
            this.CarCatDesc = oParameters.GetValueString("carcat_desc");
            this.OcuDesc = oParameters.GetValueString("ocu_desc");

            this.Layout = oParameters.GetValueInt("layout");
            this.IsServiceCost = (oParameters.GetValueInt("IsServiceCost") == 1);
            this.ServiceParkingLbl = oParameters.GetValueString("ServiceParkingLbl");
            this.ServiceFeeLbl = oParameters.GetValueString("ServiceFeeLbl");
            this.ServiceVATLbl = oParameters.GetValueString("ServiceVATLbl");
            this.QSubTotalLbl = oParameters.GetValueString("q_subtotalLbl");
            this.ServiceTotalLbl = oParameters.GetValueString("ServiceTotalLbl");

            this.Btn1Step = oParameters.GetValueInt("btn1step");
            this.Btn1StepLit = oParameters.GetValueString("btn1steplit");
            this.Btn2Step = oParameters.GetValueInt("btn2step");
            this.Btn2StepLit = oParameters.GetValueString("btn2steplit");
            this.Btn3Step = oParameters.GetValueInt("btn3step");
            this.Btn3StepLit = oParameters.GetValueString("btn3steplit");
            this.Q1 = oParameters.GetValueInt("q1");
            this.Q2 = oParameters.GetValueInt("q2");
            this.T1 = oParameters.GetValueInt("t1");
            this.T2 = oParameters.GetValueInt("t2");
            this.InitialDate = oParameters.GetValueDateTime("di");

            this.BonusAmount = oParameters.GetValueInt("bonusamount");
            this.BonusId = oParameters.GetValueString("bonusid");
            this.DiscountUrl = oParameters.GetValueString("discountURL");

            this.Steps = new List<WSParkingOperationStep>();

            if (oParameters.GetValueInt("steps_num") > 0)
            {
                for (int i = 0; i < oParameters.GetValueInt("steps_0_step_num"); i++)
                {
                    this.Steps.Add(new WSParkingOperationStep(oParameters, string.Format("steps_0_step_{0}_", i)));
                }
            }
            //public List<WSParkingOperationStep> Steps = new List<WSParkingOperationStep>();

            this.OperationType = oParameters.GetValueInt("o");
            this.Aq = oParameters.GetValueInt("a");
            this.At = oParameters.GetValueInt("at");
            //If city currency is different from user currency
            this.Chng = oParameters.GetValueDecimalNullable("chng");
            this.Qch1 = oParameters.GetValueInt("qch1");
            this.Qch2 = oParameters.GetValueInt("qch2");


        }

        public string GetBtn1StepLit()
        {
            return (!string.IsNullOrEmpty(this.Btn1StepLit) ? this.Btn1StepLit : "Resources.QueryParking_Btn1StepLit");
        }
        public string GetBtn2StepLit()
        {
            return (!string.IsNullOrEmpty(this.Btn2StepLit) ? this.Btn2StepLit : "Resources.QueryParking_Btn2StepLit");
        }
        public string GetBtn3StepLit()
        {
            return (!string.IsNullOrEmpty(this.Btn3StepLit) ? this.Btn3StepLit : "Resources.QueryParking_Btn3StepLit");
        }
        public int GetBtn1Step()
        {
            return (this.Btn1Step != 0 ? this.Btn1Step : 20);
        }
        public int GetBtn2Step()
        {
            return (this.Btn2Step != 0 ? this.Btn2Step : 60);
        }
        public int GetBtn3Step()
        {
            return (this.Btn3Step != 0 ? this.Btn3Step : this.T2);
        }
    }

    [Serializable]
    public class WSParkingOperationStep
    {
        public int T; //<t>=minimum time (t1)</t>
		public int Q; //<q>=minimum quantity(q1)</q>
		public DateTime D; //<d>=<di>+<t>(resulting tariff date)</d>
        public decimal? Qch; //<qch>q in user currency</qch> If city currency is different from user currency
        
        public int QFee; //<q_fee> cost of the service </ q_fee >		-- optional
        public int QVat; //<q_vat>vat applied to the cost of the service</ q_vat >	 -- optional		
        public int QSubTotal; //<q_subtotal> q + q_fee</ subtotal >
        public int QBonusAmount; //<qbonusam> 0 - 100</ qbonusam >
        public int QTotal; //<q_total>total amount: q + cost + vat </ q_total >	 -- optional 
        public WSParkingOperationStepDiscount Discount; //<discount> // optional – this will be the second ticket 

        public WSParkingOperationStep()
        {

        }

        public WSParkingOperationStep(SortedList oParameters, string sPrefix)
        {

            this.T = oParameters.GetValueInt(sPrefix + "t");
		    this.Q = oParameters.GetValueInt(sPrefix + "q");
		    this.D = oParameters.GetValueDateTime(sPrefix + "d");
            this.Qch = oParameters.GetValueDecimalNullable(sPrefix + "qch");

        
            this.QFee = oParameters.GetValueInt(sPrefix + "q_fee");
            this.QVat = oParameters.GetValueInt(sPrefix + "q_vat");
            this.QSubTotal = oParameters.GetValueInt(sPrefix + "q_subtotal");
            this.QBonusAmount = oParameters.GetValueInt(sPrefix + "qbonusam");
            this.QTotal = oParameters.GetValueInt(sPrefix + "q_total");


            if (oParameters.GetValueInt(sPrefix + "discount_num") > 0)
            {
                this.Discount = new WSParkingOperationStepDiscount()
                {
                    T = oParameters.GetValueInt(sPrefix + "discount_0_td"),
                    Q = oParameters.GetValueInt(sPrefix + "discount_0_zd"),
                    D = oParameters.GetValueDateTime(sPrefix + "discount_0_dd"),
                    Qch = oParameters.GetValueInt(sPrefix + "discount_0_qdch")
                };
            }
        
            
        }

    }

    [Serializable]
    public class WSParkingOperationStepDiscount
    {
        public int T; //<td>=bonus time (tb)</td>
        public int Q; //<zd>=bonus quantity(qb)</qd>
        public DateTime D; //<dd>=<di>+<t>(resulting tariff date)</dd>
        public int Qch; //<qdch>qb in user currency</qdch>
    }
}