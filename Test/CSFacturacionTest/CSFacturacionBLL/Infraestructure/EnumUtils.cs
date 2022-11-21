using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Infraestructure
{
    public class EnumUtils
    {
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");            
        }

        public static string GetDescription<T>(int m_value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
           
            //Get the (int) values of a enum
           System.Array enumValues = System.Enum.GetValues(type);
           
           for (int i = 0; i < enumValues.Length; i++)
           {
               int intValue = (int)enumValues.GetValue(i);

               if (intValue == m_value)
               {
                   //Get the enum that corresponds to the selected value
                   var field = type.GetFields()[i + 1]; //We add 1 because the first index is not a valid enum field

                   var attribute = Attribute.GetCustomAttribute(field,
                   typeof(DescriptionAttribute)) as DescriptionAttribute;

                   if (attribute != null)
                   {
                       return attribute.Description;
                   }
               }
           }           
           
           throw new ArgumentException("Not found.", "description");
        }

        public static string GetDescription(int m_value, Type type)
        {            
            if (!type.IsEnum) throw new InvalidOperationException();

            //Get the (int) values of a enum
            System.Array enumValues = System.Enum.GetValues(type);

            for (int i = 0; i < enumValues.Length; i++)
            {
                int intValue = (int)enumValues.GetValue(i);

                if (intValue == m_value)
                {
                    //Get the enum that corresponds to the selected value
                    var field = type.GetFields()[i + 1]; //We add 1 because the first index is not a valid enum field

                    var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                }
            }

            throw new ArgumentException("Not found.", "description");
        }

        public static string GetFieldName<T>(string m_fieldName)
        {   
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.Name == m_fieldName)
                {
                    FieldNameAttribute customAtrribute = prop.GetCustomAttribute(typeof(FieldNameAttribute)) as FieldNameAttribute;                    
                    
                    return customAtrribute.FieldName;
                }
            }

            throw new ArgumentException("Not found.", "fieldname");
        }
    }
}
