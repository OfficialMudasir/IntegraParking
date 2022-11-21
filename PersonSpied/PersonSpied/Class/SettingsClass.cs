using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonSpied.Class
{
    public class SettingsClass
    {
        public static String APIUrl
        {

            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["APIUrl"]) ? ConfigurationSettings.AppSettings["APIUrl"] : String.Empty; }
        }

        public static String APIAuthHashKey
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["APIAuthHashKey"]) ? ConfigurationSettings.AppSettings["APIAuthHashKey"] : String.Empty; }
        }

        public static String FileNameExcel
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["FileNameExcel"]) ? ConfigurationSettings.AppSettings["FileNameExcel"] : String.Empty; }
        }

        public static String FileNameTxt
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["FileNameTxt"]) ? ConfigurationSettings.AppSettings["FileNameTxt"] : String.Empty; }
        }

        public static String ConnectionString
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["ConnectionString"]) ? ConfigurationSettings.AppSettings["ConnectionString"] : String.Empty; }
        }

        public static String QueryCount
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["QueryCount"]) ? ConfigurationSettings.AppSettings["QueryCount"] : String.Empty; }
        }

        public static String QuerySelect
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["QuerySelect"]) ? ConfigurationSettings.AppSettings["QuerySelect"] : String.Empty; }
        }

        public static String QueryUpdateFirst
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["QueryUpdateFirst"]) ? ConfigurationSettings.AppSettings["QueryUpdateFirst"] : String.Empty; }
        }

        public static String QueryUpdateLast
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["QueryUpdateLast"]) ? ConfigurationSettings.AppSettings["QueryUpdateLast"] : String.Empty; }
        }

        public static String QueryInsert
        {
            get { return !String.IsNullOrEmpty(ConfigurationSettings.AppSettings["QueryInsert"]) ? ConfigurationSettings.AppSettings["QueryInsert"] : String.Empty; }
        }
    }
}
