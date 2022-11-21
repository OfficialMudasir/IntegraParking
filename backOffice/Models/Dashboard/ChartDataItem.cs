using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using integraMobile.Domain.Abstract;
using backOffice.Properties;

namespace backOffice.Models.Dashboard
{
    public class ChartDataItem
    {
        private string _category;
        private List<float> _series;

        public string Category {
            get { return _category; }
        }
        public float Serie0
        {
            get { if (_series != null && _series.Count >= 1) return _series[0]; else return 0; }
        }
        public float Serie1
        {
            get { if (_series != null && _series.Count >= 2) return _series[1]; else return 0; }
        }
        public float Serie2
        {
            get { if (_series != null && _series.Count >= 3) return _series[2]; else return 0; }
        }
        public float Serie3
        {
            get { if (_series != null && _series.Count >= 4) return _series[3]; else return 0; }
        }
        public float Serie4
        {
            get { if (_series != null && _series.Count >= 5) return _series[4]; else return 0; }
        }
        public float Serie5
        {
            get { if (_series != null && _series.Count >= 6) return _series[5]; else return 0; }
        }
        public float Serie6
        {
            get { if (_series != null && _series.Count >= 7) return _series[6]; else return 0; }
        }
        public float Serie7
        {
            get { if (_series != null && _series.Count >= 8) return _series[7]; else return 0; }
        }

        public List<float> Series
        {
            get { return _series; }
        }


        public ChartDataItem(DateGroupType dateGroup, bool dateGroupPattern, string sCategory, params float[] series /*float lSerie0, float lSerie1, float lSerie2*/ )
        {
            _category = sCategory;
            DateTime xDateTime;
            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        if (DateTime.TryParse(sCategory, out xDateTime))
                            _category = xDateTime.ToString("HH") + "h " + xDateTime.ToString("dd-MM");
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                        int iWeekDay = Convert.ToInt32(sCategory.Split('-')[1]);
                        iWeekDay = (iWeekDay == 7 ? 0 : iWeekDay);
                        _category = sCategory.Split('-')[0] + "-" + names[iWeekDay];
                    }
                    else
                    {                                                
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                        int iWeekDay = Convert.ToInt32(sCategory);
                        iWeekDay = (iWeekDay == 7 ? 0 : iWeekDay);
                        _category = names[iWeekDay];
                    }
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        if (DateTime.TryParse(sCategory, out xDateTime))
                            _category = xDateTime.ToString("dd-MM");
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                        _category = sCategory.Split('-')[0] + "-" + names[Convert.ToInt32(sCategory.Split('-')[1]) - 1];
                    }
                    else
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                        _category = names[Convert.ToInt32(sCategory)-1];
                    }
                    break;

            }
            _series = new List<float>();
            _series.AddRange(series);
            /*foreach (var fSerie in series)
            {
                _series.Add(fSerie);
            }*/
        }

        public ChartDataItem(string sCategory, params float[] series /*float lSerie0, float lSerie1, float lSerie2*/ )
        {
            _category = sCategory;
            _series = new List<float>();
            _series.AddRange(series);
        }
        public ChartDataItem(ChargeOperationsType oChargeOperationType, float lTotal)
        {
            _category = Resources.ResourceManager.GetString("ChargeOperationsTypes_" + System.Enum.GetName(typeof(ChargeOperationsType), oChargeOperationType));            
            _series = new List<float>();
            _series.Add(lTotal);
        }

        public ChartDataItem(DateGroupType dateGroup, bool dateGroupPattern, string sCategory, int iCount0, int iCount1, int iCount2, float[] fSum0, float[] fSum1, float[] fSum2)
        {
            _category = sCategory;
            DateTime xDateTime;
            switch (dateGroup)
            {
                case DateGroupType.hour:
                    if (!dateGroupPattern)
                    {
                        if (DateTime.TryParse(sCategory, out xDateTime))
                            _category = xDateTime.ToString("HH") + "h " + xDateTime.ToString("dd-MM");
                    }
                    break;
                case DateGroupType.weekday:
                    if (!dateGroupPattern)
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                        int iWeekDay = Convert.ToInt32(sCategory.Split('-')[1]);
                        iWeekDay = (iWeekDay == 7 ? 0 : iWeekDay);
                        _category = sCategory.Split('-')[0] + "-" + names[iWeekDay];
                    }
                    else
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                        int iWeekDay = Convert.ToInt32(sCategory);
                        iWeekDay = (iWeekDay == 7 ? 0 : iWeekDay);
                        _category = names[iWeekDay];
                    }
                    break;
                case DateGroupType.day:
                    if (!dateGroupPattern)
                    {
                        if (DateTime.TryParse(sCategory, out xDateTime))
                            _category = xDateTime.ToString("dd-MM");
                    }
                    break;
                case DateGroupType.month:
                    if (!dateGroupPattern)
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                        _category = sCategory.Split('-')[0] + "-" + names[Convert.ToInt32(sCategory.Split('-')[1]) - 1];
                    }
                    else
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                        _category = names[Convert.ToInt32(sCategory) - 1];
                    }
                    break;
            }
            _series = new List<float>();
            foreach (float fSum in fSum0)
            {
                _series.Add((iCount0 > 0 ? fSum / iCount0 : 0));
            }
            foreach (float fSum in fSum1)
            {
                _series.Add((iCount1 > 0 ? fSum / iCount1 : 0));
            }
            foreach (float fSum in fSum2)
            {
                _series.Add((iCount2 > 0 ? fSum / iCount2 : 0));
            }
        }

        public ChartDataItem(string sCategory, int[] iCounts, params float[][] paramsfSums)
        {
            _category = sCategory;
            _series = new List<float>();
            int i = 0;
            foreach (int iCount in iCounts)
            {
                foreach (float fSum in paramsfSums[i])
                {
                    _series.Add((iCount > 0 ? fSum / iCount : 0));
                }
                i += 1;
            }
        }


    }

    public class OperationsUsers
    {
        public DateTime HOUR { get; set; }
        public int? PARKINGS_COUNT { get; set; }
        public int? EXTENSIONS_COUNT { get; set; }
        public int? REFUNDS_COUNT { get; set; }
        public int? RECHARGES_COUNT { get; set; }
        public int? USERS_COUNT { get; set; }
        public int? USERS_ALL_COUNT { get; set; }
    }
}