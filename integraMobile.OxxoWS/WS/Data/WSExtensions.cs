using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Script.Serialization;

namespace integraMobile.OxxoWS.WS.Data
{
    public static class WSExtensions
    {

        public static string GetDescription(this List<WSZone> oList, int iGrpId)
        {
            string sRet = "";

            if (oList != null)
            {
                foreach (WSZone oZone in oList)
                {
                    if (oZone.Id == iGrpId)
                        sRet = oZone.Description;
                    else if (oZone.SubZones != null)
                        sRet = oZone.SubZones.GetDescription(iGrpId);
                    if (!string.IsNullOrEmpty(sRet))
                        break;
                }
            }

            return sRet;
        }

        public static string GetDescription(this List<WSTariff> oList, int iTariffId)
        {
            string sRet = "";

            if (oList != null)
            {
                var oTariff = oList.Where(tariff => tariff.Id == iTariffId).FirstOrDefault();
                if (oTariff != null)
                    sRet = oTariff.Description;
            }

            return sRet;
        }
        public static IEnumerable<ZoneTreeViewItem> Tree(this List<WSZone> oList)
        {
            return oList.Select(i => new ZoneTreeViewItem()
            {
                Id = i.Id.ToString(),
                Text = i.NumDesc + "~" + i.Description,
                Items = (i.SubZones.Tree().Cast<TreeViewItemModel>().ToList()),
                HasChildren = (i.SubZones.Count > 0)
            });
        }

        public static string ToJson(this List<WSParkingOperationStep> oList)
        {
            string sJson = "";
            sJson = new JavaScriptSerializer().Serialize(oList);
            return sJson;
        }
    }

    public static class SortedListExtension
    {
        public static int GetValueInt(this SortedList oParameters, string sParamName, int iDefaultValue = 0)
        {
            int iRet = iDefaultValue;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                iRet = Convert.ToInt32(oParameters[sParamName]);
            }
            return iRet;
        }
        public static int? GetValueIntNullable(this SortedList oParameters, string sParamName)
        {
            int? iRet = null;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                iRet = Convert.ToInt32(oParameters[sParamName]);
            }
            return iRet;
        }

        public static decimal GetValueDecimal(this SortedList oParameters, string sParamName, decimal dDefaultValue = 0)
        {
            decimal dRet = dDefaultValue;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                dRet = Decimal.Parse(oParameters[sParamName].ToString(), numberFormatProvider);
            }
            return dRet;
        }
        public static decimal? GetValueDecimalNullable(this SortedList oParameters, string sParamName)
        {
            decimal? dRet = null;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                dRet = Decimal.Parse(oParameters[sParamName].ToString(), numberFormatProvider);
            }
            return dRet;
        }

        public static string GetValueString(this SortedList oParameters, string sParamName, string sDefaultValue = "")
        {
            string sRet = sDefaultValue;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                sRet = oParameters[sParamName].ToString();
            }
            return sRet;
        }

        public static DateTime GetValueDateTime(this SortedList oParameters, string sParamName)
        {
            return oParameters.GetValueDateTime(sParamName, new DateTime(1900, 1, 1));
        }
        public static DateTime GetValueDateTime(this SortedList oParameters, string sParamName, DateTime dtDefaultValue)
        {
            DateTime dtRet = dtDefaultValue;
            if (oParameters.ContainsKey(sParamName) && !string.IsNullOrEmpty(oParameters[sParamName].ToString()))
            {
                dtRet = DateTime.ParseExact(oParameters[sParamName].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
            }
            return dtRet;
        }


        public static int GetOutputElementCount(this SortedList oSortedList, string strPath)
        {
            int iRes = 0;

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = oSortedList;
            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (i < arrTags.Count() - 1)
                        {
                            if (oArrParameters.Count == 1)
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                i++;
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }

                        }
                        else
                            iRes = oArrParameters.Count;
                    }
                    else
                    {
                        if (i == arrTags.Count() - 1)
                        {
                            iRes = 1;
                        }
                        break;
                    }


                    i++;

                }
                else
                    break;
            }

            return iRes;
        }


        public static SortedList GetOutputElement(this SortedList oSortedList, string strPath)
        {
            SortedList oList = null;
            string strTemp = null;

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = oSortedList;

            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (oArrParameters.Count == 1)
                        {
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                strTemp = oArrParameters[0].ToString();
                            }
                        }
                        else
                        {
                            i++;
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }
                            else
                            {
                                strTemp = oArrParameters[Convert.ToInt32(arrTags[i])].ToString();
                            }
                        }
                    }
                    else
                    {
                        strTemp = oCurrSortedList[arrTags[i]].ToString();
                    }

                    i++;

                    if (!string.IsNullOrEmpty(strTemp))
                        break;

                    if (i >= arrTags.Count())
                        oList = oCurrSortedList;
                }
                else
                    break;
            }

            return oList;
        }

        public static string GetOutputStringElement(this SortedList oSortedList, string strPath)
        {
            string strRes = null;
            string strTemp = "";

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = oSortedList;

            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    if (oCurrSortedList[arrTags[i]].GetType() == typeof(ArrayList))
                    {
                        ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                        if (oArrParameters.Count == 1)
                        {
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[0];
                            }
                            else
                            {
                                strTemp = oArrParameters[0].ToString();
                            }

                        }
                        else
                        {
                            i++;
                            if (oArrParameters[0].GetType() == typeof(SortedList))
                            {
                                oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                            }
                            else
                            {
                                strTemp = oArrParameters[Convert.ToInt32(arrTags[i])].ToString();
                            }

                        }
                    }
                    else
                    {
                        strTemp = oCurrSortedList[arrTags[i]].ToString();
                    }

                    i++;


                    if (i >= arrTags.Count() && (!string.IsNullOrEmpty(strTemp)))
                        strRes = strTemp;
                }
                else
                    break;
            }

            return strRes;
        }

        public static ArrayList GetOutputElementArray(this SortedList oSortedList, string strPath)
        {
            ArrayList oArray = null;

            string[] arrTags = strPath.Split(new char[] { '/' });
            SortedList oCurrSortedList = oSortedList;

            int i = 0;

            while (i < arrTags.Count())
            {
                if (oCurrSortedList[arrTags[i]] != null)
                {
                    ArrayList oArrParameters = (ArrayList)oCurrSortedList[arrTags[i]];

                    if (i < arrTags.Count() - 1)
                    {
                        if (oArrParameters.Count == 1)
                        {
                            oCurrSortedList = (SortedList)oArrParameters[0];
                        }
                        else
                        {
                            i++;
                            oCurrSortedList = (SortedList)oArrParameters[Convert.ToInt32(arrTags[i])];
                        }
                    }
                    else
                        oArray = oArrParameters;

                    i++;

                }
                else
                    break;
            }

            return oArray;
        }


    }

}