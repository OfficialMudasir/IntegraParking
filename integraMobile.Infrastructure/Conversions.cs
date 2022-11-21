using System;
using System.Collections.Generic;
using System.Linq;
#if !BackOffice
using System.Linq.Dynamic;
#endif
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace integraMobile.Infrastructure
{
    public static class Conversions
    {
        public static int Time2Seconds(DateTime dateTime)
        {
            return dateTime.Second + (dateTime.Minute * 60) + (dateTime.Hour * 3600);
        }
        public static int Time2Seconds(string sTime)
        {
            int iSeconds = 0;
            string[] items = sTime.Split(':');
            if (items.Length >= 3 && items[2].Trim() != "")
                iSeconds = Int32.Parse(items[2]);
            if (items.Length >= 2 && items[1].Trim() != "")
                iSeconds += Int32.Parse(items[1].Trim()) * 60;
            if (items.Length >= 1 && items[0].Trim() != "")
                iSeconds += Int32.Parse(items[0].Trim()) * 3600;
            return iSeconds;
        }
        public static int Date2DayOfWeek(DateTime dtDateTime)
        {
            int iWeekDay = 0;
            switch (dtDateTime.DayOfWeek)
            {
                case DayOfWeek.Monday: iWeekDay = 0; break;
                case DayOfWeek.Tuesday: iWeekDay = 1; break;
                case DayOfWeek.Wednesday: iWeekDay = 2; break;
                case DayOfWeek.Thursday: iWeekDay = 3; break;
                case DayOfWeek.Friday: iWeekDay = 4; break;
                case DayOfWeek.Saturday: iWeekDay = 5; break;
                case DayOfWeek.Sunday: iWeekDay = 6; break;
            }
            return iWeekDay;
        }
        public static DateTime Time2DateTime(DateTime dtDate, string sTime)
        {
            string[] items = sTime.Split(':');
            int iSeconds = 0;// (items.Length >= 3 && items[2].Trim() != "") ? Int32.Parse(items[2].Trim()) : 0;
            int iMinutes = (items.Length >= 2 && items[1].Trim() != "") ? Int32.Parse(items[1].Trim()) : 0;
            int iHours = (items.Length >= 1 && items[0].Trim() != "") ? Int32.Parse(items[0].Trim()) : 0;
            bool bAddDay = false;
            if (iHours >= 24)
            {
                bAddDay = true;
                iHours = 0;
                iMinutes = 0;
                iSeconds = 0;
            }
            DateTime dtDateTime = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, iHours, iMinutes, iSeconds);
            if (bAddDay) dtDateTime = dtDateTime.AddDays(1);
            return dtDateTime;
        }


        public static int MinutesInDay(DateTime dtDate)
        {
            int iNumMinutes = 0;

            iNumMinutes = Convert.ToInt32((dtDate - dtDate.Date).TotalMinutes);

            return iNumMinutes;
        }

        public static DateTime RoundSeconds(DateTime dtDate)
        {
            DateTime dtDateTime = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, dtDate.Hour, dtDate.Minute, 0);

            return dtDateTime;
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            return XmlSerializeToString(objectInstance, null);
        }
        public static string XmlSerializeToString(this object objectInstance, XmlAttributeOverrides attrOverrides)
        {
            var serializer = new XmlSerializer(objectInstance.GetType(), attrOverrides);
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static void XmlSerializeToFile(this object objectInstance, string filename)
        {
            XmlSerializeToFile(objectInstance, filename, null);
        }
        public static void XmlSerializeToFile(this object objectInstance, string filename, XmlAttributeOverrides attrOverrides)
        {
            var serializer = new XmlSerializer(objectInstance.GetType(), attrOverrides);

            using (TextWriter writer = new StreamWriter(filename, false))
            {
                serializer.Serialize(writer, objectInstance);
            }
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            return XmlDeserializeFromString(objectData, type, null);
        }
        public static object XmlDeserializeFromString(string objectData, Type type, XmlAttributeOverrides attrOverrides)
        {
            var serializer = new XmlSerializer(type, attrOverrides);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static object XmlDeserializeFromFile(string filename, Type type)
        {
            return XmlDeserializeFromFile(filename, type, null);
        }
        public static object XmlDeserializeFromFile(string filename, Type type, XmlAttributeOverrides attrOverrides)
        {
            var serializer = new XmlSerializer(type, attrOverrides);
            object result;

            using (TextReader reader = new StreamReader(filename))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static string JsonSerializeToString(this object objectInstance)
        {
            return JsonConvert.SerializeObject(objectInstance);
        }
        public static void JsonSerializeToFile(this object objectInstance, string filename)
        {
            using (TextWriter writer = new StreamWriter(filename, false))
            {
                writer.Write(JsonConvert.SerializeObject(objectInstance));                
            }
        }

        public static object JsonDeserializeFromString(string sJson, Type type)
        {
            return JsonConvert.DeserializeObject(sJson, type);
        }
        public static object JsonDeserializeFromString(Newtonsoft.Json.JsonSerializerSettings oSettings, string sJson, Type type)
        {
            object oRet = null;
            var oScriptSerializer = Newtonsoft.Json.JsonSerializer.Create(oSettings);
            using (var sr = new System.IO.StringReader(sJson))
            {
                oRet = oScriptSerializer.Deserialize(sr, type);
            }
            return oRet;
        }
        public static object JsonDeserializeFromFile(string filename, Type type)
        {
            object oRet = null;
            using (TextReader reader = new StreamReader(filename))
            {
                oRet = JsonConvert.DeserializeObject(reader.ReadToEnd(), type);
            }
            return oRet;
        }

#if !BackOffice
        public static object MergeObjects(object obj1, object obj2)
        {
            object oRet = null;

            List<object> objects = new List<object>();
            List<DynamicProperty> lstProps = new List<DynamicProperty>();

            objects.Add(obj1);
            objects.Add(obj2);

            foreach (object obj in objects)
            {
                if (obj != null)
                {
                    foreach (PropertyInfo oPropInfo in obj.GetType().GetProperties())
                    {
                        lstProps.Add(new DynamicProperty(oPropInfo.Name, oPropInfo.PropertyType));
                    }
                }
            }

            Type type = System.Linq.Dynamic.DynamicExpression.CreateClass(lstProps.ToArray());
            oRet = System.Activator.CreateInstance(type);

            foreach (object obj in objects)
            {
                if (obj != null)
                {
                    foreach (PropertyInfo oPropInfo in obj.GetType().GetProperties())
                    {
                        type.GetProperty(oPropInfo.Name).SetValue(oRet, oPropInfo.GetValue(obj, null), null);
                    }
                }
            }

            return oRet;
        }
#endif        
    }
}
