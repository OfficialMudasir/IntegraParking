using CSFacturacionBLL.Infraestructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSFacturacionBLL.Domain.Concrete
{
    public class Base
    {
        public string toJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this,
                            Newtonsoft.Json.Formatting.None, 
                            new JsonSerializerSettings { 
                                NullValueHandling = NullValueHandling.Ignore
                            });
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (PropertyInfo prop in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                FieldNameAttribute customAtrribute = prop.GetCustomAttribute(typeof(FieldNameAttribute)) as FieldNameAttribute;

                if (prop.PropertyType.FullName.StartsWith("System.Collections"))
                {
                    foreach (Object element in (System.Collections.IList)prop.GetValue(this))
                    {
                        stringBuilder.Append(element.ToString());                        
                    }
                }
                else 
                {
                    //If the fieldName is "" then we supose that the object is a complex object and we
                    //deep into his properties to build the final string
                    if (!string.IsNullOrEmpty(customAtrribute.FieldName))
                    {
                        stringBuilder.Append(customAtrribute.FieldName);
                        stringBuilder.Append("|");

                        //Get the property value
                        //If is enum, we try to get the description of the enum
                        //otherwise we get the property value
                        object propertyValue = prop.GetValue(this);
                        if (prop.PropertyType.IsEnum)
                        {
                            string enumDescription = EnumUtils.GetDescription((int)propertyValue, prop.PropertyType);
                            stringBuilder.Append(enumDescription);
                        }
                        else
                        {
                            if (propertyValue != null)
                                stringBuilder.Append(propertyValue.ToString());                         
                        }

                        stringBuilder.Append(Environment.NewLine);
                    }
                    else
                    {
                        object complexObject = prop.GetValue(this);

                        if (complexObject != null)
                        {
                            stringBuilder.Append(complexObject.ToString());
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
