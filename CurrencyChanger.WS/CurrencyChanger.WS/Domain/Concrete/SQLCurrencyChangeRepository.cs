using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Transactions;
using CurrencyChanger.WS.Domain.Abstract;
using CurrencyChanger.WS.Infrastructure.Logging.Tools;

namespace CurrencyChanger.WS.Domain.Concrete
{
    public class SQLCurrencyChangeRepository : ICurrencyChangeRepository
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(SQLCurrencyChangeRepository));
        private const int ctnTransactionTimeout = 30;
        private List<PARAMETER> parametersTable;



        public SQLCurrencyChangeRepository(string connectionString)
        {
            parametersTable = (DataContextFactory.GetScopedDataContext<iCurrencyChangeDataContext>(null, connectionString)).GetTable<PARAMETER>().ToList();
        }


        public string GetParameterValue(string strParamValue)
        {
            string res = null;

            try
            {
                res = parametersTable.Where(r => r.PAR_NAME == strParamValue).Select(r=> r.PAR_VALUE).FirstOrDefault();
            }
            catch
            {
                res = null;
            }

            return res;


        }


        public string GetParameterValue(string strParamValue,string strDefaultValue)
        {
            string res = null;

            try
            {
                res = parametersTable.Where(r => r.PAR_NAME == strParamValue).Select(r => r.PAR_VALUE).FirstOrDefault();
                if (string.IsNullOrEmpty(res))
                {
                    res = strDefaultValue;
                }
            }
            catch
            {
                res = strDefaultValue;
            }

            return res;


        }


        public int GetParameterValue(string strParamValue, int iDefaultValue)
        {
            int iRes = iDefaultValue;

            try
            {
                var res = parametersTable.Where(r => r.PAR_NAME == strParamValue).Select(r => Convert.ToInt32(r.PAR_VALUE)).FirstOrDefault();
                if (res!=null)
                {
                    iRes = res;
                }
            }
            catch
            {
                iRes = iDefaultValue;
            }

            return iRes;


        }        


        public bool GetCurrencyChange(string strSrcIsoCode, string strDstIsoCode,ref CURRENCY_CHANGE oChange,
                                      ref SortedList<string,double> oOtherCurrenciesChanges)
        {
            bool bRes = true;
            oChange = null;
            try
            {

                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    iCurrencyChangeDataContext dbContext = DataContextFactory.GetScopedDataContext<iCurrencyChangeDataContext>();

                    var cChanges = (from r in dbContext.CURRENCY_CHANGEs
                                 where r.CURRENCy.CUR_ISO_CODE == strDstIsoCode && r.CURRENCy1.CUR_ISO_CODE == strSrcIsoCode
                                 orderby r.CUCH_CHANGE_UTC_DATE descending
                                 select r);

                    if (cChanges.Count() > 0)
                    {
                        oChange = cChanges.First();
                    }


                    var oOtherCurrenciesISOCodes = (from r in dbContext.CURRENCY_CHANGEs
                                               where r.CURRENCy.CUR_ISO_CODE != strDstIsoCode && r.CURRENCy1.CUR_ISO_CODE == strSrcIsoCode
                                               select r.CURRENCy.CUR_ISO_CODE).Distinct().ToList();

                    oOtherCurrenciesChanges = new SortedList<string, double>();

                    foreach (string strISOCode in oOtherCurrenciesISOCodes)
                    {
                        oOtherCurrenciesChanges.Add(strISOCode,1);
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "GetCurrencyChange: ", e);
                bRes = false;
            }

            return bRes;

        }

        public bool SetCurrencyChange(string strSrcIsoCode, string strDstIsoCode, double dChange, ref SortedList<string, double> oOtherCurrenciesChanges)
        {
            bool bRes = true;
            try
            {

                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                                                             new TransactionOptions()
                                                                                             {
                                                                                                 IsolationLevel = IsolationLevel.ReadUncommitted,
                                                                                                 Timeout = TimeSpan.FromSeconds(ctnTransactionTimeout)
                                                                                             }))
                {
                    iCurrencyChangeDataContext dbContext = DataContextFactory.GetScopedDataContext<iCurrencyChangeDataContext>();

                    DateTime dtUTCNow = DateTime.UtcNow;

                    SetCurrencyChange(dbContext, strSrcIsoCode, strDstIsoCode, dChange, dtUTCNow);


                    foreach (KeyValuePair<string, double> oTuple in oOtherCurrenciesChanges)
                    {
                        SetCurrencyChange(dbContext, strSrcIsoCode, oTuple.Key, oTuple.Value, dtUTCNow);
                    }
    

                    // Submit the change to the database.
                    try
                    {
                        SecureSubmitChanges(ref dbContext);                       
                        transaction.Complete();

                    }
                    catch (Exception e)
                    {
                        m_Log.LogMessage(LogLevels.logERROR, "UpdateSession: ", e);
                        bRes = false;
                    }


                  
                }
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetCurrencyChange: ", e);
                bRes = false;
            }

            return bRes;

        }

        private bool SetCurrencyChange(iCurrencyChangeDataContext dbContext, string strSrcIsoCode, string strDstIsoCode, double dChange, DateTime dtUTCNow)
        {
            bool bRes = true;
            try
            {


                var cChanges = (from r in dbContext.CURRENCY_CHANGEs
                                where r.CURRENCy.CUR_ISO_CODE == strDstIsoCode && r.CURRENCy1.CUR_ISO_CODE == strSrcIsoCode
                                orderby r.CUCH_CHANGE_UTC_DATE descending
                                select r);
                
                if (cChanges.Count() > 0)
                {
                    CURRENCY_CHANGES_HI oHisChange = new CURRENCY_CHANGES_HI()
                    {
                        CUCHH_SRC_CUR_ID = cChanges.First().CUCH_SRC_CUR_ID,
                        CUCHH_DST_CUR_ID = cChanges.First().CUCH_DST_CUR_ID,
                        CUCHH_CHANGE = cChanges.First().CUCH_CHANGE,
                        CUCHH_CHANGE_UTC_INIDATE = cChanges.First().CUCH_CHANGE_UTC_DATE,
                        CUCHH_CHANGE_UTC_ENDDATE = dtUTCNow,
                    };

                    dbContext.CURRENCY_CHANGES_HIs.InsertOnSubmit(oHisChange);
                    cChanges.First().CUCH_CHANGE = Convert.ToDecimal(dChange);
                    cChanges.First().CUCH_CHANGE_UTC_DATE = dtUTCNow;

                }
                else
                {

                    CURRENCY_CHANGE oChange = new CURRENCY_CHANGE()
                    {
                        CUCH_SRC_CUR_ID = dbContext.CURRENCies.Where(r => r.CUR_ISO_CODE == strSrcIsoCode).Select(r => r.CUR_ID).First(),
                        CUCH_DST_CUR_ID = dbContext.CURRENCies.Where(r => r.CUR_ISO_CODE == strDstIsoCode).Select(r => r.CUR_ID).First(),
                        CUCH_CHANGE = Convert.ToDecimal(dChange),
                        CUCH_CHANGE_UTC_DATE = dtUTCNow,
                    };

                    dbContext.CURRENCY_CHANGEs.InsertOnSubmit(oChange);


                }
                                   
            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "SetCurrencyChange: ", e);
                bRes = false;
            }

            return bRes;

        }


        private void SecureSubmitChanges(ref iCurrencyChangeDataContext dbContext)
        {

            try
            {
                dbContext.SubmitChanges(ConflictMode.ContinueOnConflict);
            }

            catch (ChangeConflictException e)
            {
                Console.WriteLine(e.Message);
                // Automerge database values for members that client
                // has not modified.
                foreach (ObjectChangeConflict occ in dbContext.ChangeConflicts)
                {
                    occ.Resolve(RefreshMode.KeepChanges);
                }
            }

            // Submit succeeds on second try.
            dbContext.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }

    }
}