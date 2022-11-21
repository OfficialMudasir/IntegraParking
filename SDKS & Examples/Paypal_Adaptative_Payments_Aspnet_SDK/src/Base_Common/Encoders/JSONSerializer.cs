using System;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;




namespace PayPal.Platform.SDK
{
    
    public class JSONSerializer
    {

        public static String result;

        public const int TOKEN_NONE = 0;
        public const int TOKEN_CURLY_OPEN = 1;
        public const int TOKEN_CURLY_CLOSE = 2;
        public const int TOKEN_SQUARED_OPEN = 3;
        public const int TOKEN_SQUARED_CLOSE = 4;
        public const int TOKEN_COLON = 5;
        public const int TOKEN_COMMA = 6;
        public const int TOKEN_STRING = 7;
        public const int TOKEN_NUMBER = 8;
        public const int TOKEN_TRUE = 9;
        public const int TOKEN_FALSE = 10;
        public const int TOKEN_NULL = 11;

        private const int BUILDER_CAPACITY = 2000;

        protected static JSONSerializer instance = new JSONSerializer();

        /// <summary>
        /// On decoding, this value holds the position at which the parse failed (-1 = no error).
        /// </summary>
        protected int lastErrorIndex = -1;
        protected string lastDecode = "";
        static Hashtable result1;
        static Hashtable responseHash = new Hashtable();
        static ArrayList errorList = new ArrayList();
        static Type type;
        static Type type3;
        static object obj3;
        static bool isResponse = false;
        static Hashtable hs = new Hashtable();
        static bool isFault = false;

        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
        /// 
        public static object JsonDecode(string json, Type toType)
        {
            if (toType == typeof(PayPal.Platform.SDK.Fault))
            {
                isFault = true;
            }
            else
            {
                isFault = false;
            }
            // save the string for debug information
            JSONSerializer.instance.lastDecode = json;
            type = toType;
            type3 = toType;
            if (json != null)
            {
                char[] charArray = json.ToCharArray();
                int index = 0;
                bool success = true;
                result1 = (Hashtable)JSONSerializer.instance.ParseValue(charArray, ref index, ref success);

                object obj = Activator.CreateInstance(toType);
                obj3 = obj;
                FieldInfo[] finfo = obj.GetType().GetFields((BindingFlags.IgnoreCase
                    | (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)));
                convertJsonToObject(finfo, obj);

                if (success)
                {
                    JSONSerializer.instance.lastErrorIndex = -1;
                }
                else
                {
                    JSONSerializer.instance.lastErrorIndex = index;
                }
                return obj;
            }
            else
            {
                return null;
            }
        }

        private static void convertJsonToObject(FieldInfo[] finfo, object objConvert)
        {
            foreach (FieldInfo fieldInfo in finfo)
            {

                object obj4 = null;
                //bool isArray = false;

                if (fieldInfo.FieldType.IsPublic && fieldInfo.FieldType.IsArray && fieldInfo.FieldType != typeof(XmlElement[]))
                {
                    string arrayErrorKey = fieldInfo.Name.Substring(0, (fieldInfo.Name.Length - "Field".Length));

                    if (result1[arrayErrorKey] != null && result1[arrayErrorKey].GetType() == typeof(ArrayList) ) 
                    {
                        errorList = (ArrayList)result1[arrayErrorKey];
                    }
                    else if (result1[fieldInfo.Name] != null && result1[fieldInfo.Name].GetType() == typeof(Hashtable))
                    {
                        responseHash = (Hashtable)result1[fieldInfo.Name];
                        Hashtable responseNewHash = new Hashtable() ;
                        foreach ( object obj in responseHash) 
                        {    
                            if ( obj != null && obj.GetType() == typeof(DictionaryEntry) ) 
                            {
                                DictionaryEntry respTable = (DictionaryEntry)obj;
                                
                                if (respTable.Value.GetType() == typeof(ArrayList))
                                    {
                                        ArrayList respList = (ArrayList)respTable.Value;
                                        
                                        if (respList != null && respList.Count > 0 )
                                        {
                                            if (respList[0].GetType() == typeof(Hashtable))
                                                 {
                                                         Hashtable itemRespHash = (Hashtable)respList[0];
                                                         foreach (string key in itemRespHash.Keys)
                                                         {
                                                             responseNewHash.Add(key, itemRespHash[key]);
                                                         }
                                                 }
                                        }
                                    }
                              }
                         }

                     if (responseNewHash != null && responseNewHash.Count > 0)
                     {
                         responseHash = responseNewHash ;
                    
                     }
                       

                    }else if( result1[fieldInfo.Name] != null && result1[fieldInfo.Name].GetType() == typeof(ArrayList))
                    {
                        errorList = (ArrayList)result1[fieldInfo.Name];
                    }
                    
                    
                    if ( errorList != null && isFault )
                     {
                            if (responseHash != null )
                            {
                                responseHash.Clear();
                            }
                        foreach (object item in errorList)
                        {
                            if (item != null && item.GetType() == typeof(Hashtable))
                            {
                                Hashtable itemHash = (Hashtable)item;
                                foreach (string key in itemHash.Keys)
                                {
                                    responseHash.Add(key, itemHash[key]);
                                }
                            }
                        }
                    }
                    obj4 = Activator.CreateInstance(fieldInfo.FieldType.GetElementType());
                    Array array = Array.CreateInstance(obj4.GetType(), 1);
                    array.SetValue(obj4, new int[1]);

                    foreach (object arrElement in array)
                    {
                        FieldInfo[] finfo2 = arrElement.GetType().GetFields((BindingFlags.IgnoreCase
                                                           | (BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)));

                        if (!hs.Contains("array"))
                        {
                            hs.Add("array", arrElement);
                        }

                        type3 = arrElement.GetType();
                        obj3 = arrElement;
                        convertJsonToObject(finfo2, arrElement);
                    }

                    isResponse = false;
                    type = objConvert.GetType();
                    if (type.GetField(fieldInfo.Name) != null)
                    {
                        type.GetField(fieldInfo.Name).SetValue(objConvert, array);
                    }
                    else if (isFault)
                    {
                        string errorKey = fieldInfo.Name.Substring(0, (fieldInfo.Name.Length - "Field".Length));
                        if (type.GetProperty(errorKey) != null )
                        {
                            type.GetProperty(errorKey).SetValue(objConvert, array, null);
                        }
                    }
                    if (hs.Count != 0)
                    {
                        type3 = hs["array"].GetType();
                        obj3 = hs["array"];
                        hs.Clear();
                        isResponse = true;
                    }
                }

                else if (fieldInfo.FieldType.IsClass && fieldInfo.FieldType != typeof(string) && fieldInfo.FieldType != typeof(XmlElement) && fieldInfo.FieldType != typeof(Boolean) && fieldInfo.FieldType != typeof(XmlElement[]) && !fieldInfo.FieldType.IsArray)
                {
                    Type type2 = fieldInfo.FieldType;
                    object obj2 = Activator.CreateInstance(type2);
                    type = type2;
                    isResponse = true;
                    
                    if ( isFault ) 
                    {
                        if (responseHash != null)
                        {
                            responseHash.Clear();
                        }
                        foreach (string faultkey in result1.Keys)
                        {

                            if (fieldInfo.Name.Contains(faultkey))
                            {
                                if (result1[faultkey] != null && result1[faultkey].GetType() == typeof(Hashtable))
                                {
                                    responseHash = (Hashtable)result1[faultkey];
                                }
                                else if (result1[faultkey] != null && result1[faultkey].GetType() == typeof(ArrayList))
                                {
                                    errorList = (ArrayList)result1[faultkey];

                                    foreach (object item in errorList)
                                    {
                                        if (item != null && item.GetType() == typeof(Hashtable))
                                        {
                                            Hashtable itemHash = (Hashtable)item;
                                            foreach (string key in itemHash.Keys)
                                            {
                                                responseHash.Add(key, itemHash[key]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (result1[fieldInfo.Name] != null && result1[fieldInfo.Name].GetType() == typeof(Hashtable))
                    {
                        responseHash = (Hashtable)result1[fieldInfo.Name];
                    }
                    else if (result1[fieldInfo.Name] != null && result1[fieldInfo.Name].GetType() == typeof(ArrayList))
                    {
                        errorList = (ArrayList)result1[fieldInfo.Name];

                        foreach (object item in errorList)
                        {
                            if (item != null && item.GetType() == typeof(Hashtable))
                            {
                                Hashtable itemHash = (Hashtable)item;
                                foreach (string key in itemHash.Keys)
                                {
                                    responseHash.Add(key, itemHash[key]);
                                }
                            }
                        }
                    }

                    FieldInfo[] finfo1 = obj2.GetType().GetFields((BindingFlags.IgnoreCase | (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)));
                    convertJsonToObject(finfo1, obj2);
                    isResponse = false;
                    type = objConvert.GetType();
                    if (type.GetField(fieldInfo.Name) != null)
                    {
                        type.GetField(fieldInfo.Name).SetValue(objConvert, obj2);
                    }
                    else if ( isFault )
                    {
                        string restKey = fieldInfo.Name.Substring(0, (fieldInfo.Name.Length - "Field".Length));
                        string newKey = char.ToUpper(restKey[0]) + restKey.Substring(1); //restKey.ToUpperInvariant();                    
                        if (type.GetProperty(restKey) != null )
                        {
                            type.GetProperty(restKey).SetValue(objConvert, obj2, null);
                        }
                        else   if (type.GetProperty(newKey) != null )
                        {
                            type.GetProperty(newKey).SetValue(objConvert, obj2, null);
                        }
                    }
                }
                else
                {
                    if (!isResponse)
                    {
                        type = type3;
                        objConvert = obj3;
                    }

                    if (isFault && fieldInfo.FieldType == typeof(string) && responseHash != null )
                    {
                        foreach (string key in responseHash.Keys)
                        {
                            String value1 = result1[key] != null ? result1[key].ToString() : (responseHash[key] != null ? responseHash[key].ToString() : null); //(errorList[fieldInfo.Name] != null ? errorList[fieldInfo.Name] : null));
                            if (value1 != null && fieldInfo.Name.Contains(key) && type.GetProperty(key) != null)
                            {
                                type.GetProperty(key).SetValue(objConvert, value1, null);
                                break;
                            }
                           
                        }
                    }

                    if (fieldInfo.FieldType == typeof(string) && (result1[fieldInfo.Name] != null || (responseHash != null && responseHash[fieldInfo.Name] != null)))//|| errorList[fieldInfo.Name] != null ))
                    {
                        String value = result1[fieldInfo.Name] != null ? result1[fieldInfo.Name].ToString() : (responseHash[fieldInfo.Name] != null ? responseHash[fieldInfo.Name].ToString() : null); //(errorList[fieldInfo.Name] != null ? errorList[fieldInfo.Name] : null));
                        if (value != null)
                        {
                            type.GetField(fieldInfo.Name).SetValue(objConvert, value);

                        }
                    }
                    else if (fieldInfo.FieldType == typeof(DateTime) && (result1[fieldInfo.Name] != null || (responseHash != null && responseHash[fieldInfo.Name] != null))) // || errorList[fieldInfo.Name] != null ))
                    {
                        String value = result1[fieldInfo.Name] != null ? result1[fieldInfo.Name].ToString() : (responseHash[fieldInfo.Name] != null ? responseHash[fieldInfo.Name].ToString() : null);//(errorList[fieldInfo.Name] != null ? errorList[fieldInfo.Name] : null));
                        if (value != null)
                        {
                            type.GetField(fieldInfo.Name).SetValue(objConvert, Convert.ToDateTime(value));
                        }

                    }
                    else if (fieldInfo.FieldType.IsEnum)
                    {
                        MemberInfo[] memInfoArray = fieldInfo.FieldType.GetMembers();
                        String propName = String.Empty ;
                        String value = result1[fieldInfo.Name] != null ? result1[fieldInfo.Name].ToString() : (responseHash[fieldInfo.Name] != null ? responseHash[fieldInfo.Name].ToString() : null);//(errorList[fieldInfo.Name] != null ? errorList[fieldInfo.Name] : null));
                        if (isFault)
                        {
                            foreach (string key in responseHash.Keys)
                            {
                                if (fieldInfo.Name.Contains(key) && type.GetProperty(key) != null)
                                {
                                    value = result1[key] != null ? result1[key].ToString() : (responseHash[key] != null ? responseHash[key].ToString() : null); //(errorList[fieldInfo.Name] != null ? errorList[fieldInfo.Name] : null));
                                    propName = key;
                                    break;
                                }
                            }
                        }
                        foreach (MemberInfo memInfo in memInfoArray)
                        {
                            if (value != null && memInfo.Name == value)
                            {
                               if (isFault && type.GetProperty(propName) != null )
                               {
                                   type.GetProperty(propName).SetValue(objConvert, Enum.Parse(fieldInfo.FieldType, value, true),null);
                                    break;
                               }
                               else if ( type.GetField(fieldInfo.Name) != null )
                               {
                                    type.GetField(fieldInfo.Name).SetValue(objConvert, Enum.Parse(fieldInfo.FieldType, value, true));
                                    break;
                               }
                            }
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Converts a Hashtable / ArrayList object into a JSON string
        /// </summary>
        /// <param name="json">A Hashtable / ArrayList</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        //public static string JsonEncode(object json)
        //{
        //    StringBuilder builder = new StringBuilder(BUILDER_CAPACITY);
        //    bool success = JSON.instance.SerializeValue(json, builder);
        //    return (success ? builder.ToString() : null);
        //}

        /// <summary>
        /// On decoding, this function returns the position at which the parse failed (-1 = no error).
        /// </summary>
        /// <returns></returns>
        public static bool LastDecodeSuccessful()
        {
            return (JSONSerializer.instance.lastErrorIndex == -1);
        }

        /// <summary>
        /// On decoding, this function returns the position at which the parse failed (-1 = no error).
        /// </summary>
        /// <returns></returns>
        public static int GetLastErrorIndex()
        {
            return JSONSerializer.instance.lastErrorIndex;
        }

        /// <summary>
        /// If a decoding error occurred, this function returns a piece of the JSON string 
        /// at which the error took place. To ease debugging.
        /// </summary>
        /// <returns></returns>
        public static string GetLastErrorSnippet()
        {
            if (JSONSerializer.instance.lastErrorIndex == -1)
            {
                return "";
            }
            else
            {
                int startIndex = JSONSerializer.instance.lastErrorIndex - 5;
                int endIndex = JSONSerializer.instance.lastErrorIndex + 15;
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
                if (endIndex >= JSONSerializer.instance.lastDecode.Length)
                {
                    endIndex = JSONSerializer.instance.lastDecode.Length - 1;
                }

                return JSONSerializer.instance.lastDecode.Substring(startIndex, endIndex - startIndex + 1);
            }
        }

        protected Hashtable ParseObject(char[] json, ref int index)
        {
            Hashtable table = new Hashtable();
            int token;

            // {
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                token = LookAhead(json, index);
                if (token == JSONSerializer.TOKEN_NONE)
                {
                    return null;
                }
                else if (token == JSONSerializer.TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == JSONSerializer.TOKEN_CURLY_CLOSE)
                {
                    NextToken(json, ref index);
                    return table;
                }
                else
                {

                    // name
                    string name = ParseString(json, ref index);
                    if (name == null)
                    {
                        return null;
                    }

                    // :
                    token = NextToken(json, ref index);
                    if (token != JSONSerializer.TOKEN_COLON)
                    {
                        return null;
                    }

                    // value
                    bool success = true;
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }

                    table[name] = value;
                }
            }

            return table;
        }

        protected ArrayList ParseArray(char[] json, ref int index)
        {
            ArrayList array = new ArrayList();

            // [
            NextToken(json, ref index);

            bool done = false;
            while (!done)
            {
                int token = LookAhead(json, index);
                if (token == JSONSerializer.TOKEN_NONE)
                {
                    return null;
                }
                else if (token == JSONSerializer.TOKEN_COMMA)
                {
                    NextToken(json, ref index);
                }
                else if (token == JSONSerializer.TOKEN_SQUARED_CLOSE)
                {
                    NextToken(json, ref index);
                    break;
                }
                else
                {
                    bool success = true;
                    object value = ParseValue(json, ref index, ref success);
                    if (!success)
                    {
                        return null;
                    }

                    array.Add(value);
                }
            }

            return array;
        }

        protected object ParseValue(char[] json, ref int index, ref bool success)
        {
            switch (LookAhead(json, index))
            {
                case JSONSerializer.TOKEN_STRING:
                    return ParseString(json, ref index);
                case JSONSerializer.TOKEN_NUMBER:
                    return ParseNumber(json, ref index);
                case JSONSerializer.TOKEN_CURLY_OPEN:
                    return ParseObject(json, ref index);
                case JSONSerializer.TOKEN_SQUARED_OPEN:
                    return ParseArray(json, ref index);
                case JSONSerializer.TOKEN_TRUE:
                    NextToken(json, ref index);
                    return Boolean.Parse("TRUE");
                case JSONSerializer.TOKEN_FALSE:
                    NextToken(json, ref index);
                    return Boolean.Parse("FALSE");
                case JSONSerializer.TOKEN_NULL:
                    NextToken(json, ref index);
                    return null;
                case JSONSerializer.TOKEN_NONE:
                    break;
            }

            success = false;
            return null;
        }

        protected string ParseString(char[] json, ref int index)
        {
            string s = "";
            char c;

            EatWhitespace(json, ref index);

            // "
            c = json[index++];

            bool complete = false;
            while (!complete)
            {

                if (index == json.Length)
                {
                    break;
                }

                c = json[index++];
                if (c == '"')
                {
                    complete = true;
                    break;
                }
                else if (c == '\\')
                {

                    if (index == json.Length)
                    {
                        break;
                    }
                    c = json[index++];
                    if (c == '"')
                    {
                        s += '"';
                    }
                    else if (c == '\\')
                    {
                        s += '\\';
                    }
                    else if (c == '/')
                    {
                        s += '/';
                    }
                    else if (c == 'b')
                    {
                        s += '\b';
                    }
                    else if (c == 'f')
                    {
                        s += '\f';
                    }
                    else if (c == 'n')
                    {
                        s += '\n';
                    }
                    else if (c == 'r')
                    {
                        s += '\r';
                    }
                    else if (c == 't')
                    {
                        s += '\t';
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            // fetch the next 4 chars
                            char[] unicodeCharArray = new char[4];
                            Array.Copy(json, index, unicodeCharArray, 0, 4);
                            // parse the 32 bit hex into an integer codepoint
                            uint codePoint = UInt32.Parse(new string(unicodeCharArray), NumberStyles.HexNumber);
                            // convert the integer codepoint to a unicode char and add to string
                            s += Char.ConvertFromUtf32((int)codePoint);
                            // skip 4 chars
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                else
                {
                    s += c;
                }

            }

            if (!complete)
            {
                return null;
            }

            return s;
        }

        protected double ParseNumber(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);

            int lastIndex = GetLastIndexOfNumber(json, index);
            int charLength = (lastIndex - index) + 1;
            char[] numberCharArray = new char[charLength];

            Array.Copy(json, index, numberCharArray, 0, charLength);
            index = lastIndex + 1;
            return Double.Parse(new string(numberCharArray), CultureInfo.InvariantCulture);
        }

        protected int GetLastIndexOfNumber(char[] json, int index)
        {
            int lastIndex;
            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            {
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }
            }
            return lastIndex - 1;
        }

        protected void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
            {
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    break;
                }
            }
        }

        protected int LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return NextToken(json, ref saveIndex);
        }

        protected int NextToken(char[] json, ref int index)
        {
            EatWhitespace(json, ref index);

            if (index == json.Length)
            {
                return JSONSerializer.TOKEN_NONE;
            }

            char c = json[index];
            index++;
            switch (c)
            {
                case '{':
                    return JSONSerializer.TOKEN_CURLY_OPEN;
                case '}':
                    return JSONSerializer.TOKEN_CURLY_CLOSE;
                case '[':
                    return JSONSerializer.TOKEN_SQUARED_OPEN;
                case ']':
                    return JSONSerializer.TOKEN_SQUARED_CLOSE;
                case ',':
                    return JSONSerializer.TOKEN_COMMA;
                case '"':
                    return JSONSerializer.TOKEN_STRING;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                    return JSONSerializer.TOKEN_NUMBER;
                case ':':
                    return JSONSerializer.TOKEN_COLON;
            }
            index--;

            int remainingLength = json.Length - index;

            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' &&
                    json[index + 1] == 'a' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 's' &&
                    json[index + 4] == 'e')
                {
                    index += 5;
                    return JSONSerializer.TOKEN_FALSE;
                }
            }

            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' &&
                    json[index + 1] == 'r' &&
                    json[index + 2] == 'u' &&
                    json[index + 3] == 'e')
                {
                    index += 4;
                    return JSONSerializer.TOKEN_TRUE;
                }
            }

            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' &&
                    json[index + 1] == 'u' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 'l')
                {
                    index += 4;
                    return JSONSerializer.TOKEN_NULL;
                }
            }

            return JSONSerializer.TOKEN_NONE;
        }

        protected bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder)
        {
            if (objectOrArray is Hashtable)
            {
                return SerializeObject((Hashtable)objectOrArray, builder);
            }
            else if (objectOrArray is ArrayList)
            {
                return SerializeArray((ArrayList)objectOrArray, builder);
            }
            else
            {
                return false;
            }
        }

        protected bool SerializeObject(Hashtable anObject, StringBuilder builder)
        {
            builder.Append("{");

            IDictionaryEnumerator e = anObject.GetEnumerator();
            bool first = true;
            while (e.MoveNext())
            {
                string key = e.Key.ToString();
                object value = e.Value;

                if (!first)
                {
                    builder.Append(", ");
                }

                SerializeString(key, builder);
                builder.Append(":");
                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("}");
            return true;
        }

        protected bool SerializeArray(ArrayList anArray, StringBuilder builder)
        {
            builder.Append("[");

            bool first = true;
            for (int i = 0; i < anArray.Count; i++)
            {
                object value = anArray[i];

                if (!first)
                {
                    builder.Append(", ");
                }

                if (!SerializeValue(value, builder))
                {
                    return false;
                }

                first = false;
            }

            builder.Append("]");
            return true;
        }

        protected bool SerializeValue(object value, StringBuilder builder)
        {
            if (value is string)
            {
                SerializeString((string)value, builder);
            }
            else if (value is Hashtable)
            {
                SerializeObject((Hashtable)value, builder);
            }
            else if (value is ArrayList)
            {
                SerializeArray((ArrayList)value, builder);
            }
            else if (IsNumeric(value))
            {
                SerializeNumber(Convert.ToDouble(value), builder);
            }
            else if ((value is Boolean) && ((Boolean)value == true))
            {
                builder.Append("true");
            }
            else if ((value is Boolean) && ((Boolean)value == false))
            {
                builder.Append("false");
            }
            else if (value == null)
            {
                builder.Append("null");
            }
            else
            {
                return false;
            }
            return true;
        }

        protected void SerializeString(string aString, StringBuilder builder)
        {
            builder.Append("\"");

            char[] charArray = aString.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                char c = charArray[i];
                if (c == '"')
                {
                    builder.Append("\\\"");
                }
                else if (c == '\\')
                {
                    builder.Append("\\\\");
                }
                else if (c == '\b')
                {
                    builder.Append("\\b");
                }
                else if (c == '\f')
                {
                    builder.Append("\\f");
                }
                else if (c == '\n')
                {
                    builder.Append("\\n");
                }
                else if (c == '\r')
                {
                    builder.Append("\\r");
                }
                else if (c == '\t')
                {
                    builder.Append("\\t");
                }
                else
                {
                    int codepoint = Convert.ToInt32(c);
                    if ((codepoint >= 32) && (codepoint <= 126))
                    {
                        builder.Append(c);
                    }
                    else
                    {
                        builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                    }
                }
            }

            builder.Append("\"");
        }

        protected void SerializeNumber(double number, StringBuilder builder)
        {
            builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Determines if a given object is numeric in any way
        /// (can be integer, double, etc). C# has no pretty way to do this.
        /// </summary>
        protected bool IsNumeric(object o)
        {
            try
            {
                Double.Parse(o.ToString());
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        
        public static string ToJavaScriptObjectNotation(object obj)
        {
            result = null;
            //FieldInfo[] finfo = obj.GetType().GetFields((BindingFlags.IgnoreCase
            //  | (BindingFlags.Public | BindingFlags.Instance)));

            FieldInfo[] finfo = obj.GetType().GetFields((BindingFlags.IgnoreCase | (BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy)));

            result = result + "{\"" + obj.GetType().Name + "\":{";

            writeToBrowser(finfo, obj);
            String delim = ",";
            result = result.Trim(delim.ToCharArray());
            result = result + "}}";
            //Console.WriteLine(result);

            return result;
        }

        private static void writeToBrowser(FieldInfo[] finfo, object obj)
        {


            int countplus = 0;
            foreach (FieldInfo fieldInfo in finfo)
            {
                countplus++;
                object obj2 = null;
                bool isArray = false;
                if (fieldInfo.FieldType.IsPublic && fieldInfo.FieldType.IsArray && fieldInfo.FieldType != typeof(XmlElement[]))
                {
                    isArray = true;
                    obj2 = fieldInfo.GetValue(obj);
                    if (obj2 != null)
                    {
                        result = result + "\"" + fieldInfo.Name + "\":" + "{";
                        Array array = obj2 as Array;
                        String delim = ",";
                        foreach (object arrElement in array)
                        {
                            FieldInfo[] finfo2 = arrElement.GetType().GetFields((BindingFlags.IgnoreCase
                                                              | (BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)));

                            result = result + "\"" + arrElement.GetType().Name.ToLower() + "\":" + "{";
                            writeToBrowser(finfo2, arrElement);
                            result = result.Trim(delim.ToCharArray()) + "},";
                        }
                        result = result.Trim(delim.ToCharArray()) + "},";

                    }
                }

                if (!isArray)
                {
                    obj2 = fieldInfo.GetValue(obj);
                }
                if (!isArray && obj2 != null && obj2.GetType().IsClass && obj2.GetType() != typeof(string) && obj2.GetType() != typeof(XmlElement))
                {
                    if (obj2.GetType() == typeof(Array))
                    {

                    }
                    else
                    {
                        FieldInfo[] finfo1 = obj2.GetType().GetFields((BindingFlags.IgnoreCase
                                                                           | (BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)));
                        result = result + "\"" + fieldInfo.Name + "\":" + "{";
                        writeToBrowser(finfo1, obj2);
                        String delim = ",";
                        result = result.Trim(delim.ToCharArray()) + "},";
                    }

                }
                else if (obj2 != null)
                {
                    if ((fieldInfo.GetValue(obj)) != null && (fieldInfo.GetValue(obj).ToString()) != "0")
                    {
                        //if (!"".Equals(fieldInfo.GetValue(obj)) && obj2.GetType() != typeof(bool) && (obj2.GetType() == typeof(string) || obj2.GetType() == typeof(decimal) || obj2.GetType() == typeof(Int32) || obj2.GetType() == typeof(DateTime) || obj2.GetType() == typeof(Enum)))
                        if (!"".Equals(fieldInfo.GetValue(obj)) && obj2.GetType() != typeof(bool) && (obj2.GetType() == typeof(string) || obj2.GetType() == typeof(decimal) || obj2.GetType() == typeof(Int32) || obj2.GetType() == typeof(DateTime) || obj2.GetType().BaseType == typeof(Enum)))
                        {
                            if (obj2.GetType() == typeof(DateTime))
                            {
                                string strdate = String.Format("{0:s}", fieldInfo.GetValue(obj));
                                string jsondate = strdate.Substring(0, 10);
                                //DateTime dteRegistered = DateTime.ParseExact(strdate, "yyyy-MM-dd", null);
                                result = result + "\"" + fieldInfo.Name + "\":";
                                result = result + "\"" + jsondate + "\",";
                            }
                            else
                            {
                                string temp = fieldInfo.Name.ToUpper() + "SPECIFIED";
                                if (finfo[countplus].Name.ToUpper() == temp)
                                {
                                    if (finfo[countplus].GetValue(obj).ToString() == "True")
                                    {
                                        result = result + "\"" + fieldInfo.Name + "\":";
                                        result = result + "\"" + fieldInfo.GetValue(obj).ToString() + "\",";

                                    }

                                }
                                else
                                {
                                    result = result + "\"" + fieldInfo.Name + "\":";
                                    result = result + "\"" + fieldInfo.GetValue(obj).ToString() + "\",";

                                }


                            }
                        }

                    }
                }

            }
        }
    }
}


