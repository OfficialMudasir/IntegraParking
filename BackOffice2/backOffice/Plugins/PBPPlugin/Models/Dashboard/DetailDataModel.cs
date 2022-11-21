using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Web;

namespace backOffice.Models.Dashboard
{
    public class DetailDataModel
    {
        private DateGroupType _dateGroup;
        private bool _dateGroupPattern;
        private DateTime _iniDate;
        private DateTime _endDate;
        private string _iniTime;
        private string _endTime;



        private Type _modeType = null;

        private IQueryable _data;


        public DetailDataModel(DashboardDataModel filters)
        {
            this._dateGroup = filters.DateGroup;
            this._dateGroupPattern = filters.DateGroupPattern;
            this._iniTime = filters.CustomIniTime;
            this._endTime = filters.CustomEndTime;
            filters.GetDateRange(out this._iniDate, out this._endDate);
        }

        public DetailDataModel(DateGroupType dateGroup, bool dateGroupPattern, DateTime iniDate, DateTime endDate, string iniTime, string endTime)
        {
            this._dateGroup = dateGroup;
            this._dateGroupPattern = dateGroupPattern;
            this._iniDate = iniDate;
            this._endDate = endDate;
            this._iniTime = iniTime;
            this._endTime = endTime;

            this.CreateModelType();

        }

        private void CreateModelType()
        {
            // Create domain model type
            //List<DynamicProperty> lstProps = new List<DynamicProperty>();

            List<string> lstFields = new List<string>();

            DateTime dtDate;

            if (!this._dateGroupPattern)
            {
                switch (this._dateGroup)
                {
                    case DateGroupType.hour:
                        dtDate = new DateTime(this._iniDate.Year, this._iniDate.Month, this._iniDate.Day, this._iniDate.Hour, 0, 0);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddHours(1);
                        }
                        break;

                    case DateGroupType.weekday:
                    case DateGroupType.day:
                        dtDate = this._iniDate.Date;
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddDays(1);
                        }
                        break;

                    case DateGroupType.week:
                        dtDate = this._iniDate.Date;
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddDays(7);
                        }
                        break;

                    case DateGroupType.month:
                        dtDate = new DateTime(this._iniDate.Year, this._iniDate.Month, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(1);
                        }
                        break;

                    case DateGroupType.quarter:
                        dtDate = new DateTime(this._iniDate.Year, (((this._iniDate.Month - 1) / 3) * 3) + 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(3);
                        }
                        break;
                    case DateGroupType.half:
                        dtDate = new DateTime(this._iniDate.Year, (((this._iniDate.Month - 1) / 6) * 6) + 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(6);
                        }
                        break;
                    case DateGroupType.year:
                        dtDate = new DateTime(this._iniDate.Year, 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddYears(1);
                        }
                        break;
                }
            }
            else
            {
                switch (this._dateGroup)
                {
                    case DateGroupType.hour:
                        dtDate = new DateTime(this._iniDate.Year, this._iniDate.Month, this._iniDate.Day, 0, 0, 0);
                        DateTime dtEnd = dtDate.AddDays(1);
                        while (dtDate < dtEnd)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddHours(1);
                        }
                        break;

                    case DateGroupType.weekday:

                    case DateGroupType.day:
                        dtDate = this._iniDate.Date;
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddDays(1);
                        }
                        break;

                    case DateGroupType.week:
                        dtDate = this._iniDate.Date;
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddDays(7);
                        }
                        break;

                    case DateGroupType.month:
                        dtDate = new DateTime(this._iniDate.Year, this._iniDate.Month, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(1);
                        }
                        break;

                    case DateGroupType.quarter:
                        dtDate = new DateTime(this._iniDate.Year, (((this._iniDate.Month - 1) / 3) * 3) + 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(3);
                        }
                        break;
                    case DateGroupType.half:
                        dtDate = new DateTime(this._iniDate.Year, (((this._iniDate.Month - 1) / 6) * 6) + 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddMonths(6);
                        }
                        break;
                    case DateGroupType.year:
                        dtDate = new DateTime(this._iniDate.Year, 1, 1);
                        while (dtDate <= this._endDate)
                        {
                            lstFields.Add(this.DateToField(dtDate));
                            dtDate = dtDate.AddYears(1);
                        }
                        break;
                }
            }

            /*
            foreach (MaintenanceFieldDefinition oField in this._fields)
            {
                lstProps.Add(new DynamicProperty(oField.Mapping, oField.GetFieldType()));
                if (oField.IsPK)
                {
                    lstProps.Add(new DynamicProperty(oField.Mapping + "_PK", typeof(int?)));
                }
                if (oField.FKMaintenance != null)
                {
                    lstProps.Add(new DynamicProperty(oField.Mapping + "_FK", typeof(string)));
                }
                if (!string.IsNullOrWhiteSpace(oField.CurrencyInfoFieldMapping))
                {
                    lstProps.Add(new DynamicProperty(oField.CurrencyInfoFieldMapping + "_CURINFO", typeof(string)));
                }
            }
            lstProps.Add(new DynamicProperty("Access", typeof(int)));
            if (!string.IsNullOrWhiteSpace(this._insFilter))
                lstProps.Add(new DynamicProperty("INS_ID", typeof(int)));
            this._typeModel = System.Linq.Dynamic.DynamicExpression.CreateClass(lstProps.ToArray());
            */
        }

        private string DateToField(DateTime dtDateTime)
        {
            string sField = "";

            switch (this._dateGroup)
            {
                case DateGroupType.hour:
                    {
                        if (!this._dateGroupPattern)
                        {
                            sField = dtDateTime.ToString("HH") + "h " + dtDateTime.ToString("dd-MM");
                        }
                        else
                        {
                            //sField = Convert.ToInt32(sDate).ToString("00") + "h";
                        }
                        break;
                    }
                case DateGroupType.weekday:
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                        if (!this._dateGroupPattern)
                        {
                            sField = dtDateTime.Year.ToString() + "/" + dtDateTime.Month.ToString() + "/" + dtDateTime.Day.ToString() + "-" + names[(int)(dtDateTime.DayOfWeek == DayOfWeek.Sunday ? dtDateTime.DayOfWeek + 1 : dtDateTime.DayOfWeek)];
                        }
                        else
                        {
                            //sField = names[Convert.ToInt32(sDate)];
                        }
                        break;
                    }
                case DateGroupType.day:
                    {
                        if (!this._dateGroupPattern)
                        {
                            sField = dtDateTime.ToString("dd-MM");
                        }
                        break;
                    }
                case DateGroupType.month:
                    {
                        string[] names = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthGenitiveNames;
                        if (!this._dateGroupPattern)
                        {                            
                            sField = dtDateTime.Year.ToString() + "-" + names[dtDateTime.Month];
                        }
                        else
                        {                            
                            //sField = names[Convert.ToInt32(sDate) - 1];
                        }
                        break;
                    }
                case DateGroupType.quarter:
                    {
                        sField = dtDateTime.Year.ToString() + "-" + (((dtDateTime.Month - 1) / 3) + 1).ToString();
                        break;
                    }
                case DateGroupType.half:
                    {
                        sField = dtDateTime.Year.ToString() + "-" + (((dtDateTime.Month - 1) / 6) + 1).ToString();
                        break;
                    }
                case DateGroupType.year:
                    {

                        sField = dtDateTime.Year.ToString();
                        break;
                    }
            }

            return sField;
        }

    }
}