using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Study.Database.Query
{
    /// <summary>
    /// Extension for IDataReader
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// This method will map a query result to an given object.
        /// The object should contain exactly the same property names. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ConvertToObject<T>(this IDataReader reader) where T : new()
        {
            var result = new List<T>();
            if (reader.FieldCount <= 0)
                return result;

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read())
            {
                var newT = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    var prop = props.FirstOrDefault(p => string.Equals(p.Name, fieldName, StringComparison.InvariantCultureIgnoreCase));
                    if (prop != null)
                    {
                        if (reader[i] != DBNull.Value)
                            prop.SetValue(newT, reader[i], null);
                    }
                }

                result.Add(newT);
            }

            return result;
        }

    }
}
