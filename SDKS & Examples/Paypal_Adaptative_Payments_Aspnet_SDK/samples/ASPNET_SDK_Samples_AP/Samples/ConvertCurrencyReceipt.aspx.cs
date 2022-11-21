using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;

namespace ASPNET_SDK_Samples.Samples
{
    public partial class ConvertCurrencyReceipt : System.Web.UI.Page
    {
       #region Public Properties

        /// <summary>
        /// Returns CancelPreapprovalResponse value from Session
        /// </summary>
        public ConvertCurrencyResponse ConvertCurrencyResponse
        {
            get
            {
                if (Session[Constants.SessionConstants.CONVERTCURRENCYRESPONSE] == null)
                    return null;

                return (ConvertCurrencyResponse)Session[Constants.SessionConstants.CONVERTCURRENCYRESPONSE];
            }

        }

        /// <summary>
        /// GetCurrencyList
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyList()
        {
            string   result = "";

            try
            {
                if (this.ConvertCurrencyResponse != null)
                {

                    foreach (CurrencyConversionList CCList in ConvertCurrencyResponse.estimatedAmountTable)
                    {
                        foreach (CurrencyType CType in CCList.currencyList)
                        {
                            result += CType.code + " " + CType.amount.ToString () + " ";
                        }
                        result += "<br>";
                    }
                }                
            }
            catch (Exception ex)
            {
                FATALException FATALEx = new FATALException("Error occurred in Cancel Preapproval Receipt Page.", ex);
                Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                this.Response.Redirect("APIError.aspx?type=FATAL", false);
                result = "";
            }
            return result;
        }
        #endregion
    }
}
