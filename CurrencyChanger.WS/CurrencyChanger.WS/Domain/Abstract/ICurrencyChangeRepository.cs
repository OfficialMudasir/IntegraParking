using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyChanger.WS.Domain.Abstract
{
    public interface ICurrencyChangeRepository
    {
        string GetParameterValue(string strParamValue);
        string GetParameterValue(string strParamValue, string strDefaultValue);
        int GetParameterValue(string strParamValue, int iDefaultValue);
        bool GetCurrencyChange(string strSrcIsoCode, string strDstIsoCode, ref CURRENCY_CHANGE oChange, ref SortedList<string, double> oOtherCurrenciesChanges);
        bool SetCurrencyChange(string strSrcIsoCode, string strDstIsoCode, double dChange, ref SortedList<string, double> oOtherCurrenciesChanges);
    }
}